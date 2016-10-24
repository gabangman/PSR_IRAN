using UnityEngine;
using System.Collections;

public class TryLevelUp : MonoBehaviour {
	
	public delegate void ResponseServer(bool isSucess);
	public ResponseServer onRespons;
	private bool isSucc = false;
	private string upgradeKind;
	
	void OnEnable(){
		
	}
	
	void OnOkClick(){
		if(Global.isNetwork) return;

		string[] _send = gameObject.name.Split("_"[0]);
		Global.isNetwork = true;
		int index = int.Parse(_send[0]);
		string type = string.Empty;
		int caridx = 0, partid = 0;
		if(string.Equals(_send[2], "coin")){
			type = "0";
		}else {
			type = "1";
		}
		
		int tempid = 0;
		int mon = 0;
		Global.isNetwork = true;
		if(!int.TryParse(_send[3], out mon))
			mon = int.Parse(_send[3].Replace(",",string.Empty));
		if(type.Equals("0")) {
			int a = GV.myCoin - mon;
			if(a < 0) isSucc = false;
			else {
				GV.myCoin = GV.myCoin - mon;
				GV.updateCoin = mon;isSucc = true;
			}
		}else{
			int b = GV.myDollar - mon;
			if(b < 0) isSucc = false;
			else {
				GV.myDollar = GV.myDollar - mon;
				GV.updateDollar = mon;
				isSucc = true;
			}
		}
	//	Utility.LogWarning(" level up " + GV.myCoin);	Utility.LogWarning(" level up " + mon);
	//	Utility.LogWarning(" level up " +isSucc);
		if(index < 1199){ //car
			upgradeKind = _send[1];
			if(isSucc){
				caridx = GV.getTeamCarIndex(GV.SelectedTeamID);
				StartCoroutine(UpgradeCarPart(_send[2],caridx ,ChangeServerCarPart(_send[1])));
			}else{
				Invoke("UpdradeCarResult", 0.5f);
			}
			
		}else{ //crew
			upgradeKind = _send[1];
			if(isSucc){
				StartCoroutine(UpgradeCrewPart(_send[2], GV.SelectedTeamID ,ChangeServerCrewPart(_send[1])));
			}else{
				Invoke("UpdradeCrewResult", 0.5f);
			}
			
		}
	}
	
	IEnumerator UpgradeCarPart(string pay, int carIdx, int part){
		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		bool bConnect = false;
		mDic.Add("carIdx",carIdx);
		mDic.Add("part",part);
		string mParam  = "payment;"+pay;
		string mAPI = ServerAPI.Get(90010);//.  "game/car/level"
		NetworkManager.instance.HttpFormConnect("Put", mDic,mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				isSucc = true;
			}else if(status == -200){
				isSucc = false;
			
			
			
			}else isSucc = false;
			bConnect = true;
		},mParam);
		while(!bConnect){
			yield return null;
		}
		
		UpdradeCarResult();
	}
	
	IEnumerator UpgradeCrewPart(string pay, int teamId, int crewId){
		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		bool bConnect = false;
		mDic.Add("teamId",teamId);
		mDic.Add("crewId",crewId);
		//string mParam  = "payment;"+pay;
		if(pay.Equals("coin")) mDic.Add("method", 1); //coin
		else mDic.Add("method", 0); //dollar
		
		string mAPI = ServerAPI.Get(90066);//."game/team/crew/level"
		NetworkManager.instance.HttpFormConnect("Put", mDic, mAPI , (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			//response:{"result":{"level":3},"state":0,"msg":"sucess","time":1449495859}
			if (status == 0)
			{
				isSucc = true;
			}else{
				isSucc = false;
			}
			bConnect = true;
		});
		while(!bConnect){
			yield return null;
		}
		
		UpdradeCrewResult();
	}
	
	void UpdradeCarResult(){
		Global.isNetwork = false;
		if(isSucc) upgradeCarLevelUp();
		ResponseBuy(isSucc);
	}
	
	void UpdradeCrewResult(){
		Global.isNetwork = false;
		if(isSucc)
			upgradeCrewLevelUp();
		ResponseBuy(isSucc);
	}
	
	void upgradeCrewLevelUp(){
		CrewInfo crew;
		crew = GV.getTeamCrewInfo(GV.SelectedTeamID);
		switch(upgradeKind){
		case "5100": // driver
			crew.driverLv++;
			break;
		case "5101": //tire
			crew.tireLv++;
			break;
		case "5102": //chief
			crew.chiefLv++;
			break;
		case "5103": //jack
			crew.jackLv++;
			break;
		case"5104": //gas
				crew.gasLv++;
			break;
		}
		GameObject.Find("LobbyUI").SendMessage("SuccessCrewEffect", upgradeKind,SendMessageOptions.DontRequireReceiver);

	}
	
	void upgradeCarLevelUp(){
		CarInfo car;int idx = 0;
		car = GV.getTeamCarInfo(GV.SelectedTeamID);
		switch(upgradeKind){
		case "5000": // body
			car.bodyLv++;
			break;
		case "5001": //engine
			car.engineLv++;
			break;
		case "5002": //tires
			car.tireLv++;
			break;
		case "5003": //gear
			car.gearBoxLv++;
			break;
		case"5004": //intake
				car.intakeLv++;
			break;
		case "5005": //n2 power
			car.bsPowerLv++;
			break;
		case "5006": //n2 time
			car.bsTimeLv++;
			break;
		}
		GameObject.Find("LobbyUI").SendMessage("SuccessCarEffect", upgradeKind,SendMessageOptions.DontRequireReceiver);

		//myAccount.instance.updateCarPart(car);
	}

	int ChangeServerCarPart(string kinds){
		int idx = 0;
		switch(kinds){
		case "5000": // body
			idx = 1;
			break;
		case "5001": //engine
			idx = 2;
			break;
		case "5002": //tires
			idx = 3;
			break;
		case "5003": //gear
			idx = 4;
			break;
		case"5004": //intake
				idx = 5;
			break;
		case "5005": //n2 power
			idx = 6;
			break;
		case "5006": //n2 time
			idx = 7;
			break;
		}
		return idx;
		//myAccount.instance.updateCarPart(car);
	}
	// 1: driver, 2: tireman, 3: crewChief, 4: jackman, 5: gasman
	int ChangeServerCrewPart(string kinds){
		int  strName  =0;// string.Empty;
		switch(kinds){
		case "5100": // driver
			//strName = "driverLevel";
			strName = 1;
			break;
		case "5101": //tire
			//strName = "tireManLevel";
			strName = 2;
			break;
		case "5102": //chief
			//strName = "chiefLevel";
			strName = 3;
			break;
		case "5103": //jack
			//strName = "jackManLevel";
			strName = 4;
			break;
		case"5104": //gas
				//strName = "gasManLevel";
			strName = 5;
				break;
		}
		return strName;
	}
	
	void ResponseBuy(bool isSuccess){
		var child = transform.FindChild("Content_BUY") as Transform;
		child.gameObject.SetActive(false);
		child.FindChild("btnCoin").gameObject.SetActive(false);
		string[] _name = gameObject.name.Split("_"[0]);
		GameObject _parent = null;
		
		if(isSuccess){
			GV.TeamChangeFlag = 5;
			OnCloseClick();
		
		}else{
			_parent = gameObject.transform.FindChild("Content_Fail").gameObject;
			gameObject.GetComponent<TweenAction>().doubleTweenScale(_parent);
			_parent.SetActive(true);
			if(string.Equals(_name[2], "Coin") == true) _name[2] = "icon_coin";
			else _name[2]  = "icon_dollar";
			_parent.transform.FindChild("icon_product").gameObject.SetActive(false);
			//_parent.transform.FindChild("icon_product").GetComponent<UISprite>().spriteName = _name[2];
			_parent.transform.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
			_parent.transform.FindChild("lbText").GetComponent<UILabel>().text =
				KoStorage.GetKorString("76022");//
			_parent.transform.FindChild("lbName").GetComponent<UILabel>().text =
				KoStorage.GetKorString("76023");//
			_parent.transform.FindChild("btnok").GetComponentInChildren<UILabel>().text = 
				KoStorage.GetKorString("71000");//
			if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
			UserDataManager.instance._subStatus = ()=>{
				Invoke("OnCloseClick",0.1f);
				return true;
			};
		}
		onRespons(isSuccess);
	}
	
	
	public void RegistorBack(){
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnCloseClick",0.1f);
			return true;
		};
	}
	
	
	void OnCloseClick(){
		//Global.isPopUp = false;
		onRespons(false);
		var child = transform.FindChild("Content_Fail") as Transform;
		child.gameObject.SetActive(false);
		child.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		//transform.FindChild("Content_Success").gameObject.SetActive(false);
		child = transform.FindChild("Content_BUY") as Transform;
		child.gameObject.SetActive(true);
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		Destroy(this);
		gameObject.SetActive(false);
	}
	
	void OnFailClick(){
		// 상점으로 이동
		GameObject.Find("LobbyUI").SendMessage("OnDollarClick", SendMessageOptions.DontRequireReceiver);
		OnCloseClick();
	}
	
	void OnSuccessClick(){
		OnCloseClick();
	}
}
