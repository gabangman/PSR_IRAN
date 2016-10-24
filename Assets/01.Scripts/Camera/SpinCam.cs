using UnityEngine;
using System.Collections;

public class SpinCam : MonoBehaviour {


	private float orbitSpeedY;
	public float rotYSpeedModifier=0.2f;
	public float maxRotY = 120;
	public float minRotY = -95;
	public float panSpeedModifier=1f;
	bool isClickActive = false;
	// Use this for initialization
	Transform ParentTr;
	public Transform _Team;
	public Transform _Shop;
	public float maxTeamRotY = 45;
	public float minTeamRotY = -45;
	public float centerTeamRotY  = 0;
	bool isObjectPress = false;
	bool isTeamEnable = false;
	void Start () {
		orbitSpeedY=0;
		Invoke ("EnableActive",0.3f);
	//	maxTeamRotY += centerTeamRotY;
	//	minTeamRotY += centerTeamRotY;
	}
	
	void ChangeTarget(bool b){
		if(b) {
			ParentTr = _Team;
			ParentTr.localEulerAngles = new Vector3(0,centerTeamRotY,0);
			rotYSpeedModifier = 0.1f;
			isTeamEnable = true;
		}
		else {
			ParentTr =_Shop;
			ParentTr.localEulerAngles = new Vector3(0,0,0);
			rotYSpeedModifier = 0.2f;
			isTeamEnable = false;
		}
	}

	void OnPress(bool isPress){
		//Utility.Log("ispress " + isPress);
		isObjectPress = isPress;
	}
	void EnableActive(){
		isClickActive  = true;
	}
	
	void OnEnable(){
		Gesture.onDraggingE += OnDragging;
		Gesture.onMFDraggingE += OnMFDragging;
		orbitSpeedY=0;
		Invoke ("EnableActive",0.3f);
	}
	
	void OnDisable(){
		Gesture.onDraggingE -= OnDragging;
		Gesture.onMFDraggingE -= OnMFDragging;
		isClickActive  = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(isTeamEnable){
			float y = ParentTr.rotation.eulerAngles.y;
			
		//	if(y>180) y-=360;
		//	Utility.LogWarning("y " + y);
			if(y > maxTeamRotY){
				y = maxTeamRotY;
			}else if(y < minTeamRotY){
				y = minTeamRotY;
			}
			Quaternion rotationY=Quaternion.Euler(0,Mathf.Clamp(y+orbitSpeedY, minTeamRotY, maxTeamRotY), 0);
			ParentTr.rotation=rotationY;
			orbitSpeedY*=(1-Time.deltaTime*12);
		}else{

			float y = ParentTr.rotation.eulerAngles.y;

			if(y>180) y-=360;
			
			if(y > maxRotY){
				y = maxRotY;
			}else if(y < minRotY){
				y = minRotY;
			}
			Quaternion rotationY=Quaternion.Euler(0,Mathf.Clamp(y+orbitSpeedY, minRotY, maxRotY), 0);
			ParentTr.rotation=rotationY;
			orbitSpeedY*=(1-Time.deltaTime*12);
		}
	}
	
	//called when one finger drag are detected
	void OnDragging(DragInfo dragInfo){
		if(!isClickActive) return;
		if(!isObjectPress) return;
		if(dragInfo.isMouse && dragInfo.index==1){
			OnMFDragging(dragInfo);
		}
		else{
			orbitSpeedY=-dragInfo.delta.x*rotYSpeedModifier;
		}
	}
	
	void OnMFDragging(DragInfo dragInfo){
		if(!isClickActive) return;
		//Quaternion direction=Quaternion.Euler(0, ParentTr.rotation.eulerAngles.y, 0);
		//Vector3 moveDirZ=transform.parent.InverseTransformDirection(direction*Vector3.forward*-dragInfo.delta.y);
		//Vector3 moveDirX=transform.parent.InverseTransformDirection(direction*Vector3.right*-dragInfo.delta.x);
		
		//move the cmera 
		//transform.parent.Translate(moveDirZ * panSpeedModifier * Time.deltaTime);
		//transform.parent.Translate(moveDirX * panSpeedModifier * Time.deltaTime);
		
	}
}
