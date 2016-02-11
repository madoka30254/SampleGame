using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	//------------------------------------------------------------------------------
	// インスペクター設定
	//------------------------------------------------------------------------------
	public GameObject m_goGamePanel;
	public GameObject m_goMenuPanel;
	public GameObject m_goCharaSelectPanel;
	public GameObject m_goSettingPanel;

	public CtrlWaveRoot m_csCtrlWaveRoot = null;

	//------------------------------------------------------------------------------
	// パラメーター
	//------------------------------------------------------------------------------
	public enum VIEW_TYPE {
		NONE		 ,
		MENU		 ,
		GAME		 ,
		CHARA_SELECT ,
		SETTING      ,
		MAX      	 ,
	}

	private VIEW_TYPE m_eViewType = VIEW_TYPE.NONE;
	private VIEW_TYPE m_eViewTypePre = VIEW_TYPE.MAX;

	private bool m_bUISwitchFlg = false;

	//------------------------------------------------------------------------------
	// UI切り替え処理
	//------------------------------------------------------------------------------
	/// <summary>
	/// ゲーム画面の表示
	/// </summary>
	public void ShowGamePanel () {
		if (m_eViewType != VIEW_TYPE.GAME) {
			m_goGamePanel.SetActive (true);
			m_goMenuPanel.SetActive (false);
			m_goCharaSelectPanel.SetActive (false);
			m_goSettingPanel.SetActive (false);
			m_eViewType = VIEW_TYPE.GAME;
		}
	}

	/// <summary>
	/// メニュー画面の表示
	/// </summary>
	public void ShowMenuPanel () {
		if (m_eViewType != VIEW_TYPE.MENU) {
			m_goGamePanel.SetActive (true);
			m_goMenuPanel.SetActive (true);
			m_goCharaSelectPanel.SetActive (false);
			m_goSettingPanel.SetActive (false);
			m_eViewType = VIEW_TYPE.MENU;

			//ブロック初期化処理
			m_csCtrlWaveRoot.Init ();
		}
	}

	/// <summary>
	/// キャラ選択画面の表示
	/// </summary>
	public void ShowCharaSelectPanel () {
		if (m_eViewType != VIEW_TYPE.CHARA_SELECT) {
			m_goGamePanel.SetActive (true);
			m_goMenuPanel.SetActive (true);
			m_goCharaSelectPanel.SetActive (true);
			m_goSettingPanel.SetActive (false);
			m_eViewType = VIEW_TYPE.CHARA_SELECT;
		}
	}

	/// <summary>
	/// 設定画面の表示
	/// </summary>
	public void ShowSettingPanel () {
		if (m_eViewType != VIEW_TYPE.SETTING) {
			m_goGamePanel.SetActive (true);
			m_goMenuPanel.SetActive (true);
			m_goCharaSelectPanel.SetActive (false);
			m_goSettingPanel.SetActive (true);
			m_eViewType = VIEW_TYPE.SETTING;
		}
	}

	//------------------------------------------------------------------------------
	// VIEWの判定処理
	//------------------------------------------------------------------------------
	public bool IsUISwitch () {
		return m_bUISwitchFlg;
	}
	public bool IsMenuView () {
		return (m_eViewType == VIEW_TYPE.MENU);
	}

	public bool IsGameView () {
		return (m_eViewType == VIEW_TYPE.GAME);
	}

	public bool IsCharaSelectView () {
		return (m_eViewType == VIEW_TYPE.CHARA_SELECT);
	}

	public bool IsSettingView () {
		return (m_eViewType == VIEW_TYPE.SETTING);
	}

	// Use this for initialization
	void Start () {
		ShowMenuPanel ();
	}
	
	// Update is called once per frame
	void Update () {
		m_bUISwitchFlg = false;
		if( m_eViewTypePre != m_eViewType ){
			m_eViewTypePre  = m_eViewType;
			m_bUISwitchFlg = true;
		}
	}

	//------------------------------------------------------------------------------
	// シングルトン設定
	//------------------------------------------------------------------------------
	private static UIManager _instance = null;
	public static UIManager Instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<UIManager>();
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
	}


}
