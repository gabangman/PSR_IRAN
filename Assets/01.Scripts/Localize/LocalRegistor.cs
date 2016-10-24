using UnityEngine;
using System.Collections;

public class LocalRegistor : MonoBehaviour {

	public UILabel[] lblabel;
	
	void Start(){
		
		lblabel[0].text = KoStorage.GetKorString("70124"); // description
		lblabel[1].text = KoStorage.GetKorString("70125"); // registor
		
		lblabel[2].text = KoStorage.GetKorString("70126"); // 취소
		
		
		
		
		Destroy(this);
	}
}
