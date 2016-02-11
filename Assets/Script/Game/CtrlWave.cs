using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlWave : MonoBehaviour {

	//------------------------------------------------------------------------------
	// インスペクター設定
	//------------------------------------------------------------------------------

	//島オブジェクトPrefabのリスト
	public List<GameObject> m_rideObjectList = new List<GameObject>();

	//動作対象の親オブジェクト
	public GameObject m_goMoveObject = null;

	//島削除用
	public CtrlRemover m_csCtrlLeftRemover = null;
	public CtrlRemover m_csCtrlRightRemover = null;

	//一番最初の島か？
	public bool m_bStartObject = false;

	//------------------------------------------------------------------------------
	// パラメーター
	//------------------------------------------------------------------------------
	private enum STEP {
		NONE				,
//		CREATE_RIDE_OBJECTS ,
		MAX         		,
	}

	[System.Serializable]
	public class TCreateBlockInfo
	{
		public int id;
		public int blockSize;
		public int spaceSize;
	}

	[SerializeField]
	private STEP m_eStep = STEP.NONE;
	[SerializeField]
	private STEP m_eStepPre;
	private float m_fTimer;

	[SerializeField]
	private int m_iWaveCount;

	public List<GameObject> m_createObjectList = new List<GameObject>();
	private int m_iCreateObjectCount = 0;

	private CtrlWaveRoot m_csCtrlWaveRoot = null;

	public Vector3 m_vecWaveTranslate = new Vector3();

	//現在のブロック位置
	public int m_iCurrentBlockPosistion = 0;

	//Remover間の最大ブロック数
	private const int MAX_BLOCK_COUNT = 27;
	//一番大きいブロックのサイズ
	private const int MAX_BLOCK_SIZE = 3;

	//ランダム生成したブロック数のリスト
	public List<TCreateBlockInfo> m_iCreateBlockInfoList = new List<TCreateBlockInfo>();

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Init (int _waveCount,CtrlWaveRoot _waveRoot) {
		//初期生成
		for (int i = 0; i < 999; i++) {
			if (!CreateRideObject ()) {
				break;
			}
		}

		m_csCtrlWaveRoot = _waveRoot;
			
		m_iCurrentBlockPosistion = GetTotalBlockBlockCount () - 1;

		m_iWaveCount = _waveCount;
		if (m_iWaveCount % 2 == 0) {
			m_iCurrentBlockPosistion = 0;
		}
	}

	//------------------------------------------------------------------------------
	// イベント受け取り
	//------------------------------------------------------------------------------
	/// <summary>
	/// 島にねこが乗った通知受け取り
	/// </summary>
	public void NekoRideOnEvent (CtrlRideObject _rideObj) {
		if (m_csCtrlWaveRoot != null) {
			m_csCtrlWaveRoot.NekoRideOnEvent (_rideObj);
		}
	}

	/// <summary>
	/// 島にねこが去った通知受け取り
	/// </summary>
	public void NekoRideOffEvent () {
//		Debug.LogError ("NekoRideOffEvent");
	}

	/// <summary>
	/// 島の削除通知受け取り
	/// </summary>
	public void RemoveRideObject () {
		//TODO ID渡して消しこんだ方がよさげ？
		m_iCreateBlockInfoList.RemoveAt (0);
		CreateRideObject ();
	}

	/// <summary>
	/// 島の生成
	/// </summary>
	public bool CreateRideObject () {
		//生成する必要があるか？
		if (!CheckBlockCount()) {
			return false;
		}

		//ランダムブロックの作成
		TCreateBlockInfo createBlockInfo = CreateRandamBlockInfo ();
		GameObject clone = Instantiate(m_rideObjectList[createBlockInfo.blockSize-1]) as GameObject;
		clone.transform.parent = m_goMoveObject.transform;
		CtrlRideObject csCtrlRideObject = clone.GetComponent<CtrlRideObject> ();
		if (csCtrlRideObject != null) {
			csCtrlRideObject.Init (this);
		}

		//右方向の場合
		if (m_iWaveCount % 2 == 0) {
			m_iCurrentBlockPosistion-= createBlockInfo.blockSize;
			clone.transform.localPosition = new Vector3 (m_iCurrentBlockPosistion,0f,0f);
			m_iCurrentBlockPosistion-= createBlockInfo.spaceSize;
		} else {
			m_iCurrentBlockPosistion+= createBlockInfo.blockSize;
			clone.transform.localPosition = new Vector3 (m_iCurrentBlockPosistion,0f,0f);
			m_iCurrentBlockPosistion+= createBlockInfo.spaceSize;
		}

		m_iCreateBlockInfoList.Add (createBlockInfo);
		m_iCreateObjectCount++;
		return true;
	}

	/// <summary>
	/// 現在のRemover間のオブジェクト量を取得
	/// </summary>
	/// <returns><c>true</c>, if block count was checked, <c>false</c> otherwise.</returns>
	private int GetTotalBlockBlockCount () {
		int blockCount = 0;
		for (int i = 0; i < m_iCreateBlockInfoList.Count;i++) {
			blockCount = m_iCreateBlockInfoList [i].blockSize + m_iCreateBlockInfoList [i].spaceSize + blockCount;
		}
		return blockCount;
	}

	/// <summary>
	/// ブロックを生成する必要があるか？
	/// </summary>
	/// <returns><c>true</c>, if block count was checked, <c>false</c> otherwise.</returns>
	private bool CheckBlockCount () {
		int blockCount = GetTotalBlockBlockCount();
		int chkCount = MAX_BLOCK_COUNT - (MAX_BLOCK_SIZE*1);
		if (chkCount > blockCount) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// ブロックとその次のスペースをランダムで作成
	/// </summary>
	/// <returns>The randam block info.</returns>
	private TCreateBlockInfo CreateRandamBlockInfo () {
		TCreateBlockInfo createBlockInfo = new TCreateBlockInfo ();
		createBlockInfo.id = 0;
		createBlockInfo.blockSize = UnityEngine.Random.Range (1,MAX_BLOCK_SIZE+1); //1~3の整数取得
		createBlockInfo.spaceSize = UnityEngine.Random.Range (1,MAX_BLOCK_SIZE+1); //1~3の整数取得
		return createBlockInfo;
	}

	/// <summary>
	/// 島の横移動
	/// </summary>
	public void WaveMove() {
		if (m_goMoveObject == null) {
			return;
		}

		if (!m_bStartObject) {
			if (m_iWaveCount % 2 == 0) {
				m_vecWaveTranslate = Vector3.right * DEFINE.WAVE_SPEED * Time.deltaTime;
				m_goMoveObject.transform.Translate(m_vecWaveTranslate);
			} else {
				m_vecWaveTranslate = Vector3.left * DEFINE.WAVE_SPEED * Time.deltaTime;
				m_goMoveObject.transform.Translate(m_vecWaveTranslate);
			}
		}
	}

	// Use this for initialization
	void Start () {
		if (!m_bStartObject) {
			if (m_iWaveCount % 2 == 0) {
				m_csCtrlRightRemover.SetCtrlWave (this);
				m_csCtrlLeftRemover.gameObject.SetActive (false);
			} else {
				m_csCtrlLeftRemover.SetCtrlWave (this);
				m_csCtrlRightRemover.gameObject.SetActive (false);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		bool bInit = false;
		if( m_eStepPre != m_eStep ){
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch( m_eStep ) {
		case STEP.NONE:
			if (bInit) {

			}
			break;
//		case STEP.CREATE_RIDE_OBJECTS:
//			if (bInit) {
//				m_fTimer = 0f;
//			}
//
//			m_fTimer += Time.deltaTime;
//
//			break;
		case STEP.MAX:
		default:
			break;
		}

		//島の移動
		WaveMove ();

		return;
	}
}
