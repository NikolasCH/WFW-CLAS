using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using SA.CrossPlatform.UI;
using SA.CrossPlatform.Advertisement;

using SA.Foundation.Threading;


using SA.Foundation.Utility;

public class SX_Ads : MonoBehaviour
{
    float time =0;
    public UM_iAdsClient m_adsClient;
    public GameObject RevardsButton;
    
    void Start () {

        m_adsClient = UM_AdvertisementService.GetClient(UM_AdPlatform.Google);

        if (m_adsClient == null) {
            Debug.Log("Cleint not configured");
        } else {
            if (m_adsClient.IsInitialized) {
                Debug.Log("Advertesment platfrom is ready to be used");
            } else {
                m_adsClient.Initialize((result) => {
                    if(result.IsSucceeded) {
                        Debug.Log("Advertesment platfrom is ready to be used");
                    } else {
                        Debug.Log("Advertesment platfrom is failed to init");
                    }
                });
            }     
        }                   

    }

    private void FixedUpdate() {
        if (m_adsClient == null)
        {
            return;
        }

        if(RevardsButton)
            RevardsButton.SetActive(m_adsClient.RewardedAds.IsReady);
    }

//SmartBanner
    public void smartBanneLoad(){
        m_adsClient.Banner.Load((result) => {
            if(result.IsSucceeded) {
                Debug.Log("Banner ad loaded");
            } else {
                Debug.Log("Failed to load banner ads: " + result.Error.Message);
            }
        });
    }

    public void smartBanneShow(){
        Debug.Log("SmartBannerShow");
        if(m_adsClient.Banner.IsReady)
            m_adsClient.Banner.Show(() => {Debug.Log("Banner Appeared");});
        else
            smartBanneLoad();

    }

    public void showWhenReadyBannerLoad(){

        Debug.Log("SmartBanner Load & Show");
        m_adsClient.Banner.Load((result) => {
            if (result.IsSucceeded) {
                m_adsClient.Banner.Show(() => { });
            }
        });
    }

    public void smartBanneHide(){
        if(m_adsClient.Banner.IsReady)
            m_adsClient.Banner.Hide();
    }

    public void smartBanneDestroy(){

        m_adsClient.Banner.Destroy();
    }

//Revards
    public void rewardedAdsLoad(){

        Debug.Log("Rewarded Ads load request sent");
        m_adsClient.RewardedAds.Load((result) => {
            if (result.IsSucceeded) {
                Debug.Log("RewardedAds loaded");
            } else {
                Debug.Log("Failed to load RewardedAds: " + result.Error.Message);
            }
        });
    }

    public void rewardedAdsShow()
    {
        if(m_adsClient.RewardedAds.IsReady)
            m_adsClient.RewardedAds.Show((adsResult) => {
                if(adsResult == UM_RewardedAdsResult.Finished) {
                   Main.AddAdsRevard();
                }
                Debug.Log("RewardedAds Finished");
            });
        else
            showWhenReadyRewarded();
    }

    public void showWhenReadyRewarded(){
        m_adsClient.RewardedAds.Load((result) => {
            if (result.IsSucceeded) {
                m_adsClient.RewardedAds.Show((adsResult) => {
                    if(adsResult == UM_RewardedAdsResult.Finished) {
                        // Main.game.UpRevards(); <===========================================
                    }
                    Debug.Log("RewardedAds Finished");
                });
            }
        });
    }

//FullBanner
    public void nonRewardedAdsLoad(){
            m_adsClient.NonRewardedAds.Load((result) => {
                if (result.IsSucceeded) {
                    Debug.Log("NonRewardedAds loaded");
                } else {
                    Debug.Log("Failed to load NonRewardedAds: " + result.Error.Message);
                }
            });
    }

    
    public void nonRewardedAdsShow(){
           m_adsClient.NonRewardedAds.Show(() => {
                Debug.Log("Non Rewarded Ads closed");
            });
    }

    
    public void showWhenReadyNonRewarded(){
        m_adsClient.NonRewardedAds.Load((result) => {
            if (result.IsSucceeded) {
                m_adsClient.NonRewardedAds.Show(() => {
                    Debug.Log("Non Rewarded Ads closed");
                });
            }
        });
    }

    private void ShowMessage(string title, string message) {

     
        var builder = new UM_NativeDialogBuilder(title, message);
        builder.SetPositiveButton("Okay", () => {
           
        });

        var dialog = builder.Build();
        dialog.Show();

    }
}
