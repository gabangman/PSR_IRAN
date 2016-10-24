using UnityEngine;
using System.Collections;

public class BuyItem : MonoBehaviour {

	protected int BuyID, BuyPrice, BuyCount;
	protected System.Action OnBuyClick;
	protected Transform vipTr;
	protected bool bSubCheck=false;
	public virtual void SetBuyPopUp(string BuyName){

		string[] _name = BuyName.Split("_"[0]);
		this.BuyID = int.Parse(_name[0]);
		int mItemId = BuyID;
		Common_Cost.Item _item = null;
		if(BuyID >= 8502 && BuyID <=8506){
			int tempid = Base64Manager.instance.GlobalEncoding(Global.gSale);
			mItemId = BuyID+tempid;
			_item = Common_Cost.Get(mItemId);
			if(tempid == 0){
				this.BuyCount = _item.Recharge_no;
			}else {
				float a = (float)_item.Recharge_no * GV.gInfo.plusEventRatio;
				this.BuyCount = (int)a;
			}
		}else{
			_item = Common_Cost.Get(mItemId);
			this.BuyCount = _item.Recharge_no;
		}
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
		case "PRT":
			strBuyPrice= (_item.Cash_e);break;
		case "ESP":
			strBuyPrice= (_item.Cash_e);break;
		case "IDN":
			strBuyPrice= (_item.Cash_d);break;
		case "MYS":
			strBuyPrice= (_item.Cash_d);break;
		default:
			strBuyPrice= (_item.Cash_d);	break;
		}
		if(BuyID >= 8502 && BuyID <=8506){
			this.BuyPrice = (int)float.Parse(strBuyPrice);
		}else{
			this.BuyPrice = int.Parse(strBuyPrice);
		}
		
		
		
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
		Pop.transform.FindChild("lbPrice").GetComponent<UILabel>().text = string.Format("{0:#,0}", strBuyPrice);
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnFailClick",0.1f);
			return true;
		};

		UserDataManager.instance.OnSubBacksub = ()=>{
			OnCloseClick();
		};
	}
	public virtual void SetBuyPopUp(string BuyName,int idx){
			SetBuyPopUp(BuyName);
	}

	public virtual void SetBuyPopUp(string BuyName, Transform tr){
		SetBuyPopUp(BuyName);
		this.vipTr = tr;

	}

	public void ChangeVIPLevel(){
		if(vipTr == null){
			var tr  = transform.parent.FindChild("CoinShop") as Transform;
			vipTr = tr.transform.FindChild("VIP_info") as Transform;
		}
		vipTr.GetComponent<VIPPoint>().ChangeVIPLevel();
	}


	public void OnOkClick(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		if(OnBuyClick != null)
			OnBuyClick();
		OnBuyClick = null;
	}






	protected void ResponseFinish(bool bSuccess, System.Action<bool> callback){
		gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(false);
		Global.isNetwork = false;
		if(bSuccess) {
			//var lobby = GameObject.Find("LobbyUI") as GameObject;
			//lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
			//lobby.SendMessage("PlusBuyedItem",PurchaseType, SendMessageOptions.DontRequireReceiver);
			//GameObject.Find("Audio").SendMessage("CompleteSound");
			callback(bSuccess);
			OnCloseClick();
		}else{
			var _parent = gameObject.transform.FindChild("Content_Fail").gameObject as GameObject;
			_parent.SetActive(true);
			gameObject.GetComponent<TweenAction>().doubleTweenScale(_parent);
			string buyRes = "icon_coin";
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

	}

	protected void ResponseCashFinish(System.Action<bool> callback){

		Global.isNetwork = false;
			callback(true);

		transform.FindChild("Content_BUY").gameObject.SetActive(false);
		var pop = transform.FindChild("Content_Success") as Transform;
		pop.gameObject.SetActive(true);
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		pop.FindChild("lbText").GetComponent<UILabel>().text = string.Format( KoStorage.GetKorString("72817"));
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("btnok").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("71000");
		pop.FindChild("icon_product").gameObject.SetActive(false);
		pop.FindChild("lbName").GetComponent<UILabel>().text = string.Format( KoStorage.GetKorString("72816"), BuyCount);
	}
	protected void OnSuccessClick(){
		OnCloseClick();
	}

	protected bool CheckBuyMoney(int buyType, int buyMoney){
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

	public virtual void  OnCloseClick(){
	//			Utility.LogWarning("modify - oncloseClick");
				if(Global.isNetwork) return;
				UserDataManager.instance._subStatus = null;
				OnBuyClick = null;
				for(int i = 0; i < transform.childCount;i++){
					transform.GetChild(i).gameObject.SetActive(false);
				}
				transform.FindChild("bg").gameObject.SetActive(true);
				var temp = transform.FindChild("Content_BUY") as Transform;
				temp.gameObject.SetActive(true);
				gameObject.SetActive(false);
				temp.FindChild("lbName1").gameObject.SetActive(false);
				temp.FindChild("lbName").gameObject.SetActive(true);
				Global.isPopUp = false;
				UserDataManager.instance.OnSubBacksub = null;
				if(bSubCheck) Global.bLobbyBack = false;
				Destroy(this);
		}

	protected void OnFailClick(){
		Global.isNetwork=false;
	//	Utility.LogWarning("OnFailClick");
	//	GameObject.Find("LobbyUI").SendMessage("OnDollarClick", SendMessageOptions.DontRequireReceiver);
		OnCloseClick();
	}
}
