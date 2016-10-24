using UnityEngine;
using System.Collections;

public class ClubRaceHistoryItems : MonoBehaviour {

	public GameObject[] TeamLists;
	//public GameObject[] mUserInfos;

	void OnEnable(){
		foreach(GameObject mTr in TeamLists){
			mTr.transform.FindChild("Select").gameObject.SetActive(false);
			mTr.transform.gameObject.SetActive(false);
		}
	
	}


	public void OnChangeInfoOther(){

		var mTr = TeamLists[0].transform.parent as Transform;
		var tr = mTr.GetComponent<TweenPosition>() as TweenPosition;
		mTr.gameObject.SetActive(true);
		tr.Reset();
		tr.enabled = true;
		this.OnEnable();
	
		int raceCount = CClub.oppClubRaceInfoDetail.Count;
		TeamLists[0].SetActive(true);
		TeamLists[1].SetActive(true);
		TeamLists[2].SetActive(true);
		if(raceCount== 0){
			TeamLists[0].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Select").gameObject.SetActive(false);
			
			TeamLists[0].transform.FindChild("Userinfo").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("Userinfo").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Userinfo").gameObject.SetActive(false);
			
			TeamLists[0].transform.FindChild("NoRecord").gameObject.SetActive(true);
			TeamLists[1].transform.FindChild("NoRecord").gameObject.SetActive(true);
			TeamLists[2].transform.FindChild("NoRecord").gameObject.SetActive(true);
			
		}else if(raceCount== 1){
			TeamLists[0].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Select").gameObject.SetActive(false);
			
			TeamLists[0].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[1].transform.FindChild("Userinfo").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Userinfo").gameObject.SetActive(false);
			
			ChangeOppUserInfo(0);

			TeamLists[0].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("NoRecord").gameObject.SetActive(true);
			TeamLists[2].transform.FindChild("NoRecord").gameObject.SetActive(true);
			
		}else if(raceCount== 2){
			TeamLists[0].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Select").gameObject.SetActive(false);
			
			TeamLists[0].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[1].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[2].transform.FindChild("Userinfo").gameObject.SetActive(false);

			ChangeOppUserInfo(0);
			ChangeOppUserInfo(1);


			TeamLists[0].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("NoRecord").gameObject.SetActive(true);
			
		}else if(raceCount== 3){
			TeamLists[0].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Select").gameObject.SetActive(false);
			
			TeamLists[0].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[1].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[2].transform.FindChild("Userinfo").gameObject.SetActive(true);
			ChangeOppUserInfo(0);
			ChangeOppUserInfo(1);
			ChangeOppUserInfo(2);
			TeamLists[0].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("NoRecord").gameObject.SetActive(false);
		}


		NGUITools.FindInParents<ClubRace_Start>(gameObject).SetOppNextButton();
	}

	public void ChangeUserInfo(int idx){
		ChangeUser(CClub.myClubRaceInfoDetail[idx],TeamLists[idx]);
	}

	public void ChangeOppUserInfo(int idx){
		ChangeUser(CClub.oppClubRaceInfoDetail[idx],TeamLists[idx]);
	}

	void ChangeUser(ClubRaceMemberInfoDetail mDetail, GameObject mTr){
		var tr = mTr.transform.FindChild("Userinfo") as Transform;
		tr.FindChild("icon_crew").GetComponent<UISprite>().spriteName = mDetail.crewId.ToString()+"A";
		tr.FindChild("icon_car").GetComponent<UISprite>().spriteName = mDetail.carId.ToString();
		tr.FindChild("lbAble_Crew").GetComponent<UILabel>().text = mDetail.crewAbility.ToString();
		tr.FindChild("lbAble_Car").GetComponent<UILabel>().text = mDetail.carAbility.ToString();
		tr.FindChild("lbAble_CarClass").GetComponent<UILabel>().text = string.Format("{0}",GV.ChangeCarClassIDString(mDetail.carClass));
		tr.FindChild("lbStarNum").GetComponent<UILabel>().text = string.Format("X{0}",mDetail.thisRaceEarnedStarCount);
	
		if(mDetail.thisRaceEarnedStarCount == 0){
			tr.FindChild("lbStarNum").GetComponent<UILabel>().color = Color.red;
		}else{
			tr.FindChild("lbStarNum").GetComponent<UILabel>().color = Color.white;
		}
	
	}


	public void OnChangeInfoMy(){
		var mTr = TeamLists[0].transform.parent as Transform;
		var tr = mTr.GetComponent<TweenPosition>() as TweenPosition;
		mTr.gameObject.SetActive(true);
		tr.Reset();
		tr.enabled = true;
	
		int raceCount = CClub.myClubRaceInfoDetail.Count;
		TeamLists[0].SetActive(true);
		TeamLists[1].SetActive(true);
		TeamLists[2].SetActive(true);
		if(raceCount== 0){
			TeamLists[0].transform.FindChild("Select").gameObject.SetActive(true);
		//	TeamLists[0].transform.FindChild("Select").GetComponentInChildren<UILabel>().text  = string.Empty;
			TeamLists[1].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Select").gameObject.SetActive(false);

			TeamLists[0].transform.FindChild("Userinfo").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("Userinfo").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Userinfo").gameObject.SetActive(false);

			TeamLists[0].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("NoRecord").gameObject.SetActive(true);
			TeamLists[2].transform.FindChild("NoRecord").gameObject.SetActive(true);

		}else if(raceCount== 1){
			TeamLists[0].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("Select").gameObject.SetActive(true);
		//	TeamLists[1].transform.FindChild("Select").GetComponentInChildren<UILabel>().text  = string.Empty;
			TeamLists[2].transform.FindChild("Select").gameObject.SetActive(false);

			TeamLists[0].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[1].transform.FindChild("Userinfo").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Userinfo").gameObject.SetActive(false);
			ChangeUserInfo(0);
		
			TeamLists[0].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("NoRecord").gameObject.SetActive(true);

		}else if(raceCount== 2){
			TeamLists[0].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Select").gameObject.SetActive(true);
		//	TeamLists[2].transform.FindChild("Select").GetComponentInChildren<UILabel>().text  = string.Empty;

			TeamLists[0].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[1].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[2].transform.FindChild("Userinfo").gameObject.SetActive(false);
			ChangeUserInfo(0);
			ChangeUserInfo(1);

			TeamLists[0].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("NoRecord").gameObject.SetActive(false);

		}else if(raceCount== 3){
			TeamLists[0].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Select").gameObject.SetActive(false);

			TeamLists[0].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[1].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[2].transform.FindChild("Userinfo").gameObject.SetActive(true);
			ChangeUserInfo(0);
			ChangeUserInfo(1);
			ChangeUserInfo(2);
			TeamLists[0].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("NoRecord").gameObject.SetActive(false);
		}

		if(raceCount == 3) 		NGUITools.FindInParents<ClubRace_Start>(gameObject).SetNextButton(1);
		else 		NGUITools.FindInParents<ClubRace_Start>(gameObject).SetNextButton(0);

		CClub.mClubRaceCount = raceCount;
	}


	public void OnChangeInfoMyClub(){
		var mTr = TeamLists[0].transform.parent as Transform;
		var tr = mTr.GetComponent<TweenPosition>() as TweenPosition;
		mTr.gameObject.SetActive(true);
		tr.Reset();
		tr.enabled = true;
		
		int raceCount = CClub.myClubRaceInfoDetail.Count;
		TeamLists[0].SetActive(true);
		TeamLists[1].SetActive(true);
		TeamLists[2].SetActive(true);
		if(raceCount== 0){
			TeamLists[0].transform.FindChild("Select").gameObject.SetActive(false);
			//TeamLists[0].transform.FindChild("Select").GetComponentInChildren<UILabel>().text  = "Ready";
			TeamLists[1].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Select").gameObject.SetActive(false);
			
			TeamLists[0].transform.FindChild("Userinfo").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("Userinfo").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Userinfo").gameObject.SetActive(false);
			
			TeamLists[0].transform.FindChild("NoRecord").gameObject.SetActive(true);
			TeamLists[1].transform.FindChild("NoRecord").gameObject.SetActive(true);
			TeamLists[2].transform.FindChild("NoRecord").gameObject.SetActive(true);
			
		}else if(raceCount== 1){
			TeamLists[0].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("Select").gameObject.SetActive(false);
		
			TeamLists[2].transform.FindChild("Select").gameObject.SetActive(false);
			
			TeamLists[0].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[1].transform.FindChild("Userinfo").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Userinfo").gameObject.SetActive(false);
			ChangeUserInfo(0);
			
			TeamLists[0].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("NoRecord").gameObject.SetActive(true);
			TeamLists[2].transform.FindChild("NoRecord").gameObject.SetActive(true);
			
		}else if(raceCount== 2){
			TeamLists[0].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Select").gameObject.SetActive(false);

			TeamLists[0].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[1].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[2].transform.FindChild("Userinfo").gameObject.SetActive(false);
			ChangeUserInfo(0);
			ChangeUserInfo(1);
			
			TeamLists[0].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("NoRecord").gameObject.SetActive(true);
			
		}else if(raceCount== 3){
			TeamLists[0].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("Select").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("Select").gameObject.SetActive(false);
			
			TeamLists[0].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[1].transform.FindChild("Userinfo").gameObject.SetActive(true);
			TeamLists[2].transform.FindChild("Userinfo").gameObject.SetActive(true);
			ChangeUserInfo(0);
			ChangeUserInfo(1);
			ChangeUserInfo(2);
			TeamLists[0].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[1].transform.FindChild("NoRecord").gameObject.SetActive(false);
			TeamLists[2].transform.FindChild("NoRecord").gameObject.SetActive(false);
		}

		NGUITools.FindInParents<ClubRace_Start>(gameObject).SetOppNextButton();
	}




	public void OnInitMy(){
	//	TeamLists[0].transform.parent.gameObject.SetActive(false);
	}

	public void OnInitOther(){
	//	TeamLists[0].transform.parent.gameObject.SetActive(false);
	}

	public void OnSet_mUserInfoOther(){
		
	}


	public void OnShowUserInfo(){
	}

	public void OnHiddenUserInfo(){
	
	}



	public void OnSet_mUserInfoMy(){
	}


	public void OnSelectOther(GameObject obj){
		if(Global.isNetwork) return;
		string[] name = obj.name.Split('_');
		int idx = int.Parse(name[1]);
		var tr = TeamLists[idx].transform.FindChild("Select") as Transform;
		if(tr.gameObject.activeSelf) return;
		foreach(GameObject mTr in TeamLists){
			mTr.transform.FindChild("Select").gameObject.SetActive(false);
		}
		tr.gameObject.SetActive(true);

	
		//NGUITools.FindInParents<ClanMatchStart>(gameObject).SetNextButton(idx);
	
	}



}
