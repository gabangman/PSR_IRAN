using UnityEngine;
using System.Collections;

public class KakaoPopUp : basePopup {


	void Start(){


	}
	
	public override void OnOkClick(){
	/*	NetworkManager.RequestLogoutWithKakao(()=>{
			EncryptedPlayerPrefs.DeleteKey("Agree");
			EncryptedPlayerPrefs.DeleteKey("gameOption");
			EncryptedPlayerPrefs.DeleteKey("Tutorial");
			StartCoroutine("onLogout");
		});*/
	}

	IEnumerator onLogout(){
		yield return new WaitForSeconds(1.0f);
		GameObject.Find("ManagerGroup").SendMessage("GroupDestroy");
		Global.Loading = false;
		Global.isLobby = true;
		Global.isLoadFinish = false;
		Destroy(this);
		gameObject.SetActive(false);
		GV.gInfo = null;
		Global.gReLoad = 1;
		Global.gChampTutorial = 0;
		Application.LoadLevel("Splash");
	}

	public void InitPopUp(){
		string str = "카카오 ???";//getStringDic("60041");
		string str1 = str;
		string str2 = KoStorage.getStringDic("71118");
		ChangeContentString(str,str1,str2);
	}

}
