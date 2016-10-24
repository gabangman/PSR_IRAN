using UnityEngine;
using System.Collections;

public class ClubRace_Start : MonoBehaviour {

	public GameObject cReward;
	public GameObject listMy, listOther;
	public GameObject btnMy, btnOther;
	public GameObject NextObj;
	public GameObject slotPrefabs;
	public GameObject TeamMy, TeamOther;
	public UILabel lbTime;
	public Transform timeArrow;
	private GameObject myCircle, oppCircle;
	private string[] lbStr;
	private float cTime = 0.0f;
	private readonly float secondsToDegrees = 360f / 60f;
	private UILabel lbText, lbText2;
	void Awake(){
		transform.FindChild("lbRaceComplete").GetComponent<UILabel>().text =string.Empty;
		transform.FindChild("lbText1").GetComponent<UILabel>().text =KoStorage.GetKorString("77221");

		myCircle = transform.FindChild("List_MyTeam").FindChild("Ready_Circle").gameObject;
		oppCircle = transform.FindChild("List_Other").FindChild("Ready_Circle").gameObject;
		lbText = transform.FindChild("lbText_2").GetComponent<UILabel>();
		lbText.text = string.Empty;
		lbText2 = transform.FindChild("lbText_2_Other").GetComponent<UILabel>();
		lbText2.text = string.Empty;
	}
	bool bMatchTime =false;
	void initialContentWindow(){
		/*cTimes =System.Convert.ToDateTime(CClub.mReady.matchingTime);
		System.DateTime nNow = NetworkManager.instance.GetCurrentDeviceTime();
		System.TimeSpan mCompareTime =nNow - cTimes;
		if(mCompareTime.TotalSeconds >= CClub.mClubInfo.matchDurationSeconds){
			//GameObject.Find("LobbyUI").SendMessage("OnClanReturn");
			GameObject.Find("LobbyUI").SendMessage("OnBackClick");
			return;
		}*/
		if(Global.isNetwork)return;
		Global.isNetwork = true;
	//	listOther.SetActive(true);
	//	listMy.SetActive(false);
	//	gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
	//	NextObj.SetActive(false);
	//	StartCoroutine("GetOppUserInfo");
	
		gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
		StartCoroutine("GetMyUserInfo", 0);
		ChangeClubWarInfo();
		btnMy.SetActive(true);
		btnOther.SetActive(false);
		listMy.SetActive(true);
		listOther.SetActive(false);
		TeamMy.SetActive(true);
		TeamOther.SetActive(false);
		NextObj.SetActive(false);
		//if(CClub.mClubInfo.myEntry == 0 || CClub.mClubInfo.myEntryFlag == 0)
		if(CClub.mClubInfo.myEntryFlag == 0)
		{
			lbText.text =KoStorage.GetKorString("73515");
		}else{
			if(CClub.mClubInfo.myEntry == 0){
				lbText.text =KoStorage.GetKorString("73515");
			}else{
				lbText.text =KoStorage.GetKorString("77219");
			}
		}
	
	


		lbText2.text = string.Empty;



	}

	public void ChangeClubWarInfo(){

		var tr = transform.FindChild("Score") as Transform;
		
		var tr1 = tr.FindChild("MyTeam")  as Transform;
		
		var tr2 = tr.FindChild("OtherTeam") as Transform;
		
		
		tr1.FindChild("ClanName").GetComponent<UILabel>().text = CClub.mClubInfo.mClubName;
		tr1.FindChild("ClanSymbol").GetComponent<UISprite>().spriteName = CClub.mClubInfo.clubSymbol;
		
		tr2.FindChild("ClanName").GetComponent<UILabel>().text = CClub.mReady.oppclubName;
		tr2.FindChild("ClanSymbol").GetComponent<UISprite>().spriteName =CClub.mReady.oppclubsymbol;
		
		tr = tr.FindChild("StarScore");
		
		tr.FindChild("Score_M").GetComponent<UILabel>().text = CClub.mReady.myClubStarCount.ToString();
		tr.FindChild("User_M").GetComponent<UILabel>().text = string.Format("{0}/{1}", CClub.mReady.myMemeberRacePlayCount, CClub.mReady.MemberNum*3);
		tr.FindChild("Score_O").GetComponent<UILabel>().text = CClub.mReady.oppClubStarCount.ToString();
		tr.FindChild("User_O").GetComponent<UILabel>().text = string.Format("{0}/{1}",CClub.mReady.oppMemeberRacePlayCount, CClub.mReady.oppMemberNum*3);
	
		remainTime =System.Convert.ToDateTime(CClub.mReady.matchingTime);
		remainTime = remainTime.AddDays(1);
		bTimeClock = true;
		cTimes =System.Convert.ToDateTime(CClub.mReady.matchingTime);
	}

	IEnumerator GetOppUserInfo(){
		yield return StartCoroutine("readyMatchingClub");
		CClub.oppClubRaceInfo.Clear();
		bool bConnect = false;///club/getMainRaceMyUserInfo
		string mAPI = "club/getMainRaceOppUserInfo";///club/getClubRankingLocal
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
		
				ClubRaceMemberInfo mInfo = null;

				int cnt = thing["userList"].Count;
				for(int i= 0; i <cnt; i++){
					mInfo = new ClubRaceMemberInfo(thing["userList"][i]);
					CClub.oppClubRaceInfo.Add(mInfo);
				}
				if(CClub.oppClubRaceInfo.Count >=2){
					CClub.oppClubRaceInfo.Sort(delegate(ClubRaceMemberInfo x, ClubRaceMemberInfo y) {
						int a = y.EarnedStarCount.CompareTo(x.EarnedStarCount);
						if(a == 0)
							a = y.mLv.CompareTo(x.mLv);
						return a;
					});
				}
			}else if(status == -421){
				
			}else{
			}
			
			
			bConnect = true;
			
		});

		while(!bConnect){
			yield return null;
		}
		addItemsOther("MyTeam");
		gameObject.GetComponent<ClubLoading>().StopClubLoading();
		Global.isNetwork = false;
	}

	IEnumerator GetMyUserInfo(int idx = 0){
		if(idx == 1){
			yield return StartCoroutine("readyMatchingClub");
		}
		CClub.myClubRaceInfo.Clear();
		bool bConnect = false;///club/getMainRaceMyUserInfo
		string mAPI = "club/getMainRaceMyUserInfo";///club/getClubRankingLocal
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				ClubRaceMemberInfo mInfo = null;
				
				int cnt = thing["userList"].Count;
				for(int i= 0; i <cnt; i++){
					mInfo = new ClubRaceMemberInfo(thing["userList"][i]);
					CClub.myClubRaceInfo.Add(mInfo);
				}


				if(CClub.myClubRaceInfo.Count >=2){
					CClub.myClubRaceInfo.Sort(delegate(ClubRaceMemberInfo x, ClubRaceMemberInfo y) {
						int a = y.EarnedStarCount.CompareTo(x.EarnedStarCount);
						if(a == 0)
							a = y.mLv.CompareTo(x.mLv);
						return a;
					});
				}


			}else if(status == -421){
				
			}else{
		
			}
			
			bConnect = true;
			
		});
		
		while(!bConnect){
			yield return null;
		}
		addItemsMy("MyTeam");
		gameObject.GetComponent<ClubLoading>().StopClubLoading();
		Global.isNetwork = false;

	}
	void OnTeamSelect(GameObject obj){
	//	//==!!Utility.LogWarning("OnTeamSelect"+ bMatchTime);
		if(bMatchTime){
	//		//==!!Utility.LogWarning("bMatchTime" + bMatchTime);
			GameObject.Find("LobbyUI").SendMessage("OnBackClick");
			return;
		}
		NGUITools.FindInParents<ModeClan>(gameObject).OnNextTeam(obj.transform.parent.name);
	}
	
	public void SetNextButton(int x){
		if(x == 1){
			NextObj.SetActive(false);
			transform.FindChild("lbRaceComplete").GetComponent<UILabel>().text =KoStorage.GetKorString("77405");
		//	transform.FindChild("lbText1").GetComponent<UILabel>().text ="3번 플레이?";
			return;
		}
		NextObj.SetActive(true);
		var tw = NextObj.GetComponent<TweenPosition>() as TweenPosition;
		tw.Reset();
		tw.enabled = true;
		transform.FindChild("lbRaceComplete").GetComponent<UILabel>().text =string.Empty;
	//	transform.FindChild("lbText1").GetComponent<UILabel>().text =string.Empty;
	}

	public void SetOppNextButton(){
	
		NextObj.SetActive(false);
		transform.FindChild("lbRaceComplete").GetComponent<UILabel>().text =string.Empty;
	
	}
	
	void OnTeam(GameObject obj){
		if(Global.isNetwork) return;
		if(bMatchTime){
			GameObject.Find("LobbyUI").SendMessage("OnBackClick");
			return;
		}
		if(obj.name.Equals("MyTeam")){
			if(btnMy.activeSelf) return;
			Global.isNetwork  = true;
			gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
			StartCoroutine("GetMyUserInfo", 1);
			btnMy.SetActive(true);
			btnOther.SetActive(false);
			listMy.SetActive(true);
			listOther.SetActive(false);
			TeamMy.SetActive(true);
			TeamOther.SetActive(false);
			NextObj.SetActive(false);
			myCircle.SetActive(false);
			if(CClub.mClubInfo.myEntryFlag == 0)
			{
				lbText.text =KoStorage.GetKorString("73515");
			}else{
				if(CClub.mClubInfo.myEntry == 0){
					lbText.text =KoStorage.GetKorString("73515");
				}else{
					lbText.text =KoStorage.GetKorString("77219");
				}
			}
			ChangeClubWarInfo();
			
			lbText2.text = string.Empty;
		}else{
			if(btnOther.activeSelf) return;
			Global.isNetwork  = true;
			gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
			StartCoroutine("GetOppUserInfo");
			btnOther.SetActive(true);
			btnMy.SetActive(false);
			listMy.SetActive(false);
			listOther.SetActive(true);
			NextObj.SetActive(false);
			TeamMy.SetActive(false);
			TeamOther.SetActive(true);
			oppCircle.SetActive(false);
			lbText2.text =KoStorage.GetKorString("77220");
			lbText.text = string.Empty;
			ChangeClubWarInfo();
		}
	}
	
	
	void addItemsMy(string str){
		Transform grid1 = listMy.transform.GetChild(0);
		int cnt = grid1.childCount;
		int maxMem = CClub.myClubRaceInfo.Count;
		if(cnt == 0){
			for(int i =0; i < maxMem; i++){
				var temp = NGUITools.AddChild(grid1.gameObject, slotPrefabs);
				temp.name = "ClubWar_"+(i).ToString();
				temp.AddComponent<ClanWarItem>().ViewClanWarInfo(CClub.myClubRaceInfo[i],i);
			}
			grid1.GetComponent<UIGrid>().Reposition();
			//	TeamMy.SetActive(false);
		}else{
			for(int i =0; i < maxMem; i++){
				grid1.GetChild(i).GetComponent<ClanWarItem>().ViewClanWarInfo(CClub.myClubRaceInfo[i],i);
			}
			grid1.GetComponent<UIGrid>().Reposition();
		}
		return;
		var drag = listMy.GetComponent<UIDraggablePanel2>() as UIDraggablePanel2;
		drag.maxScreenLine = 1;
		drag.maxColLine = 0;
		int count = 0;
		var grid = listMy.transform.GetChild(0) as Transform;
		if(grid.childCount == 0 ){
			count = 30;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().ViewClanHistory(index);
			});
			grid.GetComponent<UIGrid>().Reposition();
		}
		//drag.SetInitialPosition();
		//drag.ResetPosition();
	}
	private bool bCircle = true;
	IEnumerator StartUserCircle(GameObject obj){
		bCircle = true;
		obj.SetActive(true);
		var sp = obj.GetComponent<UISprite>() as UISprite;
		sp.fillAmount= 0.0f;
		float delta = 0.05f;
		float mdelta = 0.0f;
		while(bCircle){
			sp.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			yield return null;
		}
	//	obj.SetActive(false);
	
	}

	void StopUserCircle(){
		if(!bCircle) return;
		bCircle = false;
		oppCircle.SetActive(false);
		myCircle.SetActive(false);
	}
	
	public void OnSelectItems(string str){
		if(Global.isNetwork) return;
	//	//==!!Utility.LogWarning(str);
		int cnt = 0;
		Transform mGrid = null;

		if(bMatchTime){
			GameObject.Find("LobbyUI").SendMessage("OnBackClick");
			return;
		}
		Global.isNetwork = true;

	//	gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
		if(listOther.activeSelf){
			lbText2.text = string.Empty;
			StartCoroutine("StartUserCircle", oppCircle);
			mGrid = listOther.transform.GetChild(0);
			StartCoroutine("GetOppUserInfoDetail", str);
		
		}else if(listMy.activeSelf){
			StartCoroutine("StartUserCircle", myCircle);
			mGrid = listMy.transform.GetChild(0);
			StartCoroutine("GetMyUserInfoDetail", str);
			lbText.text = string.Empty;
		}
		cnt = mGrid.childCount;
		for(int i =0; i < cnt; i++){
			mGrid.GetChild(i).GetComponent<ClanWarItem>().UnSelectLine(str);
		}
	}

	IEnumerator GetOppUserInfoDetail(string str){
	
		CClub.oppClubRaceInfoDetail.Clear();
		bool bConnect = false;///club/getMainRaceMyUserInfo
		string mAPI = "club/getMainRaceOppUserInfoDetail";///club/getClubRankingLocal
		int cnt = 0;
		string[] str1 = str.Split('_');
		int.TryParse(str1[1], out cnt);
		int userid = CClub.oppClubRaceInfo[cnt].clubUserId;
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		mDic.Add("clubUserId", userid.ToString());
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				ClubRaceMemberInfoDetail mInfo = null;
				CClub.oppClubRaceInfoDetail.Clear();
				int cnt1 = thing["raceList"].Count;
				for(int i= 0; i <cnt1; i++){
					mInfo = new ClubRaceMemberInfoDetail(thing["raceList"][i]);
					CClub.oppClubRaceInfoDetail.Add(mInfo);
				}
			}else if(status == -421){
				
			}else{
				
			}
			
			bConnect = true;
			
		});
		
		while(!bConnect){
			yield return null;
		}
		
		TeamOther.SendMessage("OnChangeInfoOther", CClub.oppClubRaceInfoDetail, SendMessageOptions.DontRequireReceiver);
		if(NextObj.activeSelf){
			NextObj.SetActive(false);
		}
	//	gameObject.GetComponent<ClubLoading>().StopClubLoading();
		Global.isNetwork = false;
		StopUserCircle();
	}

	IEnumerator GetMyUserInfoDetail(string str){
		CClub.myClubRaceInfoDetail.Clear();
		bool bConnect = false;///club/getMainRaceMyUserInfo
		string mAPI = "club/getMainRaceMyUserInfoDetail";///club/getClubRankingLocal
		int cnt = 0;
		string[] str1 = str.Split('_');
		int.TryParse(str1[1], out cnt);
		int userid = CClub.myClubRaceInfo[cnt].clubUserId;
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		mDic.Add("clubUserId", userid.ToString());
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				ClubRaceMemberInfoDetail mInfo = null;
				CClub.myClubRaceInfoDetail.Clear();
				int cnt1 = thing["raceList"].Count;
				for(int i= 0; i <cnt1; i++){
					mInfo = new ClubRaceMemberInfoDetail(thing["raceList"][i]);
					CClub.myClubRaceInfoDetail.Add(mInfo);
				}
			}else if(status == -421){
				
			}else{
				
			}
			
			bConnect = true;
			
		});
		
		while(!bConnect){
			yield return null;
		}
		if(userid.ToString() == GV.UserRevId)
			TeamMy.SendMessage("OnChangeInfoMy",CClub.myClubRaceInfoDetail , SendMessageOptions.DontRequireReceiver);
		else TeamMy.SendMessage("OnChangeInfoMyClub",CClub.myClubRaceInfoDetail , SendMessageOptions.DontRequireReceiver);
		//gameObject.GetComponent<ClubLoading>().StopClubLoading();
		Global.isNetwork = false;
		StopUserCircle();
	}
	
	void addItemsOther(string str){
		Transform grid1 = listOther.transform.GetChild(0);
		int cnt = grid1.childCount;
		int maxMem = CClub.oppClubRaceInfo.Count; // clan member Count
		if(cnt == 0){
			for(int i =0; i < maxMem; i++){
				var temp = NGUITools.AddChild(grid1.gameObject, slotPrefabs);
				temp.name = "ClubWar_"+i.ToString();
				temp.AddComponent<ClanWarItem>().ViewClanWarInfo(CClub.oppClubRaceInfo[i], i);
			}
			grid1.GetComponent<UIGrid>().Reposition();
		
		}else{
			for(int i =0; i < maxMem; i++){
				grid1.GetChild(i).GetComponent<ClanWarItem>().ViewClanWarInfo(CClub.oppClubRaceInfo[i], i);
			}
			grid1.GetComponent<UIGrid>().Reposition();
		}
		return;
		var drag = listOther.GetComponent<UIDraggablePanel2>() as UIDraggablePanel2;
		drag.maxScreenLine = 1;
		drag.maxColLine = 0;
		int count = 0;
		var grid = listOther.transform.GetChild(0) as Transform;
		if(grid.childCount == 0 ){
			count = 30;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().ViewClanHistory(index);
			});
			grid.GetComponent<UIGrid>().Reposition();
			
		}
		//drag.SetInitialPosition();
		//drag.ResetPosition();
	}
	
	void OnEnable(){
		listMy.SetActive(false);
		listOther.SetActive(true);
		TeamMy.SetActive(false);
		TeamOther.SetActive(true);
		btnOther.SetActive(true);
		btnMy.SetActive(false);
		NextObj.SetActive(false);
	}
	
	protected IEnumerator LoadingRound(){
		LoadingCircle();
		yield return new WaitForSeconds(1.0f);
		StopLoadingCircle();
	}
	
	protected void LoadingCircle(){
		Global.isNetwork = true;
		var loading = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
		loading.StartCoroutine("StartRaceLoading", true);
		StartCoroutine("TenSecondTimeCheck");
	}
	
	protected void StopLoadingCircle(){
		Global.isNetwork = false;
		var loading = gameObject.GetComponent<LoadingGauage>() as LoadingGauage;
		if(loading != null)
			loading.stopLoading();
		StopCoroutine("TenSecondTimeCheck");
	}
	
	public virtual IEnumerator TenSecondTimeCheck(){
		WaitForSeconds w = new WaitForSeconds(1.0f);
		
		for(int i = 0; i < 10; i++){
			yield return w;
		}
		StopLoadingCircle();
		GameObject.Find("LobbyUI").SendMessage("OnBackClick",SendMessageOptions.DontRequireReceiver);
		yield return null;
		
	}
	private bool bTimeClock;
	private System.DateTime remainTime, cTimes;
	void FixedUpdate(){
		cTime += Time.deltaTime*10;
		timeArrow.localRotation = Quaternion.Euler(0f, 0f, cTime * -secondsToDegrees);
		if(!bTimeClock) return;
		System.DateTime nNow = NetworkManager.instance.GetCurrentDeviceTime();
		System.TimeSpan mCompareTime = remainTime-nNow;//System.DateTime.Now;
	
		lbTime.text = string.Format("{0:00}:{1:00}:{2:00}", mCompareTime.Hours, mCompareTime.Minutes, mCompareTime.Seconds);
		mCompareTime = nNow - cTimes;
		if(mCompareTime.TotalSeconds >= CClub.mClubInfo.matchDurationSeconds){
			lbTime.text = string.Empty;
			bTimeClock = false;
			CClub.ClubMode = 4;
			bMatchTime = true;
			////==!!Utility.LogWarning("timeOut");
			//GameObject.Find("LobbyUI").SendMessage("OnBackClick");
			return;
		}

	}


	IEnumerator readyMatchingClub(){
		bool bConnect = false;
		string mAPI = "club/readyClubMatching";///club/getClubRankingLocal
		
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		NetworkManager.instance.ClubBaseStringConnect("Get",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				CClub.mReady = new ClubReadyInfo(thing);
				CClub.mClubInfo.clubMatchIndex = CClub.mReady.clubMatchingIndex;
				CClub.mClubInfo.matcingTime = CClub.mReady.matchingTime;
			}else if(status == -421){

			}else{
				
			}
			
			Global.isNetwork = false;
			bConnect = true;
			
		});
		
		while(!bConnect){
			yield return null;
		}
		

		
	}
}
