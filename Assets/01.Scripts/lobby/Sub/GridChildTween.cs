using UnityEngine;
using System.Collections;

public class GridChildTween : MonoBehaviour {

	public void Init(){
		 //transform.GetChild(0).GetComponent<UIGrid>().Reposition();
	}

	public void RankBoardOut(){
		transform.GetComponent<UIDraggablePanel2>().enabled = false;
		var tr = transform.GetChild(0) as Transform;
		var tween  = tr.GetComponent<TweenPosition>() as TweenPosition;
		Vector3 to = tween.to;
		tween.to = tween.from;
		tween.from = to;
		//tween.delay = delay*0.1f;
		tween.eventReceiver = gameObject;
		tween.callWhenFinished =  "DisableObject";
		tween.Reset();
		tween.enabled =true;
	}


	public void RankBoardIn(){
		var tr = transform.GetChild(0) as Transform;
		var tween  = tr.GetComponent<TweenPosition>() as TweenPosition;
		Vector3 to = tween.to;
		tween.to = tween.from;
		tween.from = to;
		//tween.delay = 0.0f;
		tween.eventReceiver = gameObject;
		tween.callWhenFinished =  "EnableObject";
		tween.Reset();
		tween.enabled =true;
	}
	
	public void RankBoardIn(GameObject obj){
		var tr = transform.GetChild(0) as Transform;
		var tween  = tr.GetComponent<TweenPosition>() as TweenPosition;
		Vector3 to = tween.to;
		tween.to = tween.from;
		tween.from = to;
		//tween.delay = 0.0f;
		tween.eventReceiver = obj;
		tween.callWhenFinished =  "AddWorldItem";
		tween.Reset();
		tween.enabled =true;
	}

	void EnableObject(){
		transform.GetComponent<UIDraggablePanel2>().enabled = true;
	}

	void DisableObject(){
		gameObject.SetActive(false);
	}
}
