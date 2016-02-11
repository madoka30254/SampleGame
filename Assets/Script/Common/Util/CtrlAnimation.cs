using UnityEngine;
using System.Collections;

public class CtrlAnimation : MonoBehaviour {

	//------------------------------------------------------------------------------
	// パラメーター
	//------------------------------------------------------------------------------
	public enum ANIMATION_TYPE {
		NONE		 ,
		JUMP		 ,		//ジャンプアニメーション
		HUWAHUWA_LOOP,		//水面ふわふわループアニメーション
		MAX          ,
	}

	public enum ANIMATION_STATUS {
		NONE		 ,
		IDLE		 ,
		START		 ,
		END			 ,
		MAX          ,
	}

	public ANIMATION_TYPE m_eAnimationType;
	public ANIMATION_STATUS m_eAnimationStatus;

	//------------------------------------------------------------------------------
	// ふわふわアニメパラメーター
	//------------------------------------------------------------------------------
	public float amplitude = 0.01f; // 振幅
	private int frameCnt = 0; // フレームカウント

	void FixedUpdate () {

		switch (m_eAnimationType) {
		case ANIMATION_TYPE.HUWAHUWA_LOOP:
			frameCnt += 1;
//			if( 10000 <= frameCnt ){
			if( 5000 <= frameCnt ){
				frameCnt = 0;
			}
			if( 0 == frameCnt%2 ){
				// 上下に振動させる（ふわふわを表現）
				float posYSin = Mathf.Sin(2.0f*Mathf.PI*(float)(frameCnt%200)/(200.0f-1.0f));
				iTween.MoveAdd(gameObject,new Vector3(0, amplitude * posYSin, 0),0.0f);
			}

			break;
		default:
			break;
		}
	}

	//------------------------------------------------------------------------------
	// アニメーション制御
	//------------------------------------------------------------------------------
	//ジャンプ
	public void StartJumpAnimation (float _toY,float _time,bool _isLocal = true) {
		iTween.MoveTo(gameObject, iTween.Hash("y", _toY,"time", _time, "islocal", _isLocal));
	}

	//ふわふわ
	public void StartHuwaHuwaAnimation() {
		m_eAnimationType = ANIMATION_TYPE.HUWAHUWA_LOOP;

	}

	//虹色に色変える
	public void StartRainbowAnimation() {
		// オブジェクトの初期色は赤
		Renderer rend = GetComponent<Renderer>();
		rend.material.color = Color.red;
		iTween.ColorTo (gameObject,iTween.Hash("color", Color.blue, "time", 1f,"looptype", iTween.LoopType.pingPong));
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
