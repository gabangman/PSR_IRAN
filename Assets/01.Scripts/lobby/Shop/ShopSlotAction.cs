using UnityEngine;
using System.Collections;

public class ShopSlotAction : InterAction {

	public void SlotButtonDisable(){
		transform.FindChild("Empty").gameObject.SetActive(true);
		transform.FindChild("Select").gameObject.SetActive(false);
		var t =transform.FindChild("Image").gameObject as GameObject;
		t.SetActive(false);
		ObjectSetActive(gameObject, "ClassPanel", false);
		ObjectSetActive(gameObject, "lbClass", false);
		ObjectSetActive(gameObject, "ClassColor", false);
		ObjectSetActive(gameObject, "lbClass_1", false);
	}
	
	public void SlotButtonEnable(){
		ObjectSetActive(gameObject, "ClassPanel", true);
		ObjectSetActive(gameObject, "lbClass", true);
		ObjectSetActive(gameObject, "ClassColor", true);
		ObjectSetActive(gameObject, "lbClass_1", true);
	}

	public void ShopCarSlotSetting(){
		string[] name = gameObject.name.Split("_"[0]);
		int _id = int.Parse(name[0]);
		if(_id <= 99){
			SlotButtonDisable();
			transform.FindChild("Image_Lock").gameObject.SetActive(false);
			transform.FindChild("Image").GetComponent<UISprite>().color = Color.white;
			transform.FindChild("Select").gameObject.SetActive(false);
			return;
		}
		SlotButtonEnable();
		Common_Car_Status.Item _car = Common_Car_Status.Get(_id);
		transform.FindChild("Empty").gameObject.SetActive(false);
		transform.FindChild("Select").gameObject.SetActive(false);
		transform.FindChild("lbClass").GetComponent<UILabel>().text = _car.Name;
		transform.FindChild("ClassColor").GetComponent<UISprite>().spriteName = "Class_"+_car.Class;
		transform.FindChild("lbClass_1").GetComponent<UILabel>().text = _car.Class;
		int carRqLV = Common_Car_Status.Get(_id).ReqLV;
		if(carRqLV <= GV.ChSeason){
		}else{
			transform.FindChild("Image_Lock").gameObject.SetActive(true);
		}
	}
}
