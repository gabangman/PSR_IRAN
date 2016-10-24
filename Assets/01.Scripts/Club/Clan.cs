using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

[System.Serializable] 
public static class CClub {
	public static int ClubTest; // 0 : 테스트 , 1 : 정상적으로

	//public static int mClubIndex;
	public static ClubInfo mClubInfo;
	public static ClubMemberInfo mClubMember;

	public static List<ClubMemberInfo> myClubMemInfo;
	public static List<ClubMemberInfo> VisitClubMemList;


	public static List<ClubRaceMemberInfo> myClubRaceInfo;
	public static List<ClubRaceMemberInfo> oppClubRaceInfo;
	public static List<ClubRaceMemberInfoDetail> myClubRaceInfoDetail;
	public static List<ClubRaceMemberInfoDetail> oppClubRaceInfoDetail;

	public static List<ClubInfoSearch> mSeachList;
	public static List<ClubInfoRanking> mRankingLocal;
	public static List<ClubInfoRanking> mRankingGlobal;
	public static List<ClubInfoHistory> mHistoryList;
	public static ClubInfoVisit mClubVisit;
	public static ClubReadyInfo mReady;
	public static System.DateTime readyWaitTime;
	public static int ClubMode = 0; // 0: no match, 1: 레이스 진행중 2. 레이스 완료 3. 매칭 중 
	public static int ClanMember = 0; // 0 : no member , 1 : clanMaseter, 2 : clanStaff, 3: ClanMember
	public static int ClubMatchFlag = 0; //0 : No wait, 1 : waiting
	public static int mClubFlag = 0; // 0 : no season, 1 : seaon ok but no clanmember, 2 : season OK & clanmember
	public static readonly int ClubDollar = 10000;
	public static int mClubRaceStarCount;
	public static int mClubRaceCount= 1;
	public static int mClubMatchingIndex;
	public static int mClubMatchingRaceIndex;
	public static int ChangeClub;
	public static int ClubTeamID;
	public static int MaxMember = 30;
	public static int MatchTimeSet = 0;
	public static int[] ClubRaceTeams;
	public static int matchDurationSeconds;//CClub.mClubInfo.matchDurationSeconds;
	public static int ClubClick=0;
	public static int gReview=0;
	public static int champID=0;
	public static System.DateTime localTime;
	public static System.DateTime GlobalTime;
	public static bool bMaterial = false;
	public static bool bRace = false;
	public static bool bClanWarChat = false;
	public static void InitClub(){
		CClub.mClubInfo = null;
		CClub.mClubMember=null;
		
		CClub.myClubMemInfo= new List<ClubMemberInfo>();
		CClub.VisitClubMemList= new List<ClubMemberInfo>();
		CClub.myClubRaceInfo= new List<ClubRaceMemberInfo>();
		CClub.oppClubRaceInfo= new List<ClubRaceMemberInfo>();
		CClub.myClubRaceInfoDetail= new List<ClubRaceMemberInfoDetail>();
		CClub.oppClubRaceInfoDetail = new List<ClubRaceMemberInfoDetail>();
		CClub.mSeachList=new List<ClubInfoSearch>();
	//	CClub.mRankingLocal=new List<ClubInfoRanking>();
	//	CClub.mRankingGlobal=new List<ClubInfoRanking>();
		CClub.mHistoryList=new List<ClubInfoHistory>();
		CClub.mClubVisit = null;
		CClub.mReady = null;
		//  System.DateTime readyWaitTime;
		CClub.ClubMode = 0; // 0: no match, 1: 레이스 진행중 2. 레이스 완료 3. 매칭 중 
		CClub.ClanMember = 0; // 0 : no member , 1 : clanMaseter, 2 : clanStaff, 3: ClanMember
		CClub.ClubMatchFlag = 0; //0 : No wait, 1 : waiting
		if(GV.ChSeasonID >= 6010)
		CClub.mClubFlag = 1; // 0 : no season, 1 : seaon ok but no clanmember, 2 : season OK & clanmember
		else CClub.mClubFlag = 0;
		ClubRaceTeams = new int[3];
		CClub.bClanWarChat = false;
		//  int mClubRaceStarCount;
		 //int mClubRaceCount= 1;
		//  int mClubMatchingIndex;
		//  int mClubMatchingRaceIndex;
	}
}

/*	string strTime =  thing["result"][i]["sponsorDate"];
					try{
						team.sponDateTime = System.Convert.ToDateTime(strTime);
						team.sponDateTime = team.sponDateTime.AddHours(12);
						team.SponRemainTime = team.sponDateTime.Ticks;
					}catch(Exception e){
						team.sponDateTime = new DateTime(1900,1,1);
						team.SponRemainTime = team.sponDateTime.Ticks;
					}
*/

[System.Serializable]
public class ClubInfo{
	public string userId;
	public  string mClubName;
	public  string clubSymbol;
	public string clubDescription;
	public int clubMember;
	public  string clubChatCh;
	public  int clubMemberNum;
	public  int clubMemberTotalNum;
	public  string victoryNum;
	public  string playNum;
	public  int clubLevel;
	public  int clubExp;
	public  int clubMode;
	public string matcingTime;
	public int myEntry;
	public int myEntryFlag;
	public int clubIndex;
	public  int clubMatchIndex;
	public string createTime;
	public bool dev_isMatchJustFinished;
	public string mLocale;
	public int matchDurationSeconds;
	public string clubMatchChatCh;
	//{"state":0,"clubMember":1,"clubDescription":"sample modified desc","mClubName":"club_name_58","clubSymbol":"sample modified symbol","clubChatCh":"temp","clubMemberNum":2,"clubMemberTotalNum":30,
	//"victoryNum":"temp","playNum":"temp","clubLevel":2,"clubExp":50,"myEntryFlag":1,"clubIndex":59,"createTime":"2016-04-13 16:11:42","dev_isMatchJustFinished":false,"myEntry":0,"clubMode":0}

	public ClubInfo(SimpleJSON.JSONNode mNode){
		this.clubMember = mNode["clubMember"].AsInt;
		this.clubDescription = mNode["clubDescription"].Value;
		this.mClubName = mNode["mClubName"].Value;
		this.clubSymbol = mNode["clubSymbol"].Value;
		this.clubChatCh = mNode["clubChatCh"].Value;
		this.clubMemberNum = mNode["clubMemberNum"].AsInt;
		this.clubMemberTotalNum = mNode["clubMemberTotalNum"].AsInt;
		this.victoryNum = mNode["victoryNum"].Value;
		this.playNum = mNode["playNum"].Value;
		this.clubLevel = mNode["clubLevel"].AsInt;
		this.clubExp = mNode["clubExp"].AsInt;
		this.myEntryFlag = mNode["myEntryFlag"].AsInt;
		this.clubIndex = mNode["clubIndex"].AsInt;
		//System.DateTime tempTime = System.DateTime.Parse(mNode["createTime"].Value);
		this.createTime = mNode["createTime"].Value;
		this.dev_isMatchJustFinished = mNode["dev_isMatchJustFinished"].AsBool;
		this.myEntry = mNode["myEntry"].AsInt;
		this.clubMode = mNode["clubMode"].AsInt;
		this.matcingTime = mNode["matchingTime"].Value;
		this.matchDurationSeconds = mNode["matchDurationSeconds"].AsInt;
	
		CClub.matchDurationSeconds = this.matchDurationSeconds;
		try{
		
		}catch(NullReferenceException e){
		
		}
		this.mLocale = mNode["mLocale"].Value;
		CClub.ClubMode=	this.clubMode;
		int flag = EncryptedPlayerPrefs.GetInt("ClubAlram");
		if(CClub.ClubMode==1){
			this.clubMatchChatCh = mNode["clubMatchChatCh"].Value;
			if(flag == 0){
				System.DateTime mTime =  System.Convert.ToDateTime(this.matcingTime);
				System.TimeSpan sTime = NetworkManager.instance.GetCurrentDeviceTime() - mTime; //2016-09-25 13:02:38
				if(sTime.TotalMinutes < 0 ) {
					return;
				}
				int rt = 1440-(int)sTime.TotalMinutes-60;
				if(rt < 60) return;
				Vibration.OnSetClubFinishTime(rt);
			}
		}else if(CClub.ClubMode == 2){
			if(flag == 1) EncryptedPlayerPrefs.SetInt("ClubAlram",0);
			this.clubMatchChatCh = string.Empty;
		}else{
			this.clubMatchChatCh = string.Empty;
		}


	//	System.DateTime mTime =  System.Convert.ToDateTime("2016-09-26 20:00:00");
	//	System.TimeSpan sTime = NetworkManager.instance.GetCurrentDeviceTime() - mTime; //2016-09-25 13:02:38
	//	Debug.LogWarning(sTime.TotalHours);
	//	int d = 24-(int)sTime.TotalHours;

//		Debug.LogWarning(sTime.Minutes);
//		Debug.LogWarning(sTime.TotalMinutes);
//		d = 1440-(int)sTime.TotalMinutes-30;
//		if(d < 30) return;
		//
//		Debug.LogWarning(sTime.Hours);
//		Debug.LogWarning(d);
//		Debug.LogWarning( NetworkManager.instance.GetCurrentDeviceTime() );
		//9/27/2016 5:37:44

	//	//==!!Utility.LogWarning("ClubMode =  " + CClub.ClubMode);
	//0 : 아무 상태아님
	//1 : 클럽 레이스 진행 중
	//2 : 클럽 레이스 완료 
	//3 : 클럽 매칭 중
	}
}

[System.Serializable]
public class ClubInfoRanking{
	public  string mClubName;
	public  string clubSymbol;
	public string clubDescription;
	public int clubLevel;
	public int clubVistoryNum;
	public int clubIndex;
	public int clubMemberNum;
	public string mLocale;

//respons : {"state":0,"clubRank":"temp","rankingList":[{"mClubName":"club_name_39","clubSymbol":"sample symbol","clubDescription":"sample desc","clubLevel":4,
	//"clubVictoryNum":23,"clubIndex":64,"mLocale":"KOR","clubMemberNum":1},{"mClubName":"club_name_58","clubSymbol":"sample modified symbol","clubDescription":"sample modified desc","clubLevel":5,"clubVictoryNum":16,"clubIndex":59,"mLocale":"KOR","clubMemberNum":2},{"mClubName":"myTest01","clubSymbol":"Clubsymbol_6","clubDescription":"This is Test01","clubLevel":2,"clubVictoryNum":4,"clubIndex":83,"mLocale":"kr","clubMemberNum":1},{"mClubName":"\ubc88\uc9c0\uc810\ud504\ub97c\ud558\ub2e4","clubSymbol":"Clubsymbol_28","clubDescription":"\uc73c\ub77c\ucc28\ucc28","clubLevel":1,"clubVictoryNum":3,"clubIndex":77,"mLocale":"KOR","clubMemberNum":2},{"mClubName":"myTest02","clubSymbol":"Clubsymbol_4","clubDescription":"TESTCLUB","clubLevel":1,"clubVictoryNum":1,"clubIndex":86,"mLocale":"kr","clubMemberNum":1}]}
	public ClubInfoRanking(SimpleJSON.JSONNode mNode){
		this.mClubName = mNode["mClubName"].Value;
		this.clubSymbol = mNode["clubSymbol"].Value;
		this.clubDescription = mNode["clubDescription"].Value;
		this.clubLevel = mNode["clubLevel"].AsInt;
		this.clubIndex = mNode["clubIndex"].AsInt;
		this.mLocale = mNode["mLocale"].Value;
		this.clubMemberNum = mNode["clubMemberNum"].AsInt;
		this.clubVistoryNum = mNode["clubVictoryNum"].AsInt;
	}

}

[System.Serializable]
public class ClubInfoHistory{
	public  string oppClubName;
	public  string oppClubSymbol;
	public string raceDateTime;
	public int resultRace;
	public int oppStarCount;
	public int myClubStarCount;
	public int oppVictoryNum;
//respons : {"state":0,"winCount":0,"loseCount":1,
//"historyList":[{"oppClubName":"club_name_39","oppClubSymbol":"sample symbol","raceDateTime":"2016-05-01 08:10:30","resultRace":0,"oppStarCount":0,"myClubStarCount":0,"oppVictoryNum":11}]}

	public ClubInfoHistory(SimpleJSON.JSONNode mNode){
		this.oppClubName = mNode["oppClubName"].Value;
		this.oppClubSymbol = mNode["oppClubSymbol"].Value;
		this.raceDateTime = mNode["raceDateTime"].Value;
		this.resultRace = mNode["resultRace"].AsInt;
		this.oppStarCount = mNode["oppStarCount"].AsInt;
		this.myClubStarCount = mNode["myClubStarCount"].AsInt;
		this.oppVictoryNum = mNode["oppVictoryNum"].AsInt;
	}

}

[System.Serializable]
public class ClubInfoSearch{
	//mClubName":"test1","clubSymbol":"Clubsymbol_5","clubDescription":"ddaabb","clubLevel":1,"clubVictoryNum":0,"clubIndex":66,"clubMember":1,"createTime":"2016-04-27 15:01:51"}]}
	public  string mClubName;
	public  string clubSymbol;
	public string clubDescription;
	public string createTime;
	public int clubLevel;
	public int clubVictoryNum;
	public int clubIndex;
	public int clubMember;
	public string mLocale;
	public ClubInfoSearch(SimpleJSON.JSONNode mNode){
		this.mClubName = mNode["mClubName"].Value;
		this.clubSymbol = mNode["clubSymbol"].Value;
		this.clubDescription = mNode["clubDescription"].Value;
		this.createTime = mNode["createTime"].Value;
		this.clubLevel = mNode["clubLevel"].AsInt;
		this.clubVictoryNum = mNode["clubVictoryNum"].AsInt;
		this.clubIndex = mNode["clubIndex"].AsInt;
		this.clubMember = mNode["clubMemberNum"].AsInt;
		this.mLocale  = mNode["mLocale"].Value;
	}
}

[System.Serializable]
public class ClubInfoVisit{
	public  string clubName;
	public  string clubSymbol;
	public string clubDescription;
	public int victoryNum;
	public int playNum;
	public int clubLevel;
	public int clubMemberNum;
	public int clubIndex;
	public string mLocale;
	public ClubInfoVisit(SimpleJSON.JSONNode mNode){
		//	respons : {"state":0,"clubDescription":"ddaabb","mClubName":"test1","clubSymbol":"Clubsymbol_5","victoryNum":0,"playNum":0,"clubMemberNum":1,"clubLevel":1}

		this.clubName = mNode["mClubName"].Value;
		this.clubSymbol = mNode["clubSymbol"].Value;
		this.clubDescription = mNode["clubDescription"].Value;
		this.victoryNum = mNode["victoryNum"].AsInt;
		this.clubLevel = mNode["clubLevel"].AsInt;
		this.playNum = mNode["playNum"].AsInt;
		this.clubLevel = mNode["clubLevel"].AsInt;
		this.clubMemberNum = mNode["clubMemberNum"].AsInt;
		this.mLocale  = mNode["mLocale"].Value;
	}
}

[System.Serializable]
public class ClubMemberInfo{
	//{"userId":487,"clubStarCount":"temp","playTotalNumber":"temp","playMyNumber":"temp","clubMember":9,
  // "joinTime":"2016-04-27 15:01:51","userInfoData":{"profileUrl":"User_Default","nickName":"3199","mLv":"1"}}

//response:{"state":0,"memberList":[{"userId":487,"clubStarCount":0,"playTotalNumber":0,"playMyNumber":0,"userEntryFlag":1,"clubMember":9,"joinTime":"2016-04-27 15:01:51","userInfoData":{"profileUrl":"User_Default","nickName":"3199","mLv":"1"}},{"userId":506,"clubStarCount":0,"playTotalNumber":0,"playMyNumber":0,"userEntryFlag":1,"clubMember":1,"joinTime":"2016-04-28 17:39:34","userInfoData":{"profileUrl":"User_Default","nickName":"m319901","mLv":"1"}}]}


	public  int userId; // 
	public  int clubStarCount; // 
	public  int playTotalNumber; //클럽레이스에서 플레이 한 총수
	public  int playNumber;// 단일레이스 횟수 / 3회를  넘길수 없다..
	public int clubMember;
	public  string JoinTime;
	public string UserProfile;
	public string nickName;
	public int mLv;
	public Texture2D userProfileTexture;
	public int mUserEntryFlag = 1;
	public int myMemInfo = 0;
	public int myRank;
	public System.DateTime mJoinTime;
	public string LastConnectTime;
	public ClubMemberInfo(SimpleJSON.JSONNode mNode){
		this.userId = mNode["userId"].AsInt;
		this.clubStarCount = mNode["clubStarCount"].AsInt;
		this.playTotalNumber = mNode["playTotalNumber"].AsInt;
		this.playNumber = mNode["playMyNumber"].AsInt;
		this.clubMember = mNode["clubMember"].AsInt;
		this.JoinTime = mNode["joinTime"].Value;


		SimpleJSON.JSONNode _node = mNode["userInfoData"];
	   string str = _node.ToString();
		////==!!Utility.LogWarning(str);
		if(string.IsNullOrEmpty(str) == true || str.Length <10 ){
			this.UserProfile = "User_Default";
			this.nickName = "NoName";
			this.mLv = 1;
			this.LastConnectTime = string.Empty;//NetworkManager.instance.GetCurrentDeviceTime();
		}else{
				this.UserProfile = mNode["userInfoData"]["profileUrl"].Value;
				this.nickName = mNode["userInfoData"]["nickName"].Value;
				string temp =  mNode["userInfoData"]["mLv"].Value;
				int.TryParse(temp,out this.mLv);
				userProfileTexture = null;
			if(this.userId.ToString() == GV.UserRevId){
				this.myMemInfo = 1;
			}

			string mTime = mNode["userInfoData"]["Ltime"].Value;
			if(string.IsNullOrEmpty(mTime) == true || mTime.Length <10){
				//this.LastConnectTime = new System.DateTime(2016,1,1);
				this.LastConnectTime = string.Empty;
			}else{
				this.LastConnectTime  = mTime;
			//	Utility.LogWarning(mTime);
			//	Utility.LogWarning(NetworkManager.instance.GetCurrentDeviceTime().ToString("yyyy/MM/dd hh:mm:ss"));
			}

		}
		mUserEntryFlag = mNode["userEntryFlag"].AsInt;
	//	//==!!Utility.LogWarning(this.JoinTime);
		this.mJoinTime  = System.Convert.ToDateTime(this.JoinTime);
	}
//response:{"state":0,"memberList":[{"userId":487,"clubStarCount":0,"playTotalNumber":"temp","playMyNumber":"temp","clubMember":9,
//"joinTime":"2016-04-27 15:01:51","userInfoData":{"profileUrl":"User_Default","nickName":"3199","mLv":"1"}}]}
}



[System.Serializable]
public class ClubRaceMemberInfo{
	// 클럽 레이스 정보 가져오기 
	public int clubUserId; // 
	public int clubraceCount; // //클럽레이스에서 플레이 한 총수
	public int EarnedStarCount; 

	// 클럽 레이스 정보 가져오기 
	public string profileUrl;// 단일레이스 횟수 / 3회를  넘길수 없다..
	public string NickName;
	public int mLv;
	public Texture2D userProfileTexture;
	//{"state":0,"userList":[{"clubUserId":502,"clubraceCount":3,"EarnedStarCount":"13","userInfoData":{"profileUrl":"sample_url","NickName":"n396","mLv":"sample_level"}}]}

	public  ClubRaceMemberInfo(SimpleJSON.JSONNode _node){
		this.clubUserId = _node["clubUserId"].AsInt;
		this.clubraceCount = _node["clubraceCount"].AsInt;
		this.EarnedStarCount = _node["EarnedStarCount"].AsInt;
		SimpleJSON.JSONNode _node1 = _node["userInfoData"];
		string str = _node1.ToString();
		if(string.IsNullOrEmpty(str) == true || str.Length <10 ){
			this.profileUrl = "User_Default";
			this.NickName = "NoName";
			this.mLv = 1;
			
		}else{
			this.profileUrl =   _node["userInfoData"]["profileUrl"].Value;
			this.NickName =   _node["userInfoData"]["nickName"].Value;
			string temp =  _node["userInfoData"]["mLv"].Value;
			int.TryParse(temp,out this.mLv);
		}

	}

}

[System.Serializable]
public class ClubRaceMemberInfoDetail{
	public int clubraceNumber; // 
	public int thisRaceEarnedStarCount; // 

	public int teamId; 
	public int carId;
	public int carClass;
	public int carIdx;
	public int level; 
	public int crewId;
	public int sponId;

	public int carAbility; 
	public int crewAbility;
	public int isRaceEnd;
	public ClubRaceMemberInfoDetail(){
	}
	public ClubRaceMemberInfoDetail(SimpleJSON.JSONNode _node){
		this.clubraceNumber = _node["clubraceNumber"].AsInt;
		this.thisRaceEarnedStarCount = _node["thisRaceEarnedStarCount"].AsInt;
	
		string temp =  _node["clubRaceData"]["teamId"].Value;
		int.TryParse(temp,out this.teamId);

		temp =  _node["clubRaceData"]["carId"].Value;
		int.TryParse(temp,out this.carId);
		temp =  _node["clubRaceData"]["carClass"].Value;
		int.TryParse(temp,out this.carClass);
		temp =  _node["clubRaceData"]["level"].Value;
		int.TryParse(temp,out this.level);
		temp =  _node["clubRaceData"]["crewId"].Value;
		int.TryParse(temp,out this.crewId);

		temp =  _node["clubRaceData"]["sponId"].Value;
		int.TryParse(temp,out this.sponId);
		temp =  _node["clubRaceData"]["carAbility"].Value;
		int.TryParse(temp,out this.carAbility);
		temp =  _node["clubRaceData"]["crewAbility"].Value;
		int.TryParse(temp,out this.crewAbility);
		temp =  _node["clubRaceData"]["carIdx"].Value;
		int.TryParse(temp,out this.carIdx);
		this.isRaceEnd = _node["isRaceEnd"].AsInt;

	}

//{"state":0,"raceList":[{"clubraceNumber":1,"thisRaceEarnedStarCount":5,"clubRaceData":{"teamId":"sample_team","carId":"sample_car","crewId":"sample_crew"}},{"clubraceNumber":2,"thisRaceEarnedStarCount":4,"clubRaceData":{"teamId":"sample_team","carId":"sample_car","crewId":"sample_crew"}},{"clubraceNumber":3,"thisRaceEarnedStarCount":4,"clubRaceData":{"teamId":"sample_team","carId":"sample_car","crewId":"sample_crew"}}]}
}

[System.Serializable]
public class resultClubUserInfo{
	public string profileUrl;// 단일레이스 횟수 / 3회를  넘길수 없다..
	public string NickName;
	public int mLv;
	public Texture2D userProfileTexture;
	public int mFoe;
	public List<resultClubRaceInfo> mRaceInfo;
	public resultClubUserInfo() {
			
	}



}

[System.Serializable]
public class resultClubRaceInfo{
	public int raceNumber;
	public int raceStar;
	
	public resultClubRaceInfo() {
		
	}
	
	
	
}

[System.Serializable]
public class ClubRaceResultInfo{
	//	{"state":0,"oppclubName":"club_name_39","oppclubsymbol":"sample symbol","oppClubStarCount":19,"myClubStarCount":13,
	//		"oppMemberTotalNum":1,"oppMemberNum":1,"raceResult":0,"racePrize":1}
//	"oppMemberNum":1,"myMemberTotalNum":1,"myMemberNum":1,
	public string oppclubName; // 
	public string oppclubsymbol; // 
	public int oppClubStarCount; //클럽레이스에서 플레이 한 총수
	public int myClubStarCount;// 단일레이스 횟수 / 3회를  넘길수 없다..
	public int oppMemberTotalNum;
	public int oppMemberNum;
	public int myMemberTotalNum;
	public int myMemberNum;
	public int raceResult;
	public int racePrize;
	public int winBonus;


	public List<resultClubUserInfo> mMyMember;
	public List<resultClubUserInfo> mOpMember;
	public ClubRaceResultInfo(SimpleJSON.JSONNode mNode){
		this.oppclubName = mNode["oppclubName"].Value;
		this.oppclubsymbol = mNode["oppclubsymbol"].Value;
		this.oppClubStarCount = mNode["oppClubStarCount"].AsInt;
		this.oppMemberTotalNum = mNode["oppMemberTotalNum"].AsInt;
		this.oppMemberNum = mNode["oppMemberNum"].AsInt;
		this.myClubStarCount = mNode["myClubStarCount"].AsInt;
		this.myMemberNum = mNode["myMemberNum"].AsInt;
		this.myMemberTotalNum = mNode["myMemberTotalNum"].AsInt;
		this.raceResult = mNode["raceResult"].AsInt;
		this.racePrize = mNode["racePrize"].AsInt;
		if(this.raceResult == 1){
			this.winBonus = mNode["winBonus"].AsInt;
		}else{
			this.winBonus = 0;
		}

		mMyMember = new List<resultClubUserInfo>();//ClubRaceMemberInfoDetail();
		mOpMember = new List<resultClubUserInfo>();
		mMyMember.Clear();
		mOpMember.Clear();
		//{"state":0,"oppclubName":"club_name_84","oppclubsymbol":"sample symbol","oppClubStarCount":9,
		//"myClubStarCount":13,"oppMemberTotalNum":1,"oppMemberNum":1,"myMemberTotalNum":2,"myMemberNum":2,
		//"raceResult":1,"racePrize":2808,"winBonus":540,
	//"raceMyInfoDetail":[{"clubUserId":502,"userInfoData":{"profileUrl":"sample_url","NickName":"n982","mLv":"sample_level"},"raceDetailList":[{"raceNumber":2,"raceStar":8},{"raceNumber":3,"raceStar":5}]}],
	//"raceOppInfoDetail":[{"clubUserId":512,"userInfoData":{"profileUrl":"sample_url","NickName":"sample_nickname","mLv":"sample_level"},"raceDetailList":[{"raceNumber":3,"raceStar":9}]}]}
		
		int mCnt = mNode["raceMyInfoDetail"].Count;
		for(int i =0; i < mCnt ; i++){
			resultClubUserInfo mUser  = new resultClubUserInfo();
			mUser.profileUrl =   mNode["raceMyInfoDetail"][i]["userInfoData"]["profileUrl"].Value;
			mUser.NickName =    mNode["raceMyInfoDetail"][i]["userInfoData"]["nickName"].Value;
			string temp =  mNode["raceMyInfoDetail"][i]["userInfoData"]["mLv"].Value;
			int.TryParse(temp,out mUser.mLv);
			int localCnt =   mNode["raceMyInfoDetail"][i]["raceDetailList"].Count;
		//	Debug.LogWarning(localCnt);
			mUser.mRaceInfo = new List<resultClubRaceInfo>();
			for(int j = 0; j < localCnt; j++){
				resultClubRaceInfo mRace = new resultClubRaceInfo();
				mRace.raceStar = mNode["raceMyInfoDetail"][i]["raceDetailList"][j]["raceStar"].AsInt;
				mRace.raceNumber = mNode["raceMyInfoDetail"][i]["raceDetailList"][j]["raceNumber"].AsInt;
				mUser.mRaceInfo.Add(mRace);
			}
			mMyMember.Add(mUser);
		}

	
		mCnt = mNode["raceOppInfoDetail"].Count;
		for(int i =0; i < mCnt ; i++){
			resultClubUserInfo mUser  = new resultClubUserInfo();
			mUser.profileUrl =   mNode["raceOppInfoDetail"][i]["userInfoData"]["profileUrl"].Value;
			mUser.NickName =    mNode["raceOppInfoDetail"][i]["userInfoData"]["nickName"].Value;
			string temp =  mNode["raceOppInfoDetail"][i]["userInfoData"]["mLv"].Value;
			int.TryParse(temp,out mUser.mLv);
			int localCnt =   mNode["raceOppInfoDetail"][i]["raceDetailList"].Count;
		//	Debug.LogWarning(localCnt);
			mUser.mRaceInfo = new List<resultClubRaceInfo>();
			for(int j = 0; j < localCnt; j++){
				resultClubRaceInfo mRace = new resultClubRaceInfo();
				mRace.raceStar = mNode["raceOppInfoDetail"][i]["raceDetailList"][j]["raceStar"].AsInt;
				mRace.raceNumber = mNode["raceOppInfoDetail"][i]["raceDetailList"][j]["raceNumber"].AsInt;
				mUser.mRaceInfo.Add(mRace);
			}
			mOpMember.Add(mUser);
		}

	}
}




[System.Serializable]
public class ClubReadyInfo{
	//{"state":0,"matchingTime":"2016-04-30 03:54:35","oppclubIndex":64,"clubMatchingIndex":57,"MemberTotalNum":1,
	//"oppclubName":"club_name_39","oppclubsymbol":"sample symbol","oppMemberTotalNum":1,"MemberNum":0,"myMemeberRacePlayCount":0,
	//"myClubStarCount":0,"oppMemberNum":0,"oppMemeberRacePlayCount":0,"oppClubStarCount":0}
	public string matchingTime; // 
	public int oppclubIndex; // 
	public int clubMatchingIndex; 
	public int MemberTotalNum;
	public int MemberNum;
	public int myMemeberRacePlayCount;
	public int myClubStarCount;

	public string oppclubName;
	public string oppclubsymbol;
	public int oppMemberTotalNum;
	public int oppMemberNum;
	public int oppMemeberRacePlayCount;
	public int oppClubStarCount;

	public ClubReadyInfo(SimpleJSON.JSONNode mNode){
		this.matchingTime = mNode["matchingTime"].Value;
		this.oppclubIndex = mNode["oppclubIndex"].AsInt;
		this.clubMatchingIndex = mNode["clubMatchingIndex"].AsInt;

		this.MemberTotalNum = mNode["MemberTotalNum"].AsInt;
		this.MemberNum = mNode["MemberNum"].AsInt;
		this.oppclubName = mNode["oppclubName"].Value;

		this.oppclubsymbol = mNode["oppclubsymbol"].Value;
		this.oppMemberTotalNum = mNode["oppMemberTotalNum"].AsInt;
		this.myMemeberRacePlayCount = mNode["myMemeberRacePlayCount"].AsInt;
		this.myClubStarCount = mNode["myClubStarCount"].AsInt;
		this.oppMemberNum = mNode["oppMemberNum"].AsInt;

		this.oppMemeberRacePlayCount = mNode["oppMemeberRacePlayCount"].AsInt;
		this.oppClubStarCount = mNode["oppClubStarCount"].AsInt;
	}
}

public class ClubRaceInfo{
	public  int 	teamId;//	팀 ID
	public  int 	carId;//	차량 ID
	public  int 	carIdx;//	차량 Index
	public  int	sponId;//	스폰서 ID
	public  int	carAbility;//	차량 능력치
	public  int	crewAbility;//	크루 능력치
	public  int 	crewId;//	크루ID
	public  int	carClassId;//	차량 class
	public  int  thisCount; // 레이스 횟수  
}

