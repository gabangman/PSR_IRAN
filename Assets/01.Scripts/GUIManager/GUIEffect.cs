using UnityEngine;
using System.Collections;

public partial class ManagerGUI : MonoBehaviour {
	public delegate void AudioControl();
	public AudioControl _audioctr = null;
	public void CardSound(bool b){
		if(b) {
			s_effect.playCardSelectSound();
		}else{
			s_effect.playCardBGMSound();
		}
	}
	public void engineStart(){
		if(s_car == null){
			s_car = GetComponent<CarSound>();
		}s_car.CarEngineRaceSound();
	}
	
	public void engineStop(){
	
	}

	public void CarSoundVolumeDown(AudioControl _audio){
		_audio();
	}
	
	public void addTween(string str, Vector3 from, Vector3 to, float _duration){
		GameObject obj = findPanel(str);
		obj.SetActive(true);
		TweenPosition _temp = obj.AddComponent<TweenPosition>();
		_temp.from =  from;
		_temp.to =  to;
		_temp.duration = _duration;
		_temp.style = UITweener.Style.Once;
		_temp.method = UITweener.Method.EaseInOut;
		
		hiddenBtn(btnaccel);
		hiddenBtn(btnclutch);
		hiddenBtn(btnGearClutch);
		obj = null;
	}
	
	
	public void endTween(string str, Vector3 from, Vector3 to, float _duration){
		GameObject obj = findPanel(str);
		TweenPosition _temp = obj.GetComponent<TweenPosition>();
		if(_temp != null) Destroy(_temp);
		
		_temp = obj.AddComponent<TweenPosition>();
		_temp.from =  to;
		_temp.to =  from;
		_temp.duration = _duration;
		_temp.style = UITweener.Style.Once;
		_temp.method = UITweener.Method.EaseInOut;
		obj = null;
	}

	void OnQuitClick(){
		GameManager.instance.OnQuitClick();
	}

}
