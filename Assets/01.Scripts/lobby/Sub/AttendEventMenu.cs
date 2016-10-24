using UnityEngine;
using System.Collections;

public class AttendEventMenu : MonoBehaviour {
	public UILabel[] lbTexts;
	public UISprite spTime;
	public GameObject[] BTNs;
	public GameObject RewardObj;
	private GameObject adBTN;
	private System.DateTime nTime;
	private System.TimeSpan sTime;
	void Awake(){
		lbTexts[14].text = string.Empty;
		BTNs[1].GetComponent<UIButtonMessage>().functionName = "OnAcceptDouble";
	}
	void Start(){
		lbTexts[0].text = KoStorage.GetKorString("72400"); // title
		lbTexts[1].text = KoStorage.GetKorString("72401"); // time title
	//	lbTexts[2].text = KoStorage.GetKorString("72402"); // time sub title
		lbTexts[3].text = KoStorage.GetKorString("72302"); // btn label
	//	lbTexts[4].text = KoStorage.GetKorString("72302"); // btn label

		lbTexts[5].text = KoStorage.GetKorString("72408"); // day title
		lbTexts[6].text = KoStorage.GetKorString("72409"); // day sub title
		lbTexts[7].text = KoStorage.GetKorString("72411"); // day 1
		lbTexts[8].text = KoStorage.GetKorString("72412"); // day 2
		lbTexts[9].text = KoStorage.GetKorString("72413"); // day 3
		lbTexts[10].text = KoStorage.GetKorString("72414"); // day 4
		lbTexts[11].text = KoStorage.GetKorString("72415"); // day 5
		lbTexts[12].text = KoStorage.GetKorString("72416"); // day 6
		lbTexts[13].text = KoStorage.GetKorString("72417"); // day 7
		lbTexts[14].text = string.Empty;
		transform.FindChild("Attend_Week").FindChild("lbPrice").GetComponent<UILabel>().text = string.Format("x{0}",Common_Attend.Get(8713).R_no);
		AdColony.Configure
			(
				"version:2.1.4.1,store:google", // Arbitrary app version and Android app store declaration.
				"app947bfd13919b4060a7",  // ADC App ID from adcolony.com
				"vzf01640bf7fb647ef83", // A zone ID from adcolony.com / /test1 
				"vz21fdbb16b7f14c08a5",
				"vz0cdb25822d7b4dd083"
				);

		ADCAdManager.Instance.init();
	}

	private int nCount = 0;
	void OnRequestReward(int dReward){
		int rewardId = myAccount.instance.account.attendevent.AttackTimeID;
		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		if(rewardId == 0) Utility.LogError("id is zero");
		mDic.Add("rewardId",rewardId);
		int n = myAccount.instance.account.attendevent.AttackTimeID;
		Common_Attend.Item attendItem = Common_Attend.Get(n);
		string strValue = null;
		if(attendItem.R_type == 4) {
			mDic.Add("type",0);
			mDic.Add("value",attendItem.R_no*dReward);
		}else if(attendItem.R_type == 3) {
			mDic.Add("type",2);
			mDic.Add("value",attendItem.R_no*dReward);
		}else if(attendItem.R_type == 8) {
			mDic.Add("type",3);
			int matID = myAccount.instance.account.attendevent.materialId;
			if(myAccount.instance.account.attendevent.materialId == 0) {
				Utility.LogError("matId is Zero");
				matID = Random.Range(6000,6019);
			}
			strValue = "value;"+matID.ToString()+":"+(attendItem.R_no*dReward).ToString();
		}else if(attendItem.R_type == 7){ //gold
			mDic.Add("type",5);
			mDic.Add("value",attendItem.R_no*dReward);
		}else if(attendItem.R_type == 6){ //siver
			mDic.Add("type",4);
			mDic.Add("value",attendItem.R_no*dReward);
		}else if(attendItem.R_type == 5){ //evo
			mDic.Add("type",3);
			strValue = "value;8620:"+(attendItem.R_no*dReward).ToString();
		}
		string mAPI = ServerAPI.Get(90068);//"user/reward"
		NetworkManager.instance.HttpFormConnect("Put",mDic, mAPI ,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				GameObject.Find ("Audio").SendMessage("AttendStampSound");
				UpdateAttendUserInfo(dReward);
				myAccount.instance.account.attendevent.AttackTimeID = 0;
				SetEventTimeCheck();
				ChangeTimeEventReward();
				UserDataManager.instance.myGameDataSave();
			}
			Global.isNetwork = false;
		},strValue);
	}

	void OnAccept(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		OnRequestReward(1);
	}

	void OnAcceptDouble(){
		if(Global.isNetwork) return;
		Global.isNetwork = true; //vz676404cc17e14805b9 .. vzabc37b232cfa4f0b81

		nCount=1;
		PlayV4VCAd("vzf01640bf7fb647ef83", false, false);
	}

	public void PlayAVideo( string zoneID )
	{
		// Check to see if a video is available in the zone.
		if(AdColony.IsVideoAvailable(zoneID))
		{
			Utility.LogWarning("Play AdColony Video");
			AdColony.ShowVideoAd(zoneID); 
		}
		else
		{
			Utility.LogWarning("Video Not Available");
			if(nCount ==1){
				nCount=2;
				PlayAVideo("vz0cdb25822d7b4dd083");
			}else{
				Global.isNetwork = false;
			}
		}
	}
	
	private void OnVideoFinished(bool ad_was_shown)
	{
		Utility.LogWarning("On Video Finished");
		if(ad_was_shown) OnRequestReward(2);
		else Global.isNetwork = false;
		// Resume your app here.
	}
	
	// When a video is available, you may choose to play it in any fashion you like.
	// Generally you will play them automatically during breaks in your game,
	// or in response to a user action like clicking a button.
	// Below is a method that could be called, or attached to a GUI action.
	public void PlayV4VCAd( string zoneID, bool prePopup, bool postPopup )
	{
		// Check to see if a video for V4VC is available in the zone.
		if(AdColony.IsV4VCAvailable(zoneID))
		{
			Utility.LogWarning("Play AdColony V4VC Ad");

			// The AdColony class exposes two methods for showing V4VC Ads.
			// ---------------------------------------
			// The first `ShowV4VC`, plays a V4VC Ad and, optionally, displays
			// a popup when the video is finished.
			// ---------------------------------------
			// The second is `OfferV4VC`, which popups a confirmation before
			// playing the ad and, optionally, displays popup when the video 
			// is finished.
			
			// Call one of the V4VC Video methods:
			// Note that you should also pause your game here (audio, etc.) AdColony will not
			// pause your app for you.
			if (prePopup) AdColony.OfferV4VC( postPopup, zoneID );
			else  AdColony.ShowV4VC( postPopup, zoneID );
		}
		else
		{
			Utility.LogWarning("V4VC Ad Not Available");
			PlayAVideo("vz21fdbb16b7f14c08a5");
		}
	}
	
	// The V4VCResult Delegate assigned in Initialize -- AdColony calls this after confirming V4VC transactions with your server
	// success - true: transaction completed, virtual currency awarded by your server - false: transaction failed, no virtual currency awarded
	// name - The name of your virtual currency, defined in your AdColony account
	// amount - The amount of virtual currency awarded for watching the video, defined in your AdColony account
	private void OnV4VCResult(bool success, string name, int amount)
	{
		if(success)
		{
			Global.isNetwork = false;
			Utility.LogWarning("V4VC SUCCESS: name = " + name + ", amount = " + amount);
			OnRequestReward(2);
		}
		else
		{
			Utility.LogWarning("V4VC FAILED!");
			Global.isNetwork = false;
		}
	}

	void OnEnable(){
		AdColony.OnVideoFinished+= this.OnVideoFinished;
		AdColony.OnV4VCResult += this.OnV4VCResult;
		nCount = 0;
	}

	void OnDisable(){
		AdColony.OnVideoFinished -= this.OnVideoFinished;
		AdColony.OnV4VCResult -= this.OnV4VCResult;
		bTimeCheck = false;
	}

	private void UpdateAttendUserInfo(int reward){
		int n = myAccount.instance.account.attendevent.AttackTimeID;
		Common_Attend.Item attendItem = Common_Attend.Get(n);
		var lobby = GameObject.Find("LobbyUI") as GameObject;
		if(attendItem.R_type == 4) {
			//= "icon_dollar";
			GV.myDollar += (attendItem.R_no*reward);
			GV.updateDollar = - (attendItem.R_no*reward);
			lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
		}
		else if(attendItem.R_type == 3) {
			//strName = "icon_gold";
			GV.myCoin += (attendItem.R_no*reward);
			GV.updateCoin = - (attendItem.R_no*reward);
			lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
		}
		else if(attendItem.R_type == 1) {
			GV.mUser.FuelCount += (attendItem.R_no*reward);
			lobby.SendMessage("PlusBuyedItem",3, SendMessageOptions.DontRequireReceiver);
			lobby.SendMessage("FuelAdd",SendMessageOptions.DontRequireReceiver);
		}
		else if(attendItem.R_type == 8) {
			//	int k = (int)Well512.Next(0,20);
			//	k += 8600;
			int 	k = myAccount.instance.account.attendevent.materialId;
			if(k == 0){
				k = (int)Well512.Next(0,20);
				k += 8600;
			}
			GV.UpdateMatCount(k, attendItem.R_no*reward);
			myAccount.instance.account.attendevent.materialId = 0;
			lobby.SendMessage("PlusBuyedItem",5, SendMessageOptions.DontRequireReceiver);
			myAcc.instance.account.bInvenBTN[1] = true;
		}
		else if(attendItem.R_type == 7){ //gold
			GV.UpdateCouponList(1, attendItem.R_no*reward);
			lobby.SendMessage("PlusBuyedItem",5, SendMessageOptions.DontRequireReceiver);
			myAcc.instance.account.bInvenBTN[3] = true;
		}else if(attendItem.R_type == 6){ //siver
			GV.UpdateCouponList(0, attendItem.R_no*reward);
			lobby.SendMessage("PlusBuyedItem",5, SendMessageOptions.DontRequireReceiver);
			myAcc.instance.account.bInvenBTN[3] = true;
		}else if(attendItem.R_type == 5){ //evo
			GV.UpdateMatCount(8620, attendItem.R_no*reward);
			lobby.SendMessage("PlusBuyedItem",5, SendMessageOptions.DontRequireReceiver);
			myAcc.instance.account.bInvenBTN[2] = true;
		}
		else Utility.LogError("Non Type " + attendItem.R_type);
		
	}
	private void SetTimeEvent(){
		BTNs[0].SetActive(false);
		BTNs[1].SetActive(false);
		lbTexts[14].transform.parent.gameObject.SetActive(false);
		if(myAccount.instance.account.attendevent.AttackTimeID == 0){
			nTime = new System.DateTime(myAccount.instance.account.attendevent.LastTime);
			bTimeCheck = true;
			lbTexts[14].text = string.Empty;
			lbTexts[14].transform.parent.gameObject.SetActive(true);
			BTNs[0].SetActive(false);
			BTNs[1].SetActive(false);
		}else{
			nTime = new System.DateTime(myAccount.instance.account.attendevent.LastTime);
			System.TimeSpan sT = nTime - NetworkManager.instance.GetCurrentDeviceTime();
			if(sT.TotalSeconds <= 0){
				BTNs[0].SetActive(true);
				BTNs[1].SetActive(true);
				bTimeCheck = false;
				lbTexts[14].text = string.Empty;
				lbTexts[14].transform.parent.gameObject.SetActive(false);
			}else{
				BTNs[0].SetActive(false);
				BTNs[1].SetActive(false);
				bTimeCheck = true;
				lbTexts[14].text = string.Empty;
				lbTexts[14].transform.parent.gameObject.SetActive(true);
			}
		}
	
		ChangeTimeEventReward();
	}

	private void SetEventTimeCheck(){
		bTimeCheck = true;
		nTime = NetworkManager.instance.GetCurrentDeviceTime();
		nTime = nTime.AddHours(2);
		myAccount.instance.account.attendevent.bAccept = false;
		myAccount.instance.account.attendevent.CurrentTime = NetworkManager.instance.GetCurrentDeviceTime().Ticks;//System.DateTime.Now.Ticks;
		myAccount.instance.account.attendevent.LastTime = nTime.Ticks;
		//Utility.LogWarning(" a " +  new System.DateTime(myAccount.instance.account.attendevent.LastTime));
		myAccount.instance.SaveAccountInfo();
		lbTexts[14].transform.parent.gameObject.SetActive(true);
		BTNs[0].SetActive(false);
		BTNs[1].SetActive(false);
	}

	
	private bool bTimeCheck = false;
	void FixedUpdate(){
		if(!bTimeCheck) return;
		sTime = nTime-NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		if(sTime.TotalMinutes < 0){
			bTimeCheck = false;
			BTNs[0].SetActive(true);
			BTNs[1].SetActive(true);
			myAccount.instance.account.attendevent.bAccept = true;
			spTime.fillAmount = 0;
			lbTexts[14].text = string.Empty;
			lbTexts[14].transform.parent.gameObject.SetActive(false);
		} else 
			lbTexts[14].text = string.Format("{0:00}:{1:00}:{2:00}", sTime.Hours, sTime.Minutes, sTime.Seconds);
		float amount = 1.0f - ((float)sTime.TotalMinutes /  120);
		if(amount < 0) amount = 0.0f;
		spTime.fillAmount = amount;//1.0f - (sTime.TotalMinutes/120);
	}

	private void ChangeTimeEventReward(){
		int n = 0;
		if(myAccount.instance.account.attendevent.AttackTimeID == 0){
			n = (int)Well512.Next(0,13);
			n += 8700;
			myAccount.instance.account.attendevent.AttackTimeID = n;
		}else{
			n = myAccount.instance.account.attendevent.AttackTimeID;
		}
		var tr = RewardObj.transform as Transform;
		for(int i= 0; i < tr.childCount; i++){
			tr.GetChild(i).gameObject.SetActive(false);
		}
		Common_Attend.Item attendItem = Common_Attend.Get(n);
		string strName = string.Empty;
		if(attendItem.R_type == 4) strName = "icon_dollar";
		else if(attendItem.R_type == 3) strName = "icon_gold";
		else if(attendItem.R_type == 1) strName = "icon_fuel";
		else if(attendItem.R_type == 8) {
			strName = "icon_Mat";
			if(myAccount.instance.account.attendevent.materialId == 0){
				n = (int)Well512.Next(0,20);
				n += 8600;
				myAccount.instance.account.attendevent.materialId = n;
			}else{
				n = myAccount.instance.account.attendevent.materialId;
			}
			tr.FindChild(strName).GetComponent<UISprite>().spriteName = n.ToString();
		}
		else Utility.LogError("Non Type " + attendItem.R_type);
	

		tr.FindChild(strName).gameObject.SetActive(true);
		tr.FindChild("lbNum").gameObject.SetActive(true);
		tr.FindChild("lbNum").GetComponent<UILabel>().text = string.Format("x {0}", attendItem.R_no);
	}
	public void ShowAttendEvent(){
		SetTimeEvent();
		var dayattend = transform.FindChild("Attend_Week") as Transform;
		if(Global.gAttendDayCount == 0){
			for(int i =1; i <= 7;i++){
				string str2 = "Attend_"+i.ToString();
				var child3 =dayattend.FindChild(str2) as Transform;  
				child3.FindChild("icon_Stamp").gameObject.SetActive(true);
				child3.FindChild("icon_Stamp_effect").gameObject.SetActive(false);
				child3.FindChild("BG_Text").GetComponent<UISprite>().color =   new Color32(255,79,0,255);
			}
			return;
		}
		for(int i =1; i <= 7;i++){
			string str2 = "Attend_"+i.ToString();
			var child3 =dayattend.FindChild(str2) as Transform; 
			if(Global.gAttendDayCount < i){
				child3.FindChild("BG_Text").GetComponent<UISprite>().color = Color.white;
			}else{
				child3.FindChild("icon_Stamp").gameObject.SetActive(true);
				child3.FindChild("icon_Stamp_effect").gameObject.SetActive(false);
				child3.FindChild("BG_Text").GetComponent<UISprite>().color =   new Color32(255,79,0,255);
			}
		}
	}
	public void ShowAttendEventInit(){
		SetTimeEvent();
		var dayattend = transform.FindChild("Attend_Week") as Transform;
		if(Global.gAttendDayCount == 0){
			for(int i =1; i <= 7;i++){
				string str2 = "Attend_"+i.ToString();
				var child3 =dayattend.FindChild(str2) as Transform;  
				child3.FindChild("icon_Stamp").gameObject.SetActive(true);
				child3.FindChild("icon_Stamp_effect").gameObject.SetActive(false);
				child3.FindChild("BG_Text").GetComponent<UISprite>().color =   new Color32(255,79,0,255);
				if(i == 7){
					if(Global.gAttend == 0){
						child3.FindChild("icon_Stamp").gameObject.SetActive(true);
						child3.FindChild("BG_Text").GetComponent<UISprite>().color =  new Color32(255,79,0,255);
					}else{
						StampAction(child3.FindChild("icon_Stamp"), false);
					}
				}
			}
			return;
		}
		for(int i =1; i <= 7;i++){
			string str2 = "Attend_"+i.ToString();
			var child3 =dayattend.FindChild(str2) as Transform; 
			if(Global.gAttendDayCount < i){
				child3.FindChild("BG_Text").GetComponent<UISprite>().color = Color.white;
			}else if(Global.gAttendDayCount == i){
				if(Global.gAttend == 0){
					child3.FindChild("icon_Stamp").gameObject.SetActive(true);
					child3.FindChild("BG_Text").GetComponent<UISprite>().color =  new Color32(255,79,0,255);
				}else{
					StampAction(child3.FindChild("icon_Stamp"), false);
				}
			}else{
				child3.FindChild("icon_Stamp").gameObject.SetActive(true);
				child3.FindChild("BG_Text").GetComponent<UISprite>().color =  new Color32(255,79,0,255);
			}
		}
	
	}
	
	void StampAction(Transform child2, bool b){
		child2.gameObject.SetActive(true);
		GameObject.Find ("Audio").SendMessage("AttendSound");
		Vector3 tscale = child2.localScale;
		child2.localScale = new Vector3(0.1f,0.1f,0.1f);
		var tw = child2.gameObject.AddComponent<TweenScale>() as TweenScale;
		tw.duration = 0.25f;
		tw.to = tscale;
		tw.from = tscale * 3.0f;
		tw.delay = 0.5f;
		tw.enabled = true;
		tw.onFinished = delegate(UITweener tween) {
			if(b)
			tween.transform.parent.FindChild("BG_Text").GetComponent<UISprite>().color =  new Color32(255,79,0,255);
			else 
			tween.transform.parent.FindChild("BG_Text").GetComponent<UISprite>().color =  new Color32(255,79,0,255);
			tween.transform.parent.FindChild("icon_Stamp_effect").gameObject.SetActive(true);
			GameObject.Find ("Audio").SendMessage("AttendStampSound");
			Destroy(tween);
		};
	}
	
	void StampSound(){
		GameObject.Find ("Audio").SendMessage("AttendStampSound");
	}

}
