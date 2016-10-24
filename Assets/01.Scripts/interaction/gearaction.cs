using UnityEngine;
using System.Collections;

public class gearaction : InterAction {

	public TweenPosition[] Gear;
	void Start(){
		Gear = gameObject.GetComponentsInChildren<TweenPosition>();
		switch(Global.gRaceInfo.sType){
		case SubRaceType.DragRace:
			initGear(0);
			break;
		case SubRaceType.RegularRace:
			initGear(2);
			break;
	//	case SubRaceType.TouringMode:
	//		initGear(0);
	//		break;
		case SubRaceType.CityRace:
			initGear(0);
			break;
		default:
			break;
		}




	
	}

	void finishDisappear(TweenPosition t){
	
		//GameMgr.instance.isGearChange = false;
		if(t == null){
			Utility.LogError("t == NULL");
		}
		t.transform.gameObject.GetComponent<UISprite>().alpha =1.0f;
		t.transform.gameObject.SetActive(false);
	}
	public void GearDownAction(int gearNum){
		if(gearNum == 0) return;
		TweenPosition tw = Gear[gearNum+1];
		tw.from = Vector3.zero;
		tw.to = new Vector3(0,-40,0);
		
		tw.eventReceiver = gameObject;// tw.transform.gameObject;
		tw.callWhenFinished = "finishDisappear";
		tw.Reset();
		tw.enabled = true;
		
		
		TweenAlpha  twa = tw.transform.gameObject.GetComponent<TweenAlpha>();//TweenAlpha.Begin(tw.transform.gameObject, 0.2f, 0	);
		twa.duration = 0.2f;
		twa.from = 1.0f;
		twa.to = 0.0f;
		twa.delay = 0.0f;
		twa.Reset();
		twa.enabled = true;
	

		TweenPosition tw1 = Gear[gearNum];
		//	tw.transform.gameObject.renderer.material.color. = 0.0f;
		tw1.transform.gameObject.GetComponent<UISprite>().alpha =0.0f;
		tw1.transform.gameObject.SetActive(true);
		tw1.to = Vector3.zero;
		tw1.from = new Vector3(0,+40,0);
		tw1.eventReceiver =null;
		tw1.callWhenFinished = null;
		tw1.Reset();
		tw1.enabled = true;
		
		TweenAlpha  twa1 = tw1.transform.gameObject.GetComponent<TweenAlpha>();//TweenAlpha.Begin(tw.transform.gameObject, 0.2f, 0	);
		twa1.duration = 0.2f;
		twa1.from = 0.0f;
		twa1.to = 1.0f;
		twa1.delay = 0.2f;
		
		twa1.Reset();
		twa1.enabled = true;

	}




	private void initGear(int gearNum){
		foreach(TweenPosition tw in Gear){
			tw.transform.gameObject.SetActive(false);
			tw.transform.gameObject.GetComponent<UISprite>().alpha =1.0f;
		}
		Gear[gearNum].transform.gameObject.SetActive(true);
		b_maxGear = false;
	}

	private bool b_maxGear;
	public void GearChangeAction(int gearNum){
		if(gearNum == 9){
				if(!b_maxGear){
					b_maxGear = true;
					disapperGear(gearNum);
					appearGear(gearNum);
			}	
			}else{
				disapperGear(gearNum);
				appearGear(gearNum);
			}
	}
	private void distorytween(int gearNum){
		TweenPosition tw = Gear[gearNum];
		var twa = tw.transform.gameObject.GetComponent<TweenAlpha>() as TweenAlpha;
		if(twa == null) return;
		else Destroy(twa);
	
	}

	private void appearGear(int gearNum){
		TweenPosition tw = Gear[gearNum];
		tw.transform.gameObject.GetComponent<UISprite>().alpha =0.0f;
		tw.transform.gameObject.SetActive(true);
		tw.to = Vector3.zero;
		tw.from = new Vector3(0,-40,0);
		tw.eventReceiver =null;
		tw.callWhenFinished = null;
		tw.Reset();
		tw.enabled = true;
	
		TweenAlpha  twa = tw.transform.gameObject.GetComponent<TweenAlpha>();//TweenAlpha.Begin(tw.transform.gameObject, 0.2f, 0	);
		twa.duration = 0.2f;
		twa.from = 0.0f;
		twa.to = 1.0f;
		twa.delay = 0.2f;

		twa.Reset();
		twa.enabled = true;

	}

	private void disapperGear(int gearNum){
		if(gearNum == 0) 
		{	
			//initGear(gearNum);
			foreach(TweenPosition tween in Gear){
				tween.transform.gameObject.SetActive(false);
				tween.transform.gameObject.GetComponent<UISprite>().alpha =1.0f;
			}
			b_maxGear = false;
			return;
		}

		int num = gearNum - 1;
		//Utility.Log("number"+num);
		TweenPosition tw = Gear[num];
		tw.from = Vector3.zero;
		tw.to = new Vector3(0,40,0);

		tw.eventReceiver = gameObject;// tw.transform.gameObject;
		tw.callWhenFinished = "selfActiveFalse";
		tw.Reset();
		tw.enabled = true;
		
		
		TweenAlpha  twa = tw.transform.gameObject.GetComponent<TweenAlpha>();//TweenAlpha.Begin(tw.transform.gameObject, 0.2f, 0	);
		twa.duration = 0.2f;
		twa.from = 1.0f;
		twa.to = 0.0f;
		twa.delay = 0.0f;
		//twa.method = UITweener.Method.EaseInOut;
		twa.Reset();
		twa.enabled = true;
	}

	private void StartAction(GameObject gear){

	}

	public void selfActiveFalse(TweenPosition t){
		//this.gameObject.SetActive(false);
		if(t == null){
			Utility.LogError("t == NULL");
		}
		t.transform.gameObject.GetComponent<UISprite>().alpha =1.0f;
		t.transform.gameObject.SetActive(false);


		
	}
	public void disableGUIObject(string str){
		//findobject(str).SetActive(false);
		int curGear = int.Parse(str.Substring(1));
		Utility.Log (curGear);
		
		//Gear.SendMessage("GearChangeAction",curGear,SendMessageOptions.DontRequireReceiver);
		return;
		
		
		
	}

	public void gearDisable(){
		
	}
}
