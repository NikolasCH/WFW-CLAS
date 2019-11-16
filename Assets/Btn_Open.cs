using UnityEngine;
using System.Collections;

public class Btn_Open : MonoBehaviour {
	GameObject Panel_Buy;
	private bool panel_buy = false;
	// Use this for initialization
	void Start () {
		Panel_Buy = GameObject.Find("Panel_Buy");
	}
	
	public void panel_buy_down(){
		if(panel_buy){
			panel_buy = false;		
			TweenPosition tp = Panel_Buy.AddComponent<TweenPosition>();
			tp.from = Panel_Buy.transform.localPosition;
			tp.to = new Vector3(0, 600, -1);
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
			tp.to = new Vector3(0, 20, -1);
			tp.duration = 0.5f;
			tp.Play(true);		
		} else {
			panel_buy_down();
		}
	}
	
	public void fb(){
			Application.OpenURL("https://www.facebook.com/SayrexGames");	
	}
	
	public void gc(){
			Main.onLeaderBoard();	
	}
}
