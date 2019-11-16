using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	public int _Level =0;
	public UILabel label;
	public UISprite star1;
	
	private AsyncOperation async; 


	
	void OnPress (bool isDown)
	{
		if (isDown == false)
		{
			PlayerPrefs.SetInt("lvl",_Level);
			StartCoroutine("_Start");
		}
	}

	IEnumerator _Start() {
		async = Application.LoadLevelAsync(2);	
		yield return async;		
	}

}
