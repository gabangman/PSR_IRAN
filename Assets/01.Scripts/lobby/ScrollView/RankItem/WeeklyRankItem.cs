using UnityEngine;
using System.Collections;

public class WeeklyRankItem : MonoBehaviour {

	public void ChangeWeeklyRankContent(int idx){
		return; /*
		int listCount = gameRank.instance.listWeekly.Count;
		if(listCount <= idx){
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}
		
		var temp = transform.FindChild("fuel") as Transform;
		temp.gameObject.SetActive(false);
		gameRank.worldRank wrank = gameRank.instance.listWeekly[idx];
		var temp1 = transform.FindChild("rank").FindChild("icon_rank") as Transform;
		string rankIcon = string.Empty;
		if(wrank.nRanking == 1) rankIcon = "Playerifo_rank1";
		else if(wrank.nRanking == 2) rankIcon ="Playerifo_rank2";
		else if(wrank.nRanking == 3) rankIcon ="Playerifo_rank3";
		else  rankIcon = "Playerifo_rank4";
		temp1.GetComponent<UISprite>().spriteName = rankIcon; 
		transform.FindChild("rank").FindChild("lbrankNum").GetComponent<UILabel>().text  = 
			string.Format("{0}", (wrank.nRanking));
		transform.FindChild("rank").FindChild("lbTime").GetComponent<UILabel>().text  = 
			string.Format("{0:00}:{1:00.000}", Mathf.Floor((wrank.fTime/60f)) ,wrank.fTime%60.0f);
		
		transform.FindChild("icon_car").GetComponent<UISprite>().spriteName
			= wrank.nMyCar.ToString();
		transform.FindChild("icon_crew").GetComponent<UISprite>().spriteName
			=wrank.nMyCrew.ToString()+"A";
		transform.FindChild("lb_Car").GetComponent<UILabel>().text = wrank.nCarAbility.ToString();
		transform.FindChild("lb_Crew").GetComponent<UILabel>().text = wrank.nCrewAbility.ToString();
		temp1 = transform.FindChild("throphy");
		temp1.FindChild("lbthrophy_1").GetComponent<UILabel>().text
			= string.Format("{0}",wrank.nGoldenTrophy);	
		temp1.FindChild("lbthrophy_2").GetComponent<UILabel>().text
			= string.Format("{0}",wrank.nSilverTrophy);	
		temp1.FindChild("lbthrophy_3").GetComponent<UILabel>().text
			= string.Format("{0}",wrank.nBronzeTrophy);	
		
		temp1 = transform.FindChild("rank_icon");
		temp1.gameObject.SetActive(true);
		for(int i =0; i < temp1.childCount;i++){
			temp1.GetChild(i).gameObject.SetActive(false);
		}
		if(wrank.nNew == 1){
			temp1.FindChild("icon_new").gameObject.SetActive(true);
		}else{
			if(wrank.nLvUp == 0){
				temp1.FindChild("icon_same").gameObject.SetActive(true);
			}else if(wrank.nLvUp > 0){
				temp1.FindChild("icon_up").gameObject.SetActive(true);
				temp1.FindChild("lbUp").gameObject.SetActive(true);
				temp1.FindChild("lbUp").GetComponent<UILabel>().text = wrank.nLvUp.ToString();
			}else{
				temp1.FindChild("icon_down").gameObject.SetActive(true);
				temp1.FindChild("lbDown").gameObject.SetActive(true);
				int temprank = wrank.nLvUp * -1;
				temp1.FindChild("lbDown").GetComponent<UILabel>().text = temprank.ToString();
			}	
		}
		
		temp1 = transform.FindChild("kakao");
		temp1.FindChild("lbLevel").GetComponent<UILabel>().text = wrank.nLevel.ToString();
		temp1.FindChild("lbNick").GetComponent<UILabel>().text = wrank.strNick;
		if(wrank.strProfileUrl == null || wrank.strProfileUrl == "NoImage"){
			temp1.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = Global.gDefaultIcon;
		}
		Texture2D userImage = null;
		userImage = UserDataManager.instance.GetTexture(8000+idx);
		StopCoroutine("fRankPic");
		if(userImage == null){
			StartCoroutine("fRankPic",idx);
		}else{
			temp1.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = userImage;
		}
		
		if(Global.gMyWeeklyRank == 0 || Global.gMyWeeklyRank >200){
			transform.FindChild("BG_me").gameObject.SetActive(false);
		}else{
			if(wrank.strNick.Equals(GV.UserNick)){
				string globalNick = KoStorage.GetKorString("70123");
				if(GV.UserNick == globalNick){
					transform.FindChild("BG_me").gameObject.SetActive(false);
				}else{
					transform.FindChild("BG_me").gameObject.SetActive(true);
				}
			}else{
				transform.FindChild("BG_me").gameObject.SetActive(false);
			}
		}*/
	}
	
	Texture2D Frank = null;
	IEnumerator fRankPic(int id){
		//string url = KakaoFriends.Instance.appFriends[id].profileImageUrl;
		string url = string.Empty;
		if(!url.Contains("http")){
			var temp1 = transform.FindChild("kakao") as Transform;
			temp1.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = Global.gDefaultIcon;
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
			Frank = www.texture;
			var temp = transform.FindChild("kakao") as Transform;
			//temp.FindChild("lbNick").GetComponent<UILabel>().text = _fr.nickname;
			//UserDataManager.instance.SaveSetTexture((8000+id),Frank);
			temp.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = Frank;
			www.Dispose();
		}
	}
}
