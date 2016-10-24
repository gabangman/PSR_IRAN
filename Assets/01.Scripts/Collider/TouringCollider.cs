using UnityEngine;
using System.Collections;

public class TouringCollider : MonoBehaviour {

	private bool isRace02 = false; 
	private bool isRace01 = false;
	private GameObject colObject;
	void Start(){
	/*	string[] Trigger = new string[]{"Race1_FinishCam_1"
			,"Race1_FinishTrigger", "Race2_FinishCam_1",
			"Race2_FinishCam_2", "Race2_FinishTrigger",
			"Race2_FinishCheck","GearDown_1"};*/
	
	}

	void Race01Finish(){
		GameManager.instance.RaceState = GameManager.GAMESTATE.RACE01FINISH;
	}
	void OnTriggerEnter(Collider col){

		if(col.gameObject.name == "Race2_StartTrigger" && isRace01){
			isRace02 = true;
			GameManager.instance.ShowSign(1);
			return;
		}

		if(col.gameObject.name == "Race1_FinishCam_1" && !isRace01){
			Invoke("Race01Finish", 1.5f);
			//GameManager.instance.RaceState = GameManager.GAMESTATE.RACE01FINISH;
			GameManager.instance.HiddenSign(0);
			return;
		}
		
		if(col.gameObject.name == "Race1_FinishTrigger"  && !isRace01){
			GameManager.instance.SwitchingCamera("Cam_R1_Finish");
			GameManager.instance.GameRaceStateChange("race01end");
			GameManager.instance.GameRaceStateChange("middleRank");
			GameManager.instance.GameRaceStateChange("Cam_Shake1");
			GameManager.instance.isEnablePress = true;
			isRace02 = false;
			isRace01 = true;
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_1"  && isRace02){
			//if(!GameManager.instance._isRace02) return;
			GameManager.instance.SwitchingCamera("Cam_R2_Finish_1");
			GameManager.instance.HiddenGUI();
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE02FINISH;
			col.gameObject.SetActive(false);
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_2"  && isRace02){
			//if(!GameManager.instance._isRace02) return;
			GameManager.instance.SwitchingCamera("Cam_R2_Finish_2");
			GameManager.instance.GameRaceStateChange("race02finish");
			GameManager.instance.StartCoroutine("SnapShotCamera");
			GameManager.instance.HiddenSign(1);
			return;
		}
		if(col.gameObject.name == "Race2_FinishCheck"  && isRace02){
			//if(!GameManager.instance._isRace02) return;
			GameManager.instance.FinalRankCompetition();
			col.gameObject.SetActive(false);
			Utility.Log("race2 finish check");
			return;
		}
		
		
		if(col.gameObject.name == "Race2_FinishTrigger"  && isRace02){
			//if(!GameManager.instance._isRace02) return;
			col.gameObject.SetActive(false);
			GameManager.instance.RaceState = GameManager.GAMESTATE.FINISH;
			GameManager.instance.StartCoroutine("RaceFinishTrigger");
			return;
		}
		
		if(col.gameObject.name == "Race2_FinishEnd"  && isRace02){
			return;
		}

		if(col.gameObject.name == "GearDown_1"){
			GameManager.instance.mgrgui.CarGearDown();
			return;
		}
	}
}
