using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public partial class LobbyManager : MonoBehaviour {
	void OnLevelWasLoaded(int level){
		//Utility.Log("onLevel Lobby" + level);
	}
	
	void EnableCamera(string cameraName){
		foreach(Camera _cam in LobbyCamList){
			if(string.Equals(cameraName, _cam.name) == true){
				_cam.enabled =true;
			}else{
				_cam.enabled = false;
			}
		}

		if(cameraName == "Lobby"){
			//transform.FindChild("panel")
			lobby.GetComponent<crewSpeak>().LobbyCamCheck(true);
		}else{
			lobby.GetComponent<crewSpeak>().LobbyCamCheck(false);
		}
	}
	
	Camera CameraAnimation(string cameraName){
		Camera cam = null;
		foreach(Camera _cam in LobbyCamList){
			if(string.Equals(cameraName, _cam.name) == true){
				cam = _cam;
				break;
			}
		}
		return cam;
	}
	
	void RTSCamControl(bool b){
		Camera cam = CameraAnimation("Lobby");
		cam.GetComponent<RTSCamModifiy>().enabled = b;
	}
	/*
	void ResetGridItem(){
		if(isMap) return;
		if(!isLobby) return;
		Global.isPopUp = true;
		var temp = MenuBottom.transform.FindChild("Menu_MyCrew_Grid").GetChild(0).GetChild(0)  as Transform;
		for(int i = 0; i < temp.childCount;i++){
			//Destroy(temp.GetChild(i).gameObject);
		}
		temp = MenuBottom.transform.FindChild("Menu_MyCar_Grid").GetChild(0).GetChild(0)  as Transform;
		for(int i = 0; i < temp.childCount;i++){
			//Destroy(temp.GetChild(i).gameObject);
		}
		temp = MenuBottom.transform.FindChild("Menu_Shop_Car").GetChild(0).GetChild(0)   as Transform;
		for(int i = 0; i < temp.childCount;i++){
			//Destroy(temp.GetChild(i).gameObject);
		}
		temp = MenuBottom.transform.FindChild("Menu_Shop_Crew").GetChild(0).GetChild(0)   as Transform;
		for(int i = 0; i < temp.childCount;i++){
			//Destroy(temp.GetChild(i).gameObject);
		}
		temp = worldmap.transform.GetChild(0).GetChild(0) as Transform;
		for(int i = 0; i < temp.childCount;i++){
			//Destroy(temp.GetChild(i).gameObject);
		}
	}*/
	
	void OnEnable(){
	
	}
	
	void Resolutions()
	{			
		Camera cam = this.transform.Find("Camera").GetComponent<Camera>();
		float width = (float)Screen.width;
		float height = (float)Screen.height;
		int type = 0;
		float x = width/height;
		if( x < 1.4f){
			cam.orthographicSize = 1.2f;
		}else if( x < 1.55f){
			cam.orthographicSize = 1.1f;
		}else if(x < 1.8f){
			cam.orthographicSize = 1.0f;
		}
	}

	

	void OnDisable(){
	
	
	}

	void Awake(){
		if(!Global.isRaceTest){
			
		}else{

		}
		SceneManager.instance.settingRaceRenderSetting("Lobby");
		GV.bLoding = true;
		StartCoroutine("remainGetAPI");
	}
	
	void Start () {
		camAni = CameraAnimation("Upgrade").GetComponent<cameraAniCtrl>();
		camAni_Tour = CameraAnimation("Upgrade").GetComponent<cameraAniCtrl>();
		_table = TableShop.GetComponent<TableShopaction>();
		if(Global.isLobby){
			isLobby = true;
			TableManager.Initialize();
			StartCoroutine ("TableInitialize"); 
		}else{
			
		}
		Resolutions();
		GoogleAnalyticsV4.instance.LogScreen(new AppViewHitBuilder().SetScreenName("Lobby"));
		//GameObject.Find("ManagerGroup").SendMessage("SetSendBirdUI",SendMessageOptions.DontRequireReceiver);
	}
	
	private bool isLevelCheck = true;
	public GameObject DirectLight;
	public void LobbyLoadResource(){
	
		transform.GetChild(0).GetComponent<Camera>().enabled = true;
		DirectLight.SetActive(true);
		if(!Global.isRaceTest) GameObject.Find("Audio").SendMessage("StartLobbyMusic");
		Global.isLoadFinish = true;
		MenuBackActivate(false);
		btnstate = buttonState.TITLETOLOBBY;
		ChangeGameState();
		//fadeIn();

	//	TotalCamera.SetActive(true);
	}


	public IEnumerator LobbyResourceInitailze(){
		yield return null;

	}


	void SetChampionSeason(){
		if(GV.ChSeasonID > 6030){
			GV.ChSeasonID = 6030;
		}
		//GV.ChSeasonID =6001;
		if(Application.isEditor){
			if(CClub.champID == 0){
			}else{
				GV.ChSeasonID = CClub.champID;
				//CClub.gReview = 2;
				//GV.bRaceLose  = true;
			}
		}
		Common_Mode_Champion.Item _item = Common_Mode_Champion.Get(GV.ChSeasonID);
		AccountManager.instance.ChampItem = _item;
		GV.ChSeason = _item.Season;
		GV.ChSeasonLV = _item.SeasonLV;
		Global.gRewardId = _item.R_ID;
		string strSeason = "s_" + GV.ChSeason.ToString();
		spriteSeason.spriteName = strSeason;

	}


	IEnumerator TableInitialize(){

		for (;;) {
			if(TableManager.initialized){
				/*if(GV.CarFlag == 1){
					GV.CarFlag = 0;
					//	GV.AddTeamInfo(10);	
				}*/
				GV.TeamInitSetting();
				GV.ModifyMyCarList();
				SetChampionSeason();
				InitLevelText();
				InitFuelCount();
				EnvInit();
				SetElevatorCar();
				ResetLevelButton(true);
				ResetLevelButton(false);
				Common_Cost.Item _item1 = Common_Cost.Get(8501);
				Global.gConvertDollar = _item1.Change;
				ChangeTeamLV("Stock", 0);

				//SetClubFlag();
				SetDealerCarEvent();
				if(Global.isRaceTest) LobbyLoadResource();
				yield break;
			}else{
				yield return null;
			}
		}
	//	yield return null;
	}

	void SetDealerCarEvent(){
		if(AccountManager.instance.ChampItem.S2_5_Event_Featured == 0){
			myAcc.instance.account.bLobbyBTN[4] = false;
		//	GV.gDealer = 0;
		//	GV.gBuyDealerCar = 0;
		}else{
			int mCar =myAccount.instance.account.eRace.featuredCarID;
			if(Common_Car_Status.Get(mCar).ReqLV <= GV.ChSeason){
				
			}else{
				mCar = SetUserDealerCar();
				myAccount.instance.account.eRace.featuredCarID = mCar;
			}

			/*
			CarInfo mcar = GV.mineCarList.Find(obj => obj.CarID == mCar);
			if(mcar == null) {
				GV.gDealerCarID = myAccount.instance.account.eRace.featuredCarID;
				myAcc.instance.account.bLobbyBTN[4] = true;
				GV.gDealer = 1;
				GV.gBuyDealerCar = 1;
				activeObject.SendMessage("SHOPNewButton",true);
			}else{
				//DealerCar Button Set
				myAcc.instance.account.bLobbyBTN[4] = false;
				GV.gDealerCarID = 0;
				GV.gBuyDealerCar = 0;
			} */
		}
	}

	private int SetUserDealerCar(){
		int dCarId = 0;
		for(int i = 0; i < 300; i++){
			dCarId = (int)Well512.Next(0,24);
			dCarId+= 1000;
			Common_Car_Status.Item item = Common_Car_Status.Get(dCarId);
			if(item.ReqLV <= GV.ChSeason){
				break;
			}
		}
		return dCarId;
	}

	private void SetUserDealerCar2(){
		int dCarId = 0;
		if(!EncryptedPlayerPrefs.HasKey("DealerCar")){
			EncryptedPlayerPrefs.SetInt("DealerCar", 1000);
		}
		int mCar = EncryptedPlayerPrefs.GetInt("DealerCar");
		for(int i = 0; i < 300; i++){
			dCarId = (int)Well512.Next(0,24);
			dCarId+= 1000;
			Common_Car_Status.Item item = Common_Car_Status.Get(dCarId);
			if(item.ReqLV <= GV.ChSeason){
				if(mCar != dCarId){
					break;
				}
			}
		}
		EncryptedPlayerPrefs.SetInt("DealerCar", dCarId);
		//return dCarId;
	}

	void EnvInit(){
		InitPanel("LOBBY");
		MakeLobbyCar();
		//CreateElevatroCar(1000);
		MakeLobbyCrew();
		menuCenter = MenuCenter.GetComponent<TeamCenterMenuAction>();
	}
	
	
	public void InitFuelCount(){

		if(GV.gVIP == 0){
			GV.mUser.FuelMax = 5;
		}else{
			int vipID = 1900+(GV.gVIP-1);
			Common_VIP.Item vItem = Common_VIP.Get(vipID);
			if(vItem.V_add_battery == 0) 	GV.mUser.FuelMax  = 10;
			else GV.mUser.FuelMax = 5 + vItem.V_add_battery;
		}
		if(GV.mUser.FuelCount >= GV.mUser.FuelMax){
			GV.mUser.FuelCount = GV.mUser.FuelMax;
		}
		if(!UserDataManager.instance.CheckCoroutineFuel()) {
			UserDataManager.instance.fuelTimeStart();

		}
		myFuel.GetComponent<FuelAction>().FuelCountCheck();
		myFuel.GetComponent<FuelAction>().FuelCount();
	}
	
	public void DecreaseFuelCount(){
		myFuel.GetComponent<FuelAction>().ChangeFuelCount();
	}

	public void FuelAdd(){
		myFuel.GetComponent<FuelAction>().FuelAddCount();
	}
	public void InitTopMenu(){
		var coin = MenuTop.transform.FindChild("Coin").gameObject;
		/*if(btnstate == buttonState.LEVELUP){
			btnstate = buttonState.WAIT;
			isShowWin=true;
		}*/
		if(isDollarCount){
			StopCoroutine("myDollarCount");
			isDollarCount = false;
		}
		if(isCoinCount){
			StopCoroutine("myCoinCount");
			isCoinCount = false;
		}
		StartCoroutine("myCoinCount",coin);
		StartCoroutine("myDollarCount",coin);
		coin  = null;
	}

	public void InitTopMenuReward(){
		var coin = MenuTop.transform.FindChild("Coin").gameObject;
		var coinsub = coin.transform.FindChild("Coin").GetComponentInChildren<UILabel>() as UILabel;
		coinsub.text = System.String.Format("{0:#,0}", GV.myCoin);
		coinsub = coin.transform.FindChild("Dollar").GetComponentInChildren<UILabel>() as UILabel;
		coinsub.text = System.String.Format("{0:#,0}", GV.myDollar);
	
	}

	void SetLobbyBtnAfterSeeing(){
		var parent  = MenuBottom.transform.FindChild("Menu_Lobby").gameObject as GameObject;
		myAcc.instance.account.bLobbyBTN[2] = true;
		parent.SendMessage("INVENTORYNewButton",true,SendMessageOptions.DontRequireReceiver);
	}


	
	void InitLevelText(){
		InitTopMenuReward();

	}
	void PostReadAction(bool b){
		InitTopMenu();
		myFuel.GetComponent<FuelAction>().FuelAddCount();
		if(!b) return;
		MenuBottom.SetActive(false);
		MenuBottom.SetActive(true);
		// 1.5f / 75000/
	}
	
	bool isDollarCount = false, isCoinCount = false;
	IEnumerator myDollarCount(GameObject top){
		int mdollar = GV.myDollar;
		int udollar = GV.updateDollar;//Base64Manager.instance.GlobalEncoding(Global.updateDollar,0);
		int total =  (mdollar+udollar);
		GV.updateDollar = 0;//Base64Manager.instance.GlobalEncoding(0,0);
		int tmpInt = 0;
		var coindollar = top.transform.FindChild("Dollar").GetComponentInChildren<UILabel>() as UILabel;
		coindollar.text = System.String.Format("{0:#,0}", total);
		bool b = false;
		var dollaricon =  top.transform.FindChild("icon_DOLLAR") as Transform;
		if(udollar == 0) {
			dollaricon.GetComponent<TweenScale>().enabled = false;
			dollaricon.localScale= new Vector3(33,33,1);
			yield break;
		}
		else if(udollar < 0) {
			tmpInt = 1;
			b = true;
		}else {
			tmpInt = -1;
			b=false;
		}
		isDollarCount = true;
		if(Mathf.Abs(udollar) > 150)
			tmpInt = (Mathf.Abs(udollar)/150)*tmpInt;
		int timeCount = 0;
		dollaricon.GetComponent<TweenScale>().enabled = true;
		Vector3 oScale = new Vector3(33,33,1);
		if(b){
			for(;;){
				total += tmpInt;
				coindollar.text = string.Format("{0:#,0}",total);
				timeCount++;
				if(mdollar <= total || timeCount >= 150){
					coindollar.text = string.Format("{0:#,0}",mdollar);
					dollaricon.GetComponent<TweenScale>().enabled = false;
					dollaricon.transform.localScale = oScale;
					isDollarCount = false;
					yield break;
				}
				yield return new WaitForSeconds(0.01f);
			}
		}else{
			for(;;){
				total += tmpInt;
				coindollar.text = string.Format("{0:#,0}",total);
				timeCount++;
				if(mdollar >= total || timeCount >= 150){
					coindollar.text = string.Format("{0:#,0}",mdollar);
					dollaricon.GetComponent<TweenScale>().enabled = false;
					dollaricon.transform.localScale = oScale;
					isDollarCount = false;
					yield break;
				}
				yield return new WaitForSeconds(0.01f);
			}
		}
	}

	IEnumerator myCoinCount(GameObject top){
		int mcoin =GV.myCoin;// Base64Manager.instance.GlobalEncoding((int)Global.coin,0);
		int ucoin = GV.updateCoin;//Base64Manager.instance.GlobalEncoding(Global.updateCoin,1);
		GV.updateCoin = 0;
		int total =  (mcoin+ucoin);
		int tmpInt = 0;
		var coinsub = top.transform.FindChild("Coin").GetComponentInChildren<UILabel>() as UILabel;
		coinsub.text = System.String.Format("{0:#,0}", total);
		var dollaricon = top.transform.FindChild("icon_COIN") as Transform;
		bool b = false;
		if(ucoin == 0) {
			dollaricon.GetComponent<TweenScale>().enabled = false;
			dollaricon.localScale = new Vector3(35,36,1);
			yield break;
		}else if(ucoin < 0){
			tmpInt = 1;
			b = true;
		}else{
			tmpInt = -1;
			b=false;
		}
		isCoinCount = true;
		if(Mathf.Abs(ucoin) >150)
			tmpInt = (Mathf.Abs(ucoin)/150)*tmpInt;
		//		Utility.Log(tmpInt);
		dollaricon.GetComponent<TweenScale>().enabled = true;
		Vector3 oScale = new Vector3(35,36,1);
		int timeCount = 0;
		if(b){
			for(;;){
				total += tmpInt;
				coinsub.text = string.Format("{0:#,0}",total);
				timeCount++;
				if(timeCount >= 150 || total > mcoin){
					coinsub.text = string.Format("{0:#,0}",mcoin);
					dollaricon.GetComponent<TweenScale>().enabled = false;
					dollaricon.transform.localScale = oScale;
					isCoinCount  = false;
					yield break;
				}
				yield return new WaitForSeconds(0.01f);
			}
		}else{
			for(;;){
				total += tmpInt;
				coinsub.text = string.Format("{0:#,0}",total);
				timeCount++;
				if(timeCount >= 150 || total < mcoin){
					coinsub.text = string.Format("{0:#,0}",mcoin);
					dollaricon.GetComponent<TweenScale>().enabled = false;
					dollaricon.transform.localScale = oScale;
					isCoinCount  = false;
					yield break;
				}
				yield return new WaitForSeconds(0.01f);
			}
		}
		
		
	}
	private void SetMyLevel(){
		var mylevel = gLevel.transform.FindChild("lbLv").GetComponent<UILabel>() as UILabel;
		mylevel.text =string.Format(KoStorage.GetKorString("76001"), Global.level);
		if(Global.level >= 100){
			Global.level = 100;
			mylevel.text = string.Format(KoStorage.GetKorString("76001"),Global.level );
			//levelbar.sliderValue = 1.0f;
		}
	}

	void testEXP(){
		for(int i = 0; i < 100; i++){
			Utility.LogWarning(string.Format("{0} / {1} / {2}", i, Common_Exp_Range.levelCheck(i*10), i*10));
		}
	}
	private readonly int EXPID = 8999;
	IEnumerator LevelBarProcess(){
		int addExp = Global.addExp;
		int exExp = Global.Exp;
		int totalExp = exExp + addExp;
		Global.Exp = totalExp;
		Global.addExp = 0;
		//int level = Global.level;
		//int nLevel = Global.addLevel;
		//if(Global.level == 0) {Global.level = 1; Global.addLevel = 1;}
		int cLevel = Global.level - Global.addLevel;
		if(cLevel < 0) { Global.gLvUp = 1;Global.gNewMsg++;gameRank.instance.listGift.Clear();}
		SetMyLevel();
		if(Global.level == 100) {isShowWin = true;spLevelBar.fillAmount = 1.0f; yield break;}
		// 레벨업 확인 
		int ExpID = EXPID+Global.level;
		Common_Exp_Range.Item expItem = Common_Exp_Range.Get(ExpID);
		if(totalExp > expItem.Max){
			Global.level++;
			//Global.gLvUp = 1;
			SetMyLevel();
			ExpID = EXPID+Global.level;
		}
		isShowWin = true;
		if(Global.level == 100)  {	spLevelBar.fillAmount = 1.0f; yield break;}
		// 레벨바 초기화
		
		int minValue = 0;
		if(Global.level == 1){
			minValue =0;
		}else{
			minValue = Common_Exp_Range.Get(ExpID-1).Max;
		}
		if(ExpID == 9099) {	spLevelBar.fillAmount = 1.0f; yield break;}
		int maxValue = Common_Exp_Range.Get(ExpID).Max;
		int barRange = maxValue - minValue;
		int initValue = totalExp-minValue;
		float value  = 0;
		float oldValue = 0;
		
		if(addExp == 0) {
			value = Mathf.Abs((float)exExp- (float)minValue);
			oldValue =value/(float)barRange;
			spLevelBar.fillAmount = oldValue;
			yield break;
		}
		if(initValue == 0) {
			spLevelBar.fillAmount = 0.0f;
			yield break;
		}
		//획득한 레벨바 이동.
		
		value = Mathf.Abs((float)totalExp-(float)minValue);
		oldValue = (float)initValue/(float)barRange;
		float Dvalue = 0.0f;
		spLevelBar.fillAmount = oldValue;
		/*for(;;){
			Dvalue += 0.003f;
			spLevelBar.fillAmount =Dvalue;
			if(Dvalue >= 1.0f){
				//levelbar.sliderValue = 1.0f;
				yield break;
			}
			if(Dvalue >= oldValue){
				yield break;
			}
			yield return new WaitForSeconds(0.01f);
		}*/
		yield return null;
	}


	void InviteEventLobbyUI(){
		rankObj.SendMessage("InviteEventRankGridAction");
	}
	

	void FBLobbyAction(){
		rankObj.SendMessage("reFBRankAction",  SendMessageOptions.DontRequireReceiver);
	}
	
	void FBProcessing(int nStatus){
		if(nStatus == 1){
			btnstate = buttonState.WAIT;
		}else if(nStatus == 0){
			btnstate =buttonState.LEVELUP;
		}
	}



	
	void MakeDriverCrew(GameObject Car){
		if(Car == null) return;
		var dr = Car.transform.GetChild(0).FindChild("Driver_Axis") as Transform;
		if(dr.childCount == 0){
			
		}else{
			DestroyImmediate(dr.GetChild(0).gameObject);
		}
		//var _drive = gameObject.AddComponent<createDriveraction>() as createDriveraction;
	//	DestroyImmediate(_drive);
	}
	
	void MakeLobbyCar(){
		int carid = GV.getTeamCarID(GV.SelectedTeamID);
		string str = GV.getTeamCarClass(GV.SelectedTeamID);
		_table.CreateLobbyCar(carid,str);
	}
	
	void MakeLobbyCrew(){
		_table.CreateLobbyCrew(GV.getTeamCrewID(GV.SelectedTeamID),0);
	}


	void LobbyChangeTeam(int id){
		int carid = GV.getTeamCarID(GV.SelectedTeamID);
		string str = GV.getTeamCarClass(GV.SelectedTeamID);
		_table.ChangeLobbyCar(0,carid,str);
		_table.ChangeLobbyCrew(0,GV.getTeamCrewID(GV.SelectedTeamID));
		isTeamInfo = false;
		ResetLevelButton(true);
		ResetLevelButton(false);
	}


	void OnPauseMessage(){
		//isPause  = true;
	}
	
	void OnResumeMessage(){
		//isPause = false;
	}
	
	
	TeamCenterMenuAction menuCenter;
	delegate void OnBackBack();
	OnBackBack onBackBack;

	void LobbyMenuInit(){
		rankObj.SetActive(true);
		menuCenter.LobbyInit();
	//	if(OnBackCall != null){
	//		OnBackCall();
	//		OnBackCall  = null;
	//	}
		if(onBackBack != null){
			onBackBack();
			onBackBack = null;
		}
	}

	IEnumerator FadeTimeDelay(float _Delay){
		yield return new WaitForSeconds(_Delay);
		fadeOut();
	}
	
	void MenuBackActivate(bool b){
		MenuTop.transform.FindChild("BackBtn").gameObject.SetActive(b);
	}

	public delegate void OnEventReceive();
	OnEventReceive onReceive;
	void ExecuteCallFunction(){
		if(onReceive != null){
			onReceive();
		}
		onReceive = null;
	}
	
	public void OnMoveToBack(string des, OnEventReceive CloseCallback){
		this.onReceive = CloseCallback;
		onReceive += new OnEventReceive(()=>{
		//	worldmap.SetActive(false);
			MenuBackActivate(false);

		});
		if(string.Equals(des, "Team")){
			OnMyCarClick();
			mapMoveToOther();
		}else if(string.Equals(des,"Shop")){
			mapMoveToOther();
		}

		if(string.Equals(des,"Coin")){
			//isMap = true;
			OnDollarClick();
			GameObject.Find("Audio").SendMessage("ChangeBGMMusic", true, SendMessageOptions.DontRequireReceiver);
		}
		return;
	}

	public void OnMoveToCoinShop(){
		OnBackClick();
		Invoke("OnDollarClick", 0.5f);
		//OnDollarClick();
		GameObject.Find("Audio").SendMessage("ChangeBGMMusic", true, SendMessageOptions.DontRequireReceiver);
	}

	public void OnMoveToTeam(){
		OnBackClick();
		Invoke("OnTeamClick", 0.5f);
		GameObject.Find("Audio").SendMessage("ChangeBGMMusic", true, SendMessageOptions.DontRequireReceiver);
	}

	void mapMoveToOther(){
		OnBackFunction = null;
		//isMap =false;
	//	_mapaction.HiddenTrackInfo();
	//	_mode.WindowDisable();
	//	if(_map != null)
	//		DestroyImmediate(_map);
		isLobby = false;
	}
	void TipInfoShow(){
		if(TipInfo == null) return;
		TipInfo.transform.parent.GetComponent<TweenAction>().tempHidden(TipInfo);
	}
	void ShowInfoTipWindow(string part){
	
		if(part == string.Empty) return;
		if(TipInfo != null){
			TipInfo.transform.parent.GetComponent<TweenAction>().ReverseTween(TipInfo);
			TipInfo.transform.parent.GetComponent<TweenAction>().ReverseTween(TipInfo.transform.parent.gameObject);
		}
		TipInfo = null;
		var modeinfo = gameObject.GetComponent<modeInfoAction>() as modeInfoAction;
		if(modeinfo == null) modeinfo = gameObject.AddComponent<modeInfoAction>();

		switch(part){
		case "Team": TipInfo = modeinfo.CreateUpgradeInfo(6300);break;
		case "Upgrade":TipInfo = modeinfo.CreateUpgradeInfo(6400);break;
		case "Crew":Utility.LogWarning("Crew Tip");break;
		case "Upgrade_Crew":TipInfo = modeinfo.CreateUpgradeInfo(6202);break;
		case "Regular":TipInfo = modeinfo.CreateRaceModeInfo(3200,3200);break;
		case "Champion":TipInfo = modeinfo.CreateRaceModeInfo(3100,3100);break; 
		case "Event":TipInfo = modeinfo.CreateRaceModeInfo(3300,3300);break;  
		case "PVP":TipInfo = modeinfo.CreateRaceModeInfo(3401,3401);break;
		case "Weekly":TipInfo = modeinfo.CreateRaceModeInfo(3601,3601);break;
		case "Club":{
			if(CClub.ClanMember == 0)
				TipInfo = modeinfo.CreateRaceModeInfo(3520,3520);
			else{
				if(CClub.ClubMode == 0) TipInfo = modeinfo.CreateRaceModeInfo(3520,3520);
				else 	TipInfo = modeinfo.CreateRaceModeInfo(3520,3520);
			}
			//Utility.LogWarning("ClubMode = 2 " + CClub.ClubMode);
		}break;
		case "sLucky":TipInfo = modeinfo.CreateTeamInfo(4005); break;
		case "gLucky":TipInfo = modeinfo.CreateTeamInfo(4006); break;
		case "Inven_Car":break;//TipInfo = modeinfo.CreateTeamInfo(5000); break;
		case "Inven_Mat":break;//TipInfo = modeinfo.CreateTeamInfo(5001); break;
		case "Inven_Cube":break;//TipInfo = modeinfo.CreateTeamInfo(5002); break;
		case "Inven_Coupon":break;//TipInfo = modeinfo.CreateTeamInfo(5003); break;
		default: break; //Utility.LogWarning("Tip name " + part); 
		
		}
	}
	
	void HiddenInfoTipWindow(){
		if(TipInfo != null){
			TipInfo.transform.parent.GetComponent<TweenAction>().ReverseTween(TipInfo.transform.parent.gameObject);
			TipInfo.transform.parent.GetComponent<TweenAction>().ReverseTween(TipInfo);
		}
		TipInfo = null;
	}

	void UpgradeSelectButtonActive(GameObject obj){
		int cnt = obj.transform.childCount;
		for(int i = 0; i <cnt; i++){
			var temp = obj.transform.GetChild(i).gameObject;
			var temp1 = temp.transform.FindChild("Select").gameObject;
			if(i ==0 ){
				temp1.SetActive(true);
			}else temp1.SetActive(false);
		}
	}
	
	void InitTableShop(int b){

		return;/*
		var obj = MenuBottom.transform.FindChild("Menu_MyCar_Grid") as Transform;
		var gridParent = obj.FindChild("View") as Transform;
		var grid = gridParent.FindChild("Grid") as Transform;
		if(grid.transform.childCount == 0){
			CreateGridPanelItem(grid.gameObject);
		}
		string carName = Base64Manager.instance.GlobalEncoding(Global.MyCarID).ToString();
		int cnt = grid.childCount;
		for(int i = 0; i <cnt; i++){
			var temp = grid.GetChild(i).GetChild(0).gameObject as GameObject;
			var select = temp.transform.FindChild("Select").gameObject;
			if(select.activeSelf){
				if(string.Equals(temp.name, carName)==true){
				}else{
					carName = temp.name;
				}
				break;
			}
		}
		int tmpid = Base64Manager.instance.GlobalEncoding(Global.MyCarID);
		int tmpids = Base64Manager.instance.GlobalEncoding(Global.MyCrewID);
		if(b==0)
		_table.OnTeam(tmpid.ToString(),tmpids.ToString());
		else 
		_table.OnTeamTour(tmpid.ToString(),tmpids.ToString());
		//_table.OnTeamTour("1010","1205");
		return;

			/*
		obj = MenuBottom.transform.FindChild("Menu_MyCrew_Grid") as Transform;
		gridParent = obj.FindChild("View") as Transform;
		grid = gridParent.FindChild("Grid") as Transform;
		cnt = grid.childCount;
		if(grid.transform.childCount == 0){
			CreateGirdPanelCrewItem(grid.gameObject);
		}
		string crewName = Base64Manager.instance.GlobalEncoding(Global.MyCrewID).ToString();
		for(int i = 0; i < cnt; i++){
			var temp = grid.GetChild(i).GetChild(0).gameObject as GameObject;
			var select = temp.transform.FindChild("Select").gameObject;
			if(select.activeSelf){
				if(string.Equals(temp.name, crewName)==true){
				}else{
					crewName = temp.name;
				}break;
			}
		}
		//_table.OnTeam(carName,crewName);
		int tmpid = Base64Manager.instance.GlobalEncoding(Global.MyCarID);
		int tmpids = Base64Manager.instance.GlobalEncoding(Global.MyCrewID);
		_table.OnTeam(tmpid.ToString(),tmpids.ToString());
		return;
		*/
	}
	
	public void SetShopButton(string str){
		
		switch(str){
		case "Car":
			InitPanel("CAR_SHOP");
			break;
		case "ShowRoom":
			InitPanel("CAR_SHOWROOM");
			break;
		case "Deal":
			InitPanel("CAR_SHOP_DEAL");
			break;
		/*case "Crew":
			InitPanel("Shop_Crew");
			isCoinShop = false;
			break;
		case "Coin":
			InitPanel("Shop_Coin");
			isCoinShop = true;
			break;*/
		default:
			break;
		}
	}
	
	void InitButton(string str){
		Utility.LogWarning("InitButton");
		var obj = MenuBottom.transform.FindChild("Menu_MyCar_Grid") as Transform;
		var gridParent = obj.FindChild("View") as Transform;
		var grid = gridParent.FindChild("Grid") as Transform;
		
		if(grid.transform.childCount == 0){
			CreateGridPanelItem(grid.gameObject);
		}
		//GameObject objtemp = null;
		//string carName = Base64Manager.instance.GlobalEncoding(Global.MyCarID).ToString();
		string carName = GV.getTeamCarID(GV.SelectedTeamID).ToString();
		for(int i = 0; i < grid.childCount; i++){
			var temp = grid.GetChild(i).GetChild(0).gameObject as GameObject;
			var select = temp.transform.FindChild("Select").gameObject;
			if(temp.name.Equals(carName)){// == Global.MyCarName ){
				select.SetActive(true);
				if(ElevatorCar.name != carName){
					string strtemp = temp.name;
					int _carid = int.Parse(strtemp);
					CreateElevatroCar(_carid);
				}
			}else {
				select.SetActive(false);
			}
		}
		
		if(str == "MyTeam"){
			//MenuCenter.transform.GetChild(2).GetComponent<UILabel>().text = "Select Your Car";
			//	MenuCenter.transform.GetChild(2).GetComponent<UILabel>().text = string.Empty;
			obj = MenuBottom.transform.FindChild("Menu_MyCrew_Grid") as Transform;
			gridParent = obj.FindChild("View") as Transform;
			grid = gridParent.FindChild("Grid") as Transform;
			//string crewName = Base64Manager.instance.GlobalEncoding(Global.MyCrewID).ToString();
			string crewName = GV.getTeamCrewID(GV.SelectedTeamID).ToString();
			// = Global.MyCrewID.ToString();
			//Utility.Log ("crewname " + crewName);
			for(int i = 0; i < grid.childCount; i++){
				var temp = grid.GetChild(i).GetChild(0).gameObject as GameObject;
				var select = temp.transform.FindChild("Select").gameObject;
				if(temp.name.Equals(crewName)){// == Global.MyCarName ){
					select.SetActive(true);
				}else {select.SetActive(false);}//temp.transform.FindChild("Select").gameObject.SetActive(false);
			}
		}else if(str == "Upgrade"){
			var _obj = MenuBottom.transform.FindChild("Menu_Upgrade_Car").gameObject;
			UpgradeSelectButtonActive(_obj);
			_obj = MenuBottom.transform.FindChild("Menu_Upgrade_Crew").gameObject;
			UpgradeSelectButtonActive(_obj);
			CrewUpNameStock = null;
			CarUpNameStock = null;
			CarUpNameTour = null;
			CrewUpNameTour = null;
			
		}
		obj =gridParent = grid = null;
	}
	
	public void OnCrewClick(GameObject obj){
		
		var _parent = activeObject.transform.GetChild(0).GetChild(0) as Transform;
		int count = 6;
		for(int i = 0; i < count; i++){
			var temp1 = _parent.GetChild(i).GetChild(0) as Transform;
			var temp2 = temp1.FindChild("Select").gameObject;
			temp2.SetActive(false);
		}
		obj.transform.FindChild("Select").gameObject.SetActive(true);
		if(raceinfo != null){
			InfoWindowDisable();
			raceinfo = null;
		}
		_table.StoreAniFinishFunction(()=>{
			var _prefab = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
			raceinfo = _prefab.MakeCrewInfo(MenuBottom.transform);
			DestroyImmediate(_prefab);
			raceinfo.name = obj.name;
		//	int crewid = int.Parse(obj.name);
			var _info = raceinfo.GetComponent<ViewTeamCarInfo>() as ViewTeamCarInfo;
			if(_info == null) _info = raceinfo.AddComponent<ViewTeamCarInfo>();
		//	_info.ShowCrewInfomation(obj.transform.GetChild(0).gameObject, crewid);
			//string _team = Base64Manager.instance.GlobalEncoding(Global.MyCrewID).ToString();
			string _team = GV.getTeamCrewID(GV.SelectedTeamID).ToString();
			raceinfo.name = obj.name;
			_info.InitInfoSet(raceinfo, _team);_info=null;		
		});
		_table.OnMyCrew(obj.name);
		return;
	}
	


	
	void ShowStatusWindow(GameObject obj){
		return;
	}
	
	
	void hiddenStatusWindow(GameObject obj){
		//Utility.LogWarning("hiddenStatusWindow");
		var tween = status.GetComponent<TweenAction>() as TweenAction;
		if(tween == null) tween = status.AddComponent<TweenAction>();
		tween.ReverseTween(status);
		status = null;
		return;
	}
	
	public void ApplyStatusWindow(string str){
		status.SendMessage("ApplyUpgradeStatus",str,SendMessageOptions.DontRequireReceiver);
	}
	

	/*
	public void OnTeamCard(){
		var temp = MenuCenter.GetComponent<TeamCenterMenuAction>() as TeamCenterMenuAction;
		if(temp.OnCardClick()) return;
		btnstate = buttonState.MYCARD;
		finishPanel();
		OnBackCall = ()=>{ menuCenter.InvenHidden();};
	}*/
	

	void ElevatorCarAni(){
		StartCoroutine("ElevatorCarAniStart");
		
	}
	IEnumerator ElevatorCarAniStart(){
		Global.isAnimation =true;
		_table.AudioDownPlay();
		yield return StartCoroutine("ElevatorAnimation","Down");
		_table.AudioUpPlay();
		yield return StartCoroutine("ElevatorAnimation","Up");
		Global.isAnimation =false;
	}
	
	IEnumerator ElevatorAnimation(string status){
		var ani = elevator.GetComponent<Animation>() as Animation;
		ani.Play(status);
		while(true){
			if(!ani.isPlaying){
				break;
			}
			yield return true;
		}
	}
	
	
	public void OnCarClick(GameObject obj){
		bool ispress = false;
		var mParent  = activeObject.transform.FindChild("View").FindChild("Grid") as Transform;
		for(int i = 0; i < mParent.transform.childCount; i++){
			var temp1 = mParent.transform.GetChild(i).GetChild(0).gameObject;
			var temp2 = temp1.transform.FindChild("Select").gameObject;
			if(temp2.activeSelf){
				if(temp1.name == obj.name){
					ispress = true;
				}
			}
			if(temp2 != null) 
				temp2.SetActive(false);
		}
		obj.transform.FindChild("Select").gameObject.SetActive(true);
		if(!ispress){
			StartCoroutine("ElevatorCarChange", obj);
			//_table.OnMyCar(obj.name);
		}
	//	var tr = activeObject.transform.GetChild(0) as Transform;
	//	if(tr.name.Equals("View"))
	//	{
	//		tr = activeObject.transform.GetChild(1);
	//	} 
	//	tr.name = obj.name;
	}
	
	IEnumerator ElevatorCarChange(GameObject obj){
		if(raceinfo != null){
			InfoWindowDisable();
			raceinfo = null;
		}
		yield return new WaitForSeconds(0.1f);
		_table.StoreAniFinishFunction(()=>{
			MakeCarInfoWindow(obj);
		});
		_table.OnMyCar(obj.name);
	}

	void SetElevatorCar(){
		var root = elevator.transform.GetChild(1).gameObject;
		var root1 = root.transform.GetChild(8).gameObject;
		root = root1.transform.GetChild(0).gameObject;
		var root2 = root.transform.GetChild(0).gameObject as GameObject;
		ElevatorCar = root2;
		ElevatorCar.SetActive(false);
		return;
	}
	void CreateElevatroCar(int _id){
	//	Utility.LogWarning("CreateElevatroCar");
		return;
	/*	var root = elevator.transform.GetChild(1).gameObject;
		var root1 = root.transform.GetChild(8).gameObject;
		root = root1.transform.GetChild(0).gameObject;
		var root2 = root.transform.GetChild(0).gameObject;
	//	int tempid = Base64Manager.instance.GlobalEncoding(Global.MyCarID);
	//	int tempids =Base64Manager.instance.GlobalEncoding(Global.MySponsorID);
		int tempid = GV.getTeamCarID(GV.SelectedTeamID);
		int tempids = GV.getTeamSponID(GV.SelectedTeamID);

		if(root2 != null) {
			if(root2.name == tempid.ToString()){
				var tex = ElevatorCar.GetComponent<CarType>() as CarType;
				if(tex == null) tex = ElevatorCar.AddComponent<CarType>();
				tex.CarTextureInitialize(tempid.ToString(),tempids.ToString());
				ElevatorCar.SetActive(false);
				tex = null;
				return;
			}
			//Utility.Log(root2.name);
			Destroy(root2);
		}
		ElevatorCar = ObjectManager.CreatePrefabs(root2.transform, "MyCar", _id.ToString());
		ElevatorCar.name  = _id.ToString();
		var tex1 = ElevatorCar.GetComponent<CarType>() as CarType;
		if(tex1 == null) tex1 = ElevatorCar.AddComponent<CarType>();

		tex1.CarTextureInitialize(tempid.ToString(),tempids.ToString());
		ElevatorCar.SetActive(false);
		 tex1 =null; */
	}
	IEnumerator ElevatorCarAniStart1(string str){
		Global.isAnimation =true;
		_table.AudioDownPlay();
		yield return StartCoroutine("ElevatorAnimation","Down");
		ElevatorCar.SetActive(true);
		CheckElevatorCar(str);
		_table.AudioUpPlay();
		yield return StartCoroutine("ElevatorAnimation","Up");
		Global.isAnimation =false;
	}

	void ChangeElevatorCar(string str){
		string[] name = str.Split('_');
		int id = int.Parse(name[1]);
		if(id == 0) {
			ElevatorCar.SetActive(false);
			return;
		}
		StartCoroutine("ElevatorCarAniStart1",str);
	}


	void ChangeElevatorCarAssy(){
		var root = elevator.transform.GetChild(1).gameObject;
		var root1 = root.transform.GetChild(8).gameObject;
		root = root1.transform.GetChild(0).gameObject;
		var root2 = root.transform.GetChild(0).gameObject;
		if(root2 != null) {
			Destroy(root2);
		}
		ElevatorCar = ObjectManager.CreatePrefabs(root2.transform, "MyCar", "9999");
		ElevatorCar.name  = "9999";//id.ToString();
	
	}

	void CreateCarClassObject(GameObject car, string carClass){
		var tr = car.GetComponent<CarType>().CarClass as Transform;
		if(tr.childCount != 0) {
			for(int  i=0; i < tr.childCount; i++){
				if(tr.GetChild(i).name.Equals("GameObject")==false) DestroyImmediate(tr.GetChild(i).gameObject);
			}
		}
		if(carClass.Equals("junk")) return;
		var temp1 = ObjectManager.CreatePrefabs(tr.GetChild(0), "Car_Class", car.name+"_"+carClass) as GameObject;
		temp1.name = carClass;
		
	}

	void CheckElevatorCar(string name1){
		var root = elevator.transform.GetChild(1).gameObject;
		var root1 = root.transform.GetChild(8).gameObject;
		root = root1.transform.GetChild(0).gameObject;
		var root2 = root.transform.GetChild(0).gameObject;
		string[] name = name1.Split('_');
		int id = int.Parse(name[1]);
		int sID = GV.getTeamSponID(GV.SelectedTeamID);
		if(root2 != null) {
			if(root2.name == id.ToString()){
				CreateCarClassObject(root2, name[2]);
			//	var tex = ElevatorCar.GetComponent<CarTexture>() as CarTexture;
			//	if(tex == null) tex = ElevatorCar.AddComponent<CarTexture>();
			//	tex.CarInitialize(id.ToString(),"1300",10);

				var tex1 = ElevatorCar.GetComponent<CarType>() as CarType;
				if(tex1 == null) tex1 = ElevatorCar.AddComponent<CarType>();

				tex1.CarTextureInitialize(id.ToString(),sID.ToString());
				tex1 = null;
				return;
			}
			Destroy(root2);
		}
		ElevatorCar = ObjectManager.CreatePrefabs(root2.transform, "MyCar", id.ToString());
		ElevatorCar.name  = id.ToString();
		CreateCarClassObject(ElevatorCar, name[2]);
		var tex2 = ElevatorCar.GetComponent<CarType>() as CarType;
		if(tex2 == null) tex2 = ElevatorCar.AddComponent<CarType>();
		tex2.CarTextureInitialize(id.ToString(),sID.ToString());
		 tex2 =null;
	}

	void ResetElevatorCar(bool bTeam, string name){
		if(bTeam){
		}else{
		}
		int carid = GV.getTeamCarID(GV.SelectedTeamID);
		string carclass = GV.getTeamCarClass(GV.SelectedTeamID);
		ElevatorCar.SetActive(true);
		string str = "Car_"+carid.ToString()+"_"+carclass;
		CheckElevatorCar(str);
	}

	void MakeCarInfoWindow(GameObject obj){
		string[] str = obj.name.Split('_');
		int _carid = int.Parse(str[0]);
		Utility.LogWarning("makeCarInfo"+obj.name);
		if(raceinfo != null){
			//raceinfo.name = obj.name;
		}else{

			var _prefab = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
			raceinfo = _prefab.MakeCarInfo(activeObject.transform.parent);

			DestroyImmediate(_prefab);	

			status.name = obj.name;
			var _state = status.GetComponent<StatsUpInfo>() as StatsUpInfo;
			if(_state == null) _state=status.AddComponent<StatsUpInfo>();
			_state.ChangeMyTeamStatus();

			string carname = GV.getTeamCarID(GV.SelectedTeamID).ToString();
			var _in = raceinfo.GetComponent<ViewTeamCarInfo>();
			if(_in == null) _in = raceinfo.AddComponent<ViewTeamCarInfo>();
			raceinfo.name = obj.name;
			_in.ShowCarInfomation(obj,_carid);
			_in.InitInfoSet(raceinfo,carname);
			_in = null;
		}
		
	}
	
	void disabelMenu(){
		int a = MenuBottom.transform.childCount;
		for(int i = 0; i < (a); i++){
			MenuBottom.transform.GetChild(i).gameObject.SetActive(false);
		}
		MenuBottom.transform.FindChild("Bar_Down").gameObject.SetActive(true);
	}
	
	void GridInit(GameObject _grid){
		var obj0 = _grid.transform.parent.gameObject as GameObject;
		Vector4 cr = obj0.transform.GetComponent<UIPanel>().clipRange;// = new Vector4(0, 60, Screen.width, 230); 
		cr.w = Screen.width;
		cr.x = 0;
		obj0.transform.GetComponent<UIPanel>().clipRange = cr;
		Vector3 pos = obj0.transform.localPosition;
		pos.x = 0;
		obj0.transform.localPosition = pos;
	}
	
	
	
	void PlusBuyedItem(int type){
		switch(type){
		case 0 : 
			isCreateCarItem = false;
			activeObject.GetComponent<newButtonAction>().setCarButton();
			break;
		case 1:
			isCreateCrewItem = false;
			activeObject.GetComponent<newButtonAction>().setCrewButton();
			break;
		case 2:
			isTeamInfo = false;
			ChangeSponsorIcon();
		//	var parent1  = MenuBottom.transform.FindChild("Menu_Lobby").gameObject as GameObject;
		//	parent1.SendMessage("MYTEAMNewButton",true,SendMessageOptions.DontRequireReceiver);
			break;
		case 3 :
			isShowWin = true;
			ResetSponsorButton();
			break;
		case 5:
			var parent  = MenuBottom.transform.FindChild("Menu_Lobby").gameObject as GameObject;
			myAcc.instance.account.bLobbyBTN[2] = true;
			parent.SendMessage("INVENTORYNewButton",true,SendMessageOptions.DontRequireReceiver);
			break;
		default:
			break;
		}
	}
	
	
	
	
	void HiddenInfoWindow(){
		if(raceinfo.GetComponent<TweenAction>() == null)
			raceinfo.AddComponent<TweenAction>().ReverseTween(raceinfo);
		else raceinfo.GetComponent<TweenAction>().ReverseTween(raceinfo);
	}
	
	
	void finishPanel(){
		EndTweenPosition(MenuBottom, new Vector3(0,-200,0), Vector3.zero, 0.5f);
		if(raceinfo != null) {
			InfoWindowDisable();
			var temp  = 	raceinfo.GetComponent<EvoInit>() as EvoInit;
			if(temp != null) raceinfo.GetComponent<EvoInit>().Hidden();
			raceinfo = null;
		}
		if(status != null) {
			hiddenStatusWindow(status);
		}
	}
	
	void finishUpPanel(){
		rankObj.SendMessage("EndTweenPositionSetActive",SendMessageOptions.DontRequireReceiver);
		EndTweenPosition(MenuBottom, new Vector3(0,-200,0), Vector3.zero, 0.2f);
	}
	
	public void AddTweenPosition(GameObject obj, Vector3 _fromPos, Vector3 _toPos, float duration){
		TweenPosition[] _temp = obj.GetComponents<TweenPosition>();// as TweenPosition;
		if(_temp.Length != 0) {
			foreach(TweenPosition tw in _temp){
				DestroyImmediate(tw);
			}
		}
		TweenPosition _temp1 = obj.AddComponent<TweenPosition>();
		_temp1.from =  _fromPos;
		_temp1.to = _toPos;
		_temp1.duration = duration;
		_temp1.style = UITweener.Style.Once;
		_temp1.method = UITweener.Method.EaseInOut;
	}
	
	public void EndTweenPosition(GameObject obj, Vector3 _fromPos, Vector3 _toPos, float duration){
		TweenPosition[] _temp = obj.GetComponents<TweenPosition>();// as TweenPosition;
		if(_temp.Length != 0) {
			foreach(TweenPosition tw in _temp){
				DestroyImmediate(tw);
			}
		}
		TweenPosition _temp1 = obj.AddComponent<TweenPosition>();
		_temp1.from =  _toPos;
		_temp1.to = _fromPos;
		_temp1.duration = duration;
		_temp1.style = UITweener.Style.Once;
		_temp1.method = UITweener.Method.EaseInOut;
	}

	void InfoWindowDisable(){
		var tween = raceinfo.GetComponent<TweenAction>() as TweenAction;
		if(tween == null) tween = raceinfo.AddComponent<TweenAction>();
		tween.ReverseTween(raceinfo);
	}
	
	void OnDestroy(){
		Resources.UnloadUnusedAssets();
		CrewUpNameStock = null;
		CarUpNameStock = null;
		lobby=null;
		rMode = null;
		activeObject=null;;
		elevator=null;;
		MenuTop= MenuCenter=null;
		MenuBottom= null;//worldmap=null;;
		ElevatorCar = null;
		raceinfo= status=null;
		System.GC.Collect();
	}

}


