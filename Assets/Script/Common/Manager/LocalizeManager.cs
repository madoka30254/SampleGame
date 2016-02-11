using UnityEngine;
using System.Collections;
using System.IO;

// originally found in: http://forum.unity3d.com/threads/35617-LocalizeManager-Localization-Script

/// <summary>
/// LocalizeManager
/// 
/// Reads PO files in the Assets\Resources\Languages directory into a Hashtable.
/// Look for "PO editors" as that's a standard for translating software.
/// 
/// Example:
/// 
/// load the language file:
///   LocalizeManager.LoadLanguage("helptext-pt-br");
/// 
/// which has to contain at least two lines as such:
///   msgid "HELLO WORLD"
///   msgstr "OLA MUNDO"
/// 
/// then we can retrieve the text by calling:
///   LocalizeManager.GetText("HELLO WORLD");
/// </summary>
public class LocalizeManager : MonoBehaviour {

	private static LocalizeManager instance;
	private static Hashtable textTable;
	private LocalizeManager () {} 
	private static bool loadFlg = false;

	private static LocalizeManager Instance 
	{
		get 
		{
			if (instance == null) 
			{
				// Because the LocalizeManager is a component, we have to create a GameObject to attach it to.
				GameObject notificationObject = new GameObject("Default LocalizeManager");

				// Add the DynamicObjectManager component, and set it as the defaultCenter
				instance = (LocalizeManager) notificationObject.AddComponent(typeof(LocalizeManager));
			}
			return instance;
		}
	}

	public static LocalizeManager GetInstance ()
	{
		return Instance;
	}	

	public static bool LoadLanguage (string filename = "")
	{
		GetInstance();

		if (loadFlg) {
			return true;
		}

		if (filename == null || filename == "")
		{
//			Debug.Log("[LocalizeManager] loading default language.");
//			textTable = null; // reset to default
//			return false; // this means: call LoadLanguage with null to reset to default
			SystemLanguage sysLang = Application.systemLanguage;
			if (sysLang == SystemLanguage.Japanese) {
				filename = "lang_ja";
//				filename = "lang_ja";
			} else {
				filename = "lang_en";
			}
		}

		string fullpath = "Languages/" +  filename; // the file is actually ".txt" in the end
		TextAsset textAsset = (TextAsset) Resources.Load(fullpath, typeof(TextAsset));
		if (textAsset == null) 
		{
			Debug.LogError("[LocalizeManager] "+ fullpath +" file not found.");
			return false;
		}

//		Debug.Log("[LocalizeManager] loading: "+ fullpath);

		if (textTable == null) 
		{
			textTable = new Hashtable();
		}

		textTable.Clear();

		StringReader reader = new StringReader(textAsset.text);
		string key = null;
		string val = null;
		string line;
		while ( (line = reader.ReadLine()) != null)
		{
			if (line.StartsWith("msgid \""))
			{
				key = line.Substring(7, line.Length - 8);
			}
			else if (line.StartsWith("msgstr \""))
			{
				val = line.Substring(8, line.Length - 9);
			}
			else
			{
				if (key != null && val != null) 
				{
					// TODO: add error handling here in case of duplicate keys
					textTable.Add(key, val);
					key = val = null;
				} 
			}
		}

		reader.Close();

		loadFlg = true;

		return true;
	}


	public static string GetText (string key)
	{

		LoadLanguage ("");

		if (key != null && textTable != null)
		{
//			foreach (string keyStr in textTable.Keys) {
//				string work;
//				work = (string)textTable[keyStr];
//
//				Debug.Log("キー値＝"+key+",値＝"+work);
//			}
			if (textTable.ContainsKey(key))
			{
				string result = (string)textTable[key];
				if (result.Length > 0)
				{
					key = result;
					key = key.Replace("¥n","\n");
				}
			}
		}
		return key;
	}
}