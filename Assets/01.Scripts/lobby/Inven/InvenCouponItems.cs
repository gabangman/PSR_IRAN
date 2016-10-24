using UnityEngine;
using System.Collections;

public class InvenCouponItems : MonoBehaviour {

	public void InitContent(int idx){
		Transform tr;
		Utility.Log("InvenCouponItems " +gameObject.name);
		if(idx %2 == 0){
			tr = transform.FindChild("Gold");
			tr.gameObject.SetActive(false);
			tr  = transform.FindChild("Silver");
			tr.FindChild("lbAmount").GetComponent<UILabel>().text =
				string.Format("x{0}", GV.myCouponList[0]);//.ToString();
		}else {
			tr=transform.FindChild("Silver");
			tr.gameObject.SetActive(false);
			tr  = transform.FindChild("Gold");
			tr.FindChild("lbAmount").GetComponent<UILabel>().text = string.Format("X{0}", GV.myCouponList[1]);
		}

	}


	public void ChangeContent(int idx){
		Transform tr;
		if(idx %2 == 0){
			tr  = transform.FindChild("Silver");
			tr.FindChild("lbAmount").GetComponent<UILabel>().text =
				string.Format("x{0}", GV.myCouponList[0]);//.ToString();
		}else {
			tr  = transform.FindChild("Gold");
			tr.FindChild("lbAmount").GetComponent<UILabel>().text = string.Format("X{0}", GV.myCouponList[1]);
		}
	
	}

	void OnClick(GameObject obj){
		var temp = obj.transform.FindChild("Selected").gameObject as GameObject;
		if(temp.activeSelf) return;
		NGUITools.FindInParents<InvenCoupon>(gameObject).DisableSelect(obj);
		temp.SetActive(true);

	}


}
