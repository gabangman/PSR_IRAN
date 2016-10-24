
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
//using HTTP;
using UnityHTTP;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Pathfinding.Serialization.JsonFx;
using Heyzap;


public class NetworkManager : MonoSingleton< NetworkManager >
{
	public int mTailCount = 0;
	// ----------------------------------------------------------------------------------------------------------- //
	void Awake()
	{	
		//prefixUrl = "http://ec2-52-68-117-216.ap-northeast-1.compute.amazonaws.com/pitinracing/v1/";
		//GAME_ID = "f35cae92dc65bb80b9d7f03a9ac60d853";
		//prefixUrl = "http://d3tmmadlzkl28l.cloudfront.net/pitinracing/v1/"; //트리니티 cloudflont
		// prefixUrl = "http://d33ke9tjkwdhqf.cloudfront.net/pitinracing/v1/";//개발서버 cloudflont

	}
	public void SetGameID(){
	// prefixUrl  = "http://ec2-52-9-118-109.us-west-1.compute.amazonaws.com/pitinracing/v1/"; //트리니티 개발 서버
		GAME_ID = Base64Manager.instance.getEncrpytString(GAME_ID, "PitStop");
	}

	public void DoGetHostAddresses(string hostname = null)
	{
		hostname = "lb-pitinracing-trinity-1410456635.us-west-1.elb.amazonaws.com";
		IPAddress[] ips;
		ips = Dns.GetHostAddresses(hostname);
		if (ips [0].AddressFamily.ToString () == ProtocolFamily.InterNetworkV6.ToString ()) {
			GV.ipAddressVersion = 6;
		} else {
			GV.ipAddressVersion = 0;
		}
	}

	// admin216 65f1484343178be6
	void Start()
	{
		DontDestroyOnLoad(gameObject);
		HeyzapAds.Start("c280cb29bef12774d528e31507a1125b",HeyzapAds.FLAG_NO_OPTIONS);
	}

	private string prefixUrl = "http://lb-pitinracing-trinity-1410456635.us-west-1.elb.amazonaws.com/pitinracing/v1/";
	private string GAME_ID = "IwVG635tZeS92Cc7oxUk3w6KtoI2z xi8UclXQO35ABwlXOTSdIPgS8y7kYxIZtQMxZBHXgdAoD74Cr5jT2F6uIq pvGfPQe7glGhd1BW8U=";

	private string mKey;
	public  int mRequestRange;
	private string mAuthKey;
	private string mSeed;
	public bool okGameInfo = false;
	public void ClubBaseConnect(string httpType, Dictionary<string,int> DicForm, string mAPI, System.Action<Request> Callback, string strServer = null)
	{
		GV.mAPI = mAPI;
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		WWWForm form = new WWWForm();
		if(DicForm.Count != 0){
			foreach(KeyValuePair<string, int> dic in DicForm) 
			{ 
				form.AddField(dic.Key , dic.Value); 
			}
		}

		if(string.IsNullOrEmpty(strServer) == false){
			string[] strs = strServer.Split(';');
			form.AddField(strs[0] , strs[1]); 
		}
		UnityHTTP.Request someRequest = new UnityHTTP.Request(httpType, prefixUrl + mAPI, form);
		tempReq =  new UnityHTTP.Request(httpType, prefixUrl + mAPI, form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if(status == -110){
				multiActionErropPopup();
			}else{
				Callback(request);
			}
		}));
	}


	public void ClubBaseStringConnect(string httpType, Dictionary<string,string> DicForm, string mAPI, System.Action<Request> Callback)
	{
		GV.mAPI = mAPI;
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		WWWForm form = new WWWForm();
		if(DicForm.Count != 0){
			foreach(KeyValuePair<string, string> dic in DicForm) 
			{ 
				form.AddField(dic.Key , dic.Value); 
			}
		}
		UnityHTTP.Request someRequest = new UnityHTTP.Request(httpType, prefixUrl + mAPI, form);
		tempReq = new UnityHTTP.Request(httpType, prefixUrl + mAPI, form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if(status == -110){
				multiActionErropPopup();
			}else{
				Callback(request);
			}
			
		}));
	}

	/*
	public void ValidateReceipt(int platform , string signedData, string signatrue, string keys , System.Action<Request> callBack)
	{
		WWWForm form = new WWWForm();
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		form.AddField("platform", 1);
		form.AddField("signedData",signedData );
		form.AddField("signature", signatrue);
		form.AddField("public_key_base64", keys);
		GV.mAPI = "game/purchase/validation";
		UnityHTTP.Request someRequest = new UnityHTTP.Request("Put", prefixUrl + "game/purchase/validation", form);
		tempReq = new UnityHTTP.Request("Put", prefixUrl + "game/purchase/validation", form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			callBack(request);
			
		}));
	} */


	public void BuyItemConnect(bool isCashItem, int itemId,  System.Action<Request> callBack, string orderId=null, int platform=0, int price=0, string currency=null){
		WWWForm form = new WWWForm();
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		form.AddField("id", GV.UserRevId);
		form.AddField("storeItemId", itemId);
		if (isCashItem)
		{
			form.AddField("orderId", orderId);
			form.AddField("platform", platform);
			form.AddField("price", price);
			form.AddField("currency", currency);
		}
		GV.mAPI = "game/purchase";
		UnityHTTP.Request someRequest = new UnityHTTP.Request("Put", prefixUrl + "game/purchase", form);
		tempReq =  new UnityHTTP.Request("Put", prefixUrl + "game/purchase", form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			callBack(request);
			
		}));

	}
	public void BuyPackageItemConnect(int itemId,  System.Action<Request> callBack, string orderId=null, int platform=0, int price=0, string currency=null){
		WWWForm form = new WWWForm();
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		form.AddField("id", GV.UserRevId);
		form.AddField("storeItemId", itemId);
		form.AddField("orderId", orderId);
		form.AddField("platform", platform);
		form.AddField("price", price);
		form.AddField("currency", currency);
		GV.mAPI = "game/purchase/package";
		UnityHTTP.Request someRequest = new UnityHTTP.Request("Post", prefixUrl + "game/purchase/package", form);
		tempReq = new UnityHTTP.Request("Post", prefixUrl + "game/purchase/package", form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			callBack(request);
		}));
	}
	public void BuyPackageItemConnectAndroid(int itemId,  System.Action<Request> callBack, string signedData=null, string signature=null){
		WWWForm form = new WWWForm();
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		form.AddField("selectCarId", itemId);
		form.AddField("signedData", signedData);
		form.AddField("signature", signature);
		GV.mAPI = "game/purchase/withVerifyAndroid";
		UnityHTTP.Request someRequest = new UnityHTTP.Request("Post", prefixUrl + "game/purchase/withVerifyAndroid", form);
		tempReq = new UnityHTTP.Request("Post", prefixUrl + "game/purchase/withVerifyAndroid", form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			callBack(request);
			
		}));
	}



	public void HttpConnect(string httpType,string mAPI, System.Action<Request> Callback){
		GV.mAPI = mAPI;
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		UnityHTTP.Request someRequest = new UnityHTTP.Request(httpType, prefixUrl + mAPI+GV.UserRevId);
		tempReq =  new UnityHTTP.Request(httpType, prefixUrl + mAPI+GV.UserRevId);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		
		StartCoroutine(SendToReceive(someRequest,(request)=>{

			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if(status == -110){
				multiActionErropPopup();
			}else{
				Callback(request);
			}
		}));
	
	}
	

	
	public void HttpFormConnect(string httpType, Dictionary<string,int> DicForm, string mAPI, System.Action<Request> Callback, string strRecords = null){
		GV.mAPI = mAPI;
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		WWWForm form = new WWWForm();
		form.AddField("id", GV.UserRevId);
		if(DicForm.Count != 0){
			foreach(KeyValuePair<string, int> dic in DicForm) 
			{ 
				form.AddField(dic.Key , dic.Value); 
			}
		}
		if(string.IsNullOrEmpty(strRecords)== false){
			string[] strs = strRecords.Split(';');
			form.AddField(strs[0] , strs[1]); 
		}
		
		UnityHTTP.Request someRequest = new UnityHTTP.Request(httpType, prefixUrl + mAPI, form);
		tempReq = new UnityHTTP.Request(httpType, prefixUrl + mAPI, form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if(status == -110){
				multiActionErropPopup();
			}else{
				Callback(request);
			}
			
		}));
	
	}
	public void HttpRaceFinishConnect(string httpType, Dictionary<string,int> DicForm, string mAPI, System.Action<Request> Callback, string strRecords = null, string strRaceData = null){
		GV.mAPI = mAPI;
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		WWWForm form = new WWWForm();
		form.AddField("id", GV.UserRevId);
		if(DicForm.Count != 0){
			foreach(KeyValuePair<string, int> dic in DicForm) 
			{ 
				form.AddField(dic.Key , dic.Value); 
			}
		}
		if(string.IsNullOrEmpty(strRecords)== false){
			string[] strs = strRecords.Split(';');
			form.AddField(strs[0] , strs[1]); 
		//	Utility.LogWarning(strs[0] + " + " + strs[1]);
		}
		if(string.IsNullOrEmpty(strRaceData)== false){
			string[] strs1 = strRaceData.Split(';');
			form.AddField(strs1[0] , strs1[1]); 
		//	Utility.LogWarning(strs1[0] + " + " + strs1[1]);
		}

		UnityHTTP.Request someRequest = new UnityHTTP.Request(httpType, prefixUrl + mAPI, form);
		tempReq =  new UnityHTTP.Request(httpType, prefixUrl + mAPI, form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());

		StartCoroutine(SendToReceive(someRequest,(request)=>{
			Callback(request);
		
		}));
	}

	public void HttpGetRaceSubInfo(string httpType, string mAPI, System.Action<Request> Callback, string sInfo = null, int page = 0){
		GV.mAPI = mAPI;
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		UnityHTTP.Request someRequest;
		if(page == 0){
			someRequest = new UnityHTTP.Request(httpType, prefixUrl + mAPI+GV.UserRevId+"/"+sInfo);
			tempReq = new UnityHTTP.Request(httpType, prefixUrl + mAPI+GV.UserRevId+"/"+sInfo);
		}else{
			someRequest = new UnityHTTP.Request(httpType, prefixUrl + mAPI+GV.UserRevId+"/"+page);
			tempReq = new UnityHTTP.Request(httpType, prefixUrl + mAPI+GV.UserRevId+"/"+page);
		}


		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			Callback(request);
			
		}));
	}

	public void multiActionErropPopup(){
		var obj =GameObject.Find("LoadScene") as GameObject;//.SendMessage("builtInPopup");
		if(obj == null){
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			pop.AddComponent<OverlapPopup>().InitPopUp();
			pop = null;
		
		}else{
			obj.SendMessage("OverlapPopup");
		}
	
	}

	public void ReplaceUserAccount(string id, string anotherUserId, string userFacebookUid, string deviceId, string platform, System.Action<Request> callback = null)
	{
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		WWWForm form = new WWWForm();
		form.AddField("id", id);
		form.AddField("anotherUserId", anotherUserId);
		form.AddField("userFacebookUid", userFacebookUid);
		form.AddField("deviceId", deviceId);
		if(Application.platform == RuntimePlatform.Android)
			form.AddField("platform", "1");
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
			form.AddField("platform", "2");
		else form.AddField("platform", "1");

		GV.mAPI = "user/account/replace";
		UnityHTTP.Request someRequest = new UnityHTTP.Request("Put", prefixUrl + "user/account/replace", form);
		tempReq = new UnityHTTP.Request("Put", prefixUrl + "user/account/replace", form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				GV.UserRevId = thing["result"]["changedUserId"];
				EncryptedPlayerPrefs.SetString("mUserId",GV.UserRevId);
				
			}
			callback(request);
			
		}));
		
	}
	public void AccountInfo(System.Action<int> callback){
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		string platform = null;
		if(Application.platform == RuntimePlatform.Android)
			platform = "1";
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
			platform = "2";
		else{
			platform = "1";
		}
		GV.mAPI = "user/account/";
		UnityHTTP.Request someRequest = new UnityHTTP.Request("Get", prefixUrl + "user/account/" + Global.gDeivceID+ "/" + platform);
		tempReq = new UnityHTTP.Request("Get", prefixUrl + "user/account/" + Global.gDeivceID+ "/" + platform);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["status"].AsInt;
			int mRe = 0;
			if (status == 0)
			{//response:{"state":0,"msg":"sucess","id":343,"nickName":"ahnsoni"}
				//response:{"state":0,"msg":"sucess","id":null,"nickName":null}
				string nickName  = thing["nickName"];
				if(nickName == "null"){
					GV.UserNick = nickName;
					EncryptedPlayerPrefs.SetString("mNick", nickName);
					mRe = 1;
				}else{
					GV.UserNick = nickName;
					GV.UserRevId = thing["id"];
					EncryptedPlayerPrefs.SetString("mUserId", GV.UserRevId);
					EncryptedPlayerPrefs.SetString("mNick", GV.UserNick);
				//	Utility.LogWarning(GV.UserNick);
					mRe = 2;
				}
				callback(mRe);
			}else{
				callback(3);
			}
			
		}));
	}


	
	/**
    * [2016/01/29] 추가
    * 사용자 재화 갱신 요청
    *  
    * [참고]
    * 
    */

	private void CheckTailValue(){
		mRequestRange = mTailCount;
		if(mTailCount == mRequestRange){
			//Debug.LogWarning("SameTail");
		}else{
			//mRequestRange = mTailCount;
			//Debug.LogWarning(string.Format("{0} diff {1}", mRequestRange, mTailCount));
		}
	}

	public void OnQuestReward(int type, int value, System.Action<bool> callback){
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		GV.mAPI = "user/updateItems";
		WWWForm form = new WWWForm();
		form.AddField("id", GV.UserRevId);
		int reMat = 0;
		if(type == 3) { //coin
			form.AddField("nType","2");
			form.AddField("nValue",value);
		}else if(type == 4) { // dollar
			form.AddField("nType","0");
			form.AddField("nValue",value);
		}else if(type == 5) { //evo
			form.AddField("nType","3");
			string strMat = "8620:"+value.ToString();
			form.AddField("nValue",strMat);//+(attendItem.R_no*dReward).ToString());
		}else if(type == 6){  //silver
			form.AddField("nType","4");
			form.AddField("nValue",value);
		}else if(type == 7){ //gold 
			form.AddField("nType","5");
			form.AddField("nValue",value);
		}else if(type == 8){ //material 
			form.AddField("nType","3");
			reMat = (int)Well512.Next(8600,8620);
			string strMat = reMat.ToString()+":"+value.ToString();
			form.AddField("nValue",strMat);//+(attendItem.R_no*dReward).ToString());
		}

		UnityHTTP.Request someRequest = new UnityHTTP.Request("Put", prefixUrl + "user/updateItems", form);
		tempReq = new UnityHTTP.Request("Put", prefixUrl + "user/updateItems", form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
					GameObject Lobby = GameObject.Find("LobbyUI") as GameObject;
				switch(type){
					case 3 :{
					GV.myCoin = GV.myCoin+value;
						Lobby.SendMessage("InitTopMenuReward", SendMessageOptions.DontRequireReceiver);
					}	break;
					case 4 :{
						GV.myDollar = GV.myDollar+value;
						Lobby.SendMessage("InitTopMenuReward", SendMessageOptions.DontRequireReceiver);
					}break;
					case 8 :{
						GV.UpdateMatCount(reMat,value);
						Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
						myAcc.instance.account.bInvenBTN[1] = true;
					}break;
					case 6 : {
						//	str = "SilverBox";
						GV.UpdateCouponList(0, value);	Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
						myAcc.instance.account.bInvenBTN[3] = true;
					}break;
					case 7 : {
						//	str = "GoldBox";
					GV.UpdateCouponList(1, value);	Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
						myAcc.instance.account.bInvenBTN[3] = true;
					}break;
					case 5 : { //evoCube
						GV.UpdateMatCount(8620, value);	Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
						myAcc.instance.account.bInvenBTN[2] = true;
					}break;

				}

				callback(true);

			}else{
				callback(false);
			}
			
		}));


	}
	public void ChangeUserGoods(string id, int coin, int fuel)
	{
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		GV.mAPI = "user/goods";
		WWWForm form = new WWWForm();
		form.AddField("id", id);
		form.AddField("coin", coin);
		form.AddField("fuel", fuel);
		
		UnityHTTP.Request someRequest = new UnityHTTP.Request("Put", prefixUrl + "user/goods", form);
		tempReq = new UnityHTTP.Request("Put", prefixUrl + "user/goods", form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				Global.isNetwork = false;
			}
			
		}));
	}

	public GameObject OnNetworkFailed(){

		var temp = ObjectManager.CreatePrefabs("Window", "popUp_Network") as GameObject;
		//	exitObj = temp;
		var obj = GameObject.Find("LobbyUI")	 as GameObject;
		if(obj != null){
			var par  = GameObject.FindGameObjectWithTag("BottomAnchor");
			temp.transform.parent = par.transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = new Vector3(0,360,-1500);
			temp.transform.localEulerAngles = Vector3.zero;
			temp.gameObject.SetActive(true);
			//	temp.GetComponent<GamePopup>().InitGameExit();
			return temp;
		}
		obj = GameObject.Find("LoadScene");
		if(obj != null){
			var par = obj.transform.FindChild("Camera").FindChild("Anchor") as Transform;
			temp.transform.parent = par.transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = new Vector3(0,0,-1500);
			temp.transform.localEulerAngles = Vector3.zero;
			temp.gameObject.SetActive(true);
			//	temp.GetComponent<GamePopup>().InitGameExit();
			return temp;
		}
		obj = GameObject.Find("GUIManager");
		if(obj != null){
			var par = GameManager.instance.getNetwork() as Transform;
			temp.transform.parent = par.transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = new Vector3(0,0,-100);
			temp.transform.localEulerAngles = Vector3.zero;
			temp.gameObject.SetActive(true);
			//	temp.GetComponent<GamePopup>().InitGameExit();
			return temp;
		}

		return temp;
	}

	public GameObject failedObject;
	private bool isNetworking = false;
	/*public IEnumerator SendToReceive_1(UnityHTTP.Request someRe, System.Action<Request> callback){

		while(isNetworking){
			yield return null;
		}
	
		isNetworking = true;
		bool bConnect = false;
		apiCount++;
		int status = 10;
		UnityHTTP.Request objNode = null;
		GameObject objPopup  = null;
		for(int i = 0; i < 100; i++){
			bConnect = true;
			someRe.Send((request) =>
			{
				try{
					string strRequest = request.response.Text;
					if(string.IsNullOrEmpty(strRequest) == true){
						if(objPopup == null){
							objPopup = OnNetworkFailed();
							failedObject = objPopup;
							if(objPopup.GetComponent<NetworkEmail>() == null){
								objPopup.AddComponent<NetworkEmail>();
							}
							objPopup.GetComponent<NetworkEmail>().SetInit();
						}
						UserDataManager.instance.bPopUpAddNetwork = true;
					}else{
						objNode = request;
						var thing = SimpleJSON.JSON.Parse(strRequest);
						Utility.LogWarning(thing);
						try{
							status = thing["state"].AsInt;
						}catch(Exception e){
							status = -103;
						}
					}
					bConnect = false;
				}catch (Exception e){
					Utility.LogWarning(e);
					if(objPopup == null){
						objPopup = OnNetworkFailed();
						failedObject = objPopup;
						if(objPopup.GetComponent<NetworkEmail>() == null){
							objPopup.AddComponent<NetworkEmail>();
						}
						objPopup.GetComponent<NetworkEmail>().SetInit();
					}
					UserDataManager.instance.bPopUpAddNetwork = true;
					bConnect = false;
				}
			});
				while(bConnect){
					yield return null;
				}

			if(objNode != null) break;
			if( i == 99) {
				if(objPopup != null) {objPopup.SetActive(false); objPopup = null;failedObject.SetActive(false); failedObject = null;
					if(UserDataManager.instance.bPopUpAddNetwork){
						if(UserDataManager.instance.bPopUpAdd)
							UserDataManager.instance.setGameExit();
					}
					UserDataManager.instance.bPopUpAddNetwork = false;
				}
				isNetworking = false;
				AccountManager.instance.ErrorPopUp();
				break;
			}
		}

		if(status == -105 || status == -203 || status == -200 || status == -201 || status == -202 || status == -204){
			yield return StartCoroutine("GameInfoReStart");
			if(status == -105){

			 	bConnect =true;
				mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
				tempReq.SetHeader("CAMNUM", mKey);
				tempReq.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
				tempReq.SetHeader("TAIL",mRequestRange.ToString());
				tempReq.Send((request) =>
				            {
						isNetworking = false;
						bConnect =false;
						objNode = request;
						string strRequest = request.response.Text;
						var thing = SimpleJSON.JSON.Parse(strRequest);
						status = thing["state"].AsInt;
						if(objPopup != null) {objPopup.SetActive(false); objPopup = null;
							failedObject.SetActive(false); failedObject = null;}
						if(status == -105 || status == -103){
							AccountManager.instance.ErrorPopUp();
							return;
						}
						
						callback(objNode);
						mRequestRange += 1;
					});
				isNetworking = false;
				while(!bConnect){
					yield return null;
				}
			}else{
				isNetworking = false;
				callback(objNode);
				yield return null;
			}
		}else if(status == -103){
			isNetworking = false;
			AccountManager.instance.ErrorPopUp();
		}else{
			isNetworking = false;
			if(objPopup != null) {
				objPopup.SetActive(false); objPopup = null;failedObject.SetActive(false); failedObject = null;
				if(UserDataManager.instance.bPopUpAddNetwork){
					if(UserDataManager.instance.bPopUpAdd)
						UserDataManager.instance.setGameExit();
				}
				UserDataManager.instance.bPopUpAddNetwork = false;
			}
			callback(objNode);
			mRequestRange += 1;
		}
	}*/



	public IEnumerator SendToReceive(UnityHTTP.Request someRe, System.Action<Request> callback){
		bool bConnect = true;
		int status = 10;
		UnityHTTP.Request objNode = null;
		GameObject objPopup  = null;
		someRe.Send((request) =>{
			if(request.exception != null){
				status = -104;
				bConnect = false;
				GV.mException+="_exception";
				return;
			}
			try{
				if(request.response == null){
					status = -104;
					bConnect = false;
					GV.mException+="_responseNull";
					return;
				}
				string strRequest = request.response.Text;
				if(string.IsNullOrEmpty(strRequest) == true){
					if(objPopup == null){
						objPopup = OnNetworkFailed();
						failedObject = objPopup;
						if(objPopup.GetComponent<NetworkEmail>() == null){
							objPopup.AddComponent<NetworkEmail>();
						}
						objPopup.GetComponent<NetworkEmail>().SetInit();
					}
					UserDataManager.instance.bPopUpAddNetwork = true;
					GV.mException+="_strRequestNull";
				}else{
					objNode = request;
					var thing = SimpleJSON.JSON.Parse(strRequest);
					//	List<string> mList =  request.response.GetHeaders();
					//	foreach(string str in mList){
					//		Debug.LogWarning("resposne : " + str);
					//	}
					//Debug.LogWarning(string.Format("respose : {0} _ {1} _ {2}", thing, GV.mAPI, mRequestRange));
					try{
						status = thing["state"].AsInt;
					//	int mTail = thing["tail"].AsInt;
					//	if(mRequestRange != mTail && mTail != 0){
					//		mRequestRange = mTail;
					//	}
					}catch(Exception e){
						status = -103;
						GV.mException+="_e_103";
					}
					bConnect = false;
				}

			}catch (Exception e){
				if(objPopup == null){
					objPopup = OnNetworkFailed();
					failedObject = objPopup;
					if(objPopup.GetComponent<NetworkEmail>() == null){
						objPopup.AddComponent<NetworkEmail>();
					}
					objPopup.GetComponent<NetworkEmail>().SetInit();
				}
				UserDataManager.instance.bPopUpAddNetwork = true;
				status = -103;
				bConnect = false;
				string str1 = "_"+e.Message;
				GV.mException +=str1;
			}
			//Debug.LogWarning(mRequestRange);
		});

		while(bConnect){
			yield return null;
		}

		if(status == -105 || status == -203 || status == -200 || status == -201 || status == -202 || status == -204){
			if(status == -105){
				//yield return StartCoroutine("GameInfoReWWW");
				yield return StartCoroutine("GameInfoReStart");
				bConnect =true;
				CheckTailValue();
				mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
				tempReq.SetHeader("CAMNUM", mKey);
				tempReq.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
				tempReq.SetHeader("TAIL",mRequestRange.ToString());
				tempReq.Send((request) =>
				 {
					bConnect =false;
					objNode = request;
					string strRequest = request.response.Text;

					if(string.IsNullOrEmpty(strRequest) == true){
						AccountManager.instance.ErrorPopUp();
						GV.mException += ("_101");
						return;
					}
					var thing = SimpleJSON.JSON.Parse(strRequest);
					//Debug.LogWarning(string.Format("ReresposeRetry : {0} _ {1}_{2}", thing, GV.mAPI,mRequestRange));
					status = thing["state"].AsInt;
					if(objPopup != null) {
						objPopup.SetActive(false); objPopup = null;
						failedObject.SetActive(false); failedObject = null;
					}
					if(status == -105 || status == -103d){
						AccountManager.instance.ErrorPopUp();
						GV.mException += ("_-105_-103");
						return;
					}
				//	mRequestRange += 1;
					callback(objNode);
				});
				while(bConnect){
					yield return null;
				}
			}else{
				callback(objNode);
				yield return null;
			}
		}else if(status == -103){
			AccountManager.instance.ErrorPopUp();
			GV.mException += ("_-103");
			Debug.LogWarning(GV.mException);
		}else if(status == -104){
			AccountManager.instance.ErrorPopUp();
			GV.mException += ("_-104");
			Debug.LogWarning(GV.mException);
		}else{
			if(objPopup != null) {
				objPopup.SetActive(false); objPopup = null;failedObject.SetActive(false); failedObject = null;
				if(UserDataManager.instance.bPopUpAddNetwork){
					if(UserDataManager.instance.bPopUpAdd)
						UserDataManager.instance.setGameExit();
				}
				UserDataManager.instance.bPopUpAddNetwork = false;
			}
		//	mRequestRange += 1;
			callback(objNode);

		}
	}


	IEnumerator GameInfoReWWW(){
		string url1 =  prefixUrl + "game/info/" + GAME_ID;
		GV.mAPI ="game/info/Re";
		WWW www = new WWW(url1);
		yield return www;
		if (www.error != null) {
			Debug.LogWarning ("[Error] " + www.error);
			AccountManager.instance.ErrorPopUp();
			GV.mException = www.error+"_"+"www";
			
		} else {
			var thing = SimpleJSON.JSON.Parse(www.text);
			int status = thing["state"].AsInt;
			Dictionary<string, string> mdic = www.responseHeaders;
			mSeed = mdic["CAMNUM"];
			string cookie = mdic["SET-COOKIE"];//_session_cookie_name=hquf3b9fklag6bg8koau8ea5q0; path=/; HttpOnly
			wwwCookieString = cookie.Replace("path=/; HttpOnly","");
			mRequestRange = Convert.ToInt32(thing["tail"]);
			mRequestRange += 1;
		}

	}

	IEnumerator GameInfoReStart(){
		bool bConnect = false;
		GV.mAPI ="game/info/Re";
		UnityHTTP.Request someRequest = new UnityHTTP.Request("Get", prefixUrl + "game/info/" + GAME_ID);
		someRequest.Send((request) =>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			if(string.IsNullOrEmpty( request.response.Text)==true){
				AccountManager.instance.ErrorPopUp();
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{ 	
				mSeed = request.response.GetHeader("CAMNUM");
			//	mRequestRange = Convert.ToInt32(thing["tail"]);
			//	mRequestRange += 1;
			//	mTailCount = mRequestRange;
				mTailCount = mRequestRange = Convert.ToInt32(thing["tail"]);
				mTailCount += 1;
				bConnect = true;
			}else{
				AccountManager.instance.ErrorPopUp();
			}


		});

		while(!bConnect){
			yield return null;
		}
	}
	// will be set to some session id after login
	private Hashtable session_ident = new Hashtable();

	public void ClearSessionCookie(){
		session_ident["Cookie"] = null;
	}
	
	public void SetSessionCookie(string s){
		session_ident["Cookie"] = s;
	}
	
	public Hashtable SessionCookie{
		get { return session_ident; }
	}
	
	public string GetSessionCookie(){
		return session_ident["Cookie"] as string;
	}
	
	public bool SessionCookieIsSet{
		get { return session_ident["Cookie"] != null; }
	}
	string errmsg = null;
	public enum ReturnCode : int { Failed=-1, Error=0, OK=1 }
	public delegate void OnNetResult(ReturnCode code, string result);
	public IEnumerator Login(OnNetResult callback, string nm, string pw)
	{
		// create params to send
		WWWForm form = new WWWForm();
		form.AddField("nm", nm);
		form.AddField("pw", pw);
		
		// let www do its thing
		WWW www = new WWW("http://path_to/login/", form);
		yield return www;
		
		// the following code can be used to see what the SET-COOKIE contains
		// Have a look at http://en.wikipedia.org/wiki/HTTP_cookie to see what Set-Cookie is all about
		// It is bascially how the server will tell you what it expects you to be doing with the cookie
		// The name you are looking for will be the first characters followed be "=", after that follows
		// the value of the cookie. There could also be other entries on the same line like 'Expires'
		// but they will all be seperated by ';'
		//if (www.responseHeaders.ContainsKey("SET-COOKIE")){
		//  Debug.Log(www.responseHeaders["SET-COOKIE"]);
		//}
		
		// handle the data from www, but first check if there where errors
		if (!string.IsNullOrEmpty(www.error) || string.IsNullOrEmpty(www.text))
		{
			errmsg = "Network communication error.";
			if (callback != null) callback(ReturnCode.Failed, errmsg);
		}
		else
		{
			errmsg = "Network communication error.";
			
			// like I mentioned in description, this code
			// expects "1player_name" on success, else "0"
			if (www.text[0] == '1')
			{
				try
				{
					// extract the public name of player
					name = www.text.Substring(1);
					ClearSessionCookie();
					
					// check if session cookie was send, if not, well, no use to continue then
					if (www.responseHeaders.ContainsKey("SET-COOKIE"))
					{
						// extract the session identifier cookie and save it
						// the cookie will be named, "auth" (this could be something else in your case)
						char[] splitter = { ';' };
						string[] v = www.responseHeaders["SET-COOKIE"].Split(splitter);
						foreach (string s in v)
						{
							if (string.IsNullOrEmpty(s)) continue;
							if (s.Substring(0, 4).ToLower().Equals("auth"))
							{   // found it
								SetSessionCookie(s);
								break;
							}
						}
					}
				}
				catch {
					// this should only possibly happen during development
					if (callback != null) callback(ReturnCode.Failed, "Network communication error.");
				}
			}
			
			// let whomever is interrested know that the login succeeded or failed
			if (callback != null)
			{
				if (www.text[0] == '1' && SessionCookieIsSet)
				{
					callback(ReturnCode.OK, "Login ok");
				}
				else if (www.text[0] == '0' || !SessionCookieIsSet)
				{
					errmsg = "Invalid login name or password.";
					// my server sometimes sends "0some_explenation", therefore this next line
					if (www.text.Length > 1) errmsg = www.text.Substring(1);
					callback(ReturnCode.Error, errmsg);
				}
				else
				{
					// this should only happen during development since there was an unexpected
					// value at [0], not 0 or 1 as expected, so probably some script error
					errmsg = "Network communication error.";
					callback(ReturnCode.Failed, errmsg);
				}
			}
		}
	}
	public IEnumerator NormalRequest(string url, OnNetResult callback, Dictionary<string,string> p)
	{
		// p: is a set of keys and values where the key is the name and value the value for the post field
		// create form to send with request
		WWWForm form = new WWWForm();
		if (p != null) {
			foreach (KeyValuePair<string, string> kv in p) form.AddField(kv.Key, kv.Value);
		}
		
		// let www do its thing, note that you send the SessionCookie along
		WWW www = new WWW(url, form.data, SessionCookie);
		yield return www;
		// ...
	}
		string GameCountry(){
		string str = null;
		string gCountry = Global.gCountryCode;
		//string gCountry =  EncryptedPlayerPrefs.GetString("CountryCode");
		switch(gCountry){
		case "KOR":
			str = gCountry;break;
		case "JPN":
			str = gCountry;break;
		case "CHN":
			str = gCountry;break;
		case "FRA":
			str = gCountry;break;
		case "RUS":
			str = gCountry;break;
		case "USA":
			str = gCountry;break;
		case "GBR":
			str = gCountry;break;
		case "DEU":
			str = gCountry;break;
		case "ITA":
			str = gCountry;break;
		case "PRT":
			str = gCountry;break;
		case "ESP":
			str = gCountry;break;
		case "IDN":
			str = gCountry;break;
		case "MYS":
			str = gCountry;break;
		default: {
			str = "USA";break;
			}

		}
		return str;
	}
	public void GameInfo()	{
		GV.mAPI ="game/info/";
		UnityHTTP.Request someRequest = new UnityHTTP.Request("Get", prefixUrl + "game/info/" + GAME_ID);
		StartCoroutine(SendToReceive(someRequest, (request)=>{
		//	List<string> mList =  request.response.GetHeaders();
		//	foreach(string str in mList){
		//		Debug.LogWarning("resposne : " + str);
		//	}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			Utility.ResponseLog(request.response.Text, GV.mAPI,1);
			int status = thing["state"].AsInt;
			if (status == 0)
			{ 	
			
				GV.gInfo = new appInfo();
				mSeed = request.response.GetHeader("CAMNUM");
				//Debug.LogWarning("mseed " + mSeed);
				mRequestRange = Convert.ToInt32(thing["tail"]);
				mTailCount = mRequestRange;

				GV.gInfo.movieAdState= thing["info"]["movieAdv"].AsInt;
				string strCountry = GameCountry();
				if(strCountry.Equals("MYS") == true){ 
					GV.gInfo.movieAdState = 1;
					strCountry = "USA";
				}else if(strCountry.Equals("IDN") == true){
					GV.gInfo.movieAdState = 1;
				}


				GV.gInfo.bundleVer_1= thing["info"]["bundle1"]["ver"];
			//	GV.gInfo.bundleVer_1 = "1.0.1";  // 1st ->  안드 2nd -> ios 3rd  -> appIcon
				string[] strArray = GV.gInfo.bundleVer_1.Split('.');
				GV.gInfo.crossADVersion = int.Parse(strArray[0]);
				GV.gInfo.crossADId = int.Parse(strArray[2]);
				//GV.gInfo.crossADVersion = int.Parse(strArray[1]);
				GV.gInfo.bundleURL_1= thing["info"]["bundle1"]["url"];
				GV.gInfo.bundleVer_2= thing["info"]["bundle2"]["ver"]; //upgrade_ver_ios
				GV.gInfo.bundleURL_2= thing["info"]["bundle2"]["url"]; // extra_url
			
				GV.gInfo.Notics1State= thing["info"]["notice1"]["state"].AsInt;
				GV.gInfo.Notics1URL= thing["info"]["notice1"]["url"];
				if(GV.gInfo.Notics1URL.Contains(".jpg") == false){
					GV.gInfo.Notics1URL = GV.gInfo.Notics1URL+strCountry+".jpg";
				}
				
				GV.gInfo.Notics2State= thing["info"]["notice2"]["state"].AsInt;
				GV.gInfo.Notics2URL= thing["info"]["notice2"]["url"];
				if(GV.gInfo.Notics2URL.Contains(".jpg") == false){
					GV.gInfo.Notics2URL = GV.gInfo.Notics2URL+strCountry+".jpg";
				}
				GV.gInfo.Notics3State= thing["info"]["notice3"]["state"].AsInt;
				GV.gInfo.Notics3URL= thing["info"]["notice3"]["url"];
				if(GV.gInfo.Notics3URL.Contains(".jpg") == false){
					GV.gInfo.Notics3URL = GV.gInfo.Notics3URL+strCountry+".jpg";
				}
				GV.gInfo.plusEventState= thing["info"]["plusEvent"]["state"].AsInt;
				GV.gPlusEvent = GV.gInfo.plusEventState;
				if(GV.gInfo.plusEventState != 0)
					Global.gSale = Base64Manager.instance.GlobalEncoding(13);
				else Global.gSale = Base64Manager.instance.GlobalEncoding(0);
				GV.gInfo.plusEventRatio = (float)GV.gInfo.plusEventState * 0.1f;
				GV.gInfo.plusEventURL= thing["info"]["plusEvent"]["url"];
				
				if(GV.gInfo.plusEventURL.Contains(".jpg") == false){
					GV.gInfo.plusEventURL = GV.gInfo.plusEventURL+GameCountry()+".jpg";
				}
				
				GV.gInfo.CouponState= thing["info"]["coupon"]["state"].AsInt;
				GV.gInfo.CouponURL= thing["info"]["coupon"]["url"];
				
		
				GV.gInfo.eNoticsState= thing["info"]["emergencyNotice"]["state"].AsInt;
				GV.gInfo.eNoticsType= thing["info"]["emergencyNotice"]["type"].AsInt;
				
				GV.gInfo.androidMarketURL= thing["info"]["androidUrl"];
				GV.gInfo.IosURL= thing["info"]["iosUrl"];
				GV.gInfo.HomeURL= thing["info"]["homePage"]["url1"];
				//fb://profile/gabangmanstudio

				if(Application.isEditor){
				
				}else if(Application.platform == RuntimePlatform.Android){
			
					Invoke("FBURL",1.0f);

				}else if(Application.platform == RuntimePlatform.IPhonePlayer){

				}
				GV.gInfo.strEmail= thing["info"]["email"];

				GV.gInfo.rewardState= thing["info"]["rewardAdv"].AsInt;
				GV.gInfo.extra01 = thing["info"]["loadingGameFriend"].AsInt; //loading_gamefriend
				GV.gInfo.extra02 = thing["info"]["openClick"].AsInt; //openclick
				GV.gInfo.extra03 = thing["info"]["chartBoost"].AsInt; //chartboost
				GV.gInfo.extra04 = thing["info"]["extra04"].AsInt; //extra01
				GV.gInfo.extra05 = thing["info"]["extra05"].AsInt; //extra02
			}
			else
			{
				Utility.LogError("GameInfo error");

				/*#if UNITY_EDITOR
            Application.OpenURL( "www.facebook.com/ziontskinnyred" );          
#elif UNITY_ANDROID
            Application.OpenURL( "www.facebook.com/ziontskinnyred" );
            //if (isFBAppInstalled)
            //{
            //   Utility.Log( "KJDL- 페이스북 앱이 있다 오예 ^^" );
            //    Application.OpenURL( "fb://profile/273109792775601" );
            //}
            //else
            //{ 
            //    Utility.Log( "KJDL- 페이스북 앱아 어디갔니... ㅠㅠ" );
            //    Application.OpenURL("www.facebook.com/ziontskinnyred" );
            //}
#elif UNITY_IOS
            float startTime = Time.timeSinceLevelLoad;
            Application.OpenURL( "fb://profile/273109792775601" );

            if (Time.timeSinceLevelLoad - startTime <= 1f)
            {
                Application.OpenURL( "www.facebook.com/ziontskinnyred" );
            }
#endif */


			}
			if(Application.isEditor){
				GV.gInfo.clientVer = thing["info"]["clientVerAndroid"];
				GV.gInfo.upgradeVer = thing["info"]["upgradeVerAndroid"]; //upgrade_ver_editor
				Global.gVersion ="2.3.0";
			}
			if(Application.platform == RuntimePlatform.Android){
				GV.gInfo.clientVer = thing["info"]["clientVerAndroid"];
				GV.gInfo.upgradeVer = thing["info"]["upgradeVerAndroid"]; //upgrade_ver_android
				Utility.LogWarning("ClientVersion " + Global.gVersion);
			}else if(Application.platform == RuntimePlatform.IPhonePlayer){
				GV.gInfo.clientVer = thing["info"]["clientVerIos"];
				GV.gInfo.upgradeVer = thing["info"]["upgradeVerIos"]; //upgrade_ver_ios
				Utility.LogWarning("ClientVersion " + Global.gVersion);
			}

			int uVer = int.Parse(GV.gInfo.upgradeVer);
			UserDataManager.instance.version = uVer;
			if(uVer== 1){
				Global.isNetwork = false;
			}else if(uVer== 4){
				AccountManager.instance.ErrorCallback(); // 서버와의 접속이 끊겼습니다. 
			}else if(uVer== 3){
				GameObject.Find("LoadScene").SendMessage("builtInPopup"); //서버 점검 중
			}else if(uVer== 2){
				GameObject.Find("LoadScene").SendMessage("emergencyNotic"); //긴급 공지 
			}else{

			}
		}));
	}

	void FBURL(){
		if(Vibration.IsFBAppInstalled()){
			GV.gInfo.HomeURL = "fb://page/305311279642363";
		}else{
			//GV.gInfo.HomeURL = "fb://page/gabangmanstudio";
		}
	}


	/** HTTP GET 방식 통신 처리 */
	public void GetGameInfo() {
		string url1 =  prefixUrl + "game/info/" + GAME_ID;
	//	UnityHTTP.Request someRequest = new UnityHTTP.Request("Get", prefixUrl + "game/info/" + GAME_ID);
	//	Dictionary<string,string> mDic =  someRequest.wwwSend();
	//	Debug.LogWarning(mDic.Count);
	//	WWWForm form = new WWWForm();
	//	Hashtable header = form.headers;//where I am getting my error*
	//	foreach(KeyValuePair<string, string> pair in mDic){
	//		header.Add(pair.Key, pair.Value); //header I need send over
	//		Debug.LogWarning(string.Format("{0} : {1}", pair.Key, pair.Value));
	//	}
	//	form.AddField("id","0");
	//	byte[] rawData = form.data;
		//WWW www = new WWW(url1, rawData, header);
		WWW www = new WWW(url1);
		StartCoroutine(WaitForRequest(www));
	}


	public void GetLoginGameInfo() {
		string url1 =  prefixUrl + "user/login";
	
	//	WWWForm form = new WWWForm();



		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		WWWForm form = new WWWForm();
	
		form.AddField("id", "512");
		form.AddField("deviceId", "admin216");
		form.AddField("version",Global.gVersion);
		form.AddField("platform","1");
		GV.mAPI ="user/login";
		UnityHTTP.Request someRequest = new UnityHTTP.Request("Post", prefixUrl + "user/login", form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		Dictionary<string,string> mDic =  someRequest.GetHeadersDic();
		Hashtable header = form.headers;//where I am getting my error*
		foreach(KeyValuePair<string, string> pair in mDic){
			header.Add(pair.Key, pair.Value); //header I need send over
		}
		byte[] rawData = form.data;
		WWW www = new WWW(url1, rawData, header);
		//WWW www = new WWW(url1);
		StartCoroutine(WaitForRequest2(www));
	}
	
	/** HTTP POST 방식 통신 처리 */
	public void post(int id, string url, IDictionary<string, string> data) {
		WWWForm form = new WWWForm();

		foreach (KeyValuePair<string, string> post_arg in data) {
			form.AddField(post_arg.Key, post_arg.Value);
		}
		
		WWW www = new WWW(url, form);
		//StartCoroutine(WaitForRequest(id, www));
	}

	private IEnumerator  WaitForRequest2(WWW www) {
		// 응답이 올떄까지 기다림
		yield return www;
		if (www.error != null) {
			
			Debug.LogWarning ("[Error] " + www.error);
			
			
		} else {
			Debug.LogWarning(www.text);
		}
	}


	/** 통신 처리를 위한 코루틴 */
	public string wwwCookieString;
	private IEnumerator  WaitForRequest(WWW www) {
		// 응답이 올떄까지 기다림
		yield return www;
		if (www.error != null) {
			Debug.LogWarning ("[Error] " + www.error);
			AccountManager.instance.ErrorPopUp();
			GV.mException = www.error+"_"+"www";
		
		} else {
			var thing = SimpleJSON.JSON.Parse(www.text);

			int status = thing["state"].AsInt;
			Dictionary<string, string> mdic = www.responseHeaders;
			mSeed = mdic["CAMNUM"];
		//	foreach(KeyValuePair<string,string> pair in mdic){
		//		string str = pair.Key;
		//		string str1 = pair.Value;
		//		Debug.LogWarning(str +"_"+str1);
		//	}
			string cookie = mdic["SET-COOKIE"];//_session_cookie_name=hquf3b9fklag6bg8koau8ea5q0; path=/; HttpOnly
			wwwCookieString = cookie.Replace("path=/; HttpOnly","");
			//Debug.LogWarning(cookie);
			mRequestRange = Convert.ToInt32(thing["tail"]);
			mTailCount = mRequestRange;
			mRequestRange += 1;
			mTailCount += 1;
			if (status == 0)
			{ 	
				
				GV.gInfo = new appInfo();

				GV.gInfo.movieAdState= thing["info"]["movieAdv"].AsInt;
				string strCountry = GameCountry();
				if(strCountry.Equals("MYS") == true){ 
					GV.gInfo.movieAdState = 1;
					strCountry = "USA";
				}else if(strCountry.Equals("IDN") == true){
					GV.gInfo.movieAdState = 1;
				}


				GV.gInfo.bundleVer_1= thing["info"]["bundle1"]["ver"];
				//	GV.gInfo.bundleVer_1 = "1.0.1";  // 1st ->  안드 2nd -> ios 3rd  -> appIcon
				string[] strArray = GV.gInfo.bundleVer_1.Split('.');
				GV.gInfo.crossADVersion = int.Parse(strArray[0]);
				GV.gInfo.crossADId = int.Parse(strArray[2]);
				//GV.gInfo.crossADVersion = int.Parse(strArray[1]);
				GV.gInfo.bundleURL_1= thing["info"]["bundle1"]["url"];
				GV.gInfo.bundleVer_2= thing["info"]["bundle2"]["ver"]; //upgrade_ver_ios
				GV.gInfo.bundleURL_2= thing["info"]["bundle2"]["url"]; // extra_url
				
				GV.gInfo.Notics1State= thing["info"]["notice1"]["state"].AsInt;
				GV.gInfo.Notics1URL= thing["info"]["notice1"]["url"];
				if(GV.gInfo.Notics1URL.Contains(".jpg") == false){
					GV.gInfo.Notics1URL = GV.gInfo.Notics1URL+strCountry+".jpg";
				}
				
				GV.gInfo.Notics2State= thing["info"]["notice2"]["state"].AsInt;
				GV.gInfo.Notics2URL= thing["info"]["notice2"]["url"];
				if(GV.gInfo.Notics2URL.Contains(".jpg") == false){
					GV.gInfo.Notics2URL = GV.gInfo.Notics2URL+strCountry+".jpg";
				}
				GV.gInfo.Notics3State= thing["info"]["notice3"]["state"].AsInt;
				GV.gInfo.Notics3URL= thing["info"]["notice3"]["url"];
				if(GV.gInfo.Notics3URL.Contains(".jpg") == false){
					GV.gInfo.Notics3URL = GV.gInfo.Notics3URL+strCountry+".jpg";
				}
				GV.gInfo.plusEventState= thing["info"]["plusEvent"]["state"].AsInt;
				GV.gPlusEvent = GV.gInfo.plusEventState;
				if(GV.gInfo.plusEventState != 0)
					Global.gSale = Base64Manager.instance.GlobalEncoding(13);
				else Global.gSale = Base64Manager.instance.GlobalEncoding(0);
				GV.gInfo.plusEventRatio = (float)GV.gInfo.plusEventState * 0.1f;
				GV.gInfo.plusEventURL= thing["info"]["plusEvent"]["url"];
				
				if(GV.gInfo.plusEventURL.Contains(".jpg") == false){
					GV.gInfo.plusEventURL = GV.gInfo.plusEventURL+GameCountry()+".jpg";
				}
				
				GV.gInfo.CouponState= thing["info"]["coupon"]["state"].AsInt;
				GV.gInfo.CouponURL= thing["info"]["coupon"]["url"];
				
				
				GV.gInfo.eNoticsState= thing["info"]["emergencyNotice"]["state"].AsInt;
				GV.gInfo.eNoticsType= thing["info"]["emergencyNotice"]["type"].AsInt;
				
				GV.gInfo.androidMarketURL= thing["info"]["androidUrl"];
				GV.gInfo.IosURL= thing["info"]["iosUrl"];
				GV.gInfo.HomeURL= thing["info"]["homePage"]["url1"];
				//fb://profile/gabangmanstudio
				
				if(Application.isEditor){
					
				}else if(Application.platform == RuntimePlatform.Android){
					
					Invoke("FBURL",1.0f);
					
				}else if(Application.platform == RuntimePlatform.IPhonePlayer){
					
				}
				GV.gInfo.strEmail= thing["info"]["email"];
				//GV.gInfo.movieAdState= thing["info"]["movieAdv"].AsInt;
				GV.gInfo.rewardState= thing["info"]["rewardAdv"].AsInt;
				GV.gInfo.extra01 = thing["info"]["loadingGameFriend"].AsInt; //loading_gamefriend
				GV.gInfo.extra02 = thing["info"]["openClick"].AsInt; //openclick
				GV.gInfo.extra03 = thing["info"]["chartBoost"].AsInt; //chartboost
				GV.gInfo.extra04 = thing["info"]["extra04"].AsInt; //extra01
				GV.gInfo.extra05 = thing["info"]["extra05"].AsInt; //extra02
			}
			else
			{
				Utility.LogError("GameInfo error");
			}
			if(Application.isEditor){
				GV.gInfo.clientVer = thing["info"]["clientVerAndroid"];
				GV.gInfo.upgradeVer = thing["info"]["upgradeVerAndroid"]; //upgrade_ver_editor
				Global.gVersion ="2.3.0";
			}
			if(Application.platform == RuntimePlatform.Android){
				GV.gInfo.clientVer = thing["info"]["clientVerAndroid"];
				GV.gInfo.upgradeVer = thing["info"]["upgradeVerAndroid"]; //upgrade_ver_android
				Utility.LogWarning("ClientVersion " + Global.gVersion);
			}else if(Application.platform == RuntimePlatform.IPhonePlayer){
				GV.gInfo.clientVer = thing["info"]["clientVerIos"];
				GV.gInfo.upgradeVer = thing["info"]["upgradeVerIos"]; //upgrade_ver_ios
				Utility.LogWarning("ClientVersion " + Global.gVersion);
			}
			
			int uVer = int.Parse(GV.gInfo.upgradeVer);
			UserDataManager.instance.version = uVer;
			if(uVer== 1){
				Global.isNetwork = false;
			}else if(uVer== 4){
				AccountManager.instance.ErrorCallback(); // 서버와의 접속이 끊겼습니다. 
			}else if(uVer== 3){
				GameObject.Find("LoadScene").SendMessage("builtInPopup"); //서버 점검 중
			}else if(uVer== 2){
				GameObject.Find("LoadScene").SendMessage("emergencyNotic"); //긴급 공지 
			}else{
				
			}

		www.Dispose();
	}
	}
	



/*string url = "http://itpaper.co.kr/demo/unity/items/item.json";
 
    // Use this for initialization
    void Start () {
        WWWHelper helper = WWWHelper.Instance;
        helper.OnHttpRequest += OnHttpRequest;
        helper.get (100, url);
    }
     
    void OnHttpRequest(int id, WWW www) {
        if (www.error != null) {
            Utility.Log ("[Error] " + www.error);
        } else {
            Utility.Log (www.text);
        }
    }
    */

	IEnumerator PostJson(string jsondata){
		var postScoreURL = "https://sandbox.itunes.apple.com/verifyReceipt";
		// 21007 status
		//	HTTP.Request someRequest = new HTTP.Request("Get", prefixUrl + "game/info/" + GAME_ID);
		var encoding = new System.Text.UTF8Encoding();
		var postHeader = new Hashtable();
		postScoreURL = prefixUrl + "game/info/" + GAME_ID;
	//	postHeader.Add("Content-Type", "text/json");
	//	postHeader.Add("Content-Length", jsondata.Length);
		
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
		//	Utility.Log("request success");
		//	Utility.Log("returned data" + request1.data); 
		//	var thing = SimpleJSON.JSON.Parse(request1.data);
		//	Utility.LogWarning(thing);
			//		{"receipt":{"original_purchase_date_pst":"2016-08-16 05:39:19 America/Los_Angeles", "purchase_date_ms":"1471351159769", "unique_identifier":"98f2693032f94ec4de4b006bd5765d4b0439395e", "original_transaction_id":"1000000229897092", "bvrs":"1.1.8", "transaction_id":"1000000229897092", "quantity":"1", "unique_vendor_identifier":"BA9DBF29-F555-46BE-9ACA-73DA9D443588", "item_id":"1127735279", "product_id":"pitstopracing.c8502", "purchase_date":"2016-08-16 12:39:19 Etc/GMT", "original_purchase_date":"2016-08-16 12:39:19 Etc/GMT", "purchase_date_pst":"2016-08-16 05:39:19 America/Los_Angeles", "bid":"com.gabangmanstudio.pitstopracing", "original_purchase_date_ms":"1471351159769"}, "status":"0"}
		//	Utility.LogWarning(thing["status"].AsInt); 
		//	Utility.LogWarning(thing["receipt"]["unique_vendor_identifier"].Value); //BA9DBF29-F555-46BE-9ACA-73DA9D443588
		//	Utility.LogWarning(thing["receipt"]["product_id"]); //"pitstopracing.c8502"
			
			
		}
	}

	private UnityHTTP.Request tempReq;
	public void Register(System.Action<int> callback)
	{
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		WWWForm form = new WWWForm();
		string mNick =EncryptedPlayerPrefs.GetString("mNick");
		if(Application.isEditor){

			form.AddField("deviceId", Global.gDeivceID);
			if(string.IsNullOrEmpty(mNick)){
				mNick = "...";
			}
			form.AddField("nickName", mNick );
			form.AddField("platform", "1");//android : 1, ios: 2
			form.AddField("gcmId", "");
			form.AddField("locale", "KOR");
		}else if(Application.platform == RuntimePlatform.Android){
			if(string.IsNullOrEmpty(Global.gDeivceID)){
				
			}
			form.AddField("deviceId", Global.gDeivceID);
			
			if(string.IsNullOrEmpty(mNick)){
				mNick = "...";
			}
			form.AddField("nickName", mNick );
			form.AddField("platform", "1");//android : 1, ios: 2
			if(string.IsNullOrEmpty(Global.gPushID)){
				Global.gPushID = "";			
			}
			form.AddField("gcmId", Global.gPushID);
			if(string.IsNullOrEmpty(Global.gCountryCode)){
				Global.gCountryCode = "USA";
			}
			form.AddField("locale", Global.gCountryCode);
			
		}else if(Application.platform == RuntimePlatform.IPhonePlayer){
			form.AddField("deviceId", "adminIOS_01");
			form.AddField("nickName", mNick);
			form.AddField("platform", "2");//android : 1, ios: 2
			form.AddField("gcmId", "");
			form.AddField("locale", "KR");
		}
		GV.mAPI ="user/register";
		UnityHTTP.Request someRequest = new UnityHTTP.Request("Post", prefixUrl + "user/register", form);
		tempReq = new UnityHTTP.Request("Post", prefixUrl + "user/register", form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			mAuthKey = request.response.GetHeader("Authorization");
			int status = thing["state"].AsInt;
			string mRecvId  = null;
			if (status == 0)
			{
				mRecvId = thing["result"]["id"];
				GV.UserRevId = mRecvId;
			}else if(status == -102){
				mRecvId = thing["result"]["id"];
				GV.UserRevId = mRecvId;
			}
			EncryptedPlayerPrefs.SetString("mUserId",GV.UserRevId );
			callback(status);
			
		}));
	}


	public void UnRegister(string id)
	{
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		GV.mAPI ="user/unregister";
		Vibration.OnUnRegister();
		WWWForm form = new WWWForm();
		form.AddField("id", id);
		UnityHTTP.Request someRequest = new UnityHTTP.Request("Post", prefixUrl + "user/unregister", form);
		tempReq = new UnityHTTP.Request("Post", prefixUrl + "user/unregister", form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			mAuthKey = request.response.GetHeader("Authorization");
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				
				EncryptedPlayerPrefs.DeleteAll();
				SNSManager.instance.DeleteGameAPP();
			}
		}));

	
	}
	

	public string pushToken = null;
	public void Login(string id, string sDeviceId)
	{
		CheckTailValue();
		mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
		WWWForm form = new WWWForm();
		form.AddField("id", id);
		form.AddField("deviceId", sDeviceId);
		form.AddField("version",Global.gVersion);
		form.AddField("platform","1");
		GV.mAPI ="user/login";
		UnityHTTP.Request someRequest = new UnityHTTP.Request("Post", prefixUrl + "user/login", form);
		tempReq = new UnityHTTP.Request("Post", prefixUrl + "user/login", form);
		someRequest.SetHeader("CAMNUM", mKey);
		someRequest.SetHeader("TAIL",mRequestRange.ToString());
		StartCoroutine(SendToReceive(someRequest,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			mAuthKey = request.response.GetHeader("Authorization");
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0){
				SetLocalTime(thing["time"].AsDouble);
				GV.mUser = new UserInfo();
				Global.level  = thing["result"]["user"]["level"].AsInt;
				Global.glevel  = thing["result"]["user"]["level"].AsInt;
				Global.Exp = thing["result"]["user"]["exp"].AsInt;
				if(Global.level == 0) Global.level = 1;
				Global.addLevel = Global.level;
				GV.myCoin = thing["result"]["user"]["coin"].AsInt;
				GV.myDollar= thing["result"]["user"]["dollar"].AsInt;
				GV.ChSeasonID = thing["result"]["user"]["champId"].AsInt;
				GV.mUser.FuelCount = thing["result"]["user"]["fuelCount"].AsInt;
				GV.mUser.FuelMax = thing["result"]["user"]["fuelMax"].AsInt;
				GV.SelectedTeamID=  thing["result"]["team"]["id"].AsInt;
				if(GV.SelectedTeamID < 10 || GV.SelectedTeamID > 20) GV.SelectedTeamID = 10;
				GV.SelectedSponTeamID = GV.SelectedTeamID;
				GV.vipExp =  thing["result"]["user"]["vipExp"].AsInt;
				Global.isNetwork = false;
				try{
					pushToken = thing["pushTokenCRC32"].Value;
				}catch(Exception e){
				
				}
			}else if(status == -109){
				GameObject.Find("ManagerGroup").SendMessage("GroupDestroy");
				Global.Loading = false;
				Global.isLobby = true;
				Global.isLoadFinish = false;
				//gameObject.SetActive(false);
				GV.gInfo = null;
				Global.gReLoad = 1;
				EncryptedPlayerPrefs.DeleteAll();
				Global.gChampTutorial = 0;
				Application.LoadLevel("Splash");
				Global.isNetwork = false;
			}else if(status == -204){
				GameObject.Find("LoadScene").SendMessage("UserDataDeletePopup"); //게임 데이터를 삭제하고 
				EncryptedPlayerPrefs.DeleteKey("mUserId");
				EncryptedPlayerPrefs.DeleteKey("mNick");
			}else{
				GameObject.Find("LoadScene").SendMessage("UserBlockPopup"); //유저 블락
			}
			if(!EncryptedPlayerPrefs.HasKey("ClubAlram")){
				EncryptedPlayerPrefs.SetInt("ClubAlram",0);
			}
		//	EncryptedPlayerPrefs.SetInt("ClubAlram",0);
		//	Vibration.OnSetClubFinishTime(5);
		}

		));
		
	}
	private System.DateTime mServerTime;
	public System.DateTime GetServerTime{ get; private set;}
	private long LocalTime;

	private void SetLocalTime(double value){
		mServerTime = GetDateTime(value);
		LocalTime = System.DateTime.UtcNow.Ticks;
	}

	public System.DateTime GetCurrentDeviceTime(){
		long mInterval = System.DateTime.UtcNow.Ticks - LocalTime;
		System.TimeSpan mSpan = new System.TimeSpan(mInterval);
		return mServerTime.AddSeconds(mSpan.TotalSeconds);
	}
	string EncodeKey(string seed, string gameId, int range)
	{
		string val = seed + gameId + range;
		return MD5Sum(val);
	}
	
	string MD5Sum(string src)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(src);
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
		string hashString = "";
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
		return hashString.PadLeft(32, '0');
	}
	

	public  System.DateTime GetDateTime(double serverTime)
	{
		return new System.DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(serverTime);
	}

	public void CheckSumPushToken(){
			if(string.IsNullOrEmpty(Global.gPushID) == true){
				if(Application.isEditor){
					Global.gPushID = "";
				}else if(Application.platform == RuntimePlatform.Android){
					Global.gPushID = "";
				}else if(Application.platform == RuntimePlatform.IPhonePlayer){
				Global.gPushID = "";	
				}
			}
			String input =Global.gPushID;
			byte[] bytes = new byte[input.Length * sizeof(char)];
			System.Buffer.BlockCopy(input.ToCharArray(), 0, bytes, 0, bytes.Length);
			ProgramCrc mCrc = new ProgramCrc();
	    	uint teCrc = 	mCrc.crc32(input);
			uint pushCrc = 0;
			try{
			//Utility.Log("00 " + pushToken);
				pushCrc = uint.Parse(pushToken);
		//	Utility.Log("01 " + pushCrc);
			}catch(Exception e){
				Global.isNetwork = false;
				return;
			}
		if(pushCrc != teCrc){
			Dictionary<string,string> mDic = new Dictionary<string, string>();
			if(Application.isEditor){
				mDic.Add("platform","1");
			}else if(Application.platform == RuntimePlatform.Android){
				mDic.Add("platform","1");
			}else if(Application.platform == RuntimePlatform.IPhonePlayer){
				mDic.Add("platform","2");
			}
			mDic.Add("pushToken",input);
			CheckTailValue();
			mKey = EncodeKey(mSeed, GAME_ID, mRequestRange);
			WWWForm form = new WWWForm();
			if(mDic.Count != 0){
				foreach(KeyValuePair<string, string> dic in mDic) 
				{ 
					form.AddField(dic.Key , dic.Value); 
				}
			}
			//Utility.LogWarning(pushCrc);
			GV.mAPI ="user/pushToken";
			UnityHTTP.Request someRequest = new UnityHTTP.Request("Put", prefixUrl + "user/pushToken", form);
			tempReq =  new UnityHTTP.Request("Put", prefixUrl + "user/pushToken", form);
			someRequest.SetHeader("CAMNUM", mKey);
			someRequest.SetHeader("Authorization", EncodeKey(mAuthKey, "", mRequestRange));
			someRequest.SetHeader("TAIL",mRequestRange.ToString());
			StartCoroutine(SendToReceive(someRequest,(request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				Global.isNetwork  = false;
			}));
		} else Global.isNetwork  = false;

	}
}

public class ProgramCrc
{
	public void Main()
	{
		// first convert string to byte-array
	//	String input = "test";
	//	byte[] bytes = new byte[input.Length * sizeof(char)];
	//	System.Buffer.BlockCopy(input.ToCharArray(), 0, bytes, 0, bytes.Length);
		
		// then calculate the value
	//	var crcVal = crc32(input);
		
	//	Console.WriteLine((int)crcVal);
		
	}
	
	public uint crc32(string input) {
		var table = new uint[]{
			0x00000000, 0x77073096, 0xEE0E612C, 0x990951BA, 0x076DC419, 0x706AF48F,
			0xE963A535, 0x9E6495A3, 0x0EDB8832, 0x79DCB8A4, 0xE0D5E91E, 0x97D2D988,
			0x09B64C2B, 0x7EB17CBD, 0xE7B82D07, 0x90BF1D91, 0x1DB71064, 0x6AB020F2,
			0xF3B97148, 0x84BE41DE, 0x1ADAD47D, 0x6DDDE4EB, 0xF4D4B551, 0x83D385C7,
			0x136C9856, 0x646BA8C0, 0xFD62F97A, 0x8A65C9EC, 0x14015C4F, 0x63066CD9,
			0xFA0F3D63, 0x8D080DF5, 0x3B6E20C8, 0x4C69105E, 0xD56041E4, 0xA2677172,
			0x3C03E4D1, 0x4B04D447, 0xD20D85FD, 0xA50AB56B, 0x35B5A8FA, 0x42B2986C,
			0xDBBBC9D6, 0xACBCF940, 0x32D86CE3, 0x45DF5C75, 0xDCD60DCF, 0xABD13D59,
			0x26D930AC, 0x51DE003A, 0xC8D75180, 0xBFD06116, 0x21B4F4B5, 0x56B3C423,
			0xCFBA9599, 0xB8BDA50F, 0x2802B89E, 0x5F058808, 0xC60CD9B2, 0xB10BE924,
			0x2F6F7C87, 0x58684C11, 0xC1611DAB, 0xB6662D3D, 0x76DC4190, 0x01DB7106,
			0x98D220BC, 0xEFD5102A, 0x71B18589, 0x06B6B51F, 0x9FBFE4A5, 0xE8B8D433,
			0x7807C9A2, 0x0F00F934, 0x9609A88E, 0xE10E9818, 0x7F6A0DBB, 0x086D3D2D,
			0x91646C97, 0xE6635C01, 0x6B6B51F4, 0x1C6C6162, 0x856530D8, 0xF262004E,
			0x6C0695ED, 0x1B01A57B, 0x8208F4C1, 0xF50FC457, 0x65B0D9C6, 0x12B7E950,
			0x8BBEB8EA, 0xFCB9887C, 0x62DD1DDF, 0x15DA2D49, 0x8CD37CF3, 0xFBD44C65,
			0x4DB26158, 0x3AB551CE, 0xA3BC0074, 0xD4BB30E2, 0x4ADFA541, 0x3DD895D7,
			0xA4D1C46D, 0xD3D6F4FB, 0x4369E96A, 0x346ED9FC, 0xAD678846, 0xDA60B8D0,
			0x44042D73, 0x33031DE5, 0xAA0A4C5F, 0xDD0D7CC9, 0x5005713C, 0x270241AA,
			0xBE0B1010, 0xC90C2086, 0x5768B525, 0x206F85B3, 0xB966D409, 0xCE61E49F,
			0x5EDEF90E, 0x29D9C998, 0xB0D09822, 0xC7D7A8B4, 0x59B33D17, 0x2EB40D81,
			0xB7BD5C3B, 0xC0BA6CAD, 0xEDB88320, 0x9ABFB3B6, 0x03B6E20C, 0x74B1D29A,
			0xEAD54739, 0x9DD277AF, 0x04DB2615, 0x73DC1683, 0xE3630B12, 0x94643B84,
			0x0D6D6A3E, 0x7A6A5AA8, 0xE40ECF0B, 0x9309FF9D, 0x0A00AE27, 0x7D079EB1,
			0xF00F9344, 0x8708A3D2, 0x1E01F268, 0x6906C2FE, 0xF762575D, 0x806567CB,
			0x196C3671, 0x6E6B06E7, 0xFED41B76, 0x89D32BE0, 0x10DA7A5A, 0x67DD4ACC,
			0xF9B9DF6F, 0x8EBEEFF9, 0x17B7BE43, 0x60B08ED5, 0xD6D6A3E8, 0xA1D1937E,
			0x38D8C2C4, 0x4FDFF252, 0xD1BB67F1, 0xA6BC5767, 0x3FB506DD, 0x48B2364B,
			0xD80D2BDA, 0xAF0A1B4C, 0x36034AF6, 0x41047A60, 0xDF60EFC3, 0xA867DF55,
			0x316E8EEF, 0x4669BE79, 0xCB61B38C, 0xBC66831A, 0x256FD2A0, 0x5268E236,
			0xCC0C7795, 0xBB0B4703, 0x220216B9, 0x5505262F, 0xC5BA3BBE, 0xB2BD0B28,
			0x2BB45A92, 0x5CB36A04, 0xC2D7FFA7, 0xB5D0CF31, 0x2CD99E8B, 0x5BDEAE1D,
			0x9B64C2B0, 0xEC63F226, 0x756AA39C, 0x026D930A, 0x9C0906A9, 0xEB0E363F,
			0x72076785, 0x05005713, 0x95BF4A82, 0xE2B87A14, 0x7BB12BAE, 0x0CB61B38,
			0x92D28E9B, 0xE5D5BE0D, 0x7CDCEFB7, 0x0BDBDF21, 0x86D3D2D4, 0xF1D4E242,
			0x68DDB3F8, 0x1FDA836E, 0x81BE16CD, 0xF6B9265B, 0x6FB077E1, 0x18B74777,
			0x88085AE6, 0xFF0F6A70, 0x66063BCA, 0x11010B5C, 0x8F659EFF, 0xF862AE69,
			0x616BFFD3, 0x166CCF45, 0xA00AE278, 0xD70DD2EE, 0x4E048354, 0x3903B3C2,
			0xA7672661, 0xD06016F7, 0x4969474D, 0x3E6E77DB, 0xAED16A4A, 0xD9D65ADC,
			0x40DF0B66, 0x37D83BF0, 0xA9BCAE53, 0xDEBB9EC5, 0x47B2CF7F, 0x30B5FFE9,
			0xBDBDF21C, 0xCABAC28A, 0x53B39330, 0x24B4A3A6, 0xBAD03605, 0xCDD70693,
			0x54DE5729, 0x23D967BF, 0xB3667A2E, 0xC4614AB8, 0x5D681B02, 0x2A6F2B94,
			0xB40BBE37, 0xC30C8EA1, 0x5A05DF1B, 0x2D02EF8D
		};
		
		unchecked 
		{
			uint crc = (uint)(((uint)0) ^ (-1));
			var len = input.Length;
			for (var i=0; i < len; i++) {
				crc = (crc >> 8) ^ table[
				                         (crc ^ (byte)input[i]) & 0xFF
				                         ];
			}
			crc = (uint)(crc ^ (-1));
			
			if (crc < 0) {
				crc += (uint)4294967296;
			}
			
			return crc;
		}
	}
	
}


