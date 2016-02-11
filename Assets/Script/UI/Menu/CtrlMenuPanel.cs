using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CtrlMenuPanel : MonoBehaviour {

	//------------------------------------------------------------------------------
	// インスペクター設定
	//------------------------------------------------------------------------------
	//タイトル
	public Text m_titleText;
	//スコア
	public Text m_scoreNumText;
	//スコアラベル
	public Text m_scoreText;
	//ベストスコア
	public Text m_bestScoreNumText;
	//ベストスコアラベル
	public Text m_bestScoreText;

	//------------------------------------------------------------------------------
	// ボタンイベント
	//------------------------------------------------------------------------------
	/// <summary>
	/// プレイボタン押下イベント
	/// </summary>
	public void OnClickPlayButton () {
		if (GameManager.Instance.GameInit ()) {
			UIManager.Instance.ShowGamePanel ();
		} else {
			Debug.LogError ("GameManagerがIDLE状態じゃない");
		}
	}

	/// <summary>
	///  キャラ選択ボタン押下イベント
	/// </summary>
	public void OnClickCharaSelectButton () {
		Debug.Log ("OnClickCharaSelectButton");
		UIManager.Instance.ShowCharaSelectPanel ();
	}

	/// <summary>
	/// 設定ボタン押下イベント
	/// </summary>
	public void OnClickSettingButton () {
		Debug.Log ("OnClickSettingButton");
		UIManager.Instance.ShowSettingPanel ();
	}

	/// <summary>
	/// ゲームセンターボタン押下イベント
	/// </summary>
	public void OnClickGameCenterButton () {
		Debug.Log ("OnClickGameCenterButton");
		//TODO ゲームセンター実装



	}



	// Use this for initialization
	void Start () {
		m_titleText.text = LocalizeManager.GetText ("app_name");
		m_scoreText.text = LocalizeManager.GetText ("score_title");
		m_bestScoreText.text = LocalizeManager.GetText ("best_score_title");
	}
	
	// Update is called once per frame
	void Update () {
		m_scoreNumText.text = DataManager.user.currentScore.ToString ();
		m_bestScoreNumText.text = DataManager.user.highScore.ToString ();
	}
}
