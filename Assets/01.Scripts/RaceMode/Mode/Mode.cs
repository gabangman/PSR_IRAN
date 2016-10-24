using UnityEngine;
using System.Collections;

public class Mode : MonoBehaviour {
	private GameObject subWin;

	public void showSubWindow(string modeName, string detailname){
		subWin  = CreateModeWindow();
		subWin.name = modeName+"_"+detailname;
		////!!--Utility.Log("subWin" + subWin.name);
		var temp = subWin.transform.FindChild(modeName).gameObject as GameObject;
		temp.SetActive(true);
		////!!--Utility.Log(temp.name);
		if(modeName != "Event")
			temp.SendMessage("OnSubWindow");
		else temp.SendMessage("EventRaceStart");
	}

	void ChangeBG(){
	
	}

	public void hiddenSubWindow(){
		if(subWin != null)
			subWin.GetComponent<TweenAction>().ReverseTween(subWin);			
		subWin = null;

	}

	GameObject CreateModeWindow(){
		var temp = ObjectManager.CreateTagPrefabs("RaceWindow") as GameObject;
		if(temp == null){
			temp = ObjectManager.CreateLobbyPrefabs(gameObject.transform.parent, "Window", "ModeWindow_1", "RaceWindow");
		}
		return temp;
	}
}
