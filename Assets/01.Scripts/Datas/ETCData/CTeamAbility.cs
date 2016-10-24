using UnityEngine;
using System.Collections;

public class CTeamAbility  {

	public CTeamAbility(){
	
	}

	public int CrewAbility(int crewID, int teamcode, int mode = 0){
		Common_Crew_Status.Item _crewstatus = Common_Crew_Status.Get(crewID);
		//Account.CrewInfo _crewinfo = myAccount.instance.GetCrewInfo(crewID);
		CrewInfo _crewinfo;
		if(mode == 1){
			_crewinfo = GV.getTeamCrewInfo(teamcode);
		}else{
			_crewinfo = GV.getTeamCrewInfo(GV.SelectedTeamID);
		}

		int chLv = 0, jLv = 0, tLv = 0, gLv =0;
		if(_crewinfo != null){
			chLv = _crewinfo.chiefLv-1;
			jLv = _crewinfo.jackLv-1;
			tLv = _crewinfo.tireLv-1;
			gLv = _crewinfo.gasLv-1;
		}
		
		float team = 0;
		team = (1-_crewstatus.Chief) *100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Chief).Ratio * chLv)*200
			+ _crewstatus.Jack *100 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio * jLv *100);
		
		float team1 =50+ (1-_crewstatus.Tire)*100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Tire).Ratio) * tLv*1000;
		team+= team1;
		
		team1 = (20-_crewstatus.Gas)*6 + (-Upgrade_Crew_Ratio.Get( (int)CrewPartID.GasMan).Ratio * (gLv) )*20
			+_crewstatus.Jack *50 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio) * jLv*80;
		team += team1;
		int _team = Mathf.RoundToInt(team) + _crewstatus.Plus_Perform;
		return _team;
	}

	public int CarAbility(int carID, int teamcode, int mode = 0){
		Common_Car_Status.Item _carstatus = Common_Car_Status.Get(carID);
		CarInfo _carinfo;
		_carinfo = GV.getTeamCarInfo(GV.SelectedTeamID);
		if(mode == 1){
			_carinfo = GV.getTeamCarInfo(teamcode);
		}else{
			_carinfo = GV.getTeamCarInfo(GV.SelectedTeamID);
		}
		int mbody=0, mengine = 0, mtire = 0, mgb = 0;
		int mintake = 0,  mbspower = 0, mbstime = 0;
		if(_carinfo != null){
			mbody = _carinfo.bodyLv-1; 
			mengine = _carinfo.engineLv-1;
			mtire = _carinfo.tireLv-1;
			mgb = _carinfo.gearBoxLv-1;
			mintake = _carinfo.intakeLv-1;
			mbspower = _carinfo.bsPowerLv-1;
			mbstime = _carinfo.bsTimeLv-1;
		}

		float gTorque = (float)_carstatus.Power + (float)_carinfo.carClass.Power + (Upgrade_Car_Ratio.Get ((int)CarPartID.Body).Ratio)*mbody
			+ (Upgrade_Car_Ratio.Get ((int)CarPartID.Engine).Ratio)*mengine
				+  (Upgrade_Car_Ratio.Get ((int)CarPartID.Intake).Ratio)*mintake;
		int val1 = _carstatus.ReqLV*7;
		int val2 = mtire*7;
		int val3 = mgb*9;
		float val4 = (float)mbspower*(Upgrade_Car_Ratio.Get ((int)CarPartID.BsPower).Ratio*0.4f);
		float val5 = (float)mbstime*(Upgrade_Car_Ratio.Get ((int)CarPartID.BsTime).Ratio * 2);
		int itemValue = 100 + Mathf.RoundToInt(gTorque*5) + val1+val2+val3+Mathf.RoundToInt(val4)+Mathf.RoundToInt(val5);
		return itemValue;
	}
	public int TeamCrewAbility(int crewID){
		Common_Crew_Status.Item _crewstatus = Common_Crew_Status.Get(crewID);
		CrewInfo _crewinfo;
		_crewinfo = GV.getTeamCrewInfo(GV.SelectedTeamID);
		int chLv = 0, jLv = 0, tLv = 0, gLv =0;
		int plus = 0;
		if(_crewinfo != null){
			chLv = _crewinfo.chiefLv-1;
			jLv = _crewinfo.jackLv-1;
			tLv = _crewinfo.tireLv-1;
			gLv = _crewinfo.gasLv-1;

		}else{
		
		}
		float team = 0;
		team = (1-_crewstatus.Chief) *100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Chief).Ratio * chLv)*200
			+ _crewstatus.Jack *100 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio * jLv *100);
		
		float team1 =50+ (1-_crewstatus.Tire)*100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Tire).Ratio) * tLv*1000;
		team+= team1;
		
		team1 = (20-_crewstatus.Gas)*6 + (-Upgrade_Crew_Ratio.Get( (int)CrewPartID.GasMan).Ratio * (gLv) )*20
			+_crewstatus.Jack *50 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio) * jLv*80;
		team += team1;
		int _team = Mathf.RoundToInt(team) + _crewstatus.Plus_Perform; 
		return _team;
	}
	
	
	
	public int TeamCarAbility(int carID){
		Common_Car_Status.Item _carstatus = Common_Car_Status.Get(carID);
		CarInfo _carinfo = GV.getTeamCarInfo(GV.SelectedTeamID);
		int mbody=0, mengine = 0, mtire = 0, mgb = 0;
		int mintake = 0,  mbspower = 0, mbstime = 0;
		if(_carinfo != null){
			mbody = _carinfo.bodyLv-1; 
			mengine = _carinfo.engineLv-1;
			mtire = _carinfo.tireLv-1;
			mgb = _carinfo.gearBoxLv-1;
			mintake = _carinfo.intakeLv-1;
			mbspower = _carinfo.bsPowerLv-1;
			mbstime = _carinfo.bsTimeLv-1;
		}
		float gTorque = (float)_carstatus.Power+ (float)_carinfo.carClass.Power+ (Upgrade_Car_Ratio.Get ((int)CarPartID.Body).Ratio)*mbody
			+ (Upgrade_Car_Ratio.Get ((int)CarPartID.Engine).Ratio)*mengine
				+  (Upgrade_Car_Ratio.Get ((int)CarPartID.Intake).Ratio)*mintake;
		int val1 = _carstatus.ReqLV*7;
		int val2 = mtire*7;
		int val3 = mgb*9;
		float val4 = (float)mbspower*(Upgrade_Car_Ratio.Get ((int)CarPartID.BsPower).Ratio*0.4f);
		float val5 = (float)mbstime*(Upgrade_Car_Ratio.Get ((int)CarPartID.BsTime).Ratio * 2);
		int itemValue = 100 + Mathf.RoundToInt(gTorque*5) + val1+val2+val3+Mathf.RoundToInt(val4)+Mathf.RoundToInt(val5);
		return itemValue;
	}

	public int MyCarAbility(int carID, string classid, int teamid){
		Common_Car_Status.Item _carstatus = Common_Car_Status.Get(carID);
		CarInfo _carinfo = GV.GetMyCarInfo(carID, classid, teamid);
		int mbody=0, mengine = 0, mtire = 0, mgb = 0;
		int mintake = 0,  mbspower = 0, mbstime = 0;
		if(_carinfo != null){
			mbody = _carinfo.bodyLv-1; 
			mengine = _carinfo.engineLv-1;
			mtire = _carinfo.tireLv-1;
			mgb = _carinfo.gearBoxLv-1;
			mintake = _carinfo.intakeLv-1;
			mbspower = _carinfo.bsPowerLv-1;
			mbstime = _carinfo.bsTimeLv-1;
		}
		float gTorque = (float)_carstatus.Power+ (float)_carinfo.carClass.Power+ (Upgrade_Car_Ratio.Get ((int)CarPartID.Body).Ratio)*mbody
			+ (Upgrade_Car_Ratio.Get ((int)CarPartID.Engine).Ratio)*mengine
				+  (Upgrade_Car_Ratio.Get ((int)CarPartID.Intake).Ratio)*mintake;
		int val1 = _carstatus.ReqLV*7;
		int val2 = mtire*7;
		int val3 = mgb*9;
		float val4 = (float)mbspower*(Upgrade_Car_Ratio.Get ((int)CarPartID.BsPower).Ratio*0.4f);
		float val5 = (float)mbstime*(Upgrade_Car_Ratio.Get ((int)CarPartID.BsTime).Ratio * 2);
		int itemValue = 100 + Mathf.RoundToInt(gTorque*5) + val1+val2+val3+Mathf.RoundToInt(val4)+Mathf.RoundToInt(val5);
		return itemValue;
	}

	public int DefaultCarAbility(int carid, Common_Class.Item strClass){
		Common_Car_Status.Item _carstatus = Common_Car_Status.Get(carid);
		int mbody=0, mengine = 0, mtire = 0, mgb = 0;
		int mintake = 0,  mbspower = 0, mbstime = 0;
		//	mbody = strClass.StarLV+strClass.UpLimit-1; 
		//	mengine =  strClass.StarLV+strClass.UpLimit-1; 
		//	mtire =  strClass.StarLV+strClass.UpLimit-1; 
		//	mgb =  strClass.StarLV+strClass.UpLimit-1; 
		//	mintake =  strClass.StarLV+strClass.UpLimit-1; 
		//	mbspower =  strClass.StarLV+strClass.UpLimit-1; 
		//	mbstime =  strClass.StarLV+strClass.UpLimit-1; 
		
		
		mbody = 0; 
		mengine =  0; 
		mtire =  0; 
		mgb =  0; 
		mintake =  0; 
		mbspower =  0; 
		mbstime =  0; 
		
		
		int power =  strClass.Class_power;
		//string[] arr  = strClass.Class_power.Split(';');
		//int.TryParse(arr[1], out power);
		float gTorque = (float)_carstatus.Power+ (float)power+ (Upgrade_Car_Ratio.Get ((int)CarPartID.Body).Ratio)*mbody
			+ (Upgrade_Car_Ratio.Get ((int)CarPartID.Engine).Ratio)*mengine
				+  (Upgrade_Car_Ratio.Get ((int)CarPartID.Intake).Ratio)*mintake;
		int val1 = _carstatus.ReqLV*7;
		int val2 = mtire*7;
		int val3 = mgb*9;
		float val4 = (float)mbspower*(Upgrade_Car_Ratio.Get ((int)CarPartID.BsPower).Ratio*0.4f);
		float val5 = (float)mbstime*(Upgrade_Car_Ratio.Get ((int)CarPartID.BsTime).Ratio * 2);
		int itemValue = 100 + Mathf.RoundToInt(gTorque*5) + val1+val2+val3+Mathf.RoundToInt(val4)+Mathf.RoundToInt(val5);
		return itemValue;
	}
/*
	public int FeaturedCar(int carID){
		Common_Car_Status.Item _carstatus = Common_Car_Status.Get(carID);
		int mbody=0, mengine = 0, mtire = 0, mgb = 0;
		int mintake = 0,  mbspower = 0, mbstime = 0;
		if(_carstatus.BodyAble != 0) mbody = _carstatus.UpLimit-1; 
		if(_carstatus.EngineAble != 0) mengine = _carstatus.UpLimit-1;
		if(_carstatus.TireAble !=0) mtire = _carstatus.UpLimit-1; 
		if(_carstatus.GBoxAble !=0 ) mgb = _carstatus.UpLimit-1; 
		if(_carstatus.IntakeAble != 0) mintake = _carstatus.UpLimit-1; 
		if(_carstatus.BsPowerAble !=0) mbspower = _carstatus.UpLimit-1; 
		if(_carstatus.BsTimeAble != 0) mbstime = _carstatus.UpLimit-1; 
		
		float gTorque = (float)_carstatus.Power + (Upgrade_Car_Ratio.Get ((int)CarPartID.Body).Ratio)*mbody
			+ (Upgrade_Car_Ratio.Get ((int)CarPartID.Engine).Ratio)*mengine
				+  (Upgrade_Car_Ratio.Get ((int)CarPartID.Intake).Ratio)*mintake;
		int val1 = _carstatus.ReqLV*10;
		int val2 = mtire*5;
		int val3 = mgb*7;
		float val4 = (float)mbspower*(Upgrade_Car_Ratio.Get ((int)CarPartID.BsPower).Ratio*3);
		float val5 = (float)mbstime*(Upgrade_Car_Ratio.Get ((int)CarPartID.BsTime).Ratio * 60);
		int itemValue = 100 + Mathf.RoundToInt(gTorque*3) + val1+val2+val3+Mathf.RoundToInt(val4)+Mathf.RoundToInt(val5);
		return itemValue;
	}
	public int FeaturedCrew(int crewID){
		int cnt = Common_Crew_Status.crewListItem.Count;
		int count =0;
		int crew = 0;
		for(int i =0; i < cnt; i++){
			//Utility.Log(Common_Crew_Status.crewListItem[i]);
			Common_Crew_Status.Item _crewstatus = Common_Crew_Status.Get(Common_Crew_Status.crewListItem[i]);
			if(Global.MySeason == _crewstatus.ReqLV){
				if(count == 0){
					crew = int.Parse(_crewstatus.ID);
				}
				count++;
			}
		}
		int abi = 0;
		if(count == 1){
			abi = FeaturedCrewAbility(crew);
		}else if(count > 1){
			int ab = crew+count;
			if(ab > Global.eCrewID) ab = Global.eCrewID;
			abi = FeaturedCrewAbility(ab);
		}
		return abi;
	}
	public int FeaturedCrewAbility(int crewID){
		Common_Crew_Status.Item _crewstatus = Common_Crew_Status.Get(crewID);
		int chLv = 0, jLv = 0, tLv = 0, gLv =0;
		int mSeason = Global.MySeason;
		chLv =mSeason -1;
		jLv =  mSeason-1;
		tLv =  mSeason-1;
		gLv =  mSeason-1;
		float team = 0;
		team = (1-_crewstatus.Chief) *100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Chief).Ratio * chLv)*200
			+ _crewstatus.Jack *100 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio * jLv *100);
		team +=50+ (1-_crewstatus.Tire)*100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Tire).Ratio) * tLv*1000;
		
		team += (20-_crewstatus.Gas)*6 + (-Upgrade_Crew_Ratio.Get( (int)CrewPartID.GasMan).Ratio * (gLv) )*20
			+_crewstatus.Jack *50 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio) * jLv*80;
		int _team = Mathf.RoundToInt(team);
		return _team;
	}
	public int DailyCrew(int crewID){
		Common_Crew_Status.Item _crewstatus = Common_Crew_Status.Get(crewID);
		int chLv = 0, jLv = 0, tLv = 0, gLv =0;
		chLv = _crewstatus.UpLimit-1;
		jLv =  _crewstatus.UpLimit-1;
		tLv =  _crewstatus.UpLimit-1;
		gLv =  _crewstatus.UpLimit-1;
		float team = 0;
		team = (1-_crewstatus.Chief) *100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Chief).Ratio * chLv)*200
			+ _crewstatus.Jack *100 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio * jLv *100);
		team +=50+ (1-_crewstatus.Tire)*100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Tire).Ratio) * tLv*1000;
		
		team += (20-_crewstatus.Gas)*6 + (-Upgrade_Crew_Ratio.Get( (int)CrewPartID.GasMan).Ratio * (gLv) )*20
			+_crewstatus.Jack *50 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio) * jLv*80;
		int _team = Mathf.RoundToInt(team);
		return _team;
	}
*/




}
