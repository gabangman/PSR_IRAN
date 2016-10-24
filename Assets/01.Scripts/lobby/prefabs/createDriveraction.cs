using UnityEngine;
using System.Collections;

public class createDriveraction : MonoBehaviour {

	public void MakeDriverCrew(GameObject Car){
		if(Car == null) return;
		int id = 0;
	//	if(Global.gRaceInfo.raceType == RaceType.DailyMode) 
	//		id =Base64Manager.instance.GlobalEncoding(Global.gDailyCrewID);
	//	else {
	//		id = Base64Manager.instance.GlobalEncoding(Global.MyCrewID);
	//	}
		id = GV.PlayCrewID;

		makeDrivercrew(id , Car, 10);
	}

	public void ChangeDriverAnimation(){
		transform.GetChild(0).FindChild("Driver_Axis").GetChild(0).GetComponent<textureaction>().ChangeAnimation();
	}

	void makeDrivercrew(int _id, GameObject Car, int num){
		var dr = Car.transform.GetChild(0).FindChild("Driver_Axis") as Transform;
		if(dr.childCount == 0){
			
		}else{
			//DestroyImmediate(dr.GetChild(0).gameObject);
		}
		string _name = _id.ToString() + "_5100" +"_"+num.ToString();
		var driver = ObjectManager.GetSelectObjectWithAIRace(_name);
		if(num == 10) {
			//int tempid = Base64Manager.instance.GlobalEncoding(GV.PlayCrewID);
			int tempid = GV.PlayCrewID;
			_name = tempid.ToString()+"_5100";
		}
		else _name = Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[num].AICrewID).ToString()+"_5100";
		//Utility.Log(_name);
		driver.name = _name; 

		ObjectManager.ChangeObjectParent(driver, dr);
		ObjectManager.ChangeObjectPosition(driver, Vector3.zero, Vector3.one, new Vector3(0,90,0));
		driver.SetActive(true);
	}

	public void AIMakeDriverCrew(GameObject Car, int num){
		if(Car == null) return;
		int id = Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[num].AICrewID);
		makeDrivercrew(id, Car, num);
		AIChangeDriverAnimation();
		Destroy(this, 0.1f);
	}
	
	public void AIChangeDriverAnimation(){
		transform.GetChild(0).FindChild("Driver_Axis").GetChild(0).GetComponent<textureaction>().ChangeAnimation();
	}
}
