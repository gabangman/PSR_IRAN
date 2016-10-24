using UnityEngine;
using System.Collections;

public class LocalRepair : MonoBehaviour {

	public UILabel[] labels;
	void Start () {
		
		labels[0].text = KoStorage.GetKorString("76007"); // 자동차 수리 
		//labels[2].text = KoStorage.GetKorString("76501"); // description
		//labels[1].text =  KoStorage.GetKorString("76502");//"수리하기?";//KoStorage.GetKorString("71000"); // 확인

		Destroy(this);
	}
}
