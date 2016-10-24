using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

public class RankUserInfo  {
	
	
	public RankUserInfo(int SNSCarID, int SNSCrewID, int SponID){
		
	}
	
	
	
	public RankUserInfo(JSONNode _wr){
		
	}
}

public class RegularAIUser{
	public int CarID;
	public int CrewID;
	public string strNick;
	public Texture2D userProfile;
	public string strProfileUrl;
	
	public RegularAIUser(int car, int crew){
		this.CarID = car;
		this.CrewID = crew;
	}
}

public class RegularUserInfo{
	public int carId;
	public int crewId;
	public int sponId;
	public int carClass;
	public string userNick;
	public Texture2D userProfile;
	public string userURL;
	public bool bUser;
	public RegularUserInfo(int car, int crew, int sponId){
		this.carId = car;
		this.crewId = crew;
		this.sponId = sponId;
		this.carClass = 0;
		bUser = true;
	}
	
	public void changeUserInfo(SimpleJSON.JSONNode _node, int id){
		JSONNode _wr = _node[id]["raceData"];
		//Utility.LogWarning(_wr.Value);
		if(_wr.Value.Contains("carId") == false){
			//Utility.LogWarning("not contain user Info");
			bUser = false;
			return;
		}
		
		try{
			this.carId = _wr["carId"].AsInt;	
		}catch(NullReferenceException e){
			bUser = false;
			return;
		}
		
		
		bUser = true;
		
		
		this.crewId = _wr["crewId"].AsInt;
		this.sponId = _wr["sponId"].AsInt;
		this.carClass = _wr["carClass"].AsInt;
		this.userNick = _wr["userNick"];
		this.userURL = _wr["userURL"];
	}
}


public class RaceUserInfo{
	public int carId;
	public int teamId;
	public int carClass;
	public int crewId;
	public int sponId;
	public int carAbility;
	public int crewAbility;
	public string userNick;
	public string userURL;
	public float Torque;
	public float PitTime;
	public float BSPower;
	public float BSTime;
	public float TireTime;
	public int GBLv;
	public int BSPressTime;
	public float[] fG;
	public float[] pD;
	public float raceTime;
	public Texture2D userProfile;
	public bool bUser;
	public int DefaultCarAblility;
	public int userLevel;
	public int clubLv;
	public string clubSymbol;
	public string clubName;
	public RaceUserInfo(int carId, int crewId){
		this.carId = carId; this.crewId = crewId;
		this.carClass = 0;
	}
	
	
	public RaceUserInfo(SimpleJSON.JSONNode _node){
		
		
		
		try{
			this.carId = _node["carId"].AsInt;
			//Utility.LogWarning("simple raceuserinfo " + this.carId );
		}
		catch(NullReferenceException e){
			bUser = false;
			return;
		}
		bUser = true;
		this.teamId = _node["teamId"].AsInt;
		this.carClass = _node["carClass"].AsInt;
		this.crewId = _node["crewId"].AsInt;
		this.sponId = _node["sponId"].AsInt;
		this.carAbility = _node["carAbility"].AsInt;
		this.crewAbility = _node["crewAbility"].AsInt;
		this.userNick = _node["userNick"];
		this.userURL = _node["userURL"];
		this.raceTime = _node["raceTime"].AsFloat;
		try{
			this.userLevel = _node["level"].AsInt;
			
		}
		catch(NullReferenceException e){
			this.userLevel = 1;
		}
		try{
			this.clubSymbol = _node["ClubSymbol"].Value;
			this.clubName = _node["ClubName"].Value;
			this.clubLv = _node["ClubLevel"].AsInt;
		}
		catch(NullReferenceException e){
			this.clubLv = 0;
		}
		
	}
	
	public RaceUserInfo(SimpleJSON.JSONNode _node, int id){
		JSONNode _wr;
		try{
			_wr = _node[id]["raceData"];
		}catch(NullReferenceException e){
			bUser = false;
			return;
		}
		try{
			string a = _wr["TestRaceData"];
			if( string.IsNullOrEmpty(a) == false) {
				bUser = false; return;
			}
		}catch(NullReferenceException e){
			
		}
		
		try{
			this.carId = _wr["carId"].AsInt;	
		}catch(NullReferenceException e){
			bUser = false;
			return;
		}
		/*	response:{"state":0,"msg":"sucess","result":[{"userId":402,"teamId":10,"record":50.567859649658,"raceData":{"carId":1023,"teamId":10,"carClass":3106,"level":2,"crewId":1200,"sponId":1300,"carAbility":1251,"crewAbility":328,"userNick":"admin","userURL":"User_Default","Torque":121,"PitTime":15.99145,"BSPower":112,"BSTime":6.960001,"TireTime":0,"GBLv":25,"BSPressTime":17,"fG1":0,"fG2":0.75,"fG3":0.35,"fG4":0.35,"fG5":0.35,"fG6":0.75,"fG7":0.75,"fG8":0.35,"fG9":0,"fG10":0.75,"fG11":0.75,"fG12":0.75,"fG13":0.35,"fG14":0.75,"fG15":0.75,"fG16":0,"fG17":0,"fG18":0,"fG19":0,"fG20":0,"pD1":0,"pD2":0,"pD3":0,"pD4":0,"pD5":0,"pD6":0,"pD7":0,"pD8":0,"pD9":0,"pD10":0,"pD11":0,"pD12":0,"pD13":0,"pD14":0,"pD15":0,"pD16":0,"pD17":0,"pD18":0,"pD19":0,"pD20":0,"raceTime":50.56786},"carId":1023,"carClass":3106}
			                                                      ,{"userId":404,"teamId":10,"record":120.19999694824,"raceData":null,"carId":1000,"carClass":3106}
			                                                      ,{"userId":415,"teamId":10,"record":120.19999694824,"raceData":null,"carId":1000,"carClass":3101}
			                                                    ,{"userId":416,"teamId":10,"record":120.19999694824,"raceData":{"TestRaceData":2},"carId":1000,"carClass":3101}
			                                                      ,{"userId":406,"teamId":17,"record":0,"raceData":null,"carId":1023,"carClass":3104}
			                                                      ,{"userId":418,"teamId":10,"record":86.701843261719,"raceData":{"carId":1000,"teamId":10,"carClass":3101,"level":1,"crewId":1200,"sponId":1300,"carAbility":263,"crewAbility":328,"userNick":"KKK","userURL":"User_Default","Torque":33.2,"PitTime":16.55883,"BSPower":16,"BSTime":1.2,"TireTime":1.909,"GBLv":1,"BSPressTime":18,"fG1":1,"fG2":1,"fG3":1,"fG4":1,"fG5":0,"fG6":0,"fG7":0.6,"fG8":0,"fG9":0,"fG10":0,"fG11":0,"fG12":0,"fG13":0,"fG14":0,"fG15":0,"fG16":0,"fG17":0,"fG18":0,"fG19":0,"fG20":0,"pD1":46.90408,"pD2":0,"pD3":0,"pD4":0,"pD5":0,"pD6":0,"pD7":0,"pD8":0,"pD9":0,"pD10":0,"pD11":0,"pD12":0,"pD13":0,"pD14":0,"pD15":0,"pD16":0,"pD17":0,"pD18":0,"pD19":0,"pD20":0,"raceTime":86.70184},"carId":1000,"carClass":3101}],"time":1457327443}
*/ 
		bUser = true;
		this.carId = _wr["carId"].AsInt;	
		this.teamId = _wr["teamId"].AsInt;
		this.carClass = _wr["carClass"].AsInt;
		this.crewId = _wr["crewId"].AsInt;
		this.sponId = _wr["sponId"].AsInt;
		this.carAbility = _wr["carAbility"].AsInt;
		this.crewAbility = _wr["crewAbility"].AsInt;
		this.userNick = _wr["userNick"];
		this.userURL = _wr["userURL"];
		this.Torque = _wr["Torque"].AsFloat;
		this.PitTime = _wr["PitTime"].AsFloat;
		this.BSPower = _wr["BSPower"].AsFloat;
		this.BSTime = _wr["BSTime"].AsFloat;
		this.TireTime = _wr["TireTime"].AsFloat;
		this.GBLv = _wr["GBLv"].AsInt;
		this.BSPressTime = _wr["BSPressTime"].AsInt;
		this.fG = new float[20];
		for(int i = 1; i <= 20 ; i++){
			string str = "fG"+i;
			//	Utility.LogWarning(this.carId);
			this.fG[i-1] = float.Parse(_wr[str]);
		}
		this.pD = new float[20];
		for(int i = 1; i <= 20 ; i++){
			string str = "pD"+i;
			this.pD[i-1] = float.Parse(_wr[str]);
		}
		this.raceTime = _node["raceTime"].AsFloat;
		//strName = "fG"+i.ToString();
		//strName = "pD"+i.ToString();
		
		try{
			this.clubSymbol = _node["ClubSymbol"].Value;
			this.clubName = _node["ClubName"].Value;
			this.clubLv = _node["ClubLevel"].AsInt;
		}
		catch(NullReferenceException e){
			this.clubLv = 0;
		}
	}
	
	public RaceUserInfo(){
		
	}


	public void RankingUserSet(){
		int mCarCount = 0;
		int mCrewCount = 0;
		int[] carList, crewList;
		int a = Common_Car_Status.stockCarList.Count ;
		int b = Common_Car_Status.SpecialCarList.Count;
		mCarCount =a+b;
		mCrewCount = Common_Team.stockCrewList.Count;
		crewList = new int[mCrewCount];
		carList = new int[mCarCount];
		for(int i = 0; i < mCrewCount;i++){
			crewList[i] = Common_Team.stockCrewList[i];
		}
		for(int i = 0; i<a;i++){
			carList[i] = Common_Car_Status.stockCarList[i];
		}
		for(int i = a; i<mCarCount;i++){
			carList[i] = Common_Car_Status.SpecialCarList[i-a];
		}
		
		
		int n = (int)Well512.Next(0,9);
		this.carId = (int)Well512.Next(1000,1024);
		this.crewId = (int)Well512.Next(1200,1207);//crewList[n];
		int _nick = 73058+ n;
		this.userNick =  KoStorage.GetKorString(_nick.ToString());
		//	this.userURL =  "https://s3-ap-northeast-1.amazonaws.com/gabangman01/MultiPicture/MultiCom_"+(n+1).ToString()+".png";
		this.userURL =  "User_Default";
		this.bUser = false;
		this.clubLv = 0;
		this.carClass = GV.getTeamCarClassId(GV.SelectedTeamID);
	}
	
	public void changeUserInfo(SimpleJSON.JSONNode _node, int id){
		JSONNode _wr;// = _node[id]["raceData"];
		//	Utility.LogWarning(_wr.Value);
		
		
		try{
			_wr = _node[id]["raceData"];
		}catch (NullReferenceException e){
			bUser = false;
			return;
		}
		try{
			string a = _wr["TestRaceData"];
			if( string.IsNullOrEmpty(a) == false) {
				bUser = false; return;
			}
		}catch(NullReferenceException e){
			
		}
		
		
		try{
			this.carId = _wr["carId"].AsInt;	
		}catch(NullReferenceException e){
			bUser = false;
			return;
		}
		bUser = true;
		this.carId = _wr["carId"].AsInt;	
		this.teamId = _wr["teamId"].AsInt;
		this.carClass = _wr["carClass"].AsInt;
		this.crewId = _wr["crewId"].AsInt;
		this.sponId = _wr["sponId"].AsInt;
		this.carAbility = _wr["carAbility"].AsInt;
		this.crewAbility = _wr["crewAbility"].AsInt;
		this.userNick = _wr["userNick"];
		this.userURL = _wr["userURL"];
		this.Torque = _wr["Torque"].AsFloat;
		this.PitTime = _wr["PitTime"].AsFloat;
		this.BSPower = _wr["BSPower"].AsFloat;
		this.BSTime = _wr["BSTime"].AsFloat;
		this.TireTime = _wr["TireTime"].AsFloat;
		this.GBLv = _wr["GBLv"].AsInt;
		this.BSPressTime = _wr["BSPressTime"].AsInt;
		this.fG = new float[20];
		for(int i = 1; i <= 20 ; i++){
			string str = "fG"+i;
			this.fG[i-1] = float.Parse(_wr[str]);
		}
		this.pD = new float[20];
		for(int i = 1; i <= 20 ; i++){
			string str = "pD"+i;
			this.pD[i-1] = float.Parse(_wr[str]);
		}
		try{
			this.clubSymbol = _node["ClubSymbol"].Value;
			this.clubName = _node["ClubName"].Value;
			this.clubLv = _node["ClubLevel"].AsInt;
		}
		catch(NullReferenceException e){
			this.clubLv = 0;
		}
	}
}


