using UnityEngine;
using System.Collections;

public class GamePopup : MonoBehaviour {

	void OnDisable(){
	//	for(int i = 1; i < 4; i++){
	//		transform.GetChild(i).gameObject.SetActive(false);
	//	}
	}

	void OnQuit(){

		UserDataManager.instance.bPopUpAdd = false;
		Vibration.OnNoticQuit();
	}

	void OnCancle(){
		UserDataManager.instance.setGameExit();
	}

	void OnContinue(){
		Time.timeScale = 1.0f;
		Global.bLobbyBack = false;
		GameManager.instance.btnContinue();
		UserDataManager.instance.OnSubBack = null;
		UserDataManager.instance.setGameExit();
	}

	void OnNoticQuit(){
		UserDataManager.instance.bPopUpAdd = false;
		Vibration.OnNoticQuit();
	}

	public void InitNotics(){
		var tr = transform.GetChild(1) as Transform;
		tr.gameObject.SetActive(true);
		transform.GetComponent<TweenAction>().doubleTweenScale(tr.gameObject);
		try{
			tr.FindChild("btnOk").FindChild("lbOk").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");
			tr.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("72407");
		}catch(System.Exception e){
		
		}
	
	}

	public void InitGameExit(){
		#if UNITY_ANDROID && !UNITY_EDITOR
			UserDataManager.instance.myGameDataSave();		
			EncryptedPlayerPrefs.Save();
		#endif

		var tr = transform.GetChild(3) as Transform;
		tr.gameObject.SetActive(true);
		transform.GetComponent<TweenAction>().doubleTweenScale(tr.gameObject);
		try{
			tr.FindChild("btnOk").FindChild("lbOk").GetComponent<UILabel>().text = KoStorage.GetKorString("70128");
			tr.FindChild("btnCancel").FindChild("lbOk").GetComponent<UILabel>().text = KoStorage.GetKorString("70129");
			tr.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("71115");
		}catch(System.Exception e){
			
		}
	
	}

	public void RacePause(){
		var tr = transform.GetChild(2) as Transform;
		tr.gameObject.SetActive(true);
		tr.FindChild("BtnContinue").FindChild("Label").GetComponent<UILabel>().text = KoStorage.GetKorString("71111");
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnContinue();
		};
	}
	public void InitNewVersion(){
		var tr = transform.GetChild(3) as Transform;
		tr.gameObject.SetActive(true);
		transform.GetComponent<TweenAction>().doubleTweenScale(tr.gameObject);

		try{
			tr.FindChild("btnOk").FindChild("lbOk").GetComponent<UILabel>().text = KoStorage.GetKorString("70128");
			tr.FindChild("btnCancel").FindChild("lbOk").GetComponent<UILabel>().text = KoStorage.GetKorString("70129");
			tr.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("71117");
		}catch(System.Exception e){
			
		}
	
		if(Application.platform == RuntimePlatform.IPhonePlayer)
			tr.FindChild("btnOk").GetComponent<UIButtonMessage>().functionName = "OnGameCenter";
		else tr.FindChild("btnOk").GetComponent<UIButtonMessage>().functionName = "OnGooglePlay";
		tr.FindChild("btnCancel").GetComponent<UIButtonMessage>().functionName = "OnVersionOver";

		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnVersionOver();
			
		};

	}

	void OnVersionOver(){
		UserDataManager.instance.setGameExit();
		//Application.Quit();
		Global.isNetwork = false;
	}
	void OnGooglePlay(){
		Application.OpenURL(GV.gInfo.androidMarketURL);
		Application.Quit();
	}
	
	void OnGameCenter(){
		Application.OpenURL(GV.gInfo.IosURL);
		Application.Quit();
	}

}
