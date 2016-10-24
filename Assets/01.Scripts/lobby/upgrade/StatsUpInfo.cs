using UnityEngine;
using System.Collections;

public class StatsUpInfo : MonoBehaviour {
	
	void Start(){
		//	string[] str = gameObject.name.Split("_"[0]);
		//	ShowStateInfo(str[0], str[1]);
	}
	
	public void ChangeStatusClass(string carClass){
		//	Utility.LogWarning("ChangeStatusClass " + carClass);
		InitCarWindow(mCarID, carClass);
	}
	
	public void ChangeStatusItem(){
		Utility.LogWarning("ChangeStatusItem " + gameObject.name);
		string[] str = gameObject.name.Split("_"[0]);
		mCarID = int.Parse(str[0]);
		string strClass = str[2];
		InitCarWindow(mCarID, strClass);
	}
	
	public void ChangeMyTeamStatus(){
		Utility.LogWarning("ChangeMyTeamStatus " + gameObject.name);
		string[] str = gameObject.name.Split("_"[0]);
		mCarID = int.Parse(str[0]);
		string strClass = str[1];
		InitCarWindow(mCarID, strClass);
	}
	
	
	int mCarID = 0;
	void InitCarWindow(int carID, string strClass){
		int mcarid = carID;
		Common_Car_Status.Item _carstatus =  Common_Car_Status.Get (mcarid);
		Common_Class.Item itemClass = GV.getClassTypeID(strClass, _carstatus.Model);
		
		int mPower = itemClass.Class_power;
		int mWeight =itemClass.Class_weight;
		int mGrip =itemClass.Class_grip_1t;
		int mGear =  itemClass.Class_gear;
		int mBsPower = itemClass.Class_bspower;
		int mBsTime =  itemClass.Class_bstime_1t;
		
		for(int i  = 1; i < gameObject.transform.childCount;i++){
			var _name = gameObject.transform.GetChild(i).gameObject as GameObject;
			float curvalue=0, nextvalue=0;
			float total = 0;
			float upRatio = 0;
			//Utility.LogWarning("InitCarWindow " + _name.name);
			switch(_name.name){
			case "Stat_Boost":
			{
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.BsTime, 0, 1);
				curvalue = _carstatus.BsTime +(float)mBsTime*0.001f +(upRatio);//(Upgrade_Car_Ratio.Get((int)CarPartID.BsTime).Ratio * 10);
				//Utility.LogWarning("carClassBStime0 " + (float)mBsTime*0.001f);
				total = 600;
				_name.GetComponent<stateUp>().InitBsTimeStatus(curvalue, nextvalue);
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.BsPower, 0, 1);
				curvalue = _carstatus.BsPower+mBsPower + upRatio*10;//((Upgrade_Car_Ratio.Get((int)CarPartID.BsPower).Ratio )*1);
				total = 700;
			}
				break;
			case "Stat_Gearbox":
			{
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Gearbox, 0, 1);
				curvalue =_carstatus.Gbox+ mGear+(-upRatio) * 5000;//(-Upgrade_Car_Ratio.Get((int)CarPartID.Gearbox).Ratio * 1) * 5000;
				total = 4000;
			}
				break;
			case "Stat_Grip":
			{
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Tire, 0, 1);
				curvalue =(4 - (_carstatus.Grip+(float)mGrip*0.001f))*20 -upRatio*300;//((- Upgrade_Car_Ratio.Get((int)CarPartID.Tire).Ratio ) * 300);
				total = 450;
			}
				break;
			case "Stat_Power":
			{
				//upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Engine, 0, 1);
				//upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Intake, 0, 1);
				curvalue = _carstatus.Power +mPower + 10*(UserDataManager.instance.GetStarLVRatio((int)CarPartID.Engine, 0, 1)
				                                          +  ( UserDataManager.instance.GetStarLVRatio((int)CarPartID.Intake, 0, 1)) );
				total = 450;
			}
				break;
			case "Stat_Weight":
			{
				//	curvalue = _carstatus.Weight+mWeight -(( Upgrade_Car_Ratio.Get((int)CarPartID.Body).Ratio * 1))*50;
				total = 3000;
				upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Body, 0, 1);
				curvalue = _carstatus.Weight  +mWeight-(upRatio)*50;
			}
				break;
			default:
				Utility.Log ("Switch Error"+_name.name);
				break;
			}
			
			nextvalue = curvalue;
			total += total*0.2f;
			_name.GetComponent<stateUp>().ChangeStatus(curvalue, nextvalue, total, 280);
		}	
		
	}
	
	void InitCrewWindow(string _id){
		int crewID = int.Parse(_id);
		Common_Crew_Status.Item _crewstatus =  Common_Crew_Status.Get (crewID);
		for(int i  = 1; i < gameObject.transform.childCount;i++){
			//gameObject.transform.GetChild(i).GetComponent<stateUp>();
			var _name = gameObject.transform.GetChild(i).gameObject as GameObject;
			float curvalue=0, nextvalue=0;
			float total = 0;	
			int partId = 0;
			switch(_name.name){
			case "Stat_Bonus": //bonus
			{
				partId = (int)CrewPartID.Driver;
			//	curvalue =	_crewstatus.Driver + (Upgrade_Crew_Ratio.Get(partId).Ratio) * 0;
			//	curvalue =	_crewstatus.Driver +_crewstatus.Driver * 0;
			//	total = 600;
				curvalue = 	_crewstatus.Driver;
				total =  _crewstatus.Driver*18;
			
			}
				
				break;
			case "Stat_Attention": //Attention
			{
				
				curvalue = (20-_crewstatus.Gas)*6 + (-Upgrade_Crew_Ratio.Get( (int)CrewPartID.GasMan).Ratio) * (0)*20
					+_crewstatus.Jack *50 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio) * 0*80;
				
				total = 250;
			}
				break;
			case "Stat_Teamwork": //Teamwork
			{
				curvalue = (1-_crewstatus.Chief) *100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Chief).Ratio) * 0 *200
					+ _crewstatus.Jack *100 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio) * 0 *100;
				total = 350;
				
			}
				break;
			case "Stat_Agility": //AGility
			{
				
				curvalue = 50+ (1-_crewstatus.Tire)*100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Tire).Ratio) * (0)*1000;
				total = 200;
				
			}
				break;
				
			default:
				Utility.Log ("Switch Error");
				break;
			}
			//Utility.Log ("curvale " +_name.name + "   " + curvalue);
			total += total*0.2f;
			nextvalue=curvalue;
			_name.GetComponent<stateUp>().ChangeStatus(curvalue, nextvalue, total, 280);
		}
		
	}
	
}
