using UnityEngine;
using System.Collections;

public class BuyFreeFuel : MonoBehaviour {

	// Use this for initialization
	private UILabel lbTimes;
	private System.DateTime cTime, hTime,rTime;
	private System.TimeSpan sTime;
	private bool bTime;
	void Start () {
	}

	void OnEnable(){
		if(lbTimes != null) {}
		
		else lbTimes = transform.parent.FindChild("lbPrice").GetComponent<UILabel>() as UILabel;
		if(setTimeCheck()){

			lbTimes.text = null;
			transform.GetChild(0).gameObject.SetActive(true);
		}else{
		//	sTime = hTime-NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		//	lbTimes.text = string.Format("{0:00}:{1:00}", sTime.Minutes, sTime.Seconds);
			transform.GetChild(0).gameObject.SetActive(false);
		
		}

	}

	void OnDisable()
	{
	}

	private System.TimeSpan ReTime;
	// Update is called once per frame
	void FixedUpdate () {
		if(bTime) return;
		ReTime = hTime-NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		lbTimes.text = string.Format("{0:00}:{1:00}", ReTime.Minutes, ReTime.Seconds);
		if(ReTime.TotalMinutes < 0){
			bTime = true;
			lbTimes.text = null;
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}

	private bool setTimeCheck(){
		hTime = new System.DateTime(myAcc.instance.account.lastViewFreeFuelTime);
		 cTime = NetworkManager.instance.GetCurrentDeviceTime();
		 sTime = cTime - hTime;
		bool b = false;
	//	Utility.LogWarning("t  " + sTime.TotalMinutes);
		if(sTime.TotalMinutes < 5){
			b = false;
			bTime = false;
			hTime = hTime.AddMinutes(5);
		}else{
			b = true;
			bTime = true;
		}
		return b;
	}

	void OnFreeFuel(){
		if(!bTime) return;
		Utility.LogWarning("ONFree");
		OnWatch();
	}

	public void OnFinishAction(){
	//	if(Global.isNetwork) return;
	//	Global.isNetwork = true;
		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		mDic.Add("rewardId",8766);
		mDic.Add("type",1);
		mDic.Add("value",GV.mUser.FuelMax);
		string mAPI = ServerAPI.Get(90068);//"user/reward"
		NetworkManager.instance.HttpFormConnect("Put",mDic, mAPI ,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				myAcc.instance.account.lastViewFreeFuelTime = NetworkManager.instance.GetCurrentDeviceTime().Ticks;
				GV.mUser.FuelCount = GV.mUser.FuelMax;
				var lobby = GameObject.Find("LobbyUI");
				lobby.SendMessage("PlusBuyedItem",3, SendMessageOptions.DontRequireReceiver);
				lobby.SendMessage("FuelAdd",SendMessageOptions.DontRequireReceiver);
				setTimeCheck();
				transform.GetChild(0).gameObject.SetActive(false);
			}
			Global.isNetwork = false;
		});



	}

	protected void OnWatch(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		Heyzap.HZVideoAd.AdDisplayListener mL1 = delegate(string adState, string adTag) {
			Utility.LogWarning("hzvideoAd state " + adState + " tag " + adTag);
			
			if ( adState.Equals("show") ) {
			}
			if ( adState.Equals("hide") ) {
				OnFinishAction();
			}
			if ( adState.Equals("click") ) {
			}
			if ( adState.Equals("failed") ) {
				this.FinishCallback = null;
				this.FailedCallback = null;
				var pop = ObjectManager.SearchWindowPopup() as GameObject;
				var mAction = pop.GetComponent<NoFreeFuelPopup>() as NoFreeFuelPopup;
				if(mAction == null) mAction = pop.AddComponent<NoFreeFuelPopup>();
				mAction.InitPopUp(()=>{
					//myAcc.instance.account.lastViewFreeFuelTime = NetworkManager.instance.GetCurrentDeviceTime().Ticks;
					Global.isNetwork = false;
					setTimeCheck();
					//transform.GetChild(0).gameObject.SetActive(false);
				});
			}
			if ( adState.Equals("available") ) {
			}
			if ( adState.Equals("fetch_failed") ) {
			
			}
			if ( adState.Equals("audio_starting") ) {
			}
			if ( adState.Equals("audio_finished") ) {
			}
			Global.isNetwork = false;
		};
		Heyzap.HZVideoAd.SetDisplayListener(mL1);
		Heyzap.HZVideoAd.Fetch();



		if (Heyzap.HZVideoAd.IsAvailable()) {
			Heyzap.HZVideoAd.Show();
			Global.isNetwork = false;
		}else{
			this.FinishCallback = null;
			this.FailedCallback = null;
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			var mAction = pop.GetComponent<NoFreeFuelPopup>() as NoFreeFuelPopup;
			if(mAction == null) mAction = pop.AddComponent<NoFreeFuelPopup>();
			mAction.InitPopUp(()=>{
				Global.isNetwork = false;
				setTimeCheck();
			});
		}
	}


	void OnFailedAction(int a){
		if(a==0){
			OnWatch3Click(OnFinishAction, OnFailedAction);
		}else{
			this.FinishCallback = null;
			this.FailedCallback = null;
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			var mAction = pop.GetComponent<NoFreeFuelPopup>() as NoFreeFuelPopup;
			if(mAction == null) mAction = pop.AddComponent<NoFreeFuelPopup>();
			mAction.InitPopUp(()=>{
				myAcc.instance.account.lastViewFreeFuelTime = NetworkManager.instance.GetCurrentDeviceTime().Ticks;
				Global.isNetwork = false;
				setTimeCheck();
				transform.GetChild(0).gameObject.SetActive(false);
			});
		}
	}

	public void OnWatchUnityAdClick(System.Action callback, System.Action<int> fCallback){
		this.FinishCallback = callback;
		this.FailedCallback = fCallback;
	//	Utility.LogWarning("OnWatchUnityAdClick");
	//	FailedCallback(0);
	//	return;
		if(!UnityAdsHelper.ShowAd(string.Empty, true, UnityAdResult)){
			FailedCallback(0);
		}
	}
	
	public void OnWatch3Click(System.Action callback, System.Action<int> fCallback){
		this.FinishCallback = callback;
		this.FailedCallback = fCallback;
		PlayAVideo("vz7888f332683541e382");
	//	Utility.LogWarning("OnWatch3Click");
	}


	private System.Action FinishCallback;
	private System.Action<int> FailedCallback;
	private void UnityAdResult(UnityEngine.Advertisements.ShowResult result){
		switch (result)
		{
		case UnityEngine.Advertisements.ShowResult.Finished:
			Utility.Log("The ad was successfully shown.ADRewardContent ");
			if(FinishCallback != null)
				FinishCallback();
			FinishCallback = null;
			break;
		case UnityEngine.Advertisements.ShowResult.Skipped:
			Utility.Log("The ad was skipped before reaching the end.ADRewardContent");
			FinishCallback = null;
			Global.isNetwork = false;
			break;
		case UnityEngine.Advertisements.ShowResult.Failed:
			Utility.LogError("The ad failed to be shown.ADRewardContent");
			Global.isNetwork = false;
			FinishCallback = null;
			break;
		}
	}
	
	
	// When a video is available, you may choose to play it in any fashion you like.
	// Generally you will play them automatically during breaks in your game,
	// or in response to a user action like clicking a button.
	// Below is a method that could be called, or attached to a GUI action.
	public void PlayAVideo( string zoneID )
	{
		// Check to see if a video is available in the zone.
		if(AdColony.IsVideoAvailable(zoneID))
		{
			Utility.LogWarning("Play AdColony Video");
			// Call AdColony.ShowVideoAd with that zone to play an interstitial video.
			// Note that you should also pause your game here (audio, etc.) AdColony will not
			// pause your app for you.
			AdColony.ShowVideoAd(zoneID); 
		}
		else
		{
			//Utility.LogWarning("Video Not Available");
			this.FailedCallback(1);
		}
	}
	
	private void OnVideoFinished(bool ad_was_shown)
	{
		Utility.LogWarning("On Video Finished");
		if(FinishCallback != null)
			FinishCallback();
		FinishCallback = null;
	}
	
	public void PlayV4VCAd( string zoneID, bool prePopup, bool postPopup )
	{
		if(AdColony.IsV4VCAvailable(zoneID))
		{
			Utility.LogWarning("Play AdColony V4VC Ad");
			if (prePopup) AdColony.OfferV4VC( postPopup, zoneID );
			else  AdColony.ShowV4VC( postPopup, zoneID );
		}
		else
		{
			Utility.LogWarning("V4VC Ad Not Available");
		}
	}

	private void OnV4VCResult(bool success, string name, int amount)
	{
		if(success)
		{Global.isNetwork = false;
			Utility.LogWarning("V4VC SUCCESS: name = " + name + ", amount = " + amount);
		}
		else
		{
			Utility.LogWarning("V4VC FAILED!");
		}
	}
	


}
