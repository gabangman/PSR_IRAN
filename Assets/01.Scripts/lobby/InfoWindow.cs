using UnityEngine;
using System.Collections;

public class InfoWindow : MonoBehaviour {
	public void showSub(GameObject obj){
		obj.SetActive(true);
		var tw = obj.GetComponent<TweenPosition>() as TweenPosition;
		tw.Reset();
		tw.enabled = true;
	}
	
	public void hiddenSub(GameObject obj){
		var tw = obj.GetComponent<TweenPosition>() as TweenPosition;
		Vector3 to = tw.from;
		tw.from = tw.to;
		tw.to = to;
		tw.onFinished = delegate(UITweener tween) {
			var tpos = tween.transform.GetComponent<TweenPosition>() as TweenPosition;
			Vector3 te = tpos.from;
			tpos.from = tpos.to;
			tpos.to = te;
			tpos.onFinished = null;
			tween.transform.gameObject.SetActive(false);
		};
		tw.Reset();
		tw.enabled = true;
	}
	public virtual void ShowWindow(){}
	public virtual void HiddenWindow(){}

}
