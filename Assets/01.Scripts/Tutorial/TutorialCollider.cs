using UnityEngine;
using System.Collections;

public class TutorialCollider : MonoBehaviour {

	
	private bool isRace02 = false; 
	private GameObject colObject;
	// Use this for initialization
	
	void Awake(){
	
	}
	
	
	void OnTriggerEnter(Collider col){
		if(col.gameObject.name == "Ready_2"&& !isRace02){
			GameManager.instance.SwitchingCamera("Cam_Ready2");
			GameManager.instance.GameRaceStateChange("Ready");
			GameManager.instance.RaceState = GameManager.GAMESTATE.READY;
			col.gameObject.SetActive(false);
			GameManager.instance.mgrgui.Tu_Ready();
			return;
		}
		if(col.gameObject.name == "Ready_3"&& !isRace02){
			GameManager.instance.SwitchingCamera("Cam_Ready3");
			GameManager.instance.DisableSetActvieUser();
			colObject = col.gameObject;

			GameManager.instance.mgrgui.Tu_ReadyOut();
			return;
		}
		if(col.gameObject.name == "Ready_Count"&& !isRace02){
			GameManager.instance.SwitchingCamera("Cam_Ready_Count");
			col.gameObject.SetActive(false);
			colObject.SetActive(false);
			return;
		}
		if(col.gameObject.name == "Race1_Cam2"&& !isRace02){
			col.gameObject.SetActive(false);
			return;
		}
		if(col.gameObject.name == "Race1_Cam3"&& !isRace02){
			col.gameObject.SetActive(false);
			return;
		}
		if(col.gameObject.name == "Race1_Cam4"&& !isRace02){
			col.gameObject.SetActive(false);
			return;
		}
		if(col.gameObject.name == "Race1_Cam5"&& !isRace02){
			col.gameObject.SetActive(false);
			return;
		}
		if(col.gameObject.name == "Race1_StartTrigger" && !isRace02){
			GameManager.instance.GameRaceStateChange("Start");
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE01;
			GameManager.instance.RaceCountCheck(col.gameObject);	
			GameManager.instance.AIRaceStart();
		//	Utility.LogWarning("Race1_StartTrigger");
			return;
		}
		
		if(col.gameObject.name == "Race1_FinishCam_1" && !isRace02){
			if(GameManager.instance._isRace02) return;
			isRace02 =  true;
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE01FINISH;
			GameManager.instance.RaceCountCheck(col.gameObject);
			GameManager.instance.HiddenSign(0);
		//	Utility.LogWarning("Race1_FinishCam_1");
			return;
		}
		
		if(col.gameObject.name == "Race1_FinishTrigger"  && isRace02){
			if(GameManager.instance._isRace02) return;
			GameManager.instance.SwitchingCamera("Cam_R1_Finish");
			GameManager.instance.GameRaceStateChange("race01end");
			GameManager.instance.GameRaceStateChange("middleRank");
			GameManager.instance.GameRaceStateChange("Cam_Shake1");
			GameManager.instance.isEnablePress = true;
		//	Utility.LogWarning("Race1_FinishTrigger");
		//	GameManager.instance.mgrgui.speechActive(false);
			return;
		}
		if(col.gameObject.name == "Race1_Crewchief" && !isRace02){
			if(GameManager.instance._isRace02) return;
			GameManager.instance.mgrpit.PitcrewActive();
			return;
		}
		if (col.gameObject.CompareTag("Player")){
			Utility.Log("Trigger Player ");
		}
		//Utility.Log ("Trigger Enter");
		
		if(col.gameObject.name == "Race2_Cam2" && isRace02){
			
			col.gameObject.SetActive(false);
			return;
		}
		if(col.gameObject.name == "Race2_Cam3"&&isRace02){
			
			col.gameObject.SetActive(false);
			return;
		}
		if(col.gameObject.name == "Race2_Cam4"&&isRace02){
			
			col.gameObject.SetActive(false);
			return;
		}
		if(col.gameObject.name == "Race2_Cam5"&&isRace02){
			
			col.gameObject.SetActive(false);
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_1"  && isRace02){
			if(!GameManager.instance._isRace02) return;
			GameManager.instance.SwitchingCamera("Cam_R2_Finish_1");
			GameManager.instance.HiddenGUI();
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE02FINISH;
			col.gameObject.SetActive(false);
		//	Utility.LogWarning("Race2_FinishCam_1");
		//	GameManager.instance.mgrgui.speechActive(false);
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_2"  && isRace02){
			if(!GameManager.instance._isRace02) return;
			GameManager.instance.SwitchingCamera("Cam_R2_Finish_2");
			GameManager.instance.GameRaceStateChange("race02finish");
			//GameManager.instance.mgrgui.speechActive(false);
			GameManager.instance.StartCoroutine("SnapShotCamera");
			GameManager.instance.RaceCountCheck(col.gameObject);
			GameManager.instance.HiddenSign(1);
			//Utility.LogWarning("Race2_FinishCam_2");
			return;
		}


		
		if(col.gameObject.name == "Race2_FinishEnd"  && isRace02){
			Utility.LogWarning("race2_finishEnd");
			return;
		}


		if(col.gameObject.name == "Race2_FinishTrigger"  && isRace02){
			if(!GameManager.instance._isRace02) return;
			GameManager.instance.RaceFinishSound();
			GameManager.instance.RaceState = GameManager.GAMESTATE.FINISH;
			GameManager.instance.StartCoroutine("RaceFinishTrigger");
			GameManager.instance.mgrgui.FinishEffect();
			return;
		}
		
	
		if(col.gameObject.name == "Race2_FinishCam_2_1"  && isRace02){
			if(!GameManager.instance._isRace02) return;
			GameManager.instance.SwitchingCamera("Cam_R2_Finish_2");
			GameManager.instance.GameRaceStateChange("race02finish");
			GameManager.instance.HiddenSign(1);
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
			GameManager.instance.StartCoroutine("RegularFinishCam_2_3");
			return;
		}
	
	}

}
