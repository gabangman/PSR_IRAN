using UnityEngine;
using System.Collections;

public class ViewClanHistory : CommonClub {

	public Transform grid;
	public Transform view;
	public UILabel lbText;

	void Start(){
		CClub.mHistoryList.Clear();
	
		if(CClub.ClanMember == 0){
			lbText.text =KoStorage.GetKorString("77410");
		}else{
			lbText.text = string.Empty;
		}
	}
	void AddItem(){
		UIDraggablePanel2 drag = view.GetComponent<UIDraggablePanel2>();
		drag.maxScreenLine = 2;
		drag.maxColLine = 0;
		int count = CClub.mHistoryList.Count;
		if(grid.childCount == 0 ){
			if(count <=2 ) count = 4;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().ViewHistory(index);
			});
		}
	}
	
	public void AddViewClanHistory(){
		//return;
		if(Global.isNetwork) return;
		if(CClub.ClanMember == 0){
			lbText.text =KoStorage.GetKorString("77410");
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
		}else{
			Global.isNetwork = true;
			StartCoroutine("getClubHistory");
		}
	}


	IEnumerator getClubHistory(){
		
		bool bConnect = false;
		string mAPI = "club/getHistoryClubResult";
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		NetworkManager.instance.ClubBaseStringConnect("Get",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
		
				ClubInfoHistory mHis;
				int cnt = thing["historyList"].Count;
				for(int  i = 0; i < cnt; i++){
					mHis = new ClubInfoHistory(thing["historyList"][i]);
					CClub.mHistoryList.Add(mHis);
				}

			//	lbText.text = string.Format("win {0} / defeat {1}", thing["winCount"].AsInt, thing["loseCount"].AsInt);
			}else if(status == -1){
			//	//==!!Utility.LogWarning("No History");
			//	lbText.text = "기록이 없다?";
				lbText.text =KoStorage.GetKorString("77410");

			}else{
			
			
			
			}
			
			Global.isNetwork = false;
			bConnect = true;

		});
		
		while(!bConnect){
			yield return null;
		}
		AddItem();
	}


}
