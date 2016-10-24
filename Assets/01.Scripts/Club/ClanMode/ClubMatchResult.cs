using UnityEngine;
using System.Collections;

public class ClubMatchResult : MonoBehaviour {

	void Start(){
		transform.FindChild("BtnInfo").FindChild("Title").GetComponent<UILabel>().text = 
			KoStorage.GetKorString("76008");
		transform.FindChild("BtnInfo").gameObject.SetActive(true);
	}


	void ClubResultInfo(ClubRaceResultInfo mClubResult){

		transform.FindChild("Title").GetComponent<UILabel>().text = "";
		var tr = transform.FindChild("Score") as Transform;
		var tr1 = tr.FindChild("MyTeam")  as Transform;
		var tr2 = tr.FindChild("OtherTeam") as Transform;

		tr1.FindChild("ClanName").GetComponent<UILabel>().text = CClub.mClubInfo.mClubName;
		tr1.FindChild("ClanSymbol").GetComponent<UISprite>().spriteName = CClub.mClubInfo.clubSymbol;

		tr2.FindChild("ClanName").GetComponent<UILabel>().text = mClubResult.oppclubName;
		tr2.FindChild("ClanSymbol").GetComponent<UISprite>().spriteName =mClubResult.oppclubsymbol;

		tr = tr.FindChild("StarScore");

		tr.FindChild("StarCount_1").GetComponent<UILabel>().text = mClubResult.myClubStarCount.ToString();
		tr.FindChild("UserCount_1").GetComponent<UILabel>().text = string.Format("{0}/{1}", mClubResult.myMemberNum, mClubResult.myMemberTotalNum);

		tr.FindChild("StarCount_2").GetComponent<UILabel>().text = mClubResult.oppClubStarCount.ToString();
		tr.FindChild("UserCount_2").GetComponent<UILabel>().text = string.Format("{0}/{1}",mClubResult.oppMemberNum, mClubResult.oppMemberTotalNum);

		tr = transform.FindChild("Result");
		Global.gNewMsg++;
		if(mClubResult.raceResult == 0){
		//	tr.FindChild("lbResult").GetComponent<UILabel>().text = "LOSE";
			tr.FindChild("img_defeat").gameObject.SetActive(true);
			tr.FindChild("img_win").gameObject.SetActive(false);
		}else{
		//	tr.FindChild("lbResult").GetComponent<UILabel>().text = "WIN";
			tr.FindChild("img_win").gameObject.SetActive(true);
			tr.FindChild("img_defeat").gameObject.SetActive(false);

		}

		tr = transform.FindChild("Reward");

		tr.FindChild("lbText1").GetComponent<UILabel>().text =KoStorage.GetKorString("72607");
		tr.FindChild("lbText2").GetComponent<UILabel>().text =KoStorage.GetKorString("72823");

		tr.FindChild("Ib_Title2").GetComponent<UILabel>().text = KoStorage.GetKorString("72832");
		tr.FindChild("Ib_Bonus").GetComponent<UILabel>().text = string.Format("{0:#,0}",mClubResult.winBonus);
		tr.FindChild("Ib_Title1").GetComponent<UILabel>().text =KoStorage.GetKorString("73012");
		tr.FindChild("Ib_Prize").GetComponent<UILabel>().text = string.Format("{0:#,0}",mClubResult.racePrize);
		tr.FindChild("lb_dollar_total").GetComponent<UILabel>().text = string.Format("{0:#,0}", mClubResult.winBonus+mClubResult.racePrize);
		myAcc.instance.account.bRaceMenuBTN[4]=false;
	}


	void OnClose(){
		CClub.ClubMode = 0;
		NGUITools.FindInParents<ClubMatch>(gameObject).ClubModeInitialize();
	}
}
