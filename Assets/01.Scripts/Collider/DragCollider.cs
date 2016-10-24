using UnityEngine;
using System.Collections;

public class DragCollider : MonoBehaviour {

	void Start(){
		/*string[] Trigger = new string[]{"Race1_FinishCam_1" ,"Race1_FinishCam_2","Race1_FinishCheck"
			,"Race1_FinishTrigger"};*/
	}
	void OnTriggerEnter(Collider col){


		if(col.gameObject.name.Equals("Race1_FinishTrigger")){
			GameManager.instance.StartCoroutine("DragFinishTrigger");
			GameManager.instance.RaceFinishSound();
			GameManager.instance.RaceState = GameManager.GAMESTATE.FINISH; 
			GameManager.instance.mgrgui.FinishEffect();
			//Utility.LogWarning("Race1_FinishTrigger");
			return;
		}

		if(col.gameObject.name == "Race1_FinishCam_1" ){
			string str = string.Empty;
			if(Global.DragSub == 0){ //short
				str = "Cam_R1_Finish_1";
			}else{ //long
				str = "Cam_R2_Finish_1";
			} 
			//Utility.LogWarning("Race1_FinishCam_1");
			GameManager.instance.SwitchingCamera(str);
			GameManager.instance.HiddenGUI();
			GameManager.instance.SetBGM_Drag();
			GameManager.instance.RaceState = GameManager.GAMESTATE.RACE02FINISH;
			GameManager.instance.QuitCnt = 0;
			return;
		}
		if(col.gameObject.name == "Race1_FinishCam_2" ){
			return;
			/*string str = string.Empty;
			if(Global.DragSub == 0){ //short
				str = "Cam_R1_Finish_2";
			}else{ //long
				str = "Cam_R2_Finish_2";
			} 
			Utility.LogWarning("Race1_FinishCam_2");
			GameManager.instance.SwitchingCamera(str);
			GameManager.instance.GameRaceStateChange("race01end_Drag");
			//GameManager.instance.StartCoroutine("SnapShotCameraDrag");
			GameManager.instance.StartCoroutine("DragSnapShotStart");
			GameManager.instance.HiddenSign(1);
			return;*/
		}
		if(col.gameObject.name == "Race1_FinishCheck" ){
			GameManager.instance.FinalRankCompetition_Drag();
			return;
		}
		if(col.gameObject.name == "Race1_FinishCam_2_1" ){
			string str = string.Empty;
			if(Global.DragSub == 0){ //short
				str = "Cam_R1_Finish_2";
			}else{ //long
				str = "Cam_R2_Finish_2";
			} 
			Utility.LogWarning("Race1_FinishCam_2_1");
			GameManager.instance.SwitchingCamera(str);
			GameManager.instance.GameRaceStateChange("race01end_Drag");
			//GameManager.instance.StartCoroutine("SnapShotCameraDrag");
			GameManager.instance.HiddenSign(1);
		//	GameManager.instance.StartCoroutine("DragSnapShotStart");
			GameManager.instance.StartCoroutine("DragFinishCam_2_1");
			return;
		}
		if(col.gameObject.name == "Race1_FinishCam_2_2" ){
			GameManager.instance.StartCoroutine("DragFinishCam_2_2");
			return;
		}
		if(col.gameObject.name == "Race1_FinishCam_2_3" ){
			GameManager.instance.StartCoroutine("DragFinishCam_2_3");
			return;
		}
	}



}
