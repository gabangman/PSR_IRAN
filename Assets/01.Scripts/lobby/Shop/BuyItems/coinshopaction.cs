using UnityEngine;
using System.Collections;

public class coinshopaction : BuyInterAction {
		
		void Start(){
	
		}

	public coinshopaction(){
		
	}
	private string idName;
	void OnBuyFuel(GameObject target){
		idName = target.transform.parent.gameObject.name;
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		var mAction = pop.GetComponent<BuyFuel>() as BuyFuel;
		if(mAction == null) mAction = pop.AddComponent<BuyFuel>();
		mAction.SetBuyPopUp(idName);
	}

	void OnBuyCash(GameObject target){
		idName = target.transform.parent.gameObject.name;
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		var mAction = pop.GetComponent<BuyCash>() as BuyCash;
		if(mAction == null) mAction = pop.AddComponent<BuyCash>();
		var tr = transform.parent.parent.FindChild("VIP_info") as Transform;//GetComponent<VIPPoint>().ChangeVIPLevel();
		mAction.SetBuyPopUp(idName, tr);
	}


	void OnBuyDollar(GameObject target){
		idName = target.transform.parent.gameObject.name;
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		var mAction = pop.GetComponent<BuyDollar>() as BuyDollar;
		if(mAction == null) mAction = pop.AddComponent<BuyDollar>();
		mAction.SetBuyPopUp(idName);
	}

	void OnBuyCube(GameObject target){
		idName = target.transform.parent.gameObject.name;
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		var mAction = pop.GetComponent<BuyMaterial>() as BuyMaterial;
		if(mAction == null) mAction = pop.AddComponent<BuyMaterial>();
		mAction.SetBuyPopUp(idName);
	}
	void OnBuyGoldCoupon(GameObject target){
		idName = target.transform.parent.gameObject.name;
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		var mAction = pop.GetComponent<BuyCoupon>() as BuyCoupon;
		if(mAction == null) mAction = pop.AddComponent<BuyCoupon>();
		mAction.SetBuyPopUp(idName, 1);
	}
	void OnBuySilverCoupon(GameObject target){
		idName = target.transform.parent.gameObject.name;
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		var mAction = pop.GetComponent<BuyCoupon>() as BuyCoupon;
		if(mAction == null) mAction = pop.AddComponent<BuyCoupon>();
		mAction.SetBuyPopUp(idName, 0);
	}

	void OnFreeFuel(GameObject target){
		idName = target.transform.parent.gameObject.name;
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		var mAction = pop.GetComponent<BuyCoupon>() as BuyCoupon;
		if(mAction == null) mAction = pop.AddComponent<BuyCoupon>();
		mAction.SetBuyPopUp(idName, 0);
	
	}


	void OnItemClick(GameObject target){
			string price = target.transform.parent.FindChild("lbPrice").GetComponent<UILabel>().text;
			CreaetPopUpWindow(target.transform.parent.gameObject,price);
		}

	void OnCoinSelect(GameObject obj){
			var tr = gameObject.transform.parent as Transform;
			for(int i = 0; i < tr.childCount; i++){
				tr.GetChild(i).FindChild("Select").gameObject.SetActive(false);
			}
				obj.transform.FindChild("Select").gameObject.SetActive(true);
		}
		

	void CreaetPopUpWindow(GameObject obj , string price){
		if(Global.isPopUp) return;
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		var popchild = pop.transform.FindChild("Content_BUY").gameObject as GameObject;
		//var pop  = ObjectManager.CreatePrefabs(transform.parent, "Window", "popUp") as GameObject;
		//ObjectManager.ChangeObjectParent(pop, obj.transform.parent);
		//ObjectManager.ChangeObjectPosition(pop, new Vector3(0,0,-1500), Vector3.one, Vector3.zero);
		//var popchild = pop.transform.FindChild("Content_BUY").gameObject as GameObject;
		ResetIcon(popchild);
		pop.name = obj.name+"_"+price;
		popchild.transform.FindChild("lbPrice").GetComponent<UILabel>().text = price.ToString();
		var _popupaction = pop.GetComponent<popupinteraction>() as popupinteraction;
		if(_popupaction == null) _popupaction = pop.AddComponent<popupinteraction>();
		_popupaction.SaveParentObject(popchild);
		_popupaction.PopWindowStart();
		_popupaction.CalledBuyCompete(ResponseBuy);
		Global.isPopUp = true;
		popchild = pop =  null;
	}

	void ResponseBuy(bool isSuccess){
		
	}

	/*
	public void FuelExpandComplete(){
		var temp = transform.FindChild("8501_Cash").GetChild(0) as Transform;
			temp.FindChild("lbPrice").GetComponent<UILabel>().text 
			= 	KoStorage.GetKorString("74011");
		temp.FindChild("btnCash").GetComponent<UIButtonMessage>().functionName = null;
		temp.FindChild("lbUnit").gameObject.SetActive(false);
		Utility.Log("A FuelExpandComplete");
	}*/

	//{"type":4,"hashMap":{"updateDollar":100500,"updateCoin":99950},"status":0,"message":"Success."}
	//{"type":5,"hashMap":{"getCardList":[],"updateCoin":94950},"status":0,"message":"Success."}
}


