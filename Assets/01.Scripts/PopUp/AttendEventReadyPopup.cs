using UnityEngine;
using System.Collections;

public class AttendEventReadyPopup : MonoBehaviour {
	private GameObject target;
	public void setObect(GameObject obj){
		this.target = obj;
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = "알림 메시지";//eText[0];//"게임 탈퇴";//KoStorage.getStringDic("60234");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text ="확인";//eText[1];// "탈퇴하기";
		pop.FindChild("lbName").GetComponent<UILabel>().text = "출석이벤트 현재 준비 중입니다.";
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);
	}


	void OnOkClick(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
		gameObject.SetActive(false);
		target.SendMessage("OnCloseClick");
		Destroy(this);
	}


}
