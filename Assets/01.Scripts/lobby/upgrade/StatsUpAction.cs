using UnityEngine;
using System.Collections;

public class StatsUpAction : MonoBehaviour {
	int ConvertToLevel(string _class){
		int  maxLV = 0;
		switch(_class){
		case "C":
			maxLV = 3;
			break;
		case "B":
			maxLV = 4;
			break;
		case "A":
			maxLV = 5;
			break;
		case "S":
			maxLV = 6;
			break;
		case "SS":
			maxLV = 7;
			break;
		}
		return maxLV;
	}

	string str;
	void Start(){
		//string _carStr = gameObject.transform.FindChild("myLabel").GetComponent<UILabel>().text;
		string _carStr = gameObject.name;
		if(string.Equals(_carStr, "CarStatus")){
			if(SelectPart == string.Empty){
				SelectPart = "Body";
			}
		}else{
			if(SelectCrew == string.Empty){
				SelectPart = "Driver";
			}
		}
	}
	
	void OnEnable(){
	}

	public void SelectedCrewStatusChange(string str){
		InitCrewStatus(str);
	}

	public void SelectedCarPartStatusChange(string str){
		InitStatus(str);
	}
	
	public void ApplyUpgradeStatus(string str){
		isChangeBar = true;
		InitStatus(str);
		InitCrewStatus(str);
		//Utility.Log ("apply " + str);
	}
	
	bool isChangeBar = false;
	string SelectPart = string.Empty;
	string SelectCrew = string.Empty;
	const int maxvalue = 280;

	void InitCrewStatus(string SelectCrew){
		//UserDataManager.CrewInfo _crewinfo = UserDataManager.instance.GetCrewInfo;
		//AccountInfo.CrewInfo _crewinfo = UserDataManager.instance.GetCrewInfo(Global.MyCrewID);
		//AccountInfo.CrewInfo _crewinfo = AccountInfo.instance.GetSelectedCrewInfo();
	//	int tmepid = Base64Manager.instance.GlobalEncoding(Global.MyCrewID);
	//	Account.CrewInfo _crewinfo = myAccount.instance.GetCrewInfo(tmepid);
	//	Common_Crew_Status.Item _crewstatus =  Common_Crew_Status.Get (_crewinfo.crewId);

		CrewInfo _crewinfo;
		int crewid = 0;
	//	if(GV.SelectedTeamCode == 1) {
	//		_crewinfo = GV.GetTourCrewInfo();
	//		crewid = GV.FindTourCrew();
	//	}else {
	//		_crewinfo = GV.GetStockCrewInfo();
	//		crewid = GV.FindStockCrew();
	//	}
		crewid = GV.getTeamCrewID(GV.SelectedTeamID);
		_crewinfo = GV.getTeamCrewInfo(GV.SelectedTeamID);
		Common_Crew_Status.Item _crewstatus =  Common_Crew_Status.Get (crewid);
		int mdriver=0,mchief=0,mgas = 0, mjack=0,mtire = 0;
	//	Utility.Log (SelectCrew);
		switch(SelectCrew){
			case "Driver":
			mdriver =1;
			break;
			case "Gas":
			mgas =1;
			break;
			case "Chief":
			mchief = 1;
			break;
			case "Jack":
			mjack = 1;
			break;
			case "Tire":
			mtire = 1;
			break;
			default:
			mtire = 200;
			break;
		}
		if(mtire == 200 ) return;
		
		for(int i  = 1; i < gameObject.transform.childCount;i++){
			var _name = gameObject.transform.GetChild(i).gameObject as GameObject;
			float curvalue=0, nextvalue=0;
			float total = 0;	
			int partId = 0;
			//Utility.Log(_name.name);
			switch(_name.name){
			case "Stat_Bonus": //bonus
			{
				partId = (int)CrewPartID.Driver;
				//curvalue = _crewstatus.Driver +_crewstatus.Driver * (_crewinfo.driverLv-1);
				curvalue = _crewstatus.Driver + (Upgrade_Crew_Ratio.Get(partId).Ratio) * (_crewinfo.driverLv-1);
				if(_crewinfo.driverLv == _crewstatus.UpLimit) mdriver = 0;
				nextvalue = _crewstatus.Driver+ (Upgrade_Crew_Ratio.Get(partId).Ratio) * ((_crewinfo.driverLv-1)+mdriver) ;
				//nextvalue = _crewstatus.Driver+ _crewstatus.Driver * ((_crewinfo.driverLv-1)+mdriver) ;
				total =  _crewstatus.Driver*18;
				int driverLevel =_crewinfo.driverLv-1, driverNextLevel = (_crewinfo.driverLv-1)+mdriver;
				switch(driverLevel){
				case 0:
					curvalue = _crewstatus.Driver * 1;
					break;
				case 1:
					curvalue = _crewstatus.Driver * 2;
					break;
				case 2:
					curvalue = _crewstatus.Driver * 4;
					break;
				case 3:
					curvalue = _crewstatus.Driver * 8;
					break;
				case 4:
					curvalue = _crewstatus.Driver * 16;
					break;
				
				}
				switch(driverNextLevel){
				case 0:
					curvalue = _crewstatus.Driver * 1;
					break;
				case 1:
					nextvalue = _crewstatus.Driver *2;
					break;
				case 2:
					nextvalue = _crewstatus.Driver *4;
					break;
				case 3:
					nextvalue = _crewstatus.Driver *8;
					break;
				case 4:
					nextvalue = _crewstatus.Driver *16;
					break;
				case 5:
					nextvalue = _crewstatus.Driver *32;
					break;
				
			}
			}
				
				break;
			case "Stat_Attention": //Attention
			{
			
				curvalue = (20-_crewstatus.Gas)*6 + (-Upgrade_Crew_Ratio.Get( (int)CrewPartID.GasMan).Ratio) * (_crewinfo.gasLv-1) *20
					+_crewstatus.Jack *50 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio) * (_crewinfo.jackLv-1) *80;

				if(_crewinfo.jackLv == _crewstatus.UpLimit) mjack = 0; 
				if(_crewinfo.gasLv == _crewstatus.UpLimit) mgas = 0;
				
				nextvalue = (20-_crewstatus.Gas)*6 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.GasMan).Ratio)* ( (_crewinfo.gasLv-1)+mgas)*20
					+_crewstatus.Jack *50 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio) * ((_crewinfo.jackLv-1) +mjack)*80;
				total = 250;
			}
				break;
			case "Stat_Teamwork": //Teamwork
			{
				curvalue = (1-_crewstatus.Chief) *100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Chief).Ratio) * (_crewinfo.chiefLv-1) *200
					+ _crewstatus.Jack *100 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio) * (_crewinfo.jackLv-1) *100;
				if(_crewinfo.jackLv == _crewstatus.UpLimit) mjack = 0;
				if(_crewinfo.chiefLv == _crewstatus.UpLimit) mchief = 0;
				
				nextvalue = (1-_crewstatus.Chief) *100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Chief).Ratio) * (_crewinfo.chiefLv-1+mchief)*200
					+ _crewstatus.Jack *100 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio )* (_crewinfo.jackLv-1 +mjack)*100;
				total = 350;
			}
				break;
			case "Stat_Agility": //AGility
			{
				if(_crewinfo.tireLv == _crewstatus.UpLimit) mtire = 0;
				curvalue =50+ (1-_crewstatus.Tire)*100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Tire).Ratio) * (_crewinfo.tireLv-1) *1000;
				nextvalue = 50+ (1-_crewstatus.Tire)*100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Tire).Ratio) * (_crewinfo.tireLv-1+mtire) *1000;
				total = 200;
			}
				break;
		
			default:
				Utility.Log ("Switch Error" + _name.name);
				break;
			}
			
			total += total*0.2f;
			if(!isChangeBar)
				_name.GetComponent<stateUp>().InitStatus(curvalue, nextvalue, total, maxvalue);
			else 
				_name.GetComponent<stateUp>().ChangeStatus(curvalue, nextvalue, total, maxvalue);
		}

	}

	
	
	void InitStatus(string selectPart){
	
		CarInfo _carinfo;
		int carID = 0;
		carID = GV.getTeamCarID(GV.SelectedTeamID);
		_carinfo = GV.getTeamCarInfo(GV.SelectedTeamID);
		Common_Car_Status.Item _carstatus =  Common_Car_Status.Get (carID);
		int upLv = _carinfo.carClass.UpLimit;
		int maxLv = upLv;// + (upLv * (_carinfo.carClass.StarLV-1));
		float upRatio = 0.0f;
	//	Utility.LogWarning(maxLv);
		for(int i  = 1; i < gameObject.transform.childCount;i++){
			var _name = gameObject.transform.GetChild(i).gameObject as GameObject;
			float curvalue=0, nextvalue=0;
			float total = 0;
			int mBsTime=0,mGear=0, mTire = 0, mEngine = 0, mIntake = 0, mBody=0, mBsPower =0;
			upRatio =0.0f;
			maxLv = upLv;
			switch(selectPart){
				case "Body":
				mBody = 1;
				break;
				case "Intake":
				mIntake = 1;
				break;
				case "Engine":
				mEngine = 1;
				break;
				case "Gearbox":
				mGear = 1;
				break;
				case "N2O":
				mBsPower = 1;
				break;
				case "Brake": // n2otime
				mBsTime = 1;
				break;
				case "Tires":
				mTire = 1;
				break;
				default:
				mBsTime = 100;
				break;
			}
			//Utility.Log(selectPart);
			if(mBsTime == 100 ) return;
		
			switch(_name.name){
			case "Stat_Boost":
			{
				maxLv += maxLv*_carinfo.bsTimeStar;
				if(_carinfo.bsTimeLv == maxLv) mBsTime = 0;
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.BsTime, _carinfo.bsTimeStar, _carinfo.bsTimeLv);
				if(mBsTime == 1){
					curvalue = _carstatus.BsTime +_carinfo.carClass.bstime +(upRatio);
					upRatio =  UserDataManager.instance.GetStarLVRatio((int)CarPartID.BsTime, _carinfo.bsTimeStar, (_carinfo.bsTimeLv+mBsTime));
					nextvalue = _carstatus.BsTime +_carinfo.carClass.bstime+(upRatio);
					_name.GetComponent<stateUp>().InitBsTimeStatus(curvalue, nextvalue);
				}else {
					mBsTime = 0;
					curvalue = _carstatus.BsTime +_carinfo.carClass.bstime+(upRatio);
					_name.GetComponent<stateUp>().InitBsTimeStatus(curvalue, 0);
				}
				maxLv = upLv;
				maxLv += maxLv*_carinfo.bsPowerStar;
				if(_carinfo.bsPowerLv ==maxLv) mBsPower = 0;
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.BsPower, _carinfo.bsPowerStar, _carinfo.bsPowerLv);
				curvalue = _carstatus.BsPower +_carinfo.carClass.bspower+(upRatio)*10;
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.BsPower, _carinfo.bsPowerStar, (_carinfo.bsPowerLv+mBsPower));
				nextvalue = _carstatus.BsPower +_carinfo.carClass.bspower+(upRatio)*10;
				total = 700;
			}
				
				break;
			case "Stat_Gearbox":
			{
				maxLv += _carinfo.gearBoxStar*maxLv;

				if(_carinfo.gearBoxLv == maxLv) mGear = 0;
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Gearbox, _carinfo.gearBoxStar, _carinfo.gearBoxLv);
				curvalue = _carstatus.Gbox +_carinfo.carClass.gear +(-upRatio) * 5000;
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Gearbox, _carinfo.gearBoxStar,  (_carinfo.gearBoxLv+mGear));
				nextvalue = _carstatus.Gbox+_carinfo.carClass.gear +(-upRatio) * 5000;
				total = 4000;//1188;
			}
				break;
			case "Stat_Grip":
			{
				maxLv += _carinfo.tireStar*maxLv;
				if(_carinfo.tireLv == maxLv) mTire = 0;
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Tire, _carinfo.tireStar, _carinfo.tireLv);
				curvalue =(4 - (_carstatus.Grip+_carinfo.carClass.grip))*20 +(-( upRatio) * 300);
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Tire, _carinfo.tireStar,(_carinfo.tireLv+mTire));
				nextvalue =(4-  (_carstatus.Grip+_carinfo.carClass.grip))*20 +(-(upRatio) * 300);
				total = 900;//525;
			}
				break;
			case "Stat_Power":
			{

				if(_carinfo.intakeLv == (maxLv+_carinfo.intakeStar*maxLv)) mIntake = 0;
				if(_carinfo.engineLv == (maxLv+_carinfo.engineStar*maxLv)) mEngine = 0;
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Engine, _carinfo.engineStar, _carinfo.engineLv);
				float upRatio1 =UserDataManager.instance.GetStarLVRatio((int)CarPartID.Intake, _carinfo.intakeStar, _carinfo.intakeLv); 
				curvalue = _carstatus.Power +_carinfo.carClass.Power+ 10*(( upRatio)
				                                  +  (upRatio1));
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Engine, _carinfo.engineStar, _carinfo.engineLv+mEngine);
				upRatio1 =UserDataManager.instance.GetStarLVRatio((int)CarPartID.Intake, _carinfo.intakeStar, _carinfo.intakeLv+mIntake); 
				nextvalue = _carstatus.Power  +_carinfo.carClass.Power+10*( (upRatio)
				                                  +  (upRatio1));
				total = 450;// 757+757*0.2f;
			//	Utility.LogWarning(string.Format(" {0}.... {1} --- {2} ===== {3} |||| {4} " , maxLv, mIntake , _carinfo.intakeLv, _carinfo.intakeStar, 
				     //                          selectPart));
				//Utility.LogWarning("selectPart" + selectPart);
			}
				break;
			case "Stat_Weight":
			{
				maxLv += _carinfo.bodyStar*maxLv;
				if(_carinfo.bodyLv == maxLv) mBody = 0;
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Body, _carinfo.bodyStar, _carinfo.bodyLv);
				curvalue = _carstatus.Weight +_carinfo.carClass.weight -(upRatio)*50;
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Body, _carinfo.bodyStar, _carinfo.bodyLv+mBody);
				nextvalue = _carstatus.Weight  +_carinfo.carClass.weight-(upRatio)*50;
				total = 3000;//8062 ;
			}
				break;
			default:
				Utility.Log ("Switch Error");
				break;
			}

		//	Utility.LogWarning(string.Format("{0}  / {1}  / {2}  / {3}", curvalue,nextvalue,total,maxvalue));
			total += total*0.2f;
			if(!isChangeBar)
				_name.GetComponent<stateUp>().InitStatus(curvalue, nextvalue, total, maxvalue);
			else 
				_name.GetComponent<stateUp>().ChangeStatus(curvalue, nextvalue, total, maxvalue);
		}	
	
	}


}
