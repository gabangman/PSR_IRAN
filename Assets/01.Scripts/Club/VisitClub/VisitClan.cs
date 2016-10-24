using UnityEngine;
using System.Collections;

public class VisitClan : MonoBehaviour {
	public Transform grid;
	public Transform view;
//	public Transform btnMember;
	public GameObject listViewPrefabs;
	private GameObject vistListView;
	void Awake(){
		transform.FindChild("Btn_SignIn").gameObject.SetActive(false);
	}
	void Start(){
		transform.FindChild("Btn_SignIn").FindChild("lbText").GetComponent<UILabel>().text
			= KoStorage.GetKorString("77206");
	}

	public void ResetVisitView(bool b){
		//vistListView.SetActive(b);
	}

	private int mClubIdx;
	public void SetClanInfo(string clanName, int clubIdx){
		if(Global.isNetwork == true) return;
		if(vistListView != null){
			CClub.mClubVisit = null;
			CClub.VisitClubMemList.Clear();
			Destroy(vistListView);
			vistListView = null;
			mClubIdx = 0;
		}
		gameObject.AddComponent<ClubLoading>().ReadyClubLoading(1);
		transform.FindChild("Info").gameObject.SetActive(false);
		StartCoroutine("GetVisitClubInfo", clubIdx);
		Global.isNetwork = true;
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			NGUITools.FindInParents<ClanPopup>(gameObject).OnClose();
		};
	
	}

	IEnumerator GetVisitClubInfo(int clubIndx){

		bool bConnect = false;
		int idx = 0;
		string mAPI = "club/visitClub";//destroyClub
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();

		mDic.Add("clubIndex",clubIndx.ToString() );
		
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
		//	respons : {"state":0,"clubDescription":"ddaabb","mClubName":"test1","clubSymbol":"Clubsymbol_5","victoryNum":0,"playNum":0,"clubMemberNum":1,"clubLevel":1}
				CClub.mClubVisit = new ClubInfoVisit(thing);
				CClub.mClubVisit.clubIndex = clubIndx;
			}else{
				idx = 1;
				
			}
			

			bConnect = true;
		});
		
		
		while(!bConnect){
			yield return null;
		}
		bConnect = false;
		mAPI ="club/getClubMemberInfo";
		mDic.Clear();
		mDic.Add("clubIndex",clubIndx.ToString() );
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				int cnt = thing["memberList"].Count;
				ClubMemberInfo cMemInfo;
				for(int i =0; i < cnt; i++){
					cMemInfo = new ClubMemberInfo(thing["memberList"][i]);
					CClub.VisitClubMemList.Add(cMemInfo);
				}

				if(CClub.VisitClubMemList.Count >=2){
					CClub.VisitClubMemList.Sort(delegate(ClubMemberInfo x, ClubMemberInfo y) {
						int a = y.clubStarCount.CompareTo(x.clubStarCount);
						if(a == 0)
							a =  y.playNumber.CompareTo(x.playNumber);
						if(a == 0) 	a =  y.mLv.CompareTo(x.mLv);
						return a;
					});
				}
			}else{
	
			}
			Global.isNetwork = false;
			bConnect = true;
		});
		
		
		while(!bConnect){
			yield return null;
		}

	//	vistListView = NGUITools.AddChild(gameObject, listViewPrefabs);
	//	view =  vistListView.transform.FindChild("View");
	//	grid = view.FindChild("grid");
	//	transform.FindChild("Btn_SignIn").gameObject.SetActive(true);
	//	AddItem();

		if(CClub.ClanMember == 0) {
			if(CClub.mClubVisit.clubMemberNum >= CClub.MaxMember) transform.FindChild("Btn_SignIn").gameObject.SetActive(false);
			else transform.FindChild("Btn_SignIn").gameObject.SetActive(true);
		
		}else transform.FindChild("Btn_SignIn").gameObject.SetActive(false);

		vistListView = NGUITools.AddChild(gameObject, listViewPrefabs);
		view =  vistListView.transform.FindChild("View");
		grid = view.FindChild("grid");
		AddItem();
		ChangeClubInfo();
		gameObject.GetComponent<ClubLoading>().StopClubLoading();
	}

	void ChangeClubInfo(){
			var tr = transform.FindChild("Info") as Transform;
		tr.FindChild("img_ClanSymbol").GetComponent<UISprite>().spriteName = CClub.mClubVisit.clubSymbol;
		tr.FindChild("lb_ClanName").GetComponent<UILabel>().text = CClub.mClubVisit.clubName;
		tr.FindChild("lb_Num").GetComponent<UILabel>().text = string.Format("{0}/{1}",CClub.mClubVisit.clubMemberNum, CClub.MaxMember);
		tr.FindChild("lb_Victory").GetComponent<UILabel>().text = CClub.mClubVisit.victoryNum.ToString();
		tr.FindChild("lb_Text").GetComponent<UILabel>().text =  CClub.mClubVisit.clubDescription;
		tr.FindChild("lb_LV").GetComponent<UILabel>().text = "LV "+CClub.mClubVisit.clubLevel.ToString();
		tr.FindChild("lb_local").GetComponent<UILabel>().text = CClub.mClubVisit.mLocale;
		tr.gameObject.SetActive(true);
	}
	void AddItem(){
		UIDraggablePanel2 drag = view.GetComponent<UIDraggablePanel2>();
		drag.maxScreenLine = 2;
		drag.maxColLine = 0;
		int count = CClub.VisitClubMemList.Count;
		if(grid.childCount == 0 ){
			if(count<2) count =4;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().ViewVisitClanMem(index);
			});
		}
	}


	public void OnVisitSignIn(){
		NGUITools.FindInParents<ClanWindow>(gameObject).OnVisitSignIn(CClub.mClubVisit.clubIndex, CClub.mClubVisit.clubName);
	}


	void OnDisable(){
		//Destroy(vistListView);
	}
}
