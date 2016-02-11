using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlEtoneko : MonoBehaviour {

	//------------------------------------------------------------------------------
	// インスペクター設定
	//------------------------------------------------------------------------------
	public GameObject m_goJumpRoot = null;
	public BoxCollider m_coJumpRoot = null;

	//------------------------------------------------------------------------------
	// パラメーター
	//------------------------------------------------------------------------------
	private enum STEP {
		NONE		,
		INIT		,
		IDLE		,
		JUMP		,
		DEAD		,
		MAX         ,
	}

	[SerializeField]
	private STEP m_eStep = STEP.NONE;
	[SerializeField]
	private STEP m_eStepPre;
	private float m_fTimer;

	//ジャンプアニメーション関連
	private const float MOVE_Z_POS = -2f;
//	private int m_iJumpCount = 0;
	private const float MOVE_TIME = 0.3f;
	private const float JUMP_Y = 0.6f;
	private const float JUMP_TIME = 0.08f;
	private float startTime;
	private Vector3 startPosition;
	private Vector3	endPosition;
	public CtrlAnimation m_csCtrlAnimation = null;

	private float NEKO_DEAD_LINE = -1f;

	//------------------------------------------------------------------------------
	// タッチイベントの取得設定
	//------------------------------------------------------------------------------
	public void OnEnable () {
		TouchManager.Instance.TouchStart += OnTouchStart;
	}

	public void OnDisable () {
//		TouchManager.Instance.TouchStart -= OnTouchStart;
	}

	void OnTouchStart (object sender, CustomInputEventArgs e)
	{
//		string text = string.Format ("OnTouchStart X={0} Y={1}", e.Input.ScreenPosition.x, e.Input.ScreenPosition.y);
//		Debug.Log (text);
		if (m_eStep == STEP.IDLE) {
			m_eStep = STEP.JUMP;
		}
	}

	public void Init () {
		m_eStep = STEP.INIT;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		bool bInit = false;
		if( m_eStepPre != m_eStep ){
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch( m_eStep )
		{
		case STEP.NONE:
			break;
		case STEP.INIT:
			if (bInit) {

			}

			if (GameManager.Instance.IsGameStart()) {
				m_eStep = STEP.IDLE;
			}

			break;
		case STEP.IDLE:
			if (bInit) {

			}

			//ねこの死亡判定
			if (m_goJumpRoot.transform.localPosition.y < NEKO_DEAD_LINE) {
				m_eStep = STEP.DEAD;
				GameManager.Instance.GameOver ();
			}

			break;
		case STEP.JUMP:
			if (bInit) {
				startTime = Time.timeSinceLevelLoad;
				startPosition = transform.localPosition;
				endPosition = new Vector3 (startPosition.x,startPosition.y,startPosition.z+MOVE_Z_POS);

				if (m_csCtrlAnimation != null) {
//					Vector3 jumpPosition = new Vector3 (startPosition.x,startPosition.y+MOVE_Z_POS,startPosition.z);
					m_csCtrlAnimation.StartJumpAnimation (JUMP_Y, JUMP_TIME, true);
				}

			}

			var diff = Time.timeSinceLevelLoad - startTime;
			if (diff > MOVE_TIME) {
				transform.localPosition = endPosition;
				m_eStep = STEP.IDLE;
			}

			var rate = diff / MOVE_TIME;
			transform.localPosition = Vector3.Lerp (startPosition, endPosition, rate);

			break;
		case STEP.DEAD:
			if (bInit) {
				if (m_coJumpRoot != null) {
					m_coJumpRoot.enabled = false;
				}
			}

			break;
		case STEP.MAX:
		default:
			break;
		}
	}
}
