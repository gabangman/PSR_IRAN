using UnityEngine;
using System.Collections;

public class gamemenuaction : MonoBehaviour {
	
	//public GameObject Btn_Quit; 
	public GameObject Btn_Lobby;
	public GameObject Btn_Continue;
	public UILabel[] _text;
	//시작 부분에서 해당 오브젝트 들에 리스너를 달아 줍니다 
	void Start() 
	{ 
		if(gameObject.name.CompareTo ("RaceDown") == 0){
			//Utility.Log ("Race Over");
		}else{
			//UIEventListener.Get(Btn_Quit).onClick = btnQuit; 
			//	UIEventListener.Get(Btn_Lobby).onClick = btnLobby; 
			//	UIEventListener.Get(Btn_Continue).onClick = btnContinue_1;
			//Utility.Log ("GameMenu");
		}
		_text[0].text =KoStorage.GetKorString("71114"); // 다시 시
		_text[1].text = KoStorage.GetKorString("71112"); 
		_text[2].text =  KoStorage.GetKorString("72332");//연료
		_text[3].text = KoStorage.GetKorString("73056"); // 참가비
		_text[4].text = KoStorage.GetKorString("71112"); 

	
	} 
	public void OnGamePause(){
		Global.isNetwork =false;
		var Ex = transform.FindChild("Exit") as Transform;
		if(Global.gRaceInfo.mType == MainRaceType.Tutorial){
			Ex.FindChild("Replay").gameObject.SetActive(false);
			Ex.FindChild("Lobby").gameObject.SetActive(false);
			Ex.FindChild("Lobby_T").gameObject.SetActive(true);
		}else{
			if(	Global.gRaceInfo.mType == MainRaceType.mEvent || Global.gRaceInfo.mType == MainRaceType.Club || Global.gRaceInfo.mType == MainRaceType.PVP ){
				Ex.FindChild("Replay").gameObject.SetActive(false);
				Ex.FindChild("Lobby").gameObject.SetActive(false);
				Ex.FindChild("Lobby_T").gameObject.SetActive(true);
			}else{
				if(Global.gChampTutorial == 0){
					Ex.FindChild("Replay").gameObject.SetActive(true);
					Ex.FindChild("Lobby").gameObject.SetActive(true);
					Ex.FindChild("Lobby_T").gameObject.SetActive(false);
				}else{
					Ex.FindChild("Replay").gameObject.SetActive(false);
					Ex.FindChild("Lobby").gameObject.SetActive(false);
					Ex.FindChild("Lobby_T").gameObject.SetActive(true);
				}
			}
		}

		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnClose();
		};
	}
	void btnQuit(GameObject obj1){
		Application.Quit();
		return;
	}
	
	void OnDisable(){
		transform.FindChild("Exit").gameObject.SetActive(true);
		transform.FindChild("Dollar").gameObject.SetActive(false);
		transform.FindChild("Fuel").gameObject.SetActive(false);
		//	transform.FindChild("Exit_Tutorial").gameObject.SetActive(false);
	}
	
	void btnReStart(){
		if(GV.mUser.FuelCount > 0){
			int mCnt = GV.myDollar-GV.gEntryFee;
			if(mCnt < 0){
				transform.FindChild("Exit").gameObject.SetActive(false);
				transform.FindChild("Dollar").gameObject.SetActive(true);
			}else{
				myAcc.instance.account.lastConnectTime = NetworkManager.instance.GetCurrentDeviceTime().Ticks;
				GV.mUser.FuelCount--;
				GV.myDollar -= GV.gEntryFee;
				Time.timeScale = 1.0f;
				//NetworkManager.instance.ChangeUserGoods(GV.UserRevId,GV.gEntryFee ,GV.mUser.FuelCount);
				StartCoroutine("ChangeUserGoods");
			}
		}else{
			transform.FindChild("Exit").gameObject.SetActive(false);
			transform.FindChild("Fuel").gameObject.SetActive(true);
			UserDataManager.instance.OnSubBack = ()=>{
				OnClose1();
				Global.bLobbyBack = true;
				UserDataManager.instance.OnSubBack = ()=>{
					OnClose();
				};
			};
		}

	}
	
	IEnumerator ChangeUserGoods(){
		Global.isNetwork = true;
		NetworkManager.instance.ChangeUserGoods(GV.UserRevId,GV.gEntryFee ,GV.mUser.FuelCount);
		while(Global.isNetwork){
			yield return null;
		}
		GameObject obj = GameObject.Find("GameManager") as GameObject;
		if(obj != null)
			obj.SendMessage("btnReStart",SendMessageOptions.DontRequireReceiver);
		
		
	}
	
	
	
	void btnLobby_Tu(GameObject obj1){
		Time.timeScale = 1.0f;
		Global.gTutorial = 0;
		Global.isTutorial = false;
		Global.isReTutorial = false;
		Global.isRaceTest = false;
		GameObject obj = GameObject.Find("GameManager") as GameObject;
		if(obj != null)
			obj.SendMessage("btnLobby",SendMessageOptions.DontRequireReceiver);
		
	}
	void btnLobby(GameObject obj1){
		Time.timeScale = 1.0f;
		Global.isTutorial = false;
		Global.isReTutorial = false;
		Global.isRaceTest = false;
		GameObject obj = GameObject.Find("GameManager") as GameObject;
		if(obj != null)
			obj.SendMessage("btnLobby",SendMessageOptions.DontRequireReceiver);
		
		
	}
	void btnLobbyToRace(){
		GameObject obj = GameObject.Find("GameManager") as GameObject;
		obj.SendMessage("btnLobbyToRace",SendMessageOptions.DontRequireReceiver);
	}


	void OnClose(){
		Time.timeScale = 1.0f;
		Global.bLobbyBack = false;
		GameManager.instance.btnContinue();
	//	GameObject obj = GameObject.Find("GameManager") as GameObject;
	//	if(obj != null)
	//		obj.SendMessage("btnContinue",SendMessageOptions.DontRequireReceiver);
	}
	
	void OnClose1(){
		//연료가 없을때 팝업
		Time.timeScale = 1.0f;
		Global.bLobbyBack = false;
		GameManager.instance.btnContinue();
	//	GameObject obj = GameObject.Find("GameManager") as GameObject;
	//	if(obj != null)
	//		obj.SendMessage("btnContinue",SendMessageOptions.DontRequireReceiver);
	}
	
}
