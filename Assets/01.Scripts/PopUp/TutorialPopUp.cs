using UnityEngine;
using System.Collections;

public class TutorialPopUp : basePopup {

	void Start(){

		
	}
	public void InitPopUp(){
		ChangeContentOKayString(KoStorage.GetKorString("72508"), KoStorage.GetKorString("71007"), KoStorage.GetKorString("79500"));
		isCallbacksub = true;
		UserDataManager.instance.OnSubBacksub = ()=>{
			OnCloseClick();
		};
	}
	public override void OnOkClick(){
		
		Global.gTutorial = 1;
		Global.gChampTutorial = 0;
		Application.LoadLevel("Title");
	}
}
