using UnityEngine;
using System.Collections;

public partial class ManagerGUI : MonoBehaviour {

	public void n2oPress(){
		N2OPressCallback();
	}
	
	public void N2OOnPress(){
		if((GameManager.instance.RaceState == GameManager.GAMESTATE.RACE01)||
		   (GameManager.instance.RaceState == GameManager.GAMESTATE.RACE02)){
			if(GameManager.instance.isN2O){
				dash_Speed.color = accelreleasecolor;
				s_car.CarEngineVolumeDelay();
				s_music.CarN2OSound();
				GameManager.instance.N2OStart(false);// = false;
				var n20 = findPanel("DashBoard").transform.FindChild("N2Obtn").gameObject;
				var n21 = n20.transform.FindChild("Btn_N2O_On").gameObject;
				if(n21 == null) Utility.LogError("error");
				StartCoroutine(n2oTimer(n21));
				n20 = n21 = null;
			}
		}
	}
	
	public void HiddenN2OButton(){
		var tw = btnn20.gameObject.AddComponent<TweenPosition>() as TweenPosition;
		tw.to = new Vector3(0,-500,0);
		tw.from = Vector3.zero;
		tw.duration = 0.25f;
		tw.enabled = true;
		tw.onFinished = delegate {
			btnn20.gameObject.SetActive(false);
		};
	}
	
	IEnumerator n2oTimer(GameObject n2o){
		//	WaitForSeconds wait = new WaitForSeconds(0.01f); 
		//int Counting = 1000; 
		float delta =0.0f;
		var n20delta = n2o.GetComponent<UISprite>() as UISprite;
		speedIndicatorTween(dash_Speed,"Color");
		speedIndicatorTween(dash_Rpm,"Color");
		_carspeed.MaxEngineRPM = (float)Base64Manager.instance.GlobalEncoding(Global.gMaxRPM)+(float)Base64Manager.instance.GlobalEncoding(Global.rpmAlpah);
		_carspeed.motorInputTorque =Base64Manager.instance.getFloatEncoding(Global.gBsPower, 0.001f)
			+ Base64Manager.instance.getFloatEncoding(Global.gTorque,0.001f);
		float tRPM = Base64Manager.instance.GlobalEncoding(Global.gCheckRPM);
		_carspeed.CheckEngineRPM = tRPM+(float)Base64Manager.instance.GlobalEncoding(Global.rpmAlpah);
		float bsT = Base64Manager.instance.getFloatEncoding(Global.gBsTime,0.001f);
		float divide = 1.0f/bsT;
		if(_carspeed.EngineRPM < tRPM){
			_carspeed.CheckEngineRPM =tRPM + (float)Base64Manager.instance.GlobalEncoding(Global.rpmAlpah);
		}else{
			_carspeed.CheckEngineRPM =_carspeed.EngineRPM;
		}
		for(;;){
			delta+= Time.deltaTime;	
			n20delta.fillAmount = 1-delta*divide;
			if( delta > bsT){
				n20delta.fillAmount = 0.0f;
				HiddenText();
				if(GameManager.instance.RaceState != GameManager.GAMESTATE.FINISH){
					s_music.StopCarN2OSound();
				}
				GameManager.instance.N2OEnd();
				_carspeed.MaxEngineRPM = Base64Manager.instance.GlobalEncoding(Global.gMaxRPM);
				_carspeed.motorInputTorque = Base64Manager.instance.getFloatEncoding(Global.gTorque,0.001f);
				_carspeed.CheckEngineRPM = tRPM;
				HiddenN2OButton();
				if(isPress) dash_Speed.color = accelpresscolor;
				else dash_Speed.color = accelreleasecolor;
				yield break;//yield return null;	
			}
			yield return null;
		}
	}
}
