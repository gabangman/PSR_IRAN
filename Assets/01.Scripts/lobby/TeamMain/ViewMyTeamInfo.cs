using UnityEngine;
using System.Collections;

public class ViewMyTeamInfo : MonoBehaviour {
	public UILabel[] lbstats;
	public UILabel[] lbNames;
	void Start(){
		lbstats[0].text = KoStorage.GetKorString("76011");//팀원
		lbstats[1].text = KoStorage.GetKorString("76012");//주의력
		lbstats[2].text =KoStorage.GetKorString("76013");// 집중력 
		lbstats[3].text =KoStorage.GetKorString("73018"); //드라이버

		lbstats[4].text = 
			KoStorage.GetKorString("76015"); // 파워
		lbstats[5].text = 
			KoStorage.GetKorString("76016"); // 무게 
		lbstats[6].text =
			KoStorage.GetKorString("76017"); // 그립
		lbstats[7].text =
			KoStorage.GetKorString("76018"); //기어박스
		lbstats[8].text = 
			KoStorage.GetKorString("76019"); //부스트
		lbstats[9].text =KoStorage.GetKorString("76020");  //내구

		lbNames[0].text = KoStorage.GetKorString("76008"); // title
		lbNames[1].text = KoStorage.GetKorString("76009"); // level : 00;
		lbNames[2].text = KoStorage.GetKorString("76010"); //level value
		lbNames[3].text = KoStorage.GetKorString("76031"); // btn TeamSelect
	//	lbNames[4].text = KoStorage.GetKorString("76021"); // btn TeamSelect

		var tr = transform.FindChild("Team_Select") as Transform;
		tr.FindChild("TeamSelect").FindChild("lbtitle").GetComponent<UILabel>().text =  KoStorage.GetKorString("76021");
		tr.FindChild("Sponsor_Change").FindChild("lbtitle").GetComponent<UILabel>().text = KoStorage.GetKorString("76003");
	}

	public void ChangeStockInfoContent(string str, bool b){


	}

	public void ChangeTeamAbility(){
	
	}

	public void ChangeInfoContent(string str, bool b){
	}

	public void unSetContent(){
	//	transform.FindChild("Car_Change").gameObject.SetActive(false);
		transform.FindChild("BTNs2").gameObject.SetActive(true);
		transform.FindChild("Team_Select").gameObject.SetActive(false);	
		transform.FindChild("Car_Repair").gameObject.SetActive(true);
		int mSponID = GV.getTeamSponID(GV.SelectedTeamID);
		if(mSponID == 1300 || mSponID == 0){
				SetSponIcon(false);
		}else{
				SetSponIcon(true);
		}
		BTN2Setting(GV.SelectedTeamID);
	}

	public void ReSetContent(){
		transform.FindChild("Btn_Gold").gameObject.SetActive(false);
		transform.FindChild("Btn_Dollar").gameObject.SetActive(false);
		transform.FindChild("Car_Select").gameObject.SetActive(true);
		transform.FindChild("BTNs2").gameObject.SetActive(false);
		transform.FindChild("Car_Repair").gameObject.SetActive(false);
		var mLock  = transform.FindChild("UnLockinfo").gameObject as GameObject;
		mLock.GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("76030");
		var tr = transform.FindChild("Team_Select") as Transform;
		tr.gameObject.SetActive(false);
		tr.FindChild("Sponsor_Change").FindChild("img_Contract").gameObject.SetActive(false);
	}

	public void SetCarContent(){
	
	}

	private void SetSponIcon(bool b){
		var temp = transform.FindChild("BTNs2").gameObject as GameObject;
		temp.transform.FindChild("Sponsor_Change").FindChild("img_Contract").gameObject.SetActive(b);
	}
	public void TeamCarSelectComplete(){
		string[] str = transform.name.Split('_');
		int id = int.Parse(str[1]);

		var mLock  = transform.FindChild("UnLockinfo").gameObject as GameObject;
		mLock.SetActive(false);
		transform.FindChild("Car_Select").gameObject.SetActive(false);
		transform.FindChild("BTNs2").gameObject.SetActive(false);
		transform.FindChild("Team_Select").gameObject.SetActive(true);
		transform.FindChild("Car_Repair").gameObject.SetActive(true);
		ChangeGraph(id);
	}

	public void TeamSponSelectComplete(){
		var mSpon = transform.FindChild("info_Spon") as Transform;
		int mSponID = GV.getTeamSponID(GV.SelectedTeamID);
		var tr = transform.FindChild("Team_Select") as Transform;
		if(mSponID == 1300 || mSponID == 0){
			mSpon.gameObject.SetActive(false);
			if(tr.gameObject.activeSelf){
				tr.FindChild("Sponsor_Change").FindChild("img_Contract").gameObject.SetActive(false);
			}else{
				SetSponIcon(false);
			}
		}else{
			mSpon.gameObject.SetActive(true);
			Common_Sponsor_Status.Item sponItem = Common_Sponsor_Status.Get(mSponID);
			mSpon.FindChild("Logo_Sponsor").GetComponent<UISprite>().spriteName = mSponID.ToString();
			mSpon.FindChild("lbSponName").GetComponent<UILabel>().text = sponItem.Name;
			mSpon.FindChild("lbSponSpec").GetComponent<UILabel>().text =
				string.Format(KoStorage.GetKorString("76112"),  sponItem.Power_inc,sponItem.Dura_dec, sponItem.Led_dec); 
			mSpon.FindChild("lbSponBonus").GetComponent<UILabel>().text = BonusSeason(sponItem).ToString();
			if(tr.gameObject.activeSelf){
				tr.FindChild("Sponsor_Change").FindChild("img_Contract").gameObject.SetActive(true);
			}else{
				SetSponIcon(true);
			}
		}
	}

	public void InitInfoContent(){
		transform.FindChild("Car_Select").gameObject.SetActive(false);
		transform.FindChild("Btn_Gold").gameObject.SetActive(false);
		transform.FindChild("Btn_Dollar").gameObject.SetActive(false);
		transform.FindChild("BTNs2").gameObject.SetActive(true);
		transform.FindChild("Team_Select").gameObject.SetActive(false);	
		transform.FindChild("Car_Repair").gameObject.SetActive(true);
		string[] str = transform.name.Split('_');
		int id = int.Parse(str[1]);
		AccountManager.instance.SetSponTime();
		ChangeGraph(id);
		BTN2Setting(id);
	}


	public void BTN2Setting(int teamID){
		var tr = transform.FindChild("BTNs2").gameObject as GameObject;
		if(CClub.ClubMode == 1){
			bool b=false;
			for(int i = 0; i < 3; i++){
				if(teamID == CClub.ClubRaceTeams[i]){
					b = true;
					break;
				}
			}
			
			if(b){
				int teamCnt = GV.listmyteaminfo.Count;
				int clubCnt = 0;
				for(int i=0; i < 3; i++){
					if(CClub.ClubRaceTeams[i] != 0){
						clubCnt++;
					}
				}
				if(teamCnt <= clubCnt) {
					tr.transform.FindChild("Car_Change").gameObject.SetActive(true);
				}else{
					if(clubCnt == 3) tr.transform.FindChild("Car_Change").gameObject.SetActive(true);
					else tr.transform.FindChild("Car_Change").gameObject.SetActive(false);
				}
				//tr.transform.FindChild("Car_Change").gameObject.SetActive(false);
			}
			else tr.transform.FindChild("Car_Change").gameObject.SetActive(true);
		}else{
			tr.transform.FindChild("Car_Change").gameObject.SetActive(true);
		}




	}
	public void ChangeInfoWindowContent(string str, bool b){
		string[] str1 = str.Split('_');
		int id = int.Parse(str1[1]);
		AccountManager.instance.SetSponTime(id);
		Common_Team.Item item = Common_Team.Get(id);
		myTeamInfo myteam = GV.listmyteaminfo.Find(obj => obj.TeamCode == id);
		var mLock  = transform.FindChild("UnLockinfo").gameObject as GameObject;
		if(myteam != null){
			transform.FindChild("Btn_Gold").gameObject.SetActive(false);
			transform.FindChild("Btn_Dollar").gameObject.SetActive(false);
			if(myteam.TeamCarID == 0){
				transform.FindChild("Car_Select").gameObject.SetActive(true);
				transform.FindChild("Team_Select").gameObject.SetActive(false);	
				mLock.SetActive(true);
				mLock.GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("76030");
				transform.FindChild("BTNs2").gameObject.SetActive(false);
				transform.FindChild("Car_Repair").gameObject.SetActive(false);
			}else{
				transform.FindChild("Car_Select").gameObject.SetActive(false);
				mLock.SetActive(false);
				if(id == GV.SelectedTeamID){
					transform.FindChild("Team_Select").gameObject.SetActive(false);	
					transform.FindChild("BTNs2").gameObject.SetActive(true);
					BTN2Setting(id);
				}else{
					transform.FindChild("Team_Select").gameObject.SetActive(true);	

					transform.FindChild("BTNs2").gameObject.SetActive(false);
				}
				transform.FindChild("Car_Repair").gameObject.SetActive(true);
			}
		
		}else{
			int SLV = GV.ChSeason;// SLV = 3;
			mLock.SetActive(true);
			transform.FindChild("BTNs2").gameObject.SetActive(false);
			if(item.ReqLV <= SLV){
				if(item.Res == 3){
					transform.FindChild("Btn_Gold").gameObject.SetActive(true);
					transform.FindChild("Btn_Gold").GetComponentInChildren<UILabel>().text =string.Format("{0:#,0}", item.Buyprice);
					//	string.Format(KoStorage.GetKorString("76026"), item.Buyprice);//.ToString();
					transform.FindChild("Btn_Dollar").gameObject.SetActive(false);
				}else{
					transform.FindChild("Btn_Gold").gameObject.SetActive(false);
					transform.FindChild("Btn_Dollar").gameObject.SetActive(true);
					transform.FindChild("Btn_Dollar").GetComponentInChildren<UILabel>().text = string.Format("{0:#,0}", item.Buyprice);
					//	string.Format(KoStorage.GetKorString("76026"), item.Buyprice);//.ToString();;//string.Format("{0:#,0}", item.Buyprice);
				}
				transform.FindChild("Car_Repair").gameObject.SetActive(false);
				transform.FindChild("Car_Select").gameObject.SetActive(false);
				mLock.GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("76025");
				transform.FindChild("Team_Select").gameObject.SetActive(false);	
			
			}else{
				//transform.FindChild("Car_Change").gameObject.SetActive(false);
				transform.FindChild("Btn_Gold").gameObject.SetActive(false);
				transform.FindChild("Btn_Dollar").gameObject.SetActive(false);
				transform.FindChild("Car_Select").gameObject.SetActive(false);
				mLock.GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("76024");
				transform.FindChild("Team_Select").gameObject.SetActive(false);	
				transform.FindChild("Car_Repair").gameObject.SetActive(false);
			}
		}
		ChangeGraph(id);
	}

	int BonusSeason(Common_Sponsor_Status.Item _item){
		int season = 0;
		switch(GV.ChSeason){
		case 1: season = _item.BonusT1;break;
		case 2:season = _item.BonusT2;	break;
		case 3:season = _item.BonusT3;	break;
		case 4:season = _item.BonusT4;	break;
		case 5: season = _item.BonusT5;break;
		case 6: season = _item.BonusT6; break;
		case 7: season = _item.BonusT7; break;
		case 8: season = _item.BonusT8; break;
		}
		return season;
	}

	public void ChangeTeamInfomation(int id, Common_Car_Status.Item caritem, Common_Crew_Status.Item crewitem){
		var mSpon = transform.FindChild("info_Spon") as Transform;
		int mSponID = GV.getTeamSponID(id);
		var tr = transform.FindChild("Team_Select") as Transform;
		if(mSponID == 1300 || mSponID == 0){
			mSpon.gameObject.SetActive(false);
			if(tr.gameObject.activeSelf){
				tr.FindChild("Sponsor_Change").FindChild("img_Contract").gameObject.SetActive(false);
			}else{
				SetSponIcon(false);
			}

		}else{
			mSpon.gameObject.SetActive(true);
			Common_Sponsor_Status.Item sponItem = Common_Sponsor_Status.Get(mSponID);
			mSpon.FindChild("Logo_Sponsor").GetComponent<UISprite>().spriteName = mSponID.ToString();
			mSpon.FindChild("lbSponName").GetComponent<UILabel>().text = sponItem.Name;
			mSpon.FindChild("lbSponSpec").GetComponent<UILabel>().text =
				string.Format(KoStorage.GetKorString("76112"),  sponItem.Power_inc,sponItem.Dura_dec, sponItem.Led_dec); 
			mSpon.FindChild("lbSponBonus").GetComponent<UILabel>().text = BonusSeason(sponItem).ToString();
			if(tr.gameObject.activeSelf){
				tr.FindChild("Sponsor_Change").FindChild("img_Contract").gameObject.SetActive(true);
			}else{
				SetSponIcon(true);
			}
		}

		Common_Team.Item mTeam = Common_Team.Get(id);
		transform.FindChild("Info_Crew").FindChild("Crewinfo").GetComponentInChildren<UISprite>().spriteName
			= mTeam.Crew.ToString()+"L";
		return;

		}

	public void ChangeGraph(int id){
	
		var tr = transform.FindChild("Stat") as Transform;
		myTeamInfo myTeam =  GV.listmyteaminfo.Find(obj => obj.TeamCode == id);
		CrewInfo crewinfo;
		CarInfo carinfo;
		int carID = 0, crewID = 0;
		if(myTeam == null){
			crewID = Common_Team.Get(id).Crew;
			carID = Common_Team.Get(id).Car_free;
			int modeid = Common_Team.Get(id).Model;
			carinfo = new CarInfo(carID,"D",id, modeid);
			carinfo.init(carID);
			int typeid = 3001;
			switch(modeid){
			case 1: typeid =typeid+ 100; break;
			case 2: typeid =typeid+ 100; break;
			case 3: typeid =typeid+ 100; break;
			}
			Common_Class.Item item1 =Common_Class.Get(typeid);
			carinfo.carClass.SetClass1(item1.UpLimit, item1.StarLV,  item1.Durability+100, item1.Repair, item1.Brake,  item1.Class_power);
			
			carinfo.carClass.SetClass2(item1.Class_weight,item1.Class_grip,  item1.Class_gear, 
			                            item1.Class_bspower, item1.Class_bstime, item1.Repair, item1.Durability+100 );
			carinfo.carClass.SetClass3(item1.GearLmt);
			crewinfo = new CrewInfo(crewID);
			crewinfo.chiefLv =1;
			crewinfo.driverLv = 1;
			crewinfo.tireLv =1;
			crewinfo.jackLv =1;
			crewinfo.gasLv =1;
		}else{
			crewID = myTeam.TeamCrewID; 
			carID = myTeam.TeamCarID;
			crewinfo = GV.getTeamCrewInfo(id);
			carinfo = GV.getTeamCarInfo(id);
		}
		//ChangeTeamInfomation(carID, crewID, carinfo, CrewInfo);
		Common_Crew_Status.Item crewstatus =  Common_Crew_Status.Get (crewID);
		Common_Car_Status.Item carstatus;

		float curvalue=0, nextvalue=0;
		float total = 0, upRatio = 0.0f;


		if(carID == 0) {
			transform.FindChild("Info_Car").gameObject.SetActive(false);
			carID = 1000;
			carstatus =  Common_Car_Status.Get (carID);
		
		}else{
			carstatus =  Common_Car_Status.Get (carID);
			var childtr = transform.FindChild("Info_Car").FindChild("Carinfo") as Transform;
			transform.FindChild("Info_Car").gameObject.SetActive(true);
			childtr.GetComponentInChildren<UISprite>().spriteName = carID.ToString();//.ID;
			childtr.FindChild("lbClass").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74024"), carinfo.ClassID);// "Class " + caritem.Class;
			childtr.FindChild("lbName").GetComponent<UILabel>().text = carstatus.Name;

			//power 파워
			upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Engine, carinfo.engineStar, carinfo.engineLv);
			float upRatio1 = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Intake, carinfo.intakeStar, carinfo.intakeLv);
			curvalue = carstatus.Power + carinfo.carClass.Power+ 10*((upRatio)
			                                                         +  (upRatio1));
			
			total =600;
			lbstats[4].transform.parent.GetComponent<stateUp>().ChangeBar(curvalue, total);
			
			//weight 무게 
			upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Body, carinfo.bodyStar, carinfo.bodyLv);
			curvalue = carstatus.Weight +carinfo.carClass.weight-(( upRatio))*50;
			total = 3500;
			lbstats[5].transform.parent.GetComponent<stateUp>().ChangeBar(curvalue, total);
			//그립 
			upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Tire, carinfo.tireStar, carinfo.tireLv);
			curvalue =(4 - (carstatus.Grip+carinfo.carClass.grip))*20 +(-( upRatio ) * 300);
			total = 700;//525;
			lbstats[6].transform.parent.GetComponent<stateUp>().ChangeBar(curvalue, total);
			
			
			//gearbox 기어박스
			upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Gearbox, carinfo.gearBoxStar, carinfo.gearBoxLv);
			curvalue = carstatus.Gbox +carinfo.carClass.gear+(-upRatio) * 5000;
			total = 3000;//
			lbstats[7].transform.parent.GetComponent<stateUp>().ChangeBar(curvalue, total);
			
			
			//부스트 
			upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.BsPower, carinfo.bsPowerStar, carinfo.bsPowerLv);
			curvalue = carstatus.BsPower + carinfo.carClass.bspower +(upRatio )*10;
			total = 1200;
			lbstats[8].transform.parent.GetComponent<stateUp>().ChangeBar(curvalue, total);

			//Durability 내구도 
			int cardu = 0;
			if(carinfo.carClass.Durability <= 0) cardu = 0;
			else cardu = carinfo.carClass.Durability;
			curvalue = (float)cardu;
			total = (float)carinfo.carClass.DurabilityRef;
			lbstats[9].transform.parent.GetComponent<stateUp>().ChangeBarDurability(curvalue, total);
		
		}



		ChangeTeamInfomation(id, carstatus, crewstatus);

	


		//팀웍 
		curvalue = (1-crewstatus.Chief) *100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Chief).Ratio) * (crewinfo.chiefLv-1) *200
			+ crewstatus.Jack *100 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio) * (crewinfo.jackLv-1) *100;
		total = 450;
		lbstats[0].transform.parent.GetComponent<stateUp>().ChangeBar(curvalue, total);

		//주의력 
		curvalue =50+ (1-crewstatus.Tire  )*100 + (-Upgrade_Crew_Ratio.Get((int)CrewPartID.Tire).Ratio) * (crewinfo.tireLv-1) *1000;
		total = 300;
		lbstats[1].transform.parent.GetComponent<stateUp>().ChangeBar(curvalue, total);

	



		//집중력
		curvalue = (20-crewstatus.Gas)*6 + (-Upgrade_Crew_Ratio.Get( (int)CrewPartID.GasMan).Ratio) * (crewinfo.gasLv-1) *20
			+crewstatus.Jack *50 + (Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio) * (crewinfo.jackLv-1) *80;
	

		total = 400;
		lbstats[2].transform.parent.GetComponent<stateUp>().ChangeBar(curvalue, total);

		//Bonus 드라이버 보너스 
		total =  crewstatus.Driver*18;
		int driverLevel =crewinfo.driverLv-1;
		switch(driverLevel){
		case 0:
			curvalue = crewstatus.Driver * 1;
			break;
		case 1:
			curvalue = crewstatus.Driver * 2;
			break;
		case 2:
			curvalue = crewstatus.Driver * 4;
			break;
		case 3:
			curvalue = crewstatus.Driver * 8;
			break;
		case 4:
			curvalue = crewstatus.Driver * 16;
			break;
		}
		lbstats[3].transform.parent.GetComponent<stateUp>().ChangeBar(curvalue, total);
	

		//curvalue = crewstatus.Driver + (Upgrade_Crew_Ratio.Get((int)CrewPartID.Driver).Ratio) * (crewinfo.driverLv-1);
		//total = 1000;
		//lbstats[3].transform.parent.GetComponent<stateUp>().ChangeBar(curvalue, total);
	
	//	if(carinfo.carClass.DurabilityRef == 0){
	//		lbstats[9].transform.parent.GetComponent<stateUp>().ChangeBarDurability(100, 100);
	//	}else{
	
	//	}

	//	foreach(CarInfo car in GV.mineCarList){
	//		Utility.LogWarning(car.CarID);
	//		Utility.LogWarning(car.carClass.Durability);
	//		Utility.LogWarning(car.carClass.DurabilityRef);
	//	}

		carinfo = null;
		crewinfo = null;
	}
	public void ReSetDurability(){

		string name = transform.name;
		int id = 0;
		if(name.Equals("TeamInfo")== true){
			id =GV.SelectedTeamID;
		}else{
			string[] str =name.Split('_');
			id = int.Parse(str[1]);
		}

		myTeamInfo myTeam =  GV.listmyteaminfo.Find(obj => obj.TeamCode == id);
		CarInfo carinfo;
		int carID = myTeam.TeamCarID;
		carinfo = GV.getTeamCarInfo(id);
		int cardu = 0;
		if(carinfo.carClass.Durability <= 0) cardu = 0;
		else cardu = carinfo.carClass.Durability;
		lbstats[9].transform.parent.GetComponent<stateUp>().ChangeBarDurability((float)cardu,  (float)carinfo.carClass.DurabilityRef);
	}

}
