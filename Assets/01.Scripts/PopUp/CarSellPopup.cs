using UnityEngine;
using System.Collections;

public class CarSellPopup : basePopup {
	private System.Action Callback1;
	public override void OnOkClick(){
		Callback();
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("Sprite (Check_V)").GetComponent<UIButtonMessage>().functionName = "OnCloseClick";
		Callback1 = null;
		OnCloseClick();
	}
	
	public void InitPopUp(System.Action callback, System.Action callback1, Common_Car_Status.Item item, int sellPrice ){
		this.Callback = callback;
		this.Callback1 = callback1;
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text =  KoStorage.GetKorString("75004");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =  string.Empty;
		string price = string.Format("{0:#,0}" , sellPrice);
		pop.FindChild("lbName").GetComponent<UILabel>().text = 
			string.Format(KoStorage.GetKorString("75005"), item.Name, price);
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
