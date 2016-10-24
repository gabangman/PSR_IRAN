using UnityEngine;
using System.Collections;

public class postMenu : MonoBehaviour {

	public UILabel[] lbText;
	void Start () {
		lbText[1].text = KoStorage.GetKorString("72300");//TableManager.ko.dictionary["60017"].String;
		lbText[0].text = KoStorage.GetKorString("72301");//TableManager.ko.dictionary["60016"].String;

		Destroy(this);
	}
}
