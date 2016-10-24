using UnityEngine;
using System.Collections;

public class emergencyPopup : basePopup {
	
	void Start(){
		
	}
	
	public void InitPopUp(){
		string[] eText = new string[3];
		if(KoStorage.kostroage == null) {
			//	eText[0] = "서버 점검 중???";
			//	eText[1] ="OK!";
			//	eText[2] = string.Format("서버 점검중입니다. \n 잠시 후에 다시 접속하세요???.");
			//	Utility.LogError("Kostorage error1");
		}else{
			eText[0] =KoStorage.GetKorString("71104");
			eText[1] = KoStorage.GetKorString("71000");
			eText[2] = KoStorage.GetKorString("71120");
		}
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = eText[0];
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =eText[1];
		pop.FindChild("lbName").GetComponent<UILabel>().text = eText[2] ;
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
	}
	
}
