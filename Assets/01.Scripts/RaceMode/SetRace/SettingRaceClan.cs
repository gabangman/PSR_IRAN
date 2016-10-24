using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SettingRaceClan : SettingRaceEnv {

	public void setClubRace(int clubTeamId, int starCount){
		Global.gRegularRaceLevel=	Base64Manager.instance.GlobalEncoding(1);
		Global.isRaceTest = false;
		int Track = 1413;
		Global.gRaceInfo = new RaceInfo();
		Global.gRaceInfo.init();
		Global.gRaceInfo.trackID = Track.ToString();
		int clubID = starCount + 6099;

		CClub.mClubRaceStarCount = starCount;
		GV.PlayCarID =GV.getTeamCarID(clubTeamId);
		GV.PlayCrewID = GV.getTeamCrewID(clubTeamId);
		GV.PlaySponID = GV.getTeamSponID(clubTeamId);
		GV.PlayClassID = GV.getTeamCarClass(clubTeamId);
		GV.PlayTeamID = clubTeamId;
		Common_Reward.Item reward = Common_Reward.Get(Global.gRewardId);
		Global.gScoreGood = Base64Manager.instance.GlobalEncoding(reward.Common_good);
		Global.gScorePerfect = Base64Manager.instance.GlobalEncoding(reward.Common_perfect);
		Common_Class.Item playClassItem = GV.getTeamCarClassItem(clubTeamId);
		Global.gP_MAX = playClassItem.p_max;
		Global.gP_MIN = playClassItem.p_min;
		Global.gR_Early = playClassItem.R_Early;
		Global.gR_Good = playClassItem.R_Good;
		Global.gR_Late = playClassItem.R_Late;
		Global.gR_Perfect = playClassItem.R_Perfect;

		Global.gRaceInfo.raceName =  Common_Track.Get(Track).Name;
		Global.gRaceInfo.extraDollar = 0;
		Global.gRaceInfo.iconName = Track.ToString()+"A";
		Global.gRaceInfo.mType = MainRaceType.Club;
		Global.gRaceInfo.sType = SubRaceType.CityRace;
		Global.gRaceInfo.mode1Rw = 0;
		Global.gRaceInfo.extraCoin = 0;
		Global.gRaceInfo.rewardMatCount =0; //material
		UserDataManager.instance.NewGamePlaySetting(GV.getTeamCarInfo(clubTeamId), GV.getTeamCrewInfo(clubTeamId));
		setScrewDelay(GV.PlayCrewID);
		RaceModeSetting(clubTeamId, clubID);
		string SceneName = Common_Track.Get(Track).Scene;
		SetTrackDurability(GV.PlayCarID, Common_Track.Get(Track), clubTeamId);
		StartCoroutine(ClubRaceStart(SceneName, Track, clubTeamId));

	//	StartCoroutine("StartRacePlay",SceneName);		
	
	}

	IEnumerator ClubRaceStart(string sceneN, int mT, int teamid){
			yield return null;
			Global.isNetwork = true;
			CClub.ClubTeamID = teamid;
			string raceAPI ="club/startClubRaceMain";
			Dictionary<string, int> mDic = new Dictionary<string, int>();
			int sponID = GV.getTeamSponID(teamid);
			SaveSponBouns(sponID);
			mDic.Add("sponID",sponID);
			mDic.Add("entryFee",GV.gEntryFee);
			mDic.Add("carIndex", GV.getTeamCarIndex(teamid));
			mDic.Add("trackID",mT);
			mDic.Add("clubMatchingIndex",CClub.mClubMatchingIndex);
			mDic.Add("clubraceCount",CClub.mClubRaceCount);
			string strdata = "clubRaceData;"+maekClubRaceData(teamid);
			int fCnt = GV.mUser.FuelCount;
			mDic.Add("mFuelcount",fCnt);
			NetworkManager.instance.ClubBaseConnect("Post", mDic,raceAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					Global.rankRaceIdx = thing["MainRaceClubIndex"].AsInt;
				if(Application.isEditor){
					StartCoroutine("sendEndRace");	
				}else{
					StartCoroutine("StartRacePlay",sceneN);	
				}
				GoogleAnalyticsV4.instance.LogScreen(new AppViewHitBuilder().SetScreenName("ClubRace"));
				}else if(status == -300){
					
				}else{
					Utility.LogError("state " + status + " Msg : " + thing["msg"]);
					Global.isNetwork = false;
					gameObject.GetComponent<LoadingGauage>().stopLoading();
				}
		}, strdata);
	}

	IEnumerator sendEndRace(){
		yield return new WaitForSeconds(1.0f);
		Dictionary<string, int> mDic = new Dictionary<string, int>();
		mDic.Clear();
		string raceAPI ="club/endClubRaceMain"; //ServerAPI.Get(90046);
		mDic.Add("clubMatchingIndex", CClub.mClubInfo.clubMatchIndex);
		string strData = "MainRaceClubIndex;"+Global.rankRaceIdx.ToString();
		mDic.Add("mRank",1);
		mDic.Add("bonus",100);
		int clubraceStar = Random.Range(5,15);
		clubraceStar = 25;
		mDic.Add("ThisRaceEarnedStarCount",clubraceStar);
		NetworkManager.instance.ClubBaseConnect("Post", mDic, raceAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
					string strTeamID = GV.PlayTeamID.ToString();
					if(EncryptedPlayerPrefs.HasKey(strTeamID)){
						int a = EncryptedPlayerPrefs.GetInt(strTeamID);
						if(a >= CClub.mClubRaceStarCount){
							
						}else{
							EncryptedPlayerPrefs.SetInt(strTeamID, clubraceStar);
						}
					}else{
						EncryptedPlayerPrefs.SetInt(strTeamID, clubraceStar);
					}
		
			}else{
				Utility.LogError("Error " + status + "msg : " +thing["msg"] );
			}
			Global.isNetwork=  false;
		},strData);
		yield return null;
	}
	private string maekClubRaceData(int id){
		string str = string.Empty;
		JSONObject obj = new JSONObject();
		obj.AddField("carId", GV.PlayCarID );
		//int idx = 
		obj.AddField("carIdx", GV.getTeamCarInfo(id).CarIndex);
		obj.AddField("teamId",id);
		obj.AddField("carClass",GV.getTeamCarClassId(id));
		obj.AddField("level",Global.level);
		obj.AddField("crewId", GV.PlayCrewID);
		obj.AddField("sponId",GV.PlaySponID);
		CTeamAbility _ability = new CTeamAbility();
		int a  = _ability.CarAbility(GV.getTeamCarID(id), id, 1);
		obj.AddField("carAbility",a);
		a = _ability.CrewAbility(GV.getTeamCrewID(id), id, 1);
		obj.AddField("crewAbility", a);
		_ability = null;
		obj.AddField("thisStar", CClub.mClubRaceStarCount);
		obj.AddField("teamCnt", GV.listmyteaminfo.Count);
		str = obj.Print();

		//Utility.LogWarning("ClubraceData " + str);
		return str;
	}

	void RaceModeSetting(int teamId, int clubID){
	//	int[] rand = gameObject.AddComponent<RandomCreate>().CreateRandomValue(8);
	//	int[] rand1 = gameObject.AddComponent<RandomCreate>().CreateRandomValue(mCarCount);
		Common_ClubAI.Item aiItem = 	Common_ClubAI.Get(clubID);
		System.Collections.Generic.List<RegularAIUser> listTemp = new System.Collections.Generic.List<RegularAIUser>();
		int n = Random.Range(0,9);
		listTemp.Add(new RegularAIUser(aiItem.Car, aiItem.Crew));
		int _nick = 73058 + n;
		listTemp[0].strNick = KoStorage.GetKorString(_nick.ToString());
		//listTemp[0].strProfileUrl = "MultiCom_"+(Random.Range(0,10)).ToString();//+".png";
		listTemp[0].strProfileUrl = "ClubCom_1";//+".png";
		Texture Tex =  (Texture)Resources.Load("ComPicture/"+listTemp[0].strProfileUrl, typeof(Texture));
		listTemp[0].userProfile = (Texture2D) Tex;


		int len = listTemp.Count;
		Global.gAICarInfo = new AICarInfo[len];
		float[] racetime = new float[8];
		int carValue =  aiItem.Car_ID;
		int crewValue =aiItem.Crew_ID;
		ModeAICar.Item _item = ModeAICar.Get(carValue);
		ModeAICrew.Item item = ModeAICrew.Get(crewValue);
		UserDataManager.instance.modeAICarID = _item.ID;
		UserDataManager.instance.modeAICrewID = item.ID;
		Common_Class.Item cItem = GV.getClassTypeID(aiItem.Car_Class, 1);
		int TorqueRand = 0;
		int[] range = gameObject.AddComponent<RandomCreate>().CreateRandomValue(5);
		for(int i = 0 ; i < listTemp.Count ; i++){
			TorqueRand = Random.Range(0, _item.Torque_pr);
			racetime[0] = (_item.Torque+(float)TorqueRand);
			racetime[1] = item.Time_Pit;
			racetime[2] = _item.B_Power;
			racetime[3] = _item.B_Time;
			racetime[4] = _item.Skid_Time;
			racetime[5] =(float)Global.gMyGearLv;
			racetime[6] = (float)_item.B_Event;
			racetime[7] = (float)_item.B_Event_R;
		//	int mn = Random.Range(0,5);
			SaveUserInfo(i, listTemp[i].CarID, listTemp[i].CrewID,aiItem.Sponsor, racetime);
			Global.gAICarInfo[i].userTexture = listTemp[i].userProfile;
			Global.gAICarInfo[i].userNick = string.Format("X {0}",CClub.mClubRaceStarCount);
			Global.gAICarInfo[i].AIRefCarID =Base64Manager.instance.GlobalEncoding(aiItem.Car_Cp);
			gameObject.AddComponent<AIGearDelay>().NewMakeGearDelay(i);
			Global.gAICarInfo[i].AIRefStarLV = _item.StarLV;
			Global.gAICarInfo[i].AIClass = cItem;
		}
		listTemp.Clear();
		_item = null;
		item = null;
		
	}
}
