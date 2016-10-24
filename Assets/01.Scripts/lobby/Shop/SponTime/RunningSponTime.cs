using UnityEngine;
using System.Collections;

public class RunningSponTime : MonoBehaviour {

	public UILabel sponTime;

	void Start(){
		//sponTime.text = AccountManager.instance.lbSponTime;
	}

	void FixedUpdate(){
		sponTime.text = AccountManager.instance.RunningSponTime();
	}


 	public void SponTimeLable(UILabel uiLable){
		sponTime = uiLable;
	}
}
