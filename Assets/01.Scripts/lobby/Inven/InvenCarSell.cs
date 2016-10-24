using UnityEngine;
using System.Collections;

public class InvenCarSell : MonoBehaviour {

	void Start(){
		transform.FindChild("BTN").GetComponentInChildren<UILabel>().text 
			= KoStorage.GetKorString("71000");
	}

	void OnClose(){
		NGUITools.FindInParents<InvenMain>(gameObject).DeleteCarSlot(transform.name,0);				
		GameObject.Find("LobbyUI").SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		gameObject.SetActive(false);
	}


	void OnMatClose(){
		//!!--Utility.Log("OnMatClose");
		NGUITools.FindInParents<InvenMain>(gameObject).ResetMatSlot();		
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		gameObject.SetActive(false);
	}

	void OnSellResult(string[] strs){
		//!!--Utility.Log("No Use OnSellResult");
		transform.FindChild("lbText").GetComponent<UILabel>().text =
			string.Format(KoStorage.GetKorString("75018"),strs[0]);
		transform.FindChild("lbQuantity").GetComponent<UILabel>().text =
			string.Format("{0:#,0}", int.Parse(strs[1]));
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			//!!--Utility.Log("OnMatClose1");
			Invoke("OnMatClose",0.1f);
			return true;
		};
	}

}
