using UnityEngine;
using System.Collections;

public class PostMessageItem : MonoBehaviour {
	public System.Action thiscallback;

	void OnAccept(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		string[] str = gameObject.name.Split('_');
		int _id = int.Parse(str[1]);
		StartCoroutine("OnAcceptPost" , _id);
	

	}


	IEnumerator OnAcceptPost(int _id){
		bool bConnect = false;
		System.Collections.Generic.Dictionary<string,int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		gameRank.GiftList gList = gameRank.instance.listGift[_id];
		mDic.Add("giftIdx", gList.nMsgIndex);
		mDic.Add("type", gList.nType);
		string mString  = "value;"+gList.rewardValue;
		int rTyp = gList.nType;
		string mAPI = ServerAPI.Get(90056); // "game/giftbox/gift"/game/giftbox/gift
	
		NetworkManager.instance.HttpFormConnect("Delete", mDic ,mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
			
			}else{
			

			}
			bConnect = true;
		},mString );
		while(!bConnect){
			yield return null;
		}
		RewardComplete(gameRank.instance.listGift[_id]);
		gameRank.instance.listGift.RemoveAt(_id);
		Global.gNewMsg = gameRank.instance.listGift.Count;
		if(thiscallback != null){
			thiscallback();
		}


		if(rTyp == 3){
			bConnect = false;
			mAPI = ServerAPI.Get(90013); // "game/material/"
			NetworkManager.instance.HttpConnect("Get",  mAPI, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				if(string.IsNullOrEmpty(request.response.Text) == true) {
					AccountManager.instance.ErrorPopUp();
					return;
				}
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					int cnt = thing["result"].Count;
					for(int i = 0; i < cnt; i++){
						int matid =  thing["result"][i]["materialId"].AsInt;
						int matcount = thing["result"][i]["count"].AsInt;
						MatInfo mat = GV.listMyMat.Find(obj => obj.MatID == matid);
						if(mat != null) mat.MatQuantity = matcount;
					}
					myAcc.instance.account.bInvenBTN[1] = true;
					GameObject.Find("LobbyUI").SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
				}else{
					if(status == -105){
						AccountManager.instance.ErrorPopUp();
						return;
					}
				}
				bConnect = true;
		
			});
			
			while(!bConnect){
				yield return null;
			}
		}
		Global.isNetwork = false;

	}

	private void RewardComplete(gameRank.GiftList gList){
		GameObject Lobby = GameObject.Find("LobbyUI") as GameObject;
		switch(gList.nType){
		case 2 :{
			GV.myCoin = GV.myCoin+int.Parse(gList.rewardValue);
			Lobby.SendMessage("InitTopMenuReward", SendMessageOptions.DontRequireReceiver);
		}	break;
		case 0 :{
			GV.myDollar = GV.myDollar+int.Parse(gList.rewardValue);
			Lobby.SendMessage("InitTopMenuReward", SendMessageOptions.DontRequireReceiver);
		}break;
		case 3 :{
			//str = "Material";
			//GV.UpdateMatCount(int.Parse(gList.rewardValue), 1);
			//StartCoroutine("GetMaterial");
		}break;
		
		case 4 : {
			//	str = "SilverBox";
			myAcc.instance.account.bInvenBTN[3] = true;
			GV.UpdateCouponList(0, int.Parse(gList.rewardValue));	Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
		}break;
		case 5 : {
			//	str = "GoldBox";
			myAcc.instance.account.bInvenBTN[3] = true;
			GV.UpdateCouponList(1, int.Parse(gList.rewardValue));	Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
		}break;
		case 6 : {
			//str = "Cube";
			GV.UpdateMatCount(8620, int.Parse(gList.rewardValue));	Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
			myAcc.instance.account.bInvenBTN[2] = true;
		}break;
		}
	}

	void OnDestroy(){
		thiscallback  = null;
	}
	
	IEnumerator Change(int _id){

		ChangeGiftItems(_id);
		yield return null;
	}
	public void reUpdate(System.Action callback){
		string[] str = gameObject.name.Split('_');
		int _id = int.Parse(str[1]);
		ChangeGiftItems(_id);
	}
	
	public void ChangeGiftItems(int _id){
		if(gameRank.instance.listGift.Count <= _id) {
			gameObject.SetActive(false);
			return;
		}
		gameObject.SetActive(true);
		string price = string.Empty;
		string _text = string.Empty;
		string iconName = string.Empty;
		int idx = 0;
		Transform tr = null;
		bool b = false;
		gameRank.GiftList postList = gameRank.instance.listGift[_id];
		for(int i = 1; i < transform.childCount; i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}
	//	transform.FindChild("Accept").GetComponentInChildren<UILabel>().text = 
	//	KoStorage.GetKorString("72302");
		//return {"state":0,"msg":"sucess","result":[{"idx":871,"type":5,"value":"1","msg":"","msgType":5},{"idx":872,"type":4,"value":"1","msg":"","msgType":5}],"time":1472012759}
		//[0:dollar]
		//	[1:fuel]
		//	[2:coin]
		//	[3:material]
		//	[4:silvercoupon]
		//	[5:goldcoupon]
		//	[6: evocube]
		Vector3 mScale = Vector3.one;
		if(postList.nType == 0){
			mScale = new Vector3(37.7f, 37.7f,1);
			price = string.Format("X {0:#,0}", int.Parse(postList.rewardValue));	iconName = "icon_dollar";
		}else if(postList.nType == 2){
			price = string.Format("X {0:#,0}", int.Parse(postList.rewardValue));iconName = "icon_coin";
			mScale = new Vector3(37.7f, 37.7f,1);
		}else if(postList.nType == 3){
			price = string.Format("X {0:#,0}",  int.Parse(postList.rewardValue));	
			iconName ="mat_random";
			mScale = new Vector3(55f, 55f,1);
			//if(postList.rewardValue == "8620") mScale = new Vector3(43f, 43f,1);
			
		}else if(postList.nType == 4 ){
			price = string.Format("X {0:#,0}", postList.rewardValue);	iconName = "Coupon_Silver";
			mScale = new Vector3(92.6f, 44.5f,1);
		}	else if(postList.nType == 5 ){
			price = string.Format("X {0:#,0}", postList.rewardValue);	iconName = "Coupon_Gold";
			mScale = new Vector3(92.6f, 44.5f,1);
		}	else if(postList.nType == 6 ){
			price = string.Format("X {0:#,0}", postList.rewardValue);	iconName = "8620";
			mScale = new Vector3(43f, 43f,1);
		}
		
		if(postList.nMsgType == 1){
			// 7일 출석 이벤트 보상
			_text = KoStorage.GetKorString("72308");
			idx = 2;
		}else if(postList.nMsgType == 2){
			//클랜전 보상
			_text = KoStorage.GetKorString("72305");
			idx = 7;
		}else if(postList.nMsgType == 3){
			//페이스북 로그인 보상
			_text = KoStorage.GetKorString("72303");
			idx = 1;
		}else if(postList.nMsgType == 4){
			//관리자 전송된 보상
			_text = KoStorage.GetKorString("72306");
			idx = 3;
		}else if(postList.nMsgType == 5){
			//쿠폰번호 입력 보상 - empty
			_text = KoStorage.GetKorString("72309");
			idx = 6;
		}else if(postList.nMsgType == 6){
			//레벨업 보상
			_text = KoStorage.GetKorString("72313");
			idx = 4;
		}else if(postList.nMsgType == 7){
			//주간리그 보상
			_text = KoStorage.GetKorString("72304");
			idx = 5;
		}
		
		tr = transform.GetChild(idx);
		tr.gameObject.SetActive(true);
		tr.FindChild("lbTitle").GetComponent<UILabel>().text = _text;
		tr.FindChild("Reward").GetComponentInChildren<UILabel>().text = price;
		tr.FindChild("Reward").FindChild("icon").transform.localScale = mScale;
		tr.FindChild("Reward").GetComponentInChildren<UISprite>().spriteName = iconName;
		return;
	}


	IEnumerator GetMaterial(){
		yield return new WaitForSeconds(0.15f);
		bool bConnect = false;
		string mAPI = ServerAPI.Get(90013); // "game/material/"
		NetworkManager.instance.HttpConnect("Get",  mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				AccountManager.instance.ErrorPopUp();
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				GV.listMyMat = new System.Collections.Generic.List<MatInfo>();
				for(int i = 0; i < 20; i++){
					GV.listMyMat.Add(new MatInfo((8600+i), 0));
				}
				int cnt = thing["result"].Count;
				for(int i = 0; i < cnt; i++){
					int matid =  thing["result"][i]["materialId"].AsInt;
					int matcount = thing["result"][i]["count"].AsInt;
					GV.UpdateMatCount(matid,matcount);
				}

				myAcc.instance.account.bInvenBTN[1] = true;
				GameObject.Find("LobbyUI").SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
			}else{
				if(status == -105){
					AccountManager.instance.ErrorPopUp();
					return;
				}
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
		

	}


	IEnumerator GetEvoCube(){
		yield return new WaitForSeconds(0.15f);
		bool bConnect = false;
		string mAPI = ServerAPI.Get(90014); // "game/material/evoCube/"
		NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			if(string.IsNullOrEmpty(request.response.Text) == true) {
				AccountManager.instance.ErrorPopUp();
				return;
			}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				int matcount = thing["result"]["count"].AsInt;
				GV.UpdateMatCount(8620,matcount);
				
			}else{
				if(status == -105){
					AccountManager.instance.ErrorPopUp();
					return;
				}
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
	}
	public void ChangePost(int _id){
	/*	if(gameRank.instance.listPost.Count <= _id) {
			gameObject.SetActive(false);
			return;
		}
		gameObject.SetActive(true);
		
		string price = string.Empty;
		string _text = string.Empty;
		string _text1 = string.Empty;
		string iconName = string.Empty;
		int idx = 0;
		Transform tr = null;
		bool b = false;
		gameRank.PostList postList = gameRank.instance.listPost[_id];
		for(int i = 1; i < transform.childCount; i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}
		transform.FindChild("Accept").GetComponentInChildren<UILabel>().text = 
			KoStorage.GetKorString("72302");
		
		switch(postList.nType){
		case 1 :  // Fuel 친구로부터 받은 연료 1개
			price = "x 1";
			_text = string.Format(KoStorage.GetKorString("72303"),postList.strNick , 1);
			idx = 1;
			iconName = "Fuel_icon";
			break;
			
		case 2: // 초대 1회에 대해서 받은 연료 1개 // 코인
			if(postList.nCarExchangeCoupon != 0){
				
			}else if(postList.nReCoin != 0){
				price = string.Format("X {0}", postList.nReCoin);
				iconName = "icon_coin";
				string str1 = KoStorage.GetKorString("71001");
				_text = string.Format(KoStorage.GetKorString("72305"),str1,str1);
			}else if(postList.nReDollar != 0){
				price = string.Format("X {0}", postList.nReDollar);
				iconName = "icon_dollar";
				string str1 = KoStorage.GetKorString("71002");
				_text = string.Format(KoStorage.GetKorString("72305"),str1,str1);
			}else if(postList.nReFuel != 0){
				price = string.Format("X {0}", postList.nReFuel);	iconName = "Fuel_icon";
				string str1 = "연료";
				_text = string.Format(KoStorage.GetKorString("72305"),str1,str1);
			}
			price = "x 1";
			idx = 4;
			break;
		case 3 : //rec_attend 초대이벤트 단계별 보상 (달러,골드,교환권)
			string type = string.Empty;
			int invitecnt = 0;;
			if(postList.nCarExchangeCoupon != 0){
				price = string.Format("X {0}", postList.nCarExchangeCoupon);
				iconName = "Coupon_Car_s";
				invitecnt = 40;
				type = "자동차 교환권리래? ";//KoStorage.getStringDic("60227");
				b = true;
			}else if(postList.nReCoin != 0){
				price = string.Format("X {0}", postList.nReCoin);
				iconName = "icon_coin";
				if(postList.nReCoin == 10) invitecnt = 10;
				if(postList.nReCoin == 20) invitecnt = 20;
				if(postList.nReCoin == 50) invitecnt = 30;
				type = KoStorage.GetKorString("71001");
			}else if(postList.nReDollar != 0){
				price = string.Format("X {0}", postList.nReDollar);
				iconName = "icon_dollar";
				type = KoStorage.GetKorString("71002");
			}
			_text =  string.Format(KoStorage.GetKorString("72312"),invitecnt,type );
			idx = 2;
			break;
		case 4 : //출석이벤트 단계별 보상 (달러,골드,교환권)

		//	 * 2.출석이벤트 보상 : 
   		//		 -타임어택 : 320
    	//		-7일연속출석 : 321
			if(postList.nCarExchangeCoupon != 0){
				price = string.Format("X {0}", postList.nCarExchangeCoupon);iconName = "Coupon_Car_s";b = true;
				_text1 = "자동차 교환권이래?:";//KoStorage.getStringDic("60227");
			}else if(postList.nReCoin != 0){
				price = string.Format("X {0}", postList.nReCoin);	iconName = "icon_coin";
				_text1 = KoStorage.GetKorString("71001");
			}else if(postList.nReDollar != 0){
				price = string.Format("X {0}", postList.nReDollar);	iconName = "icon_dollar";
				_text1 = KoStorage.GetKorString("71002");
			}
			
			_text = string.Format(KoStorage.GetKorString("72410"), _text1);
			idx = 3;
			break;
		case 5 : //rec_admin 레벨업 보상 (코인)
			price = string.Format("X {0}", postList.nReCoin);	iconName = "icon_coin";
			
			iconName = "icon_coin";
			_text =  string.Format(KoStorage.GetKorString("72313"));
			idx = 6;
			break;
		case 6 : //rec_admin 관리자가 수동으로 전송한 골드 보상
			if(postList.nCarExchangeCoupon != 0){
				price = string.Format("X {0}", postList.nCarExchangeCoupon);iconName = "Coupon_Car_s";b = true;
				_text1 ="자동차 교환권이래?:"; //KoStorage.getStringDic("60227");
			}else if(postList.nReCoin != 0){
				price = string.Format("X {0}", postList.nReCoin);iconName = "icon_coin";
				_text1 = KoStorage.GetKorString("71001");
			}else if(postList.nReDollar != 0){
				price = string.Format("X {0}", postList.nReDollar);	iconName = "icon_dollar";
				_text1 = KoStorage.GetKorString("71002");
			}else if(postList.nCardQuaility !=0 ){
				price = string.Format("X {0}", postList.nCardQuaility);	iconName = postList.nCardId.ToString();
				_text1 = KoStorage.GetKorString("75001"); //ㅈㅐㄹㅛㅋㅏㄷㅡ
			}
			_text = string.Format(KoStorage.GetKorString("72306"), _text1);
			idx = 5;
			break;
		case 7 : //데일리 모드 5회 달성시 보상 골드
			if(postList.nCarExchangeCoupon != 0){
				price = string.Format("X {0}", postList.nCarExchangeCoupon);iconName = "Coupon_Car_s";b = true;
				_text1 ="자동차 교환권이래?:"; 
			}else if(postList.nReCoin != 0){
				price = string.Format("X {0}", postList.nReCoin);iconName = "icon_coin";
				_text1 = KoStorage.GetKorString("71001");
			}else if(postList.nReDollar != 0){
				price = string.Format("X {0}", postList.nReDollar);	iconName = "icon_dollar";
				_text1 = KoStorage.GetKorString("71002");
			}
			_text = string.Format(KoStorage.GetKorString("72306"), _text1);
			idx = 7;
			break;
		case 8 : //Time Attack
			if(postList.nCarExchangeCoupon != 0){
				price = string.Format("X {0}", postList.nCarExchangeCoupon);iconName = "Coupon_Car_s";b = true;
				_text1 ="자동차 교환권이래?:"; 
			}else if(postList.nReCoin != 0){
				price = string.Format("X {0}", postList.nReCoin);iconName = "icon_coin";
				_text1 = KoStorage.GetKorString("71001");
			}else if(postList.nReDollar != 0){
				price = string.Format("X {0}", postList.nReDollar);	iconName = "icon_dollar";
				_text1 = KoStorage.GetKorString("71002");
			}else if(postList.nReFuel !=0 ){
				price = string.Format("X {0}", postList.nReFuel);	iconName = "Fuel_icon";
				_text1 = "연료?";
			}else{
				price = string.Format("X {0}", postList.nCardQuaility);	iconName = postList.nCardId.ToString();
				_text1 = KoStorage.getStringDic("75001");
			}
			_text = string.Format(KoStorage.GetKorString("72407"), _text1);
			idx = 3;
			
			break;
		case 9 : //Review
			if(postList.nCarExchangeCoupon != 0){
				price = string.Format("X {0}", postList.nCarExchangeCoupon);iconName = "Coupon_Car_s";b = true;
				_text1 ="자동차 교환권이래?:"; 
			}else if(postList.nReCoin != 0){
				price = string.Format("X {0}", postList.nReCoin);iconName = "icon_coin";
				_text1 = KoStorage.GetKorString("71001");
			}else if(postList.nReDollar != 0){
				price = string.Format("X {0}", postList.nReDollar);	iconName = "icon_dollar";
				_text1 = KoStorage.GetKorString("71002");
			}
			_text = string.Format(KoStorage.GetKorString("72308"), _text1);
			idx = 5;
			
			break;
		case 10 : //First Ranking Race
			if(postList.nCarExchangeCoupon != 0){
				price = string.Format("X {0}", postList.nCarExchangeCoupon);iconName = "Coupon_Car_s";b = true;
				_text1 ="자동차 교환권이래?:"; 
			}else if(postList.nReCoin != 0){
				price = string.Format("X {0}", postList.nReCoin);iconName = "icon_coin";
				_text1 = KoStorage.GetKorString("71001");
			}else if(postList.nReDollar != 0){
				price = string.Format("X {0}", postList.nReDollar);	iconName = "icon_dollar";
				_text1 = KoStorage.GetKorString("71002");
			}
			_text = string.Format(KoStorage.GetKorString("72309"), _text1);
			idx = 5;
			
			break;
		case 11 : //First Ranking Race
			if(postList.nReCoin != 0){
				price = string.Format("X {0}", postList.nReCoin);iconName = "icon_coin";
				_text1 = KoStorage.GetKorString("71001");
			}
			_text = string.Format(KoStorage.GetKorString("72310"), _text1);
			idx = 5;
			
			break;
		case 12 : //reward Rank Race
			if(postList.nReCoin != 0){
				price = string.Format("X {0}", postList.nReCoin);iconName = "icon_coin";
				_text1 = KoStorage.GetKorString("71001");
			}
			_text = string.Format(KoStorage.GetKorString("72304"),  postList.nReCoin);
			idx = 8;
			break;
			
		}
		_text1 = string.Empty; 
		//
		
		tr = transform.GetChild(idx);
		tr.gameObject.SetActive(true);
		tr.FindChild("lbTitle").GetComponent<UILabel>().text = _text;
		//tr.FindChild("lbdes").GetComponent<UILabel>().text = string.Empty;
		tr.FindChild("gold").GetComponentInChildren<UILabel>().text = price;
		tr.FindChild("gold").GetComponentInChildren<UISprite>().spriteName = iconName;
		if(b) 	tr.FindChild("gold").FindChild("icon_coin").localScale = new Vector3(97,37,1);
		else 	tr.FindChild("gold").FindChild("icon_coin").localScale = new Vector3(37,37,1);
		return; */
	}
}
