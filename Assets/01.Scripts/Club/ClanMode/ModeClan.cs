using UnityEngine;
using System.Collections;

public class ModeClan : ModeParent {

	public GameObject CStart, MUser; //순서 : CStart : 유저 리스트 나오고 , MUser : 게임 실행한다. "ClubRace_next";

	public override void OnSubWindow ()
	{
		// ClubMode = 0; // 0: no match, 1: match yes, 2:match wait
		base.OnSubWindow ();
		transform.FindChild("BG").gameObject.SetActive(true);
		GV.gEntryFee = 0;
		setTeamId = 0;
		ClanStartMode(CStart);
		onNext = ()=>{
			ReadyLoadingCircle();
			gameObject.AddComponent<SettingRaceClan>().setClubRace(setTeamId,starCount);
			UserDataManager.instance._subStatus = null;
		};

	
	}

	/*
	void ClubRaceReadyClick(){
		//!!--Utility.Log("ClubMode = 2 " + CClub.ClubMode);
	//	MNotEntry.SetActive(false);
	//	MRace.SetActive(true);
	//	MRace.SendMessage("initialContentWindow");
		UserDataManager.instance._subStatus = ()=>{
			//MNotEntry.SetActive(true);
		//	MRace.SetActive(false);
			return true;
		};


	}*/
	private string selectTeam;
	private int setTeamId,starCount;
	public void OnSetClubTeam(string str){
		this.selectTeam = str;
	}


	void ClanStartMode(GameObject obj){
		obj.SetActive(true);
		obj.SendMessage("initialContentWindow"); // ClanMatchStart "ClubRace_Start";
	}
	
	void ClanReadyMode(GameObject obj){
		obj.SetActive(true);
		obj.SendMessage("initialContentWindow"); //ClubRaceReady
	}

	public void OnNextTeam(string str){
		MUser.SetActive(true);
		transform.parent.GetComponent<RaceModeAnimaintion>().SelectAniPlay("ModeWin_Club_City");
		CStart.SetActive(false);
		MUser.SendMessage("InitRaceWinodw"); //ClubRaceReady
		UserDataManager.instance._subStatus = ()=>{
			MUser.SetActive(false);
			CStart.SetActive(false);
			OnSubWindow();
			return true;
		};
	}


	public void SetBackClick(){
		UserDataManager.instance._subStatus = null;
		MUser.SetActive(false);
		CStart.SetActive(false);
		GameObject.Find("LobbyUI").SendMessage("OnBackClick");
	}

	public void SetClubRaceStart(int id, int idx){
		if(UserDataManager.instance.raceFuelCountCheck()) return;
		CClub.mClubRaceCount++;
		if(CClub.mClubRaceCount <= 3){
			this.setTeamId = id;
			this.starCount =idx;
			entryDollar = GV.gEntryFee;
			CClub.ClubRaceTeams[CClub.mClubRaceCount-1] = id;
			base.OnNext(gameObject);
		}else{
			Utility.LogError("mclubRaceCount : " + CClub.mClubRaceCount);
		}
	}
}
