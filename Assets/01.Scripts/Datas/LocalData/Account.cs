using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using SimpleJSON;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;


[System.Serializable]
public class Account {
	// ----------------------------------------------------------------------------------------------------------- //

	// ----------------------------------------------------------------------------------------------------------- //
	// 자동차 정보 형식.
/*	[System.Serializable]
	public class CarInfo
	{
		public int carId;
		//public int carAbility;
		public int bodyLv;
		public int engineLv;
		public int tireLv;
		public int gearBoxLv;
		public int intakeLv;
		public int bsPowerLv;
		public int bsTimeLv;
		public int bodyStar;
		public int engineStar;
		public int tireStar;
		public int gearBoxStar;
		public int intakeStar;
		public int bsPowerStar;
		public int bsTimeStar;
		public int carSeason;
		public int upLimit;
		public int bodydurability;
		public int enginedurability;
		public int tiredurability;
		public int gearBoxdurability;
		public int intakedurability;
		public int bsPowerdurability;
		public int bsTimedurability;
		public void init(){
			carId = 1000;
			bodyLv = 1;
			engineLv = 1;
			tireLv = 1;
			gearBoxLv = 1;
			intakeLv = 1;
			bsPowerLv = 1;
			bodyStar = 0;
			bsTimeLv = 1;
			engineStar  = 0;
			tireStar = 0;
			gearBoxStar = 0;
			intakeStar = 0;
			bsPowerStar =0;
			bsTimeStar = 0;
			carSeason = 1;
			upLimit = 2;
			bodydurability = 100;
			enginedurability = 100;
			gearBoxdurability = 100;
			intakedurability =100;
			tiredurability = 100;
			bsPowerdurability = 100;
			bsTimedurability = 100;
			
		}

		public void addCar(int s, int lv, int up){
			bodyLv = 1;
			engineLv = 1;
			tireLv = 1;
			gearBoxLv = 1;
			intakeLv = 1;
			bsPowerLv = 1;
			bsTimeLv = 1;

			bodyStar = s;
			engineStar  = s;
			tireStar = s;
			gearBoxStar = s;
			intakeStar = s;
			bsPowerStar =s;
			bsTimeStar = s;
			carSeason = lv;
			upLimit = up;
			
			bodydurability = 100;
			enginedurability = 100;
			gearBoxdurability = 100;
			intakedurability =100;
			tiredurability = 100;
			bsPowerdurability = 100;
			bsTimedurability = 100;
			
			
		}
	}
	// ----------------------------------------------------------------------------------------------------------- //
	// 정비팀 정보 형식.
	[System.Serializable]
	public class CrewInfo
	{
		public int crewId;
	//	public int crewAbility;
		public int chiefLv;
		public int jackLv;
		public int driverLv;
		public int tireLv;
		public int gasLv;
		public int crewSeason;
		public int upLimit;
		public int chiefStar;
		public int jackStar;
		public int driverStar;
		public int tireStar;
		public int gasStar;
		public void init(){
			crewId = 1200;
			chiefLv = 1;
			jackLv = 1;
			driverLv = 1;
			tireLv = 1;
			gasLv = 1;
			crewSeason = 1;
			upLimit = 2;
			chiefStar = 0;
			jackStar = 0;
			gasStar = 0;
			driverStar = 0;
			tireStar = 0;
			//crewAbility = 0;
		}
		public void addCrew(int s, int l, int u){
			chiefLv = 1;
			jackLv = 1;
			driverLv = 1;
			tireLv = 1;
			gasLv = 1;
			crewSeason = l;
			upLimit = u;
			chiefStar = 0;
			jackStar = 0;
			gasStar = 0;
			driverStar = 0;
			tireStar = 0;
		}
	}
	
	// ----------------------------------------------------------------------------------------------------------- //
	
	// 스폰서 정보 형식.
	[System.Serializable]
	public class SponsorInfo
	{
		public int sponsorId;
		public long expireTime;

		public void init(){
			sponsorId = 1300;
		}
	}

	// ----------------------------------------------------------------------------------------------------------- //
	
	// 경기 정보 형식.
	
	[System.Serializable]
	public class CardInfo{
		public int cardId;		// CommonCard의 ID.
		public int quantity;	// 수량.
		public bool isNew;
	}
	
	// ----------------------------------------------------------------------------------------------------------- //
	*/
	[System.Serializable]
	public class myDeviceInfo{
		public string PhoneNumber;
		public string CountryCode;
		public string DeviceModel;
		public string PushID;
		public string DeviceID;
		public string OsVersion;
		public void Init(){
			PhoneNumber = null;
			CountryCode = null;
			DeviceModel =null;
			PushID = null;
			DeviceID  = null;
			OsVersion =null;
			if (Application.platform == RuntimePlatform.Android)
			{}//Vibration.OnRequestDevice();
			else{
				PhoneNumber = "000-000-0000";
				CountryCode = "KO";
				DeviceModel = "Editor";
				//PushID ="APA91bG09PjXiAEUMl6bRTx9MZZs0gAMxglvl_RGg-gGGwh3V0KVYDoZv_t-nT8tN-F-WYGHn48jOSxy2UOa8tbp49G8j5cHPMRhBFJDfB1kkYur_rn8kFDYDJbvXYqSFCdc-pT58S4sM3Ooh1oIkBXU8PeSm-3Kyw";
				PushID = "push_id";
				DeviceID  = "DeviceID";
				OsVersion = "EditorVersion";
			}
		}
	}

	
	public string snsType;
	public string snsUserId;
	public string nickName;
	public long creationTime;
	public long lastConnectTime;
	public long dayCheckTime;
	public long weeklyCheckTime;

	public bool isLevelUp;
	public bool isTutorial;
	public myDeviceInfo mydeviceinfo;
	// ----------------------------------------------------------------------------------------------------------- //


	public void Initialize(){
		isLevelUp = false;
		isTutorial = true;
	//	System.DateTime day =  System.DateTime.Now;
		System.DateTime day =  NetworkManager.instance.GetCurrentDeviceTime();
		creationTime  =day.Ticks;
		dayCheckTime = new System.DateTime(day.Year, day.Month, day.Day,0,0,0).Ticks;
		System.DateTime _tTime;
		if(day.DayOfWeek == DayOfWeek.Monday){
			weeklyCheckTime = new System.DateTime(day.Year, day.Month, day.Day,0,0,0).Ticks;
		}else{
			int f = (int)day.DayOfWeek -(int)DayOfWeek.Sunday;
			if(f == 0) {
				_tTime = day.AddDays((double)(-6));
			}else {
				_tTime = day.AddDays((double)(-f));
				_tTime = _tTime.AddDays(1);
			}
			weeklyCheckTime = new System.DateTime(_tTime.Year, _tTime.Month, _tTime.Day,0,0,0).Ticks;
		}
		mydeviceinfo = new myDeviceInfo();
		mydeviceinfo.Init();
		postList = new PostList();
		weeklyRace = new WeeklyRace();
		mRace = new RaceRecord();
		eRace = new EventRace();
		attendevent = new AttendEvent();
		attendevent.LastTime = NetworkManager.instance.GetCurrentDeviceTime().AddHours(2).Ticks;
		attendevent.CurrentTime = NetworkManager.instance.GetCurrentDeviceTime().Ticks;

	}

	public PostList postList;
	public WeeklyRace weeklyRace;
	public RaceRecord mRace;
	public EventRace eRace;
	public List<PostList> listPost = new List<PostList>();
	public AttendEvent attendevent;

	[System.Serializable]
	public class AttendEvent{
		public long CurrentTime;
		public long LastTime; // 0 coin, 1 dollar, 2 matID, 3 sCoupon, 4 Gcoupon
		public bool bAccept;
		public int AttackTimeID;
		public int materialId;
		public int dealerCar;
		public AttendEvent(){
			CurrentTime = 0;
			bAccept = false;
			AttackTimeID= 0;materialId = 0;
			dealerCar = 0;
		}
		
	}

	[System.Serializable]
	public class PostList{
		public int index;
		public int nType; // 0 coin, 1 dollar, 2 matID, 3 sCoupon, 4 Gcoupon
		public int nReCount;
		public string strNick;
		public long receiveTime;
		public PostList(){
			
		}
		
	}
	
	[System.Serializable]
	public class EventRace{
		public int testDriveCarID ;
		public int featuredCarID; // 0 coin, 1 dollar, 2 matID, 3 sCoupon, 4 Gcoupon
		public int EvoTrackID;
		public int featuredTrackID;
		
		public int testDrivePlayCount ;
		public int featuredPlayCount; // 0 coin, 1 dollar, 2 matID, 3 sCoupon, 4 Gcoupon
		public int EvoPlayCount;
		public int EvoAcquistCount;
		
		public int EventRaceMode;
		public int mRaceMode;
		public EventRace(){
			mRaceMode = 0;
		//	testDrivePlayCount = 0;
		//	featuredPlayCount = 0;
		//	EvoPlayCount = 0;
			EvoAcquistCount = 0;
			
			featuredTrackID =1401;// (int)Well512.Next(1401,1406);
			EvoTrackID =  (int)Well512.Next(1401,1406);
			testDriveCarID = (int)Well512.Next(1000,1024);
			featuredCarID = (int)Well512.Next(1000,1024);
			
		}
		
		public void EventDayReset(){
			mRaceMode++;
			if(mRaceMode >=3) mRaceMode= 0;
			
		//	testDrivePlayCount = 0;
		//	featuredPlayCount = 0;
		//	EvoPlayCount = 0;
			EvoAcquistCount = 0;
			int r = 0;
		//	for(int  i =0; i < 20; i++){
		//		r =(int) Well512.Next(1401,1406);
		//		if(r != featuredTrackID){
		//			featuredTrackID = r;
		//			break;
		//		}
		//	}
			for(int  i =0; i < 20; i++){
				r =(int) Well512.Next(1401,1406);
				if(r != EvoTrackID){
					EvoTrackID = r;
					break;
				}
			}
			for(int  i =0; i < 50; i++){
				r =(int) Well512.Next(1000,1024);
				if(r != featuredCarID){
					featuredCarID = r;
					break;
				}
			}


			for(int  i =0; i < 50; i++){
				r =(int) Well512.Next(1000,1024);
				if(r != testDriveCarID){
					testDriveCarID = r;
					break;
				}
			}
			
			
		}
		
	}
	
	[System.Serializable]
	public class WeeklyRace{
		public int GTrophy;
		public int STrophy; // 0 coin, 1 dollar, 2 matID, 3 sCoupon, 4 Gcoupon
		public int BTrophy;
		public float rTime;
		public int WeeklyPlayCount;
		public bool Trophy_Reset_Times;
		public WeeklyRace(){
			this.rTime = -1.0f;
			GTrophy = 0;
			STrophy = 0;
			BTrophy = 0;
			WeeklyPlayCount = 0;
			Trophy_Reset_Times = false;
		}
		
	}
	[System.Serializable]
	public class RaceRecord{
		public int regularDragWin;
		public int regularDragPlay;
		public float regularDragTime;
		
		public int regularStockWin;
		public int regularStockPlay;
		public float regularStockTime;
		
		public int featureRaceWin;
		public int featureRacePlay;
		public float featureRaceTime;
		
		public int testdriveWin;
		public int testdrivePlay;
		public float testdriveTime;
		
		public int evoWin;
		public int evoPlay;
		public float evoTime;
		
		public int pvpcityWin;
		public int pvpcityPlay;
		public float pvpcityTime;
		
		public int pvpdragWin;
		public int pvpdragPlay;
		public float pvpdragTime;
		
		public int ChampionWin;
		public int ChampionPlay;
		public float ChampionTime;


		public int ClubWin;
		public int ClubPlay;
		public float ClubTime;


		public RaceRecord(){
			regularDragWin = 0;
			regularDragPlay = 0;
			regularDragTime = 100f;
			
			regularStockWin = 0;
			regularStockPlay = 0;
			regularStockTime= 100f;
			
			featureRaceWin = 0;
			featureRacePlay = 0;
			featureRaceTime= 100f;
			
			testdriveWin = 0;
			testdrivePlay = 0;
			testdriveTime= 100f;
			
			evoWin = 0;
			evoPlay = 0;
			evoTime= 100f;
			
			pvpcityWin = 0;
			pvpcityPlay = 0;
			pvpcityTime= 100f;
			
			pvpdragWin=0;
			pvpdragPlay=0;
			pvpdragTime=100f;
			
			ChampionWin=0;
			ChampionPlay=0;
			ChampionTime=100f;

			
			ClubTime = 100f;
			ClubPlay = 0;
			ClubWin = 0;
		}
		
	}
}


public class myAccount{
	public static myAccount instance;// = new myAccount();
	public Account account = new Account();
	public myAccount(){
	}
	public void GetAccountInfo(){
		if(!EncryptedPlayerPrefs.HasKey("Account")){
			instance.account.Initialize();
			var b = new BinaryFormatter();
			var m = new MemoryStream();
			b.Serialize(m, instance.account);
			EncryptedPlayerPrefs.SetString("Account", 
			                      Convert.ToBase64String(m.GetBuffer()) );
		}

		var data = EncryptedPlayerPrefs.GetString("Account");
		if(!string.IsNullOrEmpty(data))
		{
			var b = new BinaryFormatter();
			var m = new MemoryStream(Convert.FromBase64String(data));
			instance.account = (Account)b.Deserialize(m);
		}
}

public void SaveAccountInfo()
{
	if(instance == null) return;
	var b = new BinaryFormatter();
	var m = new MemoryStream();
	b.Serialize(m, instance.account);
	EncryptedPlayerPrefs.SetString("Account", Convert.ToBase64String(m.GetBuffer()));
	//Utility.LogWarning(" b " +  new System.DateTime(myAccount.instance.account.attendevent.LastTime));
}

	/*
	public void updateCarPart(Account.CarInfo _car){
		int index = instance.account.listCarInfo.IndexOf(_car);
		instance.account.listCarInfo[index] = _car;//.quantity += quantity; 
	}

	public void updateCrewPart(Account.CrewInfo _crew){
		int index = instance.account.listCrewInfo.IndexOf(_crew);
		instance.account.listCrewInfo[index] = _crew;//.quantity += quantity; 
	}

	public void updateCarInfo(int carID){
	//	Account.CarInfo a = new Account.CarInfo();
	//	Common_Car_Status.Item _Item = Common_Car_Status.Get(carID);
		//a.addCar((_Item.StarLV), _Item.ReqLV, _Item.UpLimit);
	//	a.addCar(0, _Item.ReqLV, _Item.UpLimit);
	//	a.carId = carID;
	//	instance.account.listCarInfo.Add(a);// account.listCarInfo.Add(a);
	//	instance.account.buttonStatus.isTeamCarNew = true;
	//	instance.account.buttonStatus.TeamCarNews[(carID-1000)] = true;
	}

	public void updateCrewInfo(int crewID){
	//	Account.CrewInfo b = new Account.CrewInfo();
	//	Common_Crew_Status.Item _Item = Common_Crew_Status.Get(crewID);
	//	b.addCrew((_Item.StarLV), _Item.ReqLV, _Item.UpLimit);
	//	b.crewId = crewID;
	//	instance.account.listCrewInfo.Add(b);
	//	instance.account.buttonStatus.isTeamCrewNew = true; 
	//	instance.account.buttonStatus.TeamCrewNews[(crewID-1200)] = true;
		//Utility.Log(instance.account.listCrewInfo.Count);
	}


	public void updateSponsor(int sponID){
		instance.account.sponsorInfo.sponsorId = sponID;
		//Global.MySponsorID =Base64Manager.instance.GlobalEncoding(sponID);
		System.DateTime nTime = System.DateTime.Now;
		nTime = nTime.AddMinutes((double)720);
		myAccount.instance.account.sponsorInfo.expireTime = nTime.Ticks;
	}

	public void updateCardInfo(int id, int quantity){
		return;
		Account.CardInfo _card = instance.account.listCardInfo.Find((card) => card.cardId == id);
		int index = instance.account.listCardInfo.IndexOf(_card);
		int cnt = instance.account.listCardInfo[index].quantity;
		if(cnt < 0) return;
		if(quantity < 0){
			cnt += quantity;
			if(cnt < 0) return;
		}
		instance.account.listCardInfo[index].quantity += quantity; 
	}
	public void updateBuyedCardquantity(int id, int quantity){
		Account.CardInfo _card = instance.account.listCardInfo.Find((card) => card.cardId == id);
		int index = instance.account.listCardInfo.IndexOf(_card);
		instance.account.listCardInfo[index].quantity = quantity; 
	}

	public Account.CarInfo GetCarInfo(int _id){
		if(instance.account.listCarInfo == null ||
		   instance.account.listCarInfo.Count == 0) return null;
		List<Account.CarInfo> _listcar = instance.account.listCarInfo;
		return _listcar.Find(
			(_carInfo) => _carInfo.carId == _id);  
	}
	public Account.CrewInfo GetCrewInfo(int _id){
		if(instance.account.listCrewInfo == null ||
		   instance.account.listCrewInfo.Count == 0) return null;
		List<Account.CrewInfo> _listcar = instance.account.listCrewInfo;
		return _listcar.Find(
			(Info) => Info.crewId == _id);  
	}

	public void SetCarAbilitChange(int carID){
	
	}

	public void SetCrewAbility(int crewID){
	
	}

*/
}

	// ----------------------------------------------------------------------------------------------------------- //
	/*
	public CarInfo GetSelectedCarInfo()
	{
		if (listCarInfo == null || listCarInfo.Count == 0)
			return null;
		
		return listCarInfo.Find(
			(carInfo) => carInfo.id == selectedCarKey.id);
	}

	public void SwapListCarInfo(CarInfo car){
		CarInfo _car = GetSelectedCarInfo();
		int index = listCarInfo.IndexOf(_car);
		listCarInfo[index] = car;
	}

	public CrewInfo GetSelectedCrewInfo()
	{
		if (listCrewInfo == null || listCrewInfo.Count == 0)
			return null;
		
		return listCrewInfo.Find(
			(crewInfo) => crewInfo.id == selectedCrewKey.id);
	}
	public void SwapListCrewInfo(CrewInfo crew){
		CrewInfo _crew = GetSelectedCrewInfo();
		int index = listCrewInfo.IndexOf(_crew);
		listCrewInfo[index] = crew;
	}
*/




