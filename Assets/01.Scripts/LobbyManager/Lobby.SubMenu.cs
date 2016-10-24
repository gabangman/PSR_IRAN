using UnityEngine;
using System.Collections;

public partial class LobbyManager : MonoBehaviour {
	
	void OnUpgradeClick(){
		if(btnstate != buttonState.WAIT) return;
		fadeIn();
		menuCenter.InitCenterUpMenu("Car");
		btnstate = buttonState.UPGRADE_CAR;
		isLobby =false;
		InitButton("Upgrade");
		ElevatorCar.SetActive(true);
	}

	public void OnTeamUpCar(){
		if(camAni.aniPlaying) return;
		if(menuCenter.OnUpCarClick()) return;
		btnstate = buttonState.UPGRADE_CAR;
		finishPanel();
		if(GV.SelectedTeamID%2==0){
			if(CrewUpNameStock == null)  CrewUpNameStock = "Driver";
			sb.Length = 0;
			sb.Append("Upgrade_CarTo");
			sb.Append(CrewUpNameStock);
			camAni.ReversePlayAnimation(sb.ToString());
		}
		else {
			if(CrewUpNameTour == null)  CrewUpNameTour = "Driver";
			sb.Length = 0;
			sb.Append("Upgrade_CarTo");
			sb.Append(CrewUpNameTour);
			camAni_Tour.ReversePlayAnimation(sb.ToString());
		}
	}
	
	public void OnTeamUpCrew(){
		if(camAni.aniPlaying) return;
		if(menuCenter.OnUpCrewClick()) return;
		btnstate = buttonState.UPGRADE_CREW;
		finishPanel();
		if(GV.SelectedTeamID%2==0){
			if(CrewUpNameStock == null) CrewUpNameStock =  "Driver";
			//string str = "Upgrade_CarTo" +CrewUpNameStock;
			sb.Length = 0;
			sb.Append("Upgrade_CarTo");
			sb.Append(CrewUpNameStock);
			//sb.ToString();
			camAni.PlayAnimation(sb.ToString());
		}else{
			if(CrewUpNameTour == null) CrewUpNameTour =  "Driver";
			sb.Length = 0;
			sb.Append("Upgrade_CarTo");
			sb.Append(CrewUpNameTour);
			camAni_Tour.PlayAnimation(sb.ToString());
		}
	}

	public void OnUpgardCarClick(GameObject obj){
		if(GV.SelectedTeamID%2==0){
			StockUpgradeCar(obj);
		}else{
			TourUpgradeCar(obj);
		}
	}

	void StockUpgradeCar(GameObject obj){
		var game = obj.transform.parent.gameObject;
		if(CarUpNameStock == game.name) return;
		var child = game.transform.FindChild("upgrade_icon") as Transform;
		if(child.GetComponent<UISprite>().color == Color.black) return;
		for(int i = 0; i < activeObject.transform.childCount; i++){
			var temp = activeObject.transform.GetChild(i).gameObject;
			var temp1 = temp.transform.FindChild("Select").gameObject;
			temp1.SetActive(false);
		}
		game.transform.FindChild("Select").gameObject.SetActive(true);
		CarUpNameStock = game.name;
		if(raceinfo != null){
			InfoWindowDisable();
			var _card = raceinfo.GetComponent<EvoInit>() as EvoInit;
			_card.Hidden();
			raceinfo = null;
		}
		status.SendMessage("SelectedCarPartStatusChange",CarUpNameStock,SendMessageOptions.DontRequireReceiver);
		var _prefab = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
		raceinfo = _prefab.makeCarStatus(MenuBottom.transform,false);
		var temp2 = raceinfo.GetComponent<EvoInit>() as EvoInit;
		if(temp2 == null) temp2 = raceinfo.AddComponent<EvoInit>();
		temp2.Show(obj);
		temp2.onFinish = delegate {
			raceinfo.GetComponent<EvoInit>().tempHidden();
			var tw = raceinfo.GetComponent<TweenAction>();
			if(tw == null) tw = raceinfo.AddComponent<TweenAction>();
			tw.tempHidden();
			//raceinfo.GetComponent<UpgradeCarInfo>().UpdateLevel();
			raceinfo.GetComponent<LevelUpAction>().UpdateCarUpgarde();
			tw = status.GetComponent<TweenAction>();
			if(tw == null) tw = status.AddComponent<TweenAction>();
			tw.tempHidden();
			tw = MenuBottom.GetComponent<TweenAction>();
			if(tw == null) tw = MenuBottom.AddComponent<TweenAction>();
			tw.tempHidden();
			status.SendMessage("SelectedCarPartStatusChange",CarUpNameStock,SendMessageOptions.DontRequireReceiver);
			//Utility.Log ("car btn init");
			
		};/*;*/
		//raceinfo.SendMessage("UpgradeInit", CarUpNameStock,SendMessageOptions.DontRequireReceiver);
		raceinfo.SendMessage("InitUpgradeCarInfo", CarUpNameStock,SendMessageOptions.DontRequireReceiver);
		DestroyImmediate(_prefab);
		game = null;
	}


	void TourUpgradeCar(GameObject obj){
		var game = obj.transform.parent.gameObject;
		if(CarUpNameTour == game.name) return;
		var child = game.transform.FindChild("upgrade_icon") as Transform;
		if(child.GetComponent<UISprite>().color == Color.black) return;
		for(int i = 0; i < activeObject.transform.childCount; i++){
			var temp = activeObject.transform.GetChild(i).gameObject;
			var temp1 = temp.transform.FindChild("Select").gameObject;
			temp1.SetActive(false);
		}
		game.transform.FindChild("Select").gameObject.SetActive(true);
		CarUpNameTour = game.name;
		if(raceinfo != null){
			InfoWindowDisable();
			var _card = raceinfo.GetComponent<EvoInit>() as EvoInit;
			_card.Hidden();
			raceinfo = null;
		}
		status.SendMessage("SelectedCarPartStatusChange",CarUpNameTour,SendMessageOptions.DontRequireReceiver);
		var _prefab = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
		raceinfo = _prefab.makeCarStatus(MenuBottom.transform,false);
		var temp2 = raceinfo.GetComponent<EvoInit>() as EvoInit;
		if(temp2 == null) temp2 = raceinfo.AddComponent<EvoInit>();
		temp2.Show(obj);
		temp2.onFinish = delegate {
			raceinfo.GetComponent<EvoInit>().tempHidden();
			var tw = raceinfo.GetComponent<TweenAction>();
			if(tw == null) tw = raceinfo.AddComponent<TweenAction>();
			tw.tempHidden();
			//raceinfo.GetComponent<UpgradeCarInfo>().UpdateLevel();
			raceinfo.GetComponent<LevelUpAction>().UpdateCarUpgarde();
			tw = status.GetComponent<TweenAction>();
			if(tw == null) tw = status.AddComponent<TweenAction>();
			tw.tempHidden();
			tw = MenuBottom.GetComponent<TweenAction>();
			if(tw == null) tw = MenuBottom.AddComponent<TweenAction>();
			tw.tempHidden();
			status.SendMessage("SelectedCarPartStatusChange",CarUpNameTour,SendMessageOptions.DontRequireReceiver);
			//Utility.Log ("car btn init");
			
		};/*;*/
		//raceinfo.SendMessage("UpgradeInit", CarUpNameTour,SendMessageOptions.DontRequireReceiver);
		raceinfo.SendMessage("InitUpgradeCarInfo", CarUpNameTour,SendMessageOptions.DontRequireReceiver);
		game = null;
		DestroyImmediate(_prefab);
	}


	void StockUpgradeCrew(GameObject obj){
		if(CrewUpNameStock.Equals(obj.name)) return;
		for(int i = 0; i < activeObject.transform.childCount; i++){
			var temp = activeObject.transform.GetChild(i).gameObject;
			var temp1 = temp.transform.FindChild("Select").gameObject;
			temp1.SetActive(false);
		}
		
		string strTemp = null;
		if(CrewUpNameStock != null){
			strTemp = CrewUpNameStock;
		}else strTemp = "Driver";
		obj.transform.FindChild("Select").gameObject.SetActive(true);
		CrewUpNameStock = obj.name;
		if(strTemp != CrewUpNameStock){
			camAni.PlayAnimation(strTemp, CrewUpNameStock);
		}else return;
		if(raceinfo != null){
			InfoWindowDisable();
			var _card = raceinfo.GetComponent<EvoInit>() as EvoInit;
			_card.Hidden();/* crew build up*/
			raceinfo = null;
		}
		
		status.SendMessage("SelectedCrewStatusChange",CrewUpNameStock,SendMessageOptions.DontRequireReceiver);
		var _prefab = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
		raceinfo = _prefab.makeCrewStatus(MenuBottom.transform,false);
		/*
		 *  crew buuild up
		 */
		var temp2 = raceinfo.GetComponent<EvoInit>() as EvoInit;
		if(temp2 == null) temp2 = raceinfo.AddComponent<EvoInit>();
		temp2.Show(obj);
		temp2.onFinish = delegate {
			raceinfo.GetComponent<EvoInit>().tempHidden();
			var tw = raceinfo.GetComponent<TweenAction>();
			if(tw == null) tw = raceinfo.AddComponent<TweenAction>();
			tw.tempHidden();
			//raceinfo.GetComponent<UpgradeCrewInfo>().UpdateLevel();
			raceinfo.GetComponent<LevelUpAction>().UpdateCrewUpgrade();
			tw = status.GetComponent<TweenAction>();
			if(tw == null) tw = status.AddComponent<TweenAction>();
			tw.tempHidden();
			tw = MenuBottom.GetComponent<TweenAction>();
			if(tw == null) tw = MenuBottom.AddComponent<TweenAction>();
			tw.tempHidden();
			status.SendMessage("SelectedCrewStatusChange",CrewUpNameStock,SendMessageOptions.DontRequireReceiver);

		}; /* */
		//raceinfo.SendMessage("UpgradeInit", CrewUpNameStock,SendMessageOptions.DontRequireReceiver);
		raceinfo.SendMessage("InitUpgradeCrewInfo", CrewUpNameStock,SendMessageOptions.DontRequireReceiver);

		DestroyImmediate(_prefab);
	}

	void TourUpgradeCrew(GameObject obj){
		if(CrewUpNameTour.Equals(obj.name)) return;
		for(int i = 0; i < activeObject.transform.childCount; i++){
			var temp = activeObject.transform.GetChild(i).gameObject;
			var temp1 = temp.transform.FindChild("Select").gameObject;
			temp1.SetActive(false);
		}
		
		string strTemp = null;
		if(CrewUpNameTour != null){
			strTemp = CrewUpNameTour;
		}else strTemp = "Driver";
		obj.transform.FindChild("Select").gameObject.SetActive(true);
		CrewUpNameTour = obj.name;
		if(strTemp != CrewUpNameTour){
			camAni_Tour.PlayAnimation(strTemp, CrewUpNameTour);
		}else return;
		if(raceinfo != null){
			InfoWindowDisable();
			var _card = raceinfo.GetComponent<EvoInit>() as EvoInit;
			_card.Hidden();/* crew build up*/
			raceinfo = null;
		}
		
		status.SendMessage("SelectedCrewStatusChange",CrewUpNameTour,SendMessageOptions.DontRequireReceiver);
		var _prefab = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
		raceinfo = _prefab.makeCrewStatus(MenuBottom.transform,false);
		/*
		 *  crew buuild up
		 */
		var temp2 = raceinfo.GetComponent<EvoInit>() as EvoInit;
		if(temp2 == null) temp2 = raceinfo.AddComponent<EvoInit>();
		temp2.Show(obj);
		temp2.onFinish = delegate {
			raceinfo.GetComponent<EvoInit>().tempHidden();
			var tw = raceinfo.GetComponent<TweenAction>();
			if(tw == null) tw = raceinfo.AddComponent<TweenAction>();
			tw.tempHidden();
			//raceinfo.GetComponent<UpgradeCrewInfo>().UpdateLevel();
			raceinfo.GetComponent<LevelUpAction>().UpdateCrewUpgrade();
			tw = status.GetComponent<TweenAction>();
			if(tw == null) tw = status.AddComponent<TweenAction>();
			tw.tempHidden();
			tw = MenuBottom.GetComponent<TweenAction>();
			if(tw == null) tw = MenuBottom.AddComponent<TweenAction>();
			tw.tempHidden();
			status.SendMessage("InitUpgradeCrewInfo",CrewUpNameStock,SendMessageOptions.DontRequireReceiver);

		}; /* */
		//raceinfo.SendMessage("UpgradeInit", CrewUpNameTour,SendMessageOptions.DontRequireReceiver);
		raceinfo.SendMessage("InitUpgradeCrewInfo", CrewUpNameTour,SendMessageOptions.DontRequireReceiver);

		//Utility.LogWarning("upgradeInit " + CrewUpNameTour);
		DestroyImmediate(_prefab);
	}

	public void OnUpgardCrewClick(GameObject obj){
		if(camAni.aniPlaying) return;
	
		if(GV.SelectedTeamID%2==0) StockUpgradeCrew(obj);
		else TourUpgradeCrew(obj);
	}

	protected void RepairLobbyAction(){
		if(activeObject.name.Equals("Menu_Lobby") == true)
			activeObject.SendMessage("ResetRepairBtn", SendMessageOptions.DontRequireReceiver);
		
		InitTopMenu();
	}




}
