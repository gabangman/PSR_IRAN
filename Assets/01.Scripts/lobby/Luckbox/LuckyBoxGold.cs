using UnityEngine;
using System.Collections;

public class LuckyBoxGold : InfoWindow {
	public UILabel lbprice;
	public UILabel lbText, lbName,lbCoupon,lbVIPAdd, lbVIP,lbCnt;
	public override void ShowWindow ()
	{
		showSub(gameObject);
	}
	
	public override void HiddenWindow ()
	{
		hiddenSub(gameObject);
	}



	protected void OnExecute(){
		if(Global.isAnimation) return;
		Common_Lucky.Item item = Common_Lucky.Get(GV.GoldID);
		if(UserDataManager.instance.buyPriceCheck(item.BuyPrice)) return;
		NGUITools.FindInParents<LuckyBoxMain>(gameObject).GlodResult(item.BuyPrice);
		GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
	}

	protected void OnCoupon(){
		if(Global.isAnimation) return;

		if(GV.myCouponList[1] == 0){
			NGUITools.FindInParents<LuckyBoxMain>(gameObject).GoldCouponResultFail();
			GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
			return;
		}


		NGUITools.FindInParents<LuckyBoxMain>(gameObject).GoldCouponResult();
		GameObject.Find("LobbyUI").SendMessage("TipInfoShow");
	}

	private void ChangeLuckyInfoContent(){
		Common_Lucky.Item item = Common_Lucky.Get(GV.GoldID);
		lbprice.text = string.Format("{0:#,0}", item.BuyPrice);
		lbprice.transform.parent.FindChild("icon_Res").GetComponent<UISprite>().spriteName = "icon_coin";
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
				= string.Format("+{0}%", vItem.V_add_gold);
			lbVIP.text = "VIP";
		}
		lbCnt.text = string.Format("X {0}",GV.myCouponList[1] );
	}

	private void ChangeLuckyLockContent(){
		
		transform.FindChild("btnExe").gameObject.SetActive(false);
		transform.FindChild("btnCoupon").gameObject.SetActive(false);
		transform.FindChild("lbText_locked").gameObject.SetActive(true);
		transform.FindChild("lbText_locked").GetComponent<UILabel>().text 
			= KoStorage.GetKorString("74030");
		Common_Lucky.Item item = Common_Lucky.Get(GV.GoldID);
		lbText.text = item.Text;
		lbName.text = item.Name;
		if(GV.gVIP == 0){
			lbVIP.transform.parent.gameObject.SetActive(false);
		}else{
			int vipID = 1900+(GV.gVIP-1);
			Common_VIP.Item vItem = Common_VIP.Get(vipID);
			lbVIP.transform.parent.gameObject.SetActive(true);
			lbVIPAdd.text 
				= string.Format("+{0}%", vItem.V_add_gold);
			lbVIP.text = "VIP";
		}lbCnt.text = string.Format("X {0}",GV.myCouponList[1] );
	}

	void Start(){
		if(GV.ChSeason < 2){
			ChangeLuckyLockContent();
		}else{
			ChangeLuckyInfoContent();
		}
	}

	void OnEnable(){
		lbCnt.text = string.Format("X {0}",GV.myCouponList[1] );
	}
}
