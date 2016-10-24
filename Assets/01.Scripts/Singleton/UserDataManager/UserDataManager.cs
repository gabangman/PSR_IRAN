using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using System.Globalization;


public partial class UserDataManager : MonoSingleton< UserDataManager > {
	public float UIVolume = 1.0f;

	public string modeAICarID = string.Empty;
	public string modeAICrewID = string.Empty;
	public float delatSpeed = 0.75f;
	public int multipleBoost = 2;
	private JiverResponse jiver;
	bool isSave = false;

	void Awake(){
		DontDestroyOnLoad(gameObject);
		gameObject.AddComponent<ObjectManager>();
		pauseTime =NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		gameObject.AddComponent<JiverResponse>();
		jiver = gameObject.GetComponent<JiverResponse>();
		gameObject.AddComponent<SendBird>();
	}

	void Start(){
	
	}


	int sponsorTime = 0;

	void GetLocalAccountInfo(){
		//GV.gVIP = 0;
	}


	void GetUserPersonalInfo(){
		Global.isNetwork = false;

	
	}
	public IEnumerator ReceiveGetPersonalInfo(){
		Global.isNetwork = true;
		GetUserPersonalInfo();
		do	{
			yield return null;
		} while(Global.isNetwork);

		yield return true;
	}

	public string networkStatus;
	public bool isClear = false;
	public int weeklyResultRank = 0;
	public void GettingUserInfoData(){
		if(isClear) {
			EncryptedPlayerPrefs.DeleteAll();
			Global.isFeaturedReset = true;
			isClear = false;
		}
		myAccount.instance = new myAccount();
		myAccount.instance.GetAccountInfo();
		myAcc.instance = new myAcc();
		myAcc.instance.GetAccountInfo();
		if(Application.isEditor){
			EncryptedPlayerPrefs.DeleteKey("Quest");
		}
		QuestData.instance = new QuestData();
		QuestData.instance.GetQuestInfo();

		SettingTime();
		isSave = true;
		GetLocalAccountInfo();
	}
	public string dailyTimes(){
	
		System.DateTime currentTime =NetworkManager.instance.GetCurrentDeviceTime();// System.DateTime.Now;
		comTime = dayCheckTime - currentTime;
		if(comTime.Seconds <= 0){
			dayCheckTime = dayCheckTime.AddDays(1);
			GV.bWeeklyResetFlag = true;
			return string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
		}else{

			return  string.Format("{0:00}:{1:00}:{2:00}", comTime.Hours, comTime.Minutes, comTime.Seconds);
		}
	
	}

	void ReSetEventTrack(){
		int tId = 0;
		int oldID = myAccount.instance.account.eRace.featuredTrackID;
		if(oldID == 0) oldID = 1401;
		/*if(GV.ChSeasonID >= 6000 && GV.ChSeasonID < 6005 ){
			tId = 0;
		}else if(GV.ChSeasonID >= 6005 && GV.ChSeasonID < 6010 ){
			tId = 1401;
		}else if(GV.ChSeasonID >= 6010 && GV.ChSeasonID < 6015 ){
			tId = (int)Well512.Next(1401,1403);
		}else if(GV.ChSeasonID >= 6015 && GV.ChSeasonID < 6020 ){
			tId = (int)Well512.Next(1401,1404);
		}else if(GV.ChSeasonID >= 6020 && GV.ChSeasonID < 6025 ){
			tId = (int)Well512.Next(1401,1405);
		}else if(GV.ChSeasonID >= 6025 && GV.ChSeasonID < 6030 ){
			tId = (int)Well512.Next(1401,1406);
		}else{
			tId = (int)Well512.Next(1401,1407);

		}*/

		if(GV.ChSeasonID >= 6000 && GV.ChSeasonID < 6005 ){
			tId = 0;
		}else if(GV.ChSeasonID >= 6005 && GV.ChSeasonID < 6010 ){
			tId = 1401;
			
		}else if(GV.ChSeasonID >= 6010 && GV.ChSeasonID < 6015 ){
			tId =(int)Well512.Next(1401,1403); //1402
			for(int i = 0; i < 50; i++){
				if(oldID== tId){
					tId = (int)Well512.Next(1401,1403); //1402
				}else{
					break;
				}
			}
		}else if(GV.ChSeasonID >= 6015 && GV.ChSeasonID < 6020 ){
			tId = (int)Well512.Next(1401,1404); //1403
			for(int i = 0; i < 50; i++){
				if(oldID== tId){
					tId = (int)Well512.Next(1401,1404); //1403
				}else{
					break;
				}
			}
		}else if(GV.ChSeasonID >= 6020 && GV.ChSeasonID < 6025 ){
			tId =(int)Well512.Next(1401,1405); //1404
			for(int i = 0; i < 50; i++){
				if(oldID == tId){
					tId = (int)Well512.Next(1401,1405); //1
				}else{
					break;
				}
			}
		}else if(GV.ChSeasonID >= 6025 && GV.ChSeasonID < 6030 ){
			tId =  (int)Well512.Next(1401,1406); //1405
			for(int i = 0; i < 50; i++){
				if(oldID== tId){
					tId = (int)Well512.Next(1401,1406); //1405
				}else{
					break;
				}
			}
		}else{
			tId = (int)Well512.Next(1401,1407); //1406
			for(int i = 0; i < 50; i++){
				if(oldID== tId){
					tId =(int)Well512.Next(1401,1407); //1406
				}else{
					break;
				}
			}
		}
		myAccount.instance.account.eRace.featuredTrackID =tId;

	}
	private System.DateTime dayCheckTime;
	private System.TimeSpan  comTime;
	public void DailySetTime(){
		GV.bWeeklyReset = false;
		long lastTime =NetworkManager.instance.GetCurrentDeviceTime().Ticks;// System.DateTime.Now.Ticks;
		myAccount.instance.account.lastConnectTime = lastTime;
		long dayTime = myAccount.instance.account.dayCheckTime;
		System.DateTime mNowTime   = NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		System.DateTime mResetTime = new System.DateTime(dayTime);
		dayCheckTime = mResetTime.AddDays(1);
		System.TimeSpan mCompareTime = (mResetTime - mNowTime);
		int a = (int)mCompareTime.TotalHours / 24;

		if(a != 0) {
			a = Mathf.Abs(a);
			if(a > 10){
				System.DateTime day =  NetworkManager.instance.GetCurrentDeviceTime();
				long dayTimes = new System.DateTime(day.Year, day.Month, day.Day,0,0,0).Ticks;
				mResetTime = new System.DateTime(dayTimes).AddDays(1);
				dayCheckTime = mResetTime;
			}else{
				System.DateTime rTime = mResetTime.AddDays((double)a);
				mResetTime = rTime;
			}

			/*System.DateTime day =  NetworkManager.instance.GetCurrentDeviceTime();
			long dayTimes = new System.DateTime(day.Year, day.Month, day.Day,0,0,0).Ticks;
			mResetTime = new System.DateTime(dayTimes).AddDays(1);
			dayCheckTime = mResetTime;
		*/

			myAccount.instance.account.dayCheckTime = mResetTime.Ticks;
			GV.bDayReset = true;
			GV.QuestReset = true;
			myAcc.instance.account.currAdId = 8750;
			GV.CurrADId = 8750;
			myAcc.instance.account.lastRewardViewTimes = 0;
			myAcc.instance.account.bRewards = new bool[15];
			myAccount.instance.account.eRace.EventDayReset();
			ReSetEventTrack();
			Global.gAttend = 1;
			myAcc.instance.account.bPopEvent = true;
			GV.bWeeklyReset = true;
			myAccount.instance.account.weeklyRace.WeeklyPlayCount = 0;
			EncryptedPlayerPrefs.SetInt("DealerBuy",10);
			int mA = UnityEngine.Random.Range(1,4);
			EncryptedPlayerPrefs.SetInt("DealerCount",mA);
		}

		//myAcc.instance.account.bInvenBTN[1] = false;
	
	}

	void TimeTest(){
		System.DateTime mNowTime   = NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		dayCheckTime = mNowTime.AddSeconds(15);
		//Utility.LogWarning("dailyTime1 " + dayCheckTime);
	}
	public void SettingTime(){
		DailySetTime();
		long dayTime = myAccount.instance.account.weeklyCheckTime;
		System.DateTime mResetTime = new System.DateTime(dayTime);
		System.DateTime _tTime;
		System.DateTime mNowTime   = NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		System.TimeSpan mCompareTime = (mNowTime-mResetTime);
		if(mCompareTime.TotalDays >= 7){
			if(mNowTime.DayOfWeek == DayOfWeek.Monday){
				myAccount.instance.account.weeklyCheckTime = new System.DateTime(mNowTime.Year, mNowTime.Month, mNowTime.Day,0,0,0).Ticks;
			}else{
				int f = (int)mNowTime.DayOfWeek -(int)DayOfWeek.Sunday;
				if(f == 0) {
					_tTime = mNowTime.AddDays((double)(-6));
				}else {
					_tTime = mNowTime.AddDays((double)(-f));
					_tTime = _tTime.AddDays(1);
				}
				myAccount.instance.account.weeklyCheckTime = new System.DateTime(_tTime.Year, _tTime.Month, _tTime.Day,0,0,0).Ticks;
			}
		}

		/*
		System.DateTime mT = System.DateTime.Now;
		System.DateTime wT, _tTime;
		mT  = new System.DateTime(2015,6,30,0,1,1);
		if(mT.DayOfWeek == DayOfWeek.Monday){
			wT = new System.DateTime(mT.Year, mT.Month, mT.Day,0,0,0);
		}else{
			int f = (int)mT.DayOfWeek -(int)DayOfWeek.Sunday;
			if(f == 0) {
				_tTime = mT.AddDays((double)(-6));
			}else {
				_tTime = mT.AddDays((double)(-f));
				_tTime = _tTime.AddDays(1);
			}

			wT = new System.DateTime(_tTime.Year, _tTime.Month, _tTime.Day,0,0,0);
		}
		Utility.LogWarning(wT);
		mCompareTime =  new System.DateTime(2015,7,6,0,1,1) - wT; 
		Utility.LogWarning(mCompareTime.TotalDays);
	//	mResetTime =  new System.DateTime(mNowTime.Year, mNowTime.Month, mNowTime.Day,0,0,0);
		*/
	}

	void SponsorTimeCheck(){
		/*long SponTime = myAccount.instance.account.sponsorInfo.expireTime;
		if(SponTime <= 0) return ;
		System.DateTime eTime = new System.DateTime(SponTime);
		//eTime = eTime.AddHours(1);
		System.DateTime cTime = System.DateTime.Now;
		System.TimeSpan wTime = eTime - cTime;
		if(wTime.Minutes <=0){
			//Global.MySponsorID =Base64Manager.instance.GlobalEncoding(1300);
			GV.setTeamSponID(GV.SelectedTeamID, 1300);
			myAccount.instance.account.sponsorInfo.sponsorId = 1300;
			//myAccount.instance.account.sponsorInfo.expireTime = 0;
		}else{
		
		}*/
	}


	public void myGameDataSave(){
		if(myAccount.instance != null) 	myAccount.instance.SaveAccountInfo();
		if(myAcc.instance != null)  myAcc.instance.SaveAccountInfo();
		if(GAchieve.instance != null) 
			GAchieve.instance.SaveAchievementInfo();
		if(ClubSponInfo.instance != null) ClubSponInfo.instance.SaveSponInfo();
		if(QuestData.instance != null) QuestData.instance.SaveQuestInfo();
		//Utility.LogWarning("Save myGameDataSave");
	}
	
	public void StartShowTip(string _text){
		return;
	}

	void OnDestroy(){
		ObjectManager.ClearListGameObject();
		//ClearTexture();
		if(isSave){ 
			myGameDataSave();
		}
		gameRank.instance.InstanceNull();
		EncryptedPlayerPrefs.Save();
		System.GC.Collect(); 	
		myAccount.instance = null;
	}

	public void OnSaveMyData(string name){
		if(name.Equals("Cancle")) return;
		if(isSave){ 
			myGameDataSave();
		}
		EncryptedPlayerPrefs.Save();
		System.GC.Collect(); 
		Utility.LogError("OnSaveMyData");
	}
	

	void OnApplicationQuit(){
#if UNITY_ANDROID && !UNITY_EDITOR
		myGameDataSave();
		EncryptedPlayerPrefs.Save();
		Utility.LogError("Save OnApplicatonQuit");
#endif
		OnSubBack = null;
		OnSubBacksub = null;
		OnSubBacksubsub = null;
		OnSubBackMenu = null;
		_subStatus = null;
	}
	
	public bool isPause = false;
	bool isWaitTime = false;
	System.DateTime pauseTime;// = 0;
	public void PauseGame(){
		pauseTime = NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		isWaitTime = false;
	}

	public void ReSumeGame(){
		System.DateTime day =NetworkManager.instance.GetCurrentDeviceTime();// System.DateTime.Now;
		System.TimeSpan _span = day - pauseTime;
		if(isPause){
			isPause = false;
		}else{
			if((_span.Minutes ) > 15 || (_span.Minutes < 0)){
				isWaitTime = true;
				pauseTime = NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
			}
			gameResume(isWaitTime);
		}
	}


	public bool isTapjoy = false;
	public void TapjoyRewardCheck(){
		if(isTapjoy) {
			StartCoroutine("CheckTapJoyCurrency");
		}else{

		}

	}

	public void TapJoyLoadingBar(){
	
	}


	void gameResume(bool b){
		//Utility.LogWarning("modify - gameResume");
		if(!b) return;
		return;
		GameObject.Find("ManagerGroup").SendMessage("GroupDestroy");
		Global.Loading = false;
		Global.isLobby = true;
		Global.isLoadFinish = false;
		Global.gReLoad = 1;
		isPause = false;
		GV.gInfo = null;
		Global.gChampTutorial = 0;
		Application.LoadLevel("Splash");

	}

/*
	List<Texture2D> TexInviteList = new List<Texture2D>();
	public void SaveSetTexture(int idx, Texture2D tex){
		tex.name = idx.ToString();
		TexInviteList.Add (tex);
	}
	public Texture2D GetTexture(int idx){
		Texture2D t = TexInviteList.Find((tex)=> tex.name == idx.ToString());
		return t;
	}	
	public void ClearTexture(){
		if(TexInviteList.Count == 0) return;
		//TexInviteList.ForEach(texture => DestroyImmediate(texture,true));
		TexInviteList.Clear();
		Resources.UnloadUnusedAssets();
	}
	*/

	
	void OnDisable(){

	}

	//public System.Action publicCallback;
	public bool isQualitySettingValue;
	public delegate void ChangeSetting(bool isSetting);
	public void ApplySetting(ChangeSetting _changesetting, bool isSetting){
		_changesetting(isSetting);
	}
	
	public delegate bool SubmodeStatus();
	public SubmodeStatus _subStatus = null;
	public bool SubmodeActive(){
	
		bool ischeck = false;
		if(_subStatus != null){
			ischeck = _subStatus();
			_subStatus = null;
			//Utility.LogWarning("SubmodeActive");
		}else ischeck = false;
		return ischeck;
	}

	public System.Action OnSubBack;
	public System.Action OnSubBacksub;
	public System.Action OnSubBacksubsub;
	public System.Action OnSubBackMenu;
	public bool bPopUpAddNetwork = false;
	public bool bPopUpAdd = false;
	public int version = 0;
	public void onGameExit(){
		//if(string.Equals(version.ToString(),"3")==true){
		if(version == 2 || version ==3){
			Vibration.OnNoticQuit();
			return;
		}
		if(NetworkManager.instance.failedObject != null){
			NetworkManager.instance.failedObject.SetActive(false);
		}
		if(bPopUpAdd) return;
		bPopUpAdd = true;
		var temp = ObjectManager.CreatePrefabs("Window", "popUp_add") as GameObject;
		exitObj = temp;
		var obj = GameObject.Find("LobbyUI")	 as GameObject;
		if(obj != null){
			var par  = GameObject.FindGameObjectWithTag("BottomAnchor");
			temp.transform.parent = par.transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = new Vector3(0,360,-1500);
			temp.transform.localEulerAngles = Vector3.zero;
			temp.gameObject.SetActive(true);
			temp.GetComponent<GamePopup>().InitGameExit();
			return;
		}
		obj = GameObject.Find("LoadScene");
		if(obj != null){
			var par = obj.transform.FindChild("Camera").FindChild("Anchor") as Transform;
			temp.transform.parent = par.transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = new Vector3(0,0,-1500);
			temp.transform.localEulerAngles = Vector3.zero;
			temp.gameObject.SetActive(true);
			temp.GetComponent<GamePopup>().InitGameExit();
			return;
		}
		obj = GameObject.Find("GUIManager");
		if(obj != null){
			var par = GameManager.instance.getBTN() as Transform;
			temp.transform.parent = par.transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = new Vector3(0,0,0);
			temp.transform.localEulerAngles = Vector3.zero;
			temp.gameObject.SetActive(true);
			temp.GetComponent<GamePopup>().InitGameExit();
			return;
		}

	}

	public void onPermissionExit(){
		var temp = ObjectManager.CreatePrefabs("Window", "popUp_add") as GameObject;
		var obj = GameObject.Find("LoadScene") as GameObject;
		if(obj != null){
			var par = obj.transform.FindChild("Camera").FindChild("Anchor") as Transform;
			ObjectManager.ChangeObjectParent(temp, par);
			ObjectManager.ChangeObjectPosition(temp, new Vector3(0,0,-1500), Vector3.one, Vector3.zero);
			temp.gameObject.SetActive(true);
			temp.GetComponent<GamePopup>().InitNotics();
			return;
		}
	}
	private GameObject exitObj;
	public void setGameExit(){
		if(NetworkManager.instance.failedObject != null) NetworkManager.instance.failedObject.SetActive(true);
		bPopUpAdd = false;
		if(exitObj != null) DestroyImmediate(exitObj);
		exitObj = null;
	}

	public void OnRacePause(){
		if(exitObj != null) return;
		var temp = ObjectManager.CreatePrefabs("Window", "popUp_add") as GameObject;
		exitObj = temp;
		var obj = GameObject.Find("GUIManager");
		if(obj != null){
			var par = GameManager.instance.getBTN() as Transform;
			temp.transform.parent = par.transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = new Vector3(0,0,-100);
			temp.transform.localEulerAngles = Vector3.zero;
			temp.gameObject.SetActive(true);
			temp.GetComponent<GamePopup>().RacePause();
			return;
		}
	}

	public void NewVersionpopup(){
		if(exitObj != null) return;
		var temp = ObjectManager.CreatePrefabs("Window", "popUp_add") as GameObject;
		exitObj = temp;
		var obj = GameObject.Find("LoadScene") as GameObject;
		if(obj != null){
			var par = obj.transform.FindChild("Camera").FindChild("Anchor") as Transform;
			temp.transform.parent = par.transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = new Vector3(0,0,-1500);
			temp.transform.localEulerAngles = Vector3.zero;
			temp.gameObject.SetActive(true);
			temp.GetComponent<GamePopup>().InitNewVersion();
			return;
		}
	}




}

