using UnityEngine;
using System.Collections;

public class AIColliderManager : MonoBehaviour {

	AICarCtrl _carCtrl;
	bool isRace = true;
	public void InitAssign(GameObject _obj){
		_carCtrl = _obj.GetComponent<AICarCtrl>();
		/*switch(Global.gRaceInfo.modeType){
		case RaceModeType.DragMode:
			gameObject.AddComponent<AIDragCollider>().SettingCollider(_obj);
			break;
		case RaceModeType.StockMode:
			gameObject.AddComponent<AIStockCollider>().SettingCollider(_obj);
			break;
		case RaceModeType.TouringMode:
			gameObject.AddComponent<AITouringCollider>().SettingCollider(_obj);
			break;
		case RaceModeType.EventMode:
			gameObject.AddComponent<AIEventCollider>().SettingCollider(_obj);
			break;
		}*/

		switch(Global.gRaceInfo.sType){
		case SubRaceType.DragRace:
			gameObject.AddComponent<AIDragCollider>().SettingCollider(_obj);
			break;
		case SubRaceType.RegularRace:
			gameObject.AddComponent<AIStockCollider>().SettingCollider(_obj);
			break;
		case SubRaceType.CityRace:
			gameObject.AddComponent<AIEventCollider>().SettingCollider(_obj);
			break;
		}



		Destroy(this);
	
	}
	void OnTriggerEnter(Collider col){

		if(col.gameObject.name == "Ready_3" && isRace ){
			//AICar.SendMessage("InitAiRaceSprite");
			//AICar.GetComponent<AICarCtrl>()._raceState = AICarCtrl.RaceState.READY;
			_carCtrl._raceState = AICarCtrl.RaceState.READY;
			return;
		}

		if(col.gameObject.name == "Race1_StartTrigger"  && isRace){
			//GameManager.instance.RaceCountCheck(col.gameObject);	
			//AICar.GetComponent<AICarCtrl>()._raceState = AICarCtrl.RaceState.RACE01;
			_carCtrl.RaceStart();
		//	_carCtrl._raceState = AICarCtrl.RaceState.RACE01;
			return;
		}
		if(col.gameObject.name == "Race1_FinishCam_1" && isRace ){
			//GameManager.instance.RaceCountCheck(col.gameObject);	
			return;
		}

		if(col.gameObject.name == "Race1_FinishTrigger" && isRace ){
			isRace = false;
			return;
		}


		if(col.gameObject.name == "Race2_FinishCam_2" && !isRace ){
			//GameManager.instance.RaceCountCheck(col.gameObject);	
			//RankCheck(2);
			//isRace = false;
			//isCheck = false;
			return;
		}

		/*
		if(col.gameObject.name == "Race2_FinishTrigger" && !isRace ){
			//AICar.GetComponent<AICarCtrl>()._raceState = AICarCtrl.RaceState.FINISH;
			//_carCtrl._raceState = AICarCtrl.RaceState.FINISH;
			return;
		}
		*/
		/*if(col.gameObject.name == "Race2_Boost" && !isRace ){
			//_carCtrl.StartCoroutine("BoostEnable");

		
			return;
		}*/
	}
	/*
	void RankCheck(int race){
		int mrank = GameManager.instance.raceCount-1;
		if(race == 1){
			Global.Race01Ranking[mrank] = AICar.name;
			if(GameManager.instance.currentCamera.name == "Cam_R1_Finish" )
				AICar.SendMessage("finishPassSound",SendMessageOptions.DontRequireReceiver);
			Global.Race01Time[mrank] = GameManager.instance._totalTime;	
			AICar.SendMessage("SaveSpriteName", mrank,SendMessageOptions.DontRequireReceiver);
		}else{
			AICar.SendMessage("SaveSpriteName", mrank,SendMessageOptions.DontRequireReceiver);
			Global.Race02Ranking[mrank] = AICar.name;
			Global.Race02Time[mrank] = GameManager.instance._totalTime;
		}
	}

	*/


}
