using UnityEngine;
using System.Collections;

public class fullFeulPopup : basePopup {



	public void InitPopUp(int reFuel){
		gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(false);
		var pop = gameObject.transform.FindChild("Content_Fail").gameObject;
		pop.SetActive(true);
		gameObject.GetComponent<TweenAction>().doubleTweenScale(pop);
		var icon = 	pop.transform.FindChild("icon_product") as Transform;
		icon.gameObject.SetActive(false);
		pop.transform.FindChild("lbText").GetComponent<UILabel>().text ="연료충전??";
			//KoStorage.getStringDic("60299");// 	
		pop.transform.FindChild("lbName").GetComponent<UILabel>().text =
			string.Format(KoStorage.getStringDic("72315"), reFuel);
			//"연료가 다 차서 구매할 수 없습니다.";
		pop.transform.FindChild("btnok").GetComponentInChildren<UILabel>().text = 
			KoStorage.GetKorString("71000");//TableManager.ko.dictionary["60184"].String;
	}

	public void OnFailClick(){
		gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(true);
		gameObject.transform.FindChild("Content_Fail").gameObject.SetActive(false);
		gameObject.SetActive(false);
		Destroy(this);
	}
}
