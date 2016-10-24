using UnityEngine;
using System.Collections;

public class Club_Match_Race : MonoBehaviour {

	public GameObject grid;
	public GameObject slotPrefabs;
	void initialContentWindow(){
		//==!!Utility.LogWarning("ClubRaceStart");
		addMyTeamItem();
		
	}
	
	
	public void addMyTeamItem(){
		int cnt = grid.transform.childCount;
		int maxMem =GV.listmyteaminfo.Count;//  Common_Team.StockTeamList.Count+Common_Team.TourTeamList.Count;
		if(cnt == 0){
			for(int i =0; i < maxMem; i++){
				int num = GV.listmyteaminfo[i].TeamCode;
				var temp = NGUITools.AddChild(grid, slotPrefabs);
				temp.name = "Team_"+num.ToString();
				temp.AddComponent<ViewMyTeamItem>().ViewTeamContentInClub(num);
			}
			
			grid.GetComponent<UIGrid>().Reposition();
		}else{
			//UIScrollBar uiBar = listMy.transform.GetChild(1).GetComponent<UIScrollBar>();
			//uiBar.scrollValue = 0.0f;
			
		}
	}
}
