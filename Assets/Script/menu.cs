using UnityEngine;
using System.Collections;

public class menu : Main {

	// Use this for initialization
	void Start () {
//PlayerPrefs.DeleteAll();
		if(PlayerPrefs.GetInt("new_v2")!=1)
		{
			PlayerPrefs.DeleteAll();
			Main.install();
		}

		if(SX)
			SX.GetComponent<SX_Ads>().smartBanneHide();

		stars =PlayerPrefs.GetInt("Star");
		SX = GameObject.Find ("SX");
	}


}
