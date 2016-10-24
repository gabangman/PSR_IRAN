using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SendBirdModel;

public class JiverResponse : SendBirdResponder {
	private List<System.Object> messages = new List<System.Object>();
	private List<Channel> channels = new List<Channel>();
	private bool SendConnect = false;
	private GameObject lobby;
	#region implemented abstract members of JiverResponder
	public override void OnConnect (SendBirdModel.Channel channel)
	{
		Utility.Log ("Connect to " + channel.GetName ());
		string channelName = "#" +  channel.GetUrlWithoutAppPrefix();
		string channelUrl = channel.GetUrl ();
	}
	public override void OnError (int errorCode)
	{
		Utility.Log ("JIVER Error: " + errorCode);
	}
	public override void OnMessageReceived (SendBirdModel.Message message)
	{	
		messages.Add (message);
		int ts = (int)(message.GetTimestamp()/1000);
		string str = "[8499c8]" +ConvertFromUnixTimestamp(ts) +"(" +message.GetSenderName()+")" + "[-]\n" +message.GetMessage();
		str = string.Format(str);
		mMessageList.Add(str);
		UserDataManager.instance.jiverReceivedMessage(str);
	//	if(lobby == null)
	//		lobby = GameObject.Find("LobbyUI");
	//	lobby.SendMessage("onChatString", str , SendMessageOptions.DontRequireReceiver);
	}

	public List<string> mMessageList = new List<string>();
	System.DateTime ConvertFromUnixTimestamp(int timestamp)
	{
		System.DateTime origin = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
		return origin.AddSeconds((double)timestamp);
	}
	
	public override void OnSystemMessageReceived (SendBirdModel.SystemMessage message)
	{
		messages.Add (message);
		string str = (SystemMessageRichText(message.GetMessage()) + "\n");
		mMessageList.Add(str);
	//	if(lobby == null)
	//		lobby = GameObject.Find("LobbyUI");
	//	lobby.SendMessage("onChatString", str , SendMessageOptions.DontRequireReceiver);
	}
	
	private void TrimContent() {
		
	}
	
	public override void OnBroadcastMessageReceived (SendBirdModel.BroadcastMessage message)
	{
		Utility.LogWarning("OnBroadcastMessageReceived " + message);
		messages.Add (message);
		string str = (SystemMessageRichText(message.GetMessage()) + "\n");
			mMessageList.Add(str);
	//		if(lobby == null)
	//			lobby = GameObject.Find("LobbyUI");
	//		lobby.SendMessage("onChatString", str , SendMessageOptions.DontRequireReceiver);
	}
	
	public override void OnQueryChannelList (List<Channel> channels, bool hasNext)
	{
		this.channels = channels;
	}
	#endregion
	
	private string currentChannel;
	void Awake() {
		currentChannel = "";
	}

	public void jiverUnSetInit(){
		if(SendBird.connected)
			SendBird.Disconnect();
		messages.Clear();
		mMessageList.Clear();
	}

	public void JiverChannelJoins(){
		StartCoroutine("JiverChannelJoin");
		SendConnect = true;
	}

	IEnumerator JiverChannelJoin(){
		string appId = "348247C5-C147-401D-84EB-33E5F4CAD320";
		string userId = GV.UserRevId;
		string channelUrl = "jia_test.Unity3d";
		if(!Application.isEditor){
			channelUrl =	EncryptedPlayerPrefs.GetString("getURL");
		}
		if(CClub.mClubFlag == 2){
			SendBird.Init (appId);
		//	yield return new WaitForSeconds(0.5f);
			SendBird.Login (userId, GV.UserNick);
		//	yield return new WaitForSeconds(0.5f);
			channelUrl = CClub.mClubInfo.clubChatCh;
			if(channelUrl == "temp") channelUrl ="gabangman.PitRacingpubilcCh_20";
			SendBird.Join (channelUrl);
		//	yield return new WaitForSeconds(0.5f);
			SendBird.Connect(50);
	//		Debug.LogWarning("JiverChannelJoin : " + channelUrl);
			yield return new WaitForSeconds(0.1f);
		}else{
		
		}
	}

	public void JiverChannelChange(bool b){
		StartCoroutine("JiverChannelChanges", b);
		SendConnect = true;
	}
	
	IEnumerator JiverChannelChanges(bool b){
		mMessageList.Clear();
		messages.Clear();
		if(CClub.mClubFlag == 2){
			string channelUrl = null;
			yield return new WaitForSeconds(0.1f);
			if(!b)	channelUrl = CClub.mClubInfo.clubChatCh;
			else channelUrl = CClub.mClubInfo.clubMatchChatCh;
			if(channelUrl == "temp" || string.IsNullOrEmpty(channelUrl) == true) channelUrl ="gabangman.PitRacingpubilcCh_20";
			SendBird.Join (channelUrl);
			yield return new WaitForSeconds(0.1f);
			SendBird.Connect(50);
		//	Debug.LogWarning("JiverChannelChanges : " + channelUrl);
		}else{
			
		}
	}

	public void send(string str){
	//	Debug.LogWarning("send : " + str + " _ " + SendBird.connected);
	
		SendBird.SendMessage(str);
	}

	public int getMessageCount(){
		return  mMessageList.Count;
	}

	public List<string> getMessageList(){
		return mMessageList;
	}

	void OnDestroy(){
		if(SendBird.connected)
			SendBird.Disconnect();
		messages.Clear();
	}

	string MessageRichText(Message message)
	{
		return "<color=#" + ">" + message.GetSenderName() + ": </color>" + message.GetMessage();
	}
	
	string SystemMessageRichText(string message)
	{
		return string.Format("[8499c8]{0}[-]", message);//"<color=#" + ">" + message + "</color>";
	}
	string UserMessageRichText(string message){
		string[] na = message.Split(new string[]{"\r\n"},System.StringSplitOptions.None);
		string str = null;
		try{
			str = "[8499c8]" + na[0] + "[-]\n" + na[1];
		}
		catch(System.IndexOutOfRangeException e){
			str = message;
		}
		str = string.Format(str);
		return str;
	}
}
