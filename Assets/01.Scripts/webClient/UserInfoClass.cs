using UnityEngine;
using System.Collections;

[System.Serializable]
public class myTeamInfo{
	public int TeamCode;
	public int TeamCrewID;
	public CarInfo myCar;
	public int TeamCarID;
	public int SponID;
	public long SponRemainTime;
	public System.DateTime sponDateTime;
	public int Mode;
	public CrewInfo myCrews;
	public bool bFlag;
	public int TeamCarIndex;
	public int freeSpon;
	public myTeamInfo(int teamCode){
		TeamCode = teamCode;
		bFlag = false;
	}

	public int chiefLv;
	public int jackLv;
	public int driverLv;
	public int tireLv;
	public int gasLv;

	public myTeamInfo(int TeamCode, int mainCar, int crewid, int mode){
		this.TeamCode = TeamCode;
		this.TeamCrewID = crewid;
		this.TeamCarID = mainCar;
		this.Mode = mode;
		this.SponID = 1300;

	}
	public void setTeamCarInfo(CarInfo carInfo){
		this.myCar = carInfo;
	}

	public void setTeamCrew(int id){
		myCrews = new CrewInfo(TeamCrewID);
		myCrews.chiefLv = this.chiefLv;
		myCrews.jackLv = this.jackLv;
		myCrews.driverLv = this.driverLv;
		myCrews.tireLv = this.tireLv;
		myCrews.gasLv = this.gasLv;
	}

}


[System.Serializable]
public class CarClass{

	public int UpLimit;
	public int StarLV;
	public int Durability;
	public int ClassRepair;
	public float Brake;
	public int Power;
	public int weight;
	public int gear;
	public int bspower;
	public float grip;
	public float bstime;
	public int priceRepair;
	public int DurabilityRef;
	public int GearLmt;
	public CarClass(){
		
	}

	public void SetClass1(int up, int star, int dura, int rep, float brake, int pow){
		this.UpLimit = up; this.StarLV = star; this.Durability = dura;
		this.ClassRepair = rep; this.Brake = brake;this.Power = pow;
	}
	public void SetClass2(int weight, float grip, int gear, int bspower, float bstime, int priceRepair, int DurabilityRef){
		this.weight = weight; this.grip = grip; this.gear = gear;
		this.bspower = bspower; this.bstime = bstime;
		this.priceRepair = priceRepair; this.DurabilityRef = DurabilityRef;
	}

	public void SetClass3(int GearLmt){
		this.GearLmt = GearLmt;
	}
}

[System.Serializable]
public class CarInfo{
	public int CarIndex;
	public int CarID;
	public string ClassID;
	public int nClassID;
	public Common_Class.Item CarClassItem;
	public int TeamID;
	public int ModelID;
	public CarClass carClass;
	public int Index;
	public int SetFlag = 0;
	public int durability = 0;
	public bool bNewBuyCar =false;
	public CarInfo(int CarID, string ClassID, int TeamID, int Modelid){
		this.CarID = CarID;
		this.ClassID = ClassID;
		this.TeamID = TeamID;
		this.ModelID = Modelid;
		init (CarID);
		carClass = new CarClass();
	}
	
	public CarInfo(int carid){
		init (carid);
		carClass = new CarClass();
	}
	
	public void setCarClass(CarClass carClass){
		this.carClass = carClass;
	}
	public void setCarIndex(int index){
		this.Index = index;
	}
	public void setCarClassItem(Common_Class.Item cItem){
		this.CarClassItem = cItem;
	}
	
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
	
	public void init(int carid){
		bodyLv = 1;
		engineLv = 1;
		tireLv = 1;
		gearBoxLv = 1;
		intakeLv = 1;
		bsPowerLv = 1;
		bsTimeLv = 1;
		
		bodyStar = 0;
		engineStar  = 0;
		tireStar = 0;
		gearBoxStar = 0;
		intakeStar = 0;
		bsPowerStar =0;
		bsTimeStar = 0;
	}
}

[System.Serializable]
public class CrewInfo{
	public int crewID;
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
	public CrewInfo(int id){
		crewID = id;

	}
}


[System.Serializable]
public class MatInfo{
	public int MatID;
	public int MatQuantity;
	public MatInfo(int matID, int MatQuantity){
		this.MatID = matID;
		this.MatQuantity = MatQuantity;
	}

}

public class UserInfo{
	public int FuelCount;
	public int FuelMax;
	public int TeamCount;
	public int CarCount;
	public long CurrentTeam;
	public int UserBlock;
	public int mVIPLV;
	public string profileURL;
}



[System.Serializable]
public class appInfo{
	public string clientVer;
	public string upgradeVer;
	public string bundleVer_1;
	public string bundleURL_1;
	public string bundleVer_2;
	public string bundleURL_2;
	public int Notics1State;
	public int Notics2State;
	public int Notics3State;
	public string Notics1URL;
	public string Notics2URL;
	public string Notics3URL;
	public int plusEventState;
	public string plusEventURL;
	public int CouponState;
	public string CouponURL;
	public int eNoticsState;
	public int eNoticsType;
	public string androidMarketURL;
	public string IosURL;
	public string HomeURL;
	public string strEmail;
	public int movieAdState;
	public int rewardState;
	public int extra01;
	public int extra02;
	public int extra03;
	public int extra04;
	public int extra05;
	public float plusEventRatio;
	public int crossADVersion;
	public int crossADId;
}

public enum UserCarPartType{
	PART_BODY = 1,
	PART_ENGINE = 2,
	PART_TIRE = 3,
	PART_GEAR = 4,
	PART_INTAKE = 5,
	PART_BOOSTPOWER = 6,
	PART_BOOSTTIME = 7,
}



public enum MainRaceType{
	Champion = 1600,
	Weekly,
	Regular,
	PVP,
	mEvent,
	Club,
	Tutorial,
}

public enum SubRaceType{
	DragRace = 1,
	CityRace,
	RegularRace,
}

public enum CarPartID{
	Body = 5000,
	Engine,
	Tire,
	Gearbox,
	Intake,
	BsPower,
	BsTime,
}

public enum CrewPartID{
	Driver = 5100,
	Tire,
	Chief,
	JackMan,
	GasMan,
	
}
