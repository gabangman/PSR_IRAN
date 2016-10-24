using UnityEngine;
using System.Collections;

public class MusicSound : AudioMgr {

	IEnumerator Start(){
		StartCoroutine("audioCheck");
		yield return null;
	}
	
	IEnumerator audioCheck(){
		while(true){
			if(AudioManager.Instance != null){
				PlayMusicSound(s_MusicStart);
				break;			
			}
			yield return null;
		}
	}
	

	
	[SerializeField] protected AudioClip s_CarUp;
	public void CarUpSound(){
		PlayMusicSound(s_CarUp);
	}
	[SerializeField] protected AudioClip s_CarDown;
	public void CarDownSound(){
		PlayMusicSound(s_CarDown);
		
	}
	
	[SerializeField] protected AudioClip s_CarPitIn;
	public void CarPitInSound(){
		PlayMusicSound(s_CarPitIn);
	}
	
	[SerializeField] protected AudioClip s_N2O;
	public void CarN2OSound(){
		PlayMusicSound(s_N2O);
		
	}

	public void StopCarN2OSound(){
		StopMusicSound();
	}
	
	[SerializeField] protected AudioClip s_TireSpin;
	public void CarTireSpinSound(){
		PlayMusicSound(s_TireSpin);
		
	}
	
	[SerializeField] protected AudioClip s_MusicStart;
	public void CarMusicStart(){
		PlayMusicSound(s_MusicStart);

	}
	
	[SerializeField] protected AudioClip s_AirHorn;
	public void CarAirHornSound(){
		PlayMusicSound(s_AirHorn);
	}
	[SerializeField] protected AudioClip s_FlagDrum;
	public void playFlagDrumSound(){
		PlayMusicSound(s_FlagDrum);
	}
	[SerializeField] protected AudioClip s_winSound;
	public void playwinSound(){
		PlayMusicSound(s_winSound);
	}
	[SerializeField] protected AudioClip s_lossSound;
	public void playlossSound(){
		PlayMusicSound(s_lossSound);
	}

	public override void OnAudioClipDestory(){
		s_AirHorn = null;
		s_MusicStart = null;
		s_TireSpin = null;
		s_N2O = null;
		s_CarPitIn = null;
	//	s_MusicfinishSound = null;
		s_CarUp = null;
	}

	void OnDestroy(){
		
		OnAudioClipDestory();
	}
}
