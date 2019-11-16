using UnityEngine;
using System.Collections;

public class ReSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(Screen.width==1024 || Screen.width==2048)gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1);
	}
	
}
