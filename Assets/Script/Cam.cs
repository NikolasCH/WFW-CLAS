using UnityEngine;
using System.Collections;

public class Cam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//PlayerPrefs.DeleteAll();
		
		if(Main.cam){
			Destroy(gameObject);
		} else{
			Main.cam=true;
			DontDestroyOnLoad(gameObject.transform);
		}
	}
	
}
