using UnityEngine;
using System.Collections;

public class bg : MonoBehaviour {
	void Start () {
	 Rect newInset = new Rect(0,0,Screen.width,Screen.height);
		gameObject.GetComponent<GUITexture>().pixelInset = newInset;
	}

}
