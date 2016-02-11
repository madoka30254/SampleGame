using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class SOUND {
	public enum BGM : int {
		MAIN 			= 0,
	}

	public enum SE : int {
		BUTTON_PUSH		= 0	,
		STAR_GET			,

		MAX 				,
	}
}
 
// 音管理クラス
public class SoundManager : MonoBehaviour {

	//------------------------------------------------------------------------------
	// シングルトン設定
	//------------------------------------------------------------------------------
	private static SoundManager _instance = null;
	public static SoundManager Instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<SoundManager>();
				DontDestroyOnLoad(_instance.gameObject);
			}

			return _instance;
		}
	}

	// 音量
	public SoundVolume volume = new SoundVolume();

	// === AudioSource ===
 	// BGM
	private AudioSource BGMsource;
	// SE
	private AudioSource[] SEsources = new AudioSource[16];
	// 音声
	private AudioSource[] VoiceSources = new AudioSource[16];
  
	// === AudioClip ===
	// BGM
	public AudioClip[] BGM;
	// SE
	public AudioClip[] SE;
	// 音声
	public AudioClip[] Voice;

	void Awake (){
		if(_instance == null) {
			_instance = this;
			DontDestroyOnLoad(this);
		} else {
			if(this != _instance) Destroy(this.gameObject);
		}

		// BGM AudioSource
		BGMsource = gameObject.AddComponent<AudioSource>();
		// BGMはループを有効
		BGMsource.loop = true;

		// SE AudioSource
		for(int i = 0 ; i < SEsources.Length ; i++ ){
			SEsources[i] = gameObject.AddComponent<AudioSource>();
		}

		// 音声 AudioSource
		for(int i = 0 ; i < VoiceSources.Length ; i++ ){
			VoiceSources[i] = gameObject.AddComponent<AudioSource>();
		}
	}

	void Update () {
		// ミュート設定
		BGMsource.mute = volume.Mute;
		foreach(AudioSource source in SEsources ){
			source.mute = volume.Mute;
		}
		foreach(AudioSource source in VoiceSources ){
			source.mute = volume.Mute;
		}

		// ボリューム設定
		BGMsource.volume = volume.BGM;
		foreach(AudioSource source in SEsources ){
			source.volume = volume.SE;
		}
		foreach(AudioSource source in VoiceSources ){
			source.volume = volume.Voice;
		}

		bgm_ctrl();

		return;
	}

	public int m_intBgmStatus = 0;
	public int m_intBgmStatusPre = 0;
	public float m_fBgmSave;
	public void BgmFadeout(){
		m_fBgmSave = volume.BGM;
		m_intBgmStatus = 1;
		return;
	}

	public void BgmFadein(){
		m_intBgmStatus = 2;
		return;
	}

	private void bgm_ctrl(){

		switch( m_intBgmStatus )
		{
		case 0:
			break;

		case 1:
			if( 0.0f < volume.BGM ){
				volume.BGM -= Time.deltaTime;
			}
			else {
				volume.BGM = 0.0f;
				m_intBgmStatus = 0;
			}
			break;
			
		case 2:
			if( volume.BGM < m_fBgmSave){
				volume.BGM += Time.deltaTime;
			}
			else {
				volume.BGM = m_fBgmSave;
				m_intBgmStatus = 0;
			}
			break;

		default:
			break;

		}
	}

	// ***** BGM再生 *****
	// BGM再生
	public void PlayBGM( SOUND.BGM _eBGM ){
		int index = (int)_eBGM;
		if( 0 > index || BGM.Length <= index ){
			return;
		}
		// 同じBGMの場合は何もしない
		if( BGMsource.clip == BGM[index] ){
			return;
		}
		BGMsource.Stop();
		BGMsource.clip = BGM[index];
		BGMsource.Play();
		return;
	}

	// BGM停止
	public void StopBGM(){
		BGMsource.Stop();
		BGMsource.clip = null;
		return;
	}

	// ***** SE再生 *****
	// SE再生
	public int PlaySE(SOUND.SE _eSE , bool _bIsLoop = false , bool _bForce = false ){
		int index = (int)_eSE;
		if( 0 > index || SE.Length <= index ){
			return -1;
		}

		int intCount = 0;
		// 再生中で無いAudioSouceで鳴らす
		foreach(AudioSource source in SEsources){
			if( false == source.isPlaying ){
				source.clip = SE[index];
				source.loop = _bIsLoop;
				source.Play();

				Debug.Log("play se ["+_eSE+"]["+intCount+"]");

				return intCount;
			}
			intCount += 1;
		}

		if( _bForce ){
			StopSE(0);
			PlaySE(_eSE,_bIsLoop,_bForce);
		}
		return -1;
	}

	public bool IsPlayingSE( int _intIndex ){
		bool bRet = false;
		if( 0 <= _intIndex && _intIndex < SEsources.Length ){
			AudioSource source = SEsources[_intIndex];
			bRet = source.isPlaying;
		}
		return bRet;
	}

	// SE停止
	public void StopSE( int _intIndex = 255 ){

		if( _intIndex == 255 ){
			// 全てのSE用のAudioSouceを停止する
			foreach(AudioSource source in SEsources){
				source.Stop();
				source.clip = null;
			}
		}
		else if( 0 <= _intIndex && _intIndex < SEsources.Length ){
			AudioSource source = SEsources[_intIndex];
			Debug.Log("stop"+_intIndex);
			source.Stop();
			source = null;
		}
		else {
			;//エラー
		}
		return;
	}

	// ***** 音声再生 *****
	// 音声再生
	public void PlayVoice(int index){
		if( 0 > index || Voice.Length <= index ){
			return;
		}
		// 再生中で無いAudioSouceで鳴らす
		foreach(AudioSource source in VoiceSources){
			if( false == source.isPlaying ){
				source.clip = Voice[index];
				source.Play();
				return;
			}
		}
	}

	// 音声停止
	public void StopVoice(){
		// 全ての音声用のAudioSouceを停止する
		foreach(AudioSource source in VoiceSources){
			source.Stop();
			source.clip = null;
		}  
	}
}

// 音量クラス
[Serializable]
public class SoundVolume{
	public float BGM = 1.0f;
	public float Voice = 1.0f;
	public float SE = 1.0f;
	public bool Mute = false;
	  
	public void Init(){
		BGM = 1.0f;
		Voice = 1.0f;
		SE = 1.0f;
		Mute = false;
	}
}