using UnityEngine;
using System.Collections;
using System.Text;


public class ResultHastablesScores : Object
{
	
	public static ArrayList myArrayList;
	
	
	// helper to log Arraylists and Hashtables
	public static void logObject( object result )
	{
		myArrayList = new ArrayList();
		
		 if( result.GetType() == typeof( Hashtable ) ){
			addHashtableToString( (Hashtable)result );
		} else {
			Debug.Log( "result is not a hashtable" );
		}
	}
	
	
	public static void addHashtableToString( Hashtable item )
	{
		foreach( DictionaryEntry entry in item )
		{
			if( entry.Value is Hashtable )
			{
				addHashtableToString( (Hashtable)entry.Value );
			}
			else if( entry.Value is ArrayList )
			{
				addArraylistToString( (ArrayList)entry.Value );
			}
	
		}
	}
	
	
	public static void addArraylistToString(  ArrayList result )
	{
		// we start off with an ArrayList of Hashtables
		foreach( object item in result )
		{
			if( item is Hashtable ){
				ResultHastablesScores.addHashtableToString( (Hashtable)item );
				myArrayList.Add(item);
			}
			
		}
		
		
	}

}
