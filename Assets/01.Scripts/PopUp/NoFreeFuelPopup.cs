using UnityEngine;
using System.Collections;

public class NoFreeFuelPopup : basePopup {

	public override void OnOkClick(){
		Callback();
		OnCloseClick();
	}
	
	public void InitPopUp(System.Action callback){
		this.Callback = callback;
		string str = KoStorage.GetKorString("72706");
		string str2 = KoStorage.GetKorString("72320");
		string str1 = KoStorage.GetKorString("71000");
		ChangeContentNoCheckOKayString(str,str1,str2);
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnCloseClick",0.1f);
			return true;
		};
	}
	
	public void onFinishCallback(System.Action callback){
		this.Callback = callback;
	}
}
