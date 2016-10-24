using UnityEngine;
using System.Collections;


public class ShopMenu : MonoBehaviour {
//	public GameObject CenterMenu;
	GameObject StatusObj=null, InfoObj = null, myObj = null, CoinObj= null;
	bool isCoin = false;GameObject InfoTip = null;
//	ShopCenterMenuAction _menu;
	void Awake(){
		//_menu = CenterMenu.GetComponent<ShopCenterMenuAction>();
	}
	public bool SetisCoinWindow{	get{return isCoin;}set{isCoin = value;}}

	void OnEnable(){

	}


	void OnDestroy(){
		//CenterMenu = null;
		myObj = null;
	
		if(StatusObj != null) {
			//DestroyImmediate(StatusObj);
		}
		if(InfoObj != null){
			//DestroyImmediate(InfoObj);
		}
		StatusObj = InfoObj = InfoTip = null;
	}

	public void DestroyWindowTween(GameObject obj){
		return; 
	}

	public GameObject CreateSubWindow(string sub, GameObject obj){
		myObj = GameObject.FindGameObjectWithTag("CenterAnchor") as GameObject;
		var tr = myObj.transform.GetChild(0) as Transform;
		if(obj != null) {
			var tw = obj.GetComponent<TweenAction>() as TweenAction;
			if(tw == null) tw = obj.AddComponent<TweenAction>() as TweenAction;
			tw.ReverseTween(obj);
		}
		GameObject tempObj = null;
		switch(sub){
		case "CarShopStatus":{
			myObj = GameObject.FindGameObjectWithTag("BottomAnchor") as GameObject;
			tr = myObj.transform.GetChild(0) as Transform;
			var t = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
			tempObj = t.makeCarStatus(tr, true);
			var _state = tempObj.GetComponent<StatsUpInfo>() as StatsUpInfo;
			if(_state == null) tempObj.AddComponent<StatsUpInfo>();
			Destroy(t);
			
		}
			break;
		case "CrewShopStatus":{
			var t = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
			tempObj = t.makeCrewStatus(tr,true);
			var _state = tempObj.GetComponent<StatsUpInfo>() as StatsUpInfo;
			if(_state == null) tempObj.AddComponent<StatsUpInfo>();
			Destroy(t);
		}
			break;
		case "CrewShopInfo":{
			var t = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
			tempObj = t.makeCrewShopInfo(tr);
			Destroy(t);
		}
			break;
		case "CarShopInfo":{
			myObj = GameObject.FindGameObjectWithTag("BottomAnchor") as GameObject;
			tr = myObj.transform.GetChild(0) as Transform;
			var t = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
			tempObj = t.makeCarShopInfo(tr);
			Destroy(t);
		}
			break;
		case "SponsorInfo":{
			myObj = GameObject.FindGameObjectWithTag("BottomAnchor") as GameObject;
			tr = myObj.transform.GetChild(0) as Transform;
			var t = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
			tempObj = t.makeSponsorShopInfo(tr);
			Destroy(t);
		}break;
		case "CoinShop":{
			var t = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
			tempObj = t.makeCoinShopInfo(tr);
			Destroy(t);
		}
			break;
		case "CarShowInfo":{
			myObj = GameObject.FindGameObjectWithTag("BottomAnchor") as GameObject;
			tr = myObj.transform.GetChild(0) as Transform;
			var t = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
			tempObj = t.makeCarShowInfo(tr);
			Destroy(t);
		}
			break;
		}
		return tempObj;
	}

	void SetWindowInfomation(GameObject obj){
	
	}

	public void ShopStart(string name){
	
		if(isCoin){
			return;
		}else{
			if(name.Equals("Car"))
				GetComponent<LobbyManager>().SetShopButton("Car");
			else if(name.Equals("Deal"))
				GetComponent<LobbyManager>().SetShopButton("Deal");
			else GetComponent<LobbyManager>().SetShopButton("ShowRoom");
			isCoin = false;
		}
	}
	
	public void dealerStart(){
	
		GetComponent<LobbyManager>().SetShopButton("Car");
		isCoin = false;
	}
	
	public void ShopStop(){
		HiddenWindow();
		System.GC.Collect();
	}

	void OnCarShop(){
		if(Global.isAnimation) return;
	//	bool b =_menu.OnCarClick();
		bool b = true;
		if(b) return;
		GetComponent<LobbyManager>().settingDealInfo();
		GetComponent<LobbyManager>().SetShopButton("Car");
		//	CoinWindowDestroy();
		isCoin = false;
		//Utility.Log(StatusObj.name);
		if(StatusObj != null){
			StatusObj.GetComponent<TweenAction>().ReverseTween(StatusObj);
			StatusObj = null;
		}
		if(InfoObj != null){
			InfoObj.GetComponent<TweenAction>().ReverseTween(InfoObj);
			InfoObj = null;
		}
		if(InfoTip != null){
			InfoTip.transform.parent.GetComponent<TweenAction>().ReverseTween(InfoTip);
			InfoTip.transform.parent.GetComponent<TweenAction>().ReverseTween(InfoTip.transform.parent.gameObject);
			InfoTip = null;
		}
	//	_menu.CheckNewButton("MyCarShop");
	}

	void OnCrewShop(){
		if(Global.isAnimation) return;
		//bool b = _menu.OnCrewClick();
		bool b = true;
		if(b) return;

		//CenterButton(false, true, false);
		GetComponent<LobbyManager>().SetShopButton("Crew");
		GetComponent<LobbyManager>().settingDealInfo();
		CoinWindowDestroy();
		isCoin = false;
		if(StatusObj != null){
			StatusObj.GetComponent<TweenAction>().ReverseTween(StatusObj);
			StatusObj = null;
		}else{
			
		}
		if(InfoTip != null){
			InfoTip.transform.parent.GetComponent<TweenAction>().ReverseTween(InfoTip);
			InfoTip.transform.parent.GetComponent<TweenAction>().ReverseTween(InfoTip.transform.parent.gameObject);
			InfoTip = null;
		}
		//_menu.CheckNewButton("MyCrewShop");
	}

	void OnCoinShop(){
		if(Global.isAnimation) return;
		GetComponent<LobbyManager>().SetShopButton("Coin");
		if(StatusObj != null) {
			StatusObj.GetComponent<TweenAction>().ReverseTween(StatusObj);
		}
		if(InfoObj != null) {
			InfoObj.GetComponent<TweenAction>().ReverseTween(InfoObj);
		}
		if(InfoTip != null){
			InfoTip.transform.parent.GetComponent<TweenAction>().ReverseTween(InfoTip);
			InfoTip.transform.parent.GetComponent<TweenAction>().ReverseTween(InfoTip.transform.parent.gameObject);
		}
		StatusObj = InfoObj = null;
		InfoTip = null;
		CoinObj = CreateSubWindow("CoinShop", null);
		isCoin = true;
		SettingCoinShop();
	}

	void CoinWindowDestroy(){
		if(CoinObj == null) return;
		if(CoinObj != null){
			CoinObj.transform.FindChild("window").gameObject.SetActive(false);
			var obj  = CoinObj.transform.FindChild("View").gameObject as GameObject;
			var tween = obj.GetComponent<TweenPosition>() as TweenPosition;
			if(tween == null) {
			}else {
				Vector3 to = tween.from;
				tween.from = tween.to;
				tween.to = to;
				tween.onFinished = delegate(UITweener tween1) {
					tween1.duration = 0.3f;
					tween1.transform.parent.gameObject.SetActive(false);
				};
				tween.duration = 0.1f;
				tween.Reset();
				tween.enabled=true;
			}
			CoinObj = null;
		}
	}

	void CoinDestroy(TweenPosition tw){
		tw.duration = 0.3f;
		tw.transform.parent.gameObject.SetActive(false);
	}

	void SettingCoinShop(){
	//	var obj  = CoinObj.transform.FindChild("View").gameObject as GameObject;
	//	var tempObj = obj.transform.FindChild("Grid") as Transform;
//		CoinObj.transform.FindChild("window").gameObject.SetActive(true);
		//CoinObj.GetComponent<TweenAction>().ForwardPlayTween(obj);
	//	GetComponent<ShopItemCreate>().SettingGridItemShop(tempObj, "Slot_Coin");
	}

	void EnableButton(Transform menu, int count){
		TweenColor[] arr = new TweenColor[2];
		arr = menu.GetComponents<TweenColor>();
		TweenScale[] arr1 = new TweenScale[2];
		arr1 = menu.GetComponents<TweenScale>();// as TweenScale;
		arr[count].Reset();// =true;
		arr1[count].Reset();// = true;
		arr[count].enabled =true;
		arr1[count].enabled = true;
	}

	void CenterButton(bool car, bool crew, bool coin){
		/*if(car){
			return;
		}
		if(crew){
			return;
		}
		if(coin){
			return;
		}
		return;

		for(int i = 0; i < 3; i++){
			//var menu1 = CenterMenu.transform.GetChild(i) as Transform;
			//Utility.Log ("button " + menu1.name);
			//var temp = menu1.FindChild("activate").gameObject as GameObject;
			//if(temp.activeSelf){
			//	EnableButton(menu1, 0);
			//}
		}

		var menu = CenterMenu.transform.GetChild(0) as Transform;
		menu.GetComponent<BoxCollider>().enabled = !car;
		//menu.FindChild("activate").gameObject.SetActive(car);
		if(car){
			EnableButton(menu, 1);
		}else{
			EnableButton(menu, 0);
		}
		menu = CenterMenu.transform.GetChild(2) as Transform;
		menu.GetComponent<BoxCollider>().enabled = !crew;
		//menu.FindChild("activate").gameObject.SetActive(crew);
		if(crew){
			EnableButton(menu, 1);
		}else{
			EnableButton(menu, 0);
		}
		menu = CenterMenu.transform.GetChild(1) as Transform;
		menu.GetComponent<BoxCollider>().enabled = !coin;
		//menu.FindChild("activate").gameObject.SetActive(coin);
		if(coin){
			EnableButton(menu, 1);
		}else{
			EnableButton(menu, 0);
		}*/
	}

	public void DestroyStatusWin(){
		if(InfoObj != null){
			var tween = InfoObj.GetComponent<TweenAction>() as TweenAction;
			if(tween == null) tween = InfoObj.AddComponent<TweenAction>();
			tween.ReverseTween(InfoObj);
			InfoObj = null;
		}
	}


	public void ShowWindowInfo(string str, GameObject item){
		string[] tempstring = str.Split("_"[0]);
		var _obj  = StatusObj as GameObject;
		if(tempstring[1].Equals("Dealer")){
			
		}else{
			if(StatusObj == null)	{
				StatusObj = CreateSubWindow("CarShopStatus", _obj);
			}
			_obj = InfoObj as GameObject;
			StatusObj.name = str;
			StatusObj.GetComponent<StatsUpInfo>().ChangeStatusItem();
		}
		
		InfoObj = CreateSubWindow("CarShowInfo", _obj);
		InfoObj.name = str;
		int tipnum = InfoObj.GetComponent<ShopShowRoom>().ChangeShowContents();
		var tip = gameObject.GetComponent<modeInfoAction>() as modeInfoAction;
		if(tip == null) tip = gameObject.AddComponent<modeInfoAction>();
		if(InfoTip == null) {
			if(tipnum == 0)	InfoTip = tip.CreateShopInfo(4012);
			else if(tipnum == 1) InfoTip = tip.CreateShopInfo(4013);
			
		}
	}

	public void ChangeToClassStatus(string CarClass){
		StatusObj.GetComponent<StatsUpInfo>().ChangeStatusClass(CarClass);
	}

	public void ResetWindow(string str, GameObject item){
		//Utility.Log("Tst");
		string[] tempstring = str.Split("_"[0]);
		var _obj  = StatusObj as GameObject;
		if(tempstring[1].Equals("Car")){
			if(StatusObj == null)	{
				StatusObj = CreateSubWindow("CarShopStatus", _obj);
			}
			_obj = InfoObj as GameObject;
			//InfoObj = CreateSubWindow("CarShopInfo", _obj);
			InfoObj = CreateSubWindow("CarShowInfo", _obj);
			StatusObj.name = str;
			InfoObj.name = str;
			StatusObj.GetComponent<StatsUpInfo>().ChangeStatusItem();
			//InfoObj.GetComponent<buyaction>().ChangeContents();
			int tipnum = InfoObj.GetComponent<ShopShowRoom>().ChangeShowContents();
			var tip = gameObject.GetComponent<modeInfoAction>() as modeInfoAction;
			if(tip == null) tip = gameObject.AddComponent<modeInfoAction>();
			if(InfoTip == null) {
				if(tipnum == 0)	InfoTip = tip.CreateShopInfo(4012);
				else InfoTip = tip.CreateShopInfo(4013);
			}
		}else if(tempstring[1].Equals("Crew")){
			 _obj = StatusObj as GameObject;
			if(StatusObj == null)	StatusObj = CreateSubWindow("CrewShopStatus", _obj);
				_obj = InfoObj as GameObject;
			InfoObj = CreateSubWindow("CrewShopInfo", _obj);
			StatusObj.name = str;
			InfoObj.name = str;
			//InfoObj.GetComponent<buyaction>().ChangeContents();
			StatusObj.GetComponent<StatsUpInfo>().ChangeStatusItem();
			
			//if(InfoTip == null) {
			//	InfoTip = gameObject.AddComponent<modeInfoAction>().CreateShopInfo(155);
			//}
		}else{
			
		}

	}

	public void HiddenWindow(){
		if(StatusObj != null) {
			StatusObj.GetComponent<TweenAction>().ReverseTween(StatusObj);
		}//DestroyWindowTween(StatusObj);
		if(InfoObj != null){
			InfoObj.GetComponent<TweenAction>().ReverseTween(InfoObj);
			//DestroyWindowTween(InfoObj);
		}
		if(InfoTip != null){
			InfoTip.transform.parent.GetComponent<TweenAction>().ReverseTween(InfoTip);
			InfoTip.transform.parent.GetComponent<TweenAction>().ReverseTween(InfoTip.transform.parent.gameObject);

		}
		StatusObj = InfoObj = InfoTip =  null;
		CoinWindowDestroy();
		GetComponent<ShopItemCreate>().HiddenWindow();
	}
	
	
	public void HiddenCoinWindow(){
		isCoin = false;
		if(CoinObj == null) return;
		else {
			CoinWindowDestroy();
		}
		CoinObj =null;
	}
	public void HiddenCoinShop(){
		CoinWindowDestroy();
		isCoin = false;
	}
	
	public void ShowCoinShop(){
		CoinObj = CreateSubWindow("CoinShop", null);
		isCoin = true;
		SettingCoinShop();
	}

}
