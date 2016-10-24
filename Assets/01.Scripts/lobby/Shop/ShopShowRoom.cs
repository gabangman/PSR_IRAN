using UnityEngine;
using System.Collections;

public class ShopShowRoom : MonoBehaviour {
	public UILabel lbClass,lbStatus,lbprice;
	public UISprite spClass;
	public Transform[] btnClass;
	string[] ConvertToClass(int cnt){
		string[] carclass = new string[3];
		switch(cnt){
		case 1:
			carclass[0] = "D"; //lbClass
			carclass[1] ="Class_D"; //spClass
			carclass[2] ="_Car_D";
			break;
		case 2:
			carclass[0] = "C"; //lbClass
			carclass[1] ="Class_C"; //spClass
			carclass[2]  ="_Car_C";
			break;
		case 3:
			carclass[0] = "B"; //lbClass
			carclass[1] ="Class_B"; //spClass
			carclass[2] = "_Car_B";
			break;
		case 4:
			carclass[0] = "A"; //lbClass
			carclass[1] ="Class_A"; //spClass
			carclass[2]  ="_Car_A";
			break;
		case 5:
			carclass[0] = "S"; //lbClass
			carclass[1] ="Class_S"; //spClass
			carclass[2]  ="_Car_S";
			break;
		case 6:
			carclass[0] = "SS"; //lbClass
			carclass[1] ="Class_SS"; //spClass
			carclass[2]  ="_Car_SS";
			break;
		default:
			break;
		}

		return carclass;
	}

	void OnClassR(){
	//	if(classCnt == 6) {
	//		return;
	//	}
		classCnt++;
		if(classCnt == 6) btnClass[4].gameObject.SetActive(false);
		if(!btnClass[5].gameObject.activeSelf) btnClass[5].gameObject.SetActive(true);
		string[] tClass = ConvertToClass(classCnt);
		lbClass.text = tClass[0];
		spClass.spriteName = tClass[1];
		var lobby = GameObject.Find("LobbyUI") as GameObject;
		lobby.SendMessage("OnClassClicked", mCarID+ tClass[2], SendMessageOptions.DontRequireReceiver);
		InitShowRoomInfo(int.Parse(mCarID), tClass[0]);
	}

	void OnClassL(){
//		if(classCnt == 1) return;
		classCnt--;
		if(classCnt == 1) btnClass[5].gameObject.SetActive(false);
		if(!btnClass[4].gameObject.activeSelf) btnClass[4].gameObject.SetActive(true);
		string[] tClass = ConvertToClass(classCnt);
		lbClass.text = tClass[0];
		spClass.spriteName = tClass[1];
		var lobby = GameObject.Find("LobbyUI") as GameObject;
		lobby.SendMessage("OnClassClicked", mCarID+tClass[2], SendMessageOptions.DontRequireReceiver);
		InitShowRoomInfo(int.Parse(mCarID), tClass[0]);
	}
	string mCarID;
	public int ChangeShowContents(){
		Utility.LogWarning(" 0 " + gameObject.name);
		string[] _id  = gameObject.name.Split("_"[0]);
		int a = 0;
		switch(_id[1]){
		case "sCar":SetShowCarInfo(_id[0]);a=1;
			break;
		case "ssCar":SetShowShopCarInfo(_id[0]); a=0;
			break;
		case "tCar":SetShowCarInfo(_id[0]); a=1;
			break;
		case "Dealer":SetDealerShopCarInfo(_id[0], _id[2]); a=2;
			break;
		default:
			SetShowCarInfo(_id[0]);
			break;
		}
		return a;
	}
	int classCnt = 1;
	public void SetShowCarInfo(string str){

		int id = int.Parse(str);
		classCnt = 1;
		mCarID = str;
		btnClass[3].gameObject.SetActive(true);
		btnClass[2].gameObject.SetActive(false);
		btnClass[1].gameObject.SetActive(false);		btnClass[0].gameObject.SetActive(false);
		btnClass[5].gameObject.SetActive(false);
		btnClass[4].gameObject.SetActive(true);
		Utility.LogWarning("SetShowCarInfo");
		string[] tClass = ConvertToClass(classCnt);
		lbClass.text = tClass[0];
		spClass.spriteName = tClass[1];
		lbprice.transform.gameObject.SetActive(false);
		InitShowRoomInfo(id,tClass[0]);

	}

	private void InitShowRoomInfo(int carid, string classid){
		Common_Car_Status.Item item = Common_Car_Status.Get(carid);
		//Common_Class.Item itemClass = Common_Class.Get (typeid);
		Common_Class.Item itemClass = GV.getClassTypeID(classid, item.Model);

		transform.FindChild("lbPower").GetComponent<UILabel>().text = GV.DefaultCarAbility(carid, itemClass).ToString();
		
		transform.FindChild("lbName").GetComponent<UILabel>().text = item.Name;
		//transform.FindChild("lbClass").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("76002"), classid);// "";
	//	transform.FindChild("lbStatus").GetComponent<UILabel>().text = 
	//		string.Format(KoStorage.GetKorString("74014"), 
	//		              classid,item.ReqLV,itemClass.UpLimit,itemClass.StarLV,item.GearLmt);
		transform.FindChild("lbReqLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74027"),item.ReqLV);
		transform.FindChild("lbStarLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74028"),itemClass.StarLV);
		transform.FindChild("lbGearLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74029"),item.GearLmt+itemClass.GearLmt);
		transform.FindChild("lbClass").GetComponent<UILabel>().text =
			string.Format(KoStorage.GetKorString("74024"), classid);
	
	}
	private void InitSpecialCarInfo(Common_Car_Status.Item item, Common_Class.Item item1){
		/*transform.FindChild("lbPower").GetComponent<UILabel>().text = GV.DefaultCarAbility(int.Parse(item.ID), item1).ToString();
		transform.FindChild("lbName").GetComponent<UILabel>().text = item.Name;
		transform.FindChild("lbReqLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74027"),item.ReqLV);
		transform.FindChild("lbStarLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74028"),item1.StarLV);
		transform.FindChild("lbGearLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74029"),item.GearLmt+item1.GearLmt);
		transform.FindChild("lbClass").GetComponent<UILabel>().text =
			string.Format(KoStorage.GetKorString("74024"), GV.gDealerClass);*/
		//	클래스 [009cff]{0}[-]
		//요구시즌 : [009cff]{1}[-]시즌
		//	강화 최고단계 : [ff3600]{2}[-]단계
		//	진화 최고단계 : [ff3600]{3}[-]단계
		//	최대기어 : [009cff]{4}[-]단
	
		transform.FindChild("lbPower").GetComponent<UILabel>().text = "???";
		transform.FindChild("lbName").GetComponent<UILabel>().text = item.Name;
		transform.FindChild("lbReqLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74027"),item.ReqLV);
		transform.FindChild("lbStarLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74028"),"???");
		transform.FindChild("lbGearLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74029"),"???");
		transform.FindChild("lbClass").GetComponent<UILabel>().text =
			string.Format(KoStorage.GetKorString("74024"), "???");
	}
	public void SetShowShopCarInfo(string str){
		Utility.LogWarning("SetShopShop");
		btnClass[3].gameObject.SetActive(false);
		int id = int.Parse(str);
		Common_Car_Status.Item item = Common_Car_Status.Get(id);
	
		int typeid = 3100;
		string str1 = "S";
		switch(str1){
		case "D" : typeid =typeid+ 1;break;
		case "C" : typeid =typeid+ 2;break;
		case "B" : typeid =typeid+ 3;break;
		case "A" : typeid =typeid+ 4;break;
		case "S" : typeid =typeid+ 5;break;
		case "SS" : typeid =typeid+ 6;break;
		}
		Common_Class.Item itemClass = Common_Class.Get (typeid);
		if(item.Res == 1){
			btnClass[2].gameObject.SetActive(true);
			btnClass[0].gameObject.SetActive(false);
		}else{
			btnClass[2].gameObject.SetActive(false);
			btnClass[0].gameObject.SetActive(true);
		}
		lbprice.transform.gameObject.SetActive(true);
		lbprice.text = (item.BuyPrice+itemClass.Dealer_CoinPlus).ToString();
		/*
		if(GV.gBuyDealerCar == 1){ // 구매 안함..
			if(item.Res == 1){
				btnClass[2].gameObject.SetActive(true);
				btnClass[0].gameObject.SetActive(false);
			}else{
				btnClass[2].gameObject.SetActive(false);
				btnClass[0].gameObject.SetActive(true);
			}
			lbprice.transform.gameObject.SetActive(true);
			lbprice.text = (item.BuyPrice+itemClass.Dealer_CoinPlus).ToString();
		}else{ // 구매함.
			btnClass[2].gameObject.SetActive(false);
			btnClass[0].gameObject.SetActive(false);
			btnClass[1].gameObject.SetActive(true);
			lbprice.transform.gameObject.SetActive(false);
		}*/

		InitSpecialCarInfo(item,itemClass);
	}
	public void SetDealerShopCarInfo(string str, string strClass){
		Utility.LogWarning("SetShopShop");
		btnClass[3].gameObject.SetActive(false);
		int id = int.Parse(str);
		Common_Car_Status.Item item = Common_Car_Status.Get(id);
		
		int typeid = 3100;
		//string str1 = "S";
		switch(strClass){
		case "D" : typeid =typeid+ 1;break;
		case "C" : typeid =typeid+ 2;break;
		case "B" : typeid =typeid+ 3;break;
		case "A" : typeid =typeid+ 4;break;
		case "S" : typeid =typeid+ 5;break;
		case "SS" : typeid =typeid+ 6;break;
		}
		Common_Class.Item itemClass = Common_Class.Get (typeid);

		if(item.Res == 1){
			btnClass[2].gameObject.SetActive(true);
			btnClass[0].gameObject.SetActive(false);
		}else{
			btnClass[2].gameObject.SetActive(false);
			btnClass[0].gameObject.SetActive(true);
		}
		lbprice.transform.gameObject.SetActive(true);
		lbprice.text =item.BuyPrice.ToString();

		/*


		if(GV.gBuyDealerCar == 1){ // 구매 안함..
			if(item.Res == 1){
				btnClass[2].gameObject.SetActive(true);
				btnClass[0].gameObject.SetActive(false);
			}else{
				btnClass[2].gameObject.SetActive(false);
				btnClass[0].gameObject.SetActive(true);
			}
			lbprice.transform.gameObject.SetActive(true);
			//lbprice.text = (item.BuyPrice+itemClass.Dealer_CoinPlus).ToString();
			lbprice.text = "50";
		}else{ // 구매함.
			btnClass[2].gameObject.SetActive(false);
			btnClass[0].gameObject.SetActive(false);
			btnClass[1].gameObject.SetActive(true);
			lbprice.transform.gameObject.SetActive(false);
		}*/
		InitShowRoomInfo(id, strClass);
		//InitSpecialCarInfo(item,itemClass);
	}
	void OnBuyCoinCar(){
		Utility.LogWarning("OnBuyCoinCar " + transform.name);
		string price = lbprice.text;
		CreaetPopUpWindow(price, transform.name, "Coin");
	}

	void OnBuyDollarCar(){
		Utility.LogWarning("OnBuyDollarCar " + transform.name);
		string price = lbprice.text;
		CreaetPopUpWindow(price, transform.name, "Dollar");
	}

	private void CreaetPopUpWindow(string price, string name, string resType){
//		if(Global.isPopUp) return;
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		string[] tName = name.Split('_');
		pop.name = tName[0]+"_Car_"+resType+"_"+tName[2];
		var popchild =  pop.transform.FindChild("Content_BUY").gameObject as GameObject;
		popchild.transform.FindChild("lbPrice").GetComponent<UILabel>().text = price.ToString();
	//	var _popupaction = pop.GetComponent<popupinteraction>() as popupinteraction;
	//	if(_popupaction != null)  DestroyImmediate(_popupaction);// _popupaction = pop.AddComponent<popupinteraction>();
		var buy = pop.GetComponent<BuyDealerCar>() as BuyDealerCar;
		if(buy == null)  buy = pop.AddComponent<BuyDealerCar>();
		buy.CalledBuyCompete(ResultBuyAction, popchild);
		buy.InitWindow();
//		Global.isPopUp = true;
		popchild = pop = null;	
		buy = null;
	}

	void ResultBuyAction(bool isSuccess, string carClass){
		if(!isSuccess) return;
		btnClass[2].gameObject.SetActive(false);
		btnClass[1].gameObject.SetActive(true);
		btnClass[0].gameObject.SetActive(false);
		lbprice.transform.gameObject.SetActive(false);

	}
}
