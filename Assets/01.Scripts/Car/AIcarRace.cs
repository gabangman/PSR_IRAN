using UnityEngine;
using System.Collections;

[System.Serializable]
public class AIcarRace  {
		public WheelCollider FrontLeftWheel;
		public float[] GearRatio;
		public int CurrentGear;		
		public float[] LedTime;
		public float MaxEngineRPM ;
		public float MinEngineRPM ;
		public float CheckEngineRPM ;
		public float EngineRPM ;
		public float motorInputTorque ;
		public float brakePower ;
		public float currentSpeed ;
		public int maxGear;
		public float  tempBrakePower{get;set;}
		public float raceSpeed{get{return speed;} private set{}}
		bool isgearcheck;
		public float speed;
		public bool boostAble;
		private float tempGear;
		private float tempTorque;
		public AIcarRace(){
			tempBrakePower = 0;
		}

		public void AIinit(int index){
	/*	switch(Global.gRaceInfo.modeType){
		case RaceModeType.DragMode:
			ModeInit(index);
			break;
		case RaceModeType.EventMode:
			ModeInit(index);
			break;
		case RaceModeType.StockMode:
			StockModeInit(index);
			break;
		case RaceModeType.TouringMode:
			ModeInit(index);
			break;
			}*/
		switch(Global.gRaceInfo.sType){
		case SubRaceType.DragRace:
			ModeInit(index);
			break;
		case SubRaceType.CityRace:
			ModeInit(index);
			break;
		case SubRaceType.RegularRace:
			StockModeInit(index);
			break;
		}
		}

		public void StockModeInit(int index){
		CurrentGear = 2;
		speed = 1.0f;
		brakePower = 100.0f;
		CheckEngineRPM = 2000;
		int mgb =0;
		if(Global.isRaceTest){
			MaxEngineRPM =3000;
			MinEngineRPM = 800.0f;
			EngineRPM = 0.0f;
			motorInputTorque = 20;
			maxGear = 5;	
			GearRatio = new float[10] {5, 4.31f,2.71f, 1.88f, 1.41f,1.13f,0.93f,0.7f,0.6f,0.5f};
			boostAble = true;
			tempGear = 2.71f;
			LedTime = new float[10];
			CheckEngineRPM = 2500;
			mgb = 0;
			LedTime[0] = 0.0f;
			LedTime[1] =0.21f+(-0.01f)*mgb;
			LedTime[2] =0.3f+(-0.01f)*mgb;
			LedTime[3] =0.459f+(-0.01f)*mgb;
			LedTime[4] =0.52f+(-0.01f)*mgb;
			LedTime[5] =2+(-0.01f)*mgb;
			LedTime[6] =0+(-0.01f)*mgb;
			LedTime[7] =0+(-0.01f)*mgb;
			LedTime[8] =0+(-0.01f)*mgb;
			LedTime[9] =0+(-0.01f)*mgb;
			tempTorque =  20;
		}else{
			int carID = 0;
			if(Global.gRaceInfo.mType == MainRaceType.Weekly){
				carID = Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[index].AICarID);
			}else{
				carID =Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[index].AIRefCarID);
			}
			Common_Car_Status.Item carItem = Common_Car_Status.Get(carID);
			Common_Class.Item classItem = Global.gAICarInfo[index].AIClass;
			MaxEngineRPM = carItem.RPMMax + classItem.RPMMax;
			MinEngineRPM = 800.0f;
			motorInputTorque = Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[index].Torque,0.001f);
			motorInputTorque += classItem.Class_power;
			tempTorque =  motorInputTorque;
			maxGear = carItem.GearLmt+classItem.GearLmt;
			CheckEngineRPM = carItem.RPM+classItem.RPM;
			GearRatio = new float[10];
			GearRatio[0] = 5;
			GearRatio[1] =carItem.Gear1+classItem.Gear1;
			//GearRatio[2] = 2.71f;
			GearRatio[2] = (carItem.Gear2+classItem.Gear2+classItem.Gear3+carItem.Gear3)/2.0f;
			//tempGear = (carItem.Gear2+carItem.Gear3)/2.0f;
			tempGear = carItem.Gear2+classItem.Gear2;
			//GearRatio[2] =carItem.Gear2;
			GearRatio[3] =carItem.Gear3+classItem.Gear3;
			GearRatio[4] =carItem.Gear4+classItem.Gear4;
			GearRatio[5] =carItem.Gear5+classItem.Gear5;
			GearRatio[6] =carItem.Gear6+classItem.Gear6;
			GearRatio[7] =carItem.Gear7+classItem.Gear7;
			GearRatio[8] =carItem.Gear8+classItem.Gear8;
			GearRatio[9] =carItem.Gear9+classItem.Gear9;

			mgb =  Global.gAICarInfo[index].gbLv-1;
			LedTime = new float[10];
		
			float upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Gearbox,1,mgb);
			
			LedTime[0] = 0.0f;
			LedTime[1] =carItem.LED2+(upRatio);
			LedTime[2] =carItem.LED3+(upRatio);
			LedTime[3] =carItem.LED4+(upRatio);
			LedTime[4] =carItem.LED5+(upRatio);
			LedTime[5] =carItem.LED6+(upRatio);
			LedTime[6] =carItem.LED7+(upRatio);
			LedTime[7] =carItem.LED8+(upRatio);
			LedTime[8] =carItem.LED9+(upRatio);
			LedTime[9] =carItem.LED10+(upRatio);

			for(int i=1; i < 10; i++){
				if(LedTime[i] < 0.1f) LedTime[i]= 0.1f;
			}
			if(carItem.BsPowerAble == 0) boostAble = false;
			else boostAble = true;
		//	if(Global.MySeason < 2){
		//		boostAble = false;
		//	}
		}
		//GearRatio = new float[10] {5, 4.31f,2.71f, 1.88f, 1.41f,1.13f,0.93f,0.7f,0.6f,0.5f};
		isAIPress = true;
	}

	void ModeInit(int index){
		CurrentGear = 0;
		speed = 0.0f;
		brakePower = 100.0f;
		CheckEngineRPM = 2000;
		int mgb =0;
		if(Global.isRaceTest){
			MaxEngineRPM =3000;
			MinEngineRPM = 800.0f;
			EngineRPM = 0.0f;
			motorInputTorque = 20;
			maxGear = 5;	
			GearRatio = new float[10] {5, 4.31f,2.71f, 1.88f, 1.41f,1.13f,0.93f,0.7f,0.6f,0.5f};
			boostAble = true;
			tempGear = 2.71f;
			LedTime = new float[9];
			CheckEngineRPM = 2500;
			mgb = 0;
			LedTime[0] = 0.0f;
			LedTime[1] =0.21f+(-0.01f)*mgb;
			LedTime[2] =0.3f+(-0.01f)*mgb;
			LedTime[3] =0.459f+(-0.01f)*mgb;
			LedTime[4] =0.52f+(-0.01f)*mgb;
			LedTime[5] =2+(-0.01f)*mgb;
			LedTime[6] =0+(-0.01f)*mgb;
			LedTime[7] =0+(-0.01f)*mgb;
			LedTime[8] =0+(-0.01f)*mgb;
			for(int i=1; i < 10; i++){
				if(LedTime[i] < 0.1f) LedTime[i]= 0.1f;
			}

			tempTorque =  20;
		}else{
			int carID = 0;
			/*if(Global.gRaceInfo.raceType == RaceType.WeeklyMode){
				carID = Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[index].AICarID);
			}else{
				carID =Base64Manager.instance.GlobalEncoding( Global.gAICarInfo[index].AIRefCarID);
			}*/
			if(Global.gRaceInfo.mType == MainRaceType.Weekly){
				carID = Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[index].AICarID);
			}else{
				carID =Base64Manager.instance.GlobalEncoding( Global.gAICarInfo[index].AIRefCarID);
			}
			Common_Car_Status.Item carItem = Common_Car_Status.Get(carID);
			Common_Class.Item classItem = Global.gAICarInfo[index].AIClass;
			MaxEngineRPM = carItem.RPMMax + classItem.RPMMax;
			MinEngineRPM = 800.0f;
			motorInputTorque = Base64Manager.instance.getFloatEncoding(Global.gAICarInfo[index].Torque,0.001f);
			motorInputTorque += classItem.Class_power;
			tempTorque =  motorInputTorque;
			maxGear = carItem.GearLmt+classItem.GearLmt;
			//Utility.LogWarning(maxGear + " max ");
			CheckEngineRPM = carItem.RPM+classItem.RPM;
			GearRatio = new float[10];
			GearRatio[0] = 5;
			GearRatio[1] =carItem.Gear1+classItem.Gear1;
			GearRatio[2] = carItem.Gear2+classItem.Gear2;
			tempGear = carItem.Gear2+classItem.Gear2;
			GearRatio[3] =carItem.Gear3+classItem.Gear3;
			GearRatio[4] =carItem.Gear4+classItem.Gear4;
			GearRatio[5] =carItem.Gear5+classItem.Gear5;
			GearRatio[6] =carItem.Gear6+classItem.Gear6;
			GearRatio[7] =carItem.Gear7+classItem.Gear7;
			GearRatio[8] =carItem.Gear8+classItem.Gear8;
			GearRatio[9] =carItem.Gear9+classItem.Gear9;
			mgb =  Global.gAICarInfo[index].gbLv-1;
			LedTime = new float[10];



			float upRatio = UserDataManager.instance.GetStarLVRatio((int)CarPartID.Gearbox,1,mgb);

			LedTime[0] = 0.0f;
			LedTime[1] =carItem.LED2+(upRatio);
			LedTime[2] =carItem.LED3+(upRatio);
			LedTime[3] =carItem.LED4+(upRatio);
			LedTime[4] =carItem.LED5+(upRatio);
			LedTime[5] =carItem.LED6+(upRatio);
			LedTime[6] =carItem.LED7+(upRatio);
			LedTime[7] =carItem.LED8+(upRatio);
			LedTime[8] =carItem.LED9+(upRatio);
			LedTime[9] =carItem.LED10+(upRatio);
			for(int i=1; i < 10; i++){
				if(LedTime[i] < 0.1f) LedTime[i]= 0.1f;
			}

			if(carItem.BsPowerAble == 0) boostAble = false;
			else boostAble = true;
		}
		isAIPress = true;
	}
	
		public void changeGear2(){
			GearRatio[2] = tempGear;
			CurrentGear = 1;
			isAIPress = true;
			motorInputTorque =tempTorque;
		}	
		float tempSpeed = 0.0f;
		public void CarRace(AICarCtrl.RaceState _RState){
			EngineRPM = (FrontLeftWheel.rpm + FrontLeftWheel.rpm)/2.0f *GearRatio[CurrentGear];
		switch(_RState){
			case AICarCtrl.RaceState.RACE01:
			{
				currentSpeed = (Mathf.PI * 2 *FrontLeftWheel.radius)*FrontLeftWheel.rpm*60/1000;
			    tempSpeed = Mathf.Round(currentSpeed);	
				//tempSpeed =  Mathf.Round(tempSpeed);	
				EnginRPMUpdate();
				aniSpeedCalculation();
			}
				break;
			case AICarCtrl.RaceState.RACE02:
			{
				currentSpeed = (Mathf.PI * 2 *FrontLeftWheel.radius)*FrontLeftWheel.rpm*60/1000;
				tempSpeed = Mathf.Round(currentSpeed);	
				//tempSpeed =  Mathf.Round(currentSpeed);	
				EnginRPMUpdate();
				aniSpeedCalculation();

			}
				break;
			case AICarCtrl.RaceState.PITIN:
			{
				CurrentGear = 0;
				currentSpeed = 0;
				tempSpeed = 0.0f;
				speed= 1;
				FrontLeftWheel.brakeTorque = 100f;
				FrontLeftWheel.motorTorque = 0.0f;
				isAIPress = false;
			}
				break;
				
			case AICarCtrl.RaceState.PITINFINISH:
			{
				currentSpeed =0 ;	tempSpeed = 0.0f;
				CurrentGear = 1;
				EnginMinInit();
			}
				break;
			case AICarCtrl.RaceState.RACE02READY:
			{
				currentSpeed =0 ;	tempSpeed = 0.0f;
				CurrentGear = 1;
				EnginMinInit();
				aniSpeedCalculation();
			}
				break;
			case AICarCtrl.RaceState.READY:
			{
				currentSpeed = 80;	tempSpeed = 80.0f;
				
				speed = 1.0f;
				EnginMaxInit();
				//EnginMinInit();
		
			}
				break;
		case AICarCtrl.RaceState.FINISH:
			{
				motorInputTorque = 0;
				EnginRPMUpdate();
			}
				break;
			case AICarCtrl.RaceState.RACE02DELAY:
			{
				speed = 0.0f;
				currentSpeed =0;
				tempSpeed = 0;
				CurrentGear = 1;
				EnginDelay();
			}
				break;
			case AICarCtrl.RaceState.FINISHSNAPSHOT:
			{
			speed = 1.0f;
			
			}break;

		case AICarCtrl.RaceState.RACE01DELAY:{
			speed = 0.0f;
			currentSpeed =0;
			tempSpeed = 0;
			CurrentGear = 1;
			EnginDelay();
		}
			break;
		case AICarCtrl.RaceState.RACE01READY:{
			currentSpeed =0 ;	tempSpeed = 0.0f;
			CurrentGear = 1;
			EnginMinInit();
			aniSpeedCalculation();
		}
			break;
		case AICarCtrl.RaceState.READYEND:{
		
		}
			break;
		default:
	
				break;
			}
	
	
		}
		
		public void aniSpeedCalculation(){
			if(tempSpeed == 80){
				speed = 1;
			}else if(tempSpeed > 1 && tempSpeed <= 79){
				speed =1.0f-(80-tempSpeed)*0.005f;
			}else if(tempSpeed >=81){
				speed = (tempSpeed - 80)*0.005f+1.0f;
			}
			if(tempSpeed <= 0) {
				speed= 1.0f;
				currentSpeed = 0;
			}
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
				if(EngineRPM < MaxEngineRPM){
					FrontLeftWheel.motorTorque = motorInputTorque;//*GameManager.Instance().AniSpeed;
					FrontLeftWheel.brakeTorque = tempBrakePower;
				}else{
					FrontLeftWheel.motorTorque = 0.0f;
					FrontLeftWheel.brakeTorque = brakePower;
				}
			
			if(EngineRPM > MinEngineRPM){
				FrontLeftWheel.motorTorque = 0.0f;
				FrontLeftWheel.brakeTorque = brakePower;
				
			}else{
				FrontLeftWheel.brakeTorque = 0.0f;
				FrontLeftWheel.motorTorque = motorInputTorque;//*GameManager.Instance().AniSpeed;
			}
		}
		
	void AIEngineUpdate(){

			if(EngineRPM < CheckEngineRPM){
						
			}	
	
	}

	void EnginRPMUpdate(){
		if(isAIPress){
				if(EngineRPM < MaxEngineRPM){
					FrontLeftWheel.motorTorque = motorInputTorque;//*GameManager.Instance().AniSpeed;
					FrontLeftWheel.brakeTorque = 0.0f;
				}else{
					FrontLeftWheel.motorTorque = 0.0f;
					FrontLeftWheel.brakeTorque = brakePower;
				}
		}else{
			if(EngineRPM > MinEngineRPM){
				FrontLeftWheel.motorTorque = 0.0f;
				FrontLeftWheel.brakeTorque = brakePower/2;
				
			}else{
				FrontLeftWheel.brakeTorque = 0.0f;
				FrontLeftWheel.motorTorque = motorInputTorque;//*GameManager.Instance().AniSpeed;
			}
		}
	}
		
	public bool isAIPress = false;
	public bool ShieftGear(){
		if(CurrentGear == maxGear) return false;
		CurrentGear++;
		return true;
	}

	void AIshiftGear(){
		if ( EngineRPM >= (MaxEngineRPM) ) {
			int AppropriateGear  = CurrentGear;
			if(CurrentGear == maxGear) return;
			for ( int i = 1; i < GearRatio.Length; i ++ ) {
				if ( FrontLeftWheel.rpm * GearRatio[i] < (MaxEngineRPM) ) {
					AppropriateGear = i;
					break;
				}
			}
			CurrentGear = AppropriateGear;
		}
	}
} // class car speed