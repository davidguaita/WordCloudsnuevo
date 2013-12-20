using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class GameControl : MonoBehaviour {
	
	public int statusGame = 0; //0 start ,  1 play ,  2 pause, 3 gameover
	public int score = 0;
	
	public GameObject letterTemplate;
	
	public List<GameObject> actuallettersList;
	
	public Transform PointLimitUpLeft;
	public Transform PointLimitDownRight;
	
	public float time_start_create_letters = 1.0f;
	bool initial_letters_created = false;
	
	
	//attraction & repulsion	
	private float repulsionfactor = 200f;
	private float GConstant = 0.1f;
	private float drag= 0.95f;
	private float factor=01;
	private const float objectmasa = 20f;
	
	//
	public List<GameObject> lettersPressedObj;
	public List<string> lettersPressed;
	public tk2dTextMesh wordCreatedTextMesh;
	public string wordCreated;
	
	//comodines
	public List<int> ComodinPositions;
	
	int ComodinesCounter = 0;
	int maxComodinesCounter = 1;
	float ProbabilityShowComodin = 0.1f;
	string signoComodin = "?";
	
	//se usa para el caso de que el metodo sea coger letras de una palabra
	public int LetterFromWord_numberwords = 3;
	public List<string> LetterFromWord_letters = new List<string>();
	
	// letters life
	public float startDieButtons = 15.0f;
	public float loopDieButtons = 4.0f;
	
	//
	public GameObject btnConfirm;
	
	
	
	//ui
	public tk2dTextMesh txtScore;
	public tk2dTextMesh txtTime;
	
	public float timeplay;
	
	
	// Use this for initialization
	void Start () {


		
		statusGame = 1;
		
		//loadtxt();
		
		//Invoke("loadtxt", 0.1f);
		
		for(int i = 0; i< CloudWordsValues.numberinitialletters; i++){
		
			createLetter();
//			Debug.Log("create initial letter");
		}
		
		initial_letters_created = true;
		
		Invoke("createLetter", time_start_create_letters);
		Invoke("DieLetterLoop", startDieButtons);	
		
		
		btnConfirm.SetActive(false);
		
		timeplay = 60;
		
		updateTimePlay();
		updateScore();
		
		
		InvokeRepeating("Counterdown",1,1);
		
		ComodinPositions = new List<int>();
		
		
	}
	
	void Counterdown(){
		
		if(timeplay > 0){
		timeplay --;
			updateTimePlay();
		} else {
			CancelInvoke("Counterdown");
			endgame();	
		}
		
	}
	
	void endgame(){
	statusGame = 3;	
		for(int i = 0; i<actuallettersList.Count; i++){
					
		ButtonLetterControl btl = actuallettersList[i].GetComponent<ButtonLetterControl>();
		btl.DieLetter();
		
		}
		

	}
	
	void updateScore(){
		txtScore.text = score + "";
		txtScore.Commit();
	
	}
	
	void updateTimePlay(){
		txtTime.text = timeplay + "s";
		txtTime.Commit();		
		
	}
	
	void loadtxt(){
		
		float timeelapsed = Time.time;
		Debug.Log(" LOAD TIME:" + timeelapsed);
		//cargo los listados de palabras y letras en el idioma
		LoadWords.load_words(CloudWordsValues.language);
		LoadWords.load_letters(CloudWordsValues.language);	
		
		float endtimeelapsed = Time.time - timeelapsed;
		Debug.Log(" ELAPSED TIME:" + endtimeelapsed);
		
	}
	
	// Update is called once per frame
	void Update () {
	
		
		if(statusGame == 1){
		//update position and velocity all letters in the cloud
		if(actuallettersList.Count > 0){
		
			foreach (GameObject Target in actuallettersList)
			{ 	factor = Target.transform.localScale.x;
				float targetmasa = objectmasa*(factor/1.5f);
			  	Vector3 actualspeed = Target.GetComponent<ButtonLetterControl>().speed;
				Vector3 newspeed = Calculatevelocity (Target,targetmasa);
				Vector3 repulsionspeed = Calculaterepulsion (Target,targetmasa);
				
			    Vector3 resultvelocity = actualspeed*drag + newspeed - repulsionspeed;
				if (resultvelocity.magnitude > 02f) 
				{
					resultvelocity.Normalize();
					resultvelocity= resultvelocity * 2f;
				}
					
				Target.GetComponent<ButtonLetterControl>().speed = resultvelocity;
			}
		}
		}
		
	}
	
	
	void createLetter(){
		
		
		if(statusGame == 1){
			Vector3 RandomPos = new Vector3 (Random.Range(PointLimitUpLeft.position.x, PointLimitDownRight.position.x), Random.Range(PointLimitUpLeft.position.y, PointLimitDownRight.position.y), 0);
			GameObject newletter = Instantiate(letterTemplate, RandomPos, Quaternion.identity) as GameObject;
			newletter.SetActive(true);
			ButtonLetterControl newletterControl = newletter.GetComponent<ButtonLetterControl>();
			
			string newletterstring = "";
			
			
			
			if(ProbabilityShowComodin > Random.Range(0.0f, 1.0f) && ComodinesCounter < maxComodinesCounter){
			
				newletterstring = signoComodin;
				ComodinesCounter++;
				newletterControl.isComodin = true;
				
			} else {
			
				switch(CloudWordsValues.methodGetWords){
				case 0:
					newletterstring = randomLetter();
				break;
				case 1:
					newletterstring = LettersFromWord(99);	
				break;
				case 2:
					newletterstring = LettersFromWord(5);	
				break;				
				}
			}
			
			newletterControl.changeletter(newletterstring);
			newletterControl.letter_assigned = newletterstring;
			
			actuallettersList.Add(newletter);
			
			if(initial_letters_created)
			Invoke("createLetter", CloudWordsValues.timeBetweenLetters);
		
		}
	}
	
	
	string randomLetter(){
	
		float targetweight = Random.Range(0.0f, 100.0f);
		float actualweight = 0;
		int counter = 0;
		string finalstring = "A";
		
		for(int i = 0; i< LoadWords.letters_key.Length ; i++){
				actualweight += LoadWords.letters_porcentage[i];
			
				if( actualweight >= targetweight){
					finalstring = LoadWords.letters_key[i];
//					Debug.Log("Letra:" + finalstring + " %: " + actualweight + " >= " + targetweight);
					break;
				}
				
		}
		
		return finalstring;
	}
	
	string LettersFromWord(int maxchars){
	
		if(LetterFromWord_letters.Count < 1){
			
			for(int i=0; i <= LetterFromWord_numberwords; i++){
				
				string mynewword = "";
				while(mynewword.Length == 0 || mynewword.Length > maxchars){
				mynewword = LoadWords.words[Random.Range(0, LoadWords.words.Count)];	
				}
//				Debug.Log("palabra añadida: " + mynewword );
				foreach(char letra in mynewword){
					LetterFromWord_letters.Add(letra.ToString());
				}
			//LetterFromWord_letters += mynewword;
			}
		}		
		
			int CharPos = Random.Range(0, LetterFromWord_letters.Count);
			string CharSelected = LetterFromWord_letters[CharPos].ToString();
			//LetterFromWord_letters = LetterFromWord_letters.Substring(0, (CharPos + 1)) + LetterFromWord_letters.Substring((CharPos + 1), LetterFromWord_letters.Length);
			LetterFromWord_letters.RemoveAt(CharPos);// .Remove(0,1);// Remove(CharPos, 1);
		
			
			return CharSelected;	

	}
	
	
	// destruye la primera letra de la lista cada x tiempo
	void DieLetterLoop(){
		
		if(statusGame == 1 && actuallettersList.Count > 0){
			//ButtonLetterControl btl = new ButtonLetterControl();
			//= actuallettersList[0].GetComponent<ButtonLetterControl>();
			
			if(CloudWordsValues.LettersPressednotDelete){
				bool isfound = false;
				
				for(int i = 0; i< actuallettersList.Count; i++){
					if(!isfound){
						ButtonLetterControl btl = actuallettersList[i].GetComponent<ButtonLetterControl>();
						if(btl.state == 0){
							DieLetter( actuallettersList[i]);
							isfound = true;
						}
					}
				}		
			} else {
						ButtonLetterControl btl2 = actuallettersList[0].GetComponent<ButtonLetterControl>();
						btl2.DieLetter();
						actuallettersList.RemoveAt(0);
						delete_new_letter(btl2.letter_assigned, actuallettersList[0], btl2.isComodin);
				
			}
				
		}
				

	Invoke("DieLetterLoop", loopDieButtons);	
	}
	
	//destruye la letra de un boton determinado
	void DieLetter(GameObject btn){
		
		ButtonLetterControl btl = btn.GetComponent<ButtonLetterControl>();
		btl.DieLetter();
		actuallettersList.Remove(btn);
		
		if(!CloudWordsValues.LettersPressednotDelete && btl.state == 1)
		{
			delete_new_letter(btl.letter_assigned, btn, btl.isComodin);
			create_word();
		}
		
		
		
	}
	
	//añade la letra pulsada. Evento que viene del boton
	public void select_new_letter(string letterPress, GameObject btn, bool iscomodin){
		if(statusGame == 1){
			btnConfirm.SetActive(false);
			if(actuallettersList.Contains(btnConfirm))
				actuallettersList.Remove(btnConfirm);
			
			if(iscomodin)
			ComodinPositions.Add(lettersPressed.Count);
			
			lettersPressed.Add(letterPress);
			lettersPressedObj.Add(btn);
			create_word();
		}
	}
	
	//elimina la letra pulsada. Evento que viene del boton
	public void delete_new_letter(string letterPress, GameObject btn, bool iscomodin){
		if(statusGame == 1){
			
			btnConfirm.SetActive(false);
			if(actuallettersList.Contains(btnConfirm))
				actuallettersList.Remove(btnConfirm);
			
			if(iscomodin){
			ComodinPositions.Remove(lettersPressed.IndexOf(letterPress));
			ComodinesCounter = Mathf.Max(0, (ComodinesCounter - 1));
			}
				
			lettersPressed.Remove(letterPress);
			lettersPressedObj.Remove(btn);
			create_word();
		}
	}
	
	//funcion para el boton que limpia las letras pulsadas
	public void button_delete_letters_pressed(){
		
		if(btnConfirm)
		btnConfirm.SetActive(false);
		
		delete_letters_pressed(false);
		
		if(ComodinPositions.Count > 0)
		ComodinPositions.Clear();	
			
		
	}
	
	//borra todas las letras pulsadas, pero no los botones
	void delete_letters_pressed(bool deleteobj){
	
//		Debug.Log("delete_letters_pressed");
		lettersPressed.Clear();
		wordCreated = "";
		wordCreatedTextMesh.text = wordCreated;
		wordCreatedTextMesh.Commit();
		
		for(int i = 0; i < lettersPressedObj.Count ; i++){
			if(lettersPressedObj[i] != null){
				ButtonLetterControl lp = lettersPressedObj[i].GetComponent<ButtonLetterControl>();
				
				if(lp != null){
					if(!deleteobj){
					lp.change_state(0);
					} else {
					actuallettersList.Remove(lettersPressedObj[i]);
						lp.DieLetter(0.5f);	
						
					}
					
				}
			}
			//DieLetter(lettersPressedObj[i]);
		}
		lettersPressedObj.Clear();
	
		
	}
	
	// forma la palabra y la chequea 
	void create_word(){
			wordCreated = "";
	
		for(int i = 0; i < lettersPressed.Count; i++){
		
			wordCreated += lettersPressed[i];
			
		}
		
		wordCreatedTextMesh.text = wordCreated;
		wordCreatedTextMesh.Commit();
		
		//chequea si existe la palabra en el diccionario
		check_word();
		
	}
	
	bool containsWord(string myword){
	
		int count = myword.Length;
		switch(count){
		case 3:
		return LoadWords.words_3_letters.Contains(myword);	
		break;
		case 4:
		return LoadWords.words_4_letters.Contains(myword);	
		break;			
		case 5:
		return LoadWords.words_5_letters.Contains(myword);	
		break;			
		case 6:
		return LoadWords.words_6_letters.Contains(myword);	
		break;			
		case 7:
		return LoadWords.words_7_letters.Contains(myword);	
		break;			
		default:
		return LoadWords.words_8_letters.Contains(myword);	
		break;			
			
		}
		
		
	}
		
	// chequea que exista la palabra en el diccionario
	void check_word(){
	
			
		if(ComodinPositions.Count > 0){
			
			if(ComodinPositions.Count == 1){
				
				foreach(string letra in LoadWords.letters_key){
				string wordfinal = wordCreated;
				wordfinal = wordfinal.Replace(signoComodin, letra);
					if(containsWord(wordfinal)){
//					Debug.Log("comodin usado para: " + wordfinal);
					check_ok_word();
					break;
					}
				}
				//
			
			
			}
				
			
			
			
		} else if(containsWord(wordCreated)){
			
			check_ok_word();
			
		}
		
	}
	
	void check_ok_word(){
		
		
		
			btnConfirm.transform.position = lettersPressedObj[lettersPressedObj.Count - 1].transform.position + new Vector3(15,15,0);
			btnConfirm.SetActive(true);
			
			//añado el boton list para que afecte a la velocidad y repulsion
			actuallettersList.Add(btnConfirm);
			
			//Debug.Log("Palabra conseguida:" + wordfinal);
			//Invoke("delete_letters_pressed", 0.5f);
			
		
		
	}
	
	void confirm_word(){
		if(statusGame == 1){
			
			score += lettersPressed.Count * Mathf.Max(1, (lettersPressed.Count  - 2)) * 10;
			updateScore();
			delete_letters_pressed(true);
			btnConfirm.SetActive(false);
			
			if(ComodinPositions.Count > 0){
			ComodinPositions.Clear();	
			}
			
			//añado el boton list para que afecte a la velocidad y repulsion
			if(actuallettersList.Contains(btnConfirm))
			actuallettersList.Remove(btnConfirm);
			
		}
	}
	
	public void mainmenu(){
		CloudWordsValues.score =  score;
		CloudWordsValues.score_record = Mathf.Max(score, CloudWordsValues.score_record);
		EncryptedPlayerPrefs.SetInt("score_record", CloudWordsValues.score_record);
		
		Invoke("mainmenu2",0.5f);
			
	}
	public void mainmenu2(){
		
		Application.LoadLevel("main");	
	}	
	
	 Vector3 Calculatevelocity (GameObject Target, float targetmasa) {
		Vector3 outspeed = new Vector3 (0f,0f,0f);
		foreach (GameObject Source in actuallettersList )
		{
		
			
			if (Source != Target )
			{   //Gravitational force direction
			factor = Source.transform.localScale.x;
			float sourcemasa = objectmasa*(factor/1.5f);
			Vector3 Fg_dir = Source.transform.position - Target.transform.position;
            float distance = Fg_dir.magnitude;
 
            //Computing scalar value of gravitational force
            float GMmR2 = (GConstant * sourcemasa * targetmasa) / (distance * distance );
 
            Fg_dir.Normalize();
 
            //Aplying force by using Constant force component
            outspeed = outspeed +  new Vector3(GMmR2 * Fg_dir.x, GMmR2 * Fg_dir.y, GMmR2 * Fg_dir.z);
			}
		}
		return (outspeed);
	}	
	
	 Vector3 Calculaterepulsion (GameObject Target, float targetmasa) {
		Vector3 outspeed = new Vector3 (0f,0f,0f);
		foreach (GameObject Source in actuallettersList )
		{
			
			
			if (Source != Target )
			{   //Gravitational force direction
			factor = Source.transform.localScale.x;
			float sourcemasa = objectmasa*(factor/1.5f);
            Vector3 Fg_dir =  Source.transform.position - Target.transform.position;
            float distance = Fg_dir.magnitude;
 
            //Computing scalar value of gravitational force
            float GMmR2 = (GConstant*repulsionfactor * sourcemasa * targetmasa) / (distance *  distance *distance);
 
            Fg_dir.Normalize();
 
            //Aplying force by using Constant force component
            outspeed = outspeed +  new Vector3(GMmR2 * Fg_dir.x, GMmR2 * Fg_dir.y, GMmR2 * Fg_dir.z);
			}
		}
		return (outspeed);
	}	
	
	
	
}
