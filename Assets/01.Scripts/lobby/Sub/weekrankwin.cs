using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
public class weekrankwin : MonoBehaviour {
	
	public UILabel[] lbText;
	//public UIDraggablePanel2 dragWorld;
	//public Transform grid;
	public GameObject readyBG;
	//public GameObject btnRank;
	public Transform worldTimeBtn, worldThBtn;
	public GameObject throphyList, timeList;
	void Start () {
		lbText[0].text =KoStorage.GetKorString("72801"); //전체 랭킹
		lbText[1].text =KoStorage.GetKorString("72201");
		
		lbText[3].text =KoStorage.GetKorString("72533");
		lbText[5].text =KoStorage.GetKorString("72600"); // 트로피
		lbText[4].text =KoStorage.GetKorString("72601"); //기록
		//lbText[2].text = string.Format(KoStorage.GetKorString("72202"),KoStorage.GetKorString("72811"));
		lbText[2].text = string.Empty;

		
		
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
			lbText[2].text =string.Format(KoStorage.GetKorString("72802"),KoStorage.GetKorString("72811"));
		}else{
			lbText[2].text =string.Format(KoStorage.GetKorString("72802"),rank);
		}
	}
	public void ShowWeekRankingInfo(string mode){
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
			if(gameRank.instance.listRWR.Count == 0){
				yield return StartCoroutine("getWeeklyRankByTimeList",1);
				count= gameRank.instance.listRWR.Count;
			}else{
				count = gameRank.instance.listRWR.Count ;
				readyBG.SetActive(false);
			}
			
			if(count <= 2) count = 4;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().RankingWeeklyByTime(index);
			});
		}else{
			drag.verticalScrollBar.scrollValue = 0.0f;
			setMyRankLabel(Global.gMyRank[2]);
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
			if(gameRank.instance.listRWT.Count == 0){
				yield return StartCoroutine("getWeeklyRankByThrophyList",1);
				count = gameRank.instance.listRWT.Count;
			}else{
				count = gameRank.instance.listRWT.Count;
				readyBG.SetActive(false);
			}
			
			if(count <= 2) count = 4;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().RankingWeeklyByThrophy(index);
			});			
		}else{
			drag.verticalScrollBar.scrollValue = 0.0f;
			setMyRankLabel(Global.gMyRank[1]);
		}
	}
	
	
	public void startWholeRankWindow(){
		StartCoroutine("getWorldRank");
	}
	int TimeCount = 0; int ThrophyCount = 0;
	int TimePage = 0; int ThrophyPage = 0;
	IEnumerator getWeeklyRankByThrophyList(int page = 1){
		bool bConnect = true;
		StartCoroutine("loadingStart");
		string mAPI = ServerAPI.Get(90075);
		NetworkManager.instance.HttpGetRaceSubInfo("Get",mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			//response:{"state":0,"msg":"sucess","myRank":1,"result":[{"userId":299,"record":70.18,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"3","tropy2":"0","tropy3":"0","raceData":"TestRaceData"},{"userId":290,"record":80.12,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"4","tropy2":"0","tropy3":"0","raceData":"TestRaceData"},{"userId":300,"record":120.2,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"2","tropy2":"0","tropy3":"0","raceData":"TestRaceData"}],"time":1451286762}
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			{
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					Global.gMyRank[1] = thing["myRank"].AsInt;
					setMyRankLabel(Global.gMyRank[1] );
					ThrophyPage = thing["totalPage"].AsInt;
					//Utility.LogWarning("Throphy Page " + ThrophyPage);
					ThrophyCount = thing["result"].Count;
					gameRank.instance.InitializeRWTList(thing["result"], ThrophyCount);
					
					if(ThrophyPage != 1){
						
					}
				}
			}
			bConnect = false;
		}, null, page);
		
		while(bConnect){
			yield return null;
		}

		if(ThrophyPage > 7) ThrophyPage = 7;
		for(int i = 2; i <= ThrophyPage;i++){
			bConnect = true;
			NetworkManager.instance.HttpGetRaceSubInfo("Get",mAPI,(request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				//response:{"state":0,"msg":"sucess","myRank":1,"result":[{"userId":299,"record":70.18,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"3","tropy2":"0","tropy3":"0","raceData":"TestRaceData"},{"userId":290,"record":80.12,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"4","tropy2":"0","tropy3":"0","raceData":"TestRaceData"},{"userId":300,"record":120.2,"teamId":10,"carId":1000,"carClass":3101,"tropy1":"2","tropy2":"0","tropy3":"0","raceData":"TestRaceData"}],"time":1451286762}
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				{
					int status = thing["state"].AsInt;
					if (status == 0)
					{
						//Global.gMyRank[1] = thing["myRank"].AsInt;
						//setMyRankLabel(Global.gMyRank[1] );
						//ThrophyPage = thing["totalPage"].AsInt;
						//ThrophyCount = thing["result"].Count;
						gameRank.instance.addRWTList(thing["result"], ThrophyCount);
					}
				}
				bConnect = false;
			}, null, i);
			
			while(bConnect){
				yield return null;
			}
		}
		
		
		
		isWaiting = false;
		readyBG.SetActive(false);
	}
	
	// 0 : 친구랭킹
	// 1 : weeklyRankByThrophy
	// 2 : weeeklyRankByTime
	
	IEnumerator getWeeklyRankByTimeList(int page = 1){
		bool bConnect = true;
		StartCoroutine("loadingStart");
		string mAPI = ServerAPI.Get(90074);
		NetworkManager.instance.HttpGetRaceSubInfo("Get",mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			{
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					
					Global.gMyRank[2] = thing["myRank"].AsInt;
					setMyRankLabel(Global.gMyRank[2] );
					TimePage = thing["totalPage"].AsInt;
					TimeCount = thing["result"].Count;
					gameRank.instance.InitializeRWRList(thing["result"], TimeCount);
				//	Utility.LogWarning("TimePage Page " + TimePage);
					if(TimePage != 1){
						//	gameRank.RaceRankInfo wr = new gameRank.RaceRankInfo();
						//	wr.addPage = 1;
						//	gameRank.instance.listRWR.Add(wr);
					}
				}
			}
			bConnect = false;
		}, null, page);
		
		while(bConnect){
			yield return null;
		}
		
	
		if(TimePage > 7) TimePage = 7;
		for(int i = 2; i <= TimePage;i++){
			bConnect = true;
			NetworkManager.instance.HttpGetRaceSubInfo("Get",mAPI,(request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				{
					int status = thing["state"].AsInt;
					if (status == 0)
					{
						gameRank.instance.addRWRList(thing["result"], ThrophyCount);
					}
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
		/*if(gameRank.instance.listworld.Count != 0) {
			yield break;
		}
		StartCoroutine("loadingStart");
		yield return ProtocolManager.instance.StartCoroutine("getWorldRankToServer");
		do{
			yield return null;
		}while(ProtocolManager.instance.isWorldRankSuccess);
		AddItem();
		Invoke("testCicle", 1.0f);*/
		
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
		string mAPI = ServerAPI.Get(90075);
		for(int i = 2; i <= ThrophyPage; i++){
			NetworkManager.instance.HttpGetRaceSubInfo("Get",mAPI,(request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				{
					int status = thing["state"].AsInt;
					if (status == 0)
					{
						TimeCount = thing["result"].Count;
						gameRank.instance.addRWTList(thing["result"], TimeCount);
					}
				}
				bConnect = false;
			}, null, i);
			
			while(bConnect){
				yield return null;
			}
		}
		isWaiting = false;
		readyBG.SetActive(false);
		Global.isNetwork = false;
	}
	void CheckThrophyList(){
		int cnt = gameRank.instance.listRWT.Count;
		gameRank.instance.listRWR.RemoveAt(cnt-1);
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
		string mAPI = ServerAPI.Get(90074);
		for(int i = 2; i <= ThrophyPage; i++){
			NetworkManager.instance.HttpGetRaceSubInfo("Get",mAPI,(request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				{
					int status = thing["state"].AsInt;
					if (status == 0)
					{
						TimeCount = thing["result"].Count;
						gameRank.instance.addRWRList(thing["result"], TimeCount);
					}
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
		int cnt = gameRank.instance.listRWT.Count;
		gameRank.instance.listRWT.RemoveAt(cnt-1);
		var temp = timeList.transform.FindChild("View").FindChild("View") as Transform;
		var mGrid = temp.FindChild("Grid").gameObject as GameObject;
		//	Vector3 pos = temp.transform.localPosition;
		//	Vector3 pos1 = temp.GetComponent<UIPanel>().clipRange;
		
		for(int i = mGrid.transform.childCount; i > 0; i--){
			DestroyImmediate(mGrid.transform.GetChild(i-1).gameObject);
		}
		
		temp.localPosition = uiPos[0];
		temp.GetComponent<UIPanel>().clipRange = uiPos[1];
		temp.GetComponent<UIDraggablePanel2>().reSetGridPannel();
	}
	
	
	
}
