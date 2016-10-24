using UnityEngine;
using System.Collections;

public class RaceSubItem1404 : RaceSubItems {
	
	
	private System.DateTime RemainTime;
	private System.TimeSpan mCompareTime;
	private System.DateTime nowTime;
	private UILabel lbtime;
	
	void FixedUpdate(){
		if(lbtime == null) return;
		mCompareTime = RemainTime-NetworkManager.instance.GetCurrentDeviceTime();
		lbtime.text = string.Format("{0:00}:{1:00}:{2:00}", mCompareTime.Hours, mCompareTime.Minutes, mCompareTime.Seconds);
		
	}
	
	
	public void OnMap1404(){
		if(GV.ChSeasonID >= 6015 && GV.ChSeasonID < 6020){
			var tr= mapInfo.transform.FindChild("Info") as Transform;
			tr.FindChild("BG_On").gameObject.SetActive(true);
			tr.FindChild("lbName").GetComponent<UILabel>().text = Common_Track.Get(1404).Name;
			tr.FindChild("lbMile_1").GetComponent<UILabel>().text = string.Format( KoStorage.GetKorString("73143"), Common_Track.Get(1404).Distance);
			tr.gameObject.SetActive(true);
			tr.FindChild("Flag").FindChild("lbNum").GetComponent<UILabel>().text = string.Format("{0}/{1}",(GV.ChSeasonID - 6015),5);
			tr.FindChild("Effect").gameObject.SetActive(true);
			tr = modeInfo.transform.FindChild("Champion");
			SetChampionInfo(tr);
			//mapInfo.transform.FindChild("EventTitle").gameObject.SetActive(true);

		}else if(GV.ChSeasonID >= 6020){
			var tr= mapInfo.transform.FindChild("Info") as Transform;
			//tr.FindChild("Effect").gameObject.SetActive(true);
			tr.FindChild("lbName").GetComponent<UILabel>().text = Common_Track.Get(1404).Name;
			tr.FindChild("lbMile_1").GetComponent<UILabel>().text = string.Format( KoStorage.GetKorString("73143"), Common_Track.Get(1404).Distance);
			tr.gameObject.SetActive(true);
			tr.FindChild("Season_Clear").gameObject.SetActive(true);
			tr.FindChild("Flag").gameObject.SetActive(false);

			tr = modeInfo.transform.FindChild("Regular");
			SetRegularInfo(tr);
			if(GV.RegularTrack == 1404){
				tr.GetChild(0).FindChild("Mode_On").gameObject.SetActive(true);
				tr.GetChild(0).GetComponent<UIButtonMessage>().functionName = "OnNext1";
				mapInfo.transform.FindChild("Info").FindChild("Effect").gameObject.SetActive(true);
			}else{
				tr.GetChild(0).FindChild("Mode_On").gameObject.SetActive(false);
				tr.GetChild(0).FindChild("Mode_No").gameObject.SetActive(true);
				tr.GetChild(0).GetComponent<UIButtonMessage>().functionName = null;
			}
			
			tr = modeInfo.transform.FindChild("Event_Featured");
			SetEventInfo(tr);
			

			mapInfo.transform.FindChild("Locked").gameObject.SetActive(false);

		}else if(GV.ChSeasonID < 6015){
			var tr = mapInfo.transform.FindChild("Locked") as Transform;
			tr.gameObject.SetActive(true);
			tr.FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73146");bLock =true;
		}
	}
	
	void SetEventInfo(Transform tr){
		tr = tr.GetChild(0);
		var obj = mapInfo.transform.FindChild("EventTitle").gameObject as GameObject;
		if(AccountManager.instance.ChampItem.S2_5_Event_Featured == 0){
			tr.FindChild("Locked").gameObject.SetActive(true);
			tr.FindChild("Locked").FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73041");
			//if(!obj.activeSelf) mapInfo.transform.FindChild("EventTitle").gameObject.SetActive(false);
			tr.GetComponent<UIButtonMessage>().functionName = null;
		}else{
			
			if(GV.EventTrack == 1404){
				tr.FindChild("Locked").gameObject.SetActive(false);
				tr.GetComponent<UIButtonMessage>().functionName = "OnNext2";
			}else{
				tr.FindChild("Locked").gameObject.SetActive(false);
				tr.FindChild("Event_No").gameObject.SetActive(true);
				tr.GetComponent<UIButtonMessage>().functionName = null;
				return;
			}
			
			
			long dayTime = myAccount.instance.account.dayCheckTime;
			RemainTime = new System.DateTime(dayTime);
			RemainTime = RemainTime.AddDays(1);
			if(myAccount.instance.account.eRace.featuredPlayCount != 5){
				tr.FindChild("Event_On").gameObject.SetActive(true);
				tr.FindChild("Event_On").FindChild("lb_Title").GetComponent<UILabel>().text = KoStorage.GetKorString("73317");
				lbtime =tr.FindChild("Event_On").FindChild("lb_Time").GetComponent<UILabel>();
				tr.FindChild("Event_On").FindChild("EventTitle").gameObject.SetActive(true);
				mapInfo.transform.FindChild("EventTitle").gameObject.SetActive(true);
				mapInfo.transform.FindChild("Info").FindChild("Effect").gameObject.SetActive(true);
			}else{
				tr.FindChild("Event_Finish").gameObject.SetActive(true);
				tr.FindChild("Event_Finish").FindChild("lb_Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73312");
				lbtime = tr.FindChild("Event_Finish").FindChild("lb_Time").GetComponent<UILabel>();
				tr.GetComponent<UIButtonMessage>().functionName = string.Empty;
			}
		}
	}


	public override void OnNext (GameObject obj)
	{
		if(Global.isPopUp) return;
		if(Global.isNetwork || Global.isAnimation) return;
		string str = "ING_"+GV.ChSeason.ToString();
		NGUITools.FindInParents<RaceMenuStart>(gameObject).OpenAniING();
		GameObject.Find("LobbyUI").SendMessage("OnChampionRaceClick",str,SendMessageOptions.DontRequireReceiver);
		
	}
	
	public override void OnNext1 (GameObject obj)
	{
		if(Global.isPopUp) return;
		if(Global.isNetwork || Global.isAnimation) return;
		NGUITools.FindInParents<RaceMenuStart>(gameObject).OpenAniING();
		GameObject.Find("LobbyUI").SendMessage("OnRegularRaceClick","Stock",SendMessageOptions.DontRequireReceiver);	
	}


	
	public override void OnNext2 (GameObject obj)
	{
		if(Global.isPopUp) return;
		if(Global.isNetwork || Global.isAnimation) return;
		//GV.RegularTrack = int.Parse(transform.name);
		NGUITools.FindInParents<RaceMenuStart>(gameObject).OpenEvent(2);
		
	}
}
