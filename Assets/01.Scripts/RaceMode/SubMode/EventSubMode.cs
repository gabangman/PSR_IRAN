using UnityEngine;
using System.Collections;

public class EventSubMode : RaceMode {
	
	public GameObject popstart,popdisable, popend;
	public Transform Grid;
	
	void Awake(){
		popdisable.transform.FindChild("NEXT").FindChild("Title").GetComponent<UILabel>().text 
			= KoStorage.GetKorString("71000");
	}
	
	
	void OnEnable(){
		//popdisable.SetActive(false);
		//popstart.SetActive(false);
		if(popstart.activeSelf) popstart.SetActive(false);
		if(popdisable.activeSelf) popdisable.SetActive(false);
		if(!Grid.gameObject.activeSelf) Grid.gameObject.SetActive(true);
	}
	
	
	protected void ReadyLoadingCircle(){
		Global.isNetwork = true;
		var loading = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
		loading.StartCoroutine("StartRaceLoading", false);//raceStartLoading
	}
	
	
	public override void OnNext (GameObject obj)
	{if(Global.isPopUp) return;
		base.OnNext (obj);
		popdisable.SetActive(false);
		
		if(UserDataManager.instance.raceFuelCountCheck()) return;
		string[] str = btnName.Split('_');
		if(str[1].Equals("2") == false){
			UserDataManager.instance.raceFuelCounting();
		}
		
		GV.gEntryFee = 0;
		ReadyLoadingCircle();
		gameObject.AddComponent<SettingRaceEvent>().setEventRace(str[1],
		                                                         mTrack, mCar);
	}
	
	public override void SetRaceSubMode(){
		if(Grid.GetChild(0).name.Equals("Evnet_0")==false){
			for(int i=0; i < Grid.childCount; i++){
				Grid.GetChild(i).name = "EVENT_"+i.ToString();
				//	if(myAccount.instance.account.eRace.mRaceMode == i){
				Grid.GetChild(i).gameObject.SetActive(true);
				Grid.GetChild(i).gameObject.AddComponent<EventItem>().InitEventWindow();
				//	}else{
				//		Grid.GetChild(i).gameObject.SetActive(false);
				//	}
			}
		}
		/*
		if(Grid.GetChild(0).name.Equals("Evnet_0")==false){
			for(int i=0; i < Grid.childCount; i++){
				Grid.GetChild(i).name = "EVENT_"+i.ToString();
				if(i == 2){
					Grid.GetChild(i).gameObject.SetActive(true);
					Grid.GetChild(i).gameObject.AddComponent<EventItem>().InitEventWindow();
				}else{
					Grid.GetChild(i).gameObject.SetActive(false);
				}
			}
		}	*/
	}
	
	public void NothingEvoCubeInRace(){
		popstart.SetActive(false);
		popend.SetActive(false);
		
		popdisable.SetActive(true);
		popdisable.transform.FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73323");
		popdisable.transform.FindChild("NEXT").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("71000");
		popdisable.transform.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		//Grid.gameObject.SetActive(true);
		Grid.FindChild("EVENT_2").GetComponent<EventItem>().InitEventWindow();
	}
	
	
	
	void OnClose(){
		popstart.SetActive(false);
		popdisable.SetActive(false);
		popend.SetActive(false);
		Grid.gameObject.SetActive(true);
		Global.bLobbyBack = false;
		if(bModeCheck){
			
		}else{
			GameObject.Find("LobbyUI").SendMessage("MoveToDealer",SendMessageOptions.DontRequireReceiver);
			popdisable.transform.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
			Invoke("BackToTheDealer",0.35f);
		}
	}
	
	void BackToTheDealer(){
		NGUITools.FindInParents<RaceModeMenu>(gameObject).SetObjectDisable();
	}
	string btnName;
	bool bModeCheck;
	void OnEvent(GameObject btnObj){
		//	//!!--Utility.Log("OnEvent - modifiy");
		//Utility.Log("OnEvent " + btnObj.name);
		//return;
		
		if(popstart.activeSelf) return;
		if(popdisable.activeSelf) return;
		Grid.gameObject.SetActive(false);
		bModeCheck = true;
		if(btnObj.name == "EVENT_0"){
			CheckedNewCarEvent();
			btnName = "EVENT_0";
		}else if(btnObj.name == "EVENT_1"){
			CheckedReqCarEvent();
			btnName = "EVENT_1";
		}else{
			CheckedMaterialEvent();
			btnName = "EVENT_2";
		}
		// 바로 게임 시작
	}
	
	void OnNonEvent(){
		if(popstart.activeSelf) return;
		if(popdisable.activeSelf) return;
		bModeCheck = true;
		Grid.gameObject.SetActive(false);
		NonPlayPopup(KoStorage.GetKorString("73050"));
	}
	//set
	private int mTrack, mCar, mCrew, mPlay;
	void CheckedNewCarEvent(){
		//트랙지정 , 내가 없는 차량 지정, 할것!! 플레이 수 체크 할것.
		mTrack = 1412;//GV.eTrack[0];
		mCar = myAccount.instance.account.eRace.testDriveCarID;
		if(myAccount.instance.account.eRace.testDrivePlayCount == 5){
			//NonPlayPopup("play count is 5");
			FinishPopup(KoStorage.GetKorString("73323"));

			return;
		}
		CarInfo mcar = GV.mineCarList.Find(obj => obj.CarID == mCar);
		if(mcar != null) {
			//NonPlayPopup("너 새차 가지고 있어");
			NonPlayPopup(KoStorage.GetKorString("73303"));
			return;
		}
		// PlayPopup("새차 몰아보자");
		PlayPopup(KoStorage.GetKorString("73315"));
	}
	
	void CheckedReqCarEvent(){
		//트랙지정 , 차량 지정 후 내가 없는 차량 지정, 할것!! 플레이 수 체크 할것.
		mTrack = myAccount.instance.account.eRace.featuredTrackID;
		mCar =myAccount.instance.account.eRace.featuredCarID;
		if(myAccount.instance.account.eRace.featuredPlayCount  == 5){
			//NonPlayPopup("play count is 5");
			FinishPopup(KoStorage.GetKorString("73323"));
			return;
		}
		CarInfo mcar = GV.mineCarList.Find(obj => obj.CarID == mCar);
		if(mcar == null) {
			//NonPlayPopup("너 지정된 차 없어.");
			
			MoveToDealerPage(KoStorage.GetKorString("73306"));
			return;
		}
		//PlayPopup("지정된 차 몰아보자");
		PlayPopup(KoStorage.GetKorString("73314"));
	}
	void CheckedMaterialEvent(){
		//트랙지정, 서버에 문의 하여 실행 할 수 있는지 물어봐야 하며, 
		//한번밖에 플레이 못함. 
		mTrack =  myAccount.instance.account.eRace.EvoTrackID;
		mCar = GV.getTeamCarID(GV.SelectedTeamID);
		if(myAccount.instance.account.eRace.EvoPlayCount == 0){
			// 남아 있는 큐브가 없네.
			FinishPopup(KoStorage.GetKorString("73323"));
			return;
		}
		if(myAccount.instance.account.eRace.EvoAcquistCount == 1){
			// 큐브 한번 획득했네
			NonPlayPopup(KoStorage.GetKorString("73313"));
			return;
		}
		//PlayPopup("에보 큐브 얼릉해보자");
		PlayPopup(KoStorage.GetKorString("73316"));
	}
	
	void PlayPopup(string str){
		popstart.SetActive(true);
		popstart.transform.FindChild("Text1").GetComponent<UILabel>().text = str;
		popdisable.SetActive(false);
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnClosed();
		};
	}
	
	void FinishPopup(string str){
		popend.SetActive(true);
		popend.transform.FindChild("Text1").GetComponent<UILabel>().text = str;
		popend.transform.FindChild("NEXT").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("71000");
		
		popstart.SetActive(false);
		popdisable.SetActive(false);
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnClosed();
		};
	}
	void NonPlayPopup(string str){
		popstart.SetActive(false);
		popdisable.SetActive(true);
		popdisable.transform.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		popdisable.transform.FindChild("Text1").GetComponent<UILabel>().text = str;
		popdisable.transform.FindChild("NEXT").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("71000");
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnClosed();
		};
	}
	
	void MoveToDealerPage(string str){
		bModeCheck = false;
		popstart.SetActive(false);
		popdisable.SetActive(true);
		popdisable.transform.FindChild("Text1").GetComponent<UILabel>().text = str;
		popdisable.transform.FindChild("NEXT").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("71000");
		popdisable.transform.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
		popdisable.transform.FindChild("Sprite (Check_V)").GetComponent<UIButtonMessage>().functionName = "OnClosed";
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnClosed();
		};
	}
	
	void OnClosed(){
		bModeCheck = true;
		OnClose();
	}
	
}
