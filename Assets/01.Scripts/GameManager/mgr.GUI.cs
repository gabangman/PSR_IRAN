using UnityEngine;
using System.Collections;

partial class GameManager :  MonoSingleton< GameManager > {

	private bool isQuit =false;
	void FixedUpdate(){
		//if(Global.isTutorial) return;
	}
	void playBackfunction(){
		if(isPause){
			if(Input.GetKeyDown(KeyCode.Escape)){
				if(!Global.bLobbyBack) return;
				Global.bLobbyBack = false;
				if(UserDataManager.instance.OnSubBack != null)
					UserDataManager.instance.OnSubBack();
				isQuit = true;	
				Invoke("OnIsQuitActivation", 0.5f);
			}
			return;
		}
		if (Input.GetKey(KeyCode.Escape)){
			if(isQuit) return;
			isQuit = true;	
			Invoke("OnIsQuitActivation", 0.5f);
			btnMenu();
		}
	}

	void Update(){
		playBackfunction();
		if(isTimeCheck){
			_totalTime += Time.deltaTime;
		}
		lbGameTime.text = "Time : " + System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((_totalTime/60f)) ,_totalTime%60.0f);
	}

	private void OnIsQuitActivation(){
		//Utility.LogWarning("isQuit " + isQuit);
		isQuit = false;
	}


	public void btnLobbyToRace(){
		fade = fadeState.fadeout;
		Global.isLobby = true;
		gameState = 2;
		//GameFinishGUIEnd();
		Global.isRace = false;
		CardSelectSound();
		if(Global.gChampTutorial != 0){
			Global.gChampTutorial = 0;
		}else{
			
		}
	}

	void btnLobby(){
		Time.timeScale = 1f;
		fade = fadeState.fadeout;
		Global.isLobby = true;
		gameState = 2;
		//GameFinishGUIEnd();
		Global.isRace = false;
		if(Global.gChampTutorial != 0){
			Global.gChampTutorial = 0;
		}else{
			
		}
	}
	
	GameObject menu1;
	public void btnMenu(){
		if(menu1 != null) return;
		isPause = true;
		var test =Resources.Load("Prefabs/Race/gameMenu_1", typeof(GameObject)) as GameObject;
		menu1 = Instantiate(test) as GameObject;
		var a = mgrgui.findPanel("RaceUp") as GameObject;
		menu1.transform.parent = a.transform.parent.transform.parent;
		menu1.transform.localScale = Vector3.one;
		menu1.transform.localPosition = Vector3.zero;
		menu1.transform.localEulerAngles = Vector3.zero;
		AudioManager.Instance.OnPauseMessage();
		menu1.GetComponent<gamemenuaction>().OnGamePause();
		Time.timeScale = 0.0f;
		test = null;
		if(Global.gChampTutorial == 0){
		
		}else{
			mgrgui.TutorialHidden();
		}
		Resources.UnloadUnusedAssets();
		return;
	}

	public Transform getBTN(){
		var a = mgrgui.findPanel("RaceUp") as GameObject;
		return a.transform.parent.transform.parent;
	}

	public Transform getNetwork(){
		var a = mgrgui.findPanel("FinishGUI") as GameObject;
		return a.transform;
	}


	void OnApplicationFocus(bool _state){
		#if UNITY_ANDROID && !UNITY_EDITOR
		if(!_state){
		
		}else{
			//Time.timeScale = 1.0f;
		}
		#endif
		if(!_state){
			
		}else{
			//Time.timeScale = 1.0f;
		}
	}

	void OnApplicationPause(bool _state){
		#if UNITY_ANDROID 
		if(!_state){
		
		
		}else{
			if(isPause) return;
			AudioManager.Instance.OnPauseMessage();
			isPause = true;
			UserDataManager.instance.OnRacePause();
			UserDataManager.instance.myGameDataSave();
			Time.timeScale = 0.0f;
		}
		#endif
	}

	void OnPauseMessage(){
	}
	
	
	public void btnContinue(){
		AudioManager.Instance.OnResumeMessage();
		isPause = false;
		isQuit = false;
		if(Global.gChampTutorial == 0){
			
		}else{
			mgrgui.TutorialShow();
		}
		if(menu1 == null) return;
		DestroyImmediate(menu1);
	}

	public void btnReStart(){
		AudioManager.Instance.OnResumeMessage();
		isPause = false;
		fade = fadeState.fadeout;
		gameState = 4;
		Resources.UnloadUnusedAssets();
		System.GC.Collect();
	}

	public void btnQuit(){
		menu1 = null;
		DestroyImmediate(menu1);
		fade = fadeState.fadeout;
		//Global.isLobby = true;
	
		gameState = 1;
		//GameFinishGUIEnd();
		//Global.isRace = false;
		//Global.Loading = false;
	}

	public void OnQuitClick(){
		btnMenu();
	}
	public void GUIDashBoardStart(){
		mgrgui.speechActive(true);
		mgrgui.addTween("DashBoard", new Vector3(0,-450,0), Vector3.zero, 0.5f);
	}
	
	public void GUIDashBoardEnd(){
		if(isN2O){
			isN2O = false; Invoke("ResetN2O", 2.0f);
		}
		mgrgui.endTween("DashBoard", new Vector3(0,-450,0), Vector3.zero, 0.5f);
		mgrgui.speechActive(false);
		mgrgui.MaxGearTextAction(false);
	}

	void ResetN2O(){
		isN2O = true;
	}

	public void GUIMinMapStart(){
		mgrgui.addTween("RaceUp", new Vector3(0,400,1), new Vector3(0,0,1), 0.5f);
	}
	
	public void GUIMinMapEnd(){
		mgrgui.endTween("RaceUp", new Vector3(0,400,1), new Vector3(0,0,1), 0.5f);

	}
	public void GameFinishGUIEnd(){
		mgrgui.endTween("RaceDown", new Vector3(0,-400,0), new Vector3(0,-270,0), 0.5f);
		//mgrgui.speechActive(false);
	}

}
