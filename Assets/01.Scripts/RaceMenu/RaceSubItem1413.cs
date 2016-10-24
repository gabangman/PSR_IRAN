using UnityEngine;
using System.Collections;

public class RaceSubItem1413 : RaceSubItems {

	void FixedUpdate(){
		
		
	}
	
	
	public void OnMap1413(){


		if(GV.ChSeasonID >= 6010){
			var tr = mapInfo.transform.FindChild("Info") as Transform;
			tr.gameObject.SetActive(true);
			tr.FindChild("Effect").gameObject.SetActive(true);
			Common_Track.Item item = Common_Track.Get(1413);
			tr.FindChild("lbName").GetComponent<UILabel>().text = item.Name;
			tr.FindChild("lbMile_1").GetComponent<UILabel>().text = string.Format( KoStorage.GetKorString("73143"), item.Distance);
		}else{
			bLock = true;
			mapInfo.transform.FindChild("Locked").gameObject.SetActive(true);
			mapInfo.transform.FindChild("Locked").FindChild("Text1").GetComponent<UILabel>().text =  KoStorage.GetKorString("73046");//
			return;
		}
		//PVP

		var tr1 = modeInfo.transform.FindChild("PVP").GetChild(0) as Transform;

		if(AccountManager.instance.ChampItem.S3_3_PVP_City == 0){
			tr1.GetComponent<UIButtonMessage>().functionName = null;
			tr1.FindChild("Mode_On").gameObject.SetActive(false);
			tr1.FindChild("Locked").gameObject.SetActive(true);
			tr1.FindChild("Locked").FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73044");
		}else{
			tr1.GetComponent<UIButtonMessage>().functionName = "OnNext";
			tr1.FindChild("Mode_On").gameObject.SetActive(true);
			tr1.FindChild("Mode_On").FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73407");
			Common_Reward.Item item = Common_Reward.Get(Global.gRewardId);
			tr1.FindChild("Mode_On").FindChild("Reward").FindChild("Quantity").GetComponent<UILabel>().text =string.Format(KoStorage.GetKorString("73406"), item.Reward_mat_timesquare);
		}


		//club
	


	//ClubMode = 0; // 0: no match, 1: 레이스 진행중 2. 레이스 완료 3. 매칭 중 
	//	public static int ClanMember = 0; // 0 : no member , 1 : clanMaseter, 2 : clanStaff, 3: ClanMember
	//	public static int ClubMatchFlag = 0; //0 : No wait, 1 : waiting
	//	public static int mClubFlag = 0; // 0 : no season, 1 : seaon ok but no clanmember, 2 : season OK & clanmember



	}

	public void ClubInit(){
		var tr2 = modeInfo.transform.FindChild("Club").GetChild(0) as Transform;
		tr2.FindChild("Mode_On").gameObject.SetActive(true);
		tr2= tr2.FindChild("Mode_On");
		if(CClub.mClubFlag == 1 || CClub.mClubFlag == 0){
			tr2.FindChild("NoClub").gameObject.SetActive(true);
			tr2.FindChild("Text1").GetComponent<UILabel>().text =KoStorage.GetKorString("71229");
			return;
		}



	
		if(CClub.ClanMember == 0){
			tr2.FindChild("NoClub").gameObject.SetActive(true);
			tr2.FindChild("Text1").GetComponent<UILabel>().text =KoStorage.GetKorString("71229");

			//tr2.FindChild("NoClub").gameObject.SetActive(false);
			tr2.FindChild("InClub").gameObject.SetActive(false);
			tr2.FindChild("ClubMatchComplete").gameObject.SetActive(false);
			tr2.FindChild("ClubMatchFinish").gameObject.SetActive(false);
			tr2.FindChild("ClubMatchWait").gameObject.SetActive(false);


		}else{
			if(CClub.ClubMode == 0){
				tr2.FindChild("InClub").gameObject.SetActive(true);
				tr2.FindChild("Text1").GetComponent<UILabel>().text =KoStorage.GetKorString("73501");
				tr2.FindChild("NoClub").gameObject.SetActive(false);
				//tr2.FindChild("InClub").gameObject.SetActive(false);
				tr2.FindChild("ClubMatchComplete").gameObject.SetActive(false);
				tr2.FindChild("ClubMatchFinish").gameObject.SetActive(false);
				tr2.FindChild("ClubMatchWait").gameObject.SetActive(false);
			}else if(CClub.ClubMode == 1){
				tr2.FindChild("ClubMatchComplete").gameObject.SetActive(true);
				tr2.FindChild("Text1").GetComponent<UILabel>().text =KoStorage.GetKorString("73523");
				tr2.FindChild("NoClub").gameObject.SetActive(false);
				tr2.FindChild("InClub").gameObject.SetActive(false);
				//tr2.FindChild("ClubMatchComplete").gameObject.SetActive(false);
				tr2.FindChild("ClubMatchFinish").gameObject.SetActive(false);
				tr2.FindChild("ClubMatchWait").gameObject.SetActive(false);
			}else if(CClub.ClubMode == 2){
				tr2.FindChild("ClubMatchFinish").gameObject.SetActive(true);
				tr2.FindChild("Text1").GetComponent<UILabel>().text =KoStorage.GetKorString("73524");
				tr2.FindChild("NoClub").gameObject.SetActive(false);
				tr2.FindChild("InClub").gameObject.SetActive(false);
				tr2.FindChild("ClubMatchComplete").gameObject.SetActive(false);
				//tr2.FindChild("ClubMatchFinish").gameObject.SetActive(false);
				tr2.FindChild("ClubMatchWait").gameObject.SetActive(false);
			}else if(CClub.ClubMode ==  3){
				tr2.FindChild("ClubMatchWait").gameObject.SetActive(true);
				tr2.FindChild("Text1").GetComponent<UILabel>().text =KoStorage.GetKorString("77117");
				tr2.FindChild("ClubMatchFinish").gameObject.SetActive(true);
				tr2.FindChild("Text1").GetComponent<UILabel>().text =KoStorage.GetKorString("73524");
				tr2.FindChild("NoClub").gameObject.SetActive(false);
				tr2.FindChild("InClub").gameObject.SetActive(false);
				tr2.FindChild("ClubMatchComplete").gameObject.SetActive(false);
				tr2.FindChild("ClubMatchFinish").gameObject.SetActive(false);
				//tr2.FindChild("ClubMatchWait").gameObject.SetActive(false);
			}
		}
	
	}

	public override void OnNext1 (GameObject obj)
	{
		if(Global.isPopUp) return;
		if(Global.isNetwork || Global.isAnimation) return;
		//GameObject.Find("LobbyUI").SendMessage("OnRegularRaceClick","Drag",SendMessageOptions.DontRequireReceiver);	
		//NGUITools.FindInParents<RaceMenuStart>(gameObject).OpenAniING();
		NGUITools.FindInParents<RaceMenuStart>(gameObject).OpenClubMode();
	}
	
	public override void OnNext (GameObject obj)
	{
		if(Global.isPopUp) return;
		if(Global.isNetwork || Global.isAnimation) return;
		NGUITools.FindInParents<RaceMenuStart>(gameObject).OpenAniING();
		GameObject.Find("LobbyUI").SendMessage("OnPVPRaceClick","City",SendMessageOptions.DontRequireReceiver);
	}


}
