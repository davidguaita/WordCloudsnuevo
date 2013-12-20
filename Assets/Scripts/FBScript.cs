using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Prime31;

public class FBScript : MonoBehaviour {
	
	public HeartsControl heartscontrol;
	
	//public UIScrollList mylist;
	public GameObject[] mylistItems;
	
	private bool _hasPublishPermission = false;
	private bool _hasPublishActions = false;
	private bool onepersession = false;
	
	public bool changingscene = false;
	
	public GameObject myItemTemplate;
	
	
	public tk2dUILayout prefabItem;
	public tk2dUIScrollableArea manualScrollableArea;
	public tk2dUILayout lastListItem;
	
	public List<GameObject> scrolllist;
	
	public GameObject fbConnectBtn;
	public GameObject fbLoadingText;
	
	
		// common event handler used for all graph requests that logs the data to the console
	void completionHandler( string error, object result )
	{
		if( error != null )
			Debug.LogError( error );
		else
			Prime31.Utils.logObject( result );
	}
	
	
	        void OnEnable()
        {

            FacebookManager.sessionOpenedEvent         		+= OnEventFacebookLogin;
			FacebookManager.loginFailedEvent                += OnEventFacebookLoginFailed;
		    FacebookManager.reauthorizationSucceededEvent   += OnEventFacebookReauthorizationSucceeded;
            FacebookManager.reauthorizationFailedEvent      += OnEventFacebookReauthorizationFailed;
//			FacebookManager.graphRequestCompletedEvent 		+= OnEventFacebookGraphRequestSucceeded;

        }
        void OnDisable()
        {
			
            FacebookManager.sessionOpenedEvent         		-= OnEventFacebookLogin;
			FacebookManager.loginFailedEvent                -= OnEventFacebookLoginFailed;
            FacebookManager.reauthorizationSucceededEvent   -= OnEventFacebookReauthorizationSucceeded;
            FacebookManager.reauthorizationFailedEvent      -= OnEventFacebookReauthorizationFailed;
//			FacebookManager.graphRequestCompletedEvent 		-= OnEventFacebookGraphRequestSucceeded;

        }
	
	// Use this for initialization
	void Start () {
	
	fbLoadingText.SetActive(false);
		
	prefabItem.transform.parent = null;
	prefabItem.gameObject.SetActive(false);
	
				if(iPhoneSettings.internetReachability != iPhoneNetworkReachability.NotReachable || Network.player.ipAddress.ToString() != "127.0.0.1"){
	
				if(PlayerPrefs.GetInt("fblogged") == 1){
				
					fbConnectBtn.SetActive(false);
				
					if(FBParameters.number_friends > 0){
					Debug.Log("load temp friends");
						
						//StartCoroutine("CreateButtonsWithData");
						//fbguiloadingfriends.SetActive(true);
						//fbguiloadingfriends_text.text = LoadLanguage.gettext("Updating scores");
						//fbguiloadingfriends_text.Commit();
						Invoke("FacebookInit", 0.25f);
					} else {
					
					//fbguiloadingfriends.SetActive(true);
					//fbguiloadingfriends_text.text = LoadLanguage.gettext("Loading friends");
					//fbguiloadingfriends_text.Commit();
					FacebookInit();	
						

						
					}
				} else {
					
					NoConnection();
					
				}
					
				
			} else {
				NoConnection();	
			}
		
	
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void connectFacebook(){
		
		PlayerPrefs.SetInt("fblogged", 1);
		
		if(scrolllist.Count > 0){
		foreach(GameObject obj in scrolllist){
		Destroy(obj);	
		}
		scrolllist.Clear();
		}
		
		FacebookInit();	
		
		//Application.LoadLevel(Application.loadedLevel);
		
	}
	
	
	void createList(int[] rank, string[] name, int[] score, string[] id ){
	
			Debug.Log ("creo la lista" );
		
		if(scrolllist.Count > 0){
		foreach(GameObject obj in scrolllist){
		Destroy(obj);	
		}
		scrolllist.Clear();
		}
		
		scrolllist = new List<GameObject>();
		
		Debug.Log ("creo la lista_2" );
		// Add a bunch of items to the manual list
		// You will need to parent the object manually and then calculate the step
		float x = 0;
		float w = (prefabItem.GetMaxBounds() - prefabItem.GetMinBounds()).y;
		for (int i = 0; i < score.Length; ++i) {
			tk2dUILayout layout = Instantiate(prefabItem) as tk2dUILayout;
			layout.transform.parent = manualScrollableArea.contentContainer.transform;
			layout.transform.localPosition = new Vector3(0, x, 0);
			layout.gameObject.SetActive(true);
			x -= w;
			layout.transform.Find("Name").GetComponent<tk2dTextMesh>().text = name[i];
			layout.transform.Find("Score").GetComponent<tk2dTextMesh>().text = score[i] + "";
			layout.transform.Find("Rank").GetComponent<tk2dTextMesh>().text = rank[i] + "";
			layout.transform.GetComponent<FBplayer>().idplayer = id[i];
			layout.transform.GetComponent<FBplayer>().GetPicture();
			Debug.Log ("creo la lista_3" );
			scrolllist.Add( layout.transform.gameObject);
			
									//mylistItems[i].gameObject.GetComponent<FBplayer>().idplayer = FBParameters.friend_id[i];
						//mylist_fb_IDs[i] = FBParameters.friend_id[i];
				
						//mylistItems[i].gameObject.GetComponent<FBplayer>().GetPicture();
			
		}
		
		Debug.Log ("creo la lista_4" );
		//lastListItem.transform.localPosition = new Vector3(lastListItem.transform.localPosition.x, x, 0);
		//x -= (lastListItem.GetMaxBounds() - lastListItem.GetMinBounds()).y;
		manualScrollableArea.ContentLength = Mathf.Abs( x);
		Debug.Log ("creo la lista_5" );
		
	}
	
	
	void FacebookInit(){
		
			fbLoadingText.SetActive(true);
		
	          FacebookBinding.init();

            bool isLoggedIn = FacebookBinding.isSessionValid();
            if ( !isLoggedIn ) {
                var permissions = new string[] {  "user_games_activity", "friends_games_activity","email" };
                    FacebookBinding.loginWithReadPermissions( permissions );
            } 	
		
				// note that posting a score requires publish_actions permissions!
				var parameters = new Dictionary<string,object>()
				{ { "score", (CloudWordsValues.score_record + "") } };
				Facebook.instance.graphRequest( "me/scores", HTTPVerb.POST, parameters, completionHandler );
		
		
	}
	
	void OnEventFacebookLogin()

        {
			PlayerPrefs.SetInt("fblogged", 1);
			Debug.Log( "Successfully logged in to Facebook");
		
			_hasPublishPermission = FacebookBinding.getSessionPermissions().Contains( "publish_stream" );
			_hasPublishActions = FacebookBinding.getSessionPermissions().Contains( "publish_actions" );
		
            if(!_hasPublishActions){
            var permissions = new string[] { "publish_actions" , "publish_stream" };
                FacebookBinding.reauthorizeWithPublishPermissions( permissions, FacebookSessionDefaultAudience.Friends );
			} else {
			
			OnEventFacebookReauthorizationSucceeded();
			}
			


		}
	
	void OnEventFacebookLoginFailed( P31Error error )

        {
            Debug.Log( "Facebook login failed: " + error );
        } 
	
	
	void OnEventFacebookReauthorizationSucceeded()
        {
			_hasPublishPermission = FacebookBinding.getSessionPermissions().Contains( "publish_stream" );
			_hasPublishActions = FacebookBinding.getSessionPermissions().Contains( "publish_actions" );
		
			if(_hasPublishActions)
			PlayerPrefs.SetInt("fb_publish", 1);
		
			Debug.Log("Facebook reautorizacion con exito");
		
			if(!onepersession){
			//fbguiloadingfriends.SetActive(true);
   			Invoke("GetToken", 0.2f);
			Invoke("GetMe", 0.5f);
			onepersession = true;
			}
        }

	void OnEventFacebookReauthorizationFailed( P31Error error )
        {
            Debug.Log( "Facebook reauth failed: " + error );
		
			if(_hasPublishActions)
			PlayerPrefs.SetInt("fb_publish", 1);
			
			if(PlayerPrefs.GetInt("fblogged") == 1){
   			Invoke("GetToken", 0.2f);
			Invoke("GetMe", 0.5f);
			}
		
        }
	
	
	void GetToken(){
		
				Debug.Log ("paso_gettoken");
		
			var token = FacebookBinding.getAccessToken();
			Debug.Log( "access token: " + token );
			FBParameters.fbtoken = token;
		}
	
	
	void GetMe(){
			
		Debug.Log ("paso_getme");
			
			if(FBParameters.userId == "" || FBParameters.userId == null){
		

			
			
				Facebook.instance.graphRequest( "me?fields=name,gender,installed,picture", HTTPVerb.GET, ( error, obj ) =>
				{
					// if we have an error we dont proceed any further
					if( error != null )
						return;
					
					if( obj == null )
						return;
					
				var ht = obj as Dictionary<string,object>;
            	//Debug.Log ("pas01:" + ht2["id"].ToString());    
				
					// grab the userId and persist it for later use
					//var ht = obj as Hashtable;
				
					FBParameters.userId = ht["id"].ToString();
				
					FBParameters.userName = ht["name"].ToString();
				
					FBParameters.userPicture = ((IDictionary<string, object>)((IDictionary<string, object>)ht["picture"])["data"])["url"].ToString();
					Debug.Log( "Hello " + FBParameters.userName  );
					
					
					Invoke("GetScores", 0.1f);
				

					
				});
		
			
			} else {
					
					Invoke("GetScores", 0.1f);		
			}
		
		 
		
		}

	void GetScores(){
		
				
			Debug.Log ("paso_getscores");
		
			string myurl =  FBParameters.fbAppID + "/scores";
			Facebook.instance.graphRequest( myurl, HTTPVerb.GET, ( error, obj ) =>
			{
				// if we have an error we dont proceed any further
				if( error != null ){
				Debug.Log("hay un error al cargar scores:" + error.ToString());
					return;
				
				}
				
				//extraemos del hashtable una lista arraylist
				//ResultHastablesScores.logObject( obj );
				
				var ht = obj as Dictionary<string,object>;
				var hl = ((IList<object>)ht["data"]);
				
				int contador = hl.Count;
				Debug.Log("contador:" + contador);
			

				

				
				if (contador > 0){
				
					//creamos la lista de botones con los amigos
					//createListButtons(ResultHastablesScores.myArrayList.Count);
				
					
					//ahora asignamos a cada amigo en la lista
					int counter = 0;
				
					// resetea tabla de valores interna del score - util para despues hacer comparativas, por ejemplo
					FBParameters.number_friends = contador;
					FBParameters.friend_rank  = new int[contador];
					FBParameters.friend_id_list = new List<string>();				
					FBParameters.friend_id = new string[contador];
					FBParameters.friend_name = new string[contador];
					FBParameters.friend_firstname = new string[contador];
					FBParameters.friend_picture = new string[contador];
					FBParameters.friend_score = new int[contador];
					FBParameters.friend_isplayer = new bool[contador];
			
					for(int i=0; i<contador;i++){
					FBParameters.friend_rank[i] = i;
					FBParameters.friend_score[i] = int.Parse( ((IDictionary<string, object>)(hl[i]))["score"].ToString());
					//((IDictionary<string, object>)item["user"])["name"].ToString()
					FBParameters.friend_name[i] =  ((IDictionary<string, object>)((IDictionary<string, object>)(hl[i]))["user"])["name"].ToString();
					FBParameters.friend_id[i] = ((IDictionary<string, object>)((IDictionary<string, object>)(hl[i]))["user"])["id"].ToString();
					FBParameters.friend_id_list.Add(FBParameters.friend_id[i]);
					FBParameters.friend_isplayer[i] = false;
					
						//cambia color button
						if(FBParameters.friend_id[i] == FBParameters.userId){
	
						
							//tabla interna
							FBParameters.friend_isplayer[counter] = true;
							FBParameters.playerrank = i;
							
	
						}
					Debug.Log("FB SCORE: " + FBParameters.friend_name[i] + " " + FBParameters.friend_score[i] + " " + FBParameters.friend_isplayer[i] );
					}
	

				}
				
				if(!changingscene){
					//StartCoroutine("CreateButtonsWithData");	
	
					//get request
					//Invoke("GetAppRequest", 0.1f);
					
					//invoke getfriends to get the pictures later
					Invoke("GetFriends", 0.1f);
				}
				
			});	
			
			
		
	}
	
	void GetFriends(){
		
		
			Debug.Log("Busco amigos o que?" + FBParameters.others_number_friends);
			if(FBParameters.others_number_friends > 0){
			Debug.Log("ya los tengo");
				if(!changingscene && FBParameters.others_number_friends > FBParameters.friend_id.Length){
				StartCoroutine("CreateButtonsWithOtherFriendsData");
				
				} else {
				StartCoroutine("CreateButtonsWithData");	
				}
				return;
				
			}
			Debug.Log("los busco");
			Facebook.instance.graphRequest( "me/friends?fields=name,first_name,gender,installed,picture", HTTPVerb.GET, ( error, obj ) =>
			{
				// if we have an error we dont proceed any further
				if( error != null){
					//mylist.CreateItem(bt_invitefriends);
					//fbguiloadingfriends.SetActive(false);//EtceteraBinding.hideActivityView();
					
					return;
				}
				
				
				var ht = obj as Dictionary<string,object>;
				var hl = ((IList<object>)ht["data"]);
				
				int contador = hl.Count;
				Debug.Log("numeroAmigos:" + contador);
		

				
					if (contador> 0){

					// resetea tabla de valores interna del score - util para despues hacer comparativas, por ejemplo
					FBParameters.others_number_friends = contador;
					FBParameters.others_friend_rank  = new int[contador];
					FBParameters.others_friend_id = new string[contador];
					FBParameters.others_friend_name = new string[contador];
					FBParameters.others_friend_firstname = new string[contador];
					FBParameters.others_friend_picture = new string[contador];
				
					Debug.Log("empieza amigos");
				
					for (int i = 0; i< contador ; i++){
					
						FBParameters.others_friend_rank[i] = i;
						FBParameters.others_friend_id[i] = ((IDictionary<string, object>)(hl[i]))["id"].ToString();
						FBParameters.others_friend_name[i] =((IDictionary<string, object>)(hl[i]))["name"].ToString();
						FBParameters.others_friend_firstname[i] = ((IDictionary<string, object>)(hl[i]))["first_name"].ToString();
						FBParameters.others_friend_picture[i] = ((IDictionary<string, object>)((IDictionary<string, object>)((IDictionary<string, object>)(hl[i]))["picture"])["data"])["url"].ToString();
					/*
						var ht3 = ResultHastablesFriends.myArrayList[i] as Hashtable;
						
						FBParameters.others_friend_rank[i] = i;
						FBParameters.others_friend_id[i] = ht3["id"].ToString();
						FBParameters.others_friend_name[i] = ht3["name"].ToString();
						FBParameters.others_friend_firstname[i] = ht3["first_name"].ToString();
						FBParameters.others_friend_picture[i] = ((Hashtable)((Hashtable)ht3["picture"])["data"])["url"].ToString();
					*/	
						
					
						Debug.Log("Amigo: " + FBParameters.others_friend_id[i] + " name:" + FBParameters.others_friend_name[i] + " picture:" + FBParameters.others_friend_picture[i] );
						
					}
				
					if(!changingscene && contador > FBParameters.friend_id.Length){
					
						StartCoroutine("CreateButtonsWithOtherFriendsData");
					
					} else {
						Debug.Log("todos tus amigos estan conectados a fb");	
						StartCoroutine("CreateButtonsWithData");	
					}
				} else {
				
				//mylist.CreateItem(bt_invitefriends);
				//fbguiloadingfriends.SetActive(false);//EtceteraBinding.hideActivityView();
				}
			

			
			});	
		
			
			
			
		

			
		
	}
	
	void NoConnection(){
			

		fbConnectBtn.SetActive(true);
//				Debug.Log ("paso_NoConnection");
		
			//mylist.ClearList(true);
			mylistItems = new GameObject[3];
		
			FBParameters.friend_rank  = new int[mylistItems.Length];
			FBParameters.friend_id = new string[mylistItems.Length];
			FBParameters.friend_id_list = new List<string>();
			FBParameters.friend_name = new string[mylistItems.Length];
			FBParameters.friend_firstname = new string[mylistItems.Length];
			FBParameters.friend_picture = new string[mylistItems.Length];
			FBParameters.friend_score = new int[mylistItems.Length];
			FBParameters.friend_isplayer = new bool[mylistItems.Length];		
		
		
//					
			
			string[] playersname = {"player1", "player2", "player3"};
			int[] playerscores = {1000, 200, 0};
			int counterfriend = 0;
			bool showyou = false;
		
			for (int i = 0; i < mylistItems.Length; i++){
				
			
				if(CloudWordsValues.score_record >= playerscores[counterfriend] && !showyou){
					FBParameters.friend_rank[i] = i + 1;
					FBParameters.friend_name[i] = "YOU";
					FBParameters.friend_score[i] = CloudWordsValues.score_record ;
					FBParameters.friend_isplayer[i] = false;	
					showyou = true;
				
				} else {
					
					FBParameters.friend_rank[i] = i+1;
					FBParameters.friend_name[i] = playersname[counterfriend];
					FBParameters.friend_score[i] = playerscores[counterfriend] ;
					FBParameters.friend_isplayer[i] = false;			
					counterfriend++;
				}
				
			
			}


		
			StartCoroutine("CreateButtonsWithData");
			
		
	}
	
	
	IEnumerator CreateButtonsWithData(){
	
		
					//mylist.ClearList(true);
		
					// creamos la lista en el juego con el numero de amigos(counter)
					//mylistItems = new GameObject[FBParameters.friend_rank.Length];


				int playerrank = 0;
				for(int i = 0; i< FBParameters.friend_rank.Length; i++){
				
			
					//if(!FBParameters.friend_isplayer[i]){
				
				
						createList(FBParameters.friend_rank, FBParameters.friend_name, FBParameters.friend_score, FBParameters.friend_id);
				
//						IUIListObject myitem = mylist.CreateItem(myItemTemplate);
//						mylistItems[i] = myitem.gameObject;
				
						//mylistItems[i].transform.Find("player_name").gameObject.GetComponent<SpriteText>().Text =  FBParameters.friend_name[i];
						//mylistItems[i].transform.Find("player_distance").gameObject.GetComponent<SpriteText>().Text = FBParameters.friend_score[i] + "m"  ;
						//mylistItems[i].gameObject.GetComponent<FBplayer>().idplayer = FBParameters.friend_id[i];
						//mylist_fb_IDs[i] = FBParameters.friend_id[i];
				
						//mylistItems[i].gameObject.GetComponent<FBplayer>().GetPicture();

					//} else {
						playerrank = i;
						//mylist_fb_IDs[i] = FBParameters.userId;
						
					//}
				
			
				}
					
					
					//check record distance saved
					if(CloudWordsValues.score_record == 0)
					CloudWordsValues.score_record = PlayerPrefs.GetInt("myrecord");		
							
					//check actual rank of the player
					int myactualrank = 0;
					for(int i= 0; i < FBParameters.friend_rank.Length; i++){
						
						if(CloudWordsValues.score_record < FBParameters.friend_score[i])
							myactualrank ++;
						
					}
					
					Debug.Log("actualrank:" + myactualrank);
					
//					IUIListObject myitemplayer = mylist.CreateItem(myItemTemplate, myactualrank);
		
//					myitemplayer.transform.Find("player_name").gameObject.GetComponent<SpriteText>().Text =  FBParameters.userName + "" ;
		

//					myitemplayer.transform.Find("player_distance").gameObject.GetComponent<SpriteText>().Text = CloudWordsValues.score_record + "m"  ;	
//					myitemplayer.gameObject.GetComponent<FBplayer>().idplayer = FBParameters.userId;
//					myitemplayer.gameObject.GetComponent<FBplayer>().GetPicture();
		
//					myitemplayer.transform.Find("player_requestheart").gameObject.SetActive(false);
		
//					GameObject myobj = myitemplayer.transform.Find("playerBG").gameObject;
//					iTween.ColorTo(myobj,iTween.Hash("r",0.7f,"g",0.7f,"b",0.7f,"time",0.5,"easetype","linear"));	
		

					//reassign the correct numbers rank
//					for(int j = 0; j< mylist.Count ; j++)
//					mylist.GetItem(j).transform.Find("player_rank").gameObject.GetComponent<SpriteText>().Text = (j + 1).ToString() ;
					
					
				
//					mylist.CreateItem(bt_invitefriends);
		
				fbLoadingText.SetActive(false);
		
				yield return new WaitForEndOfFrame();
				
		
	}
	
	IEnumerator CreateButtonsWithOtherFriendsData(){
		
				StartCoroutine("CreateButtonsWithData");
				/*
		
				Debug.Log ("paso_createbuttonsfromotherfriends");
				
				int maxplayers = Mathf.Max(FBParameters.friend_rank.Length, FBParameters.maxshownfriends);
				int[] newlistrank = new int[maxplayers];
				string[] newfriendname = new string[maxplayers];
				int[] newfriendscore = new int[maxplayers];
				string[] newfriendid = new string[maxplayers];
				
				Debug.Log ("copio los amigos que si" + maxplayers);
				//copio los que estan en facebook
				for(int j=0; j < FBParameters.friend_rank.Length; j++){
					newlistrank[j] = FBParameters.friend_rank[j];
					newfriendname[j] = FBParameters.friend_name[j];
					newfriendscore[j] = FBParameters.friend_score[j];
					newfriendid[j] = FBParameters.friend_id[j];			
			
				}
		
				Debug.Log ("copio los amigos que no");
				int counter = 0;
				//añado los amigos que no estan en fb hasta 50
				if(maxplayers < FBParameters.maxshownfriends){
					
					for(int i= FBParameters.friend_rank.Length; i< maxplayers ; i++){
					newlistrank[i] = FBParameters.others_friend_rank[counter];
					newfriendname[i] = FBParameters.others_friend_name[counter];
					newfriendscore[i] = 0;
					newfriendid[i] = FBParameters.others_friend_id[counter];
					counter++;
					}
			
				}
				Debug.Log ("creo la lista");
				createList(newlistrank, newfriendname, newfriendscore, newfriendid);

		 */
		
				yield return new WaitForEndOfFrame();
	}	
	
	
	
	
	
	public void sendlife(string myfriend){
		
			
				Debug.Log("send life popup:" + heartscontrol.actual_hearts + " to: " + myfriend);
				//miro si tengo dinero
				if(heartscontrol.actual_hearts > 0){
					
					
					string mystring1 =  LoadLanguage.gettext("Send extra heart!");
					string mystring2 =  FBParameters.userName + LoadLanguage.gettext(" sent you an extra heart");  
					
					var parameters = new Dictionary<string,string>();
					parameters.Add( "title", mystring1 );
					parameters.Add( "message", mystring2 );
					parameters.Add( "data", FBParameters.gift_keyword );
					parameters.Add( "to", myfriend );	//FBParameters.userId);// send to me onlytest// myfriend );	
					parameters.Add( "description",  LoadLanguage.gettext("Play against your friends in Roll Fall Check out this great iOS game!") );
					parameters.Add( "link", "http://google.es" );
					FacebookBinding.showDialog( "apprequests", parameters );

					} 

	}
	
	
}
