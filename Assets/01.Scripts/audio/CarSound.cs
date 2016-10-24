using UnityEngine;
using System.Collections;

public class CarSound : AudioMgr {



	[SerializeField] protected AudioClip s_CarEngineRace;
	public override void CarEngineRaceSound(){
		PlayCarSound(s_CarEngineRace);
	}
	[SerializeField] protected AudioClip s_CarEnginePitStop;
	public override void CarEnginePitStopSound(){
		PlayCarSound(s_CarEnginePitStop);
	}
	public void CarEngineVolumeDelay(){
		if(isDown){
			StopCoroutine("VolumeDelay");
		}
		StartCoroutine("VolumeDelay");
	
	}
	public override void OnAudioClipDestory(){
		s_CarEngineRace = null;
		s_CarEnginePitStop = null;
	}

	void OnDestroy(){
		
		OnAudioClipDestory();
	}



}
