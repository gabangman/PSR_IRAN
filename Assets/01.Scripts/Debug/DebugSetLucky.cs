using UnityEngine;
using System.Collections;

public class DebugSetLucky : MonoBehaviour {

	public UILabel[] lbCars;

	void OnEnable(){
		CrewInfo crew = GV.getTeamCrewInfo(GV.SelectedTeamID);
		lbCars[3].text = crew.driverLv.ToString();
	}

	protected void OnOk(){
		int carID = int.Parse(lbCars[0].text);
		if(carID >=1000 && carID < 1025){

		}else return;

		string carClass = lbCars[1].text;
		bool b = false;
		switch(carClass){
		case "D": break;
		case "C": break;
		case "B": break;
		case "A": break;
		case "S": break;
		case "SS": break;
		default : b = true; break;
		}
		if(b) return;

		int carLV = int.Parse(lbCars[2].text);
		if(carLV <=0 && carLV > 26) return;
	
		int crewLV = int.Parse(lbCars[3].text);
		if(crewLV <=0 && crewLV > 5) return;
		CrewInfo crew = GV.getTeamCrewInfo(GV.SelectedTeamID);
		if(crewLV != crew.chiefLv) GV.LuckyCrewLV = crewLV;
		else GV.LuckyCrewLV = 0;

		GV.LuckyCarClick = 1;
		GV.LuckyCarID = carID;
		GV.LuckyCarClass = carClass;
		GV.LuckyCarLV = carLV;

		gameObject.SetActive(false);
	}

	protected void OnCancle(){
		GV.LuckyCarClick = 0;
		gameObject.SetActive(false);
	}

}
