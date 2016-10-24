using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModeRegular : ModeParent {
	List<RegularUserInfo> listRegular;
	List<RegularUserInfo> listStockUser = new List<RegularUserInfo>();//Regular;
	List<RegularUserInfo> listDragUser= new List<RegularUserInfo>();
	
	public override void OnSubWindow ()
	{
		base.OnSubWindow ();
		string name = transform.parent.name;
		string[] str = name.Split('_');
		int mode = 0;
		Transform tr; 
		
		if(str[1].Equals("Drag")){
			tr = transform.GetChild(1) as Transform;
			tr.gameObject.SetActive(true);
			//Drag
			mode = 1;
			transform.parent.GetComponent<RaceModeAnimaintion>().SelectAniPlay("ModeWin_Regular_Drag");
			
		}else {
			tr = transform.GetChild(0) as Transform;
			tr.gameObject.SetActive(true);
			mode = 0;
			transform.parent.GetComponent<RaceModeAnimaintion>().SelectAniPlay("ModeWin_Regular_Track");
			Global.gChampTutorial = 0;
		}


		CheckReEnter(mode);
		Global.gRegularRaceLevel = Base64Manager.instance.GlobalEncoding(1);
		int track  = RandomTrack(mode);
		SetBGTrackImg(track);
		RandomRegularUser(4, mode);
		SetRaceInfoWindow(tr, mode);
		
		SetUserWindow(tr, mode);
		onNext = ()=>{
			////!!--Utility.Log("onNext ");
			ReadyLoadingCircle();
			gameObject.AddComponent<SettingRaceRegular>().setRegularRace(mode, listRegular, track);
		};
		onRefresh = ()=>{
			transform.parent.GetComponent<RaceModeAnimaintion>().SelectAniPlay("ModeWin_Regular_Drag");
			//	//!!--Utility.Log("onRefresh " + mode);
			SetRefreshCondition(mode);
			RandomRegularUser(4, mode);
			SetUserRefresh(tr, mode);
		};
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
			listStockUser.Clear();
		}else if(mode == 1){ // drag
			listDragUser.Clear();
		}
	}
	
	private void SetUserRefresh(Transform tr, int mode){
		LoadingCircle();
		string raceAPI = string.Empty;
		if(mode == 0) raceAPI = ServerAPI.Get(90030);// "game/race/regularTrack/refresh";
		else raceAPI = ServerAPI.Get(90034);//"game/race/regularDrag/refresh";
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
						listStockUser[i].changeUserInfo(thing["result"], i);
					}else{
						listDragUser[i].changeUserInfo(thing["result"],i );
					}
				}
				if(mode == 0) StartCoroutine("SetRegularUser", tr);
				else StartCoroutine("SetDragUser", tr);
			}else{
				StopLoadingCircle();
			}
			Global.isNetwork = false;
		});
		
	}
	
	private bool bReEnter = false;
	private void SetUserWindow(Transform tr, int mode){
		LoadingCircle();
		if(bReEnter){
			StopLoadingCircle();
			return;
		}
		string raceAPI = string.Empty;
		if(mode == 0) raceAPI = ServerAPI.Get(90029);// "game/race/regularTrack/player/";
		else raceAPI = ServerAPI.Get(90033);//"game/race/regularDrag/player/";
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
						listStockUser[i].changeUserInfo(thing["result"], i);
					}else{
						listDragUser[i].changeUserInfo(thing["result"],i );
					}
				}
				if(mode == 0) StartCoroutine("SetRegularUser", tr);
				else StartCoroutine("SetDragUser", tr);
				bReEnter = true;
			}else{
				StopLoadingCircle();
			}
			Global.isNetwork = false;
		});
		
		
	}
	
	IEnumerator SetRegularUser(Transform tr){
		var child = tr.FindChild("UserSlot") as Transform;
		//StartCoroutine("TenSecondTimeCheck");
		for(int i = 0; i < listStockUser.Count;i++){
			yield return StartCoroutine(SetRegularUserSlot(child.GetChild(i), listStockUser[i]));
		}
		StopLoadingCircle();
	}
	IEnumerator SetDragUser(Transform tr){
		var child = tr.FindChild("UserSlot") as Transform;
		//StartCoroutine("TenSecondTimeCheck");
		yield return StartCoroutine(SetDragUserSlot(child, listDragUser[0]));
		StopLoadingCircle();
	}
	
	
	protected IEnumerator SetRegularUserSlot(Transform tr, 	RegularUserInfo mAI){
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
			string.Format(KoStorage.GetKorString("74024"),GV.ConvertClassInt(mAI.carClass));
		childTr= tr.FindChild("profileinfo") as Transform;
		childTr.FindChild("lbNick").GetComponent<UILabel>().text = mAI.userNick;
		
		mAI.userURL = "MultiCom_"+(Random.Range(0,8)+1).ToString();//+".png";
		Texture Tex =  (Texture)Resources.Load("ComPicture/"+mAI.userURL, typeof(Texture));
		childTr.FindChild("pic_icon").GetComponent<UITexture>().mainTexture = Tex;
		mAI.userProfile =(Texture2D) Tex;
		
	
		yield return new WaitForSeconds(0.25f);
		
	}
	
	protected IEnumerator SetDragUserSlot(Transform tr1, 	RegularUserInfo mAI){
		int CarID = mAI.carId;
		int CrewID = mAI.crewId;
		string strClass = GV.ConvertClassInt(mAI.carClass);
		var tr = tr1.GetChild(1) as Transform;
		var childTr = tr.FindChild("crew") as Transform;
		//childTr.FindChild("icon_crew_1").GetComponent<UISprite>().spriteName = CrewID.ToString();
		childTr.FindChild("icon_crew").GetComponent<UISprite>().spriteName = CrewID.ToString()+"A";
		childTr.FindChild("lbClass").GetComponent<UILabel>().text = Common_Crew_Status.Get(CrewID).Name;
		
		childTr = tr.FindChild("car") as Transform;
		childTr.FindChild("icon_car").GetComponent<UISprite>().spriteName = CarID.ToString();
		childTr.FindChild("lbClass").GetComponent<UILabel>().text = Common_Car_Status.Get(CarID).Name;
		childTr.FindChild("lbClass_Car").GetComponent<UILabel>().text = strClass;
		childTr.FindChild("ClassColor").GetComponent<UISprite>().spriteName = 
			"Class_"+strClass;
		childTr= tr.FindChild("profileinfo") as Transform;
		childTr.FindChild("lbNick").GetComponent<UILabel>().text = mAI.userNick;
		
		//mAI.userURL = "MultiCom_"+(Random.Range(0,6)+1).ToString();//+".png";
		Texture Tex =  (Texture)Resources.Load("ComPicture/"+mAI.userURL, typeof(Texture));
		childTr.FindChild("pic_icon").GetComponent<UITexture>().mainTexture = Tex;
		mAI.userProfile =(Texture2D) Tex;
		/*
		if(!mAI.userURL.Contains("http")){
			mAI.userURL = "MultiCom_"+(Random.Range(0,6)+1).ToString();//+".png";
			Texture Tex =  (Texture)Resources.Load("ComPicture/"+mAI.userURL, typeof(Texture));
			childTr.FindChild("pic_icon").GetComponent<UITexture>().mainTexture = Tex;
			mAI.userProfile =(Texture2D) Tex;
			yield break;
		}
		string myProfileUrl = System.Uri.EscapeUriString(mAI.userURL);
		WWW www = new WWW( myProfileUrl );		
		yield return www;
		if( this == null )
			yield break;
		if( www.error != null )
		{
			//!!--Utility.Log( "load failed" + mAI.strProfileUrl );
			Texture tx =  (Texture)Resources.Load("Kakao_icon", typeof(Texture));
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
		*/
		CarID =  GV.getTeamCarID(GV.SelectedTeamID);
		CrewID = GV.getTeamCrewID(GV.SelectedTeamID);
		strClass = GV.getTeamCarClass(GV.SelectedTeamID);
		tr = tr1.GetChild(0) as Transform;
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
		//yield return null;
		yield return new WaitForSeconds(0.5f);
	}

	void SetRaceInfoWindow(Transform tr, int mode){
		//	//!!--Utility.Log("Regular setRaceinfo");
		//return;
		Common_Reward.Item re = Common_Reward.Get(Global.gRewardId);
		var child = tr.FindChild("Down")as Transform;
		var reward =child.FindChild("REWARD") as Transform;
		refreshDollar = re.Refresh_regular;
		var Retr =child.FindChild("RE") as Transform;
		Retr.FindChild("lbprice").GetComponent<UILabel>().text = 
			string.Format("{0:#,0}",re.Refresh_regular);//.ToString();
		if(mode == 0){
			reward.FindChild("Dollor").FindChild("lbprice").GetComponent<UILabel>().text = string.Format("{0:#,0}",re.Reward_regular_stock);//.ToString();
			entryDollar = re.Entry_regular_stock;
		}else{
			//drag
			reward.FindChild("Material").FindChild("Quantity").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("73406"), re.Reward_mat_regular_drag);//string.Format("{0:#,0}",re.Reward_mat_regular_drag);//.ToString();
			entryDollar = re.Entry_regular_drag;
		}
		GV.gEntryFee = entryDollar;
		child.FindChild("Entryfee").FindChild("lbEntry").GetComponent<UILabel>().text = string.Format("{0:#,0}", re.Entry_regular_drag);
		child.FindChild("Entryfee").FindChild("lbFee").GetComponent<UILabel>().text 
			= KoStorage.GetKorString("73006");
		//	child.FindChild("lbtitle").GetComponent<UILabel>().text = KoStorage.GetKorString("73005");
	}
	
	int RandomTrack(int mode){
		int track = 0;
		if(mode == 1){ // drag
			track =1412;
		}else{
			//track = 1406;
			//GV.RegularTrack = Random.Range(1401,1407);
			track = GV.RegularTrack;
		}
		return track;
	}
	
	void RandomRegularUser(int nCnt, int mode){
		int mCarCount = 0;
		int mCrewCount = 0;
		int[] carList, crewList;
		if(mode == 0) {
			if(listStockUser.Count != 0) {
				listRegular = new List<RegularUserInfo>(listStockUser);
				bReEnter = true;
				return;
			}bReEnter = false;
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
			nCnt = 4;
		}else {
			if(listDragUser.Count != 0){
				listRegular = new List<RegularUserInfo>(listDragUser);
				bReEnter = true;
				return;
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
			nCnt = 1;
		}
		
		int[] rand = gameObject.AddComponent<RandomCreate>().CreateRandomValue(6);
		int[] rand1 = gameObject.AddComponent<RandomCreate>().CreateRandomValue(mCarCount);
		Common_Reward.Item reward = Common_Reward.Get(Global.gRewardId);
		//Common_Class.Item cItem = Common_Class.Get(randomClassId(reward));
		List<RegularUserInfo> listTemp = new List<RegularUserInfo>();
		for(int i =0; i < nCnt; i++){
			int n = Random.Range(0,mCrewCount);
			int sponID = Random.Range(1300,1306);
			listTemp.Add(new RegularUserInfo(carList[rand1[i]], crewList[n],sponID));
			int _nick = 73032 + rand[i];
			listTemp[i].userNick = KoStorage.GetKorString(_nick.ToString());
			//listTemp[i].userURL = "https://s3-ap-northeast-1.amazonaws.com/gabangman01/MultiPicture/MultiCom_"+(rand[i]+1).ToString()+".png";
			listTemp[i].userURL = "MultiCom_"+(rand[i]+1).ToString();//+".png";
			listTemp[i].carClass = randomClassId(reward);
			//Utility.LogWarning(listTemp[i].carClass );
		}
		
		if(mode == 0) listStockUser = new List<RegularUserInfo>(listTemp);
		else listDragUser = new List<RegularUserInfo>(listTemp);
		listRegular = new List<RegularUserInfo>(listTemp);
	}
	
	int randomClassId(Common_Reward.Item rItem){
		int rClass = 0;
		ProbabilityClass pb = new ProbabilityClass();
		rClass = pb.RegularRaceClass(rItem);
		pb =null;
		return rClass;
	}
}
