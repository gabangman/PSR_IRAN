using UnityEngine;
using System.Collections;

public class GoogleInAppProc : MonoBehaviour{
	/*public delegate void GetItemAndConsumeItem(string str);
	public GetItemAndConsumeItem buyItem;

	public GoogleInAppProc(){
		var key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAmUvXoqoHcJGbbvaERmJE7fkO3u/BByMRR7jt3j2Lf4uJEnC31jHt/aKMlQ7Hbdxl36P7bY+nCJOyylM0kSLaZoWKGjXouVjdHAqnSXz24c9cxk8/Uv/mtHwYhgwaFk/yO3WJwMlwwpc8OpLPJh8EWUoB2hm0Ic8wBqILUsqYh8bhElbnn+eHZSqFeTgRIOsbBwGPmH0DsbXXOq0VgsKSI0REYGYo7J38NJfkU8MEc3aUKmRH+nnVc5g87RKVzGGNeqoKrJIynAjmH7lFuKeYv/L8E42znynm38NNy1v/yEB3E6nF4oFQUDL26phNzuV+XYnCoYkOIFWtxKe9J+YxtwIDAQAB";
		GoogleIAB.init( key );
	}

	public void PurchasedItem(string str){
		var skus  = new string[]{str};
		GoogleIAB.queryInventory( skus );
		GoogleIAB.purchaseProduct( str );

	}

	public void cosumeItem(string str){
		var skus  = new string[]{str};
		GoogleIAB.queryInventory( skus );
		GoogleIAB.consumeProduct( str );
	}
	*/
	public delegate void OnBuyFail();
	public OnBuyFail onBuyFail;
	public delegate void OnBuySuccess();
	public OnBuySuccess onBuySuccess;

	void Start(){
		#if UNITY_ANDROID && !UNITY_EDITOR
		AccountManager.instance.bGooglePurchasing = true;
		var key ="TQBJAEkAQgBJAGoAQQBOAEIAZwBrAHEAaABrAGkARwA5AHcAMABCAEEAUQBFAEYAQQBBAE8AQwBBAFEAOABBAE0ASQBJAEIAQwBnAEsAQwBBAFEARQBBAHEALwBoAEcAOQAzAHkANAA3AGwAUQBTAHAAQQA0AFMAaQB1AEwAZQBNAE0AbwBMADAAWgBSAHoAZgBsAFcAeABtAEIAaAB4AFcAZwB0AGIATgBJAEsAUwBrAHIAWgA5AFEARwB4AGsAQwBhAFEAUABCADMATABnAEwAaQBRAHIASwBoAHoAVAB5ADUAOAAyAEgALwBhAFgAVQB5AEQAOAAyAFAAWQBCAGYAVwAyADIAQgBTAEkATQBKADYAWQBVAFUAYwA5AHcASQAvAEEAMgA0AGMAeAB2AFMAMABtADkAbQBIAGwASABJAGkAZgBBADUAYwA2AE4AVgBjAHoASwA2AFUANgBpAGwAMwBuAFMAUgBCAGIARgBDAEMAUAA3AEQAUABCAHEAZQBBAEcAZQB2AEkANgBrAHkANABDAEgAUwBNAEwAVAAzAFEAeQBHAFoAUQBOADUANwB6ADUAMgAzAHAAMQB1AEIAdABNAEcAdAAxADQAMwBiAGkAcwBxAEEAeAArADIAKwBoAFYAQgAvADAAWQB6ADQAdgArAEIAKwBRAE0ANwBsAFoASgBCAFQAUgBEAEsAVQBYAEUAdwBsAEgALwAvAHYAYgAyAGkAVABoAFQAQgBGAGQASwBZADUASgBjAEoAbgBCAGYAcABjADgAMQBZADEAMgB5AFgANAB0AEwAOQAzADQAbgB4AHUAdgBXAFEAZABCAHUAUwB4AHcAYwBwADgATQBOAGkAUgBGAFAAdgBaAGQAKwBjAEIAUwByAEsAYQBJAHcAdwA0AFoARwBLAFkAQgAwAEwAVgAzADYATgBHAGsAOQBjAGMAMwB5AHIATQBRAFQANABBAE4AegAyADIAVQBJAFcAUAB2AFcAQQBYADYAQgBYAFcAVgBRAEgARgB3AEkARABBAFEAQQBCAA==";
		GV.pKey = key;
		key = Base64Manager.instance.getStringDecodeBase(key, System.Text.Encoding.Unicode);
		GoogleIAB.init( key );
		#else 
			return;
		#endif

	}

	#if UNITY_ANDROID && !UNITY_EDITOR
	public void QueryInventory(string strItem, OnBuySuccess onbuy, OnBuyFail onfail){
		if( Application.platform == RuntimePlatform.Android ){
			onBuySuccess = onbuy;
			onBuyFail = onfail;
			StartCoroutine("QueryItem", strItem);
		}else return;

	}

	IEnumerator QueryItem(string strItem){

		while(AccountManager.instance.bGooglePurchasing){
			yield return null;
		}
		var skus = new string[]{strItem};
		AccountManager.instance.bGooglePurchasing = true;
		GoogleIAB.queryInventory(skus);
		while(AccountManager.instance.bGooglePurchasing){

			yield return null;
		}
		if(!AccountManager.instance.bPurchaseSuccess ){
			onBuyFail();
			yield break;
		}
		GoogleIAB.purchaseProduct(strItem);
		AccountManager.instance.bGooglePurchasing = true;
		while(AccountManager.instance.bGooglePurchasing){
			yield return null;
		}
		if(!AccountManager.instance.bPurchaseSuccess ){
			onBuyFail();
			yield break;
		}
		GoogleIAB.consumeProduct(strItem);
		AccountManager.instance.bGooglePurchasing = true;
		while(AccountManager.instance.bGooglePurchasing){
			yield return null;
		}
		if(!AccountManager.instance.bPurchaseSuccess){
			onBuyFail();
			yield break;
		}
		onBuySuccess();
	}

	public void PurchaseItem(string strItem){
		if( Application.platform == RuntimePlatform.Android ){
			GoogleIAB.purchaseProduct(strItem);
		}else return;


	}

	public void CosumeItem(string strItem){
		if( Application.platform == RuntimePlatform.Android ){
			GoogleIAB.consumeProduct(strItem);
		}else return;
	}
	#endif



}
