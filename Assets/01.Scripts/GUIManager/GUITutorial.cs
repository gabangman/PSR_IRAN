using UnityEngine;
using System.Collections;

public partial class ManagerGUI : MonoBehaviour {

	public void Tu_Ready(){
		if(_tutorial == null) Utility.LogError("null");
		_tutorial.TuReadyIn();
	}
	public void Tu_ReadyOut(){
		_tutorial.TuReadyOut();
	}
	
	public void CamPitIn1(){
		if(_tutorial == null) return;
		_tutorial.PinInSpeechStart();
	}
	public void CamPitIn2(){
		if(_tutorial == null) return;
		_tutorial.PinInSpeechEnd();

	}
	
	public void ShowDrillText(int count){
		_tutorial.ScrewSpeech(count);
	}
	
	public void CameraPitIn(){
		_tutorial.StartCoroutine("CarmeraPitIn");
	}
	
	public void speechActive(bool b){
		if(_tutorial == null) return;
		_tutorial.ResetSpeech();
		_tutorial.speech.SetActive(b);
	}
	
	public void TutorialEnd(){
		_tutorial.StartCoroutine("TutorialEnd");
	}

	public void TutorialHidden(){
		_tutorial.hiddenText();
	}

	public void TutorialShow(){
		_tutorial.showText();
	}
	void TutorialSetting(){
		EngineLEDCallback = ()=>{
			if(_carspeed.EngineRPM > _carspeed.CheckEngineRPM ){
				if(!isrpmcheck){
					StartCoroutine(strRpmLedCheck,_carspeed.CurrentGear);
					isrpmcheck = true;
				}
			}else{
				if(isrpmcheck){
					isrpmcheck = false;
				}
			}
		};
		
		AccelPressCallback = ()=>{
		};
		
		ClutchPressCallback = ()=>{
		};
		N2OPressCallback = ()=>{
			return;
		};
		Hidden_TextCallback = (tr)=>{
			
		};
		Show_TextCallback = (tr) =>{
		};
		
		AccelReleaseCallback = ()=>{
		};
		
		PressTimeCheckCallback = (delayf)=>{
			JudgeRPM("Perfect",0.0f);
			Effect[0].SetActive(true);
			Effect[2].SetActive(true);
			GameManager.instance.Vibrategame(400);
			s_ui.CarGearShiftSound();
			GearTextShow("Gear");
			delayCheckTime = 10.0f;
			hiddenBtn(btnclutch);
			showBtn(btnaccel);
		};
	


		if(Global.gChampTutorial == 2){
			strRpmLedCheck = "rpmLedCheck";
			TutorialRace001Setting();
		}else{
			strRpmLedCheck = "rpmLedCheck_Tutorial";
		
		}
	}



	void TutorialRace01Setting(){
		EngineLEDCallback = ()=>{
			if(_carspeed.EngineRPM > _carspeed.CheckEngineRPM ){
				if(!isrpmcheck){
					StartCoroutine(strRpmLedCheck,_carspeed.CurrentGear);
					isrpmcheck = true;
				}
			}else{
				if(isrpmcheck){
					hiddenBtn(btnclutch);
					EngineLedOff();
					//showBtn(btnaccel);
					isrpmcheck = false;
					checkTime = 0.0f;
				}
			}
		};

		AccelPressCallback = ()=>{
			if(_tutorial.isAccelPause) return;
			Time.timeScale = 1.0f;
			AudioManager.Instance.OnResumeMessage();
			_tutorial.speech.SetActive(false);
			AccelOnPress();
			AccelPressCallback = () =>{
				if(!_tutorial.isClutchPress) return;
				_tutorial.isAccelPause = false;
				Time.timeScale = 1.0f;
				AudioManager.Instance.OnResumeMessage();
				_tutorial.speech.SetActive(false);
				AccelOnPress();
			};
			btnaccel.SendMessage("BlinkBGStop",btnaccel,SendMessageOptions.DontRequireReceiver);
		};
		
		ClutchPressCallback = ()=>{
			if(_tutorial.isTimePause) return;
			Time.timeScale = 1.0f;
			_tutorial.speech.SetActive(false);
			_tutorial.isAccelPause = false;
			ClutchOnPress();
			ClutchPressCallback = null;
			ClutchPressCallback =()=>{
				if(_tutorial.isAccelPress){
					if(_tutorial.isClutchPress){
						return;
					}
				}
			//	Utility.Log(string.Format("clutchpress {0}, accelpress{1} : clutch ",_tutorial.isAccelPress, _tutorial.isAccelPress));
				_tutorial.isClutchPress = true;
				_tutorial.speech.SetActive(false);
				ClutchOnPress();
				//Time.timeScale = 1.0f;
				//AudioManager.Instance.OnResumeMessage();
				_tutorial.RaceSecondAccelPress();
			};
			_tutorial.RaceFirstAccelPress();
			btnclutch.SendMessage("BlinkBGStop", btnclutch,SendMessageOptions.DontRequireReceiver);
			btnaccel.SendMessage("BlinkBG",btnaccel, SendMessageOptions.DontRequireReceiver);	
		};
		N2OPressCallback = ()=>{
			N2OOnPress();
			return;
		};
		Hidden_TextCallback = (tr)=>{
			//if(_tutorial.isFirst) return;
			tr.SendMessage("blinkbtnStopText",SendMessageOptions.DontRequireReceiver);
			_tutorial.hiddenBtn(tr.name);
			
		};
		Show_TextCallback = (tr) =>{
			//if(_tutorial.isFirst) return;
			tr.SendMessage("blinkbtnText",SendMessageOptions.DontRequireReceiver);
			_tutorial.showBtn(tr.name);
		};
		
		AccelReleaseCallback = ()=>{
			isPress = false;dash_Speed.color =accelreleasecolor;
			GameManager.instance._isPress = false;
			if(!isclutchpress){
				if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02 ||
				   GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01)
				{
					if(_tutorial.isAccelPause) return;
					if(!_tutorial.isClutchPress)return;
					_tutorial.speech.SetActive(true);
					showBtn(btnaccel);_tutorial.isAccelPress = true;
					AudioManager.Instance.OnPauseMessage();
					Time.timeScale = 0.0f;
				}
				return;
			}
		};
	}

	public void TutorialRace001Setting(){
		StopCoroutine(strRpmLedCheck);
		strRpmLedCheck = "rpmLedCheck";
		//_tutorial.isRace02 = true;
		AccelPressCallback = ()=>{
			//Utility.LogWarning("race02 accel");
			AccelOnPress();
		};
		ClutchPressCallback = ()=>{
			//Utility.LogWarning("race02 ClutchPress");
			ClutchOnPress();
		};	N2OPressCallback = ()=>{
			N2OOnPress();
			return;
		};
		AccelReleaseCallback = ()=>{
			isPress = false;
			dash_Speed.color =accelreleasecolor;
			GameManager.instance._isPress = false;
			if(!isclutchpress){
				if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02 ||
				   GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01){
					showBtn(btnaccel);
					gearLedBlinkOff();
				}
				return;
			}
		};
		
		Hidden_TextCallback = (tr)=>{
			tr.SendMessage("hiddenbtnText",SendMessageOptions.DontRequireReceiver);
			_tutorial.hidden2Btn(tr.name);
			
		};
		Show_TextCallback = (tr) =>{
			tr.SendMessage("showbtnText",SendMessageOptions.DontRequireReceiver);
			_tutorial.show2Btn(tr.name);
		};
		
		EngineLEDCallback = ()=>{
			if(_carspeed.EngineRPM > _carspeed.CheckEngineRPM ){
				if(!isrpmcheck){
					StartCoroutine(strRpmLedCheck,_carspeed.CurrentGear);
					isrpmcheck = true;
				}
			}else{
				if(isrpmcheck){
					hiddenBtn(btnclutch);
					EngineLedOff();
					isrpmcheck = false;
					checkTime = 0.0f;
				}
			}
		};
		
		PressTimeCheckCallback = (delayf)=>{
			PressTimeCompareRace(delayf);
		};
	}
	public void TutorialRace02Setup(){
		StopCoroutine(strRpmLedCheck);
		strRpmLedCheck = "rpmLedCheck";
		_tutorial.isRace02 = true;
		AccelPressCallback = ()=>{
			//Utility.LogWarning("race02 accel");
			AccelOnPress();
		};
		ClutchPressCallback = ()=>{
			//Utility.LogWarning("race02 ClutchPress");
			ClutchOnPress();
		};
		AccelReleaseCallback = ()=>{
			isPress = false;
			dash_Speed.color =accelreleasecolor;
			GameManager.instance._isPress = false;
			if(!isclutchpress){
				if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02 ||
				   GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01){
					showBtn(btnaccel);
					gearLedBlinkOff();
				}
				return;
			}
		};
		
		Hidden_TextCallback = (tr)=>{
			tr.SendMessage("hiddenbtnText",SendMessageOptions.DontRequireReceiver);
			_tutorial.hidden2Btn(tr.name);
			
		};
		Show_TextCallback = (tr) =>{
			tr.SendMessage("showbtnText",SendMessageOptions.DontRequireReceiver);
			_tutorial.show2Btn(tr.name);
		};
		N2OPressCallback = ()=>{
			N2OOnPress();
			return;
		};
		EngineLEDCallback = ()=>{
			if(_carspeed.EngineRPM > _carspeed.CheckEngineRPM ){
				if(!isrpmcheck){
					StartCoroutine(strRpmLedCheck,_carspeed.CurrentGear);
					isrpmcheck = true;
				}
			}else{
				if(isrpmcheck){
					hiddenBtn(btnclutch);
					EngineLedOff();
					isrpmcheck = false;
					checkTime = 0.0f;
				}
			}
		};
		
		PressTimeCheckCallback = (delayf)=>{
			PressTimeCompareRace(delayf);
		};
		
	}




	IEnumerator rpmLedCheck_Tutorial(int gear){
		if(isRace01){
			s_music.CarAirHornSound();
			yield return new WaitForSeconds(0.2f);
		}
		bool isledon = false;
		float delay = Base64Manager.instance.getFloatEncoding(Global.gLedTime[gear],0.001f);
		bool isMaxGear = (MGear == gear) ? false: true;
		_isRPMCheck = true;
		checkTime = 0.0f;
		earlyled =3;
		while(true){
			if(!isMaxGear){
				GameManager.instance.isEnablePress  = false;
				yield return new WaitForSeconds(delay);
			}else{
			if(!isledon){
				gearled[0].SetActive(true);
				gearLedBlinkOff();
				gearLedBlinkOn(1);
				if(isRace01){
					_tutorial.RaceStartRed();
					yield return new WaitForSeconds(0.2f);
				}
				GameManager.instance.isEnablePress  = false;
				s_ui.LedRedPlay();
				yield return new WaitForSeconds(delay);
				gearled[1].SetActive(true);
				gearLedBlinkOff();
				gearLedBlinkOn(2);
				s_ui.LedRedPlay();
				yield return new WaitForSeconds(delay);
				gearled[2].SetActive(true);
				gearLedBlinkOff();
				gearLedBlinkOn(3);
				s_ui.LedRedPlay();
				yield return new WaitForSeconds(delay);
				if(isMaxGear){ 
					gearled[3].SetActive(true);
					_tutorial.RaceStartBlue();
					//_tutorial.isAccelPress = false;
					_tutorial.isClutchPress = false;
					if(isRace01){
						btnclutch.SendMessage("BlinkBG",btnclutch,SendMessageOptions.DontRequireReceiver);
						isRace01 = false;
						_tutorial.isClutchPress = true;
						if(Global.gChampTutorial == 2){
							TutorialRace001Setting();
						}else{
							TutorialRace01Setting();
						}
					}
					s_ui.LedBluePlay();
					showBtn(btnclutch);
					delayCheckTime = checkTime;
					yield return new WaitForSeconds(0.1f);
					yield return new WaitForSeconds(0.1f);
					isledon =true;
				}//else{Utility.Log("ABC");}
			}else{
				yield return new WaitForSeconds(0.05f);
				gearled[3].SetActive(false);
				yield return new WaitForSeconds(0.05f);
				gearled[3].SetActive(true);
				s_ui.LedBluePlay();
			}
			yield return null;
			}
		}
	}

}
