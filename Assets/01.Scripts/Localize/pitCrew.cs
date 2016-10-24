using UnityEngine;
using System.Collections;

public class pitCrew : MonoBehaviour {

	public UILabel[] lbText;
	void Start () {
		lbText[0].text =KoStorage.GetKorString("76215");
		lbText[1].text = KoStorage.GetKorString("73047");
		Destroy(this);
	}
}
