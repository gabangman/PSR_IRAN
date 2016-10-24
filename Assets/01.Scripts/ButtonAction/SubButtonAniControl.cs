using UnityEngine;
using System.Collections;

public class SubButtonAniControl : MonoBehaviour {

	// Use this for initialization
	private  Animation AniTr;
	void Awake () {
		AniTr = GetComponent<Animation>();
	}
	

	public void ReverseAni(int idx){
		string strAni = string.Empty;
		if(idx == 0){
			strAni = "MyTeamUpMenu_Ani_1";
		}else if(idx == 1){
			strAni = "MyInventory_Ani_1";
		}else if(idx == 2){
			strAni = "MyShop_Ani_2";
		}
		StartCoroutine("ReverseAnimation", strAni);
	}


	IEnumerator ReverseAnimation(string strAni){
		Global.isNetwork = true;
		Global.isAnimation = true;
		AniTr[strAni].time = AniTr[strAni].length;
		AniTr[strAni].speed = -3;
		AniTr.Play(strAni);
		
		while(AniTr.IsPlaying(strAni)){
			yield return null;
		}
		Global.isNetwork = false;
		Global.isAnimation = false;
		gameObject.SetActive(false);
	}


	public void FowardAni(int idx){

		gameObject.SetActive(true);
		string strAni = string.Empty;
		if(idx == 0){
			strAni = "MyTeamUpMenu_Ani_1";
		}else if(idx == 1){
			strAni = "MyInventory_Ani_1";
		}else if(idx == 2){
			strAni = "MyShop_Ani_2";
		}
		StartCoroutine("ForwardAnimation", strAni);
	}

	IEnumerator ForwardAnimation(string strAni){
		Global.isNetwork = true;
		Global.isAnimation = true;
		AniTr[strAni].time = 0;
		AniTr[strAni].speed = 1;
		AniTr.Play(strAni);
		
		while(AniTr.IsPlaying(strAni)){
			yield return null;
		}
		Global.isNetwork = false;
		Global.isAnimation = false;

	}


}
