using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Prime31;


public class CloudWordsValues {
	
	public static string version = "1.0";
	public static int version_counter = 0;
	
	public static float heart_every_minutes = 1;
	public static int initial_hearts = 10;
		
	
	public static int game_status = 0; // 0 before start   1-playing    2-gameover 
	
	public static bool firstrun = true;
	
	public static int score;
	public static int score_record;

	//parametros del juego
	public static string language = "ES"; //EN ES 
	public static int methodGetWords = 2;// 0= Random  1= letras de palabras 2=letras de palabras de hasta 5 letras
	
	public static bool LettersPressednotDelete = true;
	public static int numberinitialletters = 5;
	public static float timeBetweenLetters = 1.0f;
	
	
	//global parameters

	
	public static string flurry_id = "44YW76NY9D7GB4CVBJHC";
	public static bool flurry_session_started = false;
	
#if GREE		
	public static string[] IAPProducts = {"jp.gree.rollfall.pack01",  "jp.gree.rollfall.pack02", "jp.gree.rollfall.pack03", "jp.gree.rollfall.pack04", "jp.gree.rollfall.pack05"};
#endif
	
#if DAVID
	public static string[] IAPProducts = {"com.davidlinan.rollfall.pack01",  "com.davidlinan.rollfall.pack02", "com.davidlinan.rollfall.pack03", "com.davidlinan.rollfall.pack04", "com.davidlinan.rollfall.pack05"};
	
#endif
	
	//public static int[] IAPCoins = {1500, 4200, 9400, 24400, 56300};
	//public static List<StoreKitProduct> IAPproducts_loaded;
	
	//public static List<GameCenterLeaderboard> GCleaderboards;
	//public static List<GameCenterAchievement> GCachievements;

	public static bool music = true;
	public static bool soundfx = true;
	public static float sound_level = 1.0f;
	public static float music_level = 1.0f;
	
	
	
}
