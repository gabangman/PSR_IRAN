using UnityEngine;
using System.Collections;

public class BtnOkMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.GetComponent<UILabel>().text = KoStorage.GetKorString("71000");
		Destroy(this);
	}

}
