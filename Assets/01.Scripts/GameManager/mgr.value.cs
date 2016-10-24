using UnityEngine;
using System.Collections;

using System.Collections.Generic;


public partial class GameManager :  MonoSingleton< GameManager > {
	//old GameManager
	#region setter/getter variable
	public bool isEnablePress = true;
	public float _totalTime;
	public bool _isFinish = true;
	public float _AniSpeed = 0.5f;
	//public bool _isStop = false;
	//public bool _isPitIn;
	//public bool _isGame = false;
	public bool _isPress = false;
	public bool isN2O;
	public bool isWheel = true;
	public bool isFinalGUI = false;
	public bool _isRace02 = false;
	public float[] CompetitionTimes;
	public int raceCount = 0;
	GameObject Sign_PitIn, Sign_Finish;
	public bool isDrag =true;
	public int PCount = 0;
#endregion
	private bool isTimeCheck = false;
	private UILabel lbGameTime;
	private UILabel lbTotalTime;

	//public string _str; 
	//public float _pastTime;

	public ManagerGUI mgrgui;
	public PitPlay mgrpit;
	//public Animation _camAni;
	
	public enum STATE {IDLE, READY, RACE01,RACE01FINISH, PITIN, PITINING, PITINEND, RACE02READY, RACE02, RACE02FINISH, FINISH, GAMEOVER, WAIT };
	public enum GAMESTATE { PITINFINISH, IDLE, READY, RACE01, RACE02, PITINSTART,
		PITIN, PITINEND, FINISH, RACE02READY,RACE02DELAY, RACE01FINISH, PITINING,
		RACE02FINISH, RACE01DELAY, RACE01READY, READYEND};
	public STATE state = STATE.IDLE;
	public GAMESTATE RaceState = GAMESTATE.IDLE;

	public CameraTimeDelay _td;
	private int gameState;
	private GameObject[] AIcar;
	private GameObject Mycar;
	private GameObject Signal;
	public string strCameraName;
	private UISound _uiSound;
	bool isPause = false;
	public float[] GearPress;
	public float[] gPressTime;
	public int QuitCnt =0;
	public int gDrillCount, pDrillCount, gGearCount, pGearCount;
}

//public float race01length, race02length;
//public bool is_all_car_finish = false;
//public bool isSignal;


	