using UnityEngine;
using System.Collections;

public class popUpTest : basePopup {

	void Start(){
		string str = "테스트";
		string str1 = "확인";
		string str2 = "이건 테스트 문장임";
		ChangeContentString(str, str1, str2);
	}

	public override void OnOkClick ()
	{
		//base.OnOkClick ();
	}

}
