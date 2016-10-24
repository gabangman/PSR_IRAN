using UnityEngine;
using System.Collections;

public class PitPlay : MonoBehaviour {

	public GameObject Wheel;
	public GameObject[] bolt = new GameObject[5];
	public GameObject[] PitState = new GameObject[4];
	public GameObject bolt_line, bolt_alert,bolt_effect;
	public GameObject Drill;
	public GameObject Pitcrew;
	public GameObject fuel, pitLED, rewardText;
	public bool isPitIning = false;
	public bool isPitInEnd = false;
	private EffectSound _effect;
	private UISound _uisound;
	public enum PitAppear  { WheelAppear, WheelDisAppear, DrillAppear, DrillDisAppear};
	private bool isAniCam = true;
	public int _pitcnt = 0;
	public float _PitTime;
	private float boltJudgeDelay=0;
	private int pG, gG;
	private int tpG, tgG;
	void OnEnable(){
		bolt_line.SetActive(false);
		_effect = GetComponent<EffectSound>();
		_uisound = GetComponent<UISound>();
		if(Global.isTutorial){
			pG = 10;
			gG = 5;
		}else{
			pG = Base64Manager.instance.GlobalEncoding(Global.gScorePerfect);
			gG = Base64Manager.instance.GlobalEncoding(Global.gScoreGood);
		}


	}

	void Start(){
		Wheel.SetActive(true);
		Pitcrew.SetActive(true);
	}

	void OnDisable(){
		Wheel.SetActive(false);
		Pitcrew.SetActive(false);

	}
	private ManagerGUI mGUI;
	public void StartPitIn(){
		if(Global.isTutorial){
			mGUI=	transform.GetComponent<ManagerGUI>();
			PitCallBack = (count) =>{
				mGUI.ShowDrillText(count);
			};
			DrillbuttonTextHidden = ()=>{
				Drill.transform.GetChild(4).gameObject.SendMessage("hiddenbtnText",SendMessageOptions.DontRequireReceiver);
				PitCallBack(_pitcnt);
			};
			DrillbuttonTextShow = () =>{
				Drill.transform.GetChild(4).gameObject.SendMessage("showbtnText",SendMessageOptions.DontRequireReceiver);
			};
		}else{
			mGUI = null;
			DrillbuttonTextHidden = ()=>{
				if(_pitcnt == 0){
					Drill.transform.GetChild(4).gameObject.SendMessage("hiddenbtnText",SendMessageOptions.DontRequireReceiver);
				}
			};
			DrillbuttonTextShow = () =>{
			
			};
		}

		if(Global.gChampTutorial == 0){
			//mGUI = null;
		}else{
			mGUI=	transform.GetComponent<ManagerGUI>();
			PitCallBack = (count) =>{
				mGUI.ShowDrillText(count);
				
			};
			DrillbuttonTextHidden = ()=>{
				Drill.transform.GetChild(4).gameObject.SendMessage("hiddenbtnText",SendMessageOptions.DontRequireReceiver);
				PitCallBack(_pitcnt);
			};
			DrillbuttonTextShow = () =>{
				Drill.transform.GetChild(4).gameObject.SendMessage("showbtnText",SendMessageOptions.DontRequireReceiver);
			};
		}


		StartCoroutine("Startpit01");
		GameManager.instance.RaceState = GameManager.GAMESTATE.PITINING;
	}
	
	public void pitStateSet(bool _b){
		for(int j= 0 ; j<PitState.Length;j++){
			PitState[j].SetActive(_b);
		}
	}
	bool isPause = false;
	void OnPauseMessage(){
		//==!!Utility.LogWarning("onPause1");
		isPause  = true;
	}
	
	void OnResumeMessage(){
		isPause = false;
	}
	// Update is called once per frame
	void Update () {
		if(isPause) return;
		if(isPitIning){
			_PitTime += Time.deltaTime;
		}else {
			return;
		}
	}

	private void textTween(int num){
		PitState[num].SetActive(true);
		PitState[num].SendMessage("TweenScaleStartRegular", SendMessageOptions.DontRequireReceiver);
		pitLED.SetActive(true);
		pitLED.SendMessage("ShowPitLED", num, SendMessageOptions.DontRequireReceiver);
		rewardText.SetActive(true);
		rewardText.SendMessage("rewardPitAction", num, SendMessageOptions.DontRequireReceiver);
	}

	private void boltAddTweenRotation(){
		bolt[5].SetActive(true);
		bolt[5].transform.position = bolt[_pitcnt].transform.position;
		TweenRotation _tt;
		for(int i = 0; i < 4; i++){
			_tt = bolt[_pitcnt].AddComponent<TweenRotation>();// as TweenPosition;
		_tt.style = UITweener.Style.Once;
		_tt.method = UITweener.Method.Linear;
		_tt.from = new Vector3(0,0,0);
		_tt.to = new Vector3(0,0,180);
		_tt.duration = 0.1f;
		_tt.delay = i*0.1f;
		_tt.enabled = true;
		}
		
		 _tt = bolt[_pitcnt].AddComponent<TweenRotation>();// as TweenPosition;
		_tt.style = UITweener.Style.Once;
		_tt.method = UITweener.Method.Linear;
		_tt.from = new Vector3(0,0,0);
		_tt.to = new Vector3(0,0,180);
		_tt.duration = 0.1f;
		_tt.delay = 4*0.1f;
		_tt.eventReceiver = gameObject;
		_tt.callWhenFinished = "boltout";
		_tt.enabled = true;
		
		//bolt[5].SetActive(false);
		for(int i = 5; i < 8; i++){
			_tt = bolt[_pitcnt].AddComponent<TweenRotation>();// as TweenPosition;
			_tt.style = UITweener.Style.Once;
			_tt.method = UITweener.Method.Linear;
			_tt.from = new Vector3(0,0,0);
			_tt.to = new Vector3(0,0,180);
			_tt.duration = 0.1f;
			_tt.delay = i*0.1f;
			_tt.enabled = true;
		}
	}

	void boltout(){
		bolt[5].SetActive(false);
	}

	public void OnDrillClick(){
		if(!isPitIning) return;
			DrillbuttonTextHidden();
		if(!PitInTimeCheck()) return;
		TweenScale boltlineTween = bolt_line.GetComponent<TweenScale>() as TweenScale;
			boltlineTween.style = UITweener.Style.Once;
			boltlineTween.enabled =false;
			bolt_line.SetActive(false);
			bolt_alert.SetActive(false);
			StopCoroutine("ScrewWhiteSoundRepeat");
			WheelBounce();
			boltAddTweenRotation();
			boltColorChange(_pitcnt);
			_pitcnt += 1;
			isPitIning = false;
			if(_pitcnt == 5){
				_pitcnt = 0;
				StartCoroutine(delayCount());
				bolt_line.SetActive(false);
				isPitIning = false;
				_PitTime=0.0f;
				if(isAniCam){
					isAniCam = false;
					tweenTime = 0.5f;
					StartCoroutine(StartPit02());
				}else{
					GameManager.instance.CameraAniStart(1);
					tweenTime = 0.5f;
					if(mGUI != null) mGUI.speechActive(false);
					StartCoroutine(StartRace02());
				}
				
			}else{
				_PitTime=0.0f;
				bolt_line.transform.position = bolt[_pitcnt].transform.position;
				bolt_alert.transform.position =  bolt[_pitcnt].transform.position;

				DrillbuttonTextShow();
				StartCoroutine(boltwaitTime(boltJudgeDelay));
			}
	}
	
	IEnumerator delayCount(){
		//Utility.Log ("pitin-ing and pitin-end");
		yield return new WaitForSeconds(1.5f);
		pitStateSet(false);
		
	}
	System.Action<int> PitCallBack;
	System.Action DrillbuttonTextShow;
	System.Action DrillbuttonTextHidden;
	IEnumerator boltwaitTime(float delay){
		yield return new WaitForSeconds(delay);
		isPitIning = true;
		boltAddTween();
		bolt_line.SetActive(true);
		yield return new WaitForSeconds(0.5f);
	}

	void CameraPitPlay(){
		
		if(mGUI == null) return;
		else mGUI.CameraPitIn();
	}


	public IEnumerator SpeecherOut(){
		var obj = Pitcrew.transform.FindChild("Speech").gameObject as GameObject;
		obj.transform.GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("73049");
		obj.GetComponent<TweenAction>().doubleTweenScale(obj);
		yield return new WaitForSeconds(1.0f);
		endTween(Pitcrew, new Vector3(-550,0,0), Vector3.zero);
		if(this.enabled){
		}else{
			obj.SetActive(false);			
		}

	}
	IEnumerator SpeechFinish(){
		yield return new WaitForSeconds(0.1f);
		}

	IEnumerator StartRace02(){
		GameManager.instance._isPress = false;

		float fdelay = Base64Manager.instance.getFloatEncoding(Global.gPitCameraDelay, 0.001f);
		yield return new WaitForSeconds(fdelay); // 
		endTween(Wheel, new Vector3(-980,0,0), Vector3.zero);
		endTween(Drill, new Vector3(0,-450,0), Vector3.zero);
		//endTween(Pitcrew, new Vector3(-400,0,0), Vector3.zero);
		if(!GameManager.instance.isFilledGas){
			for(;;){
				if(GameManager.instance.isFilledGas){
					//GameManager.instance.PlayMyCrewAnimation(4);
					//EffectFuel();
					break;
				}
				yield return  null;
			}
		}
		GameManager.instance.PlayMyCrewAnimation(4);
		yield return new WaitForSeconds(boltJudgeDelay);
		if(mGUI != null) {
			mGUI.TutorialRace02Setup();
			mGUI.CameraPitIn();
		}
		GameManager.instance.RaceState = GameManager.GAMESTATE.PITINEND;
		GameManager.instance.GasGauageOut();
		GameManager.instance.GUIDashBoardStart();
		
		yield return new WaitForSeconds(0.3f);
		yield return new WaitForSeconds(0.5f);
		GameManager.instance.Race02StartCount();
		GameManager.instance.GameRaceStateChange("Race02Start");
		yield return new WaitForSeconds(1.0f);
		PitStateInit(false);
	}
	public void EffectFuel(){
		fuel.transform.FindChild("Gas_effect").GetChild(0).gameObject.SetActive(true);
	}

	IEnumerator MyCarRace02Ready(){
		//Utility.Log ("Total screw delay + " + totaldelay);
		yield return new WaitForSeconds(totaldelay);
		GameManager.instance.Race02StartCount();
	}

	private void PitStateInit(bool _b){
		float pitpos;
		for(int i = 0 ; i < 4; i++){
			if(_b) pitpos = -450f;
			else pitpos = -300f;
			PitState[i].transform.localPosition = new Vector3(0,pitpos,0);
		} 	
	}


	IEnumerator Startpit01(){
			
		//	if(Global.gRaceInfo.raceType ==  RaceType.DailyMode) {
		//		 tempid = Base64Manager.instance.GlobalEncoding(Global.gDailyCrewID);
		//	}else {
		//		tempid = Base64Manager.instance.GlobalEncoding(Global.MyCrewID);
		//	}
		//	tempid = GV.PlayCrewID;
		//	str = tempid.ToString()+"A";

			addTween(Wheel, new Vector3(-980,0,0), Vector3.zero);
			addTween(Drill, new Vector3(0,-450,0), Vector3.zero);
			addTween(fuel, new Vector3(0,-500,0), Vector3.zero);
			PitStateInit(true);
			StartCoroutine(CrewdelayTime (2.0f));	
			yield return new WaitForSeconds(2.0f);
			GameManager.instance.PlayMyCrewAnimation(2);	
			
			GameManager.instance.GasGague("Fill");
	}

	public IEnumerator BoltStart(float delay){
		yield return new WaitForSeconds(delay);
		GameManager.instance.CameraPitin("Cam_Pit_Play_1");
		yield return new WaitForSeconds(0.2f);
		WheelBoltStart();
	}

	public void PitcrewActive(){
		string str = string.Empty;
		int tempid = 0;
		tempid = GV.PlayCrewID;
		str = tempid.ToString()+"A";
		Pitcrew.transform.FindChild("chiefWait").GetComponent<UISprite>().spriteName =str;
		addTween(Pitcrew, new Vector3(-550,0,0), Vector3.zero);
		StartCoroutine("SpeechStart");
		return;
	}

	IEnumerator SpeechStart(){
		yield return new WaitForSeconds(tweenTime);
		var obj = Pitcrew.transform.FindChild("Speech").gameObject as GameObject;
		obj.gameObject.SetActive(true);
		obj.GetComponent<TweenAction>().doubleTweenScale(obj);
		obj.transform.GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("73047");
	}

	IEnumerator CrewdelayTime(float delay){
		yield return new WaitForSeconds(delay);
		var obj = Pitcrew.transform.FindChild("Speech").gameObject as GameObject;
		obj.gameObject.SetActive(true);
		obj.GetComponent<TweenAction>().doubleTweenScale(obj);
		obj.transform.GetComponentInChildren<UILabel>().text =KoStorage.GetKorString("73048");
	}

	void boltColorChange(int _idx){
		var _color = bolt[_idx].GetComponent<TweenColor>()  as TweenColor;
		Color c = _color.from;
		_color.from = _color.to;
		_color.to = c;
		_color.Reset();
		_color.enabled=true;
		//Utility.Log("boltColor " + _color.to + " f " + _color.from );
	}
	
	IEnumerator StartPit02(){
		Global.bGearPress = false;
		Global.bAccelPress = false;
		//카메라 애니메이션 속도와 동일하게 해야 함.
		GameManager.instance.CameraAniStart(0);
		GameManager.instance.PlayMyCrewAnimation(3);
		endTween(Wheel, new Vector3(-980,0,0), Vector3.zero);
	
		float fdelay = Base64Manager.instance.getFloatEncoding(Global.gPitCameraDelay, 0.001f);
		yield return new WaitForSeconds(fdelay);
		yield return new WaitForSeconds(boltJudgeDelay);
		yield return new WaitForSeconds(0.5f);
		for(int i = 0; i < bolt.Length-1; i++){
			boltColorChange(i);
		}
		addTween(Wheel, new Vector3(980,0,0), Vector3.zero);
		yield return new WaitForSeconds(1.0f);
		GameManager.instance.PlayMyCrewAnimation(6);
	//	//==!!Utility.LogWarning(" time : " + GameManager.instance._totalTime + "  anistep 6 : Startpit02");
		WheelBoltStart();
	}

	public void WheelBounce(){
		StartCoroutine(WheelBounceStart());
	}
	
	IEnumerator  WheelBounceStart(){
		var wheel = Wheel.transform.FindChild("Wheel_BG").gameObject;
		var wheelscale = wheel.GetComponent<TweenScale>() as TweenScale;
		wheelscale.Reset();
		wheelscale.enabled = true;
		yield return new WaitForSeconds(0.5f);
		wheelscale.enabled =false;
		wheel.transform.localScale = Vector3.one;
	}

	public void WheelBoltStart(){
		StartCoroutine(boltwaitTime(boltJudgeDelay));
		bolt_line.transform.position = bolt[_pitcnt].transform.position;
		bolt_alert.transform.position = bolt[_pitcnt].transform.position;

		Drill.transform.GetChild(4).gameObject.SendMessage("showbtnText",SendMessageOptions.DontRequireReceiver);
	}


	IEnumerator ScrewWhiteSoundRepeat(){
		yield return new WaitForSeconds(0.2f);
		while(true){
			yield return new WaitForSeconds(0.05f);
			bolt_alert.SetActive(false);
			yield return new WaitForSeconds(0.05f);
			//_uisound.ScrewWhitePlay();
			bolt_alert.SetActive(true);
		}
		
		//yield return null;
	}
	
	
	public void BoltSet(){
		//boltActive(true, _pitcnt,true);
		if(bolt_alert.activeSelf){
			bolt_alert.SetActive(false);
		}
		bolt_alert.SetActive(true);
		StartCoroutine("ScrewWhiteSoundRepeat");
		TweenScale boltlineTween = bolt_line.GetComponent<TweenScale>() as TweenScale;
		boltlineTween.style = UITweener.Style.Once;
		boltlineTween.enabled =false;
		bolt_line.SetActive(false);

	}
	float totaldelay = 0.0f;

	public bool PitInTimeCheck(){
		float _time = _PitTime;
		int num =0;
		float min = Global.Screw_MIN;
		float max = Global.Screw_MAX;
		bool b = false;
		if( _time >= (delTime+(min-1)-0.3f) && _time <  (delTime+( min-1)-0.1f)){
			PitState[0].SetActive(true);_PitTime=0.0f;
			num = 0;
			boltJudgeDelay = 1.0f;
			totaldelay += boltJudgeDelay;
			_effect.BoltScewSound();
			b = true;
		}else if(_time >= (delTime+( min-1)-0.1f) && _time <(delTime+( min-1))){
			PitState[1].SetActive(true);
			num = 1;
			boltJudgeDelay = 0.5f;
			totaldelay += boltJudgeDelay;
			int tmpG = Base64Manager.instance.GlobalEncoding(Global.gDrillScore); //good
			tmpG += gG;
			Global.gDrillScore = Base64Manager.instance.GlobalEncoding(tmpG);
			_effect.BoltScewSound();
			GameManager.instance.gDrillCount++;
			b = true;
		}else if(_time >=(delTime+( min-1)) && _time <(delTime+max)){
			PitState[2].SetActive(true);
			num = 2;
			boltJudgeDelay = 0;
			int tmpG = Base64Manager.instance.GlobalEncoding(Global.pDrillScore); //perfect
			tmpG += pG;
			Global.pDrillScore = Base64Manager.instance.GlobalEncoding(tmpG);
			GameManager.instance.pDrillCount++;
			GameManager.instance.Vibrategame(400);
			bolt_effect.transform.position =bolt_line.transform.position;
			bolt_effect.SetActive(true);
			_effect.BoltScewPerfectSound();
			b = true;
		}else if(_time >=(delTime+max)){
			PitState[3].SetActive(true);
			num = 3;
			boltJudgeDelay = 1.0f;
			totaldelay += boltJudgeDelay;
			_effect.BoltScewSound();
			b = true;
		}
		if(b) textTween(num);
		////==!!Utility.LogWarning("PitInTimeCheck() " + b + " time " + _time + " deltime :" + delTime);
		return b;
	}	


	/*
	public bool PitInTimeCheck(){
		float _time = _PitTime;
		int num =0;
		bool b = false;
		if(_time <  (delTime-0.1f) && _time >= (delTime-0.3f) ){
			PitState[0].SetActive(true);_PitTime=0.0f;
			num = 0;
			boltJudgeDelay = 1.0f;
			totaldelay += boltJudgeDelay;
			_effect.BoltScewSound();
			b = true;
		}else if(_time >= (delTime-0.1f) && _time <(delTime)){
			PitState[1].SetActive(true);
			num = 1;
			boltJudgeDelay = 0.5f;
			totaldelay += boltJudgeDelay;
			int tmpG = Base64Manager.instance.GlobalEncoding(Global.gDrillScore); //good
			tmpG += gG;
			Global.gDrillScore = Base64Manager.instance.GlobalEncoding(tmpG);
			_effect.BoltScewSound();
			GameManager.instance.gDrillCount++;

			b = true;
		}else if(_time >=(delTime) && _time <(delTime+0.1f)){
			PitState[2].SetActive(true);
			num = 2;
			boltJudgeDelay = 0;
			int tmpG = Base64Manager.instance.GlobalEncoding(Global.pDrillScore); //perfect
			tmpG += pG;
			Global.pDrillScore = Base64Manager.instance.GlobalEncoding(tmpG);
			GameManager.instance.pDrillCount++;
			GameManager.instance.Vibrategame(400);
			bolt_effect.transform.position =bolt_line.transform.position;
			bolt_effect.SetActive(true);
			_effect.BoltScewPerfectSound();
			b = true;
		}else if(_time >=(delTime+0.1f)){
			PitState[3].SetActive(true);
			num = 3;
			boltJudgeDelay = 1.0f;
			totaldelay += boltJudgeDelay;
			_effect.BoltScewSound();
			b = true;
		}
		if(b) textTween(num);
		////==!!Utility.LogWarning("PitInTimeCheck() " + b + " time " + _time + " deltime :" + delTime);
		return b;
	}	*/


	float delTime = 0.0f;
	public void boltAddTween(){
		// GameObject _go = GameObject.Find("Bolt_Line");
		_PitTime=0.0f;
		_uisound.ScrewYellowPlay();
		TweenScale _tmpscale = bolt_line.GetComponent<TweenScale>() as TweenScale;
		_tmpscale.enabled = false;
		_PitTime=0.0f;
		_tmpscale.from =  new Vector3(678,596,1);
		_tmpscale.to = new Vector3(74f,65f,1);
		delTime = Base64Manager.instance.getFloatEncoding( Global.gScrewTime, 0.001f);
		_tmpscale.duration = delTime;
		_tmpscale.style = UITweener.Style.Once;
		_tmpscale.method = UITweener.Method.Linear;
		_tmpscale.eventReceiver = gameObject;
		_tmpscale.callWhenFinished = "BoltSet";
		//_tmpscale.onFinished = delegate {
		//	BlotSet();
		//};
		_tmpscale.Reset();
		_tmpscale.enabled = true;
		return;
	}

	public float tweenTime =  1.5f;//= UserDataManager.instance.PitTweenTime;
	private void addTween(GameObject obj, Vector3 from, Vector3 to){
		obj.SetActive(true);
		TweenPosition _tween = obj.GetComponent<TweenPosition>() as TweenPosition;
		if(_tween != null){
			Destroy(_tween);
		}
		_tween = obj.AddComponent<TweenPosition>() as TweenPosition;
		_tween.from = from;
		_tween.to = to;
		_tween.duration = tweenTime;
		_tween.method = UITweener.Method.EaseInOut;
		_tween.eventReceiver = gameObject;
		//_tween.callWhenFinished = "WhenWheelStart";
	}
	
	private void endTween(GameObject obj, Vector3 from, Vector3 to){
		TweenPosition _tween = obj.GetComponent<TweenPosition>() as TweenPosition;
		if(_tween != null){
			Destroy(_tween);
		}
		_tween = obj.AddComponent<TweenPosition>() as TweenPosition;
		_tween.from = to;
		_tween.to = from;
		_tween.duration = tweenTime;
		_tween.method = UITweener.Method.EaseInOut;
		_tween.eventReceiver = gameObject;
		_tween.callWhenFinished = "TweenEnd";

	}



	private void TweenEnd(){

		for(int j= 0 ; j<5;j++){
			//boltActive(false,j,false);
		}
	
	}
	
	
}
