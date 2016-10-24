using UnityEngine;
using System.Collections;

public class modeSubMenu : MonoBehaviour {

	void ChangeSubModeScene(string mode){
		int cnt = transform.childCount;
		for(int i = 0; i < cnt; i++){
			var tr = transform.GetChild(i) as Transform;
			if(tr.name.Equals(mode)){
				tr.gameObject.SetActive(true);
				tr.GetComponent<TweenPosition>().Reset();
				tr.GetComponent<TweenPosition>().enabled = true;
				tr.gameObject.SendMessage("SetRaceSubMode");
				GameObject.Find("LobbyUI").SendMessage("ShowInfoTipWindow", mode);
			}else{
				tr.gameObject.SetActive(false);
			}
		}
	}
}
