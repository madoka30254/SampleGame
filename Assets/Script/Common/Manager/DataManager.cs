using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class DataManager : MonoBehaviour {

	//鍵情報
	private string[] EncryptedPlayerPrefsKeys = {"k4iD59Si","j8sOUaD3","oTeI8Jd6","iM0vhDB3","Lj6Si9Os"};

	// ユーザデータ ------------------------------------------------------
	public TUser tUser;
	public static TUser user {
		get{ return DataManager.Instance.tUser; }
	}

	//ユーザデータのロード
	public bool LoadUserData () {
//		InitUserData ();
		EncryptedPlayerPrefs.keys = new string[5];
		EncryptedPlayerPrefs.keys = EncryptedPlayerPrefsKeys;
		string userData = EncryptedPlayerPrefs.GetString (DEFINE.USER_DATA_PREFS_KEY1,"");
		string chkUserDataMd5 = EncryptedPlayerPrefs.GetString (DEFINE.USER_DATA_PREFS_KEY2,"");

		//改ざんチェック
		if (Md5Sum(userData) != chkUserDataMd5) {
			//TODO 不正の場合削除する？
			InitUserData ();
			return false;
		}

		byte[] decodedBytes = Convert.FromBase64String (userData);
		string decodedText = Encoding.UTF8.GetString (decodedBytes);
		Debug.Log ("[user_data]"+decodedText);
		tUser = (TUser)LitJson.JsonMapper.ToObject<TUser>(decodedText);

		return true;
	}

	//ユーザデータの保存
	public void SaveUserData (TUser _user) {
		string jsonStr = LitJson.JsonMapper.ToJson( _user );
		byte[] bytesToEncode = Encoding.UTF8.GetBytes (jsonStr);
		string encodedText = Convert.ToBase64String (bytesToEncode);
		string key = Md5Sum (encodedText);

		EncryptedPlayerPrefs.keys = new string[5];
		EncryptedPlayerPrefs.keys = EncryptedPlayerPrefsKeys;
		EncryptedPlayerPrefs.SetString (DEFINE.USER_DATA_PREFS_KEY1,encodedText);
		EncryptedPlayerPrefs.SetString (DEFINE.USER_DATA_PREFS_KEY2,key);
	}

	//ユーザデータの初期化
	private void InitUserData () {
		TUser user = new TUser ();
		user.highScore = 0;
		user.currentScore = 0;
		user.currentCatId = 1;
		user.fishCount = 0;
		user.userCatList = new List<TUserNeko> ();
		SaveUserData (user);
		LoadUserData ();
	}

	// ねこマスターデータ ------------------------------------------------------
	public List<TNeko> tNekoMaster = new List<TNeko>();
	public static List<TNeko> NekoManster {
		get{ return DataManager.Instance.tNekoMaster;}
	}
	private int NEKO_COUNT = 30;

	public void LoadNekoMaster(){
		for (int i = 0;i < NEKO_COUNT;i++) {
			TNeko neko = new TNeko ();
			int id = i + 1;
			neko.nekoId = id;
			neko.rare = 1;
			neko.nekoName = LocalizeManager.GetText ("cat_name_"+id.ToString());
			tNekoMaster.Add (neko);
		}
	}

	/// <summary>
	/// ねこIDから名前を取得
	/// </summary>
	/// <returns>The cat name.</returns>
	/// <param name="_nekoId">_neko identifier.</param>
	public string getCatName(int _nekoId) {
		for (int i = 0;i < NekoManster.Count;i++) {
			if (_nekoId == NekoManster[i].nekoId) {
				return NekoManster [i].nekoName;
			}
		}
		return "";
	}

	/// <summary>
	/// IDからねこデータの取得
	/// </summary>
	/// <returns>The neko data.</returns>
	/// <param name="_nekoId">_neko identifier.</param>
	public TNeko getNekoData (int _nekoId) {
		for (int i = 0;i < NekoManster.Count;i++) {
			if (_nekoId == NekoManster[i].nekoId) {
				return NekoManster [i];
			}
		}
		return null;
	}

	/// <summary>
	/// Md5生成
	/// </summary>
	/// <returns>The sum.</returns>
	/// <param name="strToEncrypt">String to encrypt.</param>
	public string Md5Sum(string strToEncrypt) {
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);

		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);

		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";

		for (int i = 0; i < hashBytes.Length; i++) {
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}

		return hashString.PadLeft(32, '0');
	}

	// Use this for initialization
	void Start () {
		LoadUserData ();
		LoadNekoMaster ();
	}

	// Update is called once per frame
	void Update () {

	}

	//------------------------------------------------------------------------------
	// シングルトン設定
	//------------------------------------------------------------------------------
	private static DataManager _instance = null;
	public static DataManager Instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<DataManager>();
				DontDestroyOnLoad(_instance.gameObject);
			}

			return _instance;
		}
	}

	void Awake() {
		if(_instance == null) {
			_instance = this;
			DontDestroyOnLoad(this);
		} else {
			if(this != _instance) Destroy(this.gameObject);
		}

		QualitySettings.vSyncCount = 0; // VSyncをOFFにする
		Application.targetFrameRate = DEFINE.GAME_FPS; // ターゲットフレームレートを60に設定
	}
}
