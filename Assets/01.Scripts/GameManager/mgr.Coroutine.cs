using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public partial class GameManager :  MonoSingleton< GameManager > {

	#region StartCoroutine

	IEnumerator RankCheckPoint(string str,int checknum){

		if(str == "StartMiddleRankCheck")
			StartCoroutine("MiddleRank");
		else {
			yield return StartCoroutine(str,checknum);
			StartCoroutine("FinalRank");
		}
		yield return null;
	}

	IEnumerator StartMiddleRankCheck(int checknum){
	//	int cnt = Global.Race01Ranking.Length;
	//	int cnt = 5;
	//	spriteName =  new string[AIcar.Length];
	//	while(true){
	//		if(Global.Race01Ranking[cnt-1] != null){
	//			break;
	//		}
	//		yield return null;
	//	}
		yield return null;
	}

	void AiCarFinishMessage(){
		for (int i = 0; i < AIcar.Length; i++){
			AIcar[i].SendMessage("recvFinishMsg", SendMessageOptions.DontRequireReceiver);
		}
	}

	IEnumerator StartFinalRankCheck(int checknum){
		int cnt = Global.Race02Ranking.Length;
		if(Global.gRaceInfo.mType == MainRaceType.Weekly){
			bool isMy = true;
			int rank = 0;
			while(isMy){
				for(int i = 0; i < cnt ; i++){
					if(Global.Race02Ranking[i] != null){
						if(Global.Race02Ranking[i].Equals("M_R1")){
							isMy = false;
							rank = i;
						}
					}
				}
				yield return null;
			}
			if(rank <3){
				yield return new WaitForSeconds(2.5f);
			}else{
				yield return new WaitForSeconds(2.5f);
			}
			AiCarFinishMessage();
			yield break;
		}
		int count = 0;
		if(Global.gRaceInfo.mType == MainRaceType.Champion){
			while(true){
				//Utility.Log("count " + count);
				if(count > 30){
					AiCarFinishMessage();
					yield break;
				}
				if(Global.Race02Ranking[cnt-1] != null){
					if(!isFinalGUI){
						Global.gRaceCount = 0;
						yield break;
					}
				}else{
					count++;
					yield return new WaitForSeconds(0.1f);
				}
				yield return null;
			}
			yield break;
		}else{ // PVP , Regular , Drag, Event etc 
			while(true){
				if(count > 30){
					AiCarFinishMessage();
					yield break;
				}
				if(Global.Race02Ranking[cnt-1] != null){
					if(!isFinalGUI){
						Global.gRaceCount = 0;
						yield break;
					}
				}else{
					count++;
					yield return new WaitForSeconds(0.1f);
				}
				yield return null;
			}
			yield break;
		}
	}

	IEnumerator NewStartFinalRankCheck(int checknum){
		int cnt = Global.Race02Ranking.Length;
		if(Global.gRaceInfo.mType ==  MainRaceType.Weekly){
			bool isMy = true;
			int rank = 0;
			while(isMy){
				for(int i = 0; i < cnt ; i++){
					if(Global.Race02Ranking[i] != null){
						if(Global.Race02Ranking[i].Equals("M_R1")){
							isMy = false;
							rank = i;
						}
					}
				}
				yield return null;
				if(rank <3){
					yield return new WaitForSeconds(2.5f);
				}
				AiCarFinishMessage();
			}
			yield break;
		}
		while(true){
			if(Global.Race02Ranking[cnt-1] != null){
				if(!isFinalGUI){
					Global.gRaceCount = 0;
					yield break;
				}
			}
			yield return null;
		}
	}
	#region MiddleRank
	[HideInInspector]
	public string[] rankSpritNames;// = new string[5];

	IEnumerator  MiddleRank(){
		StopCoroutine("StartMiddleRankCheck");
		var obj = mgrgui.findPanel("Rank");
		obj.SetActive(true);
		int carlength = AIcar.Length + 1;
		if (carlength == 7) carlength = 7;
		obj.AddComponent<RankPointCheck>().MiddleSprite(obj, carlength, middleRankOut);//("middleRankOut");
		yield return null;
	}

	void middleRankOut(bool isSuccess){
		isFinalGUI = true;
		var obj = mgrgui.findPanel("Rank");
		obj.SetActive(false);
		obj = null;
	}

	#endregion
		IEnumerator FinishGUI(){
		var pObj = mgrgui.findPanel("FinishGUI") as GameObject;
		pObj.SetActive(true);
		var g = ObjectManager.GetRaceObject("Race","checkflag0") as GameObject;
		g.transform.parent = pObj.transform;

		g.transform.localPosition =new Vector3(530, -420,0);
		g.transform.localScale = Vector3.one;
		g.SetActive(true);

		g = ObjectManager.GetRaceObject("Race","checkflag1") as GameObject;
		g.transform.parent = pObj.transform;
		g.transform.localPosition = new Vector3(-530, -420,0);
		g.transform.localScale = Vector3.one;
		g.SetActive(true);
		//StartCoroutine(FlagSound());
		yield return new WaitForSeconds(0.5f);
		g = ObjectManager.GetRaceObject("Race","You") as GameObject;
		g.transform.parent = pObj.transform;
		g.transform.localPosition = Vector3.zero;
		g.transform.localScale = Vector3.one;
		g.SetActive(true);
		if(Global.gRaceInfo.mType == MainRaceType.Weekly){
			bool rkcheck = false;
			if(Global.Race02Ranking[0] == "M_R1") rkcheck = true;
			else if(Global.Race02Ranking[1] == "M_R1") rkcheck = true;
			else  if(Global.Race02Ranking[2] == "M_R1") rkcheck = true;
		//	Utility.LogWarning("Race02Ranking 1 " + Global.Race02Ranking[0]);
		//	Utility.LogWarning("Race02Ranking 1 " + Global.Race02Ranking[1]);
		//	Utility.LogWarning("Race02Ranking 1 " + Global.Race02Ranking[2]);
			if(rkcheck){
				g.transform.FindChild("Win").gameObject.SetActive(true);
				g.transform.FindChild("Win").GetComponent<UISprite>().spriteName = "YouFinish";
				GameFinishSound(true);
			}else{
				g.transform.FindChild("Lose").gameObject.SetActive(true);
				g.transform.FindChild("Lose").GetComponent<UISprite>().spriteName = "YouRetire";
				GameFinishSound(false);
			}
		}else{
		//	Utility.LogWarning("FinishGUI _" + Global.Race02Ranking[0]);
			if(Global.Race02Ranking[0] == "M_R1"){
				g.transform.FindChild("Win").gameObject.SetActive(true);
				GameFinishSound(true);
			}else{
				g.transform.FindChild("Lose").gameObject.SetActive(true);
				GameFinishSound(false);
			}
		}
		
		g=null;
		pObj = null;

	}
	IEnumerator NewFinishGUI(){
		var pObj = mgrgui.findPanel("FinishGUI") as GameObject;
		pObj.SetActive(true);
		var g = ObjectManager.GetRaceObject("Race","checkflag0") as GameObject;
		g.transform.parent = pObj.transform;
		
		g.transform.localPosition =new Vector3(530, -420,0);
		g.transform.localScale = Vector3.one;
		g.SetActive(true);
		
		g = ObjectManager.GetRaceObject("Race","checkflag1") as GameObject;
		g.transform.parent = pObj.transform;
		g.transform.localPosition = new Vector3(-530, -420,0);
		g.transform.localScale = Vector3.one;
		g.SetActive(true);
		//StartCoroutine(FlagSound());
		yield return new WaitForSeconds(0.5f);
		
		//g = ObjectManager.CreatePrefabs("Race", "You") as GameObject;
		g = ObjectManager.GetRaceObject("Race","You") as GameObject;
		g.transform.parent = pObj.transform;
		g.transform.localPosition = Vector3.zero;
		g.transform.localScale = Vector3.one;
		g.SetActive(true);
		if(Global.gRaceInfo.mType == MainRaceType.Weekly){
			bool rkcheck = false;
			if(Global.Race02Ranking[0] == "M_R1") rkcheck = true;
			else if(Global.Race02Ranking[1] == "M_R1") rkcheck = true;
			else  if(Global.Race02Ranking[2] == "M_R1") rkcheck = true;
			
			if(rkcheck){
				g.transform.FindChild("Win").gameObject.SetActive(true);
				g.transform.FindChild("Win").GetComponent<UISprite>().spriteName = "YouFinish";
				GameFinishSound(true);
			}else{
				g.transform.FindChild("Lose").gameObject.SetActive(true);
				g.transform.FindChild("Lose").GetComponent<UISprite>().spriteName = "YouRetire";
				GameFinishSound(false);
			}
		}else{
			if(Global.Race02Ranking[0] == "M_R1"){
				g.transform.FindChild("Win").gameObject.SetActive(true);
				GameFinishSound(true);
			}else{
				g.transform.FindChild("Lose").gameObject.SetActive(true);
				GameFinishSound(false);
			}
		}
		
		g=null;
		pObj = null;
		
	}

	IEnumerator FinalRank()
	{
		var pObj = mgrgui.findPanel("FinishGUI") as GameObject;
		int carlength = AIcar.Length + 1;
		if (carlength == 7) carlength = 7;
		//StartCoroutine(FinalRankSound(carlength));
		pObj.AddComponent<RankPointCheck>().StartFinalRank(pObj, carlength, FinalCompenstaionShow);
		//yield return new WaitForSeconds(3.0f);
		yield return null;
	} 


	void FinalCompenstaionShow(bool isCheck, string name){
		isTimeCheck = false;
		var pObj = mgrgui.findPanel("FinishGUI") as GameObject;
		var compensation = ObjectManager.GetRaceObject("Race", "compensation_1") as GameObject;
		ObjectManager.ChangeObjectParent(compensation, pObj.transform);
		ObjectManager.ChangeObjectPosition(compensation, Vector3.zero, Vector3.one, Vector3.zero);
		compensation.SetActive(true);
		bool b= true;
		if(Global.gRaceInfo.sType == SubRaceType.DragRace){
			pObj = compensation.transform.FindChild("Pitstop_NO").gameObject;
			b = true;
		}else{
			b = false;
			pObj =compensation.transform.FindChild("Pitstop_YES").gameObject;
		}
		pObj.SetActive(true);
		StartCoroutine(CompensationSound());
		compensation.AddComponent<Resultdisplayaction>().SendToServerRaceResult();
		compensation.GetComponent<Resultdisplayaction>().CompensationResult(pObj,name, b);
		compensation = null;
		pObj = null;
	}

	IEnumerator FinalRankSound(int count){
		for(int i  =0; i < count ; i++){
			yield return new WaitForSeconds(0.12f);
			_uiSound.WhipPanelPlay();
		}
	}

	IEnumerator FlagSound(){
		//_uiSound.FlagSoundPlay();
		//mgrgui.playFinishSound();
		yield return new WaitForSeconds(0.1f);
	}
	IEnumerator YouSound(){
		_uiSound.WhipPanelPlay();
		yield return new WaitForSeconds(0.1f);

	}

	IEnumerator CompensationSound(){
		_uiSound.WhipPanelAwardPlay();
		yield return new WaitForSeconds(0.2f);
		_uiSound.WhipPanelAwardPlay();
		yield return new WaitForSeconds(0.2f);
		_uiSound.WhipPanelAwardPlay();
		yield return new WaitForSeconds(0.2f);
		_uiSound.WhipPanelAwardPlay();
		yield return new WaitForSeconds(0.2f);
		_uiSound.WhipPanelAwardPlay();
		yield return new WaitForSeconds(0.2f);
	//	_uiSound.WhipPanelAwardPlay();
	//	yield return new WaitForSeconds(0.2f);
	//	_uiSound.WhipPanelAwardPlay();
	}

	IEnumerator  GameFinishGUI(){
		//StopCoroutine("DisplayTotalDollar");
		OkButtonActive();
		mgrgui.CarSoundVolumeDown(()=>{
			AudioManager.Instance.StopAllSound();
		});

		yield return null;
	}

 void OkButtonActive(){
}
	
	public bool isCompetiton = false;
	public float CompetitionTime = 0.0f;
	public void FinalRankCompetition(){
		Mycar.SendMessage("AICompetitionCheck", SendMessageOptions.DontRequireReceiver);
		for (int i = 0; i < AIcar.Length; i++){
			AIcar[i].SendMessage("CompetitionCheck", SendMessageOptions.DontRequireReceiver);
		}

	}
	public void FinalRankCompetition_Drag(){
		Mycar.SendMessage("AICompetitionCheck_Drag", SendMessageOptions.DontRequireReceiver);
		for (int i = 0; i < AIcar.Length; i++){
			AIcar[i].SendMessage("CompetitionCheck_Drag", SendMessageOptions.DontRequireReceiver);
		}
	}
	IEnumerator SnapShotCameraDrag(){
		float delay = 0.025f;
		float delay1 = 0.025f;
			GameObject game = mgrgui.findPanel("snapshot");// GameObject.Find("snapshot");
			Camera cam = currentCamera;
			Time.timeScale = 0.1f;
			float _speed = _AniSpeed;
			cam.fieldOfView = 20;
			game.SetActive(true);
			yield return new WaitForSeconds(delay);
			game.SetActive(false);
			_uiSound.CarCameraShutterSound();
			yield return new WaitForSeconds(delay1);
			
			cam.fieldOfView = 15;
			game.SetActive(true);
			yield return new WaitForSeconds(delay);
			game.SetActive(false);
			_uiSound.CarCameraShutterSound();
			yield return new WaitForSeconds(delay1);
			
			cam.fieldOfView = 10;
			game.SetActive(true);
			yield return new WaitForSeconds(delay);
			game.SetActive(false);
			_uiSound.CarCameraShutterSound();
			yield return new WaitForSeconds(delay1);
			
			cam.fieldOfView = 5;
			game.SetActive(true);
			yield return new WaitForSeconds(delay);
			game.SetActive(false);
			_uiSound.CarCameraShutterSound();
			yield return new WaitForSeconds(delay1);
			cam.fieldOfView = 10;
			Time.timeScale = 1.0f;
			_AniSpeed =_speed;
			game = null; cam = null;
		/**** 최종 랭킹 보여주는 항목*****************/
		StartCoroutine("DecreaseAnispeed");
		StartCoroutine(RankCheckPoint("StartFinalRankCheck", 9));
		StartCoroutine("FinishGUI");
	}
	IEnumerator SnapShotCamera(){
	//	mgrgui.CarSoundVolumeDown(()=>{
	//		AudioManager.Instance.audioPitch(0.2f);
	//		AudioManager.Instance.CarVolumDown(0.3f);
	//	});
		float delay = 0.025f;
		float delay1 = 0.025f;
		//Time.timeScale = 0.1f;
		if(!isCompetiton){
			yield return new WaitForSeconds(0.2f);

		}else{
			GameObject game = mgrgui.findPanel("snapshot");// GameObject.Find("snapshot");
			Camera cam = currentCamera;
			Time.timeScale = 0.1f;
			float _speed = _AniSpeed;
			cam.fieldOfView = 20;
			game.SetActive(true);
			yield return new WaitForSeconds(delay);
			game.SetActive(false);
			_uiSound.CarCameraShutterSound();
			yield return new WaitForSeconds(delay1);
			
			cam.fieldOfView = 15;
			game.SetActive(true);
			yield return new WaitForSeconds(delay);
			game.SetActive(false);
			_uiSound.CarCameraShutterSound();
			yield return new WaitForSeconds(delay1);
			
			cam.fieldOfView = 10;
			game.SetActive(true);
			yield return new WaitForSeconds(delay);
			game.SetActive(false);
			_uiSound.CarCameraShutterSound();
			yield return new WaitForSeconds(delay1);
			
			cam.fieldOfView = 5;
			game.SetActive(true);
			yield return new WaitForSeconds(delay);
			game.SetActive(false);
			_uiSound.CarCameraShutterSound();
			yield return new WaitForSeconds(delay1);
			cam.fieldOfView = 10;
			Time.timeScale = 1.0f;
			_AniSpeed =_speed;
			game = null; cam = null;
		
		}
		//Utility.Log ("_anispeed " + _speed);
		/**** 최종 랭킹 보여주는 항목*****************/
		yield return new WaitForSeconds(1.5f);
		StartCoroutine("DecreaseAnispeed");
		if(Global.isTutorial){
			StartCoroutine("RaceTutorialFinish");
		}else{
			StartCoroutine(RankCheckPoint("StartFinalRankCheck", 9));
		}
		StartCoroutine("FinishGUI");
	}


	IEnumerator RaceTutorialFinish(){
		yield return new WaitForSeconds(1.5f);
		mgrgui.TutorialEnd();
	}

	IEnumerator DecreaseAnispeed(){
		yield return new WaitForSeconds(1.0f);
		for(;;){
			_AniSpeed -= 0.2f;
			if(_AniSpeed < 0.8f){
				_AniSpeed = 0.8f;
				StopCoroutine("DecreaseAnispeed");
				break;
			}
			yield return new WaitForSeconds(0.01f);
		}
	}

	IEnumerator GUIGameReady(){
		fade = fadeState.nothing;
		yield return new WaitForSeconds(2.0f);
		Signal = mgrgui.FindChildObject("Signal");
		Signal.BroadcastMessage("showSignal",SendMessageOptions.DontRequireReceiver);
		yield return new WaitForSeconds(1.0f);
		GUIDashBoardStart();
		GUIMinMapStart();
	}

	public void ShowSignalCount(){
		var temp = mgrgui.FindChildObject("Signal") as GameObject;
		temp.BroadcastMessage("ShowSIG",SendMessageOptions.DontRequireReceiver);
		GameManager.instance._isPress = false;
		GUIDashBoardStart();
		GUIMinMapStart();
	}

	public void ShowSignalCountStart(){
		var temp = mgrgui.FindChildObject("Signal") as GameObject;
		temp.BroadcastMessage("ShowSIGStart",SendMessageOptions.DontRequireReceiver);
	}
	public void HiddenSignalCount(){
		var temp = mgrgui.FindChildObject("Signal") as GameObject;
		temp.BroadcastMessage("HiddenSIG",SendMessageOptions.DontRequireReceiver);
	}

	public void ShowSignalFlag(){
		var temp = mgrgui.FindChildObject("Signal") as GameObject;
		temp.BroadcastMessage("ShowFlagSIG",SendMessageOptions.DontRequireReceiver);
	}

	public void HiddenSignalFlag(){
		var temp = mgrgui.FindChildObject("Signal") as GameObject;
		temp.BroadcastMessage("OnClick",SendMessageOptions.DontRequireReceiver);
	}

	public void HiddenAutoSignalFlag(){
		var temp = mgrgui.FindChildObject("Signal") as GameObject;
		temp.BroadcastMessage("OnAutoHidden",SendMessageOptions.DontRequireReceiver);
	}


	IEnumerator GUIGameReadyOver(){
		
		yield return null;
	}


	IEnumerator GUIGameSignalBlink(){
		//mgrgui.findPanel("FinishGUI").SetActive(true);
		//FinalCompenstaionShow(true);
		yield return null;
	}
	

	public void HiddenGUI(){
		GUIDashBoardEnd();
		GUIMinMapEnd();
	}

	public void SetBGM_Drag(){
		mgrgui.CarSoundVolumeDown(()=>{
			AudioManager.Instance.CarVolumDown(0.3f);
			AudioManager.Instance.audioPitch(1.0f);
			AudioManager.Instance.StopMusicSound(0.3f);
		});
	}
	IEnumerator GUIResultDrag(){
		//HiddenGUI();
		yield return new WaitForSeconds(1.5f);
		SwitchingCamera("Cam_R2_Result_1");
	
		state = STATE.FINISH;  // 최종 메뉴 전시되는 부분
		yield return new WaitForSeconds(_td.t_R2Result01);
		SwitchingCamera("Cam_R2_Result_2");
		yield return new WaitForSeconds(_td.t_R2Result02);
		SwitchingCamera("Cam_R2_Result_3");
		yield return new WaitForSeconds(_td.t_R2Result03);
	}

	IEnumerator GUIResult(){
		//HiddenGUI();
		yield return new WaitForSeconds(1.5f);
		SwitchingCamera("Cam_R2_Result_1");
	
		state = STATE.FINISH;  // 최종 메뉴 전시되는 부분
		yield return new WaitForSeconds(_td.t_R2Result01);
		SwitchingCamera("Cam_R2_Result_2");
		yield return new WaitForSeconds(_td.t_R2Result02);
		SwitchingCamera("Cam_R2_Result_3");
		yield return new WaitForSeconds(_td.t_R2Result03);
	}
	
	IEnumerator Race01End(){
		mgrgui.EngineSoundOff();
		GUIDashBoardEnd();
		if(Signal == null)  Signal = mgrgui.FindChildObject("Signal");
		Signal.BroadcastMessage("hiddenSignal",SendMessageOptions.DontRequireReceiver);
		Mycar.SendMessage("PitSoundPlay", SendMessageOptions.DontRequireReceiver);
		int carlength = AIcar.Length + 1;
		if (carlength == 7) carlength = 7;
		rankSpritNames = new string[carlength];
		Global.gRaceCount = 0;
		yield return null;
	}
	
	IEnumerator Race01Finish(){
		float pitDelay = 0.0f;
		float delay1 = 4.124f;
		switch(Global.gRaceInfo.sType){
		case SubRaceType.DragRace:
			pitDelay = 4.124f/2.0f;//_td.t_PitStart;
			break;
		case SubRaceType.CityRace:
			pitDelay = 2.458f/2.0f;
			delay1 = 2.458f;
			break;
		case SubRaceType.RegularRace:
			pitDelay = 4.124f/2.0f;
			break;
		default:
			break;
		}

		SwitchingCamera("Cam_Pit_In_1");
		mgrpit.enabled = true;
		mgrpit.StartCoroutine("BoltStart", delay1);
		mgrgui.CamPitIn1();
		yield return new WaitForSeconds(pitDelay);
		SwitchingCamera("Cam_Pit_In_2");
		yield return new WaitForSeconds(1.0f);
		mgrgui.GearTextShow("GEAR");
		mgrgui.EngineLedOff();
		mgrgui.engineStop();
		mgrpit.StartPitIn();
		yield return new WaitForSeconds(0.5f);
		mgrgui.CamPitIn2();
		yield return new WaitForSeconds(0.5f);
	}

	IEnumerator Race02Start(){
		Signal.transform.FindChild("Ready").gameObject.SetActive(false);
		Signal.SetActive(true);
		Signal.BroadcastMessage("showSignal1",SendMessageOptions.DontRequireReceiver);
		yield return new WaitForSeconds(1.0f);
	}
	IEnumerator  StartRace02Delay(float delay){
		isWheel =true;
		Mycar.SendMessage("makeSkidMark", SendMessageOptions.DontRequireReceiver);
		AudioManager.Instance.SetCarVolume(0.0f);
		SwitchingCamera("Cam_R2_Gear1_Skid");
		float delay1 = Base64Manager.instance.getFloatEncoding(Global.gTireDelay, 0.001f);
		delay1 = delay1+delay;
		mgrpit.StartCoroutine("SpeecherOut");
		yield return new WaitForSeconds(delay1);
		mgrgui.RandomSwitchCamera();
		mgrpit.enabled = false;
		Mycar.SendMessage("Race02Start", SendMessageOptions.DontRequireReceiver);
		RaceState = GAMESTATE.RACE02;
		mgrgui.setCurrentGear(1, delay);
		SaveResultTime("R2_S");
	}

	IEnumerator  StartRace01Delay(float delay){
		isWheel = true;
		Mycar.SendMessage("makeSkidMark", SendMessageOptions.DontRequireReceiver);
		AudioManager.Instance.SetCarVolume(0.0f);
		SwitchingCamera("Cam_R1_Gear1_Skid");
		float delay1 = Base64Manager.instance.getFloatEncoding(Global.gTireDelay, 0.001f);
		delay1 = delay1 + delay;
		yield return new WaitForSeconds(delay1);
		mgrgui.RandomSwitchCamera();
		mgrpit.enabled = false;
		Mycar.SendMessage("Race01Start", SendMessageOptions.DontRequireReceiver);
		HiddenSignalFlag();
		RaceState = GAMESTATE.RACE01;
		mgrgui.setCurrentGear(1, delay);
	}
	#endregion
}
