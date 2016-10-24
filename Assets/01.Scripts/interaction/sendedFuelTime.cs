using UnityEngine;
using System.Collections;

public class sendedFuelTime : MonoBehaviour {

	void OnEnable(){
		
	}
	private const float secondsToDegrees = 360f / 60f;
	public Transform tr;
	private System.DateTime _cuTime;
	public void CheckTime(string strtime){
		if(strtime == "null") {
			isTimecheck = false;
			return;
		}
		System.DateTime cT = NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
		_cuTime = System.Convert.ToDateTime(strtime);
		System.TimeSpan sp = cT - _cuTime;
		if(sp.TotalMinutes >= 61){
			isTimecheck = false;
		}else{
			isTimecheck = true;
			Ttime = ((float)Random.Range(0,100) * 0.1f);
		}
		//Utility.Log(sp.TotalMinutes);
	}
	private bool isTimecheck = false;
	float Ttime = 0.0f;

	void ShowTimeArrow(){
		if(!isTimecheck) return;
	//	System.DateTime time = System.DateTime.Now;
	//	System.TimeSpan sp = time - _cuTime;
		Ttime += Time.deltaTime*10;
		tr.localRotation = Quaternion.Euler(0f, 0f, Ttime * -secondsToDegrees);
	}

	void Update () {
		//ShowTimeArrow();
	}
	
	void LateUpdate(){
		ShowTimeArrow();
	}
	
	
}
