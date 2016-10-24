using UnityEngine;
using System.Collections;

public abstract class AudioMgr : MonoBehaviour {

	protected void PlaySound(AudioClip carClip) 
	{
		if (AudioManager.Instance == null) return;
		
		AudioManager.Instance.PlayBgmSound(carClip);
	}


	protected void PlayCarSound(AudioClip carClip) 
	{
		if (AudioManager.Instance == null) return;
		
		AudioManager.Instance.PlayCarSound(carClip);
	}

	protected void PlayUISound(AudioClip carClip) 
	{
		if (AudioManager.Instance == null) return;
		
		AudioManager.Instance.PlayUISound(carClip);
	}

	protected void StopUISound(){
		AudioManager.Instance.StopUISound();
	}

	protected void UISoundVolumeControl(float vol){
		if (AudioManager.Instance == null) return;
		
		AudioManager.Instance.UISoundVolumeControl(vol);

	}

	//effect
	protected void PlayEffectSound(AudioClip carClip) 
	{
		if (AudioManager.Instance == null) return;
		
		AudioManager.Instance.PlayEffectSound(carClip);
	}
	
	protected void StopEffectSound(){
		AudioManager.Instance.StopEffectSound();
	}
	// Musci

	protected void PlayMusicSound(AudioClip carClip) 
	{
		if (AudioManager.Instance == null) return;
		
		AudioManager.Instance.PlayMusicSound(carClip);
		//Utility.LogWarning("why not!!");
	}
	
	
	
	protected void StopMusicSound(){
		AudioManager.Instance.StopMusicN2OSound();
	}

	//BGM
	public virtual void CarEngineUserSound(){}

	//Car Sound

	public virtual void CarEnginePitStopSound(){}
	public virtual void CarEngineRaceSound(){}
	public bool isDown = false;

	IEnumerator VolumeDelay(){
		float Vol = 0.8f;
		isDown = true;
		AudioManager.Instance.SetCarVolume(Vol);
		yield return new WaitForSeconds(0.01f);

		while(true){
			Vol+= 0.02f;
			AudioManager.Instance.SetCarVolume(Vol);
			if(Vol >=1.0f){
				isDown = false;
				break;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
	//UI Sound

	public virtual void OnAudioClipDestory(){}
	//Effect sound


}
