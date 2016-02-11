using UnityEngine;
using System.Collections;
using System.Collections.Generic;			// リスト使うなら追加しないといけないっ！
using System;

public class DEFINE : MonoBehaviour {

	//------------------------------------------------------------------------------
	// アプリパラメーター
	//------------------------------------------------------------------------------
	public const string APP_NAME = "ねこジャンプ";
	public const string APP_VERSION = "1.0.0";
	public const int GAME_FPS = 60;

	public const string USER_DATA_PREFS_KEY1 = "e4ifja0gvhinva32y";
	public const string USER_DATA_PREFS_KEY2 = "bkir8d72jgsoif8ei";

	// PlayerPrefsで使うKey
	public const string PLAYREPREFS_KEY_UID  = "playerprefs_uid";

	//------------------------------------------------------------------------------
	// ゲームパラメーター
	//------------------------------------------------------------------------------
	public const float WAVE_SPEED = 2f; //足場(島)の移動スピード
//	public const float WAVE_X_POSITION = 15f; //足場移動範囲


}
