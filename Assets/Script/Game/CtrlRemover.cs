using UnityEngine;
using System.Collections;

public class CtrlRemover : MonoBehaviour {

	//------------------------------------------------------------------------------
	// パラメーター
	//------------------------------------------------------------------------------
	private CtrlWave m_csCtrlWave = null;

	public void SetCtrlWave (CtrlWave _csCtrlWave) {
		m_csCtrlWave = _csCtrlWave;
	}

	void OnTriggerEnter(Collider other) {
		if (m_csCtrlWave != null) {
			m_csCtrlWave.RemoveRideObject ();
		}

		if (!other.gameObject.CompareTag("Etoneko")) {
			Destroy(other.gameObject);
		}
	}

}
