using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public partial class LobbyManager : MonoBehaviour {
	private string strMode;
	void OnEventRaceClick(string str){
		if(btnstate != buttonState.WAIT) return;
		btnstate = buttonState.MAP_EVENT;
		fadeIn();
		strMode = str;
		OnBackFunction = ()=>{
			isModeReturn = false;
				fadeIn();
				btnstate = buttonState.MAPTORACEMODE;
			//	if(_mode != null){
			//		_mode.InfoWindowDisable();
			//		_mode.InfoWinDisable();
			//	}
			unSetRaceSubWindow();
		};
	}

	void OnClubRaceClick(string str){
		if(btnstate != buttonState.WAIT) return;
		btnstate = buttonState.MAP_CLAN;
		fadeIn();
		strMode = str;
		OnBackFunction = ()=>{
			isModeReturn = false;
			fadeIn();
			btnstate = buttonState.MAPTORACEMODE;
			//isMap =false;
			unSetRaceSubWindow();
			HiddenInfoTipWindow();
		};
	}
	void setRaceSubWindow(string modeName){
		if(rMode == null)
			rMode = activeObject.AddComponent<Mode>();
		rMode.showSubWindow(modeName, strMode);
	//	ShowInfoTipWindow(modeName);
		HiddenInfoTipWindow();
	}

	void unSetRaceSubWindow(){
		if(rMode != null)
			rMode.hiddenSubWindow();
		GameObject.Find("Audio").SendMessage("ChangeBGMMusic", true, SendMessageOptions.DontRequireReceiver);
		//HiddenInfoTipWindow();

	}
}
