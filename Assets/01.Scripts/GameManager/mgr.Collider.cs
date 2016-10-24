using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public partial class GameManager :  MonoSingleton< GameManager > {
	private bool bFinish = false;
	private int checkPos = 0;
	private float sDelay = 0.0f;
	private GameObject snapObj;
	void StartCheckPoint(){
		if(checkPos == 1){
			
		}else if(checkPos == 2){
			
		}else if(checkPos == 3){
			
		}
	}
	
	public IEnumerator DragSnapShotStart(){
		if(!isCompetiton){
			bFinish = false;
			yield break; //  return new WaitForSeconds(0.2f);
		}
		AudioManager.Instance.CarVolumDown(0.0f);
		bFinish = true;
		yield return null;
	}
	
	public IEnumerator DragFinishCam_2_1(){
		if(!isCompetiton){
			bFinish = false;
			yield break; //  return new WaitForSeconds(0.2f);
		}
		AudioManager.Instance.CarVolumDown(0.0f);
		bFinish = true;
		Utility.LogWarning("animation Speed : " + _AniSpeed); 
		if(_AniSpeed < 1.0f){
			Time.timeScale = 0.12f;
		}else if (_AniSpeed >5.5f){
			Time.timeScale = 0.012f;
		}else{
			float fTime =(_AniSpeed / 0.2f * -0.005f) + 0.15f;
			fTime = Mathf.Round(fTime/ .001f)*.001f;
			Time.timeScale = fTime;
		}
		
		snapObj= mgrgui.findPanel("snapshot");// GameObject.Find("snapshot");
		Camera cam = currentCamera;
		checkPos = 1;
		Invoke("StartCheckPoint",0.05f);
		cam.fieldOfView = 20;
		snapObj.SetActive(true);
		//yield return new WaitForSeconds(delay);
		_uiSound.CarCameraShutterSound();
		//Utility.LogWarning("DragFinishCam_2_1");
		yield return null;
	}
	
	public IEnumerator DragFinishCam_2_2(){
		if(!bFinish){
			yield break; //  return new WaitForSeconds(0.2f);
		}
		float delay = 0.02f;
		checkPos = 2;
		Camera cam = currentCamera;
		cam.fieldOfView = 18.5f;
		_uiSound.CarCameraShutterSound();
		Utility.LogWarning("DragFinishCam_2_2");
		yield return null;
	}
	
	public IEnumerator DragFinishCam_2_3(){
		if(!bFinish){
			
			//		yield  return new WaitForSeconds(1.0f);
		}else{
			if(IsInvoking("StartCheckPoint")){
				CancelInvoke("StartCheckPoint");
			}
			checkPos = 3;
			Invoke("StartCheckPoint",0.05f);
			float delay = 0.02f;
			Camera cam = currentCamera;
			cam.fieldOfView = 15;
			_uiSound.CarCameraShutterSound();
		}
		yield return null;
	}


	IEnumerator DestroyAfterFrame(){
		yield return new WaitForEndOfFrame();
		GameObject game = mgrgui.findPanel("snapshot");
		game.SetActive(false);
	}
	public IEnumerator DragFinishTrigger(){
		Time.timeScale = 1.0f;
		StartCoroutine("DestroyAfterFrame");
		Camera cam = currentCamera;
		cam.fieldOfView = 25;	
		AudioManager.Instance.CarVolumDown(1.0f);
		mgrgui.CarSoundVolumeDown(()=>{
			AudioManager.Instance.CarVolumDown(0.3f);
			AudioManager.Instance.audioPitch(1.0f);
		});
		
		StartCoroutine("DecreaseAnispeed");
		StartCoroutine(RankCheckPoint("StartFinalRankCheck", 9));
		StartCoroutine("FinishGUI");

		yield return null;
	}
	public IEnumerator RegularSnapShotStart(){
		
		if(!isCompetiton){
			bFinish = false;
			yield break; //  return new WaitForSeconds(0.2f);
		}
		AudioManager.Instance.CarVolumDown(0.0f);
		bFinish = true;
		yield return null;
	}
	
	
	
	public IEnumerator RegularFinishCam_2_1(){
		if(!isCompetiton){
			bFinish = false;
			yield break; //  return new WaitForSeconds(0.2f);
		}
		AudioManager.Instance.CarVolumDown(0.0f);
		bFinish = true;
		Utility.LogWarning("animation Speed : " + _AniSpeed); 
		if(_AniSpeed < 1.0f){
			Time.timeScale = 0.12f;
		}else if (_AniSpeed >5.5f){
			Time.timeScale = 0.012f;
		}else{
			float fTime =(_AniSpeed / 0.2f * -0.005f) + 0.15f;
			fTime = Mathf.Round(fTime/ .001f)*.001f;
			Time.timeScale = fTime;
		}
		//	Time.timeScale = 0.1f;
		float delay = 0.02f;
		if(_AniSpeed > 1.8f){
			delay = 0.01f;
		}
		GameObject game = mgrgui.findPanel("snapshot");// GameObject.Find("snapshot");
		Camera cam = currentCamera;
		
		checkPos = 1;
		Invoke("StartCheckPoint",0.05f);
		
		cam.fieldOfView = 20;
		game.SetActive(true);
		_uiSound.CarCameraShutterSound();
		yield return null;
	}
	
	public IEnumerator RegularFinishCam_2_2(){
		if(!bFinish){
			yield break; 
		}
		float delay = 0.02f;
		if(_AniSpeed > 1.8f){
			delay = 0.01f;
		}
		if(IsInvoking("StartCheckPoint")){
			CancelInvoke("StartCheckPoint");
		}
		checkPos = 2;
		Invoke("StartCheckPoint",0.05f);
		Camera cam = currentCamera;
		cam.fieldOfView = 18.5f;
		_uiSound.CarCameraShutterSound();
		yield return null;
	}
	
	public IEnumerator RegularFinishCam_2_3(){
		if(!bFinish){
			yield return new WaitForSeconds(1.0f);
			
		}else{
			float delay = 0.02f;
			if(_AniSpeed > 1.8f){
				delay = 0.01f;
			}
			if(IsInvoking("StartCheckPoint")){
				CancelInvoke("StartCheckPoint");
			}
			checkPos = 3;
			Invoke("StartCheckPoint",0.05f);
			GameObject game = mgrgui.findPanel("snapshot");// GameObject.Find("snapshot");
			Camera cam = currentCamera;
			cam.fieldOfView = 15;
			_uiSound.CarCameraShutterSound();
		
		}
	
		yield return null;
	}
	public IEnumerator RaceFinishTrigger(){
		Time.timeScale = 1.0f;
		Camera cam = currentCamera;
		cam.fieldOfView = 25;	
		StartCoroutine("DestroyAfterFrame");
		AudioManager.Instance.CarVolumDown(1.0f);
		mgrgui.CarSoundVolumeDown(()=>{
			AudioManager.Instance.CarVolumDown(0.3f);
			AudioManager.Instance.audioPitch(1.0f);
		});
		
		StartCoroutine("DecreaseAnispeed");
		if(Global.isTutorial){
			StartCoroutine("RaceTutorialFinish");
		}else{
			StartCoroutine(RankCheckPoint("StartFinalRankCheck", 9));
		}
		StartCoroutine("FinishGUI");
		
		yield return null;
	}
	
	
}
