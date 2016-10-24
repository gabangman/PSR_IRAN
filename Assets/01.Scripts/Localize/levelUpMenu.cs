using UnityEngine;
using System.Collections;

public class levelUpMenu : MonoBehaviour {

	public UILabel[] lbText;
	void Start () {
		lbText[0].text = KoStorage.GetKorString("71000");
		lbText[1].text = KoStorage.GetKorString("71113");
		Destroy(this);
	}
}
