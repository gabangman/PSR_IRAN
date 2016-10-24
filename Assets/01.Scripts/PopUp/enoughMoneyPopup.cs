using UnityEngine;
using System.Collections;

public class enoughMoneyPopup : basePopup {

	void Start(){

		
	}
	
	public override void OnOkClick(){
	//	GameObject.Find("LobbyUI").GetComponent<LobbyManager>().OnMoveToCoinShop();
		GameObject.Find("LobbyUI").SendMessage("OnDollarClick", SendMessageOptions.DontRequireReceiver);
		OnCloseClick();
		
	//	GameObject.Find("LobbyUI").SendMessage("OnDollarClick");
	//	OnCloseClick();
	
	}

	public void InitPopUp(){
	//	var pop = transform.FindChild("Content_BUY") as Transform;
	//	pop.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.getStringDic("73055");//
	//	pop.FindChild("lbPrice").GetComponent<UILabel>().text =  KoStorage.GetKorString("71000");//"확인";
	//	pop.FindChild("lbName").GetComponent<UILabel>().text =KoStorage.GetKorString("73056");

	//	pop.FindChild("btnok").gameObject.SetActive(true);
	//	pop.FindChild("icon_product").gameObject.SetActive(false);
		ChangeContentOKayString(KoStorage.GetKorString("73055"), KoStorage.GetKorString("71000"),KoStorage.GetKorString("73056"));


	}


	public void InitDollarPopUp(){
		ChangeContentOKayString(KoStorage.GetKorString("76022"), KoStorage.GetKorString("71000"),KoStorage.GetKorString("76023"));
		bSub = true;
		Global.bLobbyBack = true;
		isCallback = true;
		UserDataManager.instance.OnSubBacksubsub = ()=>{
			OnCloseClick();
		};
	}

	public void InitCoinPopUp(){
		ChangeContentOKayString(KoStorage.GetKorString("76022"), KoStorage.GetKorString("71000"),KoStorage.GetKorString("76023"));
	}
}
