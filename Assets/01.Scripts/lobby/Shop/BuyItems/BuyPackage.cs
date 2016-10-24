using UnityEngine;
using System.Collections;

public class BuyPackage : BuyItem {
	private int packageCarId = 0;
	public void SetBuyPackagePopUp(string BuyName){
		string[] _name = BuyName.Split('_');
		this.BuyID = int.Parse(_name[1]);
		this.packageCarId = int.Parse(_name[0]);
		Common_Cost.PackageItem _item = Common_Cost.PkgGet(BuyID);
		
		string strBuyPrice = string.Empty;
		switch(GV.gNationName){
		case "KOR":
			strBuyPrice= (_item.Cash_won);break;
		case "JPN":
			strBuyPrice= (_item.Cash_y);break;
		case "CHN":
			strBuyPrice =(_item.Cash_w);break;
		case "FRA":
			strBuyPrice =( _item.Cash_e);break;
	//	case "TUR":
	//		strBuyPrice =( _item.Cash_e);break;
		case "RUS":
			strBuyPrice =(_item.Cash_r);break;
		case "USA":
			strBuyPrice =( _item.Cash_d);break;
		case "GBR":
			strBuyPrice =(_item.Cash_d);break;
		case "DEU":
			strBuyPrice =(_item.Cash_e);break;
		case "ITA":
			strBuyPrice= (_item.Cash_e);break;
		case "ESP":
			strBuyPrice =(_item.Cash_e);break;
		case "PRT":
			strBuyPrice= (_item.Cash_e);break;
		case "IDN":
			strBuyPrice= (_item.Cash_d);break;
		case "MYS":
			strBuyPrice= (_item.Cash_d);break;
		default:
			strBuyPrice= (_item.Cash_d);	break;
		}
		
		this.BuyPrice = (int)float.Parse(strBuyPrice);
		
		var Pop = transform.FindChild("Content_BUY").gameObject as GameObject;
		Pop.SetActive(true);
		var temp = Pop.transform.FindChild("icon_product") as Transform;
		temp.gameObject.SetActive(true);
		temp.GetComponent<UISprite>().spriteName = (BuyID).ToString();
		temp.GetComponent<UISprite>().MakePixelPerfect();
		
		Pop.transform.FindChild("lbName1").gameObject.SetActive(true);
		Pop.transform.FindChild("lbName1").GetComponent<UILabel>().text =  string.Format( KoStorage.GetKorString("71011"), _item.Name);
		Pop.transform.FindChild("lbName").gameObject.SetActive(false);
		Pop.transform.FindChild("lbText").GetComponent<UILabel>().text = string.Format( KoStorage.GetKorString("71010"),  _item.Name);;
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnFailClick",0.1f);
			return true;
		};
		UserDataManager.instance.OnSubBacksub = ()=>{
			OnCloseClick();
		};
		Pop = transform.FindChild("Content_BUY").gameObject as GameObject;
		Pop.transform.FindChild("btnCash").gameObject.SetActive(true);
		Pop.transform.FindChild("btnCash").FindChild("lbOk").GetComponent<UILabel>().text
			= KoStorage.GetKorString("71013");
		Pop.transform.FindChild("lbPrice").GetComponent<UILabel>().text = string.Empty;
		var go = Pop.GetComponent<GoogleInAppProc>() as GoogleInAppProc;
		if(go == null) Pop.AddComponent<GoogleInAppProc>();
		OnBuyClick =()=>{
			Global.isNetwork = true;
			Utility.LogWarning("OnCash Clicked ");
			StartCoroutine("BuyCashCount_Delay");
		};


	}

	public override void SetBuyPopUp(string BuyName, Transform tr){
		SetBuyPackagePopUp(BuyName);
		this.vipTr = tr;
		
	}


	IEnumerator processPay(int buyID){
		var pop = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
		pop.StartCoroutine("buyLoading");
		string buyItemId = string.Empty;
		switch(buyID){
	//	case 8520:
	//		buyItemId = "c8520"; //8514 100 120
	//		break;
	//	case 8521 : 
	//		buyItemId = "c8521"; // 8515 210 252
	//		break;
	//	case 8522:
	//		buyItemId = "c8522"; // 8516  441 529
	//		break;
	//	case 8523:
	//		buyItemId = "c8523"; //8517 1157 1389
	//		break;
	//	case 8524:
	//		buyItemId = "c8524"; //8518 2431 2917
	//		break;
	//	case 8525:
	//		buyItemId = "c8525"; //8518 2431 2917
	//		break;
		case 8526:
			buyItemId = "c8526"; //8518 2431 2917
			break;
		case 8527:
			buyItemId = "c8527"; //8518 2431 2917
			break;
		}
		bool bWait = true;
		int mFail =  0;
		#if UNITY_ANDROID && !UNITY_EDITOR
		UserDataManager.instance.isPause = true;
		bWait = true;
		var go =  transform.FindChild("Content_BUY").GetComponent<GoogleInAppProc>() as GoogleInAppProc;
		go.QueryInventory(buyItemId, ()=>{
			mFail = 0;
			bWait = false;
		},()=>{
			mFail = 1;
			bWait = false;
		});
		while(bWait){
			yield return null;
		}
		bWait = true;


		if(mFail == 0){
			if(string.IsNullOrEmpty(Global.orderID)){
				mFail = 1;
				
			}else{
				if(Global.orderID.Contains("GPA.") == false){
					mFail = 1;
				}else{
					if(Global.orderID.Length < 26  && Global.orderID.Length >22){
						if( string.Equals(Global.packageName,"com.gabangmanstudio.pitstopracing") == true){
							if(Global.productId.Contains(buyItemId) == true){
								
							}else{
								mFail = 1;
							}
						}else{
							mFail = 1;
						}
					}else{
						mFail = 1;
					}
				}
			}
			bWait = false;
		}else{
			bWait = false;	
		}// mFail if
		while(bWait){
			yield return null;
		}
		//GV.signature = string.Empty;
		//GV.signedData = string.Empty;
		//GV.pKey = string.Empty;
		UserDataManager.instance.isPause = false;
		#else
		mFail = 0;
		#endif
		bWait = true;
		if(mFail == 1){
			OnBuyCashFailed();
			bWait = false;
		}else{

			Common_Cost.PackageItem its = Common_Cost.PkgGet(buyID);
			NetworkManager.instance.BuyPackageItemConnectAndroid(this.packageCarId, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					int[] mPack = new int[5];
					GV.UpdateMatCount(8620, its.Recharge_Cube);
					myAcc.instance.account.bInvenBTN[2] = true;
					GV.UpdateCouponList((its.Coupon_type-1), its.Recharge_Coupon);
					myAcc.instance.account.bInvenBTN[3] = true;
					mPack[1] = its.Recharge_Coupon;
					mPack[2] = its.Recharge_Cube;
					mPack[0] = its.Coupon_type;

					int i = thing["car"].Count-1;
					int nId =  thing["car"][i]["carId"].AsInt;
					CarInfo carItem = new CarInfo(nId);
					carItem.CarIndex = thing["car"][i]["idx"].AsInt;
					carItem.CarID =nId;
					carItem.nClassID = thing["car"][i]["carClass"].AsInt;
					mPack[3] = carItem.CarID;
					mPack[4] = carItem.nClassID;
					carItem.bodyLv = thing["car"][i]["partLevel"]["1"].AsInt;
					carItem.engineLv =  thing["car"][i]["partLevel"]["2"].AsInt;
					carItem.tireLv = thing["car"][i]["partLevel"]["3"].AsInt;
					carItem.gearBoxLv = thing["car"][i]["partLevel"]["4"].AsInt;
					carItem.intakeLv =  thing["car"][i]["partLevel"]["5"].AsInt;
					carItem.bsPowerLv = thing["car"][i]["partLevel"]["6"].AsInt;
					carItem.bsTimeLv =  thing["car"][i]["partLevel"]["7"].AsInt;
					
					carItem.bodyStar = thing["car"][i]["partStar"]["1"].AsInt - 1;
					carItem.engineStar =  thing["car"][i]["partStar"]["2"].AsInt-1;
					carItem.tireStar = thing["car"][i]["partStar"]["3"].AsInt-1;
					carItem.gearBoxStar = thing["car"][i]["partStar"]["4"].AsInt-1;
					carItem.intakeStar =  thing["car"][i]["partStar"]["5"].AsInt-1;
					carItem.bsPowerStar = thing["car"][i]["partStar"]["6"].AsInt-1;
					carItem.bsTimeStar =  thing["car"][i]["partStar"]["7"].AsInt-1;
					
					carItem.durability = thing["car"][i]["durability"].AsInt;
					carItem.SetFlag = 1;
					string str = GV.ChangeCarClassIDString(carItem.nClassID);
					carItem.ClassID = str;
					Common_Class.Item item1	 = GV.getClassTypeID( str, 1);
					carItem.CarClassItem = item1;
					
					int duRef = Common_Car_Status.Get(carItem.CarID).Durability +item1.Durability ;
					carItem.carClass.SetClass1(item1.UpLimit, item1.StarLV, carItem.durability, item1.Repair, item1.Brake,  item1.Class_power);
					carItem.carClass.SetClass2(item1.Class_weight,item1.Class_grip,  item1.Class_gear, 
					                           item1.Class_bspower, item1.Class_bstime, item1.Repair,  duRef );
					carItem.carClass.SetClass3(item1.GearLmt);
					carItem.bNewBuyCar = true;
					GV.mineCarList.Add(carItem);
					ResponsePackageFinish(mPack,(bSucc)=>{
						if(!bSucc) return;
						GV.vipExp += its.Cash_Exp;
						ChangeVIPLevel();
						var lobby = GameObject.Find("LobbyUI") as GameObject;
						lobby.SendMessage("PlusBuyedItem",5, SendMessageOptions.DontRequireReceiver);
						GameObject.Find("Audio").SendMessage("CompleteSound"); 
						GoogleAnalyticsV4.instance.LogEvent("buyCashItem", "package", buyID.ToString(),0);
					});
				}else if(status == -2000){
					OnBuyCashFailed();
				}else{
					AccountManager.instance.ErrorPopUp();
				}
				bWait = false;
				pop.isBuyWait = false;
				pop.BuyLoadingStopCoroutine("buyLoading");
				Destroy(pop);
				pop = null;
			}, GV.signedData, GV.signature);
		}
		
		while(bWait){
			yield return null;
		}
		if(pop != null){
			pop.isBuyWait = false;
			pop.BuyLoadingStopCoroutine("buyLoading");
			Destroy(pop);
			pop = null;
		}
	}

void ResponsePackageFinish(int[] mPack,System.Action<bool> callback){
				
		Global.isNetwork = false;
		callback(true);
		
		gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(false);
		var mTr = transform.FindChild("BuyPackage") as Transform;
		mTr.gameObject.SetActive(true);
		mTr = mTr.GetChild(0);
		int carId = mPack[3];
		string strClass = "D";
		switch(mPack[4]){
		case 3101: strClass = "D";break;
		case 3102: strClass = "B";break;
		case 3103: strClass = "C";break;
		case 3104: strClass = "A";break;
		case 3105: strClass = "S";break;
		case 3106: strClass = "SS";break;

		}
		TweenPosition[] tw = mTr.GetComponentsInChildren<TweenPosition>() as TweenPosition[];
		for(int i = 0; i < tw.Length ; i++){
			tw[i].Reset();
			tw[i].enabled = true;
		}

		Common_Car_Status.Item carItem = Common_Car_Status.Get(carId);
		Common_Class.Item classItem = GV.getClassTypeID(strClass, carItem.Model);
		var tr = mTr.transform.FindChild("Info_status") as Transform;
		tr.FindChild("name").GetComponent<UILabel>().text = carItem.Name;
		
		tr.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("74025");
		tr.FindChild("Class").FindChild("C_Text").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74024"), strClass);
		tr.FindChild("Class").FindChild("C_Icon").GetComponent<UISprite>().spriteName = "Class_"+strClass;
		tr.FindChild("Icon").GetComponent<UISprite>().spriteName = carId.ToString();
		tr.FindChild("lbStarLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74028"),classItem.StarLV);
		tr.FindChild("lbReqLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74027"),carItem.ReqLV);
		tr.FindChild("lbGearLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74029"), carItem.GearLmt+classItem.GearLmt);
		mTr.FindChild("Btn_ok").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("71000");
		if(mPack[0] == 1)tr.FindChild("Icon_Coupon").GetComponent<UISprite>().spriteName = "8513";
		else tr.FindChild("Icon_Coupon").GetComponent<UISprite>().spriteName = "8514";
		
		tr.FindChild("Coupon_N").GetComponent<UILabel>().text = string.Format("X {0}", mPack[1]);
		tr.FindChild("Cube_N").GetComponent<UILabel>().text = string.Format("X {0}", mPack[2]);
		GameObject.Find("Audio").SendMessage("CompleteSound");
	
	
	}

	void 	OnBuyCashFailed(){
		gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(false);
		var _parent = gameObject.transform.FindChild("Content_Fail").gameObject;
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
		Global.isNetwork = false;
	}
	
	private void BuyCashCount(){
	//	Utility.LogWarning(BuyID);
	//	Utility.LogWarning(this.packageCarId);
	//	Global.isNetwork = false;
	//	return;
		StartCoroutine("processPay",BuyID);
	}
	
	
	IEnumerator BuyCashCount_Delay(){
		yield return new WaitForSeconds(0.1f);
		BuyCashCount();
		
	}

}
