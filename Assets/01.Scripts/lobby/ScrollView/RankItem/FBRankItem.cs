using UnityEngine;
using System.Collections;
using System;
public class FBRankItem : MonoBehaviour {
	//  친구 순위
	
	public void DefaultChangeFBRankContent(int idx){
		gameObject.SetActive(true);
		return;
	}
	
	public void ChangeFBRank(){
		string name = transform.name;
		int idx = 0;
		string[] strs = name.Split('_');
		try{
			int.TryParse(strs[1], out idx);
			ChangeFBRankContent(idx);
		}catch(IndexOutOfRangeException e){
			gameObject.SetActive(false);
		}
	}
	
	public void ChangeFBRankContent(int idx){
		int listCount = gameRank.instance.listFFR.Count;
		if(listCount <= idx){
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}
		
		
		int rank = 0;float fTime=0;
		Transform mTr = null;
		var tr = transform.GetChild(0) as Transform;
		gameRank.RaceRankInfo rnk = gameRank.instance.listFFR[idx];
		
		if(rnk.userId.ToString() == GV.UserRevId){
			if(gameRank.instance.listFFR[idx].scoreFr){
				// is Ranking info
				mTr= tr.FindChild("throphy") as Transform;
				mTr.FindChild("lbthrophy_1").GetComponent<UILabel>().text = rnk.trophy1;
				mTr.FindChild("lbthrophy_2").GetComponent<UILabel>().text = rnk.trophy2;
				mTr.FindChild("lbthrophy_3").GetComponent<UILabel>().text = rnk.trophy3;
				tr.FindChild("icon_car").GetComponent<UISprite>().spriteName = rnk.carId.ToString();
				if(rnk.mRaceUserInfo.crewId == 0) 	transform.FindChild("icon_crew").GetComponent<UISprite>().spriteName = "1200A";
				tr.FindChild("icon_crew").GetComponent<UISprite>().spriteName = rnk.mRaceUserInfo.crewId.ToString()+"A";
				mTr = tr.FindChild("rank");
				if(CClub.ClanMember == 0){
					tr.FindChild("Club").gameObject.SetActive(false);
				}else{
					tr.FindChild("Club").gameObject.SetActive(true);
					tr.FindChild("Club").FindChild("lb_Club").GetComponent<UILabel>().text =CClub.mClubInfo.mClubName;
					tr.FindChild("Club").FindChild("icon_Club").GetComponent<UISprite>().spriteName =CClub.mClubInfo.clubSymbol;
				}
				if(rnk.level == 0) rnk.level  = 1;
				mTr = tr.FindChild("kakao");
				mTr.FindChild("lbLevel").GetComponent<UILabel>().text =rnk.level.ToString();
				mTr.FindChild("lbNick").GetComponent<UILabel>().text = rnk.userNick;
				
				tr.FindChild("lb_Car").GetComponent<UILabel>().text = rnk.mRaceUserInfo.carAbility.ToString();
				tr.FindChild("lb_Crew").GetComponent<UILabel>().text =rnk.mRaceUserInfo.crewAbility.ToString();
				
			}else{
				// not rank info but user info
				mTr = tr.FindChild("throphy") as Transform;
				mTr.FindChild("lbthrophy_1").GetComponent<UILabel>().text = "0";
				mTr.FindChild("lbthrophy_2").GetComponent<UILabel>().text = "0";
				mTr.FindChild("lbthrophy_3").GetComponent<UILabel>().text =  "0";
				tr.FindChild("icon_car").GetComponent<UISprite>().spriteName = GV.getTeamCarID(GV.SelectedTeamID).ToString();
				tr.FindChild("icon_crew").GetComponent<UISprite>().spriteName =GV.getTeamCrewID(GV.SelectedTeamID).ToString()+"A";
				
				if(CClub.ClanMember == 0){
					tr.FindChild("Club").gameObject.SetActive(false);
				}else{
					tr.FindChild("Club").gameObject.SetActive(true);
					tr.FindChild("Club").FindChild("lb_Club").GetComponent<UILabel>().text =CClub.mClubInfo.mClubName;
					tr.FindChild("Club").FindChild("icon_Club").GetComponent<UISprite>().spriteName =CClub.mClubInfo.clubSymbol;
				}
				//if(rnk.level == 0) rnk.level  = 1;
				mTr = tr.FindChild("kakao");
				mTr.FindChild("lbLevel").GetComponent<UILabel>().text =Global.level.ToString();
				mTr.FindChild("lbNick").GetComponent<UILabel>().text =  GV.UserNick;
				
				tr.FindChild("lb_Car").GetComponent<UILabel>().text = GV.CarAbility.ToString();
				tr.FindChild("lb_Crew").GetComponent<UILabel>().text = GV.CrewAbility.ToString();
			}
			rank = idx+1;
			//	if((Global.gMyRank[0]) == rank){
			tr.FindChild("BG").gameObject.SetActive(false);
			tr.FindChild("BG_me").gameObject.SetActive(true);
			//	}else{
			//		tr.FindChild("BG").gameObject.SetActive(true);
			//		tr.FindChild("BG_me").gameObject.SetActive(false);
			//	}
			
			
			
			
			
			
			mTr = tr.FindChild("rank");
			if(rank == 1){
				mTr.FindChild("icon_rank").GetComponent<UISprite>().spriteName = "Playerifo_rank1";
			}else if(rank == 2){
				mTr.FindChild("icon_rank").GetComponent<UISprite>().spriteName = "Playerifo_rank2";
			}else if(rank == 3){
				mTr.FindChild("icon_rank").GetComponent<UISprite>().spriteName = "Playerifo_rank3";
			}else {
				mTr.FindChild("icon_rank").GetComponent<UISprite>().spriteName = "Playerifo_rank4";
			}
			mTr.FindChild("lbrankNum").GetComponent<UILabel>().text = string.Format("{0}", rank);
			fTime = 100.0f;
			if(!rnk.scoreFr){
				fTime = 108.0f;
				mTr.FindChild("lbTime").GetComponent<UILabel>().text = string.Format("00:00.000");
				
			}else{
				fTime = rnk.record;
				mTr.FindChild("lbTime").GetComponent<UILabel>().text = string.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f);
				
			}
			CheckUserProfileLoad(rnk);
			return;
		}
		
		
		
		if(gameRank.instance.listFFR[idx].scoreFr){
			// is Ranking info
			mTr= tr.FindChild("throphy") as Transform;
			mTr.FindChild("lbthrophy_1").GetComponent<UILabel>().text = rnk.trophy1;
			mTr.FindChild("lbthrophy_2").GetComponent<UILabel>().text = rnk.trophy2;
			mTr.FindChild("lbthrophy_3").GetComponent<UILabel>().text = rnk.trophy3;
			tr.FindChild("icon_car").GetComponent<UISprite>().spriteName = rnk.carId.ToString();
			if(rnk.mRaceUserInfo.crewId == 0) 	transform.FindChild("icon_crew").GetComponent<UISprite>().spriteName = "1200A";
			tr.FindChild("icon_crew").GetComponent<UISprite>().spriteName = rnk.mRaceUserInfo.crewId.ToString()+"A";
			mTr = tr.FindChild("rank");
			if(rnk.mRaceUserInfo.clubLv == 0){
				tr.FindChild("Club").gameObject.SetActive(false);
			}else{
				tr.FindChild("Club").gameObject.SetActive(true);
				tr.FindChild("Club").FindChild("lb_Club").GetComponent<UILabel>().text =rnk.mRaceUserInfo.clubName;
				tr.FindChild("Club").FindChild("icon_Club").GetComponent<UISprite>().spriteName =rnk.mRaceUserInfo.clubSymbol;
			}
			
			
			if(rnk.level == 0) rnk.level  = 1;
			mTr = tr.FindChild("kakao");
			mTr.FindChild("lbLevel").GetComponent<UILabel>().text =rnk.level.ToString();
			mTr.FindChild("lbNick").GetComponent<UILabel>().text = rnk.userNick;
			
			tr.FindChild("lb_Car").GetComponent<UILabel>().text = rnk.mRaceUserInfo.carAbility.ToString();
			tr.FindChild("lb_Crew").GetComponent<UILabel>().text =rnk.mRaceUserInfo.crewAbility.ToString();
			
		}else{
			// not rank info but user info
			mTr = tr.FindChild("throphy") as Transform;
			mTr.FindChild("lbthrophy_1").GetComponent<UILabel>().text = "0";
			mTr.FindChild("lbthrophy_2").GetComponent<UILabel>().text = "0";
			mTr.FindChild("lbthrophy_3").GetComponent<UILabel>().text =  "0";
			tr.FindChild("icon_car").GetComponent<UISprite>().spriteName = "1000";
			tr.FindChild("icon_crew").GetComponent<UISprite>().spriteName = "1200A";
			tr.FindChild("Club").gameObject.SetActive(false);
			if(rnk.level == 0) rnk.level  = 1;
			mTr = tr.FindChild("kakao");
			mTr.FindChild("lbLevel").GetComponent<UILabel>().text =rnk.level.ToString();
			mTr.FindChild("lbNick").GetComponent<UILabel>().text =  rnk.userNick;
			
			tr.FindChild("lb_Car").GetComponent<UILabel>().text = "-";
			tr.FindChild("lb_Crew").GetComponent<UILabel>().text = "-";
		}
		
		
		rank = idx+1;
		//	if((Global.gMyRank[0]) == rank){
		//		tr.FindChild("BG").gameObject.SetActive(false);
		//		tr.FindChild("BG_me").gameObject.SetActive(true);
		//	}else{
		tr.FindChild("BG").gameObject.SetActive(true);
		tr.FindChild("BG_me").gameObject.SetActive(false);
		//	}
		
		
		
		
		
		
		mTr = tr.FindChild("rank");
		if(rank == 1){
			mTr.FindChild("icon_rank").GetComponent<UISprite>().spriteName = "Playerifo_rank1";
		}else if(rank == 2){
			mTr.FindChild("icon_rank").GetComponent<UISprite>().spriteName = "Playerifo_rank2";
		}else if(rank == 3){
			mTr.FindChild("icon_rank").GetComponent<UISprite>().spriteName = "Playerifo_rank3";
		}else {
			mTr.FindChild("icon_rank").GetComponent<UISprite>().spriteName = "Playerifo_rank4";
		}
		mTr.FindChild("lbrankNum").GetComponent<UILabel>().text = string.Format("{0}", rank);
		fTime = 100.0f;
		if(!rnk.scoreFr){
			fTime = 108.0f;
			mTr.FindChild("lbTime").GetComponent<UILabel>().text = string.Format("00:00.000");
			
		}else{
			fTime = rnk.record;
			mTr.FindChild("lbTime").GetComponent<UILabel>().text = string.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f);
			//Utility.LogWarning(fTime);
		}
		
		
		
		CheckUserProfileLoad(rnk);
		
	}
	
	
	
	
	void CheckUserProfileLoad(gameRank.RaceRankInfo mUser){
		Texture2D mTex = mUser.mRaceUserInfo.userProfile;
		StopCoroutine("rankUserPictureLoad");
		if(mTex == null){
			SNSManager.FaceBookFr fr = SNSManager.facebookfr.Find(obj => obj.FBID == mUser.fbUid);
			if(fr != null){
				if(fr.FBUrl =="None"){
					
				}else{
					mUser.mRaceUserInfo.userURL = fr.FBUrl;
				}
			}else{
				
			}
			StartCoroutine("rankUserPictureLoad",mUser);
		}else{
			var mTr = transform.GetChild(0) as Transform;
			var tr = mTr.FindChild("kakao") as Transform;
			tr.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = mTex;
		}
	}
	
	
	IEnumerator rankUserPictureLoad(gameRank.RaceRankInfo mUser){
		
		string url = mUser.mRaceUserInfo.userURL;
		bool bHas = false;
		if(string.IsNullOrEmpty(url)== true){
			if(mUser.userId.ToString() == GV.UserRevId){
				url = EncryptedPlayerPrefs.GetString("FBUrl");
				mUser.mRaceUserInfo.userURL = url;
			}else{
				url = SNSManager.facebookfr.Find(obj => obj.FBID == mUser.fbUid).FBUrl;
				mUser.mRaceUserInfo.userURL = url;
			}
		}
		
		if(!url.Contains("http")){
			bHas = true;
		}
		if(bHas){
			var temp1 =transform.GetChild(0).FindChild("kakao") as Transform;
			mUser.mRaceUserInfo.userProfile = (Texture2D)(Texture)Resources.Load("User_Default", typeof(Texture));
			temp1.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture =mUser.mRaceUserInfo.userProfile ;
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
			var temp =transform.GetChild(0).FindChild("kakao") as Transform;
			temp.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = pic;
			mUser.mRaceUserInfo.userProfile = pic;
			www.Dispose();
		}
	}
	
	
	
}
