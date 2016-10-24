using UnityEngine;
using System.Collections;

public partial class LobbyManager : MonoBehaviour {

	public void OnDollarClick(){
		if(Global.isPopUp) return;
		Global.isPopUp = true;
			Global.bLobbyBack = true;
	//	Utility.LogWarning("OnDollar");
	//	var tr = lobby.transform.GetChild(1) as Transform;
		var t = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
		var myObj = GameObject.FindGameObjectWithTag("CenterAnchor") as GameObject;
		var tr1 = myObj.transform.GetChild(0) as Transform;
		var tempObj = t.makeCoinShopInfo(tr1) as GameObject;
		Destroy(t);
		var obj  = tempObj.transform.FindChild("View").gameObject as GameObject;
		var Obj = obj.transform.FindChild("Grid") as Transform;
		GetComponent<ShopItemCreate>().SettingGridItemShop(Obj, "Slot_Coin");
		return;/*
		if(btnstate != buttonState.WAIT) return;
		//	Utility.Log("1");
		if(GetComponent<ShopMenu>().SetisCoinWindow) return;
		//	Utility.Log("2");
		if(Global.isNetwork ) return;
		//	Utility.Log("3" + isCoinShop);
		if(isCoinShop ||isLobbyRotation || Global.isAnimation ) return;
		if(isMap){
			Invoke ("mapDisable", 0.3f);
		}
		fadeIn();
		finishPanel();
		isLobby = false;
		btnstate = buttonState.Shop_Coin;
		GetComponent<ShopMenu>().SetisCoinWindow = true;
		GetComponent<ShopMenu>().HiddenWindow();
		HiddenInfoTipWindow();
		isCoinShop = true;
		_table.FenceDown();*/
	}


	public void OnAccountInfo(){
		if(Global.isPopUp) return;
		Global.isPopUp = true;
		Global.bLobbyBack = true;
		var myObj = GameObject.FindGameObjectWithTag("UpAnchor") as GameObject;
		var t = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
		t.makeAccInfo(myObj.transform.GetChild(0));
		Destroy(t);
	}
	public void OnChatClick(){
		if(Global.isPopUp) return;
		Global.isPopUp = true;
		Global.bLobbyBack = true;
		var myObj = GameObject.FindGameObjectWithTag("UpAnchor") as GameObject;
		var t = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
		var tr = t.makeChatWin(myObj.transform.GetChild(0)) as GameObject;
		Destroy(t);
		tr.GetComponent<MyChat>().setInit();
		MenuTop.GetComponent<LobbyAction>().setChatNewBTNActive(false);
	}

	void onChatString(string str){
		int nFlag = UserDataManager.instance.ChatEnable;
		if(nFlag == 1){
			MenuTop.GetComponent<LobbyAction>().setChatNewBTNActive(true);
		}else if(nFlag == 3){
			UserDataManager.instance.ChatEnable = 2;
		}else if(nFlag == 2){
		
		}
	}
	public void OnItemClicked(GameObject target){
		GetComponent<ShopItemCreate>().DestroyStatusWin();
		_table.ItemClick(target.name, ()=>{
			GetComponent<ShopItemCreate>().ResetWindow(target.name, target);
		}, true);
	//	_table.ItemClick(target.name, ()=>{
	//		GetComponent<ShopItemCreate>().ResetWindow(target.name, target);
	//	}, GV.SelectedTeamCode==0?true:false );
	}

	public void OnClassClicked(string targetName){
		_table.ClassItemClick(targetName);
		string[] str = targetName.Split('_');
		GetComponent<ShopMenu>().ChangeToClassStatus(str[2]);
	}

	void OnSponsorClick(){
		if(btnstate != buttonState.WAIT) return;
		fadeIn();
		isLobby = false;
		btnstate = buttonState.Sponsor;


	}
	
	void OnNextClick(string name){
		if(btnstate != buttonState.WAIT) return;
		fadeIn();
		btnstate = buttonState.START;
		if(raceinfo != null) {
			InfoWindowDisable();
			raceinfo = null;
		}
		finishUpPanel();
		Global.isRace = true;
	}
	
	
	void OnRaceModeClick(){
		if(Global.isPopUp) return;
		if(btnstate != buttonState.WAIT) return;
		AccountManager.instance.SponTimeCheck();
		isModeReturn = true;
		btnstate = buttonState.RACEMODE;
		fadeIn();
		isLobby = false;
		myAcc.instance.account.bLobbyBTN[5] = false;
		OnBackFunction = ()=>{
				btnstate = buttonState.LOBBY;
				isLobby = true;
				finishPanel();
				fadeIn();
			HiddenInfoTipWindow();
		};
	} //PitInRacing


	void OnRaceClanClick(){
		if(Global.isPopUp) return;
		if(btnstate != buttonState.WAIT) return;
		AccountManager.instance.SponTimeCheck();
		isModeReturn = true;
		btnstate = buttonState.RACEMODE;
		fadeIn();
		isLobby = false;
		myAcc.instance.account.bLobbyBTN[5] = false;
		OnBackFunction = ()=>{
			OnClanReturn();
		};
	}


	void LobbyRankUp(){
		rankObj.SendMessage("EndTweenSlow",SendMessageOptions.DontRequireReceiver);
		EndTweenPosition(MenuBottom, new Vector3(0,-200,0), Vector3.zero, 0.2f);
	}
	
	void OnLobbyBack(){
		if(isLobbyRotation){
			//bool b = rankObj.GetComponent<RankGridAction>().isDowning;
			bool b = rankObj.GetComponent<FBRankAction>().isDowning;
			if(b) return;
			//Utility.Log("lobbyback");
			isLobbyRotation = false;
			InitPanel("LOBBY");
			LobbyMenuInit();
			RTSCamControl(false);
			return;
		}
	}

	
	void OnLobbyClick(){
		if(isLobbyRotation) return;
		if(btnstate != buttonState.WAIT) return;
		bool b = rankObj.GetComponent<FBRankAction>().isDowning;
		if(b) return;
		isLobbyRotation = true;
		LobbyRankUp();
		RTSCamControl(true);
		InitPanel("TEAMINFO");
		OnBackFunction = ()=>{
			if(isLobbyRotation){
				isLobbyRotation = false;
				InitPanel("LOBBY");
				LobbyMenuInit();
				RTSCamControl(false);
			}
		};
	}

	public void OnBackClick(){
		if(btnstate != buttonState.WAIT) return;
		if(Global.isAnimation || Global.isNetwork || Global.isPopUp) return;
		GetComponent<ShopMenu>().SetisCoinWindow = false;
		if(UserDataManager.instance.SubmodeActive()) return;
		if(UserDataManager.instance.OnSubBackMenu != null){
			UserDataManager.instance.OnSubBackMenu();
			UserDataManager.instance.OnSubBackMenu = null;
		//	Utility.LogWarning("OnSubBackMenu");
			return;
		}
			
		if(OnBackFunction != null){
			OnBackFunction();
			OnBackFunction = null;
		//	Utility.LogWarning("OnBackFunction");
			return;
		}
		if(!isLobby){
			btnstate = buttonState.LOBBY;
			isLobby = true;
			finishPanel();
			fadeIn();
			GetComponent<ShopMenu>().HiddenWindow();
			HiddenInfoTipWindow();
			RotateTable.SetActive(false);
			Utility.LogWarning("isLobby");
		}
	}


	
	
	void OnClanClick(){
		if(Global.isPopUp) return;
		if(btnstate != buttonState.WAIT) return;
		btnstate = buttonState.CLAN;
		fadeIn();
		isLobby = false;
		myAcc.instance.account.bLobbyBTN[0] = false;
		OnBackFunction = ()=>{
			btnstate = buttonState.LOBBY;
			isLobby = true;
			finishPanel();
			fadeIn();
		};
	}


	void OnClanReturn(){
		if(btnstate != buttonState.WAIT) return;
		btnstate = buttonState.CLAN;
		fadeIn();
		isLobby = false;
		HiddenInfoTipWindow();
		OnBackFunction = ()=>{
				btnstate = buttonState.LOBBY;
				isLobby = true;
				finishPanel();
				fadeIn();
			};
	}



	void OnTeamClick(){
		if(Global.isPopUp) return;
		if(btnstate != buttonState.WAIT) return;
		btnstate = buttonState.TEAM;
		fadeIn();
		isLobby = false;
		myAcc.instance.account.bLobbyBTN[1] = false;
		OnBackFunction = ()=>{
			btnstate = buttonState.LOBBY;
			isLobby = true;
			finishPanel();
			fadeIn();
			transform.GetComponent<ShopItemCreate>().SponsorStop();
			HiddenInfoTipWindow();
			if(status != null) {
				hiddenStatusWindow(status);
			}
			if(raceinfo != null) {
				HiddenInfoWindow();
			}
		//	if(onBackBack != null) 
		//		onBackBack();
		//	onBackBack = null;
		};
	}

	void OnTeamUpClick(){
		if(Global.isPopUp) return;
		string str1 = string.Empty;
		if(GV.SelectedTeamID %2 != 0){
			str1 = "TeamTour_"+GV.SelectedTeamID.ToString();
		}else{
			str1 = "TeamStock_"+GV.SelectedTeamID.ToString();;
		}
		string[] name  = str1.Split('_');
		if(name[0].Equals("TeamStock")){
			if(btnstate != buttonState.WAIT) return;
			fadeIn();
			btnstate = buttonState.TEAM_UP_CREW;
			menuCenter.InitCenterUpMenu("Crew");
			isLobby =false;
			ResetElevatorCar(true, str1);
			string str = null;
			if(CrewUpNameStock == null) str = "Upgrade_CarTo" +"Driver";
			else str = "Upgrade_CarTo" +CrewUpNameStock;
			camAni.PlayAnimationFast(str);
			//GV.SelectedTeamCode = 0;
			//SettingCrewPartsLV();
		}else{
			if(btnstate != buttonState.WAIT) return;
			fadeIn();
			btnstate = buttonState.TEAM_UP_CREW;
			menuCenter.InitCenterUpMenu("Crew");
			isLobby =false;
			ResetElevatorCar(false, str1);
			string str = null;
			if(CrewUpNameTour == null) str = "Upgrade_CarTo" +"Driver";
			else str = "Upgrade_CarTo" +CrewUpNameTour;
			camAni_Tour.PlayAnimationFast(str);
		//	GV.SelectedTeamCode = 1;
		}
		OnBackFunction = ()=>{
			menuCenter.disableTeamUp();
			btnstate = buttonState.LOBBY;
			isLobby = true;
			finishPanel();
			fadeIn();
			HiddenInfoTipWindow();
			if(status != null) {
				hiddenStatusWindow(status);
			}
			if(raceinfo != null) {
				HiddenInfoWindow();
			}
			//OnTeamClick();
		};
	}



	void OnTeamCarRepair(string selectName){
		Utility.LogWarning("OnTeamCarRepair");
	}

//	bool isTeamSet = true; //true : stock , false : tour
//	bool isTeamUp = true; //true : stock, false ; tour
	/*
	void OnTeamUp(string selectName){
		if(selectName.Equals("Stock")){
			if(btnstate != buttonState.WAIT) return;
			isTeamUp = false;
			fadeIn();
			btnstate = buttonState.TEAM_UP;
			menuCenter.InitCenterUpMenu();
			isLobby =false;
			ElevatorCar.SetActive(true);
			string str = null;
			if(CrewUpNameStock == null) str = "Upgrade_CarTo" +"Driver";
			else str = "Upgrade_CarTo" +CrewUpNameStock;
			camAni.PlayAnimationFast(str);
			isTeamUp = true;
		}else{
			if(btnstate != buttonState.WAIT) return;
			isTeamUp = false;
			fadeIn();
			btnstate = buttonState.TEAM_UP;
			menuCenter.InitCenterUpMenu();
			isLobby =false;
			ElevatorCar.SetActive(true);
			string str = null;
			if(CrewUpNameTour == null) str = "Upgrade_CarTo" +"Driver";
			else str = "Upgrade_CarTo" +CrewUpNameTour;
			camAni_Tour.PlayAnimationFast(str);
		}
	}


	void OnTeamSet(string selectName){
		if(selectName.Equals("Stock")){
			if(btnstate != buttonState.WAIT) return;
			menuCenter.InitCenterMenu();
			isTeamSet = true;
			fadeIn();
			strTip = "Team";
			btnstate = buttonState.MYCAR;
			isLobby =false;
			InitTableShop(isTeamSet);
		}else{
			if(btnstate != buttonState.WAIT) return;
			menuCenter.InitCenterMenu();
			isTeamSet = false;
			fadeIn();
			strTip = "Team";
			btnstate = buttonState.MYCAR;
			isLobby =false;
			InitTableShop(isTeamSet);
		}
	}
	*/
	void OnMyCarClick(){
	
		Utility.LogWarning("OnMyCarClick");
	}
	/*
	void OnTeamChange(){
		OnTeamClick();
		transform.GetComponent<ShopItemCreate>().SponsorStop();
		HiddenInfoTipWindow();
		if(status != null) {
			hiddenStatusWindow(status);
		}
		if(raceinfo != null) {
			HiddenInfoWindow();
		}
		if(onBackBack != null) 
			onBackBack();
		onBackBack = null;
	}

	void OnTeamSpon(){
		if(menuCenter.OnSponClick()) return;
		if(raceinfo != null) {
			HiddenInfoWindow();
		}
		HiddenInfoTipWindow();
		OnSponsorClick();
		RotateTable.SetActive(false);
	}

	void OnTeamCars(){
		if(menuCenter.OnCarClick()) return;	
		if(raceinfo != null) {
			HiddenInfoWindow();
		}
		HiddenInfoTipWindow();
		transform.GetComponent<ShopItemCreate>().SponsorStop();
		if(isTeamSet){
			fadeIn();
			strTip = "Team";
			btnstate = buttonState.MYCAR;
			isLobby =false;
			InitTableShop(isTeamSet);
		}else{
			isTeamSet = false;
			fadeIn();
			strTip = "Team";
			btnstate = buttonState.MYCAR;
			isLobby =false;
			InitTableShop(isTeamSet);
		}
		//OnMyCarClick();
	}
*/
	void OnInvenClick(){
		if(Global.isPopUp) return;
		if(btnstate != buttonState.WAIT) return;
		invenAniString = "Car";
		cameraAniCtrl aniCam = CameraAnimation("Inven").GetComponent<cameraAniCtrl>();
		aniCam.InitCamera();
		btnstate = buttonState.ONINVEN;
		fadeIn();
		isLobby = false;
		ElevatorCar.SetActive(true);
		myAcc.instance.account.bLobbyBTN[2] = false;
		OnBackFunction = ()=>{
				menuCenter.disableInven();
				btnstate = buttonState.LOBBY;
				isLobby = true;
				finishPanel();
				fadeIn();
				menuCenter.initInvenSub(2);
				
		};
	}
	string invenAniString = "Car";
	bool invenAnimation(string str){
		cameraAniCtrl aniCam = CameraAnimation("Inven").GetComponent<cameraAniCtrl>();
		//string str1 = string.Empty, str2=string.Empty;
		if(str.Equals(invenAniString)){
			return true;
		}
		if(aniCam.aniPlaying) return true;
		else aniCam.PlayInvenAnimation(str,invenAniString);
		invenAniString = str;
		return false;
	}
	void OnInvenCar(){
		if(Global.isAnimation) return;
		if(invenAnimation("Car")) return;
		if(menuCenter.OnInvenCar()) return;
		strTip = "Inven_Car"; ShowInfoTipWindow(strTip);
		menuCenter.initInvenSub(0);
		InvenObj.SendMessage("OnSetInvenMenu",0,SendMessageOptions.DontRequireReceiver);
	}

	void OnInvenMat(){

		if(Global.isAnimation) return;
		if(invenAnimation("Mat")) return;
		if(menuCenter.OnInvenMat()) return;
		strTip = "Inven_Mat"; ShowInfoTipWindow(strTip);
		menuCenter.initInvenSub(1);
		InvenObj.SendMessage("OnSetInvenMenu",1,SendMessageOptions.DontRequireReceiver);
	}

	void OnInvenCoupon(){
		if(Global.isAnimation) return;
		if(invenAnimation("Coupon")) return;
		if(menuCenter.OnInvenCoupon()) return;
		strTip = "Inven_Coupon"; ShowInfoTipWindow(strTip);
		menuCenter.initInvenSub(2);
		InvenObj.SendMessage("OnSetInvenMenu",3,SendMessageOptions.DontRequireReceiver);
	}


	void OnInvenQube(){
		if(Global.isAnimation) return;
		if(invenAnimation("Cube")) return;
		if(menuCenter.OnInvenQube()) return;
		strTip = "Inven_Cube"; ShowInfoTipWindow(strTip);
		menuCenter.initInvenSub(3);
		InvenObj.SendMessage("OnSetInvenMenu",2,SendMessageOptions.DontRequireReceiver);
	}
	void OnSpecialCar(){
		if(Global.isAnimation) return;
		if(menuCenter.OnChoiceCar(2)) return;
		InvenObj.SendMessage("OnSetInvenSubCar",2,SendMessageOptions.DontRequireReceiver);
	}
	
	void OnTourCar(){
		if(Global.isAnimation) return;
		if(menuCenter.OnChoiceCar(0)) return;
		InvenObj.SendMessage("OnSetInvenSubCar",0,SendMessageOptions.DontRequireReceiver);
	}
	
	void OnStockCar(){
		if(Global.isAnimation) return;
		if(menuCenter.OnChoiceCar(1)) return;
		InvenObj.SendMessage("OnSetInvenSubCar",0,SendMessageOptions.DontRequireReceiver);
	}
	/*
	void OnCarMat(){
		if(menuCenter.OnChoiceMat(4)) return;
		InvenObj.SendMessage("OnSetInvenSubMat",1,SendMessageOptions.DontRequireReceiver);
	}
	void OnCrewMat(){
		if(menuCenter.OnChoiceMat(3)) return;
		InvenObj.SendMessage("OnSetInvenSubMat",0,SendMessageOptions.DontRequireReceiver);
	}
	void OnCubeMat(){
		if(menuCenter.OnChoiceMat(5)) return;
		InvenObj.SendMessage("OnSetInvenSubMat",2,SendMessageOptions.DontRequireReceiver);
	}*/

	/*
	void OnMechanicInven(){
		if(menuCenter.OnInvenPart()) return;
		mechanicObj.SendMessage("ChangeMaterial", false);
		finishPanel();
		InitPanel("NONMECHANIC");
		menuCenter.ShowInvenSub();
	}

	void OnMechanicMake(){
		if(menuCenter.OnInvenMat()) return;
		mechanicObj.SendMessage("ChangeMaterial" , true);
		finishPanel();
		InitPanel("ONMECHANIC");
		menuCenter.HiddenInvenSub();
	}

	void OnCarPartsClick(string str){
		var temp = activeObject.transform.FindChild(str).FindChild("Select").gameObject as GameObject;
		if(temp.activeSelf) return;
		mechanicObj.SendMessage("ChangeParts", str,SendMessageOptions.DontRequireReceiver);
		int cnt = activeObject.transform.childCount;
		for(int i = 0; i < cnt; i++){
			var tr = activeObject.transform.GetChild(i) as Transform;
			if(str.Equals(tr.name)){
				tr.FindChild("Select").gameObject.SetActive(true);
			}else{
				tr.FindChild("Select").gameObject.SetActive(false);
			}
		}
	}*/

	void OnContainerShop(){
		if(Global.isPopUp) return;
		if(btnstate != buttonState.WAIT) return;
		btnstate = buttonState.MY_SHOP;
		fadeIn();
		isLobby = false;
		RotateTable.SetActive(false);
		myAcc.instance.account.bLobbyBTN[4] = false;
		OnBackFunction = ()=>{
			menuCenter.disableShop();
			transform.GetComponent<ShopMenu>().ShopStop();
			HiddenInfoTipWindow();
			btnstate = buttonState.LOBBY;
			isLobby = true;
			finishPanel();
			fadeIn();
			RotateTable.SetActive(false);
		//	if(GV.gBuyDealerCar == 0){
		//		GV.gDealerCarID = 0;
		//	}
		};
	}

	void OnShopContainer(){
		if(menuCenter.OnShopSub(0)) return;
		btnstate = buttonState.CONTAINER_SHOP;
		fadeIn();
		HiddenInfoTipWindow();
		transform.GetComponent<ShopMenu>().ShopStop();
		RotateTable.SetActive(false);
	}

	void OnShopCar(){
		if(Global.isAnimation) return;
		if(menuCenter.OnShopSub(1)) return;
		btnstate = buttonState.CAR_SHOP;
		fadeIn();
		HiddenInfoTipWindow();
		RotateTable.SetActive(true);
		RotateTable.SendMessage("ChangeTarget",false,SendMessageOptions.DontRequireReceiver);
		if(onBackBack != null) 
			onBackBack();
		onBackBack = null;
	}

	void OnShopCarDealer(){
		if(Global.isAnimation) return;
		btnstate = buttonState.CAR_SHOP_DEAL;
		fadeIn();
		RotateTable.SetActive(true);
		RotateTable.SendMessage("ChangeTarget",false,SendMessageOptions.DontRequireReceiver);
		isLobby = false;

		OnBackFunction = ()=>{
			if(GV.gRaceMode == 3){
				transform.GetComponent<ShopMenu>().ShopStop();
				HiddenInfoTipWindow();
				finishPanel();
				fadeIn();
				RotateTable.SetActive(false);
				OnRaceModeClick();
			}else{
				transform.GetComponent<ShopMenu>().ShopStop();
				HiddenInfoTipWindow();
				btnstate = buttonState.LOBBY;
				isLobby = true;
				finishPanel();
				fadeIn();
				RotateTable.SetActive(false);
			}
		};
	}
	void MoveToDealer(){
		GV.gRaceMode = 3;
		finishPanel();
		fadeIn();
		HiddenInfoTipWindow();
		Invoke("GoToDealerPageFromRacePage",0.25f);
		
	}
	
	void MoveToBackDealer(){
		finishPanel();
		fadeIn();
		HiddenInfoTipWindow();
		Invoke ("OnShopCarDealer",0.25f);
	}

	void GoToDealerPageFromRacePage(){
		if(Global.isAnimation) return;
		btnstate = buttonState.CAR_SHOP_DEAL;
		fadeIn();
		RotateTable.SetActive(true);
		RotateTable.SendMessage("ChangeTarget",false,SendMessageOptions.DontRequireReceiver);
		//	isLobby = false;
		_table.setFenceDown();
		OnBackFunction = ()=>{
			transform.GetComponent<ShopMenu>().ShopStop();
			HiddenInfoTipWindow();
			finishPanel();
			fadeIn();
			RotateTable.SetActive(false);
			OnRaceModeClick();

		};
	}

	void OnShowRoom(){
		if(Global.isAnimation) return;
		if(menuCenter.OnShopSub(2)) return;
		btnstate = buttonState.CAR_SHOW;
		fadeIn();
		HiddenInfoTipWindow();
		RotateTable.SetActive(true);
		RotateTable.SendMessage("ChangeTarget",false,SendMessageOptions.DontRequireReceiver);
		if(onBackBack != null) 
			onBackBack();
		onBackBack = null;
	}

	void menuCarParts(bool b){
		var temp = activeObject.GetComponent<TweenPosition>() as TweenPosition;
		if(temp == null) temp = activeObject.AddComponent<TweenPosition>();
		if(b) activeObject.GetComponent<InfoWindow>().hiddenSub(activeObject);
		else activeObject.GetComponent<InfoWindow>().showSub(activeObject);
	}


	void OnLobbyTeam(GameObject obj){
		string str = obj.name;
		var temp = obj.transform.FindChild("Select").gameObject as GameObject;
		//if(GV.TeamSeason == 1) return;
		if(temp.activeSelf) return;
		if(Global.isAnimation) return;
		if(str.Equals("T_Stock")){
			obj.transform.parent.FindChild("T_Touring").FindChild("Select").gameObject.SetActive(false);

		}else{
			obj.transform.parent.FindChild("T_Stock").FindChild("Select").gameObject.SetActive(false);

		}
		temp.SetActive(true);
		StartCoroutine(MovingLobbyCam(str));
	}

	IEnumerator MovingLobbyCam(string name){
		Camera cam = CameraAnimation("Lobby");
		var temp = cam.transform.parent.parent.gameObject as GameObject;
		Animation ani = temp.GetComponent<Animation>();
		Global.isAnimation  = true;
		if(name.Equals("T_Touring")){
			ani["Lobby_ChangeTeam"].time = 0;
			ani["Lobby_ChangeTeam"].speed = 1;
			ani.Play("Lobby_ChangeTeam");
			//GV.SelectedTeam = "Tour";
			//GV.SelectedTeam = "Stock";
		}else{
			ani["Lobby_ChangeTeam"].time = ani["Lobby_ChangeTeam"].length;
			ani["Lobby_ChangeTeam"].speed = -1;
			ani.Play("Lobby_ChangeTeam");
			//GV.SelectedTeam = "Stock";
		}
		ChangeTeamLV("Stock",1);
		while(ani.isPlaying){
			yield return null;
		}
		Global.isAnimation = false;
	}

	void ChangeTeamLV(string str, int idx){
		var ab = MenuTop.transform.FindChild("Ability") as Transform;
		if(idx == 0){
			ab.gameObject.SendMessage("ChangeTeamInfo1", SendMessageOptions.DontRequireReceiver);
		}
		ab.gameObject.SendMessage("ChangeTeamInfo2",  SendMessageOptions.DontRequireReceiver);
	}



	void ChangeTeamTopInfo(){
		ChangeTeamLV("Stock",0);
	}




	void OnRepair(string carname){
		raceinfo.SendMessage("OnRepairRelay",carname,SendMessageOptions.DontRequireReceiver);
	}

	void OnLobbyRepair(){
		if(Global.isPopUp) return;
		Global.isPopUp = true;
		Global.bLobbyBack = true;
		var starTarget =  ObjectManager.CreateTagPrefabs("CardButton") as GameObject;
		if(starTarget == null) {
			var tempObj = GameObject.FindGameObjectWithTag("CenterAnchor") as GameObject;
			starTarget = ObjectManager.CreateLobbyPrefabs(tempObj.transform.GetChild(0), "Card", "EvolutionRepair", "CardButton");
			starTarget.GetComponent<TweenPosition>().enabled =true;
		}

		starTarget.transform.FindChild("Evolution").gameObject.SetActive(false);
		var child = starTarget.transform.FindChild("Repair").gameObject as GameObject;
		child.SetActive(true);
		starTarget.GetComponent<TweenAction>().doubleTweenScale(child);
	//	onFinish();
		var _card = child.GetComponent<RepairAction>();
		if(_card == null) _card = child.AddComponent<RepairAction>();
		_card.repairTeamCar();
		_card.OnRepairDes(()=>{
			Global.isPopUp = false;
		});
	}

	void OnUpCarStar(){
			
			raceinfo.SendMessage("OnStarCarPart",SendMessageOptions.DontRequireReceiver);
	
	}
	void OnUpCrewStar(){
			raceinfo.SendMessage("OnStarCrew",SendMessageOptions.DontRequireReceiver);
	}


	void  TempRepairLabel(bool b){
		var tr = activeObject.transform.GetChild(7)  as Transform;
		tr.gameObject.SetActive(b);
	}

	void InvenDismantleAni(){
		cameraAniCtrl aniCam = CameraAnimation("Inven").GetComponent<cameraAniCtrl>();
		aniCam.InvenDisMantleAniPlay();
	}
	void SuccessCrewEffect(string str){
	//	Utility.LogWarning(str);
		if(_table == null) {
			
		}else{
			_table.CrewEffect(str);
		}
	}

	void SuccessCarEffect(string str){
		var root = elevator.transform.GetChild(1).gameObject as GameObject;
		var root1 = root.transform.GetChild(8).gameObject as GameObject;
		root = root1.transform.GetChild(0).gameObject as GameObject;
		var car = Resources.Load("Effect_N/Upgrade_Car", typeof(GameObject)) as GameObject;
		var race = Instantiate(car) as GameObject;
		race.transform.parent = root.transform;
		race.transform.localPosition = Vector3.zero;
		Utility.LogWarning("carEffect");
	}
}
