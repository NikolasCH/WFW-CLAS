using UnityEngine;
using System.Collections;

public class LoadingScene : MonoBehaviour {



	private AsyncOperation async;
//	private UILabel label;
	private bool nextLoading = false;
	private int scene = 0;


	
	void next () 
	{
		scene = 1;
		nextLoading = true;
		StartCoroutine("_Start");
	}
	
	void menu () 
	{
		scene = 0;
		
		nextLoading = true;
		StartCoroutine("_Start");
	}

	IEnumerator _Start() {
		async = Application.LoadLevelAsync(scene);	
		yield return async;		
	}
	
}

