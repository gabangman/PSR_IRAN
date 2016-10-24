using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
public partial class LobbyManager : MonoBehaviour {
	/*
	void InitMap(){
		if(mappanel == null) 
			mappanel = worldmap.transform.FindChild("WorldMap").gameObject; //view
		if(mapgrid == null){ 
			mapgrid = mappanel.transform.FindChild("Grid").gameObject;
		}
		if(_mapaction == null){
			_mapaction = mapgrid.AddComponent<MapRaceInit>();
			_mapaction.SetParentObject = MenuBottom;
		}
	}
*/
	void CreateGridPanelItem(GameObject _grid){
		Utility.LogWarning("CreateGridPanelItem - no");
		/*
		if( isCreateCarItem ) {
				for(int i = 0; i < _grid.transform.childCount; i++){
					var child = _grid.transform.GetChild(i).GetChild(0) as Transform;
					child.GetComponent<myteambtnaction>().mySelectBtn(true);
				}
			return;
		}
		//Utility.Log("GridCarPanel");
		isCreateCarItem = true;
		int count = 0;
		count = _grid.transform.childCount;
		List<Account.CarInfo> _list = myAccount.instance.account.listCarInfo;
		int listcnt = _list.Count;
		if(count != 0){
			if(listcnt > 6) {
				if(count != listcnt){
				//	if(_map == null) _map = gameObject.AddComponent<Mapaction>();
					var pop = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
					string str = "Slot_Car";
					for(int i = listcnt; i < listcnt+1; i++){
						pop.CreatePrefabsItems(str, _grid) ;
					}
					count++;
					UIGrid uiGrid = _grid.GetComponent<UIGrid>();
					uiGrid.repositionNow=true;
					Destroy(pop); pop = null;
					count = listcnt;
				}
			}
		}else{
			if(listcnt < 6) {
				count = 6;
			}else{
				count = listcnt;
			}
			//if(_map == null) _map = gameObject.AddComponent<Mapaction>();
			var pop = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
			string str = "Slot_Car";
			for(int i = 0; i < count; i++){
				pop.CreatePrefabsItems(str, _grid) ;
			}
			UIGrid uiGrid = _grid.GetComponent<UIGrid>();
			uiGrid.repositionNow=true;
			Destroy(pop); pop = null;
		}

		GameObject[] CarItem = new GameObject[count];
		//int tempid = Base64Manager.instance.GlobalEncoding(Global.MyCarID);
		int tempid = GV.getTeamCarID(GV.SelectedTeamID);
		string carName = tempid.ToString();
		myteambtnaction teambtn = null;

		for(int i = 0; i < count; i++){
			CarItem[i] = _grid.transform.GetChild(i).GetChild(0).gameObject;
			teambtn = CarItem[i].GetComponent<myteambtnaction>() as myteambtnaction;
			if(teambtn == null) teambtn = CarItem[i].AddComponent<myteambtnaction>();
			if(i < listcnt){
				CarItem[i].name = _list[i].carId.ToString();
				CarItem[i].transform.parent.name = _list[i].carId.ToString();
				if(CarItem[i].name == carName) {
					CarItem[i].transform.FindChild("Select").gameObject.SetActive(true);
					CarItem[i].transform.FindChild("Image_selected").gameObject.SetActive(true);
				}
				else {
					CarItem[i].transform.FindChild("Select").gameObject.SetActive(false);
					CarItem[i].transform.FindChild("Image_selected").gameObject.SetActive(false);
				}
				CarItem[i].name = _list[i].carId.ToString();
				teambtn.CarbuttonSetting();
				teambtn.CarButtonEnable();
				teambtn.CarButtonNew();
			}else{
				CarItem[i].name = "0000";
				CarItem[i].transform.parent.name = "0000";
				teambtn.CarButtonDisable();
			}
		}
		InitButton("CarMenu");
		CarItem = null;
		//Resources.UnloadUnusedAssets(); */
	
	}

	void CreateTeamCars(GameObject _grid, string teamName){
		int count = _grid.transform.childCount;
		int myCarCount = GV.mineCarList.Count;
	//	int mysCarCount = GV.mineSCarList.Count;
		int myTotalCount = myCarCount;//+mysCarCount;
		int childcnt = myTotalCount < 6? 6:myTotalCount;
		if(count == 0){
			var pop = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
			string str = "Slot_Car";
			for(int i = 0; i < childcnt; i++){
				pop.CreatePrefabsItems(str, _grid) ;
				count++;
			}
			_grid.GetComponent<UIGrid>().repositionNow=true;
			Destroy(pop); pop = null;
		}else{
			if(childcnt <= count){
				for(int i = 0; i < count; i++){
					var child = _grid.transform.GetChild(i).GetChild(0) as Transform;
					child.GetComponent<CarSlotAction>().unSetSelectLine();
				}
			}else{
				var pop = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
				string str = "Slot_Car";
				for(int i = count; i < childcnt; i++){
					pop.CreatePrefabsItems(str, _grid) ;
					count++;
				}
				_grid.GetComponent<UIGrid>().repositionNow=true;
				Destroy(pop); pop = null;
			}
		}
		int TeamId = 0;
		if(GV.SelectedTeamID == GV.SelTeamID){
			TeamId = GV.SelectedTeamID;
		}else{
			TeamId = GV.SelTeamID;
		}
		int carid = 0, crewid = 0;
		int[] carids;
		int reqlv = 0;
		carid= GV.getTeamCarID(TeamId);
		crewid = GV.getTeamCrewID(TeamId);
	//	reqlv = Common_Team.Get(TeamId).ReqLV;
		reqlv = GV.ChSeason;
		carids = new int[myTotalCount];
		string[] _classs = new string[myTotalCount];
		int[] carindex = new int[myTotalCount];
		int[] teamid = new int[myTotalCount];
		string _class = GV.getTeamCarClass(GV.SelectedTeamID);
		int mCount = 0;
		for(int i =0; i < myCarCount;i++){
			int mCarID = GV.mineCarList[i].CarID;
			Common_Car_Status.Item item = Common_Car_Status.Get(mCarID);
			if(item.ReqLV > reqlv){
			
			}else{
				//if(GV.mineCarList[i].TeamID == 0 || GV.mineCarList[i].TeamID == GV.SelTeamID){
					carids[mCount] =  mCarID;
					_classs[mCount] = GV.mineCarList[i].ClassID;//[1];
					carindex[mCount] = i;
					teamid[mCount] = GV.mineCarList[i].TeamID;
					mCount++;
				//}
			}

		}
	//	for( int i = myCarCount; i < (myTotalCount) ; i ++){
		//	carids[i] =  GV.mineSCarList[i].CarID;
		//	_classs[i] = GV.mineSCarList[i].ClassID;//[1];
	//	}

		CarSlotAction teambtn = null;
		if(carid == 0) {
			string tmpID = carids[0].ToString()+"_"+_classs[0];
			_table.OnTeam(tmpID,crewid.ToString());
			for(int i = 0; i < count; i++){
				var item= _grid.transform.GetChild(i).GetChild(0).gameObject as GameObject;
				teambtn = item.GetComponent<CarSlotAction>() as CarSlotAction;
				if(teambtn == null) teambtn =item.AddComponent<CarSlotAction>();
				if(i < (myTotalCount)){
					item.name = carids[i].ToString() +"_" + _classs[i]+"_"+carindex[i];
					item.transform.parent.name = carids[i].ToString() +"_" + _classs[i]+"_"+carindex[i];
					if(i==0) {
						item.transform.FindChild("Select").gameObject.SetActive(true);
					//	item.transform.FindChild("Image_selected_Me").gameObject.SetActive(false);
					}else {
						item.transform.FindChild("Select").gameObject.SetActive(false);
					//	item.transform.FindChild("Image_selected_Me").gameObject.SetActive(false);
					}

					if(teamid[i] == 0){
						item.transform.FindChild("Image_selected").gameObject.SetActive(false);
						item.transform.FindChild("Image_selected_other").gameObject.SetActive(false);
					}else{
						if(teamid[i] == TeamId){
							item.transform.FindChild("Image_selected").gameObject.SetActive(true);
							item.transform.FindChild("Image_selected_other").gameObject.SetActive(false);
						}
						else{
							item.transform.FindChild("Image_selected").gameObject.SetActive(false);
							item.transform.FindChild("Image_selected_other").gameObject.SetActive(true);
						}
					}

				}else{
					item.name = "0000_D";
					item.transform.parent.name = "0000_D";
				}
				teambtn.SlotSetting();
			}

			//Invoke("SlotReposition", 1.5f);


			return;
		}
		string carName = carid.ToString();
		string carIDs = carName+"_"+ _class;
		_table.OnTeam(carIDs,crewid.ToString());
	//	int center = 0;
		for(int i = 0; i < count; i++){
			var item= _grid.transform.GetChild(i).GetChild(0).gameObject as GameObject;
			teambtn = item.GetComponent<CarSlotAction>() as CarSlotAction;
			if(teambtn == null) teambtn =item.AddComponent<CarSlotAction>();
			if(i < (myTotalCount)){
				item.name = carids[i].ToString() +"_" + _classs[i]+"_"+carindex[i];
				item.transform.parent.name = carids[i].ToString() +"_" + _classs[i]+"_"+carindex[i];
			//	if(teamid[i] ==TeamId) {
			//		item.transform.FindChild("Select").gameObject.SetActive(true);
				//	item.transform.FindChild("Image_selected").gameObject.SetActive(true);
				//	GV.CarCount = i;
			//	}else {
			//		item.transform.FindChild("Select").gameObject.SetActive(false);
				//	item.transform.FindChild("Image_selected").gameObject.SetActive(false);
			//	}

				if(teamid[i] == 0){
					item.transform.FindChild("Image_selected").gameObject.SetActive(false);
					item.transform.FindChild("Image_selected_other").gameObject.SetActive(false);
					item.transform.FindChild("Select").gameObject.SetActive(false);
				}else{
					if(teamid[i] == TeamId){
						item.transform.FindChild("Image_selected").gameObject.SetActive(true);
						item.transform.FindChild("Image_selected_other").gameObject.SetActive(false);
						item.transform.FindChild("Select").gameObject.SetActive(true);
					}
					else{
						item.transform.FindChild("Image_selected").gameObject.SetActive(false);
						item.transform.FindChild("Image_selected_other").gameObject.SetActive(true);
						item.transform.FindChild("Select").gameObject.SetActive(false);
					}
				}
				
			}else{
				item.name = "0000_D";
				item.transform.parent.name = "0000_D";
			}
			teambtn.SlotSetting();
		}
		SetTeamCar(teamName, carid);


		//Invoke("SlotReposition", 1.5f);
	//	var v = _grid.transform.GetComponent<TeamCarOnCenter>() as TeamCarOnCenter;
	//	if(v == null) v = _grid.AddComponent<TeamCarOnCenter>();
	//	v.RecenterOnCar();

	}
	void SlotReposition(){
		var v = activeObject.transform.GetChild(0).GetChild(0).GetComponent<TeamCarOnCenter>()
			as TeamCarOnCenter;
		if(v == null) v = activeObject.transform.GetChild(0).GetChild(0).gameObject.AddComponent<TeamCarOnCenter>();
	//	v.RecenterOnCar();
	}

	void SetTeamCar(string teamName, int carid){
		Utility.LogWarning("Delete SetTeamCar");
		return;
		/*
		var obj = MenuBottom.transform.FindChild("Menu_MyCar_Grid") as Transform;
		var gridParent = obj.FindChild("View") as Transform;
		var grid = gridParent.FindChild("Grid") as Transform;

		string carName = carid.ToString();
		for(int i = 0; i < grid.childCount; i++){
			var temp = grid.GetChild(i).GetChild(0).gameObject as GameObject;
			var select = temp.transform.FindChild("Select").gameObject;
			if(temp.name.Equals(carName)){
				select.SetActive(true);
				if(ElevatorCar.name != carName){
					//string strtemp = temp.name;
					//int _carid = int.Parse(strtemp);
					//CreateElevatroCar(carid);
				}
			}else {
				select.SetActive(false);
			}
		}*/
	}


	void CreateGirdPanelCrewItem(GameObject _grid){
		Utility.LogWarning("Delete SetTeamCar");
		return;
	/*	if( isCreateCrewItem ) {
			for(int i = 0; i < _grid.transform.childCount; i++){
				var child = _grid.transform.GetChild(i).GetChild(0) as Transform;
				child.GetComponent<myteambtnaction>().mySelectBtn(false);
			}
			return;
		}
		isCreateCrewItem = true;
		int count = 0;
		count = _grid.transform.childCount;
		List<Account.CrewInfo> _list = myAccount.instance.account.listCrewInfo;//listCrewInfo;
		int listcnt = _list.Count;
		if(count != 0){
			if(listcnt > 6) {
				if(count != listcnt){
					if(_map == null) _map = gameObject.AddComponent<Mapaction>();
					string str = "Slot_Crew";
					for(int i = listcnt; i < listcnt+1; i++){
						_map.CreatePrefabsItems(str, _grid) ;
					}
					count++;
					UIGrid uiGrid = _grid.GetComponent<UIGrid>();
					uiGrid.repositionNow=true;
					Destroy(_map); _map = null;
					count = listcnt;
				}
			}
		}else{
			if(listcnt < 6) {
				count = 6;
			}else{
				count = listcnt;
			}
			if(_map == null) _map = gameObject.AddComponent<Mapaction>();
			string str = "Slot_Crew";
			for(int i = 0; i < count; i++){
				_map.CreatePrefabsItems(str, _grid) ;
			}
			UIGrid uiGrid = _grid.GetComponent<UIGrid>();
			uiGrid.repositionNow=true;
			Destroy(_map); _map = null;
		}
		
		GameObject[] CrewItem = new GameObject[count];
		string crewName = Base64Manager.instance.GlobalEncoding(Global.MyCrewID).ToString();
		myteambtnaction teambtn = null;
		for(int i = 0; i < count; i++){
			CrewItem[i] = _grid.transform.GetChild(i).GetChild(0).gameObject;
			teambtn = CrewItem[i].GetComponent<myteambtnaction>() as myteambtnaction;
			if(teambtn == null) teambtn = CrewItem[i].AddComponent<myteambtnaction>();
			if(i < listcnt){
				CrewItem[i].name = _list[i].crewId.ToString();
				CrewItem[i].transform.parent.name =  _list[i].crewId.ToString();
				if(CrewItem[i].name == crewName) {
					CrewItem[i].transform.FindChild("Select").gameObject.SetActive(true);
					CrewItem[i].transform.FindChild("Image_selected").gameObject.SetActive(true);
				}else {
					CrewItem[i].transform.FindChild("Select").gameObject.SetActive(false);
					CrewItem[i].transform.FindChild("Image_selected").gameObject.SetActive(false);
				}
				CrewItem[i].transform.FindChild("Empty").gameObject.SetActive(false);
				var t = CrewItem[i].transform.FindChild("Image").gameObject;//.SetActive(true);
				t.SetActive(true);
				t.GetComponent<UISprite>().spriteName = CrewItem[i].name;
				teambtn.CrewButtonSetting();
				teambtn.CrewButtonNew();
			}else{
				CrewItem[i].transform.FindChild("Empty").gameObject.SetActive(true);
				CrewItem[i].transform.FindChild("Select").gameObject.SetActive(false);
				var t = CrewItem[i].transform.FindChild("Image").gameObject;//.SetActive(true);
				CrewItem[i].name = "0000";
				CrewItem[i].transform.parent.name = "0000";
				t.SetActive(false);
				CrewItem[i].transform.FindChild("ClassPanel").gameObject.SetActive(false);
				CrewItem[i].transform.FindChild("lbClass").gameObject.SetActive(false);
				CrewItem[i].transform.FindChild("ClassColor").gameObject.SetActive(false);
			}
		}
		CrewItem = null;*/
	}

}
