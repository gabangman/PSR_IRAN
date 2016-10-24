using UnityEngine;
using System.Collections;

public class popUpmenu : MonoBehaviour {

void OnDisable(){
		transform.name  = "popUp";
		var pop = transform.FindChild ("Content_BUY") as Transform;
		pop.FindChild("btnCoin").gameObject.SetActive(false);
		pop.FindChild("btnCash").gameObject.SetActive(false);
		pop.FindChild("btnok").gameObject.SetActive(false);
		pop.FindChild("btnDollar").gameObject.SetActive(false);
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
		pop.FindChild("lbOk").gameObject.SetActive(false);
		#if UNITY_IOS || UNITY_ANDROID
		//	if(bStart) UnityAdsHelper.HiddenAdMobBot();
		#endif
	}

	void OnEnable(){
		#if UNITY_IOS || UNITY_ANDROID
			//if(bStart) UnityAdsHelper.ShowAdMobBot();
		#endif
	}

//	private bool bStart =false;
	void Awake(){
		//bStart = false;
	}
	void Start(){
		#if UNITY_IOS || UNITY_ANDROID
			// UnityAdsHelper.ShowAdMobBot();
		#endif
	//	bStart = true;
	}

	void OnDestroy(){
		#if UNITY_IOS || UNITY_ANDROID
//		if(bStart) UnityAdsHelper.DestoryAdMobBot();
		#endif
	}

}
