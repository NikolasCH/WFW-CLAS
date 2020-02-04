using UnityEngine;
using System.Collections;
using SA.Android.Firebase.Analytics;
public class Buttons : Main {

	private AsyncOperation async;
	private bool Loading = false;

	private bool sound = true;
	private int scene = 0;

	private GameObject Panel_Buy;


	//	private UILabel label;
	
	void Start () 
	{
		Main.panel_buy = false;	
		Panel_Buy = GameObject.Find("Panel_Buy");

		if (PlayerPrefs.GetInt ("sound") == 1 && gameObject.name == "sound") {
			sound = false;
            GetComponentInChildren<UISprite>().spriteName = "btn_sound_off";
			AudioListener.volume = 0;
		} 	

		if (gameObject.name == "Restore" && PlayerPrefs.GetInt("Restore") == 1)
		{
			gameObject.SetActive(false);
		}	
	}

	void OnPress (bool isDown)
	{
		if (isDown == false)
		{

			AN_FirebaseAnalytics.LogEvent(gameObject.name);

			if(gameObject.name=="More")
				Main.showMoreApps();
				
			if(gameObject.name=="X" && (stars>=iPage.unlock[PlayerPrefs.GetInt("page")])){
				if(PlayerPrefs.GetInt("page")<1)
					iMap.MSG_1.SetActive(true);
				else if (PlayerPrefs.GetInt("page")<7)
					iMap.MSG_2.SetActive(true);
				else 
					iMap.MSG_3.SetActive(true);
			}

			if(gameObject.name=="CloseMSG"){
				iMap.MSG_1.SetActive(false);
				iMap.MSG_2.SetActive(false);
				iMap.MSG_3.SetActive(false);
			}

			if(gameObject.name=="Map"){
				LoadScene(1);
			}
			if(gameObject.name=="Start"){
				PlayerPrefs.SetInt("page", 0);
				LoadScene(1);
			}
			
			if(gameObject.name=="Level" || gameObject.name=="Next")
				LoadScene(2);
			
			if(gameObject.name=="Panel_buy")
				panel_buy_up();

			if(gameObject.name=="Sharing")
				SX.SendMessage("Sharing");   

			if(gameObject.name=="Panel_buys")
				panel_buy_ups();

			if(gameObject.name=="Back")
				LoadScene(0);	

			if(gameObject.name=="o_letters")
				Main.Open_Letters();

			if(gameObject.name=="o_words")
				Main.Open_Word();

			if(gameObject.name=="o_wordss")
				Main.Open_Words();

 			if(gameObject.name == "AdsRevard")
                GameObject.Find ("SX").SendMessage("rewardedAdsShow");

			if(gameObject.name=="Backspace" && Main.next>0)
			{
				GameObject w = GameObject.Find("letter"+(Main.next-1).ToString());;
				w.SendMessage("addLetters");
				//Main.check_next();
			}

			if(gameObject.name=="Page_right")
			{
				int page = 0;
				if(PlayerPrefs.HasKey("page"))page = PlayerPrefs.GetInt("page");
				page--;
				if(page<0)page=0;
				PlayerPrefs.SetInt("page", page);
				LoadScene(1);
			}

			if(gameObject.name=="pack1")Main.onBuy("wfw2_p1");
			if(gameObject.name=="pack2")Main.onBuy("wfw2_p2");
			if(gameObject.name=="pack3")Main.onBuy("wfw2_p3");
			if(gameObject.name=="pack4")Main.onBuy("wfw2_p4");
			if(gameObject.name=="pack5")Main.onBuy("wfw2_p5");	
			if(gameObject.name=="buy_page")Main.onBuy("page_"+PlayerPrefs.GetInt("page").ToString());


			if(gameObject.name=="Page_left")
			{
				int page = 0;
				if(PlayerPrefs.HasKey("page"))page = PlayerPrefs.GetInt("page");
				page++;
				if(page>7)page=7;
				PlayerPrefs.SetInt("page", page);
				LoadScene(1); 
			}

			if(gameObject.name=="FaceBook")
				Application.OpenURL("https://www.facebook.com/SayrexGames/");	
			
			if(gameObject.name=="Sayrex")
				Application.OpenURL("https://www.facebook.com/SayrexGames/");


			if(gameObject.name=="GameCenter")
				Main.onLeaderBoard();	

			if(gameObject.name=="ARCH")
				Main.SX.GetComponent<SX_GameCenter>().showArchievements();


			if (gameObject.name == "sound") {
				Debug.Log("Here");
				if (sound) {					
					PlayerPrefs.SetInt ("sound", 1);
					sound = false;
                    gameObject.GetComponentInChildren<UISprite>().spriteName = "btn_sound_off";
					AudioListener.volume = 0;
				} else {
					PlayerPrefs.SetInt ("sound", 0);
					sound = true;
                    gameObject.GetComponentInChildren<UISprite>().spriteName = "btn_sound_on";
					AudioListener.volume = 1;
				}
			}

						
			if (gameObject.name == "Restore" )
			{
				SX.SendMessage("Restore");
				gameObject.SetActive(false);
			}
		}
	}

	//function

	void LoadScene (int _scene) 
	{
		scene = _scene;	
		Loading = true;
		StartCoroutine("_Start");
	}

	IEnumerator _Start() {
		async = Application.LoadLevelAsync(scene);	
		yield return async;		
	}

	public void panel_buy_down(){
		if(Main.panel_buy){
			Main.panel_buy = false;		
			TweenPosition tp = Panel_Buy.AddComponent<TweenPosition>();
			tp.from = Panel_Buy.transform.localPosition;
			tp.to = new Vector3(0, 900, -1);
			tp.duration = 0.5f;
			tp.Play(true);		
		} else {
			panel_buy_up();
		}
	}
	public void panel_buy_up(){
		if(!Main.panel_buy){
			Main.panel_buy = true;	
			TweenPosition tp = Panel_Buy.AddComponent<TweenPosition>();
			tp.from = Panel_Buy.transform.localPosition;
			tp.to = new Vector3(0, 20, -1);
			tp.duration = 0.5f;
			tp.Play(true);		
		} else {
			panel_buy_down();
		}
	}
	
	public void panel_buy_ups(){
		if(!Main.panel_buy){
			Main.panel_buy = true;	
			TweenPosition tp = Panel_Buy.AddComponent<TweenPosition>();
			tp.from = Panel_Buy.transform.localPosition;
			tp.to = new Vector3(0, -120, -1);
			tp.duration = 0.5f;
			tp.Play(true);		
		} else {
			panel_buy_down();
		}
	}
	public static void ups(){
		GameObject Panel_Buys = GameObject.Find("Panel_Buy");
		if(!Main.panel_buy){
			Main.panel_buy = true;	
			TweenPosition tp = Panel_Buys.AddComponent<TweenPosition>();
			tp.from = Panel_Buys.transform.localPosition;
			tp.to = new Vector3(0, -120, -1);
			tp.duration = 0.5f;
			tp.Play(true);		
		} else {
			Main.panel_buy = false;		
			TweenPosition tp = Panel_Buys.AddComponent<TweenPosition>();
			tp.from = Panel_Buys.transform.localPosition;
			tp.to = new Vector3(0, 900, -1);
			tp.duration = 0.5f;
			tp.Play(true);		
		}
	}
}
