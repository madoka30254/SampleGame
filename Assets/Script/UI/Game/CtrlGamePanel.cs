using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CtrlGamePanel : MonoBehaviour {

	//------------------------------------------------------------------------------
	// インスペクター設定
	//------------------------------------------------------------------------------
	//保持魚数ラベル
	public Text m_fishNumText;

	//スコアラベル
	public Text m_scoreText;

	//表示制御対象のUIリスト
	public List<GameObject> m_goUIList = new List<GameObject>();

	//------------------------------------------------------------------------------
	// ボタンイベント
	//------------------------------------------------------------------------------
	/// <summary>
	/// デバッグ用戻るボタン押下イベント
	/// </summary>
	public void OnClickDebugBackButton () {
//		Debug.Log ("OnClickDebugBackButton");
		UIManager.Instance.ShowMenuPanel ();
	}

	/// <summary>
	/// ゲームUIの表示制御
	/// </summary>
	/// <param name="_active">If set to <c>true</c> _active.</param>
	private void SetGameUI (bool _active) {
		if (m_goUIList.Count > 0) {
			for (int i = 0; i < m_goUIList.Count;i++) {
				m_goUIList [i].SetActive (_active);
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (m_fishNumText != null) {
			m_fishNumText.text = DataManager.user.fishCount.ToString();
		}

		if (m_scoreText != null) {
			m_scoreText.text = DataManager.user.currentScore.ToString();
		}

		if (UIManager.Instance.IsUISwitch()) {
			if (UIManager.Instance.IsGameView ()) {
				SetGameUI(true);
			} else {
				SetGameUI(false);
			}
		}
	}
}
