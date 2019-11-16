using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

using SA.Android.GMS.Games;
using SA.Android.GMS.Auth;
using SA.Android.GMS.Common.Images;
using SA.Android.Utilities;
using SA.Android.App;
using SA.Android.GMS.Drive;

using SA.Foundation.Network.Web;

public class SX_GameCenter_Android : MonoBehaviour {
    
#pragma warning restore 649

    public string iLeaderboardId = "CgkIw7PKmfEVEAIQAA";

    //Avoid using API with Awake. The main Android activity may not be ready yet.
    private void Start() {

        AN_Logger.Log("AN_GMS_Auth_Example Start");
        //Let's see if user has already signed in
        var signedInAccount = AN_GoogleSignIn.GetLastSignedInAccount();
        if (signedInAccount != null)
            SignInNoSilent();



     
            var account = AN_GoogleSignIn.GetLastSignedInAccount();
            if (account == null) {
                SignInFlow();
            } else {
                SignOutFlow();
            }

    }

    private void SignOutFlow() {

      

        AN_GoogleSignInOptions gso = new AN_GoogleSignInOptions.Builder(AN_GoogleSignInOptions.DEFAULT_GAMES_SIGN_IN).Build(); 
        AN_GoogleSignInClient client = AN_GoogleSignIn.GetClient(gso);

        client.SignOut(() => {});
    }



    private void SignInNoSilent() {


        AN_GoogleSignInOptions.Builder builder = new AN_GoogleSignInOptions.Builder(AN_GoogleSignInOptions.DEFAULT_GAMES_SIGN_IN);
        builder.RequestId();
        builder.RequestEmail();
        builder.RequestProfile();
       
        AN_GoogleSignInOptions gso = builder.Build();
        AN_GoogleSignInClient client = AN_GoogleSignIn.GetClient(gso);
        AN_Logger.Log("SignInNoSilent Start ");

        client.SignIn((signInResult) => {
            AN_Logger.Log("Sign In StatusCode: " + signInResult.StatusCode);
            if (signInResult.IsSucceeded) {
                AN_Logger.Log("SignIn Succeeded");
                UpdateUIWithAccount(signInResult.Account);
            } else {
                AN_Logger.Log("SignIn filed: " + signInResult.Error.FullMessage);
            }
        });

      
    }

    private void SignInFlow() {
        AN_Logger.Log("Play Services Sign In started....");
        AN_GoogleSignInOptions.Builder builder = new AN_GoogleSignInOptions.Builder(AN_GoogleSignInOptions.DEFAULT_GAMES_SIGN_IN);

        //Google play documentation syas that
        // you don't need to use this, however, we recommend you still
        // add those option to your Sing In builder. Some version of play service lib
        // may retirn a signed account with all fileds empty if you will not add this.
        // However according to the google documentation this step isn't required
        // So the decision is up to you.
        builder.RequestId();
        builder.RequestEmail();
        builder.RequestProfile();

        // Add the APPFOLDER scope for Snapshot support.
        builder.RequestScope(AN_Drive.SCOPE_APPFOLDER);

        AN_GoogleSignInOptions gso = builder.Build();
        AN_GoogleSignInClient client = AN_GoogleSignIn.GetClient(gso);

        AN_Logger.Log("Let's try Silent SignIn first");
        client.SilentSignIn((result) => {
            if (result.IsSucceeded) {
                AN_Logger.Log("SilentSignIn Succeeded");
                UpdateUIWithAccount(result.Account);
            } else {

                // Player will need to sign-in explicitly using via UI

                AN_Logger.Log("SilentSignIn Failed with code: " + result.Error.Code);
                AN_Logger.Log("Starting the default Sign in flow");

                //Starting the interactive sign-in
                client.SignIn((signInResult) => {
                    AN_Logger.Log("Sign In StatusCode: " + signInResult.StatusCode);
                    if (signInResult.IsSucceeded) {
                        AN_Logger.Log("SignIn Succeeded");
                        UpdateUIWithAccount(signInResult.Account);
                    } else {
                        AN_Logger.Log("SignIn filed: " + signInResult.Error.FullMessage);
                    }
                });


            }
        });
    }

    public void showArchievements(){
        var client = AN_Games.GetAchievementsClient();
        client.GetAchievementsIntent((result) => {
            if(result.IsSucceeded) {
                var intent = result.Intent;
                AN_ProxyActivity proxy = new AN_ProxyActivity();
                proxy.StartActivityForResult(intent, (intentResult) => {
                    proxy.Finish();
                    //TODO you might want to check is user had sigend out with that UI
                });

            } else {
                Debug.Log("Failed to Get Achievements Intent " + result.Error.FullMessage);
            }
        });

    }
    
    public void unlockAchievement (string achievementId){
        var AN_client = AN_Games.GetAchievementsClient();
        AN_client.Unlock(achievementId);
    }

     public void incrementAchievement (string achievementId, int numSteps){
        var client = AN_Games.GetAchievementsClient();
        client.Increment(achievementId, numSteps);
    }

    
    public void showLeaderBoards (){

        var leaderboards = AN_Games.GetLeaderboardsClient();
        leaderboards.GetAllLeaderboardsIntent((result) => {
            if (result.IsSucceeded) {
                var intent = result.Intent;
                AN_ProxyActivity proxy = new AN_ProxyActivity();
                proxy.StartActivityForResult(intent, (intentResult) => {
                    proxy.Finish();
                    //Note: you might want to check is user had sigend out with that UI
                });

            } else {
                Debug.Log("Failed to Get leaderboards Intent " + result.Error.FullMessage);
            }
        });
             
    }

    public void submitPlayerScore (string leaderboardId, int score){

        if(leaderboardId==null)
            leaderboardId = iLeaderboardId;

        var leaderboards = AN_Games.GetLeaderboardsClient();
        leaderboards.SubmitScore(leaderboardId, score);

    }

    public void loadPlayerScore(string leaderboardId) {

        if(leaderboardId==null)
            leaderboardId = iLeaderboardId;

        var leaderboards = AN_Games.GetLeaderboardsClient();
        leaderboards.LoadCurrentPlayerLeaderboardScore(leaderboardId, AN_Leaderboard.TimeSpan.AllTime, AN_Leaderboard.Collection.Public, (result) => {
            if (result.IsSucceeded)
            {
                //Main.UpdateLevel(Convert.ToInt32(result.Data.RawScore));
				//Main.LevelUp ();
            }
            else
            {
                Debug.Log("Failed to load player score.");
            }
        });
    }

    private void PrintSignedPlayerInfo() {
        AN_PlayersClient client = AN_Games.GetPlayersClient();
        client.GetCurrentPlayer((result) => {
            if(result.IsSucceeded) {
                AN_Player player = result.Data;
                //Printing player info:
                AN_Logger.Log("player.Id: " + player.PlayerId);
                AN_Logger.Log("player.DisplayName: " + player.DisplayName);
                AN_Logger.Log("player.HiResImageUri: " + player.HiResImageUri);
                AN_Logger.Log("player.IconImageUri: " + player.IconImageUri);
                AN_Logger.Log("player.HasIconImage: " + player.HasIconImage);
                AN_Logger.Log("player.HasHiResImage: " + player.HasHiResImage);



                if (!player.HasHiResImage) {
                    var url = player.HiResImageUri;
                    AN_ImageManager manager = new AN_ImageManager();
                    manager.LoadImage(url, (imaheLoadResult) => {

                    });
                }

            } else {
                AN_Logger.Log("Failed to load Current Player " + result.Error.FullMessage);
            }


        });
    }

    private void UpdateUIWithAccount(AN_GoogleSignInAccount account) {
        AN_Logger.Log("account.HashCode:" + account.HashCode);
        AN_Logger.Log(account);
        AN_Logger.Log("SignIn IsSucceeded. user: " + account.GetDisplayName());

        //Display User info


        PrintSignedPlayerInfo();
    }
}
