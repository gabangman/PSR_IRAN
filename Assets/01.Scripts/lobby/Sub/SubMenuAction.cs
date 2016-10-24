using UnityEngine;
using System.Collections;

public class SubMenuAction : MonoBehaviour {

	System.Action CallFunction, CallFunction1;
	private GameObject aniMenu;
	private Animation winAni;
	void Awake(){
		aniMenu = transform.FindChild("MainBtn").gameObject as GameObject;
		aniMenu.GetComponent<UIButtonMessage>().functionName = "OnSubMenuAni";
		aniMenu.GetComponent<UIButtonMessage>().target = gameObject;
		winAni = gameObject.GetComponent<Animation>() as Animation;
	}
	void Start(){
		aniDir = true;
	}

	bool aniDir;
	void OnSubMenuAni(){
		if(Global.isAnimation) return;
		Global.isAnimation = true;
		Global.isNetwork = true;
		Global.isPopUp = true;
		if(aniDir){
			aniDir = false;
			winAni["SubMenu_inOut"].time = 0;
			winAni["SubMenu_inOut"].speed = 1.0f;
			winAni.Play("SubMenu_inOut");

		}else{
			aniDir = true;
			winAni["SubMenu_inOut"].time =winAni["SubMenu_inOut"].length;
			winAni["SubMenu_inOut"].speed = -1.0f;
			winAni.Play("SubMenu_inOut");
		}
		Invoke("setAniButton",1.0f);
	
	}

	private System.DateTime nTime;
	void setAniButton(){
		Global.isAnimation = false;
		Global.isNetwork = false;
		Global.isPopUp = false;
	}

	IEnumerator setCouponIcon(){
		string url = GV.gInfo.CouponURL;
		url  = System.Uri.EscapeUriString(url);
		WWW www = new WWW( url );
		yield return www;
		
		if( this == null )
			yield break;
		if( www.error != null )
		{
			AccountManager.instance.ErrorPopUp();
		}
		else
		{
			Texture2D Tex = www.texture;
			www.Dispose();
			www = null;
			transform.GetChild(4).FindChild("Texture").GetComponent<UITexture>().mainTexture = Tex;
			//transform.FindChild("gameNotics").FindChild("Texture").gameObject.SetActive(true);
			Tex = null;
		}

	
		}
	public void RankingBoardUpDown(System.Action Call, bool b){
		this.CallFunction = Call;
		if(GV.gInfo.rewardState == 1){
			transform.GetChild(6).gameObject.SetActive(b);
		}else{
			transform.GetChild(6).gameObject.SetActive(false);
		}

		if(GV.gInfo.movieAdState == 1){
			transform.GetChild(8).gameObject.SetActive(b);
		}else{
			transform.GetChild(8).gameObject.SetActive(false);
		}
	
		if(Application.platform == RuntimePlatform.Android){
			if(GV.gInfo.CouponState == 0 || GV.gInfo.CouponState == 1){
				transform.GetChild(7).gameObject.SetActive(false);
			}else 	transform.GetChild(7).gameObject.SetActive(b);
		
		}else if(Application.platform == RuntimePlatform.IPhonePlayer){
			if(GV.gInfo.CouponState == 0 || GV.gInfo.CouponState == 2){
				transform.GetChild(7).gameObject.SetActive(false);
			}else 	transform.GetChild(7).gameObject.SetActive(b);
		}else{
			if(GV.gInfo.CouponState == 0 || GV.gInfo.CouponState == 1){
				transform.GetChild(7).gameObject.SetActive(false);
			}else transform.GetChild(7).gameObject.SetActive(b);
		}
	}

	GameObject InviteObj;
	void OnEnable(){
		MesseagBoxNewCheck();
		InviteObj = null;
	//	ADBoxIconCheck();
	//	InvokeRepeating("ADBoxIconCheck", 5f, 5f);
	}

	void OnDisable(){
		//CancelInvoke("ADBoxIconCheck");

	}
	/*
	void ADBoxIconCheck(){
			System.DateTime cTime = NetworkManager.instance.GetCurrentDeviceTime();
			System.DateTime hTime = new System.DateTime(myAcc.instance.account.lastRewardViewTimes);
			System.TimeSpan sTime = cTime - hTime;
			bool b = false;
			if(sTime.TotalMinutes < 2){
				b = false;
			}else{
				b = true;
				CancelInvoke("ADBoxIconCheck");
			}
		transform.FindChild("ButtonAD_Reward").FindChild("Onair").gameObject.SetActive(b);
	}*/
	
	public void MesseagBoxNewCheck(){
		var temp = transform.FindChild("ButtonPost").FindChild("icon_New").gameObject as GameObject;
		bool isNew = true;
		if(Global.gNewMsg <= 0){
			Global.gNewMsg = 0;
			isNew = false;
		}
		temp.SetActive(isNew);
		temp =  transform.FindChild("BtnAchieve").FindChild("icon_New").gameObject;
		bool isNew1 = GV.bachieveRewardFlag;
		temp.SetActive(isNew1);

		bool isNew2 = true;
		nTime = new System.DateTime(myAccount.instance.account.attendevent.LastTime);
//		Utility.LogWarning(" c " + nTime);
		System.TimeSpan sT = nTime - NetworkManager.instance.GetCurrentDeviceTime();
		if(sT.TotalSeconds <= 0){
			isNew2 = true;
		}else isNew2 = false;
		temp = transform.FindChild("ButtonAttend").FindChild("icon_New").gameObject;
		temp.SetActive(isNew2);
		var tr = aniMenu.transform.FindChild("New").gameObject as GameObject;
		if(isNew || isNew1 || isNew2) {
			tr.SetActive(true);
		}else{
			tr.SetActive(false);
		}
	}



	GameObject CreateSubWindow(string str, GameObject obj){
		Global.bLobbyBack = true;
		var temp = ObjectManager.CreateTagPrefabs("SubWindow") as GameObject;
		if(temp != null) {
			UserDataManager.instance.OnSubBack = ()=>{
				temp.GetComponent<SubMenuWindow>().OnCloseClick();
			};
			return temp;
		}
		UserDataManager.instance.OnSubBack = ()=>{
			temp.GetComponent<SubMenuWindow>().OnCloseClick();
		};
		var Parent = GameObject.FindGameObjectWithTag("CenterAnchor") as GameObject;
		temp = ObjectManager.CreateLobbyPrefabs(Parent.transform.GetChild(0), "Window", "SubMenuWindow", "SubWindow") as GameObject;
		ObjectManager.ChangeObjectPosition(temp, new Vector3(0,0,-1000), Vector3.one, Vector3.zero);
		var scale = temp.GetComponent<TweenScale>() as TweenScale;
		scale.Reset();
		scale.enabled = true;
		scale.onFinished = null;
		return temp;
	}
	bool isFBlogin = false;
	void FBStatusProcess(int nStatus){
		if(nStatus == 0) isFBlogin = true;
		else isFBlogin = false;
	}

	/*
	void OnCrossShockAD(){
		if(Global.isPopUp) return;
		if(isFBlogin) return;
		//CrossShock
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown = ()=>{
			CallFunction();
		};
		submenuObj.transform.FindChild("CrossShock").gameObject.SetActive(true);
		//	submenuObj.GetComponent<SubMenuWindow>().CrossShockWindow();
		
	}*/


	/*
	void OnThirdAgree(GameObject obj){
		//if(isFBlogin) return;
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		var agree = submenuObj.transform.FindChild("ThirdAgree") as Transform;
		agree.gameObject.SetActive(true);
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			
			GameObject.Find("LobbyUI").SendMessage("thirdAgreeFail");
		};
		agree.GetComponent<ThirdCheckAction>().ThirdAgree(()=>{
			//obj.GetComponent<RankGridAction>().InitRankBoard();
		});
	}*/

	void OnWRankWindow(){
		if(Global.isPopUp) return;
		if(isFBlogin) return;
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
		};
		
		submenuObj.transform.FindChild("WorldRanking").gameObject.SetActive(true);
		//submenuObj.GetComponent<SubMenuWindow>().WorldRankInit();
	}

	void OnRankWindows(string rankMode){
		//Utility.LogWarning("ONRANK" +  rankMode);
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
		};
		if(rankMode == "weekly"){
			submenuObj.transform.FindChild("WeeklyRanking").gameObject.SetActive(true);
		}else{ //world
			submenuObj.transform.FindChild("WorldRanking").gameObject.SetActive(true);

		}
		submenuObj.GetComponent<SubMenuWindow>().RankInfoInit(rankMode);
	}


	void OnFriendsWindowStart(){
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
		};
		submenuObj.transform.FindChild("FriendsRanking").gameObject.SetActive(true);
		submenuObj.GetComponent<SubMenuWindow>().FriendsRankInfoInit();
	}

	void FacebookComplete(){
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.transform.FindChild("FriendsRanking").GetComponent<friendrankwin>().FBLoginComplete();

	}

	protected void OnAttendWindow(){
		if(Global.isPopUp) return;
		if(isFBlogin) return;
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
		};
		submenuObj.GetComponent<SubMenuWindow>().AttendEventInit();
		submenuObj.transform.FindChild("AttendEvent").gameObject.SetActive(true);
		
	}


	/*
	void OnAttendWindow(){
		if(Global.isPopUp) return;
		if(isFBlogin) return;
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
		};
	//	if(AccountManager.instance.webPatchVersion[7] == 0){//1
	//		submenuObj.GetComponent<SubMenuWindow>().attendReadyPopUp();
		//	return;
	//	}
		submenuObj.GetComponent<SubMenuWindow>().AttendEventInit();
		submenuObj.transform.FindChild("AttendEvent").gameObject.SetActive(true);
	} */

	void OnRankWindow(){
		if(Global.isPopUp) return;
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
			GameObject.Find("LobbyUI").SendMessage("OnRankClose", SendMessageOptions.DontRequireReceiver);
		};
		
		//submenuObj.GetComponent<SubMenuWindow>().AttendInit();
		submenuObj.transform.FindChild("Open_Ranking").gameObject.SetActive(true);
		GameObject.Find ("Audio").SendMessage("LevelUpSound");
	}

	void OpenContWindow(string str){
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
			GameObject.Find("LobbyUI").SendMessage("OnOpenContentClose", SendMessageOptions.DontRequireReceiver);
		};
		var temp = submenuObj.transform.FindChild("OpenCentents").gameObject as GameObject;
		temp.SetActive(true);
		temp.GetComponent<OpenContents>().OnOpenWindow(str);
		GameObject.Find ("Audio").SendMessage("LevelUpSound");
	}

	void OpenContentLucky(){
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
			GameObject.Find("LobbyUI").SendMessage("OnOpenContentClose", SendMessageOptions.DontRequireReceiver);
		};
		var temp = submenuObj.transform.FindChild("OpenCentents").gameObject as GameObject;
		temp.SetActive(true);
		temp.GetComponent<OpenContents>().OnOpenLuckyWindow();
		GameObject.Find ("Audio").SendMessage("LevelUpSound");
	}

	void OpenDealerRecommend(){
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
			GameObject.Find("LobbyUI").SendMessage("OnOpenContentClose", SendMessageOptions.DontRequireReceiver);
		};
		var temp = submenuObj.transform.FindChild("DealerRecommend").gameObject as GameObject;
		temp.SetActive(true);
		temp.GetComponent<DealerRecomm>().OnDealerInit();
		GameObject.Find ("Audio").SendMessage("LevelUpSound");
	}

	void OpenADWin(){
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
			GameObject.Find("LobbyUI").SendMessage("OnOpenContentClose", SendMessageOptions.DontRequireReceiver);
		};
		var temp = submenuObj.transform.FindChild("AD_Reward").gameObject as GameObject;
		temp.SetActive(true);
		temp.GetComponent<ADRewardContent>().SetSlotInitialize();

	}
	
	void OnWeeklyWindowEnd(){
		//Utility.Log("OnWeeklyWindowClick");
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
		};
		submenuObj.GetComponent<SubMenuWindow>().WeeklyInit();
		submenuObj.transform.FindChild("WeeklyFinish").gameObject.SetActive(true);
	}


	void OnWeeklyWindowStart(){
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
		};
		submenuObj.GetComponent<SubMenuWindow>().WeeklyStartInit();
		submenuObj.transform.FindChild("WeeklyStart").gameObject.SetActive(true);
	}


	void OnDailyFinish(){
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
		};
		submenuObj.GetComponent<SubMenuWindow>().OnDailyFinish();
		submenuObj.transform.FindChild("DailyFinish").gameObject.SetActive(true);
	}

	void OnSeasonUpWindow(){
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;

		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
		};
		submenuObj.GetComponent<SubMenuWindow>().SeasonUpInit();
		submenuObj.transform.FindChild("SeasonClear").gameObject.SetActive(true);
		GameObject.Find ("Audio").SendMessage("SeasonUpSound");
	}


	void OnLevelUpWindow(bool b){
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			MesseagBoxNewCheck();
			CallFunction();
		};
		submenuObj.GetComponent<SubMenuWindow>().LevelUpInit();
		submenuObj.transform.FindChild("LevelUp").gameObject.SetActive(true);
		GameObject.Find ("Audio").SendMessage("LevelUpSound");
		//StartCoroutine("OnSeasonUp", b);
		return;
	}
	
	void OnNoticsWindow(string page){
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		UserDataManager.instance.OnSubBack = null;
		submenuObj.GetComponent<TweenScale>().enabled  = false;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
		};
		submenuObj.GetComponent<SubMenuWindow>().StartCoroutine("OnNoticesInit",page);
		submenuObj.GetComponent<SubMenuWindow>().NoticeCloseAction();
		var noitcs = submenuObj.transform.FindChild("gameNotics") as Transform;
		noitcs.gameObject.SetActive(true);
		noitcs.FindChild("Texture").gameObject.SetActive(false);
		noitcs.FindChild("checkX").gameObject.SetActive(false);
		noitcs.FindChild("checkX").GetComponent<UIButtonMessage>().functionName = "OnEventPopCheck";
		UserDataManager.instance.OnSubBack = ()=>{
			submenuObj.SendMessage("OnEventPopCheck");
		};
	
		noitcs.FindChild("Btn_Home").FindChild("btn").GetComponent<UIButtonMessage>().target =gameObject;
		noitcs.FindChild("Btn_Home").FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnOpenHome";
		//submenuObj.transform.FindChild("gameNotics").FindChild("Texture").GetComponent<UIButtonMessage>().functionName ="OnUrlClick";
		noitcs.FindChild("Btn_Home").FindChild("lbID").GetComponent<UILabel>().text = KoStorage.GetKorString("72507");
		string gCountry =  EncryptedPlayerPrefs.GetString("CountryCode");
		if(string.Equals(gCountry, "KOR")==true){
			noitcs.FindChild("Btn_Naver").gameObject.SetActive(true);
			noitcs.FindChild("Btn_Naver").FindChild("btn").GetComponent<UIButtonMessage>().target =gameObject;
			noitcs.FindChild("Btn_Naver").FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnOpenNaver";
		}
	}

	void OnOpenHome(){
		Application.OpenURL(GV.gInfo.HomeURL);
	}
	void OnOpenNaver(){
			Application.OpenURL("http://cafe.naver.com/psracing.cafe");
	}

	void OnPlusEventWindow(string page){
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<TweenScale>().enabled  = false;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
		};
		submenuObj.GetComponent<SubMenuWindow>().StartCoroutine("OnNoticesInit",page);
		submenuObj.GetComponent<SubMenuWindow>().NoticeCloseAction();
		var tr = submenuObj.transform.FindChild("gameNotics") as Transform;
		tr.gameObject.SetActive(true);
		tr.FindChild("Texture").gameObject.SetActive(false);
		tr.FindChild("checkX").gameObject.SetActive(false);
		tr.FindChild("checkX").GetComponent<UIButtonMessage>()
			.functionName = "OnCloseClick";
		tr.FindChild("Check_day").gameObject.SetActive(false);
	}

	// - 1
	protected void OnAchieveClick(){
		if(Global.isPopUp) return;
		if(isFBlogin) return;
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
			MesseagBoxNewCheck();
		};
		submenuObj.transform.FindChild("Achievement").gameObject.SetActive(true);
		submenuObj.GetComponent<SubMenuWindow>().AchievementInit();
	}
	// -2
	protected void OnPostClick(){
		if(Global.isPopUp) return;
		if(isFBlogin) return;
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown = ()=>{
			CallFunction();
			MesseagBoxNewCheck();
		};
		InviteObj= submenuObj.transform.FindChild("Post").gameObject as GameObject;
		InviteObj.SetActive(true);
	
			int child = InviteObj.transform.FindChild("ListView").FindChild("View").GetChild(0).childCount;
			if(child == 0) 	InviteObj.GetComponent<GiftWindow>().InitializeGiftBox();
			else InviteObj.GetComponent<GiftWindow>().reFreshMsginit();
	}

	// -3
	protected void OnAttendClick(){
		if(Global.isPopUp) return;
		if(isFBlogin) return;
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
			MesseagBoxNewCheck();
		};
		/*
		if(AccountManager.instance.webPatchVersion[7] == 0){ //1
			submenuObj.GetComponent<SubMenuWindow>().attendReadyPopUp();
			return;
		}*/
		submenuObj.GetComponent<SubMenuWindow>().ShowAttend();
		submenuObj.transform.FindChild("AttendEvent").gameObject.SetActive(true);


	}
	// -4
	protected 	void OnInviteClick(){
		if(Global.isPopUp) return;
		if(isFBlogin) return;
		CallFunction();
		if(Global.gDefaultIcon == null){
			Global.gDefaultIcon = (Texture)Resources.Load("Kakao_PortraitSample", typeof(Texture));
		}
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
		};
		
		InviteObj = submenuObj.transform.FindChild("Invite_G").gameObject as GameObject;
		InviteObj.SetActive(true);
		//int child = InviteObj.transform.FindChild("ListView").FindChild("View").GetChild(0).childCount;
		//if(child == 0) {
		InviteObj.GetComponent<InviteGMenu>().Initialize();
		//}

	}
	// -5
	protected void OnOptionClick(){
		if(Global.isPopUp) return;
		if(isFBlogin) return;
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
		};
		submenuObj.transform.FindChild("Option").gameObject.SetActive(true);
	}
	// -	6
	protected 	void OnADClick(){
		/*	if(Global.isPopUp) return;
		if(isFBlogin) return;
		//CrossShock
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown = ()=>{
			CallFunction();
		};
		submenuObj.transform.FindChild("CrossShock").gameObject.SetActive(true);
		//	submenuObj.GetComponent<SubMenuWindow>().CrossShockWindow();
		*/	
		if(Global.isPopUp) return;
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		//submenuObj.transform.FindChild("Contents_menuBG").gameObject.SetActive(false);
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
	
		};
		var temp = submenuObj.transform.FindChild("AD_Reward").gameObject as GameObject;
		//temp.SetActive(true);
		temp.GetComponent<ADRewardContent>().TapJoyStart(()=>{
			submenuObj.GetComponent<SubMenuWindow>().OnCloseClick();
		});
	}

	// -	7
	protected void OnCouponClick(){
		if(Global.isPopUp) return;
		if(isFBlogin) return;
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown = ()=>{
			CallFunction();
			MesseagBoxNewCheck();
			GameObject.Find("LobbyUI").SendMessage("TempRepairLabel", true, SendMessageOptions.DontRequireReceiver);
		};
		submenuObj.transform.FindChild("CouponEvent").gameObject.SetActive(true);
		submenuObj.GetComponent<SubMenuWindow>().CouponWindow();
		GameObject.Find("LobbyUI").SendMessage("TempRepairLabel", false, SendMessageOptions.DontRequireReceiver);
		
	}
	// -	8
	protected void OnADMovie(){
		if(Global.isPopUp) return;
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			//ADBoxIconCheck();
			CallFunction();
			transform.FindChild("ButtonAD_Reward").gameObject.SetActive(true);
		};

		var temp = submenuObj.transform.FindChild("AD_Reward").gameObject as GameObject;
		temp.SetActive(true);
		temp.GetComponent<ADRewardContent>().SetSlotInitialize();
		transform.FindChild("ButtonAD_Reward").gameObject.SetActive(false);

	}
	
	// not use
	protected void OnFBRankClick(){
	Utility.LogError("not usel!! ");
		if(Global.isPopUp) return;
		if(isFBlogin) return;
		OnWRankWindow();
	}

	void OnInviteEvenSubMenuAction(){
		if(InviteObj == null){
			Utility.LogError("Invite Object is Null ");
		}else{
			InviteObj.GetComponent<inviteMenu>().InviteEvent();
		}
	}

	void OpenSpendRewardSub(int balance){
		if(Global.isPopUp) return;
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			GameObject.Find("LobbyUI").SendMessage("TapJoyRewardComplete");
			CallFunction();
		};
		var temp = submenuObj.transform.FindChild("AD_Tapjoy").gameObject as GameObject;
		temp.SetActive(true);
		temp.GetComponent<AdTapjoyReward>().SetTapJoyReward(balance);
		GameObject.Find ("Audio").SendMessage("SeasonUpSound");
	}


	void OpenTapJoyReward(){
		if(Global.isPopUp) return;
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			GameObject.Find("LobbyUI").SendMessage("TapJoyRewardComplete");
			CallFunction();
		};
		var temp = submenuObj.transform.FindChild("AD_Tapjoy").gameObject as GameObject;
		temp.SetActive(true);
		temp.GetComponent<AdTapjoyReward>().SetCheckTapJoyReward();
		//GameObject.Find ("Audio").SendMessage("SeasonUpSound");
	}

	void OnAchieveEnable(){
	//	MesseagBoxNewCheck();
		var temp =  transform.FindChild("BtnAchieve").FindChild("icon_New").gameObject;
		bool isNew1 = GV.bachieveRewardFlag;
		temp.SetActive(isNew1);
	}

	void OnHelpWindow(){
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			GameObject.Find("LobbyUI").SendMessage("PopUpEnd");
			CallFunction();
		};
		var temp = submenuObj.transform.FindChild("Help_1").gameObject as GameObject;
		temp.SetActive(true);
		temp.GetComponent<HelpWindow>().SetInit();
	}

	void OnCrossSubWindow(){
		CallFunction();
		var submenuObj = CreateSubWindow("SubMenuWindow", null) as GameObject;
		submenuObj.GetComponent<SubMenuWindow>()._RankDown =()=>{
			CallFunction();
			Global.isPopUp = false;
		};
		var temp = submenuObj.transform.FindChild("CrossAds").gameObject as GameObject;
		temp.SetActive(true);
		temp.GetComponent<CrossADSub>().SetInit();
	}

	void OnGoogleRank(){
		SNSManager.OnRankGoogleClick2();
	}

	void OnGoogleAchieve(){
		SNSManager.OnAchieveGoogleClick2();

	}
}
