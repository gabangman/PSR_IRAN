using UnityEngine;
using System.Collections;

public class BuyCash : BuyItem {

	public override void SetBuyPopUp (string BuyName)
	{
		base.SetBuyPopUp (BuyName);
		var Pop = transform.FindChild("Content_BUY").gameObject as GameObject;
		Pop.transform.FindChild("btnCash").gameObject.SetActive(true);
		Pop.transform.FindChild("btnCash").FindChild("lbOk").GetComponent<UILabel>().text
			= KoStorage.GetKorString("71013");
		Pop.transform.FindChild("lbPrice").GetComponent<UILabel>().text = string.Empty;
		var go = Pop.GetComponent<GoogleInAppProc>() as GoogleInAppProc;
		if(go == null) Pop.AddComponent<GoogleInAppProc>();
		OnBuyClick =()=>{
			Global.isNetwork = true;
			Utility.LogWarning("OnCash Clicked ");
			StartCoroutine("BuyCashCount_Delay");
		};
	}

	IEnumerator processPay(int buyID){
	var pop = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
	pop.StartCoroutine("buyLoading");
	int tempid = Base64Manager.instance.GlobalEncoding(Global.gSale);
	buyID += tempid;
	string buyItemId = string.Empty;
	switch(buyID){
	case 8502:
		buyItemId = "c8502"; //8514 100 120
		break;
	case 8503 : 
			buyItemId = "c8503"; // 8515 210 252
		break;
	case 8504:
			buyItemId = "c8504"; // 8516  441 529
		break;
	case 8505:
			buyItemId = "c8505"; //8517 1157 1389
		break;
	case 8506:
			buyItemId = "c8506"; //8518 2431 2917
		break;
	case 8515:
		buyItemId = "c8515"; //8514 100 120
		break;
	case 8516 : 
			buyItemId = "c8516"; // 8515 210 252
			break;
	case 8517:
			buyItemId = "c8517"; // 8516  441 529
			break;
	case 8518:
			buyItemId = "c8518"; //8517 1157 1389
			break;
	case 8519:
			buyItemId = "c8519"; //8518 2431 2917
			break;
	}
	bool bWait = true;
	int mFail =  0;

	#if UNITY_ANDROID && !UNITY_EDITOR

	UserDataManager.instance.isPause = true;
	bWait = true;
	var go =  transform.FindChild("Content_BUY").GetComponent<GoogleInAppProc>() as GoogleInAppProc;
	go.QueryInventory(buyItemId, ()=>{
			mFail = 0;
			bWait = false;
	},()=>{
			mFail = 1;
			bWait = false;
	});
	while(bWait){
		yield return null;
	}

	bWait = true;
	if(mFail == 0){
			if(string.IsNullOrEmpty(Global.orderID)){
				mFail = 1;
			}else{
				if(Global.orderID.Contains("GPA.") == false){
					
					/*if( string.Equals(Global.packageName,"com.gabangmanstudio.pitstopracing") == true){
						if(Global.productId.Contains(buyItemId) == true){
						}else{
							mFail = 1;
						}
					}else{
						
						mFail = 1;
					}*/
					mFail = 1;
					
				}else{
					if(Global.orderID.Length < 26  && Global.orderID.Length >22){
						if( string.Equals(Global.packageName,"com.gabangmanstudio.pitstopracing") == true){
							if(Global.productId.Contains(buyItemId) == true){
								
							}else{
								mFail = 1;
							}
						}else{
							
							mFail = 1;
						}
					}else{
						mFail = 1;
					}
				}
			}
			bWait = false;
	}else{
		bWait = false;	
	} // mFail if
	while(bWait){
		yield return null;
	}
	UserDataManager.instance.isPause = false;
	#else
	mFail = 0;
	#endif
		bWait = true;
	if(mFail == 1){
		OnBuyCashFailed();
		bWait = false;
	}else{
		Common_Cost.Item its = Common_Cost.Get(buyID);
			NetworkManager.instance.BuyItemConnect(true, buyID, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					ResponseCashFinish( (bSucc)=>{
						if(!bSucc) return;
						GV.myCoin += BuyCount;
						GV.updateCoin =  -BuyCount;
						GV.vipExp+=  its.Cash_Exp;
						ChangeVIPLevel();
						var lobby = GameObject.Find("LobbyUI") as GameObject;
						lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
						lobby.SendMessage("PlusBuyedItem",3, SendMessageOptions.DontRequireReceiver);
						GameObject.Find("Audio").SendMessage("CompleteSound");
						GoogleAnalyticsV4.instance.LogEvent("buyCashItem", "cash", buyID.ToString(),0);
					});
				}else{
					
				}
				bWait = false;
				pop.isBuyWait = false;
				pop.BuyLoadingStopCoroutine("buyLoading");
				Destroy(pop);
				pop = null;
			}, Global.orderID, 1, BuyPrice, GV.gNationName);
	}

	while(bWait){
		yield return null;
	}
		if(pop != null){
			pop.isBuyWait = false;
			pop.BuyLoadingStopCoroutine("buyLoading");
			Destroy(pop);
			pop = null;
		}
}



	void 	OnBuyCashFailed(){
		gameObject.transform.FindChild("Content_BUY").gameObject.SetActive(false);
		var _parent = gameObject.transform.FindChild("Content_Fail").gameObject;
		_parent.SetActive(true);
		gameObject.GetComponent<TweenAction>().doubleTweenScale(_parent);
		var icon = 	_parent.transform.FindChild("icon_product") as Transform;
		icon.gameObject.SetActive(false);
		_parent.transform.FindChild("lbText").GetComponent<UILabel>().text =
			KoStorage.GetKorString("72008");
		_parent.transform.FindChild("lbName").GetComponent<UILabel>().text =
			KoStorage.GetKorString("72009");
		_parent.transform.FindChild("btnok").GetComponentInChildren<UILabel>().text = 
			KoStorage.GetKorString("71000");
		Global.isNetwork = false;
	}

	private void BuyCashCount(){
		if(GV.gInfo.extra05 == 0){
			StartCoroutine("processPay",BuyID);
		}else{
			StartCoroutine("processPay2",BuyID);
		}
	}
	
	
	IEnumerator BuyCashCount_Delay(){
		yield return new WaitForSeconds(1.0f);
		BuyCashCount();
		
	}


	IEnumerator processPay2(int buyID){
		var pop = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
		pop.StartCoroutine("buyLoading");
		int tempid = Base64Manager.instance.GlobalEncoding(Global.gSale);
		buyID += tempid;
		string buyItemId = string.Empty;
		switch(buyID){
		case 8502:
			buyItemId = "c8502"; //8514 100 120
			break;
		case 8503 : 
			buyItemId = "c8503"; // 8515 210 252
			break;
		case 8504:
			buyItemId = "c8504"; // 8516  441 529
			break;
		case 8505:
			buyItemId = "c8505"; //8517 1157 1389
			break;
		case 8506:
			buyItemId = "c8506"; //8518 2431 2917
			break;
		case 8515:
			buyItemId = "c8515"; //8514 100 120
			break;
		case 8516 : 
			buyItemId = "c8516"; // 8515 210 252
			break;
		case 8517:
			buyItemId = "c8517"; // 8516  441 529
			break;
		case 8518:
			buyItemId = "c8518"; //8517 1157 1389
			break;
		case 8519:
			buyItemId = "c8519"; //8518 2431 2917
			break;
		}
		bool bWait = true;
		int mFail =  0;
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		
		UserDataManager.instance.isPause = true;
		bWait = true;
		var go =  transform.FindChild("Content_BUY").GetComponent<GoogleInAppProc>() as GoogleInAppProc;
		go.QueryInventory(buyItemId, ()=>{
			mFail = 0;
			bWait = false;
		},()=>{
			mFail = 1;
			bWait = false;
		});
		while(bWait){
			yield return null;
		}
		
		bWait = true;
		if(mFail == 0){
			if(string.IsNullOrEmpty(Global.orderID)){
				mFail = 1;
			}else{
				if(Global.orderID.Contains("GPA.") == false){
					
					/*if( string.Equals(Global.packageName,"com.gabangmanstudio.pitstopracing") == true){
						if(Global.productId.Contains(buyItemId) == true){
						}else{
							mFail = 1;
						}
					}else{
						
						mFail = 1;
					}*/
					mFail = 1;
					
				}else{
					if(Global.orderID.Length < 26  && Global.orderID.Length >22){
						if( string.Equals(Global.packageName,"com.gabangmanstudio.pitstopracing") == true){
							if(Global.productId.Contains(buyItemId) == true){
								
							}else{
								mFail = 1;
							}
						}else{
							
							mFail = 1;
						}
					}else{
						mFail = 1;
					}
				}
			}
			bWait = false;
		}else{
			bWait = false;	
		} // mFail if
		while(bWait){
			yield return null;
		}
		UserDataManager.instance.isPause = false;
		#else
		mFail = 0;
		#endif
		bWait = true;
		if(mFail == 1){
			OnBuyCashFailed();
			bWait = false;
		}else{
			Common_Cost.Item its = Common_Cost.Get(buyID);
			NetworkManager.instance.BuyPackageItemConnectAndroid(0, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					ResponseCashFinish( (bSucc)=>{
						if(!bSucc) return;
						GV.myCoin += BuyCount;
						GV.updateCoin =  -BuyCount;
						GV.vipExp+=  its.Cash_Exp;
						ChangeVIPLevel();
						var lobby = GameObject.Find("LobbyUI") as GameObject;
						lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
						lobby.SendMessage("PlusBuyedItem",3, SendMessageOptions.DontRequireReceiver);
						GameObject.Find("Audio").SendMessage("CompleteSound");
						GoogleAnalyticsV4.instance.LogEvent("buyCashItem", "cash", buyID.ToString(),0);
					});
				}else{
					
				}
				bWait = false;
				pop.isBuyWait = false;
				pop.BuyLoadingStopCoroutine("buyLoading");
				Destroy(pop);
				pop = null;
			}, GV.signedData, GV.signature);
		}
		
		while(bWait){
			yield return null;
		}
		if(pop != null){
			pop.isBuyWait = false;
			pop.BuyLoadingStopCoroutine("buyLoading");
			Destroy(pop);
			pop = null;
		}
	}

}
