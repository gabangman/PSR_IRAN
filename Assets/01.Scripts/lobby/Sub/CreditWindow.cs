using UnityEngine;
using System.Collections;

public class CreditWindow : MonoBehaviour {

	void OnClose(GameObject obj){
		obj.transform.parent.gameObject.SetActive(false);
		NGUITools.FindInParents<SubMenuWindow>(gameObject).OptionReturn();
		Global.isPopUp = false;
	}

}
