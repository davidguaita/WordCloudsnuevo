using UnityEngine;
using System.Collections;

public class movetotarget : MonoBehaviour {
	
	public GameObject target;
	public float mytime = 0.75f;
	
	// Use this for initialization
	void Start () {

		iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(target.transform.position.x, target.transform.position.y, -5), "time", mytime, "easetype", "linear"));
		
		iTween.ScaleTo(gameObject, iTween.Hash("x", 0.1f, "y",  0.1f, "z",  0.1f, "time", 0.5f, "delay", mytime, "easetype", "linear"));
		Invoke("hide", mytime + 0.25f);
	}
	
	void hide(){
		
	gameObject.SetActive(false);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	 void OnBecameVisible() {
		
	
		
	}
	
}
