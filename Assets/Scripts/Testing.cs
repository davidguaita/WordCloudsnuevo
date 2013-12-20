using UnityEngine;
using System.Collections;

public class Testing : MonoBehaviour {

		public Main main;
	
		public tk2dUIDropDownMenu menu_idioma;
		public tk2dUIDropDownMenu menu_metodo_juego;
		public tk2dUIToggleButton checkbox_noborrarletraspulsadas;
		
		public tk2dUIScrollbar slider1;
		public tk2dTextMesh slider1_text;
		public tk2dUIScrollbar slider2;
		public tk2dTextMesh slider2_text;	
	
		bool changeparameters = false;
	
	// Use this for initialization
	void Start () {
	
		string lang = PlayerPrefs.HasKey("language") ? PlayerPrefs.GetString("language") : "ES";
		Debug.Log("language:" + lang);
			

		switch(lang){
			
			case "EN":
			CloudWordsValues.language = "EN";
			menu_idioma.Index = 0;
			menu_idioma.SetSelectedItem();
			PlayerPrefs.SetString("language", "EN");
			break;
			
			case "ES":
			CloudWordsValues.language = "ES";
			menu_idioma.Index = 1;
			menu_idioma.SetSelectedItem();
			PlayerPrefs.SetString("language", "ES");
			break;
			
			default:
			
			break;			
			
			
		}
		
		int method = PlayerPrefs.GetInt("methodGetWords");
		menu_metodo_juego.Index = method;
		menu_metodo_juego.SetSelectedItem();
			
			
		int check_noborrarletras =  PlayerPrefs.HasKey("LettersPressednotDelete") ? PlayerPrefs.GetInt("LettersPressednotDelete") : 1;
		if(check_noborrarletras == 0){
			checkbox_noborrarletraspulsadas.IsOn = false;	
			
		} else {
			checkbox_noborrarletraspulsadas.IsOn = true;	
		}
		
		int numberinitialletters = PlayerPrefs.HasKey("numberinitialletters") ? PlayerPrefs.GetInt("numberinitialletters") : CloudWordsValues.numberinitialletters;
		CloudWordsValues.numberinitialletters = numberinitialletters;
		float slider1value = numberinitialletters * 0.05f;
		slider1.Value = slider1value;
		slider1_text.text = "Letras iniciales: " + numberinitialletters;
		
		float timeBetweenLetters = PlayerPrefs.HasKey("timeBetweenLetters") ? PlayerPrefs.GetFloat("timeBetweenLetters") : CloudWordsValues.timeBetweenLetters;
		CloudWordsValues.timeBetweenLetters = timeBetweenLetters;
		float slider2value = timeBetweenLetters / 2;
		slider2.Value = slider2value;		
		slider2_text.text = "Tiempo entre letras: " + timeBetweenLetters; 
		
		
		//checkbox_noborrarletraspulsadas. SetState();
		changeparameters = true;
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void change_language(){
		
		if(changeparameters){
			Debug.Log(menu_idioma.Index);
			if(menu_idioma.Index == 0){
			CloudWordsValues.language = "EN";
			PlayerPrefs.SetString("language", "EN");
			} else {
			CloudWordsValues.language = "ES";
			PlayerPrefs.SetString("language", "ES");	
				
			}
			
			Debug.Log("change language:" + CloudWordsValues.language);
			
			main.loadtxt();
		}
		
	}
	
	public void change_method(){
		
		if(changeparameters){
			switch(menu_metodo_juego.Index){
			case 0:
			CloudWordsValues.methodGetWords = 0;
			
			break;
			case 1:
			CloudWordsValues.methodGetWords = 1;
			break;
			case 2:
			CloudWordsValues.methodGetWords = 2;
			break;			
			}
			PlayerPrefs.SetInt("methodGetWords", CloudWordsValues.methodGetWords);
		}
		
	}
	
	public void change_checkbox(){
		if(changeparameters){
			if(checkbox_noborrarletraspulsadas.IsOn){
				
				CloudWordsValues.LettersPressednotDelete = true;
				 PlayerPrefs.SetInt("LettersPressednotDelete", 1);
			} else {
				CloudWordsValues.LettersPressednotDelete = false;
				PlayerPrefs.SetInt("LettersPressednotDelete", 0);
			}
			
		}
	}
	
	public void change_slider1(){
		int letras = Mathf.RoundToInt( slider1.Value * 20);
		
		slider1_text.text = "Letras iniciales: " + letras; 
		
		 PlayerPrefs.SetInt("numberinitialletters", letras);
		CloudWordsValues.numberinitialletters = letras;
		
	}
	
	public void change_slider2(){
		float tiempo =  Mathf.RoundToInt( slider2.Value * 20) * 0.1f;
		
		slider2_text.text = "Tiempo entre letras: " + tiempo; 
		
		PlayerPrefs.SetFloat("timeBetweenLetters", tiempo);
		CloudWordsValues.timeBetweenLetters = tiempo;
	}
}
