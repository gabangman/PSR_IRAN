using UnityEngine;
using System.Collections;

public class EventItems : MonoBehaviour {

	void Start(){
		//	transform.GetComponent<UIButtonMessage>().target = transform.parent.parent.parent.parent.gameObject;
		//	transform.GetComponent<UIDragPanelContents>().draggablePanel = transform.parent.parent.gameObject.GetComponent<UIDraggablePanel>();
		
		long dayTime = myAccount.instance.account.dayCheckTime;
		nowTime   = System.DateTime.Now;
		RemainTime = new System.DateTime(dayTime);
		RemainTime = RemainTime.AddDays(1);
		
	}
	
	public void InitEventWindow(int idx){
		if(idx == 0){
			SetNewInfo();
		}else if(idx == 1){
			SetEvoInfo();
		}else if(idx == 2){
	
			SetReqInfo();
		}
	}
	
	private System.DateTime RemainTime;
	private System.TimeSpan mCompareTime;
	private System.DateTime nowTime;
	private UILabel lbtimes;
	void FixedUpdate(){
		//	if(!isTimeCheck) return;
		if(lbtimes == null) return;
		mCompareTime = RemainTime-NetworkManager.instance.GetCurrentDeviceTime();
		lbtimes.text = string.Format("{0:00}:{1:00}:{2:00}", mCompareTime.Hours, mCompareTime.Minutes, mCompareTime.Seconds);
		
	}
	
	void SetNewInfo(){
		int carID = myAccount.instance.account.eRace.testDriveCarID;
		//transform.FindChild("EventTitle").FindChild("lbEventName").GetComponent<UILabel>().text = "";
		transform.FindChild("EventTitle").GetComponentInChildren<UILabel>().text = 
			KoStorage.GetKorString("73318");
		var tr = transform.FindChild("Info") as Transform;
		tr.FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73302");
		tr.FindChild("lbName").GetComponent<UILabel>().text = Common_Car_Status.Get(carID).Name;
		tr.FindChild("icon_car").GetComponent<UISprite>().spriteName = carID.ToString();
		tr.FindChild("lbClass").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74024"), "SS");// "";
		
		
		lbtimes = transform.FindChild("RemainTime").GetComponentInChildren<UILabel>();
		tr = transform.FindChild("Reward");
		var mTr = transform.FindChild("FlagGroup") as Transform;
		for(int  i =0 ; i < mTr.childCount; i++){
			if(myAccount.instance.account.eRace.testDrivePlayCount > i){
				mTr.GetChild(i).FindChild("Check").gameObject.SetActive(true);
			}
		}
		
		if(myAccount.instance.account.eRace.testDrivePlayCount != 5){
			tr.FindChild("Dollor").GetComponentInChildren<UILabel>().text =string.Format("{0:#,0}", Common_Reward.Get(Global.gRewardId).Reward_newcar);
			
		}else{
		//	tr.gameObject.SetActive(false);
		//	tr = transform.FindChild("Event_Finish");//.gameObject.SetActive(true);
		///	tr.gameObject.SetActive(true);
		//	tr.FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73312");
		}
	}
	
	void SetReqInfo(){
		int carID = myAccount.instance.account.eRace.featuredCarID;
		transform.FindChild("EventTitle").GetComponentInChildren<UILabel>().text = 
			KoStorage.GetKorString("73317");
		var tr = transform.FindChild("Info") as Transform;
		tr.FindChild ("Text1").GetComponent<UILabel> ().text = KoStorage.GetKorString ("73304");
		tr.FindChild ("lbText").GetComponent<UILabel> ().text = KoStorage.GetKorString ("73320");
		tr.FindChild("lbName_car").GetComponent<UILabel>().text = Common_Car_Status.Get(carID).Name;
		tr.FindChild("icon_car").GetComponent<UISprite>().spriteName = carID.ToString();
		
		tr = transform.FindChild("Reward");
		if(AccountManager.instance.ChampItem.S2_5_Event_Featured == 0){
			tr.gameObject.SetActive(false);
			transform.FindChild("Locked").gameObject.SetActive(true);
			transform.FindChild("Locked").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("73041");
			transform.FindChild("RemainTime").gameObject.SetActive(false);
			transform.FindChild("FlagGroup").gameObject.SetActive(false);
			transform.GetComponent<UIButtonMessage>().functionName = "OnNonEvent";
			return;
		}
		
		var mTr = transform.FindChild("FlagGroup") as Transform;
		for(int  i =0 ; i < mTr.childCount; i++){
			if(myAccount.instance.account.eRace.featuredPlayCount > i){
				mTr.GetChild(i).FindChild("Check").gameObject.SetActive(true);
			}
		}
		lbtimes = transform.FindChild("RemainTime").GetComponentInChildren<UILabel>();
		
		if(myAccount.instance.account.eRace.featuredPlayCount != 5){
			
			
		}else{
			tr.gameObject.SetActive(false);
			tr = transform.FindChild("Event_Finish");//.gameObject.SetActive(true);
			tr.gameObject.SetActive(true);
			tr.FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73312");
		}
		
	}
	
	void SetEvoInfo(){
		int TrackID = myAccount.instance.account.eRace.EvoTrackID;
		transform.FindChild("EventTitle").GetComponentInChildren<UILabel>().text = 
			KoStorage.GetKorString("73319");
		var tr = transform.FindChild("Info") as Transform;
		tr.FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73307");
		
		tr.FindChild("icon_map").GetComponent<UISprite>().spriteName  = TrackID.ToString()+"P";
		//tr.FindChild("icon_mapType").GetComponent<UISprite>().spriteName  = TrackID.ToString()+"T";
		//Common_Track.Item tItem = Common_Track.Get(TrackID);
		//tr.FindChild("lbName_Track").GetComponent<UILabel>().text = tItem.Name;
		//tr.FindChild("lbName_Track_Distance").GetComponent<UILabel>().text =string.Format(KoStorage.GetKorString("73143"), tItem.Distance);
		if(AccountManager.instance.ChampItem.S4_1_Event_Evocube == 0){
			transform.FindChild("Reward").gameObject.SetActive(false);
			transform.FindChild("Locked").gameObject.SetActive(true);
			transform.FindChild("Locked").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("73042");
			transform.FindChild("RemainTime").gameObject.SetActive(false);
			transform.GetComponent<UIButtonMessage>().functionName = "OnNonEvent";
			return;
		}
		bool bCheck = false;
		tr = transform.FindChild("Reward").FindChild("Deck1");
		if(myAccount.instance.account.eRace.EvoAcquistCount == 1){
			bCheck = false;
		}else{
			if(myAccount.instance.account.eRace.EvoPlayCount == 0) bCheck = false;
			else bCheck = true;
			
		}
		
		if(bCheck){
			Common_Reward.Item item = Common_Reward.Get(Global.gRewardId);
			tr.FindChild("lbAmount").GetComponent<UILabel>().text = string.Format("{0:000}/{1:000}", myAccount.instance.account.eRace.EvoPlayCount, 1000);
			//tr.FindChild("lbName").GetComponent<UILabel>().text =  KoStorage.GetKorString("75002");
			lbtimes = transform.FindChild("RemainTime").GetComponentInChildren<UILabel>();
		}else{
			transform.FindChild("Reward").gameObject.SetActive(false);
			tr = transform.FindChild("Event_Finish");//.gameObject.SetActive(true);
			tr.gameObject.SetActive(true);
			tr.FindChild("Text_1").GetComponent<UILabel>().text = KoStorage.GetKorString("73312");
		}
		
		
		
	}
}
