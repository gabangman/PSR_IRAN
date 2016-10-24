using UnityEngine;
using System.Collections;

public class BgmSound : AudioMgr {


	[SerializeField] protected AudioClip s_CarEngineUser;
	public override void CarEngineUserSound(){
		PlaySound(s_CarEngineUser);
	}
	


	public override void OnAudioClipDestory(){
		s_CarEngineUser = null;
	}

	void OnDestroy(){
	
		OnAudioClipDestory();
	}

}
