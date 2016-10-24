using UnityEngine;
using System.Collections;

public class cardbtnmenu : MonoBehaviour {

	public UILabel[] lbText;
	void Start () {
	//	lbText[0].text = KoStorage.getStringDic("60072");//TableManager.ko.dictionary["60072"].String;
	//	lbText[1].text = "수리하기";// 
	//	lbText[2].text = "부품 각성"; 
		Utility.LogWarning("KO " + gameObject.name);
		Destroy(this);
	}
}
