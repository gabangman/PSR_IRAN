using UnityEngine;
using System.Collections;

public class carStatus : MonoBehaviour {

	// Use this for initialization
	public UILabel[] lbText;
	void Start () {
		lbText[0].text = 
		KoStorage.GetKorString("76019");
		lbText[1].text = 
			KoStorage.GetKorString("76018");
		lbText[2].text =
			KoStorage.GetKorString("76017");
		lbText[3].text =
			KoStorage.GetKorString("76015");
		lbText[4].text = 
			KoStorage.GetKorString("76016");
		Destroy(this);
	}

	

}
