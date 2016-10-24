using UnityEngine;
using System.Collections;

public class firstattendPopup : basePopup {


	public override void OnOkClick(){
		Callback();
		OnCloseClick();
	}
	
	public void InitPopUp(System.Action callback){
		this.Callback = callback;
		ChangeContentNoCheckOKayString(KoStorage.GetKorString("71202"),	KoStorage.GetKorString("71000"),KoStorage.GetKorString("73400"));
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnOkClick",0.1f);
			return true;
		};
	}
}
