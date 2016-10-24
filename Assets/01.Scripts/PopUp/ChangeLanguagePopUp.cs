using UnityEngine;
using System.Collections;

public class ChangeLanguagePopUp : basePopup {

	void Start(){
	}
	
	public override void OnOkClick(){
		Callback();
		OnCloseClick();
		GameObject.Find("ManagerGroup").SendMessage("GroupDestroy");
		Global.Loading = false;
		Global.isLobby = true;
		Global.isLoadFinish = false;
		GV.gInfo = null;
		Global.gChampTutorial = 0;
		Destroy(this);
		gameObject.SetActive(false);
		Global.gReLoad = 1;
		Application.LoadLevel("Splash");
		//Application.LoadLevel("Title");
	}
	
	public void InitPopUp(){
		string str = KoStorage.GetKorString("72519");
		string str2 = null;
		string str1 =  KoStorage.GetKorString("71000");
		str2 = KoStorage.GetKorString("72520");
		ChangeContentOKayString(str,str1,str2);
		isCallback = true;
		UserDataManager.instance.OnSubBacksubsub = ()=>{
			OnCloseClick();
		};
	}


	public void onFinishCallback(System.Action callback){
		this.Callback = callback;
	}


}
