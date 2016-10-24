using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using TapjoyUnity;

public class TapjoyPlacement : MonoBehaviour {
	public TJPlacement offerwallPlacement;
	

	void Awake(){
	
	}
	void Start () {

		if(!Tapjoy.IsConnected){
			#if UNITY_ANDROID
			Tapjoy.Connect("wFQYLSeUS0GPsJKRrCCkQwECMC91xskPriKHLtLpm29c3pnnAGST1nLg0Utp");
			#elif UNITY_IOS
			Tapjoy.Connect("d8pldl5iTJaiPHU_x65eRQEBMdlQimhv0ZmLSKh4dpkJEG-XJvQ5itqAiUIY");
			#endif
		}else{
		}
		
		
		if (offerwallPlacement == null) {
			offerwallPlacement = TJPlacement.CreatePlacement("offerwall_unit");
		}
		
		
		
	}
	
	void OnEnable(){
		
		Tapjoy.OnConnectSuccess += HandleConnectSuccess;
		Tapjoy.OnConnectFailure += HandleConnectFailure;

		TJPlacement.OnRequestSuccess += HandlePlacementRequestSuccess;
		TJPlacement.OnRequestFailure += HandlePlacementRequestFailure;
		TJPlacement.OnRequestSuccess += HandlePlacementRequestSuccess;
		TJPlacement.OnRequestFailure += HandlePlacementRequestFailure;
		TJPlacement.OnContentReady += HandlePlacementContentReady;
		TJPlacement.OnContentShow += HandlePlacementContentShow;
		TJPlacement.OnContentDismiss += HandlePlacementContentDismiss;

	}
	
	void OnDisable(){
		
		Tapjoy.OnConnectSuccess -= HandleConnectSuccess;
		Tapjoy.OnConnectFailure -= HandleConnectFailure;
		TJPlacement.OnRequestSuccess -= HandlePlacementRequestSuccess;
		TJPlacement.OnRequestFailure -= HandlePlacementRequestFailure;
		TJPlacement.OnContentReady -= HandlePlacementContentReady;
		TJPlacement.OnContentShow -= HandlePlacementContentShow;
		TJPlacement.OnContentDismiss -= HandlePlacementContentDismiss;
		
	}
	
	
	public void HandlePlacementRequestSuccess(TJPlacement placement) {
		if (placement.IsContentAvailable()) {
			if(placement.GetName() == "offerwall_unit") {
				placement.ShowContent();
			}
		} else {
			Utility.Log("C#: No content available for " + placement.GetName());
		}
	}
	
	
	public void HandlePlacementRequestFailure(TJPlacement placement, string error) {
		Utility.Log("C#: HandlePlacementRequestFailure");
		Utility.Log("C#: Request for " + placement.GetName() + " has failed because: " + error);
	}
	
	#region Connect Delegate Handlers
	public void HandleConnectSuccess() {
	}
	
	public void HandleConnectFailure() {
		Utility.Log("C#:TapJoyPlacement  Handle Connect Failure");
	}
	#endregion
	
	void OnADClick(){
		if(Global.isPopUp) return;
		if (offerwallPlacement != null) {
			UserDataManager.instance.TapJoyLoadingBar();
			offerwallPlacement.RequestContent();
			UserDataManager.instance.isTapjoy = true;
			UserDataManager.instance.isPause = true;
		}else{
			
		}
	}
	
	
	public void HandlePlacementContentReady(TJPlacement placement) {
		//This gets called when content is ready to show.
		//==!!Utility.LogWarning("Wait HandlePlacementContentReady");
	}
	
	
	public void HandlePlacementContentShow(TJPlacement placement) {
		//==!!Utility.LogWarning("Wait HandlePlacementContentShow");
	}
	
	public void HandlePlacementContentDismiss(TJPlacement placement) {
		//==!!Utility.LogWarning("Wait HandlePlacementContentDismiss");
	}
	
	
	
}
