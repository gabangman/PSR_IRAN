using UnityEngine;
using System.Collections;

public class OverlapPopup : MonoBehaviour {
	public void OnOkClick(){
		gameObject.SetActive(false);
		Application.Quit();
	}
	
	public void OnCloseClick(){
		Application.Quit();
	}
	
	public void InitPopUp(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		
		pop.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("71202");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =string.Empty;
		pop.FindChild("lbName").GetComponent<UILabel>().text = KoStorage.GetKorString("71100");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("btnok").FindChild("lbOk").GetComponent<UILabel>().text  =KoStorage.GetKorString("71100");
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
	}
	
	public void BlockInitPopUp(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		
		pop.FindChild("lbName").GetComponent<UILabel>().text =string.Format(KoStorage.GetKorString("71102"),GV.UserRevId,GV.gInfo.strEmail);
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = string.Empty;
		pop.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("71014");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("btnok").FindChild("lbOk").GetComponent<UILabel>().text  =  KoStorage.GetKorString("71000");
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		pop.FindChild("lbName").GetComponent<UILabel>().maxLineCount = 5;
		
	}
	
	public void DeleteInitPopUp(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbName").GetComponent<UILabel>().text ="게임데이터를 삭제하오니 재 실행 해주세요";//string.Format(KoStorage.GetKorString("71102")+"\n UserId : {1}",GV.gInfo.strEmail,GV.UserRevId);
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = string.Empty;
		pop.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("71014");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("btnok").FindChild("lbOk").GetComponent<UILabel>().text  =  KoStorage.GetKorString("71000");
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		pop.FindChild("lbName").GetComponent<UILabel>().maxLineCount = 3;
	}



}
