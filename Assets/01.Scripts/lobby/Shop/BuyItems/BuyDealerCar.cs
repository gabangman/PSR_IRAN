using UnityEngine;
using System.Collections;

public class BuyDealerCar : MonoBehaviour {
	protected GameObject mParent = null;
	protected System.Action<bool, string> Callback;
	
	
	public void CalledBuyCompete(System.Action<bool, string> Call, GameObject obj){
		this.Callback = Call;
		this.mParent = obj;
	}
	
	
	public void InitWindow(){
		string[] _name = gameObject.name.Split("_"[0]);
		int id = int.Parse(_name[0]);
		if(string.Equals( _name[2],"Coin")){
			buyType = 2; //coin
		}else{
			buyType = 0; //dollar;
		}
		string micon = string.Empty, mnameText  = string.Empty;
		string mbuyname = string.Empty, mbuytitle = string.Empty;
		Common_Car_Status.Item _item = Common_Car_Status.Get(id);
		micon = _item.ID;
		mnameText = _item.Name;
		mbuyname = string.Format( KoStorage.GetKorString("71011"),mnameText);
		mbuytitle = string.Format( KoStorage.GetKorString("71010"), mnameText);
		
		if(mParent != null){
			mParent.transform.FindChild("icon_product").gameObject.SetActive(false);
			mParent.transform.FindChild("lbName1").gameObject.SetActive(false);
			mParent.transform.FindChild("lbName").gameObject.SetActive(true);
			mParent.transform.FindChild("lbName").GetComponent<UILabel>().text =mbuyname;
			mParent.transform.FindChild("lbText").GetComponent<UILabel>().text = mbuytitle;
			if(buyType == 2)
				mParent.transform.FindChild("btnCoin").gameObject.SetActive(true);
			else if(buyType == 0) mParent.transform.FindChild("btnDollar").gameObject.SetActive(true);// buyname = "icon_dollar";
		}
		
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnCloseClick",0.1f);
			return true;
		};
		
	}
	
	private int buyMoney;
	private int buyType;
	private bool isSuccess;
	void OnOkClick(){
		if(Global.isNetwork) return;
		string lbPirce = mParent.transform.FindChild("lbPrice").GetComponent<UILabel>().text;
		Global.isNetwork = true;
		if(!int.TryParse(lbPirce, out buyMoney))
			buyMoney = int.Parse(lbPirce.Replace(",",string.Empty));
		int a = 0;
		if(buyType == 2){
			a = GV.myCoin - buyMoney;
			if(a < 0){
				Global.isNetwork = false;
				ResponseBuyResult(false,"SS");
				return;
			}else{
				
			}
		}


		string[] str = gameObject.name.Split('_');
		System.Collections.Generic.Dictionary<string,int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		mDic.Add("carId", int.Parse(str[0]));
		string strClass = "SS";
		mDic.Add("carClass", 3106);
		string mAPI =  "game/event/carV2";//ServerAPI.Get(90057); // "game/event/car"
		//NetworkManager.instance.HttpFormConnect("Post", mDic, "game/event/car", (request)=>{
		NetworkManager.instance.HttpFormConnect("Post", mDic, mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				GV.myCoin -= buyMoney;
				GV.updateCoin = buyMoney;
				ResponseBuyResult(true, strClass);
			}else if(status == -200){
				ResponseBuyAgainResult(false, strClass);
			}else{
				ResponseBuyResult(false, strClass);
			}
			Global.isNetwork = false;
		} );
	}
	
	int CarClassChangeProbability(){
		string strClass = string.Empty;
		int rClass = 0;
		ProbabilityClass pb = new ProbabilityClass();
		rClass = pb.DealerCarClass();
		pb =null;
		return rClass;
	}
	private void ResponseBuyResult(bool b, string strClass){
		
		
		if(b) {
			var lobby = GameObject.Find("LobbyUI") as GameObject;
			lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
			GameObject.Find("Audio").SendMessage("CompleteSound");
			Callback(b, strClass);
			myAcc.instance.account.bInvenBTN[0] = true;
			string[] str = gameObject.name.Split('_');
			ResultDealerComplete(int.Parse(str[0]), strClass);
			
		}else{
			gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(false);
			mParent = gameObject.transform.FindChild("Content_Fail").gameObject;
			mParent.SetActive(true);
			mParent.transform.FindChild("icon_product").gameObject.SetActive(false);
			gameObject.GetComponent<TweenAction>().doubleTweenScale(mParent);
			mParent.transform.FindChild("lbText").GetComponent<UILabel>().text =
				KoStorage.GetKorString("76022");//TableManager.ko.dictionary["60086"].String;
			mParent.transform.FindChild("lbName").GetComponent<UILabel>().text =
				KoStorage.GetKorString("76022");//TableManager.ko.dictionary["60087"].String;
			mParent.transform.FindChild("btnok").GetComponentInChildren<UILabel>().text = 
				KoStorage.GetKorString("71000");//TableManager.ko.dictionary["60184"].String;
			Callback(b, strClass);
			if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
			UserDataManager.instance._subStatus = ()=>{
				Invoke("OnFailClick",0.1f);
				return true;
			};
		}
		
	}


	public void ResponseBuyAgainResult(bool b , string strClass){
		gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(false);
		mParent = gameObject.transform.FindChild("Content_Fail").gameObject;
		mParent.SetActive(true);
		mParent.transform.FindChild("icon_product").gameObject.SetActive(false);
		gameObject.GetComponent<TweenAction>().doubleTweenScale(mParent);
		mParent.transform.FindChild("lbText").GetComponent<UILabel>().text =
			KoStorage.GetKorString("74001");//TableManager.ko.dictionary["60086"].String;
		mParent.transform.FindChild("lbName").GetComponent<UILabel>().text =
			KoStorage.GetKorString("74036");//TableManager.ko.dictionary["60087"].String;
		mParent.transform.FindChild("btnok").GetComponentInChildren<UILabel>().text = 
			KoStorage.GetKorString("71000");//TableManager.ko.dictionary["60184"].String;
		Callback(b, strClass);
		GameObject.Find("Audio").SendMessage("CompleteSound");
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnFailClick",0.1f);
			return true;
		};
	}

	void ResultDealerComplete(int carId, string strClass){
		gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(false);
		
		var mTr = transform.FindChild("DealerComplete") as Transform;
		mTr.gameObject.SetActive(true);
		mTr = mTr.GetChild(0);

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
		GameObject.Find("Audio").SendMessage("CompleteSound");
		GV.AddMyCarList(carId, strClass);
		bBuyCheck=true;
		myAcc.instance.account.bLobbyBTN[2] = true;
		myAcc.instance.account.bInvenBTN[0] = true;
	}
	private bool bBuyCheck = false;
	void OnCloseClick(){
		if(bBuyCheck){
			//GV.gBuyDealerCar = 0;
			//GV.gDealerCarID = 0;
			EncryptedPlayerPrefs.SetInt("DealerBuy",2);
			GV.bRaceLose = false;
		}
		Callback = null;
		gameObject.name = "popUp";
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
		if(bBuyCheck) GameObject.Find("LobbyUI").SendMessage("MoveToBackDealer",SendMessageOptions.DontRequireReceiver);
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		Destroy(this);
	}
	
	void OnFailClick(){
		OnCloseClick();
	}
}
