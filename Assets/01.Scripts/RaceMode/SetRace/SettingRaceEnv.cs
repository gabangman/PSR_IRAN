using UnityEngine;
using System.Collections;

public class SettingRaceEnv : MonoBehaviour {

	protected void SaveSponBouns(int sponID){
		Common_Sponsor_Status.Item _spon = Common_Sponsor_Status.Get(sponID);
		switch(GV.ChSeason){
		case 0 : 
		
		case 1:
			Global.gRaceInfo.SponBouns = Base64Manager.instance.GlobalEncoding(_spon.BonusT1);
			break;
		case 2:
			Global.gRaceInfo.SponBouns = Base64Manager.instance.GlobalEncoding(_spon.BonusT2);
			break;
		case 3:
			Global.gRaceInfo.SponBouns = Base64Manager.instance.GlobalEncoding(_spon.BonusT3);
			break;
		case 4:
			Global.gRaceInfo.SponBouns = Base64Manager.instance.GlobalEncoding(_spon.BonusT4);
			break;
		case 5:
			Global.gRaceInfo.SponBouns = Base64Manager.instance.GlobalEncoding(_spon.BonusT5);
			break;
		case 6:
			Global.gRaceInfo.SponBouns = Base64Manager.instance.GlobalEncoding(_spon.BonusT6);
			break;
		case 7:
			Global.gRaceInfo.SponBouns = Base64Manager.instance.GlobalEncoding(_spon.BonusT7);
			break;
		case 8:
			Global.gRaceInfo.SponBouns = Base64Manager.instance.GlobalEncoding(_spon.BonusT8);
			break;
		default:
			Global.gRaceInfo.SponBouns = Base64Manager.instance.GlobalEncoding(_spon.BonusT5);
			break;
		}
		UserDataManager.instance.OnRoutinCheck(false);
	}

	protected void SaveDefaultAICarInfo(int length){
			Global.gAICarInfo[length] = new AICarInfo();
			Global.gAICarInfo[length].AICarID = Base64Manager.instance.GlobalEncoding(1000);
			Global.gAICarInfo[length].AICrewID = Base64Manager.instance.GlobalEncoding(1200);
			Global.gAICarInfo[length].AISponsorID = Base64Manager.instance.GlobalEncoding(1300);
			Global.gAICarInfo[length].Torque= Base64Manager.instance.setFloatEncoding(15,1000);
			Global.gAICarInfo[length].Pit_Time = Base64Manager.instance.setFloatEncoding(22,1000);
			Global.gAICarInfo[length].B_Power=Base64Manager.instance.setFloatEncoding(10,1000);
			Global.gAICarInfo[length].B_Time= Base64Manager.instance.setFloatEncoding(1,1000);
			Global.gAICarInfo[length].Skid_Time= Base64Manager.instance.setFloatEncoding(2,1000);
			Global.gAICarInfo[length].gbLv= 1;
			Global.gAICarInfo[length].EventTime= Base64Manager.instance.setFloatEncoding(70,1000);
			Global.gAICarInfo[length].EventRange=Base64Manager.instance.setFloatEncoding(80,1000);
			Global.gAICarInfo[length].raceResultTime = 1;
			Global.gAICarInfo[length].AIRefCarID = Base64Manager.instance.GlobalEncoding(1002);
			Global.gAICarInfo[length].isLive = false;
			Global.gAICarInfo[length].gearLedDelay = null;
			Global.gAICarInfo[length].AIRefStarLV = 0;
			Global.gAICarInfo[length].AIClass = null;
	}


	protected void SaveUserInfo(int length, int carID, int crewID, int sponID, float[] raceTime){
		Global.gAICarInfo[length] = new AICarInfo();
		Global.gAICarInfo[length].AICarID =  Base64Manager.instance.GlobalEncoding(carID);
		Global.gAICarInfo[length].AICrewID =Base64Manager.instance.GlobalEncoding(crewID); 
		Global.gAICarInfo[length].AISponsorID =Base64Manager.instance.GlobalEncoding(sponID);  
		Global.gAICarInfo[length].Torque=  Base64Manager.instance.setFloatEncoding(raceTime[0],1000);
		Global.gAICarInfo[length].Pit_Time =  Base64Manager.instance.setFloatEncoding(raceTime[1],1000);//raceTime[1];
		Global.gAICarInfo[length].B_Power= Base64Manager.instance.setFloatEncoding(raceTime[2],1000); //raceTime[2];
		Global.gAICarInfo[length].B_Time=  Base64Manager.instance.setFloatEncoding(raceTime[3],1000);//raceTime[3];
		Global.gAICarInfo[length].Skid_Time=  Base64Manager.instance.setFloatEncoding(raceTime[4],1000);//raceTime[4];
		Global.gAICarInfo[length].gbLv= (int)raceTime[5];
		Global.gAICarInfo[length].EventTime=  Base64Manager.instance.setFloatEncoding(raceTime[6],1000);//raceTime[6];
		Global.gAICarInfo[length].EventRange= Base64Manager.instance.setFloatEncoding(raceTime[7],1000); //raceTime[7];
		Global.gAICarInfo[length].isLive = true;
	}

	IEnumerator StartRacePlay(string scenName){
		SceneManager.instance.LoadingStart(scenName); 
		var temp = gameObject.GetComponent<LoadingGauage>() as LoadingGauage;
		if(temp != null) temp.stopLoading();
		//gameObject.GetComponent<LoadingGauage>().stopLoading();
		Resources.UnloadUnusedAssets();
		System.GC.Collect();
		yield return null;
	}


	protected void SetTrackLength(int trackID){
	
	}

	protected void SetClubTrackDurability(int CarID, Common_Track.Item trackItem){
		GV.TeamChangeFlag = 0;
		CarInfo _carInfo = GV.getTeamCarInfo(GV.SelectedTeamID);
		GV.PlayDruability = _carInfo.carClass.DurabilityRef;

	}
	protected void SetTrackDurability(int CarID, Common_Track.Item trackItem, int teamClubId =0){
		GV.TeamChangeFlag = 0;CarInfo _carInfo =null;
		if(teamClubId == 0)
			_carInfo = GV.getTeamCarInfo(GV.SelectedTeamID);
		else{
			GV.TeamChangeFlag = 0;
			_carInfo = GV.getTeamCarInfo(teamClubId);
		}
		if(_carInfo == null) Utility.LogError("CarInfo Null");
		
		
		if(Global.gRaceInfo.mType == MainRaceType.mEvent){
			if(Global.gRaceInfo.sType == SubRaceType.DragRace){
				Global.gRaceInfo.R1_Time = trackItem.R2_Time;
				Global.gRaceInfo.R2_Time = 0;
			}else{
				Global.gRaceInfo.R1_Time = trackItem.R1_Time;
				Global.gRaceInfo.R2_Time = trackItem.R2_Time;
			}
			GV.PlayDruability = _carInfo.carClass.DurabilityRef;
			return;
		}
		int pDura = _carInfo.carClass.Durability;
		int Durability = trackItem.Dura_dec;
		GV.PlayDruability = pDura;
		float cDur = (float)pDura / (float)_carInfo.carClass.DurabilityRef;
		if(cDur <= 0.2f){
			float t = Base64Manager.instance.getFloatEncoding(Global.gTorque,0.001f);
			t = t*0.2f;
			Global.gTorque = Base64Manager.instance.setFloatEncoding(t,1000);
			UserDataManager.instance.DurabilityMinusSetting();
		}
		
		int sponID = GV.getTeamSponID(GV.SelectedTeamID);
		int delta = 0;
		if(sponID > 1300){
			Common_Sponsor_Status.Item sItem = Common_Sponsor_Status.Get(sponID);
			int b = sItem.Dura_dec;
			delta = Durability - b;
		}else{
			delta = Durability;
		}
		int rDu = pDura - delta;
		if(rDu <= 0 ) {
			_carInfo.carClass.Durability = 0;
		}else {
			_carInfo.carClass.Durability = rDu;
		}

		if(Global.gRaceInfo.sType == SubRaceType.DragRace){
			if(Global.DragSub == 0){
				Global.gRaceInfo.R1_Time =  trackItem.R1_Time;
				Global.gRaceInfo.R2_Time = 0;
			}else{
				Global.gRaceInfo.R1_Time = trackItem.R2_Time;
				Global.gRaceInfo.R2_Time = 0;
			}
		}else{
			Global.gRaceInfo.R1_Time = trackItem.R1_Time;
			Global.gRaceInfo.R2_Time = trackItem.R2_Time;
		}
	}


	protected void setScrewDelay(int crewID){
		Common_Crew_Status.Item item = Common_Crew_Status.Get(crewID);
		Global.Screw_MAX = (float)item.P_MAX*0.001f;
		Global.Screw_MIN = (float)item.P_MIN*0.001f;
	//	Utility.Log(string.Format("{0} / {1}", Global.Screw_MAX, Global.Screw_MIN));
	}
}