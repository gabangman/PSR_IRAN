using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ModePVP : ModeParent {
	List<RaceUserInfo> listDragUser= new List<RaceUserInfo>();//Regular;
	List<RaceUserInfo> listEventUser= new List<RaceUserInfo>();
	List<RaceUserInfo> listTempUser = new List<RaceUserInfo>();
	public override void OnSubWindow ()
	{
		base.OnSubWindow ();
		string name = transform.parent.name;
		string[] str = name.Split('_');
		GameObject obj = null;
		int mode = 0;
		if(str[1].Equals("Drag")){
			obj = transform.GetChild(0).gameObject;// as GameObject;
			mode = 0;
			transform.parent.GetComponent<RaceModeAnimaintion>().SelectAniPlay("ModeWin_PVP_Drag");
		}else{
			obj = transform.GetChild(1).gameObject;// as GameObject;
			mode = 1;
			transform.parent.GetComponent<RaceModeAnimaintion>().SelectAniPlay("ModeWin_PVP_City");
		}
		CheckReEnter(mode);
		obj.SetActive(true);
		//Global.gRegularRaceLevel=	Base64Manager.instance.GlobalEncoding(1);
		int track  = RandomTrack(mode);
		SetBGTrackImg(track);
		RandomRegularUser(track, mode);
		SetRaceInfoWindow(obj.transform, mode);
		SetUserWindow(obj.transform, mode);
		onNext = ()=>{
			//!!--Utility.Log("onNext ");
			ReadyLoadingCircle();
			gameObject.AddComponent<SettingRacePVP>().setPVPRace(mode, track, listTempUser);
		};
		onRefresh = ()=>{
			//!!--Utility.Log("onRefresh ");
			if(mode == 0) transform.parent.GetComponent<RaceModeAnimaintion>().SelectAniPlay("ModeWin_PVP_Drag");
			else transform.parent.GetComponent<RaceModeAnimaintion>().SelectAniPlay("ModeWin_PVP_City");
			SetRefreshCondition(mode);
			RandomRegularUser(track, mode);
			SetUserRefresh(obj.transform, mode);
		};
		
	}
	void SetRefreshCondition(int mode){
		if(mode == 0){  
			listDragUser.Clear();
		}else if(mode == 1){ // drag
			listEventUser.Clear();
		}
	}

	void CheckReEnter(int mode){
		if(bReError){
			bReError = false;
			bReEnter = false;
			SetRefreshCondition(mode);
		}
		
	}
	private void SetUserRefresh(Transform tr, int mode){
		LoadingCircle();
		string raceAPI = string.Empty;
		if(mode == 0) raceAPI = ServerAPI.Get(90038);// "game/race/pvpDrag/refresh";
		else raceAPI =ServerAPI.Get(90042);// "game/race/pvpTimesquare/refresh";
		Global.isNetwork = true;
		Dictionary<string,int> mDic = new Dictionary<string, int>();
		mDic.Add("raceId",GV.ChSeasonID);
		NetworkManager.instance.HttpFormConnect("Put", mDic,raceAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				//response:{"state":0,"msg":"sucess","result":[{"userId":4,"teamId":1,"record":100,"raceData":"test","carId":0,"carClass":0},{"userId":6,"teamId":1,"record":100,"raceData":"s","carId":0,"carClass":0},{"userId":76,"teamId":0,"record":34003.25,"raceData":"","carId":0,"carClass":0},{"userId":204,"teamId":0,"record":0,"raceData":"","carId":0,"carClass":0}],"time":1445956729}
				int cnt = thing["result"].Count;
				for(int  i = 0; i < cnt; i++){
					if(mode == 0){
						listDragUser[i].changeUserInfo(thing["result"],i);
					}else {
						listEventUser[i].changeUserInfo(thing["result"],i);
					}
				}
				if(mode == 0) StartCoroutine("SetDragUser", tr);
				else StartCoroutine("SetEventUser", tr);
			}else{
				StopLoadingCircle();
			}
			Global.isNetwork = false;
		});
	}
	private bool bReEnter = false;
	void SetUserWindow(Transform tr, int mode){
		LoadingCircle();
		string raceAPI = string.Empty;
		if(mode == 0) raceAPI =ServerAPI.Get(90037);// "game/race/pvpDrag/player/";
		else raceAPI = ServerAPI.Get(90041);//"game/race/pvpTimesquare/player/";
		if(bReEnter){
			//listRegular = new List<RegularAIUser>(listStockUser);
			StopLoadingCircle();
			//StartCoroutine("SetStockUser", tr);
			return;
		}
		
		Global.isNetwork = true;
		NetworkManager.instance.HttpConnect("Get", raceAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				//response:{"state":0,"msg":"sucess","result":[{"userId":4,"teamId":1,"record":100,"raceData":"test","carId":0,"carClass":0},{"userId":6,"teamId":1,"record":100,"raceData":"s","carId":0,"carClass":0},{"userId":76,"teamId":0,"record":34003.25,"raceData":"","carId":0,"carClass":0},{"userId":204,"teamId":0,"record":0,"raceData":"","carId":0,"carClass":0}],"time":1445956729}
				int cnt = thing["result"].Count;
				for(int  i = 0; i < cnt; i++){
					if(mode == 0){
						listDragUser[i].changeUserInfo(thing["result"],i);
					}else {
						listEventUser[i].changeUserInfo(thing["result"],i);
					}
				}
				if(mode == 0) StartCoroutine("SetDragUser", tr);
				else StartCoroutine("SetEventUser", tr);
			}else{
				StopLoadingCircle();
			}
			Global.isNetwork = false;
		});

	}

	IEnumerator SetDragUser(Transform tr){
		var child = tr.FindChild("UserSlot") as Transform;
		yield return StartCoroutine(SetPVPSlot(child, listDragUser[0]));
		StopLoadingCircle();
	}
	IEnumerator SetEventUser(Transform tr){
		var child = tr.FindChild("UserSlot") as Transform;
		yield return StartCoroutine(SetPVPSlot(child, listEventUser[0]));
		StopLoadingCircle();
	}

	int checkmyClass(int id){
		int mC =0;
		if(id == 3106){
			mC =3106;
		}else{
			mC = GV.getTeamCarClassId(GV.SelectedTeamID);
		}

		return mC;
	}
	protected IEnumerator SetPVPSlot(Transform tr1, 	RaceUserInfo mAI){
		int CarID = mAI.carId;
		int CrewID = mAI.crewId;
		var tr = tr1.GetChild(1) as Transform;
		var childTr = tr.FindChild("crew") as Transform;
		//int mClass = mAI.carClass;
		mAI.carClass = checkmyClass(mAI.carClass);
		//childTr.FindChild("icon_crew_1").GetComponent<UISprite>().spriteName = CrewID.ToString();
		childTr.FindChild("icon_crew").GetComponent<UISprite>().spriteName = CrewID.ToString()+"A";
		childTr.FindChild("lbClass").GetComponent<UILabel>().text = Common_Crew_Status.Get(CrewID).Name;
		string strClass = GV.ConvertClassInt(mAI.carClass);
		childTr = tr.FindChild("car") as Transform;
		childTr.FindChild("icon_car").GetComponent<UISprite>().spriteName = CarID.ToString();
		childTr.FindChild("lbClass").GetComponent<UILabel>().text = Common_Car_Status.Get(CarID).Name;
		childTr.FindChild("lbClass_Car").GetComponent<UILabel>().text = strClass;
		childTr.FindChild("ClassColor").GetComponent<UISprite>().spriteName = 
			"Class_"+strClass;
		childTr= tr.FindChild("profileinfo") as Transform;
		childTr.FindChild("lbNick").GetComponent<UILabel>().text = mAI.userNick;

		if(!mAI.userURL.Contains("http")){
			Texture Tex =  (Texture)Resources.Load("User_Default", typeof(Texture));
			childTr.FindChild("pic_icon").GetComponent<UITexture>().mainTexture = Tex;
			mAI.userProfile =(Texture2D) Tex;
		}else {
			string myProfileUrls = System.Uri.EscapeUriString(mAI.userURL);
			WWW www = new WWW( myProfileUrls );
			yield return www;
			if( this == null )
				yield break;
			if( www.error != null )
			{
				//!!--Utility.Log( "load failed" + mAI.strProfileUrl );
				Texture tx =  (Texture)Resources.Load("User_Default", typeof(Texture));
				mAI.userProfile =(Texture2D) tx;
				childTr.FindChild("pic_icon").GetComponent<UITexture>().mainTexture = tx;
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
		yield return new WaitForSeconds(0.5f);
		
		CarID =  GV.getTeamCarID(GV.SelectedTeamID);
		CrewID = GV.getTeamCrewID(GV.SelectedTeamID);
		tr = tr1.GetChild(0) as Transform;
		strClass = GV.getTeamCarClass(GV.SelectedTeamID); 
		childTr = tr.FindChild("crew") as Transform;
		//childTr.FindChild("icon_crew_1").GetComponent<UISprite>().spriteName = CrewID.ToString();
		childTr.FindChild("icon_crew").GetComponent<UISprite>().spriteName = CrewID.ToString()+"A";
		childTr.FindChild("lbClass").GetComponent<UILabel>().text = Common_Crew_Status.Get(CrewID).Name;
		
		childTr = tr.FindChild("car") as Transform;
		childTr.FindChild("icon_car").GetComponent<UISprite>().spriteName = CarID.ToString();
		childTr.FindChild("lbClass").GetComponent<UILabel>().text = Common_Car_Status.Get(CarID).Name;
		childTr.FindChild("ClassColor").GetComponent<UISprite>().spriteName = 
			"Class_"+strClass;
		childTr.FindChild("lbClass_Car").GetComponent<UILabel>().text = strClass;
		childTr= tr.FindChild("profileinfo") as Transform;
		childTr.FindChild("lbNick").GetComponent<UILabel>().text = GV.UserNick;
		childTr.FindChild("pic_icon").GetComponent<UITexture>().mainTexture = AccountManager.instance.myPicture;

		/*string myProfileUrl = string.Empty;
		int rans = Random.Range(1,7);
		myProfileUrl = "https://s3-ap-northeast-1.amazonaws.com/gabangman01/MultiPicture/MultiCom_"+rans.ToString()+".png";
		myProfileUrl = System.Uri.EscapeUriString(myProfileUrl);
		www = new WWW( myProfileUrl );
		yield return www;
		if( this == null )
			yield break;
		if( www.error != null )
		{
			//!!--Utility.Log( "load failed" );
			Texture tx =  (Texture)Resources.Load("Kakao_icon", typeof(Texture));
			childTr.FindChild("pic_icon").GetComponent<UITexture>().mainTexture = tx;
			Global.myProfile =(Texture2D)tx;
		}
		else
		{
			Texture2D Tex = www.texture;
			childTr.FindChild("pic_icon").GetComponent<UITexture>().mainTexture = Tex;
			Global.myProfile = (Texture2D)Tex;
			////!!--Utility.Log(Global.myProfile.name);
			www.Dispose();
			Tex = null;
			www = null;
		}*/
	}


	void SetRaceInfoWindow(Transform tr, int mode){
		Common_Reward.Item re = Common_Reward.Get(Global.gRewardId);
		var mTr = tr.FindChild("Down") as Transform;
		var Retr = mTr.FindChild("RE") as Transform;
		var reward = mTr.FindChild("Entryfee") as Transform;
		if(mode == 0){
			entryDollar = re.Entry_drag;
			refreshDollar = re.Refresh_drag;
			mTr.FindChild("REWARD").GetChild(1).FindChild("lbprice").GetComponent<UILabel>().text =
				string.Format("{0:#,0}", re.Reward_PVP_drag);
		}else if( mode == 1){
			entryDollar = re.Entry_timesquare;
			refreshDollar = re.Refresh_timesquare;
			mTr.FindChild("REWARD").GetChild(0).FindChild("Quantity").GetComponent<UILabel>().text =
				string.Format(KoStorage.GetKorString("73406"),  re.Reward_mat_timesquare);
		}
		GV.gEntryFee = entryDollar;
		Retr.FindChild("lbprice").GetComponent<UILabel>().text = string.Format("{0:#,0}",refreshDollar);
	//	Retr.FindChild("lbtitle").GetComponent<UILabel>().text = KoStorage.GetKorString("73005");
		reward.FindChild("lbEntry").GetComponent<UILabel>().text = string.Format("{0:#,0}", entryDollar);
	}

	int RandomTrack(int mode){
		int track = 1400;
		if(mode == 0){
			track = 1412; //Drag
		}else if(mode == 1){
			track = 1413; //City
		}
		return track;
	}

	void RandomRegularUser(int track, int mode){
		int mCarCount = 0;
		int mCrewCount = 0;
		int[] carList, crewList;
		if(mode == 0) {
			if(listDragUser.Count != 0){
				listTempUser = new List<RaceUserInfo>(listDragUser);
				bReEnter = true;
				return;

			}
		}else if(mode == 1){
			if(listEventUser.Count != 0){
				listTempUser = new List<RaceUserInfo>(listEventUser);
				bReEnter = true;
				return;
			}
		}	
			bReEnter = false;
			mCarCount = Common_Car_Status.wholeCarList.Count;
			carList = new int[mCarCount];
			for(int i = 0; i<mCarCount;i++){
				carList[i] = Common_Car_Status.wholeCarList[i];
			}
			int a = Common_Team.tourCrewList.Count;
			int b =  Common_Team.stockCrewList.Count;
			mCrewCount =  a+b;
			crewList = new int[mCrewCount];
			for(int i = 0; i<b;i++){
				crewList[i] = Common_Team.stockCrewList[i];
			}
			for(int i = b; i<mCrewCount;i++){
				crewList[i] = Common_Team.tourCrewList[i-b];
			}

		
		int[] rand = gameObject.AddComponent<RandomCreate>().CreateRandomValue(9);
		int[] rand1 = gameObject.AddComponent<RandomCreate>().CreateRandomValue(mCarCount);


		List<RaceUserInfo> listTemp = new List<RaceUserInfo>();
		int n = Random.Range(0,mCrewCount);
		listTemp.Add(new RaceUserInfo(carList[rand1[0]], crewList[n]));
		int _nick = 3058 + rand[0];
		//listTemp[0].userURL = "https://s3-ap-northeast-1.amazonaws.com/gabangman01/MultiPicture/MultiCom_"+(rand[0]+1).ToString()+".png";
		//listTemp[0].strProfileUrl = "D_"+(_nick).ToString();//+".png";
		listTemp[0].userURL = "User_Default";
		_nick += 70000;
		listTemp[0].userNick = KoStorage.GetKorString(_nick.ToString());
			switch(mode){
			case 0:  listDragUser =new List<RaceUserInfo>(listTemp); break;
			case 1:  listEventUser = new List<RaceUserInfo>(listTemp); break;
			}
		listTempUser = new List<RaceUserInfo>(listTemp);
	}
}
