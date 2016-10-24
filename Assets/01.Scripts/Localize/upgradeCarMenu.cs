using UnityEngine;
using System.Collections;

public class upgradeCarMenu : MonoBehaviour {

	// Use this for initialization

	public UILabel[] lbText;
	void Start () {
		lbText[0].text = KoStorage.GetKorString("76309");//TableManager.ko.dictionary["60051"].String;
		lbText[1].text = KoStorage.GetKorString("76310");//TableManager.ko.dictionary["60052"].String;
		lbText[2].text = KoStorage.GetKorString("76018");//TableManager.ko.dictionary["60049"].String;
		lbText[3].text = KoStorage.GetKorString("76313");//TableManager.ko.dictionary["60055"].String;
		lbText[4].text = KoStorage.GetKorString("76019");//TableManager.ko.dictionary["60050"].String;
		lbText[6].text = KoStorage.GetKorString("76311");//TableManager.ko.dictionary["60053"].String;
		if(!transform.name.Equals("Menu_CarParts")){
			lbText[5].text = KoStorage.GetKorString("76315");//TableManager.ko.dictionary["60057"].String;
			Destroy(this);
		}else{
			lobby = GameObject.Find("LobbyUI");
		}
	}

	private GameObject lobby;

	void OnCarParts(GameObject obj){
		string str = obj.transform.parent.name;
		lobby.SendMessage("OnCarPartsClick",str,SendMessageOptions.DontRequireReceiver);
	}

}
