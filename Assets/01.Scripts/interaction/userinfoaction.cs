using UnityEngine;
using System.Collections;

public class userinfoaction : MonoBehaviour {


	private Vector3 _pos;
	//public Camera guiCamera;
	public GameObject[] _target;// = new GameObject[5];
	public Transform[] tr = new Transform[7];
	public Transform[] _tr;
	public Transform[] _multiTarget;
	public Transform[] _multiUser;
	public Transform[] _multiTr;
	public Transform modeNames;
	public Transform[] pvpTarget = new Transform[2];
	//bool isEnd = false;
	int count;
	void Awake(){
		int _cnt = transform.childCount;
		for(int i = 0; i < _cnt; i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	void Start(){
		MainRaceType rType = Global.gRaceInfo.mType;
		switch(rType){
		case MainRaceType.Champion:
			UserInfoC();
			//modeNames.GetChild(0).gameObject.SetActive(true);
			modeNames.FindChild("Champion").gameObject.SetActive(true);
			break;
		case MainRaceType.mEvent:
			modeNames.FindChild("EVENT").gameObject.SetActive(true);
		//	if(Global.gRaceInfo.sType == SubRaceType.RegularRace) UserInfoFD();
		//	else 	UserInfoPVP();
		//	int _cnt = transform.childCount;
		//	for(int i = 0; i < _cnt; i++){
		//		transform.GetChild(i).gameObject.SetActive(false);
		//	}
			break;
		case MainRaceType.Club:
			modeNames.FindChild("Club").gameObject.SetActive(true);
			UserInfoClub();
			break;
		case MainRaceType.Regular:
			if(Global.gRaceInfo.sType == SubRaceType.RegularRace){
				UserInfoRW();
				modeNames.FindChild("Regular").gameObject.SetActive(true);
			}else{
				modeNames.FindChild("Drag").gameObject.SetActive(true);
				UserInfoPVP();
			}
			break;
		case MainRaceType.Weekly:
			modeNames.FindChild("Ranking").gameObject.SetActive(true);
			UserInfoRW();
			break;
		case MainRaceType.PVP:
			modeNames.FindChild("PVP").gameObject.SetActive(true);
			UserInfoPVP();
			break;
		default:
			RWobj.SetActive(false);
			MultiUser.SetActive(false);
			break;
		}

		if(Global.gRaceInfo.sType == SubRaceType.DragRace){
			modeNames.parent.FindChild("MiniMap").gameObject.SetActive(false);
			modeNames.parent.FindChild("MiniMap_Drag").gameObject.SetActive(true);
		}else{
			//var tr = modeNames.parent.FindChild("MiniMap_Pitstop") as Transform;
		}


	}

	void RaceTypeUserInfo(){
		int _cnt = transform.childCount;
		for(int j = 0; j<_tr.Length; j++){
			//Utility.Log (" tr name " + _tr[j].name + " num " + j);
			for(int i = 0; i < _cnt; i++){
				var tempobj = transform.GetChild(i).gameObject as GameObject;
				if(!tempobj.activeSelf){
					if(_tr[j].name == tempobj.name){
						tempobj.SetActive(true);
						break;
					} // end if
				} // if
			} // for transform
		} // end _tr

		for(int i = 0; i < _cnt; i++){
			var temp = transform.GetChild(i).gameObject as GameObject;
			if(temp.activeSelf){
				if(temp.name == "B_Car"){
					temp.transform.FindChild("Label_ID").GetComponent<UILabel>().text = GV.UserNick;
					//if(Global.myProfile == null) Global.myProfile = (Texture2D)Global.gDefaultIcon;
					temp.transform.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = 
						AccountManager.instance.myPicture;
				}else{
					string name = temp.name;
					int id  = int.Parse(name[5].ToString());
					id--;
					//Utility.Log(id);
					temp.transform.FindChild("Label_ID").GetComponent<UILabel>().text = 
						Global.gAICarInfo[id].userNick;
				//	temp.transform.FindChild("Label_LV").GetComponent<UILabel>().text = 
				//		string.Empty;
					Texture2D tex = Global.gAICarInfo[id].userTexture;
					if(tex == null) tex = (Texture2D)Global.gDefaultIcon;
					temp.transform.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture = tex;
					//Utility.Log(Global.gAICarInfo[i].userNick );
				}
			}
		}
	}

	void mutiUserSet(){
		//gameObject.SetActive(false);

	//	for(int i = 0; i < count; i++){
	//		_multiUser[i].parent.transform.GetComponent<multiUserAction>().multiSlotDistChange(40);
		//}
		MultiUserCtrl(true);
	}
	public void hiddenUserPanel(){
		
		int cnt = 0;
		for(int i = 0; i < count; i++){
			if(tr[i] == null) return;
			var g = tr[i].parent.parent.gameObject;
			g.SetActive(false);
			//Utility.Log (g.name);
		}
		if(RWobj.activeSelf) {
			RWobj.GetComponent<TweenPosition>().enabled = true;
			if(PVPUser.activeSelf){
				PVPUser.GetComponent<TweenPosition>().enabled = true;
			}
		}

		if(Cobj.activeSelf){
			cnt = Cobj.transform.childCount;
		//	cnt--;
			for(int i = 0; i < cnt; i++){
				Cobj.transform.GetChild(i).gameObject.AddComponent<TweenOut>().tweenOut();
			}
			Invoke("selfDestroy", 1.5f);
			return;
		}

		if(FDobj.activeSelf){
			//_tween = FDobj.GetComponentsInChildren<TweenPosition>();
			cnt =  FDobj.transform.childCount;
			for(int i = 0; i < cnt; i++){
				var temp =  FDobj.transform.GetChild(i) as Transform;
				var t = temp.GetComponent<TweenPosition>() as TweenPosition;
				if(t == null) temp.gameObject.SetActive(false);
				else{
					t.onFinished = delegate(UITweener tween) {
						tween.transform.gameObject.SetActive(false);
					};
					//t.Reset();
					t.enabled = true;
				}
			}
			Invoke("selfDestroy", 1.5f);
			return;
		}

	}

	public void MultiUserCtrl(bool b){
		MainRaceType rType = Global.gRaceInfo.mType;
		if(rType == MainRaceType.Regular || rType == MainRaceType.Weekly) {
			//if(Global.gRaceInfo.sType == SubRaceType.) return;
			for(int i = 0; i < count; i++){
				_multiUser[i].parent.transform.GetComponent<multiUserAction>().SlotActive(b);
				//Utility.LogWarning("MutiiUserCtr " + b);
			}
		}else if(Global.gRaceInfo.sType != SubRaceType.RegularRace){
			for(int i = 0; i < count; i++){
				_multiUser[i].parent.transform.GetComponent<multiUserAction>().SlotActive(b);
			//	Utility.LogWarning("MutiiUserCtr " + b);
			}
		}else return;
	}
	
	public void AISlotCtrl(int id){
		bool b = false;
		MainRaceType rType = Global.gRaceInfo.mType;

		if(rType == MainRaceType.Regular || rType == MainRaceType.Weekly) {
			if(Global.gRaceInfo.sType != SubRaceType.RegularRace) b = false;
			else b = true;
		}else b = false;

		if(b)
			_multiUser[id+1].parent.transform.GetComponent<multiUserAction>().SlotActive(false);
	}
	
	public GameObject FDobj;
	public GameObject Cobj;
	public GameObject RWobj, MultiUser, PVPUser;

	public void selfDestroy(){

		_target = null;
		tr = null;
		gameObject.SetActive(false);
		FDobj.SetActive(false);
		Cobj.SetActive(false);
		PVPUser.SetActive(false);
		return;

	}

	public void LateUpdate(){
		//if(isEnd) return; 
		//Utility.Log (GameManager.instance.currentCamera.WorldToViewportPoint(transform.position));
		//transform.localPosition = GameManager.instance.currentCamera.WorldToViewportPoint(transform.position);
		/*for(int i = 0; i < count; i++){
			_pos = GameManager.instance.currentCamera.WorldToViewportPoint(_target[i].transform.position);
			_pos = guiCamera.ViewportToWorldPoint(_pos);
			_pos.z = 0.0f;
			_tr[i].position = _pos;
		}*/

		//for(int i = 0; i < count; i++){
		//	_pos = GameManager.instance.currentCamera.WorldToViewportPoint(_multiTarget[i].transform.position);
		//	_pos = guiCamera.ViewportToWorldPoint(_pos);
		//	_pos.z = 0.0f;
		//	_multiTr[i].position = _pos;
		//}

	}

	void UserInfoC(){
		Cobj.SetActive(true);
		var temp = Cobj.transform.FindChild("AIPlayer") as Transform;	
		temp.FindChild("G_Cha").GetComponentInChildren<UISprite>().spriteName = 
			Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[1].AICrewID).ToString()+"A";
		temp.FindChild("G_Bar").GetComponentInChildren<UILabel>().text =
			Global.gAICarInfo[1].userNick;
		temp = Cobj.transform.FindChild("Player") as Transform;
		//int tempid =Base64Manager.instance.GlobalEncoding(Global.MyCrewID);

		int tempid = GV.PlayCrewID;
		temp.FindChild("G_Cha").GetComponentInChildren<UISprite>().spriteName = 
			tempid.ToString()+"A";
		temp.FindChild("G_Bar").GetComponentInChildren<UILabel>().text =
			GV.UserNick;
		temp = Cobj.transform.FindChild("Raceicon").GetChild(0);
		temp.FindChild("lbName").GetComponent<UILabel>().text =Global.gRaceInfo.raceName;
		temp.FindChild("raceIcon").GetComponent<UISprite>().spriteName = Global.gRaceInfo.trackID+"L";
	}
	void UserInfoPVP(){

		PVPUserInfo();
	
	}

	void UserInfoClub(){
		
		ClubUserInfo();
		
	}
	void UserInfoFD(){
		FDobj.SetActive(true);
		var temp = FDobj.transform.FindChild("Player") as Transform;	
		temp.FindChild("Label_ID").GetComponent<UILabel>().text = GV.UserNick;
		temp.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture =AccountManager.instance.myPicture;// Global.myProfile;

		for(int  i = 1 ; i < FDobj.transform.childCount; i++){
			FDobj.transform.GetChild(i).FindChild("Label_ID").GetComponent<UILabel>().text
				= Global.gAICarInfo[i-1].userNick;
			FDobj.transform.GetChild(i).FindChild("crew_icon").GetComponent<UISprite>().spriteName
				= Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[i-1].AICrewID).ToString()+"A";
		}
	}


	void UserInfoRW(){
		//isEnd = false;
		RWobj.SetActive(true);
		MultiUser.SetActive(true);
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("UserInfoArrow");
		//Utility.LogWarning("userinfoarrow " + gos.Length);
		count = gos.Length;
		_tr =  new Transform[count];
		_target = new GameObject[count];
		_multiTarget = new Transform[count];
		_multiTr = new Transform[count];
		int num = 0;
		for(int i = 0; i < count ;i++){
			switch(gos[i].name){
			case "COM0_Target":
				_target[i] = gos[i];
				_tr[i] = tr[0];
				num = 0;
				break;
			case "COM1_Target":
				_target[i] = gos[i];
				_tr[i] = tr[3];
				num = 1;
				break;
			case "COM2_Target":
				_target[i] = gos[i];
				_tr[i] = tr[4];
				num = 2;
				break;
			case "COM3_Target":
				_target[i] = gos[i];
				_tr[i] = tr[1];
				num = 3;
				break;
			case "COM4_Target":
				_target[i] = gos[i];
				_tr[i] = tr[2];
				num = 4;
				break;
			case "COM5_Target":
				_target[i] = gos[i];
				_tr[i] = tr[5];
				num = 5;
				break;
			case "COM6_Target":
				_target[i] = gos[i];
				_tr[i] = tr[6];
				num = 6;
				break;
			}

			_multiTarget[i] = gos[i].transform;
			_multiTr[i] = _multiUser[num];
			_multiTr[i].parent.GetComponent<multiUserAction>().SetInfo(num, _multiTarget[i]);
			_multiUser[num].parent.transform.gameObject.SetActive(true);
			//_multiUser[num].parent.transform.GetComponent<multiUserAction>().SlotActive(false);
			//_multiUser[num].gameObject.SetActive(true);
		}
		// track 01 =>  4 , 1, 3, 0 , 2 // 0,4,1,3,2
		// weekly 0,4,5,1,3,6,2
		RaceTypeUserInfo();
	}

	void PVPUserInfo(){
		PVPUser.SetActive(true);
		MultiUser.SetActive(true);
		for(int i = 0; i < RWobj.transform.childCount; i++){
			RWobj.transform.GetChild(i).gameObject.SetActive(false);
		}
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("UserInfoArrow");
		count = gos.Length;
		_tr =  new Transform[count];
		_target = new GameObject[count];
		_multiTarget = new Transform[count];
		_multiTr = new Transform[count];
		int num = 0;
		for(int i = 0; i < count ;i++){
			switch(gos[i].name){
			case "COM0_Target":
				_target[i] = gos[i];
				_tr[i] = pvpTarget[0];
				num = 0;
				break;
			case "COM1_Target":
				_target[i] = gos[i];
				_tr[i] = pvpTarget[1];
				num = 1;
				break;
			case "COM2_Target":
				_target[i] = gos[i];
				_tr[i] = tr[4];
				num = 2;
				break;
			case "COM3_Target":
				_target[i] = gos[i];
				_tr[i] = tr[1];
				num = 3;
				break;
			case "COM4_Target":
				_target[i] = gos[i];
				_tr[i] = tr[2];
				num = 4;
				break;
			case "COM5_Target":
				_target[i] = gos[i];
				_tr[i] = tr[5];
				num = 5;
				break;
			case "COM6_Target":
				_target[i] = gos[i];
				_tr[i] = tr[6];
				num = 6;
				break;
			}
			
			_multiTarget[i] = gos[i].transform;
			_multiTr[i] = _multiUser[num];
			_multiTr[i].parent.GetComponent<multiUserAction>().SetInfo(num, _multiTarget[i]);
			_multiUser[num].parent.transform.gameObject.SetActive(true);
	}
		//RaceTypeUserInfo();
		var temp = PVPUser.transform.FindChild("Player") as Transform;	
		temp.FindChild("Label_ID").GetComponent<UILabel>().text = GV.UserNick;
		temp.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture =AccountManager.instance.myPicture;

		temp = PVPUser.transform.FindChild("OtherPlayer") as Transform;	
		temp.FindChild("Label_ID").GetComponent<UILabel>().text =  Global.gAICarInfo[0].userNick;
		temp.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture =Global.gAICarInfo[0].userTexture;

	}


	void ClubUserInfo(){
		PVPUser.SetActive(true);
		MultiUser.SetActive(true);
		for(int i = 0; i < RWobj.transform.childCount; i++){
			RWobj.transform.GetChild(i).gameObject.SetActive(false);
		}
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("UserInfoArrow");
		count = gos.Length;
		_tr =  new Transform[count];
		_target = new GameObject[count];
		_multiTarget = new Transform[count];
		_multiTr = new Transform[count];
		int num = 0;
		for(int i = 0; i < count ;i++){
			switch(gos[i].name){
			case "COM0_Target":
				_target[i] = gos[i];
				_tr[i] = pvpTarget[0];
				num = 0;
				break;
			case "COM1_Target":
				_target[i] = gos[i];
				_tr[i] = pvpTarget[1];
				num = 1;
				break;
			case "COM2_Target":
				_target[i] = gos[i];
				_tr[i] = tr[4];
				num = 2;
				break;
			case "COM3_Target":
				_target[i] = gos[i];
				_tr[i] = tr[1];
				num = 3;
				break;
			case "COM4_Target":
				_target[i] = gos[i];
				_tr[i] = tr[2];
				num = 4;
				break;
			case "COM5_Target":
				_target[i] = gos[i];
				_tr[i] = tr[5];
				num = 5;
				break;
			case "COM6_Target":
				_target[i] = gos[i];
				_tr[i] = tr[6];
				num = 6;
				break;
			}
			
			_multiTarget[i] = gos[i].transform;
			_multiTr[i] = _multiUser[num];
			_multiTr[i].parent.GetComponent<multiUserAction>().SetClubInfo(num, _multiTarget[i]);
			_multiUser[num].parent.transform.gameObject.SetActive(true);
		}
		//RaceTypeUserInfo();
		var temp = PVPUser.transform.FindChild("Player") as Transform;	
		temp.FindChild("Label_ID").GetComponent<UILabel>().text = GV.UserNick;
		temp.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture =AccountManager.instance.myPicture;
		
		temp = PVPUser.transform.FindChild("OtherPlayer") as Transform;	
		temp.FindChild("Label_ID").GetComponent<UILabel>().text =  string.Empty;//Global.gAICarInfo[0].userNick;
		temp.FindChild("Label_StarNum").GetComponent<UILabel>().text = string.Format("X {0}",CClub.mClubRaceStarCount);
		temp.FindChild("Label_StarNum").gameObject.SetActive(true);
		temp.FindChild("Kakao_icon").GetComponent<UITexture>().mainTexture =Global.gAICarInfo[0].userTexture;
		
	}
}


