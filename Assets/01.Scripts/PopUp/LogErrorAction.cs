using UnityEngine;
using System.Collections;

public class LogErrorAction : MonoBehaviour {

	
	public void InitLogWindow(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("71105");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");//
		pop.FindChild("lbName").GetComponent<UILabel>().text = KoStorage.GetKorString("72314");
			//"[009cff]서버와의 접속이 끊겼습니다.[-] \r\n\n 게임을 다시 실행 해주세요";
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
	}
	
	public void initLogoutWindow(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("71202");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");
		pop.FindChild("lbName").GetComponent<UILabel>().text = 
			KoStorage.GetKorString("71100");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
	}

	public void InitBlockWindow(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("71202");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");
		pop.FindChild("lbName").GetComponent<UILabel>().text = 
			KoStorage.GetKorString("71102");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
	}
}
