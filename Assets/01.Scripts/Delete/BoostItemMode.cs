using UnityEngine;
using System.Collections;

public class BoostItemMode : MonoBehaviour {

void OnEnable(){
		transform.GetComponent<TweenPosition>().Reset();
		transform.GetComponent<TweenPosition>().enabled  = true;
	}

	void OnDisable(){
		transform.localPosition = new Vector3(1500,0,-5);
	}

	void OnItemSelect(GameObject selectObj){
		var items = transform.FindChild("Detail") as Transform;
		int count = items.childCount;
		string str = selectObj.transform.parent.name;
		for(int i = 0; i < count; i++){
			var obj = items.GetChild(i).gameObject as GameObject;
			if(obj.name.Equals(str)){
				obj.transform.FindChild("BG_out").gameObject.SetActive(true);
				Global.addBSTime = 1.0f;
				Global.addBSTorque = 10;
			}else{
				obj.transform.FindChild("BG_out").gameObject.SetActive(false);
			}
		}
	}
}
