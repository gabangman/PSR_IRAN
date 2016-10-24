using UnityEngine;
using System.Collections;

public class TopTeamInfo : MonoBehaviour {
	public GameObject Info1, info2;
	public GameObject[] info2s;

	void Start(){
		if(string.IsNullOrEmpty(GV.UserNick) == true){
			GV.UserNick=EncryptedPlayerPrefs.GetString("mNick");
		}
		Info1.transform.FindChild("lbName").GetComponent<UILabel>().text = 
			GV.UserNick;
	}

	void ChangeTeamInfo1(){
		//Utility.LogWarning("TopMenu Change " + GV.ChSeasonLV);
		Info1.transform.FindChild("lbName").GetComponent<UILabel>().text = 
			GV.UserNick;

		var tr = Info1.transform.FindChild("Flag") as Transform;
		if(GV.ChSeasonID >= 6030){
			//GV.ChSeasonID = 6030;
			tr.gameObject.SetActive(false);
			Info1.transform.FindChild("Flag_Shadow").gameObject.SetActive(false);
			Info1.transform.FindChild("lbSeasonInfo").gameObject.SetActive(true);
			Info1.transform.FindChild("lbSeasonInfo").GetComponent<UILabel>().text = KoStorage.GetKorString("71015");
		}else{
			tr.gameObject.SetActive(true);
			for(int i =0; i < tr.childCount;i++){
				if((GV.ChSeasonLV-1) > i) tr.GetChild(i).gameObject.SetActive(true);
				else tr.GetChild(i).gameObject.SetActive(false);
			}
		}
	}

	void ChangeTeamInfo2(){
		string name = null;
		name = Common_Car_Status.Get(GV.getTeamCarID(GV.SelectedTeamID)).Name;
		info2s[0].GetComponent<UILabel>().text =name;
		name = Common_Crew_Status.Get(GV.getTeamCrewID(GV.SelectedTeamID)).Name;
		info2s[1].GetComponent<UILabel>().text = name;//"TourCrew";
		info2s[2].GetComponent<UILabel>().text =string.Format("{0:000}", CarAbility());
		info2s[3].GetComponent<UILabel>().text = string.Format("{0:000}", CrewAbility());
		string strClass = GV.getTeamCarClass(GV.SelectedTeamID);
		info2s[4].GetComponent<UILabel>().text = strClass;
		info2s[4].transform.parent.FindChild("Panel_class").GetComponent<UISprite>().spriteName = "Class_"+strClass;
	}

	int CarAbility(){
		CTeamAbility _ability = new CTeamAbility();
		int crewid = GV.getTeamCarID(GV.SelectedTeamID);
		int a  = _ability.TeamCarAbility(crewid);
		GV.CarAbility = a;
		return  a;//_ability.CarAbility(crewid, idx);
	}
	
	int CrewAbility(){
		CTeamAbility _ability = new CTeamAbility();
		int crewid = GV.getTeamCrewID(GV.SelectedTeamID);
		int a = _ability.TeamCrewAbility(crewid);
		GV.CrewAbility = a;
		return  a;//_ability.CrewAbility(crewid, idx);
	}


	int CarAbility(int idx){
		CTeamAbility _ability = new CTeamAbility();
		int crewid = 0;
		crewid = GV.getTeamCarID(GV.SelectedTeamID);
		int a  = _ability.CarAbility(crewid, idx);
		GV.CarAbility = a;
		return  a;//_ability.CarAbility(crewid, idx);
	}

	int CrewAbility(int idx){
		CTeamAbility _ability = new CTeamAbility();
		int crewid = 0;
	//	if(idx == 0) crewid = GV.FindStockCrew();
	//	else crewid = GV.FindTourCrew();
		crewid = GV.getTeamCrewID(GV.SelectedTeamID);
		int a = _ability.CrewAbility(crewid, idx);
		GV.CrewAbility = a;
		return  a;//_ability.CrewAbility(crewid, idx);
	}

	void ChangeCarTeamAbilityCount(int x){
		StartCoroutine("CarAbilityCount",x);
	}

	void ChangeCrewAbilityCount(int x){
		StartCoroutine("CrewAbilityCount",x);
	}

	IEnumerator CarAbilityCount(int idx){

		CTeamAbility _ability = new CTeamAbility();
		int carid = 0;
		carid = GV.getTeamCarID(GV.SelectedTeamID);
		int _ab  = _ability.CarAbility(carid, idx);
		int carA = GV.CarAbility;

		if(carA == _ab) yield break;
		GV.CarAbility = _ab;
		int count = carA;
		float delay1 = 1/(float)( _ab-count);
		var lbText = info2s[2].GetComponent<UILabel>() as UILabel;//.text= string.Format("{0}",Global.gCarAbility);
		for(;;){
			count+=1;
			lbText.text = string.Format("{0}",count);
			if(_ab<= count){

				lbText.text = string.Format("{0}",_ab);
				yield break;
			}
			yield return new WaitForSeconds(delay1);
		}		
	}
	IEnumerator CrewAbilityCount(int idx){
		int _ab =0;
		int crewA = GV.CrewAbility;

		CTeamAbility _ability = new CTeamAbility();
		int crewid = 0;
		crewid = GV.getTeamCrewID(GV.SelectedTeamID);
		_ab = _ability.CrewAbility(crewid, idx);
	//	Utility.LogWarning(_ab + "  " + crewA);
		if(crewA == _ab) yield break;
		GV.CrewAbility = _ab;
		int count = crewA;
		float delay1 = 1/(float)( _ab-count);
		var lbText = info2s[3].GetComponent<UILabel>() as UILabel;
		for(;;){
			count+=1;
			lbText.text = string.Format("{0}",count);
			if(_ab <= count){
			
				lbText.text = string.Format("{0}",_ab);
				yield break;
			}
			yield return new WaitForSeconds(delay1);
		}		
	}
	

}
