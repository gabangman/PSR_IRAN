using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using System.Linq;
using System;

public class SNSManager : MonoBehaviour {

	public static SNSManager instance { get; private set; }
	public static bool isFB{get{
			return FB.IsLoggedIn;
		}}

	public static bool isGoogle{get{
			return Social.localUser.authenticated;
		}}


	public static bool bSNS = false;
	void Awake()
	{
		instance = this;
	}


	public static void FaceBookLogin(System.Action SuccessCallback, System.Action FailedCallback){
		if(FB.IsLoggedIn) return;
			FB.Login("public_profile,email,user_friends,publish_actions", (result)=>{
			if (result.Error != null){
				Utility.Log(string.Format(" FB Login Error Response:  {0} " + result.Error));
			}
			else if (!FB.IsLoggedIn)
			{
					if(FailedCallback != null)
						FailedCallback();//FBLoginFinish();
					FailedCallback = null;

			}
			else
			{
				if(SuccessCallback != null)
					SuccessCallback();//FBLoginFinish();
				SuccessCallback = null;

				GetFaceBookmyUserProfileURL();

				if(facebookfr.Count == 0){
				//	GetFaceBookFriends();
				}
				Utility.LogWarning("Facebook login");
				AccountManager.instance.StartCoroutine("FaceBookLoginResult");

			}
			bSNS = true;
		});
	
	}
	public static void FaceBookLogOut(System.Action callback){
		FB.Logout();
		if(callback != null) 
			callback();
		callback = null;
		bSNS = true;
		EncryptedPlayerPrefs.SetString("FBUrl","None");
		var obj = GameObject.Find("LobbyUI") as GameObject;
		if(obj != null) 	obj.SendMessage("FaceBookLoginComplete",1,SendMessageOptions.DontRequireReceiver);
		else Utility.LogWarning("obj == null ");
	}
	public static string countFaceBookFriend(){
		string str = string.Empty;
		if(facebookfr.Count == 0){
			str ="friendFacebookUids;"+ "";
		}else{
			str = "friendFacebookUids;";
			for(int i =0; i < facebookfr.Count; i++){
				str += facebookfr[i].FBID;
				if(i != (facebookfr.Count-1)){
					str+=",";
				}
			}
		}
	//	Utility.LogWarning("myFB ID " + FB.UserId);
		return str;
	}



	public static string CompareFriendCount(){
		string str = string.Empty;
		if(facebookfr.Count == 0){
			str = string.Empty; 
		}else{
			//str = "friendFacebookUids;";
			str = string.Empty;
			int cnt1 = gameRank.instance.listFFR.Count-1;
			int cnt = facebookfr.Count;
			//Utility.LogWarning(cnt1);
		/*	if(gameRank.instance.listFFR.Count == 1){
				for(int i =0; i < facebookfr.Count; i++){
					string str1 = facebookfr[i].FBID;
					str +=str1;
					if(i != (facebookfr.Count-1)){
						str+=",";
					}
				}
			}else{*/

				if(cnt > cnt1){

					


					for(int i =0; i < facebookfr.Count; i++){
						string str1 = facebookfr[i].FBID;
						if(!facebookfr[i].bAddFriend){

						for(int j = 0; j <(cnt1+1); j++){
								if(str1.Equals(gameRank.instance.listFFR[j].fbUid)){
									facebookfr[i].bAddFriend = true;
									break;
								}
						}

						}else{

						}
					}
					
				
				for(int i =0; i < facebookfr.Count; i++){
					if(!facebookfr[i].bAddFriend){
						str +=  facebookfr[i].FBID+",";
					}
						
				}
					

					
				char a = str[str.Length-1];
				if(char.Equals(a, ',')== true) str = str.Substring(0,str.Length-1);
				}else{
					str = string.Empty; 
				}
		}
	//	str+="1180874048649489";
		return str;
	}

	public void DeleteGameAPP(){
		if(FB.IsLoggedIn){
			FB.API("/me/permissions", Facebook.HttpMethod.DELETE,  (result)=>{
				Utility.LogWarning("fb remove " + result);
				Application.Quit();
			});
		}else{
			Application.Quit();
		}
	}

	public IEnumerator getFaceBookFriendsAccountList(){
		bool bConnect = true;
		FB.API("me?fields=friends.fields(id,name,picture)", Facebook.HttpMethod.GET,  (result)=>{
			System.Object tempObj = Facebook.MiniJSON.Json.Deserialize(result.Text);
			FaceBookFr fb;
			Dictionary<string, System.Object> jsonDic = tempObj as Dictionary<string, System.Object>;
			/*fb = new FaceBookFr();

			mUserId = jsonDic["id"] as String;
			Dictionary<string, System.Object> mdataDic = jsonDic["picture"] as Dictionary<string, System.Object>;
			Dictionary<string, System.Object> dataDic1 = mdataDic["data"] as Dictionary<string, System.Object>;
			bool issilhouettAA = (bool)dataDic1["is_silhouette"];
			string urlAA = dataDic1["url"] as String;
			fb.FBID = FB.UserId;
			fb.FBUrl = urlAA;
			fb.FBImg = null;
			fb.issihouett = issilhouettAA;
			facebookfr.Add(fb);*/
			Dictionary<string, System.Object> mdataDic = jsonDic["friends"] as Dictionary<string, System.Object>;
			List<System.Object> dataList = mdataDic["data"] as List<System.Object>;
			if(dataList.Count == 0){
				Utility.LogWarning("Count is Zero");
				bConnect = false;
				return;
			}
			
			// Succes {"name":"\ub9e8\uac00\ubc29","picture":{"data":{"is_silhouette":false,"url":"https:\/\/scontent.xx.fbcdn.net\/hprofile-xap1\/v\/t1.0-1\/p50x50\/12342765_1520239938274317_7160296991648840889_n.jpg?oh=9021152057ffe7671004efaa1a2353a2&oe=56D90F7F"}},"friends":{"data":[{"id":"974372735930962","name":"Kim Jong","picture":{"data":{"is_silhouette":false,"url":"https:\/\/scontent.xx.fbcdn.net\/hprofile-xfa1\/v\/t1.0-1\/p50x50\/11139354_933012826733620_3650539727338477642_n.jpg?oh=f15ccfe8fbd29bd7621f2224d341265c&oe=56EB9556"}}}],"paging":{"next":"https:\/\/graph.facebook.com\/v2.3\/1466064903691821\/friends?fields=id,name,picture&access_token=CAAG1sZCZACjkgBAIW8vBps5MTZAgUyvn1Qr1HaZBEY5MozE6ERoysRdUYpZA1wZBy90C1GPturb93dYPQKheUWXZA5KhLn4WwqRt9NmZAuGAnCXCz0mKS0lDw90m878q8H2yHJUySEcDW9sgwaVUOvyIaux6Dwzc1zy36dfdoNoeUnSb7Vs2uOZCciB7UIGj6BqhxTHsm76we8wZDZD&limit=25&offset=25&__after_id=enc_AdCWOH1l3C8O6ubf9BSpg9EQXBF9mQDo19634i8uC6ZBoETvAS7IDbfaiZB23n0gFlflOprQHaNH7nQ31U31F2zDt9"},"summary":{"total_count":2}},"id":"1466064903691821"}
			
			foreach(System.Object dataObject in dataList){
				fb = new FaceBookFr();
				Dictionary<string, System.Object> dataDic = dataObject as Dictionary<string, System.Object>;
				string id = dataDic["id"] as String;
				string Name = dataDic["name"] as String;
				fb.FBID = id;
				fb.FBName = Name;
				Dictionary<string, System.Object> pictureDic = dataDic["picture"] as Dictionary<string, System.Object>;
				Dictionary<string, System.Object> pictureDataDic = pictureDic["data"] as Dictionary<string, System.Object>;
				bool issilhouett = (bool)pictureDataDic["is_silhouette"];
				string url = pictureDataDic["url"] as String;

				fb.issihouett = issilhouett;
				if(fb.issihouett){
					fb.FBUrl = "None";
				}else{
					fb.FBUrl = url;

				}
				fb.FBImg = null;
				facebookfr.Add(fb);
				Utility.LogWarning("fb name " + fb.FBName);
			}
			bConnect = false;
			//SendFaceBookFriendToServer();
		});
		while(bConnect){
			yield return null;
		}
		Utility.LogWarning("fb count :  " + facebookfr.Count);
		yield return true;
	}

	public static string[] getFacebookFriendsInfo(string id){
		FaceBookFr fr = facebookfr.Find((obj)=> obj.FBID == id);
		string[] strs = new string[2];
		strs[0] = fr.FBName;
		if(!fr.issihouett){
			strs[1] = fr.FBUrl;
		}else{
			strs[1] ="None";
		}
		return strs;
	}


	public static void SendInviteMessage(Facebook.FacebookDelegate onCallback, string[] frs){
		//System.Text.StringBuilder sb = new System.Text.StringBuilder();
		//sb.Length = 0;

		string msg = string.Empty;
		if(Application.isEditor){
			msg = "친구초대";
		}else{
			msg = KoStorage.GetKorString("72613");
		}
		FB.AppRequest(
			msg,
			frs,//str.Split(','),
			null,
			null,
			null,
			"",
			"",
			onCallback 
			);
		
		return;
	}
	public static void SendInviteMessageEditorTest(Facebook.FacebookDelegate onCallback){
		//System.Text.StringBuilder sb = new System.Text.StringBuilder();
		//sb.Length = 0;
		int cnt = facebookinvite.Count;
		string[] sb = null;
		string str = null;
		List<string> sbList = new List<string>();
		for(int i = 0; i < cnt ; i++){
			if(facebookinvite[i].isCheck){
				sbList.Add(facebookinvite[i].inviteToken);
				str += facebookinvite[i].inviteToken + ",";
			}

		}
		sb = sbList.ToArray();
		string msg = string.Empty;
		if(Application.isEditor){
			msg = "친구초대";
		}else{
			msg = KoStorage.GetKorString("72817");
		}
		FB.AppRequest(
			msg,
			sb,//str.Split(','),
			null,
			null,
			null,
			"",
			"",
			onCallback
			);

		return;
		 string FriendSelectorTitle = "";
		 string FriendSelectorMessage = "친구초대 테스트";
		string FriendSelectorFilters = "[\"app_non_users\"]";
		 string FriendSelectorData = "{}";
		 string FriendSelectorExcludeIds = "";
		 string FriendSelectorMax = "";
		int? maxRecipients = null;
		if (FriendSelectorMax != "")
		{
			try
			{
				maxRecipients = Int32.Parse(FriendSelectorMax);
			}
			catch (Exception e)
			{
				//status = e.Message;
			}
		}
		
		// include the exclude ids
		string[] excludeIds = (FriendSelectorExcludeIds == "") ? null : FriendSelectorExcludeIds.Split(',');
		List<object> FriendSelectorFiltersArr = null;
		if (!String.IsNullOrEmpty(FriendSelectorFilters))
		{
			try
			{
				FriendSelectorFiltersArr = Facebook.MiniJSON.Json.Deserialize(FriendSelectorFilters) as List<object>;
			}
			catch
			{
				throw new Exception("JSON Parse error");
			}
		}
		FB.AppRequest(
			FriendSelectorMessage,
			null,
			FriendSelectorFiltersArr,
			excludeIds,
			maxRecipients,
			FriendSelectorData,
			FriendSelectorTitle,
			(result)=>{
			//Utility.LogWarning("SendInviteMessage");
			if (result.Error != null){
				Utility.Log(string.Format(" FB Login Error Response:  {0} " + result.Error));
			}
			if (result != null)
			{

			}
			//sb.Length = 0;		
		});

		return;
		FB.AppRequest(
			"Invite Friends",
			sb,
			null,
			null,
			maxRecipients,
			"",
			"",
			(result)=>{
			Utility.LogWarning("SendInviteMessage");
			if (result.Error != null){
				Utility.Log(string.Format(" FB Login Error Response:  {0} " + result.Error));
			}
			if (result != null)
			{
			//	var responseObject =  Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string, object>;
			//	IEnumerable<object> objectArray = (IEnumerable<object>)responseObject["to"];
				
			//	if (objectArray.Count() >= 3)
			//	{
					// 3 facebook invitations send. Give user a free item or bonus etc.
			//	}
			}
			//sb.Length = 0;		
		});
		//Utility.LogWarning(sb.ToString());

//		FB.API("/me/invitable_friends?excluded_ids=["+sb.ToString()+"]",Facebook.HttpMethod.GET, (result)=>{
//			Utility.LogWarning("SendInviteMessage");
//			sb.Length = 0;		
//		});
	
		//'AVmGpCwAKIcIfaNhbrYZVmX39yQavLip8qLv8rXkIKZEO4tSHdh0-u5Tn-SgXfFprW_Mn1jj-LisbJ5URDX5dXsYr_oFQUT-WnoyRbj9UN9WZw'
	}

	public static void SendFaceBookFriendToServer(){
		Utility.LogWarning("SendFaceBookFriendToServer");
	}

	public static string mProfileURL;
	//public static string mUserId;
	public static void GetFaceBookmyUserProfileURL(){
		FB.API("me?fields=picture,id,name", Facebook.HttpMethod.GET,  (result)=>{
			System.Object tempObj = Facebook.MiniJSON.Json.Deserialize(result.Text);
			Dictionary<string, System.Object> jsonDic = tempObj as Dictionary<string, System.Object>;
			Dictionary<string, System.Object> mdataDic = jsonDic["picture"] as Dictionary<string, System.Object>;
			Dictionary<string, System.Object> dataDic1 = mdataDic["data"] as Dictionary<string, System.Object>;
			bool issilhouettAA = (bool)dataDic1["is_silhouette"];
		//	 mUserId = jsonDic["id"] as String;
			string facebookUserName = jsonDic["name"] as String;
			if(issilhouettAA) {
				mProfileURL = "User_Default";
				EncryptedPlayerPrefs.SetString("FBUrl",mProfileURL);
				return;
			}
			else {
				mProfileURL =  dataDic1["url"] as String;
				EncryptedPlayerPrefs.SetString("FBUrl",mProfileURL);
				AccountManager.instance.StartCoroutine("myProfileLoad");
			}
		//	Utility.LogWarning(facebookUserId);
		//	Utility.LogWarning(facebookUserName);Utility.LogWarning(mProfileURL);
		});
		//Succes {"picture":{"data":{"is_silhouette":false,"url":"https:\/\/scontent.xx.fbcdn.net\/hprofile-xap1\/v\/t1.0-1\/p50x50\/12342765_1520239938274317_7160296991648840889_n.jpg?oh=9021152057ffe7671004efaa1a2353a2&oe=56D90F7F"}},"id":"1466064903691821"}
	
	}

	public static void GetFaceBookFriendsField(){
		// /me?fields=name,picture,friends.fields(name,picture)
		FB.API("me?fields=name,picture,friends.fields(id,name,picture)", Facebook.HttpMethod.GET,  (result)=>{
			System.Object tempObj = Facebook.MiniJSON.Json.Deserialize(result.Text);
			FaceBookFr fb;
	//		fb = new FaceBookFr();
			Dictionary<string, System.Object> jsonDic = tempObj as Dictionary<string, System.Object>;
	//		string mName = jsonDic["name"] as String;
	//		fb.FBName = mName;
			Dictionary<string, System.Object> mdataDic = jsonDic["picture"] as Dictionary<string, System.Object>;
			Dictionary<string, System.Object> dataDic1 = mdataDic["data"] as Dictionary<string, System.Object>;
	//		bool issilhouettAA = (bool)dataDic1["is_silhouette"];
	//		string urlAA = dataDic1["url"] as String;
	//		fb.FBID = FB.UserId;
	//		fb.FBUrl = urlAA;
	//		fb.FBImg = null;
	//		fb.issihouett = issilhouettAA;
	//		facebookfr.Add(fb);
			mdataDic = jsonDic["friends"] as Dictionary<string, System.Object>;
			List<System.Object> dataList = mdataDic["data"] as List<System.Object>;
			if(dataList.Count == 0){
				Utility.LogWarning("Count is Zero");
				return;
			}

			// Succes {"name":"\ub9e8\uac00\ubc29","picture":{"data":{"is_silhouette":false,"url":"https:\/\/scontent.xx.fbcdn.net\/hprofile-xap1\/v\/t1.0-1\/p50x50\/12342765_1520239938274317_7160296991648840889_n.jpg?oh=9021152057ffe7671004efaa1a2353a2&oe=56D90F7F"}},"friends":{"data":[{"id":"974372735930962","name":"Kim Jong","picture":{"data":{"is_silhouette":false,"url":"https:\/\/scontent.xx.fbcdn.net\/hprofile-xfa1\/v\/t1.0-1\/p50x50\/11139354_933012826733620_3650539727338477642_n.jpg?oh=f15ccfe8fbd29bd7621f2224d341265c&oe=56EB9556"}}}],"paging":{"next":"https:\/\/graph.facebook.com\/v2.3\/1466064903691821\/friends?fields=id,name,picture&access_token=CAAG1sZCZACjkgBAIW8vBps5MTZAgUyvn1Qr1HaZBEY5MozE6ERoysRdUYpZA1wZBy90C1GPturb93dYPQKheUWXZA5KhLn4WwqRt9NmZAuGAnCXCz0mKS0lDw90m878q8H2yHJUySEcDW9sgwaVUOvyIaux6Dwzc1zy36dfdoNoeUnSb7Vs2uOZCciB7UIGj6BqhxTHsm76we8wZDZD&limit=25&offset=25&__after_id=enc_AdCWOH1l3C8O6ubf9BSpg9EQXBF9mQDo19634i8uC6ZBoETvAS7IDbfaiZB23n0gFlflOprQHaNH7nQ31U31F2zDt9"},"summary":{"total_count":2}},"id":"1466064903691821"}
	
			foreach(System.Object dataObject in dataList){
				fb = new FaceBookFr();
				Dictionary<string, System.Object> dataDic = dataObject as Dictionary<string, System.Object>;
				string id = dataDic["id"] as String;
				string Name = dataDic["name"] as String;
				fb.FBID = id;
				fb.FBName = Name;
				Dictionary<string, System.Object> pictureDic = dataDic["picture"] as Dictionary<string, System.Object>;
				Dictionary<string, System.Object> pictureDataDic = pictureDic["data"] as Dictionary<string, System.Object>;
				bool issilhouett = (bool)pictureDataDic["is_silhouette"];
				string url = pictureDataDic["url"] as String;
				fb.issihouett = issilhouett;
				fb.FBUrl = url;
				fb.FBImg = null;
				fb.bAddFriend = false;
				facebookfr.Add(fb);
			}

			SendFaceBookFriendToServer();
		});
	}

	public static void GetFaceBookFriends(){
		/*FB.API("/me/friends", Facebook.HttpMethod.GET,  (result)=>{
			Utility.LogWarning(" " + result.Text);
			System.Object tempObj = Facebook.MiniJSON.Json.Deserialize(result.Text);
			Dictionary<string, System.Object> jsonDic = tempObj as Dictionary<string, System.Object>;
			
			List<System.Object> dataList = jsonDic["data"] as List<System.Object>;
			if(dataList.Count == 0){
				Utility.LogWarning("Count is Zero");
				//Global.isNetwork = false;
				return;
			}
			FaceBookFr fb;
			foreach(System.Object dataObject in dataList){
				Dictionary<string, System.Object> dataDic = dataObject as Dictionary<string, System.Object>;
				string friendsID = dataDic["id"] as String;
				string Name = dataDic["name"] as String;
				fb = new FaceBookFr();
				fb.FBID = friendsID;
				fb.FBName = Name;
				fb.FBUrl = "https://graph.facebook.com/" + friendsID + "/picture?width=64&height=64"; 
				fb.FBImg = null;
				facebookfr.Add(fb);
				Utility.LogWarning("fb.FBID " + fb.FBID );
			}
			//https://graph.facebook.com/948040391897530/picture?width=64&height=64
			//WWW url = new WWW("https" + "://graph.facebook.com/" + userID + "/picture?width=64&height=64"); 
			//Texture2D textFb = new Texture2D(64, 64, TextureFormat.DXT1, false);
			//yield return url;
			//url.LoadImageIntoTexture(textFb);
			
			
			SendFaceBookFriendToServer();
			//	Global.isNetwork = false;
		});*/
	}

	public static void GetFaceBookInvitableFriends_Limit(){
		FB.API("/me?fields=invitable_friends.limit(250)", Facebook.HttpMethod.GET, (result)=>{
			System.Object tempObj = Facebook.MiniJSON.Json.Deserialize(result.Text);
			Utility.LogWarning(" " + result.Text);
			Dictionary<string, System.Object> jsonDic = tempObj as Dictionary<string, System.Object>;
			Dictionary<string, System.Object> jsonDic1 =  jsonDic["invitable_friends"]  as Dictionary<string, System.Object>;
			List<System.Object> dataList = jsonDic1["data"] as List<System.Object>;
			if(dataList.Count == 0){
				Utility.LogWarning("Count is Zero");
				Global.isNetwork = false;
				return;
			}
			FaceBookInvitable fb;// = new FaceBookInvitable();
			foreach(System.Object dataObject in dataList){
				fb = new FaceBookInvitable();
				Dictionary<string, System.Object> dataDic = dataObject as Dictionary<string, System.Object>;
				string inviteToken = dataDic["id"] as String;
				string Name = dataDic["name"] as String;
				fb.inviteToken = inviteToken.Trim();
				fb.FBName = Name;
				Dictionary<string, System.Object> pictureDic = dataDic["picture"] as Dictionary<string, System.Object>;
				Dictionary<string, System.Object> pictureDataDic = pictureDic["data"] as Dictionary<string, System.Object>;
				bool issilhouett = (bool)pictureDataDic["is_silhouette"];
				string url = pictureDataDic["url"] as String;
				fb.issihouett = issilhouett;
				fb.FBUrl = url;
				fb.isCheck = true;
				facebookinvite.Add(fb);
				//Utility.LogWarning("" + fb.inviteToken );
			}
			Utility.LogWarning("C " + facebookinvite.Count);
			Global.isNetwork = false;
		});
		
	}
	/*    if (result != null)                                                                                                        
    {                                                                                                                          
        var responseObject = Json.Deserialize(result.Text) as Dictionary<string, object>;                                      
        object obj = 0;                                                                                                        
        if (responseObject.TryGetValue ("cancelled", out obj))                                                                 
        {                                                                                                                      
            Util.Log("Request cancelled");                                                                                  
        }                                                                                                                      
        else if (responseObject.TryGetValue ("request", out obj))                                                              
        {                
      AddPopupMessage("Request Sent", ChallengeDisplayTime);
            Util.Log("Request sent");                                                                                       
        }                                                                                                                      
    }                                                               
    */


	public static void GetFaceBookInvitableFriends(){
		if(facebookinvite.Count !=0 ) {
			Global.isNetwork = false;
			return;
		}
		FB.API("v2.4/me/invitable_friends", Facebook.HttpMethod.GET, (result)=>{
			System.Object tempObj = Facebook.MiniJSON.Json.Deserialize(result.Text);
			//Utility.LogWarning(" " + result.Text);
			/*
		{"invitable_friends":{"data":[{"id":"AVkKAVuER36vOybRmDdXBho73unc_m3tyIjWgQTUMyl1re1USIg6105iogD2QGjXeWcZc6r4b14LLVQoyRPmyAEp5C75X7Ou2OQJTSZ1SDSJYQ","name":"Beomhee Kang","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xtp1\/v\/t1.0-1\/p50x50\/11060910_971204929577290_8157690619734769195_n.jpg?oh=798bfd85d187d9c04dda006846e7f740&oe=56272334&__gda__=1444151539_4640b0930f6c0e02404189a3e8aa7300"}}},{"id":"AVl3H2UNvL9x-1of7qjgfQsLfKdSLYigUoTODwbYGh8ANWPvfmj8rjNQQvyPOMuF8hWFknrK5jC3lc1yxSmFuQPVeJuhpDvyx3ggkyqjcc9ejA","name":"SangYeon Lee","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xtf1\/v\/t1.0-1\/c2.0.50.50\/p50x50\/10295779_767435699943048_3001571989553284297_n.jpg?oh=80abe121744e11bf8686582cd549e97e&oe=561ECED2&__gda__=1444069642_206a6ca8a3af1cda244c37f20336e127"}}},{"id":"AVlLd4nZFEyilJ7hHv4o7_NubTqe4T06easkwDxTIU04pfi9hEh0FjTj8YPNA9ySlcZqavrwIPubsG5hEE-n5_0aBWNIvB1M_oYlD8BrTZ9hGA","name":"Kyoung Shik Shin","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xpt1\/v\/t1.0-1\/p50x50\/11666096_1017920974905954_7655627391980010276_n.jpg?oh=b3502439406c2023addfacf6ffcbdf2a&oe=56301A07&__gda__=1444447945_411ccb56c474150acd1159fbe3505b00"}}},{"id":"AVm5UPNzKZsZRDZEdAgodkureB3BO3JEgidaL7BGUfE3QXnomuYJhznjkzUAS8x6-RUugh2dcxcIBRjWjxhIzYSbCbGVh9IGhNvSo_YwwgEAkA","name":"Hongsik Park","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfa1\/v\/t1.0-1\/c153.34.414.414\/s50x50\/378748_260579123995651_216408554_n.jpg?oh=c4ab26b726811c2327ac651a809c6601&oe=5614549F&__gda__=1444717487_459600bd035d60aa43797832397dbced"}}},{"id":"AVmXc0m3eiE5POyvMu26I4Kk6DEXG7pA8s2y4rZbG5FuxMRuC6lEjbtPzo8zezr6OS-u9E9CGJo2lJ74M5irJOmeuxq1XesMc0eK8Rhga5jgyA","name":"Min-chul Kim","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xaf1\/v\/t1.0-1\/c73.21.264.264\/s50x50\/261344_164695370263912_4795695_n.jpg?oh=8d77eae93f1463d138e31e4f4bc0dd04&oe=565B37D8&__gda__=1445636693_c00d917a1abfcdce2574be2fb4d3eca9"}}},{"id":"AVnJb4wm7TDG1MIs1Gr0nZHsKc9qosO-XXIyNlzPeven7pZvKsx_-oijHlhdGy5FxiKcexVdGAGqcgljKbR7VLhNLed_7lgjK5Uq3Bm-wB0nVQ","name":"Kwang Rok An","picture":{"data":{"is_silhouette":true,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfp1\/v\/t1.0-1\/c15.0.50.50\/p50x50\/10354686_10150004552801856_220367501106153455_n.jpg?oh=02187c1687142ef2b5f0a0e5cdb674ea&oe=5621DD2F&__gda__=1445367769_c07f7d1a5e2ce358f114e4f7c18dfe01"}}},{"id":"AVmXkubgWkGMpMCcKr0r8XDQYlJFLs2kTFYhOApllSD8jpadJp7Zl6PEAhjwR5UOwvLYND-cYfmxjRWhkp9kgDRKZG8A5lHhwXp99By8MqoXgQ","name":"SeongYeon Won","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xta1\/v\/t1.0-1\/p50x50\/11390306_1067804763248542_5280883806205810631_n.jpg?oh=bff0ce5157e57d343944379904def816&oe=561660D0&__gda__=1448813517_bd9dde820af5f6d00f355c8bfca50dcf"}}},{"id":"AVlv2ppf6wx2GYwC7U6kzKG6e3nkBDxCvjTA0mhnmvrO8jWcvMhJH2GxmK0l0MqJQ6seOI1Ln2zOw3PmmNx-D0ZO0eqnXa4l73_VCaIdlRtHrw","name":"Sang-Yeon Jeon","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfp1\/v\/t1.0-1\/p50x50\/10930550_10205165050033289_2410129091382527322_n.jpg?oh=f08046a1ba008948dc36577eef18a9c1&oe=5627091D&__gda__=1445308022_4d3c93082138a997a8927bbc356b4784"}}},{"id":"AVnY1zAfs4fwgh8g0DP43-s9EXfIPcgQ9Z7AXn9E8f0H0IcXwmKdOf9Y-9Hh0Ju33R-b7nnTkcrylvSWZqRVADzyWamVjWdoNo8TMdPiCWjt3Q","name":"Doyon Kim","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xaf1\/v\/t1.0-1\/c46.46.578.578\/s50x50\/29299_429339632462_2840083_n.jpg?oh=75f653a44b88fd75a7d1f6d357bb3e5a&oe=56124CEB&__gda__=1445349199_2a83ad7aff288ffa289e11a3505d3c7f"}}},{"id":"AVluZNvfQQs3HomTJZ2wJ8sB4ivuGzsAgNgCAlUtrFy-AyLeNXnrjoOtZhgsaB6M1zyUgwYo6r9ET__CYeIMQ1Xrhakpzwro3lIMI9kONuHs5w","name":"\uae40\ub355\uae30","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xpf1\/v\/t1.0-1\/c17.17.216.216\/s50x50\/283277_200521396668860_330603_n.jpg?oh=2b487203b1af36bf23d8f45e77672e41&oe=561BFFAA&__gda__=1445038337_a732bff5a7aecb09740902b61d8f9431"}}},{"id":"AVkmVRa9nKhbWREcqsik3smxnXf-QEk1_vgY90PALy71gwcqZskOKYGNi6-snOlrxjj2JhF8geN4x3laOS5GhXasqRk0MU18bGgHBIAs9aHqgA","name":"Jae Seok Han","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfp1\/v\/t1.0-1\/p50x50\/10521088_871533222913419_2465697393888448558_n.jpg?oh=012a1bf73b25682438d7f5b5a6b6f145&oe=562DCE6E&__gda__=1443879688_e1bfa0f292c4d8b67bdd9d0dd28fa5b9"}}},{"id":"AVn2tFwk6Z5o5cAeJTiGRp6zGssHoZixdIZKzOzI7yr24F78AQDHtZtvtwcMpab_dL2UkOkjjbS6C3b0knFeYXfOjBPc6-geCpRgkQLQIYvZCA","name":"Sae Eun Park","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xpt1\/v\/t1.0-1\/p50x50\/10425177_10204524396731024_6483504206281627388_n.jpg?oh=8fe49c1a62eef124133090247928bfb5&oe=562A7DB6&__gda__=1445718987_dba29be0658204f3fc29dc592269314a"}}},{"id":"AVm79YOBRYCUu1D974iZD1_iQz3la2sbD8DlEmHxrxxo_UPi0b2zrpPfytw0Mu9aK8FKhToI5UdJK7XlqFxGBb_j9gX19S_vLg5Uwuo5o34mGQ","name":"TaeHyoung Kim","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfa1\/v\/t1.0-1\/c15.15.183.183\/s50x50\/399585_316258155080861_1059208674_n.jpg?oh=10f9c55f516fd496cc46461be80b4804&oe=562B85DE&__gda__=1445789678_fc17edbfb08c60f5c84545ab21b531ef"}}},{"id":"AVk0FxNlRwpmR9kECZgMKAw4J4Uyrhcc1yjswk919LSiKMePBXhwW6U4p6ZmfY-7md1ljXlTTA9UFZXMwrpgs_8Y4ejI9Ih7bPtR7ZmZfaacbg","name":"Juwha Lee","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xaf1\/v\/t1.0-1\/c37.37.466.466\/s50x50\/249747_227190590642500_1869691_n.jpg?oh=f51c54128c68b6858fdbe6c5ed981016&oe=5623F6AC&__gda__=1445358625_2c4db445fac4fe94705fd95a2bb17d2b"}}},{"id":"AVk-tEHbuaidvROkB1BBhceha693tMNW8Dz7zqzLk4CVjUGIOiwQ58zHlYFi_1S_8apztY79aXO9i66h3lXsALUlNKvV6zJq_S2bIpDVssCIwQ","name":"Myoung Heon O","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xpa1\/v\/t1.0-1\/c12.13.155.155\/s50x50\/1656291_641249922578120_584217123_n.jpg?oh=fc5492a4a6dfc5d28ad7aa78327e1dd7&oe=56298C90&__gda__=1445737729_75f39ea3e93e56bd9e6c9c9d2e2fdce0"}}},{"id":"AVlDP18AdBiBwS-UBfQunP17IDMoTNpYk4UuQf5jIAMUEWKAzVYHa7IK9Hj_4k3WeqBCsN_Ylwn5w6wT11GRNcgphl5fgAjJglWZu4iDl5VI6Q","name":"GilSup Song","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-prn2\/v\/t1.0-1\/c32.32.403.403\/s50x50\/254512_224966047531045_1269623_n.jpg?oh=29ff31422138d641809de37276f70454&oe=560E7439&__gda__=1445534206_33c6122b4d2e40028a344d1749e4ab42"}}},{"id":"AVmJBxNYtadl9wu-8guOB97MbTgDkJaqUlmy0DnpFKOWtxW4nd27vcOzD6RCoRngmgOSggzMaajcts3G742uK6lI97AD0PYe4jyTNezoGhQdxA","name":"SeJoon Park","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xtf1\/v\/t1.0-1\/p50x50\/1907686_10206804152121053_6074698675859872939_n.jpg?oh=a0a9060c1e4e4605fce6e76fb7c1ba3c&oe=5627A894&__gda__=1448696793_ad91997c8d32d0831bb0e80e4c731d35"}}},{"id":"AVmBxaIM1iS0lhUgkagcGzKj5fPud2OWk5hZ62XUSXQnk_thOFtNa9WvAqc3dl8TQC5FJB_Wij4nkUdQxEwN9YNK2fvaEbrYHqptPugNPzRHkw","name":"Whon Namkoong","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfa1\/v\/t1.0-1\/p50x50\/10906224_10152893178579404_3570261495688424636_n.jpg?oh=c820d08c50e5f4064840057394ee512c&oe=561B3D24&__gda__=1445819565_3a99b3b261a904f124060ad907f83d64"}}},{"id":"AVn_c-KuACUCJnAuEzs7rv0fBYN2zWfiSv9yy2cmbsJ7z3QPxtf-5QRV9SU3iPm39OI3pqOvUK7fNQc6h4E4HUqCAc-PzaSjPLaa7txP5CLwzg","name":"HyunWook Chun","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xpa1\/v\/t1.0-1\/c0.4.50.50\/p50x50\/11221758_1001145139930122_4036714310246595443_n.jpg?oh=baa956e39a09865bdc3a7e2d7742c435&oe=5614811F&__gda__=1444808182_5b900d486ae9b023d4eaf48e89fe520a"}}},{"id":"AVnqNH2C-qcFh6oVm8NMqi-rNWT8aMkv1s1wzzHZ3elPn-fJGF_Ubf9Idvl0so8607nSOzPp9u1LccebU_P78BEEqKOEyfotyqA6tpJ9taYd8A","name":"Han Joo Lee","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfp1\/v\/t1.0-1\/p50x50\/10616259_10202962554572788_5302549824532068113_n.jpg?oh=d31f60c9db98c21e36fc0d58189b9a26&oe=56190673&__gda__=1444510744_7e52f073a966eb3ab754c5961735ec7a"}}}],"paging":{"cursors":{"before":"QVZArLVh0MjZA0NWIyODZABTVRJOXJCbTlRX1JFdS0yT2hzUFZAQVTJlbUZAyZAi1IVE5xMDBfRG5neDU5ekMxVjQyQjYxdVlBT01ySXVnc2pIR0RuSFlyMmFJLUNGemItLVdqUndjcjJfMlM5YkRLX1EZD","after":"QVZAtMUJMODhCM1JkeUVpZAHY1NV9va2lBTEhWZAC1jT1RGMVNiaXdJOWxXZAUQ4ZAC1mZAThnSFNiQktqYlVOSnJCaGRzdnBhSWUyMlRHWGdLQ2ZAET1QtWU43SG0tR2FFcndWSkZAONDV1WHFsQWxISncZD"},"next":"https:\/\/graph.facebook.com\/v2.3\/974372735930962\/invitable_friends?access_token=CAAG1sZCZACjkgBAFr3qXVZBTldclRq02DxxPV9eQ5e1323J0ZCqEhXJNWOZBbh8NPVsvAvgMrjhx3EIFyehVNhBQghQQ2DThkyZCrgI48r0Er4GtA7CrWzXCG7OHLm5qW4N6BFrRiZAEIE3YhDwVchUGJD4B4hpMqLY9UpC7ObquqMfVj76vM4qcSPwG8H1lpanNAHLOvjIkw6LHIgkbgC5&limit=20&after=QVZAtMUJMODhCM1JkeUVpZAHY1NV9va2lBTEhWZAC1jT1RGMVNiaXdJOWxXZAUQ4ZAC1mZAThnSFNiQktqYlVOSnJCaGRzdnBhSWUyMlRHWGdLQ2ZAET1QtWU43SG0tR2FFcndWSkZAONDV1WHFsQWxISncZD"}},"id":"974372735930962"}
		UnityEngine.Debug:LogWarning(Object)
				SNSManager:<GetFaceBookInvitableFriends>m__F4(FBResult) (at Assets/01.Scripts/Singleton/Network/SNSManager.cs:309)
					Facebook.<Start>c__Iterator0:MoveNext()


		{"data":[{"id":"AVlNihh3cSIKk-IbLipmaHp-3ZznNo9FZDdDQF9xWBUn8DzUcWyQhDtXpUJiP8CGDgsEt4f2CIMpjYxxtx6u_cpn4zxktLrjfRwt06VHHzZAPw","name":"Beomhee Kang","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xtp1\/v\/t1.0-1\/p50x50\/11060910_971204929577290_8157690619734769195_n.jpg?oh=798bfd85d187d9c04dda006846e7f740&oe=56272334&__gda__=1444151539_4640b0930f6c0e02404189a3e8aa7300"}}},{"id":"AVnk802NO3zCJKF2mkjyGyjibkvKIyiwAwN5DEgVSdJG5fDLqKdyAD1W91HyqQxrFuhwO0FADVkijW5UP3SBlq5FgaN8M_elPajMNWSC7bUBFg","name":"SangYeon Lee","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xtf1\/v\/t1.0-1\/c2.0.50.50\/p50x50\/10295779_767435699943048_3001571989553284297_n.jpg?oh=80abe121744e11bf8686582cd549e97e&oe=561ECED2&__gda__=1444069642_206a6ca8a3af1cda244c37f20336e127"}}},{"id":"AVn-4UsKGoVSrTGN9nr0c8V0Px4WJYGD_HY1crZUOg7SlLA7ViFWstATOE7TwdqpsJ6M-ZXEksaCzlf70PVFyP8TJPCAhAJs2P95RBlNyKuVkg","name":"Kyoung Shik Shin","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xpt1\/v\/t1.0-1\/p50x50\/11666096_1017920974905954_7655627391980010276_n.jpg?oh=b3502439406c2023addfacf6ffcbdf2a&oe=56301A07&__gda__=1444447945_411ccb56c474150acd1159fbe3505b00"}}},{"id":"AVkIjwMNVyPQg1NAatCi5xER58XfGW-3Uy3uf78djxaF-6dbtisf1tJdNy_4t4ftjZQzP4aT5_yt2k8vMYnkou3K6HN8MnGicQBjUw6FoHBSRA","name":"Hongsik Park","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfa1\/v\/t1.0-1\/c153.34.414.414\/s50x50\/378748_260579123995651_216408554_n.jpg?oh=c4ab26b726811c2327ac651a809c6601&oe=5614549F&__gda__=1444717487_459600bd035d60aa43797832397dbced"}}},{"id":"AVnbjaC2JcGfgJkD8-yMp6XgEtfs2c5Sb1VWhCeOCSBuHJXBSgsY8kWx4xMsq3RjG3Ls5FBMn9lzRlqwFJWIMsHqSZu-_3_lCbdxt8hIE53RCw","name":"Min-chul Kim","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xaf1\/v\/t1.0-1\/c73.21.264.264\/s50x50\/261344_164695370263912_4795695_n.jpg?oh=8d77eae93f1463d138e31e4f4bc0dd04&oe=565B37D8&__gda__=1445636693_c00d917a1abfcdce2574be2fb4d3eca9"}}},{"id":"AVkCPTaJ6-z578LS6OzxD2-M5XwU-9m9JOkI7UaZHu99Wk4GlqqV8nypIOPnpk31e-UzNArgy2WdcIHA3WnV-_grXYwtDh-t3CDON75V9f5t1g","name":"Kwang Rok An","picture":{"data":{"is_silhouette":true,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfp1\/v\/t1.0-1\/c15.0.50.50\/p50x50\/10354686_10150004552801856_220367501106153455_n.jpg?oh=02187c1687142ef2b5f0a0e5cdb674ea&oe=5621DD2F&__gda__=1445367769_c07f7d1a5e2ce358f114e4f7c18dfe01"}}},{"id":"AVmfBgPkpOcwwDFBg6K7NaVJ2RSszTdWZe_S9ltHWZ2JiWOhLyVwSGhFqD9t8FWNQnK99pu9b9F76bZ3ilZlrkStMlVC0Xq3S80Gt6kz6ZBs9A","name":"SeongYeon Won","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xta1\/v\/t1.0-1\/p50x50\/11390306_1067804763248542_5280883806205810631_n.jpg?oh=bff0ce5157e57d343944379904def816&oe=561660D0&__gda__=1448813517_bd9dde820af5f6d00f355c8bfca50dcf"}}},{"id":"AVlz1XR__PEqiZrzVK1RN0wA3rg9j46DV4hSuCeHbBtlb6YUMYLhKOTRbdqyH7hw0sA87NaeD9leqOQDq1T1LDHrz_UboMXikgfjQSHod4nzDA","name":"Sang-Yeon Jeon","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfp1\/v\/t1.0-1\/p50x50\/10930550_10205165050033289_2410129091382527322_n.jpg?oh=f08046a1ba008948dc36577eef18a9c1&oe=5627091D&__gda__=1445308022_4d3c93082138a997a8927bbc356b4784"}}},{"id":"AVkGEI5V3FbALGazY0J0d76v9OkeMGdPdbBaiTsutcK1pUX_FgGJ94I4TvaHc6XHgZPdS25Q-RxX-eKwxINiDFawIqJNByQWe1nz8IhmJ8m7Ow","name":"Doyon Kim","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xaf1\/v\/t1.0-1\/c46.46.578.578\/s50x50\/29299_429339632462_2840083_n.jpg?oh=75f653a44b88fd75a7d1f6d357bb3e5a&oe=56124CEB&__gda__=1445349199_2a83ad7aff288ffa289e11a3505d3c7f"}}},{"id":"AVkgsqTrHN0UF7wfzl3CStRWFZ4BA_eDUK04_WES4wqzmmJbpGtztg7rqCOuB1ZTb1EB8UnMD5njOAKOIqVAY6lpqefRK45DQAKPhshiNWfC5A","name":"\uae40\ub355\uae30","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xpf1\/v\/t1.0-1\/c17.17.216.216\/s50x50\/283277_200521396668860_330603_n.jpg?oh=2b487203b1af36bf23d8f45e77672e41&oe=561BFFAA&__gda__=1445038337_a732bff5a7aecb09740902b61d8f9431"}}},{"id":"AVn7_Leismepf3_AvGhTAeZXnw7zG6rfkh7SLlLL2c8IEm0PmcUZaSFP8ojKhbBXyD2ELVbEX7S842BY0mBixcIz_R_J9ifH1FjQd5UyZU2dnw","name":"Jae Seok Han","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfp1\/v\/t1.0-1\/p50x50\/10521088_871533222913419_2465697393888448558_n.jpg?oh=012a1bf73b25682438d7f5b5a6b6f145&oe=562DCE6E&__gda__=1443879688_e1bfa0f292c4d8b67bdd9d0dd28fa5b9"}}},{"id":"AVmo9DLxe3-xU8eTywhxof3SR_9IUAP5BWUULPHrc440yXlhV_-HmljpKcnXtb-d_jZ24FVqVh7N9n0UIRGZ967lyUm85hMBnr8uyvCDqAKwVw","name":"Sae Eun Park","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xpt1\/v\/t1.0-1\/p50x50\/10425177_10204524396731024_6483504206281627388_n.jpg?oh=8fe49c1a62eef124133090247928bfb5&oe=562A7DB6&__gda__=1445718987_dba29be0658204f3fc29dc592269314a"}}},{"id":"AVmik-7f-dUmcMdKRxI-QHT0JzhASjlKmAiFhWTCtzUcOUnPCNWU6HoBIpEe-6ZxoZa2IfRXShbVUt2I1JXw30LiIoYb3_sQs8zR9Ahc1HPBpg","name":"TaeHyoung Kim","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfa1\/v\/t1.0-1\/c15.15.183.183\/s50x50\/399585_316258155080861_1059208674_n.jpg?oh=10f9c55f516fd496cc46461be80b4804&oe=562B85DE&__gda__=1445789678_fc17edbfb08c60f5c84545ab21b531ef"}}},{"id":"AVkrbDAYXYyMMRnmi_dSF4Vpzz3GnImUAQThI8DXWAJ7cdBfUzfhFzO40MRDZKwj1B-CpS3q44HGRBl276P7YQc9u_eFm16oissL79LotVl42Q","name":"Juwha Lee","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xaf1\/v\/t1.0-1\/c37.37.466.466\/s50x50\/249747_227190590642500_1869691_n.jpg?oh=f51c54128c68b6858fdbe6c5ed981016&oe=5623F6AC&__gda__=1445358625_2c4db445fac4fe94705fd95a2bb17d2b"}}},{"id":"AVmSeC6jWh6hoSW4prnQT56dGH269XiCBg_glEAcZdLrX-ngKTXGkiehPvEWL_Bg1__eljNkL4-e8iqOU90x1zX5jUkXr3ncnbqxkyVW4Vl0mQ","name":"Myoung Heon O","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xpa1\/v\/t1.0-1\/c12.13.155.155\/s50x50\/1656291_641249922578120_584217123_n.jpg?oh=fc5492a4a6dfc5d28ad7aa78327e1dd7&oe=56298C90&__gda__=1445737729_75f39ea3e93e56bd9e6c9c9d2e2fdce0"}}},{"id":"AVmHzH8usF4OVc-xffLcqApjSTr-OpnxMvPUX9bR9BQIaO8LPg1b5uCZOdiw9-XJrlgZjCCgDJu5JFiQPDAhN-Mzlf14bauOUs4rQfQXUjGXCQ","name":"GilSup Song","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-prn2\/v\/t1.0-1\/c32.32.403.403\/s50x50\/254512_224966047531045_1269623_n.jpg?oh=29ff31422138d641809de37276f70454&oe=560E7439&__gda__=1445534206_33c6122b4d2e40028a344d1749e4ab42"}}},{"id":"AVkI2IBkSca4r4pjiKWKXk0sEcYjs6w4Cvj_GPLgIdo84V4hHMvk0yjpPXsv2CNxjFXcmCxxdhQJXzjxLR2_ax18gKJVPIhaHBqHTQLRRJFpVQ","name":"SeJoon Park","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xtf1\/v\/t1.0-1\/p50x50\/1907686_10206804152121053_6074698675859872939_n.jpg?oh=a0a9060c1e4e4605fce6e76fb7c1ba3c&oe=5627A894&__gda__=1448696793_ad91997c8d32d0831bb0e80e4c731d35"}}},{"id":"AVmYHgUZtkxuSkpLSPJJWn4ob4cx0E_JwM-Bnz4tKCKG5bnsqenHNRUM373BtOLgQFevFd0kW9uFqap8m6laMkPfg-zn0O1UnB90E5c9BQmWkw","name":"Whon Namkoong","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfa1\/v\/t1.0-1\/p50x50\/10906224_10152893178579404_3570261495688424636_n.jpg?oh=c820d08c50e5f4064840057394ee512c&oe=561B3D24&__gda__=1445819565_3a99b3b261a904f124060ad907f83d64"}}},{"id":"AVkHSPjhFaBagF_mm3F7yl5LNP_LhEQoK4fWP9VIa0elyjNrUCITnsbkpikLfrDigYOyNf-JkC82ds2FIhjOsPJUIpRsIH8rBGCmyJDt9yUGxw","name":"HyunWook Chun","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xpa1\/v\/t1.0-1\/c0.4.50.50\/p50x50\/11221758_1001145139930122_4036714310246595443_n.jpg?oh=baa956e39a09865bdc3a7e2d7742c435&oe=5614811F&__gda__=1444808182_5b900d486ae9b023d4eaf48e89fe520a"}}},{"id":"AVkjz4WvI8phtoM_X6Y0vKV6y0b0K7IpfN1DkZcuVeuFXQS0uDWyqufOFbR4Hx3sxMVkPxy-VCJJH_WiuUwL92X8ZAb2U7yftg3nT3YcZCLxKw","name":"Han Joo Lee","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfp1\/v\/t1.0-1\/p50x50\/10616259_10202962554572788_5302549824532068113_n.jpg?oh=d31f60c9db98c21e36fc0d58189b9a26&oe=56190673&__gda__=1444510744_7e52f073a966eb3ab754c5961735ec7a"}}},{"id":"AVnDMt0zb6JZYS7nBqM4o3rK-JHgfRYNRTuGAqAnca7J8CEawYsFLZbbnPBzrPHqO230e5e6hsgN3Xv37XVT0NWBsey4ItMa5KPyVFE0-xKD6Q","name":"ChangHyun  Kim","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xtp1\/v\/t1.0-1\/c0.2.50.50\/p50x50\/10401943_739929829427145_711232539700676205_n.jpg?oh=ccecf09642511b1503a5a5b359f15ff7&oe=5612D16B&__gda__=1445575231_6a32805cd626bf2291d716dc9c345836"}}},{"id":"AVmGq4dytGk7AQrrHTi--SWzrSs0FChAd3A2gh03SVhx04RiIGJGNSntxV8Xf6Pu0wLuJHm--oliYkXx8emEtJtispt2vYTBkRN_xVfdsJQRlg","name":"\uc815\uc131\uc6b4","picture":{"data":{"is_silhouette":true,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfp1\/v\/t1.0-1\/c15.0.50.50\/p50x50\/10354686_10150004552801856_220367501106153455_n.jpg?oh=02187c1687142ef2b5f0a0e5cdb674ea&oe=5621DD2F&__gda__=1445367769_c07f7d1a5e2ce358f114e4f7c18dfe01"}}},{"id":"AVlxeN6tLnphFmQSNSu5xzEQEpy5owWugHKsKZTeg8PHTHh6ulDOidfpSBf1pnK48ftCigI54kFO334EhrdfcPPVPJc6IK4oPyA5xrNxbhoSwQ","name":"Sean Yang","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xfa1\/v\/t1.0-1\/c36.36.454.454\/s50x50\/190538_555016531211480_2139084110_n.jpg?oh=4f271d7b2dd7cb780a7df05f5a4e9639&oe=56110A4A&__gda__=1444931962_898856f469e2d25d9d88788c2f47b947"}}},{"id":"AVleDWErJhmBrR2P24eNDWq_TKDGOfz8FQATbZhOsldf3GsfqZmt6WuFuVHOchZOzWJsu02lso06J6yZrS7zm76A0ciYhMXXrGlSsmSkjJ6ymw","name":"Lee Sungman","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xaf1\/v\/t1.0-1\/c37.37.466.466\/s50x50\/206570_203453973022922_4710627_n.jpg?oh=4bff2208f97a06cc6757a359b916335b&oe=561BA9B5&__gda__=1448918840_8247c4798585b71aa42deacbb7623cb7"}}},{"id":"AVmFb9Pjp-mgCmKAvVGs_PUoGxtO8PLBP1qJrM7w6cJkbdvC_7dMnMhcNkCS0m0fpd9M7ZjaExt1dokJl7wdgcLV09xo250BEjBdmBTBucNXbA","name":"\uc870\ucc3d\ud658","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xaf1\/v\/t1.0-1\/p50x50\/20214_986516701382501_2764266565929198511_n.jpg?oh=102e5d017b869469a7ba6cdc15b8aa79&oe=560FB3F1&__gda__=1445661013_e462f8f5ca8b542aede4547319bbfad8"}}}],"paging":{"cursors":{"before":"QVZAsaFU2eVItektQblZAjV3ptNUl1NDJQX3Y4WC00d3l6TFZAYd2JOaktzM19WbmtyeV9EakpETUxSTzI5SmI5QllydTRveDF3cS1fdHBCS3JZAZA1lQcXJBRHF4R2dCbXBZAa0thRVV3SlVPdGNvcmcZD","after":"QVZAra3ZAWQi1aZA29HTG5ON3prbFNSWHUzQTFZAMUc2a0hCNFFjTUVHODN2OGhHbFppSU9yZATgwT2tGdXBiUUhKRzk4b1o4UTQxbW1uelRHNzhMTlZAlLVdIWDd3Q2FrbGFKd25xTGdEbUlFMWlrQXcZD"},"next":"https:\/\/graph.facebook.com\/v2.3\/974372735930962\/invitable_friends?access_token=CAAG1sZCZACjkgBAPHLyOxtPZCl6vk3KR3iS8yPMkZCYNyRCTLZBbnFiLZArzr9aoe2xB923mRuulZAwmAh5XdSpCJRW2m5cPRw44PZBZBN2MB1ZCfqr8nqZCWInassNSGXSfzUW9yrq0KMDKvvAnHZAJr42Px2QhdPGkxxvLdVJFZBanv1hAQ3zkJPCF1hxvTcN9KE4NzIZAmzvJmNShyd72NIUzrZA&limit=25&after=QVZAra3ZAWQi1aZA29HTG5ON3prbFNSWHUzQTFZAMUc2a0hCNFFjTUVHODN2OGhHbFppSU9yZATgwT2tGdXBiUUhKRzk4b1o4UTQxbW1uelRHNzhMTlZAlLVdIWDd3Q2FrbGFKd25xTGdEbUlFMWlrQXcZD"}}
		UnityEngine.Debug:LogWarning(Object)
				SNSManager:<GetFaceBookInvitableFriends>m__F4(FBResult) (at Assets/01.Scripts/Singleton/Network/SNSManager.cs:309)
					Facebook.<Start>c__Iterator0:MoveNext() */
			Dictionary<string, System.Object> jsonDic = tempObj as Dictionary<string, System.Object>;
			List<System.Object> dataList = jsonDic["data"] as List<System.Object>;
			if(dataList.Count == 0){
				Utility.LogWarning("Count is Zero");
				Global.isNetwork = false;
				return;
			}
			Utility.LogWarning(dataList.Count);
			FaceBookInvitable fb;// = new FaceBookInvitable();
			foreach(System.Object dataObject in dataList){
				fb = new FaceBookInvitable();
				Dictionary<string, System.Object> dataDic = dataObject as Dictionary<string, System.Object>;
				string inviteToken = dataDic["id"] as String;
				string Name = dataDic["name"] as String;
				fb.inviteToken = inviteToken.Trim();
				fb.FBName = Name;
				Dictionary<string, System.Object> pictureDic = dataDic["picture"] as Dictionary<string, System.Object>;
				Dictionary<string, System.Object> pictureDataDic = pictureDic["data"] as Dictionary<string, System.Object>;
				bool issilhouett = (bool)pictureDataDic["is_silhouette"];
				string url = pictureDataDic["url"] as String;
				fb.issihouett = issilhouett;
				fb.FBUrl = url;
				fb.isCheck = true;
				facebookinvite.Add(fb);
				//Utility.LogWarning("" + fb.inviteToken );
			}
			Global.isNetwork = false;
		});

	}

	public static void ClanResultFBFeed(){
	
		FB.Feed(                                                                                                                 
		        linkCaption: "PitInRacing " ,  //string.Format(KoStorage.GetKorString("77135"), clanName);
		        picture: "https://s3-ap-northeast-1.amazonaws.com/gabangman01/FBImage/icon_128.png",                                            
		        linkName: "i'm Just Win " ,    //KoStorage.GetKorString("77134")
		        linkDescription: "WIN!!!", //KoStorage.GetKorString("77136")
		        link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")       
		        );         
	
	}

	/* public string FeedToId = "";
    public string FeedLink = "";
    public string FeedLinkName = "";
    public string FeedLinkCaption = "";
    public string FeedLinkDescription = "";
    public string FeedPicture = "";
    public string FeedMediaSource = "";
    public string FeedActionName = "";
    public string FeedActionLink = "";
    public string FeedReference = "";
    public bool IncludeFeedProperties = false;
    private Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]>();

    private void CallFBFeed()
    {
        Dictionary<string, string[]> feedProperties = null;
        if (IncludeFeedProperties)
        {
            feedProperties = FeedProperties;
        }
        FB.Feed(
            toId: FeedToId,
            link: FeedLink,
            linkName: FeedLinkName,
            linkCaption: FeedLinkCaption,
            linkDescription: FeedLinkDescription,
            picture: FeedPicture,
            mediaSource: FeedMediaSource,
            actionName: FeedActionName,
            actionLink: FeedActionLink,
            reference: FeedReference,
            properties: feedProperties,
            callback: Callback
        );
    }


private void appRequestCallback (FBResult result)                                                                              
{                                                                                                                              
    Util.Log("appRequestCallback");                                                                                         
    if (result != null)                                                                                                        
    {                                                                                                                          
        var responseObject = Json.Deserialize(result.Text) as Dictionary<string, object>;                                      
        object obj = 0;                                                                                                        
        if (responseObject.TryGetValue ("cancelled", out obj))                                                                 
        {                                                                                                                      
            Util.Log("Request cancelled");                                                                                  
        }                                                                                                                      
        else if (responseObject.TryGetValue ("request", out obj))                                                              
        {                
      AddPopupMessage("Request Sent", ChallengeDisplayTime);
            Util.Log("Request sent");                                                                                       
        }                                                                                                                      
    }        

	 */


	[System.Serializable]
	public class FaceBookInvitable{
		public string inviteToken;
		public string FBName;
		public bool issihouett;
		public string FBUrl;
		public Texture2D FBImg;
		public bool isCheck;
	}

	[System.Serializable]
	public class FaceBookFr{
		public string FBID;
		public string FBName;
		public bool issihouett;
		public string FBUrl;
		public Texture2D FBImg;
		public bool bAddFriend;
	}


	public static List<FaceBookFr> facebookfr = new List<FaceBookFr>();
	public static List<FaceBookInvitable> facebookinvite = new List<FaceBookInvitable>();
	public static void GoogleLogin(System.Action SuccessCallback, System.Action FailedCallback){
		if (!Social.localUser.authenticated) {
			Social.localUser.Authenticate((bool success) => {
				if (success) {
					/*string userInfo  = "Username: " + Social.localUser.userName + 
						"\nUser ID: " + Social.localUser.id + 
							"\nIsUnderage: " + Social.localUser.underage;
					string use = Social.localUser.friends[0].id;//
					*/
					if(SuccessCallback != null)
						SuccessCallback();//FBLoginFinish();
					SuccessCallback = null;
				} else {
					//lbStatus.text = "Authentication failed.";
					if(FailedCallback != null)
						FailedCallback();//FBLoginFinish();
					FailedCallback = null;
					Utility.LogWarning ("Google Failure");
				}
				bSNS = true;
			});
		}
	
	}
	public static void GoogleLogOut(System.Action callback){
		if (!Social.localUser.authenticated) {
			return;
		}else{
			//((GooglePlayGames.PlayGamesPlatform) Social.Active).SignOut();
			PlayGamesPlatform.Instance.SignOut();
			if(callback != null) callback();
			callback= null;
			bSNS = true;
		}
	}

	public static void OnAchieveGoogleClick(System.Action Callback){
		if (Social.localUser.authenticated) {
			Social.ShowAchievementsUI();
			UserDataManager.instance.isPause = true;
		}else{
			Callback();
		}
	}

	public static void OnAchieveGoogleClick2(){
		if (Social.localUser.authenticated) {
			Social.ShowAchievementsUI();
			UserDataManager.instance.isPause = true;
		}else{
			Social.localUser.Authenticate((bool success) => {
				if (success) {
					Social.ShowAchievementsUI();
					UserDataManager.instance.isPause = true;
				} else {
				
				}
			});
		}
	}

	public static void OnRankGoogleClick(System.Action Callback){
		if (Social.localUser.authenticated) {
			Social.ShowLeaderboardUI();
			UserDataManager.instance.isPause = true;
		}else{
			Callback();
		}
	}


	public static void OnRankGoogleClick2(){
		if (Social.localUser.authenticated) {
			Social.ShowLeaderboardUI();
			UserDataManager.instance.isPause = true;
		}else{
			Social.localUser.Authenticate((bool success) => {
				if (success) {
					Social.ShowLeaderboardUI();
					UserDataManager.instance.isPause = true;
				} else {
					
				}
			});
		}
	}
	public static void OnRBRankingTime(float tTime){
		if (Social.localUser.authenticated) {
			long mTime = (long)(tTime * 1000);
			Social.ReportScore(mTime, "CgkI1bqC7NQbEAIQKQ", (bool success) => { //time
				if(success) Utility.LogWarning("OnRBRankingTime success ");

				else Utility.LogWarning("OnRBRankingTime failed ");
			   
			});
		}else{
			Utility.LogWarning("OnRBRankingTime No Login ");
		}
	}


	public static void RecordAchievementItems(string gID, int count, int mC){
	
		if (Social.localUser.authenticated) {

			float c = (float)mC / (float)count;
			double a =  (double)c*100f;
			if( a > 100f) a = 100f;
			Social.ReportProgress(gID, a , (success)=>{
				if(success) Utility.LogWarning("Achieve success ");
				else Utility.LogWarning("Achieve failed ");
			});
		}else{
			Utility.LogWarning("google ID " + gID);

		}
	}
	public static void OnRBRankingThropy(int Count){
		if (Social.localUser.authenticated) {
			Social.ReportScore(Count, "CgkI1bqC7NQbEAIQKA", (bool success) => { // medal
				if(success) Utility.LogWarning("OnRBRankingThropy success ");
				else Utility.LogWarning("OnRBRankingThropy failed ");
			});
		}else{
			//Callback();
			Utility.LogWarning("OnRBRankingThropy No Login ");
		}
	}

	public static void RecordServerAchievement(int id, int mCount){
		Utility.LogWarning("RecordServerAchievement " + Global.isNetwork);
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		Dictionary<string, int> mDic = new Dictionary<string, int>();
		mDic.Add("acheivmentId",id);
		mDic.Add("value",mCount);
		string mAPI = ServerAPI.Get(90061);//game/acheivment/increment
		NetworkManager.instance.HttpFormConnect("Put",mDic,mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
			}
			Global.isNetwork = false;
		});
	}
	
	
	public static void unLockAchievement(string gID){
		if (Social.localUser.authenticated) {
			//	PlayGamesPlatform.Instance.ReportProgress(gID, 0.0f,(bool success)=>{
			//		if(success) Utility.LogWarning("Achieve success ");
			//		else Utility.LogWarning("Achieve failed ");
			//	});
			Social.ReportProgress(gID, 0.0f, (bool success)=>{
				//if(success) Utility.LogWarning("Achieve success ");
				//else Utility.LogWarning("Achieve failed ");
			});
		}else{
			//Callback();
		}
	}

}
