using UnityEngine;
using System.Collections;

public class SponExpirePopup : basePopup {
	
	
	public override void OnOkClick(){
		if(Callback != null)
			Callback();
		OnCloseClick();
	}
	
	public void InitPopUp(System.Action callback){
		/*this.Callback = ()=>{
			var lobby = GameObject.Find("LobbyUI") as GameObject;
			lobby.SendMessage("PlusBuyedItem",3, SendMessageOptions.DontRequireReceiver);
		}; */
		this.Callback = callback;
		ChangeContentNoCheckOKayString(KoStorage.GetKorString("76110"),	KoStorage.GetKorString("71000"),KoStorage.GetKorString("76111"));
		if(!Global.bLobbyBack){
			Global.bLobbyBack = true;
			isCallbacksub = true;
			UserDataManager.instance.OnSubBacksub = ()=>{
				Invoke("OnOkClick",0.02f);
			};

		}else{
			if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
			UserDataManager.instance._subStatus = ()=>{
				Invoke("OnOkClick",0.02f);
				return true;
			};
		}


	}

	public void InitReadyPopUp(System.Action callback){
		this.Callback = callback;//710515
		ChangeContentNoCheckOKayString(KoStorage.GetKorString("76110"),	KoStorage.GetKorString("71000"),KoStorage.GetKorString("76111"));
		if(!Global.bLobbyBack){
			Global.bLobbyBack = true;
			isCallbacksub = true;
			UserDataManager.instance.OnSubBacksub = ()=>{
				Invoke("OnOkClick",0.02f);
			};
			
		}else{
			if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
			UserDataManager.instance._subStatus = ()=>{
				Invoke("OnOkClick",0.02f);
				return true;
			};
		}
		
		
	}

	public void SetInvenCarNumber(){
		//this.Callback = callback;
		ChangeContentNoCheckOKayString(KoStorage.GetKorString("76110"),	KoStorage.GetKorString("71000"),KoStorage.GetKorString("75021"));
	}
	

	
}
