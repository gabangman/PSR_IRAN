using UnityEngine;
using System.Collections;

public class StockCollider : MonoBehaviour {
	
	private bool isRace02 = false; 
	private GameObject colObject;
	void Start(){
	/*	string[] Trigger = new string[]{"Race1_StartTrigger","Race1_FinishCam_1"
			,"Race1_FinishTrigger","Race2_FinishCam_1", "Race2_FinishCam_2",
			"Race2_FinishTrigger", "Race2_FinishCheck", "Ready_2", "Ready_3","Ready_Count"};
		*/
	}

	void Race01Finish(){
		GameManager.instance.RaceState = GameManager.GAMESTATE.RACE01FINISH;
	}
	void OnTriggerEnter(Collider col){
		if(col.gameObject.name == "Ready_2"&& !isRace02){
			GameManager.instance.SwitchingCamera("Cam_Ready2");
			GameManager.instance.GameRaceStateChange("Ready");
			GameManager.instance.RaceState = GameManager.GAMESTATE.READY;
			col.gameObject.SetActive(false);
			return;
		}


		if(col.gameObject.name == "Ready_3"&& !isRace02){
			//	Utility.Log("Ready_3 ");
			//	Utility.Log("isAnimainot " + Global.isAnimation);
			GameManager.instance.SwitchingCamera("Cam_Ready3");
			//col.gameObject.SetActive(false);
			GameManager.instance.DisableSetActvieUser();
			colObject = col.gameObject;
			if(!Global.isTutorial) return;
			GameManager.instance.mgrgui.Tu_Ready();
			return;
		}
		if(col.gameObject.name == "Ready_Count"&& !isRace02){
			GameManager.instance.SwitchingCamera("Cam_Ready_Count");
			//GameManager.instance.mgrgui.EngineFirstLed(true);
			//GameManager.instance.AICarSpeedChange(2.0f);
			//Utility.Log("Ready_Count");
			col.gameObject.SetActive(false);
			//GameManager.instance.AICarRaceAnimationStart();
			colObject.SetActive(false);
			return;
		}
		/*
		if(col.gameObject.name == "Race1_Cam2"&& !isRace02){
			col.gameObject.SetActive(false);
			//GameManager.instance.AICarSpeedChange(GameManager.instance._AniSpeed);
			//Utility.LogWarning("1 + " +GameManager.instance._AniSpeed);
			return;
		}
		if(col.gameObject.name == "Race1_Cam3"&& !isRace02){
			col.gameObject.SetActive(false);
			//Utility.LogWarning("2 + " +GameManager.instance._AniSpeed);
			//GameManager.instance.AICarSpeedChange(GameManager.instance._AniSpeed);
			
			return;
		}
		if(col.gameObject.name == "Race1_Cam4"&& !isRace02){
			col.gameObject.SetActive(false);
			//Utility.LogWarning("3 + " +GameManager.instance._AniSpeed);
			//GameManager.instance.AICarSpeedChange(GameManager.instance._AniSpeed);
			return;
		}
		if(col.gameObject.name == "Race1_Cam5"&& !isRace02){
			col.gameObject.SetActive(false);
			//Utility.LogWarning("4 + " +GameManager.instance._AniSpeed);
			//GameManager.instance.AICarSpeedChange(GameManager.instance._AniSpeed);
			return;
		}*/
		if(col.gameObject.name == "Race1_StartTrigger" && !isRace02){
			/*
			//_cam.CamNum = 3;
			//_cam.CamNum = _cam.cameraSelect("Cam_R1_Gear4");
			//_gameMgr.AniSpeed = 1.6F;
			//Utility.Log("Cam_R1_Gear2");
			//_gameMgr.AniSpeed = 1.0F;
			//col.gameObject.SetActive(false);
			//_cam.CamChange("Start");
			//GameManager.instance.gasGague("Start");
			//GameManager.instance.SaveResultTime("R1_S");
			*/
			//GameManager.instance.AIRaceStart();
			GameManager.instance.GameRaceStateChange("Start");
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE01;
			//GameManager.instance.RaceCountCheck(col.gameObject);	
			
			//gameObject.AddComponent<SaveRaceResult>().StartWriteResult();
			//	GameManager.instance._isGame = true;
			
			//GameManager.instance._isFinish = false;
			//GameManager.instance.StartWriteSpeed();
			//	GameManager.instance.StartReadSpeed();
			//Utility.LogWarning("5 + " +GameManager.instance._AniSpeed);
			return;
		}
		if(col.gameObject.name == "Race1_FinishCam_1" && !isRace02){
			if(GameManager.instance._isRace02) return;
			//GameManager.instance.SwitchingCamera("Cam_R1_Finish");
			//GameManager.instance._AniSpeed = 1.0F;
			isRace02 =  true;
			Invoke("Race01Finish", 1.0f);
			//GameManager.instance.RaceState = GameManager.GAMESTATE.RACE01FINISH;
			GameManager.instance.HiddenSign(0);
			
			//RankCheck(1);
			//GameManager.instance.PlayMyCrewAnimation(0);
			//Utility.LogWarning("6 + " +GameManager.instance._AniSpeed);
			//			//Utility.Log("Race1_FinishCam_1");
			//col.gameObject.SetActive(false);
			return;
		}
		if(col.gameObject.name == "Race1_Crewchief" && !isRace02){
			if(GameManager.instance._isRace02) return;
			GameManager.instance.mgrpit.PitcrewActive();
			return;
		}
		if(col.gameObject.name == "Race1_FinishTrigger"  &&  isRace02){
			if(GameManager.instance._isRace02) return;

			GameManager.instance.SwitchingCamera("Cam_R1_Finish");
			GameManager.instance.GameRaceStateChange("race01end");
			GameManager.instance.GameRaceStateChange("middleRank");
			GameManager.instance.GameRaceStateChange("Cam_Shake1");
			GameManager.instance.isEnablePress = true;
			return;
		}
		/*
		if (col.gameObject.CompareTag("Player")){
			Utility.Log("Trigger Player ");
		}
		//Utility.Log ("Trigger Enter");
		
		if(col.gameObject.name == "Race2_Cam2" && isRace02){
			
			col.gameObject.SetActive(false);
			//GameManager.instance.AICarSpeedChange(GameManager.instance._AniSpeed);
			//Utility.LogWarning("9 + " +GameManager.instance._AniSpeed);
			return;
		}
		if(col.gameObject.name == "Race2_Cam3"&&isRace02){
			
			col.gameObject.SetActive(false);
			//GameManager.instance.AICarSpeedChange(GameManager.instance._AniSpeed);
			//Utility.LogWarning("10 + " +GameManager.instance._AniSpeed);
			return;
		}
		if(col.gameObject.name == "Race2_Cam4"&&isRace02){
			
			col.gameObject.SetActive(false);
			//GameManager.instance.AICarSpeedChange(GameManager.instance._AniSpeed);
			//Utility.LogWarning("11 + " +GameManager.instance._AniSpeed);
			return;
		}
		if(col.gameObject.name == "Race2_Cam5"&&isRace02){
			
			col.gameObject.SetActive(false);
			//GameManager.instance.AICarSpeedChange(GameManager.instance._AniSpeed);
			//Utility.LogWarning("12 + " +GameManager.instance._AniSpeed);
			return;
		} */
		if(col.gameObject.name == "Race2_FinishCam_1"  && isRace02){
			if(!GameManager.instance._isRace02) return;
			GameManager.instance.SwitchingCamera("Cam_R2_Finish_1");
			//GameManager.instance.AICarSpeedChange(GameManager.instance._AniSpeed);
			GameManager.instance.HiddenGUI();
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE02FINISH;
			//GameManager.instance.FinalRankCompetition();
			col.gameObject.SetActive(false);
			//Utility.LogWarning("13 + " +GameManager.instance._AniSpeed);
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_2"  && isRace02){
			if(!GameManager.instance._isRace02) return;
			GameManager.instance.SwitchingCamera("Cam_R2_Finish_2");
			GameManager.instance.GameRaceStateChange("race02finish");
			//GameManager.instance.StartCoroutine("SnapShotCamera");
		///	GameManager.instance.StartCoroutine("RegularSnapShotStart");
			GameManager.instance.HiddenSign(1);
			return;
		}
		if(col.gameObject.name == "Race2_FinishCheck"  && isRace02){
			if(!GameManager.instance._isRace02) return;
			GameManager.instance.FinalRankCompetition();
			return;
		}
		
		
		if(col.gameObject.name == "Race2_FinishTrigger"  && isRace02){
			if(!GameManager.instance._isRace02) return;
			//col.gameObject.SetActive(false);
			//GameManager.instance._isGame = false;

			//GameManager.instance._isFinish = true;
			//GameManager.instance.GameFinishSound();
			GameManager.instance.RaceFinishSound();
			GameManager.instance.RaceState = GameManager.GAMESTATE.FINISH;
			GameManager.instance.StartCoroutine("RaceFinishTrigger");
			GameManager.instance.mgrgui.FinishEffect();
			//GameManager.instance.StartCoroutine("DecreaseAnispeed");
			//_cam.CamChange("Cam_Shake2");
			//GameManager.instance.ChangeCamera("Cam_Shake2");
			//Utility.LogWarning("8 + " +GameManager.instance._AniSpeed);
			//GameManager.instance.EndWriteSpeed();
			//GameManager.instance.EndReadSpeed();
			//Utility.LogWarning("Race2_FinishTrigger");
			return;
		}
		
		if(col.gameObject.name == "Race2_FinishEnd"  && isRace02){
			//GameManager.instance.StopCoroutine("DecreaseAnispeed");
			//GameManager.instance._AniSpeed = 1.0F;
			//_cam.CamChange("race02");
			
			//	col.gameObject.SetActive(false);
			//GameManager.instance._isGame = false;
			//	GameManager.instance._isFinish = true;
			//GameManager.instance.GameFinishSound();
			//GameManager.instance.RaceState = GameManager.GAMESTATE.FINISH;
			//_cam.CamChange("Cam_Shake2");
			//GameManager.instance.ChangeCamera("Cam_Shake2");
			//Utility.LogWarning("8 + " +GameManager.instance._AniSpeed);
			//GameManager.instance.EndWriteSpeed();
			//Utility.LogWarning("race2_finishEnd");
			//GameManager.instance.EndReadSpeed();
			return;
		}

		if(col.gameObject.name == "Race2_FinishCam_2_1"  && isRace02){
			if(!GameManager.instance._isRace02) return;
			GameManager.instance.SwitchingCamera("Cam_R2_Finish_2");
			GameManager.instance.GameRaceStateChange("race02finish");
			//GameManager.instance.StartCoroutine("SnapShotCamera");
			GameManager.instance.HiddenSign(1);
		//	GameManager.instance.StartCoroutine("RegularSnapShotStart");
			GameManager.instance.StartCoroutine("RegularFinishCam_2_1");
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_2_2"  && isRace02){
			if(!GameManager.instance._isRace02) return;
			GameManager.instance.StartCoroutine("RegularFinishCam_2_2");
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_2_3"  && isRace02){
			if(!GameManager.instance._isRace02) return;
			//Utility.LogWarning("Race2_F_2_3 " + isRace02);
			GameManager.instance.StartCoroutine("RegularFinishCam_2_3");
			return;
		}
	}

}
