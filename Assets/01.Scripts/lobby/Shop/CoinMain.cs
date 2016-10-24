using UnityEngine;
using System.Collections;

public class CoinMain : MonoBehaviour {
	public UILabel[] labels;
	public GameObject GoodObj, PakageObj;
	public Transform GoodTr, PakageTr;
	public GameObject pkgSlot;
	// Use this for initialization
	void Start () {
		GoodObj.transform.parent.gameObject.SetActive(true);
		PakageObj.transform.parent.gameObject.SetActive(true);
		labels[0].text  = KoStorage.GetKorString("72616");// 상품 
		labels[1].text  = KoStorage.GetKorString("72617");// package 

		if(GV.gInfo.extra04 == 2 || GV.gInfo.extra04 == 3){
			PakageObj.transform.parent.FindChild("img_event").gameObject.SetActive(true);
		}else{
			PakageObj.transform.parent.FindChild("img_event").gameObject.SetActive(false);
		}
		int tempid = Base64Manager.instance.GlobalEncoding(Global.gSale);
		if(tempid == 0){
			GoodObj.transform.parent.FindChild("img_event").gameObject.SetActive(false);
		}else{
			GoodObj.transform.parent.FindChild("img_event").gameObject.SetActive(true);
		}
	}

	void OnEnable(){
		GoogleAnalyticsV4.instance.LogScreen(new AppViewHitBuilder().SetScreenName("CoinShop"));
		OnCoinClick();
		if(!Global.bLobbyBack) return;
		UserDataManager.instance.OnSubBack= ()=>{
			OnClosed();

		};
	}
	void OnDisable(){
		//GoodObj.gameObject.SetActive (false);
		GoogleAnalyticsV4.instance.LogScreen(new AppViewHitBuilder().SetScreenName("Lobby"));
	}
	void OnCoinClick(){
		if(GoodObj.gameObject.activeSelf) return;
		GoodObj.gameObject.SetActive(true);
		PakageObj.gameObject.SetActive(false);
		GoodTr.gameObject.SetActive(true);
		PakageTr.gameObject.SetActive(false);
	}
	
	void OnPackageClick(){
		if(PakageObj.gameObject.activeSelf) return;
		GoodObj.gameObject.SetActive(false);
		PakageObj.gameObject.SetActive(true);
		
		GoodTr.gameObject.SetActive(false);
		PakageTr.gameObject.SetActive(true);
		addPackageSlot();
	}
	
	void addPackageSlot(){

		PakageTr.GetComponent<packageItem>().setInit();

		return;
		int cnt  = PakageTr.GetChild(0).childCount;
		if(cnt == 0){
			int idx = 0;
			var tr = PakageTr.GetChild(0).gameObject as GameObject;
			for(int i = 0; i < 6; i++){
				idx = 8520+i;
				var temp = NGUITools.AddChild( tr, pkgSlot);
				temp.name = idx.ToString();
				SetPackageSlot(temp, idx);
			}
			tr.GetComponent<UIGrid>().Reposition();
			
		}else{
			
		}
	}
	
	void SetPackageSlot(GameObject obj, int idx){
		Common_Cost.PackageItem item = Common_Cost.PkgGet(idx);
		Common_Class.Item classitem = Common_Class.Get(item.Car_Class);
		var tr = obj.transform.GetChild(0) as Transform;
		tr.FindChild("lbName").GetComponent<UILabel>().text = item.Name;
		var mTr = tr.FindChild("btnCash") as Transform;
		
		mTr.GetComponent<UIButtonMessage>().target = this.gameObject;
		mTr.GetComponent<UIButtonMessage>().functionName = "OnBuyPackage";
		int chID  = GV.ChSeason +8519;
		if(chID < idx){
			mTr.gameObject.SetActive(false);
		}
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
	//	case "TUR":
	//		strUnit  = KoStorage.GetKorString("72024");nPrice = item.Cash_e;break;
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
		case "IDN":
			strUnit  = KoStorage.GetKorString("72023");nPrice = item.Cash_d;break;
		case "MYS":
			strUnit  = KoStorage.GetKorString("72023");nPrice = item.Cash_d;break;
		default: 
			strUnit  = KoStorage.GetKorString("72023");nPrice = item.Cash_d;break;
			
		}
		mTr.FindChild("lbPrice").GetComponent<UILabel>().text =string.Format("{0:#,0}", nPrice);
		mTr.FindChild("lbUnit").GetComponent<UILabel>().text =strUnit;
		
		tr = tr.FindChild("Package_1");
		mTr = tr.FindChild("Car_info") as Transform;
		mTr.FindChild("lbCar_Car").GetComponent<UILabel>().text = item.Text;
		mTr.FindChild("lbCar_Class").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("72618"), classitem.Class);
		mTr.FindChild("lbCar_Evolusion").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("72619"), classitem.StarLV);
		mTr.FindChild("lbCar_Upgrade").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("72620"), classitem.StarLV*classitem.UpLimit);
		
		
		
		mTr = tr.FindChild("Goods_info_1");
		if(item.Coupon_type == 1){
			mTr.FindChild("img_Coupon_Siver").gameObject.SetActive(true);
			mTr.FindChild("img_Coupon_Gold").gameObject.SetActive(false);
		}
		else if(item.Coupon_type == 2){
			mTr.FindChild("img_Coupon_Gold").gameObject.SetActive(true);
			mTr.FindChild("img_Coupon_Siver").gameObject.SetActive(false);
		}
		mTr.FindChild("lbQuantity").GetComponent<UILabel>().text =string.Format("x {0}", item.Recharge_Coupon);//.ToString();
		mTr = tr.FindChild("Goods_info_2");
		mTr.FindChild("lbQuantity").GetComponent<UILabel>().text =string.Format("x {0}", item.Recharge_Cube); 
		
		
	}
	
	
	void OnBuyPackage(GameObject obj){
		if(Global.isNetwork)return;
		string str = obj.transform.parent.parent.name;
		Utility.LogWarning(str);
		
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		var mAction = pop.GetComponent<BuyPackage>() as BuyPackage;
		if(mAction == null) mAction = pop.AddComponent<BuyPackage>();
		var tr = transform.FindChild("VIP_info") as Transform;//GetComponent<VIPPoint>().ChangeVIPLevel();
		mAction.SetBuyPopUp(str, tr);
	}

	void OnClosed(){
		GetComponent<TweenAction>().ReverseTween(gameObject);
		Global.isPopUp = false;
		Global.bLobbyBack = false;
	}
	
	public void OnBuyPackage(int carId, int packID){
		if(Global.isNetwork)return;
	//	Global.isNetwork = true;
	//	Utility.Log("onbuyPackage");
	//	return;
		string str = carId.ToString()+"_"+ packID.ToString();//"1000_8526";	
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		var mAction = pop.GetComponent<BuyPackage>() as BuyPackage;
		if(mAction == null) mAction = pop.AddComponent<BuyPackage>();
		var tr = transform.FindChild("VIP_info") as Transform;//GetComponent<VIPPoint>().ChangeVIPLevel();
		mAction.SetBuyPopUp(str, tr);
	}
	
}

