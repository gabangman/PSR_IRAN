using UnityEngine;
using System.Collections;

public class CarJunkPopup : basePopup {

	private System.Action Callback1;
	public override void OnOkClick(){
		Callback();
		Callback1 = null;
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("Sprite (Check_V)").GetComponent<UIButtonMessage>().functionName = "OnCloseClick";
		OnCloseClick();
	}

	public void InitPopUp(System.Action callback, System.Action callback1, string carName){
		this.Callback = callback;
		this.Callback1 = callback1;
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("75008");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =  string.Empty;
		pop.FindChild("lbName").GetComponent<UILabel>().text = 
			string.Format(KoStorage.GetKorString("75009"), carName);
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);
		pop.FindChild("lbOk").gameObject.SetActive(true);
		pop.FindChild("lbOk").GetComponent<UILabel>().text =  KoStorage.GetKorString("71000");//"확인";
		pop.FindChild("Sprite (Check_V)").GetComponent<UIButtonMessage>().functionName = "OnCancel";
	
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnCancel",0.1f);
			return true;
		};
	}

	void OnCancel(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("Sprite (Check_V)").GetComponent<UIButtonMessage>().functionName = "OnCloseClick";
		Callback1();
		Callback1 = null;
		OnCloseClick();
	}
}
