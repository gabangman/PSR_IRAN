using UnityEngine;
using System.Collections;

public class EffectSound : AudioMgr {


	//public GameObject Effect;
	[SerializeField] protected AudioClip s_AccelGood;
	public void CarAccelGoodSound(){
		StopEffectSound();
		PlayEffectSound(s_AccelGood);
	}
	
	[SerializeField] protected AudioClip s_AccelPerfect;
	public void CarAccelPerfectSound(){
		StopEffectSound();
		PlayEffectSound(s_AccelPerfect);
	//	Effect[0].SetActive(true);
	//	Effect[1].SetActive(true);
	}
	
	[SerializeField] protected AudioClip s_BoltScrewGood;
	public void BoltScewSound(){
		PlayEffectSound(s_BoltScrewGood);
	}
	
	[SerializeField] protected AudioClip s_BoltScrewPerfect;
	public void BoltScewPerfectSound(){
		PlayEffectSound(s_BoltScrewPerfect);
	}
	
	
	[SerializeField] protected AudioClip s_FinishlinePass;
	public void FinishLinePassSound(){
		PlayEffectSound(s_FinishlinePass);
	}
	[SerializeField] protected AudioClip s_CardSelect;
	public void playCardSelectSound(){
		StopCoroutine("PlayBGM");
		PlayEffectSound(s_CardSelect);
		//NGUITools.PlaySound(s_CardSelect);
	}
	[SerializeField] protected AudioClip s_CardBGM;
	public void playCardBGMSound(){
		AudioManager.Instance.EffectSoundVolume();
		StartCoroutine("PlayBGM", s_CardBGM);
	}

	IEnumerator PlayBGM(AudioClip _S){
		for(;;){
			PlayEffectSound(_S);
			//NGUITools.PlaySound(_S);
			yield return new WaitForSeconds(_S.length);
		}
	}
	public override void OnAudioClipDestory(){
		s_AccelGood = null;
		s_AccelPerfect = null;
		s_BoltScrewGood = null;
		s_BoltScrewPerfect = null;
		s_FinishlinePass = null;

	}


	void OnDestroy(){
		
		OnAudioClipDestory();
	}
}
