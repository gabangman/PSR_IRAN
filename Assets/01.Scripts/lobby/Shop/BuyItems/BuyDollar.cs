using UnityEngine;
using System.Collections;

public class BuyDollar : BuyItem {

	public override void SetBuyPopUp (string BuyName)
	{
		base.SetBuyPopUp (BuyName);
		var Pop = transform.FindChild("Content_BUY").gameObject as GameObject;
		Pop.transform.FindChild("btnCoin").gameObject.SetActive(true);
		OnBuyClick =()=>{
			Global.isNetwork = true;
			StartCoroutine("BuyDollarCount_Delay");	
		};
		
	}

	private void BuyDollarCount(){
		bool b = CheckBuyMoney(1, BuyPrice);
		ResponseFinish(b, (bSucc)=>{
			if(!bSucc) return;
			GV.myDollar+= BuyCount;
			GV.updateDollar = -BuyCount;

			var lobby = GameObject.Find("LobbyUI") as GameObject;
			lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
			lobby.SendMessage("PlusBuyedItem",3, SendMessageOptions.DontRequireReceiver);
			GameObject.Find("Audio").SendMessage("CompleteSound");
		});

	}
	
	
	IEnumerator BuyDollarCount_Delay(){
		bool b = CheckBuyMoney(1, BuyPrice);
		if(!b){
			ResponseFinish(false, (bSucc)=>{
				if(!bSucc) return;
			});
			Global.isNetwork = false;
			yield break;
		}
		bool bWait = true;
		NetworkManager.instance.BuyItemConnect(false, BuyID, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				ResponseFinish(true, (bSucc)=>{
					if(!bSucc) return;
					GV.myDollar+= BuyCount;
					GV.updateDollar = -BuyCount;
					var lobby = GameObject.Find("LobbyUI") as GameObject;
					lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
					lobby.SendMessage("PlusBuyedItem",3, SendMessageOptions.DontRequireReceiver);
					GameObject.Find("Audio").SendMessage("CompleteSound");
				//	GV.vipExp+=  20;
				//	ChangeVIPLevel();
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
