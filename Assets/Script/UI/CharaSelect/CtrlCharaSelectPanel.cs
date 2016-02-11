using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CtrlCharaSelectPanel : MonoBehaviour {

	//------------------------------------------------------------------------------
	// インスペクター設定
	//------------------------------------------------------------------------------
	//タイトル
	public Text m_titleText;
	//シェアボタンテキスト
	public Text m_shareButtonText;
	//設定ボタンテキスト
	public Text m_setButtonText;

	//------------------------------------------------------------------------------
	// ボタンイベント
	//------------------------------------------------------------------------------
	/// <summary>
	/// キャラ選択ボタン押下イベント
	/// </summary>
	public void OnClickCharaSetButton () {
		Debug.Log ("OnClickCharaSetButton");
		UIManager.Instance.ShowMenuPanel ();
	}

	/// <summary>
	/// 共有ボタン押下イベント
	/// </summary>
	public void OnClickShareButton () {
		Debug.Log ("OnClickShareButton");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
