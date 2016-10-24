using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MyTeamMainMenu2 : MonoBehaviour {

	public Transform view,grid; //stock
	public GameObject teamSlotPrefabs, teamSlotPrefabs1;
	public GameObject stockSelect;
	public GameObject stockInfo;
	public GameObject stockGroup;
	public GameObject repairWin;
	void Start(){
	
		stockSelect.name = "Team_"+GV.SelectedTeamID.ToString();
		stockSelect.GetComponent<ViewMyTeamItemDetail>().myInfoContent(stockSelect.name);
		stockInfo.name = stockSelect.name;
		addStockItems();
		transform.GetComponent<TweenAction>().ForwardTween(stockInfo);
		var temp = stockInfo.GetComponent<ViewMyTeamInfo>() as ViewMyTeamInfo;
		if(temp == null) temp = stockInfo.AddComponent<ViewMyTeamInfo>();
		temp.InitInfoContent();
	}
	
	string CheckingTeamName(string name){
		string str = string.Empty;
	//	Utility.LogWarning(name);
		string[] str1 = name.Split('_');
		int idx  = int.Parse(str1[1]);
		if(idx%2 == 0){
			str = "TeamStock_"+str1[1];
		}else{
			str = "TeamTour_"+str1[1];
		}
		//Utility.LogWarning(str);
		return str;
	}

	void OnTeamCarChange(GameObject obj){
		string str = CheckingTeamName(obj.transform.parent.parent.name);
		GameObject.Find("LobbyUI").SendMessage("OnTeamCar",str,SendMessageOptions.DontRequireReceiver);
	}
	
	void OnTeamCarUp(GameObject obj){
		string str = CheckingTeamName(obj.transform.parent.parent.name);
		GameObject.Find("LobbyUI").SendMessage("OnTeamCarUp",str,SendMessageOptions.DontRequireReceiver);
	}
	
	void OnTeamCarRepair(GameObject obj){
		string str = CheckingTeamName(obj.transform.parent.name);
		repairWin.SetActive(true);
		GetComponent<TweenAction>().doubleTweenScale(repairWin);
		var _card = repairWin.GetComponent<RepairAction>();
		if(_card == null) _card = repairWin.AddComponent<RepairAction>();
		stockGroup.SetActive(false);
		string[] names = obj.transform.parent.name.Split('_');
		int temp = int.Parse(names[1]);
		_card.repairTeamCar(temp );
	}
	
	void OnTeamCrewUp(GameObject obj){
		string str = CheckingTeamName(obj.transform.parent.parent.name);
		GameObject.Find("LobbyUI").SendMessage("OnTeamCrewUp",str,SendMessageOptions.DontRequireReceiver);
	}
	
	void OnTeamSponsor(GameObject obj){
		string str = CheckingTeamName(obj.transform.parent.parent.name);
		string[] str1 = obj.transform.parent.parent.name.Split('_');
		int idx  = int.Parse(str1[1]);
		GV.SelectedSponTeamID = GV.SelectedTeamID;
		GV.SelectedTeamID = idx;
		if(GV.SelectedSponTeamID != idx)
			GameObject.Find("LobbyUI").SendMessage("LobbyChangeTeam", idx);
	

		GameObject.Find("LobbyUI").SendMessage("OnTeamSponsor",str,SendMessageOptions.DontRequireReceiver);
	}

	IEnumerator SetChangeTeam(int id, System.Action Callback){
		Global.isNetwork = true;
		Dictionary<string, int> mDic = new Dictionary<string,int>();
		mDic.Add("teamId",id); // team 선택 
		bool bConnect = false;
		string mAPI = ServerAPI.Get(90007);//"game/team/participant"
		NetworkManager.instance.HttpFormConnect("Put", mDic,mAPI  , (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
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
	
	
	IEnumerator SetBuyTeam(int id, System.Action Callback){
		Global.isNetwork = true;
		Dictionary<string, int> mDic = new Dictionary<string,int>();
		mDic.Add("teamId",id); // team 구매 
		bool bConnect = false;
		string mAPI = ServerAPI.Get(90006);//"game/team"
		NetworkManager.instance.HttpFormConnect("Post", mDic,mAPI  , (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
			}else {
				Utility.LogError("state " + status + " Msg : " + thing["msg"]);
			}
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
		string str = obj.transform.parent.parent.name;
		if(stockSelect.name.Equals(str)) return;
		string[] name = str.Split('_');
		int id = int.Parse(name[1]);int idx=0;
		Common_Team.Item item = Common_Team.Get(id);
		if(item.Model == 1)
		{
			idx = 0;}//GV.SelectedTeam = "Stock";}
		else {  
			idx = 1; }//GV.SelectedTeam = "Tour";}

		StartCoroutine(SetChangeTeam(id , ()=>{
			GV.SelectedTeamID = id;
			GV.SelectedSponTeamID = GV.SelectedTeamID;
		//	AccountManager.instance.SetSponTime();
			var temp = stockSelect.GetComponent<ViewMyTeamItemDetail>() as ViewMyTeamItemDetail;
			if(temp == null) temp = stockSelect.AddComponent<ViewMyTeamItemDetail>();
			temp.myInfoContent(str);
			stockSelect.name = str;
			var temp1 = stockInfo.GetComponent<ViewMyTeamInfo>() as ViewMyTeamInfo;
			temp1.unSetContent();
			transform.GetComponent<TweenAction>().doubleTweenScale(stockSelect);
			grid.GetComponent<ViewMyTeamSelectGrid>().SetSelectedObj(str);
			
			GameObject.Find("LobbyUI").SendMessage("LobbyChangeTeam", idx);
			GameObject.Find("LobbyUI").SendMessage("ChangeTeamTopInfo",SendMessageOptions.DontRequireReceiver);
			GameObject.Find("Audio").SendMessage("CompleteSound");
			AccountManager.instance.SetSponTime();

			int cnt = grid.childCount;
			for(int i =0 ; i < cnt; i++){
				int idx1 = 10+i;
				if(idx == GV.SelectedTeamID){
					grid.GetChild(i).GetComponent<ViewMyTeamItem>().CheckTeamSpon();
					break;
				}
			}

		}));

	}



	void OnBuyTeamGold(GameObject obj){
		string str = obj.transform.parent.name;
		//Utility.LogWarning("OnbuyTeamGold " + str);
		string[] strID = str.Split('_');
		int id = int.Parse(strID[1]);
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<TeamBuyPopup>().InitPopUp(()=>{
			StartCoroutine(SetBuyTeam(id, ()=>{
				int mPrice = Common_Team.Get(id).Buyprice;
				GV.myCoin = GV.myCoin - mPrice;
				GV.updateCoin = mPrice;
				GameObject.Find("LobbyUI").SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
				GV.AddBuyTeamInfo(id);
				var temp1 = stockInfo.GetComponent<ViewMyTeamInfo>() as ViewMyTeamInfo;
				temp1.ReSetContent();
				MyTeamTradeComplete(id);
				GameObject.Find("Audio").SendMessage("CompleteSound");
			}));
		}, id, 1);
	}



	void OnBuyTeamdollar(GameObject obj){
		string str = obj.transform.parent.name;
		//Utility.LogWarning("OnbuyTeamDollar " + str);
		string[] strID = str.Split('_');
		int id = int.Parse(strID[1]);
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<TeamBuyPopup>().InitPopUp(()=>{
			StartCoroutine(SetBuyTeam(id, ()=>{
				int mPrice = Common_Team.Get(id).Buyprice;
				GV.myDollar = GV.myDollar - mPrice;
				GV.updateDollar = mPrice;
				GameObject.Find("LobbyUI").SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
				GV.AddBuyTeamInfo(id);
				var temp1 = stockInfo.GetComponent<ViewMyTeamInfo>() as ViewMyTeamInfo;
				temp1.ReSetContent();
				MyTeamTradeComplete(id);
				GameObject.Find("Audio").SendMessage("CompleteSound");
			}));
		}, id, 2);
	}

	public void TeamCarSelectComplete(int id){
		int cnt = grid.childCount;int num = 10;
		for(int i =0 ; i < cnt; i++){
			int idx = num+i;
			if(id == idx){
				grid.GetChild(i).GetComponent<ViewMyTeamItem>().SetTeamCarComplete();
				break;
			}
		}
		var temp1 = stockInfo.GetComponent<ViewMyTeamInfo>() as ViewMyTeamInfo;
		temp1.TeamCarSelectComplete();

	}


	void MyTeamTradeComplete(int id){
		int cnt = grid.childCount;int num = 10;
		for(int i =0 ; i < cnt; i++){
			int idx = num+i;
			if(id == idx){
				grid.GetChild(i).GetComponent<ViewMyTeamItem>().SetTradeTeamComplete();
				break;
			}
		}
	}
	public void OnTeamCarSelect(GameObject obj){
		string str = obj.transform.parent.name;
		string[] name = str.Split('_');
		int id = int.Parse(name[1]);
		GameObject.Find("LobbyUI").SendMessage("OnTeamSelectCar",id,SendMessageOptions.DontRequireReceiver);
	}



	public void OnStockChange(string str){
	
	}

	public void OnItemChange(string str){
		if(stockInfo.activeSelf){
			stockInfo.SetActive(false);
		}
		transform.GetComponent<TweenAction>().ForwardTween(stockInfo);
		var temp = stockInfo.GetComponent<ViewMyTeamInfo>() as ViewMyTeamInfo;
		if(temp == null) temp = stockInfo.AddComponent<ViewMyTeamInfo>();
		stockInfo.name = str;
		bool b = (stockInfo.name.Equals(stockSelect.name))?true:false;

		temp.ChangeInfoWindowContent(str, b);
	}


	void addStockItems(){
		int cnt = Common_Team.StockTeamList.Count+Common_Team.TourTeamList.Count;
		cnt+=2;
		if(grid.childCount == 0 ){
			GameObject temp = null;
			for(int i =0 ; i < cnt; i++){
				int num = 10+i;
				temp =NGUITools.AddChild(grid.gameObject, teamSlotPrefabs);
				temp.name = "Team_"+num.ToString();
				temp.AddComponent<ViewMyTeamItem>().ViewTeamContent(num);
			}
		}else{
			
		}
		grid.GetComponent<UIGrid>().Reposition();
		
	}


	void SetDurabilityMain(){
		GV.TeamChangeFlag =0;
		//stockInfo.SendMessage("ReSetDurability");
		stockInfo.GetComponent<ViewMyTeamInfo>().ReSetDurability();
	}
	void OnEnable(){
		if(GV.TeamChangeFlag==0) {
		
		
		
		}else if(GV.TeamChangeFlag == 1){
			GV.TeamChangeFlag =0;
			stockSelect.name = "Team_"+GV.SelectedTeamID.ToString();
			var tempdetail = stockSelect.GetComponent<ViewMyTeamItemDetail>() as ViewMyTeamItemDetail;
			if(tempdetail == null) tempdetail = stockSelect.AddComponent<ViewMyTeamItemDetail>() as ViewMyTeamItemDetail;
			tempdetail.myInfoContent(stockSelect.name);
			stockInfo.name = stockSelect.name;
			var temp = stockInfo.GetComponent<ViewMyTeamInfo>() as ViewMyTeamInfo;
			temp.InitInfoContent();
		
			int cnt = grid.childCount;
			int num = 10;
			grid.GetComponent<ViewMyTeamSelectGrid>().SetSelectedObj(stockInfo.name);
			grid.GetComponent<ViewMyTeamSelectGrid>().unSetSelectedObj(stockInfo.name);
			for(int i =0 ; i < cnt; i++){
				int idx = num+i;
				if( idx == GV.SelectedTeamID){
					grid.GetChild(i).GetComponent<ViewMyTeamItem>().SelectTeamCarComplete();
				//	break;
				}else{
					//grid.GetChild(i).GetComponent<ViewMyTeamItem>().unSetSelectTeam();
					//grid.GetChild(i).GetComponent<ViewMyTeamItem>().unSetSelectTeam();
				}
			}

		}else if(GV.TeamChangeFlag == 2){
			GV.TeamChangeFlag =0;
			TeamCarSelectComplete(GV.SelTeamID);
		}else if(GV.TeamChangeFlag == 3){
			SetDurabilityMain();
		
		}else if(GV.TeamChangeFlag == 4){
			if(GV.SelectedTeamID == GV.SelectedSponTeamID){
				stockSelect.GetComponent<ViewMyTeamItemDetail>().ChangeMainSpon();
			}
			
			var temp1 = stockInfo.GetComponent<ViewMyTeamInfo>() as ViewMyTeamInfo;
			temp1.TeamSponSelectComplete();

			int cnt = grid.childCount;
			int num = 10;
			for(int i =0 ; i < cnt; i++){
				int idx = num+i;
				if(idx == GV.SelectedTeamID){
					grid.GetChild(i).GetComponent<ViewMyTeamItem>().SetTeamSpon();
					break;
				}
			}
			AccountManager.instance.SetSponTime();
			GV.TeamChangeFlag =0;
		}else if(GV.TeamChangeFlag == 5){
			GV.TeamChangeFlag =0;
			stockSelect.GetComponent<ViewMyTeamItemDetail>().ChangeTeamAbility();
			stockInfo.name = stockSelect.name;
			var temp = stockInfo.GetComponent<ViewMyTeamInfo>() as ViewMyTeamInfo;
			temp.ChangeGraph(GV.SelectedTeamID);
			
			int cnt = grid.childCount;
			int num = 10;
			for(int i =0 ; i < cnt; i++){
				int idx = num+i;
				if( idx == GV.SelectedTeamID){
					grid.GetChild(i).GetComponent<ViewMyTeamItem>().ChangeTeamAbility();
					break;
				}
			}
		
		}


		if(grid.childCount == 0) return;
		view.GetComponent<UIDraggablePanel>().ResetPosition();

	}

	/*
	void addTourItems(){
		string strTeam = "TeamTour_14";
		int cnt = Common_Team.TourTeamList.Count;
		if(grid1.childCount == 0 ){
			for(int i =0 ; i < cnt; i++){
				int num = int.Parse(Common_Team.TourTeamList[i]);
				var temp = NGUITools.AddChild(grid1.gameObject, teamSlotPrefabs1);
				temp.name = "TeamTour_"+num.ToString();
				temp.AddComponent<ViewMyTeamItem>().ViewTeamTourContent(num);
			}
		}else{
		}
		//grid1.gameObject.SendMessage("SelectBGActivate",strTeam);
		grid1.GetComponent<UIGrid>().Reposition();
	}
	*/
	void OnClose(){
	//	if(tourInfo.activeSelf) tourInfo.SetActive(false);
	//	tourInfo.name = "Default";
	//	if(stockInfo.activeSelf) stockInfo.SetActive(false);
	//	stockInfo.name = "Default";
	//	stockGroup.SetActive(true);
	//	grid.GetComponent<ViewMyTeamSelectGrid>().unSetObj();
	//	tourGroup.SetActive(true);
	//	grid1.GetComponent<ViewMyTeamSelectGrid>().unSetObj();
		stockGroup.SetActive(true);
		repairWin.SetActive(false);
	}


}


public partial class LobbyManager : MonoBehaviour {

	void OnTeamSelectCar(int teamID){
		if(btnstate != buttonState.WAIT) return;
		menuCenter.InitCenterMenu();
		fadeIn();
		//GV.SelectedTeamCode = 0;
		strTip = "Team";
		btnstate = buttonState.MYCAR;
		isLobby =false;
		//InitTableShop(GV.SelectedTeamCode);
		GV.SelTeamID = teamID;
		OnBackFunction = ()=>{
			HiddenInfoTipWindow();
			if(raceinfo != null) {
				HiddenInfoWindow();
			}
			if(status != null) hiddenStatusWindow(status);
			OnTeamClick();
			
		};
	}


	void OnTeamCar(string selectName){
		string[] name = selectName.Split('_');
		Utility.LogWarning("select name " + selectName);
		GV.SelTeamID = int.Parse(name[1]);
		if(name[0].Equals("TeamStock")){
			if(btnstate != buttonState.WAIT) return;
			menuCenter.InitCenterMenu();
			fadeIn();
		//	GV.SelectedTeamCode = 0;
			strTip = "Team";
			btnstate = buttonState.MYCAR;
			isLobby =false;
		//	InitTableShop(GV.SelectedTeamCode);
		}else{
			if(btnstate != buttonState.WAIT) return;
			menuCenter.InitCenterMenu();
		//	GV.SelectedTeamCode = 1;
			fadeIn();
			strTip = "Team";
			btnstate = buttonState.MYCAR;
			isLobby =false;
			//InitTableShop(GV.SelectedTeamCode);
		}
		OnBackFunction = ()=>{
			HiddenInfoTipWindow();
			if(raceinfo != null) 	HiddenInfoWindow();
			if(status != null) hiddenStatusWindow(status);
			OnTeamClick();
		};
	}
	void OnTeamCarUp(string selectName){
		string[] name = selectName.Split('_');
		if(name[0].Equals("TeamStock")){
			if(btnstate != buttonState.WAIT) return;
			fadeIn();
			btnstate = buttonState.TEAM_UP_CAR;
			//GV.SelectedTeamCode = 0;
			menuCenter.InitCenterUpMenu("Car");
			isLobby =false;
			//ElevatorCar.SetActive(true);
			ResetElevatorCar(true, selectName);
			string str = null;
			if(CrewUpNameStock == null) str = "Upgrade_CarTo" +"Driver";
			else str = "Upgrade_CarTo" +CrewUpNameStock;
			camAni.ReversePlayAnimationFast(str);
			//	SettingCarPartsLV();
		}else{
			if(btnstate != buttonState.WAIT) return;
			fadeIn();
			btnstate = buttonState.TEAM_UP_CAR;
			//GV.SelectedTeamCode = 1;
			menuCenter.InitCenterUpMenu("Car");
			isLobby =false;
			ResetElevatorCar(false, selectName);
			OnTeamUpCar();
			string str = null;
			if(CrewUpNameTour == null) str = "Upgrade_CarTo" +"Driver";
			else str = "Upgrade_CarTo" +CrewUpNameTour;
			//camAni_Tour.ReversePlayAnimationFast(str);
			camAni_Tour.ReversePlayAnimationFast(str);
			//SettingCarPartsLV();
		}
		OnBackFunction = ()=>{
			//	transform.GetComponent<ShopItemCreate>().SponsorStop();
			HiddenInfoTipWindow();
			if(status != null) {
				hiddenStatusWindow(status);
			}
			if(raceinfo != null) {
				HiddenInfoWindow();
			}
			OnTeamClick();
		};
	}

	
	
	void OnTeamCrewUp(string selectName){
		string[] name = selectName.Split('_');
		if(name[0].Equals("TeamStock")){
			if(btnstate != buttonState.WAIT) return;
			fadeIn();
			btnstate = buttonState.TEAM_UP_CREW;
			menuCenter.InitCenterUpMenu("Crew");
			isLobby =false;
			ResetElevatorCar(true, selectName);
			string str = null;
			if(CrewUpNameStock == null) str = "Upgrade_CarTo" +"Driver";
			else str = "Upgrade_CarTo" +CrewUpNameStock;
			camAni.PlayAnimationFast(str);
			//GV.SelectedTeamCode = 0;
			//SettingCrewPartsLV();
		}else{
			if(btnstate != buttonState.WAIT) return;
			fadeIn();
			btnstate = buttonState.TEAM_UP_CREW;
			menuCenter.InitCenterUpMenu("Crew");
			isLobby =false;
			ResetElevatorCar(false, selectName);
			string str = null;
			if(CrewUpNameTour == null) str = "Upgrade_CarTo" +"Driver";
			else str = "Upgrade_CarTo" +CrewUpNameTour;
			camAni_Tour.PlayAnimationFast(str);
			//GV.SelectedTeamCode = 1;
		}
		OnBackFunction = ()=>{
			//transform.GetComponent<ShopItemCreate>().SponsorStop();
			HiddenInfoTipWindow();
			if(status != null) {
				hiddenStatusWindow(status);
			}
			if(raceinfo != null) {
				HiddenInfoWindow();
			}
			OnTeamClick();
		};
	}
	void OnTeamSponsor(string selectName){
		string[] name = selectName.Split('_');

		if(name[0].Equals("TeamStock")){
			if(btnstate != buttonState.WAIT) return;
			fadeIn();//GV.SelectedTeamCode = 0;
			isLobby = false;
			btnstate = buttonState.Sponsor;
			RotateTable.SetActive(false);
			HiddenInfoTipWindow();
			
		}else{
			if(btnstate != buttonState.WAIT) return;
			fadeIn();//GV.SelectedTeamCode = 1;
			isLobby = false;
			btnstate = buttonState.Sponsor;
			RotateTable.SetActive(false);
			
		}
		OnBackFunction = ()=>{
			transform.GetComponent<ShopItemCreate>().SponsorStop();
			HiddenInfoTipWindow();
			if(status != null) {
				hiddenStatusWindow(status);
			}
			if(raceinfo != null) {
				HiddenInfoWindow();
			}
				OnTeamClick();
		};
	}


}

