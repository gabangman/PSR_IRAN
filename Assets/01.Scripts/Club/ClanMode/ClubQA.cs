using UnityEngine;
using System.Collections;

public class ClubQA : MonoBehaviour {

	public GameObject QnA;
	public UILabel[] lbLabels;
	void Awake(){
		
	}

	void Start(){

		lbLabels[0].text = KoStorage.GetKorString("73521");

		lbLabels[1].text = KoStorage.GetKorString("73503");
		lbLabels[2].text = KoStorage.GetKorString("73512");

		lbLabels[3].text = KoStorage.GetKorString("73504");
		lbLabels[4].text = KoStorage.GetKorString("73513");

		lbLabels[5].text = KoStorage.GetKorString("73505");
		lbLabels[6].text = KoStorage.GetKorString("73514");

	}


	void OnQNA(){
		if(Global.isPopUp) return;
		Global.isPopUp = true;
		QnA.SetActive(true);
		QnA.GetComponent<TweenAction>().doubleTweenScale(QnA);
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnClose();
		};
	}

	void OnClose(){
		QnA.SetActive(false);
		Global.isPopUp = false;
		Global.bLobbyBack = false;
		UserDataManager.instance.OnSubBack = null;
	}

}
