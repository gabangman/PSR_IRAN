using UnityEngine;
using System.Collections;

public class ViewClanSearch : CommonClub {

	public Transform grid;
	public Transform view;
	public GameObject listViewPrefabs;
	private GameObject sListView;
	public UILabel sWord;
	public UILabel[] labels;
	void OnEnable(){
		if(sListView != null){
			CClub.mSeachList.Clear();
			Destroy(sListView);
		}

		//sListView = NGUITools.AddChild(gameObject, listViewPrefabs);
		//view =  sListView.transform.FindChild("View");
		//grid = view.FindChild("grid");
		//grid.GetComponent<ClanMemberClick>().viewInfo = btnMember;
	}
	void OnHomeClick(){
		Application.OpenURL(GV.gInfo.HomeURL);
	}
	void Start(){
		sWord.text = KoStorage.GetKorString("77500");
		labels[0].text =KoStorage.GetKorString("77504");
		labels[1].text = string.Empty;
		labels[1].transform.gameObject.SetActive(false);
		transform.FindChild("Btn_Community").FindChild("lbtitle").GetComponent<UILabel>().text = KoStorage.GetKorString("72507");
	}
	
	void OnDisable(){
		//Destroy(vistListView);
	}

	void AddItem(){
		UIDraggablePanel2 drag = view.GetComponent<UIDraggablePanel2>();
		drag.maxScreenLine = 2;
		drag.maxColLine = 0;
		int count = CClub.mSeachList.Count;
		if(grid.childCount == 0 ){
			//count = gameRank.instance.listworld.Count;
			//if(count <= 2) count = 4;
			if(count < 2) count = 4;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().ViewSearch(index);
			});
		}
	}
	
	public void AddViewClanSearch(){
		AddItem();
	}

	void OnSearch(){
		if(Global.isNetwork)return;
		if(sListView != null){
			CClub.mSeachList.Clear();
			Destroy(sListView);
		}
		if(string.IsNullOrEmpty(sWord.text) || string.Equals(sWord.text,KoStorage.GetKorString("77500"))==true){

			//문자 넣으시오.
			NGUITools.FindInParents<ClanWindow>(gameObject).searchError();
			return;
		}
//		Utility.Log(sWord.text);

		Global.isNetwork = true;
		StartCoroutine("SearchClan", sWord.text);


	
	}



	IEnumerator SearchClan(string str){
		bool bConnect = false;
		string mAPI = "club/searchClub";//
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		mDic.Add("searchWord",str);
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				//CClub.mSeachList = new System.Collections.Generic.List<ClubInfoSearch>();
				int cnt = thing["searchList"].Count;
				for(int i =0; i < cnt; i++){
					ClubInfoSearch mClub = new ClubInfoSearch( thing["searchList"][i]);
					CClub.mSeachList.Add(mClub);
				}

				sListView = NGUITools.AddChild(gameObject, listViewPrefabs);
				view =  sListView.transform.FindChild("View");
				grid = view.FindChild("grid");
				AddItem();
			}else if(status == -411){
				NGUITools.FindInParents<ClanWindow>(gameObject).searchError(1);
			//	0 : success
			//		-411 : 검색어로 검색 할 수 없을 경우
			//			-404 : 검색어가 짧을 경우(2자리 미만)
			}else if(status == -404){
				NGUITools.FindInParents<ClanWindow>(gameObject).searchError(2);
			//respons : {"state":0,"searchList":[{"mClubName":"test1","clubSymbol":"Clubsymbol_5","clubDescription":"ddaabb","clubLevel":1,"clubVictoryNum":0,"clubIndex":66,"clubMember":1,"createTime":"2016-04-27 15:01:51"}]}
			}
			Global.isNetwork = false;
			bConnect = true;
		});
		
		
		while(!bConnect){
			yield return null;
		}

	}
	void OnSubmit(){
	
	}
}
