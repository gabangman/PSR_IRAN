using UnityEngine;
using System.Collections;

public partial class ManagerGUI : MonoBehaviour {

	public void CarGearDown(){
		StartCoroutine("GearBrakeDown");
	}

	IEnumerator GearBrakeDown(){
		_carspeed.motorInputTorque = _carspeed.motorInputTorque * 0.5f;
		_carspeed.tempBrakePower  = 50;
		yield return new WaitForSeconds(1.5f);
		_carspeed.motorInputTorque = _carspeed.motorInputTorque * 2.0f;
		_carspeed.tempBrakePower  = 0;
	}

	void gearLedBlinkOn(int i){
		gearledBlink[i].SetActive(true);
	}

	void gearLedBlinkOff(){
		int cnt = gearledBlink.Length;
		for(int i = 0; i < cnt ; i++){
			gearledBlink[i].SetActive(false);
		}
	}

	void ClutchOnPress(){
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


	public void OnClutchPress(){
		Utility.Log("onClutchPress");
		if((GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01)||
		   (GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02)){
			btnGearClutch.SendMessage("gearClutchPress",SendMessageOptions.DontRequireReceiver);
			isGearPress = true;
			if(isPress) {
				GameManager.instance._isPress  = false;
			}
			s_ui.CarGearClutchSound();
			hiddenBtn(btnGearClutch);
		}
	}

	public void OnClutchRelease(){
		Utility.Log("OnClutchRelease");
		if((GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01)||
		   (GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02)){
			isGearPress = false;
			if(isPress) {
				GameManager.instance._isPress  = true;
			}
		}
	}

	void AccelOnPress(){
		isFirstPress = true;
		isPress = true;//Utility.Log ("onPress race ");
		dash_Speed.color = accelpresscolor;
		GameManager.instance._isPress = true;
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.PITINEND){
			btnaccel.SendMessage("btnPress",SendMessageOptions.DontRequireReceiver);
			hiddenBtn(btnaccel);
			return;
		}
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02DELAY){
			return;
		}
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02READY){
			if(!isRace02StartLED){
			 	return;
			 }
			isPress = false;
			s_music.CarTireSpinSound();
			if(rpmjudgefinal == "Perfect"){
				GameManager.instance.StartCoroutine("StartRace02Delay",Global.gR_Perfect);
				s_effect.CarAccelPerfectSound();
			}else if(rpmjudgefinal == "Early"){
				GameManager.instance.StartCoroutine("StartRace02Delay",(Global.gR_Early+0.5f));
				s_effect.CarAccelGoodSound();
			
			}else if(rpmjudgefinal == "Good"){
				GameManager.instance.StartCoroutine("StartRace02Delay",Global.gR_Good);
				s_effect.CarAccelGoodSound();
			}else{
				GameManager.instance.StartCoroutine("StartRace02Delay",(Global.gR_Late+0.5f));
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
		
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02FINISH) return;
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01 || 
		   GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02 ){
			if(isGear) if(isGearPress) {
				GameManager.instance._isPress = false;
				//isGearPressCheck = true;
				return;
			}
			btnaccel.SendMessage("btnPress",SendMessageOptions.DontRequireReceiver);
			gearLedBlinkOn(0);
		}
		AccelPressCameraChagne();
	}
	
	public void OnPressed(){
		AccelPressCallback();

	}
	public void OnClutchClick(){
		//Global.bGearPress = true;
		if(Global.gChampTutorial == 0)
			AccelReleaseCallback();
		ClutchPressCallback();
	
	}


	public void OnReleased(){
		if(Global.gChampTutorial != 0) AccelReleaseCallback();
		else{
			if(!Global.bGearPress) AccelReleaseCallback();
		}
	}

	public void AccelBgOn(){
		speedIndicatorTween(dash_Speed,"accelpress");
	}
	public void AccelBgOff(){
	
	}

	public void GearBgOn(){
		speedIndicatorTween(dash_Rpm,"scale");
		speedIndicatorTween(dash_Rpm,"Color2");
	}

}
