using UnityEngine;
using System.Collections;

public class BuyCoupon : BuyItem {

	public override void SetBuyPopUp (string BuyName, int idx)
	{
		base.SetBuyPopUp (BuyName,idx);
		var Pop = transform.FindChild("Content_BUY").gameObject as GameObject;
		Pop.transform.FindChild("btnCoin").gameObject.SetActive(true);
		OnBuyClick =()=>{
			Global.isNetwork = true;
			StartCoroutine("BuyCouponCount_Delay", idx);	
		};
	}


	private void BuyCouponCount(int idx){
		bool b = CheckBuyMoney(1, BuyPrice);
		ResponseFinish(b, (bSucc)=>{
			if(!bSucc) return;
			GV.UpdateCouponList(idx, BuyCount);
			var lobby = GameObject.Find("LobbyUI") as GameObject;
			lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
			lobby.SendMessage("PlusBuyedItem",5, SendMessageOptions.DontRequireReceiver);
			GameObject.Find("Audio").SendMessage("CompleteSound");
			myAcc.instance.account.bInvenBTN[3] = true;

		});

	}
	
	
	IEnumerator BuyCouponCount_Delay(int idx){
		yield return new WaitForSeconds(0.5f);
		bool b = CheckBuyMoney(1, BuyPrice);
		if(!b){
			ResponseFinish(false, (bSucc)=>{
				if(!bSucc) return;
			});
			Global.isNetwork = false;
			yield break;
		}
		bool bWait = true;
		Utility.LogWarning("BuyID " + BuyID);
		NetworkManager.instance.BuyItemConnect(false, BuyID, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				ResponseFinish(true, (bSucc)=>{
					if(!bSucc) return;
					GV.UpdateCouponList(idx, BuyCount);
					var lobby = GameObject.Find("LobbyUI") as GameObject;
					lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
					lobby.SendMessage("PlusBuyedItem",5, SendMessageOptions.DontRequireReceiver);
					GameObject.Find("Audio").SendMessage("CompleteSound");
				});
			}else{
				
			}
			bWait = false;
		});
		
		while(bWait){
			yield return null;
		}
		Global.isNetwork = false;
		//BuyDollarCount();
		
	}
}
