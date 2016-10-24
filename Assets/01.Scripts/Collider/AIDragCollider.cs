using UnityEngine;
using System.Collections;

public class AIDragCollider : MonoBehaviour {

	AICarCtrl carMgr;
	public void SettingCollider(GameObject obj){
		carMgr = obj.GetComponent<AICarCtrl>();
	}
	void Start(){
	/*	string[] Trigger = new string[]{"Race1_FinishCam"
			,"Race1_FinishTrigger"};*/
		
	}
	void OnTriggerEnter(Collider col){

		if(col.gameObject.name.Equals("Race1_FinishTrigger")){
			//carMgr._raceState = AICarCtrl.RaceState.FINISH;
			//carMgr.StopAICarRace();
			carMgr.StopR2RaceFinish();
			return;
		}
		
		if(col.gameObject.name.Equals("Race1_FinishCam")){
			return;
		}

	

	}

}
