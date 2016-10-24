using UnityEngine;
using System.Collections;

public class RaceMenuStart : MonoBehaviour {
	public GameObject[] raceMenus;
	public Animation raceAnimation, raceBackAnimation;
	private string raceString;
	private string racePrevString;
	private bool bAni;
	private int aniMode;
	public RaceEventPopup ePopups;
	public Transform[] raceScale;
	public float BigScaleduration = 0.5f,SmallScaleduration =0.5f ;
	public Vector3 BigTo, BigFrom;
	public Vector3 SmallTo, SmallFrom;

	void Awake () {
		raceAnimation = gameObject.GetComponent<Animation>() as Animation;
	//	raceBackAnimation = gameObject.GetComponent<Animation>() as Animation;
		raceString = string.Empty;
		racePrevString = string.Empty;
		bAni = false;
	}

	int regularTrack(){
		int tId = 0;
		
		if(GV.ChSeasonID >= 6000 && GV.ChSeasonID < 6005 ){
			tId = 0;
		}else if(GV.ChSeasonID >= 6005 && GV.ChSeasonID < 6010 ){
			tId = 1401;
			
		}else if(GV.ChSeasonID >= 6010 && GV.ChSeasonID < 6015 ){
			tId = Random.Range(1401,1403); //1402
			for(int i = 0; i < 50; i++){
				if(GV.EventTrack == tId){
					tId = Random.Range(1401,1403); //1402
				}else{
					break;
				}
			}
		}else if(GV.ChSeasonID >= 6015 && GV.ChSeasonID < 6020 ){
			tId = Random.Range(1401,1404); //1403
			for(int i = 0; i < 50; i++){
				if(GV.EventTrack == tId){
					tId = Random.Range(1401,1404); //1403
				}else{
					break;
				}
			}
		}else if(GV.ChSeasonID >= 6020 && GV.ChSeasonID < 6025 ){
			tId = Random.Range(1401,1405); //1404
			for(int i = 0; i < 50; i++){
				if(GV.EventTrack == tId){
					tId = Random.Range(1401,1405); //1
				}else{
					break;
				}
			}
		}else if(GV.ChSeasonID >= 6025 && GV.ChSeasonID < 6030 ){
			tId = Random.Range(1401,1406); //1405
			for(int i = 0; i < 50; i++){
				if(GV.EventTrack == tId){
					tId = Random.Range(1401,1406); //1405
				}else{
					break;
				}
			}
		}else{
			tId = Random.Range(1401,1407); //1406
			for(int i = 0; i < 50; i++){
				if(GV.EventTrack == tId){
					tId =Random.Range(1401,1407); //1406
				}else{
					break;
				}
			}
		}
		return tId;
	}

	void Start(){
		GV.EventTrack = myAccount.instance.account.eRace.featuredTrackID;
		GV.RegularTrack =regularTrack();
		raceMenus[0].GetComponent<RaceSubItem1400>().OnMap1400();
		raceMenus[1].GetComponent<RaceSubItem1412>().OnMap1412();
		raceMenus[2].GetComponent<RaceSubItem1413>().OnMap1413();
		raceMenus[3].GetComponent<RaceSubItem1401>().OnMap1401();
		raceMenus[4].GetComponent<RaceSubItem1402>().OnMap1402();
		raceMenus[5].GetComponent<RaceSubItem1403>().OnMap1403();
		raceMenus[6].GetComponent<RaceSubItem1404>().OnMap1404();
		raceMenus[7].GetComponent<RaceSubItem1405>().OnMap1405();
		raceMenus[8].GetComponent<RaceSubItem1406>().OnMap1406();
		aniMode = 0;
	
	}
	public void ResetAnimation(){
	
		bAni = false;
		UserDataManager.instance.OnSubBackMenu = null;
		raceMenus[2].GetComponent<RaceSubItem1413>().ClubInit();
		if(aniMode == 3){
			aniMode = 0;
			StartCoroutine(RaceSubMenuReverseAniPlay("Modemenu_OpenClose", string.Empty));
			return;
		}

		if(CClub.ClubClick == 1){
			CClub.ClubClick = 0;
			//OpenClubMode();
			for(int i = 0; i < 9; i++){
				raceMenus[i].SetActive(false);
				raceMenus[i].transform.localScale = new Vector3(0.01f, 0.01f, 1);
			}
			raceAnimation["Open_Club"].time = 0;
			raceAnimation["Open_Club"].speed = 1;
			raceAnimation.Play("Open_Club");
			ePopups.ClubModeInitialize();	
			return;
		}else if(CClub.ClubClick ==2){
			CClub.ClubClick = 0;
			if(	UserDataManager.instance.OnSubBackMenu != null) UserDataManager.instance.OnSubBackMenu = null;
			UserDataManager.instance.OnSubBackMenu = ()=>{
				returnMenuClub();
			};
			return;
		}else if(CClub.ClubClick == 5){
			CClub.ClubClick = 0;
			ePopups.initClub();
			for(int i = 0; i < 9; i++){
				raceMenus[i].SetActive(true);
				raceMenus[i].transform.localScale = Vector3.one;
			}
		}
	}

	int preIndex = 10;
	IEnumerator modeInfoReverseAniEx(string strAni, int id){
		bAni = true;
		Global.isNetwork = true;
		raceAnimation[strAni].time = raceAnimation[strAni].length;
		raceAnimation[strAni].speed = -3;
		raceAnimation.Play(strAni);
		while(raceAnimation.IsPlaying(strAni)){
			yield return null;
		}

		raceMenus[id].SendMessage("SetDisable");
		var twscale = raceScale[id].gameObject.AddComponent<TweenScale>() as TweenScale;
		raceScale[id].gameObject.SetActive(true);
		twscale.to = SmallTo;
		twscale.from = SmallFrom;
		twscale.Reset();
		twscale.enabled = true;
		twscale.duration =SmallScaleduration;
		twscale.method = UITweener.Method.EaseInOut;
		twscale.onFinished = delegate(UITweener ui) {
			bAni = false;
			Global.isNetwork = false;
			Destroy(twscale);
		}; 

	}

	IEnumerator modeInfoForwardAniEx(string strAni, int id){
		bAni = true;
		Global.isAnimation = true;
		//Global.isNetwork = true;
		bool b = false;
		var twscale = raceScale[id].gameObject.AddComponent<TweenScale>() as TweenScale;

		twscale.from = BigFrom;
		twscale.to = BigTo;
		twscale.Reset();
		twscale.duration = BigScaleduration;
		twscale.enabled = true;
		twscale.method = UITweener.Method.EaseInOut;
		twscale.onFinished = delegate(UITweener ui) {
			raceScale[id].gameObject.SetActive(false);
			b = true;
			Destroy(twscale);

		};
		while(!b){
			yield return null;
		} 
		raceAnimation[strAni].time = 0;
		raceAnimation[strAni].speed = 1;
		raceAnimation.Play(strAni);
		while(raceAnimation.IsPlaying(strAni)){
			yield return null;
		}

		bAni = false;
		Global.isAnimation = false;

	}

	void On_1400(){
		if(Global.isNetwork || Global.isAnimation) return;
		if(raceMenus[0].GetComponent<RaceSubItems>().OnLockBool) return;
		if(bAni) return;
		bAni = true;
		if(string.IsNullOrEmpty(racePrevString) == true){
			StartCoroutine(modeInfoForwardAniEx("Open_1400",0));
		}else{
			StartCoroutine(modeInfoReverseAniEx(racePrevString, preIndex));
			StartCoroutine(modeInfoForwardAniEx("Open_1400",0));
		
		}
		racePrevString = "Open_1400";
		preIndex = 0;
		return;
	
	}
	void On_1412(){
		if(Global.isNetwork || Global.isAnimation) return;
		if(raceMenus[1].GetComponent<RaceSubItems>().OnLockBool) return;
		if(bAni) return;
		if(string.IsNullOrEmpty(racePrevString) == true){
			StartCoroutine(modeInfoForwardAniEx("Open_1412",1));
		}else{
			StartCoroutine(modeInfoReverseAniEx(racePrevString, preIndex));
			StartCoroutine(modeInfoForwardAniEx("Open_1412",1));
		
		}
		racePrevString = "Open_1412";
		preIndex = 1;
	}
	void On_1413(){
		if(Global.isNetwork || Global.isAnimation) return;
		if(raceMenus[2].GetComponent<RaceSubItems>().OnLockBool) return;
		if(bAni) return;


		if(string.IsNullOrEmpty(racePrevString) == true){
			StartCoroutine(modeInfoForwardAniEx("Open_1413",2));
		}else{
			StartCoroutine(modeInfoReverseAniEx(racePrevString, preIndex));
			StartCoroutine(modeInfoForwardAniEx("Open_1413",2));

		}
		racePrevString = "Open_1413";
		preIndex = 2;


	
	}
	void On_1401(){
		if(Global.isNetwork || Global.isAnimation) return;
		if(raceMenus[3].GetComponent<RaceSubItems>().OnLockBool) return;
		if(bAni) return;
		if(GV.ChSeasonID < 6005){
			raceString ="Open_1401";
		}else{
			raceString ="Open_1401_R";
		}


		if(string.IsNullOrEmpty(racePrevString) == true){
			StartCoroutine(modeInfoForwardAniEx(raceString,3));
		}else{
			StartCoroutine(modeInfoReverseAniEx(racePrevString, preIndex));
			StartCoroutine(modeInfoForwardAniEx(raceString,3));
		
	
		}
		racePrevString =raceString;
		preIndex = 3;



	
	}
	void On_1402(){
		if(Global.isNetwork || Global.isAnimation) return;
		if(raceMenus[4].GetComponent<RaceSubItems>().OnLockBool) return;
		if(bAni) return;

		if(GV.ChSeasonID < 6010){
			raceString ="Open_1402";
		}else{
			raceString ="Open_1402_R";
		}
		if(string.IsNullOrEmpty(racePrevString) == true){
			StartCoroutine(modeInfoForwardAniEx(raceString,4));
		}else{
			StartCoroutine(modeInfoReverseAniEx(racePrevString, preIndex));
			StartCoroutine(modeInfoForwardAniEx(raceString,4));
		
	
		}
		racePrevString =raceString;
		preIndex = 4;

	
	}
	void On_1403(){
		if(Global.isNetwork || Global.isAnimation) return;
		if(raceMenus[5].GetComponent<RaceSubItems>().OnLockBool) return;
		if(bAni) return;

		if(GV.ChSeasonID < 6015){
			raceString ="Open_1403";
		}else{
			raceString ="Open_1403_R";
		}
		if(string.IsNullOrEmpty(racePrevString) == true){
			StartCoroutine(modeInfoForwardAniEx(raceString,5));
		}else{
			StartCoroutine(modeInfoReverseAniEx(racePrevString, preIndex));
			StartCoroutine(modeInfoForwardAniEx(raceString,5));

	
			
		}
		racePrevString =raceString;
		preIndex = 5;
		
	
	}
	void On_1404(){
		if(Global.isNetwork || Global.isAnimation) return;
		if(raceMenus[6].GetComponent<RaceSubItems>().OnLockBool) return;
		if(bAni) return;

		if(GV.ChSeasonID < 6020){
			raceString ="Open_1404";
		}else{
			raceString ="Open_1404_R";
		}
		if(string.IsNullOrEmpty(racePrevString) == true){
			StartCoroutine(modeInfoForwardAniEx(raceString,6));
		}else{
			StartCoroutine(modeInfoReverseAniEx(racePrevString, preIndex));
			StartCoroutine(modeInfoForwardAniEx(raceString,6));

		}
		racePrevString =raceString;
		preIndex = 6;

	
	
	}
	void On_1405(){
		if(Global.isNetwork || Global.isAnimation) return;
		if(raceMenus[7].GetComponent<RaceSubItems>().OnLockBool) return;
		if(bAni) return;

		if(GV.ChSeasonID < 6025){
			raceString ="Open_1405";
		}else{
			raceString ="Open_1405_R";
		}
		if(string.IsNullOrEmpty(racePrevString) == true){
			StartCoroutine(modeInfoForwardAniEx(raceString,7));
		}else{
			StartCoroutine(modeInfoReverseAniEx(racePrevString, preIndex));
			StartCoroutine(modeInfoForwardAniEx(raceString,7));
		
		}
		racePrevString =raceString;
		preIndex = 7;

	
	}
	void On_1406(){
		if(Global.isNetwork || Global.isAnimation) return;
		if(raceMenus[8].GetComponent<RaceSubItems>().OnLockBool) return;
		if(bAni) return;
		if(GV.ChSeasonID < 6030){
			raceString ="Open_1406";
		}else{
			raceString ="Open_1406_R";
		}
		if(string.IsNullOrEmpty(racePrevString) == true){
			StartCoroutine(modeInfoForwardAniEx(raceString,8));
		}else{
			StartCoroutine(modeInfoReverseAniEx(racePrevString, preIndex));
			StartCoroutine(modeInfoForwardAniEx(raceString,8));
	
			
		}
		racePrevString =raceString;
		preIndex = 8;
	
	}


	void OnNext(GameObject obj){
	//	Utility.LogWarning(obj.name);
	}

	IEnumerator RaceSubMenuAniPlay(string strAni, string strPrevAni, System.Action callback = null){
		bAni = true;
		Global.isAnimation = true;
		Global.isNetwork = true;
		if(string.IsNullOrEmpty(strPrevAni) == false){
			raceAnimation[strPrevAni].time = 0;
			raceAnimation[strPrevAni].speed = 1;
			raceAnimation.Play(strPrevAni);
			while(raceAnimation.isPlaying){
				yield return null;
			}
		}
		if(callback != null) callback();
		callback = null;
		if(string.IsNullOrEmpty(strAni) == false){
			raceAnimation[strAni].time = 0;
			raceAnimation[strAni].speed = 1;
			raceAnimation.Play(strAni);
			while(raceAnimation.isPlaying){
				yield return null;
			}
		}
		bAni = false;
		Global.isAnimation = false;
		Global.isNetwork = false;
	}
	IEnumerator RaceSubMenuReverseAniPlay(string strAni, string strPrevAni, System.Action callback = null){
		bAni = true;
		Global.isAnimation = true;
		Global.isNetwork = true;
		if(string.IsNullOrEmpty(strAni) == false){
			raceAnimation[strAni].time = raceAnimation[strAni].length;
			raceAnimation[strAni].speed = -1;
			raceAnimation.Play(strAni);
			while(raceAnimation.isPlaying){
				yield return null;
			}
		}

		if(string.IsNullOrEmpty(strPrevAni) == false){
			raceAnimation[strPrevAni].time = raceAnimation[strPrevAni].length;
			raceAnimation[strPrevAni].speed = -1;
			raceAnimation.Play(strPrevAni);
			while(raceAnimation.isPlaying){
				yield return null;
			}
		}
		if(callback != null) callback();
		bAni = false;
		Global.isAnimation = false;
		Global.isNetwork = false;
		ePopups.InitEventObject();
	}
	
	public void OpenEvent(int idx){
		if(Global.isNetwork || Global.isAnimation) return;
		if(bAni) return;
		bAni = true;
		StartCoroutine(RaceSubMenuAniPlay("Event_Mode_Open", "Modemenu_OpenClose", ()=>{
			ePopups.SetEventMode(idx);
			bAni = false;
		}));
		/*
		return;
		if(bCheck==0){
			StartCoroutine(RaceSubMenuAniPlay("Event_Mode_Open", "Modemenu_OpenClose", ()=>{
				ePopups.SetEventMode(idx);
				bAni = false;
			}));
		}else{
			StartCoroutine(RaceSubMenuAniPlay("Event_Popup_Open", "Modemenu_OpenClose", ()=>{
				ePopups.SetEventPopUpMode(idx, bCheck);
				bAni = false;
			}));
		}*/
	}


	public void reverseEventPopup(){
		StartCoroutine(RaceSubMenuAniPlay("Event_Popup_Open", string.Empty));
	//	StartCoroutine(RaceSubMenuAniPlay("Event_Popup_Open", "Modemenu_OpenClose", ()=>{
			//ePopups.SetEventPopUpMode(idx, bCheck);
			//bAni = false;
	//	}));
	}

	public void OpenClubMode(){
		if(Global.isNetwork || Global.isAnimation) return;
		if(bAni) return;
		bAni = true;
		StartCoroutine(RaceSubMenuAniPlay("Open_Club", "Modemenu_OpenClose", ()=>{
				ePopups.ClubModeInitialize();
		}));

	}

	public void  OpenAniING(){
		bAni = true;
	
	}

	public void  OpenAniEnd(){
		bAni = true;
		
	}

	public void ClosedOpenMenu(){
		raceAnimation["Modemenu_OpenClose"].time = raceAnimation["Modemenu_OpenClose"].length;
		raceAnimation["Modemenu_OpenClose"].speed = -1;
		raceAnimation.Play("Modemenu_OpenClose");
	}

	public void returnMenu(){
	
		StartCoroutine(RaceSubMenuReverseAniPlay("Event_Mode_Open", "Modemenu_OpenClose"));
	}

	public void returnMenuClub(){

		StartCoroutine(RaceSubMenuReverseAniPlay("Open_Club", "Modemenu_OpenClose",()=>{
			ePopups.initClub();
		}));
	}

	public void returnMenuClubSub(){
		CClub.ClubClick = 5;
		GameObject.Find("LobbyUI").SendMessage("OnClanReturn");
		ePopups.initClub();
		return;
	
	}
	public void returnMenuPopup(){
		StartCoroutine(RaceSubMenuReverseAniPlay("Event_Popup_Open", "Modemenu_OpenClose"));
	}

	public void returnToDealer(){
		StartCoroutine(RaceSubMenuReverseAniPlay("Event_Popup_Open", string.Empty, ()=>{
			aniMode = 3;
			GameObject.Find("LobbyUI").SendMessage("MoveToDealer",SendMessageOptions.DontRequireReceiver);
			gameObject.SetActive(false);
		})
		);
	}

	void OnDisable(){

		//Global.bLobbyBack = false;

	}


	int CheckedNewCarEvent(){
		int mCar = myAccount.instance.account.eRace.testDriveCarID;
		if(myAccount.instance.account.eRace.testDrivePlayCount == 5){
			return 1;
		}
		CarInfo mcar = GV.mineCarList.Find(obj => obj.CarID == mCar);
		if(mcar != null) {
			return 2;
		}
		return 0;
	}
	
	int CheckedReqCarEvent(){
		//트랙지정 , 차량 지정 후 내가 없는 차량 지정, 할것!! 플레이 수 체크 할것.
		int mCar =myAccount.instance.account.eRace.featuredCarID;
		mCar = 1012;
		if(myAccount.instance.account.eRace.featuredPlayCount  == 5){
			return 1;
		}
		CarInfo mcar = GV.mineCarList.Find(obj => obj.CarID == mCar);
		if(mcar == null) {
			return 2;
		}
		return 0;
	}
	int CheckedMaterialEvent(){
	
		int mCar = GV.getTeamCarID(GV.SelectedTeamID);
			if(myAccount.instance.account.eRace.EvoPlayCount == 0){

			return 1;
		}
		if(myAccount.instance.account.eRace.EvoAcquistCount == 1){
			return 2;
		}
	
		return 0;
	}


	public void On1400_End(){
	
	}
	public void On1412_End(){
	
	}
	public void On1413_End(){
	
		
	}
	public void On1401_End(){
		
	}
	public void On1401_R_End(){
		
	}
	public void On1402_R_End(){
		
	}
	public void On1402_End(){
		
	}
	public void On1403_End(){
		
	}
	public void On1403_R_End(){
		
	}
	public void On1404_R_End(){
		
	}	
	public void On1404_End(){
		
	}
	public void On1405_End(){
		
	}

	public void On1405_R_End(){
		
	}

	public void On1406_End(){
		
	}

	public void On1406__R_End(){
		
	}

	public void OpenClub_End(){
	
	}

	public void modeMenu_End(){
	}

	public void EventMode_End(){
	}
	public void Eventpopup_End(){
	}


}
