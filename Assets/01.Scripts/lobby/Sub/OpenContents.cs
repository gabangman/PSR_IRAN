using UnityEngine;
using System.Collections;

public class OpenContents : MonoBehaviour {


	void Start(){
	
	
	
	
	}


	void OnClose(GameObject obj){
		obj.transform.parent.gameObject.SetActive(false);
		NGUITools.FindInParents<SubMenuWindow>(gameObject).OnCloseClick();
	}

	public void OnOpenWindow(string str){
		string name = string.Empty;
		string name1 = string.Empty;
		name = KoStorage.GetKorString("73057");
		switch(str){
		case "S1_5_Regular_Drag":
			name1 = KoStorage.GetKorString("73021");
			myAcc.instance.account.bRaceSubBTN[1] = true;
			myAcc.instance.account.bRaceMenuBTN[3]=true;
			break;
		case "S2_1_Ranking":
			name1 = KoStorage.GetKorString("73022");
			break;
		case "S2_3_PVP_Drag":
			myAcc.instance.account.bRaceSubBTN[2] = true;
			myAcc.instance.account.bRaceMenuBTN[2]=true;
			name1 = KoStorage.GetKorString("73023");
			break;
		case "S2_5_Event_Featured":
			name1 = KoStorage.GetKorString("73024");
			break;
		case "S3_1_Clubrace":
			name1 = KoStorage.GetKorString("73025");
			AccountManager.instance.StartCoroutine("getClubAccount");
			myAcc.instance.account.bLobbyBTN[0] = true;
			break;
		case "S3_3_PVP_City":
			name1 = KoStorage.GetKorString("73026");
			myAcc.instance.account.bRaceSubBTN[3] = true;
			myAcc.instance.account.bRaceMenuBTN[2]=true;
			break;
		case "S4_1_Event_Evocube":
			name1 = KoStorage.GetKorString("73027");
			break;
		default:break;
		}

		var tr = transform.FindChild(str).gameObject as GameObject;
		tr.SetActive(true);
		tr.transform.FindChild("lbTitle").GetComponent<UILabel>().text = name;
		tr.transform.FindChild("lbText").GetComponent<UILabel>().text = name1;
		tr.transform.FindChild("Sprite (Check_V)").GetComponent<UIButtonMessage>().target = gameObject;

	}

	public void OnOpenLuckyWindow(){
	//	string name ="골드박스 여렸어?";// KoStorage.GetKorString("73057");
	//	string name1 = "골드박스 사용해도 된다. ?";;//KoStorage.GetKorString("73057");
		var tr = transform.FindChild("S3_1_Luckybox_Gold").gameObject as GameObject;
		tr.SetActive(true);
		tr.transform.FindChild("lbTitle").GetComponent<UILabel>().text = KoStorage.GetKorString("74031");
		tr.transform.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("74032");
		tr.transform.FindChild("img").FindChild("lbText_title").GetComponent<UILabel>().text 
				= KoStorage.GetKorString("74004");
		tr.transform.FindChild("Sprite (Check_V)").GetComponent<UIButtonMessage>().target = gameObject;
	}

}
