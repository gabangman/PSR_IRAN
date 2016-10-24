using UnityEngine;
using System.Collections;

public class InvenCarItem : MonoBehaviour {

	public void ChangeTeamAction(){
		transform.FindChild("Image_selectable").gameObject.SetActive(false);
		transform.FindChild("Image_selected").gameObject.SetActive(false);
	}

	public void EmptyAction(){
		transform.FindChild("Image").gameObject.SetActive(false);
		transform.FindChild("ClassColor").gameObject.SetActive(false);
		transform.FindChild("ClassPanel").gameObject.SetActive(false);
		transform.FindChild("lbClass").gameObject.SetActive(false);
		transform.FindChild("Empty").gameObject.SetActive(true);
		transform.FindChild("Select").gameObject.SetActive(false);
		transform.FindChild("lbClass_1").gameObject.SetActive(false);
		transform.FindChild("Image_selected").gameObject.SetActive(false);
		transform.FindChild("Image_selectable").gameObject.SetActive(false);
		transform.FindChild("icon_New").gameObject.SetActive(false);
	}

	public void ContentChange(){
		transform.FindChild("Image").gameObject.SetActive(true);
		transform.FindChild("ClassColor").gameObject.SetActive(true);
		transform.FindChild("ClassPanel").gameObject.SetActive(true);
		transform.FindChild("lbClass").gameObject.SetActive(true);
		transform.FindChild("Empty").gameObject.SetActive(false);
		transform.FindChild("Select").gameObject.SetActive(false);
		transform.FindChild("lbClass_1").gameObject.SetActive(true);
		string[] strname = transform.parent.name.Split('_');
		int id = int.Parse(strname[1]);
		Common_Car_Status.Item _car = Common_Car_Status.Get(id);
		transform.FindChild("lbClass").GetComponent<UILabel>().text = _car.Name;
		transform.FindChild("ClassColor").GetComponent<UISprite>().spriteName = "Class_"+strname[2];
		var t = transform.FindChild("Image").gameObject as GameObject;
		t.GetComponent<UISprite>().spriteName = strname[1];
		transform.FindChild("lbClass_1").GetComponent<UILabel>().text =strname[2];
		id = int.Parse(strname[3]);
		if(id == 0){
			transform.FindChild("Image_selected").gameObject.SetActive(false);
			transform.FindChild("Image_selectable").gameObject.SetActive(false);
		//	if(_car.ReqLV > GV.ChSeason){
		//		transform.FindChild("Image_selectable").gameObject.SetActive(false);
		//	}else{
		//		transform.FindChild("Image_selectable").gameObject.SetActive(true);
		//	}
		}else{
			if(GV.SelectedTeamID == id){
				transform.FindChild("Image_selectable").gameObject.SetActive(true);
			}else{
				transform.FindChild("Image_selectable").gameObject.SetActive(false);
			}

			transform.FindChild("Image_selected").gameObject.SetActive(true);
		
		}

		id =int.Parse(strname[4]);
		if(GV.mineCarList[id].bNewBuyCar) {
			transform.FindChild("icon_New").gameObject.SetActive(true);
		}
		else  transform.FindChild("icon_New").gameObject.SetActive(false);
	
	}

	void OnCarItemClick(GameObject obj){
		if(Global.isAnimation) return;
		var tr = transform.FindChild("Empty").gameObject;
		if(tr.activeSelf) return;
		tr = transform.FindChild("Select").gameObject as GameObject;
		if(tr.activeSelf) return;
		NGUITools.FindInParents<InvenCar>(gameObject).DisableSelect(obj.transform.parent.gameObject);
		tr.SetActive(true);
		GameObject.Find("LobbyUI").SendMessage("ChangeElevatorCar", obj.transform.parent.name);

		string[] strname = obj.transform.parent.name.Split('_');
		int id = int.Parse(strname[4]);
		if(GV.mineCarList[id].bNewBuyCar) {
			transform.FindChild("icon_New").gameObject.SetActive(false);
			GV.mineCarList[id].bNewBuyCar = false;
		}

	}

	public void NonSelectBG(){
		transform.FindChild("Select").gameObject.SetActive(false);
	}

}
