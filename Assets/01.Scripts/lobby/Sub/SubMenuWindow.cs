using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

public class SubMenuWindow : MonoBehaviour {

	public delegate void RankBoardDown();
	public RankBoardDown _RankDown;

	public void OnChangeWindow(GameObject obj){
		obj.SetActive(false);
		transform.FindChild("Option").gameObject.SetActive(true);
	}
	public void OnCloseClick(){
		//if(Global.isNetwork) return;
		for(int i=0; i < transform.childCount;i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}
		gameObject.transform.FindChild ("Contents_menuBG").gameObject.SetActive(true);
		gameObject.SetActive(false);
		if(_RankDown != null)
			_RankDown();
		_RankDown = null;
		UserDataManager.instance.OnSubBack = null;
		Global.bLobbyBack = false;
	}

	public void OnSetCountry(){

		var temp = transform.FindChild("Language") as Transform;
		temp.gameObject.SetActive(true);
		temp.GetComponent<OpenLanguage>().SetLanguage();

	
	}

	public void OnCredits(){
		var temp = transform.FindChild("Credits") as Transform;
		temp.gameObject.SetActive(true);
	//	temp.GetComponent<CreditWindow>().SetLanguage();
	}

	public void OptionReturn(){
		transform.FindChild("Option").gameObject.SetActive(true);
	}


	void OnEnable(){
		_RankDown = null;
		GetComponent<TweenAction>().ReversScaleTween();
	}

	public void AchievementInit(){
		var temp = transform.FindChild("Achievement") as Transform;
		temp.GetComponent<AchievementWindow>().setAchievement();
		_RankDown+= ()=>{
			var temp1 = transform.FindChild("Achievement") as Transform;
			temp1.GetComponent<AchievementWindow>().stopAchievement();
		};
	}
	public void WorldRankInit(){
		
		var temp = transform.FindChild("WorldRanking") as Transform;
		temp.GetComponent<worldrankwin>().startWholeRankWindow();
		_RankDown+= ()=>{
			var temp1 = transform.FindChild("WorldRanking") as Transform;
			temp1.GetComponent<worldrankwin>().stopWholeRankWindow();
		};
	}

	public void RankInfoInit(string mode){
		if(mode == "weekly"){
			var temp = transform.FindChild("WeeklyRanking") as Transform;
			temp.GetComponent<weekrankwin>().ShowWeekRankingInfo(mode);//RankWindow(mode);
			
			_RankDown+= ()=>{
				var temp1 = transform.FindChild("WeeklyRanking") as Transform;
				temp1.GetComponent<weekrankwin>().StopRankList();
			};
		}else{
			var temp = transform.FindChild("WorldRanking") as Transform;
			temp.GetComponent<worldrankwin>().ShowTotalRankingInfo(mode);//RankWindow(mode);
			
			_RankDown+= ()=>{
				var temp1 = transform.FindChild("WorldRanking") as Transform;
				temp1.GetComponent<worldrankwin>().StopRankList();
			};
		}
	}

	public void FriendsRankInfoInit(){
		var temp = transform.FindChild("FriendsRanking") as Transform;
		temp.GetComponent<friendrankwin>().InitSet();//RankWindow(mode);

		_RankDown+= ()=>{
			var temp1 = transform.FindChild("FriendsRanking") as Transform;
			temp1.GetComponent<friendrankwin>().StopRankList();
		};


	}



	/*
	public void CrossShockWindow(){
		_RankDown+= ()=>{
			//gameObject.GetComponent<crossShock>().HiddenWindow();
		};
		var temp = transform.FindChild("CrossShock").gameObject.GetComponent<crossShock>() as crossShock;
		if(temp == null)  temp = transform.FindChild("CrossShock").gameObject.AddComponent<crossShock>();
		temp.GameStart_CrossShock_In_Lobby();
	
	}*/
	public void WeeklyInit(){
		var finish = transform.FindChild("WeeklyFinish") as Transform;
		finish = finish.FindChild("scrollview");
		finish = finish.FindChild("View");
		finish = finish.FindChild("grid");
		int cnt = finish.childCount;
		for(int i = 0; i < cnt ; i++){
			finish.GetChild(i).gameObject.AddComponent<weeklyBoardItem>();
		}
	}

	public void OnWeeklyFinishClick(){
		transform.FindChild("WeeklyFinish").gameObject.SetActive(false);
		transform.FindChild("WeeklyStart").gameObject.SetActive(true);
		var scale = transform.GetComponent<TweenScale>() as TweenScale;
		scale.Reset();
		scale.enabled = true;
		_RankDown+= ()=>{
			GameObject.Find("LobbyUI").SendMessage("PopUpEnd");
		};
		//gameRank.instance.listWeekly.Clear();
	}
	
	public void WeeklyStartInit(){
		
	}

	public void OnDailyFinish(){
		_RankDown+= ()=>{
			GameObject.Find("LobbyUI").SendMessage("PopUpEnd");
		};
	}
	
	public void LevelUpInit(){
		_RankDown+= ()=>{
			GameObject.Find("LobbyUI").SendMessage("PopUpEnd");
		};
	}

	public void OnLevelUpClick(){
		_RankDown+= ()=>{
			GameObject.Find("LobbyUI").SendMessage("InitTopMenu");
		};

		OnCloseClick();
	}
	public void CouponWindow(){
		transform.FindChild("CouponEvent");
	
	}
	public void SeasonUpInit(){
		var sUp = transform.FindChild("SeasonClear") as Transform;
		int seasonID = 0;

		seasonID = AccountManager.instance.ChampItem.Track;
		seasonID = seasonID - 1;
		sUp.FindChild("G_Season").FindChild("icon_season").GetComponent<UISprite>().spriteName = 
			seasonID.ToString()+"L";
		seasonID = GV.ChSeason;
		if(seasonID >1){
			seasonID -= 1;
		}
		sUp.FindChild("G_CoinT").FindChild("icon_season_n").GetComponent<UISprite>().spriteName = 
			"s_"+seasonID.ToString();
		int id =0;
		Common_Mode_Champion.Item cItem = Common_Mode_Champion.Get(GV.ChSeasonID-1);
		id = cItem.R_car;
		string strClass = cItem.R_class;
		var temp = sUp.FindChild("G_reward") as Transform;
		temp.gameObject.SetActive(true);
		temp.FindChild("icon_car").GetComponent<UISprite>().spriteName= id.ToString();
		temp.FindChild("lbcarName").GetComponent<UILabel>().text=Common_Car_Status.Get(id).Name;
		temp.FindChild("lbcarClass").GetComponent<UILabel>().text=
			string.Format(KoStorage.GetKorString("74024"),strClass);// "";

		GV.AddMyCarList(id, strClass);
		Utility.LogError("Try do in - select Car");
	}

	public void OnSeasonUpClick(){
		_RankDown+= ()=>{
			GameObject.Find("LobbyUI").SendMessage("SeasonUpEnd");
		};
	//	gameObject.transform.FindChild ("Contents_menuBG").GetComponent<UIButtonMessage>().
	//		functionName = "OnCloseClick";
		OnCloseClick();
	}

	bool isNetwork;
	IEnumerator LoadingCircle(){
		isNetwork = true;
		var temp = transform.FindChild("gameNotics").FindChild("Ready_Circle") as Transform;
		temp.gameObject.SetActive(true);
		var val = temp.GetComponent<UISprite>() as UISprite;
		float delta = 0.05f;
		float mdelta = 0.0f;
		while(isNetwork){
			val.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			yield return null;
		}
		temp.gameObject.SetActive(false);
	}

	void setUserBack(){
		UserDataManager.instance.OnSubBack = ()=>{
		//	Utility.LogWarning("OnEventPopCheck");
			OnEventPopCheck();
		//	Utility.LogWarning("OnEventPopCheck1");
		};
	}

	void setUserBack1(){
		UserDataManager.instance.OnSubBack = ()=>{
			//	Utility.LogWarning("OnEventPopCheck");
			OnCloseClick();
			//	Utility.LogWarning("OnEventPopCheck1");
		};
	}
	void OnEventPopCheck(){
		//Utility.LogWarning("OnEventPopCheck0");

	

		Global.bLobbyBack = true;
		//if(GV.gInfo.Notics3State == 2 || GV.gInfo.Notics3State == 3){ //1
		if(GV.gInfo.Notics2State == 2 || GV.gInfo.Notics2State == 3){ //1
			GV.gInfo.Notics2State = 0; //1
			var child = transform.FindChild("gameNotics") as Transform;
			var tex = child.FindChild("Texture") as Transform;
			tex.gameObject.SetActive(false);
			child.FindChild("checkX").gameObject.SetActive(false);
			tex.GetComponent<UITexture>().mainTexture = null;
			tex.GetComponent<UIButtonMessage>().functionName = null;
			Invoke("setUserBack",1.0f);
			StartCoroutine("OnNoticesInit", GV.gInfo.Notics2URL);
			if(UserDataManager.instance.OnSubBack == null) Utility.LogWarning("null");

		}else{
			if(GV.gInfo.Notics3State == 2 || GV.gInfo.Notics3State == 3){
			//if(GV.gInfo.Notics3State == 1){ //1
				if(!myAcc.instance.account.bPopEvent){
					GV.gInfo.Notics3State = 0;
					OnCloseClick();
					return;
				}
				//Utility.LogWarning("OnEventPopCheck4");
				GV.gInfo.Notics3State = 0; //1
				var child = transform.FindChild("gameNotics") as Transform;
				var tex = child.FindChild("Texture") as Transform;
				tex.gameObject.SetActive(false);
				child.FindChild("checkX").gameObject.SetActive(false);
				tex.GetComponent<UITexture>().mainTexture = null;
				//tex.GetComponent<UIButtonMessage>().functionName = null;
				Invoke("setUserBack1",1.0f);
				StartCoroutine("OnNoticesInit", GV.gInfo.Notics3URL);
				child.FindChild("checkX").GetComponent<UIButtonMessage>().functionName 
				="OnCloseClick";
				child.FindChild("Check_day").gameObject.SetActive(true);
				child.FindChild("Check_day").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("71116");

			}else{
				OnCloseClick();
			}
		}
	
	}


	void OnCheckDay(bool isCheck){
		myAcc.instance.account.bPopEvent = !isCheck;
		myAcc.instance.SaveAccountInfo();
	}
//	bool isUrlClick = false;
	void OnUrlClick(){

//		string str = AccountManager.instance.noticsURL;
//		if(str.Contains("http")){
//			Application.OpenURL(AccountManager.instance.noticsURL);
//		}
		
	}
	public void NoticeCloseAction(){
		_RankDown+= ()=>{
			GameObject.Find("LobbyUI").SendMessage("noticeEnd");
			StopCoroutine("OnNoticesInit");
		};
		
	}

	IEnumerator CheckDelayRoutine(){
		float delta = 0.0f;
		bool isDelay = true;
		while(isDelay){
			delta += 1.0f;
			yield return new WaitForSeconds(1.0f);
			if(delta >= 10){
				isDelay = false;
				isNetwork = false;
				transform.FindChild("gameNotics").FindChild("checkX").gameObject.SetActive(true);
				StopCoroutine("OnNoticesInit");
				StopCoroutine("LoadingCircle");
				yield break;
			}
		}
	}
	public IEnumerator OnNoticesInit(string page){
		StartCoroutine("CheckDelayRoutine");
		StartCoroutine("LoadingCircle");
		yield return null;
		string url = System.Uri.EscapeUriString(page);
	//	Utility.LogWarning("page " + page);
	/*	string iconPath = pathForDocumentsFile(page);
		if (File.Exists(iconPath)){
			Texture2D Tex = LoadTextureFile(iconPath);
			var child1 = transform.FindChild("gameNotics").FindChild("Texture") as Transform;
			child1.GetComponent<UITexture>().mainTexture = Tex;
			child1.gameObject.SetActive(true);
			Tex = null;
			transform.FindChild("gameNotics").FindChild("checkX").gameObject.SetActive(true);
			yield break;
		}
	//https:\s3-ap-northeast-1.amazonaws.com\gabangman01\UpContent_1.jpg
*/
	
		WWW www = new WWW( url );
		yield return www;
		 
		if( this == null )
			yield break;
		if( www.error != null )
		{
			Utility.Log( "load failed" );
			transform.FindChild("gameNotics").FindChild("checkX").gameObject.SetActive(true);
		}
		else
		{
			Texture2D Tex = www.texture;

			var child = transform.FindChild("gameNotics").FindChild("Texture") as Transform;
			child.GetComponent<UITexture>().mainTexture = Tex;
			child.gameObject.SetActive(true);
			Tex = null;
			transform.FindChild("gameNotics").FindChild("checkX").gameObject.SetActive(true);
			www.Dispose();
			www = null;
		}
		isNetwork = false;
	}

	public void attendReadyPopUp(){
		var temp = ObjectManager.SearchWindowPopup() as GameObject;
	//	var temp = gameObject.AddComponent<makePopup>().SearchWindow();	
		temp.AddComponent<AttendEventReadyPopup>().setObect(gameObject);
		_RankDown+= ()=>{
			GameObject.Find("LobbyUI").SendMessage("AttendEventEnd");
		};
	}
	
	public void LevelUpCoinStart(UITweener obj){
		var temp = obj.transform.parent.FindChild("G_Coin").gameObject as GameObject;
		temp.SetActive(true);
		temp.GetComponent<TweenScale>().onFinished = delegate(UITweener tween) {
			tween.transform.parent.FindChild("G_CoinT").gameObject.SetActive(true);
		};
	}
	/*
	public void AttendInit(){
		var attend = gameObject.transform.FindChild("AttendEvent").gameObject as GameObject;
		attend = attend.transform.FindChild("AttendGroup").gameObject;
		for(int i = 0; i < Global.gAttend; i++){
			var temp = attend.transform.GetChild(i).FindChild("icon_Stamp") as Transform;
			attend.transform.GetChild(i).FindChild("BG_Outline").gameObject.SetActive(true);
			temp.gameObject.SetActive(true);
			if(i == (Global.gAttend-1)){
				GameObject.Find ("Audio").SendMessage("AttendSound");
				Vector3 tscale = temp.localScale;
				temp.localScale = new Vector3(0.1f,0.1f,0.1f);
				var tw = temp.gameObject.AddComponent<TweenScale>() as TweenScale;
				tw.duration = 0.25f;
				tw.to = tscale;
				tw.from = tscale * 3.0f;
				tw.delay = 0.5f;
				tw.enabled = true;
				tw.onFinished = delegate(UITweener tween) {
					//	gameObject.transform.FindChild ("Contents_menuBG").GetComponent<UIButtonMessage>().
					//	functionName = "OnCloseClick";
					tween.transform.parent.FindChild("icon_Stamp_effect").gameObject.SetActive(true);
					//Invoke("StampSound",0.01f);
					GameObject.Find ("Audio").SendMessage("AttendStampSound");
					Destroy(tween);
				};
			}
		}
		
		_RankDown+= ()=>{
			//Global.dollar += (1000 * Global.gDayCount);
			int tempInt = Global.gAttend+8699;
			Common_Attend.Item _item = Common_Attend.Get(tempInt);
			if(_item.Type == 1){
				//Global.dollar += _item.Quantity;
			}else if(_item.Type == 2){
				//Global.coin += _item.Quantity;
			}else{
				//Global.gCouponCount += _item.Quantity;
			}
			Global.gAttend = 0;
			GameObject.Find("LobbyUI").SendMessage("AttendEventEnd");
		};
	}*/
	// 출석/ 이벤트 내용

	public void ShowAttend(){

		var obj = gameObject.transform.FindChild("AttendEvent").gameObject as GameObject;
		obj.GetComponent<AttendEventMenu>().ShowAttendEvent();
	
	}
	bool isAttendWindow = false;
	public void AttendEventInit(){
		var obj =  gameObject.transform.FindChild("AttendEvent").gameObject as GameObject;
		obj.GetComponent<AttendEventMenu>().ShowAttendEventInit();
		_RankDown+= ()=>{
			GameObject.Find("LobbyUI").SendMessage("AttendEventEnd");
		};
	}



	void SeasonEffect(UITweener tween){
		tween.transform.FindChild("LevelUp").gameObject.SetActive(true);
	}

	void LevelUpEffect(UITweener tween){
		tween.transform.FindChild("LevelUp").gameObject.SetActive(true);
	//	temp = temp.transform.FindChild("LevelUp").gameObject;
	//	temp.SetActive(true);
	//	temp.GetComponent<ParticleSystem>().emissionRate = 500;
	}


	public static void SaveTextureFile(string fileName, Texture2D SaveImage)
	{
		Texture2D newTexture = new Texture2D(1024, 576, TextureFormat.ARGB32, false);
		byte[] bytes = newTexture.EncodeToPNG();
		File.WriteAllBytes(fileName, bytes); 
	

		byte[] bytes1 = SaveImage.EncodeToPNG();
		File.WriteAllBytes(fileName, bytes1);


	
	}

	public static Texture2D LoadTextureFile(string fileName)
	{
		Texture2D Result = new Texture2D(1024, 576, TextureFormat.ARGB32, false);
		byte[] bytes = File.ReadAllBytes(fileName);
		if (!Result.LoadImage(bytes)) return null;
		else return Result;
	}


	//파일 경로 알아오기
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
			return Path.Combine( path, fileName );
		}
	}
}
