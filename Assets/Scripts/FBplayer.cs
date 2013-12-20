using UnityEngine;
using System.Collections;
using System.IO;

public class FBplayer : MonoBehaviour {
	
	public string idplayer;
	public FBScript fbscript;
	public HeartsControl heartscontrol;
	public Renderer myphoto;
	//public GameObject sendlife;
	public GameObject requestlife;
	
	public Texture defaulttexture;

	public bool norequestheart = false;
	public bool invitetojoin = false;
	bool photoloaded = false;
	
	// Use this for initialization
	void Start () {
		

		StartCoroutine(GetPicturesFB());
		
		//requestlife.SetActive(false);

		Invoke("Updating", 0.1f);
	}
	
	// Update is called once per frame
	void Updating () {
	
		if(idplayer == FBParameters.userId || idplayer == null || idplayer == ""){

//			requestlife.SetActive(false);			
			
		} else {
			
//			requestlife.SetActive(true);
		}
			
	}
	
	void Player_sendheart(){
	
	fbscript.sendlife(	idplayer );
	}
	
	void Player_requestheart(){
		
		if(!invitetojoin){
//			fbscript.requestlife(	idplayer );	
		} else {
//			fbscript.invitetoafriend(	idplayer );	
		}
	}
	
	
	public void GetPicture(){
		
		
		if(gameObject.activeSelf){
		StartCoroutine(GetPicturesFB());
		//Debug.Log ("GetPicture. Get Photo for : " + idplayer, gameObject);
		}
		
	}
	
	void OnDisable(){
		
//		Debug.Log("OnDisable.StopCoroutines");
		StopAllCoroutines();
		
	
		
	}
	
	
	private IEnumerator GetPicturesFB()
	{
//			Debug.Log("GetPicture from: " + transform.Find("player_rank").gameObject.GetComponent<SpriteText>().Text, gameObject);
		
			CheckPath.checkpath();
		
			if(idplayer != null){

				string url = "file://" + CheckPath.filepath +  idplayer + ".png";
			
				if(!File.Exists(CheckPath.filepath +  idplayer + ".png")){
				
					//Debug.Log ("Get Photo for : " + idplayer, gameObject);
					url = string.Format( "http://graph.facebook.com/{0}/picture?type=normal", idplayer ); //normal, large			
					
				} 			
				
					Debug.Log( "fetching profile image from url: " + url );
					
					var www = new WWW( url );
			
					yield return www;
			
					if( www.error != null )
					{
						Debug.Log( "Error attempting to load profile image: " + www.error );
					}
					else
					{
						Debug.Log( "got texture: " + www.texture );
						
						//var mytexture = new Texture2D(16,16, TextureFormat.RGB24, false);
					
						myphoto.renderer.material.mainTexture = www.texture;
						
						SaveTexture(www.texture, idplayer);
						
					photoloaded = true;
				
					}
			} else {
				myphoto.renderer.material.mainTexture = defaulttexture;
				photoloaded = true;
			}
		
			
		
		/*
		
		
		
		
			if(mytexturetest != null){
			
			SaveTexture(mytexturetest, "patata");
			} else {
			Debug.Log("holaqase");
			yield return StartCoroutine( loadtex("patata"));
			//StartCoroutine(loadtex("patata"));
			myphoto.renderer.material.mainTexture = mytexturetest;
			}
			
			if(idplayer != null ){
				Debug.Log ("Get Photo for : " + idplayer);
				var url = string.Format( "http://graph.facebook.com/{0}/picture?type=normal", idplayer ); //normal, large
				Debug.Log( "fetching profile image from url: " + url );
				
				var www = new WWW( url );
		
				yield return www;
		
				if( www.error != null )
				{
					Debug.Log( "Error attempting to load profile image: " + www.error );
				}
				else
				{
					Debug.Log( "got texture: " + www.texture );
					
					//var mytexture = new Texture2D(16,16, TextureFormat.RGB24, false);
				
					myphoto.renderer.material.mainTexture = www.texture;
					

					
					
				}
			
		}
		*/
		
	}
	

	
		
	void SaveTexture(Texture mytexture, string filename){

		Texture2D text2D = mytexture as Texture2D;    
		var bytes =text2D.EncodeToPNG();
		File.WriteAllBytes( CheckPath.filepath + filename + ".png",bytes);
		
		
		
	}
	
	
}
