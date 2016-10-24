using UnityEngine;
using System.Collections;

public class CamSpeedShake : MonoBehaviour {

	
	
	public Vector3 originPosition;
	public Quaternion originRotation;
	public float shake_decay;
	public float shake_intensity ;
	public bool isShake = true;
	
	void Start(){
		Shake ();
	}
	
	bool isPause = false;
	void OnPauseMessage(){
		isPause  = true;
	}
	
	void OnResumeMessage(){
		isPause = false;
	}
	
	
	void Update(){
		if(isPause) return;
		if(shake_intensity > 0 && isShake){
			transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
			transform.rotation = new  Quaternion(
				originRotation.x + Random.Range(-shake_intensity,shake_intensity)*.2F,
				originRotation.y + Random.Range(-shake_intensity,shake_intensity)*.2F,
				originRotation.z + Random.Range(-shake_intensity,shake_intensity)*.2F,
				originRotation.w + Random.Range(-shake_intensity,shake_intensity)*.2F);
			shake_intensity -= shake_decay;
			//Utility.Log(" inten : " + shake_intensity + " decay : "+ shake_decay);
		}else{
			
		}
	}
	
	public void Shake(){
		originPosition = transform.position;
		originRotation = transform.rotation;
		shake_intensity = .1F;
		shake_decay = 0.003F;
	}
	
	
	
}
