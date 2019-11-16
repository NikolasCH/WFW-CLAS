using UnityEngine;
using System.Collections;

public class letters : MonoBehaviour {

	//GameObject word;
	public int num = 0;
	public GameObject label;
	private bool start =true;
	Vector3 StartPos;
	public string letter;
	private bool move =true;
	private int num_pos = -1;
	public UILabel lbl;	
	GameObject pos;
	private bool load = true;
	
	public void Start(){

		gameObject.tag = "menu";
		if(PlayerPrefs.GetInt("l"+(num+1).ToString())==1)Destroy(gameObject);
	}



	void OnPress (bool isDown)
	{
		if (isDown == false)
			addLetters();	
	}
	
	void FixedUpdate () {
		//if(Main.p[num]>=0 && !Main.play){
		//	Vector3 mPos = pos.transform.position;
		//	mPos.z=-1f;
		//	transform.position = mPos;
		//}
		if(Main.play && load){
			load = false;	
			Play();	
		}
	}
	
	void Play(){
		gameObject.tag = "menu";
	}
	
	
	public void addLetters(){
		if(Main.play){
			if(move){
				if(Main.game){
					move =false;			
					gameObject.tag = "letter";

					if(start){
						StartPos = transform.position;
						start = false;
					}

					pos = GameObject.Find("l"+Main.next.ToString());
					
					Main.p[num]= Main.next;
					num_pos = Main.next;
					Main.w[Main.next]=letter;
					Vector3 mPos = pos.transform.position;
					mPos.z=-1f;
					StartCoroutine( coTween( gameObject, 0.5f, mPos) );
				}
			}else{
				move = true;
				gameObject.tag = "menu";
				Main.w[num_pos]="";
				num_pos =0;
				Main.p[num]=-1;
				StartPos.z=-1;
				StartCoroutine( coTween( gameObject, 0.5f, StartPos) );	
			}
			Main.check_next(move);
		}
	}
	
	IEnumerator coTween( GameObject Obj, float time, Vector3 toPos) 
    {
		Transform transform = Obj.transform;
        Vector3 fromPos = transform.position;
        for (float t = 0; t < time; t += Time.deltaTime) {
            float nt = Mathf.Clamp01( t / time );
            nt = Mathf.Sin(nt * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp( fromPos, toPos, nt );
            yield return 0;
        }
		transform.position = toPos;
    }		
}
