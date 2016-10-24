using UnityEngine;
using System.Collections;

public class EventCollider : MonoBehaviour {

	private GameObject colObject;
	void Start(){
		/*string[] Trigger = new string[]{"Race1_FinishCam_1"
			,"Race1_FinishTrigger","Race2_FinishCam_1", "Race2_FinishCam_2",
			"Race2_FinishTrigger", "Race2_FinishCheck", "ChangeCam_toNormal", "ChangeCam_toDrag"};
		*/
	}

	void Race01Finish(){
		GameManager.instance.RaceState = GameManager.GAMESTATE.RACE01FINISH;
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.name == "Race1_FinishCam_1" ){
			Invoke("Race01Finish", 1.5f);
			GameManager.instance.HiddenSign(0);
			GameManager.instance.QuitCnt = 0;
			return;
		}
		if(col.gameObject.name == "Race1_Crewchief" ){
			GameManager.instance.mgrpit.PitcrewActive();
			return;
		}
		if(col.gameObject.name == "Race1_FinishTrigger"){
			GameManager.instance.SwitchingCamera("Cam_R1_Finish");
			GameManager.instance.GameRaceStateChange("race01end");
			GameManager.instance.GameRaceStateChange("middleRank");
			GameManager.instance.GameRaceStateChange("Cam_Shake1");
			GameManager.instance.isEnablePress = true;
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_1" ){
			GameManager.instance.SwitchingCamera("Cam_R2_Finish_1");
			GameManager.instance.HiddenGUI();
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE02FINISH;
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_2" ){
			//GameManager.instance.SwitchingCamera("Cam_R2_Finish_2");
			//GameManager.instance.GameRaceStateChange("race02finish");
			//GameManager.instance.StartCoroutine("SnapShotCamera");
			//GameManager.instance.StartCoroutine("RegularSnapShotStart");
			//GameManager.instance.HiddenSign(1);
			return;
		}
		if(col.gameObject.name == "Race2_FinishCheck" ){
			GameManager.instance.FinalRankCompetition();
			//Utility.Log("race2 finish check");
			return;
		}
		
		
		if(col.gameObject.name == "Race2_FinishTrigger" ){

			GameManager.instance.RaceFinishSound();
			GameManager.instance.RaceState = GameManager.GAMESTATE.FINISH;
			GameManager.instance.StartCoroutine("RaceFinishTrigger");
			GameManager.instance.mgrgui.FinishEffect();
			return;
		}
		

		if(col.gameObject.name == "ChangeCam_toNormal"){
			GameManager.instance.isDrag = false;
			return;
		}
		if(col.gameObject.name == "ChangeCam_toDrag"){
			GameManager.instance.isDrag = true;
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_2_1" ){
			GameManager.instance.SwitchingCamera("Cam_R2_Finish_2");
			GameManager.instance.GameRaceStateChange("race02finish");
			//GameManager.instance.StartCoroutine("SnapShotCamera");
			GameManager.instance.HiddenSign(1);
		//	GameManager.instance.StartCoroutine("RegularSnapShotStart");
			GameManager.instance.StartCoroutine("RegularFinishCam_2_1");
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_2_2" ){
			GameManager.instance.StartCoroutine("RegularFinishCam_2_2");
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_2_3" ){
			GameManager.instance.StartCoroutine("RegularFinishCam_2_3");


			return;
		}
	}
}
