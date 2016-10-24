using UnityEngine;
using System.Collections;

public class packageItem : MonoBehaviour {


	public Transform[] btns;
	public UILabel[] lbtexts;
	private int packageID = 0, carIndex = 0, classIndex = 0;

	void Start(){
		var mTr =btns[0] as Transform;
		mTr.GetComponent<UIButtonMessage>().target = this.gameObject;
		mTr.GetComponent<UIButtonMessage>().functionName = "OnBuyPackage";
		btns[1].FindChild("Car_info").FindChild("lbCar_Request").GetComponent<UILabel>().text = string.Empty;
		btns[1].FindChild("Car_info").FindChild("lbCar_Request").gameObject.SetActive(true);
	}
	private bool bEnable = false;
	public void setInit(){


		if(GV.gInfo.extra04 == 2 || GV.gInfo.extra04 == 3){
			btns[0].gameObject.SetActive(true);
			transform.GetChild(0).FindChild("lbEvent").gameObject.SetActive(false);
			bEnable = true;
		}else{
			btns[0].gameObject.SetActive(false);
			transform.GetChild(0).FindChild("lbEvent").gameObject.SetActive(true);
			transform.GetChild(0).FindChild("lbEvent").GetComponent<UILabel>().text =KoStorage.GetKorString("73324");
			bEnable = false;
		}

		packageID = 8526;
		carIndex = 1000;
		classIndex = 0;

		btns[1].FindChild("Car_info").FindChild("Class_S").gameObject.SetActive(true);
		btns[1].FindChild("Car_info").FindChild("Class_SS").gameObject.SetActive(false);
		btns[3].gameObject.SetActive(true);
		btns[2].gameObject.SetActive(false);

		btns[5].gameObject.SetActive(true);
		btns[4].gameObject.SetActive(false);
		setBuyCashLabel(packageID);
		classIndex = 8526;


		btns[1].FindChild("Car_info").FindChild("img_Car").gameObject.SetActive(true);
		btns[1].FindChild("Car_info").FindChild("img_Lock").gameObject.SetActive(false);
		btns[1].FindChild("Car_info").FindChild("img_Car").GetComponent<UISprite>().spriteName = carIndex.ToString();
		lbtexts[1].transform.gameObject.SetActive(true);// = string.Format(KoStorage.GetKorString("72619"), item.Evo_LV); // evo
		lbtexts[2].transform.gameObject.SetActive(true);
		btns[1].FindChild("Car_info").FindChild("lbCar_Request").GetComponent<UILabel>().text = string.Empty;
	
	}

	void setBuyCashLabel(int idx){
		Common_Cost.PackageItem item = Common_Cost.PkgGet(idx);
		var mTr =btns[0] as Transform;
		lbtexts[3].text = string.Format("x {0}", item.Recharge_Coupon);
		lbtexts[4].text = string.Format("x {0}", item.Recharge_Cube);
		string nPrice = string.Empty;
		string strUnit = string.Empty;
		switch(GV.gNationName){
		case "KOR":
			strUnit  = KoStorage.GetKorString("72007");nPrice = item.Cash_won;break;
		case "JPN":
			strUnit  = KoStorage.GetKorString("72026");nPrice = item.Cash_y;break;
		case "CHN":
			strUnit  = KoStorage.GetKorString("72025");nPrice = item.Cash_w;break;
		case "FRA":
			strUnit  = KoStorage.GetKorString("72024");nPrice = item.Cash_e;break;
		case "RUS":
			strUnit  = KoStorage.GetKorString("72027");nPrice = item.Cash_r;break;
		case "USA":
			strUnit  = KoStorage.GetKorString("72023");nPrice = item.Cash_d;break;
		case "GBR":
			strUnit  = KoStorage.GetKorString("72023");nPrice = item.Cash_d;break;
		case "DEU":
			strUnit  = KoStorage.GetKorString("72024");nPrice = item.Cash_e;break;
		case "ITA":
			strUnit  = KoStorage.GetKorString("72024");nPrice = item.Cash_e;break;
		case "ESP":
			strUnit  = KoStorage.GetKorString("72024");nPrice = item.Cash_e;break;
		case "PRT":
			strUnit  = KoStorage.GetKorString("72024");nPrice = item.Cash_e;break;
		default: 
			strUnit  = KoStorage.GetKorString("72023");nPrice = item.Cash_d;break;
			
		}
		mTr.FindChild("lbPrice").GetComponent<UILabel>().text =string.Format("{0:#,0}", nPrice);
		mTr.FindChild("lbUnit").GetComponent<UILabel>().text =strUnit;

		if(idx == 8526){
			lbtexts[0].text = string.Format(KoStorage.GetKorString("72618"), "S"); //class
		}else{
			lbtexts[0].text = string.Format(KoStorage.GetKorString("72618"), "SS"); //class
		}

		lbtexts[1].text = string.Format(KoStorage.GetKorString("72619"), item.Evo_LV); // evo
		lbtexts[2].text = string.Format(KoStorage.GetKorString("72620"), item.Up_LV); //upgrade


	}
	void OnClassL(){
	//	if(classIndex == 0) return;
	//	classIndex = 1;
		btns[4].gameObject.SetActive(false);
		btns[5].gameObject.SetActive(true);
		btns[1].FindChild("Car_info").FindChild("Class_S").gameObject.SetActive(true);
		btns[1].FindChild("Car_info").FindChild("Class_SS").gameObject.SetActive(false);
		setBuyCashLabel(8526);
		classIndex = 8526;
		
	}

	void OnClassR(){
	//	if(classIndex == 1) return;
	//	classIndex = 0;
		btns[4].gameObject.SetActive(true);
		btns[5].gameObject.SetActive(false);
		btns[1].FindChild("Car_info").FindChild("Class_S").gameObject.SetActive(false);
		btns[1].FindChild("Car_info").FindChild("Class_SS").gameObject.SetActive(true);
		setBuyCashLabel(8527);
		classIndex = 8527;
	}

	void OnCarL(){
		carIndex--;
		if(carIndex <= 1000){
			carIndex  = 1000;
			//btns[2].gameObject.SetActive(true);
			btns[2].gameObject.SetActive(false);

			Common_Car_Status.Item item = Common_Car_Status.Get(carIndex);
			if(item.ReqLV > GV.ChSeason){
				if(bEnable)  btns[0].gameObject.SetActive(false);
				btns[1].FindChild("Car_info").FindChild("img_Lock").gameObject.SetActive(true);
				btns[1].FindChild("Car_info").FindChild("img_Car").GetComponent<UISprite>().spriteName = carIndex.ToString();

				lbtexts[1].transform.gameObject.SetActive(false);// = string.Format(KoStorage.GetKorString("72619"), item.Evo_LV); // evo
				lbtexts[2].transform.gameObject.SetActive(false);
				btns[1].FindChild("Car_info").FindChild("lbCar_Request").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("72603"),item.ReqLV);
			}else{
				btns[1].FindChild("Car_info").FindChild("img_Lock").gameObject.SetActive(false);
				btns[1].FindChild("Car_info").FindChild("img_Car").GetComponent<UISprite>().spriteName = carIndex.ToString();
				if(bEnable)  btns[0].gameObject.SetActive(true);
				lbtexts[1].transform.gameObject.SetActive(true);// = string.Format(KoStorage.GetKorString("72619"), item.Evo_LV); // evo
				lbtexts[2].transform.gameObject.SetActive(true);
				btns[1].FindChild("Car_info").FindChild("lbCar_Request").GetComponent<UILabel>().text = string.Empty;
			}

			return;
		}
		//carIndex--;
		if(!btns[3].gameObject.activeSelf) btns[3].gameObject.SetActive(true);
	//	btns[2].gameObject.SetActive(true);
	//	btns[3].gameObject.SetActive(false);
		Common_Car_Status.Item item1 = Common_Car_Status.Get(carIndex);
		if(item1.ReqLV > GV.ChSeason){
		//	btns[1].FindChild("Car_info").FindChild("img_Car").gameObject.SetActive(false);
			btns[1].FindChild("Car_info").FindChild("img_Lock").gameObject.SetActive(true);
			btns[1].FindChild("Car_info").FindChild("img_Car").GetComponent<UISprite>().spriteName = carIndex.ToString();
			if(bEnable)  btns[0].gameObject.SetActive(false);
			lbtexts[1].transform.gameObject.SetActive(false);
			lbtexts[2].transform.gameObject.SetActive(false);
			btns[1].FindChild("Car_info").FindChild("lbCar_Request").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("72603"),item1.ReqLV);
		}else{
		//	btns[1].FindChild("Car_info").FindChild("img_Car").gameObject.SetActive(true);
			btns[1].FindChild("Car_info").FindChild("img_Lock").gameObject.SetActive(false);
			btns[1].FindChild("Car_info").FindChild("img_Car").GetComponent<UISprite>().spriteName = carIndex.ToString();
			if(bEnable)  btns[0].gameObject.SetActive(true);
			lbtexts[1].transform.gameObject.SetActive(true);// = string.Format(KoStorage.GetKorString("72619"), item.Evo_LV); // evo
			lbtexts[2].transform.gameObject.SetActive(true);
			btns[1].FindChild("Car_info").FindChild("lbCar_Request").GetComponent<UILabel>().text = string.Empty;
		}
		//
		//btns[1].FindChild("Car_info").FindChild("img_Car").GetComponent<UISprite>().spriteName = carIndex.ToString();
	}

	void OnCarR(){
		carIndex++;
		if(carIndex >= 1023) {
			carIndex = 1023;
			btns[3].gameObject.SetActive(false);
			//btns[3].gameObject.SetActive(true);
			Common_Car_Status.Item item = Common_Car_Status.Get(carIndex);
			if(item.ReqLV > GV.ChSeason){
			//	btns[1].FindChild("Car_info").FindChild("img_Car").gameObject.SetActive(false);
				btns[1].FindChild("Car_info").FindChild("img_Lock").gameObject.SetActive(true);
				btns[1].FindChild("Car_info").FindChild("img_Car").GetComponent<UISprite>().spriteName = carIndex.ToString();
				if(bEnable)  btns[0].gameObject.SetActive(false);
				lbtexts[1].transform.gameObject.SetActive(false);
				lbtexts[2].transform.gameObject.SetActive(false);
				btns[1].FindChild("Car_info").FindChild("lbCar_Request").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("72603"),item.ReqLV);
			}else{
			//	btns[1].FindChild("Car_info").FindChild("img_Car").gameObject.SetActive(true);
				btns[1].FindChild("Car_info").FindChild("img_Lock").gameObject.SetActive(false);
				btns[1].FindChild("Car_info").FindChild("img_Car").GetComponent<UISprite>().spriteName = carIndex.ToString();
				if(bEnable)  btns[0].gameObject.SetActive(true);
				lbtexts[1].transform.gameObject.SetActive(true);// = string.Format(KoStorage.GetKorString("72619"), item.Evo_LV); // evo
				lbtexts[2].transform.gameObject.SetActive(true);
				btns[1].FindChild("Car_info").FindChild("lbCar_Request").GetComponent<UILabel>().text = string.Empty;
			}
			return;
		}
	
		if(!btns[2].gameObject.activeSelf) btns[2].gameObject.SetActive(true);
	//	btns[2].gameObject.SetActive(false);
	//	btns[3].gameObject.SetActive(true);
		Common_Car_Status.Item item1 = Common_Car_Status.Get(carIndex);
		if(item1.ReqLV > GV.ChSeason){
			//btns[1].FindChild("Car_info").FindChild("img_Car").gameObject.SetActive(false);
			btns[1].FindChild("Car_info").FindChild("img_Lock").gameObject.SetActive(true);
			btns[1].FindChild("Car_info").FindChild("img_Car").GetComponent<UISprite>().spriteName = carIndex.ToString();
			if(bEnable)  btns[0].gameObject.SetActive(false);
			lbtexts[1].transform.gameObject.SetActive(false);
			lbtexts[2].transform.gameObject.SetActive(false);
			btns[1].FindChild("Car_info").FindChild("lbCar_Request").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("72603"),item1.ReqLV);
		}else{
			//btns[1].FindChild("Car_info").FindChild("img_Car").gameObject.SetActive(true);
			btns[1].FindChild("Car_info").FindChild("img_Lock").gameObject.SetActive(false);
			btns[1].FindChild("Car_info").FindChild("img_Car").GetComponent<UISprite>().spriteName = carIndex.ToString();
			if(bEnable)  btns[0].gameObject.SetActive(true);
			lbtexts[1].transform.gameObject.SetActive(true);// = string.Format(KoStorage.GetKorString("72619"), item.Evo_LV); // evo
			lbtexts[2].transform.gameObject.SetActive(true);
			btns[1].FindChild("Car_info").FindChild("lbCar_Request").GetComponent<UILabel>().text = string.Empty;
		}
		//btns[1].FindChild("Car_info").FindChild("img_Car").GetComponent<UISprite>().spriteName = carIndex.ToString();
	}

	void OnBuyPackage(){
		btns[0].gameObject.SetActive(false);
		Invoke("reSetCashButton",1.0f);
		NGUITools.FindInParents<CoinMain>(gameObject).OnBuyPackage(carIndex, classIndex);
	}
	void reSetCashButton(){
		btns[0].gameObject.SetActive(true);
	}
}
