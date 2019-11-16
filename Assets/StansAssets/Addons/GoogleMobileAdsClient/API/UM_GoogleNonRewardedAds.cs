using System;
using SA.Foundation.Templates;

#if SA_ADMOB_INSTALLED
using GoogleMobileAds.Api;
#endif


namespace SA.CrossPlatform.Advertisement
{
    public class UM_GoogleNonRewardedAds : UM_GoogleBaseAds, UM_iNonRewardedAds
    {
#if SA_ADMOB_INSTALLED
        private InterstitialAd m_interstitial;
        private Action m_showCallback = null;
#endif

        public void Load(Action<SA_Result> callback) {
            Load(UM_GoogleAdsSettings.Instance.Platform.NonRewardedId, callback);
        }

        public void Load(string id, Action<SA_Result> callback) {
#if SA_ADMOB_INSTALLED
            m_loadCallback = callback;
            m_interstitial = new InterstitialAd(id);

            m_interstitial.OnAdLoaded += HandleAdLoaded;
            m_interstitial.OnAdFailedToLoad += HandleAdFailedToLoad;
            m_interstitial.OnAdClosed += HandleAdClosed;

            m_interstitial.LoadAd(UM_GoogleAdsClient.BuildAdRequest());
#endif
        }


        public void Show(Action callabck) {
#if SA_ADMOB_INSTALLED
            m_showCallback = callabck;
            m_interstitial.Show();
            m_isReady = false;
#endif
        }


#if SA_ADMOB_INSTALLED

        private void HandleAdClosed(object sender, EventArgs e) {
            if(m_showCallback != null) {
                m_showCallback.Invoke();
            }
        }

#endif
    }
}