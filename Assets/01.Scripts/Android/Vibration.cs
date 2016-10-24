using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaObject vibrationObj = VibrationActivity.activityObj.Get<AndroidJavaObject>("vibration");
#elif UNITY_IOS

 #else
    private static AndroidJavaObject vibrationObj;
 #endif

//	public static void OnGPGCInit(){
	//	if (Application.platform == RuntimePlatform.Android)
	//		VibrationActivity.gpgsObj.Call("Initialize");
//	}

	public static bool IsFBAppInstalled() 
	{ 
		string FBpackageName = "com.facebook.katana"; 
		AndroidJavaClass up = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" ); 
		AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>( "currentActivity" ); 
		AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>( "getPackageManager" ); 
		AndroidJavaObject appList = packageManager.Call<AndroidJavaObject>( "getInstalledPackages", 0 ); 
		for (int i = 0; i < appList.Call<int>( "size" ); ++i) 
		{ 
			AndroidJavaObject appInfo = appList.Call<AndroidJavaObject>( "get", i ); 
			if (appInfo.Get<string>( "packageName" ) == FBpackageName) 
			{ 
				return true; 
			} 
		} 
		return false; 
	}

	public static void OnUnRegister(){
		#if UNITY_ANDROID && !UNITY_EDITOR
		VibrationActivity.activityObj.Call("PushUnRegister");
		#elif UNITY_IOS
		
		#else

		#endif
	}

	public static void OnRequestDevice(){
		#if UNITY_ANDROID && !UNITY_EDITOR
		VibrationActivity.activityObj.Call("myDeviceInfo");
		#elif UNITY_IOS
		
		#else
		
		#endif
	}

	public static void onShowLog(string str){
		#if UNITY_ANDROID && !UNITY_EDITOR
		VibrationActivity.activityObj.Call("ShowLogUnity", str);
		#elif UNITY_IOS
		
		#else

		#endif
	}

	public static void OnSetBack(){
		#if UNITY_ANDROID && !UNITY_EDITOR
		VibrationActivity.activityObj.Call("OnSetBack", "OnBack");
		#elif UNITY_IOS
		
		#else
		
		#endif
	}

	public static void OnRequestPushID(){
		#if UNITY_ANDROID && !UNITY_EDITOR

		#elif UNITY_IOS
		
		#else
			Global.isNetwork = false;
			Global.gPushID = "pushID_Editor";
		#endif
	}

	public static void OnSponsorTime(){
		#if UNITY_ANDROID && !UNITY_EDITOR
		if(Global.gPushable == 0) return;
			VibrationActivity.activityObj.Call("OnSetLocalAlram", KoStorage.GetKorString("76101"), KoStorage.GetKorString("76100"), KoStorage.GetKorString("76102"));
		#elif UNITY_IOS
			if(!NotificationServices.enabledRemoteNotificationTypes) return;
			LocalNotification lo = new LocalNotification();
			lo.fireDate = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(1);
			//lo.fireDate = KoStorage.GetKorString("76100");
			NotificationServices.ScheduleLocalNotification(lo);
		#else

		#endif
	}

	public static void OnSetClubFinishTime(int rt){
		int flag = EncryptedPlayerPrefs.GetInt("ClubAlram");
		if(flag == 0){
			#if UNITY_ANDROID && !UNITY_EDITOR
			if(Global.gPushable == 0) return;
			//VibrationActivity.activityObj.Call("OnSetClubFinishAlram", "CLUB0", "CLUB1","CLUB2", rt.ToString());
			VibrationActivity.activityObj.Call("OnSetClubFinishAlram", KoStorage.GetKorString("71000"), KoStorage.GetKorString("73527"), KoStorage.GetKorString("73526"),rt.ToString());
			#elif UNITY_IOS
			if(!NotificationServices.enabledRemoteNotificationTypes) return;
			LocalNotification lo = new LocalNotification();
			lo.fireDate = NetworkManager.instance.GetCurrentDeviceTime().AddMinutes(1);
			//lo.fireDate = KoStorage.GetKorString("76100");
			NotificationServices.ScheduleLocalNotification(lo);
			#else
			
			#endif
			EncryptedPlayerPrefs.SetInt("ClubAlram",1);
		}else{

		}
	}
	public static void OnQuit(){
		#if UNITY_ANDROID && !UNITY_EDITOR
			if(Global.gPushable == 0) return;
			VibrationActivity.activityObj.Call("OnUnityQuit",KoStorage.GetKorString("71115"), KoStorage.GetKorString("70128"),KoStorage.GetKorString("70129"));
		#elif UNITY_IOS
		
		#else
		
		#endif
	}


	public static void requestInfo(){
		#if UNITY_ANDROID && !UNITY_EDITOR
			if(Global.gPushable == 0) return;
			VibrationActivity.activityObj.Call("RequestAccount");
		#elif UNITY_IOS
		
		#else
		
		#endif
	}

	public static void OnNoticQuit(){
		#if UNITY_ANDROID && !UNITY_EDITOR
		VibrationActivity.activityObj.Call("OnPermissonCancle");
		#elif UNITY_IOS
		
		#else
		Application.Quit();
		#endif

	}
    public static void Vibrate()
    {
		#if UNITY_ANDROID && !UNITY_EDITOR
		vibrationObj.Call("vibrate");
		#elif UNITY_IOS
		Handheld.Vibrate();
		#else
		
		#endif
    }

    public static void Vibrate(long milliseconds)
    {
		#if UNITY_ANDROID && !UNITY_EDITOR
		vibrationObj.Call("vibrate", milliseconds);
		#elif UNITY_IOS
		//Handheld.Vibrate((float)milliseconds);
		Handheld.Vibrate();
		#else
		
		#endif
           
    }
	/*		
	 * void Start(){
	 * string str = "eyJzaWduYXR1cmUiID0gIkE0U3ZSUFFSWWthWUpCNUI2QTBObUxzVUdHU1poOGZxUzdzWmZIRVdHKzhLd0JiMFZhWk5MU1d4MkRTdStuYmcyZUJTOFRuWUZFOVFmcGpldXRQdmY5d3ZEaElLSDBoY3V4dnUwaWRPS015RnllOE1FT0hrZlpaV3pDT0hxd2hhYkVsWUV2bitXZWlXRHdFbEl5Nkx3R2s2cHpGY3JBM0wrNGhIaE9sS1E0YUN6YzFWVEF2V0UwamtKOGlyRlZYMUQvUTdYUzBjUmY5aEp6Q1M0MDZ4bDdvdkhVeVlQdUVwQlk3bWhhUCtCWmMwSTg1SFAyR24vZlUzbGx0WWI4eFhDNjVINHVGdHU0VXU1WStzRTl0Nk5STklDR0x4ckZRbWtrc21BM0R4cHJES25YRGFjZ24wdXNnME1pMW5EUGJVN3RsTnhGNE8yeTR5UUdRYnBycnlFRHNBQUFXQU1JSUZmRENDQkdTZ0F3SUJBZ0lJRHV0WGgrZWVDWTB3RFFZSktvWklodmNOQVFFRkJRQXdnWll4Q3pBSkJnTlZCQVlUQWxWVE1STXdFUVlEVlFRS0RBcEJjSEJzWlNCSmJtTXVNU3d3S2dZRFZRUUxEQ05CY0hCc1pTQlhiM0pzWkhkcFpHVWdSR1YyWld4dmNHVnlJRkpsYkdGMGFXOXVjekZFTUVJR0ExVUVBd3c3UVhCd2JHVWdWMjl5YkdSM2FXUmxJRVJsZG1Wc2IzQmxjaUJTWld4aGRHbHZibk1nUTJWeWRHbG1hV05oZEdsdmJpQkJkWFJvYjNKcGRIa3dIaGNOTVRVeE1URXpNREl4TlRBNVdoY05Nak13TWpBM01qRTBPRFEzV2pDQmlURTNNRFVHQTFVRUF3d3VUV0ZqSUVGd2NDQlRkRzl5WlNCaGJtUWdhVlIxYm1WeklGTjBiM0psSUZKbFkyVnBjSFFnVTJsbmJtbHVaekVzTUNvR0ExVUVDd3dqUVhCd2JHVWdWMjl5YkdSM2FXUmxJRVJsZG1Wc2IzQmxjaUJTWld4aGRHbHZibk14RXpBUkJnTlZCQW9NQ2tGd2NHeGxJRWx1WXk0eEN6QUpCZ05WQkFZVEFsVlRNSUlCSWpBTkJna3Foa2lHOXcwQkFRRUZBQU9DQVE4QU1JSUJDZ0tDQVFFQXBjK0IvU1dpZ1Z2V2grMGoyak1janVJandLWEVKc3M5eHAvc1NnMVZoditrQXRlWHlqbFViWDEvc2xRWW5jUXNVbkdPWkh1Q3pvbTZTZFlJNWJTSWNjOC9XMFl1eHNRZHVBT3BXS0lFUGlGNDFkdTMwSTRTallOTVd5cG9ONVBDOHIwZXhOS2hERXBZVXFzUzQrM2RINWdWa0RVdHdzd1N5bzFJZ2ZkWWVGUnI2SXd4Tmg5S0JneEhWUE0za0xpeWtvbDlYNlNGU3VIQW5PQzZwTHVDbDJQMEs1UEIvVDV2eXNIMVBLbVBVaHJBSlFwMkR0NyttZjcvd212MVcxNnNjMUZKQ0ZhSnpFT1F6STZCQXRDZ2w3WmNzYUZwYVllUUVHZ21Kam00SFJCenNBcGR4WFBRMzNZNzJDM1ppQjdqN0FmUDRvN1EwL29tVllIdjRnTkpJd0lEQVFBQm80SUIxekNDQWRNd1B3WUlLd1lCQlFVSEFRRUVNekF4TUM4R0NDc0dBUVVGQnpBQmhpTm9kSFJ3T2k4dmIyTnpjQzVoY0hCc1pTNWpiMjB2YjJOemNEQXpMWGQzWkhJd05EQWRCZ05WSFE0RUZnUVVrYVNjL01SMnQ1K2dpdlJOOVk4MlhlMHJCSVV3REFZRFZSMFRBUUgvQkFJd0FEQWZCZ05WSFNNRUdEQVdnQlNJSnhjSnFiWVlZSXZzNjdyMlIxbkZVbFNqdHpDQ0FSNEdBMVVkSUFTQ0FSVXdnZ0VSTUlJQkRRWUtLb1pJaHZkalpBVUdBVENCL2pDQnd3WUlLd1lCQlFVSEFnSXdnYllNZ2JOU1pXeHBZVzVqWlNCdmJpQjBhR2x6SUdObGNuUnBabWxqWVhSbElHSjVJR0Z1ZVNCd1lYSjBlU0JoYzNOMWJXVnpJR0ZqWTJWd2RHRnVZMlVnYjJZZ2RHaGxJSFJvWlc0Z1lYQndiR2xqWVdKc1pTQnpkR0Z1WkdGeVpDQjBaWEp0Y3lCaGJtUWdZMjl1WkdsMGFXOXVjeUJ2WmlCMWMyVXNJR05sY25ScFptbGpZWFJsSUhCdmJHbGplU0JoYm1RZ1kyVnlkR2xtYVdOaGRHbHZiaUJ3Y21GamRHbGpaU0J6ZEdGMFpXMWxiblJ6TGpBMkJnZ3JCZ0VGQlFjQ0FSWXFhSFIwY0RvdkwzZDNkeTVoY0hCc1pTNWpiMjB2WTJWeWRHbG1hV05oZEdWaGRYUm9iM0pwZEhrdk1BNEdBMVVkRHdFQi93UUVBd0lIZ0RBUUJnb3Foa2lHOTJOa0Jnc0JCQUlGQURBTkJna3Foa2lHOXcwQkFRVUZBQU9DQVFFQURhWWIweTQ5NDFzckIyNUNsbXpUNkl4RE1JSmY0RnpSamI2OUQ3MGEvQ1dTMjR5Rnc0QlozK1BpMXk0RkZLd04yN2E0L3Z3MUxuekxyUmRyam44ZjVIZTVzV2VWdEJOZXBobUdkdmhhSUpYblk0d1BjL3pvN2NZZnJwbjRaVWhjb09Bb09zQVFOeTI1b0FRNUgzTzV5QVg5OHQ1L0dpb3FiaXNCL0tBZ1hObnJmU2VtTS9qMW1PQytSTnV4VEdmOGJncFB5ZUlHcU5LWDg2ZU9hMUdpV29SMVpkRVdCR0xqd1YvMUNLblBhTm1TQU1uQmpMUDRqUUJrdWxoZ3dIeXZqM1hLYWJsYkt0WWRhRzZZUXZWTXB6Y1ptOHc3SEhvWlEvT2piYjlJWUFZTU5wSXI3TjRZdFJIYUxTUFFqdnlnYVp3WEc1NkFlemxIUlRCaEw4Y1RxQT09IjsKInB1cmNoYXNlLWluZm8iID0gImV3b0pJbTl5YVdkcGJtRnNMWEIxY21Ob1lYTmxMV1JoZEdVdGNITjBJaUE5SUNJeU1ERTJMVEE0TFRFMklEQTFPak01T2pFNUlFRnRaWEpwWTJFdlRHOXpYMEZ1WjJWc1pYTWlPd29KSW5WdWFYRjFaUzFwWkdWdWRHbG1hV1Z5SWlBOUlDSTVPR1l5Tmprek1ETXlaamswWldNMFpHVTBZakF3Tm1Ka05UYzJOV1EwWWpBME16a3pPVFZsSWpzS0NTSnZjbWxuYVc1aGJDMTBjbUZ1YzJGamRHbHZiaTFwWkNJZ1BTQWlNVEF3TURBd01ESXlPVGc1TnpBNU1pSTdDZ2tpWW5aeWN5SWdQU0FpTVM0eExqZ2lPd29KSW5SeVlXNXpZV04wYVc5dUxXbGtJaUE5SUNJeE1EQXdNREF3TWpJNU9EazNNRGt5SWpzS0NTSnhkV0Z1ZEdsMGVTSWdQU0FpTVNJN0Nna2liM0pwWjJsdVlXd3RjSFZ5WTJoaGMyVXRaR0YwWlMxdGN5SWdQU0FpTVRRM01UTTFNVEUxT1RjMk9TSTdDZ2tpZFc1cGNYVmxMWFpsYm1SdmNpMXBaR1Z1ZEdsbWFXVnlJaUE5SUNKQ1FUbEVRa1l5T1MxR05UVTFMVFEyUWtVdE9VRkRRUzAzTTBSQk9VUTBORE0xT0RnaU93b0pJbkJ5YjJSMVkzUXRhV1FpSUQwZ0luQnBkSE4wYjNCeVlXTnBibWN1WXpnMU1ESWlPd29KSW1sMFpXMHRhV1FpSUQwZ0lqRXhNamMzTXpVeU56a2lPd29KSW1KcFpDSWdQU0FpWTI5dExtZGhZbUZ1WjIxaGJuTjBkV1JwYnk1d2FYUnpkRzl3Y21GamFXNW5JanNLQ1NKd2RYSmphR0Z6WlMxa1lYUmxMVzF6SWlBOUlDSXhORGN4TXpVeE1UVTVOelk1SWpzS0NTSndkWEpqYUdGelpTMWtZWFJsSWlBOUlDSXlNREUyTFRBNExURTJJREV5T2pNNU9qRTVJRVYwWXk5SFRWUWlPd29KSW5CMWNtTm9ZWE5sTFdSaGRHVXRjSE4wSWlBOUlDSXlNREUyTFRBNExURTJJREExT2pNNU9qRTVJRUZ0WlhKcFkyRXZURzl6WDBGdVoyVnNaWE1pT3dvSkltOXlhV2RwYm1Gc0xYQjFjbU5vWVhObExXUmhkR1VpSUQwZ0lqSXdNVFl0TURndE1UWWdNVEk2TXprNk1Ua2dSWFJqTDBkTlZDSTdDbjA9IjsKImVudmlyb25tZW50IiA9ICJTYW5kYm94IjsKInBvZCIgPSAiMTAwIjsKInNpZ25pbmctc3RhdHVzIiA9ICIwIjsKfQ==";
		str = str.Replace(" ","");
		string url =	"{"+
			"\"receipt-data\"" + ":"+"\"" +  str + "\""+"}";
		url=  url.Replace("\r","");
		url=  url.Replace("\n","");
		url = url.Replace("+","%2B");//
		StartCoroutine("PostJson", url);


		byte[] bytesToEncode = Encoding.UTF8.GetBytes (str);
		string encodedText = Convert.ToBase64String (bytesToEncode);
		Utility.LogWarning(encodedText);
		byte[] decodedBytes = Convert.FromBase64String (str);
		string decodedText = Encoding.UTF8.GetString (decodedBytes);

		Utility.LogWarning(decodedText);
	
	}


	IEnumerator PostJson(string jsondata){
		var postScoreURL = "https://sandbox.itunes.apple.com/verifyReceipt";
		// 21007 status
		
		var encoding = new System.Text.UTF8Encoding();
		var postHeader = new Hashtable();
		
		postHeader.Add("Content-Type", "text/json");
		postHeader.Add("Content-Length", jsondata.Length);
		
		print("jsonString: " + jsondata);
		
		WWW request1 = new WWW(postScoreURL, encoding.GetBytes(jsondata), postHeader);
		
		yield return request1;
		
		// Print the error to the console
		if (request1.error != null)
		{
			Utility.Log("request error: " + request1.error);
		}
		else
		{
			Utility.Log("request success");
			Utility.Log("returned data" + request1.data); 
			var thing = SimpleJSON.JSON.Parse(request1.data);
			Utility.LogWarning(thing);

//		{"receipt":{"original_purchase_date_pst":"2016-08-16 05:39:19 America/Los_Angeles", "purchase_date_ms":"1471351159769", "unique_identifier":"98f2693032f94ec4de4b006bd5765d4b0439395e", "original_transaction_id":"1000000229897092", "bvrs":"1.1.8", "transaction_id":"1000000229897092", "quantity":"1", "unique_vendor_identifier":"BA9DBF29-F555-46BE-9ACA-73DA9D443588", "item_id":"1127735279", "product_id":"pitstopracing.c8502", "purchase_date":"2016-08-16 12:39:19 Etc/GMT", "original_purchase_date":"2016-08-16 12:39:19 Etc/GMT", "purchase_date_pst":"2016-08-16 05:39:19 America/Los_Angeles", "bid":"com.gabangmanstudio.pitstopracing", "original_purchase_date_ms":"1471351159769"}, "status":"0"}
	//	UnityEngine.Debug:LogWarning(Object)
			Utility.LogWarning(thing["status"].AsInt); 
				Utility.LogWarning(thing["receipt"]["unique_vendor_identifier"].Value); //BA9DBF29-F555-46BE-9ACA-73DA9D443588
			Utility.LogWarning(thing["receipt"]["product_id"]); //"pitstopracing.c8502"

		
		}
	}

*/


	/*
	public static void Vibrate(long[] pattern, int repeat)
    {
        if (Application.platform == RuntimePlatform.Android)
            vibrationObj.Call("vibrate", pattern, repeat);
    }

    public static bool HasVibrator()
    {
        if (Application.platform == RuntimePlatform.Android)
            return vibrationObj.Call<bool>("hasVibrator");
        else
            return false;
    }

    public static void Cancel()
    {
        if (Application.platform == RuntimePlatform.Android)
            vibrationObj.Call("cancel");
    }*/

	/*
	private AndroidJavaObject curActivity;
	public string strLog = "No Java Log";
	static AndroidManager _instance;
	public static AndroidManager GetInstance(){
		if(_instance == null){
			_instance = new GameObject("AndroidManager").AddComponent<AndroidManager>();
		}
		return _instance;
	}
	
	
	void Awake(){
		AndroidJavaClass jc = new AndroidJavaClass("com.unit3d.player.UnityPlayer");
		curActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
	}
	// Use this for initialization
	void Start () {
	
	}
	
	public void CallJavaFunc(string strFuncName, string strTemp){
		if(curActivity == null){
			return;
		}
		curActivity.Call(strFuncName, strTemp);
	}
	
	void SetJavaLog(string strJavaLog){
		strLog = strJavaLog;
	}
	// Update is called once per frame
	void Update () {
	
	}*/


}