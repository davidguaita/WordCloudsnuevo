using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Awake () {
	
		CloudWordsValues.score_record = EncryptedPlayerPrefs.GetInt("score_record");
		
		
		if(!PlayerPrefs.HasKey("firstinstall")){
			
			
			
			
		}
	}
	
	void Start(){
	loadtxt();
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void loadtxt(){
		
		float timeelapsed = Time.time;
		Debug.Log(" LOAD TIME:" + timeelapsed);
		//cargo los listados de palabras y letras en el idioma
		LoadWords.load_words(CloudWordsValues.language);
		LoadWords.load_letters(CloudWordsValues.language);	
		
		float endtimeelapsed = Time.time - timeelapsed;
		Debug.Log(" ELAPSED TIME:" + endtimeelapsed);
		
	}
	
	
}
