using UnityEngine;
using System.Collections;

public class crewStatus: MonoBehaviour {

	// Use this for initialization
	public UILabel[] lbText;
	void Start () {
		lbText[0].text = KoStorage.GetKorString("76012");//TableManager.ko.dictionary["60060"].String;
		lbText[1].text = KoStorage.GetKorString("76013");//TableManager.ko.dictionary["60059"].String;
		lbText[2].text =KoStorage.GetKorString("73018");// TableManager.ko.dictionary["60061"].String;
		lbText[3].text = KoStorage.GetKorString("76011");//TableManager.ko.dictionary["60058"].String;
		Destroy(this);
	}
	

}
