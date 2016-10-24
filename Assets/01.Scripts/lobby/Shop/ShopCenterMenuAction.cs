using UnityEngine;
using System.Collections;

public class ShopCenterMenuAction : MonoBehaviour {

	void OnEnable(){
		InitNewButton();
	}

	public void DisableCarNewButton(){
		transform.FindChild("MyCarShop").FindChild("icon_New").gameObject.SetActive(false);
		//myAccount.instance.account.buttonStatus.isShopCarNew = false;
	}

	public void DisableCrewNewButton(){
		transform.FindChild("MyCrewShop").FindChild("icon_New").gameObject.SetActive(false);
		//myAccount.instance.account.buttonStatus.isShopCrewNew = false;
	}

	void InitNewButton(){
	//	bool b =myAccount.instance.account.buttonStatus.isShopCarNew;
	//	transform.FindChild("MyCarShop").FindChild("icon_New").gameObject.SetActive(b);
	//	b = myAccount.instance.account.buttonStatus.isShopCrewNew;
	//	transform.FindChild("MyCrewShop").FindChild("icon_New").gameObject.SetActive(b);
	//	int tempid = Base64Manager.instance.GlobalEncoding(Global.gSale);
	//	if(tempid== 10){
	//		transform.FindChild("MyCoinShop").FindChild("icon_Event").gameObject.SetActive(true);
	//	}else{
	//		transform.FindChild("MyCoinShop").FindChild("icon_Event").gameObject.SetActive(false);
	//	}
	}

	public void CheckNewButton(string strName){
		Utility.LogWarning("CheckNewButton " + strName);
		return;
		/*
		var shop = transform.FindChild(strName).gameObject as GameObject;
		shop  = shop.transform.FindChild("icon_New").gameObject;
		if(!shop.activeSelf) return;
		int cnt = 0; bool b = false;
		if(string.Equals(strName, "MyCrewShop")){
			// cnt = Common_Crew_Status.GetDictionaryCount();
			cnt = myAccount.instance.account.buttonStatus.isCrewNew.Length;
			for(int i = 0; i < cnt ; i++){
				b = b || myAccount.instance.account.buttonStatus.isCrewNew[i];
			}
			if(!b) myAccount.instance.account.buttonStatus.isShopCrewNew = false;
		}else{
			//cnt = Common_Car_Status.carListItem.Count;
			cnt = myAccount.instance.account.buttonStatus.isCarNew.Length;
			for(int i = 0; i < cnt ; i++){
				b = b || myAccount.instance.account.buttonStatus.isCarNew[i];
			}
			if(!b) myAccount.instance.account.buttonStatus.isShopCarNew = false;
		}
	if(b) return;
	else shop.SetActive(false);*/
	}

	public Transform Car, Crew, Coin;
	const int originScaleY = 44;
	const int clickScaleY = 55;
	public bool OnCarClick(){
		bool b = false;
		var carsprite = Car.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == Color.white) b = true;

		carsprite.color = Color.white;
		Car.localScale = new Vector3(180, clickScaleY,1);

		Crew.localScale = new Vector3(180,originScaleY,1);
		Crew.GetComponent<UISprite>().color = Color.gray;
	//	Coin.localScale = new Vector3(180, originScaleY,1);
	//	Coin.GetComponent<UISprite>().color = Color.gray;
		return b;
	}

	public bool OnCrewClick(){
		bool b = false;
		var carsprite = Crew.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == Color.white) b = true;
		
		carsprite.color = Color.white;
		Crew.localScale = new Vector3(180, clickScaleY,1);
		
	//	Coin.localScale = new Vector3(180,originScaleY,1);
	//	Coin.GetComponent<UISprite>().color = Color.gray;
		Car.localScale = new Vector3(180, originScaleY,1);
		Car.GetComponent<UISprite>().color = Color.gray;
		return b;
	}

	public bool OnCoinClick(){
		bool b = false;
		var carsprite = Coin.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == Color.white) b = true;
		
		carsprite.color = Color.white;
		Coin.localScale = new Vector3(180, clickScaleY,1);
		
		Car.localScale = new Vector3(180,originScaleY,1);
		Car.GetComponent<UISprite>().color = Color.gray;
		Crew.localScale = new Vector3(180, originScaleY,1);
		Crew.GetComponent<UISprite>().color = Color.gray;
		//Utility.LogWarning("COIN = " + Color.gray);
		return false;
	}


	public void ResetButtonColor(){
		Car.GetComponent<UISprite>().color = Color.gray;
		Crew.GetComponent<UISprite>().color = Color.gray;
		//Coin.GetComponent<UISprite>().color = Color.gray;
	}
}
