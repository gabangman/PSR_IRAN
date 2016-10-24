using UnityEngine;
using System.Collections;

public class signalaction : MonoBehaviour {
	public GameObject[] Signal;
	public GameObject ReadyObj;
	private bool isClick;
	private bool isWhite;
	// Use this for initialization

	public GameObject[] TourSignal;

	void Awake(){
	}


	void Start () {
		isClick = false;
		isWhite = false;
		switch(Global.gRaceInfo.sType){
		case SubRaceType.DragRace:
			ReadyObj.SetActive(false);
			break;
		case SubRaceType.RegularRace:
			ReadyObj.SetActive(true);
			break;
		case SubRaceType.CityRace:
			ReadyObj.SetActive(false);
			break;
		}
	}

	void ShowSIG(){
		TourSignal[0].SetActive(true);

	}

	void ShowSIGStart(){
		StartCoroutine("SignalCount");
	}

	public AudioClip Audio1st, Audio2nd;
	IEnumerator SignalCount(){
		TourSignal[1].SetActive(true);
		NGUITools.PlaySound(Audio1st);
		yield return new WaitForSeconds(1.0f);
		TourSignal[2].SetActive(true);
		NGUITools.PlaySound(Audio1st);
		GameManager.instance.mgrgui.TouringCheckTime(1.0f);
		yield return new WaitForSeconds(0.5f);
		GameManager.instance.mgrgui.TouringStart1();
		yield return new WaitForSeconds(0.5f);
		TourSignal[3].SetActive(true);
		NGUITools.PlaySound(Audio2nd);
		GameManager.instance.mgrgui.TouringStart2();
	}



	void HiddenSIG(){
		TourSignal[0].SetActive(false);
	
	}

	void ShowFlagSIG(){
		Signal[0].SetActive(true);
		Signal[1].SetActive(true);
	}

	public void HiddenFlagSIG(){
		OnClick();
	}


	// signal show
	public void showSignal(){
		//Signal[3].SetActive(true);
		Signal[0].SetActive(true);
		Signal[1].SetActive(true);
		TweenPosition _tween=
			Signal[0].GetComponent<TweenPosition>() as TweenPosition;
		if(_tween != null) {
			_tween.Reset();
			_tween.enabled = true;
		}
		_tween =Signal[1].GetComponent<TweenPosition>() as TweenPosition;
		if(_tween != null){
			_tween.Reset();
			_tween.enabled = true;
		}
	}
	
	public void  showSignal1(){
		Signal[2].SetActive(true);
		Signal[3].SetActive(true);
		TweenPosition _tween=
			Signal[2].GetComponent<TweenPosition>() as TweenPosition;
		if(_tween != null) {
			_tween.Reset();
			_tween.enabled = true;
		}
		_tween =Signal[3].GetComponent<TweenPosition>() as TweenPosition;
		if(_tween != null){
			_tween.Reset();
			_tween.enabled = true;
			
		}
		
		
	}

	void OnAutoHidden(){
		isClick = true;
		isFlag = true;
		FlagOut(0);
		FlagOut(1);
	}

	public void hiddenSignal(){
		//fadeOutFlag();
		if(isFlag) return;
		FlagOut(0);
		FlagOut(1);
		
	}
	//redlight turn on in sequence
	
	// press the other botton(accel or clutch) 
	bool isFlag = false;
	public void OnClick(){
	
		if(isClick) return;
		isClick = true;
		isFlag = true;
		if(!isWhite) fadeOutFlag();
		else fadeOutFlag2();
	}




	private void fadeOutFlag(){
		if(!GameManager.instance.isEnablePress){
			isClick = false;
			return;
		}
		FlagOut(0);
		FlagOut(1);
	}
	void FlagOut(int _idx){
		Spin _tween=
			Signal[_idx].GetComponent<Spin>() as Spin;
		if(_tween != null){
			//Signal[0].transform.localRotation = Quaternion.identity;
			_tween.enabled = true;
		}
		TweenAlpha _tween1=
			Signal[_idx].GetComponent<TweenAlpha>() as TweenAlpha;
		if(_tween1 != null) {
			_tween1.Reset();
			_tween1.enabled = true;
		}
		
	}
	
	private void fadeOutFlag2(){
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.PITINEND){
			isClick = false;
			return;
		}
		FlagOut(2);
		FlagOut(3);
	}
	
	private void disableSignal(){
		//Signal[0].transform.gameObject.
		Spin _tween=
			Signal[0].GetComponent<Spin>() as Spin;
		if(_tween != null){
			Signal[0].transform.localRotation = Quaternion.identity;
			_tween.enabled = false;
		}
		_tween =Signal[1].GetComponent<Spin>() as Spin;
		if(_tween != null){
			Signal[1].transform.localRotation = Quaternion.identity;
			_tween.enabled = false;
		}
		Signal[0].transform.FindChild("Signal").gameObject.GetComponent<UISprite>().alpha =1.0f;
		Signal[1].transform.FindChild("Signal").gameObject.GetComponent<UISprite>().alpha =1.0f;
		Signal[0].SetActive(false);
		Signal[1].SetActive(false);
		isClick = false;
		isWhite =true;
		gameObject.SetActive(false);

	}
	
	private void disablewhiteSignal(){
		//Signal[0].transform.gameObject.
		Spin _tween=
			Signal[2].GetComponent<Spin>() as Spin;
		if(_tween != null){
			Signal[2].transform.localRotation = Quaternion.identity;
			_tween.enabled = false;
		}
		_tween =Signal[3].GetComponent<Spin>() as Spin;
		if(_tween != null){
			Signal[3].transform.localRotation = Quaternion.identity;
			_tween.enabled = false;
		}
		Signal[2].transform.FindChild("Signal").gameObject.GetComponent<UISprite>().alpha =1.0f;
		Signal[3].transform.FindChild("Signal").gameObject.GetComponent<UISprite>().alpha =1.0f;
		
		isClick = false;
		gameObject.SetActive(false);
		
		
	}
	
	private void selfHidden(){
		gameObject.transform.FindChild("Ready").gameObject.SetActive(false);
	}
	/*
	void FixedUpdate(){
		if(GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01FINISH)
			OnClick();
		
	}*/
	
}
