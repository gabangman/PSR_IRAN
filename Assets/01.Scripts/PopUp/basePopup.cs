using UnityEngine;
using System.Collections;

public class basePopup : MonoBehaviour {
	protected System.Action Callback;
	protected bool isCallback = false;
	protected bool isCallbacksub = false;
	protected bool bSub = false;
	protected void OnCloseClick(){
		Global.isPopUp = false;
		Callback = null;
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		if(isCallback) UserDataManager.instance.OnSubBacksubsub = null;
		if(isCallbacksub) UserDataManager.instance.OnSubBacksub = null;
		if(bSub) {Global.bLobbyBack = false; bSub = false;}
		gameObject.SetActive(false);
		Destroy(this);
	}

	public void ChangeContentString(string lbText, string lbPrice, string lbName){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = lbText;
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =lbPrice;
	//	pop.FindChild("lbPrice").GetComponent<UILabel>().text =lbPrice;
	//	pop.FindChild("lbPrice").GetComponent<UILabel>().text =lbPrice;

		pop.FindChild("lbName").GetComponent<UILabel>().text = lbName;
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);
	}

	public void ChangeContentNoCheckOKayString(string lbText, string lbPrice, string lbName){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		ChangeContentOKayString(lbText,lbPrice,lbName);
	}

	public void ChangeContentCheckOKayString(string lbText, string lbPrice, string lbName){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
		ChangeContentOKayString(lbText,lbPrice,lbName);
	}

	public void ChangeContentOKayString(string lbText, string lbPrice, string lbName){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = lbText;
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =string.Empty;
		pop.FindChild("lbName").GetComponent<UILabel>().text = lbName;
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("btnok").FindChild("lbOk").gameObject.SetActive(true);

		if(pop.FindChild("lbOk") != null){
			pop.FindChild("lbOk").gameObject.SetActive(true);
			pop.FindChild("lbOk").GetComponent<UILabel>().text =lbPrice;
			pop.FindChild("btnok").FindChild("lbOk").GetComponent<UILabel>().text = string.Empty;
		}else{
			pop.FindChild("btnok").FindChild("lbOk").GetComponent<UILabel>().text =lbPrice;
		}
		if(pop.FindChild("icon_product") != null){
			pop.FindChild("icon_product").gameObject.SetActive(false);
		}
	
	}
	public virtual void OnOkClick(){}
}
