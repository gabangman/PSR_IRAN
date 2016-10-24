using UnityEngine;
using System.Collections;

public class waringPopUp : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	public void OnCloseClick(){
		var popchild  = transform.FindChild("Content_Event") as Transform;
		popchild.gameObject.SetActive(false);
		popchild.FindChild("btnok").GetComponent<UIButtonMessage>().functionName = "OnSuccessClick";
		transform.FindChild("Content_BUY").gameObject.SetActive(true);
		gameObject.SetActive(false);
		Global.isPopUp = false;
		Destroy(this);
	}

	public void InitPopUp(){
		var popchild = transform.FindChild("Content_Event") as Transform;
		Global.isPopUp = true;
		popchild.FindChild("lbText").GetComponent<UILabel>().text = 
			"3자 동의 경고??";
		popchild.FindChild("icon_product").gameObject.SetActive(false);
		popchild.FindChild("btnok").GetComponent<UIButtonMessage>().functionName = "OnCloseClick";
		popchild.FindChild("lbText2").gameObject.SetActive(false);
		popchild.FindChild("lbText1").gameObject.SetActive(true);
		popchild.FindChild("lbText1").GetComponent<UILabel>().text = 
			"내용없다?";
	}



}
