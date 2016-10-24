using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class popupinteraction : BuyInterAction {

	GameObject _parent = null;
	System.Action<bool> Callback;
	System.Action buyCallback;
	bool isCash;
	int buyID;
	int buyMoney;
	int buyType;
	int PurchaseType =100;

	string buyItem;
	string buyName = string.Empty;

	public void CalledBuyCompete(System.Action<bool> callback){
		this.Callback  = callback;
	}


	public void SaveParentObject(GameObject Parent){
		_parent = Parent;
		//Utility.Log(_parent.name);
	}

	void OnDisable(){
		buyName = string.Empty;
		//buyIcon = string.Empty;
	}

	public void PopWindowStart(){
		string[] _name = gameObject.name.Split("_"[0]);
		int id = int.Parse(_name[0]);
		buyID = id;
		buyItem = _name[1];

		if(string.Equals( _name[2],"Coin")){
			buyType = 1; //coin
		}else{
			buyType = 2; //dollar;
		}
		string micon = string.Empty, mnameText  = string.Empty;
		string mbuyname = string.Empty, mbuytitle = string.Empty;
		//Utility.Log (" name : " + _name[0] + "   " + _name[1] + "   " + _name[2] );
		switch(_name[1]){
		case "Car":{
			Common_Car_Status.Item _item = Common_Car_Status.Get(id);
			micon = _item.ID;
			mnameText = _item.Name;
			mbuyname = KoStorage.GetKorString("71011");//TableManager.ko.dictionary["60085"].String;
			mbuyname = string.Format(mbuyname,mnameText);
			mbuytitle =KoStorage.GetKorString("71010");// TableManager.ko.dictionary["60084"].String;
			mbuytitle = string.Format(mbuytitle, mnameText);
		}break;
		case "Crew":{
			Common_Crew_Status.Item _item = Common_Crew_Status.Get(id);
			micon = _item.ImgQ;
			mnameText = _item.Name;
			mbuyname =KoStorage.GetKorString("71011");//TableManager.ko.dictionary["60085"].String;
			mbuyname = string.Format(mbuyname,mnameText);
			mbuytitle = KoStorage.GetKorString("71010");//TableManager.ko.dictionary["60084"].String;
			mbuytitle = string.Format(mbuytitle, mnameText);
		}break;
		case "Sponsor":{
			Common_Sponsor_Status.Item _item = Common_Sponsor_Status.Get(id);
			micon = _item.img;
			mnameText = _item.Name;
			//int tempid = Base64Manager.instance.GlobalEncoding(Global.MySponsorID);
			int tempid = GV.getTeamSponID(GV.SelectedTeamID);
			if(tempid != 1300){
				mbuytitle = KoStorage.GetKorString("76114");// 
				mbuyname =KoStorage.GetKorString("76109");// "이미 보유하고 있어서 더이상 구매 할 수 없습니다.";
				buyType = 3;
				_parent.transform.FindChild("Sprite (Check_V)").gameObject.SetActive(false);	
			}else{
				mbuyname =KoStorage.GetKorString("76107");//].String;
				mbuytitle = KoStorage.GetKorString("76106");//TableManager.ko.dictionary["60192"].String;
				mbuytitle = string.Format(mbuytitle, mnameText);
			}
		}break;
		default : {
			Common_Cost.Item _item = Common_Cost.Get(id);
			micon = _item.ID;
			mnameText = _item.Name;
			isCash = true;
		}	break;
		}

		if(isCash){
			cashContentsItem(micon, mnameText, mbuyname);
			return;
		}
		if(_parent != null){
			_parent.transform.FindChild("icon_product").gameObject.SetActive(false);
			_parent.transform.FindChild("lbName1").gameObject.SetActive(false);
			_parent.transform.FindChild("lbName").gameObject.SetActive(true);
			_parent.transform.FindChild("lbName").GetComponent<UILabel>().text =mbuyname;
			_parent.transform.FindChild("lbText").GetComponent<UILabel>().text = mbuytitle;
			buyName = mnameText;
			if(buyType == 1)
					_parent.transform.FindChild("btnCoin").gameObject.SetActive(true);
			else if(buyType == 2) _parent.transform.FindChild("btnDollar").gameObject.SetActive(true);// buyname = "icon_dollar";
			else {
				_parent.transform.FindChild("lbPrice").GetComponent<UILabel>().text =string.Empty;
				_parent.transform.FindChild("lbOk").gameObject.SetActive(true);
				_parent.transform.FindChild("lbOk").GetComponent<UILabel>().text =KoStorage.GetKorString("71000");
				_parent.transform.FindChild("btnok").gameObject.SetActive(true);
				_parent.transform.FindChild("btnok").GetComponent<UIButtonMessage>().functionName
					= "OnCloseClick";
			}
		}
	}

	void cashContentsItem(string icon, string name, string text){
		if(_parent == null) return;
		var temp = _parent.transform.FindChild("icon_product") as Transform;
		temp.gameObject.SetActive(true);
		temp.GetComponent<UISprite>().spriteName = icon;
		temp.GetComponent<UISprite>().MakePixelPerfect();
		_parent.transform.FindChild("lbName1").gameObject.SetActive(true);
		string m_buyname = KoStorage.GetKorString("71011");//TableManager.ko.dictionary["60085"].String;
		m_buyname = string.Format(m_buyname, name);
		_parent.transform.FindChild("lbName1").GetComponent<UILabel>().text = m_buyname;
		_parent.transform.FindChild("lbName").gameObject.SetActive(false);
		m_buyname = string.Empty;
		m_buyname = KoStorage.GetKorString("71010");//TableManager.ko.dictionary["60084"].String;
		m_buyname = string.Format(m_buyname, name);
		_parent.transform.FindChild("lbText").GetComponent<UILabel>().text =m_buyname;
		if(buyID >= 8501 && buyID <= 8505){
			_parent.transform.FindChild("btnCash").gameObject.SetActive(true);
			_parent.transform.FindChild("btnCash").FindChild("lbOk").GetComponent<UILabel>().text
			= KoStorage.GetKorString("71013");
			//_parent.transform.FindChild("btnok").gameObject.SetActive(true);
			_parent.transform.FindChild("lbPrice").GetComponent<UILabel>().text = string.Empty;//KoStorage.GetKorString("71000");
			//_parent.transform.FindChild("btnok").GetComponent<UIButtonMessage>().functionName
			//	= "OnBuyOKClick";
			buyCallback = ()=>{
				GameObject.Find("Audio").SendMessage("buyCoin");	
			};
			var go = _parent.GetComponent<GoogleInAppProc>() as GoogleInAppProc;
			if(go == null) _parent.AddComponent<GoogleInAppProc>();
		}else{
			_parent.transform.FindChild("btnCoin").gameObject.SetActive(true);
		}
	}


	void OnOkClick(){
		Utility.LogWarning("modify - onbuyokclick");
	//	gameObject.SetActive(false);
	//	Global.isNetwork = false;
	//	Global.isPopUp = false;
	//	return;

		if(Global.isNetwork) return;
		string lbPirce = _parent.transform.FindChild("lbPrice").GetComponent<UILabel>().text;
		Global.isNetwork = true;
		if(!int.TryParse(lbPirce, out buyMoney))
			buyMoney = int.Parse(lbPirce.Replace(",",string.Empty));
		CheckClassficationType(buyItem, buyID);
	}

	void OnBuyOKClick(){
		Utility.LogWarning("modify - onbuyokclick");
		gameObject.SetActive(false);
		Global.isNetwork = false;
		return;
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		//CheckClassficationType(buyItem, buyID);
		OnBuyItemContents();
	}

	void OnBuyItemContents(){
		PurchaseType = 3;
		if(buyID >= 8501 && buyID <= 8505){
			StartCoroutine("processPay",(buyID));
		}else{
			if(buyID == 8500){
				if(GV.mUser.FuelCount >= GV.mUser.FuelMax){
					gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(false);
					Global.isPopUp = false;
					Global.isNetwork = false;
					_parent = gameObject.transform.FindChild("Content_Fail").gameObject;
					_parent.SetActive(true);
					gameObject.GetComponent<TweenAction>().doubleTweenScale(_parent);
					var icon = 	_parent.transform.FindChild("icon_product") as Transform;
					icon.gameObject.SetActive(false);
					//icon.GetComponent<UISprite>().spriteName = "8500";
					_parent.transform.FindChild("lbText").GetComponent<UILabel>().text ="연료충전??";	
						//KoStorage.GetKorString("60299");// 
					_parent.transform.FindChild("lbName").GetComponent<UILabel>().text =
						string.Format(KoStorage.GetKorString("72315"),1);
						//"연료가 다 차서 구매할 수 없습니다.";
					_parent.transform.FindChild("btnok").GetComponentInChildren<UILabel>().text = 
						KoStorage.GetKorString("71000");//TableManager.ko.dictionary["60184"].String;
					Callback(false);
					return;
				}
			}

		//	ProtocolManager.instance.addServerDataField("nItemId", buyID.ToString());
		//	string strUrl = ServerStringKeys.API.buyItem;
		//	ProtocolManager.instance.ConnectServer(strUrl, responsebuyItem);
		}
		return;
	}

	void FinishBuyAction(System.Uri uri){
		//Global.coin = (long)ProtocolManager.instance.GetIntUriQuery(uri,"nCoin");
	//	ProtocolManager.instance.updateMyCoin(uri);
	//	ProtocolManager.instance.updateMyDollar(uri);
		if(buyID == 8500 || buyID ==8501){
		//	int maxfuel  = ProtocolManager.instance.GetIntUriQuery(uri,"nMaxFuel");
		//	GV.mUser.FuelCount =  ProtocolManager.instance.GetIntUriQuery(uri,"nFuel");
			GameObject.Find("LobbyUI").SendMessage("FuelAdd");
		//	checkExpandFuel(maxfuel);
		//	if(GV.mUser.FuelMax  == GV.mUser.FuelCount ) UserDataManager.instance.fuelTimeStop();// CancelInvoke("fuelTimeCheck");
			return;
		}
		if(buyID > 8508 && buyID < 8512) {
		//	int cardnum=  ProtocolManager.instance.GetIntUriQuery(uri,"nCard5"); // sample
			//myAccount.instance.updateBuyedCardquantity(8605, cardnum);
		}
	}


	void checkExpandFuel(int max){
		if(GV.mUser.FuelMax== max) return;
		else {
			GV.mUser.FuelMax= max;
			//GameObject.Find("LobbyUI").SendMessage("InitFuelCount");
		//	var child = GameObject.FindGameObjectWithTag("CoinShop").transform.FindChild("View") as Transform;
			//child.FindChild("Grid").GetComponent<coinshopaction>().FuelExpandComplete();
		}
	}
	
	
	
	IEnumerator processPay(int id){
		var pop = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
		pop.StartCoroutine("buyLoading");
		string buyItemId = string.Empty;
		switch(id){
		case 8501:
			buyItemId = "fuelexpand";
			break;
		case 8502 : 
			buyItemId = "coin001"; // 5000
			break;
		case 8503:
			buyItemId = "coin002"; // 10000
			break;
		case 8504:
			buyItemId = "coin003"; //20000
			break;
		case 8505:
			buyItemId = "coin006"; //50000
			break;
		}
		bool iswaiting = true;
		int isfailed= 0;
		#if UNITY_ANDROID && !UNITY_EDITOR
	
		#else
		if(id != 8501){
			int tempid = Base64Manager.instance.GlobalEncoding(Global.gSale);
			id += tempid;
		}
		isfailed = 0;
		#endif
		pop.isBuyWait = false;
		if(isfailed == 1){
			//pop.isBuyWait = false;
			OnBuyCashFailed();
			
		}else{
			iswaiting = false;
		//	ProtocolManager.instance.addServerDataField("nItemId",id.ToString());
		//	string strUrl = ServerStringKeys.API.buyItem;
		//	ProtocolManager.instance.ConnectServer(strUrl,(uri)=>{
		//		bool isSuccess = false;
		//		int ret = ProtocolManager.instance.GetIntUriQuery(uri, "nRet");
		//		if(ret  == 1){
		//			isSuccess = true;
		//			FinishBuyAction(uri);
		//		}else{
		//			
		//		}
		//		Global.isNetwork = false;
		//		iswaiting = false;
		//		ResponseFinish(isSuccess);
		//	});
			while(iswaiting){
				yield return null;
			}
		}
		pop = null;
	}
	

	void CheckClassficationType(string type, int buyId){
		string strUrl= string.Empty;
		switch(type){
		case "Car":
		//	ProtocolManager.instance.addServerDataField("nCarId", buyId.ToString());
		//	strUrl = ServerStringKeys.API.buyCar;
		//	ProtocolManager.instance.ConnectServer(strUrl, responsebuyCar);
			PurchaseType = 0;
			buyCallback = ()=>{
				//myAccount.instance.updateCarInfo(buyId);
			};
			break;
		case "Crew":
		//	ProtocolManager.instance.addServerDataField("nCrewId", buyId.ToString());
		//	strUrl = ServerStringKeys.API.buyCrew;
		//	ProtocolManager.instance.ConnectServer(strUrl, responsebuyCrew);
			PurchaseType = 1;
			buyCallback = ()=>{
				//myAccount.instance.updateCrewInfo(buyId);
			};
			break;
		case "Sponsor":{
			/*ProtocolManager.instance.addServerDataField("nSponsorId", buyId.ToString());
			strUrl = ServerStringKeys.API.buySponsor;
			ProtocolManager.instance.ConnectServer(strUrl, responsebuySponsor);
	*/

			buyCallback = ()=>{
				PurchaseType = 2;
				myTeamInfo myTeam = GV.getTeamTeamInfo(GV.SelectedTeamID);
				if(myTeam == null) {
					Utility.LogWarning("myTeam null"); return; 
				}
				myTeam.SponID = buyId;
				System.DateTime sponTime = NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
				sponTime = sponTime.AddHours(12);
				myTeam.SponRemainTime = sponTime.Ticks;
			};
			responsebuySponsor(buyId);
		}break;
		default:{
			PurchaseType = 3;
			OnBuyItemContents();
		}break;
		
		}
	}

	private bool CheckBuyMoney(){
		if(buyType == 1){
			int coin = GV.myCoin;
			coin -= buyMoney;
			if(coin < 0) return false;
			else {
				GV.myCoin -= buyMoney;
				GV.updateCoin  = buyMoney;
				return true;
			}
		}else{
			int dollar = GV.myDollar;
			dollar -= buyMoney;
			if(dollar < 0) return false;
			else {
				GV.myDollar -= buyMoney;
				GV.updateDollar  = buyMoney;
				return true;
			}
		
		}

	}


	void responsebuySponsor(int sponID){
		bool isSuccess = CheckBuyMoney();
		if(isSuccess){
			Vibration.OnSponsorTime();
			if(buyCallback != null)
				buyCallback();
			GV.TeamChangeFlag = 4;
		}
		buyCallback = null;
		ResponseFinish(isSuccess);
	}


	void responsebuyCar(System.Uri uri){
		bool isSuccess = false;
	/*	int ret = ProtocolManager.instance.GetIntUriQuery(uri, "nRet");
		if(ret  == 1){
			isSuccess = true;
			ProtocolManager.instance.updateMyCoin(uri);
			ProtocolManager.instance.updateMyDollar(uri);
		}else{
		}

		if(isSuccess){
			if(buyCallback != null)
				buyCallback();
		}
		buyCallback = null;
		ResponseFinish(isSuccess); */
	}

	void responsebuyCrew(System.Uri uri){
	/*	bool isSuccess = false;
		int ret = ProtocolManager.instance.GetIntUriQuery(uri, "nRet");
		
		if(ret  == 1){
			isSuccess = true;
			ProtocolManager.instance.updateMyCoin(uri);
			ProtocolManager.instance.updateMyDollar(uri);
		}else{
		
		}

		if(isSuccess){
			if(buyCallback != null)
				buyCallback();
			Global.isDailyReset = true;

		}
		buyCallback = null;
		ResponseFinish(isSuccess);*/
	}


	void responsebuyItem(System.Uri uri){
	/*	bool isSuccess = false;
		int ret = ProtocolManager.instance.GetIntUriQuery(uri, "nRet");
	   if(ret  == 1){
			isSuccess = true;
			FinishBuyAction(uri);
			isSuccess = buyItems();

		}else if(ret == 3){
			if(buyID  == 8500){
				isSuccess = false;
			}
		}
		ResponseItem(isSuccess);
		*/
	}


	void ResponseItem(bool isSuccess){

		ResponseFinish(isSuccess);
	}
	//string coinMessage = string.Empty;
	bool buyItems(){
		bool b = false;
	//	long m = 0;
		b = true;
		switch(buyItem){
		case "Fuel":
			if(buyID == 8500){
				if(GV.mUser.FuelMax == GV.mUser.FuelCount) {
					//b = false;	
					//coinMessage = "Fuel is not Empty";
				}else{
					//Global.fuel++;
					//Global.fuel = 5000;
					//Global.coin = m;
				}
			}else{
				if(GV.mUser.FuelMax== 10) {
					
				}
			}
			break;
		case "Dollar":
			//long d = 0;
			//if(buyItem == 8505) d = (long)item.Quantity;
			//Global.dollar += item.Quantity;
			//Global.coin = m;
			break;
		case "Cash":
			//coinMessage  = " Cash is not ready , Sorry";
			//UserDataManager.instance.StartShowTip(coinMessage);
			//b= false;
			//Global.coin += item.Quantity;
			break;
		case "Card":
			//int d1 = item.Quantity;
			//myAccount.instance.updateCardInfo(8605, d1);
			//Global.coin = m;
			break;
		}

		//if(b) Global.coin = m;
		return b;
	}

	void 	OnBuyCashFailed(){
		Global.isPopUp = false;
		Global.isNetwork = false;
		gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(false);
		_parent = gameObject.transform.FindChild("Content_Fail").gameObject;
		_parent.SetActive(true);
		gameObject.GetComponent<TweenAction>().doubleTweenScale(_parent);
		var icon = 	_parent.transform.FindChild("icon_product") as Transform;
		icon.gameObject.SetActive(false);
		_parent.transform.FindChild("lbText").GetComponent<UILabel>().text =
			KoStorage.GetKorString("72008");//	"구매 취소";//KoStorage.getStringDic("60086");//TableManager.ko.dictionary["60086"].String;
		_parent.transform.FindChild("lbName").GetComponent<UILabel>().text =
			KoStorage.GetKorString("72009");	//.	"구매를 취소하였습니다. ";//KoStorage.getStringDic("60086");//TableManager.ko.dictionary["60087"].String;
		_parent.transform.FindChild("btnok").GetComponentInChildren<UILabel>().text = 
			KoStorage.GetKorString("71000");//		"확인";//KoStorage.GetKorString("71000");//TableManager.ko.dictionary["60184"].String;
		isCash = true;
		Callback(false);
	}

	void ResponseFinish(bool b){
		gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(false);
		//string[] _name = gameObject.name.Split("_"[0]);
		//Global.isPopUp = false;
		Global.isNetwork = false;
		if(b) {
			//Utility.LogWarning("Successed");
			var lobby = GameObject.Find("LobbyUI") as GameObject;
			lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
			lobby.SendMessage("PlusBuyedItem",PurchaseType, SendMessageOptions.DontRequireReceiver);

			if(buyCallback != null){
				buyCallback();
				buyCallback = null;
			}else{
				GameObject.Find("Audio").SendMessage("CompleteSound");
			}

			if(PurchaseType >= 2){
				OnCloseClick();
			}else{
				var temp = gameObject.transform.FindChild("Content_Success") as Transform;
				temp.gameObject.SetActive(true);
				temp.FindChild("lbName").GetComponent<UILabel>().text = 
					string.Format("바로 사용할래???", buyName);
				//	string.Format(KoStorage.getStringDic("60189"), buyName);
				temp.FindChild("lbText").GetComponent<UILabel>().text = 
					KoStorage.GetKorString("76031");//.String;
				temp.FindChild("btnok").GetComponent<UIButtonMessage>().functionName = "OnTeamChange";
				temp.FindChild("btnok").GetComponentInChildren<UILabel>().text = 
					KoStorage.GetKorString("71000");
			}
		}
		else{
			_parent = gameObject.transform.FindChild("Content_Fail").gameObject;
			_parent.SetActive(true);
			gameObject.GetComponent<TweenAction>().doubleTweenScale(_parent);

			string buyRes = string.Empty;
			if(buyType == 1) buyRes = "icon_coin";
			else buyRes = "icon_dollar";
			var icon = 	_parent.transform.FindChild("icon_product") as Transform;
			icon.gameObject.SetActive(false);
			icon.GetComponent<UISprite>().spriteName = buyRes;
			_parent.transform.FindChild("lbText").GetComponent<UILabel>().text =
				KoStorage.GetKorString("76022");//TableManager.ko.dictionary["60086"].String;
			_parent.transform.FindChild("lbName").GetComponent<UILabel>().text =
				KoStorage.GetKorString("76022");//TableManager.ko.dictionary["60087"].String;
			_parent.transform.FindChild("btnok").GetComponentInChildren<UILabel>().text = 
				KoStorage.GetKorString("71000");//TableManager.ko.dictionary["60184"].String;
		}
		Callback(b);
	}


	void OnTeamChange(){
		/*
		Global.isNetwork = true;
		if(buyID < 1199){
			ProtocolManager.instance.addServerDataField("nCarId", buyID.ToString());
			string strUrl = ServerStringKeys.API.setSelectedCar;
			ProtocolManager.instance.ConnectServer(strUrl, responseCar);
		}else{
			ProtocolManager.instance.addServerDataField("nCrewId", buyID.ToString());
			string strUrl = ServerStringKeys.API.setSelectedCrew;
			ProtocolManager.instance.ConnectServer(strUrl, responseCrew);
		}*/
		//idx[1] = buyID;
	}

	void responseCar(System.Uri uri){
		string nRet = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("nRet");
		int ret = 0;
		int.TryParse(nRet, out ret);
		int[] idx = new int[2];
		int tmepid = GV.getTeamCarID(GV.SelectedTeamID);//Base64Manager.instance.GlobalEncoding(Global.MyCarID);
		if(ret == 1){
			idx[0] = tmepid;
		//	Global.MyCarID = Base64Manager.instance.GlobalEncoding(buyID);
			idx[1] = buyID;
			var lobby = GameObject.Find("LobbyUI") as GameObject;
			lobby.SendMessage("SelectedTeamChange",idx,SendMessageOptions.DontRequireReceiver);
			lobby.SendMessage("SelectedTeamLVChange");

		}else{
			//Utility.Log("responscar " +ret);
		}
		GameObject.Find("Audio").SendMessage("CompleteSound");
		Global.isNetwork = false;
		OnCloseClick();
	}

	void responseCrew(System.Uri uri){
		string nRet = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("nRet");
		int ret = 0;
		int.TryParse(nRet, out ret);
		int[] idx = new int[2];
		int tmepid = GV.getTeamCrewID(GV.SelectedTeamID);//Base64Manager.instance.GlobalEncoding(Global.MyCrewID);
		if(ret == 1){
			idx[0] = tmepid;
		//	Global.MyCrewID = Base64Manager.instance.GlobalEncoding(buyID);
			idx[1] = buyID;
			var lobby = GameObject.Find("LobbyUI") as GameObject;
			lobby.SendMessage("SelectedTeamChange",idx,SendMessageOptions.DontRequireReceiver);
			lobby.SendMessage("SelectedTeamLVChange");
		}else{
		//	Utility.Log("responscrew " +ret);
		}
		GameObject.Find("Audio").SendMessage("CompleteSound");
		Global.isNetwork = false;
		OnCloseClick();
	}


	void OnFailClick(){
		if(!isCash){
			GameObject.Find("LobbyUI").SendMessage("OnDollarClick");
			OnCloseClick();
			//Utility.Log ("OnFailClick");
		}else{
			OnCloseClick();
		}
	}

	void OnCloseClick(){
		Utility.LogWarning("modify - oncloseClick");
	//	gameObject.SetActive(false);
	//	Global.isNetwork = false;Global.isPopUp = false;Destroy(this);
	//	return;

		if(Global.isNetwork) return;
		for(int i = 0; i < transform.childCount;i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}
		transform.FindChild("bg").gameObject.SetActive(true);
		var temp = transform.FindChild("Content_BUY") as Transform;
		temp.gameObject.SetActive(true);
		gameObject.SetActive(false);
		temp.FindChild("lbName1").gameObject.SetActive(false);
		temp.FindChild("lbName").gameObject.SetActive(true);
		temp.FindChild("Sprite (Check_V)").gameObject.SetActive(true);	
		Global.isPopUp = false;
		Destroy(this);
		return;
	}
	
	

	/*void SponsorTimeCheck(){
		//myAccount.instance.account.sponsorInfo.expireTime = System.DateTime.Now.Ticks;
	}*/


}
