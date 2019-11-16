using UnityEngine;
using System.Collections;
using SA.Android.Firebase.Analytics;

public class SX_Buttons : MonoBehaviour
{

    GameObject SX;
	void Start ()
	{		
        SX = GameObject.Find ("SX");
	}

	void OnPress (bool isDown)
	{
        if (isDown == false)
        {

        AN_FirebaseAnalytics.LogEvent(gameObject.name);

            Debug.Log(gameObject.name);

            if (gameObject.name == "Pack1")
              SX.SendMessage("Purchase","words_us_p1");

             if (gameObject.name == "Pack2")
                SX.SendMessage("Purchase","words_us_p2");

                
            if (gameObject.name == "Pack3")
                SX.SendMessage("Purchase","words_us_p3");

             if (gameObject.name == "Pack4")
                SX.SendMessage("Purchase","words_us_p4");

                
            if (gameObject.name == "Pack5")
                SX.SendMessage("Purchase","words_us_p5");



            if (gameObject.name == "SmarBanner")
                SX.SendMessage("showWhenReadyBannerLoad");
                
            if (gameObject.name == "SmarBannerHide")
                SX.SendMessage("smartBanneHide");

            if (gameObject.name == "SmarBannerDestroy")
                SX.SendMessage("smartBanneDestroy");

            if (gameObject.name == "SmarBannerLoad")
                SX.SendMessage("smartBanneLoad");

            if (gameObject.name == "SmarBannerShow")
                SX.SendMessage("smartBanneShow");               



            if (gameObject.name == "ReverdLoad")
                SX.SendMessage("rewardedAdsLoad");

            if (gameObject.name == "ReverdShow")
                SX.SendMessage("showWhenReadyRewarded");

                




            if (gameObject.name == "FullBanner")
                SX.SendMessage("showWhenReadyNonRewarded");






            if (gameObject.name == "fb")
                Application.OpenURL("https://www.facebook.com/SayrexGames");

            if (gameObject.name == "GC")
                SX.GetComponent <SX_GameCenter>().showLeaderBoards(SX.GetComponent <SX_GameCenter>().iLeaderboardId);     
            
            if (gameObject.name == "ARCH")
               SX.SendMessage("showArchievements"); 

            if (gameObject.name == "LoadScore")
               SX.SendMessage("loadPlayerScore", SX.GetComponent <SX_GameCenter>().iLeaderboardId);            

            if (gameObject.name == "SubmitScore"){
                gameObject.GetComponentInChildren<UILabel>().text = 360.ToString();
                SX.GetComponent <SX_GameCenter>().submitPlayerScore(null, 360);
            }
                      
		}
	}

}
