using UnityEngine;
using System.Collections;

public partial class ManagerGUI : MonoBehaviour {
	
	void 	AccelOnPressEvent(){
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

		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01READY){
			if(!isRace02StartLED){
				return;
			}
			isPress = false;
			s_music.CarTireSpinSound();
			if(rpmjudgefinal == "Perfect"){
				GameManager.instance.StartCoroutine("StartRace01Delay",Global.gR_Perfect);
				s_effect.CarAccelPerfectSound();
			}else if(rpmjudgefinal == "Early"){
				GameManager.instance.StartCoroutine("StartRace01Delay",(Global.gR_Early+0.5f));
				s_effect.CarAccelGoodSound();
				
			}else if(rpmjudgefinal == "Good"){
				GameManager.instance.StartCoroutine("StartRace01Delay",Global.gR_Good);
				s_effect.CarAccelGoodSound();
			}else{
				GameManager.instance.StartCoroutine("StartRace01Delay",(Global.gR_Late+0.5f));
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
		AccelPressCameraChangeEvent();
	}
	
	void AccelOnPressEventStart(){
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.READYEND){
			isFirstPress = true;
			isPress = true;GameManager.instance._isPress = true;
			s_music.CarTireSpinSound();
			float m_pressTime = checkTime;
			checkTime = 0.0f;
			_isRPMCheck = false;
			_carspeed.CurrentGear++;
			PressTimeCompare(m_pressTime);

			if(rpmjudgefinal == "Perfect"){
				GameManager.instance.StartCoroutine("StartRace01Delay",Global.gR_Perfect);
				s_effect.CarAccelPerfectSound();
			}else{
				float timedelay = 0;
				if(rpmjudgefinal == "Early"){
					timedelay = Global.gR_Early+0.5f;
				}else if(rpmjudgefinal == "Good"){
					timedelay = Global.gR_Good;
				}else{
					timedelay = Global.gR_Late+0.5f;
				}
				GameManager.instance.StartCoroutine("StartRace01Delay",timedelay);
				s_effect.CarAccelGoodSound();
			}
			btnaccel.SendMessage("btnPress",SendMessageOptions.DontRequireReceiver);
			hiddenBtn(btnaccel);
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE01DELAY;
			isrpmcheck = false;
			ClearSignal();
			AccelPressCallback = ()=>{
				AccelOnPressEvent();
			};
			//btnaccel.SendMessage("btnPress",SendMessageOptions.DontRequireReceiver);

		

			return;
		}
	}
	
	void ClutchOnPressEvent(){
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


