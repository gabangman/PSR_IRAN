using UnityEngine;
using System.Collections;

public class btnaction : InterAction {

	// Use this for initialization
	private Vector3 orginalpos;
	public GameObject btntext;
	float startScale = 1.5f;
	float endScale = 2.0f;
	void Start () {
		orginalpos = transform.localScale;
	}

	//common Function
	void ShowFullText(){
		var twee = btntext.GetComponent<TweenAlpha>() as TweenAlpha;
		twee.enabled = true;
		//var tween = btntext.GetComponent<TweenPosition>() as TweenPosition;
		//tween.enabled  = true;
		//var tweenScale = btntext.GetComponent<TweenScale>() as TweenScale;
		//tweenScale.enabled = true;
		var tweenScale = btntext.GetComponentInChildren<TweenScale>() as TweenScale;
		tweenScale.enabled = true;
	}

	private void finish(){
		gameObject.transform.localScale = orginalpos;
	}

	private void clutchPress(){
		Vector3 temp = gameObject.transform.localScale;
		TweenScale ts = gameObject.GetComponent<TweenScale>() as TweenScale;
		if(ts != null) { 
			ts.enabled = true;
			ts.Reset();
			return;
		}
		ts = gameObject.AddComponent<TweenScale>();
		ts.duration = 0.2f;
		ts.from = temp * startScale;
		ts.to = temp*endScale;
		ts.method = UITweener.Method.EaseInOut;
		ts.style = UITweener.Style.Once;
		ts.eventReceiver = gameObject;
		ts.callWhenFinished = "finish";
	}
	private void gearClutchPress(){
		Vector3 temp = gameObject.transform.localScale;
		TweenScale ts = gameObject.GetComponent<TweenScale>() as TweenScale;
		if(ts != null) { 
			ts.enabled = true;
			ts.Reset();
			return;
		}
		ts = gameObject.AddComponent<TweenScale>();
		ts.duration = 0.2f;
		ts.from = temp * startScale;
		ts.to = temp*endScale;
		ts.method = UITweener.Method.EaseInOut;
		ts.style = UITweener.Style.Once;
		ts.eventReceiver = gameObject;
		ts.callWhenFinished = "finish";
	}
	//when Aceel btn press in race ,  this function  action 
	private void btnPress(){
		Vector3 temp = gameObject.transform.localScale;
		TweenScale ts = gameObject.GetComponent<TweenScale>() as TweenScale;
		if(ts != null) { 
			ts.enabled = true;
			ts.Reset();
			return;
		}
		 ts = gameObject.AddComponent<TweenScale>();
		ts.duration = 0.2f;
		ts.from = temp*startScale;
		ts.to = temp*endScale;
		ts.method = UITweener.Method.EaseInOut;
		ts.style = UITweener.Style.Once;
		ts.eventReceiver = gameObject;
		ts.callWhenFinished = "restart";
	}
	private void restart(){
		if(GameManager.instance._isPress){
			gameObject.GetComponent<TweenScale>().Reset();
		}else{
			gameObject.transform.localScale = orginalpos;
		}

	}


	// when user press drillbutton in pitin
	private void btnDrillPress(){
		Vector3 temp = gameObject.transform.localScale;
		TweenScale ts = gameObject.GetComponent<TweenScale>() as TweenScale;
		if(ts != null) { 
			ts.enabled = true;
			ts.Reset();
			return;
		}
		
		ts = gameObject.AddComponent<TweenScale>();
		ts.duration = 0.2f;
		ts.from = temp*startScale;
		//temp = temp*1.5;
		ts.to = temp*endScale;
		ts.method = UITweener.Method.EaseInOut;
		ts.style = UITweener.Style.Once;
		ts.eventReceiver = gameObject;
		ts.callWhenFinished = "finish";
	}

	//N2O button Action 
	private void btnN2OPress(){
	
		Vector3 temp = gameObject.transform.localScale;
		TweenScale ts = gameObject.GetComponent<TweenScale>() as TweenScale;
		if(ts != null) { 
			ts.enabled = true;
			ts.Reset();
			return;
		}
		
		ts = gameObject.AddComponent<TweenScale>();
		ts.duration = 0.2f;
		ts.from = temp*startScale;
		//temp = temp*1.5;
		ts.to = temp*endScale;
		ts.method = UITweener.Method.EaseInOut;
		ts.style = UITweener.Style.Once;
		ts.eventReceiver = gameObject;
		ts.callWhenFinished = "finish";
	
	}

	private void HiddenText(){
		//var temp =	gameObject.transform.parent.gameObject;
		var temp1 =	transform.GetChild(0).gameObject;
		//Utility.Log (temp1.name);
		var alpha = temp1.GetComponent<TweenAlpha>() as TweenAlpha;
		alpha.to = 0;
		alpha.from = 1;
		alpha.delay = 0.0f;
		alpha.duration = 0.3f;
		alpha.eventReceiver = gameObject;
		alpha.callWhenFinished = "n2ofinish";
		alpha.Reset();
		alpha.enabled  = true;

		var pos =  temp1.GetComponent<TweenPosition>() as TweenPosition;
		pos.from = Vector3.zero;
		pos.to = new Vector3(0, 25,0);
		pos.delay = 0.0f;
		pos.duration = 0.3f;
		pos.Reset();
		pos.enabled = true;
	
	}

	private void n2ofinish(){
		var temp1 =	transform.GetChild(0).gameObject;
		temp1.SetActive(false);
	}

	private void showbtnText(){
		//transform.GetComponent<UISprite>().alpha = 0.0f;
		btntext.SetActive(true);
		var alpha = btntext.GetComponent<TweenAlpha>() as TweenAlpha;
		alpha.to = 1;
		alpha.from = 0;
		alpha.delay = 0.0f;
		alpha.duration = 0.3f;
		alpha.eventReceiver = null;
		alpha.callWhenFinished = null;
		alpha.Reset();
		alpha.enabled  = true;

		var pos =  btntext.GetComponent<TweenPosition>() as TweenPosition;
		pos.from = new Vector3(0, -25,0);
		pos.to = Vector3.zero;
		pos.delay = 0.0f;
		pos.duration = 0.3f;
		pos.Reset();
		pos.enabled = true;
	}
	
	private void hiddenbtnText(){
		//transform.GetComponent<UISprite>().alpha = 1.0f;
		var alpha = btntext.GetComponent<TweenAlpha>() as TweenAlpha;
		alpha.to = 0;
		alpha.from = 1;
		alpha.delay = 0.0f;
		alpha.duration = 0.3f;
		alpha.eventReceiver = gameObject;
		alpha.callWhenFinished = "btnfinish";
		alpha.Reset();
		alpha.enabled  = true;
		var pos =  btntext.GetComponent<TweenPosition>() as TweenPosition;
		pos.from = Vector3.zero;
		pos.to = new Vector3(0, -25,0);
		pos.delay = 0.0f;
		pos.duration = 0.3f;
		pos.Reset();
		pos.enabled = true;
	}

	private void blinkbtnText(){
		//Utility.Log("blinkbtnText+"+btntext.name);
		btntext.SetActive(true);
		var alpha = btntext.GetComponent<TweenAlpha>() as TweenAlpha;
		alpha.to = 1;
		alpha.from = 0;
		alpha.delay = 0.0f;
		alpha.duration = 0.3f;
		alpha.style = UITweener.Style.PingPong;
		alpha.eventReceiver = null;
		alpha.callWhenFinished = null;
		alpha.Reset();
		alpha.enabled  = true;
		
		var pos =  btntext.GetComponent<TweenPosition>() as TweenPosition;
		pos.from = new Vector3(0, -25,0);
		pos.to = Vector3.zero;
		pos.delay = 0.0f;
		pos.duration = 0.3f;
		pos.Reset();
		pos.enabled = true;
	}

	private void blinkbtnStopText(){
		//Utility.Log("blinkbtnStopText+"+btntext.name);
		var alpha = btntext.GetComponent<TweenAlpha>() as TweenAlpha;
		alpha.to = 0;
		alpha.from = 1;
		alpha.delay = 0.0f;
		alpha.duration = 0.3f;
		alpha.eventReceiver = gameObject;
		alpha.callWhenFinished = "btnfinish";
		alpha.style = UITweener.Style.Once;
		alpha.Reset();
		alpha.enabled  = true;
		var pos =  btntext.GetComponent<TweenPosition>() as TweenPosition;
		pos.from = Vector3.zero;
		pos.to = new Vector3(0, -25,0);
		pos.delay = 0.0f;
		pos.duration = 0.3f;
		pos.Reset();
		pos.enabled = true;
	}
    
    void btnfinish(){
    	btntext.SetActive(false);

	}

	void BlinkBG(Transform tr){
		string name = string.Empty;
		if(tr.name == "Btn_Accel_Ring"){
			name = "Btn_Accel";
		//	Utility.Log("blinkBG  - Accel");
		}else{
			name ="Btn_Gear";
		//	Utility.Log("blinkBG  - Btn_Gear");
		}
		var child = transform.parent.FindChild(name) as Transform;
		var tween = child.gameObject.AddComponent<TweenColor>() as TweenColor;
		tween.to = Color.red;
		tween.from = Color.white;
		tween.duration= 0.2f;
		tween.style = UITweener.Style.PingPong;
		tween.enabled = true;
	}

	void BlinkBGStop(Transform tr){
		string name = string.Empty;
		if(tr.name == "Btn_Accel_Ring"){
			name = "Btn_Accel";
		//	Utility.Log("BlinkBGStop  - Accel");
		}else{
			name ="Btn_Gear";
		//	Utility.Log("BlinkBGStop  - Btn_Gear");
		}
		var child = transform.parent.FindChild(name) as Transform;
		child.GetComponent<TweenColor>().style = UITweener.Style.Once;
		child.GetComponent<TweenColor>().enabled = false;

		//child.GetComponent<UISprite>().color = Color.white;
	}

}
