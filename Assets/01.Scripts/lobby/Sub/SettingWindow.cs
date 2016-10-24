using UnityEngine;
using System.Collections;
using System;
using System.Text;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
public class SettingWindow : MonoBehaviour {
	//public UILabel lbFb, lbGg;
	public Transform btnFB, btnGg, btnNation;
	GameOption.OptionSetting _opt;
	void Awake(){
		gameObject.AddComponent<GameOption>();
		_opt = GameOption.OptionData;

		//transform.FindChild("Btn_Nation").GetComponent<UILabel>() = GV.gNational;
		transform.FindChild("Btn_Youtube").FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnYouTube";
	
		string gCountry =  EncryptedPlayerPrefs.GetString("CountryCode");
		if(string.Equals(gCountry, "KOR")==true){
			transform.FindChild("Naver_Home").gameObject.SetActive(true);
		}
	
	}

	void OnNaverClick(){
		Application.OpenURL("http://cafe.naver.com/psracing.cafe");
	}
	bool isBGM, isMusic, isEffect, isQuality;
	bool isAlarm, isVibration;
	bool isThird;
	void OnEnable(){
		btnNation.FindChild("icon_Nation").GetComponent<UISprite>().spriteName = GV.gNational;
		var temp = gameObject.GetComponent<GameOption>() as GameOption;
		if(temp == null) {
			gameObject.AddComponent<GameOption>();
			_opt = GameOption.OptionData;
		}
		isMusic=_opt.isMusic;
		isEffect = _opt.isEffect;// = isEffect;
		isBGM = _opt.isBGM ;
		isQuality = _opt.isHighQuality ;
		isVibration = _opt.isVibration;
		isAlarm = (Global.gPushable == 1)?true:false;
		isThird = (Global.g3Agree == 1)?true:false;
		_opt.isAlarm = isAlarm;
		_opt.isThird = isThird;
		InitVolume();
		if(SNSManager.isFB){
		//if(FB.IsLoggedIn){
			btnFB.FindChild("lbID").GetComponent<UILabel>().text = KoStorage.GetKorString("72516");
			btnFB.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnFBLogOut";
		}else{
			btnFB.FindChild("lbID").GetComponent<UILabel>().text =KoStorage.GetKorString("72515");
			btnFB.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnFBLogin";
		}
		if(SNSManager.isGoogle){
		//if (Social.localUser.authenticated) {
			btnGg.FindChild("lbID").GetComponent<UILabel>().text =KoStorage.GetKorString("72516");
			btnGg.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnGgLogout";
		}else{
			btnGg.FindChild("lbID").GetComponent<UILabel>().text = KoStorage.GetKorString("72515");
			btnGg.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnGgLogin";
		}
		isLoggin = false;

	}

	void OnYouTube(){
		Application.OpenURL(GV.gInfo.bundleURL_1);
	}

	void OnTutorialDrag(){
		Application.OpenURL(GV.gInfo.bundleURL_2);
	}
	void OnFBLogin(){
		if(isLoggin) return;
		isLoggin = true;
		Utility.LogWarning("OnFBLogin");
		GameObject.Find("LobbyUI").SendMessage("FacebookButtonActivity", 1, SendMessageOptions.DontRequireReceiver); 
		SNSManager.FaceBookLogin(
			()=>{
			btnFB.FindChild("lbID").GetComponent<UILabel>().text =KoStorage.GetKorString("72516");
			btnFB.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnFBLogOut";
			Global.gisFBLogin = 1;
			isLoggin = false;
			//GameObject.Find("LobbyUI").SendMessage("LobbyLoginButtonStatus"); //- 추가할것
		},
		()=>{
			isLoggin = false;
			GameObject.Find("LobbyUI").SendMessage("FacebookButtonActivity", 0, SendMessageOptions.DontRequireReceiver); 
			//GameObject.Find("LobbyUI").SendMessage("LobbyLoginButtonStatus"); - 추가할것
		}
		);
		
		/*
		FB.Login("public_profile,email,user_friends,publish_actions", (result)=>{
			if (result.Error != null)
				Utility.Log(string.Format(" Error Response:  {0} " + result.Error));
			else if (!FB.IsLoggedIn)
			{
			}
			else
			{
				btnFB.FindChild("lbID").GetComponent<UILabel>().text =KoStorage.GetKorString("72516");
				btnFB.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnFBLogOut";
				Global.gisFBLogin = 1;
			}
			isLoggin = false;
			//GameObject.Find("LobbyUI").SendMessage("LobbyLoginButtonStatus"); - 추가할것
		});*/
	}
	void OnFBLogOut(){
		if(isLoggin) return;
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<SocialLogOutPopUp>().InitPopUp(0);
		pop.GetComponent<SocialLogOutPopUp>().onFinishCallback(()=>{
			//FB.Logout();
			SNSManager.FaceBookLogOut(()=>{
				btnFB.FindChild("lbID").GetComponent<UILabel>().text = KoStorage.GetKorString("72515");
				btnFB.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnFBLogin";
				Global.gisFBLogin = 0;
			});
		//	GameObject.Find("LobbyUI").SendMessage("FBLobbyAction"); - 추가할것
		//	GameObject.Find("LobbyUI").SendMessage("LobbyLoginButtonStatus"); - 추가할것 
		});

	}
	bool isLoggin = false;
	void OnGgLogin(){
		if(isLoggin) return;
		isLoggin = true;
		SNSManager.GoogleLogin(()=>{
			btnGg.FindChild("lbID").GetComponent<UILabel>().text = KoStorage.GetKorString("72516");
			btnGg.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnGgLogout";
			EncryptedPlayerPrefs.SetString("GoogleLogin","Success");
			GameObject.Find("LobbyUI").SendMessage("LobbyLoginButtonStatus");
			isLoggin = false;
		},()=>{
			Utility.LogWarning("Authentication failed");
			EncryptedPlayerPrefs.SetString("GoogleLogin","Failed");
			GameObject.Find("LobbyUI").SendMessage("LobbyLoginButtonStatus");
			isLoggin = false;
		});
		/*
		Social.localUser.Authenticate((bool success) => {
			if (success) {
				string userInfo = "Username: " + Social.localUser.userName + 
					"\nUser ID: " + Social.localUser.id + 
						"\nIsUnderage: " + Social.localUser.underage;
				Utility.Log (userInfo);
				btnGg.FindChild("lbID").GetComponent<UILabel>().text = KoStorage.GetKorString("72516");
				btnGg.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnGgLogout";
				EncryptedPlayerPrefs.SetString("GoogleLogin","Success");
			} else {
			//	lbStatus.text = "Authentication failed.";
				Utility.LogWarning("Authentication failed");
				EncryptedPlayerPrefs.SetString("GoogleLogin","Failed");
			}
			GameObject.Find("LobbyUI").SendMessage("LobbyLoginButtonStatus");
			isLoggin = false;
		});*/
	}

	void OnGgLogout(){
		if(isLoggin) return;
	
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<SocialLogOutPopUp>().InitPopUp(1);
		pop.GetComponent<SocialLogOutPopUp>().onFinishCallback(()=>{
			SNSManager.GoogleLogOut(()=>{
				btnGg.FindChild("lbID").GetComponent<UILabel>().text = KoStorage.GetKorString("72515");
				btnGg.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnGgLogin";
				//GameObject.Find("LobbyUI").SendMessage("FBLobbyAction");
				GameObject.Find("LobbyUI").SendMessage("LobbyLoginButtonStatus");
				EncryptedPlayerPrefs.DeleteKey("GoogleLogin");
				isLoggin = false;
			});
			//((GooglePlayGames.PlayGamesPlatform) Social.Active).SignOut();
		
		});
	}

	void OnDisable(){

	}

	SettingInteraction[] temp;
	void InitVolume(){
		temp = gameObject.GetComponentsInChildren<SettingInteraction>();// as Transform;
		//temp[0].InitValue(isThird); // agree
		temp[0].InitValue(_opt.isVibration); // vibrate
		temp[1].InitValue(_opt.isBGM); // bgm
		temp[2].InitValue(isAlarm); // alarm
		temp[3].InitValue(_opt.isHighQuality); //quality
		//temp[5].InitValue(_opt.isEffect) ; //effect

		var child = gameObject.transform.FindChild("Set") as Transform;
		var _obj = child.FindChild("lbID_num") as Transform;
	//	string str = ProtocolManager.instance.KakaoUserId;
		_obj.GetComponent<UILabel>().text = GV.UserRevId.ToString();//"Server ID";
	//	_obj = child.FindChild("lbVersion_num");
	//	str = _obj.GetComponent<UILabel>().text;
	//	_obj.GetComponent<UILabel>().text = " : " + Global.gVersion;

		_obj = child.FindChild("lbVersion_old_num");
	//	str = _obj.GetComponent<UILabel>().text;
		_obj.GetComponent<UILabel>().text =Global.gVersion;

	}

	void SaveVolume(){
		_opt.isMusic = isMusic;
		_opt.isEffect = isEffect;
		_opt.isBGM = isBGM;
		_opt.isHighQuality = isQuality;
		_opt.isVibration = isVibration;
		Global.isVibrate = isVibration;
		_opt.isAlarm=isAlarm;
		_opt.isThird = isThird;
		GameOption.OptionData = _opt;
	}


	void OnCheckBGM(bool ischeck){
		isBGM = ischeck;
		isMusic = ischeck;
		isEffect = ischeck;

		float uiVolume = isEffect?0:1.0f;
		UserDataManager.instance.UIVolume = uiVolume;
		GameObject.Find("Audio").GetComponent<LobbySound>().ChangeVolume( isBGM?1:0.0f, isBGM);

	}

	void OnCheckVibrate(bool ischeck){
		isVibration = ischeck;
	}
	
	void OnCheckQuality(bool ischeck){
		isQuality = ischeck;
		QualitySettings.SetQualityLevel(isQuality?7:6, true);
		UserDataManager.instance.isQualitySettingValue = isQuality;
	}

	void OnCheckAlarm(bool ischeck){
		isAlarm = ischeck;
		//
	}
	void OnThirdCheck(bool ischeck){
		isThird = ischeck;
		//
	}

	IEnumerator sendMyInfotoServer(){
		/*Global.isNetwork = true;
		int al = _opt.isAlarm?1:0;
		int bl = _opt.isThird?1:0;
		if(al == Global.gPushable){
		
		}else{
			ProtocolManager.instance.addServerDataField("nGubun","1");
			ProtocolManager.instance.addServerDataField("nValue",al.ToString());
			string strUrl = ServerStringKeys.API.setPushableThirdFuel;
			ProtocolManager.instance.ConnectServer(strUrl,(uri)=>{
				int nRet = ProtocolManager.instance.GetIntUriQuery(uri, "nRet");
				if(nRet == 1){
					Global.gPushable = isAlarm?1:0;
				}
				Global.isNetwork = false;
			});
			while(Global.isNetwork){
				yield return null;
			}
		}

		Global.isNetwork = true;
		if(bl == Global.g3Agree){
			
		}else{
			ProtocolManager.instance.addServerDataField("nGubun","2");
			ProtocolManager.instance.addServerDataField("nValue",bl.ToString());
			string strUrl2 = ServerStringKeys.API.setPushableThirdFuel;
			ProtocolManager.instance.ConnectServer(strUrl2,(uri)=>{
				int nRet = ProtocolManager.instance.GetIntUriQuery(uri, "nRet");
				if(nRet == 1){
					Global.g3Agree = isThird?1:0;
					EncryptedPlayerPrefs.SetInt("ThirdAgree", Global.g3Agree);
				}
				Global.isNetwork = false;
			});
			while(Global.isNetwork){
				yield return null;
			}
		}
		Global.isNetwork = false;
		Invoke("destorywindow", 0.25f);
		*/
		yield return null;
	}
	void OnCloseClick(){
	//	if(Global.isNetwork) return;
	//	if(isLoggin) return;
		SaveVolume();
		gameObject.GetComponent<GameOption>().SaveOptionValue();
	//	Global.isNetwork = false;
		Invoke("destorywindow", 0.25f);

	}
	void OnConfirmClick(){
	//	if(Global.isNetwork) return;
		if(isLoggin) return;
		float uiVolume = isEffect?0:1.0f;
		UserDataManager.instance.UIVolume = uiVolume;
		GameObject.Find("Audio").GetComponent<LobbySound>().ChangeVolume( isBGM?1:0.0f, isBGM);
		SaveVolume();
		QualitySettings.SetQualityLevel(isQuality?7:6, true);
		UserDataManager.instance.isQualitySettingValue = isQuality;
		gameObject.GetComponent<GameOption>().SaveOptionValue();
		temp = null;
		//Utility.Log(_opt.isHighQuality+"--"+ isQuality);
		//StartCoroutine("sendMyInfotoServer");
		Global.isNetwork = false;
	//	Invoke("destorywindow", 0.25f);
	}

	void OnWithDrawClick(){
		//GameObject.Find("FBManager").SendMessage("FBLogin");
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<KakaoPopUp>().InitPopUp();

	//	gameObject.AddComponent<makePopup>().makePopupAction("KakaoPopUp");

	}

	void OnTutorial(){
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<TutorialPopUp>().InitPopUp();

	//	gameObject.AddComponent<makePopup>().makePopupAction("TutorialPopUp");
	}
	
	void OnHomeClick(){
		//web 브라우져
		//	Application.OpenURL("https://www.facebook.com/pitinracing");
		Application.OpenURL(GV.gInfo.HomeURL);

	}

	void OnLogoutClick(){
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<LogOutPopup>().InitPopUp();
		return;
	}





	void SendEmail ()
	{		
		////	#if UNITY_ANDROID && !UNITY_EDITOR
		string email = GV.gInfo.strEmail;
		StringBuilder sb = new StringBuilder();
		string str = KoStorage.GetKorString("72530");
		sb.Append(str);
		sb.Append("\r\n ");
		string subject = EmailURL(sb.ToString());
		sb.Length = 0;
		str = KoStorage.GetKorString("72531");
		sb.Append(str);
		sb.Append("\r\n\n\n\n ");
		str = KoStorage.GetKorString("72532");
		sb.Append(str);
		sb.Append("\r\n ");
		sb.Append(" =========================================\r\n ");
		System.DateTime dateTime =NetworkManager.instance.GetCurrentDeviceTime();// System.DateTime.Now;
		sb.Append("Current Time : ");sb.Append(dateTime.ToString());sb.Append("\r\n");
		sb.Append("Device Model  : ");sb.Append(Global.gDeviceModel);sb.Append("\r\n");
		sb.Append("OS Version : ");sb.Append(Global.gOsVersion);sb.Append("\r\n");
		sb.Append("ServerID : ");sb.Append(GV.UserRevId);sb.Append("\r\n");
		sb.Append("User Country : ");sb.Append(Global.gCountryCode);sb.Append("\r\n");
		string body = EmailURL(sb.ToString());
		//Utility.Log(body);
		Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
		//"mailto:yesyoucan@hotmail.com?subject=Email&body=from Unity&attachment="myhostmyfoldermyfile.png""
		//	#endif
	}

	
	string EmailURL (string url)
	{
		return WWW.EscapeURL(url).Replace("+","%20");
	}


	void OnQnAClick(){
		SendEmail();
	}

	void OnAgreeClick(){
		var temp = ObjectManager.CreatePrefabs(transform.parent,"Window","Agreement");
		ObjectManager.ChangeObjectPosition(temp, new Vector3(0,0,-1100),
		                                   new Vector3(1.0f,1.0f,1.0f), Vector3.zero);
		temp.GetComponent<AgreementAction>();
		temp.AddComponent<TweenAction>().doubleTweenScale(temp);

	}

	void destorywindow(){
		var temp = gameObject.GetComponent<GameOption>() as GameOption;
		temp.DestroyDelay();
		Destroy(temp);
		NGUITools.FindInParents<SubMenuWindow>(gameObject).OnCloseClick();
	}

	void OnCoutry(){
		NGUITools.FindInParents<SubMenuWindow>(gameObject).OnSetCountry();
		gameObject.SetActive(false);

	}

	void OnCredit(){
		NGUITools.FindInParents<SubMenuWindow>(gameObject).OnCredits();
		gameObject.SetActive(false);
		
	}

}


public partial class LobbyManager : MonoBehaviour {
	void LobbyLoginButtonStatus(){
		rankObj.SendMessage("LoginButtonStatus",SendMessageOptions.DontRequireReceiver);
	}
}