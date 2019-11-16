using System;

using UnityEngine;

#if SA_ADMOB_INSTALLED
using GoogleMobileAds.Api;
#endif
using SA.Foundation.Templates;
using SA.Foundation.Threading;

namespace SA.CrossPlatform.Advertisement
{
    public class UM_GoogleBannerAds : UM_GoogleBaseAds, UM_iBannerAds
    {

#if SA_ADMOB_INSTALLED
        private BannerView m_banner;
        private Action m_showCallback;
#endif


        public void Load(Action<SA_Result> callback) {
            Load(UM_GoogleAdsSettings.Instance.Platform.BannerId, callback);
        }

        public void Load(string id, Action<SA_Result> callback) {

#if SA_ADMOB_INSTALLED
            AdSize size = null;
            switch (UM_GoogleAdsSettings.Instance.BannerSize) {
                case UM_GoogleBannerSize.Banner:
                    size = AdSize.Banner;
                    break;

                case UM_GoogleBannerSize.IABBanner:
                    size = AdSize.IABBanner;
                    break;

                case UM_GoogleBannerSize.Leaderboard:
                    size = AdSize.Leaderboard;
                    break;

                case UM_GoogleBannerSize.MediumRectangle:
                    size = AdSize.MediumRectangle;
                    break;

                case UM_GoogleBannerSize.SmartBanner:
                    size = AdSize.SmartBanner;
                    break;
            }
             
            AdPosition position = UM_GoogleAdsSettings.Instance.BannerPosition;

            m_isReady = false;
            m_loadCallback = callback;

            m_banner = new BannerView(id, size, position);

            m_banner.OnAdLoaded += HandleBannerLoaded;
            m_banner.OnAdFailedToLoad += HandleAdFailedToLoad;
            m_banner.OnAdOpening += HandleBannerAdOpened;
            
            // Load a banner ad.
            m_banner.LoadAd(UM_GoogleAdsClient.BuildAdRequest());
#endif
        }

      


        public void Show(Action callback) {
#if SA_ADMOB_INSTALLED
            m_showCallback = callback;
            m_banner.Show();
#endif
        }

#if SA_ADMOB_INSTALLED

        private void HandleBannerAdOpened(object sender, EventArgs e) {
            SA_MainThreadDispatcher.Enqueue(() => {
                m_showCallback.Invoke();
            });
        }

        protected void HandleBannerLoaded(object sender, EventArgs e) {

            //the OnAdLoaded callback will also be trigerred on banner reload
            //so we need to make sure that we are waiting for a load callback, and ignore reload

            if(m_loadCallback == null) {
                return;
            }

            //We are hiding because it will be showed automatically when loaded
            //but we need to prevent this.
            m_banner.Hide();
            HandleAdLoaded(sender, e);
        }
#endif

        public void Hide() {
#if SA_ADMOB_INSTALLED
            m_banner.Hide();
#endif
        }


        public void Destroy() {
#if SA_ADMOB_INSTALLED
            m_isReady = false; 
            m_banner.Destroy();
            m_banner = null;
#endif
        }

    }
}