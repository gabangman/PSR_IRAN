using System;
using UnityEngine;
using System.Collections;
//using GoogleMobileAds.Api;

public class ADRewardContent : MonoBehaviour {
	
	public Transform timeBG, timeArrow;
	public GameObject slotItem;
	public Transform grid;
	public UILabel[] lbText;
	private float cTime = 0.0f;
	public Transform[] btnMovie;
	public UILabel[] lbBtnTime;
	public UILabel[] lbMovie;
	public GameObject okObj, failObj;
	private readonly float secondsToDegrees = 360f / 60f;


	void Start(){
		for(int i =0; i < 3; i++){
			lbBtnTime[i].transform.gameObject.SetActive(true);
			lbBtnTime[i].text = string.Empty;
		}
		lbText[1].text = KoStorage.GetKorString("72702");
		lbText[2].text = KoStorage.GetKorString("72703");
		lbText[3].text = KoStorage.GetKorString("72704");
		
		lbMovie[0].text = string.Empty;
		lbMovie[1].text = KoStorage.GetKorString("72104");
		
		lbMovie[2].text = KoStorage.GetKorString("72320");
		lbMovie[3].text = KoStorage.GetKorString("71000");
		//	lbMovie[4].text = KoStorage.GetKorString("71000");
		
		//btnMovie[0].gameObject.SetActive(true);
		//btnMovie[2].gameObject.SetActive(false);
	
		transform.FindChild("Reward").gameObject.SetActive(true);
		btnMovie[0].transform.FindChild("lbtext_1").GetComponent<UILabel>().text = KoStorage.GetKorString("77407");
	/*	if(GV.gADInit == 0){
			AdColony.Configure
				(
					"version:1.0,store:google", // Arbitrary app version and Android app store declaration.
					"app6119a0659cb141938b",  // ADC App ID from adcolony.com
					"vzb4e54a5eb7714fb991" // A zone ID from adcolony.com / /test1
					//		"vz7888f332683541e382" // Any number of additional Zone IDS //Adzone #1
					//			"vz1fd5a8b2bf6841a0a4b826"
					);//Your app secret key: v4vc621c2574e7184c6fa7
			GV.gADInit = 1;
			UnityAdsHelper.SetInitialized();
		}
*/
		//appid//appe7f02bc1e36d4a27b0
		//zone//vzd122262263cb4b4996
		//cTimes =NetworkManager.instance.GetCurrentDeviceTime();// System.DateTime.Now;
		//lbText[0].text =string.Format("{0:00}:{1:00}:{2:00}", cTimes.Hour, cTimes.Minute, cTimes.Second);

	
	//	long dayTime = myAccount.instance.account.dayCheckTime;
	//	RemainTime = new System.DateTime(dayTime);
	//	System.DateTime nNow = NetworkManager.instance.GetCurrentDeviceTime();
	//	System.TimeSpan mInitTime = RemainTime-nNow;//System.DateTime.Now;
	//	if(mInitTime.Minutes < 0){
	//		RemainTime = RemainTime.AddDays(1);
	//	}


		//SetDayTimeCheck();
		/*
		Heyzap.HZVideoAd.AdDisplayListener mL1 = delegate(string adState, string adTag) {
			Utility.LogWarning("hzvideoAd state " + adState + " tag " + adTag);

				if ( adState.Equals("show") ) {
					// Sent when an ad has been displayed.
					// This is a good place to pause your app, if applicable.
				}
				if ( adState.Equals("hide") ) {

				rewardIdx = 1;
				RewardCompleteWindow();
				okObj.SetActive(true);
				transform.FindChild("Reward").gameObject.SetActive(false);
				btnMovie[0].gameObject.SetActive(false);
				GV.RewardViewTime[0] = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(2).Ticks;
				Global.isNetwork = false;
					// Sent when an ad has been removed from view.
					// This is a good place to unpause your app, if applicable.
				}
				if ( adState.Equals("click") ) {
					// Sent when an ad has been clicked by the user.
				}
				if ( adState.Equals("failed") ) {
					// Sent when you call `show`, but there isn't an ad to be shown.
					// Some of the possible reasons for show errors:
					//    - `HeyzapAds.PauseExpensiveWork()` was called, which pauses 
					//      expensive operations like SDK initializations and ad
					//      fetches, andand `HeyzapAds.ResumeExpensiveWork()` has not
					//      yet been called
					//    - The given ad tag is disabled (see your app's Publisher
					//      Settings dashboard)
					//    - An ad is already showing
					//    - A recent IAP is blocking ads from being shown (see your
					//      app's Publisher Settings dashboard)
					//    - One or more of the segments the user falls into are
					//      preventing an ad from being shown (see your Segmentation
					//      Settings dashboard)
					//    - Incentivized ad rate limiting (see your app's Publisher
					//      Settings dashboard)
					//    - One of the mediated SDKs reported it had an ad to show
					//      but did not display one when asked (a rare case)
					//    - The SDK is waiting for a network request to return before an
					//      ad can show
					transform.FindChild("Reward").gameObject.SetActive(false);
					failObj.SetActive(true);
					btnMovie[0].gameObject.SetActive(false);
					GV.RewardViewTime[0] = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(2).Ticks;
					Global.isNetwork = false;
					FinishCallback = null;
				}
				if ( adState.Equals("available") ) {
					// Sent when an ad has been loaded and is ready to be displayed,
					//   either because we autofetched an ad or because you called
					//   `Fetch`.
				}
				if ( adState.Equals("fetch_failed") ) {
					// Sent when an ad has failed to load.
					// This is sent with when we try to autofetch an ad and fail, and also
					//    as a response to calls you make to `Fetch` that fail.
					// Some of the possible reasons for fetch failures:
					//    - Incentivized ad rate limiting (see your app's Publisher
					//      Settings dashboard)
					//    - None of the available ad networks had any fill
					//    - Network connectivity
					//    - The given ad tag is disabled (see your app's Publisher
					//      Settings dashboard)
					//    - One or more of the segments the user falls into are
					//      preventing an ad from being fetched (see your
					//      Segmentation Settings dashboard)
				}
				if ( adState.Equals("audio_starting") ) {
					// The ad about to be shown will need audio.
					// Mute any background music.
				}
				if ( adState.Equals("audio_finished") ) {
					// The ad being shown no longer needs audio.
					// Any background music can be resumed.
			}
		};
		Heyzap.HZVideoAd.SetDisplayListener(mL1);
		Heyzap.HZVideoAd.Fetch();


		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		int adID =GV.CurrADId;
		mDic.Add("rewardId",8763);
		int remat = 8610;
		Common_Attend.Item rItem = Common_Attend.Get(adID);
		string strValue = null;
		switch(rItem.R_type){
		case 3 :{
			mDic.Add("type",2);
			mDic.Add("value",rItem.R_no);
		}	break;
		case 4 :{
			mDic.Add("type",0);
			mDic.Add("value",rItem.R_no);
		}break;
		case 8 :{
			mDic.Add("type",3);
			strValue = "value;"+remat.ToString()+":"+rItem.R_no.ToString();
		}break;
		case 6 : {
			mDic.Add("type",4);
			mDic.Add("value",rItem.R_no);
		}break;
		case 7 : {
			mDic.Add("type",5);
			mDic.Add("value",rItem.R_no);
			
		}break;
		case 5 : {
			//str = "Cube";
			mDic.Add("type",3);
			strValue = "value;8620:"+rItem.R_no.ToString(); 
			myAcc.instance.account.bInvenBTN[2] = true;
			//mDic.Add("value",rItem.R_no);
		}break;
		}
		
		string mAPI = ServerAPI.Get(90068);//"user/reward"
		NetworkManager.instance.HttpFormConnect("Put",mDic, mAPI ,(request)=>{
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			Utility.LogWarning("respone " + request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{

			}
			Global.isNetwork = false;
		},strValue);
		*/


	}


	void FixedUpdate(){
		cTime += Time.deltaTime*10;
		timeArrow.localRotation = Quaternion.Euler(0f, 0f, cTime * -secondsToDegrees);
		System.DateTime nNow = NetworkManager.instance.GetCurrentDeviceTime();
		lbText[0].text = UserDataManager.instance.dailyTimes();
		if(GV.RewardViewTime[0] !=0 ){
			mCompareTime = new System.DateTime(GV.RewardViewTime[0]) - nNow;
			if(mCompareTime.Seconds < 0){
				lbBtnTime[0].text = string.Empty;
				btnMovie[0].gameObject.SetActive(true);
				GV.RewardViewTime[0] = 0;
			}else{
				if(GV.CurrADId > GV.MaxADId){
					lbBtnTime[0].text =string.Empty;
					btnMovie[0].gameObject.SetActive(false);
				}else{
					lbBtnTime[0].text = string.Format("{0:00}:{1:00}", mCompareTime.Minutes, mCompareTime.Seconds);
				}
			}
		}else {
			lbBtnTime[0].text = string.Empty;
			//btnMovie[0].gameObject.SetActive(true);
		}

	}
	
	void OnClose(){
		NGUITools.FindInParents<SubMenuWindow>(gameObject).OnCloseClick();
		Global.isNetwork = false;
	}
	
	
	public void SetSlotInitialize(){
		int cnt = grid.childCount;
		int num = 8750;
		if(cnt == 0){
			for(int i = 0; i < 15 ; i++){
				num = 8750+i;
				var temp = NGUITools.AddChild(grid.gameObject, slotItem);
				temp.name = num.ToString();
				temp.AddComponent<SlotRewardItem>().InitRewardItems(num, gameObject);
			}
			grid.GetComponent<UIGrid>().Reposition();
		}else{

		}
	
		if(GV.CurrADId > GV.MaxADId) {
			lbBtnTime[0].text = string.Empty;
			btnMovie[0].gameObject.SetActive(false);
			GV.RewardViewTime[0] =0;
		}else{
			if(GV.RewardViewTime[0] != 0){
				btnMovie[0].gameObject.SetActive(false);
			}else{
				lbBtnTime[0].text = string.Empty;
				btnMovie[0].gameObject.SetActive(true);
			}
		}
	}
	
	public void OnChangeSlotItems(){
		int cnt = grid.childCount;
		for(int i = 0; i < cnt ; i++){
			grid.GetChild(i).GetComponent<SlotRewardItem>().ChangeRewardItems();
		}
	}
	
	public void OnResetSlotItems(){
		int cnt = grid.childCount;
		for(int i = 0; i < cnt ; i++){
			//grid.GetChild(i).GetComponent<SlotRewardItem>().SetRewardItems();
		}
	}
	
	
	public void OnResetItems(int currId, int nextId){
		int cnt = grid.childCount;
		for(int i = 0; i < cnt ; i++){
			grid.GetChild(i).GetComponent<SlotRewardItem>().ReSetRewardItems(currId, nextId);
		}
	
	}
	
	public void OnAd(string str){
		OnWatch();
	}
	void TestAD(){

	/*	if (interstitial.IsLoaded()) {
			interstitial.Show();
			Utility.LogWarning("ABC");
		}else{
			Utility.LogWarning("Not YET");
		}
		return; */
		rewardIdx = 1;
		RewardCompleteWindow();
		okObj.SetActive(true);
		transform.FindChild("Reward").gameObject.SetActive(false);
		btnMovie[0].gameObject.SetActive(false);
		//GV.RewardViewTime[0] = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(2).Ticks;
		GV.RewardViewTime[0] =0;
		Global.isNetwork = false;
	}
	/*
	public void Hand_AdLoaded(object sender, EventArgs args)
	{
		Utility.LogWarning("HandleInterstitialLoaded event received.");
		// Handle the ad loaded event.
	}
	public void Hand_AdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		Utility.LogWarning("HandleInterstitialFailedToLoad event received.");
		// Handle the ad loaded event.
	}
	public void Hand_AdOpened(object sender, EventArgs args)
	{
		Utility.LogWarning("HandleInterstitialOpened event received.");
		// Handle the ad loaded event.
	}
	public void Hand_AdClosing(object sender, EventArgs args)
	{
		Utility.LogWarning("HandleInterstitialClosing event received.");
		// Handle the ad loaded event.
	}
	public void Hand_AdClosed(object sender, EventArgs args)
	{
		Utility.LogWarning("HandleInterstitialClosed event received.");
		// Handle the ad loaded event.
	}
	public void Hand_AdLeftApplication(object sender, EventArgs args)
	{
		Utility.LogWarning("HandleInterstitialClosed event received.");
		// Handle the ad loaded event.
	}
	*/

	//private InterstitialAd interstitial;
	protected void OnWatch1(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		/*string adUnitID = "ca-app-pub-8675074039588692/8979807968";
		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adUnitID);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.

		interstitial.OnAdClosed += Hand_AdLoaded;
		interstitial.OnAdFailedToLoad += Hand_AdFailedToLoad;
		interstitial.OnAdLeavingApplication +=Hand_AdOpened;
		interstitial.OnAdLoaded += Hand_AdClosing;
		interstitial.OnAdOpening += Hand_AdClosed ;
//		interstitial.AdLeftApplication += Hand_AdLeftApplication;
		interstitial.LoadAd(request);
		Invoke("TestAD",2.0f);
	
		return;*/
	//	Invoke("TestAD", 2.0f);
	//	return;

		if(Application.isEditor){
			return;
		}
		Heyzap.HZVideoAd.AdDisplayListener mL1 = delegate(string adState, string adTag) {
			Utility.LogWarning("hzvideoAd state " + adState + " tag " + adTag);
			Global.isNetwork = false;
			if ( adState.Equals("show") ) {
				
			}
			if ( adState.Equals("hide") ) {
				rewardIdx = 1;
				RewardCompleteWindow();
				okObj.SetActive(true);
				transform.FindChild("Reward").gameObject.SetActive(false);
			}
			if ( adState.Equals("click") ) {
			}
			if ( adState.Equals("failed") ) {
				transform.FindChild("Reward").gameObject.SetActive(false);
				failObj.SetActive(true);
				btnMovie[0].gameObject.SetActive(false);
				GV.RewardViewTime[0] = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(5).Ticks;
				FinishCallback = null;
			}
			if ( adState.Equals("available") ) {
			}
			if ( adState.Equals("fetch_failed") ) {
			}
			if ( adState.Equals("audio_starting") ) {
			}
			if ( adState.Equals("audio_finished") ) {
			}
		};
		Heyzap.HZVideoAd.SetDisplayListener(mL1);
		Heyzap.HZVideoAd.Fetch();
		
		if (Heyzap.HZVideoAd.IsAvailable()) {
			Heyzap.HZVideoAd.Show();
			Global.isNetwork = false;
		}else{
			transform.FindChild("Reward").gameObject.SetActive(false);
			failObj.SetActive(true);
			GV.RewardViewTime[0] =0;
			Global.isNetwork = false;
			FinishCallback = null;
			
		}
		return;

		if(Global.isNetwork) return;
		Global.isNetwork = true;
		OnWatchUnityAdClick(()=>{
			rewardIdx = 1;
			RewardCompleteWindow();
			okObj.SetActive(true);
			transform.FindChild("Reward").gameObject.SetActive(false);
			btnMovie[0].gameObject.SetActive(false);
			GV.RewardViewTime[0] = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(5).Ticks;
			Global.isNetwork = false;
			//Utility.LogWarning("OnWatchUnityAdClick " + GV.CurrADId);
		});



	}
	
	protected void OnWatch2(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		
		OnWatchVungleClick(()=>{
			rewardIdx = 2;
			btnMovie[1].gameObject.SetActive(false);
			GV.RewardViewTime[1] = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(5).Ticks;
			GV.CurrADId++; if(GV.CurrADId > GV.MaxADId) GV.CurrADId = GV.MaxADId+1;  else 	OnResetSlotItems();
			Global.isNetwork = false;
			//Utility.LogWarning("OnWatchVungleClick " + GV.CurrADId);
		});
	}
	
	protected void OnWatch3(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		OnWatch3Click(()=>{
			rewardIdx = 3;
			btnMovie[2].gameObject.SetActive(false);
			RewardCompleteWindow();
			okObj.SetActive(true);
			transform.FindChild("Reward").gameObject.SetActive(false);
			GV.RewardViewTime[2] = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(5).Ticks;
			Global.isNetwork = false;
			//Utility.LogWarning("OnWatch3Click " + GV.CurrADId);
		});
	}
	
	public void RewardCompleteWindow(){
		var tr = okObj.transform.FindChild("RewardInfo") as Transform;
		for(int i = 0; i < tr.childCount;i++){
			tr.GetChild(i).gameObject.SetActive(false);
		}
		int adID =GV.CurrADId;
		Common_Attend.Item rItem = Common_Attend.Get(adID);
		string str = null;
		switch(rItem.R_type){
		case 3 : str = "Coin";break;
		case 4 : str = "Dollar";break;
		case 8 : str = "Material";break;
		case 6 : str = "SilverBox";break;
		case 7 : str = "GoldBox";break;
		case 5 : str = "Cube";break;
		}
		var temp = tr.FindChild(str).gameObject as GameObject;
		temp.SetActive(true);
		temp.transform.FindChild("lbQuantity").GetComponent<UILabel>().text
			= string.Format("X {0}", rItem.R_no);
		if(rItem.R_type == 8){
			if(reMatId != 0) 	reMatId =UnityEngine.Random.Range(8600,8620);
			temp.transform.FindChild("img_Mat").GetComponent<UISprite>().spriteName = reMatId.ToString();
			int cnt = grid.childCount;
			for(int i = 0; i < cnt ; i++){
				grid.GetChild(i).GetComponent<SlotRewardItem>().ReSetMaterialRewardItems(reMatId);
			}
		}
	}
	
	protected void OnOKClick(){
		failObj.SetActive(false);
		transform.FindChild("Reward").gameObject.SetActive(true);
	
	}
	
	protected void OnReward(){
		if(GV.CurrADId >= GV.MaxADId){
			if(Global.isNetwork) Global.isNetwork = false;
		}
		if(Global.isNetwork) return;
		Global.isNetwork = true;

		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		int adID =GV.CurrADId;
		mDic.Add("rewardId",adID);
		
		Common_Attend.Item rItem = Common_Attend.Get(adID);
		string strValue = null;
		switch(rItem.R_type){
		case 3 :{
			mDic.Add("type",2);
			mDic.Add("value",rItem.R_no);
		}	break;
		case 4 :{
			mDic.Add("type",0);
			mDic.Add("value",rItem.R_no);
		}break;
		case 8 :{
			mDic.Add("type",3);
			//mDic.Add("value",reMatId);
			if(reMatId == 0) Utility.LogError("ReMatID is Zero");
			strValue = "value;"+reMatId.ToString()+":"+rItem.R_no.ToString();
		}break;
		case 6 : {
			//	str = "SilverBox";
			mDic.Add("type",4);
			mDic.Add("value",rItem.R_no);
		}break;
		case 7 : {
			//	str = "GoldBox";
			mDic.Add("type",5);
			mDic.Add("value",rItem.R_no);
			
		}break;
		case 5 : {
			//str = "Cube";
			mDic.Add("type",3);
			strValue = "value;8620:"+rItem.R_no.ToString(); 
			myAcc.instance.account.bInvenBTN[2] = true;
			//mDic.Add("value",rItem.R_no);
		}break;
		}
		
		string mAPI = ServerAPI.Get(90068);//"user/reward"
		NetworkManager.instance.HttpFormConnect("Put",mDic, mAPI ,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				RewardComplete(adID, rItem);
				myAcc.instance.account.bRewards[adID-8750] = true;
				int cuId = GV.CurrADId;
				GV.CurrADId++; 
				if(GV.CurrADId > GV.MaxADId){
					GV.CurrADId = GV.MaxADId+1;
					lbBtnTime[0].text = string.Empty;
					btnMovie[0].gameObject.SetActive(false);
					GV.RewardViewTime[0] = 0;
					OnResetItems(GV.MaxADId, GV.MaxADId);
				}else{
					OnResetItems(cuId, GV.CurrADId);
					btnMovie[0].gameObject.SetActive(false);
					GV.RewardViewTime[0] = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(5).Ticks;
					//.RewardViewTime[0] = 0;
				} 
				//else 	OnResetItems(cuId, GV.CurrADId);
				okObj.SetActive(false);
				transform.FindChild("Reward").gameObject.SetActive(true);
				GameObject.Find("Audio").SendMessage("CompleteSound");
				rewardIdx = 0;
			}
			Global.isNetwork = false;
		},strValue);
		
		
		
	}
	
	private int rewardIdx = 0;
	protected void OnWatch(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		int id = Common_Attend.Get(GV.CurrADId).AD;
		UserDataManager.instance.isPause = true;
		Utility.LogWarning("OnWhtch " + GV.CurrADId);
		switch(id){
		case 0: break;
		case 1: OnWatchUnityAdClick(()=>{
				OnChangeSlotItems();
				myAcc.instance.account.lastRewardViewTimes = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(5).Ticks;
				GV.CurrADId++; if(GV.CurrADId > GV.MaxADId) GV.CurrADId = GV.MaxADId+1; else 	OnResetSlotItems();
				Global.isNetwork = false;
				Utility.LogWarning("OnWatchUnityAdClick " + GV.CurrADId);
			});break;
		case 2: OnWatchVungleClick(()=>{
				OnChangeSlotItems();
				myAcc.instance.account.lastRewardViewTimes = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(5).Ticks;
				GV.CurrADId++; if(GV.CurrADId > GV.MaxADId) GV.CurrADId = GV.MaxADId+1;  else 	OnResetSlotItems();
				Global.isNetwork = false;
				Utility.LogWarning("OnWatchVungleClick " + GV.CurrADId);
			});break;
		case 3: OnWatch3Click(()=>{
				OnChangeSlotItems();
				myAcc.instance.account.lastRewardViewTimes = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(5).Ticks;
				GV.CurrADId++; if(GV.CurrADId > GV.MaxADId) GV.CurrADId = GV.MaxADId+1;  else 	OnResetSlotItems();
				Global.isNetwork = false;
				Utility.LogWarning("OnWatch3Click " + GV.CurrADId);
			});break;
		case 4: break;
		case 5: break;
		default:break;
		}
	}
	
	public void OnWatchUnityAdClick(System.Action callback){
		this.FinishCallback = callback;
		Utility.LogWarning("OnWatchUnityAdClick");
		if(!UnityAdsHelper.ShowAd(string.Empty, true, UnityAdResult)){
			transform.FindChild("Reward").gameObject.SetActive(false);
			failObj.SetActive(true);
			btnMovie[0].gameObject.SetActive(false);
			GV.RewardViewTime[0] = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(5).Ticks;
			Global.isNetwork = false;
			FinishCallback = null;
		}
	}
	
	public void OnWatchVungleClick(System.Action callback){
		this.FinishCallback = callback;
		Utility.LogWarning("OnWatchVungleClick");
		Vungle.playAd();
		Vungle.setSoundEnabled( true );
	}
	
	public void OnWatch3Click(System.Action callback){
		this.FinishCallback = callback;
		PlayAVideo("vzb4e54a5eb7714fb991");
		Utility.LogWarning("OnWatch3Click");
	}
	
	public void TapJoyStart(System.Action callback){
		this.FinishCallback = callback;
		Utility.LogWarning("TapJoyStart");
		FinishCallback();
	}
	
	private System.TimeSpan mCompareTime;
	
	private System.Action FinishCallback;
	private void UnityAdResult(UnityEngine.Advertisements.ShowResult result){
		switch (result)
		{
		case UnityEngine.Advertisements.ShowResult.Finished:
			Utility.Log("The ad was successfully shown.ADRewardContent ");
			if(FinishCallback != null)
				FinishCallback();
			FinishCallback = null;
			break;
		case UnityEngine.Advertisements.ShowResult.Skipped:
			Utility.Log("The ad was skipped before reaching the end.ADRewardContent");
			FinishCallback = null;
			Global.isNetwork = false;
			break;
		case UnityEngine.Advertisements.ShowResult.Failed:
			Utility.LogError("The ad failed to be shown.ADRewardContent");
			Global.isNetwork = false;
			FinishCallback = null;
			break;
		}
	}
	
	
	// When a video is available, you may choose to play it in any fashion you like.
	// Generally you will play them automatically during breaks in your game,
	// or in response to a user action like clicking a button.
	// Below is a method that could be called, or attached to a GUI action.
	public void PlayAVideo( string zoneID )
	{
		// Check to see if a video is available in the zone.
		if(AdColony.IsVideoAvailable(zoneID))
		{
			Utility.LogWarning("Play AdColony Video");
			// Call AdColony.ShowVideoAd with that zone to play an interstitial video.
			// Note that you should also pause your game here (audio, etc.) AdColony will not
			// pause your app for you.
			AdColony.ShowVideoAd(zoneID); 
		}
		else
		{
			Utility.LogWarning("Video Not Available");
			FinishCallback = null;
			transform.FindChild("Reward").gameObject.SetActive(false);
			failObj.SetActive(true);
			Global.isNetwork = false;

			rewardIdx = 3;
			btnMovie[2].gameObject.SetActive(false);
			GV.RewardViewTime[2] = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(5).Ticks;
		}
	}
	
	private void OnVideoFinished(bool ad_was_shown)
	{
		Utility.LogWarning("On Video Finished");
		if(FinishCallback != null)
			FinishCallback();
		FinishCallback = null;
		// Resume your app here.
	}
	
	// When a video is available, you may choose to play it in any fashion you like.
	// Generally you will play them automatically during breaks in your game,
	// or in response to a user action like clicking a button.
	// Below is a method that could be called, or attached to a GUI action.
	public void PlayV4VCAd( string zoneID, bool prePopup, bool postPopup )
	{
		// Check to see if a video for V4VC is available in the zone.
		if(AdColony.IsV4VCAvailable(zoneID))
		{
			Utility.LogWarning("Play AdColony V4VC Ad");
			// The AdColony class exposes two methods for showing V4VC Ads.
			// ---------------------------------------
			// The first `ShowV4VC`, plays a V4VC Ad and, optionally, displays
			// a popup when the video is finished.
			// ---------------------------------------
			// The second is `OfferV4VC`, which popups a confirmation before
			// playing the ad and, optionally, displays popup when the video 
			// is finished.
			
			// Call one of the V4VC Video methods:
			// Note that you should also pause your game here (audio, etc.) AdColony will not
			// pause your app for you.
			if (prePopup) AdColony.OfferV4VC( postPopup, zoneID );
			else  AdColony.ShowV4VC( postPopup, zoneID );
		}
		else
		{
			Utility.LogWarning("V4VC Ad Not Available");
		}
	}

	// The V4VCResult Delegate assigned in Initialize -- AdColony calls this after confirming V4VC transactions with your server
	// success - true: transaction completed, virtual currency awarded by your server - false: transaction failed, no virtual currency awarded
	// name - The name of your virtual currency, defined in your AdColony account
	// amount - The amount of virtual currency awarded for watching the video, defined in your AdColony account
	private void OnV4VCResult(bool success, string name, int amount)
	{
		if(success)
		{Global.isNetwork = false;
			Utility.LogWarning("V4VC SUCCESS: name = " + name + ", amount = " + amount);
			//this.currency += amount;
		}
		else
		{
			Utility.LogWarning("V4VC FAILED!");
		}
	}
	
	
	
	
	
	void OnEnable()
	{
	//	Vungle.onAdStartedEvent += onAdStartedEvent;
	//	Vungle.onAdEndedEvent += onAdEndedEvent;
	//	Vungle.onAdViewedEvent += onAdViewedEvent;
	//	Vungle.onCachedAdAvailableEvent += onCachedAdAvailableEvent;
		AdColony.OnVideoFinished+= this.OnVideoFinished;
		AdColony.OnV4VCResult += this.OnV4VCResult;
		
		if(GV.RewardViewTime[0] !=0 ){
			
		}else{
			lbBtnTime[0].text = string.Empty;
			btnMovie[0].gameObject.SetActive(true);
		}


	}
	
	void OnDisable()
	{
	//	Vungle.onAdStartedEvent -= onAdStartedEvent;
	//	Vungle.onAdEndedEvent -= onAdEndedEvent;
	//	Vungle.onAdViewedEvent -= onAdViewedEvent;
	//	Vungle.onCachedAdAvailableEvent -= onCachedAdAvailableEvent;
		AdColony.OnVideoFinished-= this.OnVideoFinished;
		AdColony.OnV4VCResult -= this.OnV4VCResult;


	}
	
	
	void onAdStartedEvent()
	{
		Utility.LogWarning( "onAdStartedEvent" );Global.isNetwork = false;
	}
	
	
	void onAdEndedEvent()
	{
		Utility.LogWarning( "onAdEndedEvent" );
		if(FinishCallback != null)
			FinishCallback();
		FinishCallback = null;
	}
	
	
	void onAdViewedEvent( double watched, double length )
	{
		Utility.LogWarning( "onAdViewedEvent. watched: " + watched + ", length: " + length );Global.isNetwork = false;
	}
	
	
	void onCachedAdAvailableEvent()
	{
		Utility.LogWarning( "onCachedAdAvailableEvent" );Global.isNetwork = false;
	}
	
	
	
	[SerializeField]public int reMatId = 0;
	
	private void RewardComplete(int id, Common_Attend.Item rItem){
		GameObject Lobby = GameObject.Find("LobbyUI") as GameObject;
		//Common_Attend.Item rItem = Common_Attend.Get(id);
		switch(rItem.R_type){
		case 3 :{
			GV.myCoin = GV.myCoin+rItem.R_no;
			Lobby.SendMessage("InitTopMenuReward", SendMessageOptions.DontRequireReceiver);
		}	break;
		case 4 :{
			GV.myDollar = GV.myDollar+rItem.R_no;
			Lobby.SendMessage("InitTopMenuReward", SendMessageOptions.DontRequireReceiver);
		}break;
		case 8 :{
			//str = "Material";
			GV.UpdateMatCount(reMatId, rItem.R_no);
			Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
			myAcc.instance.account.bInvenBTN[1] = true;
			reMatId = 0;
		}break;
		case 6 : {
			//	str = "SilverBox";
			GV.UpdateCouponList(0, rItem.R_no);	Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
			myAcc.instance.account.bInvenBTN[3] = true;
		}break;
		case 7 : {
			//	str = "GoldBox";
			GV.UpdateCouponList(1, rItem.R_no);	Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
			myAcc.instance.account.bInvenBTN[3] = true;
		}break;
		case 5 : {
			//str = "Cube";
			int num =8620;
			GV.UpdateMatCount(num, rItem.R_no);	Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
			myAcc.instance.account.bInvenBTN[2] = true;
		}break;
		}
	}

	IEnumerator TestHeyZap(){
		Utility.LogWarning("HZInterstitialAd fetch");
		Heyzap.HZInterstitialAd.Fetch();
		Heyzap.HZInterstitialAd.AdDisplayListener mL0 = delegate(string state, string tag) {
			Utility.LogWarning("HZInterstitialAd state " + state + " tag " + tag);
		};
		Heyzap.HZInterstitialAd.SetDisplayListener(mL0);
		yield return new WaitForSeconds(5.0f);
		if (Heyzap.HZInterstitialAd.IsAvailable()) {
			Utility.LogWarning("HZInterstitialAd show");
			Heyzap.HZInterstitialAd.Show();
		}else{
			Utility.LogWarning("HZInterstitialAd show isNot Availiable");
		}
		
		yield return new WaitForSeconds(10.0f);
		Utility.LogWarning("HZVideoAd fetch");
		Heyzap.HZVideoAd.Fetch();
		Heyzap.HZVideoAd.AdDisplayListener mL1 = delegate(string state, string tag) {
			Utility.LogWarning("hzvideoAd state " + state + " tag " + tag);
		};
		Heyzap.HZVideoAd.SetDisplayListener(mL1);
		yield return new WaitForSeconds(5.0f);
		if (Heyzap.HZVideoAd.IsAvailable()) {
			Utility.LogWarning("HZVideoAd show");
			Heyzap.HZVideoAd.Show();
		}else{
			Utility.LogWarning("HZVideoAd show isNot Availiable");
		}
		
		yield return new WaitForSeconds(10.0f);
		Heyzap.HZIncentivizedAd.Fetch();
		Heyzap.HZIncentivizedAd.AdDisplayListener mL2 = delegate(string state, string tag) {
			Utility.LogWarning("HZIncentivizedAd state " + state + " tag " + tag);
		};
		Heyzap.HZIncentivizedAd.SetDisplayListener(mL2);
		Utility.LogWarning("HZIncentivizedAd reward fetch");
		yield return new WaitForSeconds(5.0f);
		if (Heyzap.HZIncentivizedAd.IsAvailable()) {
			Heyzap.HZIncentivizedAd.Show();
			Utility.LogWarning("HZIncentivizedAd reward show");
		}else{
			Utility.LogWarning("HZIncentivizedAd reward show isNot");
		}
		
	}



}

