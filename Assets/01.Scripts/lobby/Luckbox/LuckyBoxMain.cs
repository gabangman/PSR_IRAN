using UnityEngine;
using System.Collections;

public class LuckyBoxMain : MonoBehaviour {

	public GameObject infoS, infoG;
	public Transform btnS, btnG;
	public GameObject resultS, resultG;
	LuckyBoxAni luckyAni;
	bool isCount =false;
	void Awake(){
		luckyAni = GameObject.Find("ENV_LuckyBox").GetComponent<LuckyBoxAni>();
		Invoke("SetLuckyInit", 2.0f);

	}
	void SetLuckyInit(){
		isCount = true;
	}
	void Start(){
		resultS.transform.FindChild("SilverBtn").FindChild("lbText").GetComponent<UILabel>().text 
			= KoStorage.GetKorString("71000");
		resultG.transform.FindChild("GoldBtn").FindChild("lbText").GetComponent<UILabel>().text 
			= KoStorage.GetKorString("71000");
		btnS.FindChild("lbtitle").GetComponent<UILabel>().text = KoStorage.GetKorString("74003"); // silver
		btnG.FindChild("lbtitle").GetComponent<UILabel>().text =KoStorage.GetKorString("74004"); // gold

	


	}


	void OnEnable(){
		infoS.SetActive(true);
		infoS.SendMessage("ShowWindow");
		if(infoG.activeSelf) infoG.SetActive(false);
		btnS.FindChild("Select").gameObject.SetActive(true);
		btnG.FindChild("Select").gameObject.SetActive(false);
		luckyAni.setBoxTexture(0);
		resultS.SetActive(false);
		resultG.SetActive(false);
		var obj = btnG.transform.parent.gameObject as GameObject;
		if(!obj.activeSelf){
			obj.SetActive(true);
			infoS.GetComponent<InfoWindow>().showSub(obj);
		}else{
		//	obj.SetActive(true);
		//	infoS.GetComponent<InfoWindow>().showSub(obj);
		}
		//Utility.LogWarning("Lucky");
		luckyAni.defaultBoxAnimation(isCount);
		GV.LuckyCarClick = 0;
	}

	void OnSilver(){
		if(Global.isAnimation) return;
		var temp = btnS.FindChild("Select") as Transform;
		if(temp.gameObject.activeSelf) return;
		temp.gameObject.SetActive(true);
		infoS.SetActive(true);
		infoS.SendMessage("ShowWindow");
		infoG.SendMessage("HiddenWindow");
		btnG.FindChild("Select").gameObject.SetActive(false);
		luckyAni.PlayBoxUpDownReady(0);
		GameObject.Find("LobbyUI").SendMessage("ShowInfoTipWindow", "sLucky");

	}

	void OnGold(){
		if(Global.isAnimation) return;
		var temp = btnG.FindChild("Select") as Transform;
		if(temp.gameObject.activeSelf) return;
		temp.gameObject.SetActive(true);
		btnS.FindChild("Select").gameObject.SetActive(false);
		infoG.SetActive(true);
		infoG.SendMessage("ShowWindow");
		infoS.SendMessage("HiddenWindow");
		luckyAni.PlayBoxUpDownReady(1);
		GameObject.Find("LobbyUI").SendMessage("ShowInfoTipWindow", "gLucky");
	}


	public void SilverCouponResult(){
		if(Global.isAnimation) return;
		infoS.SendMessage("HiddenWindow");
		infoS.GetComponent<InfoWindow>().hiddenSub(btnG.transform.parent.gameObject);
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<LuckyboxPopup>().InitCouponPopUp(()=>{
			luckyAni.ChoiceBox(0,(carid, mClass)=>{
				resultS.SetActive(true);
				TweenPosition[] tw = resultS.transform.GetComponentsInChildren<TweenPosition>();
				foreach(TweenPosition ta in tw){
					ta.Reset();
					ta.enabled =true;
				}
				TweenScale t =resultS.transform.FindChild("BG_Light").GetComponent<TweenScale>();//GetComponentInChildren<TweenScale>();
				t.Reset();
				t.enabled = true;
				//transform.GetComponent<TweenAction>().doubleTweenScale(resultS);
				GV.UpdateCouponList(0,-1);
				GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
				Common_Car_Status.Item carItem = Common_Car_Status.Get(carid);
				var tr = resultS.transform.FindChild("Info_status") as Transform;
				tr.FindChild("name").GetComponent<UILabel>().text = carItem.Name;
				Common_Class.Item classItem = GV.getClassTypeID(mClass, carItem.Model);
				tr.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("74025");
				//string.Format(KoStorage.GetKorString("74024"), carItem.ReqLV, classItem.UpLimit, classItem.StarLV, carItem.GearLmt);
				tr.FindChild("Class").FindChild("C_Text").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74024"), mClass);
				tr.FindChild("Class").FindChild("C_Icon").GetComponent<UISprite>().spriteName = "Class_"+mClass;
				tr.FindChild("Icon").GetComponent<UISprite>().spriteName = carid.ToString();
				tr.FindChild("lbStarLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74028"),classItem.StarLV);
				tr.FindChild("lbReqLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74027"),carItem.ReqLV);
				tr.FindChild("lbGearLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74029"), carItem.GearLmt+classItem.GearLmt);
				GameObject.Find("Audio").SendMessage("CompleteSound");
				myAcc.instance.account.bLobbyBTN[2] = true;
				myAcc.instance.account.bInvenBTN[0] = true;
				if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
				UserDataManager.instance._subStatus = ()=>{
					Invoke("OnCancleS",0.1f);
					return true;
				};
			});
		},
		()=>{
			OnCancleS();
			GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
		}, "Silver");
	
	}

	public void SilverCouponResultFail(){
		if(Global.isAnimation) return;
		infoS.SendMessage("HiddenWindow");
		infoS.GetComponent<InfoWindow>().hiddenSub(btnG.transform.parent.gameObject);
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<LuckyboxPopup>().InitFailPopUp(()=>{
			OnCancleS();
			GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
		},
		()=>{
			OnCancleS();
			GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
		}, "Silver");
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnCancleS",0.1f);
			return true;
		};

	}
	public void SilverResult(int price){
		if(Global.isAnimation) return;
		infoS.SendMessage("HiddenWindow");
		infoS.GetComponent<InfoWindow>().hiddenSub(btnG.transform.parent.gameObject);
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<LuckyboxPopup>().InitPopUp(()=>{
			luckyAni.ChoiceBox(0,(carid, mClass)=>{
				resultS.SetActive(true);
				GV.myDollar = GV.myDollar - price;
				GV.updateDollar = price;
				TweenPosition[] tw = resultS.transform.GetComponentsInChildren<TweenPosition>();
				foreach(TweenPosition ta in tw){
					ta.Reset();
					ta.enabled =true;
				}
				TweenScale t =resultS.transform.FindChild("BG_Light").GetComponent<TweenScale>();//GetComponentInChildren<TweenScale>();
				t.Reset();
				t.enabled = true;
				//transform.GetComponent<TweenAction>().doubleTweenScale(resultS);
				GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
				Common_Car_Status.Item carItem = Common_Car_Status.Get(carid);
				var tr = resultS.transform.FindChild("Info_status") as Transform;
				tr.FindChild("name").GetComponent<UILabel>().text = carItem.Name;
				Common_Class.Item classItem = GV.getClassTypeID(mClass, carItem.Model);
				tr.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("74025");
					//string.Format(KoStorage.GetKorString("74024"), carItem.ReqLV, classItem.UpLimit, classItem.StarLV, carItem.GearLmt);
				tr.FindChild("Class").FindChild("C_Text").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74024"), mClass);
				tr.FindChild("Class").FindChild("C_Icon").GetComponent<UISprite>().spriteName = "Class_"+mClass;
				tr.FindChild("Icon").GetComponent<UISprite>().spriteName = carid.ToString();
				tr.FindChild("lbStarLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74028"),classItem.StarLV);
				tr.FindChild("lbReqLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74027"),carItem.ReqLV);
				tr.FindChild("lbGearLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74029"), carItem.GearLmt+classItem.GearLmt);
				GameObject.Find("Audio").SendMessage("CompleteSound");
				myAcc.instance.account.bLobbyBTN[2] = true;
				myAcc.instance.account.bInvenBTN[0] = true;
				if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
				UserDataManager.instance._subStatus = ()=>{
					Invoke("OnCancleS",0.1f);
					return true;
				};
			}, price);
		},
		()=>{
			OnCancleS();
			GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
		}, "Silver");

	}


	public void GoldCouponResult(){
		if(Global.isAnimation) return;
		infoG.SendMessage("HiddenWindow");
		infoG.GetComponent<InfoWindow>().hiddenSub(btnG.transform.parent.gameObject);
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<LuckyboxPopup>().InitCouponPopUp(()=>{
			luckyAni.ChoiceBox(1,(carid, mClass)=>{
				resultG.SetActive(true);
			//	GV.myCoin = GV.myCoin - price;
			//	GV.updateCoin = price;
				GV.UpdateCouponList(1,-1);
				TweenPosition[] tw = resultG.transform.GetComponentsInChildren<TweenPosition>();
				foreach(TweenPosition ta in tw){
					ta.Reset();
					ta.enabled =true;
				}
				TweenScale t =resultG.transform.FindChild("BG_Light").GetComponent<TweenScale>();
				t.Reset();
				t.enabled = true;
				//transform.GetComponent<TweenAction>().doubleTweenScale(resultG);
				GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
				Common_Car_Status.Item carItem = Common_Car_Status.Get(carid);
				var tr = resultG.transform.FindChild("Info_status") as Transform;
				tr.FindChild("name").GetComponent<UILabel>().text = carItem.Name;
				Common_Class.Item classItem = GV.getClassTypeID(mClass, carItem.Model);
				tr.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("74025");
				//string.Format(KoStorage.GetKorString("74024"), carItem.ReqLV, classItem.UpLimit, classItem.StarLV, carItem.GearLmt);
				tr.FindChild("Class").FindChild("C_Text").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74024"), mClass);
				tr.FindChild("Class").FindChild("C_Icon").GetComponent<UISprite>().spriteName = "Class_"+mClass;
				tr.FindChild("Icon").GetComponent<UISprite>().spriteName = carid.ToString();
				tr.FindChild("lbStarLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74028"),classItem.StarLV);
				tr.FindChild("lbReqLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74027"),carItem.ReqLV);
				tr.FindChild("lbGearLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74029"), carItem.GearLmt+classItem.GearLmt);
				GameObject.Find("Audio").SendMessage("CompleteSound");
				myAcc.instance.account.bLobbyBTN[2] = true;
				myAcc.instance.account.bInvenBTN[0] = true;
				if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
				UserDataManager.instance._subStatus = ()=>{
					Invoke("OnCancleG",0.1f);
					return true;
				};
			});
		},
		()=>{
			
			GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
			OnCancleG();
		}, "Gold");
	}
	public void GoldCouponResultFail(){
		if(Global.isAnimation) return;
		infoG.SendMessage("HiddenWindow");
		infoG.GetComponent<InfoWindow>().hiddenSub(btnG.transform.parent.gameObject);
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<LuckyboxPopup>().InitFailPopUp(()=>{
			OnCancleG();
			GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
		},
		()=>{
			OnCancleG();
			GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
		}, "Gold");
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnCancleG",0.1f);
			return true;
		};
	}
	public void GlodResult(int price){
		if(Global.isAnimation) return;
		
		infoG.SendMessage("HiddenWindow");
		infoG.GetComponent<InfoWindow>().hiddenSub(btnG.transform.parent.gameObject);
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<LuckyboxPopup>().InitPopUp(()=>{
			luckyAni.ChoiceBox(1,(carid, mClass)=>{
				resultG.SetActive(true);
				GV.myCoin = GV.myCoin - price;
				GV.updateCoin = price;
				TweenPosition[] tw = resultG.transform.GetComponentsInChildren<TweenPosition>();
				foreach(TweenPosition ta in tw){
					ta.Reset();
					ta.enabled =true;
				}
				TweenScale t =resultG.transform.FindChild("BG_Light").GetComponent<TweenScale>();
				t.Reset();
				t.enabled = true;
				//transform.GetComponent<TweenAction>().doubleTweenScale(resultG);
				GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
				Common_Car_Status.Item carItem = Common_Car_Status.Get(carid);
				var tr = resultG.transform.FindChild("Info_status") as Transform;
				tr.FindChild("name").GetComponent<UILabel>().text = carItem.Name;
				Common_Class.Item classItem = GV.getClassTypeID(mClass, carItem.Model);
				tr.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("74025");
				//string.Format(KoStorage.GetKorString("74024"), carItem.ReqLV, classItem.UpLimit, classItem.StarLV, carItem.GearLmt);
				tr.FindChild("Class").FindChild("C_Text").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74024"), mClass);
				tr.FindChild("Class").FindChild("C_Icon").GetComponent<UISprite>().spriteName = "Class_"+mClass;
				tr.FindChild("Icon").GetComponent<UISprite>().spriteName = carid.ToString();
				tr.FindChild("lbStarLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74028"),classItem.StarLV);
				tr.FindChild("lbReqLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74027"),carItem.ReqLV);
				tr.FindChild("lbGearLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74029"), carItem.GearLmt+classItem.GearLmt);
				GameObject.Find("Audio").SendMessage("CompleteSound");
				myAcc.instance.account.bLobbyBTN[2] = true;
				myAcc.instance.account.bInvenBTN[0] = true;
				if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
				UserDataManager.instance._subStatus = ()=>{
					Invoke("OnCancleG",0.1f);
					return true;
				};
			},price);
		},
		()=>{
		
			GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
			OnCancleG();
		}, "Gold");
	}

	void OnClose(GameObject obj){
		if(obj.name.Equals("GoldBtn")){
			OnCancleG();
		
		}else{
			OnCancleS();
		}
	}


	void OnCancleG(){
			Global.isAnimation = false;
			resultG.SetActive(false);
			infoG.SetActive(true);
			infoG.SendMessage("ShowWindow");
			infoG.transform.parent.gameObject.SetActive(true);
			infoG.GetComponent<InfoWindow>().showSub(btnG.transform.parent.gameObject);
  			GameObject.Find("LobbyUI").SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
			if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;

	}
	void OnCancleS(){
			Global.isAnimation = false;
			resultS.SetActive(false);
			infoS.SetActive(true);
			infoS.SendMessage("ShowWindow");
			infoS.transform.parent.gameObject.SetActive(true);
			infoS.GetComponent<InfoWindow>().showSub(btnG.transform.parent.gameObject);
			GameObject.Find("LobbyUI").SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
			if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;

	}

	public GameObject DebugSetObj;
	protected void OnDebugClick(){
		DebugSetObj.SetActive(true);
	}

}
