using UnityEngine;
using System.Collections;

public class ClubWarTeamItem : MonoBehaviour {
	private int raceTeamId;
	private int currentTeamId;
	public void ViewClanWarResultInfo(ClubRaceMemberInfoDetail mDetail){
	
	}

	public void ViewClanWarTeamInfo(int TeamId){
		var tr = transform.GetChild(0) as Transform;
		for(int i = 1; i < tr.childCount;i++){
			tr.GetChild(i).gameObject.SetActive(false);
		}
		int Count = CClub.myClubRaceInfoDetail.Count;
		int detailId = -1;
		currentTeamId = TeamId;
		raceTeamId = 0;
		for(int i =0; i < Count; i++){
			if(CClub.myClubRaceInfoDetail[i].teamId == TeamId){
				detailId = i;
				break;
			}
		}

		if(detailId == -1){
			CarInfo car = GV.getTeamCarInfo(TeamId);
			if(car == null){
				ChangeRaceNotCar();
			}else{
			//	int  carinfoidx = GV.getTeamCarInfo(TeamId).CarIndex;
			//	for(int i =0; i < Count; i++){
			//		if(CClub.myClubRaceInfoDetail[i].carIdx == carinfoidx){
			//			detailId = i;
			//			break;
			//		}
			//	}
			
			//	if(detailId == -1){
			//		raceTeamId = TeamId;
			//		ChangeRaceReady(TeamId);
			//	}else{
			//		raceTeamId = -1;
			//		ChangeNotRace();
			//	}
					raceTeamId = TeamId;
					ChangeRaceReady(TeamId);
			}
		}else{
			raceTeamId = 0;
			ChangeRaceResult(CClub.myClubRaceInfoDetail[detailId]);
		}
	}

	public void ChangeRaceResult(ClubRaceMemberInfoDetail m){
		var tr = transform.GetChild(0).FindChild("ClubRaceComplete") as Transform;
		tr.gameObject.SetActive(true);
		//==!!Utility.LogWarning("m.crewId " + m.crewId);
		tr = tr.FindChild("Info_Team");
		tr.FindChild("icon_car").GetComponent<UISprite>().spriteName = m.carId.ToString();
		tr.FindChild("icon_crew1").GetComponent<UISprite>().spriteName = m.crewId.ToString()+"A";
		tr.FindChild("lbName_Crew").GetComponent<UILabel>().text =Common_Crew_Status.Get(m.crewId).Name;
		tr.parent.FindChild("Result").FindChild("lbStarNum").GetComponent<UILabel>().text = string.Format("X{0}", m.thisRaceEarnedStarCount);
	}

	public void ChangeRaceReady(int teamId){
		var tr = transform.GetChild(0).FindChild("info_Car_Yes") as Transform;
		tr.gameObject.SetActive(true);
		var mCar = tr.FindChild("Info_Car") as Transform;
		var mCrew = tr.FindChild("Info_Crew") as Transform;
		var mSpon = tr.FindChild("Info_Spon") as Transform;
		myTeamInfo mTeam = GV.getTeamTeamInfo(currentTeamId);
		int carid = mTeam.TeamCarID;
		Common_Car_Status.Item carItem;
		Common_Crew_Status.Item crewItem;
		carItem = Common_Car_Status.Get(carid);
		mCar.FindChild("icon_car").GetComponent<UISprite>().spriteName = carid.ToString();
		mCar.FindChild("lbName_Car").GetComponent<UILabel>().text = carItem.Name;
		mCar.FindChild("lbAble_Car").GetComponent<UILabel>().text = CarAbility(carid , mTeam.TeamCode).ToString();
		mCar.FindChild("lbClass_Car").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74024"), mTeam.myCar.ClassID);
		int crewid = mTeam.TeamCrewID;
		crewItem = Common_Crew_Status.Get(crewid);
		
		mCrew.FindChild("lbName_Crew").GetComponent<UILabel>().text = crewItem.Name;
		mCrew.FindChild("lbAble_Crew").GetComponent<UILabel>().text = CrewAbility(crewid , mTeam.TeamCode).ToString();
		mCrew.FindChild("icon_crew").GetComponent<UISprite>().spriteName = crewid.ToString()+"A";
		
		if(mTeam.SponID <= 1300){
			mSpon.FindChild("icon_spon").gameObject.SetActive(false);//GetComponent<UISprite>().spriteName =myteam.SponID.ToString();
			mSpon.FindChild("lbName_spon").GetComponent<UILabel>().text = KoStorage.GetKorString("72020");
		}else{

				System.DateTime nowTime = NetworkManager.instance.GetCurrentDeviceTime();
				System.TimeSpan pTime = new System.DateTime(mTeam.SponRemainTime) -nowTime;
				if(pTime.TotalHours >0){
					mSpon.FindChild("icon_spon").gameObject.SetActive(true);
					mSpon.FindChild("icon_spon").GetComponent<UISprite>().spriteName =mTeam.SponID.ToString();
					mSpon.FindChild("lbName_spon").GetComponent<UILabel>().text =  string.Format(KoStorage.GetKorString("72019"), Common_Sponsor_Status.Get(mTeam.SponID).Name);
				}else{
					mSpon.FindChild("icon_spon").gameObject.SetActive(false);//GetComponent<UISprite>().spriteName =myteam.SponID.ToString();
					mSpon.FindChild("lbName_spon").GetComponent<UILabel>().text = KoStorage.GetKorString("72020");
					mTeam.SponID = 1300;
				}
		}
	}

	public void ChangeRaceNotCar(){
		var tr = transform.GetChild(0).FindChild("info_Car_No") as Transform;
		tr.gameObject.SetActive(true);
		myTeamInfo mTeam = GV.getTeamTeamInfo(currentTeamId);
		bool b = true;
		var mCar = tr.FindChild("Info_Car") as Transform;
		var mCrew = tr.FindChild("Info_Crew") as Transform;
		if(b){
			mCar.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("76033");
		}else{
			mCar.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("76034");
		}
		int crewid = mTeam.myCrews.crewID;
		Common_Crew_Status.Item crewItem = Common_Crew_Status.Get(crewid);
		
		mCrew.FindChild("lbName_Crew").GetComponent<UILabel>().text = crewItem.Name;
		mCrew.FindChild("lbAble_Crew").GetComponent<UILabel>().text = CrewAbility(crewid ,currentTeamId).ToString();
		mCrew.FindChild("icon_crew").GetComponent<UISprite>().spriteName = crewid.ToString()+"A";

	}

	public void ChangeNotRace(){
		var tr = transform.GetChild(0).FindChild("ClubChangeCar") as Transform;
		tr.gameObject.SetActive(true);
		tr = transform.FindChild("info_Car_Yes");
		tr.gameObject.SetActive(true);

		var mCar = tr.FindChild("Info_Car") as Transform;
		var mCrew = tr.FindChild("Info_Crew") as Transform;
		var mSpon = tr.FindChild("Info_Spon") as Transform;
		myTeamInfo mTeam = GV.getTeamTeamInfo(currentTeamId);
		int carid = mTeam.TeamCarID;
		Common_Car_Status.Item carItem;
		Common_Crew_Status.Item crewItem;
		carItem = Common_Car_Status.Get(carid);
		mCar.FindChild("icon_car").GetComponent<UISprite>().spriteName = carid.ToString();
		mCar.FindChild("lbName_Car").GetComponent<UILabel>().text = carItem.Name;
		mCar.FindChild("lbAble_Car").GetComponent<UILabel>().text = CarAbility(carid , mTeam.TeamCode).ToString();
		mCar.FindChild("lbClass_Car").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74024"), mTeam.myCar.ClassID);
		int crewid = mTeam.TeamCrewID;
		crewItem = Common_Crew_Status.Get(crewid);
		
		mCrew.FindChild("lbName_Crew").GetComponent<UILabel>().text = crewItem.Name;
		mCrew.FindChild("lbAble_Crew").GetComponent<UILabel>().text = CrewAbility(crewid , mTeam.TeamCode).ToString();
		mCrew.FindChild("icon_crew").GetComponent<UISprite>().spriteName = crewid.ToString()+"A";
		
		if(mTeam.SponID <= 1300){
			mSpon.FindChild("icon_spon").gameObject.SetActive(false);//GetComponent<UISprite>().spriteName =myteam.SponID.ToString();
			mSpon.FindChild("lbName_spon").GetComponent<UILabel>().text = KoStorage.GetKorString("72020");
		}else{
			mSpon.FindChild("icon_spon").gameObject.SetActive(true);
			mSpon.FindChild("icon_spon").GetComponent<UISprite>().spriteName =mTeam.SponID.ToString();
			mSpon.FindChild("lbName_spon").GetComponent<UILabel>().text =  string.Format(KoStorage.GetKorString("72019"), Common_Sponsor_Status.Get(mTeam.SponID).Name);
		}


	}
	public void OnStockClick(){
		var tr = transform.GetChild(0).FindChild("SelectInfo").gameObject as GameObject;
		if(tr.activeSelf) return;
		NGUITools.FindInParents<ClubRace_Next>(gameObject).SetSelectInfo(transform.name,raceTeamId, currentTeamId);
		tr.gameObject.SetActive(true);
	}

	public void UnSelectSelectInfo(){
		var tr = transform.GetChild(0).FindChild("SelectInfo").gameObject as GameObject;
		if(tr.activeSelf) tr.gameObject.SetActive(false);
	}



	int CarAbility(int crewid, int idx){
		CTeamAbility _ability = new CTeamAbility();
		int a  = _ability.CarAbility(crewid, idx, 1);
		_ability = null;
		//	//==!!Utility.LogWarning("CarAbility 0 " + a + " " + crewid);
		return  a;//_ability.CarAbility(crewid, idx);
	}
	
	int CrewAbility(int carid, int idx){
		CTeamAbility _ability = new CTeamAbility();
		int a = _ability.CrewAbility(carid, idx, 1);
		_ability = null;
		return  a;
	}
}
