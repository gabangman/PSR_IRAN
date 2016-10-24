using UnityEngine;
using System.Collections;

public class CarSlotAction : InterAction {

	public override void OnCarItemClick(){
		if(Global.isAnimation) return;
		var tmp = transform.FindChild("Empty").gameObject;
		if(tmp.activeSelf) return;
		tmp = transform.FindChild("Select").gameObject;
	//	int id = int.Parse(gameObject.name);
		GameObject obj = GameObject.Find("LobbyUI");
	//	if(id < 1200){
	//		bool isNew = myAccount.instance.account.buttonStatus.TeamCarNews[id-1000];
	//		if(!isNew) {}
	//		else{
	//			isNew = false;
	//			myAccount.instance.account.buttonStatus.TeamCarNews[id-1000] = isNew;
	//			transform.FindChild("icon_New").gameObject.SetActive(isNew);
	//			obj.SendMessage("DisableTeamCarNewbtn",SendMessageOptions.DontRequireReceiver);
	//		}
	//	}
	//	else{ 
	//		bool isNew = myAccount.instance.account.buttonStatus.TeamCrewNews[id-1200];
	//		if(!isNew) {
	//			
	//		}else{
	//			isNew = false;
	//			myAccount.instance.account.buttonStatus.TeamCrewNews[id-1200] = isNew;
	//			transform.FindChild("icon_New").gameObject.SetActive(isNew);
	//			obj.SendMessage("DisableTeamCrewNewbtn",SendMessageOptions.DontRequireReceiver);
	//		}
			
	//	}
		if(tmp.activeSelf) return;
		
	//	if(id < 1200){
			obj.SendMessage("OnCarClick", gameObject, SendMessageOptions.DontRequireReceiver);
		Utility.LogWarning(gameObject.name);
	//	}
	//	else{ 
	//		obj.SendMessage("OnCrewClick", gameObject, SendMessageOptions.DontRequireReceiver);
	//	}
	}

	public void SlotDisable(){
		transform.FindChild("Empty").gameObject.SetActive(true);
		//transform.FindChild("Select").gameObject.SetActive(false);
		var t =transform.FindChild("Image").gameObject as GameObject;
		t.SetActive(false);
		ObjectSetActive(gameObject, "ClassPanel", false);
		ObjectSetActive(gameObject, "lbClass", false);
		ObjectSetActive(gameObject, "ClassColor", false);
		ObjectSetActive(gameObject, "lbClass_1", false);
	}
	
	public void SlotEnable(){
		transform.FindChild("Empty").gameObject.SetActive(false);
		//transform.FindChild("Select").gameObject.SetActive(true);
		var t =transform.FindChild("Image").gameObject as GameObject;
		t.SetActive(true);
		ObjectSetActive(gameObject, "ClassPanel", true);
		ObjectSetActive(gameObject, "lbClass", true);
		ObjectSetActive(gameObject, "ClassColor", true);
		ObjectSetActive(gameObject, "lbClass_1", true);
	}
	public void unSetSelectLine(){
		transform.FindChild("Select").gameObject.SetActive(false);
	}

	public void SlotSetting(){
		string[] name = gameObject.name.Split('_');
		int _id = int.Parse(name[0]);
		if(_id == 0){
			SlotDisable();
		//	transform.FindChild("Select").gameObject.SetActive(false);
			return;
		}
		SlotEnable();
		//Table_Car.Item _car = Table_Car.Get (_id);
		Common_Car_Status.Item _car = Common_Car_Status.Get(_id);
		transform.FindChild("Empty").gameObject.SetActive(false);
	//	transform.FindChild("Select").gameObject.SetActive(false);
		transform.FindChild("lbClass").GetComponent<UILabel>().text = _car.Name;
		transform.FindChild("ClassColor").GetComponent<UISprite>().spriteName = "Class_"+ name[1];
		transform.FindChild("lbClass_1").GetComponent<UILabel>().text = name[1];
		var t = transform.FindChild("Image").gameObject as GameObject;
		t.GetComponent<UISprite>().spriteName = _id.ToString();
	}
	

	public void mySelectBtn(bool b){
		string str = transform.name;
		if(string.Equals(str,"0000")) return;
		if(b){
			int carid = GV.getTeamCarID(GV.SelectedTeamID);// Base64Manager.instance.GlobalEncoding(Global.MyCarID);
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
			int crewid =GV.getTeamCrewID(GV.SelectedTeamID);// Base64Manager.instance.GlobalEncoding(Global.MyCrewID);
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

}