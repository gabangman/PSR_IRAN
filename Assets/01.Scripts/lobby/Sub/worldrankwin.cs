using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
public class worldrankwin : MonoBehaviour {

	public UILabel[] lbText;
	//public UIDraggablePanel2 dragWorld;
	//public Transform grid;
	public GameObject readyBG;
	public GameObject btnRank;
	public Transform worldTimeBtn, worldThBtn;
	public GameObject throphyList, timeList;
	void Start () {
		lbText[0].text =KoStorage.GetKorString("72200"); //전체 랭킹
		//lbText[1].text =KoStorage.GetKorString("72201");
		lbText[1].text =KoStorage.GetKorString("72203");
		lbText[3].text =KoStorage.GetKorString("72533");
		lbText[5].text =KoStorage.GetKorString("72600"); // 트로피
		lbText[4].text =KoStorage.GetKorString("72601"); //기록
		lbText[2].text = string.Empty;

	
		if(Application.isEditor){
			btnRank.transform.FindChild("icon").GetComponent<UISprite>().spriteName = "SNS_GooglePlay";
			btnRank.transform.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnRankGoogle";
			
		}else{
			if (Application.platform == RuntimePlatform.Android){
				btnRank.transform.FindChild("icon").GetComponent<UISprite>().spriteName = "SNS_GooglePlay";
				btnRank.transform.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnRankGoogle";
			}else if(Application.platform == RuntimePlatform.IPhonePlayer){
				btnRank.transform.FindChild("icon").GetComponent<UISprite>().spriteName = "SNS_GameCenter";
				btnRank.transform.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnRankApple";
			}
		}
		uiPos = new Vector3[4];
		var temp = timeList.transform.FindChild("View").FindChild("View") as Transform;
		uiPos[0] = temp.transform.localPosition;
		uiPos[1] = temp.GetComponent<UIPanel>().clipRange;
		
		temp = throphyList.transform.FindChild("View").FindChild("View") as Transform;
		uiPos[2] = temp.transform.localPosition;
		uiPos[3] = temp.GetComponent<UIPanel>().clipRange;
		
		
	}
	private Vector3[] uiPos;
	void setMyRankLabel(int rank){
		if(rank == 0){
			lbText[2].text =string.Format(KoStorage.GetKorString("72202"),KoStorage.GetKorString("72811"));
		}else{
			lbText[2].text =string.Format(KoStorage.GetKorString("72202"),rank);
		}
	}

	void OnRankApple(){
	
	}

	void OnRankGoogle(){
		SNSManager.OnRankGoogleClick2();
		/*
		SNSManager.OnRankGoogleClick(()=>{
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			pop.AddComponent<SocialLogInPopUp>().InitPopUp(1);
			pop.GetComponent<SocialLogInPopUp>().onFinishCallback(()=>{
				NGUITools.FindInParents<SubMenuWindow>(gameObject).OnChangeWindow(gameObject);
			});
		});
		*/
	}


	public void ShowTotalRankingInfo(string mode){
		worldThBtn.GetComponent<UISprite>().color = Color.white;
		worldTimeBtn.GetComponent<UISprite>().color = Color.gray;
		throphyList.SetActive(true);
		timeList.SetActive(false);
		StartCoroutine("addItemByThrophy");
	}

	IEnumerator addItemByTime(){
		var grid = timeList.transform.FindChild("View").FindChild("View").GetChild(0) as Transform;
		int cnt = grid.childCount;
		var drag = grid.parent.GetComponent<UIDraggablePanel2>() as UIDraggablePanel2;
		if(cnt == 0){
			drag.maxScreenLine = 2;
			drag.maxColLine = -1;
			int count = 0;
			if(gameRank.instance.listRTR.Count == 0){
				yield return StartCoroutine("getWorldRankByTimeList");
				count = gameRank.instance.listRTR.Count;;
			}else{
				count = gameRank.instance.listRTR.Count;
				readyBG.SetActive(false);
			}
			if(count <= 2) count = 4;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().RankingWorldByTime(index);
			});
		}else{
			drag.verticalScrollBar.scrollValue = 0.0f;
			setMyRankLabel(Global.gMyRank[4]);
		}
	
	}


	IEnumerator addItemByThrophy(){
		var grid = throphyList.transform.FindChild("View").FindChild("View").GetChild(0) as Transform;
		int cnt = grid.childCount;
		var drag = grid.parent.GetComponent<UIDraggablePanel2>() as UIDraggablePanel2;
		if(cnt == 0){
			drag.maxScreenLine = 2;
			drag.maxColLine = -1;
			int count = 0;
			if(gameRank.instance.listRTT.Count == 0){
				yield return StartCoroutine("getWorldRankByThrophyList");
				count = gameRank.instance.listRTT.Count;
			}else{
				count = gameRank.instance.listRTT.Count;
				readyBG.SetActive(false);
			}
			if(count <= 2) count = 4;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().RankingWorldByThrophy(index);
			});			
		}else{
			drag.verticalScrollBar.scrollValue = 0.0f;
			setMyRankLabel(Global.gMyRank[3]);
		}
	}
	// 0 : 친구랭킹
	// 1 : weeklyRankByThrophy
	// 2 : weeeklyRankByTime
	// 3 : totalRankByThrophy
	// 4 : totalRankByTime
	public void startWholeRankWindow(){
		StartCoroutine("getWorldRank");
	}
	int TimeCount = 0; int ThrophyCount = 0;
	int TimePage = 0; int ThrophyPage = 0;
	IEnumerator getWorldRankByThrophyList(){
		bool bConnect = true;
		StartCoroutine("loadingStart");
		string mAPI = ServerAPI.Get(90073); //"game/leaderboard/all/tropy/"
		NetworkManager.instance.HttpGetRaceSubInfo("Get",mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					Global.gMyRank[3] = thing["myRank"].AsInt;
					setMyRankLabel(Global.gMyRank[3]);
					ThrophyPage = thing["totalPage"].AsInt;
					ThrophyCount = thing["result"].Count;
				//	Utility.LogWarning("ThrophyPage Page " + ThrophyPage);
					gameRank.instance.InitializeRTTList(thing["result"], ThrophyCount);
					if(ThrophyPage != 1){
					//	gameRank.RaceRankInfo wr = new gameRank.RaceRankInfo();
					///	wr.addPage = 1;
					//	gameRank.instance.listRTT.Add(wr);
					}
			}
			bConnect = false;
		}, null, 1);
		
		while(bConnect){
			yield return null;
		}
		if(ThrophyPage > 7) ThrophyPage = 7;
		for(int i = 2; i <= ThrophyPage; i++){
			bConnect = true;
			NetworkManager.instance.HttpGetRaceSubInfo("Get",mAPI,(request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);

					int status = thing["state"].AsInt;
					if (status == 0)
					{
						TimeCount = thing["result"].Count;
						gameRank.instance.addRTTList(thing["result"], TimeCount);
					}

				bConnect = false;
			}, null, i);
			
			while(bConnect){
				yield return null;
			}
		}
		//	yield return new WaitForSeconds(2.5f);
		isWaiting = false;
		readyBG.SetActive(false);
	}

	IEnumerator getWorldRankByTimeList(){
		bool bConnect = true;
		StartCoroutine("loadingStart");
		string mAPI = ServerAPI.Get(90050);//"game/leaderboard/all/"
		NetworkManager.instance.HttpGetRaceSubInfo("Get",mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
				if (status == 0)
				{
					Global.gMyRank[4] = thing["myRank"].AsInt;
					setMyRankLabel(Global.gMyRank[4]);
					TimePage = thing["totalPage"].AsInt;
					TimeCount = thing["result"].Count;
				//	Utility.LogWarning("TimePage Page " + TimeCount);
					gameRank.instance.InitializeRTRList(thing["result"], TimeCount);
					if(TimePage != 1){
						//gameRank.RaceRankInfo wr = new gameRank.RaceRankInfo();
						//wr.addPage = 1;
						//gameRank.instance.listRTR.Add(wr);
					}
			}
			bConnect = false;
		}, null, 1);
		
		while(bConnect){
			yield return null;
		}
		if(TimePage > 7) TimePage = 7;
		for(int i = 2; i <= TimePage; i++){
			bConnect = true;
			NetworkManager.instance.HttpGetRaceSubInfo("Get",mAPI,(request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
	
					int status = thing["state"].AsInt;
					if (status == 0)
					{

						TimeCount = thing["result"].Count;
						//Utility.LogWarning("TimePage Page "+i+ " : "+ TimeCount);
						gameRank.instance.addRTRList(thing["result"], TimeCount);
					}
				bConnect = false;
			}, null, i);
			
			while(bConnect){
				yield return null;
			}
		}



		//	yield return new WaitForSeconds(2.5f);
		isWaiting = false;
		readyBG.SetActive(false);
	}
	public void StopRankList(){
		isWaiting = false;
		readyBG.SetActive(false);
	}

	public void stopWholeRankWindow(){
		isWaiting = false;
		readyBG.SetActive(false);
		//ProtocolManager.instance.isWorldRankSuccess = false;
	}
	bool isWaiting = false;
	IEnumerator loadingStart(){
		readyBG.SetActive(true);
		isWaiting = true;
		var val = readyBG.transform.GetChild(0).GetComponent<UISprite>() as UISprite;
		//Utility.Log(val.name);
		float delta = 0.05f;
		float mdelta = 0.0f;
		while(isWaiting){
			val.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			yield return null;
		}
	}
	
	IEnumerator getWorldRank(){
		yield return null;
	}
	void testCicle(){
		readyBG.SetActive(false);
		isWaiting = false;
		StopCoroutine("loadingStart");
	}

	void OnDestroy(){
		//gameRank.instance.listworld.Clear();
	}

	void OnWorldTimeClick(){
		if(isWaiting) return;
		if(worldTimeBtn.GetComponent<UISprite>().color == Color.white) return;
		worldTimeBtn.GetComponent<UISprite>().color = Color.white;
		worldThBtn.GetComponent<UISprite>().color = Color.gray;
		//==!!Utility.LogWarning("OnWorldTimeClick");
		throphyList.SetActive(false);
		timeList.SetActive(true);
		StartCoroutine("addItemByTime");
		//addItemWorld();
	}

	void OnWorldThrophyClick(){
		if(isWaiting) return;
		if(worldThBtn.GetComponent<UISprite>().color == Color.white) return;
		worldThBtn.GetComponent<UISprite>().color = Color.white;
		worldTimeBtn.GetComponent<UISprite>().color = Color.gray;
		//==!!Utility.LogWarning("OnWorldThrophyClick");
		throphyList.SetActive(false);
		timeList.SetActive(true);
		throphyList.SetActive(true);
		timeList.SetActive(false);
		StartCoroutine("addItemByThrophy");
		//	addItemWeek();
	}

	public void addNextThrophyRank(){
		CheckThrophyList();
		StartCoroutine("RecvNextThrophyRank");
		addItemByThrophy();
	}


	IEnumerator RecvNextThrophyRank(){
		bool bConnect = true;
		StartCoroutine("loadingStart");
		Global.isNetwork = true;
		string mAPI = ServerAPI.Get(90073);//"game/leaderboard/all/tropy/"
		for(int i = 2; i <= ThrophyPage; i++){
			NetworkManager.instance.HttpGetRaceSubInfo("Get",mAPI,(request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
	
					int status = thing["state"].AsInt;
					if (status == 0)
					{
						TimeCount = thing["result"].Count;
						gameRank.instance.addRTTList(thing["result"], TimeCount);
					}
				bConnect = false;
			}, null, i);
			
			while(bConnect){
				yield return null;
			}
		}



		isWaiting = false;
		readyBG.SetActive(false);Global.isNetwork = false;
	}
	void CheckThrophyList(){
		int cnt = gameRank.instance.listRTT.Count;
		gameRank.instance.listRTT.RemoveAt(cnt-1);
		var temp = throphyList.transform.FindChild("View").FindChild("View") as Transform;
		var mGrid = temp.FindChild("Grid").gameObject as GameObject;
	//	Vector3 pos = temp.transform.localPosition;
	//	Vector3 pos1 = temp.GetComponent<UIPanel>().clipRange;

		for(int i = mGrid.transform.childCount; i > 0; i--){
			DestroyImmediate(mGrid.transform.GetChild(i-1).gameObject);
		}

		temp.localPosition = uiPos[2];
		temp.GetComponent<UIPanel>().clipRange = uiPos[3];
		temp.GetComponent<UIDraggablePanel2>().reSetGridPannel();
	}


	public void addNextTimeRank(){
		CheckTimeList();
		StartCoroutine("RecvNextTimeRank");
		addItemByTime();
	}



	IEnumerator RecvNextTimeRank(){
		bool bConnect = true;
		StartCoroutine("loadingStart");
		Global.isNetwork = true;
		//string mAPI = ServerAPI.Get(90050);
		for(int i = 2; i <= ThrophyPage; i++){
			NetworkManager.instance.HttpGetRaceSubInfo("Get","game/leaderboard/all/",(request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);

					int status = thing["state"].AsInt;
					if (status == 0)
					{
						TimeCount = thing["result"].Count;
						gameRank.instance.addRTRList(thing["result"], TimeCount);
					}
				bConnect = false;
			}, null, i);
			
			while(bConnect){
				yield return null;
			}
		}
		//	yield return new WaitForSeconds(2.5f);
		isWaiting = false;
		readyBG.SetActive(false);
		Global.isNetwork = false;
	}
	
	void CheckTimeList(){
		int cnt = gameRank.instance.listRTR.Count;
		gameRank.instance.listRTR.RemoveAt(cnt-1);
		var temp = timeList.transform.FindChild("View").FindChild("View") as Transform;
		var mGrid = temp.FindChild("Grid").gameObject as GameObject;
	//	Vector3 pos = temp.transform.localPosition;
	//	Vector3 pos1 = temp.GetComponent<UIPanel>().clipRange;
		
		for(int i = mGrid.transform.childCount; i > 0; i--){
			DestroyImmediate(mGrid.transform.GetChild(i-1).gameObject);
		}
		
		temp.localPosition = uiPos[0];
		temp.GetComponent<UIPanel>().clipRange =  uiPos[1];
		temp.GetComponent<UIDraggablePanel2>().reSetGridPannel();
	}


}
