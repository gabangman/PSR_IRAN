using UnityEngine;
using System.Collections;

public class AttentionMenu : MonoBehaviour {

	public UILabel[] lbText;
	void Start () {
	
		lbText[0].text = KoStorage.GetKorString("72400");
		lbText[1].text = KoStorage.GetKorString("72401");
	//	Common_Attend.Item _item;
	//	string iconName = string.Empty;
	//	for(int i = 0; i < lbText.Length-2; i++){
	//		_item = Common_Attend.Get((8700+i));
			//lbText[i+2].text = _item.Name;
			//lbText[i+2].transform.parent.FindChild("lbPrice").GetComponent<UILabel>().text 
			//	= string.Format(" X {0}", _item.Quantity);
			/*if(_item.R_type==1){
				iconName = "icon_dollar";
			}else if(_item.Type == 2){
				iconName = "icon_coin";
			}else{
				iconName = "Coupon_Car";
			}
			lbText[i+2].transform.parent.FindChild("icon").GetComponent<UISprite>().spriteName 
				= iconName;
				*/
	//	}
	//	Utility.LogWarning("Modifiy - AttentionMenu");
		Destroy(this);
	}

}
