using UnityEngine;
using System.Collections;

public class CreateUpgradePopUp : MonoBehaviour {

	GameObject Pop;
	void OnEnable(){
		Pop = ObjectManager.SearchWindowPopup() as GameObject;
		var popchild = Pop.transform.FindChild("Content_BUY").gameObject as GameObject;
		popchild.SetActive(true);
	//	Global.isPopUp = true;
	}
	
	System.Action<bool> callback;
	public void OnResponse(System.Action<bool> callback){
		this.callback=callback;
	}
	
	void OnDisable(){
		
	}
	
	/*
	public void UpgradeMat(int coin){
		var popChild = Pop.transform.FindChild("Content_BUY") as Transform;
		string price = string.Empty;
		string[] name = gameObject.name.Split("_"[0]);
		int id = int.Parse(name[1]);
		int level = 0;
		if(coin == 0){
			price = "0";
			Pop.name = gameObject.name + "_"+ price;
			popChild.gameObject.SetActive(false);
		}else{
			price = coin.ToString();
			popChild.FindChild("btnCoin").gameObject.SetActive(true);
			popChild.FindChild("icon_product").gameObject.SetActive(false);//GetComponent<UISprite>().spriteName  =name[1]+"D";
			popChild.FindChild("lbPrice").GetComponent<UILabel>().text = price;
			Pop.name = gameObject.name + "_"+ price;
			price = Upgrade_Car_Ratio.Get(id).Text2;
			level = CurrentCarPartLevel(name[1]);
			popChild.transform.FindChild("lbName").gameObject.SetActive(true);
			popChild.FindChild("lbName").GetComponent<UILabel>().text = price;
			//price = string.Format(KoStorage.getStringDic("60069") ,level);
			price = string.Format("{0}, 레벨로???" ,level);
			popChild.FindChild("lbText").GetComponent<UILabel>().text = price;
		}
		Pop.gameObject.AddComponent<LevelUpBuy>().onRespons = ((isSuccess)=>{
			callback(isSuccess);
		});
		if(coin == 0)
			Pop.gameObject.SendMessage("OnOkClick");
		Destroy(this);
	}*/
	
	void Start(){
		var popChild = Pop.transform.FindChild("Content_BUY") as Transform;
		string price = string.Empty;
		
		string[] name = gameObject.name.Split("_"[0]);
		if(string.Equals(name[2], "coin"))	{
			popChild.FindChild("btnCoin").gameObject.SetActive(true);
			price = transform.FindChild("Upgrade").FindChild("lbCoin").GetComponent<UILabel>().text;
		}else if(string.Equals(name[2], "dollar")){
			popChild.FindChild("btnDollar").gameObject.SetActive(true);
			price = transform.FindChild("Upgrade").FindChild("lbDollar").GetComponent<UILabel>().text;
		}else{
			return;
		}
		
		popChild.FindChild("icon_product").gameObject.SetActive(false);//GetComponent<UISprite>().spriteName  =name[1]+"D";
		popChild.FindChild("lbPrice").GetComponent<UILabel>().text = price;
		Pop.name = gameObject.name + "_"+ price;
		int id = int.Parse(name[1]);
		int level = 0;
		string pName = string.Empty;
		if(id < 5099){
			price = Upgrade_Car_Ratio.Get(id).Text2;
			level = CurrentCarPartLevel(name[1]);
			pName = CurrentCarPartName(name[1]);
		}else{
			price = Upgrade_Crew_Ratio.Get(id).Text2;
			level = CurrentCrewPartLevel(name[1]);
			pName = CurrentCrewPartName(name[1]);
		}

		popChild.transform.FindChild("lbName").gameObject.SetActive(true);
		popChild.FindChild("lbName").GetComponent<UILabel>().text = price;
		//price = string.Format(KoStorage.getStringDic("60069") ,level);
		price = string.Format(KoStorage.GetKorString("76203"),pName  ,level);
		popChild.FindChild("lbText").GetComponent<UILabel>().text = price;
		
		Pop.gameObject.AddComponent<TryLevelUp>().onRespons = ((isSuccess)=>{
			callback(isSuccess);
		});
		Pop.GetComponent<TryLevelUp>().RegistorBack();
		Destroy(this);
	}
	
	int CurrentCarPartLevel(string id){
		int level = 0;
		CarInfo car;int idx = 0;
		idx = GV.getTeamCarID(GV.SelectedTeamID);
		car = GV.getTeamCarInfo(GV.SelectedTeamID);
		switch(id){
		case "5000": // body
			level = car.bodyLv;
			break;
		case "5001": //engine
			level = car.engineLv;
			break;
		case "5002": //tires
			level = car.tireLv;
			break;
		case "5003": //gear
			level = car.gearBoxLv;
			break;
		case"5004": //intake
				level = car.intakeLv;
			break;
		case "5005": //n2 power
			level = car.bsPowerLv;
			break;
		case "5006": //n2 time
			level = car.bsTimeLv;
			break;
		}
		return (level+1);
	}

	string CurrentCarPartName(string id){
		string level = null;
		switch(id){
		case "5000": // body
			level = KoStorage.GetKorString("76309");// car.bodyLv;
			break;
		case "5001": //engine
			level = KoStorage.GetKorString("76310");//level = car.engineLv;
			break;
		case "5002": //tires
			level = KoStorage.GetKorString("76311");//level = car.tireLv;
			break;
		case "5003": //gear
			level = KoStorage.GetKorString("76018");//	level = car.gearBoxLv;
			break;
		case"5004": //intake
				level = KoStorage.GetKorString("76313");//	level = car.intakeLv;
			break;
		case "5005": //n2 power
			level = KoStorage.GetKorString("76019");//level = car.bsPowerLv;
			break;
		case "5006": //n2 time
			level = KoStorage.GetKorString("76315");//level = car.bsTimeLv;
			break;
		}
		return (level);
	}
	int CurrentCrewPartLevel(string id){
		int idx = 0;
		CrewInfo crew ; 
		idx = GV.getTeamCrewID(GV.SelectedTeamID);
		crew = GV.getTeamCrewInfo(GV.SelectedTeamID);
		int level = 0;
		switch(id){
		case "5100": // driver
			level = crew.driverLv;
			break;
		case "5101": //tire
			level = crew.tireLv;
			break;
		case "5102": //chief
			level = crew.chiefLv;
			break;
		case "5103": //jack
			level = crew.jackLv;
			break;
		case"5104": //gas
			level = crew.gasLv;
			break;
		}
		return (level+1);
	}
	string CurrentCrewPartName(string id){
		string level = null;
		switch(id){
		case "5100": // driver
			level = KoStorage.GetKorString("76213");//
			break;
		case "5101": //tire
			level = KoStorage.GetKorString("76214");//
			break;
		case "5102": //chief
			level = KoStorage.GetKorString("76215");
			break;
		case "5103": //jack
			level = KoStorage.GetKorString("76216");//l
			break;
		case"5104": //gas
				level = KoStorage.GetKorString("76217");//
			break;
		}
		return (level);
	}
}
