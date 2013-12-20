using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

 

public class EncryptedPlayerPrefs  {


    // MD5 code by Matthew Wegner (from [url]http://www.unifycommunity.com/wiki/index.php?title=MD5[/url])

    // Modify this key in this file :

    private static string privateKey="sAdxrgtdQd5gD024pPrfgzZaBd";

    

    

    public static string Md5(string strToEncrypt) {

        UTF8Encoding ue = new UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++) {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
        return hashString.PadLeft(32, '0');

    }

    

    public static void SaveEncryption(string key, string type, string value) {
		
        string uniqueIdentifier = PlayerPrefs.GetString("udi");// SystemInfo.deviceUniqueIdentifier;
		string check = Md5(key + "_" + type + "_" + privateKey + "_" + uniqueIdentifier + "_" + value);
        PlayerPrefs.SetString(key + "_rollfall", check);

    }

    

    public static bool CheckEncryption(string key, string type, string value) {

		string uniqueIdentifier = PlayerPrefs.GetString("udi");
        
		string check = Md5(key + "_" + type + "_" + privateKey + "_" + uniqueIdentifier + "_" + value);
        
        if(!PlayerPrefs.HasKey(key + "_rollfall")) return false;

        string storedCheck = PlayerPrefs.GetString(key + "_rollfall");

        return storedCheck == check;
    }

    

    public static void SetInt(string key, int value) {

        PlayerPrefs.SetInt(key, value);
        SaveEncryption(key, "int", value.ToString());

    }

    

    public static void SetFloat(string key, float value) {

        PlayerPrefs.SetFloat(key, value);
        SaveEncryption(key, "float", Mathf.Floor(value*1000).ToString());

    }

    

    public static void SetString(string key, string value) {

        PlayerPrefs.SetString(key, value);
        SaveEncryption(key, "string", value);

    }

    

    public static int GetInt(string key) {
        return GetInt(key, 0);
    }

    

    public static float GetFloat(string key) {
        return GetFloat(key, 0f);
    }

    

    public static string GetString(string key) {
        return GetString(key, "");
    }

    

    public static int GetInt(string key,int defaultValue) {

        int value = PlayerPrefs.GetInt(key);
        if(!CheckEncryption(key, "int", value.ToString())) return defaultValue;
        return value;

    }

    

    public static float GetFloat(string key, float defaultValue) {

        float value = PlayerPrefs.GetFloat(key);
        if(!CheckEncryption(key, "float", Mathf.Floor(value*1000).ToString())) return defaultValue;
        return value;

    }

    public static string GetString(string key, string defaultValue) {

        string value = PlayerPrefs.GetString(key);
        if(!CheckEncryption(key, "string", value)) return defaultValue;
        return value;

    }

    public static bool HasKey(string key) {
       return PlayerPrefs.HasKey(key);
    }

    

    public static void DeleteKey(string key) {

        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.DeleteKey(key + "_rollfall");

    }

    

}
