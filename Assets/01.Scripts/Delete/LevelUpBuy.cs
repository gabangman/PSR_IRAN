using UnityEngine;
using System.Collections;

public class LevelUpBuy : MonoBehaviour {
	/*
	public delegate void ResponseServer(bool isSucess);
	public ResponseServer onRespons;

	void OnEnable(){
	
	}

	void OnOkClick(){
	if(Global.isNetwork) return;
		string[] _send = gameObject.name.Split("_"[0]);
		Global.isNetwork = true;
		int index = int.Parse(_send[0]);
		string type = string.Empty;

		if(string.Equals(_send[2], "Coin")){
			//buyType = 1;
			type = "0";
		}else {
			//buyType = 2;
			type = "1";
		}

	//	if(!int.TryParse(_send[3], out buyMoney))
	//		buyMoney = int.Parse(_send[3].Replace(",",string.Empty));
		
		int tempid = 0;
		if(index < 1199){ //car
		//	int _uType = UserDataManager.instance.KindOfCarUpgrade(_send[1]);
			upgradeKind = _send[1];
		//	if(_uType == 0) Utility.LogError("car zero");
			tempid = GV.getTeamCarID(GV.SelectedTeamID);// Base64Manager.instance.GlobalEncoding(Global.MyCarID);
			//Utility.Log(string.Format("{0} is {1}", Global.MyCarID, upgradeKind));	
			ProtocolManager.instance.addServerDataField("nCarId",tempid.ToString());
			ProtocolManager.instance.addServerDataField("nPartId",upgradeKind);
			//Utility.Log("Car Parts : " + upgradeKind);
			ProtocolManager.instance.addServerDataField("nCoinOrDollar",type);
			string strUrl = ServerStringKeys.API.upgrade;
			ProtocolManager.instance.ConnectServer(strUrl,responseUpgradeCar);

		}else{ //crew
		//	int _cwType = UserDataManager.instance.KindOfCrewUpgrade(_send[1]);
			upgradeKind = _send[1];
		//	if(_cwType == 0) Utility.LogError("crew zero");
			tempid = GV.getTeamCrewID(GV.SelectedTeamID);//Base64Manager.instance.GlobalEncoding(Global.MyCrewID);
			///Utility.Log("nCrewID " + upgradeKind);
			//Utility.Log("nCoinOrDollar " + type);
			ProtocolManager.instance.addServerDataField("nCrewId",tempid.ToString());
			ProtocolManager.instance.addServerDataField("nPartId",upgradeKind);
			ProtocolManager.instance.addServerDataField("nCoinOrDollar",type);
			//Utility.Log("Crew Parts : " + upgradeKind);
			//Utility.Log(string.Format("{0}, {1} ,{2} "  ,Global.MyCrewID, upgradeKind, type));
			string strUrl = ServerStringKeys.API.upgradeCrew;
			ProtocolManager.instance.ConnectServer(strUrl,responseUpgradeCrew);
		}
		return;
	}


	//int buyType;
	//int buyMoney;
	string upgradeKind;
	

	void responseUpgradeCrew(System.Uri uri){
		bool isSuccess = false;
		int nret = ProtocolManager.instance.GetIntUriQuery(uri,"nRet");
		if(nret == 1){
			isSuccess = true;
			upgradeCrewLevelUp();
			ProtocolManager.instance.updateMyCoin(uri);
			ProtocolManager.instance.updateMyDollar(uri);
			//ResponseBuy(isSuccess);
		}else if(nret == 2){
			//no userID
		}else if(nret == 3){
			//no car
		}else if(nret == 4){

	
		}else if(nret == -1){

		}
		Global.isNetwork = false;
		ResponseBuy(isSuccess);
	}

	void responseUpgradeCar(System.Uri uri){
		bool isSuccess = false;
		int nret = ProtocolManager.instance.GetIntUriQuery(uri,"nRet");
		if(nret == 1){
			isSuccess = true;
			upgradeCarLevelUp();
			ProtocolManager.instance.updateMyCoin(uri);
			ProtocolManager.instance.updateMyDollar(uri);
			ResponseBuy(isSuccess);
		}else if(nret == 2){
			//no userID
		}else if(nret == 3){
			//no car
		}else if(nret == 4){
			isSuccess = false;
			ResponseBuy(isSuccess);
		}
		Global.isNetwork = false;
	}

	void upgradeCrewLevelUp(){
		int tmepid =GV.getTeamCrewID(GV.SelectedTeamID);// Base64Manager.instance.GlobalEncoding(Global.MyCrewID);
		Account.CrewInfo crew = myAccount.instance.GetCrewInfo(tmepid);



		switch(upgradeKind){
		case "5100": // driver
			crew.driverLv++;
			break;
		case "5101": //tire
			crew.tireLv++;
			break;
		case "5102": //chief
			crew.chiefLv++;
			break;
		case "5103": //jack
			crew.jackLv++;
			break;
		case"5104": //gas
			crew.gasLv++;
				break;
		}
		myAccount.instance.updateCrewPart(crew);
	}

	void upgradeCarLevelUp(){
		int tmpid =GV.getTeamCarID(GV.SelectedTeamID);// Base64Manager.instance.GlobalEncoding(Global.MyCarID);
		Account.CarInfo car = myAccount.instance.GetCarInfo(tmpid);
		switch(upgradeKind){
		case "5000": // body
			car.bodyLv++;
			break;
		case "5001": //engine
			car.engineLv++;
			break;
		case "5002": //tires
			car.tireLv++;
			break;
		case "5003": //gear
			car.gearBoxLv++;
			break;
		case"5004": //intake
			car.intakeLv++;
			break;
		case "5005": //n2 power
			car.bsPowerLv++;
			break;
		case "5006": //n2 time
			car.bsTimeLv++;
			break;
		}
		myAccount.instance.updateCarPart(car);
	}

	void ResponseBuy(bool isSuccess){
		var child = transform.FindChild("Content_BUY") as Transform;
		child.gameObject.SetActive(false);
		child.FindChild("btnCoin").gameObject.SetActive(false);
		string[] _name = gameObject.name.Split("_"[0]);
		GameObject _parent = null;

		if(isSuccess){
			//Utility.LogWarning("Successed");
			//UserDataManager.instance.SettingUserInfo();
			OnCloseClick();
			//_parent = gameObject.transform.FindChild("Content_Success").gameObject;
			//gameObject.GetComponent<TweenAction>().doubleTweenScale(_parent);
			//_parent.SetActive(true);
			//_parent.transform.FindChild("icon_product").GetComponent<UISprite>().spriteName = _name[0];
		}else{
			_parent = gameObject.transform.FindChild("Content_Fail").gameObject;
			gameObject.GetComponent<TweenAction>().doubleTweenScale(_parent);
			_parent.SetActive(true);
			if(string.Equals(_name[2], "Coin") == true) _name[2] = "icon_coin";
			else _name[2]  = "icon_dollar";
			_parent.transform.FindChild("icon_product").gameObject.SetActive(false);
			//_parent.transform.FindChild("icon_product").GetComponent<UISprite>().spriteName = _name[2];
			_parent.transform.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
			_parent.transform.FindChild("lbText").GetComponent<UILabel>().text =
				KoStorage.GetKorString("76022");//TableManager.ko.dictionary["60086"].String;
			_parent.transform.FindChild("lbName").GetComponent<UILabel>().text =
				KoStorage.GetKorString("76023");//TableManager.ko.dictionary["60087"].String;
			_parent.transform.FindChild("btnok").GetComponentInChildren<UILabel>().text = 
				KoStorage.GetKorString("71000");//TableManager.ko.dictionary["60184"].String;
		}
		onRespons(isSuccess);
	}

	void OnCloseClick(){
		Global.isPopUp = false;
		onRespons(false);
		var child = transform.FindChild("Content_Fail") as Transform;
		child.gameObject.SetActive(false);
		child.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		transform.FindChild("Content_Success").gameObject.SetActive(false);
		child = transform.FindChild("Content_BUY") as Transform;
		child.gameObject.SetActive(true);
		//child.FindChild("btnCoin").gameObject.SetActive(false);
		//child.FindChild("btnDollar").gameObject.SetActive(false);
		//child.FindChild("btnok").gameObject.SetActive(false);
		Destroy(this);
		gameObject.SetActive(false);
	}

	void OnFailClick(){
		// 상점으로 이동
		GameObject.Find("LobbyUI").SendMessage("OnDollarClick", SendMessageOptions.DontRequireReceiver);
		OnCloseClick();
	}

	void OnSuccessClick(){
		OnCloseClick();
	}
}
	/*
	int CheckCrewUpgradeType(string partname){
		//crew kind : 1=chief, 2=jack, 3=driver, 4=tire, 5=gas
		switch(partname){
		case "5100":
			return 3;
		case "5104":
			return 5;
		case "5102":
			return 1;
		case "5103":
			return 2;
		case "5101":
			return 4;
		default:
			return 0;
		}
		switch(partname){
		case "5100":
			return CrewUpgradeType.Driver;
		case "5104":
			return CrewUpgradeType.Gas;
		case "5102":
			return CrewUpgradeType.Chief;
		case "5103":
			return CrewUpgradeType.Jack;
		case "5101":
			return CrewUpgradeType.Tire;
		default:
			return CrewUpgradeType.Tire;
		}
		
	}*/
/*
	int CheckCarUpgradeType(string partname){
		//parts kind : 1=body, 2=engine, 3=tier, 4=gear, 5=intake, 6=bsPower, 7=bsTime
		switch(partname){
		case "5001":
			return 2;
		case "5000":
			return 1;
		case "5004":
			return 5;
		case "5002":
			return 3;
		case "5003":
			return 4;
		case "5005":
			return 6;
		case "5006":
			return 7;
		default:
			return 0;
		}

		switch(partname){
		case "5001":
			return CarUpgradeType.Engine;
		case "5000":
			return CarUpgradeType.Body;
		case "5004":
			return CarUpgradeType.Intake;
		case "5002":
			return CarUpgradeType.Tire;
		case "5003":
			return CarUpgradeType.GearBox;
		case "5005":
			return CarUpgradeType.BsPower;
		case "5006":
			return CarUpgradeType.BsTime;
		default:
			return CarUpgradeType.Tire;
		}
	}*/
}
