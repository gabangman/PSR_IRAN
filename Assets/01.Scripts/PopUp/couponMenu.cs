using UnityEngine;
using System.Collections;

public class couponMenu : MonoBehaviour {
	
	public UILabel[] lbText;
	void Start () {
		lbText[0].text =KoStorage.GetKorString("72310"); // title
		lbText[1].text =string.Empty;
		lbText[2].text = string.Empty;
		transform.FindChild("lbText2").GetComponent<UILabel>().text = KoStorage.GetKorString("72321");
		//return;

		var tr = transform.FindChild("Btn_FB") as Transform;
		tr.FindChild("lb_ok").GetComponent<UILabel>().text = KoStorage.GetKorString("72507");
		tr.GetComponent<UIButtonMessage>().functionName = "OnHomePage";
		tr = transform.FindChild("Btn_NAVER");
	
		string gCountry =  EncryptedPlayerPrefs.GetString("CountryCode");
		if(string.Equals(gCountry, "KOR")==true){
			tr.gameObject.SetActive(true);
			tr.GetComponent<UIButtonMessage>().functionName = "OnNaverPage";
		}else tr.gameObject.SetActive(false);


		StartCoroutine("setContentsTexture");
	}

	void OnHomePage(){
		Application.OpenURL(GV.gInfo.HomeURL);
	}
	void OnNaverPage(){
		Application.OpenURL("http://cafe.naver.com/psracing.cafe");
	}
	
	IEnumerator setContentsTexture(){
	//	path = GV.gInfo.CouponURL;
		string url = GV.gInfo.CouponURL;
		WWW www = new WWW( url );
		yield return www;
		
		if( this == null )
			yield break;
		if( www.error != null )
		{
			Utility.Log( "load failed" );
			//AccountManager.instance.ErrorPopUp();
		}
		else
		{
			Texture2D Tex = www.texture;
			www.Dispose();
			www = null;
			transform.FindChild("Texture").GetComponent<UITexture>().mainTexture = Tex;
			Tex = null;
		}
	}
	
	
	void OnSubmit(string obj){
		Utility.Log(obj);
	}
	
	void OnCouponOk(){
		string str = lbText[2].text;
		if(Global.isNetwork) return;
		if(str.Equals(string.Empty) || str.Equals(null)) return;
	//	if(str.Length != 6) return;
		Global.isNetwork = true;
		transform.FindChild("Sprite (Check_V)").GetComponent<UIButtonMessage>().functionName = null;
		StartCoroutine(SendToCouponNumber(
			(idx)=>{
			if(idx == 0){
				var pop = ObjectManager.SearchWindowPopup() as GameObject;
				pop.AddComponent<CouponEvPopup>().SuccessPopUp(1, 1);
				var lobby = GameObject.Find("LobbyUI") as GameObject;
				lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
			}else{
				var pop = ObjectManager.SearchWindowPopup() as GameObject;
				pop.AddComponent<CouponEvPopup>().InitPopUp(idx);
			}

		},str));
		
		//	var pop = ObjectManager.SearchWindowPopup() as GameObject;
		//	pop.AddComponent<CouponEvPopup>().InitPopUp(str, obj);
		
	}
	
/*	IEnumerator SendToCouponNumber(System.Action<int> callBack, string num){
		bool bConnect = false;
		string mAPI = "game/event/sharedCoupon";//ServerAPI.Get(90054);//"game/event/coupon/"
		NetworkManager.instance.HttpFormConnect("Post", mAPI, (request)=>{
		//NetworkManager.instance.HttpGetRaceSubInfo("Get" ,"game/event/coupon/", (request)=>{
		/*	if(string.IsNullOrEmpty(request.response.Text) == true) {
				return;
			}
			//response:{"state":0,"msg":"sucess","result":{"isReward":1,"coin":10,"dollar":100,"silver":1,"gold":1,"evoCube":10},"time":1459502884}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				if(thing["result"]["isReward"].AsInt == 0){
					int a = thing["result"]["type"].AsInt;
					callBack(a);
				}else{

					int resType = thing["result"]["coin"].AsInt;
					if(resType !=0 )
						RewardComplete(2, resType);
					 resType = thing["result"]["dollar"].AsInt;
					if(resType !=0 )
						RewardComplete(0, resType);

					resType = thing["result"]["silver"].AsInt;
					if(resType !=0 )
						RewardComplete(4, resType);
					resType = thing["result"]["gold"].AsInt;
					if(resType !=0 )
						RewardComplete(5, resType);

					resType = thing["result"]["evoCube"].AsInt;
					if(resType !=0 )
						RewardComplete(6, resType);


			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			bConnect = true;
			return;

			if(status == 0){
					var pop = ObjectManager.SearchWindowPopup() as GameObject;
					pop.AddComponent<CouponEvPopup>().SuccessPopUp(1, 1);
					var lobby = GameObject.Find("LobbyUI") as GameObject;
					lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
				}
			}
			bConnect = true;
		}, num);
		
		while(!bConnect){
			yield return null;
		}
		transform.FindChild("Sprite (Check_V)").GetComponent<UIButtonMessage>().functionName = "OnCloseClick";
		lbText[2].text = string.Empty;
		Global.isNetwork = false;
	} */


	IEnumerator SendToCouponNumber(System.Action<int> callBack, string num){
		bool bConnect = false;
		int returnstatus=0;
		string mAPI = "game/event/sharedCoupon";//ServerAPI.Get(90054);//"game/event/coupon/"
		string strResult = "couponName;"+num;
		NetworkManager.instance.HttpFormConnect("Post",new System.Collections.Generic.Dictionary<string, int>(), mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
			//	재료와 에보큐브 지급을 제외한 쿠폰 지급 기능이 완료되었습니다.
			//		쿠폰 입력 성공시 state 값이 0이며 실패값은 아래와 같습니다.
			//			-1: 쿠폰 이름이 존재하지 않는 경우
			//			-2: 쿠폰 유효 기간이 지난 경우
			//			-3: 이미 아이템을 지급받은 경우
				//{"state":0,"claimedItems":[{"type":0,"value":11},{"type":1,"value":1},{"type":2,"value":2},{"type":5,"value":4},{"type":4,"value":5}],
			//"claimedMaterial":[{"matId":8610,"value":1},{"matId":8613,"value":3},{"matId":8620,"value":5}]}
			/*	int mCnt = thing["claimedItems"].Count;
				if(mCnt != 0){
					for(int i =0; i <mCnt; i++){
						int rType = thing["claimedItems"][i]["type"].AsInt;
						int value = thing["claimedItems"][i]["value"].AsInt;
						if(rType != 3)
							RewardComplete(rType, value);
					}
				}
				mCnt = thing["claimedMaterial"].Count;
				if(mCnt != 0){
					for(int i =0; i <mCnt; i++){
						int rType = thing["claimedMaterial"][i]["type"].AsInt;
						int value = thing["claimedMaterial"][i]["value"].AsInt;
					}
				}*/

				Global.gNewMsg++;
				gameRank.instance.listGift.Clear();
			}
			returnstatus = status;
			bConnect = true;
		},strResult);
		
		while(!bConnect){
			yield return null;
		}
		callBack(returnstatus);
		transform.FindChild("Sprite (Check_V)").GetComponent<UIButtonMessage>().functionName = "OnCloseClick";
		lbText[2].text = string.Empty;
		Global.isNetwork = false;
	}


	private void RewardComplete(int nType, int resValue){
		//0/1;3/2;2/3;4/4;5/5
		GameObject Lobby = GameObject.Find("LobbyUI") as GameObject;
		switch(nType){
		case 2 :{ 
			GV.myCoin = GV.myCoin+resValue;
			GV.updateCoin = -resValue;
			Lobby.SendMessage("InitTopMenuReward", SendMessageOptions.DontRequireReceiver);
		}	break;
		case 0 :{
			GV.myDollar = GV.myDollar+ resValue;
			GV.updateDollar =  -resValue;
			Lobby.SendMessage("InitTopMenuReward", SendMessageOptions.DontRequireReceiver);
		}break;
		case 3 :{
			//str = "Material";
			GV.UpdateMatCount(resValue, 1);
			myAcc.instance.account.bInvenBTN[1] = true;
			Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
		}break;
		case 4 : {
			//	str = "SilverBox";
			myAcc.instance.account.bInvenBTN[3] = true;
			GV.UpdateCouponList(0, resValue);	Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
		}break;
		case 5 : {
			//	str = "GoldBox";
			myAcc.instance.account.bInvenBTN[3] = true;
			GV.UpdateCouponList(1,resValue);	Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
		}break;
		case 6 : {
			//str = "Cube";
			GV.UpdateMatCount(8620, resValue);	Lobby.SendMessage("SetLobbyBtnAfterSeeing", SendMessageOptions.DontRequireReceiver);
			myAcc.instance.account.bInvenBTN[2] = true;
		}break;
		}
	}


	void OnCouponClick(){
		//	Application.OpenURL("https://www.facebook.com/pitinracing");
		//Application.OpenURL(GV.gInfo.HomeURL);

	}
}
