using UnityEngine;
using System.Collections;

public class RaceModeAnimaintion : MonoBehaviour {

	private Animation winAni;


	// Use this for initialization
	void Awake () {
		winAni = gameObject.GetComponent<Animation>() as Animation;
	}

	public void SelectAniPlay(string aniName){
		winAni[aniName].time = 0;
		winAni[aniName].speed = 1.0f;
		winAni.Play(aniName);
	}
	

}
