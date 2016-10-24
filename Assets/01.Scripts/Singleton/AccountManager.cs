using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
public class AccountManager :MonoSingleton< AccountManager > {

	[NonSerialized] public readonly int PART_BODY = 1;
	[NonSerialized] public readonly int PART_ENGINE = 2;
	[NonSerialized] public readonly int PART_TIRE = 3;
	[NonSerialized] public readonly int PART_GEAR = 4;
	[NonSerialized] public readonly int PART_INTAKE = 5;
	[NonSerialized] public readonly int PART_BOOSTPOWER = 6;
	[NonSerialized] public readonly int PART_BOOSTTIME = 7;

	[NonSerialized] public readonly int PART_CHIEF = 3;
	[NonSerialized] 	public readonly int PART_DRIVER = 1;
	[NonSerialized] 	public readonly int PART_JACKMAN = 4;
	[NonSerialized] 	public readonly int PART_GASMAN = 5;
	[NonSerialized] 	public readonly int PART_TIREMAN = 2;

	[NonSerialized] 	public readonly int TYPE_FUEL = 1;
	[NonSerialized]  public readonly int TYPE_CASH = 2;
	[NonSerialized] 	public readonly int TYPE_COIN = 3;
	[NonSerialized] 	public readonly int TYPE_DOLLAR = 4;
	[NonSerialized] 	public readonly int TYPE_CUBE = 5;
	[NonSerialized] 	public readonly int TYPE_SILVERCOUPON = 6;
	[NonSerialized] 	public readonly int TYPE_GOLDCOUPON = 7;
	[NonSerialized] 	public readonly int TYPE_MATERIAL = 8;

	[NonSerialized] 	public readonly int TYPE_TEAM = 101;
	[NonSerialized] 	public readonly int TYPE_CAR = 102;

	[NonSerialized] 	public readonly int TYPE_SPONSOR = 107;
	[NonSerialized]  public readonly int TYPE_CAR_DURABILITY = 108; 

	public Texture2D myPicture;
	//[NonSerialized] public string strAD;
	[NonSerialized] public string strReTry;
	[NonSerialized] public float progressbarvalue;//{get;set;}
    public AssetBundle  txtbundle;
	public float  FinishInterval = 0.3f;
	public Common_Mode_Champion.Item ChampItem;
	string carUrl;
	string texUrl;
	private string versionpw;
	public string lbSponTime;
	public bool bGooglePurchasing = false;
	public bool bPurchaseSuccess =false;

	void Awake(){
		DontDestroyOnLoad(gameObject);
	}

	public IEnumerator myProfileLoad(){
		string myProfileUrl = string.Empty;
		int rans = UnityEngine.Random.Range(1,7);
		if(FB.IsLoggedIn){
			string strUrl = string.Empty;
			if(!EncryptedPlayerPrefs.HasKey("FBUrl")) {
				strUrl = "User_Default";
				
				EncryptedPlayerPrefs.SetString("FBUrl",strUrl);
			}
			strUrl = EncryptedPlayerPrefs.GetString("FBUrl");
			if(strUrl.Equals("User_Default")){
				//	myProfileUrl = "https://s3-ap-northeast-1.amazonaws.com/gabangman01/MultiPicture/MultiCom_"+rans.ToString()+".png";
				myProfileUrl = strUrl;
				
			}else{
				myProfileUrl = strUrl;
			}
		}else{
			//	myProfileUrl = "https://s3-ap-northeast-1.amazonaws.com/gabangman01/MultiPicture/MultiCom_"+rans.ToString()+".png";
			myProfileUrl = "User_Default";
		}
		GV.mUser.profileURL = myProfileUrl;
		string url  = System.Uri.EscapeUriString(myProfileUrl);
		
		if(!url.Contains("http")){
			Texture Tex =  (Texture)Resources.Load(url, typeof(Texture));
			myPicture =(Texture2D) Tex;
			yield break;
		}
		
		WWW www = new WWW( url );
		yield return www;
		if( this == null )
			yield break;
		if( www.error != null )
		{
			//	Global.myProfile = (Texture2D) (Texture)Resources.Load("Kakao_icon", typeof(Texture));
			myPicture = (Texture2D) (Texture)Resources.Load("User_Default", typeof(Texture));
		}
		else
		{
			Texture2D Tex = www.texture;
			//	Global.myProfile = Tex;
			myPicture= Tex;
			Tex = null;
			www.Dispose();
			www = null;
		}
	}



	public void CheckAbVersion(){

		string url = "http://d3r1yjan6pqmkl.cloudfront.net/assetBundle/AndAsset_1.unity3d";
		//GV.gInfo.bundleVer_2 = "4";
		if(Application.isEditor){
			//GV.gInfo.bundleVer_2 = "15";
			url = "http://d3r1yjan6pqmkl.cloudfront.net/assetBundle/AndAsset_"+GV.gInfo.bundleVer_2.Trim()+".unity3d";
		}else if(Application.platform == RuntimePlatform.Android){
			url = "http://d3r1yjan6pqmkl.cloudfront.net/assetBundle/AndAsset_"+GV.gInfo.bundleVer_2.Trim()+".unity3d";
		}else if(Application.platform == RuntimePlatform.IPhonePlayer){
			url = "http://d3r1yjan6pqmkl.cloudfront.net/assetBundle/IosAsset_"+GV.gInfo.bundleVer_2.Trim()+".unity3d";
		}
		AssetBundle texbundle = AssetBundleManager.getAssetBundle(url, int.Parse(GV.gInfo.bundleVer_2));
		if(!texbundle){
			StartCoroutine("DownloadAbText", url);//Utility.LogWarning("checkabversion1 " + url);
		}else{
			txtbundle = texbundle;
			Global.isNetwork = false;//Utility.LogWarning("checkabversion2");
		}
		versionpw = "InfiniteCrew";
		setTextEncrypt();
		setServerAPI();
		Global.isNetwork = false;
	//	Utility.LogWarning("checkabversion");
	//	AssetBundleManager.unLoad(url, int.Parse(GV.gInfo.bundleVer_2),false);
	}

	private void setServerAPI(){
		var obj = GameObject.Find("ManagerGroup") as GameObject;
		string tx = obj.GetComponent<DontDestroy>().serverAPI.text;
		var sAPI = gameObject.GetComponent<ServerAPI>() as ServerAPI;
		if(sAPI == null) sAPI = gameObject.AddComponent<ServerAPI>() as ServerAPI;
		sAPI.SetDataFile(tx); sAPI = null; tx = null;
	}


	public void DownloadOBB(){
		Global.isNetwork = true;
		progressbarvalue = 0.0f;
		#if UNITY_ANDROID && !UNITY_EDITOR
		//SceneManager.instance.downloadOBB();
		#endif
	}


	IEnumerator DownloadAbText(string url){
		Global.isNetwork = true;
		int a = int.Parse(GV.gInfo.bundleVer_2.Trim());
		yield return StartCoroutine(AssetBundleManager.downloadAssetBundle(url, a));
		txtbundle = AssetBundleManager.getAssetBundle(url, a);
		yield return null;
	}
	/*
	public void CheckVersion(){
		//StartCoroutine("GetgamePatch");
	}

	IEnumerator GetgamePatch(){
	string path1 =  "http://dev.gabangmanstudio.com/Data/GamePatch/GPatch.txt";
	WWW www = new WWW(path1);
	yield return www;
	if (www.error == null) 
	{ 
		//Utility.Log("WWW Ok!: " + www.text); 
	}else{ 
			Utility.Log("WWW Error: " + www.error); 
			//ProtocolManager.instance.ErrorCallback();
			yield break;
	}
		//TextAsset tx = (TextAsset)Resources.Load("GPatch_New_01",typeof(TextAsset));
		//string data = tx.text;
		string data = www.text;
		Utility.Log(data);

		string[] stringList1, stringTable;
		stringTable = data.Split('\n');

		string[] mString = new string[stringTable.Length-1];
		for(int i = 0; i < stringTable.Length-1; i++){
			mString[i] = stringTable[i].Split(';')[1];
		}
		int tempid = int.Parse(mString[9]);
		Global.gSale = Base64Manager.instance.GlobalEncoding(tempid);
		//Global.gSale =  Base64Manager.instance.GlobalEncoding(0);
		carUrl = mString[10].Trim(); //TextureURL
		texUrl = mString[11].Trim(); // Table
		contentsUrl =mString[12].Trim(); //URL1
		noticsURL2 = mString[13].Trim(); //URL2
		eventUrl = mString[14].Trim(); //URL3
		ReviewUrl =mString[15].Trim(); //ReviewURl
		serverContents = mString[16].Trim(); //ServerContents
		serverContents = serverContents.Replace('@','\n');
		newVesrionURL = mString[17].Trim();
		noticsURL = mString[18].Trim(); //ContentsURL
		couponIcon =  mString[19].Trim();
		couponConents = mString[20].Trim();
		versionpw = "InfiniteCrew";
		setTextEncrypt();
		
	//0 version:0 0
	//1 notics1:1
	//2 notics2:0
	//3 notics3:0
	//4 TexturePatch:1
	//5 TablePatch:1
	//6 serverNotics:0
	//7 attend:1
	//8 AD:0
	//9 sale:0 - 9 
	//10 - TexureUrl;http://cdn.gabangmanstudio.com/Data/AssetBundle_Global/PitTex_02.unity3d
	//11 -TableUrl;http://cdn.gabangmanstudio.com/Data/AssetBundle_Global/GTableAsset_01.unity3d
	//12 - Url1;http://cdn.gabangmanstudio.com/Notice/Event_NewUser_game.jpg
	//13 - Url2;http://cdn.gabangmanstudio.com/Notice/UpContent_1_1.jpg
	//14 - Url3;http://cdn.gabangmanstudio.com/Notice/UpContent_2_1.jpg
	//15 - ReviewUrl;market://details?id=com.GaBangMan.PitInRacing
	//16 - ServerContents;[ff3600]서버점검 안내[-]@@[009cff]금일 새벽 2시~3시[-]에@서버점검이 있을 예정입니다.
	//17 - 1.0.0;market://details?id=com.GaBangMan.PitInRacing
	//18 - contentUrlpage;https://www.facebook.com/pitinracing
	//couponBtnUrl;http://cdn.gabangmanstudio.com/Notice/Icon_Like_1.png
	//	couponImgUrl;http://cdn.gabangmanstudio.com/Notice/LikeEvent_coupon_1.jpg
	//1.5;no;no

		// ClientVersion : version
		//UpgradeVersion : Ver
		//AssetBundleVersion1 : ver : URL
		//AssetBundleVersion2 : ver : URL
		//Notices1 : On/Off : Image URL
		//Notices2 : On/Off : imageURL
		//Notices3 : On/Off : ImageURL
		//Store Plus Event : On/Off : imageURL
		//CouponURL : On/Off : ImageURL
		// Emergencey Notices : On/Off : Integer Value(1~10)
		// market URL : marketURL
		// market URL(IOS) : marketURL
		//HomePage 1 : Touch URL
		//HomePage 2 : Touch URL
		//Email : Email address
		//Movie Reward : Integer Value(1~10)
		//Reward AD : Integer Value (1~2)


		webPatchVersion = new int[9];
		webPatchVersion[0] = int.Parse(mString[0]);
		webPatchVersion[1] = int.Parse(mString[1]);
		webPatchVersion[2] = int.Parse(mString[2]);
		webPatchVersion[3] = int.Parse(mString[3]);
		webPatchVersion[4] = int.Parse(mString[4]);
		webPatchVersion[5] = int.Parse(mString[5]);
		webPatchVersion[6] = int.Parse(mString[6]);
		webPatchVersion[7] = int.Parse(mString[7]);
		webPatchVersion[8] = int.Parse(mString[8]);
		//Global.gStarCoupon = webPatchVersion[7];

		string[] stringList2 =stringTable[stringTable.Length-1].Split(';');
		string strTest = stringList2[0].Trim();
		float.TryParse(strTest, out FinishInterval);
		strReTry = stringList2[1].Trim();

		if(Application.isEditor) Global.gVersion ="1.0.0";
		if(webPatchVersion[0] == 0){
			Global.isNetwork = false;
		}else if(webPatchVersion[0] == 1){
			ProtocolManager.instance.ErrorCallback(); // 서버와의 접속이 끊겼습니다. 
		}else if(webPatchVersion[0] == 2){
			GameObject.Find("LoadScene").SendMessage("builtInPopup"); //점검중입니다. 
		}else if(webPatchVersion[0] == 3){
			GameObject.Find("LoadScene").SendMessage("newVersionpopup");
		}
		yield return null;
	}
*/
	public void setTextEncrypt(){
		Base64Manager.instance.setCurrentPW(versionpw);
	}


	void OnDestroy(){
		if(txtbundle != null){
			string url =null;
			if(Application.isEditor){
				
				url = "http://d3r1yjan6pqmkl.cloudfront.net/assetBundle/AndAsset_"+GV.gInfo.bundleVer_2.Trim()+".unity3d";
			}else if(Application.platform == RuntimePlatform.Android){
				url = "http://d3r1yjan6pqmkl.cloudfront.net/assetBundle/AndAsset_"+GV.gInfo.bundleVer_2.Trim()+".unity3d";
			}else if(Application.platform == RuntimePlatform.IPhonePlayer){
				url = "http://d3r1yjan6pqmkl.cloudfront.net/assetBundle/IosAsset_"+GV.gInfo.bundleVer_2.Trim()+".unity3d";
			}
			AssetBundleManager.unLoad(url, int.Parse(GV.gInfo.bundleVer_2),true);
		}
			
		System.GC.Collect(); 	
	}


	public  IEnumerator ReceiveMyRaceAccount(UILabel tip){
		string networkStatus = string.Empty;
		float delay = 0.5f;
		bool bConnect = false;
		networkStatus =KoStorage.GetKorString("70119");// "차량 정보 가져오는 중";
		tip.text = networkStatus;
		
		string mAPI = ServerAPI.Get(90012); // game/car/
		NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				GameObject.Find("LoadScene").SendMessage("DisConnectPopup"); //서버 점검 중
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				GV.mineCarList = new List<CarInfo>();
				CarInfo carItem = null;
				int cnt = thing["result"].Count;
				for(int i = 0; i < cnt; i++){
					int nId =  thing["result"][i]["carId"].AsInt;
					carItem = new CarInfo(nId);
					carItem.CarIndex = thing["result"][i]["idx"].AsInt;
					carItem.CarID =nId;
					carItem.nClassID = thing["result"][i]["carClass"].AsInt;
					
					
					carItem.bodyLv = thing["result"][i]["partLevel"]["1"].AsInt;
					carItem.engineLv =  thing["result"][i]["partLevel"]["2"].AsInt;
					carItem.tireLv = thing["result"][i]["partLevel"]["3"].AsInt;
					carItem.gearBoxLv = thing["result"][i]["partLevel"]["4"].AsInt;
					carItem.intakeLv =  thing["result"][i]["partLevel"]["5"].AsInt;
					carItem.bsPowerLv = thing["result"][i]["partLevel"]["6"].AsInt;
					carItem.bsTimeLv =  thing["result"][i]["partLevel"]["7"].AsInt;
					
					carItem.bodyStar = thing["result"][i]["partStar"]["1"].AsInt - 1;
					carItem.engineStar =  thing["result"][i]["partStar"]["2"].AsInt-1;
					carItem.tireStar = thing["result"][i]["partStar"]["3"].AsInt-1;
					carItem.gearBoxStar = thing["result"][i]["partStar"]["4"].AsInt-1;
					carItem.intakeStar =  thing["result"][i]["partStar"]["5"].AsInt-1;
					carItem.bsPowerStar = thing["result"][i]["partStar"]["6"].AsInt-1;
					carItem.bsTimeStar =  thing["result"][i]["partStar"]["7"].AsInt-1;
					
					carItem.durability = thing["result"][i]["durability"].AsInt;
					carItem.SetFlag = 0;
					GV.mineCarList.Add(carItem);
				}

			//	for(int i =0; i < GV.mineCarList.Count;i++){
			//		Utility.LogWarning("a " + GV.mineCarList[i].CarID);
			//	}

				GV.mineCarList.Sort(delegate(CarInfo x, CarInfo y) {
					int a = x.CarID.CompareTo(y.CarID);
					if(a == 0)
						a = x.nClassID.CompareTo(y.nClassID);
					return a;
				
				});

			}else{
				
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
	//	yield return new WaitForSeconds(0.1f);
		
		//networkStatus =KoStorage.GetKorString("70119");// "팀 정보 가져오는 중";
		//tip.text = networkStatus;
		bConnect = false;
		mAPI = ServerAPI.Get(90058); // game/team/
		if(ClubSponInfo.instance == null) ClubSponInfo.instance = new ClubSponInfo();
		ClubSponInfo.instance.GetSponInfo();

		NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				GameObject.Find("LoadScene").SendMessage("DisConnectPopup"); 
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				//sponse:{"state":0,"msg":"sucess","result":[{"id":10,"chiefLevel":1,"driverLevel":1,"tireManLevel":1,"jackManLevel":1,"gasManLevel":1,"sponsorDate":"1900-01-01 00:00:01"}],"time":1448679227}
				GV.mUser.TeamCount = thing["result"].Count;
				GV.listmyteaminfo = new List<myTeamInfo>();
				for(int i = 0; i < GV.mUser.TeamCount; i++){
					myTeamInfo	team = new myTeamInfo(thing["result"][i]["id"].AsInt);
					team.chiefLv = thing["result"][i]["chiefLevel"].AsInt;
					team.driverLv = thing["result"][i]["driverLevel"].AsInt;
					team.tireLv = thing["result"][i]["tireManLevel"].AsInt;
					team.jackLv = thing["result"][i]["jackManLevel"].AsInt;
					team.gasLv = thing["result"][i]["gasManLevel"].AsInt;
		
					
					team.SponID =  thing["result"][i]["sponsorId"].AsInt;
					if(team.SponID == 0 || team.SponID == 1300){
						team.SponID = 1300;
						team.freeSpon = 0;
					}else{

						string strTime =  thing["result"][i]["sponsorDate"];
						try{
							team.sponDateTime = System.Convert.ToDateTime(strTime);
							team.sponDateTime = team.sponDateTime.AddHours(12);
							team.SponRemainTime = team.sponDateTime.Ticks;
						}catch(Exception e){
							team.sponDateTime = new DateTime(1900,1,1);
							team.SponRemainTime = team.sponDateTime.Ticks;
						}
						System.DateTime nowTime = NetworkManager.instance.GetCurrentDeviceTime();
						System.TimeSpan pTime = new System.DateTime(team.SponRemainTime) -nowTime;
						if(pTime.TotalHours >0){
							team.freeSpon = 2;
						}else{
							team.SponID = 1300;
							team.freeSpon = 0;
						}
					}
					team.TeamCarIndex = thing["result"][i]["carIdx"].AsInt;
					GV.listmyteaminfo.Add(team);
				}
			}else{
				if(status == -105){
					GameObject.Find("LoadScene").SendMessage("builtInPopup");
					return;
				}
			}
			bConnect = true;
		});
	
		while(!bConnect){
			yield return null;
		}

		ClubSponInfo.instance.mClubSpon.setTeamSponCompare();

		//	yield return new WaitForSeconds(0.1f);
		/*
		networkStatus =KoStorage.GetKorString("70120");// ; -> 재료 정보 가져옴.
		tip.text = networkStatus;
		bConnect = false;
		mAPI = ServerAPI.Get(90013); // "game/material/"
		NetworkManager.instance.HttpConnect("Get",  mAPI, (request)=>{
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				GameObject.Find("LoadScene").SendMessage("DisConnectPopup"); //서버 점검 중
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				GV.listMyMat = new List<MatInfo>();
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
					GameObject.Find("LoadScene").SendMessage("builtInPopup");
					return;
				}
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}*/
		//yield return new WaitForSeconds(0.1f);
		/*
		networkStatus =KoStorage.GetKorString("70122");// " 정보 가져오는 중"; ->"  쿠폰 가져오는 중";
		tip.text = networkStatus;
		bConnect = false;
		mAPI = ServerAPI.Get(90018); // "game/container/"
		NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				GameObject.Find("LoadScene").SendMessage("DisConnectPopup"); //서버 점검 중
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				GV.myCouponList = new List<int>();
				GV.myCouponList.Add(0);
				GV.myCouponList.Add(0);
				
				int cnt = thing["result"]["silverCnt"].AsInt;
				GV.UpdateCouponList(0, cnt);
				cnt = thing["result"]["goldCnt"].AsInt;
				GV.UpdateCouponList(1, cnt);
				
			}else{
				if(status == -105){
					GameObject.Find("LoadScene").SendMessage("builtInPopup");
					return;
				}
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		} */
		//yield return new WaitForSeconds(0.1f);
		/*
		
		networkStatus =KoStorage.GetKorString("70122");// " 정보 가져오는 중"; ->" 에보 큐브 가져오는 중";
		tip.text = networkStatus;
		bConnect = false;
		mAPI = ServerAPI.Get(90014); // "game/material/evoCube/"
		NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				GameObject.Find("LoadScene").SendMessage("DisConnectPopup"); //서버 점검 중
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				int matcount = thing["result"]["count"].AsInt;
				//	Utility.LogWarning(matcount);
				GV.UpdateMatCount(8620,matcount);
				
			}else{
				if(status == -105){
					GameObject.Find("LoadScene").SendMessage("builtInPopup");
					return;
				}
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}*/
		//yield return new WaitForSeconds(0.1f);
		/*
		networkStatus =KoStorage.GetKorString("70122")+".";// "이벤트 레이스 정보 가져오는 중";
		tip.text = networkStatus;
		bConnect = false;
		mAPI = ServerAPI.Get(90049); // "game/race/eventRace/count/"
		NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				GameObject.Find("LoadScene").SendMessage("DisConnectPopup"); //서버 점검 중
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
			}else{
				if(status == -105){
					GameObject.Find("LoadScene").SendMessage("builtInPopup");
					return;
				}
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
		//yield return new WaitForSeconds(0.1f);

		networkStatus =KoStorage.GetKorString("70122")+"..";// "이벤트 레이스 정보 가져오는 중";
		tip.text = networkStatus;
		bConnect = false;
		mAPI = ServerAPI.Get(90049); // "game/race/eventRace/count/"
		NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				GameObject.Find("LoadScene").SendMessage("DisConnectPopup"); //서버 점검 중
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
					//Utility.LogWarning("One Day Reset " + thing["result"]["featuredCnt"].AsInt + "_" +  thing["result"]["testDirveCnt"].AsInt );
					myAccount.instance.account.eRace.featuredPlayCount = 0;
					myAccount.instance.account.eRace.testDrivePlayCount =0;
				}
				
			}else{
				myAccount.instance.account.eRace.featuredPlayCount = 0;
				myAccount.instance.account.eRace.testDrivePlayCount =0;
				myAccount.instance.account.eRace.EvoPlayCount = 0;
				if(status == -105){
					GameObject.Find("LoadScene").SendMessage("builtInPopup");
					return;
				}
			}
			bConnect = true;
		});
		
		
		while(!bConnect){
			yield return null;
		} */
	//	yield return new WaitForSeconds(0.1f);
		
		
		networkStatus =KoStorage.GetKorString("70122")+"...";// "출석 이벤트 가져오는 중";
		tip.text = networkStatus;
		bConnect = false;
		
		
		mAPI = ServerAPI.Get(90052); // "game/event/daily"
		//NetworkManager.instance.mRequestRange--;
		NetworkManager.instance.HttpFormConnect("Put", new System.Collections.Generic.Dictionary<string,int>() ,mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				GameObject.Find("LoadScene").SendMessage("DisConnectPopup"); //서버 점검 중
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			//response:{"state":0,"msg":"sucess","result":{"isReward":0,"days":1},"time":1447914002}
			if (status == 0)
			{
				//Global.gAttend  = thing["result"]["isReward"].AsInt;
				Global.gAttendDayCount  = thing["result"]["days"].AsInt;
			}else{
				if(status == -105){
					GameObject.Find("LoadScene").SendMessage("builtInPopup");
					return;
				}
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
	//	yield return new WaitForSeconds(0.1f);
		bConnect = false;
		mAPI = ServerAPI.Get(90055); // "game/giftbox/"
		NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				GameObject.Find("LoadScene").SendMessage("DisConnectPopup"); //서버 점검 중
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				
				Global.gNewMsg = thing["result"].Count;
				if(Global.gNewMsg != 0){
					gameRank.instance.InitializeGiftList(thing["result"], Global.gNewMsg);
				}else{
					
				}
			}else{
				if(status == -105){
					GameObject.Find("LoadScene").SendMessage("builtInPopup");
					return;
				}
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}

		bConnect = false;
		int achieveCount = 0;
		mAPI = ServerAPI.Get(90059); // "game/acheivment/"
		//NetworkManager.instance.mRequestRange--;
		NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				GameObject.Find("LoadScene").SendMessage("DisConnectPopup"); //서버 점검 중
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				//response:{"state":0,"msg":"sucess","result":[{"acheivmentId":16000,"count":1,"isCompleted":1,"isAward":1},{"acheivmentId":16001,"count":2,"isCompleted":0,"isAward":0}],"time":1448865796}
				GAchieve.instance = new GAchieve();
				GAchieve.instance.GetAchievementInfo();
				if(thing["result"].Count == 1){
					achieveCount = 1;
				}else if(thing["result"].Count <= 13){
					achieveCount = 2;
					for(int i = 0; i < thing["result"].Count ;i++){
						int achieveId = thing["result"][i]["acheivmentId"].AsInt;
						int achievemCount = thing["result"][i]["count"].AsInt;
						int achieveComplete = thing["result"][i]["isCompleted"].AsInt;
						int achieveReward = thing["result"][i]["isAward"].AsInt;
						GAchieve.instance.achieveInfo.SetUserAchievementInfo(achieveId, achievemCount, achieveComplete, achieveReward);
					//	if(achieveComplete == 1  && achieveReward == 0) GV.bachieveRewardFlag = true;
					//	tip.text = KoStorage.GetKorString("70122")+".....";
					}
				}else{
					for(int i = 0; i < thing["result"].Count ;i++){
						int achieveId = thing["result"][i]["acheivmentId"].AsInt;
						int achievemCount = thing["result"][i]["count"].AsInt;
						int achieveComplete = thing["result"][i]["isCompleted"].AsInt;
						int achieveReward = thing["result"][i]["isAward"].AsInt;
						GAchieve.instance.achieveInfo.SetUserAchievementInfo(achieveId, achievemCount, achieveComplete, achieveReward);
						if(achieveComplete == 1  && achieveReward == 0) GV.bachieveRewardFlag = true;
						tip.text = KoStorage.GetKorString("70122")+".....";
					}
				
				
				}
			}else{
				if(status == -105){
					GameObject.Find("LoadScene").SendMessage("builtInPopup");
					return;
				}
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}

	


		if(achieveCount == 1){
			tip.text = KoStorage.GetKorString("70122") + "("+"1"+"/11)";
			yield return StartCoroutine("setAchievementList", tip);
		}else if(achieveCount ==2){
			yield return StartCoroutine("setAddAchievementList", tip);
		}


		if(Application.platform == RuntimePlatform.Android){

			if(!EncryptedPlayerPrefs.HasKey("GoogleLogin")){
				bConnect = false;
				Social.localUser.Authenticate((bool success) => {
					if (success) {
						EncryptedPlayerPrefs.SetString("GoogleLogin", "Success");
					} else {
						EncryptedPlayerPrefs.SetString("GoogleLogin", "Failed");
					}
					bConnect = true;
				});
				
				while(!bConnect){
					yield return null;
				}
			}else{
			
				string str = EncryptedPlayerPrefs.GetString("GoogleLogin");
				if(string.Equals("Success", str) == true){
					bConnect = false;
					Social.localUser.Authenticate((bool success) => {
						if (success) {
							//EncryptedPlayerPrefs.SetString("GoogleLogin", "Success");
						} else {
							EncryptedPlayerPrefs.SetString("GoogleLogin", "Failed");
						}
						bConnect = true;
					});
					
					while(!bConnect){
						yield return null;
					}
				
				}else{
				
				}
			
			
			
			
			}

		
		}

		yield return StartCoroutine("myProfileLoad");
		yield return StartCoroutine("getClubAccount");
		while(UserDataManager.instance.bPopUpAdd){
			yield return null;
		}
		if(	CClub.mClubFlag == 2){
			yield return StartCoroutine("PutMyInfo");
			UserDataManager.instance.JiverInit();
		}
		if(CClub.ClubMode == 1){
			if(CClub.mClubInfo.myEntry == 1 || CClub.mClubInfo.myEntryFlag == 1) {
				yield return StartCoroutine("getMyClubRaceInfo");
			}
		}


		/*if(FB.IsLoggedIn){
			networkStatus =KoStorage.GetKorString("70133");// "페이스북 연결 설정중 ";
			if(GV.gInfo.extra01 == 1){
				bFaceBookReTry = false;
				yield return StartCoroutine("GetFaceBookFriendsList");
				yield return StartCoroutine("PutFaceBookFriends");
				yield return StartCoroutine("GetFaceBookMyRank");
				yield return StartCoroutine("GetFacebookFriendsRankList");
				yield return StartCoroutine("GetFacebookFriendsScoreRankList");
				yield return StartCoroutine("PutFacebookFriendsAdd");
				if(bFaceBookReTry){
				
				}else{
					gameRank.instance.listFFR.Clear();
					yield return StartCoroutine("GetFacebookFriendsRankList");
					yield return StartCoroutine("GetFacebookFriendsScoreRankList");
				}
				gameRank.instance.listFFR.Sort(delegate(gameRank.RaceRankInfo x, gameRank.RaceRankInfo y) {
					return y.level.CompareTo(x.level);
				});
				var obj = GameObject.Find("LobbyUI") as GameObject;
				if(obj == null){
					
				}else{
					obj.SendMessage("FaceBookLoginComplete",0,SendMessageOptions.DontRequireReceiver);
				}
				
				
			}else{
				gameRank.instance.myFFRList();
			}
		}*/


		
	}

	IEnumerator PutMyInfo(){
		bool bConnect = false;
		string mAPI = "club/putMyUserInfo";
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		mDic.Add("userInfoData",AccountManager.instance.makeCustomUserData());
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
			}
			bConnect = true;
		});
		while(!bConnect){
			yield return null;
		}
	}

	public IEnumerator getClubAccount(){
		if(CClub.ClubTest == 1){
			CClub.InitClub();
			CClub.ClanMember = 0;
			if(GV.ChSeasonID >= 6010){
				bool bConnect  = false;
				string mAPI = "club/getMyClubInfo";//ServerAPI.Get(90059);
				NetworkManager.instance.ClubBaseConnect("Get",new System.Collections.Generic.Dictionary<string,int>(), mAPI, (request)=>{
					Utility.ResponseLog(request.response.Text, GV.mAPI);
					if(string.IsNullOrEmpty(request.response.Text) == true) {
						GameObject.Find("LoadScene").SendMessage("DisConnectPopup"); //서버 점검 중
						return;
					}
					var thing = SimpleJSON.JSON.Parse(request.response.Text);
					int status = thing["state"].AsInt;
					if (status == 0)
					{
						//{"state":0,"clubMember":1,"clubDescription":"sample modified desc","mClubName":"club_name_58","clubSymbol":"sample modified symbol","clubChatCh":"temp","clubMemberNum":2,"clubMemberTotalNum":30,"victoryNum":"temp","playNum":"temp","clubLevel":2,"clubExp":50,"myEntryFlag":1,"clubIndex":59,"createTime":"2016-04-13 16:11:42","dev_isMatchJustFinished":false,"myEntry":0,"clubMode":0}
						CClub.mClubInfo = new ClubInfo(thing);

						if(CClub.mClubInfo.clubMember == 9){
							CClub.ClanMember = 1;
						}else if(CClub.mClubInfo.clubMember == 5){
							CClub.ClanMember = 2;
						}else{
							CClub.ClanMember =3;
						}
						CClub.mClubFlag = 2;
					
						if(CClub.ClubMode==1){
							myAcc.instance.account.bLobbyBTN[0] = true;
						}else if(CClub.ClubMode == 2){
							myAcc.instance.account.bLobbyBTN[0] = true;
						}else{
							myAcc.instance.account.bLobbyBTN[0] = false;
						}
					}else if(status == -1){
						CClub.mClubFlag =1;
						CClub.ClubMode = 0;
						CClub.ClanMember = 0;
						CClub.ChangeClub = 1;
					}
					//if(Application.isEditor) CClub.ClanMember = 1;
					bConnect = true;
				});
				
				while(!bConnect){
					yield return null;
				}
				
				if(CClub.mClubFlag != 1){
					System.Collections.Generic.Dictionary<string,int> mDicInt = new System.Collections.Generic.Dictionary<string,int>();
					bConnect  = false;
					mAPI = "club/getClubMemberInfo";//ServerAPI.Get(90059);
					mDicInt.Add("clubIndex",CClub.mClubInfo.clubIndex);
					NetworkManager.instance.ClubBaseConnect("Post",mDicInt, mAPI, (request)=>{
						Utility.ResponseLog(request.response.Text, GV.mAPI);
						if(string.IsNullOrEmpty(request.response.Text) == true) {
							GameObject.Find("LoadScene").SendMessage("DisConnectPopup"); //서버 점검 중
							return;
						}
						var thing = SimpleJSON.JSON.Parse(request.response.Text);
						int status = thing["state"].AsInt;
						if (status == 0)
						{
							int cnt = thing["memberList"].Count;
							ClubMemberInfo cMemInfo;
							for(int i =0; i < cnt; i++){
								cMemInfo = new ClubMemberInfo(thing["memberList"][i]);
								CClub.myClubMemInfo.Add(cMemInfo);
							}
							if(CClub.myClubMemInfo.Count >=2){
								CClub.myClubMemInfo.Sort(delegate(ClubMemberInfo x, ClubMemberInfo y) {
									int a = y.clubStarCount.CompareTo(x.clubStarCount);
									if(a == 0)
										a =  y.playNumber.CompareTo(x.playNumber);
									if(a == 0) 	a =  y.mLv.CompareTo(x.mLv);
									return a;
								});
							}
						}else if(status == -1){
						
						}
						bConnect = true;
					});
					
					while(!bConnect){
						yield return null;
					}
				}
				
			}else CClub.mClubFlag = 0; // champId
			
		} //clubTest
	}
	public IEnumerator getMyClubRaceInfo(){
		bool bConnect = false;///club/getMainRaceMyUserInfo
		string mAPI = "club/getMainRaceMyUserInfoDetail";///club/getClubRankingLocal
		int cnt = 0;
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		mDic.Add("clubUserId", GV.UserRevId);
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				ClubRaceMemberInfoDetail mInfo = null;
				CClub.myClubRaceInfoDetail.Clear();
				int cnt1 = thing["raceList"].Count;
				for(int i= 0; i <cnt1; i++){
					string temp =  thing["raceList"][i]["clubRaceData"]["teamId"].Value;
					int.TryParse(temp,out CClub.ClubRaceTeams[i]);
				}

		
			}else if(status == -421){
				
			}else{
				
			}
			
			bConnect = true;
			
		});
		
		while(!bConnect){
			yield return null;
		}
	
	
	
	}
	public IEnumerator FaceBookLoginResult(){
		if(GV.gInfo.extra01 == 1){
			bFaceBookReTry = false;
			yield return StartCoroutine("GetFaceBookFriendsList");
			yield return StartCoroutine("PutFaceBookFriends");
			yield return StartCoroutine("GetFaceBookMyRank");
			yield return StartCoroutine("GetFacebookFriendsRankList");
			yield return StartCoroutine("GetFacebookFriendsScoreRankList");
			yield return StartCoroutine("PutFacebookFriendsAdd");
			if(bFaceBookReTry){
				
			}else{
				gameRank.instance.listFFR.Clear();
				yield return StartCoroutine("GetFacebookFriendsRankList");
				yield return StartCoroutine("GetFacebookFriendsScoreRankList");
			}
			
			
			gameRank.instance.listFFR.Sort(delegate(gameRank.RaceRankInfo x, gameRank.RaceRankInfo y) {
				return y.level.CompareTo(x.level);
			});
			var obj = GameObject.Find("LobbyUI") as GameObject;
			if(obj == null){
				
			}else{
				obj.SendMessage("FaceBookLoginComplete",0,SendMessageOptions.DontRequireReceiver);
			}
		}else{
			gameRank.instance.myFFRList();
			var obj1 = GameObject.Find("LobbyUI") as GameObject;
			if(obj1 != null) 	obj1.SendMessage("FaceBookLoginComplete",0,SendMessageOptions.DontRequireReceiver);
		}
	}
	public IEnumerator GetFaceBookFriendsList(){
		bool bConnect = false;
		//친구 가져오기
		yield return SNSManager.instance.StartCoroutine("getFaceBookFriendsAccountList");
		yield return null;
	}
	
	IEnumerator TimeIntervalCheck(){
		int cnt = 0;
		for(int  i = 0; i < 20; i++){
			yield return new WaitForSeconds(1.0f);
		}
		bFBStatus = true;
	}
	
	
	public void SNSConnectFailedPopup(){
		var temp = GameObject.Find("LoadScene") as GameObject;
		if(temp != null){
			GameObject.Find("LoadScene").SendMessage("SNSFailedPopup"); //
		}else {
			
		}
		if(FB.IsLoggedIn) FB.Logout();
	}
	public IEnumerator PutFaceBookFriends(){
		// 친구 내보내기
		bool bConnect = false;
		string mAPI =ServerAPI.Get(90069); //user/facebookLogin
		string strUserIds = SNSManager.countFaceBookFriend();
		string strUserId = "userFacebookUid;"+FB.UserId;
		strUserIds = "friendFacebookUids;";
		bFBStatus = false;
		StartCoroutine("TimeIntervalCheck");
		NetworkManager.instance.HttpRaceFinishConnect("Put", new Dictionary<string, int>(), mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				SNSConnectFailedPopup();
				bConnect = true;
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				bConnect = true;	
				gameRank.instance.listGift.Clear();
				//Global.gNewMsg++;
				//response:{"state":-203,"msg":"DB UPDATE ERROR"}
				//response:{"state":0,"msg":"sucess","result":{"type":"coin"},"time":1453956441}
			}else if(status == -108){
				// "result":{"anotherUserId":343},"time":1453959054}
				string otherId = thing["result"]["anotherUserId"];
				if(otherId.Equals(GV.UserRevId) == true){
					bConnect = true;	
				}else{
					var obj = GameObject.Find("LoadScene") as GameObject;
					if(obj == null){
						var pop = ObjectManager.SearchWindowPopup() as GameObject;
						pop.AddComponent<multiLoginPopup>().InitPopUp(()=>{
							NetworkManager.instance.ReplaceUserAccount(GV.UserRevId, otherId, FB.UserId, Global.gDeivceID, "1",(mRequest)=>{
								Utility.ResponseLog(mRequest.response.Text, GV.mAPI, 1);
								GameObject.Find("ManagerGroup").SendMessage("GroupDestroy");
								Global.Loading = false;
								Global.isLobby = true;
								Global.isLoadFinish = false;
								//gameObject.SetActive(false);
								Global.gReLoad = 1;
								GV.gInfo = null;
								EncryptedPlayerPrefs.DeleteAll();
								StopAllCoroutines();
								Global.gChampTutorial = 0;
								Application.LoadLevel("Splash");
							});
							
						},()=>{
							if(FB.IsLoggedIn){
								FB.Logout();
							}
							var obj1 = GameObject.Find("LobbyUI") as GameObject;
							if(obj1 != null) 	obj1.SendMessage("FaceBookLoginComplete",1,SendMessageOptions.DontRequireReceiver);
							//obj.SendMessage("FaceBookLoginComplete",1,SendMessageOptions.DontRequireReceiver);
						});
						pop = null;
					}else{
						//obj.SendMessage("multiLoginPopup");
						bConnect = true;
					}
					
				}
			}else{
				if(status == -105){
					GameObject.Find("LoadScene").SendMessage("builtInPopup");
					return;
				}bConnect = true;
			}
			StopCoroutine("TimeIntervalCheck");bFBStatus = true;
		},strUserId, strUserIds);
		
		while(!bConnect){
			yield return null;
		}
		
		yield return null;
	}
	
	
	public IEnumerator GetFaceBookMyRank(){
		// 나의 랭킹 가져오기
		bool bConnect = false;
		string mAPI =ServerAPI.Get(90072); //"user/friend/myRank/"
		NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				SNSConnectFailedPopup();
				bConnect = true;
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				Global.gMyRank[0] = thing["myRank"].AsInt;
			}else{
				if(status == -105){
					GameObject.Find("LoadScene").SendMessage("builtInPopup");
					return;
					
				}
				Global.gMyRank[0] =0;
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
		
	}

	private bool bFBStatus = false;
	private int totalPage = 0;
	public IEnumerator GetFacebookFriendsScoreRankList(){
		// 친구 랭킹 가져오기
		bool bConnect = false;
		string mAPI =ServerAPI.Get(90076); // "user/friend/score/";//
		StartCoroutine("TimeIntervalCheck");
		totalPage  = 0;
		NetworkManager.instance.HttpGetRaceSubInfo("Get", mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				SNSConnectFailedPopup();
				bConnect = true;
				Global.isNetwork = false;
				Global.isPopUp = false;
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				int cnt = thing["result"].Count;
				totalPage = thing["totalPage"].AsInt;
				if(cnt >= 1 ){
					for(int i =0; i < cnt; i++){
						gameRank.instance.AddScoreRankingInfo(thing["result"][i], i, 1);
					}
				}else if(cnt == 0){
					if(gameRank.instance.listFFR.Count == 0)
						gameRank.instance.myFFRList();
					
				}else{
					
				}
				
			}else{
				Global.isNetwork = false;
				Global.isPopUp = false;
				if(status == -105){
					GameObject.Find("LoadScene").SendMessage("builtInPopup");
					return;
					
				}
			}/*
			var obj = GameObject.Find("LobbyUI") as GameObject;
			if(obj == null){
				
			}else{
				if(totalPage < 2)
					obj.SendMessage("FaceBookLoginComplete",0,SendMessageOptions.DontRequireReceiver);
			}*/
			bConnect = true;	
			StopCoroutine("TimeIntervalCheck");
		},null,1);
		
		while(!bConnect){
			yield return null;
		}
		
		for(int i = 2; i <= totalPage; i++){
			bConnect = false;
			NetworkManager.instance.HttpGetRaceSubInfo("Get", mAPI, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				if(string.IsNullOrEmpty(request.response.Text) == true) {
					SNSConnectFailedPopup();
					Global.isNetwork = false;
					Global.isPopUp = false;
					bConnect = true;
					return;
				}
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					//response:{"state":0,"msg":"sucess","totalPage":0,"result":[],"time":1451318193}
					int cnt = thing["result"].Count;
					if(cnt != 0){
						for(int j =0; j < cnt; j++){
							gameRank.instance.AddScoreRankingInfo(thing["Result"][j], j, 1);
						}
					}
				}else{
					if(status == -105){
						//GameObject.Find("LoadScene").SendMessage("builtInPopup");
						return;
					}
					Global.isNetwork = false;
					Global.isPopUp = false;
				}/*
				var obj = GameObject.Find("LobbyUI") as GameObject;
				if(obj == null){
					
				}else{
					obj.SendMessage("FaceBookLoginComplete",0,SendMessageOptions.DontRequireReceiver);
				}*/
				bConnect = true;	
			},null,i);
			
			while(!bConnect){
				yield return null;
			}
		}
		StopCoroutine("TimeIntervalCheck");
		
	}
	
	
	public IEnumerator GetFacebookFriendsRankList(){
		// 친구 랭킹 가져오기
		bool bConnect = false;
		string mAPI = ServerAPI.Get(90071); //"user/friend/"
		StartCoroutine("TimeIntervalCheck");
		totalPage  = 0;
		NetworkManager.instance.HttpGetRaceSubInfo("Get", mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				SNSConnectFailedPopup();
				bConnect = true;
				Global.isNetwork = false;
				Global.isPopUp = false;
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				//유저 정보
				//response:{"state":0,"msg":"sucess","totalPage":0,"result":[],"time":1451318193}
				int cnt = thing["result"].Count;
				totalPage = thing["totalPage"].AsInt;
				Debug.LogWarning("totalpage" + totalPage);
				if(cnt >= 1 ){
					gameRank.instance.InitializeFFRList(thing["result"], cnt, totalPage);
				}else if(cnt == 0){
					//cnt == 0 
					gameRank.instance.myFFRList();
				}else{
					
				}
				
			}else{
				Global.isNetwork = false;
				Global.isPopUp = false;
				if(status == -105){
					//	GameObject.Find("LoadScene").SendMessage("builtInPopup");
					return;
					
				}
			
			}
			bConnect = true;
			StopCoroutine("TimeIntervalCheck");
		},null,1);
		
		while(!bConnect){
			yield return null;
		}
		
		
		for(int i = 2; i <= totalPage; i++){
			bConnect = false;
			NetworkManager.instance.HttpGetRaceSubInfo("Get", mAPI, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				if(string.IsNullOrEmpty(request.response.Text) == true) {
					SNSConnectFailedPopup();
					bConnect = true;
					Global.isNetwork = false;
					Global.isPopUp = false;
					return;
				}
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					//response:{"state":0,"msg":"sucess","totalPage":0,"result":[],"time":1451318193}
					int cnt = thing["result"].Count;
					gameRank.instance.addFFRList(thing["result"], cnt, totalPage);
				}else{
					Global.isNetwork = false;
					Global.isPopUp = false;
					if(status == -105){
						//GameObject.Find("LoadScene").SendMessage("builtInPopup");
						return;
					}
				}
				
				bConnect = true;	
			},null,i);
			
			while(!bConnect){
				yield return null;
			}
		}
		StopCoroutine("TimeIntervalCheck");
	}
	
	private bool bFaceBookReTry = false;
	public IEnumerator PutFacebookFriendsAdd(){
		//친구 추가 하기
		string mAPI = ServerAPI.Get(90070); //"user/friend"
		string strUserIds = SNSManager.CompareFriendCount();
		if(string.IsNullOrEmpty(strUserIds)==true) {
			bFaceBookReTry = true;
			yield break;
		}
		bool bConnect = false;
		string[] strIds = strUserIds.Split(',');
		for(int i = 0; i < strIds.Length; i++){
			bConnect = false;
			strUserIds = "friendFacebookUids;"+strIds[i];
			Debug.LogWarning("strUserIds0 " + strUserIds);
			NetworkManager.instance.HttpFormConnect("Post", new Dictionary<string, int>(), mAPI, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				if(string.IsNullOrEmpty(request.response.Text) == true) {
				
				}else{
					var thing = SimpleJSON.JSON.Parse(request.response.Text);
					int status = thing["state"].AsInt;
					if (status == 0)
					{
					
					}else{
						if(status == -105){
							GameObject.Find("LoadScene").SendMessage("SNSFailedPopup");
						}else if(status == -203){
							Utility.LogWarning("strUserIds1 " + strUserIds);
						}
					}
				}
				bConnect = true;
			},strUserIds);
			
			while(!bConnect){
				yield return null;
			}
		}
		yield return null;
	}


	public IEnumerator setAchievementList(UILabel tip){
		int id = 16003;
		GAchieve.instance.achieveInfo.SetUserAchievementInfo(16000, 0, 0, 0);
		for(int i =0; i < 12; i++){
			id = 16003 + 3*i;
			GAchieve.instance.achieveInfo.SetUserAchievementInfo(id, 0, 0, 0);
			tip.text = KoStorage.GetKorString("70122") + "("+i+"/11)";
			yield return StartCoroutine("UnLockAchievement", id);
		}
	}

	public IEnumerator setAddAchievementList(UILabel tip){
		int id = 16003;
		for(int i =0; i < 12; i++){
			id = 16003 + 3*i;
			if(GAchieve.instance.achieveInfo.exsitAchieve(id)){
			
			}else{
				GAchieve.instance.achieveInfo.SetUserAchievementInfo(id, 0, 0, 0);
				yield return StartCoroutine("UnLockAchievement", id);
			}
		}
	}
	
	public IEnumerator UnLockAchievement(int id){
		Dictionary<string, int> mDic = new Dictionary<string, int>();
		bool bConnect = false;
		mDic.Clear();
		mDic.Add("acheivmentId",id);
		//NetworkManager.instance.mRequestRange++;
		string mAPI = ServerAPI.Get(90060);//"game/acheivment"
		NetworkManager.instance.HttpFormConnect("Post",mDic,mAPI, (request)=>{

			Utility.ResponseLog(request.response.Text, GV.mAPI);
			//NetworkManager.instance.HttpFormConnect("Post",mDic, "game/acheivment", (request)=>{
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				if(GameObject.Find("LoadScene") == null){
					ErrorPopUp();
				}else{
					GameObject.Find("LoadScene").SendMessage("DisConnectPopup"); //서버 점검 중
				}
			
				return;
			}
			//Utility.Log("UnLockAchievement response:" + request.response.Text);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				GAchieve.instance.achieveInfo.SetUserAchievementInfo(id, 0, 0, 0);
				GoogleAnalyticsV4.instance.LogEvent("Achievement", "UnLock", id.ToString(),0);
			}else{
				ErrorPopUp();
			}
			bConnect = true;
			
		});
		while(!bConnect){
			yield return null;
		}

		if(id == 16007 ){
			if(GV.ChSeasonID >= 6015){
				//GAchieve.instance.achieveInfo.SetAchievementInfo(id, 0, 0, 0);
				GAchieve.instance.achieveInfo.FinishUserAchievement(id);
				GameObject.Find ("LobbyUI").SendMessage("SetAchievementIncrease");
			}
		}else if(id == 16008){
			if(GV.ChSeasonID >= 6030){
				//GAchieve.instance.achieveInfo.SetAchievementInfo(id, 0, 0, 0);
				GAchieve.instance.achieveInfo.FinishUserAchievement(id);
				GameObject.Find ("LobbyUI").SendMessage("SetAchievementIncrease");
			}
		}

	}
	public void SetNextAchievement(int idx, int mid){
		StartCoroutine(SettingNextAchievement(idx, mid));
	}
	
	IEnumerator SettingNextAchievement(int idx, int mid){
		Global.isNetwork= true;
		Dictionary<string, int> mDic = new Dictionary<string, int>();
		bool bConnect = false;
		mDic.Clear();
		mDic.Add("acheivmentId",idx);
		string mAPI = ServerAPI.Get(90062);//game/acheivment/claim
		NetworkManager.instance.HttpFormConnect("Put",mDic, mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				GAchieve.instance.achieveInfo.SetAchieveComplete(idx);
			}else{
				ErrorPopUp();
			}
			bConnect = true;
		});
		while(!bConnect){
			yield return null;
		}
		if(mid != 0) {
			idx++;
			 AchievementInfo.AchievementSubInfo aInfo  = GAchieve.instance.achieveInfo.ListAchieve.Find((obj) =>obj.AcheiveID == idx);
			SNSManager.unLockAchievement(aInfo.G_ID);
			yield return StartCoroutine("UnLockAchievement", idx);
		}
		Global.isNetwork= false;
	}


	public IEnumerator ReceiveGetPersonalInfo(){
		Global.gLvUp=0;
		yield return new WaitForSeconds(1.0f);
	}

	public string SponTimeCheck(int mCnt = 0, int sponTeamId = 0){
		int teamID = GV.SelectedTeamID;
	//	if(sponTeamId != 0) teamID = GV.SelectedSponTeamID;
	//	else teamID = GV.SelectedTeamID;
		myTeamInfo mTeam = GV.getTeamTeamInfo(teamID);
		if(mTeam == null) return string.Empty;
		if(mTeam.SponID == 1300 || mTeam.SponID == 0) return string.Empty;
		long SponTime = mTeam.SponRemainTime;
		System.DateTime eTime = new System.DateTime(SponTime);
		System.DateTime cTime = NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		System.TimeSpan wTime = eTime - cTime;
		string timeString =null;
		if(wTime.TotalSeconds < 0){
			timeString = string.Empty;
			mTeam.SponID = 1300;
			bSpon = false;
			if(mCnt == 1)
				GV.TeamChangeFlag = 4;
			else{
				//GV.TeamChangeFlag = 4;
			}
			timeString ="Expire";
		}else{
			bSpon = true;
			timeString =  string.Format (KoStorage.GetKorString("71008"),wTime.Hours, wTime.Minutes); 
		}
		return timeString;
	}

	public void LobbySponTimeCheck(){
		string str = SponTimeCheck();
		//Utility.LogWarning("str " + str);
		if(str.Equals("Expire")){
			Global.gExpireSpon = 1;
			bSpon = false;
		}else{
			Global.gExpireSpon = 0;
			SetSponTime();
		}
	}

	public void CreateSponExpirePop(){
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<SponExpirePopup>().InitPopUp(()=>{

			myTeamInfo mTeam = GV.getTeamTeamInfo(GV.SelectedTeamID);
			if(mTeam.freeSpon == 1){
				ClubSponInfo.instance.mClubSpon.unSetTeamSpon(GV.SelectedTeamID);
				bSpon = false;
				GV.TeamChangeFlag = 4;
				return;
			}

			Dictionary<string, int> mDic = new Dictionary<string,int>();
			mDic.Add("teamId",GV.SelectedTeamID);
			string mAPI = ServerAPI.Get(90009);// "game/team/sponsor"
			NetworkManager.instance.HttpFormConnect("Delete", mDic, mAPI, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					
				}else{
					
				}
			});
		});
		bSpon = false;
		GV.TeamChangeFlag = 4;
		pop = null;
	}
	private System.DateTime sponTime;
	private System.DateTime nowTime;
	private System.TimeSpan pTime;
	private bool bSpon;
	public void SetSponTime(int TeamID = 0){
		myTeamInfo mTeam;
		if(TeamID == 0){
			mTeam = GV.getTeamTeamInfo(GV.SelectedTeamID);
		}else{
			mTeam = GV.getTeamTeamInfo(TeamID);
		}
		if(mTeam == null) {bSpon = false; return;}
		if(mTeam.SponID == 1300 || mTeam.SponID == 0){
			bSpon = false;
		}else {
			bSpon = true;
			sponTime = new System.DateTime(mTeam.SponRemainTime);
			pTime = sponTime-NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
			if(pTime.TotalHours >0)
			{

			}
			else{
				mTeam.SponID = 1300;
				bSpon = false;
			}
		}
	}

	public string RunningSponTime(){
		if(!bSpon) return  string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
		pTime = sponTime-NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		if(pTime.TotalHours >0)
			return string.Format("{0:00}:{1:00}:{2:00}", pTime.Hours, pTime.Minutes, pTime.Seconds);
		else 
			return string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
	}

	public string SetDayTime(){
		long dayTime = myAccount.instance.account.dayCheckTime;
		System.DateTime mNowTime   = NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		System.DateTime mResetTime = new System.DateTime(dayTime);
		System.TimeSpan mCompareTime = (mResetTime - mNowTime);
		int a = (int)mCompareTime.TotalHours / 24;
		if(a != 0) {
			System.DateTime rTime = mResetTime.AddDays((double)a);
			mResetTime = rTime;
		}
		return string.Format("{0:00}:{1:00}:{2:00}", pTime.Hours, pTime.Minutes, pTime.Seconds);
	}

	public void SetCarTexture(GameObject car, int strSpon){
		string carid = car.name;
		var carText = car.GetComponent<CarType>() as CarType;
		carText.CarTextureInitialize(carid,strSpon.ToString());
	
	}
	public System.Action ErrorCallback;
	public void ErrorPopUp(int status = 0){
		GameObject obj = null;
		obj = GameObject.Find("LobbyUI");
		if(obj !=null){
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			pop.AddComponent<disconnectPopup>().InitPopUp();
			return;
		}

		obj = GameObject.Find("LoadScene");
		if(obj != null){
			UserDataManager.instance.bPopUpAdd = true;
			obj.SendMessage("DisConnectPopup");
			return;
		}

		obj = GameObject.Find("GUIManager");
		if(obj != null){
			obj.SendMessage("DisConnectPopup");
			return;
		}
	}
	
	public void BitInPopUp(){		
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<BITINPopup>().InitPopUp();
	}
	
	public void OverlapLoginError(){
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<OverlapPopup>().InitPopUp();
	}

	public string makeCustomUserData(){
		string str = string.Empty;
		if(string.IsNullOrEmpty(GV.UserNick) == true){
			str = EncryptedPlayerPrefs.GetString("mNick");
		}else{
			str  = GV.UserNick;
		}
		Utility.LogWarning(str);
		JSONObject obj = new JSONObject();
		obj.AddField("profileUrl", GV.mUser.profileURL);
		obj.AddField("nickName",str);
		obj.AddField("mLv",Global.level.ToString());
		obj.AddField("Ltime",NetworkManager.instance.GetCurrentDeviceTime().ToString("yyyy/MM/dd hh:mm:ss"));

		//Utility.LogWarning("obj " + obj);
		return obj.Print();
	}


	public string pathForDocumentsFile( string fileName )
	{
		
		if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			string path = Application.dataPath.Substring( 0, Application.dataPath.Length - 5 );
			path = path.Substring( 0, path.LastIndexOf('/'));
			return System.IO.Path.Combine( System.IO.Path.Combine( path, "Documents"), fileName );
		}
		else if( Application.platform == RuntimePlatform.Android )
		{
			string path = Application.persistentDataPath;
			path = path.Substring( 0, path.LastIndexOf('/'));
			return System.IO.Path.Combine( path, fileName );
		}
		else
		{
			string path = Application.dataPath;
			
			path = path.Substring( 0, path.LastIndexOf('/'));
			//Utility.Log (path);
			return System.IO.Path.Combine( path, fileName );
		}
	}

	public void SaveTextureFile(string fileName, Texture2D SaveImage)
	{
	
		byte[] bytes = SaveImage.EncodeToPNG();
		System.IO.File.WriteAllBytes(fileName, bytes);
	}
	
	public Texture2D LoadTextureFile(string fileName)
	{
		Texture2D Result = new Texture2D(4, 4, TextureFormat.ARGB32, false);
		byte[] bytes = System.IO.File.ReadAllBytes(fileName);
		if (!Result.LoadImage(bytes)) return null;
		else return Result;
	}
}
