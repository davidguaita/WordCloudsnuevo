using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
//REGEX
using System.Text.RegularExpressions;
[System.Serializable]
public class LoadWords : MonoBehaviour {
	
	//public static Dictionary<string, string> languageText;
	//public static Hashtable words = new Hashtable();
	//public static Dictionary<string, int> words;
	public static List<string> words;
	
	public static List<string> words_3_letters;
	public static List<string> words_4_letters;
	public static List<string> words_5_letters;
	public static List<string> words_6_letters;
	public static List<string> words_7_letters;
	public static List<string> words_8_letters;
	
	//public static string[] words2;
	
	public static string[] letters_key;
	public static float[] letters_porcentage;
	
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
	

	
	public static void load_words(string idioma){
	
		string language = "wordlist_" + idioma;
		TextAsset data = (TextAsset)Resources.Load(language, typeof(TextAsset));

        Stream stream = new MemoryStream( data.bytes );
        StreamReader streamReader = new StreamReader( stream );

		int counter = 0;

		words = new List<string>();
		words_3_letters = new List<string>();
		words_4_letters = new List<string>();
		words_5_letters = new List<string>();
		words_6_letters = new List<string>();
		words_7_letters = new List<string>();
		words_8_letters = new List<string>();
		//words = new Dictionary<string, int>();
		words.Clear();
		words_3_letters.Clear();
		words_4_letters.Clear();
		words_5_letters.Clear();
		words_6_letters.Clear();
		words_7_letters.Clear();
		words_8_letters.Clear();
		
		using (StreamReader r = new StreamReader(stream))
   		 {
        int i = 0;
		string line = null;
        while ((line = r.ReadLine()) != null) 
        { 
            i++;
			words.Add(line);
				switch(line.Length){
				
				case 3:
				words_3_letters.Add(line);
				break;
				case 4:
				words_4_letters.Add(line);
				break;
				case 5:
				words_5_letters.Add(line);
				break;
				case 6:
				words_6_letters.Add(line);
				break;
				case 7:
				words_7_letters.Add(line);
				break;
				default:
				words_8_letters.Add(line);
				break;

				}
				
			//words.Add(line, line.Length);	
        }
        counter = i;
   		}
		
		streamReader.Close();
		
		
		Debug.Log("ADDED WORDS:" + counter );
		Debug.Log("Por Palabras: 3-"  + words_3_letters.Count + " 4-" + words_4_letters.Count + " 5-" + words_5_letters.Count + " 6-" + 
			words_6_letters.Count + " 7-" + words_7_letters.Count + " 8oMas-" + words_8_letters.Count);
		
		

	}
	
	

	
	public static void load_letters(string idioma){
		

		StringReader tr = null;
		string language = "characters_" + idioma;
		TextAsset data = (TextAsset)Resources.Load(language, typeof(TextAsset));
		tr = new StringReader(data.text); 
		int counter = CountLinesInFile(data.text);
		Debug.Log("LETTERS COUNTER:"  + counter);

		int mycounter  = 0;
		
		letters_key = new string[counter];
		letters_porcentage = new float[counter];
		
		while (mycounter < counter ){
			
			string line = tr.ReadLine();
			string[] ps = line.Split(";"[0]);
			
			string key = ps[0];
			float valor =  float.Parse(ps[1]);
			if(key != null && valor != null){
				letters_key[mycounter] = key;
				letters_porcentage[mycounter] = valor;
				
			}
					
			mycounter++;
			
		}

		tr.Close();

		Debug.Log("ADDED LETTERS:" + idioma + " with " + mycounter);
		

	}
	
	
	
	public static bool exist(string key){
		
		if(words.Contains(key)){
		return true; //(string)words[key];
		} else {
		return false;	
		}
		
	}
	
	

}
