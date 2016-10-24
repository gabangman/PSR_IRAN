using UnityEngine;
using System.Collections;

public partial class ManagerGUI : MonoBehaviour {

	void InitDragModeSet(){
		btnclutch.transform.parent.gameObject.SetActive(false);
		EngineLEDCallback = ()=>{
			if(_carspeed.EngineRPM > _carspeed.CheckEngineRPM ){
				if(!isrpmcheck){
					StartCoroutine(strRpmLedCheck,_carspeed.CurrentGear);
					isrpmcheck = true;
				}
			}else{
				if(isrpmcheck){
					hiddenBtn(btnclutch);
					hiddenBtn(btnGearClutch);
					EngineLedOff();
					isrpmcheck = false;
					checkTime = 0.0f;
				}
			}
		};
		AccelPressCallback = ()=>{
			AccelOnPressDragStart();
		};
		ClutchPressCallback = ()=>{
			ClutchOnPressDrag();
		};
		N2OPressCallback = ()=>{
			N2OOnPress();
		};
		strRpmLedCheck = "rpmLedCheck";
		Hidden_TextCallback = (tr)=>{
			tr.SendMessage("hiddenbtnText",SendMessageOptions.DontRequireReceiver);
		};
		Show_TextCallback = (tr) =>{
			tr.SendMessage("showbtnText",SendMessageOptions.DontRequireReceiver);
		};
		AccelReleaseCallback = ()=>{
			isPress = false;
			dash_Speed.color =accelreleasecolor;
			GameManager.instance._isPress = false;
			if(!isclutchpress){
				if(  GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01 || 
				     GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01DELAY){
					showBtn(btnaccel); 
					gearLedBlinkOff();
				}
				return;
			}
		};
		
		PressTimeCheckCallback = (delayf)=>{
			PressTimeCompareRace(delayf);
		};
	}



	void InitEventModeSet(){
		btnclutch.transform.parent.gameObject.SetActive(false);
		EngineLEDCallback = ()=>{
			if(_carspeed.EngineRPM > _carspeed.CheckEngineRPM ){
				if(!isrpmcheck){
					StartCoroutine(strRpmLedCheck,_carspeed.CurrentGear);
					isrpmcheck = true;
				}
			}else{
				if(isrpmcheck){
					hiddenBtn(btnclutch);
					hiddenBtn(btnGearClutch);
					EngineLedOff();
					isrpmcheck = false;
					checkTime = 0.0f;
				}
			}
		};
		AccelPressCallback = ()=>{
			AccelOnPressEventStart();
		};
		ClutchPressCallback = ()=>{
			ClutchOnPressEvent();
		};
		N2OPressCallback = ()=>{
			N2OOnPress();
		};
		strRpmLedCheck = "rpmLedCheck";
		Hidden_TextCallback = (tr)=>{
			tr.SendMessage("hiddenbtnText",SendMessageOptions.DontRequireReceiver);
		};
		Show_TextCallback = (tr) =>{
			tr.SendMessage("showbtnText",SendMessageOptions.DontRequireReceiver);
		};
		AccelReleaseCallback = ()=>{
			isPress = false;
			dash_Speed.color =accelreleasecolor;
			GameManager.instance._isPress = false;
			if(!isclutchpress){
				if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02 ||
				   GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01 || 
				   GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02DELAY
				   ||  GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01DELAY){
					showBtn(btnaccel); 
					gearLedBlinkOff();
				}
				return;
			}
		};
		


		PressTimeCheckCallback = (delayf)=>{
			PressTimeCompareRace(delayf);
		};
	
	}



	void InitStockModeSet(){
		EngineLEDCallback = ()=>{
			if(_carspeed.EngineRPM > _carspeed.CheckEngineRPM ){
				if(!isrpmcheck){
					StartCoroutine(strRpmLedCheck,_carspeed.CurrentGear);
					isrpmcheck = true;
				}
			}else{
				if(isrpmcheck){
					hiddenBtn(btnclutch);
					hiddenBtn(btnGearClutch);
					EngineLedOff();
					isrpmcheck = false;
					checkTime = 0.0f;
				}
			}
		};
		AccelPressCallback = ()=>{
			AccelOnPress();
		};
		ClutchPressCallback = ()=>{
			ClutchOnPress();
		};
		N2OPressCallback = ()=>{
			N2OOnPress();
		};
		strRpmLedCheck = "rpmLedCheck";
		Hidden_TextCallback = (tr)=>{
			tr.SendMessage("hiddenbtnText",SendMessageOptions.DontRequireReceiver);
		};
		Show_TextCallback = (tr) =>{
			tr.SendMessage("showbtnText",SendMessageOptions.DontRequireReceiver);
		};
		AccelReleaseCallback = ()=>{
			isPress = false;
			dash_Speed.color =accelreleasecolor;
			GameManager.instance._isPress = false;
			if(!isclutchpress){
				if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02 ||
				   GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01 || 
				   GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02DELAY){
					showBtn(btnaccel); 
					gearLedBlinkOff();
				}
				return;
			}
		};
		
		PressTimeCheckCallback = (delayf)=>{
			PressTimeCompareRace(delayf);
		};
	}
}
