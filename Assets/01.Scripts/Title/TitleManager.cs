using UnityEngine;
using System.Collections;
//using System.Web;
public class TitleManager : MonoBehaviour {
	
	public GameObject LoadScene, Loading, Title, loadingbar;
	public GameObject tutorial,ErrorPopUp;
	public UILabel _text , _ex;
	public UISlider _slider;
	public GameObject titleLabel, loadingAni;
	public NewTitleScene titlescene;
	//Test
//	public GameObject DebugObj;
//	public bool isDebug = true;
	
	string loadingText1, loadingText2, loadingText3,loadingText4,loadingText5;
	

	void Awake(){
		_slider.transform.gameObject.SetActive(false);
		if(!Global.isRaceTest){
		}else{
			EncryptedPlayerPrefs.DeleteKey("Tutorial");
			EncryptedPlayerPrefs.DeleteKey("Agree");
			Application.LoadLevel("Splash");
			return;
		}
		
		Loading.transform.FindChild("lbPercentage").GetComponent<UILabel>().text  = string.Empty;
		QualitySettings.vSyncCount = 1;
		if(!Global.Loading){
			Global.Loading = true;
			Title.SetActive(true);
			FirstSceneLoad();
		}else{
			if(GV.bWeeklyResetFlag){
				UserDataManager.instance.DailySetTime();
				GV.bWeeklyResetFlag = false;
			}
			SceneManager.instance.settingRaceRenderSetting("Title");
			Global.isLoadFinish = false;
			Title.SetActive(false);
			Destroy(Title);
			ObjectManager.ClearListAIObject();
			System.GC.Collect();
			SecondSceneLoad();
		}
		
		AccountManager.instance.ErrorCallback = ()=>{
			ErrorPopUp.AddComponent<LogErrorAction>().InitLogWindow();
			ErrorPopUp.SetActive(true);
		};
		
	}
	
	public void SetLoadingText(){
		loadingText1 = KoStorage.GetKorString("70110");// "게임을 로딩합니다?.";
		loadingText2 =  KoStorage.GetKorString("70111");
		loadingText3 =  KoStorage.GetKorString("70112");
		loadingText4 = KoStorage.GetKorString("70113");
		loadingText5= KoStorage.GetKorString("70114");
	}
	void OnPopupClose(){
		ErrorPopUp.SetActive(false);
		StopAllCoroutines();
		Global.isNetwork = true;
		Application.Quit();
	}
	
	void builtInPopup(){
		ErrorPopUp.SetActive(true);
		ErrorPopUp.AddComponent<BITINPopup>().InitPopUp();
	}

	void DisConnectPopup(){
		isQuit = true;
		ErrorPopUp.SetActive(true);
		ErrorPopUp.AddComponent<BITINPopup>().InitNetworkPopUp();
	}

	void SNSFailedPopup(){
		ErrorPopUp.SetActive(true);
		ErrorPopUp.AddComponent<snsFailedPopup>().InitPopUp();
	}
	
	void newVersionpopup(){
	//	ErrorPopUp.SetActive(true);
	//	ErrorPopUp.AddComponent<newVersionPopup>();
		UserDataManager.instance.NewVersionpopup();
	}


	void OnRootPopup(){
		ErrorPopUp.SetActive(true);
		ErrorPopUp.AddComponent<BITINPopup>().OnRootPopup();
	}

	void emergencyNotic(){
		ErrorPopUp.SetActive(true);
		ErrorPopUp.AddComponent<BITINPopup>().InitPopUpEmergency();
	}
	
	void OverlapPopup(){
		ErrorPopUp.SetActive(true);
		ErrorPopUp.AddComponent<OverlapPopup>().InitPopUp();
	}
	void UserDataDeletePopup(){
		ErrorPopUp.SetActive(true);
		ErrorPopUp.AddComponent<BITINPopup>().DeleteInitPopUp();
	}
	void multiLoginPopup(){
		//	ErrorPopUp.SetActive(true);
		//	ErrorPopUp.AddComponent<multiLoginPopup>().InitPopUp(()=>{
		//		
		///	});
	}
	void UserBlockPopup(){
		ErrorPopUp.SetActive(true);
		ErrorPopUp.AddComponent<BITINPopup>().BlockInitPopUp();

	}
	void FirstSceneLoad(){
		LoadScene.SetActive(true);
		Loading.SetActive(false);
		ObjectManager.ClearListGameObject();
		//SceneManager.instance.LoadSlider = Loading.transform.FindChild("Slider").GetComponent<UISlider>();
		SceneManager.instance.LoadSlider =_slider;
		titlescene.transform.gameObject.SetActive(true);
		//titlescene.InitSetting();
	}
	
	void Start(){
		GameObject.Find("Audio").SendMessage("LobbyBGMInit");
	}
	// 게임 종료 후 로비로 되돌아 왔을 경우
	void SecondSceneLoad(){
	
		if(Global.gTutorial == 1){
			Global.gChampTutorial = 0;
			_ex.text = KoStorage.GetKorString("70118");
			Global.isTutorial = true;
			Global.isReTutorial = true;
			Global.gTutorial = 0;
			Loading.SetActive(false); Title.SetActive(false);
			tutorial.SetActive(true);
			var temp1 = tutorial.transform.FindChild("Tutorial_Lobby") as Transform;//gameObject.SetActive(true);
			temp1.gameObject.SetActive(true);
			Invoke("OnIsQuitActivation", 1.5f);
			return;
		}else{
			int n = Random.Range(0,29);
			if(n >= 5){
				//	n+= 71198; 
				int n1 = Random.Range(0,29);
				n = 71203+n1;
			}else{
				n+= 73007;
			}			
			//60355 60359
			loadingText1 = KoStorage.GetKorString(n.ToString());
			loadingText2 = KoStorage.GetKorString(n.ToString());
		}
		LoadScene.SetActive(true);
		Loading.SetActive(true);
		SceneManager.instance.LoadSlider = Loading.transform.FindChild("Slider").GetComponent<UISlider>();
		_ex = Loading.transform.FindChild("lbTip").GetComponent<UILabel>();
		_ex.text = loadingText1 ;
		ObjectManager.ClearListGameObject();
		AccountManager.instance.setTextEncrypt();
		//StartCoroutine("setTempData",_ex);
		StartCoroutine("LoadLobbyScene",_ex); 
	}
	
	// 공통 적용
	IEnumerator LoadLobbyScene(UILabel Tip){
		//	yield return AccountManager.instance.StartCoroutine("ReceiveGetPersonalInfo");
		//	while(isDebug){
		//		yield return null;
		//	}		
	//	Utility.LogWarning("LoadLobbyScene");
		titleLabel.SetActive(false);
		loadingAni.SetActive(false);
		AccountManager.instance.LobbySponTimeCheck();
	
		if(titlescene.transform.gameObject.activeSelf){
			isRanking = false;
			StartCoroutine("LoadingCircleBar");
		}
	
		yield return SceneManager.instance.StartCoroutine("LoadAdditveScene_1", "Lobby");
		if(titlescene.transform.gameObject.activeSelf){
			titlescene.secondAni();
			isRanking = true;
			while(titlescene.aniCheck){
				yield return null;
			}
		}
		yield return StartCoroutine("LoadingResourceLobby",0);
		LoadScene.SetActive(false);
		GameObject.Find("LobbyUI").GetComponent<LobbyManager>().LobbyLoadResource();
		titlescene.gameObject.SetActive(false);
		DestroyImmediate(LoadScene.transform.parent.gameObject);
	}


	
	IEnumerator LoadingCircleBar(){
		loadingbar.SetActive(true);
		//Utility.LogWarning("loading Title bar");
		var circle = loadingbar.transform.FindChild("icon").GetComponent<UISprite>() as UISprite;
		float del = 0.0f;
		while(!isRanking){
			del += 0.1f;
			circle.fillAmount = del;
			if(del >=1.0f){
				del = 0.0f;
			}
			yield return null;
		}
		loadingbar.SetActive(false);
	}
	
	bool isRanking = false;
	IEnumerator LoadingResourceLobby(float a){
		float pro = SceneManager.instance.LoadSlider.sliderValue;
		isRanking = false;bool isNet = false;	
		ObjectManager.CreateLobbyObject();
		AccountManager.instance.ErrorCallback = ()=>{
			AccountManager.instance.ErrorPopUp();
		};
		do{
			pro = pro + 0.1f;
			SceneManager.instance.LoadSlider.sliderValue = pro;
			if(pro >= 1.0f) {
				pro = 1.0f;
				isNet = true;
			}
			yield return new WaitForSeconds(0.02f);
		}while(!isNet);
		isRanking = true;
		yield break;
	}
	
	
	
	//string pushID = string.Empty;
	/*
	IEnumerator RequestServer(){
		yield return StartCoroutine("WebServerLogin");
		Global.isNetwork = true;
		AccountManager.instance.CheckAbVersion();
		do	{
			yield return null;
		} while(Global.isNetwork);
		Global.isNetwork = true;
		yield return StartCoroutine("RequestLocalLogin");
		titleLabel.SetActive(true);
		titleLabel.transform.GetChild(1).GetComponent<UILabel>().text =KoStorage.GetKorString("72410");
		yield return AccountManager.instance.StartCoroutine("ReceiveMyRaceAccount", _ex);
		Global.isNetwork = false;
		StartCoroutine("LoadLobbyReadyScene");
	}
	*/
	IEnumerator RequestServer(){
		//GameObject.Find("Audio").SendMessage("LobbyBGMInit");
		//	_ex.text = KoStorage.GetKorString("70115");
		yield return StartCoroutine("WebServerLogin");
		Global.isNetwork = true;
		AccountManager.instance.CheckAbVersion();
		do	{
			//_slider.sliderValue = AccountManager.instance.progressbarvalue;
			yield return null;
		} while(Global.isNetwork);
		//	_slider.sliderValue  = 0.0f;
		//	_ex.text = KoStorage.GetKorString("70117");
		Global.isNetwork = true;
		CheckLobby ();
		yield return StartCoroutine("RequestLocalLogin");
	
		yield return AccountManager.instance.StartCoroutine("ReceiveMyRaceAccount", _ex);
		Global.isNetwork = false;
		//	_slider.transform.gameObject.SetActive(true);
		StartCoroutine("LoadLobbyReadyScene");
	}

	void CheckLobby(){
		titleLabel.SetActive(true);
		string str = null;
		try{
			str = KoStorage.GetKorString("72410");
		}catch(UnityException e){
			str = "PIT STOP RACING";
		}
		titleLabel.transform.GetChild (1).GetComponent<UILabel> ().text = str;
	}
	
	private void TestTimeSetting(){
		System.DateTime cTime = NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		System.DateTime eTime_0 = new System.DateTime(cTime.Month,cTime.Month, cTime.Day, 12,0,10);
		System.DateTime eTime_1 = new System.DateTime(cTime.Month,cTime.Month, cTime.Day, 0,0,0);                     
		System.TimeSpan wTime = eTime_0 - eTime_1;
		Utility.LogWarning(string.Format("{0} / {1} /", wTime.TotalMinutes, wTime.TotalSeconds));
		//720.166666666667 / 43210 /
		wTime = eTime_1 - eTime_0;
		Utility.LogWarning(string.Format("{0} / {1} /", wTime.TotalMinutes, wTime.TotalSeconds));
		//-720.166666666667 / -43210 /
	}
	
	
	IEnumerator LoadLobbyReadyScene(){ 
		if(Global.gTutorial == 1){
			_ex.text =  KoStorage.GetKorString("70118");
			yield return new WaitForSeconds(0.1f);
			Loading.SetActive(false); Title.SetActive(false);
			tutorial.SetActive(true);
			Global.isTutorial = true;
			var temp1 = tutorial.transform.FindChild("Tutorial_Lobby") as Transform;//gameObject.SetActive(true);
			temp1.gameObject.SetActive(true);
			Global.gTutorial = 0;
			
			yield break;
		}
		_ex.text = loadingText1;
		SceneManager.instance.LoadSlider  = _slider;
		StartCoroutine("LoadLobbyScene", _ex);
	}
	
	
	
	IEnumerator WebServerLogin(){
		//_ex.text = loadingText4;
		yield return new WaitForSeconds(0.1f);
		if(string.IsNullOrEmpty(GV.UserRevId) == true){
			yield return new WaitForSeconds(1.0f);
		}
		Global.isNetwork = true;
		Utility.LogWarning("webserverlogin");
		NetworkManager.instance.Login(GV.UserRevId, Global.gDeivceID);
		do
		{
			yield return null;
		} while(Global.isNetwork);
	}
	
	void RequestReLogin(){
		StopAllCoroutines();
		Global.isNetwork = false;
		StartCoroutine("RequestServer");
	}
	
	void KakaoTalkLoginError(){
		StopAllCoroutines();
		Global.isNetwork = false;
		Title.GetComponent<MainTitle>().OnNext1Check();
	}
	
	IEnumerator RequestLocalLogin(){
		UserDataManager.instance.GettingUserInfoData();
		//GV.UserNick = KoStorage.GetKorString("70123"); 
		_ex.text = loadingText3;
		//		Utility.LogWarning("RLocalLogin " + GV.ChSeasonID);
		yield return new WaitForSeconds(1.5f);
		
	}
	
	
	
	void OnKakaoLoginProcess()
	{
		
	}


	bool isQuit = true;
	void FixedUpdate(){
		if (Input.GetKey(KeyCode.Escape)){
			//Utility.LogWarning("a");
			if(isQuit) return;
			isQuit = true;	
			if(Global.bLobbyBack){
			//	Utility.LogWarning("b");
				Invoke("OnIsQuitActivation", 1.0f);
				UserDataManager.instance.OnSubBack();
				return;
			}
			if(!UserDataManager.instance.bPopUpAdd)
				UserDataManager.instance.onGameExit();
			else UserDataManager.instance.setGameExit();
			Invoke("OnIsQuitActivation", 1.5f);
		}
	}
	void OnIsQuitActivation(){
		isQuit = false;
	}
	
	
	void KakaoToServerAppInfo(System.Uri uri){
		//	string nRet = HttpUtility.ParseQueryString(uri.Query).Get("nRet");
		//	int ret = 0;
		//	int.TryParse(nRet, out ret);
		//	if(ret == 1){
		//		Global.currentVesion = ProtocolManager.instance.GetUriStringQuery(uri,"version");
		//		string[] str = Global.currentVesion.Split('.');
		//		if(str[2] == "9"){
		//
		//	}else{
		//		Global.isNetwork = false;
		//	}
		//	}else if(ret == 0){
		//		ProtocolManager.instance.BitInPopUp();
		//	}
		//	Global.isNetwork = false;
	}
}



