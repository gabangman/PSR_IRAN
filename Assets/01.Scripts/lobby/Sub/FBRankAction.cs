using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FBRankAction : MonoBehaviour {
	
//	public GameObject WorldSNS, FriendSNS;
//	public GameObject BtnTotalSNS, BtnFriendSNS, BtnDurability;
//	public GameObject wback, fback;
	public GameObject SubObj, QuestObj;
//	public GameObject readyBG;
//	public GameObject btnFBLogin, SNSObj;
//	public UILabel[] _Text;
	bool isActived = false;
	Color32 _bgColorDisable,_bgColorEnable;
	UIDraggablePanel _uidrag;
	UIDraggablePanel2 dragWorld, dragFriend;
	delegate void ChangeRankBoard();
	ChangeRankBoard _changeboard;
	public UILabel lbStatus,lbLogin;

	void Awake(){
	//	_bgColorDisable = BtnFriendSNS.GetComponent<UISprite>().color;
	//	_bgColorEnable = BtnTotalSNS.GetComponent<UISprite>().color;
		SubObj.GetComponent<SubMenuAction>().RankingBoardUpDown(checkRank, false);
		//worldSNS, FriendSNS 는 실질적으로 반대로 되어 있으니 확인 할것!!!!!
	//	BtnFriendSNS.GetComponent<UIButtonMessage>().functionName = "OnFirstClick";
	//	BtnTotalSNS.GetComponent<UIButtonMessage>().functionName = "OnFirstClick";
	//	readyBG.SetActive(false);
	//	transform.FindChild("Window_BG").FindChild("icon_refresh").GetComponent<UIButtonMessage>().functionName = "OnRefresh";
	//	iconRefresh = transform.FindChild("Window_BG").FindChild("icon_refresh").gameObject as GameObject;

	}
//	private GameObject iconRefresh;
//	private float cTime = 0.0f;
//	public Transform timeArrow;
//	private readonly float secondsToDegrees = 360f / 60f;
	void FixedUpdate () {
		//cTime += Time.deltaTime*10;
		//timeArrow.localRotation = Quaternion.Euler(0f, 0f, cTime * -secondsToDegrees);
		//_Text[3].text = UserDataManager.instance.dailyTimes();
	}

	void SetDurabilityArrow(bool b){
		//BtnDurability.transform.FindChild("img_arrow").gameObject.SetActive(b);
	}


	void SetFriendItems(){
	//	SNSObj.SetActive(false);
	//	WorldSNS.SetActive(true);
	//	wback.SetActive(true);
		AddFBItem();
	}

	void SetInviteFriendItems(){
	//	SNSObj.SetActive(true);
	//	WorldSNS.SetActive(false);
	//	wback.SetActive(false);
	//	SNSObj.transform.FindChild("Facebook").gameObject.SetActive(false);
	//	var tr = SNSObj.transform.FindChild("NoFriend") as Transform;
	//	tr.gameObject.SetActive(true);
	//	tr.FindChild("Btn").GetComponent<UIButtonMessage>().functionName = "OnInviteWindow";
	//	tr.FindChild("lbText_1").GetComponent<UILabel>().text  =KoStorage.GetKorString("72819");
	}

	void UnSetInviteFriendItems(){
	//	lbLogin.text =  KoStorage.GetKorString("72515");
	//	SNSObj.SetActive(true);
	//	SNSObj.transform.FindChild("Facebook").gameObject.SetActive(true);
	//	btnFBLogin.SetActive(true);
	//	WorldSNS.SetActive(false);
	//	wback.SetActive(false);
		StopFrinedLoadingCircle();
	}

	void OnRefresh(){
		if(Global.isNetwork) return;
		if(FB.IsLoggedIn){
			Global.isNetwork = true;
			Global.isPopUp = true;
			gameRank.instance.listFFR.Clear();
			StartCoroutine("FriendLoadingCircle");
			StartCoroutine("reTryGetFacebookFriendsRanking");
		}else{
		
		}
	}

	void InitLoginStatus(){
		return;
		if(FB.IsLoggedIn){
			Global.isPopUp = true;
			Global.isNetwork = true;
			Utility.LogWarning("FB.ISLOGIN");
			StartCoroutine("FriendLoadingCircle");
			lbLogin.text =  KoStorage.GetKorString("72516");
			int count = gameRank.instance.listFFR.Count;
			if(count <= 0){
			
			}else{
				SetFriendItems();
				GameObject.Find("LobbyUI").SendMessage("SetAchievementIncrease",SendMessageOptions.DontRequireReceiver);
			}
		//	iconRefresh.SetActive(true);
		}else{
			Utility.LogWarning("FB.ISLOGOUT");
			UnSetInviteFriendItems();
		//	iconRefresh.SetActive(false);
		}
	}

	IEnumerator reTryGetFacebookFriendsRanking(){
		if(GV.gInfo.extra01 == 1){
			yield return AccountManager.instance.StartCoroutine("GetFaceBookMyRank");
			yield return AccountManager.instance.StartCoroutine("GetFacebookFriendsRankList");
			yield return AccountManager.instance.StartCoroutine("GetFacebookFriendsScoreRankList");
			gameRank.instance.listFFR.Sort(delegate(gameRank.RaceRankInfo x, gameRank.RaceRankInfo y) {
				return y.level.CompareTo(x.level);
			});

			Invoke("SetFriendItems", 0.2f);
			//GameObject.Find("LobbyUI").SendMessage("SetAchievementIncrease",SendMessageOptions.DontRequireReceiver);
		}else{
			gameRank.instance.myFFRList();
			SetFriendItems();
			GameObject.Find("LobbyUI").SendMessage("SetAchievementIncrease",SendMessageOptions.DontRequireReceiver);
		}
		yield return null;
	}


	bool isFBlogin = false;
	void OnFBLogin(){
		Utility.LogWarning("OnFBLogin");
		if(Global.isPopUp) return;
		if(isFBlogin) return;
		FBStatusStart();
	
		SNSManager.FaceBookLogin(
			()=>{
			FBLoginFinish();
			Invoke("FBStatusFinish", 0.5f);
		},
		()=>{
			Invoke("FBStatusFinish", 0.5f);
	
		}
		);

	}

	void FBLoginFinish(){

		//btnFBLogin.SetActive(false);
		//WorldSNS.SetActive(true);
		//StartCoroutine("startFriendRank");
		Global.gisFBLogin = 1;
	}

	void FBStatusFinish(){
	
		isFBlogin = false;
		GameObject.Find("LobbyUI").SendMessage("FBProcessing",1, SendMessageOptions.DontRequireReceiver);
		SubObj.SendMessage("FBStatusProcess",1,SendMessageOptions.DontRequireReceiver);
	}
	
	void FBStatusStart(){
	
		isFBlogin = true;
		GameObject.Find("LobbyUI").SendMessage("FBProcessing",0, SendMessageOptions.DontRequireReceiver);
		SubObj.SendMessage("FBStatusProcess",0,SendMessageOptions.DontRequireReceiver);
	}

	void OnFBLogout(){

	//	btnFBLogin.SetActive(true);
		Global.gisFBLogin = 0;
	}

	void reFBRankAction(){
	
		if(FB.IsLoggedIn){
		//	btnFBLogin.SetActive(false);
		//	WorldSNS.SetActive(true);
			StartCoroutine("startFriendRank");
		}else{
		//	btnFBLogin.SetActive(true);
		//	fback.SetActive(false);
		//	wback.SetActive(false);
		//	WorldSNS.SetActive(false);
		}
		
	}

	public void OnSocialLogIn(){
		if(Global.isPopUp) return;
		if(isFBlogin) return;
		FBStatusStart();
	//	SNSObj.SetActive(false);
		StartCoroutine("FriendLoadingCircle");
		SNSManager.FaceBookLogin(
			()=>{
			FBLoginFinish();
			GameObject.Find("LobbyUI").SendMessage("LobbyLoginButtonStatus");
			Invoke("FBStatusFinish", 0.5f);
		},
		()=>{
		//	SNSObj.SetActive(true);
			StopFrinedLoadingCircle();
			GameObject.Find("LobbyUI").SendMessage("LobbyLoginButtonStatus");
			Invoke("FBStatusFinish", 0.5f);
		}
		);
		
		
	}

	void FBLoginComplete(int idx){
		if(idx == 0) {
			InitLoginStatus();
			SubObj.SendMessage("FacebookComplete",SendMessageOptions.DontRequireReceiver);
		}//login
		else{
			Invoke("InitLoginStatus", 0.3f);
			SubObj.SendMessage("FacebookComplete",SendMessageOptions.DontRequireReceiver);
		}
		//InitLoginStatus();
		SubObj.SendMessage("MesseagBoxNewCheck",SendMessageOptions.DontRequireReceiver);
	}
	void FBLoginButtonComplete(int idx){
		if(idx == 1){
			StartCoroutine("FriendLoadingCircle");
		//	SNSObj.transform.FindChild("Facebook").FindChild("Btn").gameObject.SetActive(false);
		}else{
			StopFrinedLoadingCircle();
		//	SNSObj.transform.FindChild("Facebook").FindChild("Btn").gameObject.SetActive(true);
		}
	}
	public void LoginButtonStatus(){
		Utility.LogWarning("LoginButtonStatus");	
	}
	void checkRank(){

	}
	
	void OnFBInvite(){
		Utility.LogWarning("OnFBInvite");
		if(!FB.IsLoggedIn) return;
		if(isFBlogin) return;
		FBStatusStart();
	
	}

	bool isWaiting = false;
	IEnumerator LoadingCircle1(){
	//	readyBG.SetActive(true);
		//Utility.LogWarning("loding 4 " + isWaiting);
//		var val = readyBG.transform.GetChild(1).GetComponent<UISprite>() as UISprite;
		float delta = 0.05f;
		float mdelta = 0.0f;
		while(isWaiting){
//			val.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			yield return null;
		}
	//	Utility.LogWarning("loding 3 " + isWaiting);
	}


	IEnumerator FriendLoadingCircle(){
		if(isWaiting) yield break;
		isWaiting = true;
		StartCoroutine("LoadingCircle1");
		yield return null;
	}

	public void StopFrinedLoadingCircle(){
		isWaiting =false;
		//readyBG.SetActive(false);
	}

	void AddFBItem(){
		/*
//		var tr = WorldSNS.transform.GetChild(0) as Transform;
//		dragWorld = WorldSNS.GetComponent<UIDraggablePanel2>();
		dragWorld.maxScreenLine = 2;
		dragWorld.maxColLine = -1;
		int count = 0;
//		WorldSNS.SetActive(true);
//		wback.SetActive(true);
		if(tr.childCount == 0 ){
			count = gameRank.instance.listFFR.Count;
			if(count <= 2) count = 5;
			dragWorld.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().FBFriendRank(index);
			} );
			
		}else{
			for(int i=0; i <tr.childCount;i++){
				var obj = tr.GetChild(i).gameObject as GameObject;
				if(obj.activeSelf){
					obj.GetComponent<FBRankItem>().ChangeFBRank();
				}
			}
		}
	
		StopFrinedLoadingCircle();
		Global.isNetwork = false;
		Global.isPopUp = false;*/
	}
	
	void ShowWorldRankTable(){
		Utility.LogWarning("ShowWorldRankTable");
	//	readyBG.SetActive(false);
		
	}

	void Start(){
		
	}

	void InitRankLable1(){
	//	_Text[0].text = KoStorage.GetKorString("72801"); //weekly
	//	_Text[1].text = KoStorage.GetKorString("72200"); //world
	//	_Text[2].text =KoStorage.GetKorString("72605");
		lbStatus.text = KoStorage.GetKorString("72820");
	
		InitLoginStatus();

	//	if(WorldSNS.GetComponent<GridChildTween>() == null)
	//		WorldSNS.AddComponent<GridChildTween>();
	}
	

	void InitRankPosition(){
		Utility.LogWarning("InitRankPosition");
		worldRankActive();
		backDragActive(true);
	}
	
	void FinishedAddItem(){
		Utility.LogWarning("FinishedAddItem");
	//	FriendSNS.AddComponent<GridChildTween>().RankBoardOut();
	}

	void backDragActive(bool b){
		if(b){
	//		fback.SetActive(false);
	//		wback.SetActive(true);
		}else{
	//		fback.SetActive(true);
	//		wback.SetActive(false);
		}
	}
	
	void worldRankActive(){
	//	WorldSNS.SetActive(true);
	//	WorldSNS.GetComponent<GridChildTween>().RankBoardIn();
	//	FriendSNS.GetComponent<GridChildTween>().RankBoardOut();
	//	BtnFriendSNS.GetComponent<UISprite>().color = _bgColorDisable;
	//	BtnTotalSNS.GetComponent<UISprite>().color = _bgColorEnable;
	}
	
	void FriendRankActive(){
	//	FriendSNS.SetActive(true);
	//	FriendSNS.GetComponent<GridChildTween>().RankBoardIn();
	//	WorldSNS.GetComponent<GridChildTween>().RankBoardOut();
	//	BtnFriendSNS.GetComponent<UISprite>().color = _bgColorEnable;
	//	BtnTotalSNS.GetComponent<UISprite>().color = _bgColorDisable;
	}
	
	
	void OnRankClick(GameObject obj){
		if(obj.GetComponent<UISprite>().color == _bgColorEnable) return;
		if(string.Equals(obj.name, "SNS_W")){
		 worldRankActive();
			backDragActive(true);  // Frined Rank
		}else{
			FriendRankActive();
			backDragActive(false); // show WorldRank
		}
	}

	public float delay = 0.4f;
	public Vector3 twTo = new Vector3(0,-430,0),
		twFrom = new Vector3(0, 150,0);
	void StartTweenPosition(){
		if(isActived) return;
		var tween = gameObject.GetComponent<TweenPosition>() as TweenPosition;
		if(tween == null){
			tween = gameObject.AddComponent<TweenPosition>() as TweenPosition;
		}
		tween.to = twTo;
		tween.from =twFrom;//Vector3.zero;
		tween.duration = delay;
		tween.style = UITweener.Style.Once;
		tween.method = UITweener.Method.EaseInOut;

		tween.onFinished =delegate {
			isDowning = false;
		};
		tween.Reset();
		tween.enabled=true;
	//	Utility.LogWarning("StartTweenPosition");
		isActived = true;
	}
	
	
	void EndTweenPosition(){
		if(!isActived) return;
		var tween = gameObject.GetComponent<TweenPosition>() as TweenPosition;
		if(tween == null){
			tween = gameObject.AddComponent<TweenPosition>() as TweenPosition;

		}
		tween.to =twFrom;//new Vector3(0,150,0);//// Vector3.zero;
		tween.from =twTo;//new Vector3(0, -430,0);
		tween.duration = delay;//0.7f;
		tween.style = UITweener.Style.Once;
		tween.method = UITweener.Method.EaseInOut;
		tween.onFinished =delegate {
			isDowning = false;
		};
		tween.Reset();
		tween.enabled=true;
		isActived = false;
//		Utility.LogWarning("EndTweenPosition");
	}
	
	void EndTweenPositionSetActive(){
		TweenPosition _temp = gameObject.GetComponent<TweenPosition>() as TweenPosition;
		if(_temp == null) {
			_temp = gameObject.AddComponent<TweenPosition>();
		}

		_temp.to =  twFrom;//new Vector3(0,150,0);////Vector3.zero;
		_temp.from = twTo;//new Vector3(0,-390,0);
		_temp.duration = 0.001f;
		_temp.style = UITweener.Style.Once;
		_temp.method = UITweener.Method.EaseInOut;
		_temp.onFinished =delegate {
			selfDisable();// = false;
		};	_temp.Reset();
		_temp.enabled = true;
		isActived = false;
	//	Utility.LogWarning("EndTweenPositionSetActive");
	}
	
	void selfDisable(){
		isDowning = false;
		gameObject.SetActive(false);
		SubObj.SetActive(false);
		QuestObj.SetActive(false);
	}
	

	
	System.Action rankDownAction;
	public bool isDowning = true;
	void rankActivation(){
		isDowning = true;
		SubObj.GetComponent<SubMenuAction>().RankingBoardUpDown(checkRankStatus, true);
		Invoke("StartTweenPosition",0.02f);
		SubObjTween(false);
		questObjTween(false);
		SubObj.GetComponent<SubMenuAction>().MesseagBoxNewCheck();
		rankDownAction = ()=>{
			isDowning = true;
			Invoke("StartTweenPosition",0.01f);
			SubObjTween(false);
			questObjTween(false);
		};
	}
	void EndTweenSlow(){
		isDowning = true;
		TweenPosition _temp = gameObject.GetComponent<TweenPosition>() as TweenPosition;
		if(_temp == null) {
			_temp = gameObject.AddComponent<TweenPosition>();
		}
		_temp.to =twFrom;// new Vector3(0,150,0);//// Vector3.zero;
		_temp.from = twTo;//new Vector3(0,-390,0);
		_temp.duration = delay;
		_temp.style = UITweener.Style.Once;
		_temp.method = UITweener.Method.EaseInOut;
		_temp.onFinished =delegate {
			selfDisable();// = false;
		};
		_temp.Reset();
		_temp.enabled = true;
		isActived = false;
		SubObjTween(true);
		questObjTween(true);
	//	Utility.LogWarning("EndTweenSlow");
	}
	
	void questObjTween(bool b){
		QuestObj.SetActive(true);
		TweenPosition _temp = QuestObj.GetComponent<TweenPosition>() as TweenPosition;
		if(_temp != null) {Destroy (_temp);}
		_temp = QuestObj.AddComponent<TweenPosition>();
		if(b) {
		//	_temp.from =  Vector3.zero;
		//	_temp.to = new Vector3(700,0,0);
			_temp.to = twFrom;
			_temp.from =twTo;//Vector3.zero;
			_temp.duration =delay;
		}else{
			//_temp.to =  Vector3.zero;
			//_temp.from = new Vector3(700,0,0);
			_temp.to = twTo;
			_temp.from =twFrom;//Vector3.zero;
			_temp.duration =delay;
		}
		_temp.style = UITweener.Style.Once;
		_temp.method = UITweener.Method.EaseInOut;
	}
	void SubObjTween(bool b){
		SubObj.SetActive(true);

		TweenPosition _temp = SubObj.GetComponent<TweenPosition>() as TweenPosition;
		if(_temp != null) {Destroy (_temp);}
		_temp = SubObj.AddComponent<TweenPosition>();
		if(b) {
			_temp.from =  Vector3.zero;
			_temp.to = new Vector3(700,0,0);
			_temp.duration =delay;
		}else{
			_temp.to =  Vector3.zero;
			_temp.from = new Vector3(700,0,0);
			_temp.duration =delay;
		}
		_temp.style = UITweener.Style.Once;
		_temp.method = UITweener.Method.EaseInOut;
	//	Utility.LogWarning("SubObjTween");
	}
	
	void OnDisable(){
		isActived = false;
	}

	void OnEnable(){
		if(rankDownAction == null) return;
		else 
			rankDownAction();
		if(FB.IsLoggedIn){
			if(isWaiting){
				isWaiting = false;
				StopCoroutine("LoadingCircle1");
			//	readyBG.SetActive(false);
			}
		}
	
	}
	
	public void checkRankStatus(){
		if(isActived){
			isDowning = true;
			EndTweenPosition();
		}else{
			isDowning = true;
			StartTweenPosition();
		}
	}
	
	void OnWRankClick(){
		if(isFBlogin) return;
		SubObj.SendMessage("OnWRankWindow", SendMessageOptions.DontRequireReceiver);
	}
	protected void LevelUpWindow(bool b){
		SubObj.SendMessage("OnLevelUpWindow", b, SendMessageOptions.DontRequireReceiver);
	}
	
	protected void AttendWindow(){
		SubObj.SendMessage("OnAttendWindow", SendMessageOptions.DontRequireReceiver);
	}
	
	protected void SeasonUpWindow(){
		SubObj.SendMessage("OnSeasonUpWindow",SendMessageOptions.DontRequireReceiver);
	}
	
	protected void RankingWindow(){
		SubObj.SendMessage("OnRankWindow",SendMessageOptions.DontRequireReceiver);
	}
	
	protected void FeaturedWindow(){
		SubObj.SendMessage("OnFeaturedWindow",SendMessageOptions.DontRequireReceiver);
	}
	protected void NoticsWindow(string page){
		SubObj.SendMessage("OnNoticsWindow",page,SendMessageOptions.DontRequireReceiver);
	}

	protected void PlusEventWindow(string page){
		SubObj.SendMessage("OnPlusEventWindow",page,SendMessageOptions.DontRequireReceiver);
		//Utility.LogWarning(page);
	}

	protected void InviteEventRankGridAction(){
		SubObj.SendMessage("OnInviteEvenSubMenuAction",SendMessageOptions.DontRequireReceiver);
	}
	protected void weeklyStart(){
		SubObj.SendMessage("OnWeeklyWindowEnd",SendMessageOptions.DontRequireReceiver);
	}

	protected void  OpenContWindow(string str){
		SubObj.SendMessage("OpenContWindow",str, SendMessageOptions.DontRequireReceiver);
	}
	/*
	protected void OnIconHelpClick(){
		if(Global.isPopUp) return;
			SubObj.SendMessage("OnWeeklyWindowStart",SendMessageOptions.DontRequireReceiver);
	} */

//	protected void OnWeeklyClick(){
//		if(Global.isPopUp) return;
//		SubObj.SendMessage("OnRankWindows","weekly" ,SendMessageOptions.DontRequireReceiver);
//	}


/*	protected void OnWorldClick(){
		if(Global.isPopUp) return;
		SubObj.SendMessage("OnRankWindows","world",SendMessageOptions.DontRequireReceiver);
	}
*/
	protected void OpenContentLucky(){
		SubObj.SendMessage("OpenContentLucky",SendMessageOptions.DontRequireReceiver);
	}


	protected void OpenDealerRecommend(){
		SubObj.SendMessage("OpenDealerRecommend",SendMessageOptions.DontRequireReceiver);
	}
	protected void OpenADWin(){
		SubObj.SendMessage("OpenADWin",SendMessageOptions.DontRequireReceiver);
	}

	protected void OpenSpendReward(int balance){
		SubObj.SendMessage("OpenSpendRewardSub",balance,SendMessageOptions.DontRequireReceiver);
	}

	protected void OpenTapJoyReward(){
		SubObj.SendMessage("OpenTapJoyReward",SendMessageOptions.DontRequireReceiver);
	}

	protected void OnInviteWindow(){
		SubObj.SendMessage("OnInviteClick",SendMessageOptions.DontRequireReceiver);
	}

	protected void OnDailyFinish(){
		SubObj.SendMessage("OnDailyFinish",SendMessageOptions.DontRequireReceiver);
	}

	protected void AchievementEnable(){
		SubObj.SendMessage("OnAchieveEnable",SendMessageOptions.DontRequireReceiver);
	}

	protected void HelpWindow(){
		SubObj.SendMessage("OnHelpWindow",SendMessageOptions.DontRequireReceiver);
	}
}
