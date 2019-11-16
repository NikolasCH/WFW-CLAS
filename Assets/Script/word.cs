using UnityEngine;
using System.Collections;

public class word : MonoBehaviour {
	GameObject letter;
	GameObject _parent;
	public UIAtlas _atlas;
	public static GameObject _word;
    public Font _font;
	GameObject _goButton;
	GameObject center;
	public static int btn1 = 3;
	public static int btn2 = 4;
	public static int btn3 = 5;
	public static int btn = 4;
	public Camera main;
	private int _open = 100;
	private int _open_word = 90;
	private int del_p = 100;
	private bool panel = false;
	private bool panel_buy = false;
	GameObject Panel_Down;
	GameObject Panel_Buy;
	
//string thisValue = (string)w[theKey];  
	
	
	void Start () {
		_word = gameObject;
		gameObject.SendMessage("onNew");
		Main.Play();
		_parent= gameObject;
		word_latter ();
		Invoke("play",1f);
		Panel_Down = GameObject.Find("Panel_Down");
		Panel_Buy = GameObject.Find("Panel_Buy");
	}
	
	void word_latter () {
		for(int i=0; i<Main.letter; i++)
		{
			
			if(Main.word.Length<10)
			{
				CreateButton(_parent,(-90*(Main.letter-1))/2+90f*i,-100f, 1f, 1f , "", i);
			}
			else if(Main.word.Length<13)
			{
				CreateButton(_parent,((-90*(Main.letter-1))/2+90f*i)*0.95f,-100f, 0.95f, 0.95f , "", i);	
			}
			else
			{
				CreateButton(_parent,((-90*(Main.letter-1))/2+90f*i)*0.85f,-100f, 0.85f, 0.85f , "", i);	
			}
			
			

			if(PlayerPrefs.GetInt("open")==1){
				UILabel nl = (UILabel)GameObject.Find("lbl"+i).GetComponent("UILabel");
				nl.text = Main.word[i].ToString().ToUpper();
			}	
		}
		//_open_word*=Main.word.Length;
		//UILabel word_p = (UILabel)GameObject.Find("coin_word").GetComponent("UILabel");
		//word_p.text = _open_word.ToString();
	}
	
	public static void btn10 (string _btn, GameObject Button) {
		
		if(Main.Coin>=btn1){		
			NGUITools.SetActive(Button, false);	
			Main.Coin-=btn1;
			Main.CoinUp();
			btn-=1;
			PlayerPrefs.SetInt(_btn,1);
			Main.b1 = true;
			_word.SendMessage("Coin");
		}
	}
	
	public static void btn20 (string _btn, GameObject Button) {
		if(Main.Coin>=btn2){		
			NGUITools.SetActive(Button, false);
			Main.Coin-=btn2;
			Main.CoinUp();
			btn-=1;
			PlayerPrefs.SetInt(_btn,1);
			Main.b2=true;
			_word.SendMessage("Coin");
		}
	}
	
	public static void btn30 (string _btn , GameObject Button) {
		if(Main.Coin>=btn3){
			NGUITools.SetActive(Button, false);
			Main.Coin-=btn3;	
			Main.CoinUp();
			btn-=1;
			PlayerPrefs.SetInt(_btn ,1);
			Main.b3=true;
			_word.SendMessage("Coin");
		}
	}
	
	void CreateButton(GameObject go, float x, float y, float sx, float sy, string name, int number)
    {
        int depth = 0;//NGUITools.CalculateNextDepth(go);
        go = NGUITools .AddChild(go);

		//go = Resources.Load("l") as GameObject;
		go.transform.localPosition = new Vector3(x, y, 0f);

		go.transform.localScale = new Vector3(sx, sy, 1f);
		

        go.name = "letter"+number.ToString();
		
		letters bt = go.AddComponent<letters>();
		//go.AddComponent<UIButtonScale>();
		//go.AddComponent<UIButtonOffset>();
		
		//UIButtonSound tp = go.AddComponent<UIButtonSound>();

        _goButton = go;

        UISprite bg = NGUITools.AddWidget<UISprite>(go);
        bg.name = "Background";
        bg.depth = depth;
		bg.type = UISprite.Type.Sliced;
        bg.atlas = _atlas;
		//bg.color = new Color(0.8f, 0.8f , 0.8f , 1f);
		bg.spriteName = "btn_8";
        //bg.transform.localScale = new Vector3(sx/10f, sy/10f, 1f);
       // bg.MakePixelPerfect();
		
        UILabel lbl = NGUITools.AddWidget<UILabel>(go);
		lbl.name = "lbl"+number.ToString();
		GameObject lb  = GameObject.Find("lbl"+number.ToString());	
		lb.transform.localPosition = new Vector3(0f, -5f, 0f);
		lbl.trueTypeFont = _font;
		lbl.fontSize = 60;
      	if(PlayerPrefs.GetInt("o0")==number || PlayerPrefs.GetInt("o1")==number || PlayerPrefs.GetInt("o2")==number || PlayerPrefs.GetInt("o3")==number || PlayerPrefs.GetInt("o4")==number || PlayerPrefs.GetInt("o5")==number || PlayerPrefs.GetInt("o6")==number || PlayerPrefs.GetInt("o7")==number || PlayerPrefs.GetInt("o8")==number) lbl.text = Main.word[number].ToString();
        lbl.MakePixelPerfect();
		lb.transform.localScale = new Vector3(1, 1, 1f);
		lbl.text = Main.word[number].ToString().ToUpper();
		lbl.color = new Color(0f, 0f , 0f , 1f);
		//lbl.effectStyle = UILabel.Effect.Outline;
		//lbl.effectColor = new Color(1f, 1f , 1f , 1f); 

		bt.letter = Main.word[number].ToString().ToUpper();
		bt.lbl =lbl;
		bt.num = number;

        // Add a collider
     
		NGUITools.AddWidgetCollider(go);
		//col.size = new Vector3(150, 150, 1);
        // Add the scripts
       // TweenPosition tp = go.AddComponent<TweenPosition>();
       // tp.to = new Vector3(0, 200, 0);
       // tp.duration = 4f;
       // tp.Play(true);
        //Invoke("RemoveButton", 5f);

    
				
	}
	
	
	//	StartCoroutine( coTween( subMenu, 0.5f, mPos) );
	
	
	public void good_wait ( ) {
		Main.game = false;
		Invoke("good",0.6f);
	}
	
	public void bad_wait ( ) {
		Main.game = false;
		Invoke("bad",0.6f);
	}
	
	public  void good ( ) {	
		gameObject.SendMessage("onWin");
		if(btn<1)btn=1;
		int score=1500*btn;
		int coin = 30;
		//Main.ScoreAdd(score);
		Main.ScoreUp();
		Main.CoinAdd(coin);
		Main.CoinUp();
		GameObject[] menus = GameObject.FindGameObjectsWithTag("menu");
		foreach(GameObject go in menus)
		NGUITools.SetActive(go, false);
		
		Vector3 Point = main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2,0));	
		Point.y*=(-1);
		Main.play = false;
		//Main.LevelAdd();
		NGUITools.SetActive( Main.bg, true);
		if(Main.b1){
			UISprite b1 = (UISprite)GameObject.Find("b1").GetComponent("UISprite");
			b1.spriteName = "lock copy";
			b1.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		if(Main.b2){
			UISprite b2 = (UISprite)GameObject.Find("b2").GetComponent("UISprite");
			b2.spriteName = "lock copy";
			b2.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		if(Main.b3){
			UISprite b3 = (UISprite)GameObject.Find("b3").GetComponent("UISprite");
			b3.spriteName = "lock copy";
			b3.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		
		UILabel score_b = (UILabel)GameObject.Find("score_b").GetComponent("UILabel");		
		score_b.text = score.ToString();
		
		UILabel coin_b = (UILabel)GameObject.Find("coin_b").GetComponent("UILabel");		
		coin_b.text = coin.ToString();
		
		Invoke("next",4f);
		StartCoroutine( coTween( gameObject, 2f, Point) );
	}
	
	public void bad ( ) {	
		Main.game = false;
		gameObject.SendMessage("onWrong");
		GameObject[] menus = GameObject.FindGameObjectsWithTag("letter");
		foreach(GameObject go in menus)
			go.SendMessage("addLetters");

		Main.game = true;
		Main.check_word();
	}

	public void goodWord ( ) {	
		gameObject.SendMessage("onWin");
		GameObject[] menus = GameObject.FindGameObjectsWithTag("letter");
		foreach(GameObject go in menus)
			go.SendMessage("addLetters");
		//Main.game = true;
	}
	
	public void play ( ) {	
		Main.play = true;
	}
	
	public void ScreenShotTake ( ) {
		ScreenCapture.CaptureScreenshot("Screenshot.png");
		string email = "";
		string subject = "Помоги угадать слово!";
		string body = "Подскажи, немогу пройти уровень";
		Application.OpenURL("mailto:" + email + "?subject:" + subject + "&body:" + body);
	}
	
	public void open ( ) {	
		
			ArrayList m =new ArrayList(); 
			for(int j=0; j<Main.word.Length; j++)
			{
				if(PlayerPrefs.GetInt("o"+j)<0 ) m.Add(j);				
			}
		
		if(Main.Coin>=_open && m.Count>0){			
			Main.Coin-=_open;
			Main.CoinUp();
			int n = Random.Range(0, m.Count-1);		
			Debug.Log(n);
			int num_lett =(int)m[n];
			PlayerPrefs.SetInt("o"+num_lett, num_lett);		
			UILabel nl = (UILabel)GameObject.Find("lbl"+num_lett).GetComponent("UILabel");
			nl.text = Main.word[num_lett].ToString().ToUpper();
			//panel_down();
		}
	}
	
	public void open_word ( ) {				
		if(Main.Coin>=_open_word){			
			Main.Coin-=_open_word;
			Main.CoinUp();
			for(int i=0; i<Main.word.Length; i++)
			{	
				UILabel nl = (UILabel)GameObject.Find("lbl"+i).GetComponent("UILabel");
				nl.text = Main.word[i].ToString().ToUpper();
				PlayerPrefs.SetInt("open", 1);		
			}
			panel_down();
		}
	}
	
	public void buy ( ) {	
			panel_down();
			panel_buy_up();
	}
	
	public void del ( ) {
		
		ArrayList m =new ArrayList(); 
		for(int j=1; j<15; j++)
		{
			string t = Main.letters[j-1].ToString().ToUpper();
			bool _del = true;			
			for(int i=0; i<Main.word.Length; i++)
			{	
				if(t==Main.word[i].ToString().ToUpper() || PlayerPrefs.GetInt("l"+j.ToString())==1)_del = false;	
			}
			if(_del) m.Add(j);					
		}
		
		if(Main.Coin>=del_p && m.Count>0){
			Main.Coin-=del_p;
			Main.CoinUp();	
			int n = Random.Range(0, m.Count-1);
			int num_lett =(int)m[n];		
			Debug.Log(num_lett);
			PlayerPrefs.SetInt("l"+num_lett.ToString(), 1);
			GameObject p = GameObject.Find("p"+num_lett.ToString());
			Destroy(p);
			//panel_down();
		}
	}
	
	public void panel_down(){
		if(panel){
			panel = false;		
			TweenPosition tp = Panel_Down.AddComponent<TweenPosition>();
			tp.from = Panel_Down.transform.localPosition;
			tp.to = new Vector3(0, -900, -500);
			tp.duration = 0.5f;
			tp.Play(true);		
		} else {
			panel_up();
		}
	}
	public void panel_up(){
		if(!panel){
			panel = true;	
			TweenPosition tp = Panel_Down.AddComponent<TweenPosition>();
			tp.from = Panel_Down.transform.localPosition;
			tp.to = new Vector3(0, 0, -500);
			tp.duration = 0.5f;
			tp.Play(true);		
		} else {
			panel_down();
		}
		if(panel_buy)panel_buy_down();
	}
	
	public void panel_buy_down(){
		if(panel_buy){
			panel_buy = false;		
			TweenPosition tp = Panel_Buy.AddComponent<TweenPosition>();
			tp.from = Panel_Buy.transform.localPosition;
			tp.to = new Vector3(0, -900, -500);
			tp.duration = 0.5f;
			tp.Play(true);		
		} else {
			panel_buy_up();
		}
	}
	public void panel_buy_up(){
		if(!panel_buy){
			panel_buy = true;	
			TweenPosition tp = Panel_Buy.AddComponent<TweenPosition>();
			tp.from = Panel_Buy.transform.localPosition;
			tp.to = new Vector3(0, 0, -500);
			tp.duration = 0.5f;
			tp.Play(true);		
		} else {
			panel_buy_down();
		}
		if(panel)panel_down();
	}
	
	public void fb(){
			Application.OpenURL("https://www.facebook.com/SayrexGames");	
	}
	
	public void gc(){
			Main.onLeaderBoard();	
	}
	
	public void next ( ) {	
		Main.game = true;
		
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
		PlayerPrefs.SetInt("open", 0);	
		PlayerPrefs.SetInt("num_lett", -1);
		Debug.Log("Next");
		Main.b1=false;
		Main.b2=false;
		Main.b3=false;
		GameObject next = GameObject.Find("next");
		next.SendMessage("next");
	}
	
	
    IEnumerator coTween( GameObject Obj, float time, Vector3 toPos) 
    {
		Transform transform = Obj.transform;
        Vector3 fromPos = transform.position;
        for (float t = 0; t < time; t += Time.deltaTime) {
            float nt = Mathf.Clamp01( t / time );
            nt = Mathf.Sin(nt * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp( fromPos, toPos, nt );
            yield return 0;
        }
    }
}


