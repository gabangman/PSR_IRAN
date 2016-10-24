using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public partial class GameManager :  MonoSingleton< GameManager > {

	// Use this for initialization
	#region base
	RaceCameraList _camList;
	void Awake(){
		_camList = 	gameObject.AddComponent<RaceCameraList>();
		RaceState = GAMESTATE.IDLE;
		QuitCnt =0;
		switch(Global.gRaceInfo.sType){
		case SubRaceType.DragRace:
			RaceState = GAMESTATE.READY;
			
			isWheel = false;
			break;
		case SubRaceType.RegularRace:
			RaceState = GAMESTATE.IDLE;
			isWheel = true;
			break;
		case SubRaceType.CityRace:
			RaceState = GAMESTATE.READY;
			isWheel = false;
			break;
		default:
			break;
		}
		isPause = false;

	}
	
	public void StartAnimation(){
		Mycar.SendMessage("StartAnimation");
		for(int i = 0; i< AIcar.Length; i++){
			AIcar[i].SendMessage("StartAnimation");
		}
	}

	void Start(){
		AIcar = GameObject.FindGameObjectsWithTag("AICar");
		Mycar = GameObject.FindGameObjectWithTag("Player");
		Global.Race02Ranking=new string[AIcar.Length+1];// = new string[5];
		Global.Race02Time=new float[AIcar.Length+1];// = new float[5];
		GameObject _temp1 = GameObject.Find ("GUIManager");
		mgrgui = _temp1.GetComponent<ManagerGUI>() as ManagerGUI;
		mgrpit = _temp1.GetComponent<PitPlay>() as PitPlay;
		_isPress = true;
		isN2O = true;
		lbGameTime = mgrgui.findPanel("RaceUp").transform.FindChild("GTime").GetComponent<UILabel>() as UILabel;
		_uiSound = GetComponent<UISound>();
		_temp1 = null;
		mgrgui.engineStart();
		_totalTime = 0;
		_temp1 = null;
		Sign_PitIn = GameObject.Find("Sign_PitIn");
		Sign_PitIn.SetActive(false);
		Invoke("Sign_PitIn_Show", 2.5f);
		Sign_Finish = GameObject.Find("Sign_Finish");
		Sign_Finish.SetActive(false);
		GearPress = new float[20];
		gPressTime = new float[20];
		g_count = 0;g_PressCount = 0;
		GlobalScoreInit();
		CompetitionTimes = new float[(1+AIcar.Length)];
		if(Global.gRaceInfo.sType == SubRaceType.DragRace){
			int carlength = AIcar.Length + 1;
			if (carlength == 7) carlength = 7;
			rankSpritNames = new string[carlength];
			Global.gRaceCount = 0;isFinalGUI = true;
		}
		Global.gRaceCount = 0;
		PCount = 0;
		gDrillCount=0; pDrillCount=0; gGearCount = 0; pGearCount=0;
	}

	private void Sign_PitIn_Show(){
		Sign_PitIn.SetActive(true);
	}


	#endregion
}
