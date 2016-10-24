using UnityEngine;
using System.Collections;

public class RankingItems : MonoBehaviour {
	
	private Transform mTr;
	bool bClick = false;
	
	void OnNextWorldByThrophy(){
		if(bClick) return;
		bClick = true;
		NGUITools.FindInParents<worldrankwin>(gameObject).addNextThrophyRank();
	}
	
	void OnNextWorldByTime(){
		if(bClick) return;
		bClick = true;
		NGUITools.FindInParents<worldrankwin>(gameObject).addNextTimeRank();
	}
	
	void OnNextWeeklyByTime(){
		if(bClick) return;
		bClick = true;
		NGUITools.FindInParents<weekrankwin>(gameObject).addNextTimeRank();
	}
	
	void OnNextWeeklyByThrophy(){
		if(bClick) return;
		bClick = true;
		NGUITools.FindInParents<weekrankwin>(gameObject).addNextThrophyRank();
	}
	
	public void ChangeRankingWeeklyByTime(int idx){
		int listCount = gameRank.instance.listRWR.Count;
		//	Utility.LogWarning(idx);
		/*	if(listCount < idx){
			gameObject.SetActive(false);
			return;
		}else if(listCount == idx){
		//	Utility.LogWarning("listcount " + listCount);
		//	Utility.LogWarning("listcounts " + idx);
			gameObject.SetActive(true);
		}else{
			gameObject.SetActive(true);
		} */
		
		if(listCount <= idx || listCount == 0){
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}
		
		
		gameRank.RaceRankInfo mRank =  gameRank.instance.listRWR[idx];
		if(mRank.addPage == 1){
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(true);
			mTr =  transform.GetChild(1) as Transform;
			mTr.GetComponent<UIButtonMessage>().functionName = "OnNextWeeklyByTime";
			bClick = false;
			return;
		}else{
			transform.GetChild(0).gameObject.SetActive(true);
			transform.GetChild(1).gameObject.SetActive(false);
		}
		
		mTr = transform.GetChild(0) as Transform;
		if(mRank.mRaceUserInfo.bUser == true){
			ChangeContentSlot0(mRank.mRaceUserInfo.userNick, mRank.mRaceUserInfo.userLevel);
			ChangeContentSlot1(mRank.carId, mRank.mRaceUserInfo.crewId, mRank.mRaceUserInfo.carAbility, mRank.mRaceUserInfo.crewAbility);
			ChangeContentSlot2(int.Parse(mRank.trophy1), int.Parse(mRank.trophy2),int.Parse(mRank.trophy3));
			if(mRank.record <1.0f){
				ChangeContentSlot3(mRank.mRaceUserInfo.raceTime, idx+1);
			}else{
				ChangeContentSlot3(mRank.record, idx+1);
			}
			if(mRank.mRaceUserInfo.clubLv == 0){
				ChangeContentSlot4(0);
			}else{
				ChangeContentSlot4(mRank.mRaceUserInfo.clubLv, mRank.mRaceUserInfo.clubName,mRank.mRaceUserInfo.clubSymbol);
			}
			
		}else{
			ChangeContentSlot0(string.Empty, 0);
			ChangeContentSlot1(mRank.carId, Common_Team.Get(mRank.teamId).Crew,0, 0);
			ChangeContentSlot2(int.Parse(mRank.trophy1), int.Parse(mRank.trophy2),int.Parse(mRank.trophy3));
			if(mRank.record <1.0f){
				ChangeContentSlot3(10.0f, idx+1);
			}else{
				ChangeContentSlot3(mRank.record, idx+1);
			}
			if(mRank.mRaceUserInfo.clubLv == 0){
				ChangeContentSlot4(0);
			}else{
				ChangeContentSlot4(mRank.mRaceUserInfo.clubLv, mRank.mRaceUserInfo.clubName,mRank.mRaceUserInfo.clubSymbol);
			}
		}
		
		CheckMyRanking(Global.gMyRank[2],idx);
		CheckUserProfileLoad(mRank.mRaceUserInfo);
	}
	
	
	void CheckMyRanking(int Ranking, int idx){
		if(Ranking == (idx+1)){
			mTr.FindChild("BG").gameObject.SetActive(false);
			mTr.FindChild("BG_me").gameObject.SetActive(true);
		}else{
			mTr.FindChild("BG").gameObject.SetActive(true);
			mTr.FindChild("BG_me").gameObject.SetActive(false);
		}
	}
	
	void checkmyTrophyRanking(string id){
		
	}
	
	void ChangeContentSlot0(string nick, int lv){
		var tr = mTr.FindChild("kakao") as Transform;
		if(lv == 0){
			tr.FindChild("lbLevel").GetComponent<UILabel>().text = "00";
		}else{
			tr.FindChild("lbLevel").GetComponent<UILabel>().text = lv.ToString();
		}
		tr.FindChild("lbNick").GetComponent<UILabel>().text  = nick;
	}
	
	void ChangeContentSlot1(int carid, int crewId, int carAbility, int crewAbility){
		if(carAbility == 0){
			mTr.FindChild("icon_car").GetComponent<UISprite>().spriteName = "1000";
			mTr.FindChild("icon_crew").GetComponent<UISprite>().spriteName = "1200A";
			mTr.FindChild("lb_Car").GetComponent<UILabel>().text = string.Empty;
			mTr.FindChild("lb_Crew").GetComponent<UILabel>().text = string.Empty;
		}else{
			mTr.FindChild("icon_car").GetComponent<UISprite>().spriteName = carid.ToString();
			mTr.FindChild("icon_crew").GetComponent<UISprite>().spriteName = crewId.ToString()+"A";
			mTr.FindChild("lb_Car").GetComponent<UILabel>().text = carAbility.ToString();
			mTr.FindChild("lb_Crew").GetComponent<UILabel>().text = crewAbility.ToString();
		}
	}
	void ChangeContentSlot2(int gt, int st, int bt){
		var tr = mTr.FindChild("throphy") as Transform;
		tr.FindChild("lbthrophy_1").GetComponent<UILabel>().text = string.Format("{0}",gt);
		tr.FindChild("lbthrophy_2").GetComponent<UILabel>().text = string.Format("{0}",st);
		tr.FindChild("lbthrophy_3").GetComponent<UILabel>().text = string.Format("{0}",bt);
	}
	
	void ChangeContentSlot3(float fTime, int rank){
		var tr = mTr.FindChild("rank") as Transform;
		if(rank == 0){
			tr.FindChild("icon_rank").GetComponent<UISprite>().spriteName = "Playerifo_rank1";
		}else if(rank == 1){
			tr.FindChild("icon_rank").GetComponent<UISprite>().spriteName = "Playerifo_rank2";
		}else if(rank == 2){
			tr.FindChild("icon_rank").GetComponent<UISprite>().spriteName = "Playerifo_rank3";
		}else {
			tr.FindChild("icon_rank").GetComponent<UISprite>().spriteName = "Playerifo_rank4";
		}
		tr.FindChild("lbrankNum").GetComponent<UILabel>().text = string.Format("{0}", rank);
		tr.FindChild("lbTime").GetComponent<UILabel>().text = string.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f);
	}
	
	void ChangeContentSlot4(int id, string clubname = null, string clubmark=null){
		if(id == 0){
			mTr.FindChild("Club").gameObject.SetActive(false);
		}else{
			mTr.FindChild("Club").gameObject.SetActive(true);
			mTr.FindChild("Club").FindChild("lb_Club").GetComponent<UILabel>().text =clubname;
			mTr.FindChild("Club").FindChild("icon_Club").GetComponent<UISprite>().spriteName =clubmark;
		}
	}
	
	void CheckUserProfileLoad(RaceUserInfo mUser){
		Texture2D mTex = mUser.userProfile;
		StopCoroutine("rankUserPictureLoad");
		if(mTex == null){
			StartCoroutine("rankUserPictureLoad",mUser);
			
		}else{
			var tr = mTr.FindChild("kakao") as Transform;
			tr.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = mTex;
		}
		//StartCoroutine("rankUserPictureLoad", mUser.strProfileUrl);
	}
	
	public void OnNoChangeRanking(int idx){
		gameObject.SetActive(false);
	}
	
	public void ChangeRankingWeeklyByThrophy(int idx){
		int listCount = gameRank.instance.listRWT.Count;
		
		if(listCount <= idx || listCount == 0){
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}
		
		gameRank.RaceRankInfo mRank =  gameRank.instance.listRWT[idx];
		if(mRank.addPage == 1){
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(true);
			mTr =  transform.GetChild(1) as Transform;
			mTr.GetComponent<UIButtonMessage>().functionName = "OnNextWeeklyByThrophy";
			bClick = false;
			return;
		}else{
			transform.GetChild(0).gameObject.SetActive(true);
			transform.GetChild(1).gameObject.SetActive(false);
		}
		mTr = transform.GetChild(0) as Transform;
		
		if(mRank.mRaceUserInfo.bUser == true){
			ChangeContentSlot0(mRank.mRaceUserInfo.userNick, mRank.mRaceUserInfo.userLevel);
			ChangeContentSlot1(mRank.carId, mRank.mRaceUserInfo.crewId, mRank.mRaceUserInfo.carAbility, mRank.mRaceUserInfo.crewAbility);
			ChangeContentSlot2(int.Parse(mRank.trophy1), int.Parse(mRank.trophy2),int.Parse(mRank.trophy3));
			if(mRank.record <1.0f){
				ChangeContentSlot3(mRank.mRaceUserInfo.raceTime, idx+1);
			}else{
				ChangeContentSlot3(mRank.record, idx+1);
			}
			if(mRank.mRaceUserInfo.clubLv == 0){
				ChangeContentSlot4(0);
			}else{
				ChangeContentSlot4(mRank.mRaceUserInfo.clubLv, mRank.mRaceUserInfo.clubName,mRank.mRaceUserInfo.clubSymbol);
			}
		}else{
			ChangeContentSlot0(string.Empty, 0);
			ChangeContentSlot1(mRank.carId, Common_Team.Get(mRank.teamId).Crew,0, 0);
			ChangeContentSlot2(int.Parse(mRank.trophy1), int.Parse(mRank.trophy2),int.Parse(mRank.trophy3));
			if(mRank.record <1.0f){
				ChangeContentSlot3(10.0f, idx+1);
			}else{
				ChangeContentSlot3(mRank.record, idx+1);
			}
			if(mRank.mRaceUserInfo.clubLv == 0){
				ChangeContentSlot4(0);
			}else{
				ChangeContentSlot4(mRank.mRaceUserInfo.clubLv, mRank.mRaceUserInfo.clubName,mRank.mRaceUserInfo.clubSymbol);
			}
		}
		CheckMyRanking(Global.gMyRank[1],idx);
		CheckUserProfileLoad(mRank.mRaceUserInfo);
	}
	
	public void ChangeRankingWorldByTime(int idx){
		int listCount = gameRank.instance.listRTR.Count;
		//	Utility.LogWarning(listCount);
		if(listCount <= idx || listCount == 0){
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}
		
		gameRank.RaceRankInfo mRank =  gameRank.instance.listRTR[idx];
		if(mRank.addPage == 1){
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(true);
			mTr =  transform.GetChild(1) as Transform;
			mTr.GetComponent<UIButtonMessage>().functionName = "OnNextWorldByTime";
			bClick = false;
			return;
		}else{
			transform.GetChild(0).gameObject.SetActive(true);
			transform.GetChild(1).gameObject.SetActive(false);
		}
		mTr = transform.GetChild(0) as Transform;
		
		if(mRank.mRaceUserInfo.bUser == true){
			ChangeContentSlot0(mRank.mRaceUserInfo.userNick, mRank.mRaceUserInfo.userLevel);
			ChangeContentSlot1(mRank.carId, mRank.mRaceUserInfo.crewId, mRank.mRaceUserInfo.carAbility, mRank.mRaceUserInfo.crewAbility);
			ChangeContentSlot2(int.Parse(mRank.trophy1), int.Parse(mRank.trophy2),int.Parse(mRank.trophy3));
			if(mRank.record <1.0f){
				ChangeContentSlot3(mRank.mRaceUserInfo.raceTime, idx+1);
			}else{
				ChangeContentSlot3(mRank.record, idx+1);
			}
			if(mRank.mRaceUserInfo.clubLv == 0){
				ChangeContentSlot4(0);
			}else{
				ChangeContentSlot4(mRank.mRaceUserInfo.clubLv, mRank.mRaceUserInfo.clubName,mRank.mRaceUserInfo.clubSymbol);
			}
		}else{
			ChangeContentSlot0(string.Empty, 0);
			ChangeContentSlot1(mRank.carId, Common_Team.Get(mRank.teamId).Crew,0, 0);
			ChangeContentSlot2(int.Parse(mRank.trophy1), int.Parse(mRank.trophy2),int.Parse(mRank.trophy3));
			if(mRank.record <1.0f){
				ChangeContentSlot3(10.0f, idx+1);
			}else{
				ChangeContentSlot3(mRank.record, idx+1);
			}
			if(mRank.mRaceUserInfo.clubLv == 0){
				ChangeContentSlot4(0);
			}else{
				ChangeContentSlot4(mRank.mRaceUserInfo.clubLv, mRank.mRaceUserInfo.clubName,mRank.mRaceUserInfo.clubSymbol);
			}
		}
		CheckMyRanking(Global.gMyRank[4],idx);CheckUserProfileLoad(mRank.mRaceUserInfo);
	}
	
	public void ChangeRankingWorldByThrophy(int idx){
		int listCount = gameRank.instance.listRTT.Count;
		//Utility.LogWarning(listCount + " idx " + idx);
		if(listCount <= idx || listCount == 0){
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}
		
		gameRank.RaceRankInfo mRank =  gameRank.instance.listRTT[idx];
		if(mRank.addPage == 1){
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(true);
			mTr =  transform.GetChild(1) as Transform;
			mTr.GetComponent<UIButtonMessage>().functionName = "OnNextWorldByThrophy";
			bClick = false;
			return;
		}else{
			transform.GetChild(0).gameObject.SetActive(true);
			transform.GetChild(1).gameObject.SetActive(false);
		}
		mTr = transform.GetChild(0) as Transform;
		
		if(mRank.mRaceUserInfo.bUser == true){
			ChangeContentSlot0(mRank.mRaceUserInfo.userNick, mRank.mRaceUserInfo.userLevel);
			ChangeContentSlot1(mRank.carId, mRank.mRaceUserInfo.crewId, mRank.mRaceUserInfo.carAbility, mRank.mRaceUserInfo.crewAbility);
			ChangeContentSlot2(int.Parse(mRank.trophy1), int.Parse(mRank.trophy2),int.Parse(mRank.trophy3));
			if(mRank.record <1.0f){
				ChangeContentSlot3(mRank.mRaceUserInfo.raceTime, idx+1);
			}else{
				ChangeContentSlot3(mRank.record, idx+1);
			}
			if(mRank.mRaceUserInfo.clubLv == 0){
				ChangeContentSlot4(0);
			}else{
				ChangeContentSlot4(mRank.mRaceUserInfo.clubLv, mRank.mRaceUserInfo.clubName,mRank.mRaceUserInfo.clubSymbol);
			}
		}else{
			ChangeContentSlot0(string.Empty,0);
			ChangeContentSlot1(mRank.carId, Common_Team.Get(mRank.teamId).Crew,0, 0);
			ChangeContentSlot2(int.Parse(mRank.trophy1), int.Parse(mRank.trophy2),int.Parse(mRank.trophy3));
			if(mRank.record <1.0f){
				ChangeContentSlot3(10.0f, idx+1);
			}else{
				ChangeContentSlot3(mRank.record, idx+1);
			}
			if(mRank.mRaceUserInfo.clubLv == 0){
				ChangeContentSlot4(0);
			}else{
				ChangeContentSlot4(mRank.mRaceUserInfo.clubLv, mRank.mRaceUserInfo.clubName,mRank.mRaceUserInfo.clubSymbol);
			}
		}
		CheckMyRanking(Global.gMyRank[3],idx);CheckUserProfileLoad(mRank.mRaceUserInfo);
	}
	
	// 0 : 친구랭킹
	// 1 : weeklyRankByThrophy
	// 2 : weeeklyRankByTime
	// 3 : totalRankByThrophy
	// 4 : totalRankByTime
	
	
	IEnumerator rankUserPictureLoad(RaceUserInfo mUser){
		string url = mUser.userURL;
		bool bHas = false;
		if(string.IsNullOrEmpty(url)== true){
			bHas = true;
		}else if(!url.Contains("http")){
			bHas = true;
		}
		if(bHas){
			var temp1 = mTr.FindChild("kakao") as Transform;
			mUser.userProfile = (Texture2D)(Texture)Resources.Load("User_Default", typeof(Texture));
			temp1.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = mUser.userProfile;
			yield break;
		}
		WWW www = new WWW( url );
		yield return www;
		if( this == null )
			yield break;
		if( www.error != null )
		{
			//Utility.Log( "load failed" );
		}
		else
		{
			Texture2D pic = www.texture;
			var temp = mTr.FindChild("kakao") as Transform;
			temp.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = pic;
			mUser.userProfile = pic;
			www.Dispose();
		}
	}
}
