using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (CtrlAnimation))]
public class CtrlRideObject : MonoBehaviour {

	public enum OBJECT_SIZE {
		SIZE1       ,
		SIZE2       ,
		SIZE3       ,
		MAX         ,
	}

	public enum OBJECT_COLOR {
		WHITE       ,
		RED       	,
		GREEN       ,
		YELLOW      ,
		RAINBOW     ,
		MAX         ,
	}

	int[] ScoreTable = new int[]{
		3,2,1
	};

	//------------------------------------------------------------------------------
	// インスペクター設定
	//------------------------------------------------------------------------------
	public OBJECT_SIZE m_eObjectSize = OBJECT_SIZE.SIZE1;

	//一番最初の島か？
	public bool m_bStartObject = false; 

	//------------------------------------------------------------------------------
	// パラメーター
	//------------------------------------------------------------------------------
	private enum STEP {
		NONE				,
		LANDING_ANIMATION   , //着地アニメーション
		REMOVE_ANIMATION    , //削除アニメーション
		MAX         		,
	}

//	private STEP m_eStep = STEP.NONE;
//	private STEP m_eStepPre;
//	private float m_fTimer;

	private List<GameObject> rideList = new List<GameObject>(); //床に乗ってるオブジェクト

	public CtrlWave m_csCtrlWave = null;

	private bool m_bNekoRideFlg = false;
	private bool m_bNekoRideFlgPre = false;

	private OBJECT_COLOR m_eObjectColor = OBJECT_COLOR.WHITE;

	/// <summary>
	/// Raises the collision enter event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Etoneko")) {
			//床の上に乗ったオブジェクトを保存
			rideList.Add(other.gameObject);
			m_bNekoRideFlg = true;
		}
	}

	/// <summary>
	/// Raises the collision exit event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnCollisionExit(Collision other) {
		if (other.gameObject.CompareTag ("Etoneko")) {
			//床から離れたので削除
			rideList.Remove (other.gameObject);
			m_bNekoRideFlg = false;
		}
	}

	public void Init (CtrlWave _ctrlWave) {
		m_csCtrlWave = _ctrlWave;
		m_bNekoRideFlg = false;
		m_bNekoRideFlgPre = false;

//		SetObjectColor ();

		CtrlAnimation csAnimation = GetComponent<CtrlAnimation> ();
		csAnimation.StartHuwaHuwaAnimation ();
	}

	/// <summary>
	/// ブロックオブジェクトの色をランダムで設定
	/// </summary>
	private void SetObjectColor() {
		int colorCount = (int)OBJECT_COLOR.MAX + 1;
		int colorIndex = UnityEngine.Random.Range (0,colorCount);
		m_eObjectColor = (OBJECT_COLOR)colorIndex;

		Color setColor = Color.white;

		switch( m_eObjectColor ) {
		case OBJECT_COLOR.WHITE:
			break;
		case OBJECT_COLOR.RED:
			setColor = Color.red;
			break;
		case OBJECT_COLOR.GREEN:
			setColor = Color.green;
			break;
		case OBJECT_COLOR.YELLOW:
			setColor = Color.yellow;
			break;
		case OBJECT_COLOR.RAINBOW:
			CtrlAnimation csAnimation = GetComponent<CtrlAnimation> ();
			csAnimation.StartRainbowAnimation ();
			return;
		default:
			break;
		}

		Renderer rend = GetComponent<Renderer>();
		rend.material.color = setColor;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if( m_bNekoRideFlg != m_bNekoRideFlgPre ){
			m_bNekoRideFlgPre  = m_bNekoRideFlg;
			bInit = true;
		}

		if (bInit) {
			if (m_bNekoRideFlg) {
				//ねこが乗った時のイベント検知
				if (m_csCtrlWave != null) {
					m_csCtrlWave.NekoRideOnEvent (this);
				}
				//最初のオブジェクトの場合、ゲーム準備完了通知
				if (m_bStartObject) {
					GameManager.Instance.GameStart ();
				} else {
					//その他はスコア加算
					GameManager.Instance.AddCurrentScore (ScoreTable[(int)m_eObjectSize]);
				}
			} else {
				//ねこ去った時のイベント検知(未使用)
				if (m_csCtrlWave != null) {
//					m_csCtrlWave.NekoRideOffEvent ();
				}
			}
		}

		//上に乗ったオブジェクトの強制移動処理
		if (rideList.Count > 0) {
			if (m_csCtrlWave != null) {
				foreach (GameObject go in rideList) {
//					Vector3 v = go.transform.position;
//					g.transform.position = new Vector3(v.x + x, v.y, v.z + z);	//yの移動は不要
					if (go != null) {
						go.transform.Translate(m_csCtrlWave.m_vecWaveTranslate);
					}
				}
			}
		}
	}
}
