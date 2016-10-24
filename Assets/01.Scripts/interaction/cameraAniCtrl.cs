using UnityEngine;
using System.Collections;

public class cameraAniCtrl : MonoBehaviour {

	// Use this for initialization
	private Animation myCamAni;
	private Vector3 myPos;
	private Quaternion myRot;

	void Start () {
		myCamAni = gameObject.GetComponent<Animation>() ;
		myPos  =gameObject.transform.position;
		myRot = gameObject.transform.rotation;
	}

	public void InitCamera(){
		if(myCamAni.isPlaying){
			myCamAni.Stop();
		}
		gameObject.transform.position = myPos;
		gameObject.transform.rotation = myRot;
	}



	public void PlayAnimation(string str){
		myCamAni[str].speed = 1;
		myCamAni.Play(str);
	}

	public void PlayAnimationFast(string str){
		myCamAni[str].speed =3;
		myCamAni.Play(str);
	}

	public void ReversePlayAnimationFast(string str){
		myCamAni[str].time = myCamAni[str].length;
		myCamAni[str].speed = -2;
		myCamAni.Play(str);
	}

	public void ReversePlayAnimation(string str){
	//	string str = "Myteam_CarToCrew";
		myCamAni[str].time = myCamAni[str].length;
		myCamAni[str].speed = -1;
		myCamAni.Play(str);
		//Utility.Log ("play reverse ani");
//		Utility.Log(str + "  revers");
	}

	public void PlayAnimation(string str1, string str2 ){
		string str = "Upgrade_"+str1+"To"+str2;
		foreach(AnimationState state in myCamAni){
			if(state.clip.name == str){
				PlayAnimation(str);
				break;
			}		
		}
		if(myCamAni.isPlaying){
			return;
		}
		str =  "Upgrade_"+str2+"To"+str1;
		ReversePlayAnimation( str);
	}

	public void PlayInvenAnimation(string str1, string str2 ){
		string str = str2+"To"+str1;
		foreach(AnimationState state in myCamAni){
			if(state.clip.name == str){
				PlayAnimation(str);
				break;
			}		
		}
		if(myCamAni.isPlaying){
			return;
		}
		str =  str1+"To"+str2;
		//Utility.Log("Reverse" + str);
		ReversePlayAnimation( str);
		
	}


	public bool aniPlaying{
		get{
			return myCamAni.isPlaying;
		}
	}

	
	public void InvenDisMantleAniPlay(){
		myCamAni["Ddismantle"].speed = 1;
		myCamAni.Play("Ddismantle");
	}



}
