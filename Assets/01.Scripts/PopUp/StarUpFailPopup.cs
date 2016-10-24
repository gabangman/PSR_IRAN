using UnityEngine;
using System.Collections;

public class StarUpFailPopup : MonoBehaviour {

	System.Action CloseCallback;
	public  void OnOkClick(){
		CloseCallback();
		OnClose();
	}

	public void OnClose(){
		CloseCallback = null;
		gameObject.SetActive(false);
		Destroy(this);
	}
	public void InitPopUp(System.Action callback){
		this.CloseCallback = callback;
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		pop.FindChild("lbText").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("76412"));//
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = null;// KoStorage.GetKorString("71000");//"확인";
		pop.FindChild("lbOk").gameObject.SetActive(true);
		pop.FindChild("lbOk").GetComponent<UILabel>().text =  KoStorage.GetKorString("71000");
		pop.FindChild("lbName").GetComponent<UILabel>().text = KoStorage.GetKorString("76413");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);
	}
}
