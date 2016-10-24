using UnityEngine;
using System.Collections;

public class OpenFeatureMenu : MonoBehaviour {

	public UILabel[] lbText;
	void Start () {
	
		Utility.LogError("KO " + gameObject.name);
		return;
		//lbText[0].text = KoStorage.getStringDic("60244");
		//lbText[1].text = KoStorage.getStringDic("60246");
		int id = 8599; 
		for(int i = 1; i <= 5; i++){
			id = 8599+i;
			Common_Material.Item _item  = Common_Material.Get(id);
			lbText[2*i].text = _item.Name; 
			lbText[2*i+1].text =string.Format("{0} %", (int)(_item.Up*100));
		}

		Destroy(this);
	}

}
