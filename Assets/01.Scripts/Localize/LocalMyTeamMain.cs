using UnityEngine;
using System.Collections;

public class LocalMyTeamMain : MonoBehaviour {
	public UILabel[] lblabel;

	void Start(){
	
		lblabel[0].text = KoStorage.GetKorString("76003"); // sponBtn
		lblabel[1].text = KoStorage.GetKorString("72002"); // upBtn

		lblabel[2].text = KoStorage.GetKorString("76005"); // carchagneBtn
		lblabel[3].text = KoStorage.GetKorString("76007"); // repairBtn

	



		Destroy(this);
	}

}
