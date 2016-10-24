using UnityEngine;
using System.Collections;

public class snsFailedPopup : basePopup {

	void Start(){
		
	}
	
	public void InitPopUp(){
		string[] eText = new string[3];
		if(KoStorage.kostroage == null) {
			eText[0] = "SNS Connect Failed";
			eText[1] ="OK";
			eText[2] = string.Format("SNS Connect Failed \n Please connect again after a while");
			Utility.LogError("Kostorage error1");
		}else{
			eText[0] = "SNS Connect Failed";
			eText[1] ="OK";
			eText[2] = string.Format("SNS Connect Failed \n Please connect again after a while");
		}
		
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = eText[0];//"게임 탈퇴";//KoStorage.getStringDic("60234");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =eText[1];// "탈퇴하기";
		pop.FindChild("lbName").GetComponent<UILabel>().text = eText[2] ;
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
	}
}
