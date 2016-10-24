using UnityEngine;
using System.Collections;

public class BuyFuel : BuyItem {

	public override void SetBuyPopUp (string BuyName)
	{
		base.SetBuyPopUp (BuyName);
		var Pop = transform.FindChild("Content_BUY").gameObject as GameObject;
		Pop.transform.FindChild("btnCoin").gameObject.SetActive(true);
		OnBuyClick =()=>{
			Global.isNetwork = true;
			StartCoroutine("BuyFuelCount_Delay");	
		};

	}

	private void BuyFuelCount(){
		if(GV.mUser.FuelCount >= GV.mUser.FuelMax){
			gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(false);
			var mPop = gameObject.transform.FindChild("Content_Fail").gameObject;
			mPop.SetActive(true);
			gameObject.GetComponent<TweenAction>().doubleTweenScale(mPop);
			var icon = 	mPop.transform.FindChild("icon_product") as Transform;
			icon.gameObject.SetActive(false);
			mPop.transform.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("71107");
			mPop.transform.FindChild("lbName").GetComponent<UILabel>().text =
				KoStorage.GetKorString("71108");
				//string.Format("{0} 다 찼다 ",1);//string.Format(KoStorage.GetKorString("72315"),1);
			mPop.transform.FindChild("btnok").GetComponentInChildren<UILabel>().text = 
				KoStorage.GetKorString("71000");

		}else{
			bool b = CheckBuyMoney(1, BuyPrice);
			ResponseFinish(b, (bSucc)=>{
				if(bSucc){
					GV.mUser.FuelCount+= BuyCount;
					var lobby = GameObject.Find("LobbyUI") as GameObject;
					lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
					lobby.SendMessage("PlusBuyedItem",3, SendMessageOptions.DontRequireReceiver);
					lobby.SendMessage("FuelAdd",SendMessageOptions.DontRequireReceiver);
					GameObject.Find("Audio").SendMessage("CompleteSound");
					if(GV.mUser.FuelMax  == GV.mUser.FuelCount ) UserDataManager.instance.FuelTimeCountStop();
				}
			});
		}
	}


	IEnumerator BuyFuelCount_Delay(){
		yield return new WaitForSeconds(0.5f);
		if(GV.mUser.FuelCount >= GV.mUser.FuelMax){
			gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(false);
			var mPop = gameObject.transform.FindChild("Content_Fail").gameObject;
			mPop.SetActive(true);
			gameObject.GetComponent<TweenAction>().doubleTweenScale(mPop);
			var icon = 	mPop.transform.FindChild("icon_product") as Transform;
			icon.gameObject.SetActive(false);
			mPop.transform.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("71107");
			mPop.transform.FindChild("lbName").GetComponent<UILabel>().text =
				KoStorage.GetKorString("71108");
			//string.Format("{0} 다 찼다 ",1);//string.Format(KoStorage.GetKorString("72315"),1);
			mPop.transform.FindChild("btnok").GetComponentInChildren<UILabel>().text = 
				KoStorage.GetKorString("71000");
			yield break;
		}else{
			bool b = CheckBuyMoney(1, BuyPrice);
			/*ResponseFinish(b, (bSucc)=>{
				if(bSucc){
					GV.mUser.FuelCount+= BuyCount;
					var lobby = GameObject.Find("LobbyUI") as GameObject;
					lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
					lobby.SendMessage("PlusBuyedItem",3, SendMessageOptions.DontRequireReceiver);
					lobby.SendMessage("FuelAdd",SendMessageOptions.DontRequireReceiver);
					GameObject.Find("Audio").SendMessage("CompleteSound");
					if(GV.mUser.FuelMax  == GV.mUser.FuelCount ) UserDataManager.instance.FuelTimeCountStop();
				}
			});*/
		
			if(!b){
				ResponseFinish(false, (bSucc)=>{
					if(!bSucc) return;
				});
				Global.isNetwork = false;
				yield break;
			}else{
			
				bool bWait = true;
				NetworkManager.instance.BuyItemConnect(false, BuyID, (request)=>{
					Utility.ResponseLog(request.response.Text, GV.mAPI);
					var thing = SimpleJSON.JSON.Parse(request.response.Text);
					int status = thing["state"].AsInt;
					if (status == 0)
					{
						ResponseFinish(true, (bSucc)=>{
							if(bSucc){
								GV.mUser.FuelCount+= BuyCount;
								var lobby = GameObject.Find("LobbyUI") as GameObject;
								lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
								lobby.SendMessage("PlusBuyedItem",3, SendMessageOptions.DontRequireReceiver);
								lobby.SendMessage("FuelAdd",SendMessageOptions.DontRequireReceiver);
								GameObject.Find("Audio").SendMessage("CompleteSound");
								if(GV.mUser.FuelMax  == GV.mUser.FuelCount ) UserDataManager.instance.FuelTimeCountStop();
							}
						});
					}else{
						
					}
					bWait = false;
				});
				
				while(bWait){
					yield return null;
				}
				Global.isNetwork = false;
			}
			
			
			
			}
		

		}





	
	
}
