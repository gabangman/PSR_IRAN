using UnityEngine;
using System.Collections;

public class sponsorBtn : MonoBehaviour {
	public Transform sponIcon;
	void OnEnable(){
	//	int tempid = Base64Manager.instance.GlobalEncoding(Global.MySponsorID);
		int tempid = GV.getTeamSponID(GV.SelectedTeamID);
		if(tempid == 1300 || tempid == 0){
			sponIcon.gameObject.SetActive(false);
		}else{
			sponIcon.gameObject.SetActive(true);
		}
	
	}
}
