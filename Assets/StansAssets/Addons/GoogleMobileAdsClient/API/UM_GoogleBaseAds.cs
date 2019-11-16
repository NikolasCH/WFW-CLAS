using UnityEngine;
using System;
#if SA_ADMOB_INSTALLED
using GoogleMobileAds.Api;
#endif
using SA.Foundation.Templates;
using SA.Foundation.Threading;

namespace SA.CrossPlatform.Advertisement
{
    public abstract class UM_GoogleBaseAds 
    {
        protected Action<SA_Result> m_loadCallback;
        protected bool m_isReady = false;


#if SA_ADMOB_INSTALLED
        protected void HandleAdLoaded(object sender, EventArgs e) {

            Debug.Log("Ad loaded here");

            SA_MainThreadDispatcher.Enqueue(() => {
                m_isReady = true;
                m_loadCallback.Invoke(new SA_Result());
                m_loadCallback = null;
            });
          
        }


        protected void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs e) {
            SA_MainThreadDispatcher.Enqueue(() => {
                var error = new SA_Error(1, e.Message);
                m_loadCallback.Invoke(new SA_Result(error));
                m_loadCallback = null;
            });
        }

#endif

        public bool IsReady {
            get {
                return m_isReady;
            }
        }
    }
}