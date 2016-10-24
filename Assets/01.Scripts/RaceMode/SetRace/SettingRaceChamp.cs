using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SettingRaceChamp : SettingRaceEnv {
	/*
	public void setChampClearRace(int seaID){
		Global.isRaceTest = false;
		Common_Mode_Champion.Item _mode = Common_Mode_Champion.Get(seaID);
		
		Global.gRaceInfo = new RaceInfo();
		Global.gRaceInfo.init();
		Global.gRaceInfo.trackID = _mode.Track.ToString();
		Global.gRaceInfo.iconName = _mode.Track.ToString()+"A";
	
		GV.PlayCarID = GV.getTeamCarID(GV.SelectedTeamID);
		GV.PlayCrewID = GV.getTeamCrewID(GV.SelectedTeamID);
		GV.PlaySponID = GV.getTeamSponID(GV.SelectedTeamID);
		GV.PlayClassID = GV.getTeamCarClass(GV.SelectedTeamID);

		Global.gRaceInfo.mType = MainRaceType.Champion;
		Global.gRaceInfo.sType = SubRaceType.RegularRace;

		Common_Class.Item playClassItem = GV.getTeamCarClassItem(GV.SelectedTeamID);
		Global.gP_MAX = playClassItem.p_max;
		Global.gP_MIN = playClassItem.p_min;
		Global.gR_Early = playClassItem.R_Early;
		Global.gR_Good = playClassItem.R_Good;
		Global.gR_Late = playClassItem.R_Late;
		Global.gR_Perfect = playClassItem.R_Perfect;
	

		Common_Reward.Item reward = Common_Reward.Get(Global.gRewardId);
		Global.gScoreGood = Base64Manager.instance.GlobalEncoding(reward.Common_good);
		Global.gScorePerfect = Base64Manager.instance.GlobalEncoding(reward.Common_perfect);
	
		Global.gRaceInfo.raceName = _mode.Name;
		Global.gRaceInfo.extraDollar = seaID;
		Global.gRaceInfo.mode1Rw =0;
		Global.gRaceInfo.extraCoin = 0;

		int sponID = GV.getTeamSponID(GV.SelectedTeamID);
		SaveSponBouns(sponID);
		UserDataManager.instance.NewGamePlaySetting();
		RaceModeSetting(_mode);
		string scene = Common_Track.Get(_mode.Track).Scene;

		SetTrackDurability(GV.PlayCarID, Common_Track.Get(_mode.Track));


		Global.isNetwork = true;
		Dictionary<string, int> mDic = new Dictionary<string, int>();
		mDic.Add("raceId",seaID);
		mDic.Add("fuelCount",GV.mUser.FuelCount);
		mDic.Add("carIdx", GV.getTeamCarIndex(GV.SelectedTeamID));
		mDic.Add("trackId",_mode.Track);
		mDic.Add("sponsorId",sponID);

		string mAPI = ServerAPI.Get(90004); // "game/race/champ"
		NetworkManager.instance.HttpFormConnect("Post", mDic, mAPI, (request)=>{
			Utility.Log("Response : " + request.response.Text);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				Utility.LogWarning("clear season play " + seaID);
				StartCoroutine("StartRacePlay",scene);	
			}else{
				Utility.LogError("state " + status + " Msg : " + thing["msg"]);
				Global.isNetwork = false;
				gameObject.GetComponent<LoadingGauage>().stopLoading();
			}
		});
	}
	*/

	public void setChampRace(){
		Global.isRaceTest = false;
		Common_Mode_Champion.Item _mode = Common_Mode_Champion.Get(GV.ChSeasonID);

		Global.gRaceInfo = new RaceInfo();
		Global.gRaceInfo.init();
		Global.gRaceInfo.trackID = _mode.Track.ToString();
		Global.gRaceInfo.iconName = _mode.Track.ToString()+"A";
	
		GV.PlayCarID = GV.getTeamCarID(GV.SelectedTeamID);
		GV.PlayCrewID = GV.getTeamCrewID(GV.SelectedTeamID);
		GV.PlaySponID = GV.getTeamSponID(GV.SelectedTeamID);
		GV.PlayClassID = GV.getTeamCarClass(GV.SelectedTeamID);
		Global.gRaceInfo.mType = MainRaceType.Champion;
		Global.gRaceInfo.sType = SubRaceType.RegularRace;


		Common_Class.Item playClassItem = GV.getTeamCarClassItem(GV.SelectedTeamID);
		Global.gP_MAX = playClassItem.p_max;
		Global.gP_MIN = playClassItem.p_min;
		Global.gR_Early = playClassItem.R_Early;
		Global.gR_Good = playClassItem.R_Good;
		Global.gR_Late = playClassItem.R_Late;
		Global.gR_Perfect = playClassItem.R_Perfect;
		Common_Reward.Item reward = Common_Reward.Get(Global.gRewardId);
		Global.gScoreGood = Base64Manager.instance.GlobalEncoding(reward.Common_good);
		Global.gScorePerfect = Base64Manager.instance.GlobalEncoding(reward.Common_perfect);
		Global.gRaceInfo.raceName = _mode.Name;
		Global.gRaceInfo.extraDollar = 0;
		Global.gRaceInfo.mode1Rw = reward.Champ_dollar;
		Global.gRaceInfo.extraCoin = reward.Champ_coin;
	
		RaceModeSetting(_mode);
		int sponID = GV.getTeamSponID(GV.SelectedTeamID);
		SaveSponBouns(sponID);
		UserDataManager.instance.NewGamePlaySetting();
		string scene = Common_Track.Get(_mode.Track).Scene;
		SetTrackDurability(GV.PlayCarID, Common_Track.Get(_mode.Track));
		setScrewDelay(GV.PlayCrewID);
		Global.isNetwork = true;
		Dictionary<string, int> mDic = new Dictionary<string, int>();
		mDic.Add("raceId",GV.ChSeasonID);
		mDic.Add("fuelCount",GV.mUser.FuelCount);
		mDic.Add("carIdx", GV.getTeamCarIndex(GV.SelectedTeamID));
		mDic.Add("trackId",_mode.Track);
		mDic.Add("sponsorId",sponID);

	

		NetworkManager.instance.HttpFormConnect("Post", mDic,  "game/race/champ", (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				StartCoroutine("StartRacePlay",scene);	
				//Utility.LogWarning("current season play " + GV.ChSeasonID);
				GoogleAnalyticsV4.instance.LogScreen(new AppViewHitBuilder().SetScreenName("ChampRace"));
			}else{
				//Utility.LogError("state " + status + " Msg : " + thing["msg"]);
				Global.isNetwork = false;
				gameObject.GetComponent<LoadingGauage>().stopLoading();
			}
		});


	}

	void RaceModeSetting(Common_Mode_Champion.Item _item){
		//int id = GV.ChSeasonID;
		Global.gAICarInfo = new AICarInfo[4];
		//Common_Mode_Champion.Item _item= Common_Mode_Champion.Get(id);
		ModeAICar.Item carItem = ModeAICar.Get(_item.Car_ID);
		ModeAICrew.Item crewItem = ModeAICrew.Get(_item.Crew_ID);

		UserDataManager.instance.modeAICarID = _item.Car_ID.ToString();
		UserDataManager.instance.modeAICrewID = _item.Crew_ID.ToString();

		/*int carValue =  GV.CarAbility;
		int crewValue =GV.CrewAbility;
		
		//!!--Utility.Log(string.Format("{0} -  {1}", carValue,crewValue));
		if(GV.ChSeasonLV == 1){
			carValue = carValue - 10*4;
			crewValue = crewValue- 10*4;
		}else if(GV.ChSeasonLV == 2){
			carValue = carValue - 10*3;
			crewValue = crewValue-10*3;
		}else if(GV.ChSeasonLV == 3){
			carValue = carValue - 10*2;
			crewValue = crewValue-10*2;
		}else if(GV.ChSeasonLV == 4){
			carValue = carValue - 10;
			crewValue = crewValue-10;
		}else if(GV.ChSeasonLV == 5){
			carValue = carValue + 10;
			crewValue = crewValue+10;
		}
		carItem = ModeAICar.GetRangeItem(carValue);
		crewItem = ModeAICrew.GetRangeItem(crewValue);
		*/
		Common_Class.Item cItem = GV.getClassTypeID(_item.Car_Class, 1);
		int TorqueRand = Random.Range(0, carItem.Torque_pr);
		int length = 1;
		Global.gAICarInfo[length] = new AICarInfo();
		Global.gAICarInfo[length].AICarID = Base64Manager.instance.GlobalEncoding(_item.Car);
		Global.gAICarInfo[length].AICrewID =  Base64Manager.instance.GlobalEncoding(_item.Crew);
		Global.gAICarInfo[length].AISponsorID = Base64Manager.instance.GlobalEncoding(_item.Sponsor);
		Global.gAICarInfo[length].Torque= Base64Manager.instance.setFloatEncoding((carItem.Torque+(float)TorqueRand), 1000);

		if(GV.ChSeasonID == 6000) Global.gAICarInfo[length].Pit_Time =Base64Manager.instance.setFloatEncoding(35.0f,1000);
		else if(GV.ChSeasonID == 6001) Global.gAICarInfo[length].Pit_Time =Base64Manager.instance.setFloatEncoding(30.0f,1000);
		else if(GV.ChSeasonID == 6002) Global.gAICarInfo[length].Pit_Time =Base64Manager.instance.setFloatEncoding(25.0f,1000);
		else Global.gAICarInfo[length].Pit_Time =Base64Manager.instance.setFloatEncoding(crewItem.Time_Pit,1000);

		Global.gAICarInfo[length].B_Power= Base64Manager.instance.setFloatEncoding(carItem.B_Power+cItem.Class_bspower,1000);
		Global.gAICarInfo[length].B_Time= Base64Manager.instance.setFloatEncoding(carItem.B_Time+cItem.Class_bstime,1000);
		Global.gAICarInfo[length].Skid_Time= Base64Manager.instance.setFloatEncoding(carItem.Skid_Time,1000);
		Global.gAICarInfo[length].gbLv=(int)carItem.Gear_LV;
		Global.gAICarInfo[length].EventTime= Base64Manager.instance.setFloatEncoding( (float)carItem.B_Event,1000);
		Global.gAICarInfo[length].EventRange=  Base64Manager.instance.setFloatEncoding( (float)carItem.B_Event_R,1000);
		Global.gAICarInfo[length].isLive = true;
		Common_Crew_Status.Item item = Common_Crew_Status.Get(_item.Crew);
		Global.gAICarInfo[length].userNick = item.Name;
		Global.gAICarInfo[length].AIRefCarID = Base64Manager.instance.GlobalEncoding( _item.Car_Cp);
		Global.gAICarInfo[length].AIRefStarLV = carItem.StarLV;
		gameObject.AddComponent<AIGearDelay>().NewMakeGearDelay(length);
		Global.gAICarInfo[length].AIClass = cItem;
		SaveDefaultAICarInfo(0);
		SaveDefaultAICarInfo(2);
		SaveDefaultAICarInfo(3);

		_item = null;
		item = null;
		carItem = null;
		crewItem = null;

	}


}
