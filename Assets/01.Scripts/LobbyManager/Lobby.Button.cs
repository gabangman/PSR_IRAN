using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public partial class LobbyManager : MonoBehaviour {
	void ChangeSponsorIcon(){
		var temp = activeObject.transform.FindChild("View").FindChild("Grid") as Transform;
		for(int i = 0; i < temp.childCount; i++){
			var obj = temp.GetChild(i).GetChild(0).gameObject as GameObject;
			if(obj.transform.FindChild("Select").gameObject.activeSelf){
				obj.transform.FindChild("Selected").gameObject.SetActive(true);
				obj.transform.FindChild("lbName").gameObject.SetActive(true);
				//string str =AccountManager.instance.SponTimeCheck();
				var tr = obj.transform.FindChild("lbName").gameObject as GameObject;
				if(tr.GetComponent<RunningSponTime>() == null){
					tr.AddComponent<RunningSponTime>().SponTimeLable(obj.transform.FindChild("lbName").GetComponent<UILabel>());
				}else{
					tr.GetComponent<RunningSponTime>().SponTimeLable(obj.transform.FindChild("lbName").GetComponent<UILabel>());
				}
				//obj.transform.FindChild("lbName").GetComponent<UILabel>().text =str;
			}else{
				obj.transform.FindChild("Selected").gameObject.SetActive(false);
				obj.transform.FindChild("lbName").gameObject.SetActive(false);
			}
		}
	//	if(raceinfo == null) return;
	//	raceinfo.SendMessage("CheckHaveSponsor",SendMessageOptions.DontRequireReceiver);
	}
	
	void ResetSponsorButton(){
		//Utility.LogWarning("ResetSponButton");
		/*return;
		var parent  = MenuBottom.transform.FindChild("Menu_Sponsor").gameObject as GameObject;
		var temp = parent.transform.FindChild("View").FindChild("Grid") as Transform;
		for(int i = 0; i < temp.childCount; i++){
			var obj = temp.GetChild(i).GetChild(0).gameObject as GameObject;
			obj.transform.FindChild("Selected").gameObject.SetActive(false);
			obj.transform.FindChild("lbName").gameObject.SetActive(false);
		}*/
	}
	/*
	void ChangeSeletedButton(int[] ID){
		var t = activeObject.transform.FindChild("View").FindChild("Grid") as Transform;
		t.FindChild(ID[0].ToString()).GetChild(0).FindChild("Image_selected").gameObject.SetActive(false);
		t.FindChild(ID[1].ToString()).GetChild(0).FindChild("Image_selected").gameObject.SetActive(true);
		SelectedTeamChange(ID);
	}
	*/
	
	void ChangeSeletedTeamCar(string strID){
		var t = activeObject.transform.FindChild("View").FindChild("Grid") as Transform;
	//	string str = GV.getTeamCarClass(GV.SelectedTeamID);
	//	t.FindChild(ID[0].ToString()+"_"+str).GetChild(0).FindChild("Image_selected").gameObject.SetActive(false);
	//	str = GV.getTeamCarClass(GV.SelectedTeamID);
	//	t.FindChild(ID[1].ToString()+"_"+str).GetChild(0).FindChild("Image_selected").gameObject.SetActive(true);
		Utility.LogWarning(strID);
		for(int i = 0 ; i < t.childCount;i++){
			t.GetChild(i).GetChild(0).FindChild("Image_selected").gameObject.SetActive(false);
		}
		t.FindChild(strID).GetChild(0).FindChild("Image_selected").gameObject.SetActive(true);

		int carid = GV.getTeamCarID(GV.SelectedTeamID);
		string strClass = GV.getTeamCarClass(GV.SelectedTeamID);
		_table.ChangeLobbyCar(0,carid,strClass);
		ChangeTeamLV("Stock",1);
		isTeamInfo = false;
		SelectedTeamChange(carid);
	}

	void ChangeSelectedTeamCarOnInven(){
		int carid = GV.getTeamCarID(GV.SelectedTeamID);
		string strClass = GV.getTeamCarClass(GV.SelectedTeamID);
		_table.ChangeLobbyCar(0,carid,strClass);
		ChangeTeamLV("Stock",1);
		isTeamInfo = false;
		SelectedTeamChange(carid);
	}



	void SetSelectedTeamCar(string strName){
		var t = activeObject.transform.FindChild("View").FindChild("Grid") as Transform;
	//	string str = GV.getTeamCarClass(GV.SelectedTeamID);
		for(int i = 0 ; i < t.childCount;i++){
			t.GetChild(i).GetChild(0).FindChild("Image_selected").gameObject.SetActive(false);
		}
		t.FindChild(strName).GetChild(0).FindChild("Image_selected").gameObject.SetActive(true);
	}

	bool isUpgradeCarPart = false;
	bool isUpgradeCrewPart = false;
	void SelectedTeamChange(int ID){
		if(ID < 1199){
			//_table.ResetLobbyCar();
			isCreateCarItem = false;
			isUpgradeCarPart = true;
		}else{
			//_table.ResetLobbyCrew();
			isCreateCrewItem = false;
			isUpgradeCrewPart = true;
		}
		isTeamInfo = false;
		//Utility.Log("id" + ID[0] + "2 " + ID[1]);
	}
	
	void SelectedTeamLVChange(){
		if(isUpgradeCarPart){
			ResetLevelButton(true);
			isUpgradeCarPart = false;
		}else if(isUpgradeCrewPart){
			ResetLevelButton(false);
			isUpgradeCrewPart = false;
		}/*
		if(!isCreateCarItem){
		
			Utility.Log("isCreateCarItem");
		}if(!isCreateCrewItem){
			ResetLevelButton(false);
			Utility.Log("isCreateCrewItem");
		}*/
	}
	
	IEnumerator CarAbilityCount(Transform tr, string _ability){
		int _ab = 0;
		int.TryParse(_ability, out _ab);
		int carA = Base64Manager.instance.GlobalEncoding(GV.CarAbility);
		if(carA == _ab) yield break;
		int count = carA;
		float delay1 = 1/(float)( _ab-count);
		var lbText = tr.FindChild("lbCar").GetComponent<UILabel>() as UILabel;//.text= string.Format("{0}",Global.gCarAbility);
		for(;;){
			count+=1;
			lbText.text = string.Format("{0}",count);
			if(_ab<= count){
				GV.CarAbility =  Base64Manager.instance.GlobalEncoding(_ab);
				lbText.text = string.Format("{0}",_ab);
				yield break;
			}
			yield return new WaitForSeconds(delay1);
		}		
	}
	IEnumerator CrewAbilityCount(Transform tr, string _ability){
		int _ab = 0;
		int.TryParse(_ability, out _ab);
		int crewA = Base64Manager.instance.GlobalEncoding(GV.CrewAbility);
		if(crewA == _ab) yield break;
		int count = crewA;
		float delay1 = 1/(float)( _ab-count);
		var lbText = tr.FindChild("lbCrew").GetComponent<UILabel>() as UILabel;//.text= string.Format("{0}",Global.gCarAbility);
		for(;;){
			count+=1;
			lbText.text = string.Format("{0}",count);
			if(_ab <= count){
				GV.CrewAbility = Base64Manager.instance.GlobalEncoding(_ab);
				lbText.text = string.Format("{0}",_ab);
				yield break;
			}
			yield return new WaitForSeconds(delay1);
		}		
	}
	

	void ChangemyCarTeamInfo(bool b){ // b : true initialize b : false successUpgrade
		Utility.LogWarning("ChangemyCarTeamInfo");
		return;
	//	CTeamAbility _ability = new CTeamAbility();
	//	int carid = Base64Manager.instance.GlobalEncoding(Global.MyCarID);
	//	string[] strRet = _ability.CarAbility(carid);
	//	myCarName = strRet[1];
	//	var ability = myFuel.transform.parent.FindChild("Ability") as Transform;
		/*if(b){
			int carA = 0;
			int.TryParse(strRet[0], out carA);
			Global.gCarAbility = Base64Manager.instance.GlobalEncoding(carA);
			ability.FindChild("lbCar").GetComponent<UILabel>().text= string.Format("{0}",carA);
			
			Common_Car_Status.Item _car = Common_Car_Status.Get (carid);
			ability.FindChild("carClass").GetComponent<UISprite>().spriteName =  "Class_"+_car.Class;
		}else{
			StartCoroutine(CarAbilityCount(ability, strRet[0]));
		
		}*/
	//	int carAA = 0;
	//	int.TryParse(strRet[0], out carAA);
	//	Global.gCarAbility = Base64Manager.instance.GlobalEncoding(carAA);
	}
	void ChangemyCrewTeamInfo(bool b){
		Utility.LogWarning("ChangemyCrewTeamInfo");
		return;/*
		CTeamAbility _ability = new CTeamAbility();
		int crewid = Base64Manager.instance.GlobalEncoding(Global.MyCrewID);
		string[] strRet = _ability.CrewAbility(crewid);
		myCrewName = strRet[1];
		var ability = myFuel.transform.parent.FindChild("Ability") as Transform;*/
		/*if(b) {
			int crewA = 0;
			int.TryParse(strRet[0], out crewA);
			Global.gCrewAbility = Base64Manager.instance.GlobalEncoding(crewA);
			ability.FindChild("lbCrew").GetComponent<UILabel>().text= string.Format("{0}", crewA);
			Common_Crew_Status.Item _car = Common_Crew_Status.Get (crewid);
			ability.FindChild("crewClass").GetComponent<UISprite>().spriteName =  "Class_"+_car.Class;
		}else{
			StartCoroutine(CrewAbilityCount(ability, strRet[0]));
		}*/
	/*	int crewAA = 0;
		int.TryParse(strRet[0], out crewAA);
		Global.gCrewAbility = Base64Manager.instance.GlobalEncoding(crewAA);*/
	}
	
	void ChangeTeamInfo(){
		var car = activeObject.transform.GetChild(0).FindChild("MyCar") as Transform;
		var crew = car.parent.FindChild("MyCrew") as Transform;
		var carStar = car.FindChild("Info_StarLV") as Transform;
		var carLV = car.FindChild("Info_PartLV") as Transform;
		var spon = car.parent.FindChild("MySponsor") as Transform;
		var crewLV = crew.FindChild("Info_PartLV") as Transform;
		int id = 0;
		// car
		id = GV.getTeamCarID(GV.SelectedTeamID);
		Common_Car_Status.Item _item = Common_Car_Status.Get(id);
		car.FindChild("icon").GetComponent<UISprite>().spriteName = id.ToString();
		car.FindChild("lbClass").GetComponent<UILabel>().text =
			string.Format(KoStorage.GetKorString("74024"),GV.getTeamCarClass(GV.SelectedTeamID));

	//	int f1 = _item.P_Control_1t;
		string pCon = string.Format(KoStorage.GetKorString("76302"), GV.getClassIDInTeamCar(id,GV.SelectedTeamID).P_Control);
	/*	if(f1 < 151){
			pCon = KoStorage.getStringDic("76305");
		}else if(f1 < 321){pCon = KoStorage.getStringDic("76304");
		}else if(f1 < 501){pCon = KoStorage.getStringDic("76303");
		}else if(f1 < 760){pCon = KoStorage.getStringDic("76302");
		}*/


		car.FindChild("lbControl").GetComponent<UILabel>().text = pCon;
		//int a = GV.getClassIDInTeamCar(id, GV.SelectedTeamID).UpLimit;
		car.FindChild("lbLVtitle").GetComponent<UILabel>().text = KoStorage.GetKorString("72018");
	
	
		car.FindChild("lbUpGear").GetComponent<UILabel>().text =  string.Format(KoStorage.GetKorString("74028"),GV.getClassIDInTeamCar(id, GV.SelectedTeamID).StarLV); //진화 최고 레벨


		CarInfo carInfo = GV.getTeamCarInfo(GV.SelectedTeamID);
		car.FindChild("lbUpLimit").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74029"),_item.GearLmt+carInfo.carClass.GearLmt); //최대 기어 
		UILabel[] lbs = carLV.GetComponentsInChildren<UILabel>();
		lbs[0].text = carInfo.bodyLv.ToString();
		lbs[1].text = carInfo.engineLv.ToString();
		lbs[2].text = carInfo.tireLv.ToString();
		lbs[3].text = carInfo.gearBoxLv.ToString();
		lbs[4].text = carInfo.intakeLv.ToString();
		lbs[5].text = carInfo.bsPowerLv.ToString();
		lbs[6].text = carInfo.bsTimeLv.ToString();
		lbs = null;
		lbs = carStar.GetComponentsInChildren<UILabel>();
		lbs[0].text = (carInfo.bodyStar+1).ToString();
		lbs[1].text =(carInfo.engineStar+1).ToString();
		lbs[2].text =(carInfo.tireStar+1).ToString();
		lbs[3].text =(carInfo.gearBoxStar+1).ToString();
		lbs[4].text = (carInfo.intakeStar+1).ToString();
		lbs[5].text =(carInfo.bsPowerStar+1).ToString();
		lbs[6].text = (carInfo.bsTimeStar+1).ToString();

		var dura = car.FindChild("Durability").gameObject as GameObject;
		int cardu = 0;
		int carduRef = carInfo.carClass.DurabilityRef;

		if(carInfo.carClass.Durability <= 0) cardu = 0;
		else cardu = carInfo.carClass.Durability;


		if(carduRef == 0){
			carduRef = 100;
			cardu = 100;
		}
		dura.transform.GetComponent<stateUp>().ChangeBarDurability((float)cardu,  (float)carduRef);
		dura.transform.FindChild("lbStatName1").GetComponent<UILabel>().text = KoStorage.GetKorString("76020"); 


		//crew
		id = GV.getTeamCrewID(GV.SelectedTeamID);
		//crew.FindChild("lbName").GetComponent<UILabel>().text = Common_Crew_Status.Get(id).Name;
		crew.FindChild("icon").GetComponent<UISprite>().spriteName =  id.ToString();
	//	crew.FindChild("lbPower").GetComponent<UILabel>().text = string.Format("[ff5400] {0} [-]",  GV.CrewAbility);
		//crew.FindChild("lbClass").GetComponent<UILabel>().text = 
		//	string.Format(KoStorage.GetKorString("76002"),Common_Crew_Status.Get(id).Class);//KoStorage.getStringDic("60080")+" "+ Common_Crew_Status.Get(id).Class;//.Class;

		Common_Crew_Status.Item item = Common_Crew_Status.Get(id);

	//	crew.FindChild("lbStats").GetComponent<UILabel>().text = 
	//		string.Format(KoStorage.GetKorString("72021"), item.Name, item.ReqLV, item.UpLimit);
	//	crew.FindChild("lbReqSeason").GetComponent<UILabel>().text = 
	//		string.Format("요구시즌? :{0}",item.ReqLV);
	//	crew.FindChild("lbUpLimit").GetComponent<UILabel>().text = 
	//		string.Format("최대 강화단계?:{0}",item.UpLimit);
		//crew.FindChild("lbClass").GetComponent<UILabel>().text = string.Format(KoStorage.getStringDic("76002"),item.Class);//.Class;
		crew.FindChild("lbName").GetComponent<UILabel>().text = item.Name;
		crew.FindChild("lbReqSeason").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74027"), item.ReqLV);
		crew.FindChild("lbLVtitle").GetComponent<UILabel>().text =KoStorage.GetKorString("72018");
		CrewInfo crewinfo = GV.getTeamCrewInfo(GV.SelectedTeamID);
		lbs = null;
		lbs =crewLV.GetComponentsInChildren<UILabel>();
		lbs[0].text = (crewinfo.driverLv).ToString();
		lbs[1].text =(crewinfo.tireLv).ToString();
		lbs[2].text =(crewinfo.chiefLv).ToString();
		lbs[3].text =(crewinfo.gasLv).ToString();
		lbs[4].text = (crewinfo.gasLv).ToString();
		lbs = null;
		car = null;crew = null; carStar = null; carLV = null;crewLV=null;
		//spon
		id = GV.getTeamSponID(GV.SelectedTeamID);
		if(id > 1300){
			spon.FindChild("lbName").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("72019"), Common_Sponsor_Status.Get(id).Name);
			spon.FindChild("icon").gameObject.SetActive(true);
			spon.FindChild("icon").GetComponent<UISprite>().spriteName =  id.ToString();
			spon.FindChild("lbSponTime").gameObject.SetActive(true);
		}else{
			spon.FindChild("icon").gameObject.SetActive(false);
			spon.FindChild("lbName").GetComponent<UILabel>().text =  string.Format(KoStorage.GetKorString("72020"));
			spon.FindChild("lbSponTime").gameObject.SetActive(false);
		}
	//ChangemyCarTeamInfo(true);
	//	ChangemyCrewTeamInfo(true);

	}

	void RankingBoardArrowChecking(bool b){
		rankObj.SendMessage("SetDurabilityArrow", b, SendMessageOptions.DontRequireReceiver);
	}
	
	void ChangeLevel(int count, string name, GameObject obj, int max, int a, int maxStarLV, int curStarLV){
		int mCount = 0;
		mCount = (count % 5);
		int mCnt = (count / 5);
		if(mCount == 0){
			if(mCnt == maxStarLV){
				mCount = 5;	
			}else if(mCnt == curStarLV){
				mCount  = 5;
			}else mCount = 0;
		}

		var temp = obj.transform.FindChild(name).FindChild("UpgradeLED") as Transform;
		var child = temp.parent.FindChild("upgrade_icon") as Transform;
	//	var childname = temp.parent.FindChild("lbName") as Transform;
		if(a == 0) {
			temp.gameObject.SetActive(false);
			child.GetComponent<UISprite>().color = Color.black;
	//		childname.GetComponent<UILabel>().color = Color.gray;
			return;
		}
		else {
			temp.gameObject.SetActive(true);
			child.GetComponent<UISprite>().color = Color.white;
			//childname.GetComponent<UILabel>().color = Color.white;
		}
		for(int i=0; i < temp.childCount-1; i++){
			if(i < max){
				temp.GetChild(i).gameObject.SetActive(true);
			}else{
				temp.GetChild(i).gameObject.SetActive(false);
			}
		}
		for(int i =0; i < mCount;i++){
			var _parent = temp.GetChild(i).FindChild("LED_on").gameObject as GameObject;//.SetActive(true);
			_parent.SetActive(true);
		}
	}
	
	void ChangeDurabiltiyStatus(int durability, string name, GameObject obj){
		/*return;

		var temp = obj.transform.FindChild(name).FindChild("Progress Bar") as Transform;
		temp.GetChild(1).GetComponent<UISprite>().fillAmount = durability*0.01f;
		//Utility.Log(name + " + " + durability);
		if(durability >= 51){
			temp.GetChild(1).GetComponent<UISprite>().color = Color.cyan;
		}else if(durability > 25){
			temp.GetChild(1).GetComponent<UISprite>().color = Color.yellow;
		}else{
			temp.GetChild(1).GetComponent<UISprite>().color = Color.red;
		}*/
	}
	
	void ChangeStarLevel(int starCnt, string name, GameObject obj, int starMax, int maxCnt){
	//	var temp1 = obj.transform.FindChild(name).FindChild("StarCount") as Transform;
	//	if(temp1.gameObject.activeSelf){
	//		temp1.gameObject.SetActive(false);
	//	}
		//return;
		starCnt+=1;
		var temp = obj.transform.FindChild(name).FindChild("StarCount") as Transform;
		int childcnt = temp.childCount;
		for(int i = 0; i < childcnt; i++){
			if(i < maxCnt){
				temp.GetChild(i).gameObject.SetActive(true);
			}else{
				temp.GetChild(i).gameObject.SetActive(false);
			}
		}
		for(int i = 0; i < maxCnt; i++){
			if(i < starCnt){
				temp.GetChild(i).FindChild("Star_on").gameObject.SetActive(true);
			}else{
				temp.GetChild(i).FindChild("Star_on").gameObject.SetActive(false);
			}
		}
	}
	
	void ResetLevelButton(bool b){
		//var temp = obj.transform.FindChild(name) as Transform; Progress Bar
		if(b){
			var uplv = MenuBottom.transform.FindChild("Menu_Upgrade_Car").gameObject as GameObject;
			for(int i = 0; i < uplv.transform.childCount; i++){
				var temp = uplv.transform.GetChild(i) as Transform;
				var temp1 = temp.FindChild("UpgradeLED") as Transform;
				for(int j = 0; j < temp1.childCount-1;j++){
					temp1.GetChild(j).FindChild("LED_on").gameObject.SetActive(false);
				}
				temp1 = temp.FindChild("StarCount");
				for(int j = 0; j < temp1.childCount;j++){
					temp1.GetChild(j).FindChild("Star_on").gameObject.SetActive(false);
				}
			}
			ChangeLevelCarButton(true);
		}else{
			var uplevel = MenuBottom.transform.FindChild("Menu_Upgrade_Crew").gameObject as GameObject;
			for(int i = 0; i < uplevel.transform.childCount; i++){
				var temp = uplevel.transform.GetChild(i) as Transform;
				var temp1 = temp.FindChild("UpgradeLED") as Transform;
				for(int j = 0; j < temp1.childCount-1;j++){
					temp1.GetChild(j).FindChild("LED_on").gameObject.SetActive(false);
				}
				/*temp1 = temp.FindChild("StarCount");
				for(int j = 0; j < temp1.childCount;j++){
					temp1.GetChild(j).FindChild("Star_on").gameObject.SetActive(false);
				}*/
			}
			ChangeLevelCrewButton(true);
		}
	}

	void ChangeLevelCarButton(bool b){
		//Utility.Log(CarUpNameStock);
		int count = 0, max = 0;
		if(b){
			var activeObject1 = MenuBottom.transform.FindChild("Menu_Upgrade_Car").gameObject as GameObject;
			int tmepid = 0;
			CarInfo _car ;
		//	int starcnt = 0;
			string upname = string.Empty;
			if(GV.SelectedTeamID%2 ==0) {
				upname = CarUpNameStock;
			}
			else {
				upname = CarUpNameTour;
			}
			tmepid = GV.getTeamCarID(GV.SelectedTeamID);
			_car = GV.getTeamCarInfo(GV.SelectedTeamID);
			Common_Class.Item cItem = GV.getClassIDInTeamCar(tmepid, GV.SelectedTeamID);
			Common_Car_Status.Item _item = Common_Car_Status.Get(tmepid);
			
			count = _car.bodyLv;//+_car.bodyStar;
			max = cItem.UpLimit;
			//max = cItem.UpLimit+_car.bodyStar;
			ChangeLevel(count, "Body", activeObject1,max, _item.BodyAble,cItem.StarLV, (_car.bodyStar+1));
			ChangeStarLevel(_car.bodyStar,"Body", activeObject1, cItem.StarLV, cItem.StarLV);
			//ChangeDurabiltiyStatus(_car.bodydurability, "Body", activeObject1);
			count = _car.engineLv;//+_car.engineStar;
			//max = cItem.UpLimit + _car.engineStar;
		
			ChangeLevel(count, "Engine", activeObject1,max, _item.EngineAble,cItem.StarLV, (_car.engineStar+1));
			ChangeStarLevel(_car.engineStar,"Engine", activeObject1, cItem.StarLV, cItem.StarLV);
			//ChangeDurabiltiyStatus(_car.enginedurability, "Engine", activeObject1);
			
			count = _car.gearBoxLv;//+_car.gearBoxStar;
			//max = cItem.UpLimit + _car.gearBoxStar;

			ChangeLevel(count, "Gearbox", activeObject1,max, _item.GBoxAble,cItem.StarLV, (_car.gearBoxStar+1));
			ChangeStarLevel(_car.gearBoxStar,"Gearbox", activeObject1, cItem.StarLV, cItem.StarLV);
			//ChangeDurabiltiyStatus(_car.gearBoxdurability, "Gearbox", activeObject1);
			
			count = _car.intakeLv;//+_car.intakeStar;
			//max = cItem.UpLimit + _car.intakeStar;
		
			ChangeLevel(count, "Intake", activeObject1,max, _item.IntakeAble,cItem.StarLV, (_car.intakeStar+1));
			ChangeStarLevel(_car.intakeStar,"Intake", activeObject1, cItem.StarLV, cItem.StarLV);
			//ChangeDurabiltiyStatus(_car.intakedurability, "Intake", activeObject1);
			
			count = _car.bsPowerLv;//+_car.bsPowerStar;
			//max = cItem.UpLimit + _car.bsPowerStar;
		

			ChangeLevel(count, "N2O", activeObject1,max, _item.BsPowerAble,cItem.StarLV, (_car.bsPowerStar+1));
			ChangeStarLevel(_car.bsPowerStar,"N2O", activeObject1, cItem.StarLV, cItem.StarLV);
			//ChangeDurabiltiyStatus(_car.bsPowerdurability, "N2O", activeObject1);
			count = _car.bsTimeLv;//+_car.bsTimeStar;
		//	max = cItem.UpLimit + _car.bsTimeStar;
		
			ChangeLevel(count, "Brake", activeObject1,max, _item.BsTimeAble,cItem.StarLV, (_car.bsTimeStar+1));
			ChangeStarLevel(_car.bsTimeStar,"Brake", activeObject1, cItem.StarLV, cItem.StarLV);
			//ChangeDurabiltiyStatus(_car.bsTimedurability, "Brake", activeObject1);
			
			count = _car.tireLv;//+_car.tireStar;
		//	max = cItem.UpLimit + _car.tireStar;
		
		//	Utility.LogWarning(string.Format("{0}, {1}", cItem.StarLV, _car.tireStar+1));
			ChangeLevel(count, "Tires", activeObject1,max, _item.TireAble,cItem.StarLV, (_car.tireStar+1));
			ChangeStarLevel(_car.tireStar,"Tires", activeObject1, cItem.StarLV, cItem.StarLV);
			//ChangeDurabiltiyStatus(_car.tiredurability, "Tires", activeObject1);
		}else{
			ChangeUpgradeCarLevel();
			isTeamInfo = false;
		}
		//ChangemyCarTeamInfo(b);
	}
	
	void ChangeLevelCrewButton(bool b){
		int count = 0, max = 0;
		if(b){
			int tmepid = 0;
			CrewInfo _crew ; 
			string upname = string.Empty;
			if(GV.SelectedTeamID%2 ==0){
				upname = CrewUpNameStock;
			}
			else {
				upname = CrewUpNameTour;
			}

			tmepid = GV.getTeamCrewID(GV.SelectedTeamID);
			_crew = GV.getTeamCrewInfo(GV.SelectedTeamID);
			Common_Crew_Status.Item _item = Common_Crew_Status.Get(tmepid);
			var activeObject1 = MenuBottom.transform.FindChild("Menu_Upgrade_Crew").gameObject as GameObject;
			count = _crew.chiefLv;
			max = _item.UpLimit + _crew.chiefStar;
			ChangeLevel(count, "Chief", activeObject1,max, 1, 1,_crew.chiefStar+1 );
			ChangeStarLevel(_crew.chiefStar,"Chief", activeObject1, _item.StarLV, _item.StarLV);
			count = _crew.driverLv;
			max = _item.UpLimit + _crew.driverStar;
			ChangeLevel(count, "Driver", activeObject1,max, 1, 1,_crew.driverStar+1);
			ChangeStarLevel(_crew.driverStar,"Driver", activeObject1, _item.StarLV, _item.StarLV);
			count = _crew.gasLv;
			max = _item.UpLimit + _crew.gasStar;
			ChangeLevel(count, "Gas", activeObject1,max, 1, 1,_crew.gasStar+1);
			ChangeStarLevel(_crew.gasStar,"Gas", activeObject1, _item.StarLV, _item.StarLV);
			count = _crew.jackLv;
			max = _item.UpLimit + _crew.jackStar;
			ChangeLevel(count, "Jack", activeObject1,max,1 , 1,_crew.jackStar+1);
			ChangeStarLevel(_crew.jackStar,"Jack", activeObject1, _item.StarLV, _item.StarLV);
			count = _crew.tireLv;
			max = _item.UpLimit + _crew.tireStar;
			ChangeLevel(count, "Tire", activeObject1,max,1, 1,_crew.tireStar+1);
			ChangeStarLevel(_crew.tireStar,"Tire", activeObject1, _item.StarLV, _item.StarLV);
		}else{
			ChangeUpgradeCrewLevel();
		}
	}

	void ChangeUpgradeCarLevel(){
		int count = 0; int max = 0;
		int tmepid = 0;//Base64Manager.instance.GlobalEncoding(Global.MyCarID);
		CarInfo _car ;
		int starcnt = 0;
		string upname = string.Empty;
		if(GV.SelectedTeamID%2==0) {
			upname = CarUpNameStock;
		}
		else {
			upname = CarUpNameTour;
		}
		//myTeamInfo myTeam = GV.getTeamTeamInfo(GV.SelectedTeamID);
		tmepid = GV.getTeamCarID(GV.SelectedTeamID);
		_car = GV.getTeamCarInfo(GV.SelectedTeamID);
		Common_Class.Item cItem = GV.getClassIDInTeamCar(tmepid, GV.SelectedTeamID);
		switch(upname){
		case "Body":
			count = _car.bodyLv;//+_car.bodyStar;
			max = cItem.UpLimit+_car.bodyStar;
			starcnt = _car.bodyStar;
			break;
		case "Gearbox":
			count = _car.gearBoxLv;//+_car.gearBoxStar;
			max = cItem.UpLimit + _car.gearBoxStar;
			starcnt = _car.gearBoxStar;
			break;
		case "Engine":
			count = _car.engineLv;//+_car.engineStar;
			max = cItem.UpLimit + _car.engineStar;
			starcnt = _car.engineStar;
			break;
		case "Intake":
			count = _car.intakeLv;//+_car.intakeStar;
			max = cItem.UpLimit + _car.intakeStar;
			starcnt = _car.intakeStar;
			break;
		case "N2O":
			count = _car.bsPowerLv;//+_car.bsPowerStar;
			max = cItem.UpLimit + _car.bsPowerStar;
			starcnt = _car.bsPowerStar;
			break;
		case "Brake":
			count = _car.bsTimeLv;//+_car.bsTimeStar;
			max = cItem.UpLimit + _car.bsTimeStar;
			starcnt = _car.bsTimeStar;
			break;
		case "Tires":
			count = _car.tireLv;//+_car.tireStar;
			max = cItem.UpLimit + _car.tireStar;
			starcnt = _car.tireStar;
			break;
		default:
			Utility.LogWarning("null");
			break;
		}
		//Utility.LogWarning("max "  + max);
		max = cItem.UpLimit;
		var activeObject1 = MenuBottom.transform.FindChild("Menu_Upgrade_Car").gameObject as GameObject;
	//	bool b = true;
	//	if(count/5 == cItem.StarLV) b = true;
	//	else b = false ;
		LevelUpgradeButton( count, max, upname,activeObject1,cItem.StarLV, starcnt+1);
		StartUpgradeButton( starcnt, cItem.StarLV, upname, activeObject1);
	}
	void ChangeUpgradeCrewLevel(){
		int count = 0; int max = 0;
		//int tmepid = Base64Manager.instance.GlobalEncoding(Global.MyCrewID);
		//Account.CrewInfo _crew = myAccount.instance.GetCrewInfo(tmepid);
		//Common_Crew_Status.Item _item = Common_Crew_Status.Get(_crew.crewId);
		int starCnt = 0;
		int tmepid = 0;
		CrewInfo _crew ; 
		string upname = string.Empty;
		if(GV.SelectedTeamID%2==0){
			upname = CrewUpNameStock;
	//		tmepid = GV.FindStockCrew();
	//		_crew = GV.GetStockCrewInfo();
		}
		else {
			upname = CrewUpNameTour;
	//		tmepid = GV.FindTourCrew();
	//		_crew = GV.GetTourCrewInfo();
		}
		tmepid = GV.getTeamCrewID(GV.SelectedTeamID);
		_crew = GV.getTeamCrewInfo(GV.SelectedTeamID);
		Common_Crew_Status.Item _item = Common_Crew_Status.Get(tmepid);
		switch(upname){
		case "Chief":
			count = _crew.chiefLv;
			max = _item.UpLimit + _crew.chiefStar;
			starCnt = _crew.chiefStar;
			break;
		case "Driver":
			count = _crew.driverLv;
			max = _item.UpLimit+ _crew.driverStar;
			starCnt = _crew.driverStar;
			break;
		case "Gas":
			count = _crew.gasLv;
			max = _item.UpLimit+ _crew.gasStar;
			starCnt = _crew.gasStar;
			break;
		case "Jack":
			count = _crew.jackLv;
			max = _item.UpLimit + _crew.jackStar;
			starCnt = _crew.jackStar;
			break;
		case "Tire":
			count = _crew.tireLv;
			max = _item.UpLimit + _crew.tireStar;
			starCnt = _crew.tireStar;
			break;
		default:
			Utility.LogWarning("null");
			break;
		}
		if(upname == null) return;
		var activeObject1 = MenuBottom.transform.FindChild("Menu_Upgrade_Crew").gameObject as GameObject;
		LevelUpgradeButton( count, max, upname,activeObject1, 1, starCnt+1);
		StartUpgradeButton( starCnt, _item.StarLV, upname, activeObject1);
	}

	void LevelUpgradeButton(int count, int max, string name,GameObject obj, int maxStarLV, int curStarLV){
		int mCnt = count %5;
	
		var temp = obj.transform.FindChild(name).FindChild("UpgradeLED") as Transform;
		//int cnt = temp.childCount;
		if(mCnt == 0){
			int mCount = count /5;
			if(mCount == maxStarLV){
				mCnt = 5;
			}else if(mCount == curStarLV){
				mCnt = 5;
			}else mCnt = 0;

			for(int j = 0; j < temp.childCount-1;j++){
				temp.GetChild(j).FindChild("LED_on").gameObject.SetActive(false);
			}
		}
		if(mCnt == 1){
			for(int j = 0; j < temp.childCount-1;j++){
				temp.GetChild(j).FindChild("LED_on").gameObject.SetActive(false);
			}
		}
	
		var effect = temp.FindChild("Effect").gameObject as GameObject;
		for(int i =0; i < mCnt;i++){
			var _parent = temp.GetChild(i).FindChild("LED_on").gameObject as GameObject;//.SetActive(true);
			if(!_parent.activeSelf){
				effect.transform.localPosition = _parent.transform.parent.transform.localPosition;
				effect.SetActive(true);
				_parent.AddComponent<LevelEffect>().levelEffect();
				_parent.SetActive(true);
			}
		}
	}
	
	void StartUpgradeButton(int starcnt, int starMax, string name, GameObject obj){
		starcnt += 1;
		var temp = obj.transform.FindChild(name).FindChild("StarCount") as Transform;
		int cnt = temp.childCount;
		for(int i=0; i <starMax; i++){
			if(i < starcnt){
				temp.GetChild(i).FindChild("Star_on").gameObject.SetActive(true);
			}else{
				temp.GetChild(i).FindChild("Star_on").gameObject.SetActive(false);
			}
			//Utility.Log(temp.GetChild(i).name);
		}
	}

	void DisableTeamCarNewbtn(){
		//menuCenter.teamCarReset();
	}

	void DisableTeamCrewNewbtn(){
		//menuCenter.teamCrewReset();
	}



	/*
	public void OnGoldClick(){
		var temp = activeObject.GetComponent<ContainerAction>() as ContainerAction;
		if(temp.OnGoldClick()){
		
		}else{
			if(raceinfo != null) HiddenInfoWindow();

		}
	}
	
	public void OnSilverClick(){
		var temp = activeObject.GetComponent<ContainerAction>() as ContainerAction;
		if(temp.OnSilverClick()){
			
		}else{
			if(raceinfo != null) HiddenInfoWindow();
		}

	}
	
	public void OnBronzeClick(){
		var temp = activeObject.GetComponent<ContainerAction>() as ContainerAction;
		if(temp.OnBronzeClick()){
			
		}else{
			if(raceinfo != null) HiddenInfoWindow();
		}

	}



	void upgradeDollarTest(){
		Common_Car_Status.Item _carstatus =  Common_Car_Status.Get (1017);
		int CONVERTER_DOLLAR = Global.gConvertDollar;
		for(int i =1; i < 7; i++){
			float levelupcoin = (_carstatus.EngineLV1*Upgrade_Car_Ratio.Get((int)CarPartID.Engine).UpRatio*0.01f);
			levelupcoin = _carstatus.EngineLV1+(i-1)*levelupcoin;
			int coin = (int)Mathf.Round(levelupcoin);
			int dollar = coin*CONVERTER_DOLLAR;
			string str = string.Format("CarID : {0}, Engine, PartLV: {1}, leveupCoin : {2}, upgradeCoin : {3}, upgradeDollar:{4}",
			                           1017, i, levelupcoin, coin, dollar);
			Utility.Log(str);
		}
		for(int i =1; i < 7; i++){
			float levelupcoin = (_carstatus.BodyLV1*Upgrade_Car_Ratio.Get((int)CarPartID.Body).UpRatio*0.01f);
			levelupcoin = _carstatus.BodyLV1+(i-1)*levelupcoin;
			int coin = (int)Mathf.Round(levelupcoin);
			int dollar = coin*CONVERTER_DOLLAR;
			string str = string.Format("CarID : {0}, Body, PartLV: {1}, leveupCoin : {2}, upgradeCoin : {3}, upgradeDollar:{4}",
			                           1017, i, levelupcoin, coin, dollar);Utility.Log(str);
		}
		for(int i =1; i < 7; i++){
			float levelupcoin = (_carstatus.IntakeLV1*Upgrade_Car_Ratio.Get((int)CarPartID.Intake).UpRatio*0.01f);
			levelupcoin = _carstatus.IntakeLV1+(i-1)*levelupcoin;
			int coin = (int)Mathf.Round(levelupcoin);
			int dollar = coin*CONVERTER_DOLLAR;
			string str = string.Format("CarID : {0}, IntakeLV1, PartLV: {1}, leveupCoin : {2}, upgradeCoin : {3}, upgradeDollar:{4}",
			                           1017, i, levelupcoin, coin, dollar);Utility.Log(str);
		}
		for(int i =1; i < 7; i++){
			float levelupcoin = (_carstatus.GBoxLV1*Upgrade_Car_Ratio.Get((int)CarPartID.Gearbox).UpRatio*0.01f);
			levelupcoin = _carstatus.GBoxLV1+(i-1)*levelupcoin;
			int coin = (int)Mathf.Round(levelupcoin);
			int dollar = coin*CONVERTER_DOLLAR;
			string str = string.Format("CarID : {0}, GBoxLV1, PartLV: {1}, leveupCoin : {2}, upgradeCoin : {3}, upgradeDollar:{4}",
			                           1017, i, levelupcoin, coin, dollar);Utility.Log(str);
		}
		for(int i =1; i < 7; i++){
			float levelupcoin = (_carstatus.TireLV1*Upgrade_Car_Ratio.Get((int)CarPartID.Tire).UpRatio*0.01f);
			levelupcoin = _carstatus.TireLV1+(i-1)*levelupcoin;
			int coin = (int)Mathf.Round(levelupcoin);
			int dollar = coin*CONVERTER_DOLLAR;
			string str = string.Format("CarID : {0}, TireLV1, PartLV: {1}, leveupCoin : {2}, upgradeCoin : {3}, upgradeDollar:{4}",
			                           1017, i, levelupcoin, coin, dollar);Utility.Log(str);
		}
		for(int i =1; i < 7; i++){
			float levelupcoin = (_carstatus.BsTimeLV1*Upgrade_Car_Ratio.Get((int)CarPartID.BsTime).UpRatio*0.01f);
			levelupcoin = _carstatus.BsTimeLV1+(i-1)*levelupcoin;
			int coin = (int)Mathf.Round(levelupcoin);
			int dollar = coin*CONVERTER_DOLLAR;
			string str = string.Format("CarID : {0}, TireLV1, PartLV: {1}, leveupCoin : {2}, upgradeCoin : {3}, upgradeDollar:{4}",
			                           1017, i, levelupcoin, coin, dollar);Utility.Log(str);
		}
		for(int i =1; i < 7; i++){
			float levelupcoin = (_carstatus.BsPowerLV1*Upgrade_Car_Ratio.Get((int)CarPartID.BsPower).UpRatio*0.01f);
			levelupcoin = _carstatus.BsPowerLV1+(i-1)*levelupcoin;
			int coin = (int)Mathf.Round(levelupcoin);
			int dollar = coin*CONVERTER_DOLLAR;
			string str = string.Format("CarID : {0}, BsPowerLV1, PartLV: {1}, leveupCoin : {2}, upgradeCoin : {3}, upgradeDollar:{4}",
			                           1017, i, levelupcoin, coin, dollar);Utility.Log(str);
		}
		
	}*/
}
