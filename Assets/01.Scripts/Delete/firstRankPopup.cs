using UnityEngine;
using System.Collections;

public class firstRankPopup : MonoBehaviour {

	void Start(){

	}

	public void OnOkClick(){
		if(Global.isNetwork) return;
		GameObject.Find("LobbyUI").SendMessage("FirstRankPopUpEnd");

		gameObject.SetActive(false);
		Destroy(this);
		
	}
	public void InitPopUp(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		
		pop.FindChild("lbText").GetComponent<UILabel>().text =  KoStorage.getStringDic("71202");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");
		pop.FindChild("lbName").GetComponent<UILabel>().text =KoStorage.getStringDic("72309");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
	}


	public void OnCloseClick(){
		OnOkClick();
	}
}
