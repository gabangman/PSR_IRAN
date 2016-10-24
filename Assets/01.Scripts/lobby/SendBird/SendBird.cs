// SendBird Unity SDK

using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using SendBirdModel;
using SimpleJSON;
using System;

#region SendBird Responder Class
/*
 * Any custom UI classes must extend this responder class to receive events from SendBird.
 */
public abstract class SendBirdResponder : MonoBehaviour {
	public abstract void OnConnect (Channel channel);
	
	public abstract void OnError (int errorCode);
	
	public abstract void OnMessageReceived (Message message);
	
	public abstract void OnSystemMessageReceived (SystemMessage message);
	
	public abstract void OnBroadcastMessageReceived (BroadcastMessage message);
	
	public abstract void OnQueryChannelList (List<Channel> channels, bool hasNext);
	
}
#endregion

public class SendBird : MonoBehaviour {
	private static SendBirdAdapter instance;
	private static SendBirdResponder sendbirdResponder;
	private static GameObject sendbirdGameObject;
	
	public static bool connected;
	
	private static float dummyMessageTimer = -1;
	private static bool dummyChannelListFlag1 = false;
	private static bool dummyChannelListFlag2 = false;
	
	public GameObject responderGameObject;
	
	#region SendBird public methods
	interface SendBirdAdapter {
		void Init (string appId, string responder);
		void Login (string uuid, string nickname);
		void Join (string channelUrl);
		void Leave (string channelUrl);
		void Connect (int prevMessageLimit);
		void Disconnect ();
		void QueryChannelList(int limit);
		void NextChannelList();
		void SendMessage(string message);
		void SendMessageWithData(string message, string data);
	}
	#endregion
	
	
	#region SendBird Native Callbacks 
	void _OnConnect(string arg) {
		Utility.Log ("OnConnect: " + arg);
		if (sendbirdResponder != null) {
			Channel channel = new Channel(arg);
			sendbirdResponder.OnConnect(channel);
		}
	}
	
	void _OnError(string arg) {
		Utility.Log ("OnError: " + arg);
		if (sendbirdResponder != null) {
			int errorCode = int.Parse(arg);
			sendbirdResponder.OnError(errorCode);
		}
	}
	
	void _OnMessageReceived(string arg) {
		//		Utility.Log ("OnMessageReceived: " + arg);
		if (sendbirdResponder != null) {
			Message message = new Message(arg);
			sendbirdResponder.OnMessageReceived(message);
		}
	}
	
	void _OnSystemMessageReceived(string arg) {
		Utility.Log ("OnSystemMessageReceived: " + arg);
		if (sendbirdResponder != null) {
			SystemMessage message = new SystemMessage(arg);
			sendbirdResponder.OnSystemMessageReceived(message);
		}
	}
	
	void _OnBroadcastMessageReceived(string arg) {
		Utility.Log ("OnBroadcastMessageReceived: " + arg);
		if (sendbirdResponder != null) {
			BroadcastMessage message = new BroadcastMessage(arg);
			sendbirdResponder.OnBroadcastMessageReceived(message);
		}
	}
	
	void _OnFileReceived(string arg) {
		// Not Yet Implemented.
		/*
		Utility.Log ("OnFileReceived: " + arg);
		if (sendbirdResponder != null) {
			FileLink fileLink = new FileLink(arg);
			sendbirdResponder.OnFileReceived(fileLink);
		}
		*/
	}
	
	void _OnMessagingStarted(string arg) {
		// Not Yet Implemented.
	}
	
	
	void _OnMessagingEnded(string arg) {
		// Not Yet Implemented.
	}
	
	void _OnReadReceived(string arg) {
		// Not Yet Implemented.
	}
	
	void _OnTypeStartReceived(string arg) {
		// Not Yet Implemented.
	}
	
	void _OnTypeEndReceived(string arg) {
		// Not Yet Implemented.
	}
	
	void _OnMessagesLoaded(string arg) {
		// Not Yet Implemented.
	}
	
	void _OnQueryChannelList(string arg) {
		Utility.Log ("OnQueryChannelList: " + arg);
		if (sendbirdResponder != null) {
			List<Channel> channelList = new List<Channel>();
			JSONClass obj = JSON.Parse(arg).AsObject;
			
			JSONArray jsonList = obj["channels"].AsArray;
			bool hasNext = obj["has_next"].AsBool;
			
			for(int i = 0; i < jsonList.Count; i++) {
				channelList.Add(new Channel(jsonList[i]));
			}
			
			sendbirdResponder.OnQueryChannelList(channelList, hasNext);
		}
	}

#endregion
	private static SendBirdAdapter GetInstance() {
		if (instance == null) {
			#if UNITY_EDITOR
			Utility.Log("SendBird on Editor.");
			instance = new SendBirdDummyImpl(); 
			dummyMessageTimer = 0;
			//ABCTest();
			#elif UNITY_IOS
			Utility.Log("SendBird on iOS.");
			instance = new SendBirdiOSImpl();
			#elif UNITY_ANDROID
			Utility.Log("SendBird on Android.");
			instance = new SendBirdAndroidImpl();
			#else
			Utility.Log("SendBird on unknown platform.");
			instance = new SendBirdDummyImpl();
			dummyValue = 0;
			#endif
		}
		
		return instance;
	}


	 void ABCTest(){
		for(int i = 0; i < 20 ; i ++){
			string msgJson = "MESG{\"channel_id\": \"0\", \"message\": \"Dummy Text on Editor Mode - " + Time.time + "\", \"user\": {\"image\": \"http://url\", \"name\": \"Sender\"}, \"ts\": 1418979273365, \"scrap_id\": \"\"}";
			_OnMessageReceived(msgJson);
		}
	}

	void Update() {
		if (dummyMessageTimer >= 0) {
			if(Mathf.Abs(dummyMessageTimer - Time.time) >= 0.3f) {
			//	string msgJson = "MESG{\"channel_id\": \"0\", \"message\": \"Dummy Text on Editor Mode - " + Time.time + "\", \"user\": {\"image\": \"http://url\", \"name\": \"Sender\"}, \"ts\": 1418979273365, \"scrap_id\": \"\"}";
			//	_OnMessageReceived(msgJson);
				dummyMessageTimer = Time.time;
			}
		}
		
		if (dummyChannelListFlag1) {
			dummyChannelListFlag1 = false;
			JSONClass root = new JSONClass();
			
			JSONArray channels = new JSONArray();
			JSONClass channel = new JSONClass();
			
			channel.Add ("id", new JSONData(1));
			channel.Add ("channel_url", new JSONData("app_prefix.channel_url"));
			channel.Add ("name", new JSONData("Sample"));
			channel.Add ("cover_img_url", new JSONData("http://localhost/image.jpg"));
			channel.Add ("member_count", new JSONData(999));
			channels.Add(channel.ToString());
			
			channel.Add ("id", new JSONData(2));
			channel.Add ("channel_url", new JSONData("app_prefix.Unity3d"));
			channel.Add ("name", new JSONData("Unity3d"));
			channel.Add ("cover_img_url", new JSONData("http://localhost/image.jpg"));
			channel.Add ("member_count", new JSONData(999));
			channels.Add(channel.ToString());
			
			channel.Add ("id", new JSONData(3));
			channel.Add ("channel_url", new JSONData("app_prefix.Lobby"));
			channel.Add ("name", new JSONData("Lobby"));
			channel.Add ("cover_img_url", new JSONData("http://localhost/image.jpg"));
			channel.Add ("member_count", new JSONData(999));
			channels.Add(channel.ToString());
			
			channel.Add ("id", new JSONData(4));
			channel.Add ("channel_url", new JSONData("app_prefix.Cocos2d"));
			channel.Add ("name", new JSONData("Cocos2d"));
			channel.Add ("cover_img_url", new JSONData("http://localhost/image.jpg"));
			channel.Add ("member_count", new JSONData(999));
			channels.Add(channel.ToString());
			
			channel.Add ("id", new JSONData(5));
			channel.Add ("channel_url", new JSONData("app_prefix.GameInsight"));
			channel.Add ("name", new JSONData("GameInsight"));
			channel.Add ("cover_img_url", new JSONData("http://localhost/image.jpg"));
			channel.Add ("member_count", new JSONData(999));
			channels.Add(channel.ToString());
			
			root.Add ("has_next", new JSONData(true));
			root.Add ("channels", channels);
			
			_OnQueryChannelList(root.ToString());
		}
		
		if (dummyChannelListFlag2) {
			dummyChannelListFlag2 = false;
			JSONClass root = new JSONClass();
			
			JSONArray channels = new JSONArray();
			JSONClass channel = new JSONClass();
			
			channel.Add ("id", new JSONData(6));
			channel.Add ("channel_url", new JSONData("app_prefix.iOS"));
			channel.Add ("name", new JSONData("iOS"));
			channel.Add ("cover_img_url", new JSONData("http://localhost/image.jpg"));
			channel.Add ("member_count", new JSONData(999));
			channels.Add(channel.ToString());
			
			channel.Add ("id", new JSONData(7));
			channel.Add ("channel_url", new JSONData("app_prefix.Android"));
			channel.Add ("name", new JSONData("Android"));
			channel.Add ("cover_img_url", new JSONData("http://localhost/image.jpg"));
			channel.Add ("member_count", new JSONData(999));
			channels.Add(channel.ToString());
			
			channel.Add ("id", new JSONData(8));
			channel.Add ("channel_url", new JSONData("app_prefix.News"));
			channel.Add ("name", new JSONData("News"));
			channel.Add ("cover_img_url", new JSONData("http://localhost/image.jpg"));
			channel.Add ("member_count", new JSONData(999));
			channels.Add(channel.ToString());
			
			channel.Add ("id", new JSONData(9));
			channel.Add ("channel_url", new JSONData("app_prefix.Lobby"));
			channel.Add ("name", new JSONData("Lobby"));
			channel.Add ("cover_img_url", new JSONData("http://localhost/image.jpg"));
			channel.Add ("member_count", new JSONData(999));
			channels.Add(channel.ToString());
			
			channel.Add ("id", new JSONData(10));
			channel.Add ("channel_url", new JSONData("app_prefix.iPad"));
			channel.Add ("name", new JSONData("iPad"));
			channel.Add ("cover_img_url", new JSONData("http://localhost/image.jpg"));
			channel.Add ("member_count", new JSONData(999));
			channels.Add(channel.ToString());
			
			root.Add ("has_next", new JSONData(false));
			root.Add ("channels", channels);
			
			_OnQueryChannelList(root.ToString());
		}
	}
	
	void OnEnable() {
	//	if (!connected) {
	//		Connect (0);
	//	}
		
	}
	
	void OnDisable() {
	//	if (connected) {
	//		Disconnect ();
	//	}
	}


	
	void Start() {
	}
	
	void Awake() {
		Utility.Log ("SendBird Awake");
		#if UNITY_ANDROID
		//		AndroidJNIHelper.debug = true;
		#endif
		sendbirdGameObject = gameObject;
		responderGameObject = GameObject.Find("UserDataManager");
		if (responderGameObject != null) {
			sendbirdResponder = responderGameObject.GetComponent<SendBirdResponder>();
		}
	}
	
	#region SendBird static methods
	public static void Init(string appId) {
		GetInstance().Init (appId, sendbirdGameObject.name);
	}
	
	public static void Login(string uuid, string nickname) {
		GetInstance().Login (uuid, nickname);
	}
	
	public static void Join(string channelUrl) {
		GetInstance().Join (channelUrl);
	}
	
	public static void Leave(string channelUrl) {
		GetInstance().Leave (channelUrl);
	}
	
	private static char[] CHARS_TO_TRIM = {'\n', '\r', };
	public static new void SendMessage(string message) {
		GetInstance ().SendMessage (message.TrimEnd(CHARS_TO_TRIM));
	}
	
	public static void Connect(int prevMessageLimit) {
		connected = true;
		GetInstance().Connect (prevMessageLimit);
	}
	
	public static void Disconnect() {
		connected = false;
		GetInstance().Disconnect ();
	}
	
	public static void QueryChannelList(int limit = 100) {
		GetInstance().QueryChannelList (limit);
	}
	
	public static void NextChannelList() {
		GetInstance().NextChannelList ();
	}
	#endregion
	
	
	
	
	#region SendBird Adatper Dummy Impl.
	class SendBirdDummyImpl : SendBirdAdapter {
		
		
		public void Init (string appId, string responder) {
			Utility.Log ("SendBird disabled.");
			Utility.Log ("SendBird runs on Test mode.");
		}
		public void Login (string uuid, string nickname) {
			Utility.Log ("Login: " + uuid + ", " + nickname);
			Utility.Log ("SendBird runs on Test mode.");
		}
		public void Join (string channelUrl) {
			Utility.Log ("Join: " + channelUrl);
			Utility.Log ("SendBird runs on Test mode.");
		}
		public void Leave (string channelUrl) {
			Utility.Log ("Leave: " + channelUrl);
			Utility.Log ("SendBird runs on Test mode.");
		}
		public void Connect (int prevMessageLimit) {
			Utility.Log ("Connect...");
			Utility.Log ("SendBird runs on Test mode.");
			
		}
		public void Disconnect () {
			Utility.Log ("Disconnect...");
			Utility.Log ("SendBird runs on Test mode.");
		}
		
		public void QueryChannelList (int limit)
		{
			Utility.Log ("QueryChannelList...");
			dummyChannelListFlag1 = true;
		}
		
		public void NextChannelList ()
		{
			Utility.Log ("NextChannelList...");
			dummyChannelListFlag2 = true;
		}
		
		
		public void SendMessage (string message) {
			Utility.Log ("SendMessage: " + message);
		}
		
		public void SendMessageWithData (string message, string data) {
			Utility.Log ("SendMessage: " + message + " with data: " + data);
		}
		
	}
	#endregion
	
	#if UNITY_ANDROID
	#region SendBird Adapter Android Impl.
	class SendBirdAndroidImpl : SendBirdAdapter {
		private AndroidJavaClass sendbirdClass;
		public void Init(string appId, string responder) {
			sendbirdClass = new AndroidJavaClass("com.sendbird.android.SendBird");
			sendbirdClass.CallStatic ("init", appId);
			sendbirdClass.CallStatic ("setUnityResponder", responder);
		}
		
		public void Login(string uuid, string nickname) {
			sendbirdClass.CallStatic ("login", uuid, nickname);
		}
		
		public void Join(string channelUrl) {
			sendbirdClass.CallStatic ("join", channelUrl);
		}
		
		public void Leave(string channelUrl) {
			sendbirdClass.CallStatic ("leave", channelUrl);
		}
		
		public void Connect(int prevMessageLimit) {
			sendbirdClass.CallStatic ("connectForUnity", prevMessageLimit);
		}
		
		public void Disconnect() {
			sendbirdClass.CallStatic ("disconnect");
		}
		
		public void QueryChannelList (int limit)
		{
			sendbirdClass.CallStatic ("queryChannelListForUnity", limit);
		}
		
		public void NextChannelList ()
		{
			sendbirdClass.CallStatic ("nextChannelListForUnity");
		}
		
		
		public void SendMessage (string message) {
			sendbirdClass.CallStatic ("send", message);	
		}
		
		public void SendMessageWithData (string message, string data) {
			sendbirdClass.CallStatic ("sendWithData", message, data);	
		}
	}
	#endregion
	#endif
	
	
	#if UNITY_IOS
	#region SendBird Adapter iOS Impl.
	[DllImport ("__Internal")]
	private static extern void _SendBird_iOS_Init (string appId, string responder);
	
	[DllImport ("__Internal")]
	private static extern void _SendBird_iOS_Login (string uuid, string nickname);
	
	[DllImport ("__Internal")]
	private static extern void _SendBird_iOS_Join (string channelUrl);
	
	[DllImport ("__Internal")]
	private static extern void _SendBird_iOS_Leave (string channelUrl);
	
	[DllImport ("__Internal")]
	private static extern void _SendBird_iOS_Connect (int prevMessageLimit);
	
	[DllImport ("__Internal")]
	private static extern void _SendBird_iOS_Disconnect ();
	
	[DllImport ("__Internal")]
	private static extern void _SendBird_iOS_QueryChannelListForUnity (int limit);
	
	[DllImport ("__Internal")]
	private static extern void _SendBird_iOS_NextChannelListForUnity ();
	
	[DllImport ("__Internal")]
	private static extern void _SendBird_iOS_Send (string message);
	
	[DllImport ("__Internal")]
	private static extern void _SendBird_iOS_SendWithData (string message, string data);
	
	class SendBirdiOSImpl : SendBirdAdapter {
		public void Init(string appId, string responder) {
			_SendBird_iOS_Init (appId, responder);
		}
		
		public void Login(string uuid, string nickname) {
			_SendBird_iOS_Login (uuid, nickname);
		}
		
		public void Join(string channelUrl) {
			_SendBird_iOS_Join (channelUrl);
		}
		
		public void Leave(string channelUrl) {
			_SendBird_iOS_Leave (channelUrl);
		}
		
		public void Connect(int prevMessageLimit) {
			_SendBird_iOS_Connect (prevMessageLimit);
		}
		
		public void Disconnect() {
			_SendBird_iOS_Disconnect ();
		}
		
		public void QueryChannelList (int limit) {
			_SendBird_iOS_QueryChannelListForUnity(limit);
		}
		
		public void NextChannelList () {
			_SendBird_iOS_NextChannelListForUnity();
		}
		
		public void SendMessage (string message) {
			_SendBird_iOS_Send (message);
		}
		
		public void SendMessageWithData (string message, string data) {
			_SendBird_iOS_SendWithData (message, data);
		}
	}
	#endregion
	#endif
}

namespace SendBirdModel {
	
	public class FileLink {
		private JSONNode node;
		
		public FileLink(string json) {
			this.node = JSON.Parse (json);
		}
		
		public string GetSenderName() {
			return node ["user"] ["name"];
		}
		
		public string GetUrl() {
			return node ["url"];
		}
		
		public string GetName() {
			return node ["name"];
		}
		
		public int GetSize() {
			return node ["size"].AsInt;
		}
		
		public new string GetType() {
			return node ["type"];
		}
		
		public string GetCustomField() {
			return node ["custom"];
		}
	}
	
	public class SystemMessage {
		private JSONNode node;
		
		public SystemMessage(string json) {
			this.node = JSON.Parse (json);
		}
		
		public string GetMessage() {
			return node ["message"];
		}
		
		public long GetTimestamp() {
			return (long)node ["ts"].AsFloat;
		}
	}
	
	public class BroadcastMessage {
		private JSONNode node;
		
		public BroadcastMessage(string json) {
			this.node = JSON.Parse (json);
		}
		
		public string GetMessage() {
			return node ["message"];
		}
		
		public long GetTimestamp() {
			return (long)node ["ts"].AsFloat;
		}
	}
	
	
	public class Channel {
		private JSONNode node;
		
		public Channel(string json) {
			this.node = JSON.Parse (json);
		}
		
		public long GetId() {
			return (long)node["id"].AsFloat;
		}
		
		public int GetMemberCount() {
			return node["member_count"].AsInt;
		}
		
		public string GetUrl() {
			return node["channel_url"];
		}
		
		
		public string GetUrlWithoutAppPrefix() {
			string url = GetUrl ();
			string[] tokens = url.Split ('.');
			if (tokens.Length > 1) {
				return tokens[1];
			}
			
			return url;
		}
		
		public string GetName() {
			return node["name"];
		}
		
		public string GetCoverUrl() {
			return node["cover_img_url"];
		}
	}
	
	public class Message {
		private JSONNode node;
		public Message(string json) {
			this.node = JSON.Parse (json);
		}
		
		public string GetMessage() {
			return node ["message"];
		}
		
		public string GetData() {
			return node ["data"];
		}
		
		public bool IsOpMessage() {
			return node ["is_op_msg"].AsBool;
		}
		
		public bool IsGuestMessage() {
			return node["is_guest_msg"].AsBool;
		}
		
		public string GetSenderName() {
			return node ["user"]["name"];
		}
		
		public long GetTimestamp() {
			return (long)node ["ts"].AsFloat;
		}
	}
}
