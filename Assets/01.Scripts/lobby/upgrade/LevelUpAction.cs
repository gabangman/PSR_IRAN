using UnityEngine;
using System.Collections;
using System.Text;

public partial class LobbyManager : MonoBehaviour {
	void ChangeTeamAbilityCount(int idx){
		var ab = MenuTop.transform.FindChild("Ability") as Transform;
		if(idx == 0) ab.gameObject.SendMessage("ChangeCarTeamAbilityCount", 0,SendMessageOptions.DontRequireReceiver);
		else ab.gameObject.SendMessage("ChangeCrewAbilityCount", 0,SendMessageOptions.DontRequireReceiver);
	}
}


public class LevelUpAction : MonoBehaviour {

	// Use this for initialization
	void OnEnable(){

	}
	void Start(){
		var temp = transform.FindChild("btnEvo").gameObject as GameObject;
		if(temp != null){
			temp.transform.FindChild("lbName").GetComponent<UILabel>().text =  KoStorage.GetKorString("76402");
			
		}
	//	temp = transform.FindChild("btnEvo_lock").gameObject;
	//	if(temp != null){
	//		temp.transform.FindChild("lbName").GetComponent<UILabel>().text =  KoStorage.GetKorString("76402");
	//	}
	
	}
	void ChangeObjectName(){
		StringBuilder objName = new StringBuilder();
		string[] _send  = gameObject.name.Split("_"[0]);
		objName.Length = 0;
		objName.Append(_send[0]);
		objName.Append("_");
		objName.Append(_send[1]);
		gameObject.name = objName.ToString();
	}

	void ClickAction(string str){
		ChangeObjectName();
		gameObject.name = gameObject.name + "_"+str;
		var temp = gameObject.GetComponent<CreateUpgradePopUp>() as CreateUpgradePopUp;
		if(temp == null) temp = gameObject.AddComponent<CreateUpgradePopUp>();//Destroy(temp); 
		temp.OnResponse(ResponseUpgrade); 

	}

	void OnUpgradeDollarClick(){
		ClickAction("dollar");
		//UserDataManager.instance.TorqueTest();
	}

	void OnUpgradeCoinClick(){
		ClickAction("coin");
		//UserDataManager.instance.TorqueTest();
	}

	void UpgradeFailed(){
		//var lobby = GameObject.Find("LobbyUI") as GameObject;
		//lobby.SendMessage("ElevatorCarAni");
//		Utility.Log("failed");
		//	ChangeObjectName();
	//	var temp  = gameObject.transform.FindChild("popUp").gameObject as GameObject;
	//	temp.transform.FindChild("lbText").GetComponent<UILabel>().text = "not enuogh money";
	//	resetPrice(temp);
	//	isfail = true;
	//	isclickok= true;
	
	}

	void ResponseUpgrade(bool isSuccess){

		if(isSuccess)
			UpgradeSuccessed();
		else UpgradeFailed();
		//Elevator Car Up
	}



	void UpgradeSuccessed(){
		//StartCoroutine("HiddenDelay");
		var lobby = GameObject.Find("LobbyUI") as GameObject;
		isUpdate = true;
		SelectedCrewInfo(selectpartstring);
		if(isUpdate) {
			changeLevelWindow();
			lobby.SendMessage("ChangeLevelCrewButton" ,false, SendMessageOptions.DontRequireReceiver);
			lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
			lobby.SendMessage("ApplyStatusWindow",selectpartstring, SendMessageOptions.DontRequireReceiver);
			GameObject.Find("Audio").SendMessage("CompleteSound");
			lobby.SendMessage("ChangeTeamAbilityCount" ,1, SendMessageOptions.DontRequireReceiver);
			return;
		}

		isUpdate = true;
		SelectedPartInfo(selectpartstring);
		if(isUpdate) {
			lobby.SendMessage("ElevatorCarAni");
			lobby.SendMessage("ChangeLevelCarButton",false, SendMessageOptions.DontRequireReceiver);
		}
		changeLevelWindow();
		lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
		lobby.SendMessage("ApplyStatusWindow",selectpartstring, SendMessageOptions.DontRequireReceiver);
		GameObject.Find("Audio").SendMessage("CompleteSound");
		lobby.SendMessage("ChangeTeamAbilityCount" ,0, SendMessageOptions.DontRequireReceiver);
	}

	public void UpdateCarUpgarde(){
		InitUpgradeCarInfo(selectpartstring);
		changeLevelWindow();
	}

	public void UpdateCrewUpgrade(){
		InitUpgradeCrewInfo(selectpartstring);
		changeLevelWindow();
	}

	IEnumerator HiddenDelay(){
		yield return new WaitForSeconds(0.1f);
		//gameObject.transform.FindChild("popUp").gameObject.SetActive(false);
		//isclickok= true;
	}

	int mlevel =0, nlevel = 0;
	int coin = 0, dollar =0;
	string selectpartstring = string.Empty;
	bool isupgrade = false;
	bool isUpdate = true;
	bool isMax = false;
	int CONVERTER_DOLLAR = Global.gConvertDollar;

	void SelectedPartInfo(string str){
		int partid =0;
		int tmepid = GV.getTeamCarID(GV.SelectedTeamID);
		CarInfo _carinfo = GV.getTeamCarInfo(GV.SelectedTeamID);

//		Common_Car_Status.Item _carstatus =  Common_Car_Status.Get (tmepid);
		CONVERTER_DOLLAR = Global.gConvertDollar;
		int MaxStarLV = _carinfo.carClass.StarLV-1;
	//	if(_carstatus.ReqLV < 2){
	//		MaxStarLV = 10;
	//		isMax = true;
	//	}else{
	//		isMax = false;
	//	}
		isMax = false;
	//	float levelupcoin= 0;
		selectpartstring = str;
		int MaxLevel = _carinfo.carClass.UpLimit;//.UpLimit;
		string lbText = string.Empty;
	//	float upratio = 0.0f;
	//	Utility.LogWarning("car " + MaxLevel);

		int upLV = 0;
		int upCar = 0;
		int upClass = 0;
		int upID=0;
		upClass = GV.getClassIDInTeamCar(GV.getTeamCarID(GV.SelectedTeamID), GV.SelectedTeamID).Class_x;
	//	Utility.LogWarning("car " + upClass);

		switch(str){
			case "Engine":
		{		
			upLV = _carinfo.engineLv;
			upID = 11000;
			upCar = AccountManager.instance.PART_ENGINE;
			upID += upLV;
			upID += (upCar-1)*25;
			upID += (upClass-1)*175;
			upID -= 1;
			partid  = (int)CarPartID.Engine;
			coin = UpgradeCarCost.Get(upID).UpCoin;
			dollar = coin*CONVERTER_DOLLAR;
			mlevel = _carinfo.engineLv;
			MaxLevel += _carinfo.engineStar *MaxLevel;
			if(_carinfo.engineStar >= MaxStarLV) isMax = true;
			if(MaxLevel <= mlevel){
				nlevel = CONVERTER_DOLLAR;
			}else{
				nlevel = mlevel+1;
			} 
			gameObject.name = tmepid+"_"+partid.ToString();//5001"; 
			lbText = Upgrade_Car_Ratio.Get(partid).Text;

		}break;
			case "Body":
			{
			upLV = _carinfo.bodyLv;
			upID = 11000;
			upCar = AccountManager.instance.PART_BODY;
			upID += upLV;
			upID += (upCar-1)*25;
			upID += (upClass-1)*175;
			upID -= 1;
			partid  = (int)CarPartID.Body;
			coin = UpgradeCarCost.Get(upID).UpCoin;
			dollar = coin*CONVERTER_DOLLAR;
			mlevel = _carinfo.bodyLv;
			MaxLevel += _carinfo.bodyStar*MaxLevel;
	
			if(_carinfo.bodyStar >=MaxStarLV) isMax = true;
			if(MaxLevel <= mlevel){
				nlevel = CONVERTER_DOLLAR;
				}else{
					nlevel = mlevel+1;
				}
			gameObject.name = tmepid+"_"+partid.ToString();//5001"; 
			lbText = Upgrade_Car_Ratio.Get(partid).Text;
		}break;
			
		case "Intake":
		{

			upLV = _carinfo.intakeLv;
			upID = 11000;
			upCar = AccountManager.instance.PART_INTAKE;
			upID += upLV;
			upID += (upCar-1)*25;
			upID += (upClass-1)*175;
			upID -= 1;
			coin = UpgradeCarCost.Get(upID).UpCoin;
			partid  = (int)CarPartID.Intake;
			//upratio = UserDataManager.instance.GetUpRatioViaStarLV((int)CarPartID.Intake, _carinfo.intakeStar);
			//levelupcoin = (_carstatus.IntakeLV1*upratio*0.01f);
			//levelupcoin = _carstatus.IntakeLV1+(_carinfo.intakeLv-1)*levelupcoin;
			//coin = (int)Mathf.Round(levelupcoin);
			dollar = coin*CONVERTER_DOLLAR;
			mlevel = _carinfo.intakeLv;
			MaxLevel += _carinfo.intakeStar*MaxLevel;
			if(_carinfo.intakeStar >= MaxStarLV) isMax = true;
			if(MaxLevel <= mlevel){
				nlevel = CONVERTER_DOLLAR;
			}else{
				nlevel = mlevel+1;
				
			}
			gameObject.name = tmepid+"_"+partid.ToString();//5001"; 
			lbText = Upgrade_Car_Ratio.Get(partid).Text;
			//gameObject.name =tmepid+"_5004"; 
			//lbText = Upgrade_Car_Ratio.Get(5004).Text;partid = 5004;
			//Utility.LogWarning(string.Format("{0} , {1} , {2} , {3} " , mlevel, MaxLevel, MaxStarLV, nlevel));
		}break;
		
		case "Tires":
		{
			upLV = _carinfo.tireLv;
			upID = 11000;
			upCar = AccountManager.instance.PART_TIRE;
			upID += upLV;
			upID += (upCar-1)*25;
			upID += (upClass-1)*175;
			upID -= 1;
			coin = UpgradeCarCost.Get(upID).UpCoin;
			partid  = (int)CarPartID.Tire;
		//	upratio = UserDataManager.instance.GetUpRatioViaStarLV((int)CarPartID.Tire, _carinfo.tireStar);
		//	levelupcoin = (_carstatus.TireLV1*upratio*0.01f);
		//	levelupcoin = _carstatus.TireLV1+(_carinfo.tireLv-1)*levelupcoin;
		//	coin = (int)Mathf.Round(levelupcoin);
			dollar = coin*CONVERTER_DOLLAR;
			mlevel = _carinfo.tireLv;
			MaxLevel += _carinfo.tireStar*MaxLevel;
			if(_carinfo.tireStar >= MaxStarLV) isMax = true;
			if(MaxLevel <= mlevel){
				nlevel = CONVERTER_DOLLAR;
			}else{
				nlevel = mlevel+1;
				
			}
			gameObject.name = tmepid+"_"+partid.ToString();//5001"; 
			lbText = Upgrade_Car_Ratio.Get(partid).Text;
			//gameObject.name =tmepid+"_5002"; 
			//lbText = Upgrade_Car_Ratio.Get(5002).Text;partid = 5002;
		}break;
		
		case "Gearbox":
		{
			upLV = _carinfo.gearBoxLv;
			upID = 11000;
			upCar = AccountManager.instance.PART_GEAR;
			upID += upLV;
			upID += (upCar-1)*25;
			upID += (upClass-1)*175;
			upID -= 1;
			coin = UpgradeCarCost.Get(upID).UpCoin;
			partid  = (int)CarPartID.Gearbox;
		//	upratio = UserDataManager.instance.GetUpRatioViaStarLV((int)CarPartID.Gearbox, _carinfo.gearBoxStar);
		//	levelupcoin =(_carstatus.GBoxLV1*upratio*0.01f);
		//	levelupcoin = _carstatus.GBoxLV1+(_carinfo.gearBoxLv-1)*levelupcoin;
		//	coin = (int)Mathf.Round(levelupcoin);
			dollar = coin*CONVERTER_DOLLAR;
			mlevel = _carinfo.gearBoxLv;
			MaxLevel += _carinfo.gearBoxStar*MaxLevel;
			if(_carinfo.gearBoxStar >= MaxStarLV) isMax = true;
			if(MaxLevel <= mlevel){
				nlevel = CONVERTER_DOLLAR;
			}else{
				nlevel = mlevel+1;
				
			}
			gameObject.name = tmepid+"_"+partid.ToString();//5001"; 
			lbText = Upgrade_Car_Ratio.Get(partid).Text;
			//gameObject.name =tmepid+"_5003"; 
			//lbText = Upgrade_Car_Ratio.Get(5003).Text;partid = 5003;
		}break;
		
		
		case "N2O":
		{
			upLV = _carinfo.bsPowerLv;
			upID = 11000;
			upCar =AccountManager.instance.PART_BOOSTPOWER;
			upID += upLV;
			upID += (upCar-1)*25;
			upID += (upClass-1)*175;
			upID -= 1;
			coin = UpgradeCarCost.Get(upID).UpCoin;
			partid  = (int)CarPartID.BsPower;
		//	upratio = UserDataManager.instance.GetUpRatioViaStarLV((int)CarPartID.BsPower, _carinfo.bsPowerStar);
		//	levelupcoin  = (_carstatus.BsPowerLV1*upratio*0.01f);
		//	levelupcoin = _carstatus.BsPowerLV1+(_carinfo.bsPowerLv-1)*levelupcoin;
		///	coin = (int)Mathf.Round(levelupcoin);
			dollar = coin*CONVERTER_DOLLAR;
			mlevel = _carinfo.bsPowerLv;
			MaxLevel += _carinfo.bsPowerStar*MaxLevel;
			if(_carinfo.bsPowerStar >= MaxStarLV) isMax = true;
			if(MaxLevel <= mlevel){
				nlevel = CONVERTER_DOLLAR;
			}else{
				nlevel = mlevel+1;
				
			}	gameObject.name = tmepid+"_"+partid.ToString();//5001"; 
			lbText = Upgrade_Car_Ratio.Get(partid).Text;
			//gameObject.name =tmepid+"_5005"; 
			//lbText = Upgrade_Car_Ratio.Get(5005).Text;partid = 5005;
		}break;
		case "Brake":
		{
			upLV = _carinfo.bsTimeLv;
			upID = 11000;
			upCar =AccountManager.instance.PART_BOOSTTIME;
			upID += upLV;
			upID += (upCar-1)*25;
			upID += (upClass-1)*175;
			upID -= 1;
			coin = UpgradeCarCost.Get(upID).UpCoin;
			partid  = (int)CarPartID.BsTime;
			//upratio = UserDataManager.instance.GetUpRatioViaStarLV((int)CarPartID.BsTime, _carinfo.bsTimeStar);
			//levelupcoin = (_carstatus.BsTimeLV1*upratio*0.01f);
			//levelupcoin = _carstatus.BsTimeLV1+(_carinfo.bsTimeLv-1)*levelupcoin;
			//coin = (int)Mathf.Round(levelupcoin);
			dollar = coin*CONVERTER_DOLLAR;
			mlevel = _carinfo.bsTimeLv;
			MaxLevel += _carinfo.bsTimeStar*MaxLevel;
			if(_carinfo.bsTimeStar >= MaxStarLV) isMax = true;
			if(MaxLevel <= mlevel){
				nlevel = CONVERTER_DOLLAR;
			}else{
				nlevel = mlevel+1;
			}
			gameObject.name = tmepid+"_"+partid.ToString();//5001"; 
			lbText = Upgrade_Car_Ratio.Get(partid).Text;
		//	gameObject.name =tmepid+"_5006"; 
		//	lbText = Upgrade_Car_Ratio.Get(5006).Text;partid = 5006;
		}break;
		default:{
			isUpdate = false;
		}
		break;
			
		}
	//	Utility.LogWarning("partid " + partid);

		var temp = gameObject.GetComponent<EvoInit>() as EvoInit;
		if(temp != null) temp.ChangeCardName();
		transform.FindChild("lbText").GetComponent<UILabel>().text 
			= lbText;
		CarPartIDStarUp(partid);
	}

	public void InitUpgradeCarInfo(string str){
	
		SelectedPartInfo(str);
		changeLevelWindow();
		
	}
	
	void changeLevelWindow(){
		var upgrade = transform.FindChild("Upgrade") as Transform;
		var mLevel = transform.FindChild("CurLevel") as Transform;
		var maxLevel = transform.FindChild("MaxLevel_Able") as Transform;
		var maxStar = transform.FindChild("MaxLevel") as Transform;
		if(nlevel != CONVERTER_DOLLAR){
			upgrade.gameObject.SetActive(true);
			mLevel.gameObject.SetActive(true);
			maxStar.gameObject.SetActive(false);
			maxLevel.gameObject.SetActive(false);
			upgrade.FindChild("lbCoin").GetComponent<UILabel>().text =System.String.Format("{0:#,0}", coin); 
			upgrade.FindChild("lbDollar").GetComponent<UILabel>().text =System.String.Format("{0:#,0}",  dollar);
			mLevel.FindChild("lbcurLevel").GetComponent<UILabel>().text = "Level "+mlevel.ToString();
			mLevel.FindChild("lbnextLevel").GetComponent<UILabel>().text = "Level "+nlevel.ToString();
		}else {
			upgrade.gameObject.SetActive(false);
			maxLevel.gameObject.SetActive(true);
			if(isMax) {
				maxStar.gameObject.SetActive(true);
				mLevel.gameObject.SetActive(false);
				maxLevel.GetComponentInChildren<UILabel>().text = 
					KoStorage.GetKorString("76414");
			}else{
				maxStar.gameObject.SetActive(false);
				mLevel.gameObject.SetActive(true);
				maxLevel.GetComponentInChildren<UILabel>().text = 
					KoStorage.GetKorString("76427");//TableManager.ko.dictionary["60188"].String;
				mLevel.FindChild("lbcurLevel").GetComponent<UILabel>().text = "Level "+mlevel.ToString();
				mLevel.FindChild("lbnextLevel").GetComponent<UILabel>().text = "Level "+(mlevel+1).ToString();
			}
			//upgrade.FindChild("lbCoin").GetComponent<UILabel>().text = "MaxLevel";
			//upgrade.FindChild("lbDollar").GetComponent<UILabel>().text ="Parts BuildUp";
			//upgrade.FindChild("btnDollar").gameObject.SetActive(false);
			//upgrade.FindChild("btnCoin").gameObject.SetActive(false);
			//mLevel.FindChild("lbcurLevel").GetComponent<UILabel>().text = "Level "+mlevel.ToString();
			//mLevel.FindChild("lbnextLevel").GetComponent<UILabel>().text = "Level Max";
		}
		//Utility.Log("car + " + mlevel); 
	}

	public void InitUpgradeCrewInfo(string str){
		SelectedCrewInfo(str);
		changeLevelWindow();
	}

	
	
	void SelectedCrewInfo(string str){

		int tempid = GV.getTeamCrewID(GV.SelectedTeamID);
		CrewInfo _crewinfo = GV.getTeamCrewInfo(GV.SelectedTeamID);
		Common_Crew_Status.Item _crewstatus =  Common_Crew_Status.Get (tempid);
		CONVERTER_DOLLAR = Global.gConvertDollar;
		int MaxStarLV = _crewstatus.StarLV;
		if(_crewstatus.ReqLV < 6){
			MaxStarLV = 10; // 
			isMax = true;
		}else{
			isMax = false;
		}
	//	float levelupcoin = 0;
		selectpartstring = str;
		string lbText = string.Empty;
		int MaxLevel = _crewstatus.UpLimit;
		int partid = 0;
		int upLV = 0;
		int upCrew = 0;
		int upTeam = 0;
		int upID=0;
		upTeam = Common_Team.Get(GV.SelectedTeamID).Team_x;

		switch(str){
		case "Driver":
		{
			upLV = _crewinfo.driverLv;
			upID = 10000;
			upCrew = AccountManager.instance.PART_DRIVER;
			upID += upLV;
			upID += (upCrew-1)*10;
			upID += (upTeam-1)*50;
			upID -= 1;
			partid = (int)CrewPartID.Driver;
			coin = UpgradeCrewCost.Get(upID).UpCoin;
		//	levelupcoin =(_crewstatus.DriverUpPr*Upgrade_Crew_Ratio.Get((int)CrewPartID.Driver).UpRatio*0.01f);
		//	levelupcoin = _crewstatus.DriverUpPr+(_crewinfo.driverLv-1)*levelupcoin;
		//	coin = (int)Mathf.Round(levelupcoin);
			dollar = coin*CONVERTER_DOLLAR;
			mlevel = _crewinfo.driverLv;
			MaxLevel += _crewinfo.driverStar;
			if(_crewinfo.driverStar>= MaxStarLV) isMax = true;
			if(MaxLevel <= mlevel){
				nlevel = CONVERTER_DOLLAR; 
			}else{
				nlevel = mlevel+1;
			}
			gameObject.name = tempid+ "_" + partid.ToString();
			lbText = Upgrade_Crew_Ratio.Get(partid).Text;
		}
			break;
		case "Gas":
		{
			upLV = _crewinfo.gasLv;
			upID = 10000;
			upCrew =  AccountManager.instance.PART_GASMAN;
			upID += upLV;
			upID += (upCrew-1)*10;
			upID += (upTeam-1)*50;
			upID -= 1;
			partid = (int)CrewPartID.GasMan;
			coin = UpgradeCrewCost.Get(upID).UpCoin;
		//	levelupcoin = (_crewstatus.GasUpPr*Upgrade_Crew_Ratio.Get((int)CrewPartID.GasMan).UpRatio*0.01f);
		//	levelupcoin = _crewstatus.GasUpPr+(_crewinfo.gasLv-1)*levelupcoin;
		//	coin = (int)Mathf.Round(levelupcoin);
			dollar = coin*CONVERTER_DOLLAR;
			mlevel = _crewinfo.gasLv;
			MaxLevel += _crewinfo.gasStar;
			if(_crewinfo.gasStar>= MaxStarLV) isMax = true;
			if(MaxLevel <= mlevel){
				nlevel = CONVERTER_DOLLAR;
			}else{
				nlevel = mlevel+1;
				
			}
			gameObject.name = tempid+ "_" + partid.ToString();
			lbText = Upgrade_Crew_Ratio.Get(partid).Text;
		}
			break;
		case "Chief":
		{

			upLV = _crewinfo.chiefLv;
			upID = 10000;
			upCrew =  AccountManager.instance.PART_CHIEF;
			upID += upLV;
			upID += (upCrew-1)*10;
			upID += (upTeam-1)*50;
			upID -= 1;
			partid = (int)CrewPartID.Chief;
			coin = UpgradeCrewCost.Get(upID).UpCoin;
			//levelupcoin = (_crewstatus.ChiefUpPr*Upgrade_Crew_Ratio.Get((int)CrewPartID.Chief).UpRatio*0.01f);
			//levelupcoin = _crewstatus.ChiefUpPr+(_crewinfo.chiefLv-1)*levelupcoin;
			//coin = (int)Mathf.Round(levelupcoin);
			dollar = coin*CONVERTER_DOLLAR;
			mlevel = _crewinfo.chiefLv;
			MaxLevel += _crewinfo.chiefStar;
			if(_crewinfo.chiefStar>= MaxStarLV) isMax = true;
			if(MaxLevel <= mlevel){
			
				nlevel = CONVERTER_DOLLAR;
			}else{
				nlevel = mlevel+1;
				
			}
			gameObject.name = tempid+ "_" + partid.ToString();
			lbText = Upgrade_Crew_Ratio.Get(partid).Text;
		}
			break;
		case "Jack":
		{
			upLV = _crewinfo.jackLv;
			upID = 10000;
			upCrew =  AccountManager.instance.PART_JACKMAN;
			upID += upLV;
			upID += (upCrew-1)*10;
			upID += (upTeam-1)*50;
			upID -= 1;
			partid = (int)CrewPartID.JackMan;
			coin = UpgradeCrewCost.Get(upID).UpCoin;
		//	levelupcoin = (_crewstatus.JackUpPr*Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).UpRatio*0.01f);
		//	levelupcoin = _crewstatus.JackUpPr+(_crewinfo.jackLv-1)*levelupcoin;
		//	coin = (int)Mathf.Round(levelupcoin);
			//coin = _crewstatus.JackUpPr;
			dollar = coin*CONVERTER_DOLLAR;
			mlevel = _crewinfo.jackLv;
			MaxLevel += _crewinfo.jackStar;
			if(_crewinfo.jackStar>= MaxStarLV) isMax = true;
			if(MaxLevel <= mlevel){
				nlevel = CONVERTER_DOLLAR;
			}else{
				nlevel = mlevel+1;
				
			}
			gameObject.name = tempid+ "_" + partid.ToString();
			lbText = Upgrade_Crew_Ratio.Get(partid).Text;
		}
			break;
		case "Tire":
		{

			upLV = _crewinfo.tireLv;
			upID = 10000;
			upCrew =  AccountManager.instance.PART_TIREMAN;
			upID += upLV;
			upID += (upCrew-1)*10;
			upID += (upTeam-1)*50;
			upID -= 1;
			partid = (int)CrewPartID.Tire;
			coin = UpgradeCrewCost.Get(upID).UpCoin;

			//levelupcoin = (_crewstatus.TireUpPr*Upgrade_Crew_Ratio.Get((int)CrewPartID.Tire).UpRatio*0.01f);
			//levelupcoin = _crewstatus.TireUpPr+(_crewinfo.tireLv-1)*levelupcoin;

			//coin = (int)Mathf.Round(levelupcoin);
			//coin = _crewstatus.TireUpPr;
			dollar = coin*CONVERTER_DOLLAR;
			mlevel = _crewinfo.tireLv;
			MaxLevel += _crewinfo.tireStar;
			if(_crewinfo.tireStar>= MaxStarLV) isMax = true;
			if(MaxLevel <= mlevel){
				nlevel = CONVERTER_DOLLAR;
			}else{
				nlevel = mlevel+1;
				
			}
			gameObject.name = tempid+ "_" + partid.ToString();
			lbText = Upgrade_Crew_Ratio.Get(partid).Text;
		}
			break;
		default:
			isUpdate = false;
			//Utility.LogWarning("SwitchError");
			break;
		}
	//	Utility.LogWarning("partID " + partid);

		var temp = gameObject.GetComponent<EvoInit>() as EvoInit;
		if(temp != null) temp.ChangeCardName();
		transform.FindChild("lbText").GetComponent<UILabel>().text 
			= lbText;
		//CrewPartStarUp(partid);
	}

	void OnDestroy(){
		System.GC.Collect();
	}
	
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

	void OnEvoCarClick(){
		GameObject.Find("LobbyUI").SendMessage("OnUpCarStar");
	}

	void OnEvoCrewClick(){
		//GameObject.Find("LobbyUI").SendMessage("OnUpCrewStar");
	}


	void CrewPartStarUp(int partID){
		int star = 0;
		int tmepid = 0;
		CrewInfo _crew ; 
	//	string upname = string.Empty;
	//	if(GV.SelectedTeamCode==0){
	//		tmepid = GV.FindStockCrew();
	//		_crew = GV.GetStockCrewInfo();
	//	}
	//	else {
	//		tmepid = GV.FindTourCrew();
	//		_crew = GV.GetTourCrewInfo();
	//	}
		tmepid = GV.getTeamCrewID(GV.SelectedTeamID);
		_crew = GV.getTeamCrewInfo(GV.SelectedTeamID);
		Common_Crew_Status.Item item = Common_Crew_Status.Get(tmepid);
		switch(partID){
		case 5102: star = _crew.chiefStar;break;
		case 5101: star = _crew.tireStar;break;
		case 5104: star = _crew.gasStar;break;
		case 5100: star = _crew.driverStar;break;
		case 5103: star = _crew.jackStar;break;
		}
		//Common_Crew_Status.Item item = Common_Crew_Status.Get(tmepid);
		int starLv = item.StarLV;
		var count = transform.FindChild("StarCount") as Transform;
	//	var lbStar =transform.FindChild("lbStarLV_no").GetComponent<UILabel>() as UILabel;
		if(item.ReqLV < 0){
			count.gameObject.SetActive(false);
	//		lbStar.text = KoStorage.getStringDic("60278");
			return;
		}else{
			count.gameObject.SetActive(true);
	//		lbStar.text = string.Empty;
		}
		for(int i = 0; i < starLv; i++){
			if( i < star){
				count.GetChild(i).FindChild("Star_on").gameObject.SetActive(true);
			}else{

				count.GetChild(i).FindChild("Star_on").gameObject.SetActive(false);
			}
			count.GetChild(i).gameObject.SetActive(true);
		}
		for(int i = starLv; i < 5; i++){
			count.GetChild(i).gameObject.SetActive(false);
		}
		var temp  = transform.FindChild("Upgrade").FindChild("btnEvo") as Transform;
		if(star == starLv){
			temp.GetComponent<UIButtonMessage>().functionName =null;
		}else{
			temp.GetComponent<UIButtonMessage>().functionName = "OnEvoCrewClick";
		}
	}
	
	void CarPartIDStarUp(int partID){
		int star = 0;
		int tmepid = 0;
		CarInfo _car ;
	//	int starcnt = 0;
		int partLV = 0;
		tmepid = GV.getTeamCarID(GV.SelectedTeamID);
		_car = GV.getTeamCarInfo(GV.SelectedTeamID);
		Common_Car_Status.Item item = Common_Car_Status.Get(tmepid);
		switch(partID){
		case 5000: star = _car.bodyStar; partLV = _car.bodyLv; break;
		case 5001: star = _car.engineStar;partLV = _car.engineLv; break;
		case 5002: star = _car.tireStar;partLV = _car.tireLv; break;
		case 5003: star = _car.gearBoxStar;partLV = _car.gearBoxLv; break;
		case 5004: star = _car.intakeStar;partLV = _car.intakeLv; break;
		case 5005: star = _car.bsPowerStar;partLV = _car.bsPowerLv; break;
		case 5006: star = _car.bsTimeStar;partLV = _car.bsTimeLv; break;//brake
		}
		//Utility.LogWarning("StarLV");
			star += 1;
		//Common_Car_Status.Item item = Common_Car_Status.Get(tmepid);
			int starLv = _car.carClass.StarLV;
			var count = transform.FindChild("StarCount") as Transform;
		//	var lbStar =transform.FindChild("lbStarLV_no").GetComponent<UILabel>() as UILabel;
			if(item.ReqLV < 0){
			count.gameObject.SetActive(false);
			//lbStar.text = KoStorage.getStringDic("60278");
			return;
		}else{
			count.gameObject.SetActive(true);
		//	lbStar.text = string.Empty;
		}
		for(int i = 0; i < starLv; i++){
			if( i < star){
				count.GetChild(i).FindChild("Star_on").gameObject.SetActive(true);
			}else{
				count.GetChild(i).FindChild("Star_on").gameObject.SetActive(false);
			}
		}
		for(int i = 0; i < starLv; i++){
			count.GetChild(i).gameObject.SetActive(true);
		}
		for(int i = starLv; i < 5; i++){
			count.GetChild(i).gameObject.SetActive(false);
		}

		var temp  = transform.FindChild("btnEvo") as Transform;
		var temp1  = transform.FindChild("btnEvo_lock") as Transform;
		//Utility.LogWarning(string.Format("Star {0} , StarLV {1}", star, starLv));
		if(star == starLv){
			temp.GetComponent<UIButtonMessage>().functionName =null;
			temp.gameObject.SetActive(false);
			temp1.gameObject.SetActive(true);
			temp1.GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("76428");// "더이상 업성?";
			//temp1.transform.FindChild("lbName").GetComponent<UILabel>().text =  KoStorage.GetKorString("76402");
		}else{
			if(partLV%5 == 0){
				//	temp.GetComponent<UIButtonMessage>().functionName = "OnEvoCarClick";
				//	temp.gameObject.SetActive(true);temp1.gameObject.SetActive(false);
				int a = partLV / 5;
				if(a == star){
					temp.GetComponent<UIButtonMessage>().functionName = "OnEvoCarClick";
					temp.gameObject.SetActive(true);temp1.gameObject.SetActive(false);
				}else{
					temp.GetComponent<UIButtonMessage>().functionName =null;
					temp1.gameObject.SetActive(true);temp.gameObject.SetActive(false);
					temp1.GetComponentInChildren<UILabel>().text =KoStorage.GetKorString("76402");
				}
			}
			else{
				temp.GetComponent<UIButtonMessage>().functionName =null;
				temp1.gameObject.SetActive(true);temp.gameObject.SetActive(false);
				temp1.GetComponentInChildren<UILabel>().text =KoStorage.GetKorString("76402");
			}

		}
	}
	 

}


