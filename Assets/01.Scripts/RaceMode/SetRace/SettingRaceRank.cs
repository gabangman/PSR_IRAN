using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SettingRaceRank : SettingRaceEnv {

	public void setRankingRace(int id,  List<RaceUserInfo> userCar, int Track){
		Global.isRaceTest = false;
		Global.gRaceInfo = new RaceInfo();
		Global.gRaceInfo.init();
		Global.gRaceInfo.trackID = Track.ToString();
		Global.gRaceInfo.iconName =Track.ToString()+"A";
		Global.gRaceInfo.mType = MainRaceType.Weekly;
		Global.gRaceInfo.sType = SubRaceType.RegularRace;
		Common_Reward.Item reward = Common_Reward.Get(Global.gRewardId);
		Common_Class.Item playClassItem = GV.getTeamCarClassItem(GV.SelectedTeamID);
		Global.gP_MAX = playClassItem.p_max;
		Global.gP_MIN = playClassItem.p_min;
		Global.gR_Early = playClassItem.R_Early;
		Global.gR_Good = playClassItem.R_Good;
		Global.gR_Late = playClassItem.R_Late;
		Global.gR_Perfect = playClassItem.R_Perfect;

		if(id == 0){
			Global.gRaceInfo.mode1Rw = 0;
			Global.gRaceInfo.extraCoin = 0;
			Global.gRaceInfo.rewardMatCount = 0; //throphy
		}else{ //!!--Utility.Log("setRankingRace : Weekly");
	//	else if ( id == 1){
	//		Global.gRaceInfo.modeType = RaceModeType.TouringMode;
	//		GV.SelectedTeamCode = 1;
	//		Global.gRaceInfo.mode1Rw = 0;
	//		Global.gRaceInfo.extraCoin = 0;
	//	}
	//	else if(id == 2){
	//		Global.gRaceInfo.modeType = RaceModeType.DragMode;
	//		Global.gRaceInfo.mode1Rw = reward.Reward_drag;
	//		Global.gRaceInfo.extraCoin = 0;
	//	}else if( id == 3){
	//		Global.gRaceInfo.modeType = RaceModeType.EventMode;
	//		Global.gRaceInfo.mode1Rw = 0;
	//		Global.gRaceInfo.extraCoin = reward.mat_timesquare;
		}

		GV.PlayCarID = GV.getTeamCarID(GV.SelectedTeamID);
		GV.PlayCrewID = GV.getTeamCrewID(GV.SelectedTeamID);
		GV.PlaySponID = GV.getTeamSponID(GV.SelectedTeamID);
		GV.PlayClassID = GV.getTeamCarClass(GV.SelectedTeamID);
		CTeamAbility _ability = new CTeamAbility();
		int a  = _ability.CarAbility(GV.PlayCarID , id);
		GV.CarAbility = a;
		int b = _ability.CrewAbility(GV.PlayCrewID, id);
		GV.CrewAbility = b;
	//	Utility.LogWarning("GV.carability " + GV.CarAbility);
	//	Utility.LogWarning("GV.crewability " + GV.CrewAbility);
		_ability = null;
		Global.gScoreGood = Base64Manager.instance.GlobalEncoding(reward.Common_good);
		Global.gScorePerfect = Base64Manager.instance.GlobalEncoding(reward.Common_perfect);
		Common_Track.Item _mode = Common_Track.Get(Track);
		Global.gRaceInfo.raceName = _mode.Name;
		Global.gRaceInfo.extraDollar = 0;
	//	Global.gRaceInfo.mode1Rw = reward.S1_C;
   // Global.gRaceInfo.extraCoin = reward.Coin;
		setScrewDelay(GV.PlayCrewID);
		int sponID = GV.getTeamSponID(GV.SelectedTeamID);
		SaveSponBouns(sponID);
		UserDataManager.instance.NewGamePlaySetting();
		NewRaceModeSetting(userCar);
		string scene = _mode.Scene;
		SetTrackDurability(GV.PlayCarID,_mode);


		int cId = GV.getTeamCarClassId(GV.SelectedTeamID);
		Dictionary<string, int> mDic = new Dictionary<string, int>();
		mDic.Add("raceId",GV.ChSeasonID);
		mDic.Add("fuelCount",GV.mUser.FuelCount);
		mDic.Add("carId", GV.PlayCarID);
		mDic.Add("carClass",cId);
		mDic.Add("teamId", GV.SelectedTeamID);
		mDic.Add("carIdx", GV.getTeamCarIndex(GV.SelectedTeamID));
		mDic.Add("trackId",Track);
		mDic.Add("sponsorId",sponID);

		string mAPI = ServerAPI.Get(90023);//"game/race/rank"
		NetworkManager.instance.HttpFormConnect("Post", mDic, mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				//response:{"state":0,"msg":"sucess","result":{"rankRaceIdx":16},"time":1445956677}
				Global.rankRaceIdx = thing["result"]["rankRaceIdx"].AsInt;
				StartCoroutine("StartRacePlay",scene);	
				GoogleAnalyticsV4.instance.LogScreen(new AppViewHitBuilder().SetScreenName("RankRace"));
			}else{
				Utility.LogError("state " + status + " Msg : " + thing["msg"]);
				Global.isNetwork = false;
				gameObject.GetComponent<LoadingGauage>().stopLoading();
			}
		});

	}
	
	void RaceModeSetting(System.Collections.Generic.List<RegularAIUser> ComCar){
		int len = ComCar.Count;
		Global.gAICarInfo = new AICarInfo[len];
		float[] racetime = new float[8];

		int carValue =  GV.CarAbility;
		int crewValue =GV.CrewAbility;
		int rand = (int)Well512.Next(0,6);
		int a = Random.Range(0,10);
		if(a % 2 == 0){
			rand = -rand*3;
		}else{
			rand = rand*3;
		}
		carValue += rand;
		ModeAICar.Item _item = ModeAICar.GetRangeItem(carValue);
		ModeAICrew.Item item = ModeAICrew.GetRangeItem(crewValue);
		UserDataManager.instance.modeAICarID = _item.ID;
		UserDataManager.instance.modeAICrewID = item.ID;
		int TorqueRand = 0;
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
			int n = Random.Range(0,5);
			SaveUserInfo(i, ComCar[i].CarID, ComCar[i].CrewID,1301+range[n], racetime);
			Global.gAICarInfo[i].userTexture = ComCar[i].userProfile;
			Global.gAICarInfo[i].userNick = ComCar[i].strNick;
			Global.gAICarInfo[i].AIRefCarID =Base64Manager.instance.GlobalEncoding(ComCar[i].CarID);
			gameObject.AddComponent<AIGearDelay>().NewMakeGearDelay(i);
			//gameObject.AddComponent<AIGearDelay>().MakePressDelay(i);
			//gameObject.AddComponent<AIGearDelay>().RankRaceGearLedSetting(wSNS[i].gearPressDelay, i);
			Global.gAICarInfo[i].AIRefStarLV = _item.StarLV;
			Global.gAICarInfo[i].AIClass = cItem;

		}
		ComCar.Clear();
		_item = null;
		item = null;

	}

	void NewRaceModeSetting(System.Collections.Generic.List<RaceUserInfo> ComCar){
		int len = ComCar.Count;
		Global.gAICarInfo = new AICarInfo[len];
		//Utility.LogWarning(ComCar.Count);
		float[] racetime = new float[8];
		for(int i = 0 ; i < ComCar.Count ; i++){
			if(!ComCar[i].bUser){
				RandomUserSetting(i, ComCar[i]);
				continue;
			}else{
				int crewValue = GV.CrewAbility;
				int min = ComCar[i].crewAbility- crewValue;
				if(min < -50){
					crewValue = crewValue - Random.Range(0,50);
					ModeAICrew.Item item = ModeAICrew.GetRangeItem(crewValue);
					racetime[1] = item.Time_Pit;
				}else{
					racetime[1] =ComCar[i].PitTime;
				}
				racetime[0] = ComCar[i].Torque;
				racetime[2] =ComCar[i].BSPower;
				racetime[3] = ComCar[i].BSTime;;
				racetime[4] =ComCar[i].TireTime;
				racetime[5] =(float)ComCar[i].GBLv;
				racetime[6] = (float)ComCar[i].BSPressTime;
				racetime[7] =0.0f;
				int n = Random.Range(0,5);
				SaveUserInfo(i, ComCar[i].carId, ComCar[i].crewId,ComCar[i].sponId, racetime);
				Global.gAICarInfo[i].userTexture = ComCar[i].userProfile;
				Global.gAICarInfo[i].userNick = ComCar[i].userNick;
				Global.gAICarInfo[i].AIRefCarID =Base64Manager.instance.GlobalEncoding(ComCar[i].carId);
				Global.gAICarInfo[i].gearLedDelay = new float[20];
				System.Array.Copy(ComCar[i].fG,Global.gAICarInfo[i].gearLedDelay, ComCar[i].fG.Length);
				Global.gAICarInfo[i].pressDelay = new float[20];
				System.Array.Copy(ComCar[i].pD,Global.gAICarInfo[i].pressDelay, ComCar[i].pD.Length);
				Global.gAICarInfo[i].AIClass = Common_Class.Get(ComCar[i].carClass);
			}
		}
		ComCar.Clear();
	}
	void RandomUserSetting(int idx, RaceUserInfo userInfo){
		float[] racetime = new float[8];
		int carValue =  GV.CarAbility;
		int crewValue =GV.CrewAbility;
		if(userInfo.DefaultCarAblility == 0){
		
		}else if(userInfo.DefaultCarAblility == 300){
			carValue = GV.CarAbility + 300;
			//crewValue = userInfo.crewAbility;
		}else if(userInfo.DefaultCarAblility == -100){
			carValue = GV.CarAbility -100;
			//crewValue = userInfo.crewAbility;
		}

		int rand = (int)Well512.Next(0,6);
		int a = Random.Range(0,10);
		if(a % 2 == 0){
			rand = -rand*3;
		}else{
			rand = rand*3;
		}
		carValue += rand;
		ModeAICar.Item _item = ModeAICar.GetRangeItem(carValue);
		UserDataManager.instance.modeAICarID = _item.ID;
		int TorqueRand = 0;
		int[] range = gameObject.AddComponent<RandomCreate>().CreateRandomValue(5);
		Common_Class.Item cItem = GV.getClassTypeID(GV.PlayClassID, 1);
			TorqueRand = Random.Range(0, _item.Torque_pr);
			racetime[0] = (_item.Torque+(float)TorqueRand);
			int min = userInfo.crewAbility- crewValue;
			if(min < -50){
				crewValue = crewValue - Random.Range(0,50);
				ModeAICrew.Item item = ModeAICrew.GetRangeItem(crewValue);
				racetime[1] = item.Time_Pit;
			}else{
				racetime[1] =userInfo.PitTime;
			}
			racetime[2] = _item.B_Power+cItem.Class_bspower;
			racetime[3] = _item.B_Time+cItem.Class_bstime;
			racetime[4] = _item.Skid_Time;
			//racetime[5] =(float) _item.Gear_LV;
			racetime[5] =(float)userInfo.GBLv;
			racetime[6] = (float)_item.B_Event;
			racetime[7] = (float)_item.B_Event_R;
			int n = Random.Range(0,5);
		SaveUserInfo(idx, userInfo.carId, userInfo.crewId,1301+range[n], racetime);
		Global.gAICarInfo[idx].userTexture =userInfo.userProfile;
		Global.gAICarInfo[idx].userNick = userInfo.userNick;
		Global.gAICarInfo[idx].AIRefCarID =Base64Manager.instance.GlobalEncoding(userInfo.carId);
		gameObject.AddComponent<AIGearDelay>().NewMakeGearDelay(idx);
		Global.gAICarInfo[idx].AIRefStarLV = _item.StarLV;
		Global.gAICarInfo[idx].AIClass = cItem;
		//Utility.LogWarning("cnt " + idx + "name " +	Global.gAICarInfo[idx].userNick );
	//	}
	//	ComCar.Clear();
		_item = null;
	}


}
