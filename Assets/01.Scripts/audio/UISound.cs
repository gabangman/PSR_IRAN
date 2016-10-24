using UnityEngine;
using System.Collections;

public class UISound : AudioMgr {

	[SerializeField] protected AudioClip s_GearShift;
	public void CarGearShiftSound(){
		//UISoundVolumeControl(2.0f);
		PlayUISound(s_GearShift);
		
	}

	public void UISoundControl(float vol){
		UISoundVolumeControl(2.0f);
	}

	[SerializeField] protected AudioClip s_GearClutchSound;
	public void CarGearClutchSound(){
		PlayUISound(s_GearClutchSound);
		
	}
	[SerializeField] protected AudioClip s_GearClutchEmptySound;
	public void CarGearClutchEmptySound(){
		PlayUISound(s_GearClutchEmptySound);
		
	}
	[SerializeField] protected AudioClip s_GearShift_ETC;
	public void CarGearShiftETCSound(){
		PlayUISound(s_GearShift_ETC);
		
	}
	
	[SerializeField] protected AudioClip s_AccelTurch;
	public void CarAccelTurchSound(){
		PlayUISound(s_AccelTurch);
	}
	
	[SerializeField] protected AudioClip s_MovingPanel;
	public void CarMovingPanelSound(){
		PlayUISound(s_MovingPanel);
	}
	
	
	[SerializeField] protected AudioClip s_CameraShutter;
	public void CarCameraShutterSound(){
		PlayUISound(s_CameraShutter);
	}

	[SerializeField] protected AudioClip s_WhipPanelUser;
	public void WhipPanelPlay(){
		PlayUISound(s_WhipPanelUser);
	}
	[SerializeField] protected AudioClip s_WhipPanelAward;
	public void WhipPanelAwardPlay(){
		PlayUISound(s_WhipPanelAward);
	}
	[SerializeField] protected AudioClip s_flagMix;
	public void FlagSoundPlay(){
		PlayUISound(s_flagMix);
	}
	[SerializeField] protected AudioClip s_ScrewYellow;
	public void ScrewYellowPlay(){
		PlayUISound(s_ScrewYellow);
	}
	[SerializeField] protected AudioClip s_ScrewWhite;
	public void ScrewWhitePlay(){
		PlayUISound(s_ScrewWhite);
	}
	[SerializeField] protected AudioClip s_LEDRed;
	public void LedRedPlay(){
		PlayUISound(s_LEDRed);
	}
	[SerializeField] protected AudioClip s_LEDBlue;
	public void LedBluePlay(){
		PlayUISound(s_LEDBlue);
	}
	[SerializeField] protected AudioClip s_GasFull;
	public void GasFullSoundPlay(){
		PlayUISound(s_GasFull);
	}

	[SerializeField] protected AudioClip s_winSound;
	public void playWinSound(){
		//PlayUISound(s_winSound);
		AudioManager.Instance.playloopSound(s_winSound);
	}
	[SerializeField] protected AudioClip s_lossSound;
	public void playLossSound(){
		//PlayUISound(s_lossSound);
		AudioManager.Instance.playloopSound(s_lossSound);
	}

	[SerializeField] protected AudioClip s_countMoney;
	public void playCountSound(){
		PlayUISound(s_countMoney);
	}

	public void playCountSoundStop(){
		StopUISound();
	}
	
	[SerializeField] protected AudioClip s_countCoin;
	public void playCountCoinSound(){
		PlayUISound(s_countCoin);
	}

	[SerializeField] protected AudioClip s_MusicfinishSound;
	public void CarMusicFirstFinishSound(){
	//	Utility.Log("name");
		PlayMusicSound(s_MusicfinishSound);
	}
	[SerializeField] protected AudioClip s_Musicfinish2Sound;
	public void CarMusicSecondFinishSound(){
	//	Utility.Log("name1");
		PlayMusicSound(s_Musicfinish2Sound);
	}

	[SerializeField] protected AudioClip s_Racefinish;
	public void RaceFinishSound(){
		//	Utility.Log("name1");
		PlayMusicSound(s_Racefinish);
	}
	[SerializeField] protected AudioClip s_rewardSound;
	public void RewardSound(){
		PlayMusicSound(s_rewardSound);
		//NGUITools.PlaySound(s_rewardSound);
	}

	/*
	[SerializeField] protected AudioClip s_CardSelectSound;
	public void playCardSelectSound(){
		PlayMusicSound(s_CardSelectSound);
	}
	
	[SerializeField] protected AudioClip s_CardSound;
	public void startCardMusic(){
		StartCoroutine("PlayCardSound", s_CardSound);// PlayMusicSound(s_CardSound);
	}
	public void stopCardMusic(){
		StopCoroutine("PlayCardSound");
	}
	IEnumerator PlayCardSound(AudioClip _S){
		for(;;){
			PlayMusicSound(_S);
			yield return new WaitForSeconds(_S.length);
		}
	}
	*/
	public override void OnAudioClipDestory(){
		s_LEDBlue = null;
		s_LEDRed = null;
		s_ScrewWhite = null;
		s_ScrewYellow = null;
		s_flagMix = null;
		s_WhipPanelAward = null;
		s_WhipPanelUser = null;
		s_CameraShutter= null;
		s_MovingPanel= null;
		s_AccelTurch= null;
		s_GearShift_ETC= null;
		s_GearShift = null;
		s_winSound = null;
		s_lossSound = null;
		s_countMoney = null;
		s_Racefinish = null;
		s_rewardSound = null;
		
	}

	void OnDestroy(){
		
		OnAudioClipDestory();
		System.GC.Collect();
	}
}
