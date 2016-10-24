using UnityEngine;
using System.Collections;

public class emptyFuelPopup : basePopup {

	public override void OnOkClick(){
		GameObject.Find("LobbyUI").SendMessage("OnDollarClick", SendMessageOptions.DontRequireReceiver);
		OnCloseClick();
		/*GameObject.Find("LobbyUI").GetComponent<LobbyManager>().OnMoveToBack("Coin", ()=>{
			Invoke("OnCloseClick",0.05f);
			//OnCloseClick();
		});*/
	}


	public void InitPopUp(){
		ChangeContentOKayString(KoStorage.GetKorString("72006"), KoStorage.GetKorString("71000"),KoStorage.GetKorString("72319"));
		bSub = true;
		Global.bLobbyBack = true;
		isCallback = true;
		UserDataManager.instance.OnSubBacksubsub = ()=>{
			OnCloseClick();
		};
		/*var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text =  KoStorage.GetKorString("72006");//
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =  KoStorage.GetKorString("71000");//"확인";
		pop.FindChild("lbName").GetComponent<UILabel>().text = KoStorage.GetKorString("72319");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);*/

	}
}
