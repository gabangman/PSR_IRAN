using UnityEngine;
using System.Collections;

public class UIdragPanelInit : MonoBehaviour {

	UIDraggablePanel _uidrga;

	void OnEnable(){
		if(_uidrga == null){
		
		}else{
			_uidrga.ResetPosition();
		//	Utility.LogWarning("uidrag enable");
		}
	}

	void OnDisable(){
	
	}

	void Start(){
		_uidrga = gameObject.transform.FindChild("View").GetComponent<UIDraggablePanel>();
		_uidrga.ResetPosition();
	}

	void OnClose(){
		GetComponent<TweenAction>().ReverseTween(gameObject);
		Global.isPopUp = false;
		//Utility.LogWarning("OnClose");
	}


}
