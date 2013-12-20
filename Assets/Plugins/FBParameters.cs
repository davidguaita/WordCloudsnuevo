using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FBParameters  {
	
	public static string fbAppID = "593992127326079";
	public static string fbsecret = "9cbc62715a249804b11682b53b370809";
	public static string fbtoken = "";
	
	public static string userId;
	public static string userName = "Player";
	public static string userPicture;
	public static int playerrank;
	
	public static string gift_keyword = "wcgiftheart";
	public static string requestgift_keyword = "wcrequestheart";
	public static int giftreceived = 0;
	
	public static int maxshownfriends = 10;
	
	public static int number_friends;
	public static int[] friend_rank;
	public static List<string> friend_id_list;
	public static string[] friend_id;
	public static string[] friend_name;
	public static string[] friend_firstname;
	public static string[] friend_picture;
	public static int[] friend_score;
	public static bool[] friend_isplayer;
	
	public static int others_number_friends;
	public static int[] others_friend_rank;
	public static string[] others_friend_id;
	public static string[] others_friend_name;
	public static string[] others_friend_firstname;
	public static string[] others_friend_picture;

	
	public static string id_friend_button_pressed;
	
	public static int notifications_counter;
	public static List<string> notification_id;
	public static List<int> notification_type;
	public static List<string> notification_friendid;
	public static List<string> notification_friendname;
		
	
	public static void reset_values(){
		
	
		userId = null;
		userName = "Player";
		userPicture = null;
		number_friends = 0;
		friend_rank = new int[0];
		friend_id = new string[0];
		friend_name = new string[0];
		friend_firstname = new string[0];
		friend_picture = new string[0];
		friend_score = new int[0];
		friend_isplayer = new bool[0];
		
		others_number_friends = 0;
		others_friend_rank = new int[0];
		others_friend_id = new string[0];
			
		if(notifications_counter > 0){
		notifications_counter = 0;
		notification_id.Clear();
		notification_type.Clear();
		notification_friendid.Clear();
		notification_friendname.Clear();	
			
		}
		
		
	}
	
}
