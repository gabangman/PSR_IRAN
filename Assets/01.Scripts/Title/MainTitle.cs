using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
public class MainTitle : MonoBehaviour
{
	public GameObject TitleMgr, Anchor_Down;
	public GameObject tutorial;
	public TextAsset koTableAsset; 
	public UILabel lbtitle;
	string KoTableString;

	public TextAsset[] LanuguageTables;


	//public MovieTexture bandMovie;
	private enum State { IDLE, START, LOBBY};
	private State state;
	public bool isClear = false;
	//bool isLoading = false;

	void OnEnable()
	{
			fadeIn();
			state = State.START;
	}
	
	void Start()
	{
	
		Anchor_Down = gameObject.transform.GetChild(0).gameObject;
	
	
	}
	
	
	void ChangeScene()
	{
		switch(state)
		{
		case State.IDLE:
			break;
			
		case State.LOBBY:
		{
			state = State.IDLE;
			break;
		}
			
		case State.START:
		{
			state = State.IDLE;
			fadeOut();
			TweenStart();

			break;
		}
			
		default:
			break;
		}
	}
	
	void TweenStart()
	{
		StartCoroutine("GameInfo");
		return;
		var bg = Anchor_Down.transform.GetChild(0).gameObject; //title_bg
		var title = Anchor_Down.transform.GetChild(1).gameObject; //title
		var menu = Anchor_Down.transform.GetChild(2).gameObject; //Menu
		
		TweenPosition[] menuTween = menu.GetComponents<TweenPosition>();
		menuTween[0].Reset();
		menuTween[0].enabled = true;
		
		
		for(int i = 0; i < bg.transform.childCount;i++){
			StartTweenPosition(bg.transform.GetChild(i).gameObject, 0);
			StartTweenColor(bg.transform.GetChild(i).gameObject, 0);
		}
		
		for(int i = 0; i < title.transform.childCount;i++){
			StartTweenPosition(title.transform.GetChild(i).gameObject, 0);
		}
	}
	
	
	void StartTweenPosition(GameObject obj, int order)
	{
		TweenPosition[] tween = obj.GetComponents<TweenPosition>();
		//Utility.Log (tween.Length);
		if(order == 3)
		{
			tween[1].eventReceiver = gameObject;
			tween[1].callWhenFinished = "OnNextCheck";
			//tween[1].duration = 0.2f;
			tween[1].Reset();
			tween[1].enabled = true;
			return;
		}
		
		if (tween.Length != 0)
		{
			tween[order].Reset();
			tween[order].enabled = true;
		}
		else
		{
			return;
		}
	}


	IEnumerator GameInfo(){
		if(Application.isEditor){
			SetCoutryCode();
		}

		GameObject.Find("LoadScene").SendMessage("OnIsQuitActivation");
		Global.isNetwork = true;
		NetworkManager.instance.DoGetHostAddresses();
		NetworkManager.instance.SetGameID();
		NetworkManager.instance.GetGameInfo();
	//	NetworkManager.instance.GameInfo();

		while(Global.isNetwork){
			yield return null;
		}
		int status = 0;
		KoStorageSet();
		if(Application.platform == RuntimePlatform.Android){
			Vibration.requestInfo();
			while(!Global.bPermission){
				yield return null;
			}
		}
		GV.bLoding = false;
		Global.isNetwork = true;
		if(Application.isEditor){
			EncryptedPlayerPrefs.DeleteKey("mUserId");
			EncryptedPlayerPrefs.DeleteKey("mNick");
		}
		if(EncryptedPlayerPrefs.HasKey("mUserId")){
			status = 0;
			GV.UserRevId = EncryptedPlayerPrefs.GetString("mUserId");
			if(!EncryptedPlayerPrefs.HasKey("mNick")){
				NetworkManager.instance.AccountInfo((mRe)=>{
					Global.isNetwork = false;
				});
			}else if(string.IsNullOrEmpty(EncryptedPlayerPrefs.GetString("mNick")) == true){
				NetworkManager.instance.AccountInfo((mRe)=>{
					Global.isNetwork = false;
				});
			}else Global.isNetwork = false; 
		}else{
			NetworkManager.instance.AccountInfo((mRe)=>{
				if(mRe == 1){
					status = 2003;
					EncryptedPlayerPrefs.DeleteKey("achievement");
					EncryptedPlayerPrefs.DeleteKey("Account_1");
					EncryptedPlayerPrefs.DeleteKey("Account");
				}else if(mRe == 2){
					status = 0;
				}else {
					Utility.LogError("GetUesrAccount Error!!!");
				}
				Global.isNetwork=  false;
	
			});
			
			
		}
		
		while(Global.isNetwork){
			yield return null;
		}


	//	EncryptedPlayerPrefs.DeleteKey("achievement");
		int ver01 = 0;
		int ver02 = 0;
		string str = Global.gVersion;
		str = str.Replace(".","");
	//	Utility.LogWarning(str);
		int.TryParse(str,out ver01);
		
		str = GV.gInfo.clientVer;
		str = str.Replace(".","");
	//	Utility.LogWarning(str);
		int.TryParse(str,out ver02);
		if(ver02 <= ver01){
			
		}else {
			Global.isNetwork = true;
			GameObject.Find("LoadScene").SendMessage("newVersionpopup");
		}
		/*if(GV.gInfo.clientVer.Equals(Global.gVersion) == true){
		//	Global.isNetwork = true;
		//	GameObject.Find("LoadScene").SendMessage("newVersionpopup");
		}else{
			//Global.isNetwork = true;
			//GameObject.Find("LoadScene").SendMessage("newVersionpopup");
		}*/

		while(Global.isNetwork){
			yield return null;
		}
		/*
		if(GV.gRootPhone == true){
			GameObject.Find("LoadScene").SendMessage("OnRootPopup");
			while(GV.gRootPhone){
				yield return null;
			}
		}
		*/
		while(!Global.bGearPress){
			yield return null;
		}
		if(Application.isEditor){
			//Global.gTutorial = 1;
		//	SetCoutryCode();
		//	KoStorageSet();
			if(status == 0){
			//	Anchor_Down.transform.FindChild("Menu").FindChild("KaKao").gameObject.SetActive(false);
			//	Anchor_Down.transform.FindChild("Menu").FindChild("Load").gameObject.SetActive(true);
				OnKakaoInitialized();
				//GameObject.Find("LoadScene").SendMessage("showRegisterNick");
			}else if(status == 2003){
				// 사용자 등록이 안될 경우
				//Global.gTutorial = 1;
				EncryptedPlayerPrefs.DeleteKey("Agree");
				StartCoroutine("setAgreeCheck");
			}
		}else if(Application.platform == RuntimePlatform.Android){
		//	KoStorageSet();
			StartCoroutine("GoogleLoginPrecess", status);
			
		}else if(Application.platform == RuntimePlatform.IPhonePlayer){
		//	KoStorageSet();
			StartCoroutine("IPhoneLoginPrecess", status);
		}
		//lbtitle.text = KoStorage.GetKorString("72410");
		//Utility.LogWarning("GetGameInfo Success  " + GV.UserRevId + "  " + Global.gDeivceID);
	}



	public void OnNextCheck()
	{ // 누구누구님 환영합니다. 환영인사

		StartCoroutine("GameInfo");       
		return;

	}

	public void OnNext1Check(){
		Anchor_Down.transform.FindChild("Menu").FindChild("KaKao").gameObject.SetActive(true);
	}

	IEnumerator setAgreeCheck(){
	//	Anchor_Down.transform.FindChild("Menu").FindChild("KaKao").gameObject.SetActive(false);
		if(!EncryptedPlayerPrefs.HasKey("Agree")){
			tutorial.SetActive(true);
			var temp1 = tutorial.transform.FindChild("Agreement_Start") as Transform;//gameObject.SetActive(true);
			temp1.gameObject.SetActive(true);
			Global.isAnimation = true;
			while(Global.isAnimation){
				yield return null;
			}
		}
		GameObject.Find("LoadScene").SendMessage("goRegistor");
		Anchor_Down.transform.FindChild("Menu").FindChild("Load").gameObject.SetActive(true);
	}
	IEnumerator GoogleLoginPrecess(int code){
		/*Global.isNetwork = true;

		Social.localUser.Authenticate((bool success) => {
			if (success) {
				Utility.LogWarning("Authenticate 1");
				EncryptedPlayerPrefs.SetString("GoogleLogin", "Success");
			} else {
				Utility.LogWarning("Authenticate 2");
				EncryptedPlayerPrefs.SetString("GoogleLogin", "Failed");
			}
			Global.isNetwork = false;
		});*/

		/*
		if(!EncryptedPlayerPrefs.HasKey("GoogleLogin")){
			Social.localUser.Authenticate((bool success) => {
				if (success) {
					Utility.LogWarning("Authenticate 1");
					EncryptedPlayerPrefs.SetString("GoogleLogin", "Success");
				} else {
					Utility.LogWarning("Authenticate 2");
					EncryptedPlayerPrefs.SetString("GoogleLogin", "Failed");
				}
				Global.isNetwork = false;
			});
			
		}else{
			string name = EncryptedPlayerPrefs.GetString("GoogleLogin");
			if(name.Equals("Success")){
				((PlayGamesPlatform)Social.Active).Authenticate((bool success)=>{
					if (success) {
					} else {
					}
					Global.isNetwork = false;
				},false);
			}else{
				Global.isNetwork = false;
			}
			
		}
		while(Global.isNetwork){
			yield return null;
		}
		*/
		if(code== 0){
			//Anchor_Down.transform.FindChild("Menu").FindChild("KaKao").gameObject.SetActive(false);
		//	Anchor_Down.transform.FindChild("Menu").FindChild("Load").gameObject.SetActive(true);
			OnKakaoInitialized();
			//GameObject.Find("LoadScene").SendMessage("showRegisterNick");
		}else if(code== 2003){
			GameObject.Find("LoadScene").SendMessage("goRegistor");
		//	Anchor_Down.transform.FindChild("Menu").FindChild("Load").gameObject.SetActive(true);
		}else Utility.LogError("return code : " + code);

		yield return null;
	}
	/*
	IEnumerator GoogleLoginPrecess(int code){
		Global.isNetwork = true;
		if(!EncryptedPlayerPrefs.HasKey("GoogleLogin")){
				PlayGamesPlatform.Instance.Authenticate((bool success)=>{
					if (success) {
						Utility.LogWarning("Authenticate 1");
						EncryptedPlayerPrefs.SetString("GoogleLogin", "Success");
					} else {
						Utility.LogWarning("Authenticate 2");
						EncryptedPlayerPrefs.SetString("GoogleLogin", "Failed");
					}
					Global.isNetwork = false;
			},false);

		}else{
			string name = EncryptedPlayerPrefs.GetString("GoogleLogin");
			if(name.Equals("Success")){
				((PlayGamesPlatform)Social.Active).Authenticate((bool success)=>{
					if (success) {
					} else {
					}
					Global.isNetwork = false;
				},false);
			}else{
				Global.isNetwork = false;
			}

		}
		while(Global.isNetwork){
			yield return null;
		}

		if(code== 0){
			Anchor_Down.transform.FindChild("Menu").FindChild("KaKao").gameObject.SetActive(false);
			Anchor_Down.transform.FindChild("Menu").FindChild("Load").gameObject.SetActive(true);
			OnKakaoInitialized();
			GameObject.Find("LoadScene").SendMessage("showRegisterNick");
		}else if(code== 2003){
			GameObject.Find("LoadScene").SendMessage("goRegistor");
			Anchor_Down.transform.FindChild("Menu").FindChild("Load").gameObject.SetActive(true);
		}else Utility.LogError("return code : " + code);
	}
	*/
	IEnumerator IPhoneLoginPrecess(int code){
		Global.isNetwork = true;
		if(!EncryptedPlayerPrefs.HasKey("IPhoneLogin")){
			EncryptedPlayerPrefs.SetString("IPhoneLogin","Success");
			Global.isNetwork = false;
		}else{
			Global.isNetwork = false;
		}
		while(Global.isNetwork){
			yield return null;
		}
		
		if(code== 0){
		//	Anchor_Down.transform.FindChild("Menu").FindChild("KaKao").gameObject.SetActive(false);
		//	Anchor_Down.transform.FindChild("Menu").FindChild("Load").gameObject.SetActive(true);
			OnKakaoInitialized();
			//GameObject.Find("LoadScene").SendMessage("showRegisterNick");
		}else if(code== 2003){
			//Global.gTutorial = 1;
			GameObject.Find("LoadScene").SendMessage("goRegistor");
		//	Anchor_Down.transform.FindChild("Menu").FindChild("Load").gameObject.SetActive(true);
		}else Utility.LogError("return code : " + code);
	
	}

	void SetCoutryCode(){
		GV.gNationName = "KOR";
		EncryptedPlayerPrefs.DeleteKey("CountryCode");
		if(EncryptedPlayerPrefs.HasKey("CountryCode")){
			Global.gCountryCode = EncryptedPlayerPrefs.GetString("CountryCode");
			if(string.IsNullOrEmpty(Global.gCountryCode)==true){
				Global.gCountryCode ="KOR";
				EncryptedPlayerPrefs.SetString("CountryCode",Global.gCountryCode);
			}
		}else{
			Global.gCountryCode ="KOR";
			EncryptedPlayerPrefs.SetString("CountryCode",Global.gCountryCode);
		}
	//	Global.gCountryCode = "IDN";
	//	EncryptedPlayerPrefs.SetString("CountryCode",Global.gCountryCode);
	//	Utility.LogWarning(Global.gCountryCode);
		switch(Global.gCountryCode){
		case "KOR":
			GV.gNational = "Nflag_korea";break;
		case "JPN":
			GV.gNational = "Nflag_japan";break;
		case "CHN":
			GV.gNational = "Nflag_china";break;
		case "FRA":
			GV.gNational = "Nflag_france";break;
	//	case "TUR":
	//		GV.gNational = "Nflag_turkey";break;
		case "RUS":
			GV.gNational = "Nflag_rusia";break;
		case "USA":
			GV.gNational = "Nflag_eng";break;
		case "GBR":
			GV.gNational = "Nflag_eng";break;
		case "DEU":
			GV.gNational = "Nflag_germany";break;
		case "ITA":
			GV.gNational = "Nflag_italy";break;
		case "ESP":
			GV.gNational = "Nflag_spain";break;
		case "PRT":
			GV.gNational = "Nflag_portugal";break;
		case "IDN":
			GV.gNational = "Nflag_indonesia";break;
		case "MYS":
			GV.gNational = "Nflag_eng";break;
		default: {
			GV.gNational = "Nflag_eng";
			Utility.LogWarning("No Contry " +Global.gCountryCode);break;
		}
			
		}

	}

	void KoStorageSet(){
		string CountryString = string.Empty;
		string gCountry =  EncryptedPlayerPrefs.GetString("CountryCode");
		switch(gCountry){
		case "KOR":
			CountryString = koTableAsset.text;break;
		case "JPN":
			CountryString = LanuguageTables[0].text;break;
		case "CHN":
			CountryString = LanuguageTables[1].text;break;
		case "FRA":
			CountryString=LanuguageTables[2].text;break;
	//	case "TUR":
	//		CountryString= LanuguageTables[3].text;break;
		case "RUS":
			CountryString= LanuguageTables[4].text;break;
		case "USA":
			CountryString = LanuguageTables[5].text;break;
		case "GBR":
			CountryString=LanuguageTables[5].text;break;
		case "DEU":
			CountryString = LanuguageTables[7].text;break;
		case "ITA":
			CountryString =LanuguageTables[8].text;break;
		case "PRT":
			CountryString =LanuguageTables[9].text;break;
		case "ESP":
			CountryString =LanuguageTables[10].text;break;
		case "IDN":
			CountryString =LanuguageTables[11].text;break;
		case "MYS":
			CountryString =LanuguageTables[5].text;break;
		default: {
			CountryString=LanuguageTables[5].text;break;
		}
		}
	//	KoTableString = koTableAsset.text;
		KoStorage ko = gameObject.AddComponent<KoStorage>();
		ko.SetDataFile(CountryString);
		koTableAsset = null;LanuguageTables = null;
		TitleMgr.GetComponent<TitleManager>().SetLoadingText();
	}


	IEnumerator LoadNextScene()
	{
		yield return new WaitForSeconds(0.5f);
		TitleMgr.GetComponent<TitleManager>().StartCoroutine("RequestServer");
	}
	
	
	void OnKakaoInitialized()
	{
			StartCoroutine("LoadNextScene");
	}

	void OnKakaoClick() {
		Anchor_Down.transform.FindChild("Menu").FindChild("KaKao").gameObject.SetActive(false);
		Anchor_Down.transform.FindChild("Menu").FindChild("Load").gameObject.SetActive(true);
		OnKakaoInitialized();
	}
	
	
	void StartTweenColor(GameObject obj, int order)
	{
		TweenColor[] tween = obj.GetComponents<TweenColor>();
		
		if(tween.Length == 0)
			return;
		
		if(order == 3)
		{
			tween[order].duration = 1.2f;
			tween[order].eventReceiver = gameObject;
			tween[order].callWhenFinished = "OnNextClick";
		}
		
		tween[order].Reset();
		tween[order].enabled = true;
	}
	
	void StartTweenAlpha(GameObject obj, int order)
	{
		TweenAlpha[] tween = obj.GetComponents<TweenAlpha>();
		if(tween.Length == 0)
			return;
		
		if(order == 3)
		{
			tween[order].to = 1f;
			tween[order].from = 0f;
			tween[order].duration = 1.2f;
			tween[order].eventReceiver = gameObject;
			tween[order].callWhenFinished = "OnNextClick";
		}
		
		tween[order].Reset();
		tween[order].enabled = true;
	}

	/*
	Invitation mInvitation = null;
	TurnBasedMatch mMatching = null;
	void OnInvitationReceived(Invitation invitation, bool shouldAutoAccept){
		if(shouldAutoAccept){
		}else{
			mInvitation = invitation;
		}
	}
	void OnGotMatch(TurnBasedMatch match, bool shouldAutoLauch){
		if(shouldAutoLauch){
			OnMatchStarted(true, match);
		}else{
			mMatching = match;
		}
	}
	
	void OnMatchStarted(bool success, TurnBasedMatch match) {
		if (success) {
			byte[] myData = match.Data;
			bool canPlay = (match.Status == TurnBasedMatch.MatchStatus.Active &&
			                match.TurnStatus == TurnBasedMatch.MatchTurnStatus.MyTurn);
		} else {
		}
	}*/



	#region onGUI
	
	public Texture2D blackTexture;
	private float alpha = 1f;
	private float fadeDir = 1.0f; 
	private bool m_bStop;
	private float tempalpha;
	private bool m_balphainit;

	void StopState()
	{
		if(m_bStop)
		{
			if(!m_balphainit)
			{
				tempalpha = alpha;
				m_balphainit = true;
			}
			
			alpha = Mathf.Clamp01(0.5f);  
		}
		else
		{
			if(m_balphainit)
			{
				m_balphainit = false;
				alpha = tempalpha;
			}
			alpha = Mathf.Clamp01(alpha);  
		}
		
		if(alpha == 1 || alpha == 0)
		{
			//Utility.Log ("Test "  + alpha);
			ChangeScene();
		}
	}
	
	void  fadeIn()
	{ 
		fadeDir = 1;  
		m_balphainit = false;
		return;
	} 
	
	void  fadeOut()
	{ 
		fadeDir = -1;    
		m_balphainit = false;
		return;
	}
	
//	bool islogo = true;	
	void OnGUI()
	{
		alpha += fadeDir * Mathf.Clamp01(Time.deltaTime);
		StopState(); 
		GUI.color = new Color(0,0,0,alpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture); 
		return;
	}
	#endregion
	
}
