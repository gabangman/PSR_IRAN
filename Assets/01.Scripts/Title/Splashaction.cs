using UnityEngine;
using System.Collections;

public class Splashaction : MonoBehaviour {

	private enum State { IDLE, KAKAO, TITLE, COMLOGO,CHANGE,FADEIN,FADEOUT};
	private State state;

//response:{"state":0,"msg":"sucess","totalPage":1,"myRank":1,"result":[{"userId":299,"record":70.18,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"3","tropy2":"0","tropy3":"0","raceData":null},{"userId":292,"record":78.64,"teamId":10,"carId":1002,"carClass":3102,"tropy1":"1","tropy2":"0","tropy3":"0","raceData":{"carId":1002,"teamId":10,"carClass":3102,"crewId":1200,"sponId":1300,"carAbility":279,"crewAbility":328,"userNick":"w","userURL":"https:\/\/s3-ap-northeast-1.amazonaws.com\/gabangman01\/MultiPicture\/MultiCom_1.png","Torque":38.2,"PitTime":16.28261,"BSPower":20,"BSTime":1.24,"TireTime":1.87,"GBLv":479507,"BSPressTime":38,"fG1":0,"fG2":1,"fG3":0,"fG4":0,"fG5":0,"fG6":0.6,"fG7":1,"fG8":0,"fG9":1,"fG10":1,"fG11":0,"fG12":0,"fG13":0,"fG14":0,"fG15":0,"fG16":0,"fG17":0,"fG18":0,"fG19":0,"fG20":0,"pD1":0,"pD2":0,"pD3":0,"pD4":0,"pD5":0,"pD6":0,"pD7":0,"pD8":0,"pD9":0,"pD10":0,"pD11":0,"pD12":0,"pD13":0,"pD14":0,"pD15":0,"pD16":0,"pD17":0,"pD18":0,"pD19":0,"pD20":0,"raceTime":78.64458}},{"userId":290,"record":80.12,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"4","tropy2":"0","tropy3":"0","raceData":null},{"userId":286,"record":85.73,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"1","tropy2":"0","tropy3":"0","raceData":{"carId":1000,"teamId":10,"carClass":3101,"crewId":1200,"sponId":1300,"carAbility":274,"crewAbility":387,"userNick":"GB","userURL":"https:\/\/s3-ap-northeast-1.amazonaws.com\/gabangman01\/MultiPicture\/MultiCom_4.png","Torque":34.2,"PitTime":15.7255,"BSPower":16,"BSTime":1.2,"TireTime":1.909,"GBLv":1,"BSPressTime":50,"fG1":0,"fG2":0.6,"fG3":0,"fG4":0.6,"fG5":0,"fG6":0,"fG7":0,"fG8":0,"fG9":0,"fG10":0,"fG11":0,"fG12":0,"fG13":0,"fG14":0,"fG15":0,"fG16":0,"fG17":0,"fG18":0,"fG19":0,"fG20":0,"pD1":2.364666,"pD2":3.762593,"pD3":38.50095,"pD4":5.15957,"pD5":2.951113,"pD6":3.145336,"pD7":3.475605,"pD8":0,"pD9":0,"pD10":0,"pD11":0,"pD12":0,"pD13":0,"pD14":0,"pD15":0,"pD16":0,"pD17":0,"pD18":0,"pD19":0,"pD20":0,"raceTime":85.73016}},{"userId":300,"record":120.2,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"10","tropy2":"0","tropy3":"0","raceData":null}],"time":1452842734}

//response:{"state":0,"msg":"sucess","totalPage":1,"myRank":1,"result":[{"userId":300,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"10","tropy2":"0","tropy3":"0","raceData":null},{"userId":299,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"3","tropy2":"0","tropy3":"0","raceData":null},{"userId":292,"teamId":10,"carId":1002,"carClass":3102,"tropy1":"1","tropy2":"0","tropy3":"0","raceData":{"carId":1002,"teamId":10,"carClass":3102,"crewId":1200,"sponId":1300,"carAbility":279,"crewAbility":328,"userNick":"w","userURL":"https:\/\/s3-ap-northeast-1.amazonaws.com\/gabangman01\/MultiPicture\/MultiCom_1.png","Torque":38.2,"PitTime":16.28261,"BSPower":20,"BSTime":1.24,"TireTime":1.87,"GBLv":479507,"BSPressTime":38,"fG1":0,"fG2":1,"fG3":0,"fG4":0,"fG5":0,"fG6":0.6,"fG7":1,"fG8":0,"fG9":1,"fG10":1,"fG11":0,"fG12":0,"fG13":0,"fG14":0,"fG15":0,"fG16":0,"fG17":0,"fG18":0,"fG19":0,"fG20":0,"pD1":0,"pD2":0,"pD3":0,"pD4":0,"pD5":0,"pD6":0,"pD7":0,"pD8":0,"pD9":0,"pD10":0,"pD11":0,"pD12":0,"pD13":0,"pD14":0,"pD15":0,"pD16":0,"pD17":0,"pD18":0,"pD19":0,"pD20":0,"raceTime":78.64458}},{"userId":290,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"4","tropy2":"0","tropy3":"0","raceData":null},{"userId":286,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"1","tropy2":"0","tropy3":"0","raceData":{"carId":1000,"teamId":10,"carClass":3101,"crewId":1200,"sponId":1300,"carAbility":274,"crewAbility":387,"userNick":"GB","userURL":"https:\/\/s3-ap-northeast-1.amazonaws.com\/gabangman01\/MultiPicture\/MultiCom_4.png","Torque":34.2,"PitTime":15.7255,"BSPower":16,"BSTime":1.2,"TireTime":1.909,"GBLv":1,"BSPressTime":50,"fG1":0,"fG2":0.6,"fG3":0,"fG4":0.6,"fG5":0,"fG6":0,"fG7":0,"fG8":0,"fG9":0,"fG10":0,"fG11":0,"fG12":0,"fG13":0,"fG14":0,"fG15":0,"fG16":0,"fG17":0,"fG18":0,"fG19":0,"fG20":0,"pD1":2.364666,"pD2":3.762593,"pD3":38.50095,"pD4":5.15957,"pD5":2.951113,"pD6":3.145336,"pD7":3.475605,"pD8":0,"pD9":0,"pD10":0,"pD11":0,"pD12":0,"pD13":0,"pD14":0,"pD15":0,"pD16":0,"pD17":0,"pD18":0,"pD19":0,"pD20":0,"raceTime":85.73016}}],"time":1452842657}

	void StartSplash(){
		fadeIn();
		if(Global.gReLoad == 1){
			LOGO=kakaoLogo;
			state = State.KAKAO;
		}else{
			LOGO=comlogo;
			state = State.COMLOGO;
		}

	//	state = State.TITLE;
	//	StartCoroutine("NextScene");
		islogo = false;
		Global.isRaceTest = false;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		checkPackageProcess();
	}

	void Awake(){
		//Global.gReLoad = 0;
		//GameObject.Find("FBManager").SendMessage("FBInit");
		//StartSplash();
		Global.isRaceTest = false;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		checkPackageProcess();
		SceneManager.instance.StartCoroutine("LoadFirstScene","Title");
		return;
	}
	
	void checkPackageProcess(){
		System.Diagnostics.Process.GetProcesses();
	}

	void ChangeScene(){
		switch(state){
		case State.IDLE:
			break;
		case State.KAKAO:
		{
			state = State.IDLE;
			fadeOut();
			KakaoTalkLogo();
		}break;
		case State.TITLE:
		{
			state = State.IDLE;
			fadeOut();
			if(Application.platform == RuntimePlatform.Android)
				//SceneManager.instance.CheckOBB();
				SceneManager.instance.StartCoroutine("LoadFirstScene","Title");
			else if(Application.platform == RuntimePlatform.IPhonePlayer){
				SceneManager.instance.StartCoroutine("LoadFirstScene","Title");
			}else {
				SceneManager.instance.StartCoroutine("LoadFirstScene","Title");
				//SceneManager.instance.StartCoroutine("loadLocalLevel");

			}
		}break;
		case State.COMLOGO:
		{
			state = State.IDLE;
			fadeIn();
			CompanyLogo();
		}break;

		case State.FADEIN:{
			fadeIn();
			state = State.FADEOUT;
		}
			break;
		case State.FADEOUT:{
			fadeOut();
			LOGO=comlogo;
			state = State.COMLOGO;
		}
			break;
		case State.CHANGE:
			break;
		default:
			break;
		}
	}
	
	void KakaoTalkLogo(){
		StartCoroutine("MonitorDelay");
	}
	
	IEnumerator MonitorDelay(){
		yield return new WaitForSeconds(0.0002f);
		state = State.FADEIN;
		fadeIn();

	}

	void CompanyLogo(){
		StartCoroutine("NextScene");
	}
	
	IEnumerator NextScene(){
		yield return new WaitForSeconds(0.001f);
		state = State.TITLE;
		fadeIn();
		
	}
	
	IEnumerator NextScene01(){
		yield return new WaitForSeconds(0.002f);
		//	state = State.TITLE;
		//	fadeIn();
		if(Application.platform == RuntimePlatform.Android)
			//SceneManager.instance.CheckOBB();
			SceneManager.instance.StartCoroutine("LoadFirstScene","Title");
		else if(Application.platform == RuntimePlatform.IPhonePlayer){
			SceneManager.instance.StartCoroutine("LoadFirstScene","Title");
		}else {
			SceneManager.instance.StartCoroutine("LoadFirstScene","Title");
			//SceneManager.instance.StartCoroutine("loadLocalLevel");
			
		}
	}

	#region onGUI
	public Texture2D blackTexture;
	private float alpha = 1f;
	private float fadeDir = 1.0f; 
	private bool m_bStop;
	private float tempalpha;
	private bool m_balphainit;
	public Texture2D comlogo;
	public Texture2D kakaoLogo;

	public Texture2D LOGO;

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
		if(alpha == 1 || alpha == 0){
			//Utility.Log ("Test "  + alpha);
			ChangeScene();
		}
	}
	
	void  fadeIn(){ 
		fadeDir = 1;  
		//m_bFadeIn = false;
		m_balphainit = false;
		return;
	} 
	
	void  fadeOut(){ 
		fadeDir = -1;    
		//m_bFadOut = false;
		m_balphainit = false;
		return;
		
	} 
	bool islogo = true;
	void OnGUI(){
		alpha += fadeDir * Mathf.Clamp01(Time.deltaTime);
		StopState(); 
		// 잠시 화면을 반투명하게 어둡게 하는 효과가 있다.
		// 정지 효과시 화면을 약간 어둡게 만들기 위해 만들었다. 
		//GUI.depth = drawDepth; 
		//Utility.Log ("Test");
		if(islogo){
			GUI.color = new Color(0,0,0,alpha);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture); 
		}else{
		//GUI.color = new Color(0,0,0,0);
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), LOGO); 
		}
		//GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), LOGO); 
		return;
	}
	#endregion
}
