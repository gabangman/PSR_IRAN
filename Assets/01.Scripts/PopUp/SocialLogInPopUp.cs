using UnityEngine;
using System.Collections;

public class SocialLogInPopUp : basePopup {
	
	void Start(){
	}
	
	public override void OnOkClick(){
		Callback();
		OnCloseClick();
	}
	
	public void InitPopUp(int idx ){
		//idx = 0 FB, idx = 1 Google
		string str =  null;
		string str2 = null;
		string str1 = KoStorage.GetKorString("71000");
		
		if(idx == 0){
			str = KoStorage.GetKorString("72322");
			str2 = KoStorage.GetKorString("72323");
			
		}else if(idx == 1){
			str = KoStorage.GetKorString("72325");
			str2 = KoStorage.GetKorString("72326");
			//str2 = "구글 계정에 로그인 하시겠습니까?";
			
		}else if(idx == 2){
			str = KoStorage.GetKorString("72328");
			str2 = KoStorage.GetKorString("72329");
		}
		ChangeContentOKayString(str,str1,str2);
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
