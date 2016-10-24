using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SendBirdModel;

public class JiverCustomUI : MonoBehaviour {
//	private List<System.Object> messages = new List<System.Object>();
//	private List<Channel> channels = new List<Channel>();
	public UISprite spWait;
//	private bool SendConnect = false;
	private GameObject lobby;
	#region implemented abstract members of JiverResponder
	/*public override void OnConnect (SendBirdModel.Channel channel)
	{
		Utility.Log ("Connect to " + channel.GetName ());
		string channelName = "#" +  channel.GetUrlWithoutAppPrefix();
		string channelUrl = channel.GetUrl ();
		bWait = false;
	}
	public override void OnError (int errorCode)
	{
		Utility.Log ("JIVER Error: " + errorCode);
	}

	private List<string> mList = new List<string>();
	System.DateTime ConvertFromUnixTimestamp(int timestamp)
	{
		System.DateTime origin = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
		return origin.AddSeconds((double)timestamp);
	}

	public override void OnSystemMessageReceived (SendBirdModel.SystemMessage message)
	{
		//	Utility.LogWarning("OnSystemMessageReceived " + message);
		messages.Add (message);
		string str = (SystemMessageRichText(message.GetMessage()) + "\n");
		if(chatIndex == 0){
		   //gTextList.Add(str);
			//Utility.LogWarning("Chat message " +str);
		}
		else if(chatIndex ==1){
			cTextList.Add(str);
			mList.Add(str);
			if(lobby == null)
				lobby = GameObject.Find("LobbyUI");
			lobby.SendMessage("onChatString", str , SendMessageOptions.DontRequireReceiver);
		}
	}

	private void TrimContent() {
		
	}
	
	public override void OnBroadcastMessageReceived (SendBirdModel.BroadcastMessage message)
	{
		Utility.LogWarning("OnBroadcastMessageReceived " + message);
		messages.Add (message);
		string str = (SystemMessageRichText(message.GetMessage()) + "\n");
		if(chatIndex == 0){
			//	gTextList.Add(str);
		}
		else if(chatIndex ==1){
			cTextList.Add(str);
			mList.Add(str);
			if(lobby == null)
				lobby = GameObject.Find("LobbyUI");
			lobby.SendMessage("onChatString", str , SendMessageOptions.DontRequireReceiver);
		}
	
	}
	
	public override void OnQueryChannelList (List<Channel> channels, bool hasNext)
	{
		this.channels = channels;
	}

	public override void OnMessageReceived (SendBirdModel.Message message)
	{	
		messages.Add (message);
		int ts = (int)(message.GetTimestamp()/1000);
		string str = "[8499c8]" +ConvertFromUnixTimestamp(ts) +"(" +message.GetSenderName()+")" + "[-]\n" +message.GetMessage();
		str = string.Format(str);
		if(chatIndex == 0){
			//gTextList.Add(str);
			//Utility.LogWarning("Chat message " +str);
		}else if(chatIndex ==1){
			mList.Add(str);
			cTextList.Add(str);
			if(chatInit == 0) return;
			if(lobby == null)
				lobby = GameObject.Find("LobbyUI");
			lobby.SendMessage("onChatString", str , SendMessageOptions.DontRequireReceiver);
		}
	}



	public override void OnMessageReceived (SendBirdModel.Message message)
	{	
		Debug.LogWarning("abc");
		int ts = (int)(message.GetTimestamp()/1000);
		string str = "[8499c8]" +ConvertFromUnixTimestamp(ts) +"(" +message.GetSenderName()+")" + "[-]\n" +message.GetMessage();
		str = string.Format(str);
		UserDataManager.instance.jiverAddMessage(str);
		cTextList.Add(str);
	}
	System.DateTime ConvertFromUnixTimestamp(int timestamp)
	{
		System.DateTime origin = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
		return origin.AddSeconds((double)timestamp);
	}

	public override void OnConnect (SendBirdModel.Channel channel)
	{
	}
	public override void OnError (int errorCode)
	{
		Utility.Log ("JIVER Error: " + errorCode);
	}



	public override void OnSystemMessageReceived (SendBirdModel.SystemMessage message)
	{
		string str = (SystemMessageRichText(message.GetMessage()) + "\n");
			cTextList.Add(str);
	}

	private void TrimContent() {
		
	}
	
	public override void OnBroadcastMessageReceived (SendBirdModel.BroadcastMessage message)
	{
		string str = (SystemMessageRichText(message.GetMessage()) + "\n");
			cTextList.Add(str);
	}
	
	public override void OnQueryChannelList (List<Channel> channels, bool hasNext)
	{
	//	this.channels = channels;
	}
	 */
	#endregion
	
	public ChatTextList gTextList, cTextList;
	public UIInput gInputString, cInputString, chString;
	private string currentChannel;
	private bool bWait;
	private int mListCount = 0;
	void Awake() {
		currentChannel = "";
	}
	
	IEnumerator WaitForConnect(){
		Global.isNetwork= true;
		bWait = true;
		spWait.fillAmount = 0.0f;
		spWait.transform.gameObject.SetActive(true);
		float delta = 0;
		while(bWait){
			delta+= 0.1f;
			if(delta >= 1.0f){
				delta = 0.0f;
			}
			spWait.fillAmount = delta;
			//	Utility.Log("delat " +  delta);
			yield return null;
		}
	//	spWait.transform.gameObject.SetActive(false);
	}
	
	IEnumerator WaitingForConnetTimeCheck(float delay){
		yield return new WaitForSeconds(delay);
		if(bWait) bWait = false;
		spWait.transform.gameObject.SetActive(false);
		Global.isNetwork = false;
	}


	IEnumerator JiverChannelJoin(){
		yield break;
		/*
		string appId = "348247C5-C147-401D-84EB-33E5F4CAD320";
		string userId = GV.UserRevId;

		//string userName = "Unity-" + userId.Substring (0, 5);
		string channelUrl = "jia_test.Unity3d";
		if(!Application.isEditor){
			channelUrl =	EncryptedPlayerPrefs.GetString("getURL");
		}
		if(CClub.mClubFlag == 2){
			SendBird.Init (appId);
			yield return new WaitForSeconds(0.5f);
			SendBird.Login (userId, GV.UserNick);
			yield return new WaitForSeconds(0.5f);
		//	gTextList.Clear();
				cTextList.Clear();
				channelUrl = CClub.mClubInfo.clubChatCh;
				if(channelUrl == "temp") channelUrl ="gabangman.PitRacingpubilcCh_20";
				chatIndex = 1;
				SendBird.Join (channelUrl);
				yield return new WaitForSeconds(0.5f);
				SendBird.Connect(50);
				chatInit = 0;
				yield return new WaitForSeconds(2.0f);
				chatInit = 1;
		}else{
			//SendBird.Join (channelUrl);
		}
		//OnChangeChatWindow(0);
		*/
	}

	private int chatInit = 0;
	void Start(){

		return;
		/*
		if(CClub.mClubFlag == 2){
			StartCoroutine("JiverChannelJoin");
		//	StartCoroutine("WaitForConnect");
		//	StartCoroutine("WaitingForConnetTimeCheck",2.0f);
			SendConnect = true;
			//Debug.LogWarning("TEST");
		}
	
		//gTextList.transform.gameObject.SendMessage("OnResetScroll", SendMessageOptions.DontRequireReceiver);
		if(Application.isEditor) {
		//	chString.text = "1"; 
			return;
		} */
	//	string[] strs = EncryptedPlayerPrefs.GetString("getURL").Split('_');
	//	int a = int.Parse(strs[1]);
	//	chString.text = a.ToString();

	}

	public void addText(string text){
		cTextList.Add(text);
	}
	void OnResetChat(){
		cTextList.Clear();
		gTextList.Clear();
		bCheck = false;
	}
	public void OnVisibleText1(){
		List<string> mList = null;
		cTextList.Clear();
		gTextList.Clear();
		localMessageCount =0;
		if(!CClub.bClanWarChat){
			//cTextList.Clear();
		//	mList = UserDataManager.instance.jiverGetMessage();
		//	messageCount = UserDataManager.instance.JiverMessageCount();
		//	localMessageCount = messageCount;
		//	for(int i =0; i< mList.Count; i++){
		//		cTextList.Add(mList[i]);
		//	}
		}else{
			//gTextList.Clear();
		//	mList = UserDataManager.instance.jiverGetMessage();
		//	messageCount = UserDataManager.instance.JiverMessageCount();
		//	localMessageCount = messageCount;
		//	for(int i =0; i< mList.Count; i++){
		//		gTextList.Add(mList[i]);
		//	}
		}
	}


	public void OnVisibleText(){
		CClub.bClanWarChat = false;
		cTextList.Clear();
		localMessageCount =0;
	//	List<string> mList = UserDataManager.instance.jiverGetMessage();
	//	messageCount = UserDataManager.instance.JiverMessageCount();
	//	localMessageCount = messageCount;
	//	for(int i =0; i< mList.Count; i++){
	//		cTextList.Add(mList[i]);
	//	}
	}

	/*
	void OnChChange(){
		//Utility.LogWarning("OnchChange");
		string str = chString.text;
		string mCh = EncryptedPlayerPrefs.GetString("getURL");
		if(string.IsNullOrEmpty(str)) {
			string[] mchs = mCh.Split('_');
			chString.text = int.Parse(mchs[1]).ToString();
			return;
		}
	
		int ch =0; int nCh = 0;
		if(int.TryParse(str, out ch)){
			if(ch < 10 && ch >0) {
				str = "gabangman.PitRacingpubilcCh_0"+ch.ToString();
			//	chString.text = ch.ToString();
			}
			else if(ch > 21){ str ="gabangman.PitRacingpubilcCh_20";
			//	chString.text = "20";
				ch = 20;
			}else {
				str = "gabangman.PitRacingpubilcCh_"+ch.ToString();
			//	chString.text = ch.ToString();
			}
		}else{
			str ="gabangman.PitRacingpubilcCh_20";
			ch = 20;
			//chString.text = "20";
		}
		if(str == mCh){
			chString.text = ch.ToString();
			return;
		}else{
			chString.text = ch.ToString();
		}
		SendBird.Leave(mCh);

		EncryptedPlayerPrefs.SetString("getURL",str);

		StartCoroutine("JiverChannelChange", str);

		gTextList.Clear();

		messages.Clear();

	}
	
*/

	/*
	void OnEnable(){
		if(!SendConnect) return;
		if(!SendBird.connected)
			SendBird.Connect(50);
	}
	
	void Disable(){
		if(!SendConnect) return;
		if(SendBird.connected)
			SendBird.Disconnect();
	}
*/
	/*
	void OnDestroy(){
		if(SendBird.connected)
			SendBird.Disconnect();
	//	gTextList.Clear();
		messages.Clear();
		cTextList.Clear();
	}*/
	
	IEnumerator JiverChannelChange(string str){
	//	StartCoroutine("WaitForConnect");
	//	StartCoroutine("WaitingForConnetTimeCheck", 1.0f);
		yield return new WaitForSeconds(0.2f);
//		SendBird.Join(str);
		yield return new WaitForSeconds(0.2f);
//		SendBird.Connect(50);
	//	Debug.LogWarning("TEST1");
	}
	void Submit() {
		//Jiver.SendMessage(inputString);
		/*	if (inputMessage.text.Length > 0) {
				Jiver.SendMessage(inputMessage.text);
				inputMessage.text = "";
				//ScrollToBottom();
			}*/
	}
	
	/*void OnGSubmit(){
		if (inputMessage.text.Length > 0) {
			Jiver.SendMessage(inputMessage.text);
			inputMessage.text = "";
			//ScrollToBottom();
		}
	}*/
	
	
	void OnCWSubmit ()
	{
		if (gTextList != null)
		{
			// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
			//string text1 =string.Format(KoStorage.GetKorString("77231"), System.DateTime.Now, GV.UserNick, gInputString.text);
			//string text1 = System.DateTime.Now.ToString()+" ("+GV.UserNick+ ")"+"\r\n"+gInputString.text;
			string text = NGUITools.StripSymbols(gInputString.text);
			
			if (!string.IsNullOrEmpty(text))
			{
				gTextList.transform.gameObject.SendMessage("OnResetScroll", SendMessageOptions.DontRequireReceiver);//GetComponent<ChatTextList>().OnResetScroll();
				UserDataManager.instance.JiverSend(text);
				gInputString.text = "";
				gInputString.selected = false;
				UserDataManager.instance.ChatEnable = 3;
			}
		}
		//mIgnoreNextEnter = true;
	}
	void OnCSubmit(){
		if (cTextList != null)
		{
			// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
		//	string text1 =string.Format(KoStorage.GetKorString("77231"), System.DateTime.Now, GV.UserNick, cInputString.text);
			string text = NGUITools.StripSymbols(cInputString.text);
		//	string text = NGUITools.StripSymbols(text1);
			if (!string.IsNullOrEmpty(text))
			{
				cTextList.transform.gameObject.SendMessage("OnResetScroll", SendMessageOptions.DontRequireReceiver);
				UserDataManager.instance.JiverSend(text);
				cInputString.text = "";
				cInputString.selected = false;
				UserDataManager.instance.ChatEnable = 3;
			}
		}
	}

	private int chatIndex=0;
	public void OnChangeChatWindow(int index){
	//	StartCoroutine("WaitForConnect");
	//	StartCoroutine("WaitingForConnetTimeCheck",1.0f);
		string str = string.Empty;
		if(index == 0){
			//global Chat
			gTextList.Clear();
			chatIndex = 0;
			str  = EncryptedPlayerPrefs.GetString("getURL");
			StartCoroutine("JiverChannelChange", str);
		}else if(index == 1){
			//Clan Chat
//			if(SendConnect) {
				//cTextList.Clear();
				str = CClub.mClubInfo.clubChatCh;
				if(str == "temp") str ="gabangman.PitRacingpubilcCh_20";
				chatIndex = 1;
			//	OnVisibleText(1);
//			}else{
//				StartCoroutine("JiverChannelJoin");
//			}

		}else {
			//Clan Msg
		}
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
	private int messageCount;
	private int localMessageCount;
	private bool bCheck = false;
	void FixedUpdate(){
		if(!bCheck) return;
		messageCount = UserDataManager.instance.JiverMessageCount();
		if(messageCount == localMessageCount) return;
		List<string> mListMgs = UserDataManager.instance.jiverGetMessage();
		for(int i = localMessageCount; i < messageCount;  i++){
			if(!CClub.bClanWarChat) cTextList.Add(mListMgs[i]);
			else gTextList.Add(mListMgs[i]);
		}
		localMessageCount = messageCount;
	}

	public void OnStartChat(){
		bCheck = true;
		//UserDataManager.instance.ChatEnable = 2;
	}

	public void OnEndChat(){
		bCheck = false;
	}
}
