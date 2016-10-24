using UnityEngine;
using System.Collections;

public class rewardTextAction : MonoBehaviour {
	private int pGear, gGear;
	void Start(){
		gameObject.SetActive(true);
		if(Global.isTutorial){
			pGear = 10;
			gGear = 5;
		}else{
			pGear = Base64Manager.instance.GlobalEncoding(Global.gScorePerfect);
			gGear = Base64Manager.instance.GlobalEncoding(Global.gScoreGood);
		}
	}


	void RewardAction(string str){
		int dollar = 0;
		int num = 4;
		switch(str){
		case "Late":
			num = 4;
			break;
		case "Good":
			num = 1;
			dollar = gGear;
			break;
		case "Perfect":
			num = 0;
			dollar = pGear;
			break;
		case "Early":
			num = 4;
			break;
		}
		
		if(num == 4) return;
		var tr = transform.GetChild(num) as Transform;
		tr.gameObject.SetActive(true);
		addScale(tr);
		tweenScaleAlpha(tr);
		tr.FindChild("Label").GetComponent<UILabel>().text =
			string.Format("X {0}" ,  dollar);
	
	}
	
	void rewardPitAction(int num){
		string str = null;
		switch(num){
		case 2:
		str = "Perfect";
		break;
		case 1:
		str = "Good";
		break;
		case 3:
		break;
		case 4:
		break;
		}
		RewardAction(str);
		
	}
	void addScale(Transform tr){
		var twscale = tr.gameObject.AddComponent<TweenScale>() as TweenScale;
		Vector3 orgSize = tr.localScale;
		twscale.from = new Vector3(0.1f,0.1f,0.1f);
		twscale.to = orgSize * 1.8f;
		twscale.duration = 0.2f;
		twscale.method = UITweener.Method.EaseInOut;
		twscale.Reset();
		twscale.enabled = true;
		twscale.onFinished = delegate(UITweener tween) {
			var twscale1 = tween.transform.GetComponent<TweenScale>() as TweenScale;
			twscale1.from = tween.transform.localScale;
			twscale1.to = tween.transform.localScale/1.8f;
			twscale1.onFinished = null;
			twscale1.duration = 0.2f;
			twscale1.Reset();
			twscale1.enabled = true;
		};
	}
	
	void tweenScaleAlpha(Transform tr){
		TweenAlpha scaletween
			= tr.gameObject.GetComponent<TweenAlpha>() as TweenAlpha;
		if(scaletween != null) Destroy(scaletween);
		scaletween = tr.gameObject.AddComponent<TweenAlpha>();
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
		
}
