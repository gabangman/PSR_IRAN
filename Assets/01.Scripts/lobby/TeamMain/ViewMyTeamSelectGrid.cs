using UnityEngine;
using System.Collections;

public class ViewMyTeamSelectGrid : MonoBehaviour {

	public void SetSelectedObj(string name){
		int cnt = transform.childCount;
		for(int i=0; i < cnt ; i++){
			var temp = transform.GetChild(i) as Transform;
			if(temp.name == name)
			temp.GetChild(0).FindChild("SetEntry").gameObject.SetActive(true);
			else temp.GetChild(0).FindChild("SetEntry").gameObject.SetActive(false);
		}

	}

	public void unSetSelectedObj(string name){
		int cnt = transform.childCount;
		for(int i=0; i < cnt ; i++){
			var temp = transform.GetChild(i) as Transform;
			//temp.GetChild(0).FindChild("Selected").gameObject.SetActive(false);
			temp.GetChild(0).FindChild("SelectInfo").gameObject.SetActive(false);
		}
	}

	public void unSetObj(){
		int cnt = transform.childCount;
		for(int i=0; i < cnt ; i++){
			var temp = transform.GetChild(i) as Transform;
			if(temp.gameObject.activeSelf){
				temp.GetChild(0).FindChild("SetEntry").gameObject.SetActive(false);
				temp.GetChild(0).FindChild("SelectInfo").gameObject.SetActive(false);
			}
		}
	}

/*	public string getSelectTeam(){
		int cnt = transform.childCount;
		string str =string.Empty;
		for(int i=0; i < cnt ; i++){
			var temp = transform.GetChild(i).GetChild(0).FindChild("Selected") as Transform;
			//Utility.Log(transform.GetChild(i).name);
			if(temp.gameObject.activeSelf){
				str = transform.GetChild(i).name;
				break;
			}
		}
		Utility.LogWarning(str);
		return str;
	}*/
}
