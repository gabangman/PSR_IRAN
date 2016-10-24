using UnityEngine;
using System.Collections;

public class RaceCameraAction : LookatTarget {

	[SerializeField] float swaySpeed = .5f;
	[SerializeField] Vector2 swaySpeedRange = new Vector2(1,20);
	[SerializeField] float baseSwayAmount = .5f;
	[SerializeField] Vector2 baseSwayAmountRange = new Vector2(10,5);
	[SerializeField] float trackingSwayAmount = .5f;
	[Range(-1,1)][SerializeField] float trackingBias = 0;


	public  bool isHandHeld = false;
	public bool targetMoving = false;
	public bool CameraMoving = false;
	public bool FOVmoving = true;
	public float FOV = 60.0f;
	public float FOV_Value = 0.01f;
	public float FOV_Stop = 100.0f;
	public Vector3 targetPos;
	public float targetMaxOffset = 5.0f;
	public float targetMinOffset = -5.0f;
	private Vector3 target_Max_position;
	private Vector3 target_Min_position;
	public Transform camTarget;
	public Vector3 cameraPos;
	public float cameraMaxOffset = 10.0f;
	public float cameraMinOffset = -10.0f;
	private Vector3 camera_Max_position;
	private Vector3 camera_Min_position;
	public float damping = 6.0f;
	
	public void GearCameraValueChange(Vector2 swayspeed, Vector3 swayrange){
		swaySpeedRange = swayspeed;
		baseSwayAmountRange = swayrange;
	}
	public void GearCameraValueChange(Vector2 swayspeed, Vector3 swayrange, int n){
		swaySpeedRange = swayspeed;
		baseSwayAmountRange = swayrange;
		initTime = Time.timeSinceLevelLoad;
	}
	void InsideCamInit(){
		camera.fieldOfView = FOV;
		target_Max_position = camTarget.localPosition + Vector3.one*targetMaxOffset;
		target_Min_position = camTarget.localPosition + Vector3.one*targetMinOffset;
		camera_Max_position = transform.position + Vector3.one*cameraMaxOffset;
		camera_Min_position = transform.position + Vector3.one*cameraMinOffset;
		transform.LookAt(camTarget);	
	
	}
	public float currentFov = 0;
	void OnEnable(){
		if(isHandHeld)
			base.Start();
		else 
			InsideCamInit();

		//currentFov = transform.GetComponent<Camera>().fieldOfView;
		//transform.GetComponent<Camera>().fieldOfView = currentFov;
		//Utility.Log("name " + gameObject.name + " fov " + currentFov);
	}
	void OnDisable(){
		transform.GetComponent<Camera>().fieldOfView = currentFov;
	}
	
	void Awake(){
		currentFov = transform.GetComponent<Camera>().fieldOfView;
		//Utility.Log("name 0" + gameObject.name + " fov " + currentFov);
	}
	
	public void initFOV(){
		//Utility.Log("name 1" + gameObject.name + " fov " + currentFov);
		//Utility.Log("name 2" + gameObject.name + " fov " + transform.GetComponent<Camera>().fieldOfView);
		//transform.GetComponent<Camera>().fieldOfView = currentFov;
	}
	protected override void Start () {
		if(isHandHeld){
			base.Start();
			enableTime = 0.0f;
		//	isTimeTable = true;
		}else 
			InsideCamInit();
	}
	
	protected override void FollowTarget (float deltaTime)
	{
		if(isHandHeld)
			followTarget(deltaTime);
		else 
			InSideCamTarget(deltaTime);
	}
	//Time.timeSinceLevelLoad
	float maxSpeed = 2.0f, minSpeed = 0.5f;
	void ChangeSwayValue(){
		float _speed = GameManager.instance._AniSpeed;
		//float _speed = 1.0f;
		float _value = Mathf.Clamp(_speed, minSpeed, maxSpeed);
		_value  = (_value-minSpeed)/(maxSpeed-minSpeed);
		float delta = (swaySpeedRange.y - swaySpeedRange.x)*_value+swaySpeedRange.x;
		//float _delta = (swayRange.y - swayRange.x)/(maxSpeed-minSpeed);
		//_value = _value*10*_delta;
		swaySpeed = Mathf.Clamp(delta, swaySpeedRange.x, swaySpeedRange.y);
		//Utility.Log(swaySpeed + "   delta " + delta);
		delta = baseSwayAmountRange.x - _value*(baseSwayAmountRange.x -  baseSwayAmountRange.y);
		baseSwayAmount = Mathf.Clamp(delta, baseSwayAmountRange.y, baseSwayAmountRange.x);
		//Utility.Log(baseSwayAmount + "   baseSwayAmount " + delta);
	}

	float enableTime,initTime = 0;
	bool isTimeTable;
	void followTarget(float deltaTime){
		base.FollowTarget(deltaTime);
		ChangeSwayValue();
		//enableTime+=Time.deltaTime;
		enableTime = Time.timeSinceLevelLoad - initTime;
		//Utility.Log("delt " + gameObject.name + "time " + enableTime);
		//float bx = (Mathf.PerlinNoise(0,Time.timeSinceLevelLoad*swaySpeed)-0.5f);
		//float by = (Mathf.PerlinNoise(0,(Time.timeSinceLevelLoad*swaySpeed)+100))-0.5f;
		float bx = (Mathf.PerlinNoise(0,enableTime*swaySpeed)-0.5f);
		float by = (Mathf.PerlinNoise(0,(enableTime*swaySpeed)+100))-0.5f;
		bx *= baseSwayAmount;
		by *= baseSwayAmount;
		//float tx = (Mathf.PerlinNoise(0,Time.timeSinceLevelLoad*swaySpeed)-0.5f)+trackingBias;
		//float ty = ((Mathf.PerlinNoise(0,(Time.timeSinceLevelLoad*swaySpeed)+100))-0.5f)+trackingBias;
		float tx = (Mathf.PerlinNoise(0,enableTime*swaySpeed)-0.5f)+trackingBias;
		float ty = ((Mathf.PerlinNoise(0,(enableTime*swaySpeed)+100))-0.5f)+trackingBias;


		tx *= -trackingSwayAmount * followVelocity.x;
		ty *= trackingSwayAmount * followVelocity.y;
		
		transform.Rotate( bx+tx, by+ty, 0 );
		//Utility.Log("Time.timeSinceLevelLoad " +Time.timeSinceLevelLoad);
	}

	void InSideCamTarget(float deltaTime){

		if (camTarget) {
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
				transform.LookAt(camTarget);
				camTarget.localPosition += targetPos;
				Vector3 maxoffset = camTarget.localPosition - target_Max_position; // minus
				Vector3 minoffset = camTarget.localPosition - target_Min_position; // plus
				Vector3 temp = camTarget.localPosition;
				if(maxoffset.x >0 || minoffset.x <0){
					temp.x -= targetPos.x;
					camTarget.localPosition = temp;
				}
				if(maxoffset.y >0 || minoffset.y <0){
					temp.y -= targetPos.y;
					camTarget.localPosition = temp;
				}
				if(maxoffset.z >0|| minoffset.z <0){
					temp.z -= targetPos.z;
					camTarget.localPosition = temp;
				}
			}else if(CameraMoving){
				Quaternion rotation = Quaternion.LookRotation(camTarget.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);				
				transform.LookAt(camTarget);
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
