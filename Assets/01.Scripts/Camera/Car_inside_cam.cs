using UnityEngine;
using System.Collections;

public class Car_inside_cam : MonoBehaviour {

	public bool targetMoving = false;
	public bool CameraMoving = false;
	//public bool itweencheck = false;
	
	public bool FOVmoving = true;
	public float FOV = 60.0f;
	public float FOV_Value = 0.01f;
	public float FOV_Stop = 100.0f;
	

	//public Transform startTarget;
	//public Transform endTarget;
	
	//public float smoothTime = 5.0f;
	//private Vector3 velocity = new Vector3(0.0001f, 0.0001f, 0.0001f);
	public Vector3 targetPos;
	public float targetMaxOffset = 5.0f;
	public float targetMinOffset = -5.0f;
	private Vector3 target_Max_position;
	private Vector3 target_Min_position;
	
	public Transform target;
	public Vector3 cameraPos;
	public float cameraMaxOffset = 10.0f;
	public float cameraMinOffset = -10.0f;
	private Vector3 camera_Max_position;
	private Vector3 camera_Min_position;
	
	public float damping = 6.0f;
	

	
	void Start(){
		//GameObject _gameObejct = GameObject.Find("CameraTarget");
		//target = _gameObejct.GetComponent<Transform>() as Transform;
		camera.fieldOfView = FOV;
		
		target_Max_position = target.localPosition + Vector3.one*targetMaxOffset;
		target_Min_position = target.localPosition + Vector3.one*targetMinOffset;
		camera_Max_position = transform.position + Vector3.one*cameraMaxOffset;
		camera_Min_position = transform.position + Vector3.one*cameraMinOffset;
		transform.LookAt(target);	
	}
	
	// Update is called once per frame
	bool isPause = false;
	void OnPauseMessage(){
		isPause  = true;
	}
	
	void OnResumeMessage(){
		isPause = false;
	}


	void LateUpdate () {	
		if(isPause) return;
		if (target) {
			
			if(FOVmoving){
				if(FOV_Value > 0){
					if(FOV > FOV_Stop){
						//Utility.LogError(" Check in FOV_STOP  lager than FOV " +gameObject.name);
					}
					camera.fieldOfView += FOV_Value;
					if(camera.fieldOfView >= FOV_Stop){
						camera.fieldOfView = FOV_Stop;
					}
				}else if(FOV_Value < 0){
					if(FOV < FOV_Stop){
						//Utility.LogError(" Check in FOV_STOP  less than FOV " +gameObject.name);
					}
					camera.fieldOfView += FOV_Value;
					if(camera.fieldOfView <= FOV_Stop ){
						camera.fieldOfView = FOV_Stop;
					}
				}
			} // end of FOVmoving
	
			
			if (targetMoving)
			{	
				//Vector3 targetOffset = target.position - endTarget.position;

				//startTarget.position = Vector3.SmoothDamp(startTarget.position, endTarget.position, ref velocity, smoothTime);
				transform.LookAt(target);
				target.localPosition += targetPos;
				//transform.LookAt(target);
				Vector3 maxoffset = target.localPosition - target_Max_position; // minus
				//Utility.Log ("maxoffset"+maxoffset);
				Vector3 minoffset = target.localPosition - target_Min_position; // plus
				Vector3 temp = target.localPosition;
				if(maxoffset.x >0 || minoffset.x <0){
					temp.x -= targetPos.x;
					target.localPosition = temp;
				}
				if(maxoffset.y >0 || minoffset.y <0){
					temp.y -= targetPos.y;
					target.localPosition = temp;
				}
				if(maxoffset.z >0|| minoffset.z <0){
					temp.z -= targetPos.z;
					target.localPosition = temp;
				}
				
				//Utility.Log ("target test " + startTarget.localPosition);
				//transform.LookAt(startTarget);
				
			}else if(CameraMoving){
				Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);				
				transform.LookAt(target);
				transform.position += cameraPos;
				Vector3 Maxoffset = transform.position - camera_Max_position;
				Vector3 Minoffset = transform.position - camera_Min_position;
				Vector3 tmp = transform.position;
				
				if(Maxoffset.x >0 || Minoffset.x <0){
					tmp.x -= cameraPos.x;
					transform.position = tmp;
				}
				if(Maxoffset.y >0 || Minoffset.y <0){
					tmp.y -= cameraPos.y;
					transform.position = tmp;
				}
				if(Maxoffset.z >0|| Minoffset.z <0){
					tmp.z -= cameraPos.z;
					transform.position = tmp;
				}
			}
				
		}
		

	}
}
