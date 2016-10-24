using UnityEngine;
using System.Collections;
[System.Serializable]
public class CarSpeed {
	public WheelCollider FrontLeftWheel;
	public float[] GearRatio;
	public int CurrentGear;		
	public float EngineTorque ;
	public float MaxEngineRPM ;
	public float MinEngineRPM ;
	public float CheckEngineRPM ;
	public float EngineRPM ;
	public float motorInputTorque ;
	public float brakePower ;
	public float currentSpeed ;
	public float _anispeedcheck;
	public float[] gearLED;
	private float tempGear2;
	public float arrowSpeed;
	public float tempBrakePower{get;set;}
	public CarSpeed(){
		tempBrakePower = 0;
	}

	private void SetGearLED(){
		gearLED  = new float[10];
		for(int i = 0; i < 10; i++){
			gearLED[i] = Base64Manager.instance.getFloatEncoding(Global.gLedTime[i],0.001f);
		}
	
	}
	void InitStockCar(){
		CurrentGear = 2;		
		MaxEngineRPM = Base64Manager.instance.GlobalEncoding(Global.gMaxRPM);
		MinEngineRPM = 800.0f;
		CheckEngineRPM = Base64Manager.instance.GlobalEncoding(Global.gCheckRPM);
		EngineRPM = 0.0f;
		motorInputTorque = Base64Manager.instance.getFloatEncoding(Global.gTorque,0.001f);
		brakePower = 100.0f;
		EngineTorque = 600;
		//, 4, 3, 2, 1, 0.9f,0.8f};
		if(Global.isRaceTest){
			GearRatio = new float[10] {5, 4.31f,2.71f, 1.88f, 1.41f,1.13f,0.93f,0.7f,0.6f,0.5f};
			tempGear2 = 2.71f;
		}else{
			GearRatio = new float[10];
			GearRatio[0] = 5;
			for(int  i = 1; i < GearRatio.Length; i++){
				GearRatio[i] = Base64Manager.instance.getFloatEncoding(Global.gGearRatio[i-1],0.001f);
			}
			tempGear2 = GearRatio[2];
			GearRatio[2] = (GearRatio[2]+GearRatio[3]) / 2.0f;
		}
		SetGearLED();
		RaceUpdate = ()=>{
			StockRaceUpdate();
		};
	}

	void InitDragCar(){
		CurrentGear = 0;		
		MaxEngineRPM = Base64Manager.instance.GlobalEncoding(Global.gMaxRPM);
		//MaxEngineRPM = 3000;
		MinEngineRPM = 800.0f;
		CheckEngineRPM = Base64Manager.instance.GlobalEncoding(Global.gCheckRPM);
		//CheckEngineRPM = 2500;
		EngineRPM = 0.0f;
		motorInputTorque = Base64Manager.instance.getFloatEncoding(Global.gTorque,0.001f);
		
		brakePower = 100.0f;
		EngineTorque = 600;
		
		if(Global.isRaceTest){
			GearRatio = new float[10] {5, 4.31f,2.71f, 1.88f, 1.41f,1.13f,0.93f,0.7f,0.6f,0.5f};
			tempGear2 = 2.71f;
		}else{
			GearRatio = new float[10];
			GearRatio[0] = 5;
			for(int  i = 1; i < GearRatio.Length; i++){
				GearRatio[i] = Base64Manager.instance.getFloatEncoding(Global.gGearRatio[i-1],0.001f);
			}
		}

		SetGearLED();
		RaceUpdate = ()=>{
			DragRaceUpdate();
		};
	}



	void InitEventCar(){
			CurrentGear = 0;		
			MaxEngineRPM = Base64Manager.instance.GlobalEncoding(Global.gMaxRPM);
			MinEngineRPM = 800.0f;
			CheckEngineRPM = Base64Manager.instance.GlobalEncoding(Global.gCheckRPM);
			EngineRPM = 0.0f;
			motorInputTorque = Base64Manager.instance.getFloatEncoding(Global.gTorque,0.001f);
			
			brakePower = 100.0f;
			EngineTorque = 600;
			
			if(Global.isRaceTest){
				GearRatio = new float[10] {5, 4.31f,2.71f, 1.88f, 1.41f,1.13f,0.93f,0.7f,0.6f,0.5f};
				tempGear2 = 2.71f;
			}else{
				GearRatio = new float[10];
				GearRatio[0] = 5;
				for(int  i = 1; i < GearRatio.Length; i++){
					GearRatio[i] = Base64Manager.instance.getFloatEncoding(Global.gGearRatio[i-1],0.001f);
				}
			}
			tempGear2 = GearRatio[2];
			SetGearLED();
			RaceUpdate = ()=>{
				EventRaceUpdate();
			};	
	
	}

	public void init(){
		switch(Global.gRaceInfo.sType){
		case SubRaceType.DragRace:
			InitDragCar();
			break;
		case SubRaceType.RegularRace:
			InitStockCar();
			break;
		case SubRaceType.CityRace:
			InitEventCar();
			break;
		default:
			break;
		}
	}

	public void chagneGear2(){
		GearRatio[2] = tempGear2;
	}

	float tempSpeed = 0.0f;
	int speedcnt = 20;
	System.Action RaceUpdate;
	public void CarRace(){
		if(RaceUpdate == null) return;
		RaceUpdate();
	}

	void StockRaceUpdate(){
		_anispeedcheck = GameManager.instance._AniSpeed;
		EngineRPM = (FrontLeftWheel.rpm + FrontLeftWheel.rpm)/2.0f *GearRatio[CurrentGear];
		float _pitch =  Mathf.Abs((EngineRPM / MaxEngineRPM)) + 0.4f ;

		switch(GameManager.instance.RaceState){
		case GameManager.GAMESTATE.RACE01:
		{
			currentSpeed = (Mathf.PI * 2 *FrontLeftWheel.radius)*FrontLeftWheel.rpm*60/1000;
			arrowSpeed = Mathf.Round(currentSpeed);	
			if(currentSpeed > 450) currentSpeed = 450;
			arrowSpeed =  Mathf.Round(arrowSpeed);	
			EnginRPMUpdate();
			aniSpeedCalculation();
			_pitch = Mathf.Clamp(_pitch,1.0f,1.5f);AudioManager.Instance.caraudiopitch = _pitch ;
		}
			break;
		case GameManager.GAMESTATE.RACE02:
		{
			currentSpeed = (Mathf.PI * 2 *FrontLeftWheel.radius)*FrontLeftWheel.rpm*60/1000;
			if(speedcnt > 0){
				speedcnt--;
			}
			arrowSpeed = Mathf.Round(currentSpeed-speedcnt);	
			if(currentSpeed > 450) currentSpeed = 450;
			arrowSpeed =  Mathf.Round(arrowSpeed+speedcnt);	
			EnginRPMUpdate();
			aniSpeedCalculation();
			_pitch = Mathf.Clamp(_pitch,1.0f,1.5f);
			AudioManager.Instance.caraudiopitch = _pitch ;
		}
			break;
		case GameManager.GAMESTATE.PITIN:
		{
			CurrentGear = 0;
			currentSpeed = 0;
			arrowSpeed = 0.0f;
			GameManager.instance._AniSpeed = 1;
			FrontLeftWheel.brakeTorque = 100f;
			FrontLeftWheel.motorTorque = 0.0f;
			AudioManager.Instance.audioPitch(1.0f);
		}
			break;
			
		case GameManager.GAMESTATE.PITINEND:
		{
			currentSpeed =0 ;	arrowSpeed = 0.0f;
			CurrentGear = 0;
			EnginMinInit();
			AudioManager.Instance.audioPitch(1.0f);
		}
			break;
		case GameManager.GAMESTATE.RACE02READY:
		{
			currentSpeed =0 ;	arrowSpeed = 0.0f;
			EnginMinInit();
			aniSpeedCalculation();
		}
			break;
		case GameManager.GAMESTATE.READY:
		{
			currentSpeed = 80;	arrowSpeed = 80.0f;
			GameManager.instance._AniSpeed = 1.0f;
			EnginMaxInit();
			//aniSpeedCalculation();
			_pitch = Mathf.Clamp(_pitch,1.0f,1.2f);
			AudioManager.Instance.caraudiopitch = _pitch ;
			AudioManager.Instance.SetCarVolume(0.6f);
		}
			break;
		case GameManager.GAMESTATE.FINISH:
		{
			currentSpeed = 80.0f;
			arrowSpeed = 80.0f;
			CurrentGear = 0;
			motorInputTorque = 0;
			EnginRPMUpdate();
		}
			break;
		case GameManager.GAMESTATE.RACE02DELAY:
		{
			GameManager.instance._AniSpeed = 0.0f;
			currentSpeed =0;
			arrowSpeed = 0;
			EnginDelay();
		}
			break;
		default:
			break;
		}
	}


	void EventRaceUpdate(){
		_anispeedcheck = GameManager.instance._AniSpeed;
		EngineRPM = (FrontLeftWheel.rpm + FrontLeftWheel.rpm)/2.0f *GearRatio[CurrentGear];
		float _pitch =  Mathf.Abs((EngineRPM / MaxEngineRPM)) + 0.4f ;
		switch(GameManager.instance.RaceState){
		case GameManager.GAMESTATE.RACE01:
		{
			currentSpeed = (Mathf.PI * 2 *FrontLeftWheel.radius)*FrontLeftWheel.rpm*60/1000;
			arrowSpeed = Mathf.Round(currentSpeed);	
			if(currentSpeed > 450) currentSpeed = 450;
			arrowSpeed =  Mathf.Round(arrowSpeed);	
			EnginRPMUpdate();
			aniSpeedCalculation();
			_pitch = Mathf.Clamp(_pitch,1.0f,1.5f);
			AudioManager.Instance.caraudiopitch = _pitch ;
		}
			break;
		case GameManager.GAMESTATE.RACE02:
		{
			currentSpeed = (Mathf.PI * 2 *FrontLeftWheel.radius)*FrontLeftWheel.rpm*60/1000;
			if(speedcnt > 0){
				speedcnt--;
			}
			arrowSpeed = Mathf.Round(currentSpeed-speedcnt);	
			if(currentSpeed > 450) currentSpeed = 450;
			arrowSpeed =  Mathf.Round(arrowSpeed+speedcnt);	
			EnginRPMUpdate();
			aniSpeedCalculation();
			_pitch = Mathf.Clamp(_pitch,1.0f,1.5f);
			AudioManager.Instance.caraudiopitch = _pitch ;
		}
			break;
		case GameManager.GAMESTATE.PITIN:
		{
			CurrentGear = 0;
			currentSpeed = 0;
			arrowSpeed = 0.0f;
			GameManager.instance._AniSpeed = 1;
			FrontLeftWheel.brakeTorque = 100f;
			FrontLeftWheel.motorTorque = 0.0f;
			AudioManager.Instance.audioPitch(1.0f);
		}
			break;
			
		case GameManager.GAMESTATE.PITINEND:
		{
			currentSpeed =0 ;	arrowSpeed = 0.0f;
			CurrentGear = 0;
			EnginMinInit();
			AudioManager.Instance.audioPitch(1.0f);
		}
			break;
		case GameManager.GAMESTATE.RACE02READY:
		{
			currentSpeed =0 ;	arrowSpeed = 0.0f;
			EnginMinInit();
			aniSpeedCalculation();
		}
			break;
		case GameManager.GAMESTATE.RACE01READY:
		{
			currentSpeed =0 ;	arrowSpeed = 0.0f;
			EnginMinInit();
		}
			break;
		case GameManager.GAMESTATE.RACE01DELAY:
		{
			GameManager.instance._AniSpeed = 0.0f;
			currentSpeed =0;
			arrowSpeed = 0;
			EnginDelay();
		}
			break;
		case GameManager.GAMESTATE.FINISH:
		{
			currentSpeed = 80.0f;
			arrowSpeed = 80.0f;
			CurrentGear = 0;
			motorInputTorque = 0;
			EnginRPMUpdate();
		}
			break;
		case GameManager.GAMESTATE.RACE02DELAY:
		{
			GameManager.instance._AniSpeed = 0.0f;
			currentSpeed =0;
			arrowSpeed = 0;
			EnginDelay();
		}
			break;
		case GameManager.GAMESTATE.READY:
			GameManager.instance._AniSpeed = 0.0f;
			currentSpeed =0 ;	arrowSpeed = 0.0f;
			CurrentGear = 0;
			EnginMinInit();
			break;
		case GameManager.GAMESTATE.READYEND:
			GameManager.instance._AniSpeed = 0.0f;
			currentSpeed =0 ;	arrowSpeed = 0.0f;
			CurrentGear = 0;
			EnginMinInit();
			AudioManager.Instance.audioPitch(1.0f);
			break;
		default:
			break;
		}
	}


	void DragRaceUpdate(){
		_anispeedcheck = GameManager.instance._AniSpeed;
		EngineRPM = (FrontLeftWheel.rpm + FrontLeftWheel.rpm)/2.0f *GearRatio[CurrentGear];
		float _pitch =  Mathf.Abs((EngineRPM / MaxEngineRPM)) + 0.4f ;
		switch(GameManager.instance.RaceState){
		case GameManager.GAMESTATE.RACE01:
		{
			currentSpeed = (Mathf.PI * 2 *FrontLeftWheel.radius)*FrontLeftWheel.rpm*60/1000;
			arrowSpeed = Mathf.Round(currentSpeed);	
			if(currentSpeed > 450) currentSpeed = 450;
			arrowSpeed =  Mathf.Round(arrowSpeed);	

		//	Utility.LogWarning(currentSpeed);
			EnginRPMUpdate();
			aniSpeedCalculation();
			_pitch = Mathf.Clamp(_pitch,1.0f,1.5f);AudioManager.Instance.caraudiopitch = _pitch ;
		}
			break;
		case GameManager.GAMESTATE.RACE01READY:
		{
			currentSpeed =0 ;	arrowSpeed = 0.0f;
			EnginMinInit();
		}
			break;
		case GameManager.GAMESTATE.RACE01DELAY:
		{
			GameManager.instance._AniSpeed = 0.0f;
			currentSpeed =0;
			arrowSpeed = 0;
			EnginDelay();
		}
			break;
		case GameManager.GAMESTATE.FINISH:
		{
			currentSpeed = 80.0f;
			arrowSpeed = 80.0f;
			CurrentGear = 0;
			motorInputTorque = 0;
			EnginRPMUpdate();
		}
			break;
		case GameManager.GAMESTATE.READY:
			GameManager.instance._AniSpeed = 0.0f;
			currentSpeed =0 ;	arrowSpeed = 0.0f;
			CurrentGear = 0;
			EnginMinInit();
			break;
		case GameManager.GAMESTATE.READYEND:
			GameManager.instance._AniSpeed = 0.0f;
			currentSpeed =0 ;	arrowSpeed = 0.0f;
			CurrentGear = 0;
			EnginMinInit();
			AudioManager.Instance.audioPitch(1.0f);
			break;
		default:
			break;
		}
	}




	public void aniSpeedCalculation(){
		if(arrowSpeed == 80){
			GameManager.instance._AniSpeed = 1;
		}else if(arrowSpeed > 1 && arrowSpeed <= 79){
			GameManager.instance._AniSpeed =1.0f-(80-arrowSpeed)*0.005f;
			//GameManager.instance._AniSpeed =1.0f-(80-currentSpeed)*0.01f;
		}else if(arrowSpeed >=81){
			//GameManager.instance._AniSpeed = (currentSpeed - 80)*0.005f+1.0f;
			GameManager.instance._AniSpeed = (arrowSpeed - 80)*0.005f+1.0f;

		}
		if(arrowSpeed <= 0) {
			GameManager.instance._AniSpeed = 1.0f;
			AudioManager.Instance.SetCarVolume(0.0f);
			currentSpeed = 0;
		}
		
		//Utility.Log(string.Format("{0}, {1}, {2}, ", currentSpeed, EngineRPM, FrontLeftWheel.rpm));
	}

	 void EnginMaxInit(){
		if(EngineRPM < MaxEngineRPM){
			FrontLeftWheel.motorTorque = 30f;//*GameManager.Instance().AniSpeed;
			FrontLeftWheel.brakeTorque = 0.0f;
		}else{
			FrontLeftWheel.motorTorque = 0.0f;
		}
	}

	void EnginDelay(){
		if(EngineRPM > MinEngineRPM){
			FrontLeftWheel.motorTorque = 0.0f;
			FrontLeftWheel.brakeTorque = brakePower;
		}else{
			FrontLeftWheel.brakeTorque = 0.0f;
			FrontLeftWheel.motorTorque = motorInputTorque;//*GameManager.Instance().AniSpeed;
		}
	}
	
	 void EnginMinInit(){
		if(GameManager.instance._isPress){
			if(EngineRPM < MaxEngineRPM){
				FrontLeftWheel.motorTorque = motorInputTorque;//*GameManager.Instance().AniSpeed;
				FrontLeftWheel.brakeTorque = 0.0f;
			}else{
				FrontLeftWheel.motorTorque = 0.0f;
				FrontLeftWheel.brakeTorque = brakePower;
			}
			return;
		}
		
		if(EngineRPM > MinEngineRPM){
			FrontLeftWheel.motorTorque = 0.0f;
			FrontLeftWheel.brakeTorque = brakePower;
			
		}else{
			FrontLeftWheel.brakeTorque = 0.0f;
			FrontLeftWheel.motorTorque = motorInputTorque;//*GameManager.Instance().AniSpeed;
		}
		
	}
	
	 void EnginRPMUpdate(){
		if(GameManager.instance._isPress){
			if(EngineRPM < MaxEngineRPM){
				FrontLeftWheel.motorTorque = motorInputTorque;//*GameManager.Instance().AniSpeed;
				FrontLeftWheel.brakeTorque = tempBrakePower;
			}else{
				FrontLeftWheel.motorTorque = 0.0f;
				FrontLeftWheel.brakeTorque = brakePower;
			}
		}else{
			if(EngineRPM > MinEngineRPM){
				FrontLeftWheel.motorTorque = 0.0f;
				FrontLeftWheel.brakeTorque = 50f;
				
			}else{
				FrontLeftWheel.brakeTorque = 0.0f;
				FrontLeftWheel.motorTorque = motorInputTorque;//*GameManager.Instance().AniSpeed;
			}
		} // ispress end of else if 
	}
	
	
	
} // class car speed
