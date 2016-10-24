using UnityEngine;
using System.Collections;

public class ViewHistoryItem : SlotItems {

	public void ViewHistoryContent(int idx){
	}

	public override void ChangeContents (int idx)
	{
		int listCount = CClub.mHistoryList.Count;
		if(listCount <= idx){
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}


		ClubInfoHistory mHis = CClub.mHistoryList[idx];
		var tr = transform.FindChild("Graph") as Transform;
		ChangeStarGraph(tr, mHis.myClubStarCount, mHis.oppStarCount);

		tr = transform.FindChild("Info");
		tr.FindChild("lb_StarScore_M").GetComponent<UILabel>().text = mHis.myClubStarCount.ToString();
		tr.FindChild("lb_StarScore_O").GetComponent<UILabel>().text = mHis.oppStarCount.ToString();
		tr.FindChild("lb_RaceMember_O").GetComponent<UILabel>().text = mHis.oppVictoryNum.ToString();
		tr.FindChild("lb_Time").GetComponent<UILabel>().text = mHis.raceDateTime;
		tr.FindChild("lbName").GetComponent<UILabel>().text = mHis.oppClubName;
		tr.FindChild("ClanSymbol").GetComponent<UISprite>().spriteName = mHis.oppClubSymbol;
		if(mHis.resultRace == 0){
			transform.FindChild("Result_Win").gameObject.SetActive(false);
			transform.FindChild("Result_Defeat").gameObject.SetActive(true);
		}else{
			transform.FindChild("Result_Win").gameObject.SetActive(true);
			transform.FindChild("Result_Defeat").gameObject.SetActive(false);
		}

	}

	void ChangeStarGraph(Transform tr, int mStar, int oStar){
		var tr1 = tr.FindChild("Bar") as Transform;
		var tr2 = tr.FindChild("Bar_Bg") as Transform;
		Vector3 msize = tr1.localScale;

		float total = (float)mStar+(float)oStar;
		float per = 0.0f;float z =0;
		if(oStar == 0){
			tr1.gameObject.SetActive(false);
		}else{
			tr1.gameObject.SetActive(true);
			per = (float)oStar / total;
			z = 438.0f * per;
			tr1.localScale = new Vector3(z, msize.y, msize.z);
		
		}



	}
}
