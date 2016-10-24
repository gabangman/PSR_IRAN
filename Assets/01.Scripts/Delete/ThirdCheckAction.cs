using UnityEngine;
using System.Collections;

public class ThirdCheckAction : MonoBehaviour {
	//System.Action callback;
	public void ThirdAgree(System.Action callback){
		//this.callback = callback;
		transform.FindChild("lbName").GetComponent<UILabel>().text =
			KoStorage.GetKorString("71116");
		transform.FindChild("lbText").GetComponent<UILabel>().text =
			KoStorage.GetKorString("72517");//"3자 정보 제공 동의????";
			//KoStorage.getStringDic("");
		transform.FindChild("lbPrice").GetComponent<UILabel>().text = "동의?";
			//KoStorage.getStringDic("");
		transform.FindChild("btnok").GetComponent<UIButtonMessage>().target
			= gameObject;
		transform.FindChild("btnok").GetComponent<UIButtonMessage>().functionName
			= "OnAgreeOk";
		transform.FindChild("Check_V").GetComponent<UIButtonMessage>().target
			= gameObject;
		transform.FindChild("Check_V").GetComponent<UIButtonMessage>().functionName
			= "OnCheckClick";
	}

	void OnCheckClick(){
		GameObject.Find("LobbyUI").SendMessage("thirdAgreeFail");
		EncryptedPlayerPrefs.SetInt("FirstLogin", 10);
		gameObject.SetActive(false);
		gameObject.transform.parent.gameObject.SetActive(false);
		//callback();
		//Destroy(this);
	}

	void OnAgreeOk(){
		var temp = gameObject.AddComponent<GameOption>() as GameOption;
		GameOption.OptionSetting _opt = GameOption.OptionData;
		_opt.isThird = true;
		Global.isThirdCheckable = true;
		GameOption.OptionData = _opt;
		temp.SaveOptionValue();
		EncryptedPlayerPrefs.SetInt("FirstLogin", 10);
		//GameObject.Find("LobbyUI").SendMessage("thirdAgreeOk");
		gameObject.SetActive(false);
		gameObject.transform.parent.gameObject.SetActive(false);
		//Destroy(this);
	}

	void OnThirdCheck(bool isCheck){
		
	}

	public void waringWindow(){
		Global.isPopUp = true;
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<waringPopUp>().InitPopUp();

		//var temp = gameObject.AddComponent<makePopup>().makeEventPopUp() as GameObject;

		pop.name = "Waring_Mode";
		pop.transform.FindChild("Content_BUY").gameObject.SetActive(false);
		var popchild = pop.transform.FindChild("Content_Event") as Transform;
		popchild.gameObject.SetActive(true);

	}



}
