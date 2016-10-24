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
public class AccInfo {
	// ----------------------------------------------------------------------------------------------------------- //
	// ----------------------------------------------------------------------------------------------------------- //
	// ----------------------------------------------------------------------------------------------------------- //

	[System.Serializable]
	public class nmyDeviceInfo{
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
	// ----------------------------------------------------------------------------------------------------------- //
	
	// 경기 정보 형식.
	
	[System.Serializable]
	public class nCardInfo{
		public int cardId;		// CommonCard의 ID.
		public int quantity;	// 수량.
		public bool isNew;
		
	}
	
	// ----------------------------------------------------------------------------------------------------------- //
	
	
	public string snsType;
	public string snsUserId;
	public string nickName;
	public nmyDeviceInfo mydeviceinfo;
	
	//	public  int TeamSeason ; 
	//	public  string SelectedTeam ;//= "Stock";
	//public  int SelectedTeamCode;//=0; // 0: stock team, 1: tour team
	public bool isTutorial;
	public  int ChSeasonID;// = 240101;
	public int myCoin;
	public int myDollar;
	public int maxfuel;
	public int myFuel;
	public int SelectedTeamID;
	public int SponID;
	public int CarFlag;
	public int gReview;
	public bool bPopEvent;
	public int LV;
	public int EXP;
	public int currAdId;
	public bool[] bRewards;
	public  bool[] bLobbyBTN;
	public bool[] bInvenBTN;
	public bool[] bRaceMenuBTN;
	public bool[] bRaceSubBTN;
	public List<CarInfo> mineCarList;
	public List<int> myCouponList;
	public long lastRewardViewTimes;
	public long lastViewFreeFuelTime;
	public int remainFuelSeconds;
	public long lastConnectTime;
	//	public List<CarInfo> mineSCarList;
	// ----------------------------------------------------------------------------------------------------------- //
	public void Initialize(){
		isTutorial = true;
		mydeviceinfo = new nmyDeviceInfo();
		mydeviceinfo.Init();
		//  TeamSeason = 1; 
		//  SelectedTeam = "Stock";//= "Stock";
		//  SelectedTeamCode=0; // 0: stock team, 1: tour team
		
		ChSeasonID= 6000;
		if (Application.isEditor) myCoin  = 1000000;
		else myCoin = 1000000;
		myDollar = 10000000;
		//	maxfuel = 5;
		//	myFuel = 5;
		//SelectedTeamID = 10;
		bLobbyBTN = new bool[6]{false, true, true, true, true, true};
		LV = 1;
		gReview = 0;
		bPopEvent = true;
		currAdId = 8750;
		lastRewardViewTimes = 0;
		lastViewFreeFuelTime = 0;
		bRewards = new bool[16];
		bInvenBTN = new bool[4];
		bRaceMenuBTN = new bool[5];
		bRaceMenuBTN[0] = true;
		bRaceMenuBTN[3] = true;
		bRaceSubBTN = new bool[5];
		bRaceSubBTN[0] = true;
		EXP = 0;
		remainFuelSeconds = 0;
		lastConnectTime = 0;
		Global.gAttend = 1;
	}
	
	public void InitializeCarInfo(){

	}
	
	
}


public class myAcc{
	public static myAcc instance;// = new myAccount();
	public AccInfo account = new AccInfo();
	public myAcc(){
	}
	public void GetAccountInfo(){
		if(!EncryptedPlayerPrefs.HasKey("Account_1")){
			instance.account.Initialize();
			instance.account.InitializeCarInfo();
			var b = new BinaryFormatter();
			var m = new MemoryStream();
			//Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER","yes");
			b.Serialize(m, instance.account);
			EncryptedPlayerPrefs.SetString("Account_1", 
			                               Convert.ToBase64String(m.GetBuffer()) );
		}
		
		var data = EncryptedPlayerPrefs.GetString("Account_1");
		if(!string.IsNullOrEmpty(data))
		{
			var b = new BinaryFormatter();
			var m = new MemoryStream(Convert.FromBase64String(data));
			instance.account = (AccInfo)b.Deserialize(m);
		}
		getGVValue();
	}
	
	public void SaveAccountInfo()
	{
		if(instance == null) return;
		setGVValue();
		var b = new BinaryFormatter();
		var m = new MemoryStream();
		b.Serialize(m, instance.account);
		EncryptedPlayerPrefs.SetString("Account_1", Convert.ToBase64String(m.GetBuffer()));
		//Utility.LogWarning("Save AccountInfo");
	}
	
	public void getGVValue(){
		
	//	GV.ChSeasonID = instance.account.ChSeasonID;
	//	GV.myCoin = instance.account.myCoin;
	//	GV.myDollar = instance.account.myDollar;
	//	GV.SelectedTeamID = instance.account.SelectedTeamID;
		GV.SponID =  instance.account.SponID;
		//GV.CarFlag = instance.account.CarFlag;
		Global.gReview = instance.account.gReview;
		GV.CurrADId = instance.account.currAdId;
	//	Global.level = instance.account.LV;
	//	Global.Exp = instance.account.EXP;
		GV.fuelTime = instance.account.remainFuelSeconds;
	}
	
	public void setGVValue(){
	//	instance.account.ChSeasonID= GV.ChSeasonID;// =;
	//	instance.account.myCoin = GV.myCoin;
	//	instance.account.myDollar = GV.myDollar;
	//	instance.account.SelectedTeamID = GV.SelectedTeamID ;
		instance.account.SponID =GV.SponID;
		//instance.account.CarFlag = GV.CarFlag;
		instance.account.gReview = Global.gReview;
		instance.account.currAdId = GV.CurrADId;
	//	instance.account.LV =	Global.level;
	//	instance.account.EXP =	Global.Exp;
		instance.account.remainFuelSeconds = GV.fuelTime;
	}
}
