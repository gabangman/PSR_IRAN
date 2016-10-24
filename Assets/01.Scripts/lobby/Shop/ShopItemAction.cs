using UnityEngine;
using System.Collections;

public class ShopItemAction : MonoBehaviour {

	void OnItemClick(GameObject obj){
		if(obj.transform.FindChild("Empty").gameObject.activeSelf) return;
		var select = obj.transform.FindChild("Select").gameObject as GameObject;
		if(select.activeSelf) {
			NewButtonSetting(obj);
			return;
		}
		//Utility.Log ("OnItemClick ShopItemAction");
		if(Global.isAnimation) return;
		DisableSelectImage();
		obj.transform.FindChild("Select").gameObject.SetActive(true);
//		Utility.Log ("click obj " + obj.name);
		SendCreateWindow(obj);	
		NewButtonSetting(obj);
	}

	void NewButtonSetting(GameObject item){



	//	Utility.LogWarning("NewButtonSetting " + item.name);
		return;
		/*
		var newicon = item.transform.FindChild("icon_New").gameObject as GameObject;
		if(!newicon.activeSelf) return;
		string[] name = item.name.Split("_"[0]);
		int selectID = int.Parse(name[0]);
		newicon.SetActive(false);
		if(selectID < 1200){
			selectID -= 1000;
			myAccount.instance.account.buttonStatus.isCarNew[selectID] = false;
			CheckNewCarButton();
		}else if(selectID >= 1200 && selectID < 1300){
			selectID -= 1200;
			myAccount.instance.account.buttonStatus.isCrewNew[selectID] = false;
			CheckNewCrewButton();
		}else if(selectID > 1300){
			//selectID -= 1300;
			//myAccount.instance.account.buttonStatus.isSponsorNew[selectID] = false;
		}*/
	
	}

	void DisableSelectImage(){
		var tr = gameObject.transform.parent.parent as Transform;
		int count = tr.childCount;
		for(int i = 0; i <count; i++){
			tr.GetChild(i).GetChild(0).FindChild("Select").gameObject.SetActive(false);
		}
	}

	void Start(){
		gameObject.GetComponent<UIButtonMessage>().target = gameObject;
		gameObject.GetComponent<UIButtonMessage>().functionName = "OnItemClick";//gameObject;
	}

	void SendCreateWindow(GameObject item){
		var lobby = GameObject.Find("LobbyUI") as GameObject;
		lobby.SendMessage("OnItemClicked", item, SendMessageOptions.DontRequireReceiver);
	}


	void CheckNewCarButton(){
		Utility.LogWarning("CheckNewCarButton");
		/*
		int cnt = myAccount.instance.account.buttonStatus.isCarNew.Length;
		bool b = false;
		for(int i = 0; i < cnt ; i++){
			//	b = b || myAccount.instance.account.buttonStatus.isCarNew[i];
			if(myAccount.instance.account.buttonStatus.isCarNew[i]){
				b = true;
				break;
			}
		}
		if(b) return;
		else {
			GameObject.Find("LobbyUI").SendMessage("DisableCarNewButton", SendMessageOptions.DontRequireReceiver);
		}*/
	}

	void CheckNewCrewButton(){
		Utility.LogWarning("CheckNewCrewButton");/*
		int cnt = myAccount.instance.account.buttonStatus.isCrewNew.Length;
		bool b = false;
		for(int i = 0; i < cnt ; i++){
			if(myAccount.instance.account.buttonStatus.isCrewNew[i]){
				b = true;
				break;
			}
		}
		if(b) return;
		else {
			GameObject.Find("LobbyUI").SendMessage("DisableCrewNewButton");
		}*/
	}
	void OnDisable(){

	}


}
