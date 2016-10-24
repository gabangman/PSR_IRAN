using UnityEngine;
using System.Collections;

public class MyCarCtrl : MonoBehaviour {

	// Use this for initialization
	public Animation myCarAnimation, myCarAni;
	public Animation[] crews;
	public AudioSource S_car;
	private UISprite _mycar;
	private float playTime;
	public float _mycarPos;
	public float _mycarinitPos;
	private MusicSound _music;// _pitinSound;
	public AnimationClip longR1;

	private Transform flWheel;
	private Transform frWheel;
	private Transform rlWheel;
	private Transform rrWheel;
	Transform tr, trarrow;
	public float totalLength;
	public float SpriteLength;
	private string mySpriteName;
	ParticleSystem _speedEffect, _speedEffect_Boost, _speedEffect_Add,_speedEffect_Add1;
	private string currentAniName;
	public AnimationState stateRace, stateOther;
	float previousLength = 0.0f;
	GameObject myCarObject;
	string myCarName;
	int mycarid, mycrewid, mysponid;

	void InitMyCar(){
		mycrewid = GV.PlayCrewID;
		mysponid = GV.PlaySponID;
		myCarName = GV.PlayCarID.ToString();
		var _carinit = gameObject.AddComponent<CarInit>() as CarInit;
		var car = _carinit.MakeCar(myCarName,10, gameObject) as GameObject;		
		flWheel = car.GetComponent<CarType>().Tires[0];
		frWheel = car.GetComponent<CarType>().Tires[1];
		rlWheel = car.GetComponent<CarType>().Tires[2];
		rrWheel = car.GetComponent<CarType>().Tires[3];
	
		mySpriteName =myCarName;
		car.AddComponent<createDriveraction>().MakeDriverCrew(car);
		if(Global.gRaceInfo.sType != SubRaceType.DragRace){
			transform.FindChild("PitBox_1").gameObject.AddComponent<createPBaction>().CreateMyPB(mysponid);
		}
		myCarObject = car;
		car = null;
		Destroy(_carinit);
	}

	void SpeedFx_Pause(){
		_speedEffect.transform.gameObject.SetActive(false);
		_speedEffect_Boost.transform.gameObject.SetActive(false);
		_speedEffect_Add.transform.gameObject.SetActive(false);
		_speedEffect_Add1.transform.gameObject.SetActive(false);
	}

	void SpeedFx_Resume(){
		_speedEffect.transform.gameObject.SetActive(true);
		_speedEffect_Boost.transform.gameObject.SetActive(true);
		_speedEffect_Add.transform.gameObject.SetActive(true);
		//_speedEffect_Add1.transform.gameObject.SetActive(true);
	}

	void InitSpeedEffect(){
		_speedEffect = transform.GetChild(0).GetChild(0).FindChild("SpeedFx").GetChild(0).GetComponent<ParticleSystem>();
		_speedEffect.emissionRate = 0;
		_speedEffect_Boost = transform.GetChild(0).GetChild(0).FindChild("SpeedFx_Boost").GetChild(0).GetComponent<ParticleSystem>();
		_speedEffect_Boost.emissionRate = 0;
		_speedEffect_Add = transform.GetChild(0).GetChild(0).FindChild("Boost_Add").GetChild(0).GetComponent<ParticleSystem>();
		_speedEffect_Add.emissionRate = 0;
		_speedEffect_Add1 = transform.GetChild(0).GetChild(0).FindChild("Boost_Add").GetComponent<ParticleSystem>();
		_speedEffect_Add1.emissionRate = 0;
	}

	void InitAnimation(){
		myCarAnimation = gameObject.GetComponent<Animation>() as Animation;	
		if(Global.gRaceInfo.sType == SubRaceType.DragRace) {
			if(Global.DragSub == 1){
				myCarAnimation.RemoveClip("R1");
				myCarAnimation.AddClip(longR1,"R1");
			}
		}
		currentAniName = "R1";
		stateRace = myCarAnimation["R1"];
		GameManager.instance._AniSpeed = 1.0f;
		myCarAnimation.Play("R1");
		stateRace.speed = 0.0f;
		myCarAni = myCarObject.GetComponent<CarType>().CarAni;
		myCarAni.Play("Running");
		if(Global.gRaceInfo.sType == SubRaceType.DragRace) {
			stateOther= null;//	myCarAnimation.PlayQueued("End",QueueMode.CompleteOthers,PlayMode.StopAll);
			return;
		}
		stateOther = myCarAnimation.PlayQueued("PitIn",QueueMode.CompleteOthers,PlayMode.StopAll);
	}

	void InitCarSprite(){
		var obj = GameObject.Find ("GUIManager").GetComponent<ManagerGUI>() as ManagerGUI;
		var sprite = obj.findPanel("RaceUp").transform as Transform;
		var sprite1 = sprite.FindChild("MiniMapMyCar") as Transform;//.GetComponent<UISprite>() as UISprite;
		_mycar =sprite1.GetComponent<UISprite>() as UISprite;

		_mycarinitPos = _mycar.transform.localPosition.x;
		tr  = _mycar.transform;	
		sprite1 = sprite.FindChild("MiniMapMyCar_arrow");
		trarrow = sprite1;

		sprite1 = sprite.FindChild("MiniMap_Pitstop") as Transform;
		if(Global.gRaceInfo.sType != SubRaceType.DragRace){
		//	_mycarPos =myCarAnimation["R1"].length + myCarAnimation["R2"].length;
			_mycarPos = Global.gRaceInfo.R1_Time + Global.gRaceInfo.R2_Time;
			float sLength = _mycarinitPos+(Global.gRaceInfo.R1_Time /_mycarPos) *(640f);
		//	Utility.LogWarning(string.Format("{0}    {1}", _mycarPos,sLength));
			sprite1.localPosition = new Vector3(sLength, sprite1.localPosition.y, sprite1.localPosition.z);
		}else{
			//_mycarPos =myCarAnimation["R1"].length;
			_mycarPos = Global.gRaceInfo.R1_Time;
			sprite1.gameObject.SetActive(false);
		}
	}
	
	void Awake () {
	
	
	}

	void Start () {
		AudioManager.Instance.CarSountInit(gameObject);
		_music = gameObject.GetComponent<MusicSound>();
		InitMyCar();
	   	InitSpeedEffect();
		InitAnimation();
		InitCarSprite();
		/*
		if(Global.gRaceInfo.modeType != RaceModeType.DragMode){
			var  _script = transform.GetChild(1).gameObject.AddComponent<createCrewaction>() as createCrewaction;
			_script.CrewCreate(mycrewid);
			crews =_script.CrewAniamtion();
			Destroy(_script);
		}*/
		if(Global.gRaceInfo.sType != SubRaceType.DragRace){
			var  _script = transform.GetChild(1).gameObject.AddComponent<createCrewaction>() as createCrewaction;
			_script.CrewCreate(mycrewid);
			crews =_script.CrewAniamtion();
			Destroy(_script);
		}
		BoostEffectStart(false);
	}

	void StartAnimation(){
		Utility.LogWarning("StartAnim");
		return;
		//myCarAnimation.Play("R1");
	}

	public void SaveSpriteName(int _rank){
		GameManager.instance.rankSpritNames[_rank] = mySpriteName;
	}

	public void MinMapMyCar(){
		if(Global.gRaceInfo.sType == SubRaceType.DragRace){
			totalLength = myCarAnimation["R1"].time+previousLength;
		}else{
			totalLength = myCarAnimation["R1"].time + myCarAnimation["R2"].time+previousLength;
		}

		SpriteLength = _mycarinitPos+(totalLength/_mycarPos) *(640f);
		tr.localPosition = new Vector3(SpriteLength, tr.localPosition.y, tr.localPosition.z);
		trarrow.localPosition  = new Vector3(SpriteLength, trarrow.localPosition.y, trarrow.localPosition.z);
	}
	void AICompetitionCheck(){
		float t = myCarAnimation["R2"].time;
		GameManager.instance.CompetitionTime = t;
	}

	void AICompetitionCheck_Drag(){
		float t = myCarAnimation["R1"].time;
		GameManager.instance.CompetitionTime = t;
	}

	void Race02Start(){
		currentAniName = "R2";
		stateRace =  myCarAnimation["R2"];
		myCarAnimation.Play("R2");
		stateOther=	null;//myCarAnimation.PlayQueued("End",QueueMode.CompleteOthers,PlayMode.StopAll);
		isSkid = true;
		StartCoroutine("pitinSoundvolumedown");
		_mycar.transform.GetComponent<Spin>().enabled = true;
		SpeedFx_Resume();
		AnimationPlay(5);
	}

	void Race01Start(){
		isSkid = true;
		_mycar.transform.GetComponent<Spin>().enabled = true;
		AudioManager.Instance.StartCoroutine("carSoundDecreas");
		StartCoroutine("pitinSoundvolumedown");
	}

	IEnumerator PitAniStart(){
		//float delay = myCarAnimation["PitIn"].length*0.1f;
		//yield return new WaitForSeconds(delay);
		AnimationPlay(1);
		yield return null;
	}

	void RankCheck(int race){
		int mrank = 0;
		if(race == 1){
			//Global.Race01Ranking[mrank] = gameObject.name;
			//Global.Race01Time[mrank] = GameManager.instance._totalTime;
			//SaveSpriteName(mrank);
			//obj.gameObject.SendMessage("SaveSpriteName", mrank, SendMessageOptions.DontRequireReceiver);
		}else{
			mrank = Global.gRaceCount;
			Global.gRaceCount++;
			SaveSpriteName(mrank);
			Global.Race02Ranking[mrank] =  gameObject.name;
			Global.Race02Time[mrank] = GameManager.instance._totalTime;
		}
	}

	float previousTime = 0.0f;
	float previousLength1 = 0.0f;
	void CarAniSpeedCalculation(){
		previousTime = Time.timeSinceLevelLoad;
		previousLength1 = myCarAnimation["R1"].time;
	
	}
	IEnumerator MyCarWheelStop(){
		if(Global.gRaceInfo.sType == SubRaceType.CityRace){
			yield return new WaitForSeconds((2.458f-0.5f));
			_mycar.transform.GetComponent<Spin>().enabled = false;
			GameManager.instance.isWheel = false;
		}else if(Global.gRaceInfo.sType == SubRaceType.RegularRace){
			yield return new WaitForSeconds((4.124f-0.5f));
			_mycar.transform.GetComponent<Spin>().enabled = false;
			GameManager.instance.isWheel = false;
		}
	}
	

	protected void StopMyCarDragRace(){
		GameManager.instance.SaveResultTime("R1_DragE");
		RankCheck(2);
		GameManager.instance.mgrgui.userPanelInfo.GetComponent<userinfoaction>().AISlotCtrl(-1);
	}

	protected void R2FinishState(){
		if(Global.gRaceInfo.sType == SubRaceType.DragRace){
			StopMyCarDragRace();
			return;
		}
		currentAniName = string.Empty;
		previousLength+= myCarAnimation["R2"].length;
		GameManager.instance.SaveResultTime("R2_E");
		RankCheck(2);
		GameManager.instance.mgrgui.userPanelInfo.GetComponent<userinfoaction>().AISlotCtrl(-1);
		Utility.LogWarning("R2FinishState");
	}

	float timeX = 0.0f;
	void AnimationUpdate(){
		timeX += Time.deltaTime;
		switch(currentAniName){
		case "R1":{
			myCarAnimation["R1"].speed = GameManager.instance._AniSpeed;
			GameManager.instance.CompetitionTimes[0] = myCarAnimation["R1"].time;
			if(!myCarAnimation.IsPlaying("R1")){
				if(Global.gRaceInfo.sType != SubRaceType.DragRace){
					//previousLength = myCarAnimation["R1"].length;
					previousLength = Global.gRaceInfo.R1_Time;
					currentAniName = "PitIn";
					AnimationPlay(0);
					StartCoroutine("PitAniStart");
					GameManager.instance.SaveResultTime("R1_E");
					_music.CarPitInSound();
					//_mycar.transform.GetComponent<Spin>().enabled = false;
					RankCheck(1);
					GameManager.instance.GameRaceStateChange("race01finish");
					SpeedFx_Pause();
					StartCoroutine("MyCarWheelStop");
				}else{
				//	StopMyCarRace();
				//	currentAniName = "End";
				//	GameManager.instance.mgrgui.userPanelInfo.GetComponent<userinfoaction>().AISlotCtrl(-1);
					GameManager.instance.isWheel = false;
					currentAniName = string.Empty;
				}
			}
		}
			break;
		case "R2":{
			myCarAnimation["R2"].speed = GameManager.instance._AniSpeed;
			GameManager.instance.CompetitionTimes[0] = myCarAnimation["R2"].time;
			if(!myCarAnimation.IsPlaying("R2")){
				//currentAniName = "End";
				//previousLength+= myCarAnimation["R2"].length;
				//GameManager.instance.SaveResultTime("R2_E");
				//RankCheck(2);
				//GameManager.instance.mgrgui.userPanelInfo.GetComponent<userinfoaction>().AISlotCtrl(-1);
				//stateOther.speed =1.0f;
				GameManager.instance.isWheel = false;
				currentAniName = string.Empty;
			}
		}
			break;
		case "PitIn":{
			if(myCarAnimation.isPlaying) 
				//stateOther.speed = GameManager.instance._AniSpeed;
				stateOther.speed =1.0f;
			if(!myCarAnimation.IsPlaying("PitIn")){
				currentAniName = string.Empty;
				GameManager.instance.SaveResultTime("PitIn_S");
				myCarAni.Play("Right_Up");
				_music.CarUpSound();
			}
		}
			break;
		case "End":{
		}
			break;
		default :
			break;
		}
	}

	void Update () {
		if(isPause) return;
		MinMapMyCar();
		AnimationUpdate();
		ParticleSystemUpdate();
		if(GameManager.instance.isWheel){
			rotateWheel();			
		}
//		if(GameManager.instance.RaceState == GameManager.GAMESTATE.PITIN){
//			if(!S_car.isPlaying)
//			{
//				if(!S_car.mute)
//					S_car.Play();
//			}
//				return;
//		}
	}

	void PitSoundPlay(){
		S_car.volume = 1.0f;
		if(!S_car.isPlaying)
		{
			if(!S_car.mute)
		 		S_car.Play();
		}
	}


	void ParticleSystemUpdate(){
		if(GameManager.instance._AniSpeed < 1.05f){
			_speedEffect.emissionRate = 0;
		}else{
			_speedEffect.emissionRate= GameManager.instance._AniSpeed * 110f-80;
		}
		if(isBoost){
			_speedEffect_Boost.emissionRate= 200f;
			transform.GetChild(0).GetChild(0).FindChild("SpeedFx_Boost").FindChild("RunePrisonUplights").gameObject.SetActive(true);
			transform.GetChild(0).GetChild(0).FindChild("SpeedFx_Boost").FindChild("RunePrisonDownlights").gameObject.SetActive(true);
			transform.GetChild(0).GetChild(0).FindChild("SpeedFx_Boost").FindChild("BoostEffect_Mesh").gameObject.SetActive(true);
			if(isBoostAdd) return;
			_speedEffect_Add1.transform.gameObject.SetActive(true);
			_speedEffect_Add.emissionRate = 200f;
			_speedEffect_Add1.emissionRate = 200f;
			StartCoroutine("EffectAddDisable");
			isBoostAdd = true;
		}
	
	}
	bool isBoostAdd = false;

	IEnumerator pitinSoundvolumedown(){
		float vol = 1.0f;
		while(true){
			vol-= 0.01f;
			S_car.volume = vol;
			if(vol <= 0){
				S_car.Stop();
				break;
			}
			yield return new WaitForSeconds(0.05f);
		}
		yield return null;
	}

	void rotateWheel(){
		float rotateTime =0.0f;
		if(GameManager.instance._AniSpeed == 0){
			 rotateTime = Time.deltaTime*100*30*1;
		}else{
			 rotateTime = Time.deltaTime*100*30*GameManager.instance._AniSpeed;
		}
		flWheel.Rotate(0,0,rotateTime);
		frWheel.Rotate(0,0,rotateTime);
		rlWheel.Rotate(0,0,rotateTime);
		rrWheel.Rotate(0,0,rotateTime);
	}

	void GasManOut(){
		crews[3].Play("gas_exit");
		AnimationState _ani=	crews[3].PlayQueued("gas_exit_idle",QueueMode.CompleteOthers,PlayMode.StopAll);	
		_ani.wrapMode = WrapMode.Loop;
	}

	public void AnimationPlay(int step){
		AnimationState _ani = null;
		switch(step){
		case 0: 
		{
			foreach(Animation _animation in crews){
				_animation.Play ();
			}
		}break;
		case 1:
		{
			if(Global.gRaceInfo.sType == SubRaceType.CityRace) {
				crews[0].Play("City_tire_run_R");
				crews[0]["City_tire_run_R"].speed = 1.0f;
				crews[3].Play("City_gas_run");
				crews[2].Play ("City_jack_run_pushup_R");
				crews[2]["City_jack_run_pushup_R"].speed = 1.0f;
			}else { 
				crews[0].Play("tire_run_R");
				crews[0]["tire_run_R"].speed = 1.0f;
				crews[3].Play("gas_run");
				crews[2].Play ("jack_run_pushup_R");
				crews[2]["jack_run_pushup_R"].speed = 1.0f;
			}
			_ani = crews[0].PlayQueued("tire_crew_idle_R",QueueMode.CompleteOthers,PlayMode.StopAll);
			_ani.wrapMode = WrapMode.Loop;
			crews[1].Play ("chief_idle_stand");
		}break;
		case 2:
		{
			crews[3].Play("gas_insert");
			crews[3]["gas_insert"].wrapMode =WrapMode.Once;
			crews[2].Play ("jack_run_pushup_R_Up");
			_ani = crews[2].PlayQueued("jack_pushup_idle_R",QueueMode.CompleteOthers,PlayMode.StopAll);
			_ani.wrapMode = WrapMode.Loop;
		}
			break;
		case 3:
		{
			float fDelay = Base64Manager.instance.getFloatEncoding(Global.gPitCrewTime, 0.001f);
			crews[0].Play("tire_run_L");
			crews[0]["tire_run_L"].speed = fDelay;
			_ani = crews[0].PlayQueued("tire_crew_idle_L",QueueMode.CompleteOthers,PlayMode.StopAll);
			_ani.wrapMode = WrapMode.Loop;
			myCarAni.Play("Right_Down");
			_music.CarDownSound();
			crews[2].Play ("jack_run_pushup_L");
			crews[2]["jack_run_pushup_L"].speed =fDelay;
		}break;
		case 4:
		{
			crews[0].Play("tire_exit");
			_ani = crews[0].PlayQueued("tire_exit_idle",QueueMode.CompleteOthers,PlayMode.StopAll);
			_ani.wrapMode = WrapMode.Loop;
			crews[2].Play ("jack_exit");
			_ani = crews[2].PlayQueued("jack_exit_idle",QueueMode.CompleteOthers,PlayMode.StopAll);
			_ani.wrapMode = WrapMode.Loop;
			myCarAni.Play("Left_Down");
			_music.CarDownSound();
		}break;
		case 5:
		{
			for(int i = 0 ; i  < 4; i++){
				crews[i].Stop();
			}
			myCarAni.Play("Running");
		}break;
		case 6:
		{
			myCarAni.Play("Left_Up");
			_music.CarUpSound();
			crews[2].PlayQueued("jack_run_pushup_L_Up",QueueMode.CompleteOthers,PlayMode.StopAll);
			crews[2].PlayQueued("jack_pushup_idle_L",QueueMode.CompleteOthers,PlayMode.StopAll);
		}break;
		default:
			break;
		}
	
	}

	bool isPause = false;
	float tempSpeed = 0.0f;
	void OnPauseMessage(){
		tempSpeed = GameManager.instance._AniSpeed;
		GameManager.instance._AniSpeed = 0.0f;
		AnimationUpdate();
		isPause  = true;
	}
	
	void OnResumeMessage(){
		GameManager.instance._AniSpeed = tempSpeed;
		AnimationUpdate();
		isPause = false;
	}



	public void finishSound(){
		//_music.CarMusicFinishSound();
	
	}

	void finishPassSound(){
		EffectSound _effect;
		_effect = GetComponent<EffectSound>();
		_effect.FinishLinePassSound();
	}
	
	public void makeSkidMark(){
		var skid01 = ObjectManager.GetRaceObject("Effect","skidL") as GameObject;
		//var skidParent = gameObject.transform.FindChild("Joint_Root_Me").GetChild(0).FindChild(myCarName).GetChild(0).FindChild("Skid_Pos_L").gameObject;
		//var skidParent = gameObject.transform.GetChild(0).GetChild(0).FindChild(myCarName).GetChild(0).FindChild("Skid_Pos_L").gameObject;
		var skidParent = myCarObject.GetComponent<CarType>().Skid_L;
		ObjectManager.ChangeObjectParent(skid01,skidParent.transform);
		ObjectManager.ChangeObjectPosition(skid01, Vector3.zero, Vector3.one, Vector3.zero);
		//var skid02 = Instantiate(obj) as GameObject;//, skidpos[1].localPosition, skidpos[1].localRotation) as GameObject;
		var skid02 = ObjectManager.GetRaceObject("Effect","skidR") as GameObject;
		//skidParent = gameObject.transform.GetChild(0).GetChild(0).FindChild(myCarName).GetChild(0).FindChild("Skid_Pos_R").gameObject;
		//skidParent = gameObject.transform.FindChild("Joint_Root_Me").GetChild(0).FindChild(myCarName).GetChild(0).FindChild("Skid_Pos_R").gameObject;
		skidParent = myCarObject.GetComponent<CarType>().Skid_R;
		ObjectManager.ChangeObjectParent(skid02,skidParent.transform);
		ObjectManager.ChangeObjectPosition(skid02, Vector3.zero, Vector3.one, Vector3.zero);
		StartCoroutine(SkidDisable(2.5f, skid01));
		StartCoroutine(SkidDisable(2.5f, skid02));
	}
	bool isSkid = false;
	IEnumerator SkidDisable(float delay, GameObject obj){
		ParticleSystem _p = obj.GetComponent<ParticleSystem>();
		float _emission = 0.0f;
		isSkid = false;
		for(;;){
			_emission += 4f;
			if(_emission >= 20.0f){
				_p.emissionRate = 20.0f;
				break;
			}
			_p.emissionRate = _emission;
			yield return new WaitForSeconds(0.1f);
		}

		for(;;){
			if(isSkid){
				break;
			}
			yield return null;
		}
		_emission = 20.0f;
		for(;;){
			_emission -= 2.0f;
			if(_emission < 0.0f){
				obj.SetActive(false);
				yield break;
			}
			_p.emissionRate = _emission;
			yield return new WaitForSeconds(0.1f);
		}
	}

	void BoostEffectStart(bool b){
		Transform[] boosts = myCarObject.GetComponent<CarType>().boosts;
		int count = boosts.Length;
		GameObject[] boostItem = new GameObject[count];

	//	for(int i = 0; i < count ; i++){
	//		boostItem[i] = boosts[i].gameObject;
	//	}

		string[] boostNozzle = new string[4];
		if(!b){
			boostNozzle[0] = "NozzleFL";
			boostNozzle[1] = "NozzleFR";
			boostNozzle[2] = "NozzleRL";
			boostNozzle[3] = "NozzleRR";
		}else{
			isBoost = true;
			boostNozzle[0] = "BoostFL";
			boostNozzle[1] = "BoostFR";
			boostNozzle[2] = "BoostRL";
			boostNozzle[3] = "BoostRR";
		}
		switch(count){
		case 2:
		{
			for(int i = 0; i < 2; i++){
				boostItem[i] = ObjectManager.GetRaceObject("Effect",boostNozzle[i]) as GameObject;
				//boostItem[i].transform.parent = temp.GetChild(i);
				boostItem[i].transform.parent = boosts[i];
				ObjectManager.ChangeObjectPosition(boostItem[i], Vector3.zero, Vector3.one, Vector3.zero);
			}
		}
			break;
		case 4:{
		
			for(int i = 0; i < 4; i++){
				boostItem[i] = ObjectManager.GetRaceObject("Effect",boostNozzle[i]) as GameObject;
				//boostItem[i].transform.parent = temp.GetChild(i);
				boostItem[i].transform.parent = boosts[i];
				ObjectManager.ChangeObjectPosition(boostItem[i], Vector3.zero, Vector3.one, Vector3.zero);
			}
		}
			break;

		case 8:{
		
			for(int i = 0; i < 4; i++){
				boostItem[i] = ObjectManager.GetRaceObject("Effect",boostNozzle[i]) as GameObject;
				//boostItem[i].transform.parent = temp.GetChild(i);
				boostItem[i].transform.parent = boosts[i];
				ObjectManager.ChangeObjectPosition(boostItem[i], Vector3.zero, Vector3.one, Vector3.zero);
			}
			for(int i = 4; i < 8; i++){
				boostItem[i] = Instantiate(boostItem[0]) as GameObject;
				//boostItem[i].transform.parent = temp.GetChild(i);
				boostItem[i].transform.parent = boosts[i];
				ObjectManager.ChangeObjectPosition(boostItem[i], Vector3.zero, Vector3.one, Vector3.zero);
			//	boostItem[i].transform.FindChild("RunePrisonUplights").gameObject.SetActive(true);
			//	boostItem[i].transform.FindChild("RunePrisonDownlights").gameObject.SetActive(true);
			}
		}
			break;
		}
		
		if(!b){
			
		}else{
			float total = myCarAnimation[currentAniName].length;
			float pressT = myCarAnimation[currentAniName].time;
			Global.PressBoostTime = (int)Mathf.Round((pressT/total)*100);
			Global.PressBoostTime = Base64Manager.instance.GlobalEncoding(Global.PressBoostTime);
			float delays = Base64Manager.instance.getFloatEncoding(Global.gBsTime, 0.001f);
			StartCoroutine(BoostDisable(delays, boostItem));
		}
		
	}

	bool isBoost = false;
	IEnumerator BoostDisable(float delay, GameObject[] boost){
		yield return new WaitForSeconds(delay);
		foreach(GameObject obj in boost){
			obj.SetActive(false);
		}
		isBoost = false;
		_speedEffect_Boost.emissionRate = 0.0f;
		_speedEffect_Add.emissionRate = 0.0f;
		_speedEffect_Add1.emissionRate = 0.0f;
		transform.GetChild(0).GetChild(0).FindChild("SpeedFx_Boost").FindChild("RunePrisonUplights").gameObject.SetActive(false);
		transform.GetChild(0).GetChild(0).FindChild("SpeedFx_Boost").FindChild("RunePrisonDownlights").gameObject.SetActive(false);//BoostEffect_Mesh
		transform.GetChild(0).GetChild(0).FindChild("SpeedFx_Boost").FindChild("BoostEffect_Mesh").gameObject.SetActive(false);
		StopCoroutine("EffectAddDisable");
	}

	IEnumerator EffectAddDisable(){
		float p = 200f;
		for(;;){
			p -=10;
			_speedEffect_Add.emissionRate = p;
			if(p < 0) {
				_speedEffect_Add1.transform.gameObject.SetActive(false);
			
				yield break;}
			yield return null;
		}
	}

	void BoostEffectEnd(){
		
	}


}

	
	

