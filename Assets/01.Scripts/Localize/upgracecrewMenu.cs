using UnityEngine;
using System.Collections;

public class upgracecrewMenu : MonoBehaviour {

	public UILabel[] lbText;
	void Start () {
		lbText[0].text = KoStorage.GetKorString("76215");
		lbText[1].text = KoStorage.GetKorString("76213");
		lbText[2].text = KoStorage.GetKorString("76217");
		lbText[3].text = KoStorage.GetKorString("76216");
		lbText[4].text = KoStorage.GetKorString("76214");
		Destroy(this);
	}

}
