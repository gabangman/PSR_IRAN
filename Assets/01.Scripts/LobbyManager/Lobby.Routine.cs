using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
public partial class LobbyManager : MonoBehaviour {


	protected void FirstRankPopUpEnd(){
		isShowWin = true;
		Global.gFirstRank = 0;
	}
	protected void PopUpEnd(){
		isShowWin = true;
	}
	protected void ReviewEnd(){
		Global.gReview = 0;
		CClub.gReview = 0;
		isShowWin = true;
	}
	protected void ReviewFail(){
		isShowWin = true;
	}
	protected void noticeEnd(){
		isShowWin = true;
	}
	protected void AttendEventEnd(){
		isShowWin = true;
	}
	protected void SeasonUpEnd(){
		isShowWin = true;
	}
	protected void OnRankClose(){
		isShowWin = true;
	}
	protected void OnOpenContentClose(){
		isShowWin = true;
	}
	IEnumerator remainGetAPI(){
		if(CClub.bMaterial){
			Global.isNetwork = false;
			yield break;
		}else{


			for(int j =0; j < 1; j++){
			//	yield return new WaitForSeconds(5.0f);
			CClub.bMaterial = true;
			CClub.bRace = true;
			Global.isNetwork = true;
			NetworkManager.instance.CheckSumPushToken();
			while(Global.isNetwork){
				yield return null;
			}
		//	Global.isNetwork = true;
			bool bConnect = false;
			string mAPI = ServerAPI.Get(90018); // "game/container/"
			NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
					Utility.ResponseLog(request.response.Text, GV.mAPI);
				if(string.IsNullOrEmpty(request.response.Text) == true) {
					AccountManager.instance.ErrorPopUp();
					return;
				}
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					GV.myCouponList = new System.Collections.Generic.List<int>();
					GV.myCouponList.Add(0);
					GV.myCouponList.Add(0);
					
					int cnt = thing["result"]["silverCnt"].AsInt;
					GV.UpdateCouponList(0, cnt);
					cnt = thing["result"]["goldCnt"].AsInt;
					GV.UpdateCouponList(1, cnt);
					
				}else{
					if(status == -105){
						AccountManager.instance.ErrorPopUp();
						return;
					}
				}
				bConnect = true;
			});
			
			while(!bConnect){
				yield return null;
			}
			bConnect = false;
			mAPI = ServerAPI.Get(90013); // "game/material/"
			NetworkManager.instance.HttpConnect("Get",  mAPI, (request)=>{
					Utility.ResponseLog(request.response.Text, GV.mAPI);
				if(string.IsNullOrEmpty(request.response.Text) == true) {
					AccountManager.instance.ErrorPopUp();
					return;
				}
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					GV.listMyMat = new System.Collections.Generic.List<MatInfo>();
					for(int i = 0; i < 21; i++){
						GV.listMyMat.Add(new MatInfo((8600+i), 0));
						//Utility.LogWarning(" " + (8600+i));
					}
					int cnt = thing["result"].Count;
					for(int i = 0; i < cnt; i++){
						int matid =  thing["result"][i]["materialId"].AsInt;
						int matcount = thing["result"][i]["count"].AsInt;
						GV.UpdateMatCount(matid,matcount);
					}
				}else{
					if(status == -105){
						AccountManager.instance.ErrorPopUp();
						return;
					}
				}
				bConnect = true;
			});
			
			while(!bConnect){
				yield return null;
			}
		
			bConnect = false;
			mAPI = ServerAPI.Get(90014); // "game/material/evoCube/"
			NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
					Utility.ResponseLog(request.response.Text, GV.mAPI);
				if(string.IsNullOrEmpty(request.response.Text) == true) {
					AccountManager.instance.ErrorPopUp();
					return;
				}
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					int matcount = thing["result"]["count"].AsInt;
					GV.UpdateMatCount(8620,matcount);
					
				}else{
					if(status == -105){
						AccountManager.instance.ErrorPopUp();
						return;
					}
				}
				bConnect = true;
			});

			while(!bConnect){
				yield return null;
			}
				bConnect = false;
				mAPI = ServerAPI.Get(90049); // "game/race/eventRace/count/"
				NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
					Utility.ResponseLog(request.response.Text, GV.mAPI);
					if(string.IsNullOrEmpty(request.response.Text) == true) {
						AccountManager.instance.ErrorPopUp();
						return;
					}
					var thing = SimpleJSON.JSON.Parse(request.response.Text);
					int status = thing["state"].AsInt;
					if (status == 0)
					{
						myAccount.instance.account.eRace.featuredPlayCount = thing["result"]["featuredCnt"].AsInt;
						myAccount.instance.account.eRace.testDrivePlayCount  = thing["result"]["testDirveCnt"].AsInt;
						myAccount.instance.account.eRace.EvoPlayCount = thing["result"]["evoCubeCnt"].AsInt;
						if(GV.bDayReset){
							myAccount.instance.account.eRace.featuredPlayCount = 0;
							myAccount.instance.account.eRace.testDrivePlayCount =0;
						}
						
					}else{
						myAccount.instance.account.eRace.featuredPlayCount = 0;
						myAccount.instance.account.eRace.testDrivePlayCount =0;
						myAccount.instance.account.eRace.EvoPlayCount = 0;
						if(status == -105){
							AccountManager.instance.ErrorPopUp();
							return;
						}
					}
					bConnect = true;
				});
				
				while(!bConnect){
					yield return null;
				}
			}
			Global.isNetwork  = false;
	
		}
	}
	IEnumerator InitPopUpWindow(){
		rankObj.SendMessage("InitRankLable");
		btnstate = buttonState.LEVELUP;
		/*isShowWin = false;
		Social.localUser.Authenticate((bool success) => {
			if (success) {
				//	Utility.LogWarning("Authenticate 1");
				EncryptedPlayerPrefs.SetString("GoogleLogin", "Success");
			} else {
				//	Utility.LogWarning("Authenticate 2");
				EncryptedPlayerPrefs.SetString("GoogleLogin", "Failed");
			}
			isShowWin = true;
		});
		while(!isShowWin){
			yield return null;
		}*/

		isShowWin = false;
		noticsShow(0);
		while(!isShowWin){
			yield return null;
		}

		isShowWin = false;
		noticsShow(1);
		while(!isShowWin){
			yield return null;
		}

		isShowWin = false;
		noticsShow(2);
		while(!isShowWin){
			yield return null;
		}

		isShowWin = false;
		ServerPopUp();
		while(!isShowWin){
			yield return null;
		}

		isShowWin = false;
		PlusEventShow();
		while(!isShowWin){
			yield return null;
		}

		isShowWin = false;
		RankWeeklyResult();
		while(!isShowWin){
			yield return null;
		}
		isShowWin = false;
		AttendEvent();
		while(!isShowWin){
			yield return null;
		}
		if(Global.gSeasonUp == 11 ){
			isShowWin = false;
			SeasonUpWindow();
			while(!isShowWin){
				yield return null;
			}
		}
		isShowWin = false;
		OpenContentsWindow();
		while(!isShowWin){
			yield return null;
		}

	
		if(Global.gSeasonUp >= 10){
			isShowWin = false;
			OpenContentsLucky();
			while(!isShowWin){
				yield return null;
			}

		}

		Global.gSeasonUp = 0;
		isShowWin = false;
		StartCoroutine("LevelBarProcess");
		while(!isShowWin){
			yield return null;
		}
		isShowWin = false;
		Review();
		while(!isShowWin){
			yield return null;
		}

		/*isShowWin = false;
		StartCoroutine("LevelBarProcess");
		while(!isShowWin){
			yield return null;
		}*/

		isShowWin = false;
		LevelUpWindow();
		while(!isShowWin){
			yield return null;
		}
		while(Global.isNetwork){
			yield return null;
		}
		isShowWin = false;
		SponosorExprieWin();
		while(!isShowWin){
			yield return null;
		}
		isShowWin = false;
		OpenADWindow();
		while(!isShowWin){
			yield return null;
		}
		if(GV.gTapJoy !=0){
			isShowWin = false;
			SetTapJoyBalanceCheck();
			while(!isShowWin){
				yield return null;
			}
		}

	
	//	isShowWin = false;
	//	SetAchievementIncrease();
	//	while(!isShowWin){
	//		yield return null;
	//	}


		isShowWin = false;
		OpenDailyFinish();
		while(!isShowWin){
		yield return null;
		}
		
		isShowWin = false;
		OpenHelpwindow();
		while(!isShowWin){
			yield return null;
		}

		isShowWin = false;
		SecondTutorial();
		while(!isShowWin){
			yield return null;
		}

	//	GV.bRaceLose = true;
	//	EncryptedPlayerPrefs.DeleteKey("DealerBuy");
		SetAchievementIncrease();
		if(GV.ChSeasonID >= 6001){
			isShowWin = false;
			OpenDealerWindow();
			while(!isShowWin){
				yield return null;
			}
		}
		//if(!FB.IsLoggedIn)

		btnstate = buttonState.WAIT;
		rankObj.SendMessage("rankActivation",SendMessageOptions.DontRequireReceiver);
		activeObject.SendMessage("ShowRepairLable", true, SendMessageOptions.DontRequireReceiver);
		lobby.GetComponent<crewSpeak>().LobbyCamCheck(true);
		InitTopMenu();
		UserDataManager.instance.OnRoutinCheck(true);
	//	yield break;
		/*
		Global.isNetwork = true;
		NetworkManager.instance.OnQuestReward(0,"1000");
		while(Global.isNetwork){
			yield return null;
		}


		Global.isNetwork = true;
		NetworkManager.instance.OnQuestReward(2,"1000");
		while(Global.isNetwork){
			yield return null;
		}

		Global.isNetwork = true;
		NetworkManager.instance.OnQuestReward(4,"10");
		while(Global.isNetwork){
			yield return null;
		}

		Global.isNetwork = true;
		NetworkManager.instance.OnQuestReward(5,"10");
		while(Global.isNetwork){
			yield return null;
		}


		Global.isNetwork = true;
		NetworkManager.instance.OnQuestReward(3,"10");
		while(Global.isNetwork){
			yield return null;
		}*/
	}

	public void TESTAC(){
		Dictionary<string, int> mDic = new Dictionary<string, int>();
		mDic.Add("acheivmentId",16000);
		mDic.Add("value",1);
		string mAPI = ServerAPI.Get(90061);//game/acheivment/increment
		NetworkManager.instance.HttpFormConnect("Put",mDic,mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				//response:{"state":0,"msg":"sucess","result":{"count":50},"time":1459745139}
			}
			//Utility.LogWarning("set count : " + id);
		});
	}


	private void OpenDealerWindow(){
		if(!EncryptedPlayerPrefs.HasKey("DealerBuy")){
			EncryptedPlayerPrefs.SetInt("DealerBuy",10);
		}
		int a =  EncryptedPlayerPrefs.GetInt("DealerBuy");
		if(!GV.bRaceLose){
			if(a != 10){
				if(a == 1){ // 유저가 클릭했다.
					myAcc.instance.account.bLobbyBTN[4] = true;
					GV.bRaceLose = true;
				}else if(a == 0){ // 유저가 취소했다. 
					GV.bRaceLose = false;
					myAcc.instance.account.bLobbyBTN[4] = false;
				}else if(a == 2){// 유저가 구매했다. 
					GV.bRaceLose = false;
					myAcc.instance.account.bLobbyBTN[4] = false;
				}
			}
			isShowWin = true;
			return;
		}

		int mCnt = GV.mineCarList.Count;
		int mcount = 0;
		for(int i = 0; i < mCnt; i++){
			if(GV.mineCarList[i].ClassID== "SS"){

				mcount++;
			}
			//Utility.LogWarning(GV.mineCarList[i].ClassID);
			if(mcount == 3){
				break;
			}
		}

		if(mcount >= 3){
			isShowWin = true;GV.bRaceLose = false;
			EncryptedPlayerPrefs.SetInt("DealerBuy", 0);
			return;
		}
		if(a != 10){
			//int a = EncryptedPlayerPrefs.GetInt("DealerBuy");
			if(a == 1){ // 유저가 클릭했다.
				myAcc.instance.account.bLobbyBTN[4] = true;
				GV.bRaceLose = true;
			}else if(a == 0){ // 유저가 취소했다. 
				myAcc.instance.account.bLobbyBTN[4] = false;
				GV.bRaceLose = false;
			}else if(a == 2){// 유저가 구매했다. 
				GV.bRaceLose = false;
				myAcc.instance.account.bLobbyBTN[4] = false;
			}
			isShowWin = true;
			return;
		}

		SetUserDealerCar2();
		var temp = ObjectManager.CreatePrefabs("Window", "popUp_Dealer") as GameObject;
		var par  = GameObject.FindGameObjectWithTag("BottomAnchor");
			temp.transform.parent = par.transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = new Vector3(0,360,-1500);
			temp.transform.localEulerAngles = Vector3.zero;
			//temp.gameObject.SetActive(true);
			temp.GetComponent<DealerPopup>().SetInit();
		/*if(GV.gDealer == 1){
			if(myAccount.instance.account.attendevent.dealerCar == 0){
				myAccount.instance.account.attendevent.dealerCar = SetUserDealerCar();
				GV.gDealerCarID = myAccount.instance.account.attendevent.dealerCar;
			}else{
				GV.gDealerCarID = myAccount.instance.account.attendevent.dealerCar;
			}
			rankObj.SendMessage("OpenDealerRecommend",SendMessageOptions.DontRequireReceiver);
			myAcc.instance.account.bLobbyBTN[4] = true;
			activeObject.SendMessage("SHOPNewButton",true);
			GV.gDealer = 0;
		}else{
			myAccount.instance.account.attendevent.dealerCar = 0;
			GV.gDealerCarID=0;
			isShowWin = true;
		}*/
	}
	


	void noticsShow(int idx){
		if(idx == 0){
			if(GV.gInfo.Notics1State == 2 || GV.gInfo.Notics1State == 3){ //1
				GV.gInfo.Notics1State = 0;
				string str = GV.gInfo.Notics1URL ;
				rankObj.SendMessage("NoticsWindow",str, SendMessageOptions.DontRequireReceiver);
				return;
			}else {
				isShowWin = true;
				return;
			}
		}else if(idx == 1){
			if(GV.gInfo.Notics2State == 2 || GV.gInfo.Notics2State == 3){ //1
				GV.gInfo.Notics2State = 0;
				string str = GV.gInfo.Notics2URL ;
				rankObj.SendMessage("NoticsWindow",str, SendMessageOptions.DontRequireReceiver);
				return;
			}else {
				isShowWin = true;
				return;
			}

		}else{
			if(GV.gInfo.Notics3State == 2 || GV.gInfo.Notics3State == 3){ //1
				GV.gInfo.Notics3State = 0;
				string str = GV.gInfo.Notics3URL ;
				rankObj.SendMessage("NoticsWindow",str, SendMessageOptions.DontRequireReceiver);
				return;
			}else {
				isShowWin = true;
				return;
			}

		}
	}
	
	void PlusEventShow(){
		if(GV.gInfo.plusEventState != 0){ //1
			GV.gInfo.plusEventState = 0; //1
			//여기에 사항 함수 추가 할것....
			string str = GV.gInfo.plusEventURL;
			rankObj.SendMessage("PlusEventWindow",str, SendMessageOptions.DontRequireReceiver);
		}else {
			isShowWin = true;
		}
	}
	void ServerPopUp(){
		if(GV.gInfo.eNoticsState== 1){
			GV.gInfo.eNoticsState= 0;
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			pop.AddComponent<noticesPopup>().InitPopUp();
		}else{
			isShowWin = true;
			return;
		}
	}
	
	void RankWeeklyResult(){
		if(Global.gWeeklyStart == 1){
			rankObj.SendMessage("weeklyStart", SendMessageOptions.DontRequireReceiver);
			Global.gWeeklyStart = 0;
		}else{
			isShowWin = true;
		}
	}

	void OpenDailyFinish(){
		if(GV.bWeeklyReset){
			if(GV.ChSeasonID >= 6003){
				rankObj.SendMessage("OnDailyFinish", SendMessageOptions.DontRequireReceiver);
				GV.bWeeklyReset = false;
			}else {
				GV.bWeeklyReset = false;
				isShowWin = true;
			}
		}else{
			GV.bWeeklyReset = false;
			isShowWin = true;
		}
	}

	void firstRankRace(){
		if(Global.gFirstRank == 1 && Global.gFirstRankRace == 1){
			Global.gFirstRankRace = 0;
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			pop.AddComponent<firstRankPopup>().InitPopUp();
		}else{
			isShowWin = true;
		}
	}
	void Review(){
		if(CClub.gReview == 1){
			if(GV.gInfo.extra03 == 2 || GV.gInfo.extra03 == 3){ //1
			//if(GV.gInfo.extra03 == 1){
				var pop = ObjectManager.SearchWindowPopup() as GameObject;
				pop.AddComponent<ReviewPopup>().InitPopUp();
			}else{
				CClub.gReview = 0;isShowWin = true;
			}
		
		}else if(CClub.gReview == 2){
			if(GV.gInfo.extra03 == 2 || GV.gInfo.extra03 == 3){ //1
				OnReview();
			}else{
				CClub.gReview = 0;isShowWin = true;
			}
		}else{
			isShowWin = true;
		}
	}


	private void OnReview(){
			var temp = ObjectManager.CreatePrefabs("Window", "popUp_review") as GameObject;
			var par  = GameObject.FindGameObjectWithTag("BottomAnchor");
			temp.transform.parent = par.transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = new Vector3(0,360,-1500);
			temp.transform.localEulerAngles = Vector3.zero;
			temp.gameObject.SetActive(true);
			temp.GetComponent<ReviewReward>().SetInitReview();
	}

	void SponosorExprieWin(){


		if(Global.gExpireSpon == 1){ 
			Global.gExpireSpon = 0;
			Global.isPopUp = true;
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			pop.AddComponent<SponExpirePopup>().InitPopUp(()=>{

				myTeamInfo mTeam = GV.getTeamTeamInfo(GV.SelectedTeamID);
				if(mTeam.freeSpon == 1){
					ClubSponInfo.instance.mClubSpon.unSetTeamSpon(GV.SelectedTeamID);
					isShowWin = true;
					Global.isPopUp = false;
					return;
				}


				Dictionary<string, int> mDic = new Dictionary<string,int>();
				mDic.Add("teamId",GV.SelectedTeamID);
				//mDic.Add("sponsorId",1300);
				string mAPI = ServerAPI.Get(90009);// "game/team/sponsor"
				NetworkManager.instance.HttpFormConnect("Delete", mDic, mAPI, (request)=>{
					Utility.ResponseLog(request.response.Text, GV.mAPI);
					var thing = SimpleJSON.JSON.Parse(request.response.Text);
					int status = thing["state"].AsInt;
					if (status == 0)
					{
						
					}else{
						
					}
					isShowWin = true;
					Global.isPopUp = false;
				});
			});
		}else{
			isShowWin = true;
		}
	}
	
	void thirdAgreeFail(){
		Utility.LogWarning("thirdAgreeFail");
	}
	
	void thirdAgreeOk(){
		Utility.LogWarning("thirdAgreeOk");
	}
	
	
	void SNSFriendOK(){
		Utility.LogWarning("SNSFriendOK");
	}
	
	
	void AttendEvent(){
		if(Global.gAttend == 0) {
			isShowWin = true;
			return;
		}else{
			rankObj.SendMessage("AttendWindow");
			Global.gAttend = 0;
		}
	}
	
	void SeasonUpWindow(){
		
		rankObj.SendMessage("SeasonUpWindow", SendMessageOptions.DontRequireReceiver);
		myAcc.instance.account.bLobbyBTN[1] = true;
		activeObject.SendMessage("MYTEAMNewButton",true);
		myAcc.instance.account.bLobbyBTN[2] = true;
		myAcc.instance.account.bInvenBTN[0] = true;
		activeObject.SendMessage("INVENTORYNewButton",true);
	}
	
	void LevelUpWindow(){
		if(Global.gLvUp == 0) {
			isShowWin = true;
			Global.isNetwork = false;
		}else{
			rankObj.SendMessage("LevelUpWindow" , true,  SendMessageOptions.DontRequireReceiver);
			Global.gLvUp = 0;
		}
		
	}
	
	private void OpenContentsLucky(){
		if(Global.gSeasonUp == 11){
			if(GV.ChSeason == 2){
				rankObj.SendMessage("OpenContentLucky",SendMessageOptions.DontRequireReceiver);
				myAcc.instance.account.bLobbyBTN[4] = true;
				activeObject.SendMessage("SHOPNewButton",true);
			}else isShowWin = true;
		}else{
			isShowWin = true;
		}
	}

	private void OpenContentsWindow(){
		if(GV.gChamWin == 1){
			string season = AccountManager.instance.ChampItem.R_open;
			if(season.Equals("S") == true){
				isShowWin = true;
			}else{
				rankObj.SendMessage("OpenContWindow",season, SendMessageOptions.DontRequireReceiver);
				myAcc.instance.account.bLobbyBTN[5] = true;
				activeObject.SendMessage("NEXTNewButton",true);
			}
			GV.gChamWin = 0;
		}else{
			isShowWin = true;
		}
		
	}

	void OpenHelpwindow(){
		if(GV.bHelp){
			rankObj.SendMessage("HelpWindow", SendMessageOptions.DontRequireReceiver);
			GV.bHelp =false;
		}else{
			isShowWin = true;
		}
	}
	
	private void OpenADWindow(){
		if(GV.gADWindow == 1){
			rankObj.SendMessage("OpenADWin",SendMessageOptions.DontRequireReceiver);
			GV.gADWindow = 0;
		}else{
			isShowWin = true;
		}
		
	}



	private void SetAchievementIncrease(){
		if(GV.achieveId == 0){
		//	isShowWin = true;
		}else{
			StartCoroutine("SetAchievementIncreaseToServer");
		} 
	}
	
	private void SetAchievementIncreaseStarUp(int idx){
		Global.isNetwork = true;
		int id = GAchieve.instance.achieveInfo.ListAchieve[idx].AcheiveID;
		Dictionary<string, int> mDic = new Dictionary<string, int>();
		mDic.Add("acheivmentId",id);
		mDic.Add("value",1);
		string mAPI = ServerAPI.Get(90061);//game/acheivment/increment
		int state = 0;
		NetworkManager.instance.HttpFormConnect("Put",mDic,mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
		//	Utility.LogWarning("equest.response.Text " + request.response.Text);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				int count = thing["result"]["count"].AsInt;
				if(count >= GAchieve.instance.achieveInfo.ListAchieve[idx].TargetCount){
					GAchieve.instance.achieveInfo.ListAchieve[idx].bFinish = true;
					GAchieve.instance.achieveInfo.ListAchieve[idx].bReward = false;
					GAchieve.instance.achieveInfo.ListAchieve[idx].mCount
						= GAchieve.instance.achieveInfo.ListAchieve[idx].TargetCount;
					state = 1;
					GV.bachieveRewardFlag = true;
					rankObj.SendMessage("AchievementEnable", SendMessageOptions.DontRequireReceiver);
				}else{
					GAchieve.instance.achieveInfo.ListAchieve[idx].mCount = count;
				}
			}else if(status == -203){
				
			}
			isShowWin = true;
			Global.isNetwork = false;
		});
	}


	private IEnumerator SetUserAchievementIncreaseToServer(){
		int cnt = GAchieve.instance.achieveInfo.ListAchieve.Count;
		bool b = false; Global.isNetwork = true;
		for(int i = 0; i < cnt ; i++){

			if(GAchieve.instance.achieveInfo.ListAchieve[i].mUnLock){
		  	  if(GAchieve.instance.achieveInfo.ListAchieve[i].bUpLoad){
			  	  b = false;
				  int id = GAchieve.instance.achieveInfo.ListAchieve[i].AcheiveID;
			 	  Dictionary<string, int> mDic = new Dictionary<string, int>();
				  mDic.Add("acheivmentId",id);
				//  Utility.LogWarning("set 업적 : " + id);
				  id = GAchieve.instance.achieveInfo.ListAchieve[i].mPlusCount;
			 	  mDic.Add("value",id);
				  string mAPI = ServerAPI.Get(90061);//game/acheivment/increment
				  NetworkManager.instance.HttpFormConnect("Put",mDic,mAPI, (request)=>{
						Utility.ResponseLog(request.response.Text, GV.mAPI);
						var thing = SimpleJSON.JSON.Parse(request.response.Text);
					int status = thing["state"].AsInt;
					if (status == 0)
					{
						//response:{"state":0,"msg":"sucess","result":{"count":50},"time":1459745139}


						GAchieve.instance.achieveInfo.ListAchieve[i].bUpLoad = false;
						GAchieve.instance.achieveInfo.ListAchieve[i].mPlusCount=0;
					}
					//Utility.LogWarning("set count : " + id);
					b = true;
				});
			  }else b = true; // if bUpLoad
			}else b = true; // if mUnlock
			while(!b){
				yield return null;
			} 
		} // for
		//isShowWin = true;
		Global.isNetwork = false;
		GV.achieveId = 0;
		yield return null;
	}

	private IEnumerator SetAchievementIncreaseToServer(){
		int cnt = GAchieve.instance.achieveInfo.ListAchieve.Count;
		bool b = false; int re = 0; 
		Global.isNetwork = true;
		for(int i = 0; i < cnt ; i++){
			re = 0;
			b = false;
			if(GAchieve.instance.achieveInfo.ListAchieve[i].mUnLock){
				if(GAchieve.instance.achieveInfo.ListAchieve[i].bUpLoad){
					b = false;
					int id = GAchieve.instance.achieveInfo.ListAchieve[i].AcheiveID;
					Dictionary<string, int> mDic = new Dictionary<string, int>();
					mDic.Add("acheivmentId", id);
					id = GAchieve.instance.achieveInfo.ListAchieve[i].mPlusCount;
					mDic.Add("value",id);
					string mAPI = ServerAPI.Get(90061);//game/acheivment/increment
					NetworkManager.instance.HttpFormConnect("Put",mDic,mAPI, (request)=>{
						Utility.ResponseLog(request.response.Text, GV.mAPI);
						var thing = SimpleJSON.JSON.Parse(request.response.Text);
						int status = thing["state"].AsInt;
						if (status == 0)
						{
							int count = thing["result"]["count"].AsInt;
							if(count >= GAchieve.instance.achieveInfo.ListAchieve[i].TargetCount){
								GAchieve.instance.achieveInfo.ListAchieve[i].bFinish = true;
								GAchieve.instance.achieveInfo.ListAchieve[i].bReward = false;
								GAchieve.instance.achieveInfo.ListAchieve[i].mCount
									= GAchieve.instance.achieveInfo.ListAchieve[i].TargetCount;
								GV.bachieveRewardFlag = true;
							}else{
								GAchieve.instance.achieveInfo.ListAchieve[i].mCount = count;
							}
							GAchieve.instance.achieveInfo.ListAchieve[i].mPlusCount = 0;
							GAchieve.instance.achieveInfo.ListAchieve[i].bUpLoad = false;
							re = 0;
						}else if(status == -203){
							re =100; Global.isNetwork = false;
						}else re = 0;

						b = true;
					});
				}else b = true; // if bUpLoad
			}else b = true; // if mUnlock

			while(!b){
				yield return null;
			} 
			if(re == 100){
				b = false;
				yield return AccountManager.instance.UnLockAchievement(GAchieve.instance.achieveInfo.ListAchieve[i].AcheiveID);
			}else b = true;
			while(!b){
				yield return null;
			} 
		} // for
		//isShowWin = true;
		Global.isNetwork = false;
		GV.achieveId = 0;
		yield return null;
	}

	protected void OnOpenDealer(){
		Invoke("OnShopCarDealer", 0.1f);
		isShowWin = true;
	}

	void TapJoyRewardComplete(){
		isShowWin = true;
	}

	private void SetTapJoyBalanceCheck(){
		if(!rankObj.activeSelf){
		}else{
			rankObj.SendMessage("OpenTapJoyReward",SendMessageOptions.DontRequireReceiver);
		}
	}

	void FaceBookLoginComplete(int idx){
		rankObj.SendMessage("FBLoginComplete",idx,SendMessageOptions.DontRequireReceiver);
	}

	void FacebookButtonActivity(int idx){
		rankObj.SendMessage("FBLoginButtonComplete",idx,SendMessageOptions.DontRequireReceiver);
	}

	void SetLobbyReturn(bool b){
		isLobby = b;
	}

	void OnCross(){
		rankObj.SendMessage("OnCrossWindow",SendMessageOptions.DontRequireReceiver);
	}



}
