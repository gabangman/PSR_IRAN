using UnityEngine;
using System.Collections;

public class buildupDisablePopup : MonoBehaviour {

	System.Action<GameObject> callback;

	public void functionCall(System.Action<GameObject> callback){
		this.callback = callback;
	}

	void OnCloseClick(GameObject obj){
		if(callback != null){
			callback(obj);
		}
	}
	
	void OnOkClick(GameObject obj){
		if(callback != null){
			callback(obj);
		}
		
	}

	void OnDisable(){
		this.callback = null;
		Destroy(this);
	}
}
