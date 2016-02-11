using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

//ユーザ情報
[System.Serializable]
public class TUserNeko {
	public int nekoId;   			//ねこID
}

public class DataUserNeko : MonoBehaviour {
	// ====================================================================================
	// 以下シングルトン宣言
	private static DataUserNeko instance = null;
	public static DataUserNeko Instance {
		get{ return DataUserNeko.instance; }
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