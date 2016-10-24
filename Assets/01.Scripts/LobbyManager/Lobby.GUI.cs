using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public partial class LobbyManager : MonoBehaviour {
	GameObject raceMenu;
	bool isQuit =false;
	void FixedUpdate(){
		if (Input.GetKey(KeyCode.Escape)){
			if(isQuit) return;
			isQuit = true;	

			if(UserDataManager.instance.bPopUpAddNetwork){
					if(!UserDataManager.instance.bPopUpAdd)
						UserDataManager.instance.onGameExit();
					else UserDataManager.instance.setGameExit();
				Invoke("OnIsQuitActivation", 1.0f);
				return;
			}
			if(Global.bLobbyBack){
				if(Global.isNetwork) {
					Invoke("OnIsQuitActivation", 1.0f);
					return;
				}
				if(UserDataManager.instance.OnSubBacksubsub != null){
					UserDataManager.instance.OnSubBacksubsub();
					UserDataManager.instance.OnSubBacksubsub = null;
					Invoke("OnIsQuitActivation", 1.0f);
					return;
				}
				if(UserDataManager.instance.OnSubBacksub != null){
					UserDataManager.instance.OnSubBacksub();
					UserDataManager.instance.OnSubBacksub = null;
					Invoke("OnIsQuitActivation", 1.0f);
					return;
				}
				if(UserDataManager.instance.OnSubBack != null){
					UserDataManager.instance.OnSubBack();
				}

				UserDataManager.instance.OnSubBack = null;
				Invoke("OnIsQuitActivation", 1.0f);
				return;
			}
			if(isLobby){
				if(!UserDataManager.instance.bPopUpAdd)
					UserDataManager.instance.onGameExit();
				else UserDataManager.instance.setGameExit();
			}else{
				OnBackClick();
			}
			Invoke("OnIsQuitActivation", 1.0f);
		}
	}
	void OnIsQuitActivation(){
		isQuit = false;
	}
	private void ChangeGameState(){
		switch(btnstate){
		case buttonState.LOBBY:
		{
			beforeState =btnstate;
			//Utility.LogWarning("OnLOBBY");
			LobbyMenuInit();
			btnstate = buttonState.WAIT;
			InitPanel("LOBBY");
			camAni.InitCamera();
			transform.GetComponent<ShopMenu>().ShopStop();
			transform.GetComponent<ShopItemCreate>().SponsorStop();
			_table.TableShopDelete();
			//AccountManager.instance.SponTimeCheck();
			MakeLobbyCar();
			MakeLobbyCrew();
			int id = GV.getTeamCarID(GV.SelectedTeamID);
			CreateElevatroCar(id);
			isLobby = true;
			EnableCamera("Lobby");
			HiddenInfoTipWindow();
			fadeOut();
			MenuBackActivate(false);
			if(onBackBack != null) 
				onBackBack();
			onBackBack = null;
			System.GC.Collect();
		}
			break;
		case buttonState.MENU:
			
			break;
		case buttonState.NOTHING:
		{
			
		}
			break;
		case buttonState.MYCAR:
		{
			beforeState =btnstate;
			menuCenter.TeamInit();
			finishUpPanel();
			btnstate = buttonState.WAIT;
			if(GV.SelectedTeamID%2==0)
			InitPanel("MYCAR_STOCK");
			else InitPanel("MYCAR_TOUR");
			EnableCamera("MyTeam");
			ExecuteCallFunction();
			fadeOut();
			ShowInfoTipWindow(strTip);
			strTip = string.Empty;
			_table.FenceDown();
			MenuBackActivate(true);
			if(onBackBack != null) 
				onBackBack();
			onBackBack = null;
		
		}break;
		case buttonState.OVER:
		{
			Application.Quit();
			btnstate = buttonState.WAIT;
		}
			break;
		case buttonState.START:
		{
			btnstate = buttonState.WAIT;
			Global.isRace = true;
			Application.LoadLevel(2);
			Utility.Log("START");
			fadeOut();
			if(raceoff[0] != null){
				Destroy(raceoff[0],0.5f);Destroy(raceoff[1],0.5f);Destroy(raceoff[2],0.5f);
				raceoff[0]= raceoff[1] = raceoff[2] = null;
			}
		}
			break;

		case buttonState.UPGRADE_CAR:{
			beforeState =btnstate;
			finishUpPanel();
			InitPanel("UPGRADE_CAR");
			EnableCamera("Upgrade");
			btnstate = buttonState.WAIT;
			fadeOut();
			MenuBackActivate(true);
			ShowInfoTipWindow("Upgrade");
			menuCenter.TeamUpCarInit();
		}break;
			
		case buttonState.UPGRADE_CREW:{
			beforeState =btnstate;
			finishUpPanel();
			InitPanel("UPGRADE_CREW");
			btnstate = buttonState.WAIT;
			ShowInfoTipWindow("Upgrade_Crew");
			MenuBackActivate(true);
				EnableCamera("Upgrade");
			menuCenter.TeamUpCrewInit();
		}break;
		case buttonState.TEAM_UP_CREW:{
			beforeState =btnstate;
			btnstate = buttonState.WAIT;
			finishUpPanel();
			menuCenter.TeamUpInit();
			InitPanel("UPGRADE_CREW");
			ShowInfoTipWindow("Upgrade_Crew");
			MenuBackActivate(true);
			fadeOut();
				EnableCamera("Upgrade");
			if(onBackBack != null) 
				onBackBack();
			onBackBack = null;
		}break;
		case buttonState.TEAM_UP_CAR:{
			beforeState =btnstate;
			btnstate = buttonState.WAIT;
			finishUpPanel();
			menuCenter.TeamUpInit();
			InitPanel("UPGRADE_CAR");
			ShowInfoTipWindow("Upgrade");
			MenuBackActivate(true);
			fadeOut();
				EnableCamera("Upgrade");
			if(onBackBack != null) 
				onBackBack();
			onBackBack = null;
		}break;
		case buttonState.TITLETOLOBBY:
		{
			beforeState =buttonState.LOBBY;// LOBBY;
			btnstate = buttonState.WAIT;
			lobby.SetActive(true);          
			EnableCamera("Lobby");
			lobby.GetComponent<crewSpeak>().LobbyCamCheck(false);
			if(isLevelCheck){
				btnstate = buttonState.LEVELUP;
				StartCoroutine("InitPopUpWindow");
				isLevelCheck = false;
			}
			fadeOut();
		}
			break;
			
		case buttonState.Sponsor:
		{
			beforeState =btnstate;
			btnstate = buttonState.WAIT;
			finishUpPanel();

			if(GV.SelectedTeamID%2 ==0) {
				_table.SponsorStart(true);
				InitPanel("SPONSOR_STOCK");
				transform.GetComponent<ShopItemCreate>().SponsorStart("Stock");
			}else {
				_table.SponsorStart(false);
				InitPanel("SPONSOR_TOUR");
				transform.GetComponent<ShopItemCreate>().SponsorStart("Tour");
			}
			EnableCamera("Sponsor");
			fadeOut();
			MenuBackActivate(true);
			if(onBackBack != null) 
				onBackBack();
			onBackBack = null;
		}break;

		case buttonState.MAP:
		{
			btnstate = buttonState.WAIT;
			finishUpPanel();
			InitPanel("MAP");
			menuCenter.LobbyInit();
			EnableCamera(string.Empty);
			fadeOut();
			MenuBackActivate(true);
		}
			break;
		case buttonState.MAPTOLOBBY:{
			beforeState =btnstate;
			btnstate = buttonState.WAIT;
			finishUpPanel();
			InitPanel("LOBBY");
			LobbyMenuInit();
			EnableCamera("Lobby");
			fadeOut();
			MenuBackActivate(false);
		}break;
		case buttonState.MAPTORACEMODE:{
			beforeState =btnstate;
			btnstate = buttonState.WAIT;
			finishUpPanel();
			InitPanel("RACEMODE");
			EnableCamera("RaceMenuCam");
			_table.RaceCamStart(true);
			//EnableCamera(string.Empty);
			fadeOut();

			//ShowInfoTipWindow(strMode);
			OnBackFunction = ()=>{
					btnstate = buttonState.LOBBY;
					isLobby = true;
					finishPanel();
					fadeIn();
					HiddenInfoTipWindow();
			};

		}	
			break;
		case buttonState.RACEMODE:{
			beforeState =btnstate;	
			btnstate = buttonState.WAIT;
			finishUpPanel();
			fadeOut();
			MenuBackActivate(true);
			InitPanel("RACEMODE");
			EnableCamera("RaceMenuCam");
			_table.RaceCamStart(true);
			//EnableCamera(string.Empty);
		}
			break;
		case buttonState.MAP_CLAN:
		{
			btnstate = buttonState.WAIT;
			finishUpPanel();
			InitPanel("MAP_CLAN");
			EnableCamera(string.Empty);
			fadeOut();
			onBackBack();
		}break;
		case buttonState.MAP_RACE:{
			btnstate = buttonState.WAIT;
			finishUpPanel();
			InitPanel("MAP_RACE");
			menuCenter.LobbyInit();
			EnableCamera(string.Empty);
			fadeOut();
			onBackBack();
		}
			break;
		case buttonState.MAP_RANK:{
			btnstate = buttonState.WAIT;
			finishUpPanel();
			InitPanel("MAP_RANK");
			menuCenter.LobbyInit();
			EnableCamera(string.Empty);
			fadeOut();
			onBackBack();
		}
			break;
		case buttonState.MAP_CHAMPION:{
			btnstate = buttonState.WAIT;
			finishUpPanel();
			InitPanel("MAP_CHAMPION");
			menuCenter.LobbyInit();
			EnableCamera(string.Empty);
			fadeOut();
			onBackBack();
		}
			break;
		case buttonState.MAP_PVP:{
			btnstate = buttonState.WAIT;
			finishUpPanel();
			InitPanel("MAP_PVP");
			menuCenter.LobbyInit();
			EnableCamera(string.Empty);
			fadeOut();
			onBackBack();
		}
			break;
		case buttonState.MAP_EVENT:{
			btnstate = buttonState.WAIT;
			finishUpPanel();
			InitPanel("MAP_EVENT");
			menuCenter.LobbyInit();
			EnableCamera(string.Empty);
			fadeOut();
			onBackBack();
		}
			break;
		case buttonState.CLAN:{
			beforeState =btnstate;	
			btnstate = buttonState.WAIT;
			finishUpPanel();
			if(onBackBack != null)
				onBackBack();
			InitPanel("CLAN");
			EnableCamera(string.Empty);
			fadeOut();
			MenuBackActivate(true);

		}
			break;
		case buttonState.TEAM:{
			beforeState =btnstate;	
			btnstate = buttonState.WAIT;
			finishUpPanel();
			InitPanel("TEAM");
			_table.TableShopDelete();
			EnableCamera(string.Empty);
			fadeOut();
			MenuBackActivate(true);
			menuCenter.LobbyInit();

			if(GV.SelectedTeamID != GV.SelectedSponTeamID){
				GV.SelectedTeamID = GV.SelectedSponTeamID;
				LobbyChangeTeam(GV.SelectedTeamID);
			}
		}
			break;
		case buttonState.ONINVEN:{
			beforeState =btnstate;	
			btnstate = buttonState.WAIT;
			finishUpPanel();
			InitPanel("ONINVEN");
			EnableCamera("Inven");
			fadeOut();
			MenuBackActivate(true);
			strTip = "Inven_Car";
			ShowInfoTipWindow(strTip);
			menuCenter.InitCenterInvenMenu();
			menuCenter.InvenInit();

		}
			break;
		case buttonState.MY_SHOP:
			beforeState =btnstate;	
			btnstate = buttonState.WAIT;
			finishUpPanel();
			InitPanel("MY_SHOP");
			EnableCamera("Luckybox");
			fadeOut();
			MenuBackActivate(true);
			strTip = "sLucky";
			ShowInfoTipWindow(strTip);
			menuCenter.InitCenterShopMenu();
			menuCenter.ShopInit();
			break;
		case buttonState.CONTAINER_SHOP:
			beforeState =btnstate;	
			btnstate = buttonState.WAIT;
			finishUpPanel();
			InitPanel("MY_SHOP");
			EnableCamera("Luckybox");
			strTip = "sLucky";
			ShowInfoTipWindow(strTip);
			fadeOut();
			break;
		case buttonState.CAR_SHOP:
			beforeState =btnstate;		
			btnstate = buttonState.WAIT;
			finishUpPanel();
			transform.GetComponent<ShopMenu>().ShopStop();
			transform.GetComponent<ShopMenu>().ShopStart("Car");
			_table.ShopStart();
			EnableCamera("Dealer");
			fadeOut();

			break;
		case buttonState.CAR_SHOP_DEAL:
			beforeState =btnstate;		
			btnstate = buttonState.WAIT;
			finishUpPanel();
			transform.GetComponent<ShopMenu>().ShopStop();
			transform.GetComponent<ShopMenu>().ShopStart("Deal");
			_table.ShopStart();
			EnableCamera("Dealer");
			MenuBackActivate(true);
			menuCenter.InitDealerMenu();
			fadeOut();
			break;
		case buttonState.CAR_SHOW:
			beforeState =btnstate;		
			btnstate = buttonState.WAIT;
			finishUpPanel();
			transform.GetComponent<ShopMenu>().ShopStop();
			transform.GetComponent<ShopMenu>().ShopStart("Room");
			_table.ShopStart();
			EnableCamera("Shop");
			fadeOut();
			break;
		default:
			break;
		}
	}

	
	/*case buttonState.Shop_Car:
		{	
			beforeState =btnstate;		
			btnstate = buttonState.WAIT;
			finishUpPanel();
			transform.GetComponent<ShopMenu>().ShopStart();
			_table.ShopStart();
			menuCenter.LobbyInit();
			EnableCamera("Shop");
			ExecuteCallFunction();
			//fadeOut();
			//MenuBackActivate(true);
			//menuCenter.InvenHidden();
			
			//btnstate = buttonState.WAIT;
			//finishUpPanel();
			//EnableCamera("Shop_Container");
			//InitPanel("Container");
			fadeOut();
			MenuBackActivate(true);
			
			
		}break;
		case buttonState.CONTAINER:
		{
			btnstate = buttonState.WAIT;
			finishUpPanel();
			EnableCamera("Shop_Container");
			InitPanel("Container");
			fadeOut();
			MenuBackActivate(true);

		}	break;

		case buttonState.Shop_Coin:
		{	
			//beforeState =btnstate;		
			btnstate = buttonState.WAIT;
			finishUpPanel();
			transform.GetComponent<ShopMenu>().ShopStart();
			_table.ShopStart();
			menuCenter.LobbyInit();
			EnableCamera("Shop");
			ExecuteCallFunction();
			fadeOut();
			MenuBackActivate(true);
			menuCenter.InvenHidden();
			if(onBackBack != null){
				onBackBack();
				onBackBack = null;
			}
		}break;
 */
	/*	case buttonState.BEFROECOIN:
		{
			GetComponent<ShopMenu>().HiddenCoinWindow();
			//GetComponent<ShopMenu>().CenterMenu.SetActive(false);
			btnstate = beforeState;
			if(btnstate == buttonState.MYCREW){
				finishUpPanel();
				transform.GetComponent<ShopMenu>().ShopStop();
				menuCenter.TeamCrewInit();
				btnstate = buttonState.WAIT;
				InitPanel("MYCREW");
				EnableCamera("MyTeam");
				isCoinShop = false;
				fadeOut();
				MenuBackActivate(true);
			}else if(btnstate == buttonState.UPGRADE_CREW){
				finishUpPanel();
				transform.GetComponent<ShopMenu>().ShopStop();
				//menuCenter.TeamUpCrewInit();
				btnstate = buttonState.WAIT;
				InitPanel("UPGRADE_CREW");
				EnableCamera("Upgrade");
				isCoinShop = false;
				fadeOut();
				MenuBackActivate(true);
				ShowInfoTipWindow("Upgrade_Crew");
			}else if( btnstate == buttonState.MYCARD){
				menuCenter.TeamInit();
				menuCenter.InitCenterMenu();
				finishUpPanel();
				btnstate = buttonState.WAIT;
				InitPanel("MYCAR");
				EnableCamera("MyTeam");
				ExecuteCallFunction();
				fadeOut();//Utility.LogWarning(" OnMYTeam .MYCAR" +Time.time);
				ShowInfoTipWindow(strTip);
				strTip = string.Empty;
				MenuBackActivate(true);
				isCoinShop = false;
				//fadeOut();
			}else if( btnstate == buttonState.RACEMODE){
				btnstate = buttonState.WAIT;
				finishUpPanel();
				fadeOut();
				MenuBackActivate(true);
				InitPanel("RACEMODE");
				EnableCamera(string.Empty);
				isCoinShop = false;
			}else  if( btnstate == buttonState.CLAN){
				btnstate = buttonState.WAIT;
				finishUpPanel();
				InitPanel("CLAN");
				EnableCamera(string.Empty);
				fadeOut();
				MenuBackActivate(true);
				isCoinShop = false;
			}else {
				isCoinShop = false;
				fadeIn();
			}
			
		}break;*/
	/*
		case buttonState.MYCREW:
		{
			beforeState =btnstate;
			finishUpPanel();
			btnstate = buttonState.WAIT;
			ShowInfoTipWindow(strTip);
			InitPanel("MYCREW");
			strTip = string.Empty;
		}
			break;
		case buttonState.MYCARD:
		{
			beforeState =btnstate;
			finishUpPanel();
			btnstate = buttonState.WAIT;
			menuCenter.InvenWindow();
			HiddenInfoTipWindow();
			InitPanel("MYCARD");
			strTip = "Team";
		}
			break;*/
	private GameObject InvenObj;
	private bool isModeReturn = false;
	void InitPanel(string str){
		var _prefab = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;

		switch(str){
		case "MY_SHOP":
		{
			disabelMenu();
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject = MenuBottom.transform.FindChild("Menu_Container").gameObject;
			activeObject.SetActive(true);
			var temp = _prefab.makeLuckyBox(MenuBottom.transform) as GameObject;
			onBackBack = delegate {
				temp.SetActive(false);
			};
		
		}break;
		case "TEAM":{
			AccountManager.instance.SponTimeCheck(1);
			disabelMenu();
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			var myObj = GameObject.FindGameObjectWithTag("UpAnchor") as GameObject;
			var temp = _prefab.makeTeam(myObj.transform.GetChild(0)) as GameObject;
		//	var temp = _prefab.makeTeam(MenuBottom.transform) as GameObject;
			onBackBack = delegate {
				temp.SetActive(false);
			};
		}
			break;
		case "ONINVEN":{
			disabelMenu();
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject = MenuBottom.transform.FindChild("Menu_CarParts").gameObject;
			activeObject.SetActive(true);
			activeObject.transform.localPosition = Vector3.zero;
			InvenObj = _prefab.makeInven(MenuBottom.transform) as GameObject;
			onBackBack = delegate {
				InvenObj.SetActive(false);
			};
			InvenObj.SendMessage("OnInitInvenMenu",SendMessageOptions.DontRequireReceiver);
		}
			break;
		/*case "NOTONINVEN":{
			disabelMenu();
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject = MenuBottom.transform.FindChild("Menu_Inven").gameObject;
			activeObject.SetActive(true);//Utility.Log(str);
		}
			break;
		case "ONONINVEN":{
			disabelMenu();
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject = MenuBottom.transform.FindChild("Menu_CarParts").gameObject;
			activeObject.SetActive(true);
			//Utility.Log(str);
		}
			break;*/
		case "CLAN":{
			disabelMenu();
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			var myObj = GameObject.FindGameObjectWithTag("UpAnchor") as GameObject;
			var temp = _prefab.makeClan(myObj.transform.GetChild(0)) as GameObject;
			temp.GetComponent<ClanWindow>().playMenuAni();
			onBackBack = delegate {
				//temp.GetComponent<ClanWindow>().playMenuReverseAni();
				temp.SetActive(false);
			};
		}
			break;

		case "RACEMODE":{
			AccountManager.instance.SponTimeCheck(1);
			disabelMenu();
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
		//	Utility.LogWarning(GV.gRaceMode + " racemode");
			if(GV.gRaceMode == 0) {
				if(onBackBack != null)
					onBackBack();
			}
			var temp = _prefab.makeRaceMenu(MenuBottom.transform) as GameObject;
			temp.GetComponent<RaceMenuStart>().ResetAnimation();
			if(isModeReturn){
				temp.SendMessage("InitButtonStatus",SendMessageOptions.DontRequireReceiver);
			}else{
			
			}

			onBackBack = delegate {
			
				temp.SetActive(false);
			
			};
		}
			break;
		case "TORACEMODECLAN":{
			AccountManager.instance.SponTimeCheck(1);
			disabelMenu();
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			var temp = _prefab.makeRaceMenu(MenuBottom.transform) as GameObject;

			if(isModeReturn){
				temp.SendMessage("InitButtonStatus",SendMessageOptions.DontRequireReceiver);
			}else{
				
			}
			onBackBack();
			onBackBack = delegate {
				temp.SetActive(false);
				//HiddenInfoTipWindow();
			};
		}
			break;
		case "TEAMINFO":
		{
			disabelMenu();
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject = MenuBottom.transform.FindChild("Menu_TeamInfo_1").gameObject;
			activeObject.SetActive(true);
			if(!isTeamInfo){
				isTeamInfo = true;
				ChangeTeamInfo();
			}
		
		}break;
		case "LOBBY":{
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Lobby").gameObject;
			activeObject.SetActive(true);
			activeObject.SendMessage("ResetRepairBtn", SendMessageOptions.DontRequireReceiver);
			AccountManager.instance.SponTimeCheck(1);
			isTeamRotate = false;
		}
			break;
		case "MYCAR_STOCK":{
		//	Utility.LogWarning("Mycarstock");
			RotateTable.SetActive(true);
			if(!isTeamRotate){
				isTeamRotate = true;
				RotateTable.SendMessage("ChangeTarget",true,SendMessageOptions.DontRequireReceiver);
			}
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_MyCar_Grid").gameObject;
			activeObject.SetActive(true);
			var test = activeObject.GetComponent<UIdragPanelInit>() as UIdragPanelInit;
			if(test== null){
				activeObject.AddComponent<UIdragPanelInit>();
			}
			test = null;
			if(raceinfo != null) {
				HiddenInfoWindow();
			}
			if(status != null) {
				hiddenStatusWindow(status);
			}


			var _parent = activeObject.transform.FindChild("View").GetChild(0).gameObject;
			CreateTeamCars(_parent, "Stock");
			//var tr = activeObject.transform.GetChild(0) as Transform;
			for(int i = 0 ; i < _parent.transform.childCount;i++){
				var obj1 = _parent.transform.GetChild(i).GetChild(0).FindChild("Select").gameObject;
				if(obj1.activeSelf){
					if(status != null) {
						hiddenStatusWindow(status);
					}
					var obj = obj1.transform.parent.gameObject as GameObject;
					status = _prefab.makeCarStatus(MenuBottom.transform, true);
					status.name = obj.name;
					var _state = status.GetComponent<StatsUpInfo>() as StatsUpInfo;
					if(_state == null) _state=status.AddComponent<StatsUpInfo>();
					_state.ChangeMyTeamStatus();
					raceinfo =_prefab.MakeCarInfo(MenuBottom.transform) as GameObject;

					string[] names  = obj.name.Split('_');
					int _id = int.Parse(names[0]);
					var _in = raceinfo.GetComponent<ViewTeamCarInfo>();
					if(_in == null) _in = raceinfo.AddComponent<ViewTeamCarInfo>();
					_in.ShowCarInfomation(obj1,_id);
					_in.InitInfoSet(raceinfo,_id.ToString());
					_in = null;
					break;
				}
			}
			var _card = raceinfo.GetComponent<EvoInit>() as EvoInit;
			if(_card == null) _card = raceinfo.AddComponent<EvoInit>();
			_card.Show(null);
		//	string str1 = null;
			_card.onFinish = delegate {
				raceinfo.GetComponent<EvoInit>().tempHidden();
				var tw = raceinfo.GetComponent<TweenAction>();
				if(tw == null) tw = raceinfo.AddComponent<TweenAction>();
				tw.tempHidden();
				tw = status.GetComponent<TweenAction>();
				if(tw == null) tw = status.AddComponent<TweenAction>();
				tw.tempHidden();
				tw = MenuBottom.GetComponent<TweenAction>();
				if(tw == null) tw = MenuBottom.AddComponent<TweenAction>();
				tw.tempHidden();
			};
		}break;
		case "MYCAR_TOUR":{
			//Utility.LogWarning("MYCAR_TOUR");
	
			RotateTable.SetActive(true);
			if(!isTeamRotate){
				isTeamRotate = true;
				RotateTable.SendMessage("ChangeTarget",true,SendMessageOptions.DontRequireReceiver);
			}
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_MyCar_Grid").gameObject;
			activeObject.SetActive(true);
			var test = activeObject.GetComponent<UIdragPanelInit>() as UIdragPanelInit;
			if(test== null){
				activeObject.AddComponent<UIdragPanelInit>();
			}
			test = null;
			if(raceinfo != null) {
				HiddenInfoWindow();
			}
			if(status != null) {
				hiddenStatusWindow(status);
			}

			var _parent = activeObject.transform.FindChild("View").GetChild(0).gameObject;
			
		
			CreateTeamCars(_parent, "Tour");
	
			for(int i = 0 ; i < _parent.transform.childCount;i++){
				var obj1 = _parent.transform.GetChild(i).GetChild(0).FindChild("Select").gameObject;
				if(obj1.activeSelf){
					if(status != null) {
						hiddenStatusWindow(status);
					}
					var obj = obj1.transform.parent.gameObject as GameObject;
					status = _prefab.makeCarStatus(MenuBottom.transform, true);
					status.name = obj.name;
					var _state = status.GetComponent<StatsUpInfo>() as StatsUpInfo;
					if(_state == null) _state=status.AddComponent<StatsUpInfo>();
					_state.ChangeMyTeamStatus();
					raceinfo =_prefab.MakeCarInfo(MenuBottom.transform) as GameObject;

					string[] names  = obj.name.Split('_');
					int _id = int.Parse(names[0]);
					var _in = raceinfo.GetComponent<ViewTeamCarInfo>() as ViewTeamCarInfo;
					if(_in == null) _in = raceinfo.AddComponent<ViewTeamCarInfo>();
					_in.ShowCarInfomation(obj1,_id);
					_in.InitInfoSet(raceinfo,_id.ToString());
					_in = null;
					break;
				}
			}
			var _card = raceinfo.GetComponent<EvoInit>() as EvoInit;
			if(_card == null) _card = raceinfo.AddComponent<EvoInit>();
			_card.Show(null);
			//string str1 = null;
			_card.onFinish = delegate {
				raceinfo.GetComponent<EvoInit>().tempHidden();
				var tw = raceinfo.GetComponent<TweenAction>();
				if(tw == null) tw = raceinfo.AddComponent<TweenAction>();
				tw.tempHidden();
				tw = MenuBottom.GetComponent<TweenAction>();
				if(tw == null) tw = MenuBottom.AddComponent<TweenAction>();
				tw.tempHidden();
				tw = status.GetComponent<TweenAction>();
				if(tw == null) tw = status.AddComponent<TweenAction>();
				tw.tempHidden();
			};
		}break;
		case "UPGRADE_CAR":{
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Upgrade_Car").gameObject;
			activeObject.SetActive(true);
			if(raceinfo != null) {
				HiddenInfoWindow();
			}
			if(status != null) {
				hiddenStatusWindow(status);
			}
			
			status = _prefab.makeCarStatus(MenuBottom.transform, true);
			raceinfo = _prefab.makeCarStatus(MenuBottom.transform,false);
			var _card = raceinfo.GetComponent<EvoInit>() as EvoInit;
			if(_card == null) _card = raceinfo.AddComponent<EvoInit>();
			_card.Show(null);
			string str1 = null;
			_card.onFinish = delegate {
				raceinfo.GetComponent<EvoInit>().tempHidden();
				var tw = raceinfo.GetComponent<TweenAction>();
				if(tw == null) tw = raceinfo.AddComponent<TweenAction>();
				tw.tempHidden();
				//raceinfo.GetComponent<UpgradeCarInfo>().UpdateLevel();
				raceinfo.GetComponent<LevelUpAction>().UpdateCarUpgarde();
				tw = status.GetComponent<TweenAction>();
				if(tw == null) tw = status.AddComponent<TweenAction>();
				tw.tempHidden();
				tw = MenuBottom.GetComponent<TweenAction>();
				if(tw == null) tw = MenuBottom.AddComponent<TweenAction>();
				tw.tempHidden();
				status.SendMessage("SelectedCarPartStatusChange",str1,SendMessageOptions.DontRequireReceiver);
			};
			ShowStatusWindow(status);
			if(GV.SelectedTeamID%2 == 0){
				if(CarUpNameStock == null)  {str1 = "Body"; CarUpNameStock = str1;}
				else str1 = CarUpNameStock;
			}else{
				if(CarUpNameTour == null)  {str1 = "Body"; CarUpNameTour = str1;}
				else str1 = CarUpNameTour;
			}

			status.SendMessage("SelectedCarPartStatusChange",str1, SendMessageOptions.DontRequireReceiver);
			//raceinfo.SendMessage("UpgradeInit", str1, SendMessageOptions.DontRequireReceiver);		
			raceinfo.SendMessage("InitUpgradeCarInfo", str1, SendMessageOptions.DontRequireReceiver);
			//raceinfo.SendMessage("InitUpgradeCarInfo", str1, SendMessageOptions.DontRequireReceiver);
			for(int i = 0; i < activeObject.transform.childCount; i++){
				var temp = activeObject.transform.GetChild(i).gameObject;
				var temp1 = temp.transform.FindChild("Select").gameObject;
				if(temp.name.Equals(str1)){
					temp1.SetActive(true);
				}else temp1.SetActive(false);
			}
		}
			break;
			
		case "UPGRADE_CREW":{
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Upgrade_Crew").gameObject;
			activeObject.SetActive(true);
			if(raceinfo != null) {
				//DestroyImmediate(raceinfo);
				HiddenInfoWindow();
			}
			if(status != null) {
				hiddenStatusWindow(status);
			}
			status = _prefab.makeCrewStatus(MenuBottom.transform, true);
			raceinfo = _prefab.makeCrewStatus(MenuBottom.transform,false);
			/* crew build up*/
			var _card = raceinfo.GetComponent<EvoInit>() as EvoInit;
			if(_card == null) _card = raceinfo.AddComponent<EvoInit>();
			_card.Show(null);
			_card.onFinish = delegate {
				raceinfo.GetComponent<EvoInit>().tempHidden();
				var tw = raceinfo.GetComponent<TweenAction>();
				if(tw == null) tw = raceinfo.AddComponent<TweenAction>();
				tw.tempHidden();
				//raceinfo.GetComponent<UpgradeCrewInfo>().UpdateLevel();
				raceinfo.GetComponent<LevelUpAction>().UpdateCrewUpgrade();
				tw = status.GetComponent<TweenAction>();
				if(tw == null) tw = status.AddComponent<TweenAction>();
				tw.tempHidden();
				tw = MenuBottom.GetComponent<TweenAction>();
				if(tw == null) tw = MenuBottom.AddComponent<TweenAction>();
				tw.tempHidden();
			}; 
			/*  */
			ShowStatusWindow(status);
			string str1 = null;
			if(GV.SelectedTeamID%2==0){
				if(CrewUpNameStock == null)  {str1 = "Driver"; CrewUpNameStock = str1;}
				else str1 = CrewUpNameStock;
			}else{
				if(CrewUpNameTour == null)  {str1 = "Driver";CrewUpNameTour = str1;}
				else str1 = CrewUpNameTour;
			}
			status.SendMessage("SelectedCrewStatusChange",str1,SendMessageOptions.DontRequireReceiver);
			//raceinfo.SendMessage("UpgradeInit", str1, SendMessageOptions.DontRequireReceiver);
			raceinfo.SendMessage("InitUpgradeCrewInfo", str1, SendMessageOptions.DontRequireReceiver);
			for(int i = 0; i < activeObject.transform.childCount; i++){
				var temp = activeObject.transform.GetChild(i).gameObject;
				var temp1 = temp.transform.FindChild("Select").gameObject;
				if(temp.name.Equals(str1)){
					temp1.SetActive(true);
				}else temp1.SetActive(false);
			}
		}
			break;
			
		case "MAP":{
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Map").gameObject;
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject.SetActive(true);
			//var comp = activeObject.GetComponent<Gamemodeaction>() as Gamemodeaction;
			//InitMap();
			//_mapaction.ShowRaceInfo(worldmap.transform, "Map");
		//	if(_mode == null) {
		//		_mode = activeObject.AddComponent<Gamemodeaction>() ;
		///		_mode.SetMapRaceInit = _mapaction;
		//		_mapaction.SetGamemodeaction = _mode;
		//	}//as Gamemodeaction;
		//	_mode.InitParent(mapgrid, gameObject);
		}
			break;
		case "MAP_CLAN":{
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Map").gameObject;
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject.SetActive(true);
		//	activeObject.transform.GetChild(0).GetComponent<RaceModeButton>().setBGLine(false);
			activeObject.transform.GetChild(0).GetComponent<RaceModeButton>().setActivateButton("Clan");
			//InitMap();
			//_mapaction.ShowRaceInfo(worldmap.transform, "Clan");
		//	if(_mode == null) {
		//		_mode = activeObject.AddComponent<Gamemodeaction>() ;
			//	_mode.InitParent(mapgrid, gameObject);
				//_mode.SetMapRaceInit = _mapaction;
				//_mapaction.SetGamemodeaction = _mode;
		//	}//as Gamemodeaction;
		//	_mode.showInfoWindow("Clan");
			setRaceSubWindow("Clan");

		}break;
		case "MAP_RANK":{
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Map").gameObject;
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject.SetActive(true);
			activeObject.transform.GetChild(0).GetComponent<RaceModeButton>().setActivateButton("Weekly");
	//		activeObject.transform.GetChild(0).GetComponent<RaceModeButton>().setBGLine(true);
		//	InitMap();
		//	_mapaction.ShowRaceInfo(worldmap.transform,"Weekly");
		//	if(_mode == null) {
		//		_mode = activeObject.AddComponent<Gamemodeaction>() ;
			//	_mode.SetMapRaceInit = _mapaction;
			//	_mapaction.SetGamemodeaction = _mode;
			//	_mode.InitParent(mapgrid, gameObject);
		//	}//as Gamemodeaction;
		//	_mode.showInfoWindow("Weekly");
			setRaceSubWindow("Weekly");
		}break;
		case "MAP_RACE":{
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Map").gameObject;
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject.SetActive(true);
			activeObject.transform.GetChild(0).GetComponent<RaceModeButton>().setActivateButton("Race");
		//	activeObject.transform.GetChild(0).GetComponent<RaceModeButton>().setBGLine(true);
		//	InitMap();
		//	_mapaction.ShowRaceInfo(worldmap.transform, "Race");
		//	if(_mode == null) {
		//		_mode = activeObject.AddComponent<Gamemodeaction>() ;
			//	_mode.SetMapRaceInit = _mapaction;
			//	_mapaction.SetGamemodeaction = _mode;
			//	_mode.InitParent(mapgrid, gameObject);
		//	}//as Gamemodeaction;
		
		//	_mode.showInfoWindow("Race");
			setRaceSubWindow("Regular");
		}break;
		case "MAP_CHAMPION":{
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Map").gameObject;
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject.SetActive(true);
			activeObject.transform.GetChild(0).GetComponent<RaceModeButton>().setActivateButton("Champion");
		//	activeObject.transform.GetChild(0).GetComponent<RaceModeButton>().setBGLine(true);
			//InitMap();
			//_mapaction.ShowRaceInfo(worldmap.transform,"Champion");
	//		if(_mode == null) {
		//		_mode = activeObject.AddComponent<Gamemodeaction>() ;
			//	_mode.SetMapRaceInit = _mapaction;
			//	_mapaction.SetGamemodeaction = _mode;
			//	_mode.InitParent(mapgrid, gameObject);
		//	}//as Gamemodeaction;
		
		//	_mode.showInfoWindow("Champion");
			setRaceSubWindow("Champion");
		}break;
		case "MAP_PVP":{
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Map").gameObject;
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject.SetActive(true);
			activeObject.transform.GetChild(0).GetComponent<RaceModeButton>().setActivateButton("PVP");
			setRaceSubWindow("PVP");
		}break;
		case "MAP_EVENT":{
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Map").gameObject;
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject.SetActive(true);
			activeObject.transform.GetChild(0).GetComponent<RaceModeButton>().setActivateButton("Event");
		//	activeObject.transform.GetChild(0).GetComponent<RaceModeButton>().setBGLine(false);
		//	InitMap();
		//	_mapaction.ShowRaceInfo(worldmap.transform,"Champion");
		//	if(_mode == null) {
			//	_mode = activeObject.AddComponent<Gamemodeaction>() ;
			//	_mode.SetMapRaceInit = _mapaction;
			//	_mapaction.SetGamemodeaction = _mode;
			//	_mode.InitParent(mapgrid, gameObject);
		//	}//as Gamemodeaction;
		//	_mode.showInfoWindow("Event");
			setRaceSubWindow("Event");
		}break;
		case "SPONSOR_STOCK":
		{
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Sponsor").gameObject;
			activeObject.SetActive(true);
			var grid = activeObject.transform.FindChild("View").transform.FindChild("Grid") as Transform;
			GetComponent<ShopItemCreate>().SettingGridItemShop(grid, "Slot_Sponsor");
		}break;
		case "SPONSOR_TOUR":
		{
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Sponsor").gameObject;
			activeObject.SetActive(true);
			var grid = activeObject.transform.FindChild("View").transform.FindChild("Grid") as Transform;
			GetComponent<ShopItemCreate>().SettingGridItemShop(grid, "Slot_Sponsor");
		}
			break;
		case "CAR_SHOP":
		{
			if(!RotateTable.activeSelf)
			{	RotateTable.SetActive(true);
				RotateTable.SendMessage("ChangeTarget",false,SendMessageOptions.DontRequireReceiver);
			}
			disabelMenu();
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject = MenuBottom.transform.FindChild("Menu_DealerRecommend").gameObject;
			activeObject.SetActive(true);
			string strDeal = null;
			if(!EncryptedPlayerPrefs.HasKey("DealerCar")){
				EncryptedPlayerPrefs.SetInt("DealerCar", 1000);
			}
			int mCar = EncryptedPlayerPrefs.GetInt("DealerCar");
			if(GV.bRaceLose){
				
				strDeal = mCar.ToString()+"_Dealer_SS";
				activeObject.transform.FindChild("event_On").gameObject.SetActive(true);
				activeObject.transform.FindChild("event_Off").gameObject.SetActive(false);
				
			}else{
				strDeal = mCar.ToString()+"_DealerNo_SS";
				activeObject.transform.FindChild("event_On").gameObject.SetActive(false);
				activeObject.transform.FindChild("event_Off").gameObject.SetActive(true);
				var tw = 	activeObject.transform.FindChild("event_Off").GetComponent<TweenPosition>() as TweenPosition;
				tw.Reset();
				tw.enabled = true;
				int dbuy = EncryptedPlayerPrefs.GetInt("DealerBuy");
				if(dbuy != 10){
				//	int a = EncryptedPlayerPrefs.GetInt("DealerBuy");
					if(dbuy == 0){
						activeObject.SendMessage("ChangeTextLabel",1,SendMessageOptions.DontRequireReceiver);
					}else if(dbuy == 1){
						activeObject.SendMessage("ChangeTextLabel",1,SendMessageOptions.DontRequireReceiver);
					}else if(dbuy == 2){	// 구매 했다.
						activeObject.SendMessage("ChangeTextLabel",0,SendMessageOptions.DontRequireReceiver);
					}
					
				}else{
					//추천 차량이 없다. 
					activeObject.SendMessage("ChangeTextLabel",1,SendMessageOptions.DontRequireReceiver);
				}
			}
			GetComponent<ShopItemCreate>().DestroyStatusWin();
			_table.ItemClick(strDeal, ()=>{
				GetComponent<ShopItemCreate>().ResetWindow(strDeal, gameObject);
			}, true );
			// 1000_Dealer_D;
		}
			break;

		case "CAR_SHOP_DEAL":
		{
			if(!RotateTable.activeSelf)
			{	RotateTable.SetActive(true);
				RotateTable.SendMessage("ChangeTarget",false,SendMessageOptions.DontRequireReceiver);
			}
			disabelMenu();
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject = MenuBottom.transform.FindChild("Menu_DealerRecommend").gameObject;
			activeObject.SetActive(true);
			string strDeal = null;

			if(!EncryptedPlayerPrefs.HasKey("DealerCar")){
				EncryptedPlayerPrefs.SetInt("DealerCar", 1000);
			}
			int mCar = EncryptedPlayerPrefs.GetInt("DealerCar");
			if(GV.bRaceLose){

				strDeal = mCar.ToString()+"_Dealer_SS";
				activeObject.transform.FindChild("event_On").gameObject.SetActive(true);
				activeObject.transform.FindChild("event_Off").gameObject.SetActive(false);

			}else{
				strDeal = mCar.ToString()+"_DealerNo_SS";
				activeObject.transform.FindChild("event_On").gameObject.SetActive(false);
				activeObject.transform.FindChild("event_Off").gameObject.SetActive(true);
				var tw = 	activeObject.transform.FindChild("event_Off").GetComponent<TweenPosition>() as TweenPosition;
				tw.Reset();
				tw.enabled = true;

				int dbuy = EncryptedPlayerPrefs.GetInt("DealerBuy");
				if(dbuy != 10){
					if(dbuy == 0){
						activeObject.SendMessage("ChangeTextLabel",1,SendMessageOptions.DontRequireReceiver);
					}else if(dbuy == 1){
						activeObject.SendMessage("ChangeTextLabel",1,SendMessageOptions.DontRequireReceiver);
					}else if(dbuy == 2){	// 구매 했다.
						activeObject.SendMessage("ChangeTextLabel",0,SendMessageOptions.DontRequireReceiver);
					}

				}else{
					//추천 차량이 없다. 
					activeObject.SendMessage("ChangeTextLabel",1,SendMessageOptions.DontRequireReceiver);
				}

				//	if(GV.gBuyDealerCar == 0){

			//		activeObject.SendMessage("ChangeTextLabel",0,SendMessageOptions.DontRequireReceiver);
			//	}else{

					
			//	}


			}
			GetComponent<ShopItemCreate>().DestroyStatusWin();
			_table.ItemClick(strDeal, ()=>{
				GetComponent<ShopItemCreate>().ResetWindow(strDeal, gameObject);
			},true );

			/*
			if(GV.gDealerCarID != 0){

			}else {
			
			}
			//	var grid = activeObject.transform.FindChild("View").FindChild("Grid") as Transform;
			//GetComponent<ShopItemCreate>().ChangeCarGridItems(grid, "Slot_Car");
			
		
			// 1000_Dealer_D;
			*/

		}
			break;
		case "CAR_SHOWROOM":
		{
			if(!RotateTable.activeSelf)
			{	RotateTable.SetActive(true);
				RotateTable.SendMessage("ChangeTarget",false,SendMessageOptions.DontRequireReceiver);
			}
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Shop_Car").gameObject;
			activeObject.SetActive(true);
			var test = activeObject.GetComponent<UIdragPanelInit>() as UIdragPanelInit;
			if(test== null){
				activeObject.AddComponent<UIdragPanelInit>();
			}
			var grid = activeObject.transform.FindChild("View").FindChild("Grid") as Transform;
			GetComponent<ShopItemCreate>().ChangeCarGridItems(grid, "Slot_Car_ShowRoom");
			//Utility.LogWarning("SHOP CAR");
		}
			break;
			/*case "Shop_Car":
		{
			if(!RotateTable.activeSelf)
			{	RotateTable.SetActive(true);
				RotateTable.SendMessage("ChangeTarget",false,SendMessageOptions.DontRequireReceiver);
			}
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_Shop_Car").gameObject;
			activeObject.SetActive(true);
			var test = activeObject.GetComponent<UIdragPanelInit>() as UIdragPanelInit;
			if(test== null){
				activeObject.AddComponent<UIdragPanelInit>();
			}
			var grid = activeObject.transform.FindChild("View").FindChild("Grid") as Transform;
			GetComponent<ShopItemCreate>().SettingGridItemShop(grid, "Slot_Car");
			//Utility.LogWarning("SHOP CAR");
		}
			break;

case "Shop_Crew":
		{
			disabelMenu();
			if(!RotateTable.activeSelf){
				RotateTable.SetActive(true);
				RotateTable.SendMessage("ChangeTarget",false,SendMessageOptions.DontRequireReceiver);
			}
			activeObject = MenuBottom.transform.FindChild("Menu_Shop_Crew").gameObject;
			activeObject.SetActive(true);
			var test = activeObject.GetComponent<UIdragPanelInit>() as UIdragPanelInit;
			if(test== null){
				activeObject.AddComponent<UIdragPanelInit>();
			}
			test = null;
			
			var grid = activeObject.transform.FindChild("View").FindChild("Grid") as Transform;
			GetComponent<ShopItemCreate>().SettingGridItemShop(grid, "Slot_Crew");
		}
			break;
	case "Shop_Coin":
		{
			RotateTable.SetActive(false);
			disabelMenu();
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			activeObject = MenuBottom.transform.FindChild("Menu_Shop_Coin").gameObject;
			activeObject.SetActive(true);
			//GetComponent<SponsorMenu>().SettingGridItemShop(grid, "Slot_Coin");
		}break;
			  */

			/*case "MYCREW":{
			disabelMenu();
			activeObject = MenuBottom.transform.FindChild("Menu_MyCrew_Grid").gameObject;
			activeObject.SetActive(true);
			if(activeObject.GetComponent<UIdragPanelInit>() == null){
				activeObject.AddComponent<UIdragPanelInit>();
			}
			if(raceinfo != null) {
				HiddenInfoWindow();
			}
			var _parent = activeObject.transform.FindChild("View").GetChild(0).gameObject;
			CreateGirdPanelCrewItem(_parent);
			
			for(int i = 0; i <  _parent.transform.childCount; i++){
				var obj1 = _parent.transform.GetChild(i).GetChild(0).FindChild("Select").gameObject;
				if(obj1.activeSelf){
					if(status != null) {
						hiddenStatusWindow(status);
					}
					raceinfo = _prefab.MakeCrewInfo(MenuBottom.transform) as GameObject;
					var obj = obj1.transform.parent.gameObject;
					int _id = int.Parse(obj.name);
					var _in = raceinfo.GetComponent<Infoaction>() as Infoaction;
					if(_in == null) _in = raceinfo.AddComponent<Infoaction>() as Infoaction;
					_in.ShowCrewInfomation(obj1, _id);
					int tmepid = Base64Manager.instance.GlobalEncoding(Global.MyCrewID);
					_in.InitInfoSet(raceinfo, tmepid.ToString()); _in = null;
					break;
				}
			}
		}
			break;
		case "MYCARD":{
			disabelMenu();
			MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(false);
			if(raceinfo != null) {
				HiddenInfoWindow();
			}
		}
			break;*/
		default:
			Utility.LogError("initpanel " + str);
			break;
		}
		
		AddTweenPosition(MenuBottom, new Vector3(0,-200,0), Vector3.zero, 0.5f);
		DestroyImmediate(_prefab);	
	}
	
	public void settingDealInfo(){
		if(raceinfo != null) {
			HiddenInfoWindow();
		}
		if(status != null) {
			hiddenStatusWindow(status);
		}
	}

	#region onGUI
	public Texture2D blackTexture;
	private float alpha = 1f;
	private float fadeDir = 1.0f; 
	private bool m_bStop;
	private float tempalpha;
	private bool m_balphainit;
	
	// 0 일때 밝아지고
	// 1일때 가장 어두워 진다.
	// 시작하기 위해서는 검은색 텍스쳐가 한장 필요하다.
	
	void StopState()
	{
		if(m_bStop)
		{
			if(!m_balphainit)
			{
				tempalpha = alpha;
				m_balphainit = true;
			}
			
			alpha = Mathf.Clamp01(0.5f);  
		}
		else
		{
			if(m_balphainit)
			{
				m_balphainit = false;
				alpha = tempalpha;
			}
			alpha = Mathf.Clamp01(alpha);  
		}
		if(alpha == 1 || alpha == 0){
			ChangeGameState();
		}
	}
	
	// Update is called once per frame
	
	void  fadeIn(){ 
		fadeDir = 1;  
		//	m_bFadeIn = false;
		m_balphainit = false;
		m_bStop = false;
		return;
	} 
	
	void  fadeOut(){ 
		fadeDir = -1;    
		//	m_bFadOut = false;
		m_balphainit = false;
		m_bStop = false;
		return;
		
	} 
	
	
	
	void OnGUI(){
		if(!Global.isLoadFinish){
			//Utility.Log("Ongui");
			return;
		}
		alpha += fadeDir * Mathf.Clamp01(Time.deltaTime);
		alpha = Mathf.Clamp01(alpha);
		if(alpha == 1 || alpha == 0){
			ChangeGameState();
			//Utility.LogWarning("Alpha " + alpha + "deleta time : " + Time.time);
		}  
		//Utility.LogWarning("Alpha " + alpha + "deleta time : " + Time.deltaTime);
		
		GUI.color = new Color(0,0,0,alpha);
		//Utility.Log("alpha" + alpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture); 
		
		return;
	}
	#endregion


	void OnApplicationPause(bool isStatus){
		if(isStatus){
			#if UNITY_ANDROID && !UNITY_EDITOR
			UserDataManager.instance.myGameDataSave();		
			#endif
			UserDataManager.instance.PauseGame();
		}else{
			UserDataManager.instance.TapjoyRewardCheck();
			UserDataManager.instance.ReSumeGame();
		}
	}
	
	void OnApplicationFocus(bool isStatus){
		
		//Utility.Log("Focus " + isStatus);
	}

}
