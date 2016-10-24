using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SettingRaceRegular : SettingRaceEnv {
	public void setRegularRace(int id, System.Collections.Generic.List<RegularUserInfo> ComCar, int Track){
		Global.isRaceTest = false;
		//Common_Mode_Champion.Item _mode = Common_Mode_Champion.Get(GV.ChSeasonID);
		Global.gRaceInfo = new RaceInfo();
		Global.gRaceInfo.init();
		Global.gRaceInfo.trackID = Track.ToString();
		Global.gRaceInfo.iconName =Track.ToString()+"A";
		Global.gRaceInfo.mType = MainRaceType.Regular;
		
		GV.PlayCarID = GV.getTeamCarID(GV.SelectedTeamID);
		GV.PlayCrewID = GV.getTeamCrewID(GV.SelectedTeamID);
		GV.PlaySponID = GV.getTeamSponID(GV.SelectedTeamID);
		GV.PlayClassID = GV.getTeamCarClass(GV.SelectedTeamID);
		
		Common_Class.Item playClassItem = GV.getTeamCarClassItem(GV.SelectedTeamID);
		Global.gP_MAX = playClassItem.p_max;
		Global.gP_MIN = playClassItem.p_min;
		Global.gR_Early = playClassItem.R_Early;
		Global.gR_Good = playClassItem.R_Good;
		Global.gR_Late = playClassItem.R_Late;
		Global.gR_Perfect = playClassItem.R_Perfect;
		
		CTeamAbility _ability = new CTeamAbility();
		int a  = _ability.CarAbility(GV.PlayCarID , id);
		GV.CarAbility = a;
		int b = _ability.CrewAbility(GV.PlayCrewID, id);
		GV.CrewAbility = b;
		_ability = null;
		
		Common_Reward.Item reward = Common_Reward.Get(Global.gRewardId);
		Global.gScoreGood = Base64Manager.instance.GlobalEncoding(reward.Common_good);
		Global.gScorePerfect = Base64Manager.instance.GlobalEncoding(reward.Common_perfect);
		Common_Track.Item _mode = Common_Track.Get(Track);
		Global.gRaceInfo.raceName = _mode.Name;
		Global.gRaceInfo.extraDollar = 0;
		string raceAPI = string.Empty;
		if(id == 0){
			Global.gRaceInfo.sType = SubRaceType.RegularRace;
			Global.gRaceInfo.mode1Rw = reward.Reward_regular_stock;
			Global.gRaceInfo.extraCoin = 0;//reward.Reward_mat_timesquare;
			Global.gRaceInfo.rewardMatCount = 0;
			raceAPI = ServerAPI.Get(90027);//"game/race/regularTrack";
		}
		else {
			Global.gRaceInfo.sType = SubRaceType.DragRace;
			Global.DragSub = 0; // middle drag
			Global.gRaceInfo.mode1Rw = 0;//reward.Reward_mat2;
			Global.gRaceInfo.extraCoin =0;
			Global.gRaceInfo.rewardMatCount = reward.Reward_mat_regular_drag;
			raceAPI =ServerAPI.Get(90031);// "game/race/regularDrag";
		}
		
		setScrewDelay(GV.PlayCrewID);
		int sponID = GV.getTeamSponID(GV.SelectedTeamID);
		SaveSponBouns(sponID);
		UserDataManager.instance.NewGamePlaySetting();
		//Common_Class.Item cItem = Common_Class.Get(randomClassId(reward));//.getClassTypeID(GV.PlayClassID, 1);
		NewRaceModeSetting(ComCar);
		string scene = _mode.Scene;
		SetTrackDurability(GV.PlayCarID,_mode);
		
		Global.isNetwork = true;
		Dictionary<string, int> mDic = new Dictionary<string, int>();
		mDic.Add("raceId",GV.ChSeasonID);
		mDic.Add("fuelCount",GV.mUser.FuelCount);
		mDic.Add("carIdx", GV.getTeamCarIndex(GV.SelectedTeamID));
		mDic.Add("trackId",Track);
		mDic.Add("sponsorId",sponID);
		Global.rankRaceIdx = -1;
		NetworkManager.instance.HttpFormConnect("Post", mDic,raceAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				Global.rankRaceIdx = thing["result"]["raceIdx"].AsInt;
				StartCoroutine("StartRacePlay",scene);	
				GoogleAnalyticsV4.instance.LogScreen(new AppViewHitBuilder().SetScreenName("RegularRace"));
			}else{
				Utility.LogError("state " + status + " Msg : " + thing["msg"]);
				Global.isNetwork = false;
				gameObject.GetComponent<LoadingGauage>().stopLoading();
			}
		});
		
	}
	/*
	void RaceModeSetting(System.Collections.Generic.List<RegularAIUser> ComCar){
		Global.gAICarInfo = new AICarInfo[4];
		float[] racetime = new float[8];
		int carValue =  GV.CarAbility;
		int crewValue =GV.CrewAbility;

		//!!--Utility.Log(string.Format("{0} -  {1}", carValue,crewValue));
		int rLv  = Base64Manager.instance.GlobalEncoding(Global.gRegularRaceLevel);
		rLv = 10;
		if(rLv <= 3){
			carValue = carValue - 10*5;
			crewValue = crewValue-10*3;
		}else if(rLv > 3 && rLv <= 5){
			carValue = carValue - 10*2;
			crewValue = crewValue-10*1;
		}
		ModeAICar.Item _item = ModeAICar.GetRangeItem(carValue);
		ModeAICrew.Item item = ModeAICrew.GetRangeItem(crewValue);
		UserDataManager.instance.modeAICarID = _item.ID;
		UserDataManager.instance.modeAICrewID = item.ID;
		int TorqueRand =0;
		int[] range = gameObject.AddComponent<RandomCreate>().CreateRandomValue(5);
		Common_Class.Item cItem = GV.getClassTypeID(GV.PlayClassID, 1);
		for(int i = 0 ; i < ComCar.Count ; i++){
			TorqueRand = Random.Range(0, _item.Torque_pr);
			racetime[0] = (_item.Torque+(float)TorqueRand);
			racetime[1] = item.Time_Pit;
			racetime[2] = _item.B_Power+cItem.Class_bspower;
			racetime[3] = _item.B_Time+cItem.Class_bstime;
			racetime[4] = _item.Skid_Time;
			//racetime[5] =(float) _item.Gear_LV;
			racetime[5] =(float)Global.gMyGearLv;
			racetime[6] = (float)_item.B_Event;
			racetime[7] = (float)_item.B_Event_R;
			SaveUserInfo(i, ComCar[i].CarID, ComCar[i].CrewID,1301+range[i], racetime);
			Global.gAICarInfo[i].userTexture = ComCar[i].userProfile;
			Global.gAICarInfo[i].userNick = ComCar[i].strNick;
			Global.gAICarInfo[i].AIRefCarID =Base64Manager.instance.GlobalEncoding(ComCar[i].CarID);
			gameObject.AddComponent<AIGearDelay>().NewMakeGearDelay(i);
			Global.gAICarInfo[i].AIRefStarLV = _item.StarLV;
			Global.gAICarInfo[i].AIClass = cItem;
		}
		ComCar.Clear();
		_item = null;
		item = null;

		//cItem = null;

	}*/
	

	
	void NewRaceModeSetting(System.Collections.Generic.List<RegularUserInfo> ComCar){
		Global.gAICarInfo = new AICarInfo[4];
		float[] racetime = new float[8];
		int carValue =  GV.CarAbility;
		int crewValue =GV.CrewAbility;
		Common_Class.Item cItem;
		//!!--Utility.Log(string.Format("{0} -  {1}", carValue,crewValue));
		//int rLv  = Base64Manager.instance.GlobalEncoding(Global.gRegularRaceLevel);
		/*int rLv = 10;
		if(rLv <= 3){
			carValue = carValue - 10*5;
			crewValue = crewValue-10*3;
		}else if(rLv > 3 && rLv <= 5){
			carValue = carValue - 10*2;
			crewValue = crewValue-10*1;
		}*/
		ModeAICar.Item _item = ModeAICar.GetRangeItem(carValue);
		ModeAICrew.Item item = ModeAICrew.GetRangeItem(crewValue);//ModeAICrew.GetRangeItem( crewValue + Common_Crew_Status.Get(GV.getTeamCrewID(GV.SelectedTeamID)).Plus_Perform);
	//	UserDataManager.instance.modeAICarID = _item.ID;
	//	UserDataManager.instance.modeAICrewID = item.ID;
		int TorqueRand =0;
		int[] range = gameObject.AddComponent<RandomCreate>().CreateRandomValue(5);
		
		
		for(int i = 0 ; i < ComCar.Count ; i++){
		//	int cVal = crewValue + Common_Crew_Status.Get(ComCar[i].crewId).Plus_Perform;
		//	item = ModeAICrew.GetRangeItem(cVal);
		//	Utility.LogWarning(item.Time_Pit);
		//	Utility.LogWarning(Common_Crew_Status.Get(ComCar[i].crewId).Plus_Perform);
		//	Utility.LogWarning(crewValue);

			cItem = Common_Class.Get(ComCar[i].carClass);
			TorqueRand = Random.Range(0, _item.Torque_pr);
			racetime[0] = (_item.Torque+(float)TorqueRand);
			racetime[1] = item.Time_Pit;
			racetime[2] = _item.B_Power+cItem.Class_bspower;
			racetime[3] = _item.B_Time+cItem.Class_bstime;
			racetime[4] = _item.Skid_Time;
			//racetime[5] =(float) _item.Gear_LV;
			racetime[5] =(float)Global.gMyGearLv;
			racetime[6] = (float)_item.B_Event;
			racetime[7] = (float)_item.B_Event_R;
			SaveUserInfo(i, ComCar[i].carId, ComCar[i].crewId,1301+range[i], racetime);
			Global.gAICarInfo[i].userTexture = ComCar[i].userProfile;
			Global.gAICarInfo[i].userNick = ComCar[i].userNick;
			Global.gAICarInfo[i].AIRefCarID =Base64Manager.instance.GlobalEncoding(ComCar[i].carId);
			gameObject.AddComponent<AIGearDelay>().NewMakeGearDelay(i);
			Global.gAICarInfo[i].AIRefStarLV = _item.StarLV;
			Global.gAICarInfo[i].AIClass =cItem;
		}
		ComCar.Clear();
		_item = null;
		item = null;
		
		//cItem = null;
		
	}
	
}