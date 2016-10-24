using UnityEngine;
using System.Collections;

public class DealerRecomm : MonoBehaviour {

	// Use this for initialization
	public UILabel labels;

	void Start () {
		labels.text = KoStorage.GetKorString("71000");
		transform.FindChild("lbText").GetComponent<UILabel>().text =
			KoStorage.GetKorString("74033");
	}


  	public void OnDealerInit(){
	//	transform.FindChild("lbText").GetComponent<UILabel>().text 
	//		= string.Format("Car ? : {0}, Class ? : {1}", Common_Car_Status.Get(1022).Name, "SS");

	}

	protected void OnMove(){
		GameObject.Find("LobbyUI").SendMessage("OnOpenDealer", SendMessageOptions.DontRequireReceiver);
		GV.gInfo.extra03 = 0;
		NGUITools.FindInParents<SubMenuWindow>(gameObject).OnCloseClick();
	}
	

}
