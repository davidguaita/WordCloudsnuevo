using UnityEditor;
using UnityEngine;
using System.Collections;

public static class WordCloudsEditor
{
	

	[MenuItem("WordClouds/Change License/License davidlinan", false, 10100)]
	public static void ChangeLicense()
	{
		PlayerSettings.companyName = "davidlinan";
		PlayerSettings.bundleIdentifier = "com.davidlinan.wordcloudsb";
		PlayerSettings.bundleVersion = "1.0";
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iPhone, "LINAN");
		
		
	}
	
	[MenuItem("WordClouds/Change License/License David Guaita", false, 10101)]
	public static void ChangeLicenseGREE()
	{
		PlayerSettings.companyName = "davidguaita";
		PlayerSettings.bundleIdentifier = "com.davidguaita.wordclouds";
		PlayerSettings.bundleVersion = "1.0";
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iPhone, "GUAITA");
	}
	
	
}
