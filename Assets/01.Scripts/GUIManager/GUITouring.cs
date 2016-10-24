using UnityEngine;
using System.Collections;

public partial class ManagerGUI : MonoBehaviour {
	private int dragCheck = 0;
	public void TouringCheckTime(float delay){
		_isRPMCheck = true;
		checkTime = 0;
		delayCheckTime = delay;
	//	StartCoroutine("CheckStartJudge");
	}

	IEnumerator CheckStartJudge(){
		dragCheck = 0;
		for(int i = 0; i < 20; i++){
			yield return new WaitForSeconds(0.1f);
			dragCheck = i+1;
		}
	}

	public void TouringStart1(){
		GameManager.instance.RaceState = GameManager.GAMESTATE.READYEND;
		showBtn(btnaccel);
		GameManager.instance.DisableSetActvieUser();
		dragCheck = 1;
	}

	public void TouringStart2(){
		//GameManager.instance.RaceState = GameManager.GAMESTATE.READYEND;
		s_music.CarAirHornSound();
		float delay = Base64Manager.instance.getFloatEncoding(Global.gLedTime[1],0.001f);
		StartCoroutine("AIAutoStartRoutine", 0.1f);
		isRace01 = false;
		GameManager.instance.isEnablePress = true;
		isMaxGear = false;
		dragCheck = 2;
	}

	public void TouringStart3(){
		dragCheck = 3;
	}

	void 	AccelOnPressTouring(){
		isPress = true;//Utility.Log ("onPress race ");
		dash_Speed.color = accelpresscolor;
		GameManager.instance._isPress = true;
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.PITINEND){
			btnaccel.SendMessage("btnPress",SendMessageOptions.DontRequireReceiver);
			hiddenBtn(btnaccel);
			return;
		}
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02DELAY
		   ||GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01DELAY ){
			return;
		}
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02READY){
			if(!isRace02StartLED){
				return;
			}
			isPress = false;
			s_music.CarTireSpinSound();
			if(rpmjudgefinal == "Perfect"){
				GameManager.instance.StartCoroutine("StartRace02Delay",0.0f);
				s_effect.CarAccelPerfectSound();
			}else{
				GameManager.instance.StartCoroutine("StartRace02Delay",1.0f);
				s_effect.CarAccelGoodSound();
			}
			btnaccel.SendMessage("btnPress",SendMessageOptions.DontRequireReceiver);
			hiddenBtn(btnaccel);
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE02DELAY;
			GameManager.instance.SaveResultTime("PitIn_E");
			isrpmcheck = false;
			gearLedBlinkOn(0);
			return;
		}

		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01READY){
			if(!isRace02StartLED){
				return;
			}
			isPress = false;
			s_music.CarTireSpinSound();
			if(rpmjudgefinal == "Perfect"){
				GameManager.instance.StartCoroutine("StartRace01Delay",0.0f);
				s_effect.CarAccelPerfectSound();
			}else{
				GameManager.instance.StartCoroutine("StartRace01Delay",1.0f);
				s_effect.CarAccelGoodSound();
			}
			btnaccel.SendMessage("btnPress",SendMessageOptions.DontRequireReceiver);
			hiddenBtn(btnaccel);
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE01DELAY;
			isrpmcheck = false;
			gearLedBlinkOn(0);
			return;
		}
		
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02FINISH ||
		   GameManager.instance.RaceState == GameManager.GAMESTATE.READY ||
		   GameManager.instance.RaceState == GameManager.GAMESTATE.READYEND) return;
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01 || 
		   GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02 ){
			btnaccel.SendMessage("btnPress",SendMessageOptions.DontRequireReceiver);
			gearLedBlinkOn(0);
		}
		AccelPressCameraChagne();
	}

	void AccelOnPressTouringStart(){
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.READYEND){
			isPress = true;GameManager.instance._isPress = true;
			s_music.CarTireSpinSound();
			float m_pressTime = checkTime;
			checkTime = 0.0f;
			_isRPMCheck = false;
			_carspeed.CurrentGear++;
			PressTimeCompare(m_pressTime);
			if(rpmjudgefinal == "Perfect"){
				GameManager.instance.StartCoroutine("StartRace01Delay",0.0f);
				s_effect.CarAccelPerfectSound();
			}else{
				GameManager.instance.StartCoroutine("StartRace01Delay",1.0f);
				s_effect.CarAccelGoodSound();
			}
			btnaccel.SendMessage("btnPress",SendMessageOptions.DontRequireReceiver);
			hiddenBtn(btnaccel);
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE01DELAY;
			isrpmcheck = false;
			ClearSignal();
			AccelPressCallback = ()=>{
				AccelOnPressTouring();
			};btnaccel.SendMessage("btnPress",SendMessageOptions.DontRequireReceiver);
			return;
		}
	}

	private void ClearSignal(){
		GameManager.instance.HiddenSignalCount();
		GameManager.instance.ShowSignalFlag();
		btnclutch.transform.parent.gameObject.SetActive(true);

	}

	void ClutchOnPressTouring(){
	//	Utility.LogWarning("1 : " + GameManager.instance.isEnablePress);
	//	Utility.LogWarning("2 : " + isRace02StartLED);

		if(GameManager.instance.isEnablePress) return;
		GameManager.instance.isEnablePress=true;
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.READY)return;
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.PITIN)return;
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02FINISH) return;
		isclutchpress = true;
		int num = _carspeed.CurrentGear; 
		string str= null;
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.PITINEND){
			if(!isRace02StartLED) {
				GameManager.instance.isEnablePress= false;
				return;
			}
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE02READY;
			str = GameManager.instance.strCameraName;
			GameManager.instance.ZoomInOutCamera(str);
			_isRPMCheck = false;
			float m1_pressTime = checkTime;
			checkTime = 0.0f;
			_carspeed.CurrentGear = 1;
			PressTimeCompare(m1_pressTime);
			_carspeed.CurrentGear = 0;
			hiddenBtn(btnclutch);
			showBtn(btnaccel);
			EngineLedOff();
			GearBgOn();
			return;
		}
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.READYEND){
			Utility.Log(isRace02StartLED);
			if(!isRace02StartLED) {
				GameManager.instance.isEnablePress= false;
				return;
			}
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE01READY;
			str = GameManager.instance.strCameraName;
			GameManager.instance.ZoomInOutCamera(str);
			_isRPMCheck = false;
			float m1_pressTime = checkTime;
			checkTime = 0.0f;
			_carspeed.CurrentGear = 1;
			PressTimeCompare(m1_pressTime);
			_carspeed.CurrentGear = 0;
			hiddenBtn(btnclutch);
			showBtn(btnaccel);
			EngineLedOff();
			GearBgOn();
			return;
		}
		GameManager.instance._isPress = false;
		int mG = Base64Manager.instance.GlobalEncoding(Global.gMaxGear);
		if(_carspeed.CurrentGear == mG) {
			isMaxGear = true;
			return;
		}
		if(_carspeed.CurrentGear == (mG-1)) MaxGearTextAction(true);
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02 || 
		   GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01){
			if(isGear) if(!isGearPress) {
				s_ui.CarGearClutchEmptySound();
				return;
			}
			btnclutch.SendMessage("clutchPress",SendMessageOptions.DontRequireReceiver);
			GearBgOn();
			str = GameManager.instance.strCameraName;
			if(num != 9) 
				GameManager.instance.ZoomInOutCamera(str);
			if(!GameManager.instance.isN2O){
				EngineLedOff();
			}
		}
		_isRPMCheck = false;
		float m_pressTime = checkTime;
		checkTime = 0.0f;
		_carspeed.CurrentGear++;
		PressTimeCompare(m_pressTime);
		return;
	}


}
