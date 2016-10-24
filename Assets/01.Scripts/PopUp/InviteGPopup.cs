using UnityEngine;
using System.Collections;

public class InviteGPopup : basePopup {

	void Start(){
	}
	
	public override void OnOkClick(){
		Callback();
		OnCloseClick();
	}
	
	public void InitPopUp(){
	//	string str = KoStorage.GetKorString("72817");
	//	string str2 = "선택된 친구에서 보낼꺼야?";
	//	string str1 = KoStorage.GetKorString("71000");
	//	ChangeContentOKayString(str,str1,str2);
	}
	
	public void onFinishCallback(System.Action callback){
		this.Callback = callback;
	}
}
