using UnityEngine;
using System.Collections;

public class myteambtnaction : InterAction {


	public override void OnCarItemClick(){
		if(Global.isAnimation) return;
		var tmp = transform.FindChild("Empty").gameObject;
		if(tmp.activeSelf) return;
		tmp = transform.FindChild("Select").gameObject;
		int id = int.Parse(gameObject.name);
		GameObject obj = GameObject.Find("LobbyUI");
		if(id < 1200){
			bool isNew =false;
			if(!isNew) {}
			else{
				isNew = false;
				transform.FindChild("icon_New").gameObject.SetActive(isNew);
				obj.SendMessage("DisableTeamCarNewbtn",SendMessageOptions.DontRequireReceiver);
			}
		}
		else{ 
			bool isNew = false;
			if(!isNew) {

			}else{
				isNew = false;
				transform.FindChild("icon_New").gameObject.SetActive(isNew);
				obj.SendMessage("DisableTeamCrewNewbtn",SendMessageOptions.DontRequireReceiver);
			}

		}
		if(tmp.activeSelf) return;

		if(id < 1200){
			obj.SendMessage("OnCarClick", gameObject, SendMessageOptions.DontRequireReceiver);
		}else{ 
			obj.SendMessage("OnCrewClick", gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}

	void checkNewButton(){
	
	}


	public void CarButtonDisable(){
		transform.FindChild("Empty").gameObject.SetActive(true);
		transform.FindChild("Select").gameObject.SetActive(false);
		var t =transform.FindChild("Image").gameObject as GameObject;
		t.SetActive(false);
		ObjectSetActive(gameObject, "ClassPanel", false);
		ObjectSetActive(gameObject, "lbClass", false);
		ObjectSetActive(gameObject, "ClassColor", false);
		ObjectSetActive(gameObject, "lbClass_1",false);
	}

	public void CarButtonEnable(){
		ObjectSetActive(gameObject, "ClassPanel", true);
		ObjectSetActive(gameObject, "lbClass", true);
		ObjectSetActive(gameObject, "ClassColor", true);
		ObjectSetActive(gameObject, "lbClass_1",true);
	}
	public void CarButtonNew(){
	
	}
	public void CrewButtonNew(){
	
	}
	public delegate void GridButtonInit();
	GridButtonInit _buttoninit;

	public void CarButtonSet(bool b){
		_buttoninit = b? new GridButtonInit(CarButton):new GridButtonInit(ShopButton) ;
		_buttoninit();
	}





	void CarButton(){
		string name = gameObject.name;
		int _id = int.Parse(name);
		ButtonInit(_id);	
		var t = transform.FindChild("Image").gameObject as GameObject;
		t.SetActive(true);
		t.GetComponent<UISprite>().spriteName = gameObject.name;
	}

	void ButtonInit(int _id){
		Common_Car_Status.Item _car = Common_Car_Status.Get (_id);
		transform.FindChild("Empty").gameObject.SetActive(false);
		transform.FindChild("lbClass").GetComponent<UILabel>().text = _car.Name;
		transform.FindChild("ClassColor").GetComponent<UISprite>().spriteName = "Class_"+_car.Class;

		int mSeason = GV.ChSeasonID;//Base64Manager.instance.GlobalEncoding(Global.ChampionSeason);
		int mSLV = 0;
		if(mSeason >= 6030){
			Common_Mode_Champion.Item item= Common_Mode_Champion.Get(mSeason);
			mSLV = item.Season;
		}else{
			mSLV = GV.ChSeason;
		}
		if(mSLV < _car.ReqLV){
			transform.FindChild("Image").GetComponent<UISprite>().color = Color.black;
			transform.FindChild("Image_Lock").gameObject.SetActive(true);
		} 
	}

	public void CarbuttonSetting(){
		string name = gameObject.name;
		int _id = int.Parse(name);
		var t = transform.FindChild("Image").gameObject as GameObject;
		t.SetActive(true);
		t.GetComponent<UISprite>().spriteName = gameObject.name;
		Common_Car_Status.Item _car = Common_Car_Status.Get (_id);
		transform.FindChild("Empty").gameObject.SetActive(false);
		transform.FindChild("lbClass").GetComponent<UILabel>().text = _car.Name;
		transform.FindChild("ClassColor").GetComponent<UISprite>().spriteName = "Class_"+_car.Class;
		transform.FindChild("lbClass_1").GetComponent<UILabel>().text = GV.getTeamCarClass(GV.SelectedTeamID);
	}
	public void CrewButtonSet(){
		string[] name = gameObject.name.Split("_"[0]);
		int _id = int.Parse(name[0]);
		Common_Crew_Status.Item _car = Common_Crew_Status.Get (_id);
		transform.FindChild("lbClass").GetComponent<UILabel>().text = _car.Name;
		transform.FindChild("ClassColor").GetComponent<UISprite>().spriteName = "Class_"+_car.Class;
		int mSeason = GV.ChSeasonID;//Base64Manager.instance.GlobalEncoding(Global.ChampionSeason);
		int mSLV = 0;
		if(mSeason >= 6030){
			Common_Mode_Champion.Item item= Common_Mode_Champion.Get(mSeason);
			mSLV = item.Season;
		}else{
			mSLV = GV.ChSeason;
		}
		if(mSLV < _car.ReqLV){
			transform.FindChild("Image").GetComponent<UISprite>().color = Color.black;
			transform.FindChild("Image_Lock").gameObject.SetActive(true);
		} 

	}

	public void CrewButtonSetting(){
		string[] name = gameObject.name.Split("_"[0]);
		int _id = int.Parse(name[0]);
		ObjectSetActive(gameObject, "ClassPanel", true);
		ObjectSetActive(gameObject, "lbClass", true);
		ObjectSetActive(gameObject, "ClassColor", true);
		Common_Crew_Status.Item _car = Common_Crew_Status.Get (_id);
		//transform.FindChild("lbClass").GetComponent<UILabel>().text = KoStorage.getStringDic("60080")+" "+_car.Class;//"Class "+_car.Class;
		transform.FindChild("lbClass").GetComponent<UILabel>().text = _car.Name;
		transform.FindChild("ClassColor").GetComponent<UISprite>().spriteName = "Class_"+_car.Class;
	}

	void ShopButton(){
		string[] name = gameObject.name.Split("_"[0]);
		int _id = int.Parse(name[0]);
		ButtonInit(_id);
		return;
	}

	public void ReSetButtonInit(string str, Transform obj){
		if(str == "Slot_Sponsor") {
			return;
		}
		string[] name = gameObject.name.Split("_"[0]);
		int _id = int.Parse(name[0]); 
		int reqLV = 0;
		var Lock = transform.FindChild("Image_Lock").gameObject as GameObject;
		if(!Lock.activeSelf) return;
		if(str == "Slot_Crew"){
			Common_Crew_Status.Item _crew = Common_Crew_Status.Get (_id);
			reqLV = _crew.ReqLV;
		}else{
			Common_Car_Status.Item _car = Common_Car_Status.Get (_id);
			reqLV = _car.ReqLV;
		}
		if(GV.ChSeason < reqLV){
			transform.FindChild("Image").GetComponent<UISprite>().color = Color.black;
			Lock.SetActive(true);
		}else{
			transform.FindChild("Image").GetComponent<UISprite>().color = Color.white;
			Lock.SetActive(false);
		}
	}
	
	public void mySelectBtn(bool b){
		string str = transform.name;
		if(string.Equals(str,"0000")) return;
		if(b){
			int carid =GV.getTeamCarID(GV.SelectedTeamID);;// Base64Manager.instance.GlobalEncoding(Global.MyCarID);
			if(string.Equals(str,carid.ToString())){
				transform.FindChild("Select").gameObject.SetActive(true);
				transform.FindChild("Image_selected").gameObject.SetActive(true);
				transform.FindChild("Image_selectable").gameObject.SetActive(true);
			}else{
				transform.FindChild("Select").gameObject.SetActive(false);
				transform.FindChild("Image_selected").gameObject.SetActive(false);
				transform.FindChild("Image_selectable").gameObject.SetActive(false);
			}
		}else{
			int crewid = GV.getTeamCrewID(GV.SelectedTeamID);//Base64Manager.instance.GlobalEncoding(Global.MyCrewID);
				if(string.Equals(str,crewid.ToString())){
					transform.FindChild("Select").gameObject.SetActive(true);
					transform.FindChild("Image_selected").gameObject.SetActive(true);
				}else{
					transform.FindChild("Select").gameObject.SetActive(false);
					transform.FindChild("Image_selected").gameObject.SetActive(false);
				}
			transform.FindChild("Image_selectable").gameObject.SetActive(false);
		}
	}

	void OnEnable(){
	}

	void OnDisable(){
	}
}
