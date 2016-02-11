using UnityEngine;
using System;
using System.Collections;
using System.Globalization;

[System.Serializable]
public class TNeko {
	// ねこマスターデータに必要なデータ
	public int nekoId;
	public string nekoName;
	public int rare;
}

public class DataNeko : MonoBehaviour {
	// ====================================================================================
	// 以下シングルトン宣言
	private static DataNeko instance = null;
	public static DataNeko Instance {
		get{ return DataNeko.instance; }
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