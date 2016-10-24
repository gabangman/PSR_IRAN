using UnityEngine;
using System.Collections;

public class RaceWindowControl : MonoBehaviour {

	public void AllModeDisable(){
		for(int i = 0; i < transform.childCount; i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}


	public void EnableOnMap(bool show, string modeName){
		return;
		var temp = transform.FindChild("RaceMap").gameObject as GameObject;
		temp.SetActive(show);
		switch(modeName){
		case "Race":
			temp.transform.FindChild("map_Weekly").gameObject.SetActive(false);
			temp.transform.FindChild("map_Regular").gameObject.SetActive(true);
			break;
		case "Champion":
			break;
		case "Clan":
			break;
		case "Weekly":
			temp.transform.FindChild("map_Weekly").gameObject.SetActive(true);
			temp.transform.FindChild("map_Regular").gameObject.SetActive(false);
			break;
		}
	
	}

}
