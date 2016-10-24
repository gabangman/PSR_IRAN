using UnityEngine;
using System.Collections;

public class LuckyBoxSilver : InfoWindow {
	public UILabel lbprice;
	public UILabel lbText, lbName,lbCoupon, lbVIP, lbVIPAdd, lbCnt;
	public override void ShowWindow ()
	{
		showSub(gameObject);
	}
	
	public override void HiddenWindow ()
	{
		hiddenSub(gameObject);
	}
	
	void OnExecute(){
		if(Global.isAnimation) return;
		Common_Lucky.Item item = Common_Lucky.Get(GV.SilverID);
		if(UserDataManager.instance.buyPriceCheck(item.BuyPrice)) return;
		NGUITools.FindInParents<LuckyBoxMain>(gameObject).SilverResult(item.BuyPrice);
		GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
	}
	void OnCoupon(){
		if(Global.isAnimation) return;
		if(GV.myCouponList[0] == 0){
			NGUITools.FindInParents<LuckyBoxMain>(gameObject).SilverCouponResultFail();
			GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
			return;
		}
		NGUITools.FindInParents<LuckyBoxMain>(gameObject).SilverCouponResult();
		GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
	}

	private void ChangeContent(){
		Common_Lucky.Item item = Common_Lucky.Get(GV.SilverID);
		lbprice.text = string.Format("{0:#,0}", item.BuyPrice);
		lbprice.transform.parent.FindChild("icon_Res").GetComponent<UISprite>().spriteName = "icon_dollar";
		lbText.text = item.Text;
		lbName.text = item.Name;
		lbCoupon.text =  KoStorage.GetKorString("74007");
		if(GV.gVIP == 0){
			lbVIP.transform.parent.gameObject.SetActive(false);
		}else{
			int vipID = 1900+(GV.gVIP-1);
			Common_VIP.Item vItem = Common_VIP.Get(vipID);
			lbVIP.transform.parent.gameObject.SetActive(true);
			lbVIPAdd.text 
				= string.Format("+{0}%", vItem.V_add_silver);
			lbVIP.text = "VIP";
		}
		lbCnt.text = string.Format("X {0}",GV.myCouponList[0] );
	}
	public void ChangeLuckyInfoContent(){
			ChangeContent();
	}

	void OnEnable(){
		try {
			lbCnt.text = string.Format("X {0}",GV.myCouponList[0] );
		

		}catch(System.Exception  e){
			Utility.LogWarning("Try Catch Exception  error ");
		}

		//}lbCnt.text = string.Format("X {0}",GV.myCouponList[0] );
	}
	void Start(){
		ChangeLuckyInfoContent();
	}
}
