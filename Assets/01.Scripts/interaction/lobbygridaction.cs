using UnityEngine;
using System.Collections;

public class lobbygridaction : MonoBehaviour {



	bool isActived = false;
	void Start(){

	
	}


	void StartTweenPosition(){
		if(isActived) return;
		gameObject.AddComponent<TweenAction>().AddWindowTween(gameObject,
		                                                      new Vector3(0, -390,0), Vector3.zero, 0.8f);
		isActived = true;
		return; 
		/*TweenPosition _temp = gameObject.GetComponent<TweenPosition>() as TweenPosition;
		if(_temp != null) {Destroy (_temp);}

		_temp = gameObject.AddComponent<TweenPosition>();
		_temp.from =  Vector3.zero;
		_temp.to = new Vector3(0,-390,0);
		_temp.duration = 0.8f;
		_temp.style = UITweener.Style.Once;
		_temp.method = UITweener.Method.EaseInOut;
			isActived = true;
			*/
	}



	void EndTweenPosition(){
		if(!isActived) return;
		gameObject.AddComponent<TweenAction>().AddWindowTween(gameObject,
		                                                      Vector3.zero,new Vector3(0, -390,0),  0.8f);
		isActived = false;
		return;
		/*
		TweenPosition _temp = gameObject.GetComponent<TweenPosition>() as TweenPosition;
		if(_temp != null) {Destroy (_temp);}
		_temp = gameObject.AddComponent<TweenPosition>();
		
		_temp.to =  Vector3.zero;
		_temp.from = new Vector3(0,-390,0);
		_temp.duration = 0.8f;
		_temp.style = UITweener.Style.Once;
		_temp.method = UITweener.Method.EaseInOut;
		 isActived = false;
		*/
	}


	void EndTweenPositionSetActive(){
		//if(!isActive) return;
		TweenPosition _temp = gameObject.GetComponent<TweenPosition>() as TweenPosition;
		if(_temp != null) {Destroy (_temp);}
		_temp = gameObject.AddComponent<TweenPosition>();
		
		_temp.to =  Vector3.zero;
		_temp.from = new Vector3(0,-390,0);
		_temp.duration = 0.001f;
		_temp.style = UITweener.Style.Once;
		_temp.method = UITweener.Method.EaseInOut;
		_temp.eventReceiver = gameObject;
		_temp.callWhenFinished  = "selfDisable";
		 isActived = false;
	}

	void selfDisable(){
		gameObject.SetActive(false);
	}

	Vector3 viewpos;
	Vector4 clipingView;

	void OnEnable(){
		if(gameObject.name == "rank"){
			
			Invoke("tweenStart",0.5f);
			//StartTweenPosition();
		}
		var view = transform.FindChild("view") as Transform;
		viewpos = view.localPosition;
		clipingView = view.GetComponent<UIPanel>().clipRange;
	}

	void tweenStart(){
		StartTweenPosition();
		CancelInvoke("tweenStart");
		//Utility.Log ("tweenStart");
	}

	void OnDisable(){
		isActived = false;
		var view = transform.FindChild("view") as Transform;
		view.localPosition = viewpos;
		view.GetComponent<UIPanel>().clipRange = clipingView;
	}


	void OnButtonClick(){
		if(isActived){
			EndTweenPosition();
	}else{
			StartTweenPosition();
	}
	}

	public void checkRankStatus(){
		if(isActived){
			EndTweenPosition();

		}
	
	}
}
