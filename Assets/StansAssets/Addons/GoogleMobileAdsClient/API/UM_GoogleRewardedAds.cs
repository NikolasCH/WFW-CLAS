using System;
using SA.Foundation.Templates;

#if SA_ADMOB_INSTALLED
using GoogleMobileAds.Api;
#endif

using SA.Foundation.Threading;

namespace SA.CrossPlatform.Advertisement
{
    public class UM_GoogleRewardedAds : UM_GoogleBaseAds, UM_iRewardedAds
    {

#if SA_ADMOB_INSTALLED
        private RewardBasedVideoAd m_rewardedVideo;
        private Action<UM_RewardedAdsResult> m_showCallback;

        private bool m_isInited = false;
        private bool m_isRewarded = false;
#endif

        public void Load(Action<SA_Result> callback) {
            Load(UM_GoogleAdsSettings.Instance.Platform.RewardedId, callback);
        }

        public void Load(string id, Action<SA_Result> callback) {
#if SA_ADMOB_INSTALLED
            m_loadCallback = callback;
          
            if(!m_isInited) {
                m_isInited = true;
                m_rewardedVideo = RewardBasedVideoAd.Instance;
                m_rewardedVideo.OnAdLoaded += HandleAdLoaded;
                m_rewardedVideo.OnAdFailedToLoad += HandleAdFailedToLoad;

                m_rewardedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
                m_rewardedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
            }

            m_rewardedVideo.LoadAd(UM_GoogleAdsClient.BuildAdRequest(), id);
#endif
        }

        public void Show(Action<UM_RewardedAdsResult> callabck) {
#if SA_ADMOB_INSTALLED
            m_isReady = false;
            m_isRewarded = false;
            m_showCallback = callabck;
            m_rewardedVideo.Show();
#endif
        }


#if SA_ADMOB_INSTALLED
        private void HandleRewardBasedVideoRewarded(object sender, Reward e) {
            m_isRewarded = true;
        }

        private void HandleRewardBasedVideoClosed(object sender, EventArgs e) {

            SA_MainThreadDispatcher.Enqueue(() => {
                if (m_isRewarded) {
                    m_showCallback.Invoke(UM_RewardedAdsResult.Finished);
                } else {
                    m_showCallback.Invoke(UM_RewardedAdsResult.Skipped);
                }

                m_isRewarded = false;
                m_showCallback = null;
            });
        }
#endif


    }
}