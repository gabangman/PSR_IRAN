using UnityEngine;
using System.Collections;

public class weeklyBoardItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*
			int idx = 0;
			string[] name = gameObject.name.Split('_');
			int.TryParse(name[1], out idx);
			
			gameRank.weeklyRank wrank = gameRank.instance.listresult[idx];
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
			temp1 = transform.FindChild("Coin");
			temp1.FindChild("lbquantity").GetComponent<UILabel>().text = string.Format("{0}", wrank.nRewardCoin);
			
			
			temp1 = transform.FindChild("kakao");
			temp1.FindChild("lbLevel").GetComponent<UILabel>().text = wrank.nLevel.ToString();
			temp1.FindChild("lbNick").GetComponent<UILabel>().text = wrank.strNick;
			if(wrank.strProfileUrl == null || wrank.strProfileUrl == "NoImage"){
				temp1.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = Global.gDefaultIcon;
			}
			Texture2D userImage = null;
		//	userImage = UserDataManager.instance.GetTexture(4000+idx);
			StopCoroutine("fRankPic");
			if(userImage == null){
				StartCoroutine("fRankPic",idx);
			}else{
				temp1.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = userImage;
			}
			
			if(wrank.strNick.Equals(GV.UserNick)){
				transform.FindChild("BG_me").gameObject.SetActive(true);
			}else{
				transform.FindChild("BG_me").gameObject.SetActive(false);
			} */
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
			//	UserDataManager.instance.SaveSetTexture((4000+id),Frank);
				temp.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = Frank;
				www.Dispose();
				www = null;
			}
		}
	
	
	
}
