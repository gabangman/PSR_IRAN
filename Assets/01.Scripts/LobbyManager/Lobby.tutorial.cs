using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public partial class LobbyManager : MonoBehaviour {

	public GameObject TutorialObj;

	void StartTutorial(){
		GameObject.Find("Audio").GetComponent<LobbySound>().CheckBGMVolume(true);
		TutorialObj.SetActive(true);
	}

	void SecondTutorial(){
		if(Global.gChampTutorial == 1){
			Global.gChampTutorial = -1;
			Global.isReTutorial = true;
			TutorialObj.SetActive(true);
			GameObject.Find("Audio").GetComponent<LobbySound>().CheckBGMVolume(true);
		}else{
			isShowWin = true;
			Global.gChampTutorial = 0;
		}
	}

	void EndTutorial(){
		TutorialObj.SetActive(false);
		isShowWin = true;
		GameObject.Find("Audio").GetComponent<LobbySound>().CheckBGMVolume(false);
	}





}
