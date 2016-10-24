using UnityEngine;
using System.Collections;


[System.Serializable] 
public static class Global {
	//GameRace Variable
	
	public static string[] Race02Ranking;// = new string[5];
	public static float[] Race02Time;// = new float[5];
	public static int gRaceCount = 0;
	public static int pGearScore; //y
	public static int gGearScore; //y
	public static int pDrillScore; //y
	public static int gDrillScore; //y
	public static int myRank;//y
	public static int gScorePerfect; //y 
	public static int gScoreGood; //y
	public static float gCtrlDelay = 0.2f;
	public static float[] gCtrlDelays;
	public static float gP_MAX = 0.35f;
	public static float gP_MIN = 0.05f;
	public static float Screw_MAX = 0.35f;
	public static float Screw_MIN = 0.05f;
	public static int rankRaceIdx = -1;
	public static float gR_Perfect = 0.0f;
	public static float gR_Good = 0.5f;
	public static float gR_Late = 1.0f;
	public static float gR_Early = 1.0f;
	public static bool bPermission = false;
	public static bool bGearPress = false;
	public static bool bAccelPress = false;
	public static bool bLobbyBack = false;
	//public static float Race1ResutTime;
	//public static float Race2ResutTime;
	// Rank 레이스 유저에 필요한 데이터들 
	
	public static float PitInResutTime;
	public static float RaceResutTime;
	public static int PressBoostTime = 0;
	public static int gMyGearLv = 0;
	//Game Attribute
	public static bool isRace;
	public static bool Loading; //초기화면
	public static bool isLobby = true; //업그레이드 
	public static bool isLoadFinish = false;
	public static bool isRaceTest = true;
	public static string orderID;
	public static string packageName;
	public static string productId;
	//userinfo
	public static int level;
	public static int glevel;
	public static long coin; //y
	public static long dollar; //y
	public static int Exp;
	public static int addExp = 0;
	public static int addLevel = 0;
//	public static int fuel;
//	public static int maxFuel;
//	public static int userIndex;
	
	//user personalinfo
	public static bool isVibrate;
	public static int gPushable = 1;
	public static int g3Agree;
	
	public static int gCardCount;
	public static int fuelTime;
	public static bool isThirdCheckable = false;
	public static int gReceivedFuel;
	public static int gLvUp=0;
	public static int gSeasonUp;
	public static int gNewMsg;
	public static int gInviteCount = 0;
	public static int gCouponCount = 0;
	//GameSetting
	public static int gReLoad = 0;
	public static string gVersion;
	public static int gSale = 0; //y
	public static string currentVesion;
	public static string gPushID;

	public static string gDeivceID;
	public static string gCountryCode;
	public static int gExpireSpon;
	public static int gSeasonLvClear = 0;
	public static int gReview=0;
	public static int gTutorial = 0;
	public static int gChampTutorial = 0;
	public static int gRewardId = 6600;
	public static int gConvertDollar;

	public static bool isDailyReset = true;
	public static bool isFeaturedReset = false;
	public static int gWeeklyStart;

	//Apply Car Attribute
	public static float gTorque = 20;
	public static float[] gLedTime = new float[10]{0.6f, 0.2f,0.3f,0.45f,0.5f,0.6f,0.65f,0.7f,0.75f, 0.8f};
	public static float gTireDelay = 1.9f;
	public static float gBsPower = 50;
	public static float gBsTime = 1.7f;
	public static float[] gGearRatio;
	//Apply Crew Attribute
	public static float gRaceLedTime;//= 0.85f;
	public static float gScrewTime = 0.65f;
	public static float gGasTime = 0.1f;
	public static float gPitCameraTime = 1.02f;
	public static float gPitCrewTime = 1.0f;
	public static float gPitCameraDelay = 0.98f;
	public static int gBonus = 200;
	public static int gMaxGear = 6;
	public static int gCheckRPM = 2500;
	public static int gMaxRPM = 3000;
	public static int rpmAlpah = 1000;
	public static bool isBoostable;

	//game proc value
	public static bool isNetwork = false;
	public static bool isPopUp = false;
	public static bool isAnimation = false;
	public static System.DateTime matchedTime;


	public static int gRegularRaceLevel; //y
	public static Texture gDefaultIcon;
	public static string myNick_0;
	public static string myProfileUrl;

	public static bool isTutorial = false;
	public static bool isReTutorial = false;

	public static readonly int eTrackID = 1405;
	public static readonly int eCrewID = 1205;

	//Attend Event
	public static int[] gTimeAttackEvent;
	//public static int gTimeEvent ;
	public static int[]  gTimeHistory;
	public static int gAttendDayCount ;
	public static int gAttend;
	
	//Post
	public static int[] gMyRank = new int[5];
//	public static int gMyWholeRank;
//	public static int gMyWeeklyRank;
	public static int gFriendRank;
	public static int gFirstRank = 1;
	public static int gFirstRankRace = 0;

	public static int gStarCoupon = 0;
	//add boost
	public static int addBoost = 0;
	public static float addBSTime;
	public static int addBSTorque;
	//FaceBook
	public static int gisFBLogin;
	public static bool isCoinPop;
	public static bool isLobbyRotate;
	public static string gOsVersion;
	public static string gDeviceModel;

	public static void tutorialracesetting(){
		gTorque = Base64Manager.instance.setFloatEncoding(20, 1000);
		gTireDelay = Base64Manager.instance.setFloatEncoding(1.9f, 1000);
		gBsTime = Base64Manager.instance.setFloatEncoding(1.7f, 1000);
		gBsPower = Base64Manager.instance.setFloatEncoding(50, 1000);
		gScrewTime = Base64Manager.instance.setFloatEncoding(0.65f,1000);
		gGasTime = Base64Manager.instance.setFloatEncoding(0.1f,1000);
		gPitCameraTime = Base64Manager.instance.setFloatEncoding(1.02f,1000);
		gPitCrewTime = Base64Manager.instance.setFloatEncoding(1.0f,1000);
		gPitCameraDelay = Base64Manager.instance.setFloatEncoding(0.98f,1000);
		rpmAlpah = Base64Manager.instance.GlobalEncoding(1000);
		gMyGearLv = 0;
		gMaxGear = Base64Manager.instance.GlobalEncoding(6);
		gCheckRPM = Base64Manager.instance.GlobalEncoding(2500);
		gMaxRPM = Base64Manager.instance.GlobalEncoding(3000);
		gRegularRaceLevel = Base64Manager.instance.GlobalEncoding(10);
		gBonus = Base64Manager.instance.GlobalEncoding(200);
		isNetwork = false;
		isPopUp = false;
		isAnimation = false;
		gRaceInfo = new RaceInfo();
		gLedTime = new float[10]{
			Base64Manager.instance.setFloatEncoding(0.6f,1000), 
			Base64Manager.instance.setFloatEncoding(0.2f,1000), 
			Base64Manager.instance.setFloatEncoding(0.3f,1000), 
			Base64Manager.instance.setFloatEncoding(0.45f,1000), 
			Base64Manager.instance.setFloatEncoding(0.5f,1000), 
			Base64Manager.instance.setFloatEncoding(0.6f,1000), 
			Base64Manager.instance.setFloatEncoding(0.65f,1000), 
			Base64Manager.instance.setFloatEncoding(0.7f,1000), 
			Base64Manager.instance.setFloatEncoding(0.75f,1000),
			Base64Manager.instance.setFloatEncoding(0.8f,1000)
		};
		gRaceLedTime = Base64Manager.instance.setFloatEncoding(0.85f,1000);
		gRaceInfo.mType = MainRaceType.Tutorial;
		gRaceInfo.R1_Time = 24.958f;		gRaceInfo.R2_Time = 22.875f;
	}

	public static void Init(){
		gRegularRaceLevel = Base64Manager.instance.GlobalEncoding(10);
		isNetwork = false;
		isPopUp = false;
		isAnimation = false;
		gFirstRankRace = 0;
	}


	//AI Race
	public static AICarInfo[] gAICarInfo;
	public static RaceInfo gRaceInfo;
	public static int DragSub;
}

public struct AICarInfo{
	public float  Torque;
	public float Pit_Time;
	public float B_Power;
	public float B_Time;
	public float Skid_Time;
	public int gbLv;
	public float raceResultTime;
	public int AICarID;
	public int AICrewID;
	public int AISponsorID;
	public bool isLive;
	public string userNick;
	public Texture2D userTexture;
	public float EventTime;
	public float EventRange;
	public float[] gearLedDelay;
	public float[] pressDelay;
	public int AIRefCarID;
	public int AIRefStarLV;
	public Common_Class.Item AIClass;
}

public struct RaceInfo{
	public string trackID;
	public int extraDollar;
	public string iconName;
	public string raceName;
	public int SponBouns;
	public MainRaceType mType;
	public SubRaceType sType;
	public int mode1Rw;
	public int extraCoin;
	public string eventModeName;
	public int rewardMatCount;
	public float R1_Time;
	public float R2_Time;
	public void init(){
		trackID = string.Empty;
		extraDollar = 0;
		iconName =string.Empty;
		raceName = string.Empty;
		SponBouns = 0;
		mode1Rw = 0;
		mType = MainRaceType.Tutorial;
		sType = SubRaceType.RegularRace;
		rewardMatCount = 0;
	}
}
