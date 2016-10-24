using UnityEngine;
using System.Collections;

public class LevelEffect : MonoBehaviour {

	public void levelEffect(){
		Vector3 scaleOrg = transform.localScale;
		var scale = gameObject.AddComponent<TweenScale>() as TweenScale;
		scale.delay = 0.2f;
		scale.duration = 0.25f;
		scale.from = scaleOrg*0.1f;
		scale.to = scaleOrg*1.5f;
		scale.Reset();
		scale.enabled = true;
		scale.onFinished = delegate(UITweener tween) {
			var s = tween.transform.GetComponent<TweenScale>() as TweenScale;
			s.delay = 0.0f;
			s.from = tween.transform.localScale;
			s.to = scaleOrg;
			s.Reset();
			s.enabled = true;
			s.onFinished = delegate(UITweener tween1) {
				Destroy(tween1);
			};
		};
	}
}
