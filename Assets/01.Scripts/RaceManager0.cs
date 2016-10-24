using UnityEngine;
using System.Collections;

public class RaceManager0 : MonoBehaviour {
	/*
	static ApplicationManager _instance;
	public static ApplicationManager GetInstance(){
		if(_instance == null){
			_instance = new GameObject("ApplicationManager").AddComponent<ApplicationManager>();
			//isLoaded = true;
		}
		return _instance;
	}
*/

	public int modeType = 0;
	public Texture2D FarTexture, NearTexture;
	void Awake(){
		//**** 테스트용 삭제
		if(Global.isRaceTest){
		//	EncryptedPlayerPrefs.keys=new string[5];
		//	EncryptedPlayerPrefs.keys[0]="23easLw";
		//	EncryptedPlayerPrefs.keys[1]="Sio89aAd";
		//	EncryptedPlayerPrefs.keys[2]="tasAAras5";
		//	EncryptedPlayerPrefs.keys[3]="tHet2Easr";
		//	EncryptedPlayerPrefs.keys[4]="Kaws8EDaS"; 

		//	GV.PlayCarID = 1000;
		//	GV.PlayCrewID = 1200;
		//	GV.PlaySponID = 1300;
		/*Global.gRaceInfo = new RaceInfo();
			Global.gRaceInfo.init();
			switch(modeType){
			case 0:
				Global.gRaceInfo.sType = SubRaceType.DragRace;
				break;
			case 1:
				Global.gRaceInfo.sType = SubRaceType.RegularRace;
				break;
			case 2:
				Global.gRaceInfo.sType = SubRaceType.RegularRace;
				break;
			case 3:
				Global.gRaceInfo.sType = SubRaceType.CityRace;
				break;
			}
			Global.gRaceInfo.sType = SubRaceType.RegularRace;
			Global.DragSub = 1;
		//	Global.gTorque = Base64Manager.instance.setFloatEncoding(25f,1000);
		*/
			// PitPlay
		//	Global.gScorePerfect = Base64Manager.instance.GlobalEncoding(10);
		//	Global.gScoreGood = Base64Manager.instance.GlobalEncoding(5);
		//	Global.gPitCameraTime =Base64Manager.instance.setFloatEncoding(1,1000); 
		//	Global.gPitCrewTime = Base64Manager.instance.setFloatEncoding(0.5f,1000); 
		//	Global.gPitCameraDelay = Base64Manager.instance.setFloatEncoding(0.5f,1000);
		//	Global.gScrewTime = Base64Manager.instance.setFloatEncoding(0.5f ,1000);
		//	Global.gGasTime =Base64Manager.instance.setFloatEncoding(0.5f ,1000);
		}
		//*************
		//Global.DragSub = 0;
		QualitySettings.vSyncCount = 1;
		bool isquality = UserDataManager.instance.isQualitySettingValue;
		UserDataManager.instance.ApplySetting(
		(isSetting) => {
			QualitySettings.SetQualityLevel(isSetting?7:6, true);
		},isquality
		);
		SceneInit();
		RaceModeSetting();
	}


	void SceneInit(){
	//	UserDataManager.instance.fuelTimeStop();
		var mgr = ObjectManager.CreatePrefabs("Manager","0_AudioManager") as GameObject;
		mgr.name =  "AudioManager";
		mgr = ObjectManager.CreatePrefabs("Manager","0_GameManager") as GameObject;
		mgr.name  = "GameManager";
		mgr = ObjectManager.CreatePrefabs("Manager","0_GUIManager") as GameObject;
		mgr.name  = "GUIManager";
		mgr  = null;
		return;
	}
	void OnLevelWasLoaded(int level){
	//	Utility.LogWarning("OnlevelWasLoaded " + level);
	}

	void RaceModeSetting(){
		if(Global.isRaceTest) {
				Global.gAICarInfo = new AICarInfo[6];
		}
	
		InitRace();
		if(Global.isRaceTest) {
			ObjectManager.CreateRaceObject(transform);
			ObjectManager.CreateAIObject();
		}
	}

	GameObject[] AICar;
	void InitRace(){
		AICar = GameObject.FindGameObjectsWithTag("AICar");
		int[] randomID = gameObject.AddComponent<RandomCreate>().CreateRandomValue(AICar.Length);
		for(int i = 0; i < AICar.Length; i++){
			AICar[i].SendMessage("AICarInit",randomID[i],SendMessageOptions.DontRequireReceiver);
		}
		GameManager.instance.RaceResourceLoad();
	
		//C2 -> C4 -> C1 -> C3
	}
	void OnDestroy(){
	
	
	}


	
		
	
		
	
}
