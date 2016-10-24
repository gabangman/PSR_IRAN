using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public static AudioManager Instance = null;
	
	[SerializeField] private AudioSource _Car_AudioSource;
	[SerializeField] private AudioSource _bgm_AudioSource;
	[SerializeField] private AudioSource _Effect_AudioSource;
	[SerializeField] private AudioSource _Music_AudioSource;
	[SerializeField] private AudioSource _UI_AudioSource;
	[SerializeField] private AudioSource _PitIn_AudioSource;
//	private float _car_Volume = 1.0f;
//	private float _bgm_Volume = 1.0f;
	
//	public float GetCarVolume() { return _car_Volume; }
//	public float GetBgmVolume() { return _bgm_Volume; }

	void OnDestroy(){
		Instance = null;
	}

	void Awake(){
		Instance = this;		
	}


	void Start () {

	}

	public void CarSountInit(GameObject obj){
		//var car = GameObject.FindWithTag("Player") as GameObject;
		_Car_AudioSource = obj.transform.GetChild(0).GetChild(0).FindChild("CarSound").GetComponent<AudioSource>();
		_PitIn_AudioSource =obj.transform.GetChild(0).GetChild(0).FindChild("CarSound_Pit").GetComponent<AudioSource>();
		SetAllVolume();
		if(Global.gRaceInfo.sType != SubRaceType.RegularRace)
		//	StartCoroutine("PitSoundDecreas");
		{
			obj.SendMessage("PitSoundPlay");
			_Car_AudioSource.volume = 0.0f;
		}
		else
			StartCoroutine("carSoundDecreas");
	}

	bool isBGM, isMusic, isEffect;
	float mBGM, mMusic, mEffect;
	void SetAllVolume(){
		var temp = gameObject.AddComponent<GameOption>() as GameOption;
		GameOption.OptionSetting _opt = GameOption.OptionData;
		mBGM = 1.0f;
		mEffect =1.0f;
		mMusic= 1.0f;
		isMusic=_opt.isMusic;
		isEffect = _opt.isEffect;// = isEffect;
		isBGM = _opt.isBGM ;
		_Car_AudioSource.mute = !isEffect;
		_bgm_AudioSource.mute =  !isBGM;
		_bgm_AudioSource.Stop();
		_Effect_AudioSource.mute=  !isEffect;
		_Music_AudioSource.mute= !isMusic;
		_UI_AudioSource.mute=!isEffect;
		_PitIn_AudioSource.mute =  !isEffect;
		_Car_AudioSource.volume =  Mathf.Clamp((mEffect),0f,1f);
		_bgm_AudioSource.volume = Mathf.Clamp((mBGM),0f,1f);
		_Effect_AudioSource.volume=  Mathf.Clamp((mEffect),0f,1f);
		_Music_AudioSource.volume= Mathf.Clamp((mMusic),0f,1f);
		_UI_AudioSource.volume= Mathf.Clamp((mEffect),0f,1f);
		_PitIn_AudioSource.volume =  Mathf.Clamp((mEffect),0f,1f);
		//Utility.LogWarning(isEffect+ "  " + isBGM + "  " + isMusic);
		temp.DestroyDelay();
		Destroy(temp);
	}


	public IEnumerator carSoundDecreas(){
		float vol  = 0.0f;
		int count = 0;
		_Car_AudioSource.volume = 0.0f;
		yield return new WaitForSeconds(0.5f);
		_Car_AudioSource.Play();
		while(true){
			_Car_AudioSource.volume = Mathf.Clamp((vol),0f,mEffect);
			vol += 0.02f;
			if(count >= 50 ){
				_Car_AudioSource.volume = mEffect;
				break;
			}
			count++;
			yield return new WaitForSeconds(0.01f);
			}
	}

	public void PlayCarSound(AudioClip carClip)
	{
		if (carClip == null || _Car_AudioSource == null) return;
		_Car_AudioSource.clip = carClip;
		_Car_AudioSource.Play();
		_Car_AudioSource.loop = true;
	}

	public void PlayBgmSound(AudioClip carClip)
	{
		if (carClip == null || _bgm_AudioSource == null) return;
		_bgm_AudioSource.PlayOneShot(carClip);
	}


	public void PlayMusicSound(AudioClip carClip)
	{
		if (carClip == null || _Music_AudioSource == null) return;
		if(_Music_AudioSource.isPlaying) _Music_AudioSource.Stop();
		_Music_AudioSource.PlayOneShot(carClip);
		
		return;
	}
	
	public void PlayMusicLoopSound(AudioClip carClip)
	{
		if (carClip == null || _Music_AudioSource == null) return;
		//_Music_AudioSource.loop = true;
		_Music_AudioSource.PlayOneShot(carClip);
		
		return;
	}

	
	
	public void PlayUISound(AudioClip carClip)
	{
		if (carClip == null || _UI_AudioSource == null) return;
		_UI_AudioSource.PlayOneShot(carClip);
	}

	public void UISoundVolumeControl(float vol){
		if ( _UI_AudioSource == null) return;
		StopCoroutine("UISoundVolumeDown");
		StartCoroutine("UISoundVolumeDown", vol);
	}

	IEnumerator UISoundVolumeDown(float volume){
		_UI_AudioSource.volume = volume;
		while(true){
			volume -= 0.005f;
			_UI_AudioSource.volume =Mathf.Clamp( volume ,1.0f,2.0f);
			if(volume < 1.0f ){
				_UI_AudioSource.volume = 1.0f;
				break;
			}
			yield return new WaitForSeconds(0.01f);
		}
		yield return null;
	}


	public void PlayEffectSound(AudioClip carClip)
	{
		if (carClip == null || _Effect_AudioSource == null) return;
		_Effect_AudioSource.PlayOneShot(carClip);
	}

	public void StopMusicSound(float vol){
		StartCoroutine(MusicSoundVolume(vol));
	}

	public void StopMusicN2OSound(){
		_Music_AudioSource.Stop();
	}

	public void StopAllSound()
	{
		if (_Car_AudioSource != null) 
		{
		}
		StartCoroutine(BgmSoundVolume());
	}

	IEnumerator MusicSoundVolume(float volume){
		float vol  = 1.0f;
		while(true){
			vol -= 0.005f;
			_Car_AudioSource.volume =Mathf.Clamp((vol-volume),0f,mEffect);
			//_Effect_AudioSource.volume = Mathf.Clamp(vol, 0f, mEffect);

			if(vol <= 0 ){
				_Car_AudioSource.Stop();
				//_Effect_AudioSource.Stop();
				break;
			}
			
			yield return new WaitForSeconds(0.01f);
		}
		yield return null;
	}
	
	public void EffectSoundVolume(){
		_Effect_AudioSource.volume = 1.0f;
	}

	IEnumerator BgmSoundVolume(){
		float vol  = 1.0f;
		while(true){
			vol -= 0.01f;
			_bgm_AudioSource.volume = Mathf.Clamp((vol),0,mBGM);
			_Music_AudioSource.volume = Mathf.Clamp((vol),0.0f,mMusic);

			if(vol <= 0 ){
				
				break;
			}
			
			yield return new WaitForSeconds(0.005f);
		}
		yield return null;
	}


	public void SetCarVolume(float value)
	{
		value = Mathf.Clamp(value, 0.0f, mEffect);
		_Car_AudioSource.volume = value;
		
		//jks - todo : Save sound effect volume here. //PlayerPrefs.SetFloat("_sfx_Volume", _sfx_Volume);
	}
	
	public void SetBgmVolume(float value)
	{
		value = Mathf.Clamp(value, 0.0f, mBGM);
		_bgm_AudioSource.volume =value;
		
	}
	
	public void audioPitch(float num){
	
		_Car_AudioSource.pitch = num;
	}
	
	public float caraudiopitch{
		get{
			return _Car_AudioSource.pitch;
		}
		set{
			_Car_AudioSource.pitch = value;
		}
		
		
	}
	
	public float caraudioVolume{
		set{
			_Car_AudioSource.volume = value;
		}
	}


	public void CarVolumDown(float vol){
		_Car_AudioSource.volume = Mathf.Clamp(vol, 0.0f, mEffect);
	}
	public void CarAudioStop(){
		_Car_AudioSource.Stop();
	}
	IEnumerator CarAudioVolumDown(){
		float vol = mEffect;
		for(;;){
			vol -= 0.01f;
			_Car_AudioSource.volume = Mathf.Clamp(vol, 0.0f, mEffect);
			if(vol <= 0)
				break;
		yield return new WaitForSeconds(0.05f);
		}
	}

	public void StopUISound(){
		_UI_AudioSource.Stop();
	}
	
	public void StopEffectSound(){
		_Effect_AudioSource.Stop();
	}

	void audioPause(){
		AudioListener.pause = true;
	}
	void audioResume(){
		AudioListener.pause = false;
	}

	public void OnPauseMessage(){
	//	Utility.LogWarning("Sound Pause");
		//audioPause();
		if(!_Car_AudioSource.mute)	_Car_AudioSource.mute = true;
		if(!_bgm_AudioSource.mute)_bgm_AudioSource.mute = true;
		if(!_Effect_AudioSource.mute)_Effect_AudioSource.mute = true;
		if(!_Music_AudioSource.mute)_Music_AudioSource.mute = true;
	}
	
	public void OnResumeMessage(){
		//audioResume();
		_Car_AudioSource.mute = !isEffect;
		_bgm_AudioSource.mute =  !isBGM;
		_Effect_AudioSource.mute=  !isEffect;
		_Music_AudioSource.mute= !isMusic;
		_UI_AudioSource.mute=!isEffect;
		_PitIn_AudioSource.mute =  !isEffect;

		//_Car_AudioSource.mute = false;
		//_bgm_AudioSource.mute = false;
		//_Effect_AudioSource.mute = false;
		//_Music_AudioSource.mute = false;
	}
	
	public void playloopSound(AudioClip _au){
		_UI_AudioSource.clip = _au;
		_UI_AudioSource.Play();
		_UI_AudioSource.loop = false;
		
	}
}
