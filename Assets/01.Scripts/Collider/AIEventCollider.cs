using UnityEngine;
using System.Collections;

public class AIEventCollider : MonoBehaviour {

	AICarCtrl carMgr;
	bool isRace = true;
	public void SettingCollider(GameObject obj){
		carMgr = obj.GetComponent<AICarCtrl>();
	}
	void Start(){
		/*string[] Trigger = new string[]{"Race1_FinishCam_1"
			,"Race1_FinishTrigger","Race2_FinishCam_1", "Race2_FinishCam_2",
		"Race2_FinishTrigger", "Race2_FinishCheck", "ChangeCam_toNormal", "ChangeCam_toDrag"};
		*/
	}
	void OnTriggerEnter(Collider col){
		
		if(col.gameObject.name == "Race1_FinishCam_1" ){
			return;
		}
		
		if(col.gameObject.name == "Race1_FinishTrigger"){
		
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_1" ){
			return;
		}
		if(col.gameObject.name == "Race2_FinishCam_2" ){
			return;
		}



		
		
		if(col.gameObject.name == "Race2_FinishTrigger" ){
			carMgr.StopR2RaceFinish();
			return;
		}
		
		/*
		if(col.gameObject.name == "ChangeCam_toNormal"){
			return;
		}
		if(col.gameObject.name == "ChangeCam_toDrag"){
			return;
		}*/
	}
}
