using UnityEngine;
using System.Collections;

public class MusicAI : MonoBehaviour {
	
	public AudioClip win;
	public AudioClip wrong;
	public AudioClip New;
	public AudioClip coin;
	public AudioClip _coin;
	public static bool mute = true;
	// Use this for initialization
	
	public void onWin()
	{
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().PlayOneShot(win);	
	}
	public void onWrong()
	{
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().PlayOneShot(wrong);	
	}
	public void onNew()
	{
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().PlayOneShot(New);	
	}
	public void onCoin()
	{
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().PlayOneShot(coin);	
	}

	public void Coin()
	{
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().PlayOneShot(_coin);	
	}

}
