using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SettingRacePVP : SettingRaceEnv {
	public void setPVPRace(int id, int Track, System.Collections.Generic.List<RaceUserInfo> ComCar){
		Global.isRaceTest = false;

		Global.gRaceInfo = new RaceInfo();
		Global.gRaceInfo.init();
		Global.gRaceInfo.trackID = Track.ToString();
		
		GV.PlayCarID =GV.getTeamCarID(GV.SelectedTeamID);
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

		Common_Reward.Item reward = Common_Reward.Get(Global.gRewardId);
		Global.gScoreGood = Base64Manager.instance.GlobalEncoding(reward.Common_good);
		Global.gScorePerfect = Base64Manager.instance.GlobalEncoding(reward.Common_perfect);
		
		Global.gRaceInfo.raceName =  Common_Track.Get(Track).Name;
		Global.gRaceInfo.extraDollar = 0;
		Global.gRaceInfo.iconName = Track.ToString()+"A";
		Global.gRaceInfo.mType = MainRaceType.PVP;
		string raceAPI = string.Empty;
		if(id == 0){ //drag
			Global.gRaceInfo.sType = SubRaceType.DragRace;
			Global.gRaceInfo.mode1Rw = reward.Reward_PVP_drag;
			Global.gRaceInfo.extraCoin = 0;
			Global.gRaceInfo.rewardMatCount = 0; //material
			Global.DragSub = 1; // long drag
			raceAPI  =  ServerAPI.Get(90035);// "game/race/pvpDrag";
		}else if(id == 1){
			Global.gRaceInfo.sType = SubRaceType.CityRace;
			Global.gRaceInfo.mode1Rw = 0;
			Global.gRaceInfo.extraCoin = 0;
			Global.gRaceInfo.rewardMatCount =reward.Reward_mat_timesquare; //material
			raceAPI  = ServerAPI.Get(90039); // "game/race/pvpTimesquare";
		}
		UserDataManager.instance.NewGamePlaySetting();
		NewRaceModeSetting(ComCar);
		setScrewDelay(GV.PlayCrewID);
		int sponID = GV.getTeamSponID(GV.SelectedTeamID);
		SaveSponBouns(sponID);
		string scene = Common_Track.Get(Track).Scene;
		SetTrackDurability(GV.PlayCarID, Common_Track.Get(Track));


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
				GoogleAnalyticsV4.instance.LogScreen(new AppViewHitBuilder().SetScreenName("PVPRace"));
			}else{
				Utility.LogError("state " + status + " Msg : " + thing["msg"]);
				Global.isNetwork = false;
				gameObject.GetComponent<LoadingGauage>().stopLoading();
			}
		});
	}

	void NewRaceModeSetting(System.Collections.Generic.List<RaceUserInfo> ComCar){
		int len = ComCar.Count;
		Global.gAICarInfo = new AICarInfo[len];
		//	//!!--Utility.Log(ComCar.Count);
		float[] racetime = new float[8];
		for(int i = 0 ; i < ComCar.Count ; i++){
			if(!ComCar[i].bUser){
				RandomUserSetting(i, ComCar[i]);
				continue;
			}
			if(ComCar[i].carClass == 3106){
				racetime[0] = ComCar[i].Torque;
			}else{
				racetime[0] = ComCar[i].Torque + Random.Range(1,5000)*0.001f;
			}
		
			racetime[1] =ComCar[i].PitTime;
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
		//	Utility.LogWarning(ComCar[i].carClass);
		}
		ComCar.Clear();
	}
	void RandomUserSetting(int idx, RaceUserInfo userInfo){
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
		GV.CarAbility += rand;
		ModeAICar.Item _item = ModeAICar.GetRangeItem(carValue);
		ModeAICrew.Item item = null;// ModeAICrew.GetRangeItem(crewValue);
		int cVal = crewValue + Common_Crew_Status.Get(userInfo.crewId).Plus_Perform;
		item = ModeAICrew.GetRangeItem(crewValue);
		int TorqueRand = 0;
		int[] range = gameObject.AddComponent<RandomCreate>().CreateRandomValue(5);
		Common_Class.Item cItem = GV.getClassTypeID(GV.PlayClassID, 1);
		TorqueRand = Random.Range(0, _item.Torque_pr);
		racetime[0] = (_item.Torque+(float)TorqueRand);
		racetime[1] = item.Time_Pit;
		racetime[2] = _item.B_Power+cItem.Class_bspower;
		racetime[3] = _item.B_Time+cItem.Class_bstime;
		racetime[4] = _item.Skid_Time;
		racetime[5] =(float)Global.gMyGearLv;
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
	//	Utility.LogWarning(cItem.ID);
		_item = null;
		item = null;
		
	}



}
