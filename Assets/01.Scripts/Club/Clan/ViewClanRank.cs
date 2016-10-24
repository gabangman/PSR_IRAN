using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ViewClanRank : InfoWindow {

	private Transform grid;
	private Transform view;
	public Transform global, local;
	public GameObject GlobalBoard, LocalBoard, ReadyBG;
	public Color pressColor, ReleaseColor;
	private bool isButtonPress = false;
	public UILabel lbtext,lbLocal;
	void OnEnable(){
		showSub(global.parent.gameObject);
		showSub(local.parent.gameObject);
		ResetButtonPress();
		if(CClub.ClanMember == 0){
			lbLocal.text = GV.gNationName;
		}else{
			lbLocal.text = CClub.mClubInfo.mLocale;
		}

	}

	void OnDisable(){
	}

	void ResetButtonPress(){
		isButtonPress = false;
	}

	public override void HiddenWindow ()
	{
		if(global.parent.gameObject.activeSelf) hiddenSub(global.parent.gameObject);
		if(local.parent.gameObject.activeSelf) hiddenSub(local.parent.gameObject);
	}
	void Start(){
		local.GetComponent<UISprite>().color = pressColor;
		global.GetComponent<UISprite>().color = ReleaseColor;
		LocalBoard.SetActive(true);
		view = LocalBoard.transform.FindChild("View");
		grid = view.FindChild("grid");

	}

	void Awake(){
		CClub.localTime = NetworkManager.instance.GetCurrentDeviceTime();
		CClub.GlobalTime = NetworkManager.instance.GetCurrentDeviceTime();
		CClub.mRankingLocal=new List<ClubInfoRanking>();
		CClub.mRankingGlobal=new List<ClubInfoRanking>();
	
	}

	void LocalAddItem(){
		StopLoadingCircle();
		LocalBoard.SetActive(true); //true
		UIDraggablePanel2 drag = view.GetComponent<UIDraggablePanel2>();
		drag.maxScreenLine = 1;
		drag.maxColLine = 0;
		int count = CClub.mRankingLocal.Count;
		if(grid.childCount == 0 ){
			if(count <= 2) count = 4;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().ViewRankLocal(index);
			});
		}else{
			int mCnt = grid.childCount;
			for(int  i =0; i < mCnt;i++){
				var t = grid.GetChild(i).gameObject as GameObject;
				if(t.activeSelf){
					t.GetComponent<ViewRankItem>().ChangeContentIndex(0);
				}
			}
		}
	}
	void GlobalAddItem(){
		StopLoadingCircle();
		GlobalBoard.SetActive(true); //false
		UIDraggablePanel2 drag = view.GetComponent<UIDraggablePanel2>();
		drag.maxScreenLine = 1;
		drag.maxColLine = 0;
		int count = CClub.mRankingGlobal.Count;
		if(grid.childCount == 0 ){
			if(count <=2) count = 4;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().ViewRankGlobal(index);
			});
		}else{
			int mCnt = grid.childCount;
			for(int  i =0; i < mCnt;i++){
				var t = grid.GetChild(i).gameObject as GameObject;
				if(t.activeSelf){
					t.GetComponent<ViewRankItem>().ChangeContentIndex(1);
				}
			}
		}
	}

	public void AddViewClanRank(){
		if(Global.isNetwork)return;
		StartCoroutine("LoadingCircle");
		local.GetComponent<UISprite>().color = pressColor;
		global.GetComponent<UISprite>().color = ReleaseColor;
		AddViewLocalRank();



	}
	IEnumerator getRankingLocal(){
	
		if(CClub.mRankingLocal.Count !=0) {
			System.TimeSpan sTime = NetworkManager.instance.GetCurrentDeviceTime() -CClub.localTime;
			if(sTime.TotalMinutes < 2){
				LocalAddItem();
				Global.isNetwork = false;
				Utility.LogWarning(sTime.TotalMinutes);
				yield break;
			}
			Utility.LogWarning(sTime.TotalMinutes);
		}



		bool bConnect = false;
		string mAPI = "club/getClubRankingLocal";///club/getClubRankingLocal
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		NetworkManager.instance.ClubBaseStringConnect("Get",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				//"clubRank":"temp","rankingList":
				CClub.mRankingLocal.Clear();
				int clubRank = thing["clubRank"].AsInt;
				int cnt = thing["rankingList"].Count;
				for(int i =0; i < cnt; i++){
					CClub.mRankingLocal.Add(new ClubInfoRanking(thing["rankingList"][i]));
				}
				CClub.localTime = NetworkManager.instance.GetCurrentDeviceTime();
			}else{
				
				
			}
			
	
			bConnect = true;
			
		});
		
		while(!bConnect){
			yield return null;
		}
		LocalAddItem();
		lbtext.text = string.Format(KoStorage.GetKorString("77312"), 10);
		Global.isNetwork = false;
	
	}


	void AddViewLocalRank(){
		Global.isNetwork = true;
		LocalBoard.SetActive(false); //true
		GlobalBoard.SetActive(false); //false
		view = LocalBoard.transform.FindChild("View");
		grid = view.FindChild("grid");
		StartCoroutine("getRankingLocal");

	}
	void AddViewGlobalRank(){
		Global.isNetwork = true;
		LocalBoard.SetActive(false); // false
		GlobalBoard.SetActive(false); //true;
		view = GlobalBoard.transform.FindChild("View");
		grid = view.FindChild("grid");
		StartCoroutine("getRankingGlobal");
	}
	void OnLocalRank(){
		if(isButtonPress) return;
		if(Global.isNetwork) return;
		isButtonPress = true; Invoke ("ResetButtonPress",0.5f);
		if(local.GetComponent<UISprite>().color == pressColor) return;
		local.GetComponent<UISprite>().color = pressColor;
		global.GetComponent<UISprite>().color =  ReleaseColor;
		StartCoroutine("LoadingCircle");

		AddViewLocalRank();
	}
	
	void OnGlobalRank(){
		if(Global.isNetwork) return;
		if(isButtonPress) return;
		isButtonPress = true; Invoke ("ResetButtonPress",0.5f);
		if(global.GetComponent<UISprite>().color == pressColor) return;
		global.GetComponent<UISprite>().color = pressColor;
		local.GetComponent<UISprite>().color =  ReleaseColor;
		StartCoroutine("LoadingCircle");
		AddViewGlobalRank();
	}
	IEnumerator getRankingGlobal(){
		if(CClub.mRankingGlobal.Count !=0) {
			System.TimeSpan sTime = NetworkManager.instance.GetCurrentDeviceTime() -CClub.GlobalTime;
			if(sTime.TotalMinutes < 2){
				GlobalAddItem();
				Utility.LogWarning(sTime.TotalMinutes);
				Global.isNetwork = false;
				yield break;
			}
		}
	//	if(CClub.mRankingGlobal.Count !=0) {
	//		GlobalAddItem();
	//		yield break;
	//	}
		bool bConnect = false;
		string mAPI = "club/getClubRankingGlobal";///club/getClubRankingLocal
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		NetworkManager.instance.ClubBaseStringConnect("Get",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				CClub.mRankingGlobal.Clear();
				int clubRank = thing["clubRank"].AsInt;
				int cnt = thing["rankingList"].Count;
				for(int i =0; i < cnt; i++){
					CClub.mRankingGlobal.Add(new ClubInfoRanking(thing["rankingList"][i]));

				}
				//==!!Utility.LogWarning(CClub.mRankingGlobal.Count);
				CClub.GlobalTime = NetworkManager.instance.GetCurrentDeviceTime();
			}else{
				

			}
			
			bConnect = true;
			
		});
		
		while(!bConnect){
			yield return null;
		}
		GlobalAddItem();
		lbtext.text = string.Format(KoStorage.GetKorString("77313"), 10);
		Global.isNetwork = false;
	

	}





	IEnumerator LoadingCircle(){
		isWait = true;
		ReadyBG.SetActive(true);
		var circle = ReadyBG.transform.GetChild(0).GetComponent<UISprite>() as UISprite;
		circle.fillAmount = 0.0f;
		int cnt = 0;
		while(isWait){
			cnt++;
			circle.fillAmount = (float)cnt*0.1f;
			yield return new WaitForSeconds(0.1f);
		}
	}
	private bool isWait = false;
	void StopLoadingCircle(){
		isWait = false;
		ReadyBG.SetActive(false);
	}

}
