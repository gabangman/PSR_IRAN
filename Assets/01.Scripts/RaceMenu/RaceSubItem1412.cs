using UnityEngine;
using System.Collections;

public class RaceSubItem1412 : RaceSubItems {

	void FixedUpdate(){
		if(lbtime == null) return;
		mCompareTime = RemainTime-NetworkManager.instance.GetCurrentDeviceTime();
		lbtime.text = string.Format("{0:00}:{1:00}:{2:00}", mCompareTime.Hours, mCompareTime.Minutes, mCompareTime.Seconds);

		
	}
	
	
	public void OnMap1412(){
		var tr = mapInfo.transform.FindChild("Info") as Transform;
		tr.FindChild("Effect").gameObject.SetActive(true);
		Common_Track.Item item = Common_Track.Get(1412);
		tr.FindChild("lbName").GetComponent<UILabel>().text = item.Name;
		tr.FindChild("lbMile_1").GetComponent<UILabel>().text = string.Format( KoStorage.GetKorString("73143"), item.Distance);
		tr.FindChild("lbMile_2").GetComponent<UILabel>().text =string.Format( KoStorage.GetKorString("73143"), item.Distance_big);
		bLock = false;

		tr = modeInfo.transform.FindChild("Regular").GetChild(0); //Next
		if(AccountManager.instance.ChampItem.S1_5_Regular_Drag == 0){
			tr.GetComponent<UIButtonMessage>().functionName = null;
			tr.FindChild("Locked").gameObject.SetActive(true);
			tr.FindChild("Locked").FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73040");
		}else{
			tr.GetComponent<UIButtonMessage>().functionName = "OnNext";
			tr.FindChild("Locked").gameObject.SetActive(false);
			tr.FindChild("Mode_On").gameObject.SetActive(true);
			tr.FindChild("Mode_On").FindChild("Text1").GetComponent<UILabel>().text =KoStorage.GetKorString("73207");
			Common_Reward.Item items = Common_Reward.Get(Global.gRewardId);
			tr.FindChild("Mode_On").FindChild("Reward").FindChild("Quantity").GetComponent<UILabel>().text =string.Format(KoStorage.GetKorString("73406"), items.Reward_mat_regular_drag);
			//tr1.FindChild("Title_Reward").GetComponent<UILabel>().text =  KoStorage.GetKorString("73144");
		//	tr1.FindChild("Text1").GetComponent<UILabel>().text =  KoStorage.GetKorString("73207");
		}
 		tr = modeInfo.transform.FindChild("PVP").GetChild(0); //Next1
		if(AccountManager.instance.ChampItem.S2_3_PVP_Drag == 0){
			tr.GetComponent<UIButtonMessage>().functionName = null;
			tr.FindChild("Locked").gameObject.SetActive(true);
			tr.FindChild("Locked").FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73043");
//			tr.FindChild("Reward").gameObject.SetActive(false);
		}else{
			tr.GetComponent<UIButtonMessage>().functionName = "OnNext1";
			tr.FindChild("Locked").gameObject.SetActive(false);
			tr.FindChild("Mode_On").gameObject.SetActive(true);
			tr.FindChild("Mode_On").FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73408");
			//Common_Reward.Item items = Common_Reward.Get(Global.gRewardId);
			tr.FindChild("Mode_On").FindChild("Reward").FindChild("Quantity").GetComponent<UILabel>().text = KoStorage.GetKorString("73205");//string.Format(KoStorage.GetKorString("73406"), items.Reward_mat_regular_drag);
		}

		tr = modeInfo.transform.FindChild("Event_NewCar").GetChild(0); //Next2
		var tr1 = tr.FindChild("Event_Finish") as Transform;
		
		var tr2 = tr.FindChild("Event_On") as Transform;
		if(myAccount.instance.account.eRace.testDrivePlayCount != 5){
			tr2.gameObject.SetActive(true);
			tr2.FindChild("Title").GetComponent<UILabel>().text =	KoStorage.GetKorString("73318");
			tr2.FindChild("Num").GetComponent<UILabel>().text = string.Format("{0}/{1}", myAccount.instance.account.eRace.testDrivePlayCount,5);
			mapInfo.transform.FindChild("EventTitle").gameObject.SetActive(true);
			tr2.FindChild("icon_car").GetComponent<UISprite>().spriteName =  myAccount.instance.account.eRace.testDriveCarID.ToString();
		}else{
			tr1.gameObject.SetActive(true);
			tr1.FindChild("lb_Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73312");
			lbtime = tr1.FindChild("lb_Time").GetComponent<UILabel>();
			mapInfo.transform.FindChild("EventTitle").gameObject.SetActive(false);
			tr.GetComponent<UIButtonMessage>().functionName = string.Empty;
			long dayTime = myAccount.instance.account.dayCheckTime;
			RemainTime = new System.DateTime(dayTime);
			RemainTime = RemainTime.AddDays(1);
		}

	}
	public override void OnNext (GameObject obj)
	{
		if(Global.isPopUp) return;
		if(Global.isNetwork || Global.isAnimation) return;
		//string str = "ING_"+GV.ChSeason.ToString();
		NGUITools.FindInParents<RaceMenuStart>(gameObject).OpenAniING();
		GameObject.Find("LobbyUI").SendMessage("OnRegularRaceClick","Drag",SendMessageOptions.DontRequireReceiver);	
		
	}
	
	public override void OnNext1 (GameObject obj)
	{
		if(Global.isPopUp) return;
		if(Global.isNetwork || Global.isAnimation) return;
		NGUITools.FindInParents<RaceMenuStart>(gameObject).OpenAniING();
		GameObject.Find("LobbyUI").SendMessage("OnPVPRaceClick","Drag",SendMessageOptions.DontRequireReceiver);
	}
	
	
	public override void OnNext2 (GameObject obj)
	{
		if(Global.isPopUp) return;
		if(Global.isNetwork || Global.isAnimation) return;
		NGUITools.FindInParents<RaceMenuStart>(gameObject).OpenEvent(0);
		
	}
	
	private System.DateTime RemainTime;
	private System.TimeSpan mCompareTime;
	private System.DateTime nowTime;
	private UILabel lbtime;
}
