using UnityEngine;
using System.Collections;

public class FriendRankItem : MonoBehaviour {
	

	int myid = 0;
	public void FRankContent(int idx){
		return;
		/*
		myid = idx;
		int listCount = gameRank.instance.listfriend.Count;
		if(listCount <= idx){
		//	Utility.Log("list count : " + listCount);
		//	Utility.Log("list idx : " + idx);
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}
		gameRank.friendRank wrank = gameRank.instance.listfriend[idx];
		var temp1 = transform.FindChild("rank").FindChild("icon_rank") as Transform;
		string rankIcon = string.Empty;
		if(wrank.nRanking == 1) rankIcon = "Playerifo_rank1";
		else if(wrank.nRanking == 2) rankIcon ="Playerifo_rank2";
		else if(wrank.nRanking == 3) rankIcon ="Playerifo_rank3";
		else  rankIcon = "Playerifo_rank4";
		if(wrank.nPlayTimes == 0) rankIcon = "Playerifo_rank4";
		temp1.GetComponent<UISprite>().spriteName = rankIcon; 
		
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

		temp1 = transform.FindChild("kakao");
		temp1.FindChild("lbLevel").GetComponent<UILabel>().text = wrank.nLevel.ToString();
		temp1.FindChild("lbNick").GetComponent<UILabel>().text = wrank.strNick;
		if(wrank.strProfileUrl == null || wrank.strProfileUrl == "NoImage"){
			temp1.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = Global.gDefaultIcon;
		}
		Texture2D userImage = null;
		userImage  = UserDataManager.instance.GetTexture(2000+idx);
		StopCoroutine("fRankPic");
		if(userImage == null){
			StartCoroutine("fRankPic",idx);
		}else{
			temp1.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = userImage;
		}
		temp1 = transform.FindChild("fuel");
		bool b = false;
		if(wrank.nIndex.Equals(ProtocolManager.instance.snsUserId)){
			b = true;
			Global.gRankMyrank = idx;
		}
	
		if(b){
			transform.FindChild("BG_me").gameObject.SetActive(true);
			if(Global.gReceivedFuel == 1){ // ok receive fule
				ResetIcon(0);
			}else{
				ResetIcon(1); // no receive fuel
				temp1.FindChild("myFuel_off").GetComponent<UIButtonMessage>().functionName = "OnMyFuelOff";
			}
		}else{
			transform.FindChild("BG_me").gameObject.SetActive(false);
		//	if(wrank.nBlock == 1){ // block ok
		//		ResetIcon(1);
		//		temp1.FindChild("myFuel_off").GetComponent<UIButtonMessage>().functionName = null;
		//	}else{
		//		ResetIcon(2); // none block
		//	}
			temp1.FindChild("myFuel_off").GetComponent<UIButtonMessage>().functionName = null;
			if(wrank.tmMsgTime.Equals(string.Empty) || wrank.tmMsgTime.Equals(null)){
			
			}else{
				SendedFuelTimeCheck(wrank.tmMsgTime);
				//ResetIcon(3);
			}
		}
		
		var rankInfo = transform.FindChild("rank") as Transform;
		if(wrank.nPlayTimes != 0){
			rankInfo.FindChild("lbrankNum").GetComponent<UILabel>().text  = 
				string.Format("{0}", (wrank.nRanking));
			rankInfo.FindChild("lbTime").GetComponent<UILabel>().text  = 
				string.Format("{0:00}:{1:00.000}", Mathf.Floor((wrank.fTime/60f)) ,wrank.fTime%60.0f);
			transform.FindChild("lbText").gameObject.SetActive(false);
		}else{
			rankInfo.FindChild("lbrankNum").GetComponent<UILabel>().text  = 
				"No Rank";
			transform.FindChild("lbText").gameObject.SetActive(true);
			transform.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("72811");
			rankInfo.FindChild("lbTime").GetComponent<UILabel>().text  = 
				string.Empty;
		}
		*/
	}

	Texture2D Frank = null;
	IEnumerator fRankPic(int id){
		string url = gameRank.instance.listfriend[id].strProfileUrl;
		if(!url.Contains("http")){
			var temp1 = transform.FindChild("kakao") as Transform;
		//	if(Frank == null) Utility.LogWarning("a " + id);
		//	UserDataManager.instance.SaveSetTexture((2000+id),Frank);
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
			//UserDataManager.instance.SaveSetTexture((2000+id),Frank);
			temp.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = Frank;
			www.Dispose();
		}
	}

	void ResetIcon(int cnt){
		var temp = transform.FindChild("fuel") as Transform;
		bool a = false, b = false, c = false, d = false;
		if(cnt == 0){
			a = true;
		}else if(cnt == 1){
			b = true;
		}else if(cnt == 2){
			c = true;
		}else if(cnt == 3){
			d= true;
		}
		temp.FindChild("myFuel_on").gameObject.SetActive(a);
		temp.FindChild("myFuel_off").gameObject.SetActive(b);
		temp.FindChild("sendFuel").gameObject.SetActive(c);
		temp.FindChild("sendedFuel").gameObject.SetActive(d);
		if(d){
		//	Utility.Log(gameRank.instance.listfriend[myid].tmMsgTime + "-- " + myid);
			temp.FindChild("sendedFuel").GetComponent<sendedFuelTime>().CheckTime(gameRank.instance.listfriend[myid].tmMsgTime);
		}
	}

	void SendedFuelTimeCheck(string sdtime){
		if(sdtime == "null") {
			ResetIcon(2); return;
		}
		System.DateTime cT = NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		System.DateTime sT = System.Convert.ToDateTime(sdtime);
		System.TimeSpan sp = cT - sT;
		if(sp.TotalMinutes >= 61){
			ResetIcon(2);
		}else{
			ResetIcon(3);
		}
	}
	void OnsendedFuel(){
		
		//ResetIcon(2); //Fuel On
	}
	/*
	void OnsendFuel(){

		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<sendFuelpopUp>().InitPopUp(gameObject);
		//gameObject.AddComponent<makePopup>().makePopupAction("sendFuelpopUp", gameObject);
	
	}

	void OnMyFuelOff(){
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<ReceiveFuelOnPopUp>().InitPopUp(gameObject);
		//gameObject.AddComponent<makePopup>().makePopupAction("ReceiveFuelOnPopUp", gameObject);
	}

	void OnMyFuelOn(){
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<ReceiveFuelOffPopUp>().InitPopUp(gameObject);
		//gameObject.AddComponent<makePopup>().makePopupAction("ReceiveFuelOffPopUp", gameObject);
	}
*/
	void OnMyFuelAction(int idx){
		ResetIcon(idx);
	}
	
	void OnSendFuelAction(int idx){
		if(idx == 3){
			Utility.Log(gameRank.instance.listfriend[myid].tmMsgTime);
			ResetIcon(3);

		}else if(idx == 2){
			ResetIcon(2);
		}
	}

	public void RankFriendContent(int idx){
	/*	
	 * 
	 * 
	 * public void ChangeFriendRankContent(int idx){
		int listCount = gameRank.instance.listfriend.Count;
		if(listCount <= idx){
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}
		//	var temp = transform.FindChild("fuel") as Transform;
		//	temp.gameObject.SetActive(false);
		gameRank.friendRank wrank = gameRank.instance.listfriend[idx];
		var temp1 = transform.FindChild("rank").FindChild("icon_rank") as Transform;
		string rankIcon = string.Empty;
		if(wrank.nRanking == 1) rankIcon = "Playerifo_rank1";
		else if(wrank.nRanking == 2) rankIcon ="Playerifo_rank2";
		else if(wrank.nRanking == 3) rankIcon ="Playerifo_rank3";
		else  rankIcon = "Playerifo_rank4";
		if(wrank.nPlayTimes == 0) rankIcon = "Playerifo_rank4";
		temp1.GetComponent<UISprite>().spriteName = rankIcon; 
		
		
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
		
		
		temp1 = transform.FindChild("kakao");
		temp1.FindChild("lbLevel").GetComponent<UILabel>().text = wrank.nLevel.ToString();
		temp1.FindChild("lbNick").GetComponent<UILabel>().text = wrank.strNick;
		if(wrank.strProfileUrl == null || wrank.strProfileUrl == "NoImage"){
			//Texture texture = (Texture)Resources.Load("Kakao_icon", typeof(Texture));
			temp1.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = Global.gDefaultIcon;
		}
		var rankInfo = transform.FindChild("rank") as Transform;
		if(wrank.nPlayTimes != 0){
			rankInfo.FindChild("lbrankNum").GetComponent<UILabel>().text  = 
				string.Format("{0}", (wrank.nRanking));
			rankInfo.FindChild("lbTime").GetComponent<UILabel>().text  = 
				string.Format("{0:00}:{1:00.000}", Mathf.Floor((wrank.fTime/60f)) ,wrank.fTime%60.0f);
			//rankInfo.FindChild("lbText").GetComponent<UILabel>().text = string.Empty;
			transform.FindChild("lbText").gameObject.SetActive(false);
		}else{
			rankInfo.FindChild("lbrankNum").GetComponent<UILabel>().text  = 
				"No Rank";
			transform.FindChild("lbText").gameObject.SetActive(true);
			transform.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.getStringDic("60250");
			rankInfo.FindChild("lbTime").GetComponent<UILabel>().text  = 
				string.Empty;
		}
		temp1 = transform.FindChild("fuel");
		bool b = false;
		if(wrank.nRanking == Global.gFriendRank){
			if(wrank.nIndex == ProtocolManager.instance.snsUserId){
				Global.gRankMyrank = idx;
				b = true;
				UserDataManager.instance.myPicture = (Texture2D)Global.gDefaultIcon;
			}
		}
		if(b){
			transform.FindChild("BG_me").gameObject.SetActive(true);
			if(Global.gReceivedFuel == 1){
				ResetIcon(0);
			}else{
				ResetIcon(1);
				temp1.FindChild("myFuel_off").GetComponent<UIButtonMessage>().functionName = "OnMyFuelOff";
			}
			
		}else{
			temp1.FindChild("myFuel_off").GetComponent<UIButtonMessage>().functionName = null;
			transform.FindChild("BG_me").gameObject.SetActive(false);
		//	if(wrank.nBlock == 1){
		//		ResetIcon(1);
		//		temp1.FindChild("myFuel_off").GetComponent<UIButtonMessage>().functionName = null;
		//	}else{
		//		ResetIcon(2);
		//		
		//	}
			if(wrank.tmMsgTime.Equals(string.Empty) || wrank.tmMsgTime.Equals(null)){
			}else	{
				SendedFuelTimeCheck(wrank.tmMsgTime);
			}
		}/*
		if(wrank.nBlock == null){
			if(Global.gReceivedFuel == 1){
				ResetIcon(0);
			}else{
				ResetIcon(1);
				temp1.FindChild("myFuel_off").GetComponent<UIButtonMessage>().functionName = "OnMyFuelOff";
			}
		}else{
			if(wrank.nBlock == 1){
				ResetIcon(1);
				temp1.FindChild("myFuel_off").GetComponent<UIButtonMessage>().functionName = null;
			}else{
				ResetIcon(2);
			}
		
		}*/
	

		/*} int cnt = KakaoFriends.Instance.appFriends.Count;
		//var temp = transform.FindChild("Coin") as Transform;
		//temp.gameObject.SetActive(false);
		var temp = transform.FindChild("rank") as Transform;
		int radomX = Random.Range(0,5);
		//Utility.LogWarning("RankFriendContent" + idx + " + " + cnt);
		var temp1 = temp.FindChild("icon_rank") as Transform;
		string rankIcon = string.Empty;
		
		if(idx == 0) rankIcon = "Playerifo_rank1";
		else if(idx == 1) rankIcon ="Playerifo_rank2";
		else if(idx == 2) rankIcon ="Playerifo_rank3";
		else rankIcon = "Playerifo_rank4";
		temp1.GetComponent<UISprite>().spriteName = rankIcon; 
		
		if(idx == cnt){
			temp.FindChild("lbrankNum").GetComponent<UILabel>().text  = 
				string.Format("{0}", (idx+1));
			temp.FindChild("lbTime").GetComponent<UILabel>().text 
				= string.Format("{0:00}:{1:00}:{2:000}", 0,(25+radomX*idx), (32+radomX*idx));
			temp.FindChild("lbrankNum").GetComponent<UILabel>().text 
				= string.Format("{0}", (idx+1));
			
			temp = transform.FindChild("kakao");
			temp.FindChild("lbLevel").GetComponent<UILabel>().text = (10+idx*radomX+1).ToString();
			if(Global.myProfile == null){
				//UserDataManager.instance.StartCoroutine(	"myProflieLoad");
			}
			temp.FindChild("lbNick").GetComponent<UILabel>().text = Global.myNick;
			temp.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = Global.myProfile;
			//Global.gReceivedFuel = 1;
			//Utility.Log("Global  gReceivedFuel " + Global.gReceivedFuel);
			temp = transform.FindChild("fuel");
			if(Global.gReceivedFuel == 1){
				ResetIcon(0);
			}else{
				ResetIcon(1);
				temp.FindChild("myFuel_off").GetComponent<UIButtonMessage>().functionName = "OnMyFuelOff";
			}
			//	if(KakaoLocalUser.Instance.messageBlocked){
			//		ResetIcon(1);
			//		temp.FindChild("myFuel_off").GetComponent<UIButtonMessage>().functionName = "OnMyFuelOff";
			//	}else{
			//		ResetIcon(0);
			//		//temp.FindChild("myFuel_off").GetComponent<UIButtonMessage>().functionName = "OnMyFuelOff";
			//	}
		}else{
			KakaoFriends.Friend _fr = KakaoFriends.Instance.appFriends[idx];
			temp.FindChild("lbrankNum").GetComponent<UILabel>().text  = 
				string.Format("{0}", (idx+1));
			temp.FindChild("lbTime").GetComponent<UILabel>().text 
				= string.Format("{0:00}:{1:00}:{2:000}", 0,(25+radomX*idx), (32+radomX*idx));
			temp.FindChild("lbrankNum").GetComponent<UILabel>().text 
				= string.Format("{0}", (idx+1));
			temp = transform.FindChild("kakao");
			temp.FindChild("lbLevel").GetComponent<UILabel>().text = (10+idx*radomX+1).ToString();
			temp.FindChild("lbNick").GetComponent<UILabel>().text = _fr.nickname;
			Texture2D tx = UserDataManager.instance.GetTexture((2000+idx));
			StopCoroutine("fRankPic");
			if(tx == null){
				temp.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = null;
				StartCoroutine("fRankPic",idx);
			}else{
				temp.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = tx;
			}
			
			temp = transform.FindChild("fuel");
			if(_fr.messageBlocked){
				ResetIcon(1);
				temp.FindChild("myFuel_off").GetComponent<UIButtonMessage>().functionName = null;
			}else{
				ResetIcon(2);
			}
			//KakaoFriends.Friend _fr = KakaoFriends.Instance.appFriends[idx];
		}
*/
	}


}
