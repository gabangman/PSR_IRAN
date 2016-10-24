using UnityEngine;
using System.Collections;
using System.Collections.Generic;
partial class GameManager :  MonoSingleton< GameManager > {
	
	//camera
	public Camera currentCamera;
	float gasAmount = 1.0f;



	public void SwitchingCamera(string str){
		strCameraName = _camList.SwitchingCamera(str);
		//Utility.LogWarning(strCameraName);
	}

	public void ChangeSwage(int n){
		_camList.InsertCameraValue(n);
	}

	public IEnumerator ZoomFOVCamera(string strjudge){
		float delay = 0.0f;
		switch(strjudge){
		case "Perfect":
			delay = 0.0f;
			break;
			case "Early":
			delay = 1.5f;
			break; 
			case "Late":
			delay = 1.5f;
			break;
			case "Good":
			delay = 1.0f;
			break;
		}
		yield return new WaitForSeconds(delay);
		_camList.ZoomFOV(strCameraName);
	}

	public float fixedDeltaTime;
	public float previousTime;
	public float deltaTime;
	public void AIRaceStart(){
		isTimeCheck = true;
		GameManager.instance.QuitCnt = 1;
		Utility.LogWarning("AIRACESTART");
		for (int i = 0; i < AIcar.Length; i++){
			AIcar[i].SendMessage("StartRace01", SendMessageOptions.DontRequireReceiver);
		}
		Mycar.SendMessage("CarAniSpeedCalculation", SendMessageOptions.DontRequireReceiver);
		previousTime = Time.timeSinceLevelLoad;
	}

//	public void StopMyCarDragRace(){
//		Mycar.SendMessage("StopMyCarRace", SendMessageOptions.DontRequireReceiver);
//	}
	public void GameRaceStateChange(string str){
		switch(str){
		case "race01finish":
			RaceState = GAMESTATE.PITIN;
			StartCoroutine(Race01Finish());
			break;
		case "race02finish":
		{
			mgrgui.EngineLedOff();
			StartCoroutine(GUIResult());
		}
			break;
		case "Ready":
		{
			StartCoroutine(GUIGameReady());
		}
			break;
		case "Start":{
			//mgrgui.EngineFirstLedOn();
			StartCoroutine(GUIGameSignalBlink());
		}break;
		case "Cam_Shake1":{
			mgrgui.CarSoundVolumeDown(()=> {
				AudioManager.Instance.StartCoroutine("CarAudioVolumDown");
			});
			mgrgui.EngineLedOff();
			//Utility.Log (currentCamera.name);
			Cam_Shake temp = currentCamera.GetComponent<Cam_Shake>() as Cam_Shake;
			temp.enabled = true;
		}break;
		case "Cam_Shake2":{
			_camList.CamShake(21);
			//temp1. enabled = true;
		}break;
		case "HiddenLight":
			StartCoroutine(GUIGameReadyOver());
			break;
		case "middleRank":
			StartCoroutine(RankCheckPoint("StartMiddleRankCheck", 4));
			break;
		case "race01end":
			StartCoroutine(Race01End());
			break;
		case "Race02Start":
		{
			_isRace02 = true;
			StartCoroutine("Race02Start");
		}		break;
		case "race01end_Drag":
		{
			mgrgui.EngineLedOff();
			StartCoroutine(GUIResultDrag());
		}
			break;
		default:
			break;
		}
	}



	public void ZoomInOutCamera(string str){
		_camList.ZoomInOutCamera(str);
	}
	public void ZoomInCamera(string str){
		_camList.ZoomInCamera(str);
	}

	//user info disable
	public void DisableSetActvieUser(){
		var info = mgrgui.findPanel("RaceUp") as GameObject;
		var info1 = info.transform.parent.gameObject;
		info = info1.transform.FindChild("UserInfo_RW").gameObject;	
		info.SendMessage("hiddenUserPanel",SendMessageOptions.DontRequireReceiver);
		info = info1 = null;
	}

	//GUI Funtion
	public void CameraAniStart(int _i){
		GameObject _temp = GameObject.Find("Cam_Pit_Play_1");
		_temp.AddComponent<PitCameraAni>().CameraPitAni(_i);
		_temp = null;
		
	}
	public IEnumerator PitCameraUserDelay(float delay){
		mgrgui.userPanelInfo.GetComponent<userinfoaction>().MultiUserCtrl(false);
		yield return new WaitForSeconds(delay);
		mgrgui.userPanelInfo.GetComponent<userinfoaction>().MultiUserCtrl(true);
	}
	public void CameraPitin(string str){
		//_cam.CamNum = _cam.cameraSelect(str);
		SwitchingCamera(str);
	}
	
	/*
	private GameObject MakeSignalFlag(){
		var signal = ObjectManager.CreatePrefabs("Race","Signal") as GameObject;
		signal.transform.parent = mgrgui.findPanel("TextObject").transform;
		signal.transform.localPosition = new Vector3(0,-75,0);
		signal.transform.FindChild("Ready").gameObject.SetActive(true);
		signal.name = "Signal";
		signal.transform.localScale = Vector3.one;
		return signal;
	}*/



	public void Race02StartCount(){
		mgrgui.StartCoroutine("StartRace02Count");//();
		mgrgui.engineStart();
	}


	public void RaceFinishSound(){
		_uiSound.RaceFinishSound();
		Mycar.SendMessage("R2FinishState",SendMessageOptions.DontRequireReceiver);
	}

	public void RewardSound(){
		//Utility.LogWarning("RewardSound");
		_uiSound.RewardSound();
	}

	public void GameFinishSound(bool b){
		//AudioManager.Instance.StopUISound();
		if(b)
			_uiSound.CarMusicFirstFinishSound();
		else
			_uiSound.CarMusicSecondFinishSound();
		//mgrgui.playFinishSound(b);
		AudioManager.Instance.CarAudioStop();
		mgrgui.CarSoundVolumeDown(()=> {
			//AudioManager.Instance.StartCoroutine("CarAudioVolumDown");
		});
	}

	public void PlayMyCrewAnimation(int step){
		Mycar.SendMessage("AnimationPlay",step, SendMessageOptions.DontRequireReceiver);
	}

	delegate void GasCheck();
	public bool isFilledGas = false;
	public void GasGague(string str){
		if(str == "Start"){
		}else if(str == "Fill"){
			gasAmount = 0.0f;
			var obj = mgrgui.findPanel("Fuel") as GameObject;
			StartCoroutine(GasGaguefillstart(obj, ()=>{
				Mycar.SendMessage("GasManOut",SendMessageOptions.DontRequireReceiver);
			}
			));
			obj = null;
		}
	}

	IEnumerator GasGaguefillstart(GameObject obj, GasCheck _gas){
		yield return new WaitForSeconds(0.5f);
		var gas = obj.transform.GetChild(0).gameObject as GameObject;
		var gasbar = gas.transform.FindChild("GasFg").GetComponent<UISprite>() as UISprite;
		gasbar.fillAmount = gasAmount;
		float delta = Base64Manager.instance.getFloatEncoding(Global.gGasTime,0.001f)*0.1f;
		var gas1 = gas.transform.FindChild("GasBg").gameObject;
		var twColor = gas1.GetComponent<TweenColor>() as TweenColor;
		twColor.enabled = true;
		while(true){
			gasAmount += delta;
			gasbar.fillAmount = gasAmount;
			if(gasAmount > 1.0f){
				_gas();
				isFilledGas = true;
				gasAmount = 1.0f;
				gasbar.fillAmount = gasAmount;
				twColor.style  = UITweener.Style.Once;
				_uiSound.GasFullSoundPlay();
				twColor.onFinished = delegate(UITweener tween) {
					tween.transform.GetComponent<UISprite>().color = Color.white;
				};
				twColor.transform.parent.gameObject.SendMessage("ShowFullText");
				mgrpit.EffectFuel();
				gas  = gas1 = null;
				yield break;
			}
			yield return new WaitForSeconds(0.1f);
		}
		//yield return null;
	}

	public void GasGauageOut(){
		var obj = mgrgui.findPanel("Fuel") as GameObject;
		TweenPosition _tween = obj.GetComponent<TweenPosition>() as TweenPosition;
		if(_tween != null){
			Destroy(_tween);
		}
		_tween = obj.AddComponent<TweenPosition>() as TweenPosition;
		_tween.from = Vector3.zero;
		_tween.to = new Vector3(0,-500,0);
		_tween.duration = 0.5f;
		_tween.method = UITweener.Method.EaseIn;
		_tween.eventReceiver = gameObject;
		_tween.callWhenFinished = "TweenEnd";
		obj = null;
	}

	public void N2OStart(bool b){
		isN2O = b;
		Mycar.SendMessage("BoostEffectStart", true, SendMessageOptions.DontRequireReceiver);
		_camList.movingFOV();
	}

	public void N2OEnd(){
		Mycar.SendMessage("BoostEffectStop", SendMessageOptions.DontRequireReceiver);
		_camList.movingFOVStop();
	}


	public int DragCamearNumber(int n){
		int num = 0;
		if(isDrag){
			float mytime = CompetitionTimes[0];
			float aitime = CompetitionTimes[1];
			//float aitime = 0;
			float delta = mytime - aitime;
			if(n >=10) n=n-10;
			if(delta <= -1.0f){
				//lose //50
				n = n+50;
			}else if(delta >= 1.0f){
				//win // 70
				n = n+70;
			}else if(delta <1.0f && delta > -1.0f){
				//similar //60
				n = n+60;
			}
			num = n;
		//	Utility.LogWarning("number " + n);
		//	Utility.LogWarning("mytime " + mytime);
		//	Utility.LogWarning("aitime " + aitime);
		}
		else{
			num = n;
		}
		return num;
	}


	void TweenEnd(TweenPosition tw){
		tw.transform.gameObject.SetActive(false);
	}

	void OnDestoryGameObject(){
		Resources.UnloadUnusedAssets();
		currentCamera = null;
		rankSpritNames = null;
	}

	public void RaceCountCheck(GameObject colObj){
		raceCount++;
		int cnt = AIcar.Length+1;
		if(raceCount == cnt){
			colObj.SetActive(false);
		}else if(raceCount == (cnt+1)){
			raceCount = 1;
		}

	}

	public void SaveResultTime(string state){
		switch(state){
		case "R1_DragE":
			Global.RaceResutTime = Base64Manager.instance.setFloatEncoding(_totalTime, 1000000);
			break;
		case "R1_E":
			break;
		case "R2_E":{
			Global.RaceResutTime = Base64Manager.instance.setFloatEncoding(_totalTime, 1000000);
		}
			break;
		case "PitIn_E":
		{
			float time1 =  Base64Manager.instance.getFloatEncoding(Global.PitInResutTime,0.000001f) ;
			time1 = _totalTime - time1 ;
			Global.PitInResutTime = Base64Manager.instance.setFloatEncoding(time1, 1000000);
		}break;
		case "R2_S":
			ShowSign(1);
			break;
		case "PitIn_S":
			float time2 = 0 ;
			time2 = _totalTime ;
			Global.PitInResutTime =  Base64Manager.instance.setFloatEncoding(time2, 1000000);
			break;
		default:
			break;
		}
	}

	public void Vibrategame(long vib){
		if(!Global.isVibrate) return;
		Vibration.Vibrate(vib);
	
	}

	public void playWinSound(){
		_uiSound.playWinSound();
	}
	public void playLossSound(){
		_uiSound.playLossSound();
	}
	public void playStarSound(){
		_uiSound.CarGearShiftSound();
	}
	
	public void CardSelectSound(){
		//_uiSound.stopCardMusic();
		//_uiSound.playCardSelectSound();
		mgrgui.CardSound(true);
	}
	public void playCardSound(){
		//_uiSound.startCardMusic();
		mgrgui.CardSound(false);
	}

	public void HiddenSign(int idx){
		if(idx == 0) {
			Sign_PitIn.SetActive(false);
			Mycar.SendMessage("SpeedFx_Pause");
		}
		else Sign_Finish.SetActive(false);
	}

	public void ShowSign(int idx){
		if(idx == 0) Sign_PitIn.SetActive(true);
		else Sign_Finish.SetActive(true);
	}

	public void CountMoneyStart(){
		_uiSound.playCountSound();
	}
	public void CountMoneyFinish(){
		_uiSound.playCountSoundStop();
	}
	public void CountCoinStart(){
		_uiSound.playCountCoinSound();
	}
	public void CountCoinFinish(){
		//_uiSound.playCountCoinSoundStop();
	}

	public void featuredSelectSound(){
		_uiSound.CarGearClutchSound();
	}
	private int g_count = 0;
	public void gearJudgement(float gearPress){
		GearPress[g_count] = gearPress;
		g_count++;
		if(g_count > 19) g_count = 19;
	//	Utility.LogWarning("GearPress " + GearPress[g_count]);
	//	Utility.LogWarning("GearPress1 " +gearPress);
	//	Utility.LogWarning("GearPress2 " +g_count);
	}

	private int g_PressCount = 0;
	public void PressDelayJudgement(double press){
		float fTime = (float)press;
		gPressTime[g_PressCount] = fTime;
		g_PressCount++;
		if(g_PressCount > 19) g_PressCount = 19;
	}

	public void GlobalScoreInit(){

			Global.gGearScore = Base64Manager.instance.GlobalEncoding(0);
			Global.pGearScore = Base64Manager.instance.GlobalEncoding(0);
			Global.pDrillScore = Base64Manager.instance.GlobalEncoding(0);
			Global.gDrillScore = Base64Manager.instance.GlobalEncoding(0);
			Global.myRank = Base64Manager.instance.GlobalEncoding(10,1);
			gDrillCount=0; pDrillCount=0; gGearCount = 0; pGearCount=0;
	}

}