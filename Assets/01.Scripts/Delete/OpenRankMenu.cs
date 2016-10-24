using UnityEngine;
using System.Collections;

public class OpenRankMenu : MonoBehaviour {

	public UILabel[] lbText;
	void Start () {
		Utility.LogError("KO " + gameObject.name);
		return;
		//lbText[0].text = KoStorage.getStringDic("60243");
		//lbText[1].text = KoStorage.getStringDic("60245");
		
		
		Destroy(this);
	}

}
