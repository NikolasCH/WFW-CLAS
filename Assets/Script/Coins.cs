using UnityEngine;
using System.Collections;

public class Coins : MonoBehaviour {

	public UILabel coins;
	// Use this for initialization

	// Update is called once per frame
	public void Add () {
		coins.alpha = 1;
		TweenAlpha a = gameObject.AddComponent<TweenAlpha>();
		a.from = 1;
		a.to = 0;
		a.delay = 1;
		a.duration = 1;
		a.Play();
	}
}
