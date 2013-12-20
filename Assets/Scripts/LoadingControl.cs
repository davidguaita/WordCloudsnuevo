using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadingControl : MonoBehaviour {
	
	

	public tk2dUIProgressBar progressbar;
	public TextAsset wordlist;
	
	
	// Use this for initialization
	void Start () {
		
		progressbar.Value = 0.0f;
		Invoke("loadtxt", 0.3f);
		
		Debug.Log("STaRT");
	}
	
	void loadtxt(){
		
		progressbar.Value = 0.1f;

		
		//cargo los listados de palabras y letras en el idioma
		
		LoadWords.load_words("EN");
		
		LoadWords.load_letters("EN");	
		progressbar.Value = 1.0f;
		
		Invoke ("loadgame",0.2f);
		
	}
	
	// Update is called once per frame
	void Update () {
	
		
		
	}
	
	
	void loadgame(){
	Application.LoadLevel("game");	
		
	}
	
	
}
