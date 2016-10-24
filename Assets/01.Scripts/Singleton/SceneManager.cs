using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
public class SceneManager :  MonoSingleton< SceneManager > {
	private AsyncOperation async = null;
	bool IsLoadGame = false;
	 UISlider _slider;
	 UILabel _label;
	public string RaceName;
	public UISlider LoadSlider{get{return _slider;}	set{_slider = value;}}
	public UILabel LoadLabel{ get{return _label;}set{_label = value;}}

	public UISlider uislider;
	public UILabel uilable;

	void Awake(){
		DontDestroyOnLoad(gameObject);
	}

	void Start() 
	{
	}


	void OnLevelWasLoaded()
	{

	}
	
	void OnDestroy()
	{

	}

	public delegate void OnComplete();
	public OnComplete _oncomplete = null;
	
	public void AddLoadScene(string strSceneName){
	}

	public IEnumerator LoadAdditveScene( string strSceneName )
	{
		yield return new WaitForSeconds(0.2f);
		if (IsLoadGame == false) 
		{
			IsLoadGame = true;
			//AccountManager.instance.sceneBundle.LoadAll();
			async = Application.LoadLevelAdditiveAsync(strSceneName);
			while(async.isDone == false) 
			{
				_slider.sliderValue = async.progress;
				yield return true;
			}
			IsLoadGame = false;
		}
	}

	public IEnumerator LoadAdditveScene_1( string strSceneName )
	{
		yield return new WaitForSeconds(0.2f);
		//Utility.LogWarning("sc 1 ");
		if (IsLoadGame == false) 
		{
			IsLoadGame = true;
			async = Application.LoadLevelAdditiveAsync(strSceneName);
			while(async.isDone == false) 
			{
				//_slider.sliderValue = async.progress;
				SceneManager.instance.LoadSlider.sliderValue =  async.progress*0.5f;
				yield return true;
			}
			//Utility.LogWarning("sc 2 ");
			IsLoadGame = false;
		}
		//Utility.LogWarning("sc 3 ");
	}
	
	public IEnumerator LoadAddScene(string SceneName){
		//_label.text = string.Empty;
		yield return new WaitForSeconds(1.0f);
		if (IsLoadGame == false) 
		{
			async = Application.LoadLevelAsync(SceneName);
			while(async.isDone == false) 
			{
				//float p = async.progress *100f;
				//int pRounded = Mathf.RoundToInt(p);
				//_label.text = System.String.Format("...{0:0}%",pRounded);
				//_slider.sliderValue = async.progress;
				yield return true;
			}
		}
	}


	public void LoadingStart(string RaceName){
		this.RaceName = RaceName;
		IsLoadGame = false;
		StartCoroutine("LoadAddScene","Load");
	}

	public IEnumerator LoadFirstScene(string SceneName){
		Application.LoadLevel(SceneName);
		IsLoadGame = false;
		yield return null;
	}

	public IEnumerator LoadReplayScene(){
		Application.LoadLevel("Load");
		IsLoadGame = false;
		yield return null;
	}
	
	public IEnumerator RaceGameOver(){
		Application.Quit();
		yield return null;
	}

	void OnLevelWasLoaded(int level){
		Utility.LogWarning("Scene Mgr " + level);
	}


	private string expPath;
	private string logtxt;
	private bool alreadyLogged = false;
	private string nextScene = "Lobby";
	private bool downloadStarted;
	
	void log( string t )
	{
		logtxt += t + "\n";
		//print("MYLOG " + t);
		Utility.LogWarning("Unity3d " + t);
	}

	public void CheckOBB(){
		StartCoroutine("CheckSceneOBB");
	}

	IEnumerator CheckSceneOBB(){
		#if UNITY_ANDROID && !UNITY_EDITOR
		Global.isNetwork = true;
		downloadOBB();
		while(Global.isNetwork){
			yield return null;
		}

		#elif UNITY_IOS
			
		#endif
		yield return null;
		StartCoroutine("LoadFirstScene","Title");
	}

	string ObbDataPath(){
		string path = pathForDocumentsFile("main.1.com.Infinitecrew.PitInRacing.obb");
		return path;
		//path1 = pathForDocumentsFile("AiCarSpeedData");
		//File.Copy(path, path1, true);
	}
	public string pathForDocumentsFile( string fileName )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			string path = Application.dataPath.Substring( 0, Application.dataPath.Length - 5 );
			path = path.Substring( 0, path.LastIndexOf('/'));
			return Path.Combine( Path.Combine( path, "Documents"), fileName );
		}
		else if( Application.platform == RuntimePlatform.Android )
		{
			string path = Application.persistentDataPath;
			path = path.Substring( 0, path.LastIndexOf('/'));
			return Path.Combine( path, fileName );
		}
		else
		{
			string path = Application.dataPath;
			
			path = path.Substring( 0, path.LastIndexOf('/'));
			//Utility.Log (path);
			return Path.Combine( path, fileName );
		}
	}
	public  IEnumerator loadLocalLevel() 
	{ 
		string mainPath = ObbDataPath();
			
		if(!File.Exists(mainPath)){
			Utility.LogWarning("File Not " + mainPath);			
				string sceneURL = "http://cdn.gabangmanstudio.com/Data/AssetBundle_Global/main.1.com.Infinitecrew.PitInRacing.zip";
				using(WWW www1 = new WWW(sceneURL)){
					while(!www1.isDone){
						//AccountManager.instance.progressbarvalue = www.progress;	
						yield return null;
					}
					if (www1.error != null)
					{
						log ("wwww error " + www1.error);
						yield break;
					}
					else
					{
						log ("wwww Finish ");
						string fullPath = ObbDataPath();
						File.WriteAllBytes (fullPath, www1.bytes);
					}
				}
	  }
		string uri = "file://" + mainPath;
		log("downloading " + uri);
		using(WWW www = WWW.LoadFromCacheOrDownload(uri, 0)){
				while(!www.isDone){
					AccountManager.instance.progressbarvalue = www.progress;	
					yield return null;
				}
				if (www.error != null)
				{
					log ("wwww error " + www.error);
					
				}
				else
				{
					log ("wwww Finish ");
					StartCoroutine("LoadFirstScene","Title");
				}
			}

		Global.isNetwork = false;
	}

	#if UNITY_ANDROID && !UNITY_EDITOR
	public void downloadOBB(){
		if (!GooglePlayDownloader.RunningOnAndroid())
		{
			log ("Use GooglePlayDownloader only on Android device!");
			return;
		}
		
		expPath = GooglePlayDownloader.GetExpansionFilePath();
		if (expPath == null)
		{
			//log ("External storage is not available!");
		}
		else
		{
			string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
			string patchPath = GooglePlayDownloader.GetPatchOBBPath(expPath);
			if( alreadyLogged == false )
			{
				alreadyLogged = true;
				log( "expPath = "  + expPath );
				log( "Main = "  + mainPath );
			//	log( "Main = " + mainPath.Substring(expPath.Length));
				
				if (mainPath != null){
					//StartCoroutine(loadLevel());
					Global.isNetwork = false;
					//StartCoroutine("LoadFirstScene","Title");

				}
				
			}
			if (mainPath == null)
			{
			//	log("The game needs to download 200MB of game content. It's recommanded to use WIFI connexion.");
				StartDownLoadObb();
			}
		}
	}
	
	void StartDownLoadObb(){
		//log("Start Download !");
		GooglePlayDownloader.FetchOBB();
		StartCoroutine(loadLevel());
	}

	protected IEnumerator loadLevel() 
	{ 
		string mainPath;
		do
		{
			yield return new WaitForSeconds(0.5f);
			mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);	
			log("waiting mainPath "+mainPath);
		}
		while( mainPath == null);
		
		if( downloadStarted == false )
		{
			downloadStarted = true;
			
			string uri = "file://" + mainPath;
			log("downloading " + uri);
			using(WWW www = WWW.LoadFromCacheOrDownload(uri, 0)){
				while(!www.isDone){
					AccountManager.instance.progressbarvalue = www.progress;	
					yield return null;
				}
			if (www.error != null)
			{
				log ("wwww error " + www.error);
				
			}
			else
			{
				log ("wwww Finish ");
			}
		}
	}
		Global.isNetwork = false;
	}

	protected IEnumerator RaceLoadScene(string sceneName) 
		{ 
			string mainPath;
			do
			{
				yield return new WaitForSeconds(0.5f);
				mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);	
				log("waiting mainPath "+mainPath);
			}
			while( mainPath == null);
			
			if( downloadStarted == false )
			{
				downloadStarted = true;
				
				string uri = "file://" + mainPath;
				log("downloading " + uri);
				
				WWW www = WWW.LoadFromCacheOrDownload(uri , 0);		
					yield return www;
					
					if (www.error != null)
					{
						log ("wwww error " + www.error);
					}
					else
					{
					//	Application.LoadLevel(sceneName);
								yield return new WaitForSeconds(0.2f);
								if (IsLoadGame == false) 
								{
									IsLoadGame = true;
									
								async = Application.LoadLevelAdditiveAsync(sceneName);
									while(async.isDone == false) 
									{
										_slider.sliderValue = async.progress;
										yield return true;
									}
									IsLoadGame = false;
								}
					}
				}
		}
	#endif
	public void settingRaceRenderSetting(string strName){
		var re = gameObject.GetComponent<RenderUserSetting>() as RenderUserSetting;
		if(re == null) re = gameObject.AddComponent<RenderUserSetting>();
		re.SetRaceRender(strName);
	}

}
