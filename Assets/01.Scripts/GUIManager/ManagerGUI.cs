using UnityEngine;
using System.Collections;

public partial class ManagerGUI : MonoBehaviour {
	void Resolutions()
	{			
		Camera cam = this.transform.Find("Camera").GetComponent<Camera>();
		float width = (float)Screen.width;
		float height = (float)Screen.height;
		int type = 0;
		float x = width/height;
		if( x < 1.4f){
			cam.orthographicSize = 1.2f;
		}else if( x < 1.55f){
			cam.orthographicSize = 1.1f;
		}else if(x < 1.8f){
			cam.orthographicSize = 1.0f;
		}
	}
	void Awake(){
		GV.mException = string.Empty;
		_panel = GameObject.FindGameObjectsWithTag("GUIPanel");
		foreach(GameObject obj in _panel){
			obj.SetActive(false);
		}
		TutorialRaceCheckRoutine();
		Resolutions();
		if(Global.gRaceInfo.sType == SubRaceType.RegularRace) isFlagType = true;
		else isFlagType = false;
		Global.bGearPress = false;
		Global.bAccelPress = false;
		GV.bRaceLose = false;
	}

	private int MGear = 0;private GameObject mGearTextObj;
	void Start () {
		var Car = GameObject.FindWithTag("Wheel") as GameObject;
		_carspeed.FrontLeftWheel = Car.GetComponent<WheelCollider>();
		_carspeed.FrontLeftWheel.motorTorque =10.0f;
		_carspeed.init();
		findPanel("TextObject").SetActive(true);
		findPanel("PitIn").SetActive(true);
		Gear = findPanel("DashBoard").transform.FindChild("Gear").gameObject;
		s_car = GetComponent<CarSound>(); 
		s_effect = GetComponent<EffectSound>();
		s_music = GetComponent<MusicSound>();
		s_ui = GetComponent<UISound>();
		if(!Global.isBoostable) btnn20.gameObject.SetActive(false);
		btnGearClutch.parent.transform.gameObject.SetActive(false);
		isGear = false;
		MGear = Base64Manager.instance.GlobalEncoding(Global.gMaxGear);
		if(!Global.isTutorial){
			SetDurability();
		}
		SetCloseButton();
		mGearTextObj = findPanel("DashBoard").transform.FindChild("Maxgear").gameObject;
		mGearTextObj.transform.FindChild("Text").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("72606"),GV.PlayClassID);
	}

	private void SetDurability(){
		//	int cardu = 0;
		int teamID =0;
		if(Global.gRaceInfo.mType == MainRaceType.Club){
			teamID = CClub.ClubTeamID;
		}else{
			teamID = GV.SelectedTeamID;
		}
		CarInfo carinfo = GV.getTeamCarInfo(teamID);
		if(carinfo.carClass.DurabilityRef == 0){
			Durability.transform.GetComponent<stateUp>().ChangeDashBoardBarDurability(100, 100);
			return;
		}
		
		//	if(carinfo.carClass.Durability <= 0) cardu = 0;
		//	else cardu = carinfo.carClass.Durability;
		Durability.transform.GetComponent<stateUp>().ChangeDashBoardBarDurability((float)GV.PlayDruability,  (float)carinfo.carClass.DurabilityRef);
	}

	void TutorialRaceCheckRoutine(){
		if(Global.isTutorial){
			Tutorial.SetActive(true);
			_tutorial = Tutorial.GetComponent<TutorialRace>();
			if(!Global.isReTutorial) closeBtn.SetActive(false);
			pGear = 10;
			gGear = 5;
			TutorialSetting();
			CameraNumber = gameObject.AddComponent<RandomCreate>().CreateRandomValue(20);
		}else{
			pGear = Base64Manager.instance.GlobalEncoding(Global.gScorePerfect);
			gGear = Base64Manager.instance.GlobalEncoding(Global.gScoreGood);
			RaceModeSet();
		}

		if(Global.gChampTutorial == 1){
			Tutorial.SetActive(true);
			_tutorial = Tutorial.GetComponent<TutorialRace>();
			closeBtn.SetActive(false);
			TutorialSetting();
			CameraNumber = gameObject.AddComponent<RandomCreate>().CreateRandomValue(20);
		}else if(Global.gChampTutorial == 2){
			Tutorial.SetActive(true);
			_tutorial = Tutorial.GetComponent<TutorialRace>();
			_tutorial.disableCrew();
			closeBtn.SetActive(false);
			TutorialSetting();
			CameraNumber = gameObject.AddComponent<RandomCreate>().CreateRandomValue(20);
		}else {
		
		}
		cameracount = 0;
	}

	private void SetCloseButton(){
		switch(Global.gRaceInfo.mType){
		case MainRaceType.PVP:
			closeBtn.SetActive(false);
			break;
		case MainRaceType.Club:
			closeBtn.SetActive(false);
			break;
		default:
			closeBtn.SetActive(true);
			break;
		}
	
	
	}
	void RaceModeSet(){
		int cnt =0;
		/*switch(Global.gRaceInfo.modeType){
		case RaceModeType.DragMode:
			InitDragModeSet();
			cnt = 10;
			break;
		case RaceModeType.StockMode:
			InitStockModeSet();
			cnt = 20;
			break;
		case RaceModeType.TouringMode:
			InitTouringModeSet();
			cnt = 20;
			break;
		case RaceModeType.EventMode:
			cnt = 20;
			InitEventModeSet();
			break;
		default:
			break;
		}*/
		switch(Global.gRaceInfo.sType){
		case SubRaceType.DragRace:
			InitDragModeSet();	
			cnt = 10;
			break;
		case SubRaceType.RegularRace:
			InitStockModeSet();

			cnt = 20;
			break;
		case SubRaceType.CityRace:
			cnt = 20;
			InitEventModeSet();
			break;
		default:
			break;
		}
		CameraNumber = gameObject.AddComponent<RandomCreate>().CreateRandomValue(cnt);

	}

	public GameObject findPanel(string str){
		GameObject temp = null;
		foreach(GameObject obj in _panel){
			if(obj.name == str){
				temp = obj;
				break;
			}
		}
		if(temp == null) Utility.LogError(" return null");
		return temp;
	}


	
	public GameObject FindChildObject(string str){
		GameObject temp = findPanel("TextObject");
		GameObject t = temp.transform.FindChild(str).gameObject;
		temp = null;
		return t;
	}

	public void GearTextShow(string str){
		int curGear = _carspeed.CurrentGear;
		Gear.SendMessage("GearChangeAction",curGear,SendMessageOptions.DontRequireReceiver);
		return;
	}

	public void HiddenText(){
		btnn20.SendMessage("HiddenText",SendMessageOptions.DontRequireReceiver);	
	}

	public void hiddenBtn(Transform tr){
		Hidden_TextCallback(tr);
	}
	
	public void showBtn(Transform tr){
		Show_TextCallback(tr);
	}

	public void playFinishSound(){
	//	if(b)
	//	s_music.playwinSound();
	//	else
	//	s_music.playlossSound();
		s_music.playFlagDrumSound();
	}


	public void OnPauseMessage(){
		isPause  = true;
		torqueTemp= _carspeed.motorInputTorque;
		_carspeed.motorInputTorque =0;
		_carspeed.brakePower = 0;
		_carspeed.CarRace();
		DashboardIndicator();
	}
	
	void OnResumeMessage(){
		isPause = false;
		_carspeed.motorInputTorque =torqueTemp;
		_carspeed.brakePower = 100;
	}
	// Update is called once per frame
	void FixedUpdate () {
		if(isPause) return;
		if(_isRPMCheck){
			checkTime += Time.deltaTime;
		}
		_carspeed.CarRace();
		DashboardIndicator();
		if(	GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01 ||
				GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02){
			StartCoroutine(EngineLedBlink());
			//return;
		}
	}

#if UNITY_EDITOR
	void LateUpdate(){
		if (Input.GetKey(KeyCode.Space)){
			n2oPress();
		}
		if(Input.GetKeyUp(KeyCode.A)){ //clutch
			OnClutchRelease();
		}

		if(Input.GetKeyDown(KeyCode.A)){
			OnClutchPress();
		}

		if(Input.GetKeyUp(KeyCode.S)){ // accel
			OnReleased();
		}
		if(Input.GetKeyDown(KeyCode.S)){
			OnPressed();
		}

		if(Input.GetKeyUp(KeyCode.D)){ //gear change
		}
		if(Input.GetKeyDown(KeyCode.D)){
			OnClutchClick();
		}
	}
#endif
	private void DashboardIndicator(){
		_speed = -_carspeed.currentSpeed*(16.2f/24.4f)*UserDataManager.instance.delatSpeed;
		float zAngle = Mathf.SmoothDampAngle(arrowSpeed.eulerAngles.z, _speed+100.0f, ref yVelocity, smooth, Mathf.Infinity, Time.deltaTime);
		if(zAngle >180) zAngle-=360;
		if(zAngle < -90) zAngle = -90-Random.Range(0,5);

		Vector3 position = new Vector3(0,0,zAngle);//target.position;
		arrowSpeed.localRotation = Quaternion.Euler(position);

		//_rpm = -_carspeed.EngineRPM*(170f/3000f) ;
		_rpm = _carspeed.EngineRPM;
		if(_rpm > 3600){
			_rpm = 3580;
		}
		_rpm = -_rpm*(170f/3000f);
		zAngle = Mathf.SmoothDampAngle(arrowRPM.eulerAngles.z, _rpm+100.0f, ref yVelocity, smooth, Mathf.Infinity, Time.deltaTime);

		position = new Vector3(0,0,zAngle);//target.position
		arrowRPM.localRotation = Quaternion.Euler(position);
	}

	public void  EngineLedOff(){
		StopCoroutine(strRpmLedCheck);
		foreach(GameObject obj in gearled){
			obj.SetActive(false);
		}
		gearLedBlinkOff();
		if(!GameManager.instance.isEnablePress) GameManager.instance.isEnablePress = true;
		return;
	}

	public void  EngineSoundOff(){
		StopCoroutine(strRpmLedCheck);
	}
	
	IEnumerator EngineLedBlink(){
		EngineLEDCallback();
		yield return null;
	}


	IEnumerator AIAutoStartRoutine(float delay){
		//yield return new WaitForSeconds(delay*3.0f);
		GameManager.instance.AIRaceStart();
		yield return null;
	}

	IEnumerator FirstTouchCheckRoutine(float delay){
		yield return new WaitForSeconds(delay*6.0f);
		if(Global.gRaceInfo.sType == SubRaceType.RegularRace){
			if(isFirstPress) {
				yield return null;
			}else{
				GameManager.instance.HiddenAutoSignalFlag();
				GameManager.instance._isPress = false;

			}

		}
	}


	IEnumerator rpmLedCheck(int gear){
		float delay = Base64Manager.instance.getFloatEncoding(Global.gLedTime[gear],0.001f);

		//delay = 0.5f;
		if(isRace01){
			s_music.CarAirHornSound();
			StartCoroutine("AIAutoStartRoutine", delay);
			StartCoroutine("FirstTouchCheckRoutine", delay);
			isRace01 = false;
		}
		bool isledon = false;
		bool isMaxGear = (MGear == gear) ? false: true;
		_isRPMCheck = true;
		checkTime = 0.0f;
		earlyled = 0;

		while(true){
			if(!isMaxGear){
				GameManager.instance.isEnablePress  = false;
				yield return new WaitForSeconds(delay);
			}else{
				if(!isledon){
					earlyled = 1;
					gearled[0].SetActive(true);
					gearLedBlinkOff();
					gearLedBlinkOn(1);
					GameManager.instance.isEnablePress  = false;
					s_ui.LedRedPlay();

					yield return new WaitForSeconds(delay);
					earlyled= 2;
					gearled[1].SetActive(true);
					gearLedBlinkOff();
					gearLedBlinkOn(2);
					s_ui.LedRedPlay();

					yield return new WaitForSeconds(delay);
					earlyled= 3;
					gearled[2].SetActive(true);
					gearLedBlinkOff();
					gearLedBlinkOn(3);
					s_ui.LedRedPlay();
					yield return new WaitForSeconds(delay);
						gearled[3].SetActive(true);
						s_ui.LedBluePlay();
						showBtn(btnclutch);
						showBtn(btnGearClutch);
					 	delayCheckTime = checkTime;
						yield return new WaitForSeconds(0.1f);
						yield return new WaitForSeconds(0.1f);
						isledon =true;
				//	}
				}else{
					yield return new WaitForSeconds(0.05f);
					gearled[3].SetActive(false);
					yield return new WaitForSeconds(0.05f);
					gearled[3].SetActive(true);
					s_ui.LedBluePlay();
				}
				yield return null;
			}
		}
	}

	private void ResetEngineLED(){
		if(_carspeed.EngineRPM > _carspeed.CheckEngineRPM ){
			hiddenBtn(btnclutch);
			hiddenBtn(btnGearClutch);
			EngineLedOff();
			isrpmcheck = false;
			checkTime = 0.0f;
		}
	}

	public IEnumerator StartRace02Count(){
		_carspeed.chagneGear2();
		GameManager.instance.isEnablePress = true;
		isMaxGear = false;	
		StartCoroutine(strRpmLedCheck,0);
		float temp = Base64Manager.instance.getFloatEncoding(Global.gLedTime[0],0.001f) *2;
	//	temp = 0.5f;
	//	Utility.LogWarning("Test Routine");
		yield return new WaitForSeconds(temp);
		isRace02StartLED = true;
	}

	public IEnumerator StartRace01Count(){
		GameManager.instance.isEnablePress = true;
		isMaxGear = false;

		yield return null;
	}
	

	void speedIndicatorTween(UISprite _sprite, string str){
		//var speedo = findPanel("DashBoard").transform.FindChild("board").gameObject;
		//var speedocolor = speedo.transform.FindChild("Dashboard_Speed").gameObject;
		var speedocolor = _sprite.transform.gameObject as GameObject;
		TweenColor[] speedback = speedocolor.GetComponents<TweenColor>() ;
		if(str == "Color"){
			float bstime = Base64Manager.instance.getFloatEncoding(Global.gBsTime, 0.001f);
			speedback[0].duration = bstime;
			speedback[0].Reset();
			//speedback[0].ResetToBeginning();
			speedback[0].enabled = true;
			speedback[1].delay = bstime;
			speedback[1].duration =0.3f;
			speedback[1].Reset();
			speedback[1].enabled = true;
		}else if(str == "Color2"){
			speedback[2].Reset();
			speedback[2].enabled = true;
			speedback[3].Reset();
			speedback[3].enabled = true;
		}else if(str == "scale"){
			TweenScale[] tween  = speedocolor.GetComponents<TweenScale>();
			tween[0].Reset();
			tween[0].enabled = true;
			StartCoroutine(delayTweenTime(tween[0]));
		}else if(str == "accelpress"){
		
		}
		speedocolor = null;
	}
	
	IEnumerator delayTweenTime(TweenScale tScale){
		yield return new WaitForSeconds(0.3f);
		tScale.enabled =false;
		tScale.transform.localScale = new Vector3(266,199,1); 
	}

	public void setCurrentGear(int gear, float delay){
		_carspeed.CurrentGear = gear;
		if(isPress){
			OnPressed();
		}
		StartCoroutine(AccelSpeedDelay(delay));
	}

	public void RandomSwitchCamera(){
		string str = "Cam_R_0";//+CameraNumber[cameracount].ToString();
		if(Global.gRaceInfo.sType != SubRaceType.RegularRace){//
			int n = GameManager.instance.DragCamearNumber(0);
			str = "Cam_R_" + n.ToString();
			GameManager.instance.SwitchingCamera(str);
		}else{
			GameManager.instance.SwitchingCamera(str);
		}
		GameManager.instance.ChangeSwage(_carspeed.CurrentGear);

	}
	private bool isFlagType;
	public void JudgeRPM(string str, float delay){
		GameObject temp = findPanel("TextObject");
		GameObject t = temp.transform.FindChild(str).gameObject;
		t.SetActive(true);
		raceLED.SetActive(true);
		if(isFlagType){
			t.SendMessage("TweenScaleStartRegular",SendMessageOptions.DontRequireReceiver);
			raceLED.SendMessage("ShowRaceLEDRegular",str, SendMessageOptions.DontRequireReceiver);
		}else{
			t.SendMessage("TweenScaleStart",SendMessageOptions.DontRequireReceiver);
			raceLED.SendMessage("ShowRaceLED",str, SendMessageOptions.DontRequireReceiver);
			isFlagType = true;		
		}
		rewardText.SetActive(true);
		rewardText.SendMessage("RewardAction",str,SendMessageOptions.DontRequireReceiver);
		//Utility.LogWarning("JudgeRPM  ");
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02 ||
		   GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01){
			StartCoroutine(AccelSpeedDelay(delay));
			ResetEngineLED();
		}
		GameManager.instance.gearJudgement(delay);
		rpmjudgefinal = str;
		temp =  t = null;
	}
	
	IEnumerator AccelSpeedDelay(float delay){
		_carspeed.motorInputTorque = Base64Manager.instance.getFloatEncoding(Global.gTorque,0.001f)*0.1f;
		yield return new WaitForSeconds(delay);
		_carspeed.motorInputTorque = Base64Manager.instance.getFloatEncoding(Global.gTorque,0.001f);
		if(delay > 0.1f){
			GameManager.instance.Vibrategame(100);
		}
		if(isPress){
			s_ui.CarAccelTurchSound();
			speedIndicatorTween(dash_Speed,"scale");
			speedIndicatorTween(dash_Speed,"Color2");
		}
	}

	public void AccelPressCameraChangeDrag(){
		if(!isclutchpress){
			hiddenBtn(btnaccel);
			s_car.CarEngineVolumeDelay();
			s_effect.CarAccelGoodSound();
			return;
		}
		if(isMaxGear){
			hiddenBtn(btnaccel);
			isclutchpress  = false;
			return;
		}
		int num = _carspeed.CurrentGear; 
		int n = 0;
		string str= null;
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01 )
		{
			if(cameracount >= CameraNumber.Length) cameracount = Random.Range(0,10);
			n = CameraNumber[cameracount];
			n=GameManager.instance.DragCamearNumber(n);
			if(n == 0) {
				cameracount++;
				n=CameraNumber[cameracount];
			}
			str = "Cam_R_"+n.ToString();
			cameracount++;
			GameManager.instance.SwitchingCamera(str);
			GameManager.instance.ChangeSwage(num);
		}
		
		if(rpmjudgefinal == "Perfect"){
			s_effect.CarAccelPerfectSound();
			
		}else{
			s_effect.CarAccelGoodSound();
		}
		GameManager.instance.StartCoroutine("ZoomFOVCamera",rpmjudgefinal);
		hiddenBtn(btnaccel);
		isclutchpress  = false;
	}

	public void AccelPressCameraChangeEvent(){
		if(!isclutchpress){
			hiddenBtn(btnaccel);
			s_car.CarEngineVolumeDelay();
			s_effect.CarAccelGoodSound();
			return;
		}
		if(isMaxGear){
			hiddenBtn(btnaccel);
			isclutchpress  = false;
			return;
		}
		int num = _carspeed.CurrentGear; 
		int n = 0;
		string str= null;
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02 || GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01 )
		{
			if(cameracount >= CameraNumber.Length) cameracount = Random.Range(1,18);
			n = CameraNumber[cameracount];
			n=GameManager.instance.DragCamearNumber(n);
			if(n == 0) {
				cameracount++;
				n=CameraNumber[cameracount];
			}
			//Utility.LogWarning("caemrcount " + cameracount);
			str = "Cam_R_"+n.ToString();
			cameracount++;
			GameManager.instance.SwitchingCamera(str);
			GameManager.instance.ChangeSwage(num);
		}
		
		if(rpmjudgefinal == "Perfect"){
			s_effect.CarAccelPerfectSound();
			
		}else{
			s_effect.CarAccelGoodSound();
		}
		GameManager.instance.StartCoroutine("ZoomFOVCamera",rpmjudgefinal);
		hiddenBtn(btnaccel);
		isclutchpress  = false;
	}
	public void AccelPressCameraChagne(){
		if(!isclutchpress){
			hiddenBtn(btnaccel);
			s_car.CarEngineVolumeDelay();
			s_effect.CarAccelGoodSound();
			return;
		}
		if(isMaxGear){
			hiddenBtn(btnaccel);
			isclutchpress  = false;
			return;
		}
		int num = _carspeed.CurrentGear; 
		int n = 0;
		string str= null;
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02 || GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01 )
		{
			if(cameracount >= CameraNumber.Length) cameracount = Random.Range(1,17);
			n = CameraNumber[cameracount];
			if(n == 0) {
				cameracount++;
				n=CameraNumber[cameracount];
			}
			str = "Cam_R_"+n.ToString();
			cameracount++;
			GameManager.instance.SwitchingCamera(str);
			GameManager.instance.ChangeSwage(num);
		}

		if(rpmjudgefinal == "Perfect"){
			s_effect.CarAccelPerfectSound();
			
		}else{
			s_effect.CarAccelGoodSound();
		}
		GameManager.instance.StartCoroutine("ZoomFOVCamera",rpmjudgefinal);
		hiddenBtn(btnaccel);
		isclutchpress  = false;
	}



	void PressTimeCompare(float pressTime){
		PressTimeCheckCallback(pressTime);
	}

	void DragTimeCompare(float pressTime){
		float m_pressTime = pressTime;
			float a1=0,a2=0;//,f1=0;
			a1 = Global.gP_MIN;
			a2 = Global.gP_MAX;
			s_car.CarEngineVolumeDelay();
		if(dragCheck >6  && dragCheck <=9){
			JudgeRPM("Good", Global.gR_Good);
			Effect[1].SetActive(true);
			Effect[3].SetActive(true);
			int temp = Base64Manager.instance.GlobalEncoding(Global.gGearScore);
			temp += gGear;
			Global.gGearScore = Base64Manager.instance.GlobalEncoding(temp);
			s_ui.CarGearShiftETCSound();
			GameManager.instance.gGearCount++;
		}else if(dragCheck <= 6){
			float mDelay =  Global.gR_Early+(7-dragCheck)*0.1f;
			JudgeRPM("Early", mDelay);
			Effect[1].SetActive(true);
			Effect[3].SetActive(true);
			s_ui.CarGearShiftETCSound();
		}else if(dragCheck > 9 && dragCheck >=12){
			GameManager.instance.Vibrategame(400);
			JudgeRPM("Perfect",Global.gR_Perfect);
			Effect[0].SetActive(true);
			Effect[2].SetActive(true);
			int temp1 = Base64Manager.instance.GlobalEncoding(Global.pGearScore);
			temp1 += pGear;
			Global.pGearScore = Base64Manager.instance.GlobalEncoding(temp1);
			s_ui.CarGearShiftSound();
			GameManager.instance.PCount++;
			GameManager.instance.pGearCount++;
		}else{
			JudgeRPM("Late",Global.gR_Late);
			Effect[1].SetActive(true);
			Effect[3].SetActive(true);
			s_ui.CarGearShiftETCSound();
		}
		//Utility.LogWarning("Check " + dragCheck + " " +rpmjudgefinal);
			GearTextShow("Gear");
			delayCheckTime = 10.0f;
		if(_carspeed.CurrentGear !=9){
			hiddenBtn(btnclutch);
			showBtn(btnaccel);
		}
	}

	void PressTimeCompareRace(float pressTime){

		float m_pressTime = pressTime;
		if(_carspeed.CurrentGear > 9){
			_carspeed.CurrentGear = 9;
		}else{
			float a1=0,a2=0;//,f1=0;
			a1 = Global.gP_MIN;
			a2 = Global.gP_MAX;
			s_car.CarEngineVolumeDelay();
			if(m_pressTime < (delayCheckTime+ a2) && m_pressTime >= (delayCheckTime+a1))
			{
				GameManager.instance.Vibrategame(400);
				JudgeRPM("Perfect",Global.gR_Perfect);
				Effect[0].SetActive(true);
				Effect[2].SetActive(true);
				int temp1 = Base64Manager.instance.GlobalEncoding(Global.pGearScore);
				temp1 += pGear;
			//	Utility.LogWarning("Perfect Gear " + pGear);
			//	Utility.LogWarning("Perfect Gear t " + temp1);
				Global.pGearScore = Base64Manager.instance.GlobalEncoding(temp1);
				s_ui.CarGearShiftSound();
				GameManager.instance.PCount++;
				GameManager.instance.pGearCount++;
			}else if(m_pressTime >= (delayCheckTime+a2)){
				JudgeRPM("Late",Global.gR_Late);
				Effect[1].SetActive(true);
				Effect[3].SetActive(true);
				s_ui.CarGearShiftETCSound();
			}else if(m_pressTime < (delayCheckTime+a1) && m_pressTime > (delayCheckTime)){
				JudgeRPM("Good", Global.gR_Good);
				Effect[1].SetActive(true);
				Effect[3].SetActive(true);
				int temp = Base64Manager.instance.GlobalEncoding(Global.gGearScore);
				temp += gGear;
				Global.gGearScore = Base64Manager.instance.GlobalEncoding(temp);
				s_ui.CarGearShiftETCSound();
				GameManager.instance.gGearCount++;
			}else if(m_pressTime <= (delayCheckTime)){
				float mDelay =  Global.gR_Early;
				if(Global.gChampTutorial == 0){
					if(earlyled == 0){
						mDelay+= 2.0f;
					}else if(earlyled == 1){
						mDelay+= 1.5f;
					}else if(earlyled==2){
						mDelay+= 1.1f;
					}else if(earlyled ==3){
						mDelay+= 0.5f;
					}else if(earlyled ==4){
						mDelay+= 0.2f;
					}
				}
				//Utility.LogWarning("CHECK " + mDelay + "  " + earlyled);
				JudgeRPM("Early", mDelay);
				Effect[1].SetActive(true);
				Effect[3].SetActive(true);
				s_ui.CarGearShiftETCSound();
			}
			GearTextShow("Gear");
			delayCheckTime = 10.0f;
		}
		if(_carspeed.CurrentGear !=9){
			hiddenBtn(btnclutch);
			showBtn(btnaccel);
		}
	}
	private int earlyled =0;
	void OnDestroy(){
		_panel = null;
		arrowSpeed= null;
		arrowRPM= null;
		s_car= null;
		s_music= null;
		s_ui= null;
		s_effect= null;
		Gear= null;
		btnn20= null;
		btnclutch= null;
		btnGearClutch = null;
		btnaccel= null;
		gearled= null;
		System.GC.Collect();
	}
	public void MaxGearTextAction(bool b){
		if(b){
			if(mGearTextObj.activeSelf) return;
			var tw = mGearTextObj.GetComponent<TweenPosition>() as TweenPosition;
			tw.Reset();
			tw.enabled = true;
			mGearTextObj.SetActive(true);
		}else{
			if(!mGearTextObj.activeSelf) return;
			mGearTextObj.SetActive(false);
		}
	}

	public void FinishEffect(){
		var root = transform.GetChild(0).GetChild(0).FindChild("effect").gameObject as GameObject;
		var car = Resources.Load("Effect_N/Finishline_Pass", typeof(GameObject)) as GameObject;
		var race = Instantiate(car) as GameObject;
		race.transform.parent = root.transform;
		race.transform.localPosition = Vector3.zero;
		//Utility.LogWarning("FinishEffect");
	}

	public void DisConnectPopup(){
		ePopUp.SetActive(true);
		ePopUp.GetComponent<disconnectPopup>().InitRacePopup();
	}
	public GameObject ePopUp;
}


