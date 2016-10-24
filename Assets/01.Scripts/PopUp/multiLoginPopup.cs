using UnityEngine;
using System.Collections;

public class multiLoginPopup : MonoBehaviour {
	
	void Start(){
		
	}
	
	public void InitPopUp(System.Action callback, System.Action closeCallback){
		Global.isPopUp = true;
		this.thisCallback = callback;
		this.closeCallback = closeCallback;
		string[] eText = new string[3];
		if(KoStorage.kostroage == null) {
			eText[0] = "SNS Connect Failed";
			eText[1] ="OK";
			eText[2] = string.Format("SNS Connect Failed \n Please connect again after a while");
			Utility.LogError("Kostorage error1");
		}else{
			eText[0] = KoStorage.GetKorString("71202");
			eText[1] =KoStorage.GetKorString("71000");
			eText[2] = KoStorage.GetKorString("72331");
		}
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = eText[0];//"게임 탈퇴";//KoStorage.getStringDic("60234");
		pop.FindChild("lbName").GetComponent<UILabel>().text = eText[2] ;
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =string.Empty;
		pop.FindChild("lbOk").gameObject.SetActive(true);
		pop.FindChild("lbOk").GetComponent<UILabel>().text =eText[1];
		pop.FindChild("icon_product").gameObject.SetActive(false);//GetComponent<UILabel>().text =eText[1];
		
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
		pop.FindChild("btnok").gameObject.SetActive(true);
	}
	
	private System.Action thisCallback, closeCallback;
	public void OnOkClick(){
		if(thisCallback != null) thisCallback();
		thisCallback = null;
		closeCallback = null;
		//OnCloseClick();
		Destroy(this);
		gameObject.SetActive(false);
	}
	public void OnCloseClick(){
		
		Global.isPopUp = false;
		thisCallback = null;
		closeCallback();
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		Destroy(this);
		gameObject.SetActive(false);
	}
	
}
