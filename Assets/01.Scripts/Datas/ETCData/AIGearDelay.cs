using UnityEngine;
using System.Collections;

public class AIGearDelay : MonoBehaviour {


	public void NewMakeGearDelay(int id){
		Global.gAICarInfo[id].gearLedDelay = new float[20];
		int cnt = Global.gAICarInfo[id].gearLedDelay.Length;
		
		// AI 카는 랭킹을 제외하고는 0~0.5f;
		// 챔피언스 모드, 데일리 , 피쳐드 모드 0.2로 제공..
		MainRaceType racetype = Global.gRaceInfo.mType;
		int modeID = 1450;
		switch(racetype){
		case MainRaceType.Champion:
			modeID = 1450;
			//SetChampionDelay(cnt,id);
			break;
		case MainRaceType.Club:
			modeID = 1455;
		//	SetClubDelay(cnt, id);
			break;
		case MainRaceType.mEvent:
			modeID = 1453;
			//SetEventDelay(cnt,id);
			break;
		case MainRaceType.Regular:
			modeID = 1451;
		//	SetRegularDelay(cnt,id);
			break;
		case MainRaceType.Weekly:
			modeID = 1454;
			//SetWeeklyDelay(cnt, id);
			break;
		case MainRaceType.PVP:
			modeID = 1452;
			//SetPVPDelay(cnt, id);
			break;
		default:
			modeID = 1456;
			//SetRegularDelay(cnt, id);
			break;
		}

		if(modeID == 1456){
			SetAIDelay(cnt, id, 15,15);
			Global.gCtrlDelay = 0.3f;
			SetCtrlPressDelay(0.3f,id);
		}else{
			ModeAIDelay.Item _item = ModeAIDelay.Get(modeID);
			SetAIDelay(cnt, id, _item.Re_Max, _item.Re_Min);
			Global.gCtrlDelay = _item.Ctrl_Delay;
			SetCtrlPressDelay(_item.Ctrl_Delay, id);
		}
		//Utility.Log("makeGearDelay " + id);	
		Invoke("disableScript",0.25f);
	}

	private void SetAIDelay(int cnt, int id, int max, int min){
		float r = 0;
		int n = 0;
		for(int  i = 0; i < cnt ; i++){
			n = Random.Range(15,20);
			r = (float)n*0.01f;
			Global.gAICarInfo[id].gearLedDelay[i] = r;
		}
	}


	void disableScript(){
		Destroy(this);
	}

	private void SetCtrlPressDelay(float indexs,int id){
		Global.gCtrlDelays = new float[20];
		Global.gAICarInfo[id].pressDelay = new float[20];
		for(int i=0; i < 20; i++){
			Global.gCtrlDelays[i] = indexs;
			Global.gAICarInfo[id].pressDelay[i] = indexs;
		}
	}

	private void SetChampionDelay(int cnt, int id, int max, int min){
		float r = 0;
		int n = 0;
		for(int  i = 0; i < cnt ; i++){
			n = Random.Range(max,min);
			r = (float)n*0.01f;
			//Global.gAICarInfo[id].gearLedDelay[i] = UserDataManager.instance.AIGearDelay;
			Global.gAICarInfo[id].gearLedDelay[i] = r;
		}
	}

	private void SetClubDelay(int cnt, int id, int max, int min){
		float r = 0;
		int n = 0;
		for(int  i = 0; i < cnt ; i++){
			n = Random.Range(max,min);
			r = (float)n*0.01f;
			Global.gAICarInfo[id].gearLedDelay[i] = r;
		}
	}
	private void SetEventDelay(int cnt, int id, int max, int min){
		float r = 0;
		int n = 0;
		for(int  i = 0; i < cnt ; i++){
			n = Random.Range(max,min);
			r = (float)n*0.01f;
			Global.gAICarInfo[id].gearLedDelay[i] = r;
		}
	}
	private void SetRegularDelay(int cnt, int id, int max, int min){
		float r = 0;
		int n = 0;
		for(int  i = 0; i < cnt ; i++){
			n = Random.Range(max,min);
			r = (float)n*0.01f;
			Global.gAICarInfo[id].gearLedDelay[i] = r;
		}
	}
	private void SetPVPDelay(int cnt, int id){
		float r = 0;
		int n = 0;
		for(int  i = 0; i < cnt ; i++){
			n = Random.Range(15,20);
			r = (float)n*0.01f;
			Global.gAICarInfo[id].gearLedDelay[i] = r;
		}
	}
	private void SetWeeklyDelay(int cnt, int id, int max, int min){
		float r = 0;
		int n = 0;
		for(int  i = 0; i < cnt ; i++){
			n = Random.Range(15,20);
			r = (float)n*0.01f;
			Global.gAICarInfo[id].gearLedDelay[i] = r;
		}
	}



	public void RankRaceGearLedSetting(float[] leds, int id){
		Global.gAICarInfo[id].gearLedDelay = new float[20];
		System.Array.Copy(leds,Global.gAICarInfo[id].gearLedDelay, leds.Length);
	}

	public void RankRaceCtrlPressDelaySetting(float[] ctrlDelay, int id){
		Global.gCtrlDelays = new float[20];
		System.Array.Copy(ctrlDelay,Global.gCtrlDelays, ctrlDelay.Length);
	}

}
