using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlWaveRoot : MonoBehaviour {

	//------------------------------------------------------------------------------
	// インスペクター設定
	//------------------------------------------------------------------------------
	public GameObject m_prefabWave;

	//------------------------------------------------------------------------------
	// パラメーター
	//------------------------------------------------------------------------------
	private const int CREATE_WAVE_NUM = 10;
	private const float CREATE_WAVE_Z_POS = -2;

	public List<CtrlWave> m_createWaveList = new List<CtrlWave>();

	private enum STEP {
		NONE		,
		INIT		,
		IDLE		,
		MAX         ,
	}

//	[SerializeField]
//	private STEP m_eStep = STEP.NONE;
//	[SerializeField]
//	private STEP m_eStepPre;
//	private float m_fTimer;

	private int m_iCurrentWaveCount = 0;
	private const int MAX_WAVE_COUNT = 8;
//	private Vector3 m_startPosition = new Vector3(-12f,0f,0f);

	public bool n_bInitFlg = false;

	public void Init() {
		if (n_bInitFlg) {
			return;
		}

		n_bInitFlg = true;

		if (m_createWaveList.Count > 0) {
			for (int i = 0; i < m_createWaveList.Count;i++) {
				Destroy(m_createWaveList[i].gameObject);
			}
			m_createWaveList = new List<CtrlWave> ();
		}

		m_iCurrentWaveCount = 0;
		for (int i = 0; i < MAX_WAVE_COUNT;i++) {
			CreateWave (m_iCurrentWaveCount);
		}
//		m_eStep = STEP.IDLE;
	}
		
	/// <summary>
	/// Creates the wave.
	/// </summary>
	/// <param name="_waveCount">_wave count.</param>
	private void CreateWave (int _waveCount) {
		// プレハブを取得
		GameObject prefab = (GameObject)Resources.Load ("Prefab/Wave");
		// プレハブからインスタンスを生成
		GameObject go = Instantiate (prefab) as GameObject;
		go.transform.parent = transform;
		go.transform.localPosition = new Vector3 (-12f,0f,m_iCurrentWaveCount*-2);
		CtrlWave csWave = go.GetComponent<CtrlWave> ();
		if (csWave != null) {
			csWave.Init (m_iCurrentWaveCount,this);
			m_iCurrentWaveCount++;
			m_createWaveList.Add (csWave);
		}
	}

	/// <summary>
	/// Deletes the old wave.
	/// </summary>
	/// <param name="_waveCount">_wave count.</param>
	private void DeleteOldWave () {
		if (m_createWaveList.Count > MAX_WAVE_COUNT+8) {
			Destroy (m_createWaveList[0].gameObject);
			m_createWaveList.RemoveAt (0);
		}
	}

	//------------------------------------------------------------------------------
	// イベント受け取り
	//------------------------------------------------------------------------------
	/// <summary>
	/// 島にねこが乗った通知受け取り
	/// </summary>
	public void NekoRideOnEvent (CtrlRideObject _rideObj) {
		n_bInitFlg = false;
		CreateWave (m_iCurrentWaveCount);
		DeleteOldWave ();
	}
}
