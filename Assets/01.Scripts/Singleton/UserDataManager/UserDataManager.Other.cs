using UnityEngine;
using System.Collections;

public partial class UserDataManager : MonoSingleton< UserDataManager > {

	public bool buyPriceCheck(int dollar){
		bool b = false;
		int dec = GV.myDollar - dollar;
		if(dec < 0) b = true;
		if(b){
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			pop.AddComponent<enoughMoneyPopup>().InitDollarPopUp();
		}
		return b;
	}
	public bool buyPriceCheckCoin(int coin){
		bool b = false;
		int dec = GV.myCoin - coin;
		if(dec < 0) b = true;
		if(b){
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			pop.AddComponent<enoughMoneyPopup>().InitCoinPopUp();
		}
		return b;
	}
	public  bool raceFuelCountCheck(){
		bool isFuel = false;
		if(GV.mUser.FuelCount == 0) {
			isFuel = true;
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			pop.AddComponent<emptyFuelPopup>().InitPopUp();
			Global.isNetwork = false;
		}
		return isFuel;
	}
	
	public void raceFuelCounting(){
		if(GV.mUser.FuelCount == GV.mUser.FuelMax){
			Invoke("FuelTimeCount", (float)FUELTIME);
		}else{
			//GV.mUser.FuelCount--;
			//myAcc.instance.account.lastConnectTime = NetworkManager.instance.GetCurrentDeviceTime().Ticks;
		}
		GV.mUser.FuelCount--;

		myAcc.instance.account.lastConnectTime = NetworkManager.instance.GetCurrentDeviceTime().Ticks;
		GameObject.Find("LobbyUI").SendMessage("DecreaseFuelCount");
		GameObject.Find("Audio").SendMessage("StartButtonPress");
	}
	public void checkFuelCountInvoke(){
		if(IsInvoking("FuelTimeCount")) return;
		fuelTimeStart();
	
	}
	
	public void FuelTimeCountStop(){
		if(IsInvoking("FuelTimeCount"))
			CancelInvoke("FuelTimeCount");
	}

	System.DateTime mCurrentTime;
	System.DateTime fTime;
	System.TimeSpan remainTime;
	private readonly int FUELTIME = 600;
	public bool CheckCoroutineFuel(){
		return IsInvoking("FuelTimeCount");
	}
	public void fuelTimeStart(){
		int max = GV.mUser.FuelMax;
		int cnt = GV.mUser.FuelCount;
		if(max == cnt){
			return;
		
		}else{
			remainTime = NetworkManager.instance.GetCurrentDeviceTime() - new System.DateTime(myAcc.instance.account.lastConnectTime);//System.DateTime.Now - fTime;
			int mCnt = max - cnt;
			mCnt = mCnt * FUELTIME;
			//Utility.LogWarning("time " + remainTime);
			if(remainTime.TotalSeconds > mCnt){
				GV.mUser.FuelCount = GV.mUser.FuelMax;
			
			}else{
				GV.fuelTime = (int)remainTime.TotalSeconds;
				int a = GV.fuelTime / FUELTIME;
				int b = GV.fuelTime %FUELTIME;
				GV.mUser.FuelCount += a;
				b = FUELTIME - b;
				if(b<0) Utility.LogWarning("b is Minus");
				Invoke("FuelTimeCount", (float)b);
				//Utility.LogWarning("TimeCount " + b);
			}
		}

	}
	void FuelTimeCount(){
		GV.mUser.FuelCount++;
		var lobby = GameObject.Find("LobbyUI") as GameObject;
		if(lobby!=null) lobby.SendMessage("FuelAdd");
		if(GV.mUser.FuelCount >= GV.mUser.FuelMax){
			GV.mUser.FuelCount = GV.mUser.FuelMax;
			//myAcc.instance.account.lastConnectTime = NetworkManager.instance.GetCurrentDeviceTime().Ticks;
		}else{
			//myAcc.instance.account.lastConnectTime = NetworkManager.instance.GetCurrentDeviceTime().Ticks;
			Invoke("FuelTimeCount", (float)FUELTIME);
		}
		Utility.LogWarning("TimeCount Finish " );
	}


	public void SetPhoneNumber(string param){
		//myAccount.instance.account.mydeviceinfo.PhoneNumber = param.Trim();
	}
	public void SetCountryCode(string param){
		myAccount.instance.account.mydeviceinfo.CountryCode = param.Trim();;
	}

	public void SetPushID(string param){
		Global.gPushID = param;
		myAccount.instance.account.mydeviceinfo.PushID =param.Trim();
		Global.isNetwork = false;
	}

	private bool bTap = false;
	public IEnumerator CheckTapJoyCurrency(){
		//==!!Utility.LogWarning("CheckTapJoy");
		
		if(Application.isEditor){
			yield return new WaitForSeconds(1.5f);
			if(isTapjoy){
				GV.gTapJoy = 1;
				GameObject.Find("LobbyUI").SendMessage("SetTapJoyBalanceCheck");
				isTapjoy = false;
			}
			yield break;
		}
		
		TapjoyUnity.Tapjoy.Connect();
		
		TapjoyUnity.Tapjoy.OnGetCurrencyBalanceResponse += HandleGetCurrencyBalanceResponse;
		TapjoyUnity.Tapjoy.OnGetCurrencyBalanceResponseFailure += HandleGetCurrencyBalanceResponseFailure;
		//현재 벌어들인 금액
		TapjoyUnity.Tapjoy.OnEarnedCurrency += HandleEarnedCurrency;
		// 소비 금액
		TapjoyUnity.Tapjoy.OnSpendCurrencyResponse += HandleSpendCurrencyResponse;
		TapjoyUnity.Tapjoy.OnSpendCurrencyResponseFailure += HandleSpendCurrencyResponseFailure;
		bTap = false;
		
		if(TapjoyUnity.Tapjoy.IsConnected){
			TapjoyUnity.Tapjoy.GetCurrencyBalance();
		}else{
			GV.gTapJoy = 0;
			TapjoyUnity.Tapjoy.OnGetCurrencyBalanceResponse -= HandleGetCurrencyBalanceResponse;
			TapjoyUnity.Tapjoy.OnGetCurrencyBalanceResponseFailure -= HandleGetCurrencyBalanceResponseFailure;
			TapjoyUnity.Tapjoy.OnEarnedCurrency -= HandleEarnedCurrency;
			TapjoyUnity.Tapjoy.OnSpendCurrencyResponse -= HandleSpendCurrencyResponse;
			TapjoyUnity.Tapjoy.OnSpendCurrencyResponseFailure -= HandleSpendCurrencyResponseFailure;
			bTap = true;
			yield break;
		}
		
		while(!bTap){
			yield return null;
		}
		
		TapjoyUnity.Tapjoy.OnGetCurrencyBalanceResponse -= HandleGetCurrencyBalanceResponse;
		TapjoyUnity.Tapjoy.OnGetCurrencyBalanceResponseFailure -= HandleGetCurrencyBalanceResponseFailure;
		//현재 벌어들인 금액
		TapjoyUnity.Tapjoy.OnEarnedCurrency -= HandleEarnedCurrency;
		// 소비 금액
		TapjoyUnity.Tapjoy.OnSpendCurrencyResponse -= HandleSpendCurrencyResponse;
		TapjoyUnity.Tapjoy.OnSpendCurrencyResponseFailure -= HandleSpendCurrencyResponseFailure;
	}
	
	public void HandleGetCurrencyBalanceResponse(string currencyName, int balance) {
		////==!!Utility.LogWarning("C#: Lobby HandleGetCurrencyBalanceResponse: currencyName: " + currencyName + ", balance: " + balance);
		//GameObject.Find("LobbyUI").SendMessage("SetOfferWallReward", balance, SendMessageOptions.DontRequireReceiver);
		if(balance == 0){
			GV.gTapJoy = 0;
			isTapjoy = false;
		}else{
			GV.gTapJoy = balance;
			if(isTapjoy){
				GameObject.Find("LobbyUI").SendMessage("SetTapJoyBalanceCheck");
				isTapjoy = false;
			}else{
				TapjoyUnity.Tapjoy.SpendCurrency(GV.gTapJoy);
			}
		}
		
	}
	
	public IEnumerator TapJoyRewardSpend(int mCoin){
		if(Application.isEditor){
			////==!!Utility.LogWarning("tapjoy");	
			yield break;
		}
		bTap = false;
		TapjoyUnity.Tapjoy.OnSpendCurrencyResponse += HandleSpendCurrencyResponse;
		TapjoyUnity.Tapjoy.OnSpendCurrencyResponseFailure += HandleSpendCurrencyResponseFailure;
		
		TapjoyUnity.Tapjoy.SpendCurrency(mCoin);
		while(!bTap){
			yield return null;
		}
		TapjoyUnity.Tapjoy.OnSpendCurrencyResponse -= HandleSpendCurrencyResponse;
		TapjoyUnity.Tapjoy.OnSpendCurrencyResponseFailure -= HandleSpendCurrencyResponseFailure;
		
	}
	
	
	public void HandleGetCurrencyBalanceResponseFailure(string error) {
		////==!!Utility.LogWarning("C#: Lobby HandleGetCurrencyBalanceResponseFailure: " + error);
		bTap =true;//	bCircle = false;
		GV.gTapJoy = 0;
		isTapjoy = false;
	}
	
	public void HandleEarnedCurrency(string currencyName, int amount) {
		////==!!Utility.LogWarning("C#: HandleEarnedCurrency: currencyName: " + currencyName + ", amount: " + amount);
		//bTap =true;	bCircle = false;
	}
	
	public void HandleSpendCurrencyResponse(string currencyName, int balance) {
		////==!!Utility.LogWarning("C#: HandleSpendCurrencyResponse: currencyName: " + currencyName + ", balance: " + balance);
		//if(balance != 0) //==!!Utility.LogWarning("Spend Currency " + balance);
		bTap = true;
	}
	
	public void HandleSpendCurrencyResponseFailure(string error) {
		//GameObject.Find("LobbyUI").SendMessage("SetOfferWallReward", -2, SendMessageOptions.DontRequireReceiver);
		//bTap = true;SetClose();
		bTap = true;
		GV.gTapJoy = 0;
		isTapjoy = false;
		//==!!Utility.LogWarning("C#: HandleSpendCurrencyResponseFailure: " + error);
	}
}
