using UnityEngine;
using System.Collections;

public class UIPosition : MonoBehaviour {
	
	public float OffSetX = 0f;
	public float OffSetY = 0f;
	
	void Start () {
		Vector3 Point =Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*(OffSetX/100), Screen.height*(OffSetY/100),0));
		Point.z = gameObject.transform.position.z;
		gameObject.transform.position = Point;
	}
}
