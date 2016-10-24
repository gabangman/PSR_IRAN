using UnityEngine;
using System.Collections;

public class InvenCouponInfo : MonoBehaviour {
	public UILabel lbText, lbName;
	void ChangeCarInfo(){

		string[] names = transform.name.Split('_');
		Common_Lucky.Item item;
		if(names[2].Equals("0")){
			item = Common_Lucky.Get(GV.SilverID);
			//lbprice.text = string.Format("{0:#,0}", item.BuyPrice);
			//lbprice.transform.parent.FindChild("icon_Res").GetComponent<UISprite>().spriteName = "icon_dollar";
			lbText.text = item.Text;
			lbName.text = item.Name;
			lbText.transform.parent.FindChild("Icon").GetComponent<UISprite>().spriteName = "icon_Coupon_Silver";
		}else{
			item = Common_Lucky.Get(GV.GoldID);
		//	lbprice.text = string.Format("{0:#,0}", item.BuyPrice);
		//	lbprice.transform.parent.FindChild("icon_Res").GetComponent<UISprite>().spriteName = "icon_coin";
			lbText.text = item.Text;
			lbName.text = item.Name;
			lbText.transform.parent.FindChild("Icon").GetComponent<UISprite>().spriteName = "icon_Coupon_Gold";
		}
	}

}
