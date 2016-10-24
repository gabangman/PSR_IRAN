using UnityEngine;
using System.Collections;

public class ShopItemCreate : MonoBehaviour {


	GameObject StatusObj;
	//bool isWindow;

	public void SponsorStart(string teamName){
		StatusObj = GetComponent<ShopMenu>().CreateSubWindow("SponsorInfo", null);
		string str = string.Empty;
	//	int tmepid = Base64Manager.instance.GlobalEncoding(Global.MySponsorID);
	//	if(tmepid == 1300){
	//		str = "1301_Sponsor";
	//	}else{
	//		str = "1301_Sponsor";
	//	}
		if(teamName.Equals("Stock")){
			str = "1301_Sponsor";//+GV.StockTeamCode.ToString();
		}else{
			str = "1301_Sponsor";//+GV.TourTeamCode.ToString();
		}
		StatusObj.name= str;
		StatusObj.GetComponent<buyaction>().ChangeContents();
		var tip = gameObject.GetComponent<modeInfoAction>() as modeInfoAction;
		if(tip == null) tip = gameObject.AddComponent<modeInfoAction>();
		InfoTip =tip.CreateSponsorInfo(6103);
	}
	
	public void SponsorStop(){
		if(StatusObj != null){
			StatusObj.GetComponent<TweenAction>().ReverseTween(StatusObj);
		}
		if(InfoTip != null) {
			InfoTip.transform.parent.GetComponent<TweenAction>().ReverseTween(InfoTip.transform.parent.gameObject);
			InfoTip.transform.parent.GetComponent<TweenAction>().ReverseTween(InfoTip);
		}
		StatusObj = InfoTip= null;
	}
	
	public void HiddenWindow(){
		if(StatusObj != null) {
			StatusObj.GetComponent<TweenAction>().ReverseTween(StatusObj);
		}
		if(InfoTip != null){
			InfoTip.transform.parent.GetComponent<TweenAction>().ReverseTween(InfoTip.transform.parent.gameObject);
			InfoTip.transform.parent.GetComponent<TweenAction>().ReverseTween(InfoTip);
		}
		StatusObj = InfoTip= null;
		
	}
	GameObject InfoTip = null;
	int CountTableString(string str){
		int count = 0;
		switch(str){
		case "Slot_Sponsor":
			count = Common_Sponsor_Status.GetDictionaryCount();
			count -= 1;
			break;
		case "Slot_Car":
			count = Common_Car_Status.wholeCarList.Count;
			break;
		case "Slot_Crew":
			count = Common_Crew_Status.crewListItem.Count;
			break;
		case "Slot_Coin":
			 count = Common_Cost.GetDictionaryCount();
			 count -=5;
			break;
		default:
			break;
		}

		return count;
	}




	public void ChangeCarGridItems(Transform grid, string _itemName){
		if(grid.childCount !=  0 ) 
		{
			CreateCarSlotItem(grid, _itemName);
			return;
		}
		//int count =  Table_Car.GetCount();
		int count = Common_Car_Status.wholeCarList.Count;
		var pop = gameObject.GetComponent<CPopUpCreate>() as CPopUpCreate;
		if(pop == null) pop = gameObject.AddComponent<CPopUpCreate>();
		for(int i = 0; i < count; i++){
			var _obj = pop.CreatePrefabsItems("Slot_Car", grid.gameObject) as GameObject;
			_obj.SetActive(true);
			_obj.transform.parent =grid;
			_obj.transform.localScale = Vector3.one;
			_obj.transform.localPosition = Vector3.zero;
			_obj.transform.localEulerAngles = Vector3.zero;
		}
		Destroy(pop);
		CreateCarSlotItem(grid, _itemName);
	}

	void CreateCarSlotItem(Transform _grid, string _itemName){
		string name = string.Empty;
		int _length = 0;
		int[] tempList;//;
		if(_itemName.Equals("Slot_Car")){
			name = "_ssCar";
			tempList = Common_Car_Status.SpecialCarList.ToArray();
		}else{
			name = "_tCar";
		//	tempList = Table_Car.listCarCode.ToArray();
			tempList = Common_Car_Status.wholeCarList.ToArray();

		}
		_length = tempList.Length;
		string _id = null;
		for(int i = 0 ; i < _grid.transform.childCount; i++){
			var temp = _grid.transform.GetChild(i).GetChild(0) as Transform;
			if(i < _length){
				Common_Car_Status.Item tItem=Common_Car_Status.Get(tempList[i]);
				_id = tItem.ID;
				temp.name = _id+name+"_"+tItem.Class;//+"_"+"Car";
				temp.parent.name = _id+name;//"_"+"Car";
				temp.FindChild("Image").gameObject.SetActive(true);
				temp.FindChild("Image").GetComponent<UISprite>().spriteName = _id;
			}else{
				_id = "99";
				temp.name = _id+name;
				temp.parent.name = _id+name;
				temp.FindChild("Image").gameObject.SetActive(false);
			}
			var btn1 = temp.gameObject.GetComponent<ShopItemAction>() as ShopItemAction;
			if(btn1 == null) btn1 = temp.gameObject.AddComponent<ShopItemAction>();
			var btn = temp.gameObject.GetComponent<ShopSlotAction>() as ShopSlotAction;
			if(btn == null) btn= temp.gameObject.AddComponent<ShopSlotAction>();
			btn.ShopCarSlotSetting();
		}

		_grid.transform.GetChild(0).GetChild(0).FindChild("Select").gameObject.SetActive(true);
		GetComponent<LobbyManager>().OnItemClicked(_grid.transform.GetChild(0).GetChild(0).gameObject);//StartCoroutine("OnShopCar",temp.gameObject);
		_grid.GetComponent<UIGrid>().repositionNow = true;
	
	}

	public void ChangeContentSeasonUp(Transform _grid, string _item){
		_grid.parent.parent.GetComponent<newButtonAction>().setButton(_item);
	}

	public void SettingGridItemShop(Transform _grid, string _item){
		if(!_grid.parent.gameObject.activeSelf) 	_grid.parent.gameObject.SetActive (true);
		if(_grid.childCount !=  0 ) 
		{
			if(_item == "Slot_Coin"){
				_grid.GetComponent<UIGrid>().repositionNow = true;
				return;
			}
			if(_item == "Slot_Sponsor") {
				InitSponsorShop(_grid); 
				return;
			}
			InitSelectButton(_grid, _item);
			ChangeContentSeasonUp(_grid,_item);
			return;
		}
		int count =  CountTableString(_item);
		if(count < 5) count =5;
		var pop = gameObject.GetComponent<CPopUpCreate>() as CPopUpCreate;
		if(pop == null) pop = gameObject.AddComponent<CPopUpCreate>();

		for(int i = 0; i < count; i++){
			var _obj = pop.CreatePrefabsItems(_item, _grid.gameObject) as GameObject;
			_obj.SetActive(true);
			_obj.transform.parent =_grid;
			_obj.transform.localScale = Vector3.one;
			_obj.transform.localPosition = Vector3.zero;
			_obj.transform.localEulerAngles = Vector3.zero;
		}
		Destroy(pop);
		//Resources.UnloadUnusedAssets();
		InitGridItem(_grid, _item);
			
	}

	const int COINSHOP_ID = 8500;
	const int CARSHOP_ID =1000 ;
	const int CREWSHOP_ID = 1200;
	const int SPONSORSHOP_ID = 1300;

	string ResourceToString(int res) {
		string str = string.Empty;
		switch(res){
		case 0:
			str = "Free"; break;
		case 1:
			str =  "Fuel"; break;
		case 3:
			str = "Cash"; break;
		case 4:
			str =  "Dollar";break;
		case 5:
			str = "Qube";break;
		case 2:
			str = "Fuel";break;
		case 6 :
			str = "Silver";break;
		case 7:
			str = "Gold";break;
		
		default:
			break;
		}
		return str;
	}

	void InitGridItem(Transform _grid, string _item){
		switch(_item){
		case "Slot_Car":
			InitCarShop(_grid);
			break;
		case "Slot_Crew":
			InitCrewShop(_grid);
			break;
		case "Slot_Sponsor":
			InitSponsorShop(_grid);
			break;
		case "Slot_Coin":
			InitCoinShop(_grid);
			break;
		default:
			break;
		}
	}
	void InitSelectButton(Transform _grid, string str){
		for(int i = 0 ; i < _grid.transform.childCount; i++){
			var temp = _grid.transform.GetChild(i).GetChild(0) as Transform;
			var select = temp.FindChild("Select").gameObject as GameObject;
			select.SetActive(false);
			var tbtn = temp.GetComponent<myteambtnaction>() as myteambtnaction;
			if(tbtn != null)
				temp.GetComponent<myteambtnaction>().ReSetButtonInit(str,temp);
			if(i == 0) {
				temp.FindChild("Select").gameObject.SetActive(true);
				if(str.Equals("Slot_Crew"))
					GetComponent<LobbyManager>().OnItemClicked(temp.gameObject);
				else if(str.Equals("Slot_Car"))	GetComponent<LobbyManager>().OnItemClicked(temp.gameObject);//("OnShopCar",temp.gameObject);
			}
		}
	}

	void InitCarShop(Transform _grid){
	//	myAccount.instance.InitShopButton();
		for(int i = 0 ; i < _grid.transform.childCount; i++){
			int _id= Common_Car_Status.wholeCarList[i];
			var temp = _grid.transform.GetChild(i).GetChild(0) as Transform;
			temp.gameObject.name = _id.ToString()+"_"+"Car";
			temp.parent.name = _id.ToString()+"_"+"Car";
			temp.FindChild("Image").GetComponent<UISprite>().spriteName = _id.ToString();
			temp.gameObject.AddComponent<ShopItemAction>();
			if(i == 0) {
				temp.FindChild("Select").gameObject.SetActive(true);
				GetComponent<LobbyManager>().OnItemClicked(temp.gameObject);//StartCoroutine("OnShopCar",temp.gameObject);
			}
			temp.gameObject.AddComponent<myteambtnaction>().CarButtonSet(false);
		}
		_grid.GetComponent<UIGrid>().repositionNow = true;
		_grid.parent.parent.GetComponent<newButtonAction>().setCarButton();
		//_grid.parent.parent.GetComponent<newButtonAction>().setCarButton();
	}

	void InitCrewShop(Transform _grid){
		
		for(int i = 0 ; i < _grid.transform.childCount; i++){
			int _id =  Common_Crew_Status.crewListItem[i];
			var temp = _grid.transform.GetChild(i).GetChild(0) as Transform;
			temp.gameObject.name = _id.ToString()+"_"+"Crew";
			temp.parent.name = _id.ToString()+"_"+"Crew";
			temp.FindChild("Image").GetComponent<UISprite>().spriteName = _id.ToString();
			temp.gameObject.AddComponent<ShopItemAction>();
			if(i == 0) {
				temp.FindChild("Select").gameObject.SetActive(true);
				GetComponent<LobbyManager>().OnItemClicked(temp.gameObject);
				//myAccount.instance.account.buttonStatus.isCrewNew[i] = false;
			}
			temp.gameObject.AddComponent<myteambtnaction>().CrewButtonSet();

		}
		_grid.GetComponent<UIGrid>().repositionNow = true;
		_grid.parent.parent.GetComponent<newButtonAction>().setCrewButton();
	}

	void InitSponsorShop(Transform _grid){
		int tempid = GV.getTeamSponID(GV.SelectedTeamID);
		for(int i = 0 ; i < _grid.transform.childCount; i++){
			int _id = SPONSORSHOP_ID + i+1;
			var temp = _grid.transform.GetChild(i).GetChild(0) as Transform;
			temp.gameObject.name = _id.ToString()+"_"+"Sponsor";
			temp.parent.name = temp.name;
			temp.FindChild("Image").GetComponent<UISprite>().spriteName = _id.ToString();
			temp.gameObject.AddComponent<ShopItemAction>();
			if(tempid == _id){
				string str = AccountManager.instance.SponTimeCheck(0,1);
				if(string.Equals(str, string.Empty)){
					temp.FindChild("Selected").gameObject.SetActive(false);
					temp.FindChild("lbName").gameObject.SetActive(false);
				}else{
					temp.FindChild("lbName").gameObject.SetActive(true);
					temp.FindChild("lbName").gameObject.AddComponent<RunningSponTime>().SponTimeLable(temp.FindChild("lbName").GetComponent<UILabel>());
				//	temp.FindChild("lbName").GetComponent<UILabel>().text = str;
					temp.FindChild("Selected").gameObject.SetActive(true);
				}
			}else{
				temp.FindChild("lbName").gameObject.SetActive(false);
				temp.FindChild("Selected").gameObject.SetActive(false);

			}
			if(_id == 1301) {
				temp.FindChild("Select").gameObject.SetActive(true);
			}else{
				temp.FindChild("Select").gameObject.SetActive(false);
			}
		}
		_grid.GetComponent<UIGrid>().repositionNow = true;
	
	}



	void InitCoinShop(Transform _grid){
		Transform tempObj = _grid;
		tempObj.gameObject.AddComponent<coinshopaction>();
		for(int i = 0 ; i < tempObj.childCount; i++){
			int _id = COINSHOP_ID + i;
			Common_Cost.Item _item = Common_Cost.Get(_id);
			//int nPrice = _item.Cash_won;
			int nPrice = 0;
			nPrice = int.Parse(_item.Cash_won);
			int nQuantity = _item.Recharge_no;
			var temp = tempObj.GetChild(i).GetChild(0) as Transform;
			temp.FindChild("lbPrice").GetComponent<UILabel>().text = 
				System.String.Format("{0:#,0}", nPrice);
			temp.FindChild("lbName").GetComponent<UILabel>().text = 
				_item.Name;
			temp.FindChild("lbQuantity").GetComponent<UILabel>().text = 
				"x " + System.String.Format("{0:#,0}",nQuantity);
			string str = ResourceToString( System.Convert.ToInt32(_item.Recharge_type));
			//string str1 = string.Empty;
			temp.gameObject.name = _id.ToString()+"_"+ str;
			temp.parent.name = _id.ToString()+"_"+ str;
			temp.FindChild("bg_Box").GetComponent<UISprite>().spriteName = (_id).ToString();
			temp.FindChild("bg_Box").GetComponent<UISprite>().MakePixelPerfect();
			temp.FindChild("Image_sale").gameObject.SetActive(false);
			temp.FindChild("lbQuantity_Event").GetComponent<UILabel>().text = string.Empty;
			switch(str){
			case "Fuel":
			{
				var child = temp.FindChild("btnCoin") as Transform;
				child.gameObject.SetActive(true);
				var childbtn = child.GetComponent<UIButtonMessage>() as UIButtonMessage;
				childbtn.target = tempObj.gameObject;
				//childbtn.functionName = "OnItemClick";
				childbtn.functionName = "OnBuyFuel";
			}
				break;
			case "Cash":
			{
				string strUnit = string.Empty;
				string strPrice = string.Empty;
				switch(GV.gNationName){
				case "KOR":
					strUnit  = KoStorage.GetKorString("72007");strPrice = _item.Cash_won;break;
				case "JPN":
					strUnit  = KoStorage.GetKorString("72026");strPrice = _item.Cash_y;break;
				case "CHN":
					strUnit  = KoStorage.GetKorString("72025");strPrice = _item.Cash_w;break;
				case "FRA":
					strUnit  = KoStorage.GetKorString("72024");strPrice = _item.Cash_e;break;
				case "RUS":
					strUnit  = KoStorage.GetKorString("72027");strPrice = _item.Cash_r;break;
				case "USA":
					strUnit  = KoStorage.GetKorString("72023");strPrice = _item.Cash_d;break;
				case "GBR":
					strUnit  = KoStorage.GetKorString("72023");strPrice = _item.Cash_d;break;
				case "DEU":
					strUnit  = KoStorage.GetKorString("72024");strPrice = _item.Cash_e;break;
				case "ITA":
					strUnit  = KoStorage.GetKorString("72024");strPrice = _item.Cash_e;break;
				case "ESP":
					strUnit  = KoStorage.GetKorString("72024");strPrice = _item.Cash_e;break;
				case "PRT":
					strUnit  = KoStorage.GetKorString("72024");strPrice = _item.Cash_e;break;
				default: 
					strUnit  = KoStorage.GetKorString("72023");strPrice = _item.Cash_d;break;
				}
				tempObj.GetChild(i).GetChild(0) .FindChild("lbPrice").GetComponent<UILabel>().text = 
					System.String.Format("{0:#,0}", strPrice);
				
				var child = temp.FindChild("btnCash") as Transform;
				child.gameObject.SetActive(true);
				var childbtn = child.GetComponent<UIButtonMessage>() as UIButtonMessage;
				childbtn.target = tempObj.gameObject;
				childbtn.functionName = "OnBuyCash";
				temp.FindChild("lbUnit").gameObject.SetActive(true);
				temp.FindChild("lbUnit").GetComponent<UILabel>().text =strUnit;
				int tempid = Base64Manager.instance.GlobalEncoding(Global.gSale);
				if(tempid == 13){
					int _q = _item.Recharge_no;// = Common_Cost.Get(_id);
					Common_Cost.Item cItem = Common_Cost.Get(_id+tempid);
					int a = cItem.Recharge_no * GV.gInfo.CouponState;
					float b = (float) cItem.Recharge_no * GV.gInfo.plusEventRatio;
					temp.FindChild("lbQuantity_Event").GetComponent<UILabel>().text = 
						string.Format(" +{0}", (-_q+(int)b));
					temp.FindChild("Image_sale").gameObject.SetActive(true);
					cItem = null;
				}
			}
				break;
			case "Dollar":
			{
				var child = temp.FindChild("btnCoin") as Transform;
				child.gameObject.SetActive(true);
				var childbtn = child.GetComponent<UIButtonMessage>() as UIButtonMessage;
				childbtn.target = tempObj.gameObject;
				childbtn.functionName = "OnBuyDollar";
			}
				break;
			case "Qube":{
				var child = temp.FindChild("btnCoin") as Transform;
				child.gameObject.SetActive(true);
				var childbtn = child.GetComponent<UIButtonMessage>() as UIButtonMessage;
				childbtn.target = tempObj.gameObject;
				childbtn.functionName = "OnBuyCube";
			}break;
			case "Gold":{
				var child = temp.FindChild("btnCoin") as Transform;
				child.gameObject.SetActive(true);
				var childbtn = child.GetComponent<UIButtonMessage>() as UIButtonMessage;
				childbtn.target = tempObj.gameObject;
				childbtn.functionName = "OnBuyGoldCoupon";
			}break;
			case "Silver":{
				var child = temp.FindChild("btnCoin") as Transform;
				child.gameObject.SetActive(true);
				var childbtn = child.GetComponent<UIButtonMessage>() as UIButtonMessage;
				childbtn.target = tempObj.gameObject;
				childbtn.functionName = "OnBuySilverCoupon";
				
			}break;
				
			case "Free":{
				//무료 광고
				temp.FindChild("lbName").GetComponent<UILabel>().text = 
					_item.Name;
				temp.FindChild("lbQuantity").GetComponent<UILabel>().text = 
					null;//"x " + System.String.Format("{0:#,0}",nQuantity);
				temp.FindChild("lbPrice").GetComponent<UILabel>().text = 
					null;//System.String.Format("{0:#,0}", nPrice);
				var child = temp.FindChild("btnAD") as Transform;
				child.gameObject.SetActive(true);
				var childbtn = child.GetChild(0).GetComponent<UIButtonMessage>() as UIButtonMessage;
				childbtn.target = child.gameObject;
				childbtn.functionName = "OnFreeFuel";
				child.gameObject.AddComponent<BuyFreeFuel>();
				temp.FindChild("lbFull").gameObject.SetActive(true);
			}break;
			default:{
			}break;
			}		
		}
		tempObj.GetComponent<UIGrid>().repositionNow = true;
	}


	public void ResetWindow(string str, GameObject Item){
		ShopMenu shop = GetComponent<ShopMenu>() as ShopMenu;
		string[] name = str.Split("_"[0]);
		if(name[1].Equals("Sponsor")){
			StatusObj = shop.CreateSubWindow("SponsorInfo", null);
			StatusObj.name  = str;
			StatusObj.GetComponent<buyaction>().ChangeContents();
		}else if(name[1].Equals("Dealer")){
			//shop.ResetWindow(str,Item);
			shop.ShowWindowInfo(str,Item);
		}else{
			shop.ShowWindowInfo(str,Item);
			
		}
	}
	public void DestroyStatusWin(){
		ShopMenu shop = GetComponent<ShopMenu>() as ShopMenu;
		if(StatusObj != null){
			StatusObj.GetComponent<TweenAction>().ReverseTween(StatusObj);
			StatusObj =null;
		}
		shop.DestroyStatusWin();
	}

}
