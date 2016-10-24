using UnityEngine;
using System.Collections;

public class ClanRegularPopup : MonoBehaviour {

	public void InitContent(string[] str, int idx = 0){
		Global.isPopUp = true;
		transform.FindChild("lbText").GetComponent<UILabel>().text = str[0];
		transform.FindChild("Btn_Ok").GetComponentInChildren<UILabel>().text = str[2];
		transform.FindChild("lbTitle").GetComponentInChildren<UILabel>().text = str[1];

		if(idx == 0){
			transform.FindChild("Close").gameObject.SetActive(true);
		}else if(idx == 1){
			transform.FindChild("Close").gameObject.SetActive(false);
		}
	}
	public void changeButtonMessage(GameObject obj, string fuName){
		transform.FindChild("Btn_Ok").GetComponent<UIButtonMessage>().target=obj;
		transform.FindChild("Btn_Ok").GetComponent<UIButtonMessage>().functionName=fuName;

	}
	public void CreateClanPopup(){
		//InitContent(new string[]{"클랜 생성!!!! "," 생성 "," 생성할래?"});
		//InitContent(new string[]{string.Format("{0:#,0} {1}",CClub.ClubDollar, "으로 생성하시겠습니까?"),KoStorage.GetKorString("77108"),KoStorage.GetKorString("71000")});
		//changeButtonMessage(transform.parent.gameObject, "CreateOk");
	}

	public void ModifyClanPopup(){
		InitContent(new string[]{"클랜 수정! ","수정하시겠습니까?",KoStorage.GetKorString("71000")});
		changeButtonMessage(transform.parent.gameObject, "ModifyOk");
	}
	public void SignOutPopup(){
		InitContent(new string[]{KoStorage.GetKorString("77608"),KoStorage.GetKorString("77125"),KoStorage.GetKorString("71000")});
		changeButtonMessage(transform.parent.gameObject, "SignOutOk");
	}

	public void SignOutFailPopup(){
		InitContent(new string[]{"클랜 해제!!!! ","클럽 멤버가 있다.",KoStorage.GetKorString("71000")});
		changeButtonMessage(transform.parent.gameObject, "CloseOk");
	}
	public void SignOutRaceFailPopup(){
		InitContent(new string[]{KoStorage.GetKorString("77402"),KoStorage.GetKorString("72510"),KoStorage.GetKorString("71000")},1 );
		changeButtonMessage(transform.parent.gameObject, "CloseOk");
	}

	public void CreateFailPopup(int idx){
		if(idx == 0){
			InitContent(new string[]{"클럽이름을 넣어주세요 "," 클럽 생성 실패 ?",KoStorage.GetKorString("71000")});
		}else{
			InitContent(new string[]{"클럽 설명문을 넣어주세요 "," 클럽 생성 실패 ? ",KoStorage.GetKorString("71000")});
		}
		changeButtonMessage(transform.parent.gameObject, "CreateFailedCloseOk");
	}

	public void SignInPopup(int idx, string cName){
	
		if(idx == 1) transform.FindChild("Close").GetComponent<UIButtonMessage>().functionName = "OnVisitCancle";
		InitContent(new string[]{string.Format(KoStorage.GetKorString("77404"), cName),KoStorage.GetKorString("77403"),KoStorage.GetKorString("71000")});
		changeButtonMessage(transform.parent.gameObject, "SignInOk");
	}

	public void NoCreateNotMoney(){
		InitContent(new string[]{string.Format(KoStorage.GetKorString("77106")),KoStorage.GetKorString("77108"),KoStorage.GetKorString("71000")}, 1);
		changeButtonMessage(transform.parent.gameObject, "CloseOk");
	}

	public void NoSignInPopUp(int idx){
		InitContent(new string[]{KoStorage.GetKorString("77131"),KoStorage.GetKorString("77206"),KoStorage.GetKorString("71000")});
		changeButtonMessage(transform.parent.gameObject, "CloseOk");
	}

	public void AlreadySignInPopUp(int idx){
		InitContent(new string[]{"다른 클랜에 가입되어 있다",KoStorage.GetKorString("77206"),KoStorage.GetKorString("71000")},1);
		changeButtonMessage(transform.parent.gameObject, "CloseOk");
	}

	public void SearchErrorPopUp(int index){
		if(index == 0) InitContent(new string[]{KoStorage.GetKorString("77211"),KoStorage.GetKorString("77116"),KoStorage.GetKorString("71000")},1);
		else if(index == 1)  InitContent(new string[]{KoStorage.GetKorString("77503") ,KoStorage.GetKorString("77116"),KoStorage.GetKorString("71000")},1);
		else if(index == 2) InitContent(new string[]{KoStorage.GetKorString("77211"),KoStorage.GetKorString("77116"),KoStorage.GetKorString("71000")},1);
		changeButtonMessage(transform.parent.gameObject, "CloseOk");
	}

	public void SignOutMemPopup(){
		InitContent(new string[]{KoStorage.GetKorString("77608"),KoStorage.GetKorString("77125"),KoStorage.GetKorString("71000")});
		changeButtonMessage(transform.parent.gameObject, "SignOutMemOk");
	}

	void OnEnable(){
		transform.FindChild("Close").GetComponent<UIButtonMessage>().functionName = "OnCancleClick";
	}

	void OnCancleClick(){
		Global.isPopUp = false;
		gameObject.SetActive(false);
	}

	void OnVisitCancle(){
		Global.isPopUp = false;
		NGUITools.FindInParents<ClanPopup>(gameObject).CheckVisitCancle();
		gameObject.SetActive(false);
	
	}

	public void KickMemeberPopup(){
		InitContent(new string[]{KoStorage.GetKorString("77210"), KoStorage.GetKorString("77227"),KoStorage.GetKorString("71000")});
		changeButtonMessage(transform.parent.gameObject, "OnKickOk");
	}

	public void EntryFlagPopup(){
		string str =null;
		if(CClub.mClubInfo.myEntryFlag ==0){
			str = " 참가 할꺼야?";
		}else{
			str = " 참가 안할꺼야?";
		}
		InitContent(new string[]{str,"참가?",KoStorage.GetKorString("71000")});
		changeButtonMessage(transform.parent.gameObject, "OnEntryOk");
	}
}
