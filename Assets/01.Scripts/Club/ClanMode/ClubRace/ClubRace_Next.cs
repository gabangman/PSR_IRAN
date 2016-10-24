using UnityEngine;
using System.Collections;

public class ClubRace_Next : MonoBehaviour {

	public UILabel lbStar,lbOldStar;
	public GameObject LBTN, RBTN;
	public UILabel lbText1, lbText2;
	public UILabel lbEntryText, lbEntry, lbNext;
	public Transform ListView, Next;
	public GameObject SlotTeam;
	private int StarCount;
	private Transform starEffect;
	public UILabel lbDifficult;
	private System.DateTime cTimes;
	private readonly int starDollar = 1000;
	bool bMatching = false;
	 void Start(){
		lbText1.text = KoStorage.GetKorString("77118");
		lbText2.text =  KoStorage.GetKorString("77120");
		lbEntryText.text = KoStorage.GetKorString("73006");
		lbEntry.text=string.Format("{0:#,0}", StarCount*starDollar);
		if(starEffect == null){
			starEffect = LBTN.transform.parent.FindChild("Star") as Transform;
		}
		lbOldStar.transform.gameObject.SetActive(true);

	}

	void FixedUpdate(){
		if(bMatching) return;
		System.DateTime nNow = NetworkManager.instance.GetCurrentDeviceTime();
		System.TimeSpan mCompareTime =nNow - cTimes;
		if(mCompareTime.TotalSeconds >= CClub.mClubInfo.matchDurationSeconds){
			//GameObject.Find("LobbyUI").SendMessage("OnClanReturn");
			//GameObject.Find("LobbyUI").SendMessage("OnBackClick");
			CClub.ClubMode = 4;
			bMatching = true;
			return;
		}
		
	}


	public void InitRaceWinodw(){
		lbStar.text = string.Format("X{0}",1);
		int cnt = GV.listmyteaminfo.Count;
		int childcnt = ListView.GetChild (0).childCount;
		if(childcnt == 0){
			for(int i=0; i <cnt ;i++){
				var temp = NGUITools.AddChild(ListView.GetChild(0).gameObject, SlotTeam);
				temp.name = "ClubWarTeam_"+ GV.listmyteaminfo[i].TeamCode.ToString();
				temp.AddComponent<ClubWarTeamItem>().ViewClanWarTeamInfo(GV.listmyteaminfo[i].TeamCode);
			}
			ListView.GetChild(0).GetComponent<UIGrid>().Reposition();
		}else{
			if(cnt != childcnt){
				for(int i = childcnt; i <cnt; i++){
					var temp = NGUITools.AddChild(ListView.GetChild(0).gameObject, SlotTeam);
					temp.name = "ClubWarTeam_"+ GV.listmyteaminfo[i].TeamCode.ToString();
					temp.AddComponent<ClubWarTeamItem>();
				}
			}
			childcnt = ListView.GetChild (0).childCount;
			for(int i=0; i <childcnt ;i++){
				ListView.GetChild(0).GetChild(i).GetComponent<ClubWarTeamItem>().ViewClanWarTeamInfo(GV.listmyteaminfo[i].TeamCode);
			}
			ListView.GetChild(0).GetComponent<UIGrid>().Reposition();
		}
		cTimes =System.Convert.ToDateTime(CClub.mReady.matchingTime);
	}

	void OnNext(){
		if(bMatching) {
			NGUITools.FindInParents<ModeClan>(gameObject).SetBackClick();
			return;
		}
		Next.gameObject.SetActive(false);
		NGUITools.FindInParents<ModeClan>(gameObject).SetClubRaceStart(raceTeamId, StarCount);
	}

	void OnEnable(){
		lbStar.text = string.Format("X{0}",1);
		LBTN.SetActive(false);
		RBTN.SetActive(true);
		StarCount = 1;
		lbNext.text = string.Empty;
		Next.gameObject.SetActive(false);
		lbEntry.text=string.Format("{0:#}", StarCount*starDollar);
		GV.gEntryFee = StarCount*starDollar;
		lbDifficult.text = string.Empty;
		this.raceTeamId = 0;
		lbOldStar.text = string.Empty;
	}

	void StarEffectEnable(){
		TweenScale[] tw  = starEffect.GetComponents<TweenScale>();
		tw[0].Reset();
		tw[1].Reset();
		tw[0].enabled = true;
		tw[1].enabled =true;
	}

	void OnLeft(){
		if(StarCount == 1) return;
		StarCount--;
		if(!RBTN.activeSelf) RBTN.SetActive(true);
		if(StarCount == 1) LBTN.SetActive(false);
		lbStar.text = string.Format("X{0}",StarCount);
		lbEntry.text=string.Format("{0:#,0}", StarCount*starDollar);
		GV.gEntryFee = StarCount*starDollar;
		StarEffectEnable();
		SetDifficultDisplay(StarCount, this.raceTeamId); 
	}


	void OnRight(){
		if(StarCount == 30) return;
		StarCount++;
		if(!LBTN.activeSelf) LBTN.SetActive(true);
		if(StarCount==30) RBTN.SetActive(false);
		lbStar.text = string.Format("X{0}",StarCount);
		lbEntry.text=string.Format("{0:#,0}", StarCount*starDollar);
		GV.gEntryFee = StarCount*starDollar;
		StarEffectEnable();
		SetDifficultDisplay(StarCount, this.raceTeamId); 
	}

	void SetDifficultDisplay(int star, int teamID){
		if(teamID == 0){
			lbDifficult.text = string.Empty;
		}else{
			int carClass = GV.getTeamCarClassId(teamID);
			int clubID = StarCount+6099;
			Common_ClubAI.Item aiItem = 	Common_ClubAI.Get(clubID);
			int clubClass = GV.ConvertClassString(aiItem.Car_Class);
			int comID = carClass - clubClass;
			switch(comID){
			case -5: lbDifficult.color = new Color32(255,0,0,255); lbDifficult.text = KoStorage.GetKorString("13106"); break; //hard
			case -4: lbDifficult.color =new Color32(255,0,0,255); ; lbDifficult.text = KoStorage.GetKorString("13106"); break; //hard
			case -3: lbDifficult.color = new Color32(255,0,0,255);  lbDifficult.text = KoStorage.GetKorString("13106"); break;
			case -2: lbDifficult.color = new Color32(255,0,185,255); lbDifficult.text = KoStorage.GetKorString("13105"); break;
			case -1: lbDifficult.color = new Color32(113,13,255,255); lbDifficult.text = KoStorage.GetKorString("13104"); break;
			case 0: lbDifficult.color = new Color32(0,160,255,255); lbDifficult.text = KoStorage.GetKorString("13103"); break;
			case 1: lbDifficult.color = new Color32(40,255,0,255); lbDifficult.text = KoStorage.GetKorString("13102"); break;
			case 2: lbDifficult.color = new Color32(255,248,0,255); lbDifficult.text = KoStorage.GetKorString("13101"); break; //easy
			case 3: lbDifficult.color =new Color32(255,248,0,255); lbDifficult.text = KoStorage.GetKorString("13101"); break; // easy
			case 4: lbDifficult.color = new Color32(255,248,0,255); lbDifficult.text = KoStorage.GetKorString("13101"); break; //easy 
			case 5: lbDifficult.color =new Color32(255,248,0,255); lbDifficult.text = KoStorage.GetKorString("13101"); break; //easy
			default : //==!!Utility.LogWarning("comID " + comID);
				break;//113,13,255,255
			}
		}
	}


	private int raceTeamId;
	public void SetSelectInfo(string str, int vIdx, int currentTeamID){
		var tr = ListView.GetChild(0) as Transform;
		SetHistoryRace(currentTeamID);
		for(int i = 0; i < tr.childCount;i++){
			tr.GetChild(i).GetComponent<ClubWarTeamItem>().UnSelectSelectInfo();
		}

		if(vIdx == -1){
			lbNext.text = KoStorage.GetKorString("77119");
			Next.gameObject.SetActive(false);
			SetDifficultDisplay(StarCount, 0);
		} else if(vIdx == 0){
			lbNext.text =  KoStorage.GetKorString("77119");
			Next.gameObject.SetActive(false);
			SetDifficultDisplay(StarCount, 0);
		}else{
			lbNext.text = string.Empty;
			Next.gameObject.SetActive(true);
			this.raceTeamId = vIdx;
			SetDifficultDisplay(StarCount, this.raceTeamId);
		}

	}

	void SetHistoryRace(int teamID){
		if(teamID == 0){
			lbOldStar.text = string.Empty;
		}else{
			string strTeamID = teamID.ToString();
			if(EncryptedPlayerPrefs.HasKey(strTeamID)){
				lbOldStar.text = string.Format("BEST RECORD : {0}",EncryptedPlayerPrefs.GetInt(strTeamID).ToString());
			}else{
				lbOldStar.text = string.Empty;
			}
		}
	}
}

