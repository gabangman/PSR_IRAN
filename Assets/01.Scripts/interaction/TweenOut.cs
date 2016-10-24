using UnityEngine;
using System.Collections;

public class TweenOut : MonoBehaviour {

	public void tweenOut(){
		var tween = transform.GetChild(0).GetComponent<TweenPosition>() as TweenPosition;
		//Vector3 to = tween.from;
		//float delay = tween.delay;
		//tween.from = tween.to;
		//tween.to = to;
		//tween.delay = delay;
		//tween.Reset();
		tween.enabled =true;
	
		 tween = transform.GetChild(1).GetComponent<TweenPosition>() as TweenPosition;
		// to = tween.from;
		//tween.from = tween.to;
		//tween.to = to;
		//delay = tween.delay;
		//tween.delay = delay;
		tween.onFinished = delegate(UITweener tw) {
			tw.transform.parent.gameObject.SetActive(false);
		};
		//tween.Reset();
		tween.enabled =true;
	}


}
