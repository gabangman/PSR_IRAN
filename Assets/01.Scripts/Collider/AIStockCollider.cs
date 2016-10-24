using UnityEngine;
using System.Collections;

public class AIStockCollider : MonoBehaviour {

	AICarCtrl carMgr;
	bool isRace = true;
	public void SettingCollider(GameObject obj){
		carMgr = obj.GetComponent<AICarCtrl>();
	}
	void Start(){
	/*	string[] Trigger = new string[]{"Race1_StartTrigger","Race1_FinishCam_1"
			,"Race1_FinishTrigger","Race2_FinishCam_1", "Race2_FinishCam_2",
			"Race2_FinishTrigger", "Race2_FinishCheck", "Ready_2", "Ready_3","Ready_Count"};
		*/
	}
	void OnTriggerEnter(Collider col){
		
		if(col.gameObject.name == "Ready_3" && isRace ){
			carMgr._raceState =  AICarCtrl.RaceState.READY;
			return;
		}
		
		if(col.gameObject.name == "Race1_StartTrigger"  && isRace){
			carMgr.RaceStart();
			return;
		}
		if(col.gameObject.name == "Race1_FinishCam_1" && isRace ){
			return;
		}
		
		if(col.gameObject.name == "Race1_FinishTrigger" && isRace ){
			isRace = false;
			return;
		}
		
		if(col.gameObject.name == "Race2_FinishCam_2" && !isRace ){
			return;
		}


	
		
		if(col.gameObject.name == "Race2_FinishTrigger" && !isRace ){
			if(!carMgr.isRace02) return;
			carMgr.StopR2RaceFinish();
			return;
		}
	}
}
