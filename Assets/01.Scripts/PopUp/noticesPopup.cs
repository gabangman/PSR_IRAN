using UnityEngine;
using System.Collections;

public class noticesPopup : MonoBehaviour {

	void Start(){

	}

	public void InitPopUp(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("71202");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =string.Empty;
		pop.FindChild("lbOk").gameObject.SetActive(true);
		pop.FindChild("lbOk").GetComponent<UILabel>().text =KoStorage.GetKorString("71000");
		
		pop.FindChild("lbName").GetComponent<UILabel>().text = KoStorage.GetKorString("71121");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);	pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
	}

	public void OnOkClick(){
		GameObject.Find("LobbyUI").SendMessage("PopUpEnd");
		gameObject.SetActive(false);
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
		Destroy(this);
	}

}
