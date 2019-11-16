using System;
using System.Collections.Generic;
using UnityEngine;

#if SA_ADMOB_INSTALLED
using GoogleMobileAds.Api;
#endif


using SA.Foundation.Templates;
using SA.Foundation.Threading;


namespace SA.CrossPlatform.Advertisement
{
    internal class UM_GoogleAdsClient : UM_AbstractAdsClient, UM_iAdsClient
    {

        public void Initialize(Action<SA_Result> callback) {
            Initialize(UM_GoogleAdsSettings.Instance.Platform.AppId, callback);
        }

        protected override void ConnectToService(string appId, Action<SA_Result> callback) {
#if SA_ADMOB_INSTALLED
            SA_MainThreadDispatcher.Init();

            MobileAds.Initialize(appId);
            callback.Invoke(new SA_Result());
#endif
        }

#if SA_ADMOB_INSTALLED
        public static AdRequest BuildAdRequest() {

            var builder = new AdRequest.Builder();

            foreach(var deviceId in  UM_GoogleAdsSettings.Instance.TestDevices) {
                builder.AddTestDevice(deviceId);
            }

            foreach (var keyword in UM_GoogleAdsSettings.Instance.Keywords) {
                builder.AddKeyword(keyword);
            }

            builder.TagForChildDirectedTreatment(UM_GoogleAdsSettings.Instance.TagForChildDirectedTreatment);


            if(UM_GoogleAdsSettings.Instance.NPA) {
                builder.AddExtra("npa", "1");
            }

            if (UM_GoogleAdsSettings.Instance.Gender != Gender.Unknown) {
                builder.SetGender(UM_GoogleAdsSettings.Instance.Gender);
            }

            if(UM_GoogleAdsSettings.Instance.Birthday != DateTime.MinValue) {
                builder.SetBirthday(UM_GoogleAdsSettings.Instance.Birthday);
            }

            return builder.Build();
        }

#endif
        public UM_iBannerAds Banner {
            get {
                if (m_banner == null) {
                    m_banner = new UM_GoogleBannerAds();
                }
                return m_banner;
            }
        }

        public UM_iRewardedAds RewardedAds {
            get {
                if (m_rewardedAds == null) {
                    m_rewardedAds = new UM_GoogleRewardedAds();
                }

                return m_rewardedAds;
            }
        }

        public UM_iNonRewardedAds NonRewardedAds {
            get {
                if (m_nonRewardedAds == null) {
                    m_nonRewardedAds = new UM_GoogleNonRewardedAds();
                }
                return m_nonRewardedAds;
            }
        }


    }
}