using UnityEngine;
using System.Collections;
using System.Text;
[System.Serializable]
public class AIRaceInfo{
	public float  Torque;
	public float Pit_Time;
	public float B_Power;
	public float B_Time;
	public float Skid_Time;
	public int gbLv;
	public float raceResultTime;
	public int AICarID;
	public int AICrewID;
	public int AISponsorID;
	public bool isLive;
	public string userNick;
	public Texture2D userTexture;
	public float EventTime;
	public float EventRange;
	public float[] gearLedDelay;
	public float[] pressDelay;
	public int AIRefCarID;
	public int AIRefStarLV;
	public string ClassId;
}

public class AICarCtrl : MonoBehaviour {
	[SerializeField] public  AIRaceInfo mAIRaceInfo;
	[SerializeField] protected  Transform flWheel;
	[SerializeField] protected  Transform frWheel;
	[SerializeField] protected  Transform rlWheel;
	[SerializeField] protected  Transform rrWheel;
	public  Animation[] AICrew;
	[SerializeField] protected Animation AICarAnimation,AiCarAni;
	public float AiAniSpeed;
	public AnimationClip LongR1;
	[SerializeField] protected  GameObject[] skid;
	[SerializeField] protected  GameObject wheel, nozzle, boost;// skid;
	[SerializeField] protected  Transform UserTarget;

	private const float aiCar_Speed = 1.0f;
	public  float pace_Speed = 1.15f;
	private bool isMiniMap;
	private float aicarPos;
	private float aicarinitPos;
	private float aicarAnitime;
	protected bool isRace = true;
	protected string AiCarName;
	protected int aiCarID;
	protected int aiRadomID;
	protected GameObject carBody, AICarObject;
	private UISprite aicaricon;
	ParticleSystem _speedEffect_Add1;
	private GameObject speedBoost;
	public enum RaceState {WAIT,RACE01,RACE02READY, RACE02, RACE02DELAY, 
		READY,FINISHSNAPSHOT, FINISH, PITIN, PITINFINISH, PITINING, RACE01READY, RACE01DELAY, READYEND};
	public RaceState _raceState = RaceState.READY;
	Transform aiPos;
	AnimationState aniStateRace,aniStateExtra;
	string currentAniName = string.Empty;
	float previousLength = 0.0f;
	float totalDelay;
	System.Action AiSpeed;
	[SerializeField] public AIcarRace _carRace = new AIcarRace();


	void GlobalValueInit(){
		if(Global.isRaceTest){
			Global.gAICarInfo[aiCarID] = new AICarInfo();
			int n = Random.Range(2,10);
			Global.gAICarInfo[aiCarID].AICarID =  Base64Manager.instance.GlobalEncoding(1000+n);
			Global.gAICarInfo[aiCarID].AICrewID = Base64Manager.instance.GlobalEncoding(1200);
			if(aiRadomID == 2){
				Global.gAICarInfo[aiCarID].AISponsorID =  Base64Manager.instance.GlobalEncoding(1301+4);
			}else{
				Global.gAICarInfo[aiCarID].AISponsorID =  Base64Manager.instance.GlobalEncoding(1301+aiRadomID);
			}
			Global.gAICarInfo[aiCarID].AIRefCarID = Base64Manager.instance.GlobalEncoding(1005);
			Global.gAICarInfo[aiCarID].Torque= Base64Manager.instance.setFloatEncoding(31.5f,1000);
			Global.gAICarInfo[aiCarID].Pit_Time =  Base64Manager.instance.setFloatEncoding(30,1000);
			Global.gAICarInfo[aiCarID].B_Power=  Base64Manager.instance.setFloatEncoding(26.5f,1000);
			Global.gAICarInfo[aiCarID].B_Time=  Base64Manager.instance.setFloatEncoding(1.44f,1000);
			Global.gAICarInfo[aiCarID].Skid_Time= Base64Manager.instance.setFloatEncoding( 1.78f,1000);
			Global.gAICarInfo[aiCarID].isLive = true;
			Global.gAICarInfo[aiCarID].EventRange = Base64Manager.instance.setFloatEncoding( 70,1000);
			Global.gAICarInfo[aiCarID].EventTime = Base64Manager.instance.setFloatEncoding( 10,1000);
			gameObject.AddComponent<AIGearDelay>().NewMakeGearDelay(aiCarID);
			Global.gAICarInfo[aiCarID].AIRefStarLV = 0;
			totalDelay =Base64Manager.instance.getFloatEncoding( Global.gAICarInfo[aiCarID].Pit_Time,0.001f);
			
		}else{
			totalDelay =Base64Manager.instance.getFloatEncoding( Global.gAICarInfo[aiCarID].Pit_Time,0.001f);
			if(totalDelay < 5.0f ) totalDelay = 24.0f;
		}

		mAIRaceInfo = new AIRaceInfo();
		if(!mAIRaceInfo.isLive) return; 
		mAIRaceInfo.AICarID = Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[aiCarID].AICarID);//Global.gAICarInfo[aiCarID].AICarID;
		mAIRaceInfo.AICrewID =Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[aiCarID].AICrewID);		                                                           
		mAIRaceInfo.AISponsorID =Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[aiCarID].AISponsorID);
		mAIRaceInfo.AIRefCarID =Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[aiCarID].AIRefCarID);
		mAIRaceInfo.Torque =Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[aiCarID].Torque,0.001f);
		mAIRaceInfo.Pit_Time =Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[aiCarID].Pit_Time,0.001f);
		mAIRaceInfo.B_Power =Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[aiCarID].B_Power,0.001f);
		mAIRaceInfo.B_Time =Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[aiCarID].B_Time,0.001f);
		mAIRaceInfo.Skid_Time =Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[aiCarID].Skid_Time,0.001f);
		mAIRaceInfo.isLive =Global.gAICarInfo[aiCarID].isLive;
		mAIRaceInfo.EventRange =Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[aiCarID].EventRange,0.001f);
		mAIRaceInfo.EventTime =Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[aiCarID].EventTime,0.001f);
		mAIRaceInfo.AIRefStarLV =Global.gAICarInfo[aiCarID].AIRefStarLV;
		
		mAIRaceInfo.gbLv = Global.gAICarInfo[aiCarID].gbLv;
		mAIRaceInfo.gearLedDelay = new float[20];
		System.Array.Copy(Global.gAICarInfo[aiCarID].gearLedDelay, mAIRaceInfo.gearLedDelay,20);
		mAIRaceInfo.pressDelay = new float[20];
		System.Array.Copy(Global.gAICarInfo[aiCarID].pressDelay, mAIRaceInfo.pressDelay,20);
		mAIRaceInfo.ClassId = Global.gAICarInfo[aiCarID].AIClass.ID;
	}
	int aiCheckCarId;
	public void AICarInit(int _AIRadomID){
		string name = gameObject.name;
		aiCarID  = int.Parse(name[1].ToString());aiCheckCarId=aiCarID;
		aiCarID--;
		aiRadomID = _AIRadomID;
	 GlobalValueInit();
		if(!Global.gAICarInfo[aiCarID].isLive) {DestroyImmediate(gameObject); return;}
		_carRace.FrontLeftWheel = wheel.GetComponent<WheelCollider>();
		_carRace.FrontLeftWheel.motorTorque =10.0f;
		_carRace.AIinit(aiCarID);
		if(Global.gRaceInfo.mType == MainRaceType.Champion) aiCheckCarId = 1;
	}

	void OnEnable(){


	}

	void OnDisable(){
	
	}

	
	void InitSpeedEffect(){
		_speedEffect_Add1 = transform.GetChild(0).GetChild(0).FindChild("Boost_Add").GetComponent<ParticleSystem>();
		_speedEffect_Add1.emissionRate = 0;
		speedBoost = transform.GetChild(0).GetChild(0).FindChild("SpeedFx_Boost_User").gameObject;
	}

	void PaceCarInit(){
		var _carinit = gameObject.AddComponent<CarInit>() as CarInit;
		var car = transform.GetChild(0).GetChild(0).GetChild(4).gameObject as GameObject;

		flWheel = _carinit.FindPaceWheel(car, 1);
		frWheel = _carinit.FindPaceWheel(car, 2);
		rlWheel = _carinit.FindPaceWheel(car, 3);
		rrWheel = _carinit.FindPaceWheel(car, 4);
	}

	void InitAiCar(){
		var _carinit = gameObject.AddComponent<CarInit>() as CarInit;
		int tempid = Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[aiCarID].AICarID);
		AiCarName = tempid.ToString();
		var car = _carinit.MakeCar(AiCarName,aiCarID, gameObject) as GameObject;			
		AICarObject = car;
		var cartype  = AICarObject.GetComponent<CarType>() as CarType;
		flWheel = cartype.Tires[0];//_carinit.FindWheel(car, 1);
		frWheel = cartype.Tires[1];//_carinit.FindWheel(car, 2);
		rlWheel = cartype.Tires[2];//_carinit.FindWheel(car, 3);
		rrWheel = cartype.Tires[3];//_carinit.FindWheel(car, 4);
		car.AddComponent<createDriveraction>().AIMakeDriverCrew(car,aiCarID);	//car.GetComponent<createDriveraction>().AIChangeDriverAnimation();

		//AiCarAni = _carinit.FindAnimation(gameObject, AiCarName);
		AiCarAni = cartype.CarAni;
		AiCarAni.Play("Running");

		car = null;
		Destroy(_carinit);
	}

	void InitAiAnimation(){
		if(Global.gRaceInfo.sType == SubRaceType.DragRace) return;
		var  _script = transform.GetChild(1).gameObject.AddComponent<createCrewaction>() as createCrewaction;
		_script.AICrewCreate(aiCarID);
		AICrew =_script.CrewAniamtion();
		transform.FindChild("PitBox_1").gameObject.AddComponent<createPBaction>().CreatePB(aiCarID);
		Destroy(_script);
	}

	void InitAiRaceSprite(){
		isMiniMap = true;
		var obj = GameObject.Find ("GUIManager").GetComponent<ManagerGUI>() as ManagerGUI;
		var sprite = obj.findPanel("RaceUp").transform.FindChild(gameObject.name).gameObject;//.GetComponent<UISprite>() as UISprite;
		sprite.SetActive(true);
		if(Global.gRaceInfo.sType != SubRaceType.DragRace){
			//aicarPos =AICarAnimation["R1"].length + AICarAnimation["R2"].length;
			aicarPos =Global.gRaceInfo.R1_Time+ Global.gRaceInfo.R2_Time;
		}else{
			//aicarPos =AICarAnimation["R1"].length;
			aicarPos =Global.gRaceInfo.R1_Time;
		}
		aicaricon =sprite.GetComponent<UISprite>() as UISprite;
		aiPos = aicaricon.transform;
		aicarinitPos = aiPos.localPosition.x;	
	}

	void aniFinish(){
		gameObject.SetActive(false);
	}
	void Awake(){
		if(Global.isTutorial){
			AiSpeed = () =>{

				AiAniSpeed = _carRace.raceSpeed;
				aniStateRace.speed = AiAniSpeed;
				CheckShiftGear();
			};
		}else{
			AiSpeed = ()=>{
				AiAniSpeed = _carRace.raceSpeed;
				aniStateRace.speed = AiAniSpeed;
				if(_raceState == RaceState.RACE01 || _raceState == RaceState.RACE02 )
				CheckShiftGear();
			};
		}
	
		switch(Global.gRaceInfo.sType){
			case SubRaceType.DragRace:
			_raceState = RaceState.READYEND;
			isWheel = false;
			break;
			case SubRaceType.CityRace:
			_raceState = RaceState.READYEND;
			isWheel = false;
			break;
			case SubRaceType.RegularRace:
			isWheel = true;//aicaricon.transform.GetComponent<Spin>().enabled = true;
			break;
		}
	}
	void Start () {
		AiAniSpeed =0.996935f;
		AICarAnimation = gameObject.GetComponent<Animation>() as Animation;
	//	if(Global.gRaceInfo.mType == MainRaceType.Regular){
	//		if(GV.ChSeason <= 3)
	//			onPressDelay = 0.2f;
	//		else if(GV.ChSeason == 4) onPressDelay = 0.25f;
	//		else onPressDelay = 0.3f;
	//	}
	//	else onPressDelay = 0.2f;

		if(gameObject.name == "FaceCar"){
			AICarAnimation[gameObject.name].speed = pace_Speed;
			AnimationEvent ani = new AnimationEvent();
			ani.time = AICarAnimation[gameObject.name].length;
			ani.functionName = "aniFinish";
			AICarAnimation[gameObject.name].clip.AddEvent(ani);
			isMiniMap = false;
			PaceCarInit();
			return;
		}
		   carBody = transform.GetChild(0).GetChild(0).gameObject;
		   carBody.AddComponent<AIColliderManager>().InitAssign(gameObject);
			InitSpeedEffect();
			InitAiCar();
			InitAiAnimation();
			InitAICarAnimation();	
			InitAiRaceSprite();
			SetBoostEffect(false); // nozzle setting
		if(Global.gRaceInfo.mType == MainRaceType.Weekly){
			ArrayPressDelay = Global.gAICarInfo[aiCarID].pressDelay;
			for(int i =0; i< ArrayPressDelay.Length; i++){
				if(ArrayPressDelay[i] > 3.0f){
					ArrayPressDelay[i] = 0.50f;
				}else	if(ArrayPressDelay[i] > 2.5f){
					ArrayPressDelay[i] = 0.45f;
				}else	if(ArrayPressDelay[i] > 2.0f){
					ArrayPressDelay[i] = 0.40f;
				}else	if(ArrayPressDelay[i] > 1.5f){
					ArrayPressDelay[i] = 0.35f;
				}else	if(ArrayPressDelay[i] > 1.0f){
					ArrayPressDelay[i] = 0.30f;
				}else	if(ArrayPressDelay[i] > 0.5f){
					ArrayPressDelay[i] = 0.25f;
				}else	if(ArrayPressDelay[i] > 0.1f){
					ArrayPressDelay[i] = 0.20f;
				}else{
					ArrayPressDelay[i] = 0.15f;
				}
				
				/*	if(ArrayPressDelay[i] > 2.0f){
					ArrayPressDelay[i] = 0.3f;
				}else if(ArrayPressDelay[i] >= 1.5f){
					ArrayPressDelay[i]  = 0.5f;
				}else if(ArrayPressDelay[i] >= 1.0f){
					ArrayPressDelay[i]  = 0.4f;
				}else if(ArrayPressDelay[i] >= 0.5f){
					ArrayPressDelay[i]  = 0.3f;
				}else {
					ArrayPressDelay[i]  = 0.2f;
				}*/
				
				
				
			}
			//	mAIRaceInfo.pressTestDelay = new float[20];
			//	System.Array.Copy(ArrayPressDelay, mAIRaceInfo.pressTestDelay,20);
		}else if(Global.gRaceInfo.mType == MainRaceType.PVP){
			//	if(Global.gCtrlDelays.Length == 0){
			ArrayPressDelay = Global.gAICarInfo[aiCarID].pressDelay;
			for(int i =0; i< ArrayPressDelay.Length; i++){
				if(ArrayPressDelay[i] > 3.0f){
					ArrayPressDelay[i] = 0.50f;
				}else	if(ArrayPressDelay[i] > 2.5f){
					ArrayPressDelay[i] = 0.45f;
				}else	if(ArrayPressDelay[i] > 2.0f){
					ArrayPressDelay[i] = 0.40f;
				}else	if(ArrayPressDelay[i] > 1.5f){
					ArrayPressDelay[i] = 0.35f;
				}else	if(ArrayPressDelay[i] > 1.0f){
					ArrayPressDelay[i] = 0.30f;
				}else	if(ArrayPressDelay[i] > 0.5f){
					ArrayPressDelay[i] = 0.25f;
				}else	if(ArrayPressDelay[i] > 0.1f){
					ArrayPressDelay[i] = 0.20f;
				}else{
					ArrayPressDelay[i] = 0.15f;
				}
			}
		}else{
			ArrayPressDelay = new float[20];
			System.Array.Copy(Global.gCtrlDelays, ArrayPressDelay, Global.gCtrlDelays.Length);
		}



	}

	public void InitAICarAnimation(){
		/*if(Global.gRaceInfo.modeType == RaceModeType.DragMode) {
			if(Global.DragSub == 1){
				AICarAnimation.RemoveClip("R1");
				AICarAnimation.RemoveClip("End");
				AICarAnimation.AddClip(r2,"R1");
				AICarAnimation.AddClip(r2end, "End");
			}
		}*/
		if(Global.gRaceInfo.sType == SubRaceType.DragRace) {
			if(Global.DragSub == 1){
				AICarAnimation.RemoveClip("R1");
				AICarAnimation.AddClip(LongR1,"R1");
			}
			SetBoostInitalize("R1");
		}
		AICarAnimation.Play("R1");
		previousTime = Time.timeSinceLevelLoad;
		aniStateRace = AICarAnimation["R1"];
		currentAniName = "R1";
		/*if(Global.gRaceInfo.modeType == RaceModeType.DragMode) 
			aniStateExtra =  AICarAnimation.PlayQueued("End",QueueMode.CompleteOthers,PlayMode.StopAll);
		else 
			aniStateExtra = AICarAnimation.PlayQueued("PitIn",QueueMode.CompleteOthers,PlayMode.StopAll);
		*/
		if(Global.gRaceInfo.sType == SubRaceType.DragRace) 
			aniStateExtra =  null;
		else 
			aniStateExtra = AICarAnimation.PlayQueued("PitIn",QueueMode.CompleteOthers,PlayMode.StopAll);

	}
	
	 void CompetitionCheck(){
		float checkTime = GameManager.instance.CompetitionTime;
		float cTime = AICarAnimation["R2"].time;
		float dTime = checkTime - cTime;
		dTime = Mathf.Abs(dTime);
		float interval = AccountManager.instance.FinishInterval;
		if(GameManager.instance.isCompetiton) return;
		if(dTime < interval) GameManager.instance.isCompetiton = true;
		else GameManager.instance.isCompetiton = false;
	}
	void CompetitionCheck_Drag(){
		float checkTime = GameManager.instance.CompetitionTime;
		float cTime = AICarAnimation["R1"].time;
		float dTime = checkTime - cTime;
		dTime = Mathf.Abs(dTime);
		float interval = AccountManager.instance.FinishInterval;
		if(GameManager.instance.isCompetiton) return;
		if(dTime < interval) GameManager.instance.isCompetiton = true;
		else GameManager.instance.isCompetiton = false;
	}

	bool isRPM = true;
	void CheckShiftGear(){
		if(isRPM){
			if(_carRace.EngineRPM > _carRace.CheckEngineRPM){
					StartCoroutine("AILedCheck");
					isRPM = false;
			}
		}else{
		
		}
	}
	private int gCount = 0;
	private float[] ArrayPressDelay;
	IEnumerator AILedCheck(){
		float delay = _carRace.LedTime[_carRace.CurrentGear];
		delay = delay*3.0f;
		yield return new WaitForSeconds(delay);
		_carRace.isAIPress = false;
		bool b = _carRace.ShieftGear();
		float ctrlDelay = ArrayPressDelay[gCount];
		yield return new WaitForSeconds(ctrlDelay);
		_carRace.isAIPress =true;
		delay = Global.gAICarInfo[aiCarID].gearLedDelay[gCount];
		gCount++;
		if(gCount > 19) gCount = 19;
		_carRace.motorInputTorque = _carRace.motorInputTorque*0.1f;
		yield return new WaitForSeconds(delay);
		isRPM = b;
		_carRace.motorInputTorque = _carRace.motorInputTorque*10f;
	//	Utility.Log("_car1 " + _carRace.motorInputTorque + "  " + gameObject.name + "  " + Time.timeSinceLevelLoad);
	}


	void StartAnimation(){
		AICarAnimation.Play("R1");
	}
	public void RaceStart(){
		previousTime = Time.timeSinceLevelLoad;

	}
	public void GearDown(){
		StartCoroutine("AIGearDown");
	
	}

	IEnumerator AIGearDown(){
		_carRace.brakePower = 50;
		_carRace.motorInputTorque = _carRace.motorInputTorque * 0.5f;
		yield return new WaitForSeconds(1.5f);
		_carRace.brakePower = 0;
		_carRace.motorInputTorque = _carRace.motorInputTorque * 2.0f;
	}

	float previousTime = 0.0f;
	int airank = 20;

	void RankCheck(int race){
		int mrank = 0;
		if(race == 1){
			if(GameManager.instance.currentCamera.name == "Cam_R1_Finish" )
				finishPassSound();
		}else{
			if(airank != 20) return;
			mrank = Global.gRaceCount;
			airank = mrank;
			Global.gRaceCount++;
			SaveSpriteName(mrank);
			Global.Race02Ranking[mrank] = gameObject.name;
			Global.Race02Time[mrank] = GameManager.instance._totalTime;
		}
	}

	void recvFinishMsg(){
		if(airank == 20){
			airank = Global.gRaceCount;
			Global.gRaceCount++;
			SaveSpriteName(airank);
			Global.Race02Ranking[airank] = gameObject.name;
			Global.Race02Time[airank] = GameManager.instance._totalTime;
		}else{
		
		}
	}

	public void PitInAniStart(){
		//StartCoroutine("AIPitInCrew1");
		StartCoroutine("AIPitInCrew2");
		StartCoroutine("AIPitInCrew3");
		StartCoroutine("AIPitInCrew4");
		StartCoroutine("AIPitInCrew5");
	}
	
	 
	IEnumerator AIPitInCrew1(){
		if(Global.gRaceInfo.sType == SubRaceType.CityRace) AICrew[2].Play ("City_jack_run_pushup_R");
		else AICrew[2].Play ("jack_run_pushup_R");
		if(Global.gRaceInfo.sType == SubRaceType.CityRace) AICrew[0].Play("City_tire_run_R");
		else AICrew[0].Play("tire_run_R");
		AICrew[0].PlayQueued("tire_crew_idle_R",QueueMode.CompleteOthers,PlayMode.StopAll);
		if(Global.gRaceInfo.sType == SubRaceType.CityRace) AICrew[0].Play("City_gas_run");
		else AICrew[3].Play("gas_run");
		yield return new WaitForSeconds(0.1f);
	}
	IEnumerator AIPitInCrew2(){
		AICrew[2].Play ("jack_run_pushup_R_Up");
		AICrew[2].PlayQueued("jack_pushup_idle_R",QueueMode.CompleteOthers,PlayMode.StopAll);
		AICrew[3].Play("gas_insert");//,QueueMode.CompleteOthers,PlayMode.StopAll);
		AiCarAni.Play("Right_Up");
		yield return new WaitForSeconds(0.1f);
	}
	
	IEnumerator AIPitInCrew3(){
		
		float _timeDelay = totalDelay*0.33f;
		yield return new WaitForSeconds(_timeDelay);
		AICrew[2].Play ("jack_run_pushup_L");
		AICrew[2].PlayQueued("jack_run_pushup_L_Up",QueueMode.CompleteOthers,PlayMode.StopAll);
		AICrew[2].PlayQueued("jack_pushup_idle_L",QueueMode.CompleteOthers,PlayMode.StopAll);
		AICrew[0].Play("tire_run_L");
		AICrew[0].PlayQueued("tire_crew_idle_L",QueueMode.CompleteOthers,PlayMode.StopAll);
		AiCarAni.Play("Right_Down");
		_timeDelay =	AICrew[2]["jack_run_pushup_L"].length;
		yield return new WaitForSeconds(_timeDelay);
		AiCarAni.Play("Left_Up");
	}

	IEnumerator AIPitInCrew4(){
		float _timeDelay = totalDelay*0.85f;
		yield return new WaitForSeconds(_timeDelay);
		AICrew[0].Play("tire_exit");
		AICrew[0].PlayQueued("tire_exit_idle",QueueMode.CompleteOthers,PlayMode.StopAll);
		AICrew[3].Play("gas_exit");
		AICrew[3].PlayQueued("gas_exit_idle",QueueMode.CompleteOthers,PlayMode.StopAll);
		AICrew[2].Play ("jack_exit");
		AICrew[2].PlayQueued("jack_exit_idle",QueueMode.CompleteOthers,PlayMode.StopAll);
		AiCarAni.Play("Left_Down");
	
	}
	IEnumerator AIPitInCrew5(){
		float _timeDelay = totalDelay;
		yield return new WaitForSeconds(_timeDelay);
		StartRace02();
	}

	void StartRace01(){
		if(Global.gRaceInfo.sType== SubRaceType.RegularRace){
			_raceState = RaceState.RACE01;
		}else{
			_raceState = RaceState.RACE01DELAY;
			StartCoroutine("AITireDelay01");
			aicaricon.transform.GetComponent<Spin>().enabled = true;
			isWheel = true;
		}
	}

	private void SetBoostInitalize(string rName){
		AnimationEvent ani = new AnimationEvent();
		int max = (int)Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[aiCarID].EventRange,0.001f);
		float min = Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[aiCarID].EventTime,0.001f)*0.01f;
		int range = Random.Range(0,max);
		float _time = min + (float)range*0.01f;
		if(Global.gRaceInfo.sType == SubRaceType.DragRace){
			ani.time = Global.gRaceInfo.R1_Time * _time;
		}else{
			ani.time = Global.gRaceInfo.R2_Time * _time;
		}
		ani.functionName = "BoostEvent";
		AICarAnimation[rName].clip.AddEvent(ani);
	}

	void StartRace02(){
		isWheel = true;
		aicaricon.transform.GetComponent<Spin>().enabled = true;
		AICarAnimation.Play("R2");
		aniStateRace =  AICarAnimation["R2"];
		aniStateExtra =  null;//AICarAnimation.PlayQueued("End",QueueMode.CompleteOthers,PlayMode.StopAll);
		currentAniName = "R2";
		_raceState = RaceState.RACE02DELAY;
		_carRace.changeGear2();
		StartCoroutine("AITireDelay");
		if(Global.isTutorial) return;
		SetBoostInitalize("R2");
	}
	IEnumerator AITireDelay(){
		//isWheel = true;
		SetSkidEffect();
		float delay = Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[aiCarID].Skid_Time,0.001f);
		yield return new WaitForSeconds(delay);
		UnSetSkidEffect();
		_raceState = RaceState.RACE02;
		isRPM = true;
		float _timeDelay = Global.gAICarInfo[aiCarID].gearLedDelay[gCount];
		gCount++;
		if(gCount > 19) gCount = 19;
		_carRace.motorInputTorque = _carRace.motorInputTorque*0.1f;
		yield return new WaitForSeconds(_timeDelay);
		_carRace.motorInputTorque = _carRace.motorInputTorque*10f;
	}

	IEnumerator AITireDelay01(){
		isWheel = true;
		SetSkidEffect();
		float delay = Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[aiCarID].Skid_Time,0.001f);
		//Utility.LogWarning(delay);
		yield return new WaitForSeconds(delay);
		UnSetSkidEffect();
		_raceState = RaceState.RACE01;
		isRPM = true;
		float _timeDelay = Global.gAICarInfo[aiCarID].gearLedDelay[gCount];
		gCount++;
		if(gCount > 19) gCount = 19;
		_carRace.motorInputTorque = _carRace.motorInputTorque*0.1f;
		yield return new WaitForSeconds(_timeDelay);
		//Utility.LogWarning(_timeDelay);
		_carRace.motorInputTorque = _carRace.motorInputTorque*10f;
	}

	protected void StopAICarRace(){
	//	previousLength = AICarAnimation[currentAniName].length;
	//	tempPosition= previousLength;
	//	currentAniName = string.Empty;
		_raceState = RaceState.FINISH;
		RankCheck(2);
		AiSpeed = ()=>{
			aniStateRace.speed = AiAniSpeed;
		};
		StopCoroutine("AILedCheck");
		StartCoroutine("DecreaseAnispeed");
		//Utility.LogWarning("StopAICarRace");
	}


	public void StopR2RaceFinish(){
	//	bRaceOver = false;
		if(Global.gRaceInfo.sType == SubRaceType.DragRace){
			StopAICarRace();
			return;
		}


	//	previousLength = AICarAnimation[currentAniName].length;
	//	tempPosition+= previousLength;
		currentAniName =string.Empty;
		RankCheck(2);
		aicaricon.transform.GetComponent<Spin>().enabled  = false;
		_raceState = RaceState.FINISH;
		StopCoroutine("AILedCheck");
		StartCoroutine("DecreaseAnispeed");
		Invoke("AICarSetFalse",2.0f);
		GameManager.instance.mgrgui.userPanelInfo.GetComponent<userinfoaction>().AISlotCtrl(aiCarID);
	

	}


	IEnumerator AICarWheelStopPos(){
		if(Global.gRaceInfo.sType == SubRaceType.CityRace){
			yield return new WaitForSeconds((2.458f-0.5f));
			aicaricon.transform.GetComponent<Spin>().enabled = false;
			isWheel = false;
		}else if(Global.gRaceInfo.sType == SubRaceType.RegularRace){
			yield return new WaitForSeconds((4.124f-0.5f));
			aicaricon.transform.GetComponent<Spin>().enabled = false;
			isWheel = false;
		}

	}
	public bool isRace02 = false;
	void AnimationUpdate(string stateName){
		switch(stateName){
		case "R1":
		{
			ApplyAiCarSpeed();
			GameManager.instance.CompetitionTimes[aiCheckCarId] = AICarAnimation["R1"].time;
			if(!AICarAnimation.IsPlaying(stateName)){
				if(Global.gRaceInfo.sType == SubRaceType.DragRace) {
				//	StopAICarRace();
				//	previousLength = AICarAnimation[currentAniName].length;
				//	tempPosition= previousLength;
				//	currentAniName = string.Empty;
				//	StopCoroutine("DecreaseAnispeed");
					currentAniName = string.Empty;
					Invoke("AICarSetFalse",2.0f);
					isWheel =false;
					AiAniSpeed = 0.0f;
					return;
				}else{
					isRace02 = true;
					AiAniSpeed = 1.0f;
					previousLength = AICarAnimation[currentAniName].length;
					currentAniName = "PitIn";
					tempPosition+= previousLength;
					RankCheck(1);
					_raceState = RaceState.PITIN;
					StopCoroutine("AILedCheck");
					StartCoroutine("AIPitInCrew1");
					StartCoroutine("AICarWheelStopPos");
				}
			}
		}break;
		case "R2":{
			ApplyAiCarSpeed();
			GameManager.instance.CompetitionTimes[aiCheckCarId] = AICarAnimation["R2"].time;
			if(!AICarAnimation.IsPlaying(stateName)){
			//	previousLength = AICarAnimation[currentAniName].length;
			//	tempPosition+= previousLength;
			//	aniStateRace = aniStateExtra;
			//	currentAniName =string.Empty;
			//	RankCheck(2);
			//	aicaricon.transform.GetComponent<Spin>().enabled  = false;
			//	_raceState = RaceState.FINISH;
			//	StopCoroutine("AILedCheck");
			//	StartCoroutine("DecreaseAnispeed");
			//	GameManager.instance.mgrgui.userPanelInfo.GetComponent<userinfoaction>().AISlotCtrl(aiCarID);
				StopCoroutine("DecreaseAnispeed");
				isWheel =false;
				AiAniSpeed = 0.0f;
				Invoke("AICarSetFalse",2.0f);
			
			}
		}break;
		case "PitIn":
		{
			if(AICarAnimation.isPlaying) {
				aniStateExtra.speed = 1.0f;
			}
			if(!AICarAnimation.IsPlaying(stateName)){
				currentAniName = string.Empty;
				isRace = false;
				//previousLength = 0.0f;
				_raceState = RaceState.PITINING;
				PitInAniStart();
			}
		}break;
		case "End":
		{
		//	if(AICarAnimation.isPlaying){
		//		aniStateRace.speed = AiAniSpeed;
		//	}
		//	if(!AICarAnimation.IsPlaying(stateName)){
		//		currentAniName = string.Empty;
		//		previousLength = 0.0f;
			//	StopCoroutine("DecreaseAnispeed");
			//	Invoke("AICarSetFalse",2.0f);
		//	}
		}break;
		default:
			break;
		}
	}

	IEnumerator DecreaseAnispeed(){
	/*	if(Global.isTutorial) {
			for(;;){
				AiAniSpeed  -= 0.004f*GV.ChSeason; 
				yield return new WaitForSeconds(0.05f);
				if(AiAniSpeed< 1.0f){
					AiAniSpeed= 1.0f;
					yield break;
				}
			}
		}else{
			yield return new WaitForSeconds(1.0f);
			for(;;){
			//	AiAniSpeed -= 0.004f*GV.ChSeason;
				AiAniSpeed -= 0.01f;//*GV.ChSeason;
				if(AiAniSpeed < 0.8f){
					AiAniSpeed = 0.8f;
					StopCoroutine("DecreaseAnispeed");
					break;
				}
				yield return new WaitForSeconds(0.01f);
			}
		}

	*/

		yield return new WaitForSeconds(1.0f);
		for(;;){
			AiAniSpeed -= 0.2f;
			if(AiAniSpeed < 0.8f){
				AiAniSpeed = 0.8f;
				StopCoroutine("DecreaseAnispeed");
				break;
			}
			yield return new WaitForSeconds(0.01f);
		}

	}



	void AICarSetFalse(){
		gameObject.SetActive(false);
	}

	void ApplyAiCarSpeed(){
		AiSpeed();
	}

	float Round_To_float(float val){
		return Mathf.Round(val*100f)/100f;// * 0.01f;
	}

	float tempPosition = 0;
	void finishPassSound(){
		EffectSound _effect;
		_effect = GetComponent<EffectSound>();
		_effect.FinishLinePassSound();
	}



	public void SaveSpriteName(int _rank){
		GameManager.instance.rankSpritNames[_rank] = AiCarName;
	}

	// Update is called once per frame
	protected void MinMapMyCar(){
		if(Global.gRaceInfo.sType == SubRaceType.DragRace){
			aicarAnitime = AICarAnimation["R1"].time;
		}else{
			aicarAnitime = AICarAnimation["R1"].time + AICarAnimation["R2"].time+tempPosition;
		}
		/*	if(Global.gRaceInfo.modeType == RaceModeType.DragMode){
			aicarAnitime = AICarAnimation["R1"].time;
		}else{
			aicarAnitime = AICarAnimation["R1"].time + AICarAnimation["R2"].time+previousLength;
		}*/
		float _temp = aicarinitPos+(aicarAnitime/aicarPos) *(640f);
		aiPos.localPosition = new Vector3(_temp
		                                  ,aiPos.localPosition.y, aiPos.localPosition.z);
	}

	void FixedUpdate () {
		if(isPause) return;
		if(!isMiniMap) {
			rotateWheel(pace_Speed);
			return;
		}
		_carRace.CarRace(_raceState);
		if(_raceState != RaceState.PITINING)
		if(isWheel) {
			rotateWheel(AiAniSpeed);
		}
	}

	void Update(){
		if(!isMiniMap)
			return;
		if(_raceState != RaceState.FINISH)
			MinMapMyCar();
		AnimationUpdate(currentAniName);
	}
	private bool isWheel = false;
	private bool bRaceOver = false;
	public void SetSkidEffect(){
		var skid01 = Instantiate(skid[0]) as GameObject;
		//var skidParent = gameObject.transform.GetChild(0).GetChild(0).FindChild(AiCarName).GetChild(0).FindChild("Skid_Pos_L").gameObject;
		var skidParent = AICarObject.GetComponent<CarType>().Skid_L as GameObject;
		ObjectManager.ChangeObjectParent(skid01,skidParent.transform);
		ObjectManager.ChangeObjectPosition(skid01, Vector3.zero, Vector3.one, Vector3.zero);
		var skid02 = Instantiate(skid[0]) as GameObject;
		//skidParent = gameObject.transform.GetChild(0).GetChild(0).FindChild(AiCarName).GetChild(0).FindChild("Skid_Pos_R").gameObject;
		skidParent =AICarObject.GetComponent<CarType>().Skid_R as GameObject;
		ObjectManager.ChangeObjectParent(skid02,skidParent.transform);
		ObjectManager.ChangeObjectPosition(skid02, Vector3.zero, Vector3.one, Vector3.zero);
		StartCoroutine(SkidDisable(skid01));
		StartCoroutine(SkidDisable(skid02));
	}
	bool isSkid = false;
	IEnumerator SkidDisable(GameObject obj){
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
	public void UnSetSkidEffect(){
		isSkid = true;
	}
	


	void OnTriggerEnter(Collider col){
		//Utility.LogWarning (gameObject.name);
	}

	float rotateTime = 0;
	public void rotateWheel(float _aniSpeed){
		if(_aniSpeed <=  0){
			rotateTime = Time.deltaTime*100*30*1;
		}else{
			rotateTime = Time.deltaTime*100*30*GameManager.instance._AniSpeed;
		}
		flWheel.Rotate(0,0,rotateTime);
		frWheel.Rotate(0,0,rotateTime);
		rlWheel.Rotate(0,0,rotateTime);
		rrWheel.Rotate(0,0,rotateTime);
	}


	bool isPause = false;
	float tempSpeed = 0.0f;
	void OnPauseMessage(){
		isPause  = true;
		tempSpeed = AiAniSpeed;
		AiAniSpeed = 0.0f;
		if(gameObject.name == "FaceCar"){
			AICarAnimation[gameObject.name].speed =0.0f;
		}else{
		}
	}
	
	void OnResumeMessage(){
		isPause = false;
		AiAniSpeed = tempSpeed;
		if(gameObject.name == "FaceCar"){
			AICarAnimation[gameObject.name].speed =1.15f;
		}else{
			//AniSpeedApply();
		}

	}

	void OnDestroy(){
		//Destroy(_curve);
		AiCarName = null;
		aicaricon = null;
		flWheel = null;
		frWheel = null;
		rlWheel = null;
		rrWheel = null;
		AICrew = null;
		AICarAnimation = AiCarAni = null;
	}


	void BoostEvent(){
		StartCoroutine("BoostEnable");
	}

	public IEnumerator BoostEnable(){

		if(!_carRace.boostAble) yield break;
		speedBoost.SetActive(true);
		float temp = _carRace.MaxEngineRPM;
		float temp1 = _carRace.CheckEngineRPM;
		_carRace.CheckEngineRPM = temp1 + (float)Base64Manager.instance.GlobalEncoding(Global.rpmAlpah);
		_carRace.MaxEngineRPM = temp + (float)Base64Manager.instance.GlobalEncoding(Global.rpmAlpah);
		float temp2 = _carRace.motorInputTorque;
		_carRace.motorInputTorque = Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[aiCarID].B_Power,0.001f)+temp2;
		SetBoostEffect(true);
		float delay = Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[aiCarID].B_Time,0.001f);
		yield return new WaitForSeconds(delay);
		UnSetBoostEffect();
		_carRace.MaxEngineRPM = temp;
		_carRace.motorInputTorque = temp2;
		_carRace.CheckEngineRPM = temp1;
		speedBoost.SetActive(false);
	}
	void UnSetBoostEffect(){
		isBoost = false;
	}
	
	bool isBoost = false;
	void SetBoostEffect(bool b){
		//var temp = AICarObject.transform.GetChild(0).FindChild("TireGroup_Axis_R").GetChild(0).GetChild(0).FindChild("Boost") as Transform;
		Transform[] _boost  = AICarObject.GetComponent<CarType>().boosts;
		//int count = temp.childCount;
		int count = _boost.Length;
		GameObject[] boostItem = new GameObject[count];
		GameObject boostNozzle = null;

		if(!b){
			boostNozzle = nozzle;
		}else{
			boostNozzle = boost;
			isBoost = true;
		}
		switch(count){
		case 2:
		{

			boostItem[0] = Instantiate(boostNozzle) as GameObject;
			boostItem[0].transform.parent = _boost[0];//temp.GetChild(0);
			ObjectManager.ChangeObjectPosition(boostItem[0], Vector3.zero, Vector3.one, Vector3.zero);
			boostItem[1]= Instantiate(boostNozzle) as GameObject;
			boostItem[1].transform.parent = _boost[1];//temp.GetChild(1);
			ObjectManager.ChangeObjectPosition(boostItem[1], Vector3.zero, Vector3.one, Vector3.zero);
		}
			break;
		case 4:{
			boostItem[0] = Instantiate(boostNozzle) as GameObject;
			boostItem[0].transform.parent = _boost[0];// temp.GetChild(0);
			ObjectManager.ChangeObjectPosition(boostItem[0], Vector3.zero, Vector3.one, Vector3.zero);
			boostItem[1] = Instantiate(boostNozzle) as GameObject;
			boostItem[1].transform.parent = _boost[1];// temp.GetChild(1);
			ObjectManager.ChangeObjectPosition(boostItem[1], Vector3.zero, Vector3.one, Vector3.zero);
			boostItem[2] =Instantiate(boostNozzle) as GameObject;
			
			boostItem[2].transform.parent = _boost[2];// temp.GetChild(2);
			ObjectManager.ChangeObjectPosition(boostItem[2], Vector3.zero, Vector3.one, Vector3.zero);
			boostItem[3] = Instantiate(boostNozzle) as GameObject;
			boostItem[3].transform.parent = _boost[3];// temp.GetChild(3);
			ObjectManager.ChangeObjectPosition(boostItem[3], Vector3.zero, Vector3.one, Vector3.zero);
			
		}
			break;
			
		case 8:{
			boostItem[0] = Instantiate(boostNozzle) as GameObject;
			boostItem[1] = Instantiate(boostNozzle) as GameObject;
			boostItem[2] = Instantiate(boostNozzle) as GameObject;
			boostItem[3] = Instantiate(boostNozzle) as GameObject;
			
			boostItem[0].transform.parent =  _boost[0];//temp.GetChild(0);
			ObjectManager.ChangeObjectPosition(boostItem[0], Vector3.zero, Vector3.one, Vector3.zero);
			boostItem[1].transform.parent = _boost[1];// temp.GetChild(1);
			ObjectManager.ChangeObjectPosition(boostItem[1], Vector3.zero, Vector3.one, Vector3.zero);
			boostItem[2].transform.parent = _boost[2];// temp.GetChild(2);
			ObjectManager.ChangeObjectPosition(boostItem[2], Vector3.zero, Vector3.one, Vector3.zero);
			boostItem[3].transform.parent = _boost[3];// temp.GetChild(3);
			ObjectManager.ChangeObjectPosition(boostItem[3], Vector3.zero, Vector3.one, Vector3.zero);
			
			boostItem[4] = Instantiate(boostItem[0]) as GameObject;
			boostItem[5] =  Instantiate(boostItem[0]) as GameObject;
			boostItem[6] = Instantiate(boostItem[0]) as GameObject;
			boostItem[7] = Instantiate(boostItem[0]) as GameObject;
			
			boostItem[4].transform.parent = _boost[4];// temp.GetChild(4);
			ObjectManager.ChangeObjectPosition(boostItem[4], Vector3.zero, Vector3.one, Vector3.zero);
			boostItem[5].transform.parent =  _boost[5];//temp.GetChild(5);
			ObjectManager.ChangeObjectPosition(boostItem[5], Vector3.zero, Vector3.one, Vector3.zero);
			boostItem[6].transform.parent =  _boost[6];//temp.GetChild(6);
			ObjectManager.ChangeObjectPosition(boostItem[6], Vector3.zero, Vector3.one, Vector3.zero);
			boostItem[7].transform.parent = _boost[7];// temp.GetChild(7);
			ObjectManager.ChangeObjectPosition(boostItem[7], Vector3.zero, Vector3.one, Vector3.zero);
			
			
			
		}
			break;
		}
		if(!b){
		}else{
			StartCoroutine(BoostDisable(boostItem));
		}
		
	}
	IEnumerator BoostDisable(GameObject[] boost){
		_speedEffect_Add1.transform.gameObject.SetActive(true);
		_speedEffect_Add1.emissionRate = 200f;
		for(;;){

			if(!isBoost) {
				foreach(GameObject obj in boost){
					obj.SetActive(false);
				}
				_speedEffect_Add1.emissionRate = 0f;
				_speedEffect_Add1.transform.gameObject.SetActive(false);
				yield break;
			}
			yield return null;
		}

	}

} // end of Class



