using UnityEngine;
using System.Collections;

public class CPopUpCreate : MonoBehaviour {

	public GameObject CreatePrefabsItems(string name, GameObject _parent){
		var _obj = ObjectManager.CreateLobbyPrefabs("Items",name);
		if(_obj == null){
			_obj = ObjectManager.CreatePrefabs(_parent.transform,"Items", name); 
		}
		_obj.transform.parent = _parent.transform;
		ObjectManager.ChangeObjectPosition(_obj, Vector3.zero, Vector3.one, Vector3.zero);
		_obj.SetActive(true);
		return _obj;
	}
	public GameObject makeChatWin(Transform _parent){
		var temp = ObjectManager.CreateTagPrefabs("ChatWin") as GameObject;
		if(temp != null) return temp;
		return ObjectManager.CreateLobbyPrefabs(_parent,  "Window", "ChatWindow", "ChatWin") ;
	}
	public GameObject makeLuckyBox(Transform _parent){
		var temp = ObjectManager.CreateTagPrefabs("Luckybox") as GameObject;
		if(temp != null) 	return temp;
		return ObjectManager.CreateLobbyPrefabs(_parent, "Window", "Luckybox_1", "Luckybox");
	}
	public GameObject makeInven(Transform _parent){
		var temp = ObjectManager.CreateTagPrefabs("Mechanic") as GameObject;
		if(temp != null) 	return temp;
		return ObjectManager.CreateLobbyPrefabs(_parent, "Window", "Inventory", "Mechanic");
	}
	public GameObject makeTeam(Transform _parent){
		var temp = ObjectManager.CreateTagPrefabs("TeamMain") as GameObject;
		if(temp != null) 	return temp;
		return ObjectManager.CreateLobbyPrefabs(_parent, "Window", "MyTeamMain_4", "TeamMain");
	}
	public GameObject makeClan(Transform _parent){
		var temp = ObjectManager.CreateTagPrefabs("ClanWindow") as GameObject;
		if(temp != null) 	return temp;
		//return ObjectManager.CreatePrefabs(_parent, "Window", "CoinShop", "CoinShop");
		return ObjectManager.CreateLobbyPrefabs(_parent, "Window", "ClanWindow", "ClanWindow");
	}
	public GameObject makeRaceMenu(Transform _parent){

		var temp = ObjectManager.CreateTagPrefabs("RaceMenu") as GameObject;
		if(temp != null) {
			return temp;
		}
		var Parent = GameObject.FindGameObjectWithTag("CenterAnchor") as GameObject;
				//return ObjectManager.CreatePrefabs(_parent, "Window", "CoinShop", "CoinShop");
        return ObjectManager.CreateLobbyPrefabs(Parent.transform.GetChild(0), "Window", "RaceMenu_2", "RaceMenu");
	}
	public GameObject makeCoinShopInfo(Transform _parent){
		var temp = ObjectManager.CreateTagPrefabs("CoinShop") as GameObject;
		if(temp != null) 	{
			return temp;
		}
		return ObjectManager.CreateLobbyPrefabs(_parent, "Window", "CoinShop", "CoinShop");
	}
	public GameObject makeSponsorShopInfo(Transform _parent){
		var temp = ObjectManager.CreateTagPrefabs("ShopSponsorInfo") as GameObject;
		if(temp != null) return temp;
		return ObjectManager.CreateLobbyPrefabs(_parent, "Lobby", "SponsorInfo_1", "ShopSponsorInfo");
	}
	public GameObject makeCarShopInfo(Transform _parent){
		var temp = ObjectManager.CreateTagPrefabs("ShopCarInfo") as GameObject;
		if(temp != null) return temp;
		return ObjectManager.CreateLobbyPrefabs(_parent, "Lobby", "CarShopInfo_1", "ShopCarInfo");
	}
	public GameObject makeCarShowInfo(Transform _parent){
		var temp = ObjectManager.CreateTagPrefabs("ShowCarInfo") as GameObject;
		if(temp != null) return temp;
		return ObjectManager.CreateLobbyPrefabs(_parent, "Lobby", "CarShowroomInfo_2", "ShowCarInfo");
	}
	public GameObject makeCrewShopInfo(Transform _parent){
		var temp = ObjectManager.CreateTagPrefabs("ShopCrewInfo") as GameObject;
		if(temp != null) return temp;
		return ObjectManager.CreateLobbyPrefabs(_parent, "Window", "CrewShopInfo", "ShopCrewInfo");
	}
	public GameObject MakeCarInfo(Transform _parent){
		var temp = ObjectManager.CreateTagPrefabs("CarTeamInfo") as GameObject;
		if(temp != null) return temp;
		
		return ObjectManager.CreateLobbyPrefabs(_parent, "Lobby", "carinfo_1", "CarTeamInfo");
	}

	public GameObject MakeCrewInfo(Transform _parent){
		var temp = ObjectManager.CreateTagPrefabs("CrewTeamInfo") as GameObject;
		if(temp != null) return temp;
		return ObjectManager.CreateLobbyPrefabs(_parent, "Lobby", "crewinfo","CrewTeamInfo");
	}
	public GameObject makeCarStatus(Transform _parent, bool b){
		//GameObject obj = null;
		if(b) {
			var temp = ObjectManager.CreateTagPrefabs("CarStatus") as GameObject;
			if(temp != null) return temp;
			return ObjectManager.CreatePrefabs(_parent, "Lobby", "carinfo_status", "CarStatus");
			//obj = ObjectManager.CreatePrefabs(_parent, "Lobby", "carinfo_status");
			//obj.name = "CarStatus";
		}
		else {
			var temp = ObjectManager.CreateTagPrefabs("CarUpInfo") as GameObject;
			if(temp != null) 
				return temp;
			
			return ObjectManager.CreateLobbyPrefabs(_parent, "Lobby", "carinfo_upgrade_2", "CarUpInfo");
			//obj =  ObjectManager.CreatePrefabs(_parent, "Lobby", "carinfo_upgrade");
		}
		//return obj;
	}
	
	public GameObject makeCrewStatus(Transform _parent, bool b){
	//	GameObject obj = null;
		if(b) {
			var temp = ObjectManager.CreateTagPrefabs("CrewStatus") as GameObject;
			if(temp != null) 
				return temp;

			return ObjectManager.CreateLobbyPrefabs(_parent, "Lobby", "crewinfo_status", "CrewStatus");
			//obj = ObjectManager.CreatePrefabs(_parent, "Lobby", "crewinfo_status");
			//obj.name = "CrewStatus";
		}else {
			var temp = ObjectManager.CreateTagPrefabs("CrewUpInfo") as GameObject;
			if(temp != null) 
				return temp;
			return ObjectManager.CreateLobbyPrefabs(_parent, "Lobby", "crewinfo_upgrade_2", "CrewUpInfo");
		}
	}
	public GameObject makeAccInfo(Transform _parent){
		var temp = ObjectManager.CreateTagPrefabs("AccInfo") as GameObject;
		if(temp != null) return temp;
		
		return ObjectManager.CreateLobbyPrefabs(_parent,  "Lobby", "MyAccountInfo", "AccInfo") ;
	}
	public void InitInfoSet(GameObject info){
		info.transform.FindChild("button").gameObject.SetActive(false);
		info.transform.FindChild("check").gameObject.SetActive(true);
		info.transform.FindChild("lbSelect").GetComponent<UILabel>().text = KoStorage.GetKorString("76308");
	}

	public void InitInfoRelease(GameObject info){
		info.transform.FindChild("button").gameObject.SetActive(true);	
		info.transform.FindChild("check").gameObject.SetActive(false);
	}
}
