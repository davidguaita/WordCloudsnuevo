using UnityEngine;
using System.Collections;

public class movetotargetsplash : MonoBehaviour {
	
	public GameObject target;
	public GameObject splashheart;
	public float mytime = 0.75f;
	private Vector3 destino;
	private float pos= -5;
	private float x=0;
	private float y=0;
	private float z=0;

	
	
	void Awake () {
	
	}
	// Use this for initialization
	void Start () {
		Invoke("hide", mytime*1.5f );
		//mytime = 4f;
	SetTransformZ(-14);
		iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(target.transform.position.x, target.transform.position.y, -14), "time", mytime, "easetype", "linear"));
	iTween.ScaleTo(gameObject, iTween.Hash("x", 3f, "y",  3f, "z",  3f, "time", mytime/2, "easetype", "easeOutQuad"));
		iTween.ScaleTo(gameObject, iTween.Hash("x", 1f, "y",  1f, "z",  1f, "delay", mytime/2,"time", mytime/2, "easetype", "easeInQuad"));
		iTween.ScaleTo(gameObject, iTween.Hash("x", 4f, "y",  4f, "z",  4f, "time", 0.5f, "delay", mytime, "easetype", "linear"));
		iTween.ColorTo(gameObject, iTween.Hash("a", 0f, "time", 0.5f, "delay", 0.5f, "easetype", "linear"));
		Invoke("Splashhearts", mytime );
	}
	
	void hide(){
		
	gameObject.SetActive(false);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	 void OnBecameVisible() {
		
	
		
	}
	
	void Splashhearts() {
		
	for ( int angulo = 0; angulo < 359; angulo=angulo+30)
		{
		GameObject newheart = GameObject.Instantiate(splashheart, new Vector3(target.transform.position.x, target.transform.position.y, -5), Quaternion.identity) as GameObject ;
		x= target.transform.position.x+(30f*Mathf.Sin (angulo));
		y= target.transform.position.y+(30f*Mathf.Cos (angulo));
		z=-5;
		destino = new Vector3 (x, y, z);
//		print ("vector" + destino);
		newheart.SendMessage ("setDest", destino);
		newheart.SetActive(true);
		}
		
		
		
	}
	void SetTransformZ(float n){

    transform.position = new Vector3(transform.position.x, transform.position.y, n);

}


	
}
