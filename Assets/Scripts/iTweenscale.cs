using UnityEngine;
using System.Collections;

public class iTweenscale : MonoBehaviour {
	
 	public bool scale_in = false; 
	public float mytime = 1.0f;
	public iTween.EaseType easetype_in;
	public iTween.EaseType easetype_out;
	// Use this for initialization
	void Start () {

		if(scale_in){
		iTween.ScaleTo(gameObject, iTween.Hash("x", 0.1f, "y",  0.1f, "z",  0.1f, "time", mytime, "easetype", easetype_in));
		} else {
		iTween.ScaleFrom(gameObject, iTween.Hash("x", 0.0f, "y",  0.0f, "z",  0.0f, "time",mytime, "easetype", easetype_out));	
		}
		Invoke("hide", mytime + 0.25f);
	}
	
	void hide(){
		
	gameObject.SetActive(false);	
	}
	

	
}
