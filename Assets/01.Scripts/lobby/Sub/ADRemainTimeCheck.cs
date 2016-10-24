using UnityEngine;
using System.Collections;

public class ADRemainTimeCheck : MonoBehaviour {

	public GameObject OnAirIcon;
	public UISprite spRing;
	private System.DateTime cTime, hTime;
	private System.TimeSpan sTime;
	private bool bCheckTime;
	// Use this for initialization
	void Start () {
		ADBoxIconCheck();
	}
	System.TimeSpan mCompareTime;
	bool bNewIcon = true;
	// Update is called once per frame
	void FixedUpdate () {
		if(bCheckTime) return;
		System.DateTime nNow = NetworkManager.instance.GetCurrentDeviceTime();
		if(GV.RewardViewTime[0] !=0 ){
			mCompareTime = new System.DateTime(GV.RewardViewTime[0]) - nNow;
			if(mCompareTime.Seconds < 0){
				GV.RewardViewTime[0] = 0;
				ADBoxIconCheck();
			}else{
			
			}
		}else{
		}
	
	}

	void OnEnable(){
		ADBoxIconCheck();
	}

	void OnDisable(){
	}

	void ADBoxIconCheck(){
		if(	GV.RewardViewTime[0] <= 0){
			if(GV.CurrADId > GV.MaxADId){
				OnAirIcon.SetActive(false);
				bCheckTime = false;
			}else{
				OnAirIcon.SetActive(true);
				bCheckTime = true;
			}
		}else{
			OnAirIcon.SetActive(false);
			bCheckTime = false;
		}


	}




}
