using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
[System.Serializable] 
public static class GV {
	//appInfo
	public static appInfo gInfo;
	//userInfo
	public static bool QuestReset = false;
	public static int gRaceCount = 0;
	public static bool bRaceLose = false;
	public static bool bHelp = false;
	public static bool gRootPhone = false;
	public static int SelTeamID = 10;
	public static int TeamChangeFlag = 0;
	public static int ChSeasonID = 6000;
	public static int myCoin =0;
	public static int myDollar=0;
	public static int fuelTime;
	public static int SelectedTeamID = 0;
	public static int SponID;
	public static int gVIP = 1; // 1 : on 0 : off
	public static int vipExp = 0;
	public static float gVIPExp = 0;
	public static int gADInit = 0;
	public static int SelectedSponTeamID = 10;
	public static int ipAddressVersion = 0;
	public static string mAPI = null;
	public static string mException = null;
	public static long mReTime = 0;
	public static int responsStatus = 0;
	//temp
	public static int ChSeason = 1;
	public static int ChSeasonLV = 1;
	public static int PlayCarID;
	public static int PlayCrewID;
	public static int PlaySponID;
	public static string PlayClassID;
	public static int PlayTeamID;
	public static int PlayDruability;
	public static int CarCount;
	public static int CarAbility;
	public static int CrewAbility;
	//money
	public static int updateCoin;
	public static int updateDollar;
	//User
	public static string UserKey;
	public static string UserNick;
	public static string UserRevId;
	
	public static string gNational;
	public static Texture DefaultProfileTexture;
	public static bool isStarLV = false;
	
	//Race
	public static int gChamWin = 2;
	public static int RegularTrack = 1401;
	public static int EventTrack  = 1401;
	public static int gEntryFee = 0;
//	public static int gDealer = 0;
//	public static int gDealerCarID = 0;
//	public static string gDealerClass = "D";
//	public static int gBuyDealerCar = 0;
	public static int gADWindow = 0;
	public static int gTapJoy=0;
	public static int LuckyCarID = 1000;
	public static string LuckyCarClass ="D";
	public static int LuckyCarLV = 1;
	public static int LuckyCrewLV = 1;
	public static int LuckyCarClick = 0;
	
	public static int CurrADId = 8750 ;// max 8765
	public static readonly int MaxADId = 8764;
	public static long[] RewardViewTime = new long[3];
	
	public static List<int> myCouponList = new List<int>(); // 0 silver , 1 : Gold
	public static List<CarInfo> mineCarList;
	
	//public static int CarFlag;
	public readonly static int QubeID = 8620;
	public readonly static int GoldID =9518;
	public readonly static int SilverID = 9500;
	public static bool mInviteFlag;
	public static bool bDayReset = false;
	public static bool bWeeklyReset=false;
	public static bool bWeeklyResetFlag=false;
	//web
	public static UserInfo mUser = new UserInfo();
	//Club


	//other
	public static List<myTeamInfo> listmyteaminfo;
	public static List<MatInfo> listMyMat;
	public static int[] arrayMatID;
	public static int achieveId = 0;
	public static bool bachieveRewardFlag;
	public static string signedData;
	public static string signature;
	public static string purchaseToken;
	public static string pKey;
	public static string gNationName = string.Empty;
	public static int gRaceMode = 5;
	public static int gPlusEvent = 0;
	public static bool bLoding = false;
	public static void InitAccountValue(){
	
	} // Global Init
	
	public static void DeleteMyCarList(int CarID, string strClass, int idx){
		
	}
	
	public static void DeleteMyCarList(int idx){
		//CarInfo _carInfo = mineCarList[idx];
		mineCarList.RemoveAt(idx);
	}
	public static void AddMyCarList(int CarID, string strClass){
		/*int model = (strClass.Equals("S"))?3:1;
		CarInfo carinfo = new CarInfo(CarID,strClass,0,model);
		Common_Class.Item item1	 = getClassTypeID(strClass, model);
		int du = Common_Car_Status.Get(CarID).Durability +item1.Durability ;
		carinfo.carClass.SetClass1(item1.UpLimit, item1.StarLV, du, item1.Repair, item1.Brake,  item1.Class_power);
		carinfo.carClass.SetClass2(item1.Class_weight,item1.Class_grip,  item1.Class_gear, 
		                           item1.Class_bspower, item1.Class_bstime, item1.Repair, du );
		carinfo.carClass.SetClass3(item1.GearLmt);
		int mCnt = mineCarList[mineCarList.Count-1].Index;
		carinfo.setCarIndex(mCnt+1);
		carinfo.CarClassItem = item1;
		carinfo.SetFlag = 1;
		
		mineCarList.Add(carinfo);*/
		
		
		string mAPI = ServerAPI.Get(90012); // game/car/
		NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				GameObject.Find("LoadScene").SendMessage("builtInPopup"); //서버 점검 중
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{			
				int i = thing["result"].Count-1;
				int nId =  thing["result"][i]["carId"].AsInt;
				CarInfo carItem = new CarInfo(nId);
				carItem.CarIndex = thing["result"][i]["idx"].AsInt;
				carItem.CarID =nId;
				carItem.nClassID = thing["result"][i]["carClass"].AsInt;
				
				
				carItem.bodyLv = thing["result"][i]["partLevel"]["1"].AsInt;
				carItem.engineLv =  thing["result"][i]["partLevel"]["2"].AsInt;
				carItem.tireLv = thing["result"][i]["partLevel"]["3"].AsInt;
				carItem.gearBoxLv = thing["result"][i]["partLevel"]["4"].AsInt;
				carItem.intakeLv =  thing["result"][i]["partLevel"]["5"].AsInt;
				carItem.bsPowerLv = thing["result"][i]["partLevel"]["6"].AsInt;
				carItem.bsTimeLv =  thing["result"][i]["partLevel"]["7"].AsInt;
				
				carItem.bodyStar = thing["result"][i]["partStar"]["1"].AsInt - 1;
				carItem.engineStar =  thing["result"][i]["partStar"]["2"].AsInt-1;
				carItem.tireStar = thing["result"][i]["partStar"]["3"].AsInt-1;
				carItem.gearBoxStar = thing["result"][i]["partStar"]["4"].AsInt-1;
				carItem.intakeStar =  thing["result"][i]["partStar"]["5"].AsInt-1;
				carItem.bsPowerStar = thing["result"][i]["partStar"]["6"].AsInt-1;
				carItem.bsTimeStar =  thing["result"][i]["partStar"]["7"].AsInt-1;
				
				carItem.durability = thing["result"][i]["durability"].AsInt;
				carItem.SetFlag = 1;
				string str = ChangeCarClassIDString(carItem.nClassID);
				carItem.ClassID = str;
				Common_Class.Item item1	 = getClassTypeID( str, 1);
				carItem.CarClassItem = item1;
			
				int duRef = Common_Car_Status.Get(carItem.CarID).Durability +item1.Durability ;
				carItem.carClass.SetClass1(item1.UpLimit, item1.StarLV, carItem.durability, item1.Repair, item1.Brake,  item1.Class_power);
				carItem.carClass.SetClass2(item1.Class_weight,item1.Class_grip,  item1.Class_gear, 
				                            item1.Class_bspower, item1.Class_bstime, item1.Repair,  duRef );
				carItem.carClass.SetClass3(item1.GearLmt);
				carItem.bNewBuyCar = true;
				mineCarList.Add(carItem);
				GV.mineCarList.Sort(delegate(CarInfo x, CarInfo y) {
					int a = x.CarID.CompareTo(y.CarID);
					if(a == 0)
						a = x.nClassID.CompareTo(y.nClassID);
					return a;
					
				});
			//	Utility.LogWarning(string.Format("carIndex {0}, CarId {1}, CarClass {2}, Dura {3}, Dura1 {4} DuraRef {5}  carDrua {6}, classDura {7}" ,
			//	                               carItem.CarIndex, carItem.CarID, carItem.nClassID, carItem.durability, carItem.carClass.Durability, 
			//	                               carItem.carClass.DurabilityRef, Common_Car_Status.Get(carItem.CarID).Durability, item1.Durability));
			}else{
				
			}
			
		});
	}
	
	
	public static void ModifyMyCarList(){
		int nCnt = mineCarList.Count;
		if(mineCarList[0].SetFlag == 1) return;
		for(int i = 0; i < nCnt; i++){
			CarInfo nCarinfo = mineCarList[i];
			string strClass = ChangeCarClassIDString(nCarinfo.nClassID);
			nCarinfo.ClassID = strClass;
			//CarInfo carinfo = new CarInfo(CarID,strClass,0,1);
			Common_Class.Item item1	 = getClassTypeID( strClass, 1);
			int duRef = Common_Car_Status.Get(nCarinfo.CarID).Durability +item1.Durability ;
			nCarinfo.carClass.SetClass1(item1.UpLimit, item1.StarLV, nCarinfo.durability, item1.Repair, item1.Brake,  item1.Class_power);
			nCarinfo.carClass.SetClass2(item1.Class_weight,item1.Class_grip,  item1.Class_gear, 
			                            item1.Class_bspower, item1.Class_bstime, item1.Repair,  duRef );
			nCarinfo.carClass.SetClass3(item1.GearLmt);
			nCarinfo.CarClassItem = item1;
			nCarinfo.SetFlag = 1;
			nCarinfo.bNewBuyCar = false;
			//Utility.LogWarning("modei");
			//	int mCnt = mineCarList[mineCarList.Count-1].Index;
			//	carinfo.setCarIndex(mCnt+1);
		}
	}
	
	public static string ChangeCarClassIDString(int nid){
		string strClass = null;
		switch(nid){
		case 3101: strClass = "D"; break;
		case 3102: strClass = "C"; break;
		case 3103: strClass = "B"; break;
		case 3104: strClass = "A"; break;
		case 3105: strClass = "S"; break;
		case 3106: strClass = "SS"; break;
		}
		return strClass;
	}
	
	public static void AddMyCarListLucky(int CarID, string strClass, int CarLV, int CrewLV, int nClassID, int carIdx){
		int model = (strClass.Equals("S"))?3:1;
		//CarInfo carinfo = new CarInfo(CarID,strClass,0,model);
		CarInfo carinfo = new CarInfo(CarID);
		carinfo.CarIndex = carIdx;
		carinfo.nClassID = nClassID;
		carinfo.CarID = CarID;
		carinfo.ClassID = strClass;
		Common_Class.Item item1	 = getClassTypeID(strClass, model);
		int abc =0; // star
		//int bcd = 0;
		if(CarLV >= item1.StarLV*5){
			CarLV = item1.StarLV*5;
			abc = item1.StarLV-1;
		}else{
			abc = CarLV/5;
			//abc = abc;
		}
		carinfo.bodyStar = abc;
		carinfo.engineStar = abc;
		carinfo.tireStar = abc;
		carinfo.intakeStar = abc;
		carinfo.gearBoxStar = abc;
		carinfo.bsPowerStar = abc;
		carinfo.bsTimeStar = abc;
		
		carinfo.bodyLv = CarLV;
		carinfo.engineLv = CarLV;
		carinfo.tireLv = CarLV;
		carinfo.intakeLv = CarLV;
		carinfo.gearBoxLv = CarLV;
		carinfo.bsPowerLv = CarLV;
		carinfo.bsTimeLv = CarLV;
		
		carinfo.CarClassItem = item1;
		int du = Common_Car_Status.Get(CarID).Durability +item1.Durability ;
		carinfo.carClass.SetClass1(item1.UpLimit, item1.StarLV, du, item1.Repair, item1.Brake,  item1.Class_power);
		carinfo.carClass.SetClass2(item1.Class_weight,item1.Class_grip,  item1.Class_gear, 
		                           item1.Class_bspower, item1.Class_bstime, item1.Repair,du );
		carinfo.carClass.SetClass3(item1.GearLmt);
		int mCnt = mineCarList[mineCarList.Count-1].Index;
		carinfo.setCarIndex(mCnt+1);
		carinfo.bNewBuyCar = true;
		mineCarList.Add(carinfo);
		//	if(model == 3) mineSCarList.Add(carinfo);
	
		if(CrewLV != 0){
			CrewInfo crew = getTeamCrewInfo(SelectedTeamID);
			crew.chiefLv = CrewLV;
			crew.jackLv = CrewLV;
			crew.tireLv = CrewLV;
			crew.driverLv = CrewLV;
			crew.gasLv = CrewLV;
			GameObject.Find("LobbyUI").SendMessage("ResetLevelButton",false);
		}

		GV.mineCarList.Sort(delegate(CarInfo x, CarInfo y) {
			int a = x.CarID.CompareTo(y.CarID);
			if(a == 0)
				a = x.nClassID.CompareTo(y.nClassID);
			return a;
		});
	}
	public static void AddTeamSponsor(int sponID, int TeamID){
		myTeamInfo myteam;// = FindStockTeam(int.Parse(teamname));
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == TeamID);
		myteam.SponID = sponID;
	}
	public static void DeleteTeamSponsor(int sponID, int TeamID){
		myTeamInfo myteam;// = FindStockTeam(int.Parse(teamname));
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == TeamID);
		myteam.SponID = 1300;
	}
	
	private static void FindCarClass(string carClass){
		List<CarInfo> carInfo = mineCarList.FindAll(delegate(CarInfo obj) {
			return obj.ClassID == carClass;
		});
		
	}
	
	private static bool FindWithClass(CarInfo carinfo)
	{
		bool b= false;
		
		return b;
	}
	
	
	
	
	public static void DeleteMineCar(int carid, string carclass, int teamid){
		//List<CarInfo> carInfo = mineCarList.Find(delegate(CarInfo obj) {
		//	bool b = false;
		//	return b;
		//});
		
		CarInfo carInfo = mineCarList.Find(delegate(CarInfo obj) {
			bool a = false;
			if(obj.CarID == carid){
				if(obj.ClassID.Equals(carclass)){
					if(obj.TeamID == teamid){
						a = true;
					}
				}
			}
			return a;
		});
		//	if(carInfo.ModelID == 3) mineSCarList.Remove(carInfo);
		mineCarList.Remove(carInfo);
		
	}
	
	public static CarInfo GetMyCarInfo(int carid, string classid, int teamid){
		CarInfo carInfo = null;
		if(teamid == 0){
			carInfo = mineCarList.Find(delegate(CarInfo obj) {
				bool a = false;
				if(obj.CarID == carid){
					if(obj.ClassID.Equals(classid)){
						a = true;
					}
				}
				return a;
			});
		}else{
			carInfo = mineCarList.Find(delegate(CarInfo obj) {
				bool a = false;
				if(obj.CarID == carid){
					if(obj.ClassID.Equals(classid)){
						if(obj.TeamID == teamid){
							a = true;
						}
					}
				}
				return a;
			});
		}
		if(carInfo == null) Utility.LogWarning("CarInfo is Null");
		
		return carInfo;
		
	}
	
	public static int getTeamCarIndex(int TeamID){
		myTeamInfo myteam;
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == TeamID);
		if(myteam == null ) Utility.LogError("getTeamcarID error");
		return myteam.myCar.CarIndex;
	
	}
	
	public static int getTeamCarID(int TeamID){
		myTeamInfo myteam;
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == TeamID);
		if(myteam == null ) Utility.LogError("getTeamcarID error");
		return myteam.TeamCarID;
	}
	
	public static int getTeamCrewID(int TeamID){
		myTeamInfo myteam;
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == TeamID);
		if(myteam == null ) Utility.LogError("getTeamCrewID error");
		return myteam.TeamCrewID;
	}
	
	public static string getTeamCarClass(int TeamID){
		myTeamInfo myteam;
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == TeamID);
		return myteam.myCar.ClassID;
		
	}

	
	public static string ConvertClassInt(int carClass){
		string str = string.Empty;
		switch(carClass){
			case 3101: str = "D";break;
			case 3102: str = "C";break;
			case 3103: str = "B";break;
			case 3104: str = "A";break;
			case 3105: str = "S";break;
			case 3106: str = "SS";break;
			default : str = "D";break;
		}
		
		return str;
		
		
	}
	
	public static int ConvertClassString(string carClass){
		int typeid = 0;
		switch(carClass){
			case "D" : typeid =3101;break;
			case "C" : typeid =3102;break;
			case "B" : typeid =3103;break;
			case "A" : typeid =3104;break;
			case "S" : typeid =3105;break;
			case "SS" : typeid =3106;break;
			default: typeid = 3101;break;
		}
		return typeid;
	}

	public static int getTeamCarClassId(int TeamID){
		myTeamInfo myteam;
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == TeamID);
		return myteam.myCar.nClassID;
		
	}

	public static Common_Class.Item getTeamCarClassItem(int TeamID){
		myTeamInfo myteam;
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == TeamID);
		return myteam.myCar.CarClassItem;
	}
	public static void setTeamMainCarID(int teamID, int carID, CarInfo carInfo){
		myTeamInfo  myteam = listmyteaminfo.Find(obj=> obj.TeamCode == teamID);
		myteam.TeamCarID = carID;
		myteam.setTeamCarInfo(carInfo);
	}
	
	public static void setTeamMyCar(int teamID, int idx){
		mineCarList[idx].TeamID = teamID;
	}
	public static void unSetTeamMyCar(int idx){
		mineCarList[idx].TeamID = 0;
	}
	
	public static int getTeamSponID(int TeamID){
		myTeamInfo myteam;
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == TeamID);
	
		if(myteam == null) return 1300;
		return myteam.SponID;
	}	
	
	public static void setTeamSponID(int TeamID, int sponID){
		myTeamInfo myteam;
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == TeamID);
		if(myteam == null) Utility.LogError("Team is Null");
		myteam.SponID = sponID;
	}
	
	public static CarInfo getTeamCarInfo(int teamID){
		myTeamInfo myteam;
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == teamID);
		return myteam.myCar;
	}
	
	public static CrewInfo getTeamCrewInfo(int teamID){
		myTeamInfo myteam;
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == teamID);
		return myteam.myCrews;
	}
	
	public static myTeamInfo getTeamTeamInfo(int teamid){
		return listmyteaminfo.Find(obj=> obj.TeamCode == teamid);
		
	}
	
	
	public static int getMatCount(int matID){
		MatInfo mat = listMyMat.Find(obj => obj.MatID == matID);
		return mat.MatQuantity;// listMyMat.Find(obj => obj.MatID == matID).MatQuantity;
	}
	
	public static void UpdateMatCount(int matID, int matCount){
		MatInfo mat = listMyMat.Find(obj => obj.MatID == matID);
		if(matCount < 0){
			if(mat.MatQuantity == 0) return;
		}else{
		//	if(matID == 8620) myAcc.instance.account.bInvenBTN[2] = true;
		//	else myAcc.instance.account.bInvenBTN[1] = true;
		}
		int idx = listMyMat.IndexOf(mat);
		listMyMat[idx].MatQuantity =   mat.MatQuantity+matCount;
		
		
	}

	public static void UpdateCouponList(int idx, int count){
		myCouponList[idx] = myCouponList[idx] + count;
		//if(count > 0)	myAcc.instance.account.bInvenBTN[3] = true;
	}

	
	public static Common_Class.Item getClassTypeID(string mS, int modeID){
		int typeid = 3000;
		switch(mS){
		case "D" : typeid =typeid+ 1;break;
		case "C" : typeid =typeid+ 2;break;
		case "B" : typeid =typeid+ 3;break;
		case "A" : typeid =typeid+ 4;break;
		case "S" : typeid =typeid+ 5;break;
		case "SS" : typeid =typeid+ 6;break;
		}
		switch(modeID){
		case 1: typeid =typeid+ 100; break;
		case 2: typeid =typeid+ 100; break;
		case 3: typeid =typeid+ 100; break;
		default : typeid = typeid+ 100; break;
		}
		return Common_Class.Get (typeid);
	}


	public static Common_Class.Item  getClassIDInTeamCar(int carid, int teamid){
		int typeid = 3000;
		myTeamInfo myteam;
		int idx = teamid;
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == idx);
		//CarInfo carinfo = myteam.myCars.Find(obj1 => obj1.CarID == carid);
		CarInfo carinfo = myteam.myCar;
		switch(carinfo.ClassID){
		case "D" : typeid =typeid+ 1;break;
		case "C" : typeid =typeid+ 2;break;
		case "B" : typeid =typeid+ 3;break;
		case "A" : typeid =typeid+ 4;break;
		case "S" : typeid =typeid+ 5;break;
		case "SS" : typeid =typeid+ 6;break;
		}
		
		switch(carinfo.ModelID){
		case 1: typeid =typeid+ 100; break;
		case 2: typeid =typeid+ 100; break;
		case 3: typeid =typeid+ 100; break;
		default : typeid = typeid+ 100; break;
		}
		return Common_Class.Get (typeid);
	}
	public static void AddBuyTeamInfo(int teamId){
		Common_Team.Item item = Common_Team.Get(teamId);
		myTeamInfo	team = new myTeamInfo(teamId);
		team.chiefLv =1;
		team.driverLv = 1;
		team.tireLv =1;
		team.jackLv =1;
		team.gasLv =1;
		team.sponDateTime = new System.DateTime(1900,1,1);
		team.SponRemainTime = team.sponDateTime.Ticks;
		team.TeamCarIndex =0;
		team.SponID = 1300;
		
		
		team.TeamCrewID = item.Crew;
		//listmyteaminfo[i].myCrews = new CrewInfo(item.Crew);
		team.setTeamCrew(item.Crew);
		team.bFlag  = true;
		listmyteaminfo.Add(team);
		
	}

	public static void TeamInitSetting(){
		if(listmyteaminfo[0].bFlag == true) return;
		for(int  i = 0; i < listmyteaminfo.Count; i++){
			Common_Team.Item item = Common_Team.Get(listmyteaminfo[i].TeamCode);
			listmyteaminfo[i].TeamCrewID = item.Crew;
			listmyteaminfo[i].setTeamCrew(item.Crew);
			listmyteaminfo[i].bFlag  = true;
			int teamCarIdx = listmyteaminfo[i].TeamCarIndex;
			if(teamCarIdx != 0){
				CarInfo carinfo =  mineCarList.Find(obj=> obj.CarIndex == listmyteaminfo[i].TeamCarIndex);
				if(carinfo != null){
					carinfo.TeamID =  listmyteaminfo[i].TeamCode;
					listmyteaminfo[i].TeamCarID = carinfo.CarID;
					listmyteaminfo[i].setTeamCarInfo(carinfo);
					listmyteaminfo[i].myCar.CarClassItem = Common_Class.Get(carinfo.nClassID);
				}else{
					listmyteaminfo[i].TeamCarIndex = 0;
				}
			}
		}

		int idx = getTeamTeamInfo(GV.SelectedTeamID).TeamCarIndex;
		if(idx != 0) return;

		for(int i = 0; i < mineCarList.Count;i++){
			if(mineCarList[i].TeamID != 0){
			}else{
				myTeamInfo myTeam = getTeamTeamInfo(GV.SelectedTeamID);
				Common_Team.Item item1 = Common_Team.Get(GV.SelectedTeamID);
				myTeam.TeamCrewID = item1.Crew;
				myTeam.setTeamCrew(item1.Crew);
				myTeam.bFlag  = true;
				CarInfo carinfo = mineCarList[i];
				myTeam.TeamCarIndex = carinfo.CarIndex;
				carinfo.TeamID = myTeam.TeamCode;
				myTeam.TeamCarID = carinfo.CarID;
				myTeam.setTeamCarInfo(carinfo);
				myTeam.myCar.CarClassItem = Common_Class.Get(carinfo.nClassID);

				break;
			}
		
		}

	}
	
	public static bool CheckDefaultCar(int carid, string carclass, int teamid){
		bool  b = false;
		CarInfo carInfo = mineCarList.Find(delegate(CarInfo obj) {
			bool a = false;
			if(obj.CarID == carid){
				if(obj.ClassID.Equals(carclass)){
					if(obj.TeamID == teamid){
						a = true;
					}
				}
			}
			return a;
		});
		if(carInfo != null) b = true;
		
		return b;
	}
	
	public static int MyCarAbilityStats(int carid, string classid, int teamid){
		CTeamAbility _ability = new CTeamAbility();
		return _ability.MyCarAbility(carid, classid, teamid);
	}
	
	public static int DefaultCarAbility(int carid, Common_Class.Item itemclass){
		CTeamAbility _ability = new CTeamAbility();
		return _ability.DefaultCarAbility(carid, itemclass);
		
	}
	
	public static int[] eTrack;
	public static int[] eCarID;
	public static int[] ePlayCount;
	public static int gSetEventMode =1;
	public static int gEventTime = 0;
	public static void setEventRace(){
		Utility.LogWarning("이벤트 차량 지정하는 루틴 로비씬 로딩될때마다 됨.");
		eTrack = new int[3];
		eCarID = new int[3];
		ePlayCount = new int[3];
		for(int i = 0; i < eTrack.Length; i++){
			eTrack[i] = Random.Range(1401,1406);
		}
		for(int i = 0; i < eTrack.Length; i++){
			eCarID[i] = Random.Range(1000,1023);
		}
		for(int i = 0; i < eTrack.Length; i++){
			ePlayCount[i] = 0;//.Range(1000,1023);
		}
		gSetEventMode = Random.Range(0,3);
	}
	
	public static void UtilLogWaring(string str){
		Utility.LogWarning(str);
	}
}

/*
	public static void ChangeMainCar(int TeamID, int carID,string strClass1){ // 차량 바꿀때
		myTeamInfo team = getTeamTeamInfo(TeamID);
		if(team != null) listmyteaminfo.Remove(team);
		mineCarList.Clear();
		mineSCarList.Clear();

		Common_Team.Item item = Common_Team.Get(TeamID);
		//Common_
		team = new myTeamInfo(TeamID, carID,item.Crew,item.Model); //1000
		int mo = Common_Car_Status.Get(carID).Model;
		CarInfo carinfo = new CarInfo(carID,strClass1,TeamID,mo);
	
		int typeid = 3000;
		string strClass = strClass1;
		switch(strClass){
		case "D" : typeid =typeid+ 1;break;
		case "C" : typeid =typeid+ 2;break;
		case "B" : typeid =typeid+ 3;break;
		case "A" : typeid =typeid+ 4;break;
		case "S" : typeid =typeid+ 5;break;
		case "SS" :typeid =typeid+ 6;break;
		}
		switch(mo){
		case 1: typeid =typeid+ 100; break;
		case 2: typeid =typeid+ 100; break;
		case 3: typeid =typeid+ 300; break;
		}
		
		Common_Class.Item item1 =Common_Class.Get(typeid);
		int dura = 0, pow = 0;
		string[] str = item1.Class_power.Split(';');
		int.TryParse(str[0], out pow);
		str = item1.Durability.Split(';');
		int.TryParse(str[0], out dura);
		carinfo.carClass.SetClass1(item1.UpLimit, item1.StarLV, dura, item1.Repair, item1.Brake, pow);
		
		str = item1.Class_weight.Split(';');
		int a = int.Parse(str[0]);
		str = item1.Class_grip_1t.Split(';');
		int b = int.Parse(str[0]);
		str = item1.Class_gear.Split(';');
		int c = int.Parse(str[0]);
		str = item1.Class_bspower.Split(';');
		int d = int.Parse(str[0]);
		str = item1.Class_bstime_1t.Split(';');
		int e = int.Parse(str[0]);
		carinfo.carClass.SetClass2(a, (float)b*0.001f, c, d, (float)e*0.001f, item1.Repair, dura);
		team.setTeamCarInfo(carinfo);
		listmyteaminfo.Add(team);
		mineCarList.Add(carinfo);
		if(mo == 3) mineSCarList.Add(carinfo);
	}*/



//	myCouponList.Add(0);
//	myCouponList.Add(0);
//	return;
//	SelectedTeam = "Stock";
//	listmystockinfo = new List<myStockInfo>();
//	myStockInfo mystockinfo = new myStockInfo(10, new int[]{1000, 1002},new int[] {1,1});
//	listmystockinfo.Add(mystockinfo);
//	mystockinfo = new myStockInfo(11, new int[]{1001, 1003},new int[] {1,3});
//	listmystockinfo.Add(mystockinfo);

//	mySpecialCarList = new List<string>();
//	myStockCarList = new List<string>();
//	myTourCarList = new List<string>();


//	List<myStockInfo> a = listmystockinfo;
//	Utility.Log(a.Count);
//	Utility.Log(listmystockinfo.Count);
//	if(TeamSeason != 1){
//		listmytourinfo = new List<myTourInfo>();
//		myTourInfo mytourinfo =  new myTourInfo(14, new int[]{1012, 1013},new int[] {1,3});
//		listmytourinfo.Add(mytourinfo);
//		mytourinfo =  new myTourInfo(13, new int[]{1014, 1015},new int[] {1,1});
//		listmytourinfo.Add(mytourinfo);

//		listmytourinfo.ForEach(obj => {
//			obj.myCars.ForEach(obj1 =>{
//				string str = obj1.CarID.ToString()+"_"+obj1.ClassID.ToString()+"_"+obj.TeamCode.ToString();
//				if(obj1.ClassID >=3){
//					mySpecialCarList.Add(str);
//				}else{
//					myTourCarList.Add(str);
//				}
//			});
//		});


//	myTourInfo mytour = FindTourTeam(14);
//	Utility.LogWarning(mytour.myCars.Count);
//	if(mytour == null || mytour.myCars.Count == 0) return;
//	int id = 1013;
//	CarInfo _carinfo = mytour.myCars.Find((info)=>info.CarID == id);
//	mytour.myCars.Remove(_carinfo);
//	Utility.LogWarning(mytour.myCars.Count);
//	Utility.LogWarning(myTourCarList.Count);

//	string str5 = myTourCarList[0];
//	string str3 = myTourCarList.Find(str4=>  str4 == str5 );
//	myTourCarList.Remove(str3);
//	Utility.LogWarning(myTourCarList.Count);
//	}
//	listmystockinfo.ForEach(obj => {
//		obj.myCars.ForEach(obj1 =>{
//			string str = obj1.CarID.ToString()+"_"+obj1.ClassID.ToString()+"_"+obj.TeamCode.ToString();
//			if(obj1.ClassID >=3){
//				mySpecialCarList.Add(str);
//			}else{
//				myStockCarList.Add(str);
//				Utility.LogWarning("str : "+str);
//			}
//		});
//	});

//	listmystockinfo.Find(
//		(Info) => Info.CrewID == 1200);  
//FIND
//	myStockCarList.Sort(
//		delegate(int x, int y) {
//		return x.CompareTo(y);
//	});
//	myTourCarList.Sort(
//		delegate(int x, int y) {
//		return x.CompareTo(y);
//	});
//	mySpecialCarList.Sort(
//		delegate(int x, int y) {
//		return x.CompareTo(y);
//	});
//Sort
/*ex 2) Dictionary 정렬
class cTmp
{
 string m_tmp1;
 string m_tmp2;
}

Dictionary<string, cTmp> m_tmpList = new Dictionary<string, cTmp>();

class cTmpComparer : IComparer<KeyValuePair<string, cTmp>>
{
 public int Compare(KeyValuePair<string, cTmp> x, KeyValuePair<string, cTmp> y)
 {
  return x.Value.m_tmp1.CompareTo(y.Value.m_tmp1);
 }
}

// Dictionary -> List 로 변환
List<KeyValuePair<string, cTmp>> list = m_tmpList.ToList();
// 정렬
list.Sort(new cTmpComparer());
// List ->Dictionary 로 변환
m_tmpList = list.ToDictionary(p => p.Key, p => p.Value); //(참고 1,2)참고
*/
/*
		Dictionary<string,int> dictionary = new Dictionary<string, int>();
		
		dictionary.Add("car", 11);
		
		dictionary.Add("apple", 67);
		
		dictionary.Add("zebra", 33);
		
		dictionary.Add("mouse", 55);
		
		dictionary.Add("year", 99);

		foreach (KeyValuePair<string, int> d in dictionary)
		{
			//Utility.LogWarning(string.Format("{0}: {1}", d.Key, d.Value));
		}

		// Key로 정렬해보겠습니다. (사람 이름으로 정렬합니다.)
			// 이름 순으로 정렬
		var varList = dictionary.Keys.ToList();
		varList.Sort();
		foreach (var d in varList)
		{
			//Utility.LogWarning(string.Format("{0}: {1}", d, dictionary[d]));
		}
		//Value로 정렬해보겠습니다. (나이로 정렬합니다)
			// 나이순으로 정렬
		var items = from pair in dictionary orderby pair.Value ascending select pair;
		foreach (var d in items)
		{
			//Utility.LogWarning(string.Format("{0}: {1}", d.Key, d.Value));
		} */


/*
	public static myStockInfo FindStockTeam(int id){
		return listmystockinfo.Find((Info) => Info.TeamCode == id);  
	}

	public static myTourInfo FindTourTeam(int id){
		return listmytourinfo.Find((Info) => Info.TeamCode == id);  
	}
	public static int FindStockCar(){

		return  FindStockTeam(StockTeamCode).TeamCarID;
	}
	public static int FindStockCrew(){
		return FindStockTeam(StockTeamCode).TeamCrewID;
	}
	public static int FindTourCar(){
		//if(TeamSeason == 1) return 0;
		return FindTourTeam(TourTeamCode).TeamCarID;
	}
	public static int FindTourCrew(){
		return FindTourTeam(TourTeamCode).TeamCrewID;
	}

	public static int FindStockCar(int idx){
		
		return  FindStockTeam(idx).TeamCarID;
	}
	public static int FindStockCrew(int idx){
		return FindStockTeam(idx).TeamCrewID;
	}
	public static int FindTourCar(int idx){
		//if(TeamSeason == 1) return 0;
		return FindTourTeam(idx).TeamCarID;
	}
	public static int FindTourCrew(int idx){
		return FindTourTeam(idx).TeamCrewID;
	}
	public static int FindStockSponsor(){
		return FindTourTeam(TourTeamCode).SponID;
	}
	public static int FindTourSponsor(){
		return FindTourTeam(TourTeamCode).SponID;
	}

	public static int GetTeamSpon(int idx){
		int id = 0;
		if(idx == 0){
			id = FindStockSponsor();
		}else{
			id = FindTourSponsor();
		}
		return id;
	}

	public static void DeleteStockCar(string carname, string teamname){
		myStockInfo mytour = FindStockTeam(int.Parse(teamname));
		if(mytour == null || mytour.myCars.Count == 0) return;
		int id = int.Parse(carname);
		CarInfo _carinfo = mytour.myCars.Find((info)=>info.CarID == id);
		mytour.myCars.Remove(_carinfo);
	}


	


	public static CarInfo GetStockCarInfo(){
		myStockInfo mytour = FindStockTeam(StockTeamCode);
		if(mytour == null || mytour.myCars.Count == 0) return null;
		int id = mytour.TeamCarID;
		CarInfo _carinfo = mytour.myCars.Find((info)=>info.CarID == id);
		return _carinfo;
	}
	public static CarInfo GetTourCarInfo(){
		myTourInfo mytour = FindTourTeam(TourTeamCode);
		if(mytour == null || mytour.myCars.Count == 0) return null;
		int id = mytour.TeamCarID;
		CarInfo _carinfo = mytour.myCars.Find((info)=>info.CarID == id);
		return _carinfo;
	}
	public static CrewInfo GetStockCrewInfo(){
		myStockInfo mytour = FindStockTeam(StockTeamCode);
		if(mytour == null || mytour.myCars.Count == 0) return null;
		return mytour.myCrews;
	}
	public static CrewInfo GetTourCrewInfo(){
		myTourInfo mytour = FindTourTeam(TourTeamCode);
		if(mytour == null || mytour.myCars.Count == 0) return null;
		return mytour.myCrews;
	}




	public static  int FindSelectedCar(){
		int a =0;
		if(SelectedTeam.Equals("Stock")){
			a = FindStockCar();
		}else{
			a = FindTourCar();
		}
		return a;
	}
	public static  int FindSelectedCrew(){
		int a =0;
		if(SelectedTeam.Equals("Stock")){
			a = FindStockCrew();
		}else{
			a = FindTourCrew();
		}
		return a;
	}
	public static  int FindSelectedSponsor(){
		int a =0;
		if(SelectedTeam.Equals("Stock")){
			a = FindStockSponsor();
		}else{
			a = FindTourSponsor();
		}
		return a;
	}
*/
/*
	public static void AddTeamCar(int CarID, string strClass, int teamID){

	}

	public static void DeleteTeamCar(int carid, string strClass, int teamID){
		myTeamInfo myteam;// = FindStockTeam(int.Parse(teamname));
		myteam = listmyteaminfo.Find(obj=> obj.TeamCode == teamID);
		if(myteam == null) return;// || myteam.myCars.Count == 0) return;
		//CarInfo _carinfo = myteam.myCar;//s.Find((info)=>info.CarID == carID);
		myteam.myCar = null; //.Clear();
		myteam.TeamCarID = 0;
	}
	*/
