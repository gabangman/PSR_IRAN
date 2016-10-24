using UnityEngine;
using System.Collections;

public class ClanMode : MonoBehaviour {

	void OnEnable(){
	
	}

	void OnDisable(){
	
	}
	public  void initInfoWin(GameObject obj){
		int idx = 0;
		//var temp = transform.FindChild("Clan") as Transform;
		GameObject tempobj = null;
		switch(idx){
		case 0:
			tempobj= obj.transform.FindChild("Match_No").gameObject;
			obj.transform.FindChild("Match_Yes").gameObject.SetActive(false);
			obj.transform.FindChild("Match_Start").gameObject.SetActive(false);
			ClanMatchMode(tempobj);
			break;
		case 1:
			obj.transform.FindChild("Match_No").gameObject.SetActive(false);
			tempobj =obj.transform.FindChild("Match_Yes").gameObject;
			obj.transform.FindChild("Match_Start").gameObject.SetActive(false);
			ClanReadyMode(tempobj);
			break;
		case 2:
			obj.transform.FindChild("Match_No").gameObject.SetActive(false);
			obj.transform.FindChild("Match_Yes").gameObject.SetActive(false);
			tempobj = obj.transform.FindChild("Match_Start").gameObject;
			ClanStartMode(tempobj);
			break;
		}
	}


	void ClanMatchMode(GameObject obj){
		obj.SetActive(true);
		obj.SendMessage("initialContentWindow");
	}

	void ClanStartMode(GameObject obj){
		obj.SetActive(true);
		obj.SendMessage("initialContentWindow");
	}

	void ClanReadyMode(GameObject obj){
		obj.SetActive(true);
		obj.SendMessage("initialContentWindow");
	}



}
