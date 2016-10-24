using UnityEngine;
using System.Collections;

public class refreshPopup : MonoBehaviour {

	int idx;
	GameObject obj;
	public void InitWindow(int idx, GameObject obj){
		var pop = transform.FindChild("Content_BUY") as Transform;
		
		pop.FindChild("lbText").GetComponent<UILabel>().text =  KoStorage.GetKorString("73019");//"리뷰쓰기";//KoStorage.getStringDic("60234");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");//"리뷰쓰러가기";// KoStorage.GetKorString("71000");//"시작";
		pop.FindChild("lbName").GetComponent<UILabel>().text = KoStorage.GetKorString("73020");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);
		this.idx = idx;
		this.obj = obj;
	}
	
	
	void OnOkClick(){
		obj.SendMessage("reFreshStart",idx);
		gameObject.SetActive(false);
		Destroy(this);
		
	}
	
	void OnCloseClick(){
		GameObject.Find("LobbyUI").SendMessage("OnBackClick");
		gameObject.SetActive(false);
		Destroy(this);
	}
}
