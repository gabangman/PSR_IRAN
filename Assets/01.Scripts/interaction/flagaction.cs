using UnityEngine;
using System.Collections;

public class flagaction : InterAction {

	//private bool isRaceMode;
	void Awake(){
		if(Global.gRaceInfo.sType != SubRaceType.RegularRace){
			//isRaceMode = false;
		}else{
			//isRaceMode = true;
		}
	}


	//common function
	public void selfActiveFalse(){
		Utility.LogWarning("selfActiveFalse");return;
		gameObject.GetComponent<UISprite>().alpha = 1.0f;
		gameObject.SetActive(false);
	}

	public void selfDestroy(){
		gameObject.SetActive(false);
		//Destroy(gameObject);
	}

	
	//flag action (finish action)
	public void ObjectOut(TweenPosition tw){
		tw.transform.gameObject.SetActive(false);
		GameManager.instance.isFinalGUI = false;
	}


	public void RetryRotation(){
		var pos = transform.localPosition;
		Vector3 to;
		Vector3 from;

		if(pos.x < 0) {
			pos.x = pos.x -100;
			//isminus = true;
			pos.x = -pos.x;
			transform.localPosition = pos;
			transform.localRotation = Quaternion.Euler(0,0,180);


			from = new Vector3(0,0,180);
			to =new Vector3(0,0,0);

		}
		else {
			pos.x = pos.x+100;
		//	isminus = false;
			pos.x = -pos.x;
			transform.localPosition = pos;
			transform.localRotation = Quaternion.Euler(0,0,0);
			from = new Vector3(0,0,0);
			to =new Vector3(0,0,180);
		}

		var tween = gameObject.GetComponent<TweenRotation>() as TweenRotation;
		tween.to = to;
		tween.from = from;
		tween.delay = 0.5f;
		tween.eventReceiver = gameObject;
		tween.callWhenFinished = "selfDestroy";
		tween.enabled = true;
		tween.Reset();

	
	}


	public void TweenScaleStartRegular(){
		var scale = transform.localScale;
		gameObject.GetComponent<UISprite>().alpha = 1.0f;
		TweenScale scaletween
			= gameObject.GetComponent<TweenScale>() as TweenScale;
		if(scaletween != null) Destroy(scaletween);
		scaletween = gameObject.AddComponent<TweenScale>();
		//= TweenScale.Begin(gameObject, 0.4f, scale*1.2f);// as TweenScale;
		scaletween.from = scale*1.2f;
		scaletween.to = scale;
		scaletween.duration = 0.4f;
		scaletween.method = UITweener.Method.EaseInOut;
		scaletween.style = UITweener.Style.Once;
		scaletween.eventReceiver = null;
		scaletween.callWhenFinished = null;
		
		scaletween.Reset();
		scaletween.enabled = true;
		TweenAlphaStart();
	}
	// display Transmission statue  in Race and display Text action with bolt judgement in Pitin
	public void TweenScaleStart(){
		var scale = transform.localScale;
		gameObject.GetComponent<UISprite>().alpha = 1.0f;
		TweenScale scaletween
			= gameObject.GetComponent<TweenScale>() as TweenScale;
		if(scaletween != null) Destroy(scaletween);
		scaletween = gameObject.AddComponent<TweenScale>();
		//= TweenScale.Begin(gameObject, 0.4f, scale*1.2f);// as TweenScale;
		scaletween.from = scale*1.2f;
		scaletween.to = scale;
		scaletween.duration = 0.4f;
		scaletween.method = UITweener.Method.EaseInOut;
		scaletween.style = UITweener.Style.Once;
		scaletween.eventReceiver = null;
		scaletween.callWhenFinished = null;
	
		scaletween.Reset();
		scaletween.enabled = true;
		Vector3 pos = transform.localPosition;
		transform.localPosition = new Vector3(0, -130f, 0);
		TweenAlphaStart(pos);
	
	}

	private void TweenAlphaStart(){
		
		TweenAlpha scaletween
			= gameObject.GetComponent<TweenAlpha>() as TweenAlpha;
		if(scaletween != null) Destroy(scaletween);
		scaletween = gameObject.AddComponent<TweenAlpha>();
		scaletween.delay = 0.5f;
		scaletween.duration = 0.4f;
		scaletween.from = 1;
		scaletween.to = 0.2f;
		scaletween.method = UITweener.Method.EaseInOut;
		scaletween.style = UITweener.Style.Once;
		scaletween.onFinished = delegate(UITweener tween) {
			gameObject.GetComponent<UISprite>().alpha = 1.0f;
			gameObject.SetActive(false);
			
		};
		scaletween.Reset();
		scaletween.enabled = true;
		
	}
	private void TweenAlphaStart(Vector3 pos){

		TweenAlpha scaletween
			= gameObject.GetComponent<TweenAlpha>() as TweenAlpha;
		if(scaletween != null) Destroy(scaletween);
		scaletween = gameObject.AddComponent<TweenAlpha>();
		scaletween.delay = 0.5f;
		scaletween.duration = 0.4f;
		scaletween.from = 1;
		scaletween.to = 0.2f;
		scaletween.method = UITweener.Method.EaseInOut;
		scaletween.style = UITweener.Style.Once;
		scaletween.onFinished = delegate(UITweener tween) {
			transform.localPosition  = pos;
			gameObject.GetComponent<UISprite>().alpha = 1.0f;
			gameObject.SetActive(false);
			tween.onFinished = null;

		};
		scaletween.Reset();
		scaletween.enabled = true;
	
	}


}
