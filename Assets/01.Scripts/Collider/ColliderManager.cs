using UnityEngine;
using System.Collections;

public class ColliderManager : MonoBehaviour {
	

	private bool isRace02 = false; 
	private GameObject colObject;
	// Use this for initialization

	void Awake(){

		if(Global.isTutorial)
		{
			Global.gRaceInfo.sType = SubRaceType.RegularRace;
			gameObject.AddComponent<TutorialCollider>();
			Destroy(this);
			return;
		}

		switch(Global.gRaceInfo.sType){
		case SubRaceType.DragRace:
			gameObject.AddComponent<DragCollider>();
			break;
		case SubRaceType.RegularRace:
			if(Global.gChampTutorial == 0){
				gameObject.AddComponent<StockCollider>();
			}else{
				gameObject.AddComponent<ChampTutorial>();
				Destroy(this);
			}
		
			break;
		case SubRaceType.CityRace:
			gameObject.AddComponent<EventCollider>();
			break;
		}
	}

}
	

