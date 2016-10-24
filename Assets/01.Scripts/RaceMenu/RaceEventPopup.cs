using UnityEngine;
using System.Collections;

public class RaceEventPopup : MonoBehaviour {
	public GameObject[] mEvent;
	public GameObject mFinish, mDisable;
	public GameObject ClubWin;
	public RaceMenuStart rStart;
	public void SetEventMode(int idx){
	
		for(int i = 0; i < 3; i++){
			if( i == idx) {
				mEvent[i].SetActive(true);
				mEvent[i].GetComponent<EventItems>().InitEventWindow(idx);
			}
			else mEvent[i].SetActive(false);
		}

	
		if(	UserDataManager.instance.OnSubBackMenu != null) UserDataManager.instance.OnSubBackMenu = null;
		UserDataManager.instance.OnSubBackMenu = ()=>{
			rStart.returnMenu();
		};

	}

	public void InitEventObject(){
		for(int i = 0; i < 3; i++){
			mEvent[i].SetActive(false);
		}
		//Utility.LogWarning("InitEventObject");
	}

	public void SetEventPopUpMode(int idx, int mode){
		/*
		UserDataManager.instance._subStatus = ()=>{
			mFinish.SetActive(false);
			mDisable.SetActive(false);
			rStart.returnMenuPopup();
			return true;
		};*/

		for(int i = 0; i < 3; i++){
			mEvent[i].SetActive(false);
		}
		//Utility.LogWarning("SetEventPopUpMode");
		if(	UserDataManager.instance.OnSubBackMenu != null) UserDataManager.instance.OnSubBackMenu = null;
		UserDataManager.instance.OnSubBackMenu = ()=>{
			mFinish.SetActive(false);
			mDisable.SetActive(false);
			rStart.returnMenuPopup();
		};
		rStart.reverseEventPopup();
		if(mode == 1) FinishPopup(KoStorage.GetKorString("73323"));

		else {
			if(idx == 0){
				NonPlayPopup(KoStorage.GetKorString("73303"));
			}else if(idx == 1){
				NonPlayPopup(KoStorage.GetKorString("73313"));
			}else if(idx == 2){
				MoveToDealerPage(KoStorage.GetKorString("73306"));
			}
		}
		Global.isNetwork = false;
	}

	void SettingEventInfo(GameObject obj){
	
	}

	void OpenClubPopup(){
	
	}

	protected void ReadyLoadingCircle(){
		Global.isNetwork = true;
		var loading = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
		loading.StartCoroutine("StartRaceLoading", false);//raceStartLoading
	}


	int mTrack, mCar;
	void OnEvent1(){
		if(Global.isNetwork) return;
			Global.isNetwork = true;
		if(!CheckedNewCarEvent()) {
			return;
		}

		if(UserDataManager.instance.raceFuelCountCheck()) return;
		UserDataManager.instance.raceFuelCounting();
		GV.gEntryFee = 0;
		ReadyLoadingCircle();
		gameObject.AddComponent<SettingRaceEvent>().setEventRace("0",
		                                                         1412, mCar);
		UserDataManager.instance.OnSubBackMenu = null;
	}

	void OnEvent2(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		if(!CheckedReqCarEvent()){
			//Utility.LogWarning("OnEvent2");
			return;
		}
		if(UserDataManager.instance.raceFuelCountCheck()) return;
		UserDataManager.instance.raceFuelCounting();
		GV.gEntryFee = 0;
		ReadyLoadingCircle();
		gameObject.AddComponent<SettingRaceEvent>().setEventRace("1",mTrack, mCar);
		//Utility.LogWarning("OnEvent2 " + mCar + "  " + mTrack);
		UserDataManager.instance.OnSubBackMenu = null;
	}

	void OnEvent3(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		if(!CheckedMaterialEvent()) {
			return;
		}
		if(UserDataManager.instance.raceFuelCountCheck()) return;
		GV.gEntryFee = 0;
		ReadyLoadingCircle();
		gameObject.AddComponent<SettingRaceEvent>().setEventRace("2",1400, mCar);
		UserDataManager.instance.OnSubBackMenu = null;
	}

	bool CheckedNewCarEvent(){
		//트랙지정 , 내가 없는 차량 지정, 할것!! 플레이 수 체크 할것.
	//	mTrack = 1412;//GV.eTrack[0];
	//	mCar = myAccount.instance.account.eRace.testDriveCarID;
	/*	if(myAccount.instance.account.eRace.testDrivePlayCount == 5){
			//NonPlayPopup("play count is 5");
			FinishPopup(KoStorage.GetKorString("73323"));
			
			return false;
		}
		CarInfo mcar = GV.mineCarList.Find(obj => obj.CarID == mCar);
		if(mcar != null) {
			//NonPlayPopup("너 새차 가지고 있어");
			NonPlayPopup(KoStorage.GetKorString("73303"));
			return false;
		}*/
		// PlayPopup("새차 몰아보자");
		//PlayPopup(KoStorage.GetKorString("73315"));
	//	return true;
		mTrack = 1412;//GV.eTrack[0];
		mCar = myAccount.instance.account.eRace.testDriveCarID;
		if(myAccount.instance.account.eRace.testDrivePlayCount == 5){
			SetEventPopUpMode(0,1);
			return false;
		}
		CarInfo mcar = GV.mineCarList.Find(obj => obj.CarID == mCar);
		if(mcar != null) {
			SetEventPopUpMode(0,0);
			return false;
		}
		return true;
	}
	
	bool CheckedReqCarEvent(){

		mTrack = GV.EventTrack;
		mCar =myAccount.instance.account.eRace.featuredCarID;
		if(myAccount.instance.account.eRace.featuredPlayCount  == 5){
			SetEventPopUpMode(2,1);
			return false;
		}
		CarInfo mcar = GV.mineCarList.Find(obj => obj.CarID == mCar);
		if(mcar == null) {
			SetEventPopUpMode(2,0);
			return false;
		}
		return true;
	}


	bool CheckedMaterialEvent(){
		mTrack =  1400;
		mCar = GV.getTeamCarID(GV.SelectedTeamID);
		if(myAccount.instance.account.eRace.EvoPlayCount == 0){
			SetEventPopUpMode(1,0);
			return false;
		}
		if(myAccount.instance.account.eRace.EvoAcquistCount == 1){
			SetEventPopUpMode(1,1);
			return false;
		}
		return true;
	}

	void FinishPopup(string str){
		bModeCheck = true;
		mFinish.SetActive(true);
		mDisable.SetActive(false);
		mFinish.transform.FindChild("Text1").GetComponent<UILabel>().text = str;
	}
	void NonPlayPopup(string str){
		bModeCheck = true;
		mDisable.SetActive(true);
		mFinish.SetActive(false);
		mDisable.transform.FindChild("Text1").GetComponent<UILabel>().text = str;
		mDisable.transform.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
	
	}
	
	void MoveToDealerPage(string str){
		bModeCheck = true;
		mDisable.SetActive(true);
		mFinish.SetActive(false);
		try{
			mDisable.transform.FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73304");
		}catch(System.Exception e){
			mDisable.transform.FindChild("Text1").GetComponent<UILabel>().text = str;
		}

		mDisable.transform.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		mDisable.transform.FindChild("Sprite (Check_V)").GetComponent<UIButtonMessage>().functionName = "OnClosed";

	}
	private bool bModeCheck;
	void OnClosed(){
		if(UserDataManager.instance.OnSubBackMenu != null){
			UserDataManager.instance.OnSubBackMenu();
			UserDataManager.instance.OnSubBackMenu = null;
		}
	}

	void OnClose(){
			if(bModeCheck){
				if(UserDataManager.instance.OnSubBackMenu != null){
					UserDataManager.instance.OnSubBackMenu();
					UserDataManager.instance.OnSubBackMenu = null;
				}
			}else{
				UserDataManager.instance.OnSubBackMenu = null;
				rStart.returnToDealer();
		}
	}


	public void ClubModeInitialize(){
		ClubWin.SendMessage("ClubModeInitialize", SendMessageOptions.DontRequireReceiver);
		UserDataManager.instance.OnSubBackMenu = ()=>{
			NGUITools.FindInParents<RaceMenuStart>(gameObject).OpenAniEnd();
			rStart.returnMenuClub();
		};
	}

	void ApplicationQuit(){
		UserDataManager.instance.OnSubBackMenu = null;
	}

	void OnGoClan(){
		UserDataManager.instance.OnSubBackMenu = null;
		rStart.returnMenuClubSub();
	
	}

	public void initClub(){
		for(int i = 0; i < ClubWin.transform.childCount; i++){
			ClubWin.transform.GetChild(i).gameObject.SetActive(false);
		}
	}

}
