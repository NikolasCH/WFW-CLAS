using UnityEngine;
using System;
using System.Collections.Generic;

#if SA_ADMOB_INSTALLED
using GoogleMobileAds.Api;
#endif

using SA.Foundation.Patterns;
using SA.Foundation.Config;

namespace SA.CrossPlatform.Advertisement
{
    public class UM_GoogleAdsSettings : SA_ScriptableSingleton<UM_GoogleAdsSettings>
    {
        public UM_PlatfromAdIds AndroidIds;
        public UM_PlatfromAdIds IOSIds;

        public List<string> TestDevices = new List<string>();
        public List<string> Keywords = new List<string>();
        public bool TagForChildDirectedTreatment = false;
        public bool NPA = true;

#if SA_ADMOB_INSTALLED

        public AdPosition BannerPosition = AdPosition.Bottom;
        public UM_GoogleBannerSize BannerSize = UM_GoogleBannerSize.SmartBanner;
        


        public DateTime Birthday;
        public Gender Gender = GoogleMobileAds.Api.Gender.Unknown;
#endif
      

        public UM_PlatfromAdIds Platform {
            get {
                switch (Application.platform) {
                    case RuntimePlatform.Android:
                        return AndroidIds;
                    case RuntimePlatform.IPhonePlayer:
                        return IOSIds;
                    default:
                        return new UM_PlatfromAdIds();
                }
            }
        }


        //--------------------------------------
        // SA_ScriptableSettings
        //--------------------------------------

        public override string PluginName {
            get {
                return "UM Google AdMob";
            }
        }

        public override string DocumentationURL {
            get {
                return "https://unionassets.com/ultimate-mobile-pro/google-admob-784";
            }
        }

        public override string SettingsUIMenuItem {
            get {
                return SA_Config.EDITOR_MENU_ROOT + "Cross-Platform/3rd-Party";
            }
        }

        protected override string BasePath {
            get {
                return string.Empty;
            }
        }
    }
}