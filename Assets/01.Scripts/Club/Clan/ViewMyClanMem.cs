using UnityEngine;
using System.Collections;

public class ViewMyClanMem : MonoBehaviour {
	public Transform grid;
	public Transform view;
	public GameObject clubPrefabs;
	public GameObject ListView;
	void Awake(){
		ListView = null;
	}
	void Start(){
		transform.FindChild("Btn_ModifyInfo").GetComponent<UIButtonMessage>().functionName = "OnModifyClan";
		transform.FindChild("BtnClubRace").GetChild(0).GetComponent<UIButtonMessage>().target = gameObject;
	
	}

	public void RefreshClubInfo(System.Action<int> callback){
		gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
		MyClanMemberInit();
		StartCoroutine("refreshClub",callback);
	
	}

	IEnumerator refreshClub(System.Action<int> callback){
		Global.isNetwork = true;
		CClub.InitClub();
		yield return AccountManager.instance.StartCoroutine("getClubAccount");
		Global.isNetwork = false;
//		Invoke("stopLoadingCircle", 1.0f);

		if(CClub.ClanMember == 0 && CClub.mClubFlag == 1 && CClub.ClubMode == 0){
			gameObject.GetComponent<ClubLoading>().StopClubLoading();
			yield return new WaitForSeconds(0.5f);
			callback(0);
		}else{
			callback(1);
			AddItem();
			gameObject.GetComponent<ClubLoading>().StopClubLoading();
		}
	}

	void OnRaceClick(){
		GV.gRaceMode = 0;
		CClub.ClubClick = 1;
		GameObject.Find("LobbyUI").SendMessage("OnRaceModeClick",SendMessageOptions.DontRequireReceiver);
	}


	void AddItem(){
		if(ListView == null) {
			ListView = NGUITools.AddChild(gameObject, clubPrefabs);
			view =  ListView.transform.FindChild("View");
			grid = view.FindChild("grid");
			grid.GetComponent<ClanMemberClick>().SetViewInfo(transform.FindChild("BtnMember").transform);
			ListView.transform.localPosition = new Vector3(0.5428175f, 268.3322f,0);
		}
		UIDraggablePanel2 drag = view.GetComponent<UIDraggablePanel2>();
		drag.maxScreenLine = 2;
		drag.maxColLine = -1;
		int count = 0;
		if(grid.childCount == 0 ){
			count = CClub.myClubMemInfo.Count;
			if(count < 2) count = 4;
			drag.Init(count, delegate(UIListItem item, int index) {
			item.Target.GetComponent<UIScrollListBase>().ViewMyClan(index);
		});
			view.FindChild("grid").GetComponent<UIGrid>().Reposition();

	}else{
		
	}


	}

	public void AddViewMyClanMem(){
		AddItem();
	}

	private System.DateTime cTimes;
	private bool bTimeClock = false;
	private System.DateTime remainTime;
	private UILabel lbTime;
	void FixedUpdate () {
		if(!bTimeClock) return;
		System.DateTime nNow = NetworkManager.instance.GetCurrentDeviceTime();
		System.TimeSpan mCompareTime = remainTime-nNow;//System.DateTime.Now;
		lbTime.text = string.Format("[ff3600]{0:00}:{1:00}:{2:00}[-]", mCompareTime.Hours, mCompareTime.Minutes, mCompareTime.Seconds);
		mCompareTime = nNow - cTimes;
		if(mCompareTime.TotalSeconds >= CClub.matchDurationSeconds){
			lbTime.text = string.Format("[ffffff]{0:00}:{1:00}:{2:00}[-]",0,0,0);
			transform.FindChild("BtnClubRace").GetChild(0).FindChild("On").gameObject.SetActive(false);
			bTimeClock = false;
			return;
		}
	}

	public void ChangeStaffSlot(int index){
		int cnt = grid.childCount;
		for(int i = 0; i< cnt;i ++){
			var tr = grid.GetChild(i).gameObject as GameObject;
				if(tr.name ==( "item_"+index.ToString())){
					tr.GetComponent<ViewMyClanItem>().ChangeStaffContents(index);
					break;
				}
			}
	}
	public void ChangeClubInfo(){
	//	if(CClub.ClanMember == 1){
	//		transform.FindChild("Btn_ModifyInfo").gameObject.SetActive(true);
	//	}else{
	//		transform.FindChild("Btn_ModifyInfo").gameObject.SetActive(false);
	//	}
		var tr = transform.FindChild("Info") as Transform;
		tr.FindChild("img_ClanSymbol").GetComponent<UISprite>().spriteName = CClub.mClubInfo.clubSymbol;
		tr.FindChild("lb_ClanName").GetComponent<UILabel>().text = CClub.mClubInfo.mClubName;
		tr.FindChild("lb_Num").GetComponent<UILabel>().text = string.Format("{0}/{1}",CClub.mClubInfo.clubMemberNum, CClub.mClubInfo.clubMemberTotalNum);
		tr.FindChild("lb_Victory").GetComponent<UILabel>().text = CClub.mClubInfo.victoryNum.ToString();
		tr.FindChild("lb_Text").GetComponent<UILabel>().text =  CClub.mClubInfo.clubDescription;
		tr.FindChild("lb_LV").GetComponent<UILabel>().text = "LV "+CClub.mClubInfo.clubLevel.ToString();
		tr.FindChild("lb_Local").GetComponent<UILabel>().text = CClub.mClubInfo.mLocale;
		//==!!Utility.LogWarning("ChangeClubInfo");
		tr = transform.FindChild("Exp_Gage") as Transform;
		int lv = 2899+CClub.mClubInfo.clubLevel;
		int exLv = lv+1;
		if(lv >= 2900 && lv < 2950){
			Common_Exp_Range.ClubExpItem item = Common_Exp_Range.ClubExpGet(exLv);
			int exExp = item.Club_point;
			item = Common_Exp_Range.ClubExpGet(lv);
			int cuExp =item.Club_point;
			int delta =exExp-cuExp;
			float fDelta = (float)CClub.mClubInfo.clubExp;
			if(CClub.mClubInfo.clubExp == 0){
				tr.FindChild("Gage_bar").gameObject.SetActive(false);
				tr.FindChild("lb_Exp").GetComponent<UILabel>().text = string.Format("{0}/{1}", CClub.mClubInfo.clubExp, delta);
			}else{
				float fdelta1 = fDelta/(float)delta;
				tr.FindChild("Gage_bar").localScale = new Vector3(184*fdelta1,20,1);
				tr.FindChild("lb_Exp").GetComponent<UILabel>().text = string.Format("{0}/{1}", CClub.mClubInfo.clubExp, delta);
			}

		}else if(lv >=2950){
			tr.FindChild("lb_Exp").GetComponent<UILabel>().text = string.Format("{0}/{1}", 100, 100);
			tr.FindChild("Gage_bar").localScale = new Vector3(184,20,1);
		}
	
		transform.FindChild("Btn_SignIn").GetComponentInChildren<UILabel>().text =
			KoStorage.GetKorString("72510");

	/*	if(CClub.ClanMember == 1){

		}else{
			transform.FindChild("Btn_SignIn").GetComponentInChildren<UILabel>().text =
				KoStorage.GetKorString("72510");
		}*/
	
	}


	void TimeBtnCheck(){
		var lbs = transform.FindChild("BtnClubRace").FindChild("lb_text") as Transform;
		lbTime = lbs.GetComponent<UILabel>();
		if(CClub.ClubMode == 1){
			if(bTimeClock) return;
			transform.FindChild("BtnClubRace").GetChild(0).FindChild("On").gameObject.SetActive(true);
			transform.FindChild("BtnClubRace").FindChild("lb_text").GetComponent<UILabel>().text =KoStorage.GetKorString("73523");
			lbs.GetComponent<UILabel>().text =  string.Format("[ff3600]{0:00}:{1:00}:{2:00}[-]",0,0,0);
			bTimeClock = true;
			remainTime =System.Convert.ToDateTime(CClub.mClubInfo.matcingTime);
			cTimes = remainTime;
			System.DateTime nNow = NetworkManager.instance.GetCurrentDeviceTime();
			remainTime = remainTime.AddDays(1);
			
		}
		else{
			transform.FindChild("BtnClubRace").GetChild(0).FindChild("On").gameObject.SetActive(false);
			lbs.GetComponent<UILabel>().text =  string.Format("[ffffff]{0:00}:{1:00}:{2:00}[-]",0,0,0);
			bTimeClock = false;
		}
	}

	void OnEnable(){
		TimeBtnCheck();
	}

	public void ChangeUserSlot(int index){
		if(ListView == null) {
			ListView = NGUITools.AddChild(gameObject, clubPrefabs);
			view =  ListView.transform.FindChild("View");
			grid = view.FindChild("grid");
			grid.GetComponent<ClanMemberClick>().SetViewInfo(transform.FindChild("BtnMember").transform);
			ListView.transform.localPosition = new Vector3(0.5428175f, 268.3322f,0);
		}
		int cnt = grid.childCount;
		for(int i = 0; i< cnt;i ++){
			var tr = grid.GetChild(i).gameObject as GameObject;
			if(tr.activeSelf){
				if(tr.name ==( "item_"+index.ToString())){
					tr.GetComponent<ViewMyClanItem>().ChangeContents(index);
					break;
				}
			}
		}
	}


	public void ChangeDeleteClubInfo(){
		var tr = transform.FindChild("Info") as Transform;
		tr.FindChild("lb_Num").GetComponent<UILabel>().text = string.Format("{0}/{1}",CClub.mClubInfo.clubMemberNum, CClub.mClubInfo.clubMemberTotalNum);
		int cnt = grid.childCount;
		for(int i = 0; i< cnt;i ++){
			tr = grid.GetChild(i);
			if(tr.gameObject.activeSelf){
				tr.GetComponent<ViewMyClanItem>().ChangeItemContents();
			}
		}
	}

	
	public void AddViewJoinMyClan(){
		if(ListView == null) {
			ListView = NGUITools.AddChild(gameObject, clubPrefabs);
			view =  ListView.transform.FindChild("View");
			grid = view.FindChild("grid");
			grid.GetComponent<ClanMemberClick>().SetViewInfo(transform.FindChild("BtnMember").transform);
			ListView.transform.localPosition = new Vector3(0.5428175f, 268.3322f,0);
		}

		UIDraggablePanel2 drag = view.GetComponent<UIDraggablePanel2>();
		drag.maxScreenLine = 2;
		drag.maxColLine = -1;
		int count = 0;
		if(grid.childCount == 0 ){
			count = CClub.myClubMemInfo.Count;
			if(count < 2) count = 4;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().ViewMyClan(index);
			});
		}
	//	transform.FindChild("Btn_SignIn").GetComponentInChildren<UILabel>().text 
	//		=KoStorage.GetKorString("77209");
	}

	public void AddViewFisrtMyClan(){
		if(ListView == null) {
			ListView = NGUITools.AddChild(gameObject, clubPrefabs);
			view =  ListView.transform.FindChild("View");
			grid = view.FindChild("grid");
			grid.GetComponent<ClanMemberClick>().SetViewInfo(transform.FindChild("BtnMember").transform);
			ListView.transform.localPosition = new Vector3(0.5428175f, 268.3322f,0);
		}
		UIDraggablePanel2 drag = view.GetComponent<UIDraggablePanel2>();
		drag.maxScreenLine = 2;
		drag.maxColLine = -1;
		int count = 0;
		if(grid.childCount == 0 ){
			count = CClub.myClubMemInfo.Count;
			if(count < 2) count = 4;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().ViewMyClan(index);
			});
		}
	//	transform.FindChild("Btn_SignIn").GetComponentInChildren<UILabel>().text =
	//		KoStorage.GetKorString("77209");
	//	transform.FindChild("Btn_SignIn").GetComponent<UIButtonMessage>().target = gameObject;
	//	transform.FindChild("Btn_SignIn").GetComponent<UIButtonMessage>().functionName = "OnSignOut";
	}

	public void MyClanMemberInit(){
		if(ListView == null) return;
		grid.GetComponent<ClanMemberClick>().unSetViewInfo();
		Destroy(ListView);
		view = null;
		grid = null;
		return;
		if(grid.childCount != 0){
			int cnt = grid.childCount;
			for(int j = cnt; j > 0; j--){
				var temp = grid.transform.GetChild(j-1) as Transform;
				Destroy(temp.gameObject);
			}
			UIGrid dataContainer = grid.GetComponent<UIGrid>();
			dataContainer.transform.DetachChildren();
			UIDraggablePanel2 drag = view.GetComponent<UIDraggablePanel2>();
			drag.reSetGridPannel();
		}
	}
}
