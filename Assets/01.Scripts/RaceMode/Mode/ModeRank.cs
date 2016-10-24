using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ModeRank : ModeParent {
//	List<RegularAIUser> listRegular; //= new List<RegularAIUser>();
//	List<RegularAIUser> listStockUser = new List<RegularAIUser>();//Regular;
	List<RaceUserInfo> listRaceUser = new List<RaceUserInfo>();
	public override void OnSubWindow ()
	{
		base.OnSubWindow ();
		transform.parent.GetComponent<RaceModeAnimaintion>().SelectAniPlay("ModeWin_Weekly_Track");

		string[] str = transform.parent.name.Split('_');
		int mode = 0, nCnt = 0;
		Transform tr;
		if(str[1].Equals("Weekly")){
			tr = transform.GetChild(0);
			//mode= GV.SelectedRankingMode;
			mode = 0;
			nCnt = 6;
		}else if(str[1].Equals("Drag")){
			tr = transform.GetChild(1);
		//	mode = 2;nCnt = 1;
		}else{ //City
			tr = transform.GetChild(1);
		//	mode = 3;nCnt = 1;
		}
		CheckReEnter(mode);
		tr.gameObject.SetActive(true);
		Global.gRegularRaceLevel=	Base64Manager.instance.GlobalEncoding(1);
		int track  = RandomTrack(mode);
		SetBGTrackImg(track);
		SetRaceInfoWindow(tr, mode);
		SetUserWindow(tr, mode);
		//RandomRegularUser(nCnt, track, mode);

		onNext = ()=>{
			//!!--Utility.Log("onNext ");
			ReadyLoadingCircle();
			gameObject.AddComponent<SettingRaceRank>().setRankingRace(mode, listRaceUser, track);
		};
		onRefresh = ()=>{
			SetRefreshCondition(mode);
			SetUserRefresh(tr, mode);
			//RandomRegularUser(nCnt, track, mode);
		};
	}

	void UserSlotSetting(){
	
	}

	void CheckReEnter(int mode){
		if(bReError){
			bReError = false;
			bReEnter = false;
			SetRefreshCondition(mode);
		}
		
	}


	void SetRefreshCondition(int mode){
		if(mode == 0){  
			//listRaceUser.Clear();
		}else if(mode == 1){ // drag
		//	listTourUser.Clear();
		}else if(mode == 2){
		//	listDragUser.Clear();
		}else{
		//	listEventUser.Clear();
		}
	}

	void SetUserRefresh(Transform tr, int mode){
		LoadingCircle();
		Global.isNetwork = true;
		Dictionary<string,int> mDic = new Dictionary<string, int>();
		mDic.Add("raceId",GV.ChSeasonID);
		string raceAPI = ServerAPI.Get(90026);//"game/race/rank/player"
		NetworkManager.instance.HttpFormConnect("Put", mDic,raceAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				listRaceUser.Clear();
				int cnt = thing["result"].Count;
				for(int  i = 0; i < cnt; i++){
					RaceUserInfo user = new RaceUserInfo(thing["result"], i);
					if(!user.bUser){
						user.RankingUserSet();
						user.DefaultCarAblility = 0;
					}else{
						int min = user.carAbility - GV.CarAbility;
						if(min > 300){
							user.DefaultCarAblility = 300;	user.bUser = false;//+300
						}else if(min < -200 ){
							user.DefaultCarAblility = -100;	user.bUser = false;
						}else {
							user.DefaultCarAblility = 0;
						}
					}
					listRaceUser.Add(user);
				}
				if(cnt != 6){

					for(int i = cnt ; i < 6; i++){
						RaceUserInfo lsa = new RaceUserInfo();
						lsa.RankingUserSet();
						listRaceUser.Add(lsa);
					
					}
				}
				//Utility.LogWarning("count : " + cnt);
				StartCoroutine("SetStockUser", tr);
			}else{
				StopLoadingCircle();
			}
			Global.isNetwork = false;
		});
	}

	void SetUserWindow(Transform tr, int mode){
		LoadingCircle();
		if(	bReEnter){
			//listRegular = new List<RegularAIUser>(listStockUser);
			StopLoadingCircle();
			//StartCoroutine("SetStockUser", tr);
			return;
		}
		Global.isNetwork = true;
		string raceAPI = ServerAPI.Get(90025);//"game/race/rank/player/"
	//	Utility.LogWarning(raceAPI);
		NetworkManager.instance.HttpConnect("Get", raceAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				//response:{"state":0,"msg":"sucess","result":[{"userId":4,"teamId":1,"record":100,"raceData":"test","carId":0,"carClass":0},{"userId":6,"teamId":1,"record":100,"raceData":"s","carId":0,"carClass":0},{"userId":76,"teamId":0,"record":34003.25,"raceData":"","carId":0,"carClass":0},{"userId":204,"teamId":0,"record":0,"raceData":"","carId":0,"carClass":0}],"time":1445956729}
				int cnt = thing["result"].Count;
				for(int  i = 0; i < cnt; i++){
					RaceUserInfo ls = new RaceUserInfo(thing["result"],i);
					if(!ls.bUser){
						ls.RankingUserSet();
						ls.DefaultCarAblility = 0;
					}else{
						int min = ls.carAbility - GV.CarAbility;
						if(min > 300){
							ls.DefaultCarAblility = 300;		ls.bUser = false;
						}else if(min < -200 ){
							ls.DefaultCarAblility = -100;		ls.bUser = false;
						}else {ls.DefaultCarAblility = 0;}
					}
					listRaceUser.Add(ls);
				}

				for(int i = cnt ; i < 6; i++){
					RaceUserInfo lsa = new RaceUserInfo();
					lsa.RankingUserSet();
					listRaceUser.Add(lsa);
					
				}
//				Utility.LogWarning("count : " + cnt);
				bReEnter = true;
				StartCoroutine("SetStockUser", tr);
			}else{
				StopLoadingCircle();
			}
			Global.isNetwork = false;
		});

	}


	IEnumerator SetStockUser(Transform tr){
	//	//!!--Utility.Log ("SetStockUser"+ listStockUser.Count + "name : " + listStockUser[0].strNick);
		var child = tr.FindChild("UserSlot") as Transform;
		//StartCoroutine("TenSecondTimeCheck");
		for(int i = 0; i < listRaceUser.Count;i++){
			yield return StartCoroutine(SetRegularUserSlot(child.GetChild(i), listRaceUser[i]));
		}
		StopLoadingCircle();
	}

	
	protected IEnumerator SetRegularUserSlot(Transform tr, 	RaceUserInfo mAI){
		int CarID = mAI.carId;
		int CrewID = mAI.crewId;
		
		var childTr = tr.FindChild("crew") as Transform;
		
		childTr.FindChild("icon_crew_1").GetComponent<UISprite>().spriteName = CrewID.ToString();
		childTr.FindChild("icon_crew_2").GetComponent<UISprite>().spriteName = CrewID.ToString()+"A";
		childTr.FindChild("lbName").GetComponent<UILabel>().text = Common_Crew_Status.Get(CrewID).Name;
		
		childTr = tr.FindChild("car") as Transform;
		childTr.FindChild("icon_car").GetComponent<UISprite>().spriteName = CarID.ToString();
		childTr.FindChild("lbName").GetComponent<UILabel>().text = Common_Car_Status.Get(CarID).Name;
		childTr.FindChild("lbClass").GetComponent<UILabel>().text = 
			string.Format(KoStorage.GetKorString("74024"),GV.ChangeCarClassIDString(mAI.carClass));
		childTr= tr.FindChild("profileinfo") as Transform;
		childTr.FindChild("lbNick").GetComponent<UILabel>().text = mAI.userNick;

		//Texture Tex =  (Texture)Resources.Load("ComPicture/"+mAI.userURL, typeof(Texture));
		//childTr.FindChild("pic_icon").GetComponent<UITexture>().mainTexture = Tex;
		//mAI.userProfile =(Texture2D) Tex;

		if(!mAI.userURL.Contains("http")){
		//	mAI.userURL = "MultiCom_"+(Random.Range(0,6)+1).ToString();//+".png";
		//	Texture Tex =  (Texture)Resources.Load("ComPicture/"+mAI.userURL, typeof(Texture));
		//	childTr.FindChild("pic_icon").GetComponent<UITexture>().mainTexture = Tex;
		//	mAI.userProfile =(Texture2D) Tex;

			Texture Tex =  (Texture)Resources.Load("User_Default", typeof(Texture));
			childTr.FindChild("pic_icon").GetComponent<UITexture>().mainTexture = Tex;
			mAI.userProfile =(Texture2D) Tex;
			yield break;
		}else{
			string myProfileUrl = System.Uri.EscapeUriString(mAI.userURL);
			WWW www = new WWW( myProfileUrl );

			yield return www;
			if( this == null )
				yield break;
			if( www.error != null )
			{
				//!!--Utility.Log( "load failed" + mAI.strProfileUrl );
				Texture tx =  (Texture)Resources.Load("User_Default", typeof(Texture));
				childTr.FindChild("pic_icon").GetComponent<UITexture>().mainTexture = tx;
				mAI.userProfile =(Texture2D) tx;
			}
			else
			{
				Texture2D Tex = www.texture;
				childTr.FindChild("pic_icon").GetComponent<UITexture>().mainTexture = Tex;
				mAI.userProfile = Tex;
				www.Dispose();
				Tex = null;
				www = null;
			}
		}
		//		yield return new WaitForSeconds(0.25f);
	}

	void SetRaceInfoWindow(Transform tr, int mode){
		Common_Reward.Item re = Common_Reward.Get(Global.gRewardId);
		var mTr = tr.FindChild("Down") as Transform;
		var Retr = mTr.FindChild("RE") as Transform;
		var reward = mTr.FindChild("REWARD") as Transform;
		if(mode == 0){
			entryDollar = re.Entry_Weekly;
			refreshDollar = re.Refresh_weekly;
		}


		GV.gEntryFee = entryDollar;
		Retr.FindChild("lbprice").GetComponent<UILabel>().text = string.Format("{0}",refreshDollar);
		reward.FindChild("lbEntry").GetComponent<UILabel>().text = string.Format("{0:#,0}", entryDollar);

	}

	int RandomTrack(int mode){
		int track = 1400;
		if(mode == 0){
			track = 1400;
		}else if(mode == 1){
		//	track = 1407;
		}else if(mode == 2){
		//	track = 1412; //Drag
		}else {
		//	track = 1413; //City
		}
		return track;
	}
	private bool bReEnter = false;
	void RandomRegularUser(int nCnt, int track, int mode){

		int mCarCount = 0;
		int mCrewCount = 0;
		int[] carList, crewList;
		if(mode == 0) {

		}else if(mode == 1){
		
		}
	
		int a = Common_Car_Status.stockCarList.Count ;
		int b = Common_Car_Status.SpecialCarList.Count;
		mCarCount =a+b;
		mCrewCount = Common_Team.stockCrewList.Count;
		crewList = new int[mCrewCount];
		carList = new int[mCarCount];
		for(int i = 0; i < mCrewCount;i++){
			crewList[i] = Common_Team.stockCrewList[i];
		}
		for(int i = 0; i<a;i++){
			carList[i] = Common_Car_Status.stockCarList[i];
		}
		for(int i = a; i<mCarCount;i++){
			carList[i] = Common_Car_Status.SpecialCarList[i-a];
		}

		int[] rand = gameObject.AddComponent<RandomCreate>().CreateRandomValue(6);
		int[] rand1 = gameObject.AddComponent<RandomCreate>().CreateRandomValue(mCarCount);

	
	}
}
