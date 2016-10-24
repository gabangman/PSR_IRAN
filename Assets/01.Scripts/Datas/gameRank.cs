using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

public class gameRank{
	
	[System.Serializable]
	public class friendRank{
		public string nIndex;
		public int nRanking;
		public string strProfileUrl;
		public string strNick;
		public int nGoldenTrophy;
		public int nSilverTrophy;
		public int nBronzeTrophy;
		public float fTime;
		public int nMyCar;
		public int nMyCrew;
		public int nCarAbility;
		public int nCrewAbility;
		public int nLevel;
		public int nLvUp;
		public string tmMsgTime;
		public int nBlock;
		public int nReceiveFuel;
		public int nPlayTimes;
		public int myRank;
		public friendRank(JSONNode _wr){
			this.nIndex = _wr["nIndex"].Value;
			this.nRanking = _wr["nRanking"].AsInt;
			this.strProfileUrl=_wr["strProfileUrl"].Value;
			this.strNick=_wr["strNick"].Value;
			this.nGoldenTrophy=_wr["nGoldenTrophy"].AsInt;
			this.nSilverTrophy=_wr["nSilverTrophy"].AsInt;
			this.nBronzeTrophy=_wr["nBronzeTrophy"].AsInt;
			this.fTime=_wr["fTime"].AsFloat;
			this.nMyCar=_wr["nMyCar"].AsInt;
			this.nMyCrew=_wr["nMyCrew"].AsInt;
			this.nCarAbility=_wr["nCarAbility"].AsInt;
			this.nCrewAbility=_wr["nCrewAbility"].AsInt;
			this.nLevel=_wr["nLevel"].AsInt;
			this.tmMsgTime=_wr["tmMsgTime"].Value;
			this.nBlock=_wr["nBlock"].AsInt;
			this.nReceiveFuel=_wr["nReceiveFuel"].AsInt;
			this.nPlayTimes = _wr["nPlayTimes"].AsInt;
			this.myRank = 0;
			/*if(this.strProfileUrl.Length > 10){
				try {
					this.strProfileUrl = Base64Manager.instance.getEncrpytString(this.strProfileUrl);
				}
				catch (Exception e) {
					this.strProfileUrl = "NoImage";
				}  
			}
			if(this.strNick.Length > 10){
				try{
					this.strNick = Base64Manager.instance.getEncrpytString(this.strNick);
				}
				catch(Exception e){
					this.strNick = "PitRacing";
				}
			}*/
		}			
	}
	
	static gameRank _instance;
	public static gameRank instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new gameRank();
			}
			return _instance;
		}
	}
	
	public void InstanceNull(){
		_instance = null;
	}
	
	public void init(){
		//	string json = "[{\"nick\":\"Fred\",\"lv\":5},{\"nick\":\"Molly\",\"lv\":13}]";
		//	friendRank[] jsonObject = JsonReader.Deserialize<friendRank[]>(json);	
		//	string json1 = "[{\"nick\":\"Fred\",\"lv\":5},{\"nick\":\"Molly\",\"lv\":13}]";
		//	worldRank[] jsonObject1 = JsonReader.Deserialize<worldRank[]>(json1);
	}
	
	
	public List<friendRank> listfriend = new List<friendRank>();
	
	
	
	
	public void initializeFriend(string data){
		listfriend.Clear();
		JSONNode root = JSON.Parse(data);
		for(int i = 0; i < root.Count; i++){
			JSONNode a = root[i];
			friendRank wr = new friendRank(a);
			listfriend.Add(wr);
		}
	}
	public void sortlistfriendRank(){
		if(listfriend.Count <= 1) return;
		
		int cnt = listfriend.Count;
		for(int i = cnt ; i > 0 ; i--){
			if(listfriend[i-1].nMyCar == 0)
				listfriend.RemoveAt(i-1);
		}
		
		listfriend.Sort(
			delegate(friendRank x, friendRank y) {
			return x.nRanking.CompareTo(y.nRanking);
		});
	}
	
	
	
	void OnApplicationQuit(){
		_instance = null;
	}
	
	/*
	public List<PostList> listPost = new List<PostList>();
	[System.Serializable]
	public class PostList{
		public int index;
		public int nType;
		public int nMsgIndex;
		public int nReCoin;
		public int nReDollar;
		public int nReFuel;
		public int nCarExchangeCoupon;
		public string strNick;
		public int nCardId;
		public int nCardQuaility;
		public PostList(JSONNode _wr, int id){
			this.nType = _wr["nType"].AsInt;
			this.nMsgIndex=_wr["nMsgIndex"].AsInt;
			this.nReCoin=_wr["nReCoin"].AsInt;
			this.nReDollar=_wr["nReDollar"].AsInt;
			this.nReFuel=_wr["nReFuel"].AsInt;
			this.nCarExchangeCoupon=_wr["nCarExchangeCoupon"].AsInt;
			this.strNick=_wr["strNick"].Value;
			this.nCardId = _wr["nCardId"].AsInt;
			this.nCardQuaility = _wr["nCardQuantity"].AsInt;
			this.index = id;
			if(this.strNick == "null" || this.strNick == string.Empty){
				return;
			}

			try {
				this.strNick = Base64Manager.instance.getEncrpytString(this.strNick);
			}
			catch (Exception e) {
				this.strNick = "PitRacing";
			}  
		}
	}

	public void InitialMsg(string data){
		listPost.Clear();
		JSONNode root = JSON.Parse(data);
		for(int i = 0; i < root.Count; i++){
			JSONNode a = root[i];
			PostList wr = new PostList(a, i);
			listPost.Add(wr);
		}
	}
*/
	
	public List<GiftList> listGift = new List<GiftList>();
	[System.Serializable]
	public class GiftList{
		public int index;
		public int nType;
		public int nMsgIndex;
		public int nMsgType;
		public string rewardValue;
		public string strNick;
		public GiftList(JSONNode _wr, int id){
			this.index = id;
			this.nType = _wr["type"].AsInt;
			this.nMsgIndex = _wr["idx"].AsInt;
			this.rewardValue = _wr["value"];
			this.nMsgType = _wr["msgType"].AsInt;
		}
	}
	
	public void InitializeGiftList(SimpleJSON.JSONNode _node , int cnt){
		listGift.Clear();
		for(int i = 0; i < cnt; i++){
			JSONNode a = _node[i];
			GiftList wr = new GiftList(a, i);
			listGift.Add(wr);
		}
	}
	
	public void sortListGift(){
		if(listGift.Count <= 1) return;
		
		int cnt = listGift.Count;
		for(int i = 0 ; i < cnt ; i++){
			listGift[i].index = i;
		}
	}
	
	
	public List<RaceRankInfo> listRTR = new List<RaceRankInfo>();
	public List<RaceRankInfo> listRTT = new List<RaceRankInfo>();
	public List<RaceRankInfo> listRWT = new List<RaceRankInfo>();
	public List<RaceRankInfo> listRWR = new List<RaceRankInfo>();
	public List<RaceRankInfo> listFFR = new List<RaceRankInfo>();
	public void InitializeRTRList(SimpleJSON.JSONNode _node , int cnt){
		listRTR.Clear(); addRTRList(_node, cnt);
	}
	
	public void addRTRList(SimpleJSON.JSONNode _node , int cnt){
		for(int i = 0; i < cnt; i++){
			JSONNode a = _node[i];
			if(string.IsNullOrEmpty(a.ToString())){
				
			}else{
				RaceRankInfo wr = new RaceRankInfo(a, i);
				listRTR.Add(wr);
			}
		}
	}
	
	public void InitializeRTTList(SimpleJSON.JSONNode _node , int cnt){
		listRTT.Clear();
		addRTTList(_node, cnt);
	}
	public void addRTTList(SimpleJSON.JSONNode _node , int cnt){
		for(int i = 0; i < cnt; i++){
			JSONNode a = _node[i];
			if(string.IsNullOrEmpty(a.ToString())){
				
			}else{
				RaceRankInfo wr = new RaceRankInfo(a, i);
				listRTT.Add(wr);
			}
		}
	}
	public void InitializeRWTList(SimpleJSON.JSONNode _node , int cnt){
		listRWT.Clear();
		addRWTList(_node,cnt);
	}
	public void addRWTList(SimpleJSON.JSONNode _node , int cnt){
		for(int i = 0; i < cnt; i++){
			JSONNode a = _node[i];
			if(string.IsNullOrEmpty(a.ToString())){
				
			}else{
				RaceRankInfo wr = new RaceRankInfo(a, i);
				listRWT.Add(wr);
			}
		}
	}
	public void InitializeRWRList(SimpleJSON.JSONNode _node , int cnt){
		listRWR.Clear();
		addRWRList(_node,cnt);
	}
	
	public void addRWRList(SimpleJSON.JSONNode _node , int cnt){
		for(int i = 0; i < cnt; i++){
			JSONNode a = _node[i];
		//	Utility.LogWarning(a.ToString());
			if(string.IsNullOrEmpty(a.ToString())){
			
			}else{
				RaceRankInfo wr = new RaceRankInfo(a, i);
				listRWR.Add(wr);
			}
		
		}
	}
	
	public void InitializeFFRList(SimpleJSON.JSONNode _node , int cnt, int totalPage){
		if(listFFR.Count == 0) listFFR.Clear();
		addFFRList(_node, cnt, totalPage);
	}
	
	public void addFFRList(SimpleJSON.JSONNode _node , int cnt, int totalPage){
		for(int i = 0; i < cnt; i++){
			JSONNode a = _node[i];
			if(string.IsNullOrEmpty(a.ToString())){
				
			}else{
				RaceRankInfo wr = new RaceRankInfo(a, i,1);
				listFFR.Add(wr);
			}
		}
	}

	public void myFFRList(){
		RaceRankInfo wr = new RaceRankInfo(1);
		listFFR.Add(wr);
	}

	public void RankingListReset(){
		listRTR.Clear();
		listRTT.Clear();
		listRWR.Clear();
		listRWT.Clear();
	}


	public void AddScoreRankingInfo(JSONNode _wr, int id, int flag){
		//"userId":299,"record":70.18,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"3","tropy2":"0","tropy3":"0","raceData":"TestRaceData"
		//response:{"state":0,"msg":"sucess","totalPage":1,"result":[{"id":292,"fbUid":1520544144910563,"teamId":10,"carId":1002,"carClass":3102,"tropy1":"1","tropy2":0,"tropy3":0,"raceData":"{\"carId\":1002,\"teamId\":10,\"carClass\":3102,\"crewId\":1200,\"sponId\":1300,\"carAbility\":279,\"crewAbility\":328,\"userNick\":\"w\",\"userURL\":\"https:\/\/s3-ap-northeast-1.amazonaws.com\/gabangman01\/MultiPicture\/MultiCom_1.png\",\"Torque\":38.2,\"PitTime\":16.28261,\"BSPower\":20,\"BSTime\":1.24,\"TireTime\":1.87,\"GBLv\":479507,\"BSPressTime\":38,\"fG1\":0,\"fG2\":1,\"fG3\":0,\"fG4\":0,\"fG5\":0,\"fG6\":0.6,\"fG7\":1,\"fG8\":0,\"fG9\":1,\"fG10\":1,\"fG11\":0,\"fG12\":0,\"fG13\":0,\"fG14\":0,\"fG15\":0,\"fG16\":0,\"fG17\":0,\"fG18\":0,\"fG19\":0,\"fG20\":0,\"pD1\":0,\"pD2\":0,\"pD3\":0,\"pD4\":0,\"pD5\":0,\"pD6\":0,\"pD7\":0,\"pD8\":0,\"pD9\":0,\"pD10\":0,\"pD11\":0,\"pD12\":0,\"pD13\":0,\"pD14\":0,\"pD15\":0,\"pD16\":0,\"pD17\":0,\"pD18\":0,\"pD19\":0,\"pD20\":0,\"raceTime\":78.64458}","record":78.644577026367,"level":0,"nickName":"Try-Again"}],"time":1451920753}
		int idx =( _wr["id"].AsInt);
		
		RaceRankInfo rnk = listFFR.Find(obj => obj.userId == idx);
		if(rnk == null) return;
		rnk.scoreFr = true;
		try{
			rnk.record = _wr["record"].AsFloat;
		}
		catch(NullReferenceException e){
			rnk.record = 0.0f;
		//	rnk.scoreFr = false;
		}

//	response:{"state":0,"msg":"sucess","totalPage":1,"result":[{"id":462,"fbUid":1052894381447457,"teamId":17,"carId":1021,"carClass":3106,"tropy1":"12","tropy2":"1","tropy3":0,"raceData":{"carId":1021,"teamId":17,"carClass":3106,"level":7,"crewId":1210,"sponId":1305,"carAbility":1239,"crewAbility":761,"userNick":"\ub85c\ud14d \uae40\uc2e4\uc7a5","userURL":"https:\/\/scontent.xx.fbcdn.net\/hprofile-xpa1\/v\/t1.0-1\/c20.20.249.249\/s50x50\/246795_153310944739143_312616_n.jpg?oh=442ecd1e46b6bf87ce185c5cc3f78b98&oe=578D8070","Torque":157.3,"PitTime":9.440723,"BSPower":112,"BSTime":6.920001,"TireTime":0.09900001,"GBLv":25,"BSPressTime":20,"fG1":0,"fG2":0,"fG3":0,"fG4":0,"fG5":0,"fG6":0,"fG7":0,"fG8":0,"fG9":0.35,"fG10":0,"fG11":0,"fG12":0,"fG13":0.35,"fG14":0,"fG15":0.35,"fG16":0,"fG17":0,"fG18":0,"fG19":0,"fG20":0,"pD1":2.098936,"pD2":0.543905,"pD3":0.77402,"pD4":0.839816,"pD5":0.960099,"pD6":1.106204,"pD7":1.856879,"pD8":17.06518,"pD9":0.842881,"pD10":0.76867,"pD11":0.692261,"pD12":0.740566,"pD13":0.763383,"pD14":1.061354,"pD15":1.013842,"pD16":2.647578,"pD17":0,"pD18":0,"pD19":0,"pD20":0,"raceTime":40.38137},"record":40.381370544434,"level":7,"nickName":"\ub85c\ud14d \uae40\uc2e4\uc7a5"},{"id":461,"fbUid":1065464796821755,"teamId":14,"carId":1002,"carClass":3106,"tropy1":"10","tropy2":"8","tropy3":"1","raceData":{"carId":1002,"teamId":14,"carClass":3106,"level":8,"crewId":1204,"sponId":1300,"carAbility":481,"crewAbility":683,"userNick":"\uc54c\ub85c\uacf5\ub8e1","userURL":"https:\/\/scontent.xx.fbcdn.net\/hprofile-xfa1\/v\/t1.0-1\/p50x50\/11139354_933012826733620_3650539727338477642_n.jpg?oh=1e1aa6a8f5d8dda4fae4b18fff0db57d&oe=5789C956","Torque":69.59901,"PitTime":10.94137,"BSPower":36,"BSTime":2.24,"TireTime":1.37,"GBLv":1,"BSPressTime":42,"fG1":0,"fG2":0,"fG3":0,"fG4":0.75,"fG5":0.35,"fG6":0,"fG7":0,"fG8":0.35,"fG9":0.35,"fG10":0,"fG11":0,"fG12":0.35,"fG13":0,"fG14":0,"fG15":0,"fG16":0,"fG17":0,"fG18":0,"fG19":0,"fG20":0,"pD1":2.014751,"pD2":3.087659,"pD3":3.294294,"pD4":3.750877,"pD5":19.14311,"pD6":3.819093,"pD7":2.433506,"pD8":2.747918,"pD9":3.12745,"pD10":3.148406,"pD11":3.297499,"pD12":3.70619,"pD13":0,"pD14":0,"pD15":0,"pD16":0,"pD17":0,"pD18":0,"pD19":0,"pD20":0,"raceTime":61.20669},"record":61.206691741943,"level":8,"nickName":"\uc54c\ub85c\uacf5\ub8e1"},{"id":472,"fbUid":1063962810334948,"teamId":10,"carId":1001,"carClass":3105,"tropy1":0,"tropy2":0,"tropy3":0,"raceData":{"carId":1001,"teamId":10,"carClass":3105,"level":2,"crewId":1200,"sponId":1300,"carAbility":346,"crewAbility":328,"userNick":"Ios","userURL":"https:\/\/scontent.xx.fbcdn.net\/hprofile-xtl1\/v\/t1.0-1\/p50x50\/12729104_1063888753675687_5038487002692259701_n.jpg?oh=442989d7a3b8e11547ea27e69fe51d4f&oe=5780B55B","Torque":52.199,"PitTime":16.50142,"BSPower":30,"BSTime":1.9,"TireTime":1.969,"GBLv":1,"BSPressTime":47,"fG1":0,"fG2":0.4,"fG3":0,"fG4":0,"fG5":0.4,"fG6":0,"fG7":0,"fG8":0,"fG9":0,"fG10":0.4,"fG11":0.4,"fG12":0,"fG13":0.4,"fG14":0,"fG15":0,"fG16":0,"fG17":0,"fG18":0,"fG19":0,"fG20":0,"pD1":2.238275,"pD2":3.371745,"pD3":3.272189,"pD4":3.402476,"pD5":3.772075,"pD6":22.14233,"pD7":4.625027,"pD8":2.572555,"pD9":2.568516,"pD10":3.273839,"pD11":3.338632,"pD12":3.273761,"pD13":3.705587,"pD14":0,"pD15":0,"pD16":0,"pD17":0,"pD18":0,"pD19":0,"pD20":0,"raceTime":70.37258},"record":70.372581481934,"level":2,"nickName":"Ios"}],"time":1462042582}
//	response:{"state":0,"msg":"sucess","totalPage":1,"result":[{"id":47,"fbUid":1133298086740419,"teamId":0,"carId":0,"carClass":0,"tropy1":0,"tropy2":0,"tropy3":0,"raceData":"","record":0,"level":1,"nickName":"\ub85c\ud14d"},{"id":27,"fbUid":1122351587799742,"teamId":10,"carId":1005,"carClass":3102,"tropy1":"2","tropy2":"3","tropy3":"1","raceData":{"carId":1005,"teamId":10,"carClass":3102,"level":4,"crewId":1200,"sponId":1300,"carAbility":279,"crewAbility":328,"userNick":"GABBAMAN","userURL":"User_Default","Torque":36.2,"PitTime":16.9257,"BSPower":18,"BSTime":1.379,"TireTime":2.09,"GBLv":1,"BSPressTime":37,"fG1":0,"fG2":0,"fG3":0,"fG4":0,"fG5":0,"fG6":0,"fG7":0,"fG8":0,"fG9":0.55,"fG10":0,"fG11":0,"fG12":0,"fG13":0,"fG14":0,"fG15":0,"fG16":0,"fG17":0,"fG18":0,"fG19":0,"fG20":0,"pD1":2.476312,"pD2":3.749648,"pD3":3.590994,"pD4":33.09951,"pD5":5.004536,"pD6":2.912917,"pD7":3.089501,"pD8":3.432103,"pD9":3.751875,"pD10":0,"pD11":0,"pD12":0,"pD13":0,"pD14":0,"pD15":0,"pD16":0,"pD17":0,"pD18":0,"pD19":0,"pD20":0,"raceTime":81.11725},"record":81.117248535156,"level":4,"nickName":"GABBAMAN"}],"time":1462045586}

		rnk.teamId = _wr["teamId"].AsInt;
		rnk.carId = _wr["carId"].AsInt;
		rnk.carClass = _wr["carClass"].AsInt;
		rnk.trophy1 = _wr["tropy1"];
		rnk.trophy2 = _wr["tropy2"];
		rnk.trophy3 = _wr["tropy3"];
		rnk.mRank = id;

		SimpleJSON.JSONNode _node = _wr["raceData"];
		string str = _node.ToString();
		//Utility.LogWarning(_node.ToString());
		if(string.IsNullOrEmpty(str) == true){
			rnk.scoreFr = false;
		}else{
			rnk.mRaceUserInfo = new RaceUserInfo(_wr["raceData"]);
		}
	 


	}
	
	[System.Serializable]
	public class RaceRankInfo{
		public int index;
		public int userId;
		public string userNick;
		public int userevel;
		public float record;
		public int teamId;
		public int carId;
		public int carClass;
		public string trophy1;
		public string trophy2;
		public string trophy3;
		public int level;
		public string fbUid;
		public bool scoreFr;
		public RaceUserInfo mRaceUserInfo;
		public bool addFr;
		public int mRank;
		public int addPage;
		public RaceRankInfo(JSONNode _wr, int id){ // 정보 가져오기
			addPage = 0;
			this.index = id;
			try{
				this.record = _wr["record"].AsFloat;
			}
			catch(NullReferenceException e){
				this.record = 0.0f;
			
			}
			try{
				this.teamId = _wr["teamId"].AsInt;
				this.carId = _wr["carId"].AsInt;
				this.carClass = _wr["carClass"].AsInt;
				this.trophy1 = _wr["tropy1"];
				this.trophy2 = _wr["tropy2"];
				this.trophy3 = _wr["tropy3"];
				this.level = 1;
				//Utility.LogWarning(_wr["raceData"]);
				mRaceUserInfo = new RaceUserInfo(_wr["raceData"]);
			}
			catch(NullReferenceException e){

			}

		}
		
		public RaceRankInfo(JSONNode _wr, int id , int idx){ // 정보 가져오기
			
			//{"id":292,"fbUid":1520544144910563,"level":0,"nickName":"Try-Again"}
			this.index =id;
			this.userNick = (_wr["nickName"]);
			this.userId = ( _wr["id"].AsInt);
			this.scoreFr = false;
			this.level = _wr["level"].AsInt;
			this.fbUid = _wr["fbUid"];
			this.mRank = -1;
			//Utility.LogWarning("id");
			mRaceUserInfo = new RaceUserInfo();

		}
		
		public RaceRankInfo(){
			
		}

		public RaceRankInfo(int idx){
			this.index =0;
			this.userNick = GV.UserNick;
			this.userId = int.Parse(GV.UserRevId);
			this.scoreFr = false;
			this.level = Global.level;
			this.fbUid =FB.UserId;
			this.mRank = 0;
			mRaceUserInfo = new RaceUserInfo();
		}
		
	}
	
}