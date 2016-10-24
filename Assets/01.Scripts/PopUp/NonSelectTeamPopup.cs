using UnityEngine;
using System.Collections;

public class NonSelectTeamPopup : basePopup {

	void Start(){
		
		
	}
	
	public override void OnOkClick(){
		GameObject.Find("LobbyUI").GetComponent<LobbyManager>().OnMoveToTeam();
		OnCloseClick();
	}
	
	public void InitPopUp(int id){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text =  "팀 선택";//KoStorage.getStringDic("60299");//
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =  KoStorage.GetKorString("71000");//"확인";
		if(id == 0) 	pop.FindChild("lbName").GetComponent<UILabel>().text = "Touring팀을 선택하세요.";//string.Format(KoStorage.getStringDic("60086")+"\n\n\n\r"+KoStorage.getStringDic("60087"));
		else pop.FindChild("lbName").GetComponent<UILabel>().text = "Stock팀을 선택하세요";
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);
	}
}
