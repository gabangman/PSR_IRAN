using UnityEngine;
using System.Collections;

public class pitcrewQAction : MonoBehaviour {


	void pitCrewIn(){

		var temp = gameObject.GetComponent<TweenPosition>() as TweenPosition;
		if(temp == null) temp = gameObject.AddComponent<TweenPosition>();
		
		temp.to = Vector3.zero;
		temp.from = new Vector3(-500,0,0);
		temp.method  = UITweener.Method.EaseInOut;
		temp.duration = 0.25f;
		temp.Reset();
		temp.enabled = true;
		temp.onFinished = null;
	}


	void pitCrewOut(){
		var temp = gameObject.GetComponent<TweenPosition>() as TweenPosition;
		if(temp == null) temp = gameObject.AddComponent<TweenPosition>();
		temp.from = Vector3.zero;
		temp.to = new Vector3(-500,0,0);
		temp.method  = UITweener.Method.EaseInOut;
		temp.duration = 0.25f;
		temp.Reset();
		temp.enabled = true;
		temp.onFinished = delegate(UITweener tween) {
			tween.transform.gameObject.SetActive(false);
		};
	}


}
