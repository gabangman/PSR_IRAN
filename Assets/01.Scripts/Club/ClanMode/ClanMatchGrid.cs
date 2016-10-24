using UnityEngine;
using System.Collections;

public class ClanMatchGrid : MonoBehaviour {
	public GameObject view, grid;
 	/*public void setSelectLines(string str){
		int cnt = grid.transform.childCount;

		for(int  i = 0; i < cnt; i++){
			var temp = grid.transform.GetChild(i) as Transform;
			if(temp.gameObject.activeSelf){
				if(str.Equals(temp.name)){
					temp.GetComponent<ClanWarItem>().setSelectLine();
					NGUITools.FindInParents<ClanMatchStart>(gameObject).setHistory(temp.name);
				}else{
					temp.GetComponent<ClanWarItem>().unsetSelectLine();
				}
			}
		}
	}

	public void unsetSelectLines(string str){
		int cnt = grid.transform.childCount;
		for(int  i = 0; i < cnt; i++){
			var temp = grid.transform.GetChild(i) as Transform;
			if(temp.gameObject.activeSelf){
				temp.GetComponent<ClanWarItem>().unsetSelectLine();
			}
		}
	}*/


	public void UnSelectedInfo(){
		int cnt = grid.transform.childCount;
		for(int  i = 0; i < cnt; i++){
			var temp = grid.transform.GetChild(i) as Transform;
			if(temp.gameObject.activeSelf){
				temp.GetComponent<ViewMyTeamItem>().unSetSelectTeam();
			}
		}
	}



}
