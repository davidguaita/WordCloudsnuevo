using UnityEngine;
using System.Collections;

public class ButtonLetterControl : MonoBehaviour {
	
	public GameControl gamecontrol;
	public tk2dUIToggleButton togglebutton;
	public tk2dTextMesh letterFont;
	public bool ispressed = false;
	public Vector3 speed = new Vector3 (0,0,0);
	Transform tr;
	public string letter_assigned;
	public int state;
	public bool isBtnConfirm = false;
	
	public bool isComodin = false;
	
	
	void Awake(){
	tr = transform;
	
	
	}
	
	// Use this for initialization
	void Start () {
	
	state = 0;	
		
		iTween.ScaleFrom(gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f, "easetype", "easeOutBack"));	
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
		tr.position += speed;
		
		//if(!isBtnConfirm)
		//check_state();
		
	}
	
	public void DieLetter(){
		state = 2;
		iTween.ScaleTo(gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 1.0f, "easetype", "easeInBack"));	
		Invoke("deleteobj", 1.0f);
	}
	
	public void DieLetter(float timetodestroy){
		state = 2;
		iTween.ScaleTo(gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "time", timetodestroy, "easetype", "easeInBack"));	
		Invoke("deleteobj", timetodestroy);
	}
	
	void deleteobj(){
	Destroy(gameObject);	
	}
	
	public void changeletter(string newletter){
	
		if(letterFont){
			letterFont.text = newletter;	
			letterFont.Commit();
		}
		
	}
	
	void press_button(){
		
		switch(state){
		case 0:
		gamecontrol.select_new_letter(letter_assigned, gameObject, isComodin);
		state = 1;
		break;
		case 1:
		gamecontrol.delete_new_letter(letter_assigned, gameObject, isComodin);
		state = 0;
		break;
		}
		
		check_state();
		
	}
	
	public void check_state(){
	
		bool mystate = false;
		switch(state){
		case 0:
		case 2:
			mystate = false;
		break;
		case 1:
			mystate = true;
		break;
		}
		
		//if(mystate != togglebutton.IsOn){
			togglebutton.IsOn = mystate;	
		//}
		
	}
	
	public void change_state(int newstate){
	
		state = newstate;
		
		check_state();
		
		/*
		switch(newstate){
		case 0:
		case 2:
			togglebutton.IsOn = false;
		break;
		case 1:
			togglebutton.IsOn = true;
		break;
		}
		*/
		
		
	}
	
	
	
}
