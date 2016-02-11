using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

//ユーザ情報
[System.Serializable]
public class TUser {
	public int highScore;			//ハイスコア
	public int currentScore;		//現在のスコア
	public int currentCatId;		//設定中のねこID
	public int fishCount;			//保有魚数
	public List<TUserNeko> userCatList = new List<TUserNeko> ();
}

public class DataUser : MonoBehaviour {
	// ====================================================================================
	// 以下シングルトン宣言
	private static DataUser instance = null;
	public static DataUser Instance {
		get{ return DataUser.instance; }
	}

	void Awake(){
		if(instance == null){
			instance = this;
		}
		else {
			Destroy(this);
		}
	}

	// Use this for initialization
	void Start () {
		// 自害させない
		DontDestroyOnLoad(this);			// 削除させない
		this.enabled = false;				// アップデートさせない
	}

	// Update is called once per frame
	void Update () {

	}
}