using UnityEngine;
using System.Collections;

public class ClanMatchNo : MonoBehaviour {
	public GameObject btnClan, btnMatch, btnWait;
	public GameObject matchPopup;
	public UILabel lbtime;

	void OnEnable(){
	//	Utility.Log(Global.matchingTime);
	//	if(Global.matchingTime == 0)
	//		 isMatching =false;
	//	else isMatching = true;
	}

	void OnGoClan(){
		GameObject.Find("LobbyUI").SendMessage("OnClanReturn");
		
	}

	void OnGoMatch(){
		matchPopup.SetActive(true);
		NGUITools.FindInParents<TweenAction>(gameObject).doubleTweenScale(matchPopup);
		matchPopup.GetComponent<ClanMatchSys>().OnMatching((b)=>{
			if(b){
				btnMatch.SetActive(false);
				btnWait.SetActive(true);
				//isMatching = true;
				Global.matchedTime =NetworkManager.instance.GetCurrentDeviceTime();//  System.DateTime.Now;
				CClub.ClubMatchFlag = 1;
			}else {
			//	lbtime.text = System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((_totalTime/60f)) ,_totalTime%60.0f);
			}
		});
	}

	void initialContentWindow(){
	
	}
//	bool isMatching = false;
//	System.DateTime matchedTime;
//	System.TimeSpan PassedTime;
//	void Update(){
//		if(Global.matchingFlag == 0 ) return;
//		waitTimeCheck();

//	}

//	void waitTimeCheck(){
//		PassedTime = System.DateTime.Now - Global.matchedTime;
//		lbtime.text = System.String.Format("{0:00}:{1:00}:{2:00}", PassedTime.Hours, PassedTime.Minutes, PassedTime.Seconds);
//	}

}
