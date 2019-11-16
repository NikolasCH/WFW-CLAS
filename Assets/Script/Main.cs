using UnityEngine;
using System.Collections;
using SA.Android.Firebase.Analytics;
public class Main : MonoBehaviour {
	
	public static int Score = 0;
	public static int lvl = 1;	
	public static int Coin = 500;
	public static bool play = false;
	public static int ShowTime = 0;
	public static int letter=0;
	public static string[] letters;
	public static string txt="w";
	public static string word = "угадай";
	public static string _word = "";
	public static int next = 0;
	public static int [] p =  new[]{-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
	public static GameObject bg;
	public static bool game =true;
	public static UILabel _txt;
	public static UILabel _check;

	public static UILabel _w_o;
	public static UILabel _ws_o;
	public static UILabel _l_o;
	public static bool cam = false;
	public static bool b1=false;	
	public static bool b2=false;
	public static bool b3=false;
	public static int stars=0;
	public static UISprite s_stars;

	public static UISprite s_stars1;
	public static UISprite s_stars2;
	public static UISprite s_stars3;

	public static Map iMap;
	public static page iPage;

	
	public static int w_o=10;
	public static int ws_o=0;
	public static int l_o=0;

	public static bool panel_buy = false;
		
	public static GameObject SX;
	
	public static Hashtable w = new Hashtable();  
	public static Hashtable ws = new Hashtable();  
	
	public static void check_next (bool check) {
		int n=-1;
		for(int j=0; j<p.Length; j++)
		{	
			for(int i=0; i<p.Length; i++)
			{
				if(p[i]>n && p[i]-1==n)
					n=p[i];

				next=n+1;

			}
		}
		if(!check)
			check_word();
	}

	public static void check_stars() {
		stars =PlayerPrefs.GetInt("Star");
	}


	
	public static void check_word () {

		Debug.Log("check_word");

		int l=0;
		string t ="";

		string text ="";

		for(int i=0; i<letters.Length; i++)
		{
			 t = t+(string)w[i];
		}


		string g_word = "";

		if(PlayerPrefs.HasKey("lvl_"+lvl.ToString()))
			g_word = PlayerPrefs.GetString("lvl_"+lvl.ToString());


		string[] col_word = g_word.Split(","[0]);

		int c_w = 0;
		if(PlayerPrefs.HasKey("lvl_"+lvl.ToString()))c_w = col_word.Length;

		_check.text = c_w.ToString()+"/"+letters.Length.ToString();

	

// проверяем сложилли пользователь слово
		for(int j=0; j<letters.Length; j++)
		{
			if(ws[t]==null && t==letters[j].ToString().ToUpper() && game){
				Debug.Log(t+ "- "+ws[t]+" -");
				ws[t] = 1;
				l+=1;

				if(g_word=="")
					PlayerPrefs.SetString("lvl_"+lvl.ToString(), t);
				else
					PlayerPrefs.SetString("lvl_"+lvl.ToString(), g_word+","+t);

				CoinAdd(1+PlayerPrefs.GetInt("page"));

				AN_FirebaseAnalytics.LogEvent("open_word");

				c_w++;
				_check.text = c_w.ToString()+"/"+letters.Length.ToString();
				int _star = 0;
				if(c_w*100/letters.Length>=33)_star = 1;
				if(c_w*100/letters.Length>=66)_star = 2;
				if(c_w*100/letters.Length>=99)_star = 3;
				if(_star>PlayerPrefs.GetInt("s_"+lvl.ToString())){
					PlayerPrefs.SetInt("s_"+lvl.ToString(), _star);
					stars=PlayerPrefs.GetInt("Star");
					stars++;
					PlayerPrefs.SetInt("Star", stars);
					ScoreUp();
				}


				GameObject w = GameObject.Find("word");
				w.SendMessage("goodWord");	
			}
		}

		g_word = PlayerPrefs.GetString("lvl_"+lvl.ToString());
		
		col_word = g_word.Split(","[0]);	

		text = "[000000]";

		

		for(int i=0; i<letters.Length; i++)
		{
			bool open_word = false;
			for(int s=0; s<col_word.Length; s++)
			{
				if(col_word[s]==letters[i].ToString().ToUpper())
				{
					bool _color = false;
					if(t.Length>0 && c_w>0 && t.Length<=col_word[s].Length){

						int p = 0;
						for(int a=0; a<t.Length; a++)
						{

							if(t[a]==col_word[s][a])p++;

						}
						if(p==t.Length)_color =true;

					}

					ws[col_word[s]] = 1;
					if(_color)
						text +="    [EB2F00]"+col_word[s]+"[-]";
					else
						text +="    "+col_word[s];


					open_word =true;
				}
			}
			if(!open_word)
			{
				text +="    ";
				for(int d=0; d<letters[i].Length; d++)
				{
					text +="●";
				}
			}


		}


		_txt.text = text+"";


		PlayerPrefs.SetFloat("w_"+lvl.ToString(), letters.Length);
		PlayerPrefs.SetFloat("c_"+lvl.ToString(), c_w);

		s_stars.fillAmount =(float)c_w/letters.Length;

		s_stars1.gameObject.SetActive(c_w*100/letters.Length>=33);	
		s_stars2.gameObject.SetActive(c_w*100/letters.Length>=66);
		s_stars3.gameObject.SetActive(c_w*100/letters.Length>=99);
		

	
		Cost_Word();

		if( PlayerPrefs.GetFloat("w_"+lvl.ToString())==PlayerPrefs.GetFloat("c_"+lvl.ToString()) ){
		    ArrayList f = new ArrayList { "Супер!", "Отлично!", "Невероятно!", "Круто!", "Прекрасно!", "Восхитительно!", "Превосходно!", "Поразительно!", "Прекрасно!", "Удивительно!", "Потрясающе!", "Фантастика!", "Изумительно!", "Потрясно!", "Сногсшибательно!", "Головокружительно!", "Грандиозно!", "Великолепно!" };
        	GameObject finish = GameObject.Find("finish");
			finish.GetComponent<UILabel>().text = f[Random.Range(0, f.Count - 1)].ToString();
			finish.GetComponent<TweenScale>().ResetToBeginning();
        	finish.GetComponent<TweenScale>().Play(true);
		}
		
	}
	

	public static void onArchiments(){
		

		SX_GameCenter GameCenter = SX.GetComponent <SX_GameCenter>();

		if (PlayerPrefs.GetInt("Star") >= 100 && !PlayerPrefs.HasKey ("achievement_100")) {
			PlayerPrefs.SetInt ("achievement_100", 1);
			GameCenter.unlockAchievement ("CgkIo9v11o8TEAIQAQ");
		} 
		
		 if (PlayerPrefs.GetInt("Star") >= 200 && !PlayerPrefs.HasKey ("achievement_200")) {
			PlayerPrefs.SetInt ("achievement_200", 1);
			GameCenter.unlockAchievement ("CgkIo9v11o8TEAIQAg");
		} 
		
		 if (PlayerPrefs.GetInt("Star") >= 300 && !PlayerPrefs.HasKey ("achievement_300")) {
			PlayerPrefs.SetInt ("achievement_300", 1);
			GameCenter.unlockAchievement ("CgkIo9v11o8TEAIQAw");
		} 
		
		 if (PlayerPrefs.GetInt("Star") >= 400 && !PlayerPrefs.HasKey ("achievement_400")) {
			PlayerPrefs.SetInt ("achievement_400", 1);
			GameCenter.unlockAchievement ("CgkIo9v11o8TEAIQBA");
		} 
		
		 if (PlayerPrefs.GetInt("Star") >= 500 && !PlayerPrefs.HasKey ("achievement_500")) {
			PlayerPrefs.SetInt ("achievement_500", 1);
			GameCenter.unlockAchievement ("CgkIo9v11o8TEAIQBQ");
		} 
		
		 if (PlayerPrefs.GetInt("Star") >= 600 && !PlayerPrefs.HasKey ("achievement_600")) {
			PlayerPrefs.SetInt ("achievement_600", 1);
			GameCenter.unlockAchievement ("CgkIo9v11o8TEAIQBw");
		} 

		if (PlayerPrefs.GetInt("Coin") >= 1000 && !PlayerPrefs.HasKey ("coin_1000")) {
			PlayerPrefs.SetInt ("coin_1000", 1);
			GameCenter.unlockAchievement ("CgkIo9v11o8TEAIQCA");
		} else if (PlayerPrefs.GetInt("Coin") >= 2000 && !PlayerPrefs.HasKey ("coin_2000")) {
			PlayerPrefs.SetInt ("coin_2000", 1);
			GameCenter.unlockAchievement ("CgkIo9v11o8TEAIQCQ");
		} else if (PlayerPrefs.GetInt("Coin") >= 3000 && !PlayerPrefs.HasKey ("coin_3000")) {
			PlayerPrefs.SetInt ("coin_3000", 1);
			GameCenter.unlockAchievement ("CgkIo9v11o8TEAIQCg");
		}else if (PlayerPrefs.GetInt("Coin") >= 4000 && !PlayerPrefs.HasKey ("coin_4000")) {
			PlayerPrefs.SetInt ("coin_4000", 1);
			GameCenter.unlockAchievement ("CgkIo9v11o8TEAIQCw");
		} else if (PlayerPrefs.GetInt("Coin") >= 5000 && !PlayerPrefs.HasKey ("coin_5000")) {
			PlayerPrefs.SetInt ("coin_5000", 1);
			GameCenter.unlockAchievement ("CgkIo9v11o8TEAIQDA");
		} 
	}

	public static void Play(){

		AN_FirebaseAnalytics.LogEvent("StartLevel");
		_l_o = (UILabel)GameObject.Find("_l_o").GetComponent("UILabel");

		_txt = (UILabel)GameObject.Find("txt").GetComponent("UILabel");
		_check = (UILabel)GameObject.Find("check").GetComponent("UILabel");
		s_stars = (UISprite)GameObject.Find("Star1").GetComponent("UISprite");
		s_stars1 = (UISprite)GameObject.Find("Star_1").GetComponent("UISprite");
		s_stars2 = (UISprite)GameObject.Find("Star_2").GetComponent("UISprite");
		s_stars3 = (UISprite)GameObject.Find("Star_3").GetComponent("UISprite");

		SX.GetComponent<SX_Ads>().RevardsButton = GameObject.Find("AdsRevard");
		SX.GetComponent<SX_Ads>().rewardedAdsLoad();

		s_stars1.gameObject.SetActive(false);
		s_stars2.gameObject.SetActive(false);
		s_stars3.gameObject.SetActive(false);

		Coin = PlayerPrefs.GetInt("Coin");

		stars= PlayerPrefs.GetInt("Star");
		lvl = PlayerPrefs.GetInt("lvl");

		postScoreToLeaderBoard( 0, stars);

		p = new[]{-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
		w.Clear();
		ws.Clear();
		next=0;
		word = baza.words[lvl,0];	
		letters =  baza.words[lvl,2].Split(","[0]);

		bg  = GameObject.Find("bg");

		NGUITools.SetActive(bg, false);
		stars =PlayerPrefs.GetInt("Star");
		ScoreUp();
		CoinUp();

		letter = word.Length;

		LevelUp();

		check_word ();
	}


	public static void Cost_Word () {	
		w_o = 0;
		//ws_o = 0;
		for(int f=0; f<letters.Length; f++)
		{
			string t =letters[f].ToString().ToUpper();
			if(ws[t]==null){
				if(w_o==0)w_o=10+(10*PlayerPrefs.GetInt("page"));
			}
		}
		
		_l_o.text= w_o.ToString();
		//_ws_o.text= ws_o.ToString();
	}

	public static void Open_Word () {	
		if(Coin>=w_o){

			AN_FirebaseAnalytics.LogEvent("buy_word");

			Coin-=w_o;
			PlayerPrefs.SetInt("Coin", Coin);

			UILabel Txt = (UILabel)GameObject.Find("coin_lbl").GetComponent("UILabel");		
			Txt.text = Coin.ToString();

			string g_word = "";
			
			if(PlayerPrefs.HasKey("lvl_"+lvl.ToString()))
				g_word = PlayerPrefs.GetString("lvl_"+lvl.ToString());
			
			
			string[] col_word = g_word.Split(","[0]);
			int c_w = 0;
			if(PlayerPrefs.HasKey("lvl_"+lvl.ToString()))c_w = col_word.Length;

			for(int f=0; f<letters.Length; f++)
			{
				string t =letters[f].ToString().ToUpper();
				if(ws[t]==null){
					_txt.text +=" "+t;
					ws[t] =1;

					if(g_word=="")
						PlayerPrefs.SetString("lvl_"+lvl.ToString(), t);
					else
						PlayerPrefs.SetString("lvl_"+lvl.ToString(), g_word+","+t);

					int _star = 0;
					c_w++;

					if(c_w*100/letters.Length>=33)_star = 1;
					if(c_w*100/letters.Length>=66)_star = 2;
					if(c_w*100/letters.Length>=99)_star = 3;

					if(_star>PlayerPrefs.GetInt("s_"+lvl.ToString())){
						PlayerPrefs.SetInt("s_"+lvl.ToString(), _star);
						stars=PlayerPrefs.GetInt("Star");
						stars++;
						PlayerPrefs.SetInt("Star", stars);
						ScoreUp();
					}
	
					break;
				}
			}
			check_word();
		}else{
			Buttons.ups();
		}
	}

	public static void Open_Words () {	
		if(Coin>=ws_o){
			Coin-=ws_o;
			PlayerPrefs.SetInt("Coin", Coin);
			
			UILabel Txt = (UILabel)GameObject.Find("coin_lbl").GetComponent("UILabel");		
			Txt.text = Coin.ToString();
			
			string g_word = "";
			
			if(PlayerPrefs.HasKey("lvl_"+lvl.ToString()))
				g_word = PlayerPrefs.GetString("lvl_"+lvl.ToString());
			
			
			string[] col_word = g_word.Split(","[0]);
			int c_w = 0;
			if(PlayerPrefs.HasKey("lvl_"+lvl.ToString()))c_w = col_word.Length;
			
			for(int f=0; f<letters.Length; f++)
			{
				string t =letters[f].ToString().ToUpper();
				if(ws[t]==null){
					_txt.text +=" "+t;
					ws[t] =1;
					
					if(g_word=="")
						g_word=t;
					else
						g_word+=","+t;
					
					PlayerPrefs.SetString("lvl_"+lvl.ToString(), g_word);
					
					int _star = 0;
					c_w++;
					
					if(c_w*100/letters.Length>=33)_star = 1;
					if(c_w*100/letters.Length>=66)_star = 2;
					if(c_w*100/letters.Length>=99)_star = 3;
					
					if(_star>PlayerPrefs.GetInt("s_"+lvl.ToString())){
						PlayerPrefs.SetInt("s_"+lvl.ToString(), _star);
						stars=PlayerPrefs.GetInt("Star");
						stars++;
						PlayerPrefs.SetInt("Star", stars);
						ScoreUp();
					}
				}
			}
			check_word();
		}else{
			Buttons.ups();
		}
	}



	public static void Cost_Letters () {	
		int k = 0;
		l_o = 0;
		if(PlayerPrefs.HasKey("o_"+lvl.ToString()))k = PlayerPrefs.GetInt("o_"+lvl.ToString());
		for(int f=0; f<letters.Length; f++)
		{
			string t =letters[f].ToString().ToUpper();
			if(ws[t]==null){
				if(k<letters[f].Length)
					l_o=0;
			}
		}
		_w_o.text= l_o.ToString();
	}

	public static void Open_Letters () {	
		if(Coin>=l_o){
			Coin-=l_o;
			PlayerPrefs.SetInt("Coin", Coin);
			UILabel Txt = (UILabel)GameObject.Find("coin_lbl").GetComponent("UILabel");		
			Txt.text = Coin.ToString();

			int k = 0;
			if(PlayerPrefs.HasKey("o_"+lvl.ToString()))
				k = PlayerPrefs.GetInt("o_"+lvl.ToString());

			PlayerPrefs.SetInt("o_"+lvl.ToString(),k+1);

			check_word();
		}else{
			Buttons.ups();
		}
	}	

	public static void AddAdsRevard(){
		CoinAdd(150);
		AN_FirebaseAnalytics.LogEvent("AddAdsRevard");
		GameObject w = GameObject.Find("word");
		w.SendMessage("onCoin");
	}

	public static void ScoreUp ( ) {
		UILabel Txt = (UILabel)GameObject.Find("score_lbl").GetComponent("UILabel");	
		Txt.text = PlayerPrefs.GetInt("Star").ToString();			
	}	
	
	public static void CoinAdd (int AddScore) 
	{	
		PlayerPrefs.SetInt("Coin", Coin+AddScore);		
		CoinUp();
		onArchiments();
		Coins co = (Coins)GameObject.Find("coin_add").GetComponent("Coins");
		co.coins.text = "+"+AddScore.ToString(); 
		co.Add();
	}	
	
	public static void CoinUp ( ) 
	{	
		Coin  = PlayerPrefs.GetInt("Coin");
		GameObject.Find("coin_lbl").GetComponent<UILabel>().text = Coin.ToString();			
	}	

	public static void LevelAdd ( ) 
	{
		lvl = PlayerPrefs.GetInt("lvl")+1;
		PlayerPrefs.SetInt("lvl", lvl);
	}	
	
	public static void LevelUp ( ) {	
			UILabel Txt = (UILabel)GameObject.Find("lvl_lbl").GetComponent("UILabel");		
			Txt.text = lvl.ToString();
	}

	public static void good ( ) {		
		GameObject w = GameObject.Find("word");
		w.SendMessage("good_wait");		
	}
	
	public static void buy ( ) {		
		AN_FirebaseAnalytics.LogEvent("Buy_Coins");
		GameObject w = GameObject.Find("word");;
		w.SendMessage("onCoin");
	}
	
	public static void bad ( ) {		
		GameObject w = GameObject.Find("word");;
		w.SendMessage("bad_wait");
	}
	

	public static void onBuy (string product)
	{
		Debug.Log("InApp " + product);
		GameObject.Find("SX").GetComponent<SX_InApp_Android>().Purchase(product);
	}

	public static void showMoreApps ()
	{
		Application.OpenURL("https://play.google.com/store/apps/developer?id=Sayrex");
	}
	public static void onLeaderBoard ()
	{
		Debug.Log("onLeaderBoard");
		GameObject.Find("SX").GetComponent<SX_GameCenter>().showLeaderBoards(null);
	}

	public static void postScoreToLeaderBoard(int id,int _score)
	{
		SX.GetComponent<SX_GameCenter>().submitPlayerScore(null , _score);
	}

	public static void onShowInterstitial ()
	{
		Debug.Log ("onShowInterstitial");
		if (PlayerPrefs.GetInt ("Ad") != 1)
			SX.GetComponent<SX_Ads> ().showWhenReadyNonRewarded ();
	}


	public static void onShow(){
		Debug.Log(ShowTime);

	}
	
	
	public static void install(){		
		PlayerPrefs.SetInt("Btn1",0);
		PlayerPrefs.SetInt("Btn2",0);
		PlayerPrefs.SetInt("Btn3",0);
		PlayerPrefs.SetInt("Btn4",0);
		PlayerPrefs.SetInt("Btn5",0);
		PlayerPrefs.SetInt("Btn6",0);
		PlayerPrefs.SetInt("Btn7",0);
		PlayerPrefs.SetInt("Btn8",0);
		PlayerPrefs.SetInt("Btn9",0);
		PlayerPrefs.SetInt("Btn10",0);
		PlayerPrefs.SetInt("Btn11",0);
		PlayerPrefs.SetInt("Btn12",0);
			PlayerPrefs.SetInt("l1", -1);
			PlayerPrefs.SetInt("l2", -1);
			PlayerPrefs.SetInt("l3", -1);
			PlayerPrefs.SetInt("l4", -1);
			PlayerPrefs.SetInt("l5", -1);
			PlayerPrefs.SetInt("l6", -1);
			PlayerPrefs.SetInt("l7", -1);
			PlayerPrefs.SetInt("l8", -1);
			PlayerPrefs.SetInt("l9", -1);
			PlayerPrefs.SetInt("l10", -1);
			PlayerPrefs.SetInt("l11", -1);
			PlayerPrefs.SetInt("l12", -1);
			PlayerPrefs.SetInt("l13", -1);
			PlayerPrefs.SetInt("l14", -1);
			PlayerPrefs.SetInt("o0", -1);
			PlayerPrefs.SetInt("o1", -1);
			PlayerPrefs.SetInt("o2", -1);
			PlayerPrefs.SetInt("o3", -1);
			PlayerPrefs.SetInt("o4", -1);
			PlayerPrefs.SetInt("o5", -1);
			PlayerPrefs.SetInt("o6", -1);
			PlayerPrefs.SetInt("o7", -1);
			PlayerPrefs.SetInt("o8", -1);
			PlayerPrefs.SetInt("num_lett", -1);
			PlayerPrefs.SetInt("new_v2", 1);
			PlayerPrefs.SetInt("lvl", 1);
			PlayerPrefs.SetInt("Coin", 50);
			PlayerPrefs.SetInt("Score", 0);
			PlayerPrefs.SetInt("HIGHSCORE", 0);	
			PlayerPrefs.SetInt("open", 0);
			PlayerPrefs.SetInt("bon", 0);
			PlayerPrefs.SetInt("Ad",0);
	}
	
}
