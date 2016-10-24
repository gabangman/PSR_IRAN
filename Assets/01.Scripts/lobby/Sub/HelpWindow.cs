using UnityEngine;
using System.Collections;

public class HelpWindow : MonoBehaviour {

	public UILabel[] lbtext;


	void Start(){
		lbtext[0].text = KoStorage.GetKorString("72508");
		lbtext[1].text  = KoStorage.GetKorString("79536");
		lbtext[2].text  = KoStorage.GetKorString("73201"); // regular
		lbtext[3].text  = KoStorage.GetKorString("73202"); // drag
	}

	void OnYouTube(){
		Application.OpenURL(GV.gInfo.bundleURL_1);
	}

	void OnYouTubeDrag(){
		Application.OpenURL(GV.gInfo.bundleURL_2);
	}
	void OnTutorial(){
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<TutorialPopUp>().InitPopUp();
	}

	public void SetInit(){
	
	
	}
}
