using UnityEngine;
using System.Collections;

public class btn : MonoBehaviour {

	public UILabel lbl;

	
	void Start () {
		if(PlayerPrefs.GetInt(gameObject.name.ToString())>0)gameObject.SetActive(false);

		if(gameObject.name=="Btn1" || gameObject.name=="Btn2" || gameObject.name=="Btn3" || gameObject.name=="Btn4")
			lbl.text = word.btn1.ToString();

		if(gameObject.name=="Btn5" || gameObject.name=="Btn6" || gameObject.name=="Btn7" || gameObject.name=="Btn8")
			lbl.text = word.btn2.ToString();

		if(gameObject.name=="Btn9" || gameObject.name=="Btn10" || gameObject.name=="Btn11" || gameObject.name=="Btn12")
			lbl.text = word.btn3.ToString();

		lbl.fontSize =40;
	}

	void OnPress (bool isDown)
	{
		if (!isDown)
		{

			if(gameObject.name=="Btn1" || gameObject.name=="Btn2" || gameObject.name=="Btn3" || gameObject.name=="Btn4")
				word.btn10(gameObject.name, gameObject);

			if(gameObject.name=="Btn5" || gameObject.name=="Btn6" || gameObject.name=="Btn7" || gameObject.name=="Btn8")
				word.btn20(gameObject.name, gameObject);

			if(gameObject.name=="Btn9" || gameObject.name=="Btn10" || gameObject.name=="Btn11" || gameObject.name=="Btn12")
				word.btn30(gameObject.name, gameObject);
		}
	}
}
