using UnityEngine;
using System.Collections;

public class moveSplashheart : MonoBehaviour {
	
//	public GameObject target;
	public float mytime = 0.75f;
	public Vector3 dest;
	private float pos= -5;
	
	
	// Use this for initialization
	void Start ( ) {
		//mytime = 4f;
	
		
	}
	
	void hide(){
		
	Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	 void OnBecameVisible() {
		
	
		
	}
	public void setDest (Vector3 destino) {
//		print ("destino"+ destino);
//		print ("actual" + transform.position);
		iTween.MoveTo(gameObject, iTween.Hash("position", destino, "time", 0.5f, "easetype", "linear"));
		iTween.ScaleTo(gameObject, iTween.Hash("x", 0f, "y",  0f, "z",  0f, "time", 0.2f, "delay", 0.2f, "easetype", "linear"));
		iTween.ColorTo(gameObject, iTween.Hash("a", 0f, "time", 0.2f, "delay", 0.2f, "easetype", "linear"));
		Invoke("hide", 0.5f);
	}
	
	void SetTransformZ(float n){

    transform.position = new Vector3(transform.position.x, transform.position.y, n);

}


	
}
