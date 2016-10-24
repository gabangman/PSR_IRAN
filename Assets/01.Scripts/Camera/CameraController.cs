using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public int cameraSensitivity = 60;
	
	// Use this for initialization
	void Start () {
		
	}
	bool isPause = false;
	void OnPauseMessage(){
		isPause  = true;
	}
	
	void OnResumeMessage(){
		isPause = false;
	}
	// Update is called once per frame
	void Update () { 
		if(isPause) return;
		if(Input.touchCount==2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved){
			transform.Translate(-1*Input.GetTouch(0).deltaPosition.x/cameraSensitivity, 
			                    -1*Input.GetTouch(0).deltaPosition.y/cameraSensitivity, 0);
			transform.Translate(-1*Input.GetTouch(1).deltaPosition.x/cameraSensitivity, 
			                    -1*Input.GetTouch(1).deltaPosition.y/cameraSensitivity, 0);
		}
		
	}
}


