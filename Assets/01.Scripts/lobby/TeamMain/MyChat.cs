
using UnityEngine;
using System.Collections;

public class MyChat : MonoBehaviour {
	public GameObject GlobalChat,ClanChat,ClanWarChat;
	public GameObject GlobalObj,CChatObj, CChatWarObj;
	public GameObject JiverUI, JiverUIOther;
	public UILabel[] lbText;
	private bool bStart;
	private Transform subTr;
	void Awake()
	{
		GlobalChat.GetComponent<UISprite>().color = Color.white;
		ClanChat.GetComponent<UISprite>().color = Color.gray;
		bStart = false;

		string gCountry =  EncryptedPlayerPrefs.GetString("CountryCode");
		if(string.Equals(gCountry, "KOR")==true){
			GlobalObj.transform.FindChild("Btn_Naver").gameObject.SetActive(true);
		}
		subTr = transform.FindChild("ChatwindowSub") as Transform;

	}

	private int chatIndex=0;
	void OnYouTube(){
		Application.OpenURL(GV.gInfo.bundleURL_1);
	}
	void OnYouTubeDrag(){
		Application.OpenURL(GV.gInfo.bundleURL_2);
	}
	void OnHomePage(){

		Application.OpenURL(GV.gInfo.HomeURL);

	}
	void OnNaverPage(){
		Application.OpenURL("http://cafe.naver.com/psracing.cafe");
	}


	public void setInit(){
		if(subTr == null) return;
		if(lbText[0] != null) {
			lbText[0].text = KoStorage.GetKorString("72012");
			lbText[1].text = KoStorage.GetKorString("72531");// "Global???????";
			lbText[2].text = KoStorage.GetKorString("73201"); // you tube
			lbText[3].text =  KoStorage.GetKorString("72507");
			lbText[4].text =  KoStorage.GetKorString("73202"); 
			lbText[5].text =  KoStorage.GetKorString("72531"); 
			lbText[6].text =  KoStorage.GetKorString("72012"); 
			SetClanChat();
			lbText[0] = null;
		}
	
		subTr.gameObject.SetActive(true);
		OnInitChat();
		UserDataManager.instance.ChatEnable = 2;
		JiverUI.SendMessage("OnStartChat",SendMessageOptions.DontRequireReceiver);
	}

	public void unSetInit(){
		if(subTr == null) return;
		UserDataManager.instance.ChatEnable = 1;
		JiverUI.SendMessage("OnEndChat",SendMessageOptions.DontRequireReceiver);
		subTr.gameObject.SetActive(false);
	

	}
	void OnInitChat(){
		if(Global.bLobbyBack) {
				UserDataManager.instance.OnSubBack = ()=>{
				OnClose();
			};
		}
		if(CClub.ClubMode == 1){
			ClanWarChat.transform.parent.gameObject.SetActive(true);
			if(!CClub.bClanWarChat){
				//ClanWarChat.transform.parent.gameObject.SetActive(true);
				ClanWarChat.GetComponent<UISprite>().color = Color.gray;

			}else{
				ClanWarChat.GetComponent<UISprite>().color = Color.white;
				CChatObj.SetActive(false);
				GlobalObj.SetActive(false);
				CChatWarObj.SetActive(true);
			}
			ClanChat.GetComponent<UIButtonMessage>().functionName = "OnClanChat1";
			GlobalChat.GetComponent<UIButtonMessage>().functionName = "OnGlobalChat1";
			JiverUI.SendMessage("OnVisibleText1",SendMessageOptions.DontRequireReceiver);
		
		}else{
			ClanWarChat.transform.parent.gameObject.SetActive(false);
			if(CChatWarObj.activeSelf){
				CChatObj.SetActive(true);
				GlobalObj.SetActive(false);
				CChatWarObj.SetActive(false);
				CClub.bClanWarChat = false;
				ClanChat.GetComponent<UISprite>().color = Color.white;
				UserDataManager.instance.OnChangeChannel(false);
				JiverUI.SendMessage("OnVisibleText1",SendMessageOptions.DontRequireReceiver);
			}else{
				JiverUI.SendMessage("OnVisibleText",SendMessageOptions.DontRequireReceiver);
			}
		}
		if(CClub.ChangeClub == 0) return;
			JiverUI.SendMessage("OnResetChat",SendMessageOptions.DontRequireReceiver);
			CClub.ChangeClub = 0;
			LockObj = subTr.transform.FindChild("Locked").gameObject;
			 if(CClub.mClubFlag == 1){
				CChatObj.SetActive(false);
				LockObj.SetActive(false);
				//LockObj.transform.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("73522");// "클럽 가입 안하?";
				ClanChat.GetComponent<UIButtonMessage>().functionName = "OnClan1";
				GlobalChat.GetComponent<UIButtonMessage>().functionName = "OnGlobal1";
				GlobalChat.GetComponent<UISprite>().color = Color.gray;
				ClanChat.GetComponent<UISprite>().color = Color.gray;
				OnGlobal1();
				
			}else if(CClub.mClubFlag == 2){
				CChatObj.SetActive(true);
				LockObj.SetActive(false);
				if(CClub.ClubMode == 1){
					ClanWarChat.transform.parent.gameObject.SetActive(true);
					ClanChat.GetComponent<UIButtonMessage>().functionName = "OnClanChat1";
					GlobalChat.GetComponent<UIButtonMessage>().functionName = "OnGlobalChat1";
					ClanWarChat.GetComponent<UISprite>().color = Color.gray;
					GlobalChat.GetComponent<UISprite>().color = Color.gray;
					ClanChat.GetComponent<UISprite>().color = Color.gray;
					OnGlobalChat1();
			
				}else{
					ClanWarChat.transform.parent.gameObject.SetActive(false);
					ClanChat.GetComponent<UIButtonMessage>().functionName = "OnClanChat";
					GlobalChat.GetComponent<UIButtonMessage>().functionName = "OnGlobalChat";
					GlobalChat.GetComponent<UISprite>().color = Color.gray;
					ClanChat.GetComponent<UISprite>().color = Color.gray;
					OnGlobalChat();
				}

			}

	}
	private GameObject LockObj;
	void Start(){
	
		bStart = true;
	/*	if(CClub.mClubFlag == 2){
		
		}else{
		
		}*/
	}
	
	void SetClanChat(){
		LockObj = subTr.transform.FindChild("Locked").gameObject;
		LockObj.transform.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("73522");
		if(CClub.mClubFlag == 0){// 0 : no season, 1 : seaon ok but no clanmember, 2 : season OK & clanmember
			//LockObj.SetActive(false);
			ClanChat.GetComponent<UIButtonMessage>().functionName = "OnClan1";
			GlobalChat.GetComponent<UIButtonMessage>().functionName = "OnGlobal1";
			CChatObj.SetActive(false);
			GlobalObj.SetActive(true);
		}else if(CClub.mClubFlag == 1){
			//LockObj.SetActive(false);
			ClanChat.GetComponent<UIButtonMessage>().functionName = "OnClan1";
			GlobalChat.GetComponent<UIButtonMessage>().functionName = "OnGlobal1";
			CChatObj.SetActive(false);
			GlobalObj.SetActive(true);
		}else if(CClub.mClubFlag == 2){
			//LockObj.SetActive(false);
			ClanChat.GetComponent<UIButtonMessage>().functionName = "OnClanChat";
			GlobalChat.GetComponent<UIButtonMessage>().functionName = "OnGlobalChat";
			CChatObj.SetActive(true);
			GlobalObj.SetActive(false);
			ClanChat.GetComponent<UISprite>().color = Color.white;
			GlobalChat.GetComponent<UISprite>().color = Color.gray;

			//lbText[3].textd =KoStorage.GetKorString("73522");// "Club???????????????????";//
		}
		//Utility.LogWarning("CClub.mClubFlag = " + CClub.mClubFlag);
	}
	
	void OnGlobal1(){
		if(GlobalChat.GetComponent<UISprite>().color == Color.white) return;
		GlobalChat.GetComponent<UISprite>().color = Color.white;
		ClanChat.GetComponent<UISprite>().color = Color.gray;
		CChatObj.SetActive(false);
		LockObj.SetActive(false);
		GlobalObj.SetActive(true);
	}
	
	void OnClan1(){
		if(ClanChat.GetComponent<UISprite>().color == Color.white) return;
		ClanChat.GetComponent<UISprite>().color = Color.white;
		GlobalChat.GetComponent<UISprite>().color = Color.gray;
		CChatObj.SetActive(false);
		LockObj.SetActive(true);
		GlobalObj.SetActive(false);
	}
	

	
	void OnClose(){
		if(Global.isNetwork) return;
		Global.isPopUp = false;
		GetComponent<TweenAction>().ReverseTween(gameObject);
		//gameObject.SetActive(false);
		unSetInit();
		Global.bLobbyBack = false;
		
	}
	private bool bWait = false;
	void OnGlobalChat(){
		if(GlobalChat.GetComponent<UISprite>().color == Color.white) return;
		GlobalChat.GetComponent<UISprite>().color = Color.white;
		ClanChat.GetComponent<UISprite>().color = Color.gray;
		CChatObj.SetActive(false);
		GlobalObj.SetActive(true);
	}


	void OnClanChat(){
		if(ClanChat.GetComponent<UISprite>().color == Color.white) return;
		ClanChat.GetComponent<UISprite>().color = Color.white;
		GlobalChat.GetComponent<UISprite>().color = Color.gray;
		CChatObj.SetActive(true);
		GlobalObj.SetActive(false);
		JiverUI.SendMessage("OnVisibleText",SendMessageOptions.DontRequireReceiver);
	}

	void OnGlobalChat1(){
		if(GlobalChat.GetComponent<UISprite>().color == Color.white) return;
		GlobalChat.GetComponent<UISprite>().color = Color.white;
		ClanChat.GetComponent<UISprite>().color = Color.gray;
		CChatObj.SetActive(false);
		GlobalObj.SetActive(true);
		CChatWarObj.SetActive(false);
		ClanWarChat.GetComponent<UISprite>().color = Color.gray;
		CClub.bClanWarChat = false;
	}


	void OnClanChat1(){
		if(ClanChat.GetComponent<UISprite>().color == Color.white) return;
		ClanChat.GetComponent<UISprite>().color = Color.white;
		GlobalChat.GetComponent<UISprite>().color = Color.gray;
		CChatObj.SetActive(true);
		GlobalObj.SetActive(false);
		CChatWarObj.SetActive(false);
		ClanWarChat.GetComponent<UISprite>().color = Color.gray;
		CClub.bClanWarChat = false;
		UserDataManager.instance.OnChangeChannel(false);
		JiverUI.SendMessage("OnVisibleText1",SendMessageOptions.DontRequireReceiver);
	}
	

	void OnClanWar(){
		if(ClanWarChat.GetComponent<UISprite>().color == Color.white) return;
		ClanChat.GetComponent<UISprite>().color = Color.gray;
		GlobalChat.GetComponent<UISprite>().color = Color.gray;
		ClanWarChat.GetComponent<UISprite>().color = Color.white;
		CChatObj.SetActive(false);
		GlobalObj.SetActive(false);
		CChatWarObj.SetActive(true);
		CClub.bClanWarChat = true;
		UserDataManager.instance.OnChangeChannel(true);
		JiverUI.SendMessage("OnVisibleText1",SendMessageOptions.DontRequireReceiver);
	}
	
	void OnChangeCH(){
		
		
		
	}
	void OnDisable(){
//		#if UNITY_IOS || UNITY_ANDROID
//		#endif
	}
	

	
	void OnDestroy(){
//		#if UNITY_IOS || UNITY_ANDROID
//		#endif
	}
	
}
