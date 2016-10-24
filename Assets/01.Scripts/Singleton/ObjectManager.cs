using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
public class ObjectManager :   MonoBehaviour {

	static ObjectManager instance = null;

	void Awake(){
		if(instance == null)
			instance = this;
		else {
			Utility.LogWarning("ObjectManager is not null");
			windList.Clear();
			listAI.Clear();
			System.GC.Collect();
		}
	}

	void OnDestroy(){
		instance = null;
		windList.Clear();
		listAI.Clear();
		System.GC.Collect();
	}
	
	
	void OnApplicationQuit(){
		instance = null;
		windList.Clear();
		System.GC.Collect();
	}

	void Start() {

	}

	public static GameObject CreatePrefabs(Transform _parent, string path, string name){
		var car = CreatePrefabs(path, name) as GameObject;
		car.transform.parent = _parent.transform.parent;
		car.transform.localScale = Vector3.one;
		car.transform.localPosition = Vector3.zero;
		car.transform.localEulerAngles = Vector3.zero;
		return car;
	}

	/*
	public static GameObject ReUsePrefabs(string name, string child){
		GameObject temp = GetSelectObjectInPool(name);
		var temp1 = temp.transform.FindChild(child).gameObject;
		temp1.SetActive(true);
		temp.SetActive(true);
		return temp1;
	}*/

	public static GameObject CreatePrefabs(string path, string name){
		var car = Resources.Load("Prefabs/"+path + "/"+name, typeof(GameObject)) as GameObject;
		var obj = Instantiate(car) as GameObject;
		//Resources.UnloadUnusedAssets();
		return obj;
	}

	public static void ChangeObjectPosition(GameObject obj, Vector3 pos, Vector3 scale, Vector3 rot){
		obj.transform.localScale = scale;
		obj.transform.localPosition = pos;
		obj.transform.localEulerAngles = rot;
	}

	public static void ChangeObjectParent(GameObject obj, Transform parent){
		obj.transform.parent =parent;
	}
	//----------------- Create Texture ------------------//
	public static void CreateSponsorTexture(){
		/*if(TextureList.Count !=0){
			foreach(Texture tx in TextureList){
				//Destroy(tx);
				//tx = null;
			}
			TextureList.Clear();
			System.GC.Collect();
		}
		string sponID = Global.MySponsorID.ToString();
		string texID = Global.MyCarID.ToString()+"_"+sponID;
		Texture mainTex =(Texture)Resources.Load("Textures/Car/"+texID, typeof(Texture));
		mainTex.name = "Car";
		TextureList.Add (mainTex);
		texID = sponID +"_PB";
		mainTex = (Texture)Resources.Load("Textures/PitBox/"+texID, typeof(Texture));
		mainTex.name = "PB";
		TextureList.Add (mainTex);

		texID =Global.MyCrewID.ToString()+"_"+sponID;
		mainTex = (Texture)Resources.Load("Textures/PitCrew/"+texID, typeof(Texture));
		mainTex.name = "Crew";
		TextureList.Add (mainTex);

		string headID = sponID+"_5100";
		mainTex = (Texture)Resources.Load("Textures/PitCrew/"+headID, typeof(Texture));
		mainTex.name = "CrewHead_5100";
		TextureList.Add (mainTex);

		headID = sponID+"_5102";
		mainTex = (Texture)Resources.Load("Textures/PitCrew/"+headID, typeof(Texture));
		mainTex.name =  "CrewHead_5102";
		TextureList.Add (mainTex);

		headID = sponID+"_5103";
		mainTex = (Texture)Resources.Load("Textures/PitCrew/"+headID, typeof(Texture));
		mainTex.name = "CrewHead_5103";
		TextureList.Add (mainTex);
		Resources.UnloadUnusedAssets();*/

	}

	public static List<Texture> TextureList = new List<Texture>();
	public static Texture GetTextureInPool(string name){
		if(TextureList.Count == 0) return null;
		return TextureList.Find((obj)=> obj.name == name);
	}

	// ================= tag name search ===============
	public static List<GameObject> windList = new List<GameObject>();

	static GameObject GetSelectObjectInPool(string name){
		if(windList.Count == 0) return null;
		return windList.Find((obj)=> obj.tag == name);
	}
	public static void ClearListGameObject(){
		if(windList.Count == 0) return;
		else windList.Clear();
	}
	public static GameObject CreateTagPrefabs(string tagname){
		GameObject temp = GetSelectObjectInPool(tagname);

		if(temp != null){
			temp.SetActive(true);
			temp.GetComponent<TweenAction>().ForwardPlayTween(temp);
			return temp;
		}else{
			return temp;
		}
	}
	public static GameObject CreateTagPrefabs(string tagname, Transform _parent){
		GameObject temp = GetSelectObjectInPool(tagname);
		if(temp != null){
			temp.transform.parent = _parent.transform.parent;
			temp.SetActive(true);
			temp.GetComponent<TweenAction>().ForwardPlayTween(temp);
			return temp;
		}else{
			return temp;
		}
	}
	public static GameObject CreatePrefabs(string path, string name, string tagname){
		var obj = CreatePrefabs(path, name) as GameObject;
		windList.Add(obj);
		obj.tag = tagname;
		return obj;
	}

	public static GameObject CreatePrefabs(Transform _parent, string path, string name, string tagname){
		//GameObject car = GetLobbyObject(name); 
		var car = CreatePrefabs(path, name, tagname) as GameObject;
		car.transform.parent = _parent.transform.parent;
		car.transform.localScale = Vector3.one;
		car.transform.localPosition = Vector3.zero;
		car.transform.localEulerAngles = Vector3.zero;
		return car;
	}
	// --------------------------- race Objct  --------------------------------//
	public static GameObject CreateRacePrefabs(string path, string name, Transform parent){
			var car = Resources.Load("Prefabs/"+path + "/"+name, typeof(GameObject)) as GameObject;
			var race = Instantiate(car) as GameObject;
			race.transform.parent = parent;
			race.name = name;
			windList.Add(race);
			race.SetActive(false);
		return race;
	}
	static GameObject GetSelectObjectWithRace(string name){
		if(windList.Count == 0) return null;
		return windList.Find((obj)=> obj.name == name);
	}

	public static GameObject GetRaceObject(string path, string name){
		var race = GetSelectObjectWithRace(name) as GameObject;
		if(race == null){
			//Utility.Log("GETRACE");
			var car = Resources.Load("Prefabs/"+path + "/"+name, typeof(GameObject)) as GameObject;
			race = Instantiate(car) as GameObject;
			race.name = name;
			windList.Add(race);
			//Utility.LogWarning("GetRaceObject " + name);
		}
		race.SetActive(true);
		return race;
	}
	// ShopCarInfo, ShopCrewInfo, ShopSponsorInfo, CarStatus, CrewStatus, 
	//CarTeamInfo, CrewTeamInfo, CarUpInfo, CrewUpInfo, CoinShop, RaceWindow
	//SubWindow

	public static void CreateRaceObject(Transform parent){
		ClearListGameObject();
		CreateRacePrefabs("Effect","skidL",parent);
		CreateRacePrefabs("Effect","skidR",parent);
		CreateRacePrefabs("Race","gameMenu_1",parent);
		CreateRacePrefabs("Race","checkflag0",parent);
		CreateRacePrefabs("Race","checkflag1",parent);

		if(Global.gRaceInfo.sType != SubRaceType.RegularRace){
			CreateRacePrefabs("Race","Rank1",parent);
			for(int i = 2; i<6; i++){
				var obj = CreateRacePrefabs("Race","Rank2",parent);
				obj.name = "Rank"+i;
			}
		}else{
			if(Global.gRaceInfo.mType == MainRaceType.Weekly){
				var obj1 = CreateRacePrefabs("Race","RankR1",parent);
				obj1.name = "Rank1";
				for(int i = 2; i<8; i++){
					var obj = CreateRacePrefabs("Race","RankR2",parent);
					obj.name = "Rank"+i;
				}
			}else if(Global.gRaceInfo.mType == MainRaceType.mEvent && Global.gRaceInfo.eventModeName == "Qube"){
				var obj1 = CreateRacePrefabs("Race","RankR1",parent);
				obj1.name = "Rank1";
				for(int i = 2; i<8; i++){
					var obj = CreateRacePrefabs("Race","RankR2",parent);
					obj.name = "Rank"+i;
				}
			}else{
				CreateRacePrefabs("Race","Rank1",parent);
				for(int i = 2; i<6; i++){
					var obj = CreateRacePrefabs("Race","Rank2",parent);
					obj.name = "Rank"+i;
				}
			
			}
		}





		CreateRacePrefabs("Race","compensation_1",parent);
		CreateRacePrefabs("Race","You",parent);
		CreateRacePrefabs("Race","ModeRewardWindow_1",parent);
		CreateRacePrefabs("Race","Signal",parent);
		CreateRacePrefabs("Race","MiddleRank",parent);

		var temp  = CreateRacePrefabs("Effect","Boost",parent) as GameObject;
		temp.name = "BoostFL";
		temp = CreateRacePrefabs("Effect","Boost",parent);
		temp.name = "BoostFR";
		temp = CreateRacePrefabs("Effect","Boost",parent);
		temp.name = "BoostRR";
		temp = CreateRacePrefabs("Effect","Boost",parent);
		temp.name = "BoostRL";
		
		temp = CreateRacePrefabs("Effect","Nozzle",parent);
		temp.name = "NozzleFL";
		temp = CreateRacePrefabs("Effect","Nozzle",parent);
		temp.name = "NozzleFR";
		temp = CreateRacePrefabs("Effect","Nozzle",parent);
		temp.name = "NozzleRR";
		temp = CreateRacePrefabs("Effect","Nozzle",parent);
		temp.name = "NozzleRL";
		
		Resources.UnloadUnusedAssets();
	}

	public static List<GameObject> listAI = new List<GameObject>();
	public static GameObject GetSelectObjectWithAIRace(string name){
		if(listAI.Count == 0) return null;
		return listAI.Find((obj)=> obj.name == name);
	}
	public static void ClearListAIObject(){
		if(listAI.Count == 0) return;
		else listAI.Clear();
	}

	public static void CreateAIObject(){
		ClearListAIObject();
	
		int gcount = Global.gAICarInfo.Length;
		for(int i = 0; i < gcount ; i++){
			if(Global.gAICarInfo[i].isLive){
				string name = Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[i].AICarID).ToString();
				string name1 =Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[i].AISponsorID).ToString();
				if(Global.gAICarInfo[i].AIClass == null){
					CreateAICar(name, name1,i ,"S");
				}else{
					CreateAICar(name, name1,i , Global.gAICarInfo[i].AIClass.Class);
				}
				name =Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[i].AICrewID).ToString();
				CreateAICrew(name, name1,i);
			}
		}
		int carID=0, crewID = 0, sponID = 0;
		carID = GV.PlayCarID;
		crewID = GV.PlayCrewID;
		sponID = GV.PlaySponID;
		string classID = GV.PlayClassID;
		CreateAICar(carID.ToString(), sponID.ToString(),10 ,classID);
		//if(Global.gRaceInfo.sType != SubRaceType.DragRace)
		CreateAICrew(crewID.ToString(), sponID.ToString(),10);
		Resources.UnloadUnusedAssets();
	}

	 static void CreateAICar(string carId, string sponID,int num, string strClass){
		//Utility.LogWarning("carid " + carId);
		var car = Resources.Load("Prefabs/MyCar/"+ carId, typeof(GameObject)) as GameObject;
		var race = Instantiate(car) as GameObject;
		race.SetActive(false);

		var tr = race.GetComponent<CarType>().CarClass as Transform;
		if(tr.childCount != 0) {
			for(int  i=0; i < tr.childCount; i++){
				if(strClass.Equals(tr.GetChild(i).name)) DestroyImmediate(tr.GetChild(i).gameObject);
			}
		}
		var temp1 = CreatePrefabs(tr.GetChild(0) , "Car_Class", carId+"_"+strClass) as GameObject;
		temp1.name = strClass;

		listAI.Add (race);
		race.name = carId+"_"+num.ToString();
		race.GetComponent<CarType>().CarTextureInitialize(carId, sponID);
	}
	//static StringBuilder sb = new StringBuilder();
	static void CreateAICrew(string crewId, string sponID, int num){
		string crews = string.Empty;
		for(int i = 0; i < 5; i++){
			int ids = 5100+i;
			crews = crewId+"_"+ ids.ToString();
			//Utility.LogWarning(crews);
			var car = Resources.Load("Prefabs/PitCrew/"+ crews, typeof(GameObject)) as GameObject;
			var race = Instantiate(car) as GameObject;
			race.SetActive(false);
			listAI.Add (race);
			race.name = crewId+"_"+ids.ToString()+"_"+num.ToString();
			//Utility.LogWarning("race " + race.name);
			//race.AddComponent<CarTexture>().Crewinitalize(crewId,sponID,num);
			//string[] teamID = race.name.Split("_"[0]);
			var tex = race.GetComponent<textureaction>() as textureaction;
			if(tex == null) tex = race.AddComponent<textureaction>();
			tex.CrewHeadTextureInit(sponID,ids.ToString(), true);
			tex.CrewBodyTextureInit(crewId,sponID, true);
		}
	}
// ---------------------------------------------------------------------------------------------- //
	static GameObject CreateLobbyWindow(string path, string name, string tagname){
		//var obj = CreatePrefabs(path, name) as GameObject;
		var car = Resources.Load("Prefabs/"+path + "/"+name, typeof(GameObject)) as GameObject;
		var obj = Instantiate(car) as GameObject;
		obj.tag = tagname;
		obj.name = name;
		obj.SetActive(false);
		return obj;
	}

	static GameObject GetLobbyObject(string name){
		if(listAI == null || listAI.Count == 0) return null;
		return listAI.Find((obj) => obj.name == name );
	}

	public static void CreateLobbyObject(){
		ClearListAIObject();
		//Utility.Log("CreateLobbyObject");
		var temp = CreateLobbyWindow("Window", "CoinShop", "CoinShop") as GameObject; listAI.Add(temp);
		temp = CreateLobbyWindow( "Lobby", "crewinfo_upgrade_2", "CrewUpInfo") as GameObject;listAI.Add(temp);
		temp = CreateLobbyWindow( "Lobby", "crewinfo_status", "CrewStatus") as GameObject;listAI.Add(temp);
		temp = CreateLobbyWindow( "Lobby", "carinfo_upgrade_2", "CarUpInfo") as GameObject;listAI.Add(temp);
		temp = CreateLobbyWindow( "Lobby", "carinfo_status","CarStatus") as GameObject;listAI.Add(temp);
		temp = CreateLobbyWindow( "Lobby", "carinfo_1", "CarTeamInfo") as GameObject;listAI.Add(temp);
		//temp = CreateLobbyWindow( "Window", "CrewShopInfo", "ShopCrewInfo")as GameObject;listAI.Add(temp);
		temp = CreateLobbyWindow( "Lobby", "CarShopInfo_1", "ShopCarInfo")as GameObject;listAI.Add(temp);
		temp = CreateLobbyWindow( "Lobby", "SponsorInfo_1", "ShopSponsorInfo")as GameObject;listAI.Add(temp);
		temp = CreateLobbyWindow("Card", "EvolutionRepair", "CardButton") as GameObject;listAI.Add(temp);
		temp = CreateLobbyWindow( "Window", "popUp", "PopWindow") as GameObject;listAI.Add(temp);
		temp = CreateLobbyWindow( "Card", "CardPopup", "CardPopup") as GameObject;listAI.Add(temp);
		temp = CreateLobbyWindow( "Window", "SubMenuWindow", "SubWindow") as GameObject;listAI.Add(temp);
		temp =CreateLobbyWindow( "Window", "ModeWindow_1", "RaceWindow") as GameObject;listAI.Add(temp);
		temp =CreateLobbyWindow( "Window", "ModeInfo", "ModeInfo") as GameObject;listAI.Add(temp);
		temp =CreateLobbyWindow( "Window", "ClanWindow", "ClanWindow") as GameObject;listAI.Add(temp);
		temp =CreateLobbyWindow( "Window", "MyTeamMain_4", "TeamMain") as GameObject;listAI.Add(temp);
		temp =CreateLobbyWindow( "Window", "Inventory", "Mechanic") as GameObject;listAI.Add(temp);
		temp =CreateLobbyWindow( "Window", "Luckybox_1", "Luckybox") as GameObject;listAI.Add(temp);
		temp =CreateLobbyWindow( "Window", "RaceMenu_2", "RaceMenu") as GameObject;listAI.Add(temp);
		temp =CreateLobbyWindow( "Lobby", "CarShowroomInfo_2", "ShowCarInfo") as GameObject;listAI.Add(temp);
		temp =CreateLobbyWindow( "Lobby", "MyAccountInfo", "AccInfo") as GameObject;listAI.Add(temp);
		temp =CreateLobbyWindow( "Window", "ChatWindow", "ChatWin") as GameObject;listAI.Add(temp);
		temp.SetActive(true);
		System.GC.Collect();
	//	GameObject Lobby=null;

	//	for(int i = 0; i< 3; i++){
	//		Lobby =CreateLobbyWindow( "Maps", "map", "Coin");
	//		listAI.Add(Lobby);
	//		Lobby.name = "map"+i.ToString();
	//	}

		Resources.UnloadUnusedAssets();
		return;/*
		for (int i = 25; i < 30; i++){
			Lobby =CreateLobbyWindow( "Items", "Slot_Sponsor", "Coin");
			listAI.Add(Lobby);
			Lobby.name = "Slot_Sponsor"+i.ToString();
		}

		for (int i = 0; i < 12; i++){
			Lobby =CreateLobbyWindow( "Items", "Slot_Coin", "Coin");
			listAI.Add(Lobby);
		}


		for(int i = 0; i < 20; i++){
			Lobby =CreateLobbyWindow( "Items", "Slot_Car", "Coin");
			listAI.Add(Lobby);
			Lobby.name = "Slot_Car"+i.ToString();
		}
		for(int i = 25; i < 50; i++){
			Lobby =CreateLobbyWindow( "Items", "Slot_Car", "Coin");
			listAI.Add(Lobby);
			Lobby.name = "Slot_Car"+i.ToString();
		}
		System.GC.Collect();
		for (int i = 0; i < 7; i++){
			Lobby =CreateLobbyWindow( "Items", "Slot_Crew", "Coin");
			listAI.Add(Lobby);
			Lobby.name = "Slot_Crew"+i.ToString();
		}
		for (int i = 25; i < 33; i++){
			Lobby =CreateLobbyWindow( "Items", "Slot_Crew", "Coin");
			listAI.Add(Lobby);
			Lobby.name = "Slot_Crew"+i.ToString();
		}
		System.GC.Collect();

		Lobby =temp = null;
		System.GC.Collect();*/
	}

	public static GameObject CreateLobbyPrefabs(string path, string name){
		GameObject win = GetLobbyObject(name);
		return win;
	}

	public static GameObject CreateLobbyPrefabs(Transform _parent, string path, string name, string tagname){
		GameObject win = GetLobbyObject(name);
		if(win == null) Utility.LogError("lobby error " + name);
		win.transform.parent = _parent.transform.parent;
		win.transform.localScale = Vector3.one;
		win.transform.localPosition = Vector3.zero;
		win.transform.localEulerAngles = Vector3.zero;
		windList.Add(win);
		listAI.Remove(win);
		win.SetActive(true);
		return win;
	}

	public static GameObject SearchWindowPopup(string mAnchor = null){
		var pop = CreateTagPrefabs("PopWindow") as GameObject;
		if(pop != null) {
			
		}else{
			if(mAnchor == null) mAnchor = "CenterAnchor";
			var Parent = GameObject.FindGameObjectWithTag(mAnchor) as GameObject;
			pop = CreateLobbyPrefabs(Parent.transform, "Window", "popUp", "PopWindow") as GameObject;
			pop.transform.parent = Parent.transform;
			ChangeObjectPosition(pop, new Vector3(0,0,-1400), Vector3.one, Vector3.zero);
		}
		var popchild = pop.transform.FindChild("Content_BUY").gameObject as GameObject;
		pop.GetComponent<TweenAction>().doubleTweenScale(popchild);
		return pop;
	}


	/*
	 * 	public static List<Texture> listText = new List<Texture>();
	public static Texture GetSelectObjectWithAITexture(string name){
		if(listText.Count == 0) return null;
		return listText.Find((obj)=> obj.name == name);
	}

	public static void ClearListAITexture(){
		if(listText.Count == 0) return;
		else listText.Clear();
	}
	public delegate void OnClickCheck();
	public static OnClickCheck _onClickCheck = null;
	System.Action Callbacks;

	public  void CheckObjectName(OnClickCheck _onclick, System.Action Callbacks){
		this.Callbacks = Callbacks;
		_onClickCheck = _onclick;
		checkobjectname1();
	}

	public static void checkobjectname1(){
		_onClickCheck();
	}

	public delegate GameObject makePrefabs(string name);
	public static makePrefabs _makeprefabs;

	public static GameObject PrefabTest(makePrefabs _fun , string name){

		var obj = CreatePrefabs("Car","1000");
		_fun(name);
		return obj;
	}
*/
}
