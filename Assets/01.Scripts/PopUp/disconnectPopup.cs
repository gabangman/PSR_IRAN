using UnityEngine;
using System.Collections;

public class disconnectPopup : basePopup {

	void Start(){


	}
	
	public override void OnOkClick(){
		StartCoroutine("onLogout");
	}
	void OnBuyOKClick(){
		StartCoroutine("onLogout");
	}

	public void InitPopUp(){
		//Utility.LogWarning("disconnet");
		string[] eText = new string[3];
		if(KoStorage.kostroage == null) {
			eText[0] = "네트워크 연결 실패";
			eText[1] ="확인";
			eText[2] = string.Format("[009cff]서버와의 접속이 끊겼습니다.[-] \n 게임을 다시 실행합니다.");
			Utility.LogError("Kostorage error2");
		}else{
			eText[0] = KoStorage.getStringDic("71105");// "연결 실패";
			eText[1] = KoStorage.GetKorString("71000");
			eText[2] = KoStorage.getStringDic("72314");
		}
		transform.FindChild("Content_BUY").FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		ChangeContentNoCheckOKayString(eText[0], eText[1], eText[2]);
	}

	public void InitRacePopup(){
		transform.FindChild("Content_BUY").FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		transform.FindChild("lbOk").GetComponent<UILabel>().text =KoStorage.GetKorString("71000");
		transform.FindChild("lbName").GetComponent<UILabel>().text =KoStorage.getStringDic("72314");
	}

	IEnumerator onLogout(){
	yield return new WaitForSeconds(1.0f);
	GameObject.Find("ManagerGroup").SendMessage("GroupDestroy");
	Global.Loading = false;
	Global.isLobby = true;
	Global.isLoadFinish = false;
	Destroy(this);
	gameObject.SetActive(false);
	Global.gReLoad = 1;
	Global.gChampTutorial = 0;
	GV.gInfo = null;
	Application.LoadLevel("Splash");
}

}
