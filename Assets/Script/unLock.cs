using UnityEngine;
using System.Collections;

public class unLock : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(Main.b1 && gameObject.name == "b1" || Main.b2 && gameObject.name == "b2" || Main.b3 && gameObject.name == "b3"){
			UISprite bg = (UISprite)gameObject.GetComponent("UISprite");
			bg.spriteName = "lock copy";
			bg.transform.localScale = new Vector3(89f, 115f, 1f);
			Debug.Log("Open");
		}
	
	}
	
}
