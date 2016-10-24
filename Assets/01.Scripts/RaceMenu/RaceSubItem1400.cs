using UnityEngine;
using System.Collections;

public class RaceSubItem1400 : RaceSubItems {

	
	private System.DateTime RemainTime;
	private System.TimeSpan mCompareTime;
	private System.DateTime nowTime;
	private UILabel lbtime;
	void FixedUpdate(){
		//	if(!isTimeCheck) return;
		if(lbtime == null) return;
		mCompareTime = RemainTime-NetworkManager.instance.GetCurrentDeviceTime();
		lbtime.text = string.Format("{0:00}:{1:00}:{2:00}", mCompareTime.Hours, mCompareTime.Minutes, mCompareTime.Seconds);
		
	}


	public void OnMap1400(){
		if(AccountManager.instance.ChampItem.S2_1_Ranking == 0){
			bLock = true;
			var tr = mapInfo.transform.FindChild("Locked") as Transform;
			tr.gameObject.SetActive(true);
			tr.FindChild("Text1").GetComponent<UILabel>().text =KoStorage.GetKorString("73045");
			modeInfo.transform.FindChild("Ranking").GetChild(0).GetComponent<UIButtonMessage>().functionName = null;
		}else{
			bLock = false;
			mapInfo.transform.FindChild("Locked").gameObject.SetActive(false);
			var tr = mapInfo.transform.FindChild("Info") as Transform;
			tr.gameObject.SetActive(true);
			tr.FindChild("Effect").gameObject.SetActive(true);
			tr.FindChild("lbName").GetComponent<UILabel>().text = Common_Track.Get(1400).Name;
			tr.FindChild("lbMile_1").GetComponent<UILabel>().text = string.Format( KoStorage.GetKorString("73143"), Common_Track.Get(1400).Distance);

			var rankTr = modeInfo.transform.FindChild("Ranking").GetChild(0) as Transform;
			if(myAccount.instance.account.weeklyRace.WeeklyPlayCount >= 100){
				rankTr.FindChild("Mode_On").gameObject.SetActive(false);
				rankTr.FindChild("Mode_Finish").gameObject.SetActive(true);
				rankTr.FindChild("Mode_Finish").FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("72806");
				rankTr.GetComponent<UIButtonMessage>().functionName = string.Empty;
			}else{
				rankTr.GetComponent<UIButtonMessage>().functionName = "OnNext";
				rankTr.FindChild("Mode_On").FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("72203");
				rankTr.FindChild("Mode_On").FindChild("lbMaxCount").GetComponent<UILabel>().text = string.Format("{0:00}/{1:00}", myAccount.instance.account.weeklyRace.WeeklyPlayCount, 100);
			}

			var tr2 = modeInfo.transform.FindChild("Event_Evocube").GetChild(0) as Transform;
			if(AccountManager.instance.ChampItem.S4_1_Event_Evocube == 0){
				tr2.FindChild("Locked").gameObject.SetActive(true);
				tr2.FindChild("Locked").FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73042");
				//tr2.FindChild("EventTitle").gameObject.SetActive(false);
				tr2.GetComponent<UIButtonMessage>().functionName = null;
				//mapInfo.transform.FindChild("Info").FindChild("EventTitle").gameObject.SetActive(false);
			}else{
				mapInfo.transform.FindChild("EventTitle").gameObject.SetActive(true);
				tr2.FindChild("Locked").gameObject.SetActive(false);
				long dayTime = myAccount.instance.account.dayCheckTime;
				RemainTime = new System.DateTime(dayTime);
				RemainTime = RemainTime.AddDays(1);
				if(myAccount.instance.account.eRace.EvoAcquistCount == 1){
				//	transform.FindChild("Reward").gameObject.SetActive(false);
					var tr1 = modeInfo.transform.FindChild("Event_Evocube").GetChild(0).FindChild("Event_Finish") as Transform;//.gameObject.SetActive(true);
					tr1.gameObject.SetActive(true);
					tr1.FindChild("lb_Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73312");
					lbtime = tr1.FindChild("lb_Time").GetComponent<UILabel>();
					tr2.GetComponent<UIButtonMessage>().functionName = string.Empty;
					mapInfo.transform.FindChild("EventTitle").gameObject.SetActive(false);
				}else{
					if(myAccount.instance.account.eRace.EvoPlayCount == 0) {
						var tr1 = modeInfo.transform.FindChild("Event_Evocube").GetChild(0).FindChild("Event_Finish") as Transform;//.gameObject.SetActive(true);
						tr1.gameObject.SetActive(true);
						tr1.FindChild("lb_Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73312");
						lbtime = tr1.FindChild("lb_Time").GetComponent<UILabel>();
						mapInfo.transform.FindChild("EventTitle").gameObject.SetActive(false);
						tr2.GetComponent<UIButtonMessage>().functionName = string.Empty;
						//tr2.GetComponent<UIButtonMessage>().functionName = OnNext1;
					}else{//Event_On.
						var tr1 = modeInfo.transform.FindChild("Event_Evocube").GetChild(0).FindChild("Event_On") as Transform;//.gameObject.SetActive(true);
						tr1.gameObject.SetActive(true);
						tr1.FindChild("EventTitle").gameObject.SetActive(true);
						tr1.FindChild("lb_Num").GetComponent<UILabel>().text = string.Format("{0:000}/{1:000}", myAccount.instance.account.eRace.EvoPlayCount, 1000);
						//tr.FindChild("lbName").GetComponent<UILabel>().text =  KoStorage.GetKorString("75002");
						lbtime = tr1.FindChild("lb_Time").GetComponent<UILabel>();
						mapInfo.transform.FindChild("EventTitle").gameObject.SetActive(true);
						tr2.GetComponent<UIButtonMessage>().functionName = "OnNext1";

					}
					
				}
				
			}
		}
		
	}
	public override void OnNext (GameObject obj)
	{
		if(Global.isPopUp) return;
		if(Global.isNetwork || Global.isAnimation) return;
		if(myAccount.instance.account.weeklyRace.WeeklyPlayCount == 100) return;
		NGUITools.FindInParents<RaceMenuStart>(gameObject).OpenAniING();
		GameObject.Find("LobbyUI").SendMessage("OnRankRaceClick",obj.name,SendMessageOptions.DontRequireReceiver);

	}


	public override void OnNext1 (GameObject obj)
	{
		if(Global.isPopUp) return;
		if(Global.isNetwork || Global.isAnimation) return;
		NGUITools.FindInParents<RaceMenuStart>(gameObject).OpenEvent(1);
	}


}
