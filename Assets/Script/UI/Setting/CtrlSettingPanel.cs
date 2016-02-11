using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CtrlSettingPanel : MonoBehaviour {

	//------------------------------------------------------------------------------
	// インスペクター設定
	//------------------------------------------------------------------------------
	//タイトル
	public Text m_titleText;
	//BGM ON/OFFボタンテキスト
	public Text m_bgmButtonText;
	//SE ON/OFFボタンテキスト
	public Text m_seButtonText;
	//閉じるボタンテキスト
	public Text m_closeButtonText;

	//------------------------------------------------------------------------------
	// ボタンイベント
	//------------------------------------------------------------------------------
	/// <summary>
	/// BGM ON/OFF ボタン押下イベント
	/// </summary>
	public void OnClickSetBGMButton () {
		Debug.Log ("OnClickSetBGMButton");
	}

	/// <summary>
	/// SE ON/OFF ボタン押下イベント
	/// </summary>
	public void OnClickSetSEButton () {
		Debug.Log ("OnClickSetSEButton");
	}

	/// <summary>
	/// CLOSE ボタン押下イベント
	/// </summary>
	public void OnClickCloseButton () {
		Debug.Log ("OnClickCloseButton");
		UIManager.Instance.ShowMenuPanel ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
