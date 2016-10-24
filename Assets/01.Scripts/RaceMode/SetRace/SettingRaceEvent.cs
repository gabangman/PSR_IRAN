using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SettingRaceEvent : SettingRaceEnv {
	
	public void setEventRace(string mode, int Track, int mCarID){
		Global.isRaceTest = false;
		int id = int.Parse(mode);
		//	Common_Mode_Champion.Item _mode = Common_Mode_Champion.Get(GV.ChSeasonID);
		//	id = 1;
		Global.gRaceInfo = new RaceInfo();
		Global.gRaceInfo.init();
		Global.gRaceInfo.trackID = Track.ToString();
		
		GV.PlayCarID =mCarID;
		GV.PlayCrewID = GV.getTeamCrewID(GV.SelectedTeamID);
		GV.PlaySponID = GV.getTeamSponID(GV.SelectedTeamID);
		GV.PlayClassID = GV.getTeamCarClass(GV.SelectedTeamID);
		
		Common_Reward.Item reward = Common_Reward.Get(Global.gRewardId);
		Global.gScoreGood = Base64Manager.instance.GlobalEncoding(reward.Common_good);
		Global.gScorePerfect = Base64Manager.instance.GlobalEncoding(reward.Common_perfect);
		
		Global.gRaceInfo.raceName =  Common_Track.Get(Track).Name;
		Global.gRaceInfo.extraDollar = 0;
		Global.gRaceInfo.iconName = Track.ToString()+"A";
		Global.gRaceInfo.mType = MainRaceType.mEvent;
		if(Track == 1412) {
			Global.gRaceInfo.sType = SubRaceType.DragRace;
			Global.DragSub = 1;// long drag
			//	Global.DragSub = 0; 
		}
		else Global.gRaceInfo.sType = SubRaceType.RegularRace;
		
		Common_Class.Item playClassItem = GV.getTeamCarClassItem(GV.SelectedTeamID);
		Global.gP_MAX = playClassItem.p_max;
		Global.gP_MIN = playClassItem.p_min;
		Global.gR_Early = playClassItem.R_Early;
		Global.gR_Good = playClassItem.R_Good;
		Global.gR_Late = playClassItem.R_Late;
		Global.gR_Perfect = playClassItem.R_Perfect;
		string raceAPI = string.Empty;
		if(id == 0){
			int[] value = UserDataManager.instance.NewGamePlayFullySetting();
			Global.gRaceInfo.mode1Rw = reward.Reward_newcar; //dollar
			Global.gRaceInfo.extraCoin = 0;
			Global.gRaceInfo.eventModeName = "New";
			Global.gRaceInfo.rewardMatCount = 0;
			
			GV.PlayClassID = "SS";
			
			playClassItem =  GV.getClassTypeID(GV.PlayClassID, 1);
			Global.gP_MAX = playClassItem.p_max;
			Global.gP_MIN = playClassItem.p_min;
			Global.gR_Early = playClassItem.R_Early;
			Global.gR_Good = playClassItem.R_Good;
			Global.gR_Late = playClassItem.R_Late;
			Global.gR_Perfect = playClassItem.R_Perfect;
			myAccount.instance.account.eRace.testDrivePlayCount++;
			RaceModeSetting(value);
			raceAPI = ServerAPI.Get(90045);// "game/race/eventRace/testDrive";
			GV.PlaySponID = Random.Range(1300,1306);
		}else if(id == 1){
			
			Global.gRaceInfo.mode1Rw = 0;
			Global.gRaceInfo.extraCoin = 0;
			Global.gRaceInfo.eventModeName = "Select";
			Global.gRaceInfo.rewardMatCount = reward.Reward_selectcar; //material;
			myAccount.instance.account.eRace.featuredPlayCount++;
			
			raceAPI = ServerAPI.Get(90043);//"game/race/eventRace/featured";
			int mCnt = 0;
			//CarInfo carinfo;
			List<CarInfo> mList = new List<CarInfo>();
			for(int i = 0; i < GV.mineCarList.Count; i++){
				if(GV.mineCarList[i].CarID == mCarID){
					mList.Add(GV.mineCarList[i]);// = GV.mineCarList[i];
				}
			}
			if(mList.Count == 1) GV.PlayClassID = mList[0].ClassID;
			else {
				mList.Sort(delegate(CarInfo x, CarInfo y) {
					return x.nClassID.CompareTo(y.nClassID);
				});
				GV.PlayClassID = mList[0].ClassID;
			}
			UserDataManager.instance.NewGamePlaySetting(mList[0]);
			RaceModeSetting();
			
		}else{
			UserDataManager.instance.NewGamePlaySetting();
			Global.gRaceInfo.mode1Rw = 0;
			Global.gRaceInfo.extraCoin = 0;
			Global.gRaceInfo.eventModeName = "Qube";
			Global.gRaceInfo.rewardMatCount = reward.Reward_resource; //evo
			myAccount.instance.account.eRace.EvoPlayCount++;
			RaceModeSetting();
			raceAPI = ServerAPI.Get(90047);// "game/race/eventRace/evoCube";
		}
		setScrewDelay(GV.PlayCrewID);
		int sponID = GV.getTeamSponID(GV.SelectedTeamID);
		SaveSponBouns(sponID);
		string SceneName = Common_Track.Get(Track).Scene;
		SetTrackDurability(GV.PlayCarID, Common_Track.Get(Track));
		
		StartCoroutine(EventRaceCheck(raceAPI, id, SceneName));
		
		
	}
	
	IEnumerator EventRaceCheck(string raceAPI, int id, string sceneN){
		yield return null;
		Global.isNetwork = true;
		/*if(id == 2){
			bool isConnect = false;
			string mAPI = ServerAPI.Get(90049); // "game/race/eventRace/count/"
			NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					if(id == 2){
						myAccount.instance.account.eRace.EvoPlayCount = thing["result"]["evoCubeCnt"].AsInt;
						if(myAccount.instance.account.eRace.EvoPlayCount != 0){
							UserDataManager.instance.raceFuelCounting();
						}else{
							gameObject.GetComponent<EventSubMode>().NothingEvoCubeInRace();
							gameObject.GetComponent<LoadingGauage>().stopLoading();
							Global.isNetwork = false;
						}
					}
				}else{
					
				}
				isConnect = true;
			});

			while(!isConnect){
				yield return null;
			}
			if(!Global.isNetwork) yield break;
		}*/
		
		Dictionary<string, int> mDic = new Dictionary<string, int>();
		mDic.Add("raceId",GV.ChSeasonID);
		int fCnt = GV.mUser.FuelCount;
		if(id == 2) fCnt--;
		mDic.Add("fuelCount",fCnt);
	//	Utility.LogWarning(GV.mUser.FuelCount);
		Global.rankRaceIdx = -1;
		NetworkManager.instance.HttpFormConnect("Post", mDic,raceAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				Global.rankRaceIdx = thing["result"]["raceIdx"].AsInt;
				if(id==2) {
					UserDataManager.instance.raceFuelCounting();
					myAccount.instance.account.eRace.EvoPlayCount = thing["result"]["evoCubeCnt"].AsInt;
				}
				StartCoroutine("StartRacePlay",sceneN);	
				GoogleAnalyticsV4.instance.LogScreen(new AppViewHitBuilder().SetScreenName("EventRace"));
			}else if(status == -300){
				myAccount.instance.account.eRace.EvoPlayCount = 0;
				gameObject.GetComponent<EventSubMode>().NothingEvoCubeInRace();
				gameObject.GetComponent<LoadingGauage>().stopLoading();
				Global.isNetwork = false;
				
			}else{
				Utility.LogError("state " + status + " Msg : " + thing["msg"]);
				Global.isNetwork = false;
				gameObject.GetComponent<LoadingGauage>().stopLoading();
			}
		});
		
	}
	
	
	
	void RaceModeSetting(int[] value = null){
		System.Collections.Generic.List<RegularAIUser> ComCar
			= new System.Collections.Generic.List<RegularAIUser>();
		int a = Common_Car_Status.wholeCarList.Count ;
		int mCrewCount = Common_Crew_Status.crewListItem.Count;
		int[] rand1 = gameObject.AddComponent<RandomCreate>().CreateRandomValue(a);
		int[] rand = gameObject.AddComponent<RandomCreate>().CreateRandomValue(8);
		int dragmode = 0;
		int carValue =  0;
		int crewValue =0;
		
		if(Global.gRaceInfo.sType == SubRaceType.DragRace){
			dragmode = 1;
			carValue = value[0];
			crewValue = value[1];
		}else{
			if(Global.gRaceInfo.eventModeName == "Select"){
				dragmode = 4;
			}else{
				dragmode = 6;
			}
			if(value == null) {
				carValue =GV.CarAbility;
				crewValue = GV.CrewAbility;
			}else{
				carValue = value[0];
				crewValue = value[1];
			}
		}
		/*int rLv  = 10;//Base64Manager.instance.GlobalEncoding(Global.gRegularRaceLevel);
		if(rLv <= 3){
			carValue = carValue - 10*5;
			crewValue = crewValue-10*3;
		}else if(rLv > 3 && rLv <= 5){
			carValue = carValue - 10*2;
			crewValue = crewValue-10*1;
		}*/
		
		
		Common_Class.Item cItem = GV.getClassTypeID(GV.PlayClassID, 1);
		for(int i =0; i < dragmode; i++){
			int n = Random.Range(0,mCrewCount);
			ComCar.Add(new RegularAIUser(Common_Car_Status.wholeCarList[rand1[i]], Common_Crew_Status.crewListItem[n]));
			int _nick = 73032 + rand[i];
			ComCar[i].strNick = KoStorage.GetKorString(_nick.ToString());
			//ComCar[i].strProfileUrl = "https://s3-ap-northeast-1.amazonaws.com/gabangman01/MultiPicture/MultiCom_"+(rand[i]+1).ToString()+".png";
			ComCar[i].strProfileUrl = "MultiCom_"+(Random.Range(0,9)).ToString();//+".png";
			Texture Tex =  (Texture)Resources.Load("ComPicture/"+ComCar[i].strProfileUrl, typeof(Texture));
			ComCar[i].userProfile =(Texture2D) Tex;
		}
		int len = ComCar.Count;
		if(len == 6){
			EvoCubeRaceSetting(ComCar);
			return;
		}

		Global.gAICarInfo = new AICarInfo[len];
	
		float[] racetime = new float[8];
		if(Global.gRaceInfo.eventModeName == "Select"){
			carValue = carValue + (myAccount.instance.account.eRace.featuredPlayCount-1)*10;
			crewValue = crewValue +(myAccount.instance.account.eRace.featuredPlayCount-1)*12;
		}else if(Global.gRaceInfo.eventModeName == "New"){
			carValue = carValue + (myAccount.instance.account.eRace.testDrivePlayCount-1)*10;
		}

		ModeAICar.Item _item = ModeAICar.GetRangeItem(carValue);
		ModeAICrew.Item item =  ModeAICrew.GetRangeItem(crewValue);
	//	UserDataManager.instance.modeAICarID = _item.ID;
	//	UserDataManager.instance.modeAICrewID = item.ID;
		int TorqueRand=  0;
		int[] range = gameObject.AddComponent<RandomCreate>().CreateRandomValue(5);
		
		for(int i = 0 ; i < ComCar.Count ; i++){
		//	int cVal = crewValue + Common_Crew_Status.Get(ComCar[i].CrewID).Plus_Perform;
		//	item = ModeAICrew.GetRangeItem(crewValue);
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
			Global.gAICarInfo[i].AIRefStarLV = _item.StarLV;
			Global.gAICarInfo[i].AIClass = cItem;
		}
		ComCar.Clear();
		_item = null;
		item = null;
	}
	void EvoCubeRaceSetting(System.Collections.Generic.List<RegularAIUser> ComCar){
		Global.gAICarInfo = new AICarInfo[6];
		float[] racetime = new float[8];
		int carValue =  GV.CarAbility;
		int crewValue =GV.CrewAbility;
		Common_Class.Item cItem;
		int rLv = 10;
		if(rLv <= 3){
			carValue = carValue - 10*5;
			crewValue = crewValue-10*3;
		}else if(rLv > 3 && rLv <= 5){
			carValue = carValue - 10*2;
			crewValue = crewValue-10*1;
		}
		ModeAICar.Item _item = ModeAICar.GetRangeItem(carValue);
		ModeAICrew.Item item =  ModeAICrew.GetRangeItem(crewValue);
		int TorqueRand =0;
		int[] range = gameObject.AddComponent<RandomCreate>().CreateRandomValue(5);
		
		Common_Reward.Item reward = Common_Reward.Get(Global.gRewardId);
		for(int i = 0 ; i < ComCar.Count ; i++){
		//	int cVal = crewValue + Common_Crew_Status.Get(ComCar[i].CrewID).Plus_Perform;
		//	item = ModeAICrew.GetRangeItem(crewValue);
			int carClass = randomClassId(reward);
			cItem = Common_Class.Get(carClass);
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
			Global.gAICarInfo[i].AIRefStarLV = _item.StarLV;
			Global.gAICarInfo[i].AIClass =cItem;
		}
		ComCar.Clear();
		_item = null;
		item = null;
		
		//cItem = null;
		
	}
	void DragRaceModeSetting(){
		
	}

	int randomClassId(Common_Reward.Item rItem){
		int rClass = 0;
		ProbabilityClass pb = new ProbabilityClass();
		rClass = pb.RegularRaceClass(rItem);
		pb =null;
		return rClass;
	}
}
