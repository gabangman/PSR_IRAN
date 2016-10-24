using UnityEngine;
using System.Collections;

public class RankSubMode : RaceMode {


	public override void OnNext (GameObject obj)
	{if(Global.isPopUp) return;
		base.OnNext (obj);
		GameObject.Find("LobbyUI").SendMessage("OnRankRaceClick",obj.name,SendMessageOptions.DontRequireReceiver);
	
	}
	public override void SetRaceSubMode(){
		
		var tr = transform.GetChild(0).FindChild("mapinfo") as Transform;
		var tr1 = transform.GetChild(0).FindChild("WS_Slot") as Transform;
		if(isSet) return;
		isSet = true;
		SetRankInfo(tr,tr1);
	}
	void SetRankInfo(Transform Tr, Transform Tr1){
		var t = transform.GetChild(0).gameObject as GameObject;
		if(!t.activeSelf) return;
		UILabel[] label = Tr.GetComponentsInChildren<UILabel>();
		Common_Track.Item tItem = Common_Track.Get(1400);
	//	label[0].text = KoStorage.GetKorString("72203");
	//	label[1].text = tItem.Name;//string.Format( KoStorage.GetKorString("73143"), tItem.Distance);
	//	label[2].text =  string.Format( KoStorage.GetKorString("73143"), tItem.Distance);
	//	label[3].text =  string.Format( KoStorage.GetKorString("73608"), 6);

		label[0].text = KoStorage.GetKorString("72203");
		label[1].text =string.Format( KoStorage.GetKorString("73143"), tItem.Distance);
		Tr1.FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73614");
		//Utility.LogWarning( KoStorage.GetKorString("72203"));
	//	for(int i =0; i < label.Length; i++){
	//		//!!--Utility.Log("label " + label[i].transform.name);
	//	}
		/*var mTr = Tr1.GetChild(0) as Transform;
		Common_Attend.Item item = Common_Attend.Get(8718);
		mTr.FindChild("lbRank").GetComponent<UILabel>().text = string.Format("{0} {1}", item.Target, KoStorage.GetKorString("72809"));
		mTr.FindChild("lbPrice").GetComponent<UILabel>().text = item.R_no.ToString();

		item = Common_Attend.Get(8719);
		mTr = Tr1.GetChild(1) as Transform;
		mTr.FindChild("lbRank").GetComponent<UILabel>().text = string.Format("{0} {1}", item.Target, KoStorage.GetKorString("72809"));
		mTr.FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();
		item = Common_Attend.Get(8720);
		mTr = Tr1.GetChild(2) as Transform;
		mTr.FindChild("lbRank").GetComponent<UILabel>().text = string.Format("{0} {1}", item.Target, KoStorage.GetKorString("72809"));
		mTr.FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();
		item = Common_Attend.Get(8721);
		mTr = Tr1.GetChild(3) as Transform;
		mTr.FindChild("lbRank").GetComponent<UILabel>().text = string.Format("~{0} {1}", item.Target, KoStorage.GetKorString("72809"));
		mTr.FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();
		item = Common_Attend.Get(8722);
		mTr = Tr1.GetChild(4) as Transform;
		mTr.FindChild("lbRank").GetComponent<UILabel>().text = string.Format("~{0} {1}", item.Target, KoStorage.GetKorString("72809"));
		mTr.FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();
		item = Common_Attend.Get(8723);
		mTr = Tr1.GetChild(5) as Transform;
		mTr.FindChild("lbRank").GetComponent<UILabel>().text = string.Format("~{0} {1}", item.Target, KoStorage.GetKorString("72809"));
		mTr.FindChild("lbPrice").GetComponent<UILabel>().text = item.R_no.ToString();*/

		}

	void Start(){
		if(AccountManager.instance.ChampItem.S2_1_Ranking == 0){
			transform.FindChild("Mode").gameObject.SetActive(false);
			transform.FindChild("Locked").gameObject.SetActive(true);
			transform.FindChild("Locked").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("73045");
		}else{
		
			var tr = transform.FindChild("Mode") as Transform;
			tr = tr.FindChild("WS_Slot") as Transform;
			int cnt = myAccount.instance.account.weeklyRace.GTrophy;
			int rewardCnt = 0;
	//		cnt = 101;
		/*	if(cnt < 50){
				if(myAccount.instance.account.weeklyRace.Trophy_Reset_Times){
					for(int i=0; i < tr.childCount-1; i++){
						tr.GetChild(i).FindChild("icon_reward").gameObject.SetActive(true);
					}
				}else{
					
				}
				return;
			}else if(cnt < 100){
				rewardCnt = 5;
			}else if(cnt < 200){
				rewardCnt = 4;
			}else if(cnt < 300){
				rewardCnt = 3;
			}else if(cnt < 400){
				rewardCnt = 2;
			}else if(cnt < 500){
				rewardCnt = 1;
			}
			for(int i=  tr.childCount-2; i >= 0 ; i--){
				if(i >=rewardCnt) tr.GetChild(i).FindChild("icon_reward").gameObject.SetActive(true);
			}*/

		
			if(cnt < 2){
				if(myAccount.instance.account.weeklyRace.Trophy_Reset_Times){
					for(int i=0; i < tr.childCount-1; i++){
						tr.GetChild(i).FindChild("icon_reward").gameObject.SetActive(true);
					}
				}else{
					
				}
				return;
			}else if(cnt < 4){
				rewardCnt = 5;
			}else if(cnt < 6){
				rewardCnt = 4;
			}else if(cnt < 8){
				rewardCnt = 3;
			}else if(cnt < 10){
				rewardCnt = 2;
			}else if(cnt < 12){
				rewardCnt = 1;
			}
			for(int i=  tr.childCount-2; i >= 0 ; i--){
				if(i >=rewardCnt) tr.GetChild(i).FindChild("icon_reward").gameObject.SetActive(true);
			}
		
		}

	}
}

public partial class LobbyManager : MonoBehaviour {
	void OnRankRaceClick(string str){
		if(btnstate != buttonState.WAIT) return;
		btnstate = buttonState.MAP_RANK;
		fadeIn();
		strMode = "Weekly";
	//	Utility.Log("str" + str);
		OnBackFunction = ()=>{
			isModeReturn = false;
			fadeIn();
			btnstate = buttonState.MAPTORACEMODE;
			unSetRaceSubWindow();
		};
	}
}


