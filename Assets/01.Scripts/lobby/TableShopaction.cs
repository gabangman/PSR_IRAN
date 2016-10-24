using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TableShopaction : MonoBehaviour {

	Animation ShopAnimation;
	Animation FenceAni;
	public Transform[] teamPos;
	public Transform[] shopPos;
	public Transform[] sponPos;
	public Transform[] lobbyPos;
	public Transform[] lobbyPos_Tour;
	GameObject[] _tempObject;
	System.Action Callback;
	public delegate void FinishAnimation();
	public FinishAnimation _finishani;
	public GameObject Fence;
//	bool isSponStock = false;
//	bool isSponTour = false;
	bool isFenceDown = false;
	bool isSponsor = false;
	bool isShop = false;
	public AudioClip AudioUp, AudioDown, AudioFence;
	void Awake(){
		ShopAnimation = GetComponent<Animation>();
		FenceAni = Fence.GetComponent<Animation>();
		//StartCoroutine("ShopAniStart", "My_Car_Up");
		ShopAnimation.Play("My_Car_Up");
	}

	void Start(){
		_tempObject = new GameObject[6];
	}

	void OnDestroy(){
		listCarTemp.ForEach(obj => Destroy(obj));
		listCarTemp.Clear();
		listCrewTemp.ForEach(obj => Destroy(obj));
		listCrewTemp.Clear();
		listTeam.ForEach(obj => Destroy(obj));
		listTeam.Clear();
		listTeam_Tour.ForEach(obj => Destroy(obj));
		listTeam_Tour.Clear();
	}

	void CallFunction(){
		if(_finishani != null)
			_finishani();
		_finishani = null;
	}
	public void StoreAniFinishFunction(FinishAnimation _onFinish){
		_finishani = _onFinish;
	}

	public void TableShopDelete(){

		DeleteTempObject(0);
		DeleteTempObject(5);
		if(isSponsor) {
			isSponsor =false;
			FenceDown();
		//	int tempid = Base64Manager.instance.GlobalEncoding(Global.MySponsorID);
		//	if(isSponStock) {
		//		tempid = GV.FindStockSponsor();
		//		ChangeSponTeam(tempid, isSponsor);
		//		isSponStock = false;
		//	}
		//	if(isSponTour){
		//		tempid = GV.FindTourSponsor();
		//		ChangeSponTeamTour(tempid, isSponsor);
		//		isSponTour = false;
		//	}
			int tempid = GV.getTeamSponID(GV.SelectedTeamID);
			ChangeSponTeam(tempid, isSponsor);
		}

		if(isShop){
			ShopAnimation.Play("My_Car_Up");
			isShop = false;
		}
		Resources.UnloadUnusedAssets();
	}

	public void OnTeam(string carID, string crewID){
		DeleteTempObject(0);
		DeleteTempObject(5);
		CreateTeamCar(carID);
		CreateTeamCrew(crewID);
	}

	public void OnTeamTour(string carID, string crewID){
		DeleteTempObject(0);
		DeleteTempObject(5);
		CreateTeamCar(carID);
		CreateTeamCrew(crewID);
	}


	public void OnMyCar(string carID){
		StartCoroutine("TeamCarChange", carID);
	}

	public void OnMyCrew(string crewID){
		StartCoroutine("TeamCrewChange", crewID);
	}

	public void OnSponClick(int sponID, bool b){
		StartCoroutine(FenceAniStart(sponID,b));
	}

	public void FenceDown(){
		if(isFenceDown) return;
		FenceAni["Take 001"].time = 	FenceAni["Take 001"].length;
		FenceAni["Take 001"].speed = 3;
		FenceAni.Play ("Take 001");
		isFenceDown = true;
	}

	public void FenceDownState(){
		//FenceAni["Take 001"].time =  0;
	}

	public void SponsorStart(bool b){
		if(!isSponsor){
			isSponsor = true;
			if(b) {
				ChangeSponTeam(1301, isSponsor);
				//isSponStock = true;
			}
			else {
				ChangeSponTeam(1301, isSponsor);
				//ChangeSponTeamTour(1301, isSponsor);
				//isSponTour = true;
			}
		}

		FenceAni["Take 001"].time = FenceAni["Take 001"].length;
		FenceAni["Take 001"].speed = -1;
		FenceAni.Play ("Take 001");
		isFenceDown = false;
	}

	public void RaceCamStart(bool b){
		if(!isSponsor){
			isSponsor = true;
			ChangeSponTeam(GV.getTeamSponID(GV.SelectedTeamID), true);
		}
		FenceAni["Take 001"].time = FenceAni["Take 001"].length;
		FenceAni["Take 001"].speed = -1;
		FenceAni.Play ("Take 001");
		isFenceDown = false;

	}
	public void setFenceDown(){
		if(isFenceDown) return;
		FenceAni["Take 001"].time = 0;
		FenceAni["Take 001"].speed = 1;
		FenceAni.Play ("Take 001");
		isFenceDown = true;
		//fendce close
	}

	public void ShopStart(){
		isShop = true;
	}

	public void OnShopCar(string carID, string carClass){
		StartCoroutine(ShopCarChange(carID, carClass));
	}

	public void OnDealerCar(string carID, string carClass){
		StartCoroutine(DealerCarChange(carID, carClass));
	}


	public void OnNoDealerCar(){
		DeleteTempObject(5);

	}

	public void ClassItemClick(string name){
		string[] strTarget = name.Split('_');
		ChangeClassCar(strTarget[0], strTarget[2]);
	}


	public void ItemClick(string _name, System.Action Callback, bool b){
		string[] name = _name.Split("_"[0]);
		this.Callback = Callback;
		Utility.LogWarning(_name); 
		switch(name[1]){
		case "Sponsor":
			int id = int.Parse(name[0]);
			OnSponClick(id, b);
			break;
		case "tCar":
			OnShopCar(name[0], name[2]);
			break;
		case "Crew":
			//OnShopCrew(name[0]);
			break;
		case "ssCar":
			OnShopCar(name[0], name[2]);
			break;
		case "Car":
			OnShopCar(name[0], name[2]);
			break;
		case "Dealer":
			OnDealerCar(name[0], name[2]);
			break;
		case "DealerNo":
			OnNoDealerCar();
			break;
		default:
			break;
		}
	}

	List<GameObject> listCrewTemp = new List<GameObject>();
	List<GameObject> listCarTemp = new List<GameObject>();
	List<GameObject> listTeam = new List<GameObject>();
	List<GameObject> listTeam_Tour = new List<GameObject>();
	void DeleteTempObject(int index){
		int len = _tempObject.Length;
		int j=0;
		if(index == 0){
			j = 0; len = 5;
		}else{
			j = 5; len = 6;
		}
		for(int i = j ; i < len ; i++){
			if(_tempObject[i] != null)
			{
				_tempObject[i].SetActive(false);
				_tempObject[i] = null;
			}
		}
	}

	public void AudioDownPlay(){
		NGUITools.PlaySound(AudioDown);
	}

	public void AudioUpPlay(){
		NGUITools.PlaySound(AudioUp);
	}

	IEnumerator TeamCarChange(string carID){
		Global.isAnimation = true;	
	//	UserDataManager.instance.StartShowTip("Waiting.......");
		NGUITools.PlaySound(AudioDown);
		yield return StartCoroutine("ShopAniStart", "My_Car_Down");
		DeleteTempObject(5);
		CreateTeamCar(carID);
		CallFunction();
		yield return new WaitForSeconds(0.1f);
		NGUITools.PlaySound(AudioUp);
		yield return StartCoroutine("ShopAniStart", "My_Car_Up");
	//	UserDataManager.instance.StartShowTip(null);
		Global.isAnimation = false;
	}

	IEnumerator TeamCrewChange(string crewID){
		Global.isAnimation = true;	
		UserDataManager.instance.StartShowTip("Waiting.......");
		yield return StartCoroutine("ShopAniStart", "My_Crew_Down");
		DeleteTempObject(0);
		 CreateTeamCrew(crewID);
		CallFunction();
		yield return new WaitForSeconds(0.1f);
		NGUITools.PlaySound(AudioUp);
		yield return StartCoroutine("ShopAniStart", "My_Crew_Up");
		UserDataManager.instance.StartShowTip(null);
		Global.isAnimation = false;
	}
	/*
	IEnumerator ShopCrewChange(string crewID){
		Global.isAnimation = true;	
		UserDataManager.instance.StartShowTip("Waiting.......");
		yield return StartCoroutine("ShopAniStart", "Shop_All_Down");
		DeleteTempObject(0);
		DeleteTempObject(5);
		  CreateShopCrew(crewID);
		Callback();
		yield return new WaitForSeconds(0.1f);
		yield return StartCoroutine("ShopAniStart", "Shop_All_Up");
		UserDataManager.instance.StartShowTip(null);
		Global.isAnimation = false;
	}*/

	IEnumerator ShopCarChange(string carID, string carClass){
		Global.isAnimation = true;	
	//	UserDataManager.instance.StartShowTip("Waiting.......");
		NGUITools.PlaySound(AudioDown);
		yield return StartCoroutine("ShopAniStart", "Shop_All_Down");
		DeleteTempObject(0);
		DeleteTempObject(5);
		CreateShopCar(carID, carClass);
		Callback();
		yield return new WaitForSeconds(0.1f);
		NGUITools.PlaySound(AudioUp);
		yield return StartCoroutine("ShopAniStart", "Shop_All_Up");
	//	UserDataManager.instance.StartShowTip(null);
		Global.isAnimation = false;
	}

	IEnumerator DealerCarChange(string carID, string carClass){
		Global.isAnimation = true;	
		NGUITools.PlaySound(AudioDown);
		yield return StartCoroutine("ShopAniStart", "Shop_All_Down");
		DeleteTempObject(0);
		DeleteTempObject(5);
		CreateDealerShopCar(carID, carClass);
		Callback();
		yield return new WaitForSeconds(0.1f);
		NGUITools.PlaySound(AudioUp);
		yield return StartCoroutine("ShopAniStart", "Shop_All_Up");
		Global.isAnimation = false;
	}

	IEnumerator ShopAniStart(string aniName){
		ShopAnimation.Play(aniName);
		do{
			yield return true;
		}while(ShopAnimation.isPlaying);

	}

	IEnumerator FenceAnimation(Animation ani){
		while(true){
			if(!ani.isPlaying){
				break;
			}
			yield return null;
		}
	}


	IEnumerator FenceAniStart(int sponID, bool b){
		Global.isAnimation = true;	
	//	UserDataManager.instance.StartShowTip("Waiting.......");
		NGUITools.PlaySound(AudioFence);
		FenceAni["Take 001"].time = 0;
		FenceAni["Take 001"].speed = 1;
		FenceAni.Play ("Take 001");
		yield return StartCoroutine("FenceAnimation", FenceAni);
		ChangeSponTexture(sponID);
		Callback();
		yield return new WaitForSeconds(0.1f);
		FenceAni["Take 001"].time = FenceAni["Take 001"].length;
		FenceAni["Take 001"].speed = -1;
		FenceAni.Play ("Take 001");
		yield return StartCoroutine("FenceAnimation", FenceAni);
	//	UserDataManager.instance.StartShowTip(null);
		isFenceDown = false;
		Global.isAnimation = false;
		
	}
	// My_Car_Down, My_Car_Up, My_Crew_Down, My_Crew_Up, Shop_All_Down, Shop_All_Up

	GameObject GetSelectCarInPool(string carID){
		if(listCarTemp.Count == 0) return null;
		return listCarTemp.Find((obj)=> obj.name == carID);
	}

	GameObject GetSelectCrewInPool(string crewID){
		if(listCrewTemp.Count == 0) return null;
		return listCrewTemp.Find((obj)=> obj.name == crewID);
	}

	 void ChangePosition(GameObject orginalT, Transform target){
		orginalT.transform.parent =target.transform.parent;
		orginalT.transform.localScale = target.localScale;
		orginalT.transform.localPosition = target.localPosition;
		orginalT.transform.localRotation = target.localRotation;
	}


	void CreateCarClassObject(GameObject car, string carClass){
	
		var tr = car.GetComponent<CarType>().CarClass as Transform;
		if(tr.childCount != 0) {
			for(int  i=0; i < tr.childCount; i++){
				//Utility.LogWarning(tr.GetChild(i).name);
				if(tr.GetChild(i).name.Equals("GameObject")==false) DestroyImmediate(tr.GetChild(i).gameObject);
			//	if(carClass.Equals(tr.GetChild(i).name)) DestroyImmediate(tr.GetChild(i).gameObject);
			}
		}
		var temp1 = ObjectManager.CreatePrefabs(tr.GetChild(0), "Car_Class", car.name+"_"+carClass) as GameObject;
		temp1.name = carClass;

	}


	void ChangeClassCar(string CarID, string CarClass){
		var car = GetSelectCarInPool(CarID) as GameObject;
		if(car != null){
			car.SetActive(true);
			ChangePosition(car, shopPos[5]);
			CreateCarClassObject(car, CarClass);
			_tempObject[5] = car;
		}else{
		//	car = ObjectManager.CreatePrefabs("MyCar",CarID) as GameObject;
		//	ChangePosition(car, shopPos[5]);
		//	car.name = carID;
		//	CreateCarClassObject(car, "D");
		//	listCarTemp.Add(car);
		}
	}
	private void CreateShopCar(string carID, string carClass){
		//CreateDealerShopCar
		var car = GetSelectCarInPool(carID) as GameObject;
		if(car != null){
			car.SetActive(true);
			ChangePosition(car, shopPos[5]);
			CreateCarClassObject(car, "D");
			_tempObject[5] = car;
		}else{
			car = ObjectManager.CreatePrefabs("MyCar",carID) as GameObject;
			ChangePosition(car, shopPos[5]);
			car.name = carID;
			CreateCarClassObject(car, "D");
			listCarTemp.Add(car);
		}
		/*
		int _id = int.Parse(carID);
		Common_Car_Status.Item Item = Common_Car_Status.Get(_id);
		int mSeason = GV.ChSeasonID;
		int mSLV = 0;
		if(mSeason > 6035){
			Common_Mode_Champion.Item item= Common_Mode_Champion.Get(mSeason);
			mSLV = item.Season;
		}else{
			mSLV = GV.ChSeason;
		}*/
		//CarTextureInit(car, 1300);
		AccountManager.instance.SetCarTexture(car,1300);
		_tempObject[5] = car;
		
	}
	private void CreateDealerShopCar(string carID, string carClass){
		var car = GetSelectCarInPool(carID) as GameObject;
		if(car != null){
			car.SetActive(true);
			ChangePosition(car, shopPos[5]);
			CreateCarClassObject(car, carClass);
			_tempObject[5] = car;
		}else{
			car = ObjectManager.CreatePrefabs("MyCar",carID) as GameObject;
			ChangePosition(car, shopPos[5]);
			car.name = carID;
			CreateCarClassObject(car, carClass);
			listCarTemp.Add(car);
		}
		//CarTextureInit(car, 1300);
		AccountManager.instance.SetCarTexture(car,1300);
		_tempObject[5] = car;

	}
	/*
	void CreateShopCrew(string crewID){
		string carID = crewID+"_5100";
		var crew = GetSelectCrewInPool(carID) as GameObject;
		int _id =5100;
		string name = string.Empty;
		
		if(crew != null){
			crew.SetActive(true);
			ChangePosition(crew, shopPos[0]);
			_tempObject[0] = crew;
			for(int i = 1; i < 5; i++){
				_id = 5100+i;
				 name =crewID+"_"+ _id.ToString();
				crew = GetSelectCrewInPool(name);
				ChangePosition(crew, shopPos[i]);
				crew.SetActive(true);
				_tempObject[i] = crew;
			}
	

			//return;
		}else{
			
		for(int i = 0; i < 5; i++){
			_id = 5100+i;
			name = crewID+"_"+ _id.ToString();
			var car = ObjectManager.CreatePrefabs("PitCrew",name) as GameObject;
			ChangePosition(car, shopPos[i]);
			car.name = name;
			//CrewTextureInit(car,1300);
			listCrewTemp.Add(car);
			_tempObject[i] = car;
			//CrewTextureInit(car);
			_id++;
		}
		}
		int id = int.Parse(crewID);
		Common_Crew_Status.Item Item = Common_Crew_Status.Get(id);
		int mSeason = Base64Manager.instance.GlobalEncoding(Global.ChampionSeason);
		int mSLV = 0;
		if(mSeason > 6024){
			Common_Mode_Champion.Item item= Common_Mode_Champion.Get(mSeason);
			mSLV = item.Season;
		}else{
			mSLV = Global.MySeason;
		}

		if(mSLV >= Item.ReqLV){
			for(int  i = 0; i < 5; i++){
				CrewTextureInit(_tempObject[i],1300);
			}
		}else{
			for(int  i = 0; i < 5; i++){
				CrewTextureInit(_tempObject[i]);
			}
		}

	}
*/
	void CreateTeamCar(string carID){
		string[] name = carID.Split('_');
	//	Utility.LogWarning(carID);
		var car = GetSelectCarInPool(name[0]);
		if(car != null){
			car.SetActive(true);
			ChangePosition(car, teamPos[5]);
			CreateCarClassObject(car,name[1]);
			_tempObject[5] = car;
			//return;
		}else{
			car = ObjectManager.CreatePrefabs("MyCar",name[0]) as GameObject;
			ChangePosition(car, teamPos[5]);
			_tempObject[5] = car;
			car.name = name[0];
			CreateCarClassObject(car, name[1]);
			listCarTemp.Add(car);
		}
		int tempid =GV.getTeamSponID(GV.SelectedTeamID);// Base64Manager.instance.GlobalEncoding(Global.MySponsorID);
		//tempid = 1300;
		//CarTextureInit(car, tempid);
		AccountManager.instance.SetCarTexture(car,tempid);
		//Utility.Log("create 3");
	}

	void CreateTeamCrew(string crewID){
		//Utility.LogWarning("crewID " + crewID);
		string carID = crewID+"_5100";
		var crew = GetSelectCrewInPool(carID) as GameObject;
		int _id =5100;
		string name = string.Empty;
		int tempid = GV.getTeamSponID(GV.SelectedTeamID);//Base64Manager.instance.GlobalEncoding(Global.MySponsorID);
	//	tempid = 1300;
		if(crew != null){
			crew.SetActive(true);
			ChangePosition(crew, teamPos[0]);
			_tempObject[0] = crew;
			CrewTextureInit(crew,tempid);
			for(int i = 1; i < 5; i++){
				_id = 5100+i;
				name =crewID+"_"+ _id.ToString();
				crew = GetSelectCrewInPool(name);
				ChangePosition(crew, teamPos[i]);
				crew.SetActive(true);
				CrewTextureInit(crew,tempid);
				_tempObject[i] = crew;
			}
		}else{
			for(int i = 0; i < 5; i++){
				_id = 5100+i;
				name =crewID+"_"+ _id.ToString();
				crew = ObjectManager.CreatePrefabs("PitCrew",name) as GameObject;
				ChangePosition(crew, teamPos[i]);
				listCrewTemp.Add(crew);
				_tempObject[i] = crew;
				crew.name = name;
				CrewTextureInit(crew,tempid);
				_id++;
			}//Resources.UnloadUnusedAssets();
		}

	}

	void ChangeSponTexture(int sponID){
		GameObject temp = null;
		for(int i = 0; i < 5;i++){
			temp = sponPos[i].GetChild(0).gameObject;
			CrewTextureInit(temp, sponID);
		}
		temp = sponPos[5].GetChild(0).gameObject;
		//CarTextureInit(temp, sponID);
		AccountManager.instance.SetCarTexture(temp,sponID);
	}

	void ChangeSponTeam(int sponID, bool b){
		GameObject temp=null;
		Transform[] arrobj =null, target=null;
		if(b) {
			arrobj = lobbyPos;target = sponPos;
		}
		else{
			arrobj = sponPos;target = lobbyPos;
		}
		for(int i = 0; i < 5;i++){
			//Utility.Log(i + "  " + arrobj[i].name);
			temp = arrobj[i].GetChild(0).gameObject;
			temp.transform.parent = target[i].transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = Vector3.zero;
			temp.transform.localEulerAngles = Vector3.zero;
			CrewTextureInit(temp, sponID);
		}
		temp = arrobj[5].GetChild(0).gameObject;
		temp.transform.parent = target[5].transform;
		temp.transform.localScale = Vector3.one;
		temp.transform.localPosition =Vector3.zero;
		temp.transform.localEulerAngles =Vector3.zero;
		//CarTextureInit(temp, sponID);
		AccountManager.instance.SetCarTexture(temp,sponID);
		var ani = target[0].GetChild(0).GetChild(0).GetComponent<Animation>() as Animation;
		if(b){
			ani.wrapMode = WrapMode.Loop;
			ani.Play("driver_idle_stand");
		}else{
			ani.wrapMode = WrapMode.Loop;
			ani.Play("driver_idle");
		}

	}

	void ChangeSponTeamTour(int sponID, bool b){
		GameObject temp=null;
		Transform[] arrobj =null, target=null;
		if(b) {
			arrobj = lobbyPos_Tour;target = sponPos;
		}
		else{
			arrobj = sponPos;target = lobbyPos_Tour;
		}
		for(int i = 0; i < 5;i++){
			//Utility.Log(i + "  " + arrobj[i].name);
			temp = arrobj[i].GetChild(0).gameObject;
			temp.transform.parent = target[i].transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = Vector3.zero;
			temp.transform.localEulerAngles = Vector3.zero;
			CrewTextureInit(temp, sponID);
		}
		temp = arrobj[5].GetChild(0).gameObject;
		temp.transform.parent = target[5].transform;
		temp.transform.localScale = Vector3.one;
		temp.transform.localPosition =Vector3.zero;
		temp.transform.localEulerAngles =Vector3.zero;
		//CarTextureInit(temp, sponID);
		AccountManager.instance.SetCarTexture(temp,sponID);
		
		var ani = target[0].GetChild(0).GetChild(0).GetComponent<Animation>() as Animation;
		if(b){
			ani.wrapMode = WrapMode.Loop;
			ani.Play("driver_idle_stand");
		}else{
			ani.wrapMode = WrapMode.Loop;
			ani.Play("driver_idle");
		}
		
	}


	public void CreateLobbyCar(int id, string carClass){
		if(lobbyPos[5].childCount != 0) {
			var texture = lobbyPos[5].GetChild(0).gameObject.GetComponent<CarType>() as CarType;
			texture.CarSponTextureInitialize(lobbyPos[5].GetChild(0).gameObject.name, GV.getTeamSponID(GV.SelectedTeamID).ToString());
			return;
		}
		string _carName = id.ToString();
		GameObject LobbyCar = listTeam.Find((obj) => obj.name == _carName);
		if(LobbyCar != null){
			LobbyCar.SetActive(true);
			CreateCarClassObject(LobbyCar, carClass);
		}else{
			LobbyCar = ObjectManager.CreatePrefabs(lobbyPos[5], "MyCar" , _carName) as GameObject;
			LobbyCar.name = _carName;
			CreateCarClassObject(LobbyCar, carClass);
			listTeam.Add(LobbyCar);
		}
		ObjectManager.ChangeObjectParent(LobbyCar,  lobbyPos[5]);
		LobbyCar.transform.localEulerAngles = new Vector3(0,0,0);
		lobbyPos[0] = LobbyCar.transform.GetChild(0).FindChild("Driver_Axis");
		lobbyPos[0].localEulerAngles = new Vector3(0,270,0);
		int tempid = GV.getTeamSponID(GV.SelectedTeamID);
		//CarTextureInit(LobbyCar, tempid);
		AccountManager.instance.SetCarTexture(LobbyCar,tempid);
	}


	public void CreateLobbyCrew(int idx, int idx1){
		if(lobbyPos[1].childCount != 0){
			//Utility.LogWarning((lobbyPos[0].GetChild(0).gameObject.name));
			int sID = GV.getTeamSponID(GV.SelectedTeamID);
			for(int i = 1; i< 5; i++){
				CrewTextureInit(lobbyPos[i].GetChild(0).gameObject,sID);
			}
			//CrewTextureInit(lobbyPos[0].GetChild(0).gameObject,sID);
			string name =  idx.ToString()+"_"+(5100).ToString();
			GameObject driver = listTeam.Find((obj) => obj.name == name);
			if(driver != null){
				driver.SetActive(true);
			}else{
				driver = ObjectManager.CreatePrefabs( lobbyPos[0], "pitCrew", name) as GameObject;
				driver.name = name;
				listTeam.Add(driver);
			}
			ObjectManager.ChangeObjectParent(driver,  lobbyPos[0]);
			driver.transform.position = lobbyPos[0].position;
			driver.transform.rotation =lobbyPos[0].rotation;
			CrewTextureInit(driver,sID);
			return;
		}
		string id = idx.ToString();
		for(int i = 0; i< 5; i++){
			string name = id+"_"+(5100+i);
			GameObject driver = listTeam.Find((obj) => obj.name == name);
			if(driver != null){
				driver.SetActive(true);
			}else{
				driver = ObjectManager.CreatePrefabs( lobbyPos[i], "pitCrew", name) as GameObject;
				driver.name = name;
				listTeam.Add(driver);
			}
			ObjectManager.ChangeObjectParent(driver,  lobbyPos[i]);
			driver.transform.position = lobbyPos[i].position;
			driver.transform.rotation =lobbyPos[i].rotation;
			//int tempid = Base64Manager.instance.GlobalEncoding(Global.MySponsorID);
			int tempid = GV.getTeamSponID(GV.SelectedTeamID);
			CrewTextureInit(driver,tempid);
		}
	}

	public void ChangeLobbyCrew(int idx, int crewid){
		if(idx == 0){
			if(lobbyPos[1].childCount == 0) return;
			for(int i = 0; i < 5; i++){
				var temp = lobbyPos[i].GetChild(0).gameObject as GameObject;
				listTeam.ForEach(obj => {
					if(temp == obj){
						obj.SetActive(false);
						obj.transform.parent = lobbyPos[i].parent;
					}
				}
				);
				CreateLobbyCrew(crewid, 0);
			}
		}else{
			/*if(lobbyPos_Tour[1].childCount == 0) return;
			for(int i = 0; i < 5; i++){
				var temp = lobbyPos_Tour[i].GetChild(0).gameObject as GameObject;
				listTeam_Tour.ForEach(obj => {
					if(temp == obj){
						obj.SetActive(false);
						obj.transform.parent = lobbyPos_Tour[i].parent;
					}
				}
				);
			}
			CreateLobbyCrew(0, crewid);*/
		}
	}


	public void ChangeLobbyCar(int idx, int carid, string carclass){
		GameObject temp = null;
		if(idx == 0){
		
			if(lobbyPos[5].childCount == 0) return;
			temp = lobbyPos[5].GetChild(0).gameObject;
			foreach(GameObject obj in listTeam){
				if(temp == obj){
					temp.SetActive(false);
					temp.transform.parent = lobbyPos[5].parent;
					break;
				}
			}
			var d = lobbyPos[0].GetChild(0) as Transform;
			CreateLobbyCar(carid, carclass);
			d.parent = lobbyPos[0].transform;
		}else{/*
			temp = lobbyPos_Tour[5].GetChild(0).gameObject;
			if(lobbyPos_Tour[5].childCount == 0) return;
			foreach(GameObject obj in listTeam_Tour){
				if(temp == obj){
					temp.SetActive(false);
					temp.transform.parent = lobbyPos[5].parent;
					break;
				}
			}
			var d1 = lobbyPos_Tour[0].GetChild(0) as Transform;
			CreateLobbyCar(carid, carclass);
			d1.parent = lobbyPos_Tour[0].transform;*/
		}
	}

	void CrewTextureInit(GameObject crew){
		var tx = crew.GetComponent<textureaction>() as textureaction;
		if(tx == null) tx = crew.AddComponent<textureaction>();
		tx.defaultCrewBodyTexture();
		tx.defaultCrewHeadTexture();
	}


	void CrewTextureInit(GameObject crew, int sponID){
		var tx = crew.GetComponent<textureaction>() as textureaction;
		if(tx == null) tx = crew.AddComponent<textureaction>();
		string[] crewID = crew.name.Split("_"[0]);
		tx.CrewBodyTextureInit(crewID[0],sponID.ToString());
		tx.CrewHeadTextureInit(sponID.ToString(), crewID[1]);
	}

	public void CrewEffect(string id){
		var car = Resources.Load("Effect_N/Upgrade_Crew", typeof(GameObject)) as GameObject;
		var race = Instantiate(car) as GameObject;
		int a = 0;
		switch(id){
		case "5100": // driver
			a = 0;
			break;
		case "5101": //tire
			a = 1;
			break;
		case "5102": //chief
			a = 2;
			break;
		case "5103": //jack
			a = 3;
			break;
		case"5104": //gas
				a = 4;
			break;
		}
		race.transform.parent = lobbyPos[a];
		race.transform.localPosition = Vector3.zero;
	}
}