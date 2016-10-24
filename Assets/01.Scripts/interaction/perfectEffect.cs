using UnityEngine;
using System.Collections;

public class perfectEffect : MonoBehaviour {

	void OnEnable(){
		var tr = transform as Transform;
		tr.GetComponent<TweenScale>().Reset();
		tr.GetComponent<TweenScale>().enabled = true;
		tr.GetComponent<TweenAlpha>().Reset();
		tr.GetComponent<TweenAlpha>().enabled = true;
	}


	void OnTweenAlpha(UITweener tween){
		tween.transform.GetComponent<UISprite>().alpha = 0.0f;
		gameObject.SetActive(false);
	}

	void OnTweenScale(UITweener tween){
		//tween.transform.localScale = new Vector3(25,25,0);//<UISprite>().alpha = 0.0f;
		//gameObject.SetActive(false);
	}
}
