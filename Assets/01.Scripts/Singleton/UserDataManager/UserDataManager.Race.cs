using UnityEngine;
using System.Collections;

public partial class UserDataManager : MonoSingleton< UserDataManager > {
	void RaceStringSetting(){
		if(AccountManager.instance.myPicture == null)
			AccountManager.instance.StartCoroutine("myProfileLoad");
		ObjectManager.ClearListGameObject();
		System.GC.Collect();
	}
	
	public void GamePlaySetting(bool isAI){
		RaceStringSetting();
		
	}

	public void DurabilityMinusSetting(){
		int _carid = GV.PlayCarID;
		Common_Car_Status.Item _carstatus =  Common_Car_Status.Get (_carid);
		Global.gGearRatio = new float[9];
		Global.gGearRatio[0] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear1,1000);
		Global.gGearRatio[1] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear2,1000);
		Global.gGearRatio[2] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear3,1000);
		Global.gGearRatio[3] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear4,1000);
		Global.gGearRatio[4] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear5,1000);
		Global.gGearRatio[5] =Base64Manager.instance.setFloatEncoding(_carstatus.Gear6,1000);
		Global.gGearRatio[6] =Base64Manager.instance.setFloatEncoding(_carstatus.Gear7,1000);
		Global.gGearRatio[7] =Base64Manager.instance.setFloatEncoding(_carstatus.Gear8,1000);
		Global.gGearRatio[8] =Base64Manager.instance.setFloatEncoding(_carstatus.Gear9,1000);
	}
	public int[] NewGamePlayFullySetting(){
		RaceStringSetting();
		int _carid = GV.PlayCarID;
		int _crewid =GV.PlayCrewID;
		Common_Crew_Status.Item _crewstatus =  Common_Crew_Status.Get (_crewid);
		Common_Car_Status.Item _carstatus =  Common_Car_Status.Get (_carid);
		
		int mbody=0, mengine=0, mtire=0, mgb = 0, mintake=0, mbspower=0, mbstime=0;
		int mchief =0, mjack = 0, mgas=0, mdriver=0, mtireman=0;
		CarInfo _carinfo = new CarInfo(_carid,"SS",10,_carstatus.Model);
		int typeid = 3000;
		string strClass = "SS";
		typeid =typeid+ 6;
		switch(_carstatus.Model){
		case 1: typeid =typeid+ 100; break;
		case 2: typeid =typeid+ 100; break;
		case 3: typeid =typeid+ 100; break;
		}
		
		Common_Class.Item item1 =Common_Class.Get(typeid);
		_carinfo.carClass.SetClass1(item1.UpLimit, item1.StarLV,  item1.Durability, item1.Repair, item1.Brake,  item1.Class_power);
		_carinfo.carClass.SetClass2(item1.Class_weight,item1.Class_grip,  item1.Class_gear, 
		                            item1.Class_bspower, item1.Class_bstime, item1.Repair, item1.Durability );
		_carinfo.carClass.SetClass3(item1.GearLmt);
		Global.gCheckRPM = _carstatus.RPM+item1.RPM;
		Global.gCheckRPM = Base64Manager.instance.GlobalEncoding(Global.gCheckRPM);
		Global.gMaxRPM = _carstatus.RPMMax+item1.RPMMax;
		Global.gMaxRPM = Base64Manager.instance.GlobalEncoding(Global.gMaxRPM);
		
		//	int starLV = _carinfo.carClass.StarLV;
		int starLV =5;
		mbody = _carinfo.carClass.UpLimit+(starLV-1)*_carinfo.carClass.UpLimit; 
		mengine = _carinfo.carClass.UpLimit+(starLV-1)*_carinfo.carClass.UpLimit; 
		mtire = _carinfo.carClass.UpLimit+(starLV-1)*_carinfo.carClass.UpLimit; 
		mgb = _carinfo.carClass.UpLimit+(starLV-1)*_carinfo.carClass.UpLimit; 
		//Global.gMyGearLv = Base64Manager.instance.GlobalEncoding(_carinfo.gearBoxLv);
		Global.gMyGearLv = mgb+1;
		mintake = _carinfo.carClass.UpLimit+(starLV-1)*_carinfo.carClass.UpLimit; 
		mbspower = _carinfo.carClass.UpLimit+(starLV-1)*_carinfo.carClass.UpLimit; 
		mbstime = _carinfo.carClass.UpLimit+(starLV-1)*_carinfo.carClass.UpLimit; 
		
		int mPow = _carinfo.carClass.Power;
		float mGe = _carinfo.carClass.grip;
		int bsPow = _carinfo.carClass.bspower;
		float bsTim = _carinfo.carClass.bstime;
		
		CrewInfo _crewinfo;
		_crewinfo = GV.getTeamCrewInfo(GV.SelectedTeamID);
		mchief = _crewinfo.chiefLv-1;
		mgas = _crewinfo.gasLv-1;
		mtireman = _crewinfo.tireLv-1;
		mjack = _crewinfo.jackLv-1;
		mdriver = _crewinfo.driverLv-1;
		
		if(_carstatus.BsPowerAble == 0) Global.isBoostable = false;
		else Global.isBoostable = true;
		//Global.p_control = _carstatus.P_Control;
		Global.rpmAlpah = Base64Manager.instance.GlobalEncoding(_carstatus.RPM_Boost+item1.RPM_Boost);
		Global.gScrewTime = _crewstatus.Tire + (Upgrade_Crew_Ratio.Get ((int)CrewPartID.Tire).Ratio)*(mtireman);
		Global.gScrewTime = Base64Manager.instance.setFloatEncoding(Global.gScrewTime ,1000);
		Global.gGasTime = _crewstatus.Gas + (Upgrade_Crew_Ratio.Get ((int)CrewPartID.GasMan).Ratio)*(mgas);
		Global.gGasTime = 1/Global.gGasTime; 
		Global.gGasTime =Base64Manager.instance.setFloatEncoding(Global.gGasTime ,1000);
		
		//int tempbonus = _crewstatus.Driver +(int) (Mathf.Abs(Upgrade_Crew_Ratio.Get((int)CrewPartID.Driver).Ratio) * (mdriver) );
		int tempbonus = 0;
		switch(mdriver){
		case 0:
			tempbonus = _crewstatus.Driver * 1;
			break;
		case 1:
			tempbonus = _crewstatus.Driver * 2;
			break;
		case 2:
			tempbonus = _crewstatus.Driver * 4;
			break;
		case 3:
			tempbonus = _crewstatus.Driver * 8;
			break;
		case 4:
			tempbonus = _crewstatus.Driver * 16;
			break;
			
		}



		Global.gBonus = Base64Manager.instance.GlobalEncoding(tempbonus); 	
		
		Global.gTorque = _carstatus.Power+mPow +  (GetStarLVRatio((int)CarPartID.Body, starLV, mbody))
			+ (GetStarLVRatio((int)CarPartID.Engine, starLV,mengine))
				+  (GetStarLVRatio((int)CarPartID.Intake, starLV,mintake));
		
		Global.gTorque = Base64Manager.instance.setFloatEncoding(Global.gTorque,1000);
		
		Global.gTireDelay = _carstatus.Grip +mGe+ (GetStarLVRatio((int)CarPartID.Tire, starLV,mtire));
		if(Global.gTireDelay < 0) Global.gTireDelay = 0.0f;
		Global.gTireDelay = Base64Manager.instance.setFloatEncoding(Global.gTireDelay,1000);

		Global.gBsPower = (float)( _carstatus.BsPower +bsPow+ (GetStarLVRatio((int)CarPartID.BsPower, starLV, mbspower))) * multipleBoost;
		Global.gBsPower =  Base64Manager.instance.setFloatEncoding(Global.gBsPower,1000);
		Global.gBsTime = _carstatus.BsTime+bsTim + (GetStarLVRatio((int)CarPartID.BsTime, starLV,mbstime));
		Global.gBsTime =  Base64Manager.instance.setFloatEncoding(Global.gBsTime,1000);
		
		Global.gRaceLedTime = _crewstatus.Chief + ((Upgrade_Crew_Ratio.Get((int)CrewPartID.Chief).Ratio) * (mchief) );
		Global.gRaceLedTime = Base64Manager.instance.setFloatEncoding(Global.gRaceLedTime, 1000);
		Global.gLedTime = new float[10];
		Global.gLedTime[0] = Global.gRaceLedTime;
		float ledRatio = (Upgrade_Car_Ratio.Get ((int)CarPartID.Gearbox).Ratio)*mgb;
		
		Global.gLedTime[1]=_carstatus.LED2+ledRatio;
		Global.gLedTime[2]=_carstatus.LED3+ledRatio;
		Global.gLedTime[3]=_carstatus.LED4+ledRatio;
		Global.gLedTime[4]=_carstatus.LED5+ledRatio;
		Global.gLedTime[5]=_carstatus.LED6+ledRatio;
		Global.gLedTime[6]=_carstatus.LED7+ledRatio;
		Global.gLedTime[7]=_carstatus.LED8+ledRatio;
		Global.gLedTime[8]=_carstatus.LED9+ledRatio;
		Global.gLedTime[9]=_carstatus.LED10+ledRatio;
		for(int i=1; i < 10; i++){
			if(Global.gLedTime[i] < 0.1f) Global.gLedTime[i]= 0.1f;
		}
		
		Global.gLedTime[1]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[1],1000);
		Global.gLedTime[2]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[2],1000);
		Global.gLedTime[3]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[3],1000);
		Global.gLedTime[4]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[4],1000);
		Global.gLedTime[5]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[5],1000);
		Global.gLedTime[6]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[6],1000);
		Global.gLedTime[7]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[7],1000);
		Global.gLedTime[8]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[8],1000);
		Global.gLedTime[9]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[9],1000);
		
		
		
		
		Global.gGearRatio = new float[9];
		Global.gGearRatio[0] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear1+item1.Gear1,1000);
		Global.gGearRatio[1] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear2+item1.Gear2,1000);
		Global.gGearRatio[2] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear3+item1.Gear3,1000);
		Global.gGearRatio[3] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear4+item1.Gear4,1000);
		Global.gGearRatio[4] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear5+item1.Gear5,1000);
		Global.gGearRatio[5] =Base64Manager.instance.setFloatEncoding(_carstatus.Gear6+item1.Gear6,1000);
		Global.gGearRatio[6] =Base64Manager.instance.setFloatEncoding(_carstatus.Gear7+item1.Gear7,1000);
		Global.gGearRatio[7] =Base64Manager.instance.setFloatEncoding(_carstatus.Gear8+item1.Gear8,1000);
		Global.gGearRatio[8] =Base64Manager.instance.setFloatEncoding(_carstatus.Gear9+item1.Gear9,1000);
		
		//Utility.Log ("GameData Setting Complete");
		// fiilAmoust Delta : default 0.015f;
		
		Global.gPitCameraTime =   _crewstatus.Jack + (Upgrade_Crew_Ratio.Get ((int)CrewPartID.JackMan).Ratio)*(mjack);//delay time (default 1.0f)
		Global.gPitCrewTime =   _crewstatus.Jack + (Upgrade_Crew_Ratio.Get ((int)CrewPartID.JackMan).Ratio)*(mjack)*0.8f;//delay time (default 1.0f)
		Global.gPitCameraDelay = 2.0f - Global.gPitCameraTime;// +_crewstatus.Jack + (-Upgrade_Crew_Ratio.Get ((int)CrewPartID.JackMan).Ratio)*(mjack);
		if(Global.gPitCameraDelay < 0) Global.gPitCameraDelay = 0;
		Global.gMaxGear = _carstatus.GearLmt + _carinfo.carClass.GearLmt;
		Global.gMaxGear = Base64Manager.instance.GlobalEncoding(Global.gMaxGear);
		Global.gPitCameraTime =Base64Manager.instance.setFloatEncoding(Global.gPitCameraTime,1000); 
		Global.gPitCrewTime = Base64Manager.instance.setFloatEncoding(Global.gPitCrewTime,1000); 
		Global.gPitCameraDelay = Base64Manager.instance.setFloatEncoding(Global.gPitCameraDelay,1000); 
		
		
		//		mbody = _carinfo.bodyLv-1; 
		//		mengine = _carinfo.engineLv-1;
		//		mtire = _carinfo.tireLv-1;
		//		mgb = _carinfo.gearBoxLv-1;
		//		mintake = _carinfo.intakeLv-1;
		//		mbspower = _carinfo.bsPowerLv-1;
		//		mbstime = _carinfo.bsTimeLv-1;
		int[] value = new int[2];
		float gTorque = (float)_carstatus.Power + (float)_carinfo.carClass.Power + (Upgrade_Car_Ratio.Get ((int)CarPartID.Body).Ratio)*mbody
			+ (Upgrade_Car_Ratio.Get ((int)CarPartID.Engine).Ratio)*mengine
				+  (Upgrade_Car_Ratio.Get ((int)CarPartID.Intake).Ratio)*mintake;
		int val1 = _carstatus.ReqLV*7;
		int val2 = mtire*7;
		int val3 = mgb*9;
		float val4 = (float)mbspower*(Upgrade_Car_Ratio.Get ((int)CarPartID.BsPower).Ratio*0.4f);
		float val5 = (float)mbstime*(Upgrade_Car_Ratio.Get ((int)CarPartID.BsTime).Ratio * 2);
		value[0] = 100 + Mathf.RoundToInt(gTorque*5) + val1+val2+val3+Mathf.RoundToInt(val4)+Mathf.RoundToInt(val5);
		
		float team = 0;
		team = (1-_crewstatus.Chief) *100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Chief).Ratio * mchief)*200
			+ _crewstatus.Jack *100 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio * mjack *100);
		
		float team1 =50+ (1-_crewstatus.Tire)*100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Tire).Ratio) * mtireman*1000;
		team+= team1;
		
		team1 = (20-_crewstatus.Gas)*6 + (-Upgrade_Crew_Ratio.Get( (int)CrewPartID.GasMan).Ratio * (mgas) )*20
			+_crewstatus.Jack *50 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio) * mjack*80;
		team += team1;
		value[1] = Mathf.RoundToInt(team);
		_carinfo = null;
		return value;

	}
	public void NewGamePlaySetting(CarInfo mCarInfo = null, CrewInfo mCrewInfo=null){
		RaceStringSetting();
		int _carid = GV.PlayCarID;
		int _crewid =GV.PlayCrewID;
		Common_Crew_Status.Item _crewstatus =  Common_Crew_Status.Get (_crewid);
		Common_Car_Status.Item _carstatus =  Common_Car_Status.Get (_carid);
		
		int mbody=0, mengine=0, mtire=0, mgb = 0, mintake=0, mbspower=0, mbstime=0;
		int mchief =0, mjack = 0, mgas=0, mdriver=0, mtireman=0;
		CarInfo _carinfo = mCarInfo;
		if(_carinfo == null){
			_carinfo = GV.getTeamCarInfo(GV.SelectedTeamID);
		}

		mbody = _carinfo.bodyLv-1; 
		mengine = _carinfo.engineLv-1;
		mtire = _carinfo.tireLv-1;
		mgb = _carinfo.gearBoxLv-1;
		Global.gMyGearLv = mgb+1;
		mintake = _carinfo.intakeLv-1;
		mbspower = _carinfo.bsPowerLv-1;
		mbstime = _carinfo.bsTimeLv-1;
		
		int typeid = 3100;
		string strClass = GV.PlayClassID;
		switch(strClass){
		case "D" : typeid =typeid+ 1;break;
		case "C" : typeid =typeid+ 2;break;
		case "B" : typeid =typeid+ 3;break;
		case "A" : typeid =typeid+ 4;break;
		case "S" : typeid =typeid+ 5;break;
		case "SS" : typeid =typeid+ 6;break;
			
		}
		
		Common_Class.Item item1 =Common_Class.Get(typeid);
		Global.gCheckRPM = _carstatus.RPM+item1.RPM;
		Global.gCheckRPM = Base64Manager.instance.GlobalEncoding(Global.gCheckRPM);
		Global.gMaxRPM = _carstatus.RPMMax+item1.RPMMax;
		Global.gMaxRPM = Base64Manager.instance.GlobalEncoding(Global.gMaxRPM);
		
		int sBody = 0, sEngine =0, sTire=0, sGearBox = 0, sIntake =0, sBsPw = 0, sBsPwTime = 0;
		sBody = _carinfo.bodyStar;
		sEngine = _carinfo.engineStar;
		sGearBox = _carinfo.gearBoxStar;
		sIntake = _carinfo.intakeStar;
		sBsPw = _carinfo.bsPowerStar;
		sTire = _carinfo.tireStar;
		sBsPwTime = _carinfo.bsTimeStar;
		
		int mPow = _carinfo.carClass.Power;
		float mGe = _carinfo.carClass.grip;
		int bsPow = _carinfo.carClass.bspower;
		float bsTim = _carinfo.carClass.bstime;
		CrewInfo _crewinfo=mCrewInfo;
		//	if(GV.SelectedTeamCode ==0 )	_crewinfo = GV.GetStockCrewInfo();
		//	else 	_crewinfo = GV.GetTourCrewInfo();
		if(_crewinfo == null){
			_crewinfo = GV.getTeamCrewInfo(GV.SelectedTeamID);
		}
		mchief = _crewinfo.chiefLv-1;
		mgas = _crewinfo.gasLv-1;
		mtireman = _crewinfo.tireLv-1;
		mjack = _crewinfo.jackLv-1;
		mdriver = _crewinfo.driverLv-1;
		int sponID = GV.getTeamSponID(GV.SelectedTeamID);
		
		
		if(_carstatus.BsPowerAble == 0) Global.isBoostable = false;
		else Global.isBoostable = true;
		//Global.p_control = _carstatus.P_Control;
		Global.rpmAlpah = Base64Manager.instance.GlobalEncoding(_carstatus.RPM_Boost+item1.RPM_Boost);
		Global.gScrewTime = _crewstatus.Tire + (Upgrade_Crew_Ratio.Get ((int)CrewPartID.Tire).Ratio)*(mtireman);
		Global.gScrewTime = Base64Manager.instance.setFloatEncoding(Global.gScrewTime ,1000);
		Global.gGasTime = _crewstatus.Gas + (Upgrade_Crew_Ratio.Get ((int)CrewPartID.GasMan).Ratio)*(mgas);
		Global.gGasTime = 1/Global.gGasTime; 
		Global.gGasTime =Base64Manager.instance.setFloatEncoding(Global.gGasTime ,1000);
		
		int tempbonus = 0;
		switch(mdriver){
		case 0:
			tempbonus = _crewstatus.Driver * 1;
			break;
		case 1:
			tempbonus = _crewstatus.Driver * 2;
			break;
		case 2:
			tempbonus = _crewstatus.Driver * 4;
			break;
		case 3:
			tempbonus = _crewstatus.Driver * 8;
			break;
		case 4:
			tempbonus = _crewstatus.Driver * 16;
			break;
		}
		Global.gBonus = Base64Manager.instance.GlobalEncoding(tempbonus); 	
		
		Global.gTorque = _carstatus.Power+mPow+GetStarLVRatio((int)CarPartID.Body,sBody,mbody)//Upgrade_Car_Ratio.Get ((int)CarPartID.Body).Ratio)*mbody
			+GetStarLVRatio((int)CarPartID.Engine,sEngine,mengine)
				+ GetStarLVRatio((int)CarPartID.Intake,sIntake,mintake);
		
		if(sponID != 1300){
			Common_Sponsor_Status.Item sItem = Common_Sponsor_Status.Get(sponID);
			Global.gTorque = Global.gTorque + (Global.gTorque*(float)sItem.Power_inc*0.01f);
		}
		
		Global.gTorque = Base64Manager.instance.setFloatEncoding(Global.gTorque,1000);
		
		Global.gTireDelay = _carstatus.Grip +mGe+GetStarLVRatio((int)CarPartID.Tire,sTire,mtire);
		if(Global.gTireDelay < 0) Global.gTireDelay = 0.0f;
		
		Global.gTireDelay = Base64Manager.instance.setFloatEncoding(Global.gTireDelay,1000);
		
		Global.gBsPower = (float)( _carstatus.BsPower +bsPow+ (GetStarLVRatio((int)CarPartID.BsPower,sBsPw,mbspower))) * multipleBoost;
		Global.gBsPower =  Base64Manager.instance.setFloatEncoding(Global.gBsPower,1000);
		
		
		
		
		Global.gBsTime = _carstatus.BsTime+bsTim + GetStarLVRatio((int)CarPartID.BsTime,sBsPwTime,mbstime );
		
		
		
		Global.gBsTime =  Base64Manager.instance.setFloatEncoding(Global.gBsTime,1000);
		
		Global.gRaceLedTime = _crewstatus.Chief + ((Upgrade_Crew_Ratio.Get((int)CrewPartID.Chief).Ratio) * (mchief) );
		Global.gRaceLedTime = Base64Manager.instance.setFloatEncoding(Global.gRaceLedTime, 1000);
		Global.gLedTime = new float[10];
		Global.gLedTime[0] = Global.gRaceLedTime;
		float ledRatio =GetStarLVRatio((int)CarPartID.Gearbox, sGearBox,mgb);// (Upgrade_Car_Ratio.Get ((int)CarPartID.Gearbox).Ratio)*mgb;
		if(sponID != 1300){
			Common_Sponsor_Status.Item sItem = Common_Sponsor_Status.Get(sponID);
			//	int sSpon = sItem.Led_dec;
			Global.gLedTime[1]=_carstatus.LED2-(_carstatus.LED2*(float)sItem.Led_dec*0.01f)+ledRatio;
			Global.gLedTime[2]=_carstatus.LED3-(_carstatus.LED3*(float)sItem.Led_dec*0.01f)+ledRatio;
			Global.gLedTime[3]=_carstatus.LED4-(_carstatus.LED4*(float)sItem.Led_dec*0.01f)+ledRatio;
			Global.gLedTime[4]=_carstatus.LED5-(_carstatus.LED5*(float)sItem.Led_dec*0.01f)+ledRatio;
			Global.gLedTime[5]=_carstatus.LED6-(_carstatus.LED6*(float)sItem.Led_dec*0.01f)+ledRatio;
			Global.gLedTime[6]=_carstatus.LED7-(_carstatus.LED7*(float)sItem.Led_dec*0.01f)+ledRatio;
			Global.gLedTime[7]=_carstatus.LED8-(_carstatus.LED8*(float)sItem.Led_dec*0.01f)+ledRatio;
			Global.gLedTime[8]=_carstatus.LED9-(_carstatus.LED9*(float)sItem.Led_dec*0.01f)+ledRatio;
			Global.gLedTime[9]=_carstatus.LED10-(_carstatus.LED10*(float)sItem.Led_dec*0.01f)+ledRatio;
		}else{
			Global.gLedTime[1]=_carstatus.LED2+ledRatio;
			Global.gLedTime[2]=_carstatus.LED3+ledRatio;
			Global.gLedTime[3]=_carstatus.LED4+ledRatio;
			Global.gLedTime[4]=_carstatus.LED5+ledRatio;
			Global.gLedTime[5]=_carstatus.LED6+ledRatio;
			Global.gLedTime[6]=_carstatus.LED7+ledRatio;
			Global.gLedTime[7]=_carstatus.LED8+ledRatio;
			Global.gLedTime[8]=_carstatus.LED9+ledRatio;
			Global.gLedTime[9]=_carstatus.LED10+ledRatio;
		}
		
		for(int i=1; i < 10; i++){
			if(Global.gLedTime[i] < 0.1f) Global.gLedTime[i]= 0.1f;
		}
		
		Global.gLedTime[1]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[1],1000);
		Global.gLedTime[2]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[2],1000);
		Global.gLedTime[3]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[3],1000);
		Global.gLedTime[4]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[4],1000);
		Global.gLedTime[5]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[5],1000);
		Global.gLedTime[6]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[6],1000);
		Global.gLedTime[7]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[7],1000);
		Global.gLedTime[8]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[8],1000);
		Global.gLedTime[9]=Base64Manager.instance.setFloatEncoding(Global.gLedTime[9],1000);
		
		Global.gGearRatio = new float[9];
		Global.gGearRatio[0] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear1+item1.Gear1,1000);
		Global.gGearRatio[1] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear2+item1.Gear2,1000);
		Global.gGearRatio[2] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear3+item1.Gear3,1000);
		Global.gGearRatio[3] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear4+item1.Gear4,1000);
		Global.gGearRatio[4] = Base64Manager.instance.setFloatEncoding(_carstatus.Gear5+item1.Gear5,1000);
		Global.gGearRatio[5] =Base64Manager.instance.setFloatEncoding(_carstatus.Gear6+item1.Gear6,1000);
		Global.gGearRatio[6] =Base64Manager.instance.setFloatEncoding(_carstatus.Gear7+item1.Gear7,1000);
		Global.gGearRatio[7] =Base64Manager.instance.setFloatEncoding(_carstatus.Gear8+item1.Gear8,1000);
		Global.gGearRatio[8] =Base64Manager.instance.setFloatEncoding(_carstatus.Gear9+item1.Gear9,1000);
		//Utility.Log ("GameData Setting Complete");
		// fiilAmoust Delta : default 0.015f;
		Global.gPitCameraTime =   _crewstatus.Jack + (Upgrade_Crew_Ratio.Get ((int)CrewPartID.JackMan).Ratio)*(mjack);//delay time (default 1.0f)
		Global.gPitCrewTime =   _crewstatus.Jack + (Upgrade_Crew_Ratio.Get ((int)CrewPartID.JackMan).Ratio)*(mjack)*0.8f;//delay time (default 1.0f)
		Global.gPitCameraDelay = 2.0f - Global.gPitCameraTime;// +_crewstatus.Jack + (-Upgrade_Crew_Ratio.Get ((int)CrewPartID.JackMan).Ratio)*(mjack);
		if(Global.gPitCameraDelay < 0) Global.gPitCameraDelay = 0;
		Global.gMaxGear = _carstatus.GearLmt+_carinfo.carClass.GearLmt;
		Global.gMaxGear = Base64Manager.instance.GlobalEncoding(Global.gMaxGear);
		Global.gPitCameraTime =Base64Manager.instance.setFloatEncoding(Global.gPitCameraTime,1000); 
		Global.gPitCrewTime = Base64Manager.instance.setFloatEncoding(Global.gPitCrewTime,1000); 
		Global.gPitCameraDelay = Base64Manager.instance.setFloatEncoding(Global.gPitCameraDelay,1000); 
	}

	public float GetStarLVRatio(int partid, int sLV, int mLV){
		Upgrade_Car_Ratio.Item upItem = Upgrade_Car_Ratio.Get(partid);
		int tLV = (mLV+1)/5;
		int rLV = (mLV+1)%5;
		if(rLV == 0) {
			tLV -= 1;
			rLV = 5;
		}
		float up = 0.0f;
		float totalValue = 0.0f;
		for(int i = 0; i < tLV ; i++){
			up = 0.0f;
			switch(i){
			case 0: up = upItem.Ratio;break;
			case 1: up = upItem.Ratio2;break;
			case 2: up = upItem.Ratio3;break;
			case 3: up = upItem.Ratio4;break;
			case 4: up = upItem.Ratio5;break;
			}
			totalValue += up*5;
		}
		switch(tLV){
		case 0: totalValue += upItem.Ratio*rLV;break;
		case 1: totalValue += upItem.Ratio2*rLV;break;
		case 2: totalValue += upItem.Ratio3*rLV;break;
		case 3: totalValue += upItem.Ratio4*rLV;break;
		case 4: totalValue += upItem.Ratio5*rLV;break;
		}
		return totalValue;
	}
	
	public int GetUpRatioViaStarLV(int partid, int sLV){
		Upgrade_Car_Ratio.Item upItem = Upgrade_Car_Ratio.Get(partid);
		int up = 0;
		switch(sLV){
		case 0: up = upItem.UpRatio;break;
		case 1: up = upItem.UpRatio_2;break;
		case 2: up = upItem.UpRatio_3;break;
		case 3: up = upItem.UpRatio_4;break;
		case 4: up = upItem.UpRatio_5;break;
		}
		return up;
	}


}
