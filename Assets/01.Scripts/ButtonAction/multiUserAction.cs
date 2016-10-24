using UnityEngine;
using System.Collections;

public class multiUserAction : MonoBehaviour {
	
	bool isMoving = false;
	public Transform tr, childTr;
	public Vector3 _pos;
	public Camera guiCamera;
	public int maxDist = 40;
	public int minDist ;
	public Transform target;
	public float PosZ = 2.65f;
	public void SetInfo(int num, Transform target){
		tr = transform;
		this.target  = target;
		//childTr = tr.GetChild(0);
		Texture2D profile = null;
		string nick = string.Empty;
		if(num == 0){
			profile =AccountManager.instance.myPicture;
			nick = GV.UserNick;
			//	Utility.LogWarning(Global.myProfile.name);
		}else{
			num--;
			nick = Global.gAICarInfo[num].userNick;
			profile = Global.gAICarInfo[num].userTexture;
			if(profile == null) profile = (Texture2D)Global.gDefaultIcon;
		}
		childTr = tr.GetChild(0);
		childTr.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = profile;
		childTr.FindChild("Label_ID").GetComponent<UILabel>().text = nick;
	}

	public void SetClubInfo(int num, Transform target){
		tr = transform;
		this.target  = target;
		Texture2D profile = null;
		string nick = string.Empty;
		childTr = tr.GetChild(0);
		if(num == 0){
			profile =AccountManager.instance.myPicture;
			nick = GV.UserNick;
			childTr.FindChild("Label_ID").GetComponent<UILabel>().text = nick;
		}else{
			num--;
			nick = Global.gAICarInfo[num].userNick;
			profile = Global.gAICarInfo[num].userTexture;
			if(profile == null) profile = (Texture2D)Global.gDefaultIcon;
		
			if(childTr.FindChild("Label_StarNum") != null){
				//childTr.FindChild("Label_StarNum").GetComponent<UILabel>().text = string.Format("X {0}",CClub.mClubRaceStarCount);
				childTr.FindChild("Label_StarNum").GetComponent<UILabel>().text = string.Empty;
				childTr.FindChild("Label_StarNum").gameObject.SetActive(true);
				childTr.FindChild("Label_ID").GetComponent<UILabel>().text = string.Empty;
			}else{
				childTr.FindChild("Label_ID").GetComponent<UILabel>().text =  string.Empty;
			
			}
		}
		childTr.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = profile;
	}
	
	public void SlotActive(bool b){
	//	Utility.LogWarning("SlotActive");
		isMoving = b;
		childTr.gameObject.SetActive(b);
		isvisible = b;
	}
	
	public void multiSlotDistChange(int x){
		//maxDist = 40;
		//	minDist = 0;
	}
	void Awake(){
		if(guiCamera == null)
			guiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
	}
	void OnEnable(){
		
	}
	
	void OnDisable(){
		
	}
	
	bool isvisible = true;
	void LateUpdate(){
		if(!isMoving) return;
		_pos = GameManager.instance.currentCamera.WorldToViewportPoint(target.position);
		if (_pos.x > 0 && _pos.x < 1 && _pos.y > 0 && _pos.y < 1 && _pos.z > 0 && _pos.z < maxDist){
			if(!isvisible)childTr.gameObject.SetActive(true); 
			isvisible = true;
		}else{
			if(isvisible) childTr.gameObject.SetActive(false);
			isvisible = false;
		}
		if(!isvisible) return;
		_pos = guiCamera.ViewportToWorldPoint(_pos);
		_pos.z =1.25f;
		transform.GetChild(0).position = _pos;
	//	Utility.LogWarning(string.Format("{0}  ?  {1}", _pos, childTr.localPosition));
	}
	
}
