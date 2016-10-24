using UnityEngine;
using System.Collections;

public class newButtonAction : MonoBehaviour {

	void OnEnable(){
		string menuName = gameObject.name;
		switch(menuName){
		case "Menu_Lobby":
			LobbybuttonIcon();
			break;
		case "Menu_Map":
			//mapNewIcon();
			break;
		default:
			break;
		}
	}
	void ResetRepairBtn(){
		var tr = transform.FindChild("BtnRepair") as Transform;
	//	int carID = GV.getTeamCarID(GV.SelectedTeamID);
		CarInfo mCar = GV.getTeamCarInfo(GV.SelectedTeamID);
		int pDura = mCar.carClass.Durability; 
		int CarDura = mCar.carClass.DurabilityRef;
		float diff1 = (float)pDura / (float)CarDura;
		int mCnt = (int)(diff1*100);
		if( mCnt <= 20){
			tr.FindChild("img_Alert").gameObject.SetActive(true);
		}else tr.FindChild("img_Alert").gameObject.SetActive(false);

		tr  = tr.FindChild("bg_gage");
		if(mCnt <= 0){
			tr.gameObject.SetActive(false);
			tr.GetComponent<UISprite>().fillAmount = 0.0f;
		}else{
			if(!tr.gameObject.activeSelf)tr.gameObject.SetActive(true);
			//tr.localScale = new Vector3(165.3f * mCnt * 0.01f,44.03f,1);
			tr.GetComponent<UISprite>().fillAmount = (float)mCnt*0.01f;
		}
	}
	void Awake(){
		string menuName = gameObject.name;
		switch(menuName){
		case "Menu_Lobby":
			var tr = transform.FindChild("BtnRepair") as Transform;
			tr.FindChild("bg_gage").GetComponent<UISprite>().pivot = UIWidget.Pivot.Left;
			if(GV.ChSeasonID == 6000){
				transform.FindChild("Arrow").gameObject.SetActive(true);
			}
			break;
		case "Menu_Map":
			break;
		default:
			break;
		}

	}
	void ShowRepairLable(bool b){
		var tr = transform.FindChild("BtnRepair") as Transform;
		tr.gameObject.SetActive(b);
	}



	void LobbybuttonIcon(){
		//Utility.LogWarning("LobbyButton");
		//if(GV.ChSeason == 3)   myAcc.instance.account.bLobbyBTN[0] = true;
		//else myAcc.instance.account.bLobbyBTN[0] = false;

		ClanNewButton(myAcc.instance.account.bLobbyBTN[0]);
		if(GV.ChSeason >= 3){
			if(CClub.ClanMember == 0)
			{ transform.FindChild("CLAN").FindChild("icon_Club").gameObject.SetActive(false);
				transform.FindChild("CLAN").FindChild("Sprite (icon)").gameObject.SetActive(true);
			}
			 else{
				transform.FindChild("CLAN").FindChild("icon_Club").gameObject.SetActive(true);
				transform.FindChild("CLAN").FindChild("Sprite (icon)").gameObject.SetActive(false);
				transform.FindChild("CLAN").FindChild("icon_Club").FindChild("img_Club").GetComponent<UISprite>().spriteName = CClub.mClubInfo.clubSymbol;
			}
			}
		    
		MYTEAMNewButton(myAcc.instance.account.bLobbyBTN[1]);
		INVENTORYNewButton(myAcc.instance.account.bLobbyBTN[2]);
	//	if(GV.gDealerCarID == 0){
	//		myAcc.instance.account.bLobbyBTN[4] = false;
	//	}else{
	//		if(GV.gBuyDealerCar == 1) myAcc.instance.account.bLobbyBTN[4] = true;
	//		else myAcc.instance.account.bLobbyBTN[4] = false;
	//	}
		int a =  EncryptedPlayerPrefs.GetInt("DealerBuy");
		if(a != 10){
			//int a = EncryptedPlayerPrefs.GetInt("DealerBuy");
			if(a == 0){
				myAcc.instance.account.bLobbyBTN[4] = false;
			}else if(a == 1){
				myAcc.instance.account.bLobbyBTN[4] = true;
			}else if(a == 2){	// 구매 했다.
				myAcc.instance.account.bLobbyBTN[4] = false;
			}
		}else{
			myAcc.instance.account.bLobbyBTN[4] = false;
		}

		SHOPNewButton(myAcc.instance.account.bLobbyBTN[4]);
		NEXTNewButton(myAcc.instance.account.bLobbyBTN[5]);
	}

	void ClanNewButton(bool b){
		transform.FindChild("CLAN").FindChild("icon_New").gameObject.SetActive(b);
	}
	void MYTEAMNewButton(bool b){
		transform.FindChild("MYTEAM").FindChild("icon_New").gameObject.SetActive(b);
		if(GV.getTeamSponID(GV.SelectedTeamID) == 1300){
			transform.FindChild("MYTEAM").FindChild("icon_Contract").gameObject.SetActive(false);
		}else{
			transform.FindChild("MYTEAM").FindChild("icon_Contract").gameObject.SetActive(true);
		}
	}
	void INVENTORYNewButton(bool b){
		transform.FindChild("INVENTORY").FindChild("icon_New").gameObject.SetActive(b);
	}
	void SHOPNewButton(bool b){
		transform.FindChild("SHOP").FindChild("icon_New").gameObject.SetActive(b);
	}
	void NEXTNewButton(bool b){
		transform.FindChild("NEXT").FindChild("icon_New").gameObject.SetActive(b);
	}

	void lobbyNewIcon(){
	/*	bool isShopCarNew = myAccount.instance.account.buttonStatus.isShopCarNew;
		bool isShopCrewNew = myAccount.instance.account.buttonStatus.isShopCrewNew;
		bool isCheckShop = isShopCarNew || isShopCrewNew;
		//bool isNew = myAccount.instance.account.buttonStatus.isShopNew;
		transform.FindChild("SHOP").FindChild("icon_New").gameObject.SetActive(isCheckShop);
		//isNew = myAccount.instance.account.buttonStatus.isSponNew;
		bool isNew = false;
		//transform.FindChild("SPONSOR").FindChild("icon_New").gameObject.SetActive(isNew);
		int tempid = Base64Manager.instance.GlobalEncoding(Global.gSale);
		if(tempid == 10){
			transform.FindChild("SHOP").FindChild("icon_Event").gameObject.SetActive(true);
		}else{
			//transform.FindChild("SHOP").FindChild("icon_Event").gameObject.SetActive(false);
		}
		isNew = RaceNewCheck();
		//isNew = myAccount.instance.account.buttonStatus.isMapNew;
		transform.FindChild("NEXT").FindChild("icon_New").gameObject.SetActive(isNew);

		isNew = myAccount.instance.account.buttonStatus.TeamNew;
		transform.FindChild("MYTEAM").FindChild("icon_New").gameObject.SetActive(isNew);
		*/
	}



	public void OpenNewButtonEvent(){
		//Utility.Log("onnewButtonEvent");
		//bool isNew =  myAccount.instance.account.buttonStatus.isFeatureNew;
		//if(isNew) {
		//	myAccount.instance.account.buttonStatus.isFeatureNew = false;
		//	myAccount.instance.account.buttonStatus.isMapNew = false;
		//}
		
		//else return;
		//isNew = false;
		//var tr = transform.FindChild("Btn_Mode") as Transform;
		//tr.FindChild("Event").FindChild("icon_New").gameObject.SetActive(isNew);
	}
	
	public void OpenNewButtonRank(){
		/*bool isNew =  myAccount.instance.account.buttonStatus.isWeeklyNew;
		if(isNew) {
			myAccount.instance.account.buttonStatus.isWeeklyNew = false;
			myAccount.instance.account.buttonStatus.isMapNew = false;
		}
		else return;
		isNew = false;
		var tr = transform.FindChild("Btn_Mode") as Transform;
		tr.FindChild("Event").FindChild("icon_New").gameObject.SetActive(isNew);*/
	}
	public void setCarButton(){
		Utility.LogWarning("SetCarButton-no");
		/*var tr = transform.FindChild("View").FindChild("Grid") as Transform;
		int count = myAccount.instance.account.listCarInfo.Count;
		int cnt = tr.childCount;
		string str = string.Empty;
		int id = 0;
		for(int j = 0; j < count; j++){
			 id = myAccount.instance.account.listCarInfo[j].carId;
			str = id.ToString()+"_Car";
			for(int i = 0; i <cnt; i++){
				if(tr.GetChild(i).name == str){
					var temp = tr.GetChild(i).GetChild(0).FindChild("myOne") as Transform;
					temp.gameObject.SetActive(true);
					temp.GetComponentInChildren<UILabel>().text  = KoStorage.GetKorString("71006");
					//myAccount.instance.account.buttonStatus.isCrewNew[id-1000] = false;
					break;
				}
			}
		}*/

	}

	public void setCrewButton(){
		Utility.LogWarning("setCrewButton-no");
		/*var tr = transform.FindChild("View").FindChild("Grid") as Transform;
		string str = string.Empty;
		int id = 0;
		int count = myAccount.instance.account.listCrewInfo.Count;
		int cnt = tr.childCount;
		for(int j = 0; j < count; j++){
			id = myAccount.instance.account.listCrewInfo[j].crewId;
			str = id.ToString()+"_Crew";
		for(int i = 0; i < cnt; i++){
				if(tr.GetChild(i).name == str){
					var temp = tr.GetChild(i).GetChild(0).FindChild("myOne") as Transform;
					temp.gameObject.SetActive(true);
					temp.GetComponentInChildren<UILabel>().text  = KoStorage.GetKorString("71006");
					//myAccount.instance.account.buttonStatus.isCrewNew[id-1200] = false;
					break;
				}
			}
		}*/

	}

	void setNewButtonCar(){
		/*var tr = transform.FindChild("View").FindChild("Grid") as Transform;
		int cnt = tr.childCount;
		bool b =false;
		for(int i = 0; i < cnt; i++){
			b = myAccount.instance.account.buttonStatus.isCarNew[i];
			tr.GetChild(i).GetChild(0).FindChild("icon_New").gameObject.SetActive(b);
		}*/
	}

	void setNewButtonCrew(){
		/*var tr = transform.FindChild("View").FindChild("Grid") as Transform;
		int cnt = tr.childCount;
		bool b = false;
		for(int i = 0; i < cnt; i++){
			b = myAccount.instance.account.buttonStatus.isCrewNew[i];
			tr.GetChild(i).GetChild(0).FindChild("icon_New").gameObject.SetActive(b);
				
		}*/
	}

	public void setButton(string itemName){
	
		switch(itemName){
		case "Slot_Car":
			setCarButton();
			setNewButtonCar();
			break;
		case "Slot_Crew":
			setCrewButton();
			setNewButtonCrew();
			break;
		case "Slot_Sponsor":
			//setSponButton();
			break;
			default:
			break;
		}
	}

}
