using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GoogleIABEventListener : MonoBehaviour
{
#if UNITY_ANDROID
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
	
	}


	void OnDisable()
	{
		// Remove all event handlers
		GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
	}



	void billingSupportedEvent()
	{
		AccountManager.instance.bGooglePurchasing = false;
		Utility.Log( "billingSupportedEvent: " );
	}


	void billingNotSupportedEvent( string error )
	{
		Utility.Log( "billingNotSupportedEvent: " + error );
	}


	void queryInventorySucceededEvent( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus )
	{
		AccountManager.instance.bGooglePurchasing= false;
		AccountManager.instance.bPurchaseSuccess  = true;
		Utility.Log( string.Format( "queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count ) );
		//Prime31.Utils.logObject( purchases );
		//Prime31.Utils.logObject( skus );


	}


	void queryInventoryFailedEvent( string error )
	{
		//Utility.Log( "queryInventoryFailedEvent: " + error );
		AccountManager.instance.bGooglePurchasing = false;
		AccountManager.instance.bPurchaseSuccess  = false;

	}


	void purchaseCompleteAwaitingVerificationEvent( string purchaseData, string signature )
	{
	//	Utility.Log( "purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature );
	//	Utility.LogWarning("purchaseData : " + purchaseData);
	//	Utility.LogWarning("signature : " + signature);
	}

	void purchaseSucceededEvent( GooglePurchase purchase )
	{
	//	Utility.Log( "purchaseSucceededEvent: " + purchase );
		AccountManager.instance.bGooglePurchasing= false;
		AccountManager.instance.bPurchaseSuccess  = true;
		//GooglePurchase Item = purchase;

	}


	void purchaseFailedEvent( string error )
	{
	//	Utility.Log( "purchaseFailedEvent: " + error );
		AccountManager.instance.bGooglePurchasing = false;
		AccountManager.instance.bPurchaseSuccess = false;
	}


	void consumePurchaseSucceededEvent( GooglePurchase purchase )
	{
		//Utility.Log( "consumePurchaseSucceededEvent: " + purchase );
		AccountManager.instance.bGooglePurchasing = false;
		AccountManager.instance.bPurchaseSuccess = true;
	}

	void consumePurchaseFailedEvent( string error )
	{
	//	Utility.Log( "consumePurchaseFailedEvent: " + error );
		AccountManager.instance.bGooglePurchasing = false;
		AccountManager.instance.bPurchaseSuccess  = false;
	}


#endif
}


