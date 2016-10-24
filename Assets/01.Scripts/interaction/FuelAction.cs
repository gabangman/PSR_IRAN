using UnityEngine;
using System.Collections;

public class FuelAction : MonoBehaviour {

	public GameObject maxFuel, minFuel;
	public GameObject[] mFuels;

	public void ChangeFuelCount(){
		int fuelcount = GV.mUser.FuelCount;
		if(fuelcount >= GV.mUser.FuelMax){
			FullFuel();
		}else{
			EmptyFuel(fuelcount);
		}
	
	}

	public void FuelCount(){
		var child = myFuel.FindChild("FuelCount") as Transform;
		int cnt = 0;
		if(GV.mUser.FuelCount > GV.mUser.FuelMax) cnt = GV.mUser.FuelMax;
		else cnt = GV.mUser.FuelCount;

		for(int i = 0; i < cnt; i++){
			child.GetChild(i).gameObject.SetActive(true);
		}
	}
	Transform myFuel;
	public void FuelCountCheck(){
		for(int i = 0; i < mFuels.Length; i++){
			mFuels[i].SetActive(false);
		}
		switch(GV.mUser.FuelMax){
		case 5:{mFuels[0].SetActive(true); myFuel = mFuels[0].transform;}break;
		case 6:{mFuels[1].SetActive(true);myFuel = mFuels[1].transform;}break;
		case 7:{mFuels[2].SetActive(true);myFuel = mFuels[2].transform;}break;
		case 8:{mFuels[3].SetActive(true);myFuel = mFuels[3].transform;}break;
		case 9:{mFuels[4].SetActive(true);myFuel = mFuels[4].transform;}break;
		case 10:{mFuels[5].SetActive(true);myFuel = mFuels[5].transform;}break;
		default:break;
		}

		/*
		if(GV.maxFuel == 10){
			maxFuel.SetActive(true);
			minFuel.SetActive(false);
			myFuel = maxFuel.transform;
		}else{
			maxFuel.SetActive(false);
			minFuel.SetActive(true);
			myFuel = minFuel.transform;
		}*/
	}

	void FullFuel(){
		var child = myFuel.FindChild("FuelCount") as Transform;
		int cnt = child.childCount;
		for(int i = 0; i < cnt-1; i++){
			child.GetChild(i).gameObject.SetActive(true);
		}
	}

	void EmptyFuel(int count){
		var child = myFuel.FindChild("FuelCount") as Transform;
		int remaincount = child.childCount-1;
		var effect = child.GetChild(remaincount).gameObject as GameObject;
		if(count >=  GV.mUser.FuelMax) count = GV.mUser.FuelMax;
		for(int i = 0; i < count; i++){
			child.GetChild(i).gameObject.SetActive(true);
		}
	//	var temp = child.GetChild(count).gameObject as GameObject;
	//	effectFueldisappear(temp.transform, effect);
	//	return;
	//	temp.SetActive(false);
	//	effect.transform.localPosition = temp.transform.localPosition;
	//	effect.SetActive(true);
		
		
		for(int i = count ; i < remaincount; i++){
			var temp1 = child.GetChild(i).gameObject as GameObject;
			if(temp1.activeSelf){
				effectFueldisappear(temp1.transform, effect);
				//temp1.SetActive(false);
				//effect.transform.localPosition = temp.transform.localPosition;
				//effect.SetActive(true);
			}
		}
	}
	
	void effectFueldisappear(Transform tr, GameObject obj){
		var twscale = tr.gameObject.GetComponent<TweenScale>() as TweenScale;
		if(twscale == null) twscale = tr.gameObject.AddComponent<TweenScale>() as TweenScale;
		twscale.from = tr.localScale;//new Vector3(0.1f,0.1f,0.1f);
		twscale.to = tr.localScale*1.5f;
		twscale.method = UITweener.Method.EaseInOut;
		twscale.duration = 0.25f;
		twscale.Reset();
		twscale.enabled = true;
		twscale.onFinished = delegate(UITweener tween) {
			tr.gameObject.SetActive(false);
			tr.localScale = tr.localScale / 1.5f;
			obj.transform.localPosition = tr.localPosition;
			obj.SetActive(true);
		};
	}

	void effectFuelAppear(Transform tr, GameObject obj){
		tr.gameObject.SetActive(true);
		var twscale = tr.GetComponent<TweenScale>() as TweenScale;
		if(twscale == null) twscale = tr.gameObject.AddComponent<TweenScale>() as TweenScale;
		twscale.from = tr.localScale;//new Vector3(0.1f,0.1f,0.1f);
		twscale.to = tr.localScale*1.5f;
		twscale.method = UITweener.Method.EaseInOut;
		twscale.duration = 0.25f;
		twscale.Reset();
		twscale.enabled = true;
		twscale.onFinished = delegate(UITweener tween) {
			tr.localScale = tr.localScale / 1.5f;
			obj.transform.localPosition = tr.localPosition;
			obj.SetActive(true);
		};
	}
	public void FuelAddCount(){
		var child = myFuel.FindChild("FuelCount") as Transform;
		int cnt =GV.mUser.FuelCount;
		if(cnt >= GV.mUser.FuelMax){
			cnt = GV.mUser.FuelMax;
		}

		for(int i = 0; i < cnt; i++){
			var temp = child.GetChild(i).gameObject as GameObject;//.SetActive(true);
			temp.SetActive(true);
			if( i == (cnt-1)){
				var effect = child.GetChild((child.childCount-1)).gameObject as GameObject;
				effectFuelAppear(temp.transform, effect);
			}
		}
	}


}
