using UnityEngine;
using System.Collections;

public class InvenCarInfo : MonoBehaviour {
	
	
	void Start(){
		transform.FindChild("btnSell").FindChild("lbTitle").GetComponent<UILabel>().text = KoStorage.GetKorString("75004");// "";
		transform.FindChild("btnDisassemble").FindChild("lbTitle").GetComponent<UILabel>().text = KoStorage.GetKorString("75008");
			transform.FindChild("btnSelect").FindChild("lbTitle").GetComponent<UILabel>().text = KoStorage.GetKorString("76307");
		transform.FindChild("btnSetTeam").GetComponent<UIButtonMessage>().functionName = "OnTeamChange";
		transform.FindChild("btnSetTeam").FindChild("lbTitle").GetComponent<UILabel>().text = KoStorage.GetKorString("76021");
	}



	
	void ChangeCarInfo(){
		//Utility.UtLogW("ChangeCarInfo " +gameObject.name);
		string[] names = gameObject.name.Split('_');
		int carid = int.Parse(names[1]);
		int teamcode = int.Parse(names[3]);
		
		Common_Car_Status.Item item = Common_Car_Status.Get(carid);
		if(teamcode == 0){
			transform.FindChild("btnSell").gameObject.SetActive(false);
			transform.FindChild("btnDisassemble").gameObject.SetActive(true);
			//int teamReqLV = Common_Team.Get(GV.SelectedTeamID).ReqLV;
			if(item.ReqLV > GV.ChSeason){
					transform.FindChild("btnSelect").gameObject.SetActive(false);
			}else{
					transform.FindChild("btnSelect").gameObject.SetActive(true);
			}

			if(CClub.ClubMode == 1){
					bool b=false;
					for(int i = 0; i < 3; i++){
						if(GV.SelectedTeamID == CClub.ClubRaceTeams[i]){
							b = true;
							break;
						}
					}

				if(b) {
					//transform.FindChild("btnSelect").gameObject.SetActive(false);
					int teamCnt = GV.listmyteaminfo.Count;
					int clubCnt = 0;
					for(int i=0; i < 3; i++){
						if(CClub.ClubRaceTeams[i] != 0){
							clubCnt++;
						}
					}
					if(teamCnt <= clubCnt) {
						transform.FindChild("btnSelect").gameObject.SetActive(true);
					}else{
						if(clubCnt == 3) transform.FindChild("btnSelect").gameObject.SetActive(true);
						else transform.FindChild("btnSelect").gameObject.SetActive(false);
					}
				}else{
					transform.FindChild("btnSelect").gameObject.SetActive(true);
				}
			}
			transform.FindChild("btnSetTeam").gameObject.SetActive(false);
			transform.FindChild("TeamInfo").gameObject.SetActive(false);
		}else{
			transform.FindChild("btnSell").gameObject.SetActive(false);
			transform.FindChild("btnDisassemble").gameObject.SetActive(false);
			transform.FindChild("btnSelect").gameObject.SetActive(false);
			if(GV.SelectedTeamID == teamcode) {
				transform.FindChild("btnSetTeam").gameObject.SetActive(false);
			}
			else{
				transform.FindChild("btnSetTeam").gameObject.SetActive(true);
			}

			transform.FindChild("TeamInfo").gameObject.SetActive(true);
			transform.FindChild("TeamInfo").FindChild("lbTitle").GetComponent<UILabel>().text = Common_Team.Get(teamcode).Name;
			transform.FindChild("TeamInfo").FindChild("icon_team").GetComponent<UISprite>().spriteName = Common_Team.Get(teamcode).Crew.ToString()+"A";

		}
		
		CarInfo carInfo = GV.GetMyCarInfo(carid, names[2], 0);
		transform.FindChild("lbPower").GetComponent<UILabel>().text = GV.MyCarAbilityStats(carid, names[2], 0).ToString();
		transform.FindChild("lbName").GetComponent<UILabel>().text = item.Name;
		transform.FindChild("lbClass").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74024"), names[2]);// "";
		transform.FindChild("lbReqLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74027"), item.ReqLV);
		transform.FindChild("lbStarLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74028"), carInfo.carClass.StarLV);
		transform.FindChild("lbGearLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74029"), item.GearLmt+carInfo.carClass.GearLmt);
	}
	
	
	public void ChangeCarSubInfo(){
		transform.FindChild("btnSell").gameObject.SetActive(false);
		transform.FindChild("btnDisassemble").gameObject.SetActive(false);
			transform.FindChild("btnSelect").gameObject.SetActive(false);
		transform.FindChild("btnSetTeam").gameObject.SetActive(false);
		transform.FindChild("TeamInfo").gameObject.SetActive(true);
		transform.FindChild("TeamInfo").FindChild("lbTitle").GetComponent<UILabel>().text = Common_Team.Get(GV.SelectedTeamID).Name;
		transform.FindChild("TeamInfo").FindChild("icon_team").GetComponent<UISprite>().spriteName = Common_Team.Get(GV.SelectedTeamID).Crew.ToString()+"A";

	}

	public void ChangeCarSubInfo2(){
	//	transform.FindChild("btnSell").gameObject.SetActive(false);
	//	transform.FindChild("btnDisassemble").gameObject.SetActive(false);
	//	transform.FindChild("btnSelect").gameObject.SetActive(false);
		transform.FindChild("btnSetTeam").gameObject.SetActive(false);
		transform.FindChild("TeamInfo").gameObject.SetActive(true);
		transform.FindChild("TeamInfo").FindChild("lbTitle").GetComponent<UILabel>().text = Common_Team.Get(GV.SelectedTeamID).Name;
		transform.FindChild("TeamInfo").FindChild("icon_team").GetComponent<UISprite>().spriteName = Common_Team.Get(GV.SelectedTeamID).Crew.ToString()+"A";

	}


}
