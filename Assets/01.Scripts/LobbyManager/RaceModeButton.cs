using UnityEngine;
using System.Collections;

public class RaceModeButton : MonoBehaviour {

	public void setActivateButton(string str){





		int cnt = transform.childCount;
		for(int i = 0; i < cnt; i++){
			var temp = transform.GetChild(i).gameObject as GameObject;
		//	if(temp.name.Equals(str)){
		//		temp.SetActive(true);
		//	}else{
				temp.SetActive(false);
		//	}
		}
	}

	public void setBGLine(bool b){
		transform.GetChild(0).gameObject.SetActive(b);
	}
}
