using UnityEngine;
using System.Collections;

public class RTSCamModifiy : MonoBehaviour {
	private float dist;
	
	private float orbitSpeedX;
	private float orbitSpeedY;
	private float zoomSpeed;
	
	public float rotXSpeedModifier=0.25f;
	public float rotYSpeedModifier=0.25f;
	public float zoomSpeedModifier=5;
	
	public float minRotX=-20;
	public float maxRotX=70;
	public float maxRotY = 20;
	public float minRotY = -20;
	public float panSpeedModifier=1f;
	Vector3 targetPos;
	// Use this for initialization

	void Start () {
		dist=transform.localPosition.z;
		targetPos = transform.localPosition;
	}

	void EnableActive(){
		isClickActive  = true;
	}
	
	void OnEnable(){
		Gesture.onDraggingE += OnDragging;
		Gesture.onMFDraggingE += OnMFDragging;
		Gesture.onMultiTapE += OnMultiTap;
		Gesture.onDoubleTapE += OnDoubleTap;
		//Gesture.onPinchE += OnPinch;
		orbitSpeedX=0;
		orbitSpeedY=0;
		zoomSpeed=0;
		Invoke ("EnableActive",0.3f);
	}
	
	void OnDisable(){
		Gesture.onDraggingE -= OnDragging;
		Gesture.onMFDraggingE -= OnMFDragging;
		Gesture.onMultiTapE -= OnMultiTap;
		Gesture.onDoubleTapE -= OnDoubleTap;
		//Gesture.onPinchE -= OnPinch;
		isClickActive  = false;
	}
	
	
	// Update is called once per frame
	void Update () {
		
		//get the current rotation
		float x=transform.parent.rotation.eulerAngles.x;
		float y=transform.parent.rotation.eulerAngles.y;
		
		//make sure x is between -180 to 180 so we can clamp it propery later
		if(x>180) x-=360;
		if(y>180) y-=360;

		if(y > maxRotY){
			y = maxRotY;
		}else if(y < minRotY){
			y = minRotY;
		}

		Quaternion rotationY=Quaternion.Euler(0, y, 0)*Quaternion.Euler(0, orbitSpeedY, 0);
		Quaternion rotationX=Quaternion.Euler(Mathf.Clamp(x+orbitSpeedX, minRotX, maxRotX), 0, 0);
		
		//apply the rotation
		transform.parent.rotation=rotationY*rotationX;
		
		//calculate the zoom and apply it
		dist+=Time.deltaTime*zoomSpeed*0.01f;
		dist=Mathf.Clamp(dist, -15, -3);
		//transform.localPosition=new Vector3(0, 0, dist);
		transform.localPosition = new Vector3(targetPos.x, targetPos.y, dist);
		//reduce all the speed
		orbitSpeedX*=(1-Time.deltaTime*12);
		orbitSpeedY*=(1-Time.deltaTime*3);
		zoomSpeed*=(1-Time.deltaTime*4);
		
		//use mouse scroll wheel to simulate pinch, sorry I sort of cheated here
		zoomSpeed+=Input.GetAxis("Mouse ScrollWheel")*500*zoomSpeedModifier;
	}
	
	//called when one finger drag are detected
	void OnDragging(DragInfo dragInfo){
		//if the drag is perform using mouse2, use it as a two finger drag
		if(!isClickActive) return;
		if(dragInfo.isMouse && dragInfo.index==1) OnMFDragging(dragInfo);
		//else perform normal orbiting
		else{
			//vertical movement is corresponded to rotation in x-axis
			orbitSpeedX=-dragInfo.delta.y*rotXSpeedModifier;
			//horizontal movement is corresponded to rotation in y-axis
			orbitSpeedY=dragInfo.delta.x*rotYSpeedModifier;
		}
	}
	
	//called when pinch is detected
	void OnPinch(PinchInfo pinfo){
		//zoomSpeed-=pinfo.magnitude*zoomSpeedModifier;
	}
	
	//called when a dual finger or a right mouse drag is detected
	void OnMFDragging(DragInfo dragInfo){
		if(!isClickActive) return;
		/*return;
		//make a new direction, pointing horizontally at the direction of the camera y-rotation
		Quaternion direction=Quaternion.Euler(0, transform.parent.rotation.eulerAngles.y, 0);
		
		//calculate forward movement based on vertical input
		Vector3 moveDirZ=transform.parent.InverseTransformDirection(direction*Vector3.forward*-dragInfo.delta.y);
		//calculate sideway movement base on horizontal input
		Vector3 moveDirX=transform.parent.InverseTransformDirection(direction*Vector3.right*-dragInfo.delta.x);
		
		//move the cmera 
		transform.parent.Translate(moveDirZ * panSpeedModifier * Time.deltaTime);
		transform.parent.Translate(moveDirX * panSpeedModifier * Time.deltaTime);
		*/
	}
	bool isClickActive = false;
	/*
	void OnMultiTap(Tap tap){
		return;
	//	OnDoubleTap(tap.pos);
	//	return;
		if(!isClickActive) return;
		if(Global.isLobbyRotate) return;
		GameObject.Find ("LobbyUI").SendMessage("OnLobbyBack");
	}*/

	void OnMultiTap(Tap tap){
		if(!isClickActive) return;
		if(Global.isLobbyRotate) return;
		GameObject.Find ("LobbyUI").SendMessage("OnLobbyBack");
	}

	void OnDoubleTap(Vector2 pos){
	//	if(!isClickActive) return;
	//	if(Global.isAnimation) return;
	//	GameObject.Find ("LobbyUI").SendMessage("OnLobbyBack");
	}

}
