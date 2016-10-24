using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InvenMain : InfoWindow {
	public GameObject[] detailInfo; //0 car 1 mat 2 cou
	public GameObject[] invenInfo; //0 car 1 mat 2 cou
	public GameObject resultSell, resultDisAssy;
	LuckyBoxAni junkAni;
	void OnDisAssemble(){
	}

	void Awake(){
		junkAni = GameObject.Find("ENV_LuckyBox").GetComponent<LuckyBoxAni>();
	}

	void OnSetInvenSubCar(int idx){

		var obj0 = invenInfo[0].gameObject as GameObject;
		obj0.SetActive(false);
		showSub(obj0);
		if(idx !=1) 
			obj0.SendMessage("ChangeCarSlotItem", idx);

		var obj2 = detailInfo[0].gameObject as GameObject;
		obj2.SetActive(false);
		string name = string.Empty;
		bool b=false;
		if(idx == 0) name = "sCar_"+GV.mineCarList[0].CarID.ToString()+"_"+GV.mineCarList[0].ClassID+GV.mineCarList[0].TeamID+"_0";
		else if(idx == 1) {
			if(GV.mineCarList.Count !=0)
				name = "tCar_";
			else {
				name = "tCar_0000_1";b=true;
			}
		}
		else{
			if(GV.mineCarList.Count !=0)
				name = "sCar_";//+GV.mySpecialCarList[0];
			else {
				name = "sCar_0000_1"; b = true;}
		}
		obj2.name = name;
		if(!b) 	{
			showSub(obj2);
			obj2.SendMessage("ChangeCarInfo");
		}
		GameObject.Find("LobbyUI").SendMessage("ChangeElevatorCar", name);
		//!!--Utility.Log("onsetInvensubcar");
		
	}


	void OnSetInvenSubMat(int idx){
		//0 car 1 crew 2 cube
		var obj1= detailInfo[1].gameObject as GameObject;
		for(int i = 0; i < 3; i++){
			var obj2=detailInfo[1].transform.GetChild(i).gameObject as GameObject;
			obj2.SetActive(false);
		}
		var obj3=detailInfo[1].transform.GetChild(idx).gameObject as GameObject;
		obj3.SetActive(true);
		if(obj1.activeSelf) obj1.SetActive(false);
		showSub(obj1);
	//!!--Utility.Log("OnSetInvenSubMat");
		string objName = string.Empty;
		if(idx == 0) objName = "Car_Mat_0_Deck_0";
		else if(idx == 1) objName ="Car_Mat_0_Deck_0";
		else objName = "Cube_0";
		obj1.name = objName;
		obj1.SendMessage("ChangeMatInfo");
		var obj0 = invenInfo[1].gameObject as GameObject;
		obj0.SetActive(false);
		showSub(obj0);
		obj0.SendMessage("ChangeMatSlotItem", idx);
	}


	public void ShowSubDetail(string objName, int idx){
		//!!--Utility.Log("obj " + objName);
		var obj1= detailInfo[idx].gameObject as GameObject;
		if(obj1.activeSelf) obj1.SetActive(false);
		showSub(obj1);
		obj1.name = objName;
		obj1.SendMessage("ChangeCarInfo");
	}

	public void ShowSubMatDetail(string objName, int idx){
		//!!--Utility.Log("obj " + objName);
		var obj1= detailInfo[1].gameObject as GameObject;
		obj1.SetActive(true);
		for(int i = 0; i < 3; i++){
			var obj2=detailInfo[1].transform.GetChild(i).gameObject as GameObject;
			obj2.SetActive(false);
		}
		if(obj1.activeSelf) obj1.SetActive(false);
		showSub(obj1);
		obj1.name = objName;
		obj1.SendMessage("ChangeMatInfo");
		//!!--Utility.Log("ShowSubMatDetail");
	}

	void OnInitInven(){
		int mCnt = GV.mineCarList.Count;
		if(mCnt >= 100){
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			pop.AddComponent<SponExpirePopup>().SetInvenCarNumber();
		}
		
		var obj2 = invenInfo[0].gameObject as GameObject;
		showSub(obj2);
		obj2.SendMessage("InitCarItems");
		var obj1 = detailInfo[0].gameObject as GameObject;
		if(obj1.activeSelf) obj1.SetActive(false);
		showSub(obj1);
		obj1.name = "sCar_"+GV.mineCarList[0].CarID.ToString()+"_"+GV.mineCarList[0].ClassID+"_"+GV.mineCarList[0].TeamID+"_0";
		obj1.SendMessage("ChangeCarInfo");
	}

	void OnInitInvenMenu(){
		OnInitInven();
	}
	void OnSetInvenMenu(int idx){
			for(int i = 0; i < 4; i++){
			var obj1=detailInfo[i].gameObject as GameObject;
			var obj2 = invenInfo[i].gameObject as GameObject;
			if(idx == i){
			
			}else{
				if(obj1.activeSelf)
					hiddenSub(obj1);
				if(obj2.activeSelf)
					hiddenSub(obj2);
			}
		}

		showSub(detailInfo[idx].gameObject);
		showSub(invenInfo[idx].gameObject);
		if(idx == 1){
			invenInfo[idx].GetComponent<InvenMat>().InitInvenItems(idx, ()=>{
				ShowSubMatDetail("Car_Mat_0_Deck_0", 0);
			});
		}else if(idx == 3){

			invenInfo[idx].GetComponent<InvenCoupon>().InitInvenItems(idx, ()=>{
				ShowSubDetail("Silver_SlotCoupon_0", 3);
			});
		}
		else if(idx == 2){
			invenInfo[idx].GetComponent<InvenQube>().InitInvenItems(idx, ()=>{
				ShowSubDetail("Qube_0", 2);
			});
		}
		else {
			showSub(detailInfo[idx].gameObject);
			showSub(invenInfo[idx].gameObject);
			invenInfo[idx].gameObject.SendMessage("InitInvenItems", idx);
		}
	}

	void OnDisable(){
		for(int i = 0; i < 4; i++){
			detailInfo[i].gameObject.SetActive(false);
			invenInfo[i].gameObject.SetActive(false);
		}
	}

	void OnCarSell(GameObject obj){
		string name = obj.transform.parent.name;
		//Utility.LogWarning("name ; " + name);
		string[] strName = name.Split('_');
		int carID = int.Parse(strName[1]);
		int nTeam = int.Parse(strName[4]);
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		hiddenSub(detailInfo[0].gameObject);
		hiddenSub(invenInfo[0].gameObject);
		int pMoney = Common_Car_Status.Get(carID).Sellprice;
		pMoney+= GV.mineCarList[nTeam].CarClassItem.Sellprice;

		pop.AddComponent<CarSellPopup>().InitPopUp(()=>{
			junkAni.PlaySellAni((bSucess)=>{
				if(bSucess){
					resultSell.name = obj.transform.parent.name;
					resultSell.gameObject.SetActive(true);
					transform.GetComponent<TweenAction>().doubleTweenScale(resultSell);
					resultSell.transform.FindChild("BTN").GetComponent<UIButtonMessage>().functionName = "OnClose";
					
					GV.myDollar = GV.myDollar + pMoney;
					GV.updateDollar = -pMoney;
					resultSell.transform.FindChild("lbText").GetComponent<UILabel>().text =
						KoStorage.GetKorString("75007");
					resultSell.transform.FindChild("lbQuantity").GetComponent<UILabel>().text =
						string.Format("{0:#,0}", pMoney);
					GameObject.Find("Audio").SendMessage("CompleteSound");
					if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
					UserDataManager.instance._subStatus = ()=>{
						resultSell.SendMessage("OnClose",SendMessageOptions.DontRequireReceiver);
						return true;
					};
					
				}else{
					showSub(detailInfo[0].gameObject);
					showSub(invenInfo[0].gameObject);
				}
				
			}, nTeam);
			}, ()=>{
				showSub(detailInfo[0].gameObject);
				showSub(invenInfo[0].gameObject);
		},Common_Car_Status.Get(carID), pMoney);
	}

	void OnCarJunk(GameObject obj){
		string name = obj.transform.parent.name;
		Utility.Log(string.Format("Juck Name {0} ", name));
		string[] strName = name.Split('_');
		int carID = int.Parse(strName[1]);
		int nTeam = int.Parse(strName[4]);
		string strClass = GV.mineCarList[nTeam].ClassID;
		hiddenSub(detailInfo[0].gameObject);
		hiddenSub(invenInfo[0].gameObject);
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<CarJunkPopup>().InitPopUp(()=>{
			junkAni.PlayJunkAni((bSuccess)=>{
				if(bSuccess){
					resultDisAssy.name = obj.transform.parent.name;
					resultDisAssy.gameObject.SetActive(true);
					transform.GetComponent<TweenAction>().doubleTweenScale(resultDisAssy);
					string strMaterials = junkAni.GetMaterialLists();
					resultDisAssy.SendMessage("OnResultDisAssy", strMaterials);
					GameObject.Find("Audio").SendMessage("CompleteSound");
					myAcc.instance.account.bLobbyBTN[2] = true;
					myAcc.instance.account.bInvenBTN[1] = true;
				}else{
					showSub(detailInfo[0].gameObject);
					showSub(invenInfo[0].gameObject);
				}
			}, nTeam);
		}, ()=>{
			showSub(detailInfo[0].gameObject);
			showSub(invenInfo[0].gameObject);

		},Common_Car_Status.Get(carID).Name);
	
	}

	public void DeleteCarSlot(string targetname, int type){

		int idx = 0;
		string[] tName = targetname.Split('_');
		if(tName[0].Equals("sCar")){
			idx = 0;
			GV.DeleteMyCarList(int.Parse(tName[4]));
		}else if(tName[0].Equals("tCar")){
			idx = 1;
		}else{
			idx = 2;
			GV.DeleteMyCarList(int.Parse(tName[4]));
		}
		var obj0 = invenInfo[0].gameObject as GameObject;
		obj0.SetActive(false);
		showSub(obj0);
		if(idx !=1)
			obj0.SendMessage("ChangeCarSlotItem", idx);
		
		var obj2 = detailInfo[0].gameObject as GameObject;
		obj2.SetActive(false);
		string name = string.Empty;
		bool b=false;
		if(idx == 0) name = "sCar_"+GV.mineCarList[0].CarID.ToString()+"_"+GV.mineCarList[0].ClassID+"_"+GV.mineCarList[0].TeamID+"_0";
		else if(idx == 1) {
		}else{
		}
		obj2.name = name;
		if(!b) 	{
			showSub(obj2);
			obj2.SendMessage("ChangeCarInfo");
		}
		if(type == 0)
		GameObject.Find("LobbyUI").SendMessage("ChangeElevatorCar", name);
		else GameObject.Find("LobbyUI").SendMessage("ChangeElevatorCar", name);
	}


	void OnResolve(GameObject obj){
		string str = obj.transform.parent.parent.name;
		string[] names = str.Split('_');
		int a = int.Parse(names[2]);
		int b = int.Parse(names[4]);
		int c = a+b;
		int matID = GV.listMyMat[c].MatID;
		
		string matName = Common_Material.Get(matID).Name;
		int matPrice = Common_Material.Get(matID).Mat_sell;
		string[] strs = new string[]{matName,matPrice.ToString()};
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<MatSellPopup>().InitPopUp(()=>{
			Global.isNetwork = true;
			System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string,int>();
			mDic.Add("materialId",matID);
			string mAPI =  "game/material";//ServerAPI.Get(90077);// "game/team/car"
			NetworkManager.instance.HttpFormConnect("Delete", mDic, mAPI , (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					resultSell.name = obj.transform.parent.name;
					resultSell.gameObject.SetActive(true);
					resultSell.SendMessage("OnSellResult", strs,SendMessageOptions.DontRequireReceiver);
					transform.GetComponent<TweenAction>().doubleTweenScale(resultSell);
					resultSell.transform.FindChild("BTN").GetComponent<UIButtonMessage>().functionName = "OnMatClose";
					GV.myDollar += matPrice;
					GV.updateDollar = -matPrice;
					GV.UpdateMatCount(GV.listMyMat[c].MatID, -1);
					GameObject.Find("LobbyUI").SendMessage("InitTopMenu",SendMessageOptions.DontRequireReceiver);
					GameObject.Find("Audio").SendMessage("CompleteSound");
				}else {
					Utility.LogError("state " + status + " Msg : " + thing["msg"]);
				}
				Global.isNetwork = false;
			});
		}, strs);
	}

	public void ResetMatSlot(){
		invenInfo[1].GetComponent<InvenMat>().ResetInvenItems(()=>{
			ShowSubMatDetail("Car_Mat_0_Deck_0", 0);
		});
	}



	void OnCarChangeTeam(GameObject obj){
		if(Global.isNetwork) return;
		string name = obj.transform.parent.name;
		string[] strName = name.Split('_');
		int carID = int.Parse(strName[1]);
		int myTeamID = 0;
		myTeamID = GV.SelectedTeamID;
		int[] carids = new int[2];
		carids[1] = int.Parse(strName[1]);
		carids[0] = GV.getTeamCarID(myTeamID);

		int index = int.Parse(strName[4]);
		CarInfo carinfo = GV.mineCarList[index];

		StartCoroutine(SetCarInTeam(myTeamID, carinfo.CarIndex, ()=>{
			CarInfo _carinfo = GV.mineCarList.Find((obj1)=>obj1.TeamID == myTeamID);
			if(_carinfo != null) _carinfo.TeamID = 0;
			carinfo.TeamID = myTeamID;
			myTeamInfo myteam = GV.getTeamTeamInfo(myTeamID);
			myteam.TeamCarID = carids[1];
			myteam.setTeamCarInfo(carinfo);
			GameObject.Find("LobbyUI").SendMessage("ChangeSelectedTeamCarOnInven",SendMessageOptions.DontRequireReceiver);
			GameObject.Find("LobbyUI").SendMessage("SelectedTeamLVChange");
			GV.TeamChangeFlag = 1;
			GameObject.Find("Audio").SendMessage("CompleteSound");
			var obj0 = invenInfo[0].gameObject as GameObject;
			obj0.SetActive(false);
			showSub(obj0);
			obj0.SendMessage("ChangeTeamCarSlotItem", 0, SendMessageOptions.DontRequireReceiver);


			var obj2 = detailInfo[0].gameObject as GameObject;
			obj2.SetActive(false);
			obj.transform.parent.GetComponent<InvenCarInfo>().ChangeCarSubInfo();
			string names = string.Empty;
			bool b=false;
			names = "sCar_"+GV.mineCarList[0].CarID.ToString()+"_"+GV.mineCarList[0].ClassID+"_"+GV.mineCarList[0].TeamID+"_0";
			obj2.name = names;
			showSub(obj2);
		}));
	}

	IEnumerator SetCarInTeam(int teamId, int carIdx, System.Action Callback){
		Global.isNetwork = true;
		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string,int>();
		mDic.Add("teamId",teamId); // 팀 차량 지정
		mDic.Add("carIdx",carIdx);
		bool bConnect = false;
		string mAPI = ServerAPI.Get(90008);// "game/team/car"
		NetworkManager.instance.HttpFormConnect("Put", mDic, mAPI , (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				
			}else {
				Utility.LogError("state " + status + " Msg : " + thing["msg"]);}
			Global.isNetwork = false;
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
		if(Callback != null)
			Callback();
	}

	void OnTeamChange(GameObject obj){
		if(Global.isNetwork) return;
		Global.isNetwork= true;
		string str = obj.transform.parent.name;
		string[] name = str.Split('_');
		int id = int.Parse(name[3]);
		StartCoroutine(SetChangeTeam(id , ()=>{
			GV.SelectedTeamID = id;
			GV.SelectedSponTeamID = GV.SelectedTeamID;
			GV.TeamChangeFlag = 1;
			GameObject.Find("LobbyUI").SendMessage("LobbyChangeTeam", 0, SendMessageOptions.DontRequireReceiver);
			GameObject.Find("LobbyUI").SendMessage("ChangeTeamTopInfo",SendMessageOptions.DontRequireReceiver);
			GameObject.Find("Audio").SendMessage("CompleteSound");
			AccountManager.instance.SetSponTime();
			var obj0 = invenInfo[0].gameObject as GameObject;
			obj0.SetActive(false);
			showSub(obj0);
			obj0.SendMessage("ChangeTeamCarSlotItem", 0, SendMessageOptions.DontRequireReceiver);
			var obj2 = detailInfo[0].gameObject as GameObject;
			obj2.SetActive(false);
			obj.transform.parent.GetComponent<InvenCarInfo>().ChangeCarSubInfo2();
			string names = string.Empty;
			bool b=false;
			names = "sCar_"+GV.mineCarList[0].CarID.ToString()+"_"+GV.mineCarList[0].ClassID+"_"+GV.mineCarList[0].TeamID+"_0";
			obj2.name = names;
			showSub(obj2);

		}));
		
	}

	IEnumerator SetChangeTeam(int id, System.Action Callback){
		Global.isNetwork = true;
		Dictionary<string, int> mDic = new Dictionary<string,int>();
		mDic.Add("teamId",id); // team 선택 
		bool bConnect = false;
		string mAPI = ServerAPI.Get(90007);//"game/team/participant"
		NetworkManager.instance.HttpFormConnect("Put", mDic,mAPI  , (request)=>{
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
			}else {Utility.LogError("state " + status + " Msg : " + thing["msg"]);}
			Global.isNetwork = false;
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
		if(Callback != null)
			Callback();
	}


}
