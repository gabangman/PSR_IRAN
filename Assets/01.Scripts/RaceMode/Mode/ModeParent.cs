using UnityEngine;
using System.Collections;


public class ModeParent : MonoBehaviour {
	public delegate void OnNextClick();
	public OnNextClick onNext;

	public delegate void OnRefreshClick();
	public OnRefreshClick onRefresh;

	public virtual void OnSubWindow(){
		GameObject.Find("Audio").SendMessage("ChangeBGMMusic", false, SendMessageOptions.DontRequireReceiver);
		//transform.GetComponent<TweenPosition>().Reset();
		//transform.GetComponent<TweenPosition>().enabled = true;
	}
	protected bool bStart = false;
	protected int refreshDollar;
	protected int entryDollar;
	protected bool bReError = false;
	protected bool RaceReadyCheckMoney(int money){
		bool b = false;
		int dec = GV.myDollar - money;
		if(dec < 0) {
			b = true;
		
		}else{
			GV.myDollar -= money;
			GV.updateDollar = money;
			GameObject.Find("LobbyUI").SendMessage("InitTopMenu",SendMessageOptions.DontRequireReceiver);
		}
		if(b){
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			pop.AddComponent<enoughMoneyPopup>().InitPopUp();
			StopLoadingCircle();
		}
		return b;
	}

	public UILabel[] lbText;

	protected void Start(){
		if(lbText.Length == 0) return;
		lbText[0].text =KoStorage.GetKorString("73005");
		if(lbText[2] != null) lbText[2].text =lbText[0].text ;
		lbText[1].text =  KoStorage.GetKorString("73006");
		if(lbText[3] != null) lbText[3].text =lbText[1].text ;

	}

	public void OnNext(GameObject clickBtn){
	
		if(onNext != null)
		{
			if(Global.isNetwork){
				return;}
			Global.isNetwork = true;
			if(UserDataManager.instance.raceFuelCountCheck()) return;
			if(RaceReadyCheckMoney(entryDollar))	return;
			UserDataManager.instance.raceFuelCounting();
			if(bStart) return;
			GameObject.Find("Audio").SendMessage("CompleteSound", SendMessageOptions.DontRequireReceiver);
			onNext();
		}
	}

	protected void RaceNotEnoughEntry(int entryFee){
		
	}
	public void OnRefresh(){
		if(onRefresh != null)
		{	
			if(Global.isNetwork) return;
			//Global.isNetwork = true;
			if(RaceReadyCheckMoney(refreshDollar)) return;
			onRefresh();
		}
	}

	void OnDisable(){
		onNext = null; onRefresh = null;
		for(int i = 0; i < transform.childCount;i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}

	}

	protected void ReadyLoadingCircle(){
		Global.isNetwork = true;
		var loading = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
		loading.StartCoroutine("StartRaceLoading", false);//raceStartLoading
	}

	protected void LoadingCircle(){
		Global.isNetwork = true;
		var loading = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
		loading.StartCoroutine("StartRaceLoading", true);
		StartCoroutine("TenSecondTimeCheck");
	}
	
	protected void StopLoadingCircle(){
		Global.isNetwork = false;
		var loading = gameObject.GetComponent<LoadingGauage>() as LoadingGauage;
		if(loading != null)
			loading.stopLoading();
		StopCoroutine("TenSecondTimeCheck");
	}

	public virtual IEnumerator TenSecondTimeCheck(){
		WaitForSeconds w = new WaitForSeconds(1.0f);
		
		for(int i = 0; i < 8; i++){
			yield return w;
		}
		StopLoadingCircle();
		bReError = true;
		GameObject.Find("LobbyUI").SendMessage("OnBackClick",SendMessageOptions.DontRequireReceiver);
		yield return null;
	}

	void OnEnable(){
		onNext = null; onRefresh = null;
		Global.isNetwork = false;	
		bStart = false;
		//GameObject.Find("Audio").SendMessage("ChangeBGMMusic", false, SendMessageOptions.DontRequireReceiver);
		//GameObject.Find("Audio").SendMessage("ChangeBGMMusic", false, SendMessageOptions.DontRequireReceiver);
	/*	string subName = gameObject.name;
	//	var storeRaceInfo =GetComponent<StoreRaceInfo>() as StoreRaceInfo;
	//	if(storeRaceInfo == null) storeRaceInfo = gameObject.AddComponent<StoreRaceInfo>();
		switch(subName){
		case "Champion":
			break;
		case "Ranking":
			break;
		case "Event":
			break;
		case "Regular":
			break;
		case "Clan":
			break;
		}*/
	}

	protected void SetBGTrackImg(int TrackID){
		transform.parent.FindChild("BG").GetComponent<UISprite>().spriteName = TrackID.ToString()+"B";
	}



	public virtual void MakeTimeDelayPopup(int idx){
	}




}
