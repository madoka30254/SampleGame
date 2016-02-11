using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	//------------------------------------------------------------------------------
	// インスペクター設定
	//------------------------------------------------------------------------------
	public GameObject m_goPlayerRoot = null;
//	public CameraFollow m_csCameraFollow = null;
	public List<CameraFollow> m_csCameraFollow = new List<CameraFollow>();

	//------------------------------------------------------------------------------
	// パラメーター
	//------------------------------------------------------------------------------
	private enum STEP {
		IDLE		,
		GAME_INIT	,
		GAME_START	,
		GAME_END	,
		MAX         ,
	}

	[SerializeField]
	private STEP m_eStep;
	[SerializeField]
	private STEP m_eStepPre;
	private float m_fTimer;

	//ねこのスタート初期位置
	private Vector3 m_nekoStartPosition = new Vector3(0f,0f,0f);

	//生成したねこオブジェクト
	private GameObject m_goNeko = null;

	/// <summary>
	/// ゲームの開始準備
	/// </summary>
	public bool GameInit() {
		if (m_eStep == STEP.IDLE || m_eStep == STEP.GAME_END) {
			m_eStep = STEP.GAME_INIT;
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// ゲームの開始
	/// </summary>
	public bool GameStart() {
		if (m_eStep == STEP.GAME_INIT) {
			m_eStep = STEP.GAME_START;
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// ゲームオーバー
	/// </summary>
	public bool GameOver() {
		if (m_eStep == STEP.GAME_START) {
			m_eStep = STEP.GAME_END;
			return true;
		} else {
			return false;
		}
	}

	//ゲーム開始待ちフラグ
	public bool IsGameIdle () {
		return (m_eStep == STEP.IDLE);
	}

	//ゲーム開始フラグ
	public bool IsGameStart () {
		return (m_eStep == STEP.GAME_START);
	}

	//ゲームオーバーフラグ
	public bool IsGameOver () {
		return (m_eStep == STEP.GAME_END);
	}

	//スコアの初期化
	public void ResetCurrentScore () {
		DataManager.user.currentScore = 0;
	}

	//スコアの更新
	public void AddCurrentScore (int _score) {
		DataManager.user.currentScore += _score;
	}

	// Use this for initialization
	void Start () {
		m_eStep = STEP.IDLE; 
		m_eStepPre = STEP.MAX;
	}
	
	// Update is called once per frame
	void Update () {
		bool bInit = false;
		if( m_eStepPre != m_eStep ){
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.IDLE:
			if (bInit) {

			}

			break;
		case STEP.GAME_INIT:
			if (bInit) {
				if (m_goNeko != null) {
					Destroy (m_goNeko);
				}

				// プレハブを取得
				GameObject prefab = (GameObject)Resources.Load ("Model/Etoneko");
				// プレハブからインスタンスを生成
				m_goNeko = Instantiate (prefab) as GameObject;
				m_goNeko.transform.parent = m_goPlayerRoot.transform;
				m_goNeko.transform.localPosition = m_nekoStartPosition;
				CtrlEtoneko csEtoneko = m_goNeko.GetComponent<CtrlEtoneko> ();
				if (csEtoneko != null) {
					csEtoneko.Init ();
					GameManager.Instance.ResetCurrentScore ();
					//ゲームのスタートは、ねこが一番最初の島に着地した時に開始
//					m_eStep = STEP.GAME_START;
				} else {
					Debug.LogError ("Game初期化失敗");
				}
			}

			break;
		case STEP.GAME_START:
			if (bInit) {
				for (int i = 0; i < m_csCameraFollow.Count;i++) {
//					m_csCameraFollow.SetTargetTransform (m_goNeko.transform);
					m_csCameraFollow[i].SetTargetTransform (m_goNeko.transform);
				}
			}

			break;
		case STEP.GAME_END:
			if (bInit) {
				//ハイスコアの更新
				if (DataManager.user.currentScore > DataManager.user.highScore) {
					DataManager.user.highScore = DataManager.user.currentScore;
				}
				//ユーザデータの保存
				DataManager.Instance.SaveUserData (DataManager.user);
				//メニュー画面表示
				UIManager.Instance.ShowMenuPanel ();
				//カメラに追従解除
				for (int i = 0; i < m_csCameraFollow.Count;i++) {
//					m_csCameraFollow.RemoveTargetTransform ();
					m_csCameraFollow[i].RemoveTargetTransform ();
				}
			}

			break;
		default:
			break;
		}
	}

	//------------------------------------------------------------------------------
	// シングルトン設定
	//------------------------------------------------------------------------------
	private static GameManager _instance = null;
	public static GameManager Instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<GameManager>();
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
