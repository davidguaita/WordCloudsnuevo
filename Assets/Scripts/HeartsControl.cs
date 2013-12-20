using UnityEngine;
using System.Collections;
using System;
using Prime31;

public class HeartsControl : MonoBehaviour {
	
	
	
	public Main main;
	public FBScript fbscript;
//	public flurry_script flurryscript;
	
	public int numberofhearts;
	public long diference_inseconds;
	//public float heart_every_minutes = 10;
	
	long target_time;
	long last_timestamp;
	long last_timestamp_internet;
	long timestamp;
	int minutes_passed;
	bool istimefrominternet = false;
	int temp_seconds = 0;
	
	
	public GameObject[] gui_hearts;
	public GameObject heart_move;
	public GameObject heart_scale_in;
	public GameObject heart_scale_out;
	public int actual_hearts;
	public tk2dTextMesh extra_hearts;
	public tk2dTextMesh counterdown_mesh;
	
	public GameObject prefab_gui_loading;
	
	public GameObject getmorehearts;
	
	bool presstoplay = false;
	
	public GameObject prefab_music;
	private GameObject cloned_music;
	
	void Start(){
		
		
		//testing
		//PlayerPrefs.SetInt("actual_hearts", 3);
		
		
		Time.timeScale = 1.0f;
		
		/*
		//flurry get object	
		GameObject myflurryobject = GameObject.Find("Flurry");
		if(myflurryobject)
		flurryscript = myflurryobject.GetComponent<flurry_script>();
		*/
		
		if(PlayerPrefs.HasKey("last_timestamp")){
		last_timestamp = PlayerPrefs.GetInt("last_timestamp");	
		} else { 
		last_timestamp = convert_seconds(DateTime.Now);		
		PlayerPrefs.SetInt("last_timestamp", (int)last_timestamp);		
		}
		
		actual_hearts = EncryptedPlayerPrefs.GetInt("actual_hearts");
		update_hearts(0);
		
		if(actual_hearts <= 1){
		//getmorehearts.SetActive(true);	
		}
		
		if(Application.internetReachability != NetworkReachability.NotReachable){
			
		
			
		} 
		

		
		CancelInvoke("counterdown");
		InvokeRepeating("counterdown",0, 1);
		
		//Invoke ("start_music", 0.5f);
		
	}
	
	void start_music(){
	
		cloned_music = GameObject.Find("MusicMenu");
		
		if(!cloned_music){
		
			cloned_music = Instantiate(prefab_music) as GameObject;
			cloned_music.name = "MusicMenu";
			
		}
		
	}
	
	

	
	void addseconds(){
		temp_seconds ++;
		
	}
	
	void counterdown(){
	
		
			
		if(istimefrominternet){
			diference_inseconds = timestamp + temp_seconds - last_timestamp_internet;
			
		} else {
			long totalseconds =  convert_seconds(DateTime.Now);
			diference_inseconds = totalseconds - last_timestamp;
				
		}
		
		diference_inseconds = (long)(CloudWordsValues.heart_every_minutes * 60) - diference_inseconds;
		
		if(diference_inseconds <= 0){
			
			float number = Mathf.Abs(diference_inseconds) / (CloudWordsValues.heart_every_minutes * 60);
			long excess = (long)((number - Mathf.FloorToInt( number )) * (CloudWordsValues.heart_every_minutes * 60));
			numberofhearts = 1 + (int)Mathf.FloorToInt( number );
			
			if(actual_hearts < 5){
			//case more 5 hearts
			if((actual_hearts + numberofhearts) > 5){
			numberofhearts = 5 - actual_hearts;	
			} 
			} else {
			numberofhearts = 0;
				
			}
			
			update_hearts(numberofhearts);
			
			Debug.Log("new hearts: " + numberofhearts + "  " + excess);	
			
			if(!istimefrominternet){
			last_timestamp = convert_seconds(DateTime.Now) - excess;	
			PlayerPrefs.SetInt("last_timestamp", (int)last_timestamp);	
			}
			
		}
		
		int minutes = Mathf.FloorToInt( diference_inseconds * 0.01666667f);
		int seconds = Mathf.FloorToInt(diference_inseconds - minutes * 60);
		string string_seconds = seconds > 9 ? ("" + seconds) : ("0" + seconds);
		minutes = minutes > 0 ? minutes : 0;
		counterdown_mesh.text =  minutes + ":" + string_seconds;
		counterdown_mesh.Commit();
		
		
//		Debug.Log("time: " + diference_inseconds );
	}
	
	
	public void update_hearts(int hearts_added){
		
		
		actual_hearts += hearts_added;
		
		//if(actual_hearts >= 1 && getmorehearts.activeSelf)
		//	getmorehearts.SetActive(false);
		
		
		if(hearts_added > 0)
		animate_add_heart();
		
		EncryptedPlayerPrefs.SetInt("actual_hearts", actual_hearts);
		
		if(actual_hearts >= 1){
			gui_hearts[0].SetActive(true);
		} else {
			gui_hearts[0].SetActive(false);
		}
		
		if(actual_hearts >= 2){
			gui_hearts[1].SetActive(true);
		} else {
			gui_hearts[1].SetActive(false);
		}
		
		if(actual_hearts >= 3){
			gui_hearts[2].SetActive(true);
		} else {
			gui_hearts[2].SetActive(false);
		}
		
		if(actual_hearts >= 4){
			gui_hearts[3].SetActive(true);
			
		} else {
			gui_hearts[3].SetActive(false);
		}
		
		if(actual_hearts >= 5){
			gui_hearts[4].SetActive(true);
			
		} else {
			gui_hearts[4].SetActive(false);
		}
		
		if(actual_hearts >= 6){
			extra_hearts.text = "+" + (actual_hearts - 5);
			
		} else {
			extra_hearts.text = "+0";
		}
		
		extra_hearts.Commit();
		
		
	}
	
	public void animate_add_heart(){
		
		switch(actual_hearts){
		
		case 0:
			
		break;
			
		case 1:
			 Instantiate(heart_scale_out, gui_hearts[0].transform.position + new Vector3(0,0,-2) , Quaternion.identity);
		break;
		case 2:
			Instantiate(heart_scale_out, gui_hearts[1].transform.position + new Vector3(0,0,-2) , Quaternion.identity);
		break;			
		case 3:
			Instantiate(heart_scale_out, gui_hearts[2].transform.position + new Vector3(0,0,-2), Quaternion.identity);
		break;
		case 4:
			Instantiate(heart_scale_out, gui_hearts[3].transform.position + new Vector3(0,0,-2), Quaternion.identity);
		break;
		case 5:
			Instantiate(heart_scale_out, gui_hearts[4].transform.position + new Vector3(0,0,-2), Quaternion.identity);
		break;

			
		}
		
		

	}
	
	
	public void delete_heart(){
	
		animate_delete_heart(heart_scale_in);
		
	}
	
	
	public void delete_heart_toplay(){
		
		animate_delete_heart(heart_move);
		
	}
	
	
	void animate_delete_heart(GameObject myheart){
		
		switch(actual_hearts){
		
		case 0:
			
		break;
			
		case 1:
			 Instantiate(myheart, gui_hearts[0].transform.position, Quaternion.identity);
		break;
		case 2:
			Instantiate(myheart, gui_hearts[1].transform.position, Quaternion.identity);
		break;			
		case 3:
			Instantiate(myheart, gui_hearts[2].transform.position, Quaternion.identity);
		break;
		case 4:
			Instantiate(myheart, gui_hearts[3].transform.position, Quaternion.identity);
		break;
		case 5:
			Instantiate(myheart, gui_hearts[4].transform.position, Quaternion.identity);
		break;
		default:
			Instantiate(myheart, gui_hearts[4].transform.position + new Vector3(10,0,0), Quaternion.identity);
		break;
			
		}
		
		if(actual_hearts > 0){
		actual_hearts --;
		EncryptedPlayerPrefs.SetInt("actual_hearts", actual_hearts);

		update_hearts(0);

		}
		

	}
	
	void game(){
	
		Debug.Log("press play");
		if(!presstoplay){		
			if(actual_hearts > 0){
				presstoplay = true;
				delete_heart_toplay();
				
				//fbscript.changingscene = true;
				//fbscript.StopAllCoroutines();
				
				//flurry event
				//if(flurryscript)
				//flurryscript.press_button_play();
				
				if(cloned_music)
				cloned_music.SendMessage("DestroyMusic");
				

				Invoke("load_game_scene", 1.2f);
				
				
			} else {
				var buttons = new string[] { "Close" };
				string texto =  LoadLanguage.gettext("You need more hearts to play");
				EtceteraBinding.showAlertWithTitleMessageAndButtons( LoadLanguage.gettext("More Hearts"), texto , buttons );
			}
		}
	}
	
	void customize(){
//		fbscript.changingscene = true;
		fbscript.StopAllCoroutines();
		GameObject gui_loading = Instantiate( prefab_gui_loading ) as GameObject;
		Invoke ("loading_shop", 0.5f);
		
		
	}
	
	void loading_shop(){
		Application.LoadLevel("shop");
	}
	
	void load_game_scene(){
		
		//GameObject gui_loading = Instantiate( prefab_gui_loading ) as GameObject;
		Invoke("load_game_scene2", 1.2f);
		
		
	}
	
	void load_game_scene2(){
		Application.LoadLevel("game");
	}
	
	public void onemoreheart(){
		
		
		update_hearts(1);	
	}

	public void onemorelessheart(){
		Debug.Log("delete heart");
	delete_heart();	
	}
	
	

	

	
	
	
	long convert_seconds(DateTime mytime){
		
		DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		TimeSpan diff = mytime - origin;
		long totalseconds =  (diff.Days * 86400 ) + (diff.Hours * 3600) + (diff.Minutes * 60) + diff.Seconds;	
		return totalseconds;
		
	}
	
}
