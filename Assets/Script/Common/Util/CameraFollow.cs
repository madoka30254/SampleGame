using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	//------------------------------------------------------------------------------
	// インスペクター設定
	//------------------------------------------------------------------------------
	public Transform target = null;
	public float smoothing = 0.1f;
	Vector3 offset;
	Vector3 initCameraPos;

	/// <summary>
	/// Sets the target transform.
	/// </summary>
	/// <param name="_targetTransform">_target transform.</param>
	public void SetTargetTransform (Transform _targetTransform) {
		target = _targetTransform;
		offset = transform.position - target.position;
	}

	/// <summary>
	/// Removes the target transform.
	/// </summary>
	/// <param name="_targetTransform">_target transform.</param>
	public void RemoveTargetTransform () {
		target = null;
		gameObject.transform.localPosition = initCameraPos;
	}

	void Start() {
		initCameraPos = gameObject.transform.localPosition;
	}

	void FixedUpdate() {
		if (target != null) {
			Vector3 targetCamPos = target.position + offset;
			transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
		}
	}
}