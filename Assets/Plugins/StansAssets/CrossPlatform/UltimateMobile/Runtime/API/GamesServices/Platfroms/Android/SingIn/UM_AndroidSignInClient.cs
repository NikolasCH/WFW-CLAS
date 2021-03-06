using System;
using System.Collections.Generic;
using SA.Foundation.Events;
using SA.Foundation.Templates;
using SA.Android.App;
using SA.Android.App.View;
using SA.Android.GMS.Auth;
using SA.Android.GMS.Games;
using SA.Android.GMS.Drive;
using SA.Android.GMS.Common;
using SA.Android.Utilities;

namespace SA.CrossPlatform.GameServices
{
    internal class UM_AndroidSignInClient : UM_AbstractSignInClient, UM_iSignInClient
    {
        private readonly List<int> m_ResolvedErrors = new List<int>();

        public UM_AndroidSignInClient() 
        {
            SA_MonoEvents.OnApplicationPause.AddSafeListener(this, paused => 
            {
                if(!paused) 
                {
                    //We do not want to do Silent SignIn on resume in case player not yet signed.
                    if (PlayerInfo.State == UM_PlayerState.SignedOut) 
                    {
                        // In case it's not null, this means we are missing something, so we will do  Silent SignIn
                        // The case may happen because we sending fail event on proxy Activity Destroy event.
                        // But proxy Activity Destroy not always means that player is failed to log in.
                        // We have to send fail event on proxy Activity Destroy, since if we not, in cases where google and our proxy
                        // activity both are destroyed, we will not get any event.
                        if (AN_GoogleSignIn.GetLastSignedInAccount() == null) 
                            return;
                    }

                    //We need to perform Silent SignIn every time we back from pause
                    SignInClient.SilentSignIn(silentSignInResult => 
                    {
                        if (silentSignInResult.IsSucceeded) 
                            RetrievePlayer(result => { });
                        else 
                            //looks Like player singed out
                            UpdatePlayerInfo(null);
                    });
                }
            });
        }

        protected override void StartSingInFlow(Action<SA_Result> callback) 
        {
            m_ResolvedErrors.Clear();
            StartSingInFlowInternal(callback);
        }

        private void StartSingInFlowInternal(Action<SA_Result> callback) 
        {
            AN_Logger.Log("UM_AndroidSignInClient, starting silent sing-in");
            SignInClient.SilentSignIn(silentSignInResult => 
            {
                if(silentSignInResult.IsSucceeded) 
                {
                    AN_Logger.Log("UM_AndroidSignInClient, silent sing-in Succeeded");
                    RetrievePlayer(callback);
                } 
                else 
                {
                    AN_Logger.Log("UM_AndroidSignInClient, silent sing-in Failed");
                    AN_Logger.Log("UM_AndroidSignInClient, starting interactive sing-in");
                    SignInClient.SignIn(interactiveSignInResult => 
                    {
                        AN_Logger.Log("UM_AndroidSignInClient, interactive sing-in completed");
                        if (interactiveSignInResult.IsSucceeded) 
                        {
                            AN_Logger.Log("UM_AndroidSignInClient, interactive sing-in succeeded");
                            RetrievePlayer(callback);
                        } 
                        else 
                        {
                            AN_Logger.Log("UM_AndroidSignInClient, interactive sing-in failed");
                            var errorCode = interactiveSignInResult.Error.Code;
                            switch (errorCode) 
                            {
                                //Retry may solve the issue
                                case (int)AN_CommonStatusCodes.NETWORK_ERROR:
                                case (int)AN_GoogleSignInStatusCodes.SIGN_IN_CURRENTLY_IN_PROGRESS:
                                    m_ResolvedErrors.Add(errorCode);
                                    //Let's see if we tried to do it before
                                    if(m_ResolvedErrors.Contains(errorCode)) {
                                        AN_Logger.Log("UM_AndroidSignInClient, sending fail result");
                                        callback.Invoke(new SA_Result(interactiveSignInResult.Error));
                                    } else {
                                        //Nope, this is new one, let's try to resolve it
                                        AN_Logger.Log("Trying to resolved failed sigin-in result with code: " + errorCode);
                                        StartSingInFlowInternal(callback);
                                    } 
                                    break;
                                default:
                                    AN_Logger.Log("UM_AndroidSignInClient, sending fail result");
                                    callback.Invoke(new SA_Result(interactiveSignInResult.Error));
                                    break;
                            }
                        }
                    });
                }
            });
        }

        public void SingOut(Action<SA_Result> callback) 
        {
            SignInClient.RevokeAccess(() => 
            {
                UpdatePlayerInfo(null);
                callback.Invoke(new SA_Result());
            });
        }
        
        //--------------------------------------
        //  Private Methods
        //--------------------------------------

        private void UpdatePlayerInfo(AN_Player player) 
        {
            UM_PlayerInfo playerInfo;
            if(player != null) 
                playerInfo = new UM_PlayerInfo(UM_PlayerState.SignedIn, new UM_AndroidPlayer(player));
            else 
                playerInfo = new UM_PlayerInfo(UM_PlayerState.SignedOut, null);

            UpdateSignedPlater(playerInfo);
        }
        
        private void RetrievePlayer(Action<SA_Result> callback) 
        {
            AN_Logger.Log("UM_AndroidSignInClient, client signed-in, getting the player info");

            //When Sign in is finished with successes
            var gamesClient = AN_Games.GetGamesClient();
            gamesClient.SetViewForPopups(AN_MainActivity.Instance);

            //optionally
            gamesClient.SetGravityForPopups(AN_Gravity.TOP | AN_Gravity.CENTER_HORIZONTAL);

            var client = AN_Games.GetPlayersClient();
            SA_Result apiResult;
            client.GetCurrentPlayer(result => 
            {
                if (result.IsSucceeded) 
                {
                    apiResult = new SA_Result();
                    AN_Logger.Log("UM_AndroidSignInClient, player info retrieved, OnPlayerChanged event will be sent");
                    UpdatePlayerInfo(result.Data);
                } 
                else 
                {
                    apiResult = new SA_Result(result.Error);
                }
                
                AN_Logger.Log("UM_AndroidSignInClient, sending sing in result");
                callback.Invoke(apiResult);
            });
        }


        private AN_GoogleSignInClient SignInClient 
        {
            get 
            {
                var builder = new AN_GoogleSignInOptions.Builder(AN_GoogleSignInOptions.DEFAULT_SIGN_IN);
                builder.RequestId();
                builder.RequestScope(new AN_Scope(AN_Scopes.GAMES_LITE));

                if (UM_Settings.Instance.AndroidRequestEmail) 
                    builder.RequestEmail();

                if (UM_Settings.Instance.AndroidRequestProfile) 
                    builder.RequestProfile();

                if(UM_Settings.Instance.AndroidSavedGamesEnabled) 
                    builder.RequestScope(AN_Drive.SCOPE_APPFOLDER);

                if(UM_Settings.Instance.AndroidRequestServerAuthCode) 
                    builder.RequestServerAuthCode(UM_Settings.Instance.AndroidGMSServerId, false);

                var gso = builder.Build();
                return AN_GoogleSignIn.GetClient(gso);
            }
        }
    }
}