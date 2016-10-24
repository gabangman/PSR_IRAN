// UnityAdsHelper.cs - Written for Unity Ads Asset Store v1.0.4 (SDK 1.3.10)
//  by Nikkolai Davenport <nikkolai@unity3d.com>
//
// Setup Instructions:
// 1. Attach this script to a new game object.
// 2. Enter game IDs into the fields provided.
// 3. Enable Development Build in Build Settings to 
//     enable test mode and show SDK debug levels.
// 
// Usage Guide:
//  Write a script and call UnityAdsHelper.ShowAd() to show an ad. 
//  Customize the HandleShowResults method to perform actions based 
//  on whether an ad was succesfully shown or not.
//
// Notes:
//  - Game IDs by platform are required to initialize Unity Ads.
//  - Test game IDs are optional. If not set while in test mode, 
//     test game IDs will default to platform game IDs.
//  - The various debug levels and test mode are only used when
//     Development Build is enabled in Build Settings.
//  - Test mode can be disabled while Development Build is set
//     by checking the option to disable it in the inspector.
using UnityEngine;
using System.Collections;
#if UNITY_IOS || UNITY_ANDROID
using UnityEngine.Advertisements;
//using GoogleMobileAds.Api;
#endif

public class UnityAdsHelper : MonoBehaviour 
{
	[System.Serializable]
	public struct GameInfo
	{  
		[SerializeField]
		private string _gameID ;
		[SerializeField]
		private string _testGameID;
		
		public string GetGameID ()
		{
			return Debug.isDebugBuild && !string.IsNullOrEmpty(_testGameID) ? _testGameID : _gameID;
		}
	}
	public GameInfo iOS;
	public GameInfo android;
	
	// Development Build must be enabled in Build Settings
	//  in order to use test mode and to show debug levels.
	public bool disableTestMode;
	public bool showInfoLogs;
	public bool showDebugLogs;
	public bool showWarningLogs = true;
	public bool showErrorLogs = true;

	//private static string adUnitIDBot = "ca-app-pub-8675074039588692/5506856762";
	//private static string adUnitIDTop = "ca-app-pub-8675074039588692/4030123568";
	//private static BannerView bannerViewBot = null;
	//private static BannerView bannerViewTop = null;

	public static void SetInitialized() {
	//	this.Awake();
	//	return;
		/*#if UNITY_IOS || UNITY_ANDROID
		string gameID = null;
		
		#if UNITY_IOS
		gameID = 46533;//iOS.GetGameID();
		#elif UNITY_ANDROID
		gameID = "46524";//android.GetGameID();
		#endif
		
		if (string.IsNullOrEmpty(gameID))
		{
			Utility.LogError("A valid game ID is required to initialize Unity Ads.");
		}
		else
		{
			Advertisement.debugLevel = Advertisement.DebugLevel.NONE;	
			if (showInfoLogs) Advertisement.debugLevel    |= Advertisement.DebugLevel.INFO;
			if (showDebugLogs) Advertisement.debugLevel   |= Advertisement.DebugLevel.DEBUG;
			if (showWarningLogs) Advertisement.debugLevel |= Advertisement.DebugLevel.WARNING;
			if (showErrorLogs) Advertisement.debugLevel   |= Advertisement.DebugLevel.ERROR;
			
			bool enableTestMode = Utility.isDebugBuild && !disableTestMode; 
			Utility.Log(string.Format("Initializing Unity Ads for game ID {0} with test mode {1}...",
			                        gameID, enableTestMode ? "enabled" : "disabled"));
			
			Advertisement.Initialize(gameID,enableTestMode);
		}
		
		
		#else
		Utility.LogWarning("Unity Ads is not supported on the current build platform.");
		#endif
		*/


	}


	protected void Awake() 
	{
		#if UNITY_IOS || UNITY_ANDROID
		string gameID = null;
		
		#if UNITY_IOS
		gameID = iOS.GetGameID();
		#elif UNITY_ANDROID
		gameID = android.GetGameID();
		#endif
		
		if (string.IsNullOrEmpty(gameID))
		{
			Utility.LogError("A valid game ID is required to initialize Unity Ads.");
		}
		else
		{
			Advertisement.debugLevel = Advertisement.DebugLevel.NONE;	
			if (showInfoLogs) Advertisement.debugLevel    |= Advertisement.DebugLevel.INFO;
			if (showDebugLogs) Advertisement.debugLevel   |= Advertisement.DebugLevel.DEBUG;
			if (showWarningLogs) Advertisement.debugLevel |= Advertisement.DebugLevel.WARNING;
			if (showErrorLogs) Advertisement.debugLevel   |= Advertisement.DebugLevel.ERROR;
			
			bool enableTestMode = Debug.isDebugBuild && !disableTestMode; 
			Debug.Log(string.Format("Initializing Unity Ads for game ID {0} with test mode {1}...",
			                        gameID, enableTestMode ? "enabled" : "disabled"));
			
			Advertisement.Initialize(gameID,enableTestMode);
		}


		#else
		Utility.LogWarning("Unity Ads is not supported on the current build platform.");
		#endif

		



	}
	
	public static bool isInitialized { get {
			#if UNITY_IOS || UNITY_ANDROID
			return Advertisement.isInitialized;
			#else
			return false;
			#endif
		}}
	
	public static bool isReady (string zone = null)
	{
		#if UNITY_IOS || UNITY_ANDROID
		if (string.IsNullOrEmpty(zone)) zone = null;
		return Advertisement.isReady(zone);
		#else
		return false;
		#endif
	}
	
	public static bool ShowAd (string zone = null, bool pauseGameDuringAd = true)
	{
		#if UNITY_IOS || UNITY_ANDROID
		if (string.IsNullOrEmpty(zone)) zone = null;
		
		if (!Advertisement.isReady(zone))
		{
			Utility.LogWarning(string.Format("Unable to show ad. The ad placement zone ($0) is not ready.",
			                               zone == null ? "default" : zone));
			return false;
		}
		
		ShowOptions options = new ShowOptions();
		options.pause = pauseGameDuringAd;
		options.resultCallback = HandleShowResult;
		
		Advertisement.Show(zone,options);
		
		return true;
		#else
		Utility.LogError("Failed to show ad. Unity Ads is not supported on the current build platform.");
		return false;
		#endif
	}

	public static bool ShowAd (string zone = null, bool pauseGameDuringAd = true, System.Action<ShowResult> Callback = null)
	{
		#if UNITY_IOS || UNITY_ANDROID
		if (string.IsNullOrEmpty(zone)) zone = null;
		
		if (!Advertisement.isReady(zone))
		{
			Utility.LogWarning(string.Format("Unable to show ad. The ad placement zone ($0) is not ready.",
			                               zone == null ? "default" : zone));
			return false;
		}
		
		ShowOptions options = new ShowOptions();
		options.pause = pauseGameDuringAd;
		options.resultCallback = Callback;
		
		Advertisement.Show(zone,options);
		
		return true;
		#else
		Utility.LogError("Failed to show ad. Unity Ads is not supported on the current build platform.");
		return false;
		#endif
	}

	#if UNITY_IOS || UNITY_ANDROID
	private static void HandleShowResult (ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Utility.Log("The ad was successfully shown.");
			break;
		case ShowResult.Skipped:
			Utility.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Utility.LogError("The ad failed to be shown.");
			break;
		}
	}

	/*
	public static void ShowAdMobBot(){
		if(bannerViewBot != null){
			bannerViewBot.Show(); 
			return;
		}
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-8675074039588692/5506856762";
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		bannerViewBot = new BannerView(
			adUnitId, AdSize.Banner,AdPosition.Bottom);
		AdRequest request = new AdRequest.Builder().Build();
		// Load the banner with the request.
		bannerViewBot.LoadAd(request);
		bannerViewBot.Show();
	
	}*/
	/*
	 * 
	 	public static void ShowAdMobTop(){
		if(bannerViewTop != null){
			bannerViewTop.Show(); 
			return;
		}
		bannerViewTop = new BannerView(
			adUnitIDTop, AdSize.Banner,AdPosition.Top);
		AdRequest request = new AdRequest.Builder().Build();
		// Load the banner with the request.
		bannerViewTop.LoadAd(request);
		bannerViewTop.Show();
	}
	public static void HiddenAdMobTop(){
		if(bannerViewTop == null) return;
		bannerViewTop.Hide();
	}

	public static void DestoryAdMobTop(){
		if(bannerViewTop == null) return;
		bannerViewTop.Destroy();
			bannerViewTop = null;
	}
	public static void HiddenAdMobBot(){
		if(bannerViewBot != null){
			bannerViewBot.Hide();
		}return;
	}
	
	public static void DestoryAdMobBot(){
		if(bannerViewBot == null) return;
			bannerViewBot.Destroy();
			bannerViewBot = null;
	} */
	#endif









}