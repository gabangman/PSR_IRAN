using UnityEngine;
using System.Collections;

public class LobbySound : MonoBehaviour {

	[SerializeField] private AudioSource _bgmSound;
	[SerializeField] private AudioSource _titleSound;
	[SerializeField] private AudioClip _bgm, _load, _complete, _bgm1;
	[SerializeField] private AudioClip _level, _season, _money, _coin,_start;
	[SerializeField] private AudioClip _attend, _attendStamp;
	bool isbgmPlay, istitlePlay;

	void LobbyBGMInit(){

		//return;
		var temp = gameObject.AddComponent<GameOption>() as GameOption;
		GameOption.OptionSetting _opt = GameOption.OptionData;
		isbgmPlay  = _opt.isBGM;
		if(_opt.isBGM){
			if(!Global.Loading){
				_titleSound.Play();
			}else{
				_titleSound.Stop();
				//_titleSound.PlayOneShot(_load);
			}
			//Utility.Log("tt");
		}else{
			_titleSound.Stop();
		}
		_bgmSound.Stop();
		float uiVolume = _opt.isEffect?0.0f:1.0f;
		AudioListener.volume = _opt.isEffect?1.0f:0.0f;
		UserDataManager.instance.UIVolume = uiVolume;
		Global.isVibrate = _opt.isVibration;
	//	UserDataManager.instance.isQualitySettingValue = _opt.isHighQuality;
	//	QualitySettings.SetQualityLevel((UserDataManager.instance.isQualitySettingValue?7:6));
	//	Destroy(temp);

	//	return;
		if(EncryptedPlayerPrefs.HasKey("Quailty")){
			_opt.isHighQuality = false;

			UserDataManager.instance.isQualitySettingValue = _opt.isHighQuality;
			QualitySettings.SetQualityLevel((UserDataManager.instance.isQualitySettingValue?7:6));
			gameObject.GetComponent<GameOption>().SaveOptionValue();
			EncryptedPlayerPrefs.DeleteKey("Quailty");
		}else{
	//	float width = (float)Screen.width;
	//		if(width >= 1366){
	//			_opt.isHighQuality = true;
	// 		}
	//		gameObject.GetComponent<GameOption>().SaveOptionValue();
			UserDataManager.instance.isQualitySettingValue = _opt.isHighQuality;
			QualitySettings.SetQualityLevel((UserDataManager.instance.isQualitySettingValue?7:6));
	//		EncryptedPlayerPrefs.SetInt("Quailty", 1);
		}
	
	





		//	Global.isThirdCheckable = _opt.isThird;
		//	Global.isPushAlram = _opt.isAlarm;
		//Global.isThirdCheckable = false;
		//초기 false 값임.
		// int isT = _opt.isHighQuality?7:6;
		//Utility.Log(_opt.isHighQuality + "  " + isT); // 6
		//	UserDataManager.instance.ApplySetting(InitQuality
		//	                                      ,_opt.isHighQuality);
		//InitQuality(true);
		Destroy(temp);
		/*UserDataManager.instance.OnSetCarinfo(()=>{
			Utility.LogWarning("instanc OnSetCarinfo");
		});

		UserDataManager.instance.OnGetCarinfo(()=>{
			Utility.LogWarning("instanc OnGetCarinfo");
		});*/
	}
	void Awake(){

	}
	void InitApplyOption(){
	
	
	}
	void InitQuality(bool isHigh){
	//	QualitySettings.SetQualityLevel(isHigh?7:6, true);
	//	Utility.Log("Quailty " + QualitySettings.GetQualityLevel() + " " + isHigh);
	//	UserDataManager.instance.isQualitySettingValue = isHigh;
	}

	public void ChangeVolume(float vol, bool isBGM){
		if(isBGM){
			_bgmSound.Play();
			//_bgmSound.PlayOneShot(_bgm);
		}else{
			_bgmSound.Stop();
		}
		AudioListener.volume = vol;
	//	Utility.Log(vol + " : " + isBGM);
	}

	void OnLevelWasLoaded(int level){
		//Utility.LogWarning("OnlevelWasLoaded " + level);
	}

	public void StartLobbyMusic(){
		_titleSound.Stop();
		if(isbgmPlay){
			_bgmSound.Play();
		}else{
			_bgmSound.Stop();
			
		}
	//	Utility.LogWarning(isbgmPlay);
	}

	IEnumerator BGMPause(AudioClip au){
		_bgmSound.mute = true;
		NGUITools.PlaySound(au, 1.0f, 1.0f);
		yield return new WaitForSeconds(au.length);
		_bgmSound.mute = false;
	}

	public void CompleteSound(){
		StartCoroutine("BGMPause" , _complete);
		//Utility.LogWarning("CompleteSound ");
	}

	public void SeasonUpSound(){
		StartCoroutine("BGMPause" , _season);
		//NGUITools.PlaySound(_season, 1.0f, 1.0f);
	}

	public void LevelUpSound(){
		StartCoroutine("BGMPause" , _level);
		//NGUITools.PlaySound(_level, 1.0f, 1.0f);
	}

	public void buyCoin(){
		StartCoroutine("BGMPause" , _coin);
	}

	public void CompensationCoin(){
		StartCoroutine("BGMPause" , _money);
	}

	public void CountCoinStart(){
		StartCoroutine("countCoin", _coin);
	}
	public void CountCoinStop(){
		StopCoroutine("countCoin");
		_bgmSound.mute = false;
	}


	public void AttendStampSound(){
		StartCoroutine("BGMPause" , _attendStamp);
	}
	public void AttendSound(){
		StartCoroutine("BGMPause" , _attend);
	}


	public void CountDollarStart(){
		StartCoroutine("countDollar", _money);
	}
	public void CountDollarStop(){
		_bgmSound.mute = false;
		_titleSound.Stop();
		if(_tempClip != null)
			_titleSound.clip = _tempClip;
		StopCoroutine("countDollar");
	}
	private AudioClip _tempClip = null;
	IEnumerator countDollar(AudioClip au){
		_bgmSound.mute = true;
		_tempClip = _titleSound.clip;
		_titleSound.clip = au;
		_titleSound.Play();
		yield return new WaitForSeconds(au.length);
		_titleSound.Play();
	}

	IEnumerator countCoin(AudioClip au){
		_bgmSound.mute = true;
		for(;;){
			NGUITools.PlaySound(au, 1.0f, 1.0f);
			yield return new WaitForSeconds(au.length);
		}
		_bgmSound.mute = false;
		yield return null;
		
	}

	public void ChangeBGMMusic(bool back){
		//Utility.LogWarning("BGM");
		_bgmSound.Stop();
		if(back){
			_bgmSound.clip = _bgm1;  //lobby_music1
		}else{
			_bgmSound.clip = _bgm; // lobby_music2
		}
		_bgmSound.Play();
	}

	public void StartButtonPress(){
		StartCoroutine("BGMPause" , _start);
	}


	public void CheckBGMVolume(bool b){
		if(b){
			if(AudioListener.volume >0.1f){
				_bgmSound.Stop();
			}
			Utility.LogWarning("CheckBGMVolume"  + b);
		}else{
			if(AudioListener.volume >0.1f){
				_bgmSound.Play();
			}
			Utility.LogWarning("CheckBGMVolume"  + b);
		}
	}
}


