using UnityEngine;
using System.Collections;

public class Map : Main {

	public GameObject _obj;

	public UISprite p0;
	public UISprite p1;
	public UISprite p2;
	public UISprite p3;
	public UISprite p4;
	public UISprite p5;
	public UISprite p6;
	public UISprite p7;
	public GameObject MSG_1;
	public GameObject MSG_2;
	public GameObject MSG_3;
	public UILabel Rate;


	// Update is called once per frame
	void Start () {

		MSG_1.SetActive(false);
		MSG_2.SetActive(false);
		MSG_3.SetActive(false);

		iMap = this;
		SX.GetComponent<SX_Ads>().smartBanneShow();

		ScoreUp();

		CoinUp();

		if(Screen.width==1024 || Screen.width==2048)gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1);
		if(Screen.width==960)gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 1);

		int page = PlayerPrefs.GetInt("page");

		Rate.text = "[FFF900]"+ "X"+(page+1)+"[-]";


		if(page==0)p0.spriteName = "tochka2";
		if(page==1)p1.spriteName = "tochka2";
		if(page==2)p2.spriteName = "tochka2";
		if(page==3)p3.spriteName = "tochka2";
		if(page==4)p4.spriteName = "tochka2";
		if(page==5)p5.spriteName = "tochka2";
		if(page==6)p6.spriteName = "tochka2";
		if(page==7)p7.spriteName = "tochka2";



		int l = 0+(25*page);
		int o = 1;
		bool msg = true;
		bool finish = true;
		for (int i = 0; i<5; i++){
			for (int j = 0; j<5; j++){
				l++;

				Level _lvl = GameObject.Find("l_"+o.ToString()).GetComponent<Level>();
				_lvl.label.text = l.ToString();
				_lvl._Level = l;
				
				_lvl.star1.fillAmount = 0;
				float w = 0;
				float c = 0;
				if(PlayerPrefs.HasKey("w_"+l.ToString()))
					w =PlayerPrefs.GetFloat("w_"+l.ToString());
				if(PlayerPrefs.HasKey("c_"+l.ToString()))
					c =PlayerPrefs.GetFloat("c_"+l.ToString());

				if(c>0 && w>0)
				{
					_lvl.star1.fillAmount=c/w;
					if(c/w>0.33)
						msg =false;
				}
				if(c/w<1 || c<1 || w<1)	
					finish =false;

				o ++;
			}
		}

		if(stars>=iPage.unlock[page] )
		{
			if(PlayerPrefs.GetInt("page")<1)
				iMap.MSG_1.SetActive(msg);
			else if (PlayerPrefs.GetInt("page")<7)
				iMap.MSG_2.SetActive(msg);
			else 
				iMap.MSG_3.SetActive(msg);

			if(finish)
				Rate.text = "[EB2F00]"+ "X"+(page+1)+"[-]";
		}

		
	}
}
