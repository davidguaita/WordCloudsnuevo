using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class LoadLanguage : MonoBehaviour {
	
	//public static Dictionary<string, string> languageText;
	public static Hashtable languageText = new Hashtable();
	public static string filepath;
	public string myfilename;
	
	// Use this for initialization
	void Start () {
	
		
		
	}

	

	
	 static int CountLinesInFile(string f)
    {
		int count = 0;
		using (StringReader r = new StringReader(f))
		{
		    string line;
		    while ((line = r.ReadLine()) != null)
		    {
			count++;
		    }
		}
		return count;
	 }
	

	
	
	public static void load_language(string idioma){
		

		
		StringReader tr = null;
		string language = "language_" + idioma;
		TextAsset data = (TextAsset)Resources.Load(language, typeof(TextAsset));
		tr = new StringReader(data.text); 
		int counter = CountLinesInFile(data.text);
		Debug.Log("languages lines:"  + counter);

		int mycounter  = 0;
		
		languageText = new Hashtable();
		languageText.Clear();
		
		while (mycounter < counter ){
			
			string line = tr.ReadLine();
			string[] ps = line.Split(";"[0]);
			
			string key = ps[0];
			string valor = ps[1];
			if(key != null && valor != null){
				if(!languageText.ContainsKey(key))
				languageText.Add(ps[0], ps[1]);
			}
					
			mycounter++;

		}

		tr.Close();

		Debug.Log("ADDED LANGUAGE:" + idioma + " with " + mycounter);
		

	}
	
	public static string gettext(string key){
		
		if(languageText.ContainsKey(key)){
		return (string)languageText[key];
		} else {
		return key;	
		}
		
	}
	

}
