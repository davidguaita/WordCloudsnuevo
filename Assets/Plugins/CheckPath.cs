using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class CheckPath : MonoBehaviour {
	
	public string myfilename;
	public static string filepath;
	
	// Use this for initialization
	void Start () {
	
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public static void checkpath(){
		
		
		
		if( Application.platform == RuntimePlatform.IPhonePlayer )
        {

		filepath =	 Application.persistentDataPath + "/";
		} else if (Application.platform == RuntimePlatform.WindowsPlayer){
			filepath = Application.dataPath + "/modules/"; 
		} else {
		filepath = Application.dataPath + "/Resources/modules/";	
		}
		
	}


	
}
