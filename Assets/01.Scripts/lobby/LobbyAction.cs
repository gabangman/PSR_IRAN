using UnityEngine;

public class LobbyAction : MonoBehaviour {

	bool isCheck = false;
	//public GameObject debug;

	void setDebugMode(){
	//	Utility.SetActive(true);
	}

	void OnPress (bool isPressed){
		if(Global.isAnimation) return;
		if(!isPressed){
			if(isCheck)	sendLobbyMessage();
		}else{
			isCheck = true;	
		}
	}

	void OnDrag (Vector2 delta){
		isCheck = false;
		return;
	}



	void sendLobbyMessage(){
		if(Global.isPopUp) return;
		GameObject.Find("LobbyUI").SendMessage("OnLobbyClick",SendMessageOptions.DontRequireReceiver);
	}
	void Start(){
		if(transform.name == "Collider") return;
		if(GV.gInfo.extra04 == 2 || GV.gInfo.extra04 == 3){
			
		}else{
			if(GV.gPlusEvent !=0){
			
			}else{
				transform.FindChild("Coin").FindChild("icon_Event").gameObject.SetActive(false);
			}
		}
	}

	public void setChatNewBTNActive(bool b){
		var tr = transform.FindChild("ChatBtn").FindChild("New") as Transform;
		tr.gameObject.SetActive(b);
	}

}
