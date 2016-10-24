using UnityEngine;
using System.Collections;

public class ClubMatch : RaceMode {
	
	public GameObject btnMatch, btnClub;
	public GameObject matchPopup, matchNo;
	public GameObject matchStart, matchWait;
	public GameObject resultInfo;
	private int strIdx;
	GameObject matchResult;



	void Awake(){
		gameObject.AddComponent<ClubSubMode>();
		matchResult = transform.FindChild("Match_Result").gameObject;
		strIdx = 0;
	
	}
	
	string randomText(){
		string str = string.Empty;
		int idx = strIdx;
		
		switch(idx){
		case 0:str = "71229";break;
		case 1:str = "71230";break;
		case 2:str = "71231";break;
		case 3:str = "73510";break;
		case 4:str = "77221";break;
		default : str = "73510"; strIdx = -1;break;
		}
		strIdx++;
		return str;
		
	}

	IEnumerator CheckMyClubMode(){

		gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
		bool bConnect = false;
		int mMode = 10;
		ClubInfo mSub = null;
		string mAPI = "club/getMyClubInfo";//ServerAPI.Get(90059);
		NetworkManager.instance.ClubBaseConnect("Get",new System.Collections.Generic.Dictionary<string,int>(), mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				mMode = thing["clubMode"].AsInt;
		//		CClub.ClubMode = mMode;
				mSub = new ClubInfo(thing);
			}else if(status == -1){
				
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}

			transform.FindChild("Clan_QA").gameObject.SetActive(true);
		int index = CClub.ClubMode;// 0: no match, 1: match yes, 2:match wait
		switch(index){
		case 0:  //no action
			matchNo.SetActive(true);
			matchNo.transform.FindChild("Title").GetComponent<UILabel>().text = KoStorage.GetKorString("73505");
			btnClub.transform.GetComponentInChildren<UILabel>().text =KoStorage.GetKorString("73519");
			matchStart.SetActive(false);
			matchWait.SetActive(false);
			matchResult.SetActive(false);
			string strText = randomText();
			int matchMemCnt = 3;
			if(Application.isEditor){
				matchMemCnt = 1;
			}
			if(CClub.ClanMember == 1 || CClub.ClanMember == 2){
				if(CClub.mClubInfo.clubMemberNum >=matchMemCnt){
					btnMatch.SetActive(true);
					btnMatch.GetComponent<UIButtonMessage>().functionName ="OnGoMatch";
					matchNo.transform.FindChild("BtnMatch_No").gameObject.SetActive(false);
				}else{
					btnMatch.SetActive(false);
					btnMatch.GetComponent<UIButtonMessage>().functionName = string.Empty;
					var objTr = matchNo.transform.FindChild("BtnMatch_No") as Transform;
					objTr.gameObject.SetActive(true);
					objTr.GetComponent<UIButtonMessage>().functionName = string.Empty;
					objTr.FindChild("lbText1").GetComponent<UILabel>().text  =  KoStorage.GetKorString("77218");
				}
				btnMatch.transform.FindChild("lbText1").GetComponent<UILabel>().text = KoStorage.GetKorString("77218");
				matchNo.transform.FindChild("lbText1").GetComponent<UILabel>().text = KoStorage.GetKorString(strText);
				
			}else if(CClub.ClanMember == 3){
				btnMatch.SetActive(false);
				matchNo.transform.FindChild("lbText1").GetComponent<UILabel>().text = KoStorage.GetKorString(strText);
			}else{
				btnMatch.SetActive(false);
				matchNo.transform.FindChild("lbText1").GetComponent<UILabel>().text = KoStorage.GetKorString(strText);
			}
			gameObject.GetComponent<ClubLoading>().StopClubLoading();
			break;
		case 1: // 레이스 진행 중
			//	matchNo.SetActive(false);
			//	matchStart.SetActive(true);
			//	matchWait.SetActive(false);
			//StartCoroutine("GetMatchInfo");
			//	gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
			if(string.IsNullOrEmpty(CClub.mClubInfo.clubMatchChatCh) == true){
				CClub.mClubInfo.clubMatchChatCh = mSub.clubMatchChatCh;
			}
			CClub.mClubInfo.matcingTime = mSub.matcingTime;
			System.DateTime mTime =  System.Convert.ToDateTime(CClub.mClubInfo.matcingTime);
			System.TimeSpan sTime = NetworkManager.instance.GetCurrentDeviceTime() - mTime;
			//if(sTime.TotalHours >=24){
			if(sTime.TotalSeconds >= CClub.mClubInfo.matchDurationSeconds){
				if(netFlag  == 1) yield break;
				netFlag = 1; 
				StartCoroutine("getMainClubResult",mSub);
			}else{
				matchStart.SetActive(true);
				matchStart.GetComponent<UIButtonMessage>().functionName = "OnNext";
				StartCoroutine("readyMatchingClub", 1);
			}


			break;
		case 2: //레이스 결과중 
			//	matchNo.SetActive(false);
			//	matchStart.SetActive(false);
			//	matchWait.SetActive(true);
			//	gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
			StartCoroutine("getMainClubRaceResult");
			
			break;
		case 3: //레이스 매칭 대기중 
			//	matchNo.SetActive(false);
			//	matchStart.SetActive(false);
			//	matchWait.SetActive(true);
			//	gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
			StartCoroutine("readyMatchingClub", 3);
			
			break;
		case 4: // 레이스 중 종료임.
			if(netFlag  == 1) yield break;
			netFlag = 1; 
			//	gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
			StartCoroutine("getMainClubResult",mSub);
			matchStart.GetComponent<UIButtonMessage>().functionName = "OnReqeustResult";
			break;
		case 5: // 레이스 중 종료임.
			//gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
			StartCoroutine("getMainClubResult",mSub);
			break;
		}
	}

	public void ClubModeInitialize(){
		transform.FindChild("Clan_QA").gameObject.SetActive(true);
		if(CClub.ClubTest == 0){
			transform.FindChild("ComingSoon").gameObject.SetActive(true);
			transform.FindChild("ComingSoon").GetComponentInChildren<UILabel>().text =KoStorage.GetKorString("73506");
			return;
		}
		if(CClub.mClubFlag == 0){
			transform.FindChild("Locked").gameObject.SetActive(true);
			transform.FindChild("Locked").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("73046");//
			return;
		}
		StartCoroutine("CheckMyClubMode");
	
	}
	
	private int netFlag = 0;
	void Start(){
		
		
	}
	
	void OnRefresh(){
		if(CClub.ClubMode == 3)
			ClubModeInitialize();
		else{
			//Utility.LogError("ClubMode " + CClub.ClubMode);
			CClub.ClubMode = 3;
			ClubModeInitialize();
		}
	}
	
	IEnumerator readyMatchingClub(int idx){
		bool bConnect = false;
		string mAPI = string.Empty;
		/*	if(idx == 3){
			int mMode = 10;
			mAPI = "club/getMyClubInfo";//ServerAPI.Get(90059);
			NetworkManager.instance.ClubBaseConnect("Get",new System.Collections.Generic.Dictionary<string,int>(), mAPI, (request)=>{
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					mMode = thing["clubMode"].AsInt;
					
					
				}else if(status == -1){
					
				}
				bConnect = true;
			});
			
			while(!bConnect){
				yield return null;
			}
			
			if(mMode == 0){
				CClub.ClubMode = 0;
				ClubModeInitialize();
				yield break;	
			}
			
		}*/
		bConnect = false;
		mAPI = "club/readyClubMatching";///club/getClubRankingLocal
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		NetworkManager.instance.ClubBaseStringConnect("Get",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				CClub.ClubMode = 1;
				matchNo.SetActive(false);
				matchStart.SetActive(true);
				matchWait.SetActive(false);
				matchResult.SetActive(false);
				CClub.mReady = new ClubReadyInfo(thing);
				CClub.mClubInfo.clubMatchIndex = CClub.mReady.clubMatchingIndex;
				matchStart.SendMessage("ClubMatchReadyInit",	CClub.mReady,SendMessageOptions.DontRequireReceiver);
				CClub.mClubInfo.matcingTime = CClub.mReady.matchingTime;
			//	NGUITools.FindInParents<RaceModeMenu>(gameObject).SetClubOnButton();
			}else if(status == -421){
				string strName = thing["RequestMatchingTime"].Value;
				if(strName == null || strName == ""){
					CClub.readyWaitTime = NetworkManager.instance.GetCurrentDeviceTime();
				}else{
					CClub.readyWaitTime =  System.Convert.ToDateTime(strName);
				}
				matchNo.SetActive(false);
				matchStart.SetActive(false);
				matchWait.SetActive(true);
				matchWait.SendMessage("OnSetMatchStart", SendMessageOptions.DontRequireReceiver);
				if(matchResult.activeSelf) matchResult.SetActive(false);
			}else{
				
			}
			Global.isNetwork = false;
			bConnect = true;
			
		});
		
		while(!bConnect){
			yield return null;
		}
		
		gameObject.GetComponent<ClubLoading>().StopClubLoading();
		
	}


	private ClubRaceResultInfo mClubResult;
	IEnumerator getMainClubRaceResult(){
		bool bConnect = false;//club/requestClubMatching
		string mAPI = "club/getMainClubRaceResult";///club/getClubRankingLocal
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		mDic.Add("clubMatchingIndex",CClub.mClubInfo.clubMatchIndex.ToString());
		int status = 0;
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			status = thing["state"].AsInt;
			if (status == 0)
			{
				//Debug.LogWarning(request.response.Text);
				matchNo.SetActive(false);
				matchStart.GetComponent<UIButtonMessage>().functionName = "OnNext";
				matchStart.SetActive(false);
				matchWait.SetActive(false);
				//	{"state":0,"oppclubName":"club_name_39","oppclubsymbol":"sample symbol","oppClubStarCount":19,"myClubStarCount":13,
				//		"oppMemberTotalNum":1,"oppMemberNum":1,"raceResult":0,"racePrize":1}
				mClubResult = null;
				mClubResult = new ClubRaceResultInfo(thing);
				matchResult.SetActive(true);
				matchResult.SendMessage("ClubResultInfo", mClubResult);
				CClub.ClubMode = 0;
				CClub.mClubInfo.matcingTime = NetworkManager.instance.GetCurrentDeviceTime().ToString();
			//	oppClanName = mClubResult.oppclubName;
			//	oppClanSymbol = mClubResult.oppclubsymbol;
				EncryptedPlayerPrefs.SetInt("ClubAlram",0);
			}
			else if(status == -1){
				CClub.ClubMode = 4;
			}else{
				CClub.ClubMode = 0;
			}
			bConnect = true;
			
		});
		
		while(!bConnect){
			yield return null;
		}
		
		Global.isNetwork = false;
		gameObject.GetComponent<ClubLoading>().StopClubLoading();
		if(status == -1){
			//CClub.ClubMode = 1;
			//ClubModeInitialize();
		}
	}
	
	IEnumerator getMainClubResult(ClubInfo mSub = null){
		
		//	yield return new WaitForSeconds(1.0f);
		int status = 0;
		bool bConnect  = false;
		string mAPI = "club/getMyClubInfo";//ServerAPI.Get(90059);
		if(mSub != null){
			NetworkManager.instance.ClubBaseConnect("Get",new System.Collections.Generic.Dictionary<string,int>(), mAPI, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				status = thing["state"].AsInt;
				if (status == 0)
				{
					
					CClub.mClubInfo = new ClubInfo(thing);

					if(CClub.mClubInfo.clubMember == 9){
						CClub.ClanMember = 1;
					}else if(CClub.mClubInfo.clubMember ==5){
						CClub.ClanMember =2;
					}else{
						CClub.ClanMember =3;
					}
					
				}else if(status == -1){
					
					
				}
				bConnect = true;
			});
			
			while(!bConnect){
				yield return null;
			}
		}else{
			CClub.mClubInfo = mSub;
			if(CClub.mClubInfo.clubMember == 9){
				CClub.ClanMember = 1;
			}else if(CClub.mClubInfo.clubMember ==5){
				CClub.ClanMember =2;
			}else{
				CClub.ClanMember =3;
			}
		}

		netFlag = 0;
		if(status == -1){
			CClub.mClubFlag =1;
			CClub.ClubMode = 0;
			CClub.ClanMember = 0;
			CClub.ChangeClub = 1;
			ClubModeInitialize();
			yield break;
		}
		
		if(CClub.ClubMode == 2){
			yield return StartCoroutine("getMainClubRaceResult");
		}else if(CClub.ClubMode == 0){
		//	matchStart.SetActive(true);
			gameObject.GetComponent<ClubLoading>().StopClubLoading();
			matchStart.GetComponent<UIButtonMessage>().functionName = "OnNext";
		}else if(CClub.ClubMode == 1){
		//	matchStart.SetActive(true);
			matchStart.GetComponent<UIButtonMessage>().functionName = "OnReqeustResult";
			gameObject.GetComponent<ClubLoading>().StopClubLoading();
		}else{
		//	matchStart.SetActive(true);
			matchStart.GetComponent<UIButtonMessage>().functionName = "OnNext";
			gameObject.GetComponent<ClubLoading>().StopClubLoading();
		}
		
	}
	void OnEnable(){
		
		if(matchNo.activeSelf){
			
		}
		if(CClub.ClubMode == 2){
			if(CClub.ClubMatchFlag == 1){
				matchWait.GetComponent<UIButtonMessage>().functionName = "OnNext";
			}
		}
	}
	
	public void ReClubMatch(){
		matchNo.SetActive(true);
		matchStart.SetActive(false);
		matchWait.SetActive(false);
		CClub.ClubMode = 0;
		CClub.ClubMatchFlag = 0;
	}
	
	void OnGoMatch(){
		matchPopup.SetActive(true);
		NGUITools.FindInParents<TweenAction>(gameObject).doubleTweenScale(matchPopup);
		matchPopup.GetComponent<ClanMatchSys>().OnMatching((b)=>{
			if(b){
				matchWait.SetActive(true);
				matchNo.SetActive(false);
				CClub.ClubMatchFlag = 1;
				if(CClub.ClubMatchFlag == 1){
					CClub.readyWaitTime = NetworkManager.instance.GetCurrentDeviceTime();
					matchWait.GetComponent<UIButtonMessage>().functionName = "OnNext";
					matchWait.SendMessage("OnSetMatchStart", SendMessageOptions.DontRequireReceiver);
				}else{
					
				}
				CClub.ClubMode = 3; // 
			}else {
			//	matchWait.SetActive(false);
			//	matchNo.SetActive(false);
			//	matchResult.SetActive(false);
			//	matchStart.SetActive(false);
				CClub.ClubMode = 1;
				CClub.mClubInfo.matcingTime = NetworkManager.instance.GetCurrentDeviceTime().ToString();
				ClubModeInitialize();
			}
		});
	}
	
	public void OnMatchCancel(){
		CClub.ClubMode = 0;
		ClubModeInitialize();
	}
	void OnGoClan(){
		GameObject.Find("LobbyUI").SendMessage("OnClanReturn");
	}
	
	
	public override void OnNext (GameObject obj)
	{
		base.OnNext (obj);
		UserDataManager.instance.OnSubBackMenu = null;
		CClub.ClubClick = 2;
		GameObject.Find("LobbyUI").SendMessage("OnClubRaceClick",obj.name,SendMessageOptions.DontRequireReceiver);
	}
	
	public void OnReqeustResult(){
		CClub.ClubMode = 5;
		ClubModeInitialize();
	}

	public void OnResultClose(){
		matchResult.SetActive(true);
		resultInfo.SetActive(false);
		transform.FindChild("Clan_QA").gameObject.SetActive(true);

	}

	public void OnResultList(){
		if(mClubResult == null) return;
		transform.FindChild("Clan_QA").gameObject.SetActive(false);
		var temp = resultInfo.GetComponent<ClubMatchUserResult>() as ClubMatchUserResult;
		if(temp == null) temp = resultInfo.AddComponent<ClubMatchUserResult>();
		matchResult.SetActive(false);
		resultInfo.SetActive(true);
		temp.AddItem( mClubResult.oppclubName, mClubResult.oppclubsymbol, mClubResult);

	}

}
