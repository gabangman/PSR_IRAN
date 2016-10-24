using UnityEngine;
using System.Collections;

public class GUIPressDelay : MonoBehaviour {



	bool bGearOn;
	bool bAccelOn;
	float gTime;
	System.DateTime pTime;
	System.DateTime rTime;
	System.TimeSpan cTime;
	void Start(){
		bGearOn = false;
		bAccelOn = false;
	}


	void OnCtrlAccelPress(){
		if(!bAccelOn) {
			bAccelOn = true;
			pTime = System.DateTime.Now;
		}
		else return;
	}

	void OnCtrlAccelRelease(){
		if(bAccelOn) bGearOn = true;
	}

	void OnCtrlGearPress(){
		Global.bGearPress = true;
		if(bAccelOn && bGearOn){
			rTime = System.DateTime.Now;
			cTime = rTime - pTime;
			GameManager.instance.PressDelayJudgement(cTime.TotalSeconds);
			gTime = 0;
			bAccelOn = false;
		}
	}

	void OnCtrlGearRelease(){
		Global.bGearPress = false;
		if(!bAccelOn) bGearOn = false;
	}

}
