using UnityEngine;
using System.Collections;

public class TweenAction : MonoBehaviour {

	public void ReverseTween(GameObject target){
		if(!target.activeSelf) return;
		var tween = target.GetComponent<TweenPosition>() as TweenPosition;
			Vector3 to = tween.from;
			tween.from = tween.to;
			tween.to = to;
			tween.onFinished= delegate(UITweener tween1) {
				tween1.transform.gameObject.SetActive(false);
			};
		//Utility.Log("reverse " + tween.from + " : " + tween.to);
			tween.Reset();
			tween.enabled=true;
	}

	public void ForwardPlayTween(GameObject target){
		var tween = target.GetComponent<TweenPosition>() as TweenPosition;
		Vector3 to = tween.from;
		tween.from = tween.to;
		tween.to = to;
		tween.onFinished= null;
		tween.Reset();
		tween.enabled=true;
		//Utility.Log("forward " + tween.from + " : " + tween.to);
		//Destroy(this);
	}
	public void ForwardTween(GameObject target){
		target.SetActive(true);
		var tween = target.GetComponent<TweenPosition>() as TweenPosition;
		tween.Reset();
		tween.enabled=true;
	}
	public void ReversePlayTween(GameObject target){
		var tween = target.GetComponent<TweenPosition>() as TweenPosition;
		Vector3 to = tween.from;
		tween.from = tween.to;
		tween.to = to;
		tween.onFinished= delegate(UITweener tween1) {
			tween1.transform.gameObject.SetActive(false);
		};
		tween.Reset();
		tween.enabled=true;
	}

	public void ReversScaleTween(){
		var scale = GetComponent<TweenScale>() as TweenScale;
		scale.Reset();
		scale.enabled = true;
	}

	public void doubleTweenScale(GameObject obj){
		var scale = obj.GetComponents<TweenScale>() as TweenScale[];
		scale[0].Reset();
		scale[0].enabled = true;
		scale[1].Reset();
		scale[1].enabled = true;
	}


	public void DestroyWindowTween(GameObject target){
		
		if(target == null ) return;
		var tween = target.GetComponent<TweenPosition>() as TweenPosition;
		if(tween == null) DestroyImmediate(target);
		else {
			Vector3 to = tween.from;
			tween.from = tween.to;
			tween.to = to;
			tween.eventReceiver = gameObject;
			tween.callWhenFinished = "DestroyWindow";
			tween.Reset();
			tween.enabled=true;
		}
		
	}


	public void tempHidden(){
			var tween = gameObject.GetComponent<TweenPosition>() as TweenPosition;
			Vector3 to = tween.from;
			tween.from = tween.to;
			tween.to = to;
			tween.Reset();
			tween.enabled=true;
	}

	public void tempHidden(GameObject obj){
		var tween = obj.GetComponent<TweenPosition>() as TweenPosition;
		Vector3 to = tween.from;
		tween.from = tween.to;
		tween.to = to;
		tween.Reset();
		tween.enabled=true;
	}

	
	void DestroyWindow(TweenPosition tw){
		Destroy(tw.transform.gameObject);

		//if(StatusObj != null) DestroyImmediate(StatusObj);
		//if(InfoObj != null) DestroyImmediate(InfoObj);
	}

	public void AddWindowTween(GameObject target, Vector3 to, Vector3 from, float _duration){
		var tween = target.GetComponent<TweenPosition>() as TweenPosition;
		if(tween != null){
			DestroyImmediate(tween);
		}
		tween = target.AddComponent<TweenPosition>() as TweenPosition;
		tween.to = to;
		tween.from =from;
		tween.duration = _duration;
		tween.style = UITweener.Style.Once;
		tween.method = UITweener.Method.EaseInOut;
		tween.Reset();
		tween.enabled=true;
		Destroy(this);
	}
	
	public void RankTween(float _delay, float _duration, Vector3 _from, Vector3 _to){

		TweenPosition tw = gameObject.GetComponent<TweenPosition>();
		if(tw == null){
			Utility.LogError("tween Null");
			return;
		}
		tw.delay = _delay;
		tw.duration = _duration;
		tw.from = _from;
		tw.to =_to;
		tw.Reset();
		tw.enabled = true;
	}
}
