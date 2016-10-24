using UnityEngine;
using System.Collections;

public class TextLEDAction : MonoBehaviour {

	void OnEnable(){
		
	}
	void Awake(){
	}
	
	void OnDisable(){
		var sig_on = transform.FindChild("Sig_On") as Transform;
		sig_on.gameObject.SetActive(false);
	}

	void ShowRaceLEDRegular(string str){
		//Late, Good, Perfect, Early
		int num = 0;
		
		switch(str){
		case "Late":
			num = 3;
			break;
		case "Good":
			num = 1;
			break;
		case "Perfect":
			num = 2;
			break;
		case "Early":
			num = 0;
			break;
		}
		var target = transform.GetChild(num) as Transform;
		var sig_on = transform.FindChild("Sig_On") as Transform;
		sig_on.localPosition = target.localPosition;
		sig_on.gameObject.SetActive(true);
		SigOnScale(sig_on);
		tweenScaleAlpha();
	}

	void ShowRaceLED(string str){
		//Late, Good, Perfect, Early
		int num = 0;

		switch(str){
			case "Late":
			num = 3;
			break;
			case "Good":
			num = 1;
			break;
			case "Perfect":
			num = 2;
			break;
			case "Early":
			num = 0;
			break;
		}
			var target1 = transform.GetChild(num) as Transform;
			var sig_on1 = transform.FindChild("Sig_On") as Transform;
			sig_on1.localPosition = target1.localPosition;
			sig_on1.gameObject.SetActive(true);
			var twscale =sig_on1.GetComponent<TweenScale>() as TweenScale;
			Vector3 sigParent = transform.localPosition;// as Vector3;
			transform.localPosition = new Vector3(0,-90,-1.35f);
			if(twscale == null) twscale = sig_on1.gameObject.AddComponent<TweenScale>();
			Vector3 orgSize = sig_on1.localScale;
			twscale.from = new Vector3(0.1f,0.1f,0.1f);
			twscale.to = orgSize * 2f;
			twscale.duration = 0.2f;
			twscale.method = UITweener.Method.EaseInOut;
			twscale.Reset();
			twscale.enabled = true;
			twscale.onFinished = delegate(UITweener tween) {
				var twscale1 = tween.transform.GetComponent<TweenScale>() as TweenScale;
				twscale1.from = tween.transform.localScale;
				twscale1.to = tween.transform.localScale/2f;
				twscale1.onFinished = null;
				twscale1.duration = 0.2f;
				twscale1.Reset();
				twscale1.enabled = true;
				//tween.transform.parent.localPosition = sigParent;
			};
			tweenScaleAlpha(sigParent);
		
	}



	
	void SigOnScale(Transform tr){
		var twscale = tr.GetComponent<TweenScale>() as TweenScale;
		if(twscale == null) twscale = tr.gameObject.AddComponent<TweenScale>();
		Vector3 orgSize = tr.localScale;
		twscale.from = new Vector3(0.1f,0.1f,0.1f);
		twscale.to = orgSize * 2f;
		twscale.duration = 0.2f;
		twscale.method = UITweener.Method.EaseInOut;
		twscale.Reset();
		twscale.enabled = true;
		twscale.onFinished = delegate(UITweener tween) {
			var twscale1 = tween.transform.GetComponent<TweenScale>() as TweenScale;
			twscale1.from = tween.transform.localScale;
			twscale1.to = tween.transform.localScale/2f;
			twscale1.onFinished = null;
			twscale1.duration = 0.2f;
			twscale1.Reset();
			twscale1.enabled = true;
		};
	
	}
	
	
	void ShowPitLED(int num){
		//early good perfect late
		var target = transform.GetChild(num) as Transform;
		var sig_on = transform.FindChild("Sig_On") as Transform;
		sig_on.localPosition = target.localPosition;
		sig_on.gameObject.SetActive(true);
		SigOnScale(sig_on);
		tweenScaleAlpha();
	}
	
	
	void tweenScaleAlpha(){
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
			tween.transform.gameObject.SetActive(false);
		};
		scaletween.Reset();
		scaletween.enabled = true;
	
	}
	void tweenScaleAlpha(Vector3 pos){
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
			tween.transform.gameObject.SetActive(false);
			tween.transform.localPosition = pos;
		};
		scaletween.Reset();
		scaletween.enabled = true;
		
	}
}
