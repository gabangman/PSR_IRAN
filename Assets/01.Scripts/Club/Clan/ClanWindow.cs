using UnityEngine;
using System.Collections;

public class ClanWindow : MonoBehaviour {
	GameObject sub = null;
	private Animation menuAni;
	public GameObject btnClubRw;
	void Awake(){
		menuAni = GetComponent<Animation>();
	}
	void Start(){
		sub = transform.FindChild("SubWindow").gameObject as GameObject;
		btnClubRw.SetActive(true);
		if(CClub.mClubFlag == 0){// 0 : no season, 1 : seaon ok but no clanmember, 2 : season OK & clanmember
			sub.transform.FindChild("Locked").gameObject.SetActive(true);	
			UILabel[] lb = sub.transform.FindChild("Locked").GetComponentsInChildren<UILabel>();
			lb[0].text = KoStorage.GetKorString("77131");
		}else if(CClub.mClubFlag == 1){
			sub.transform.FindChild("MyClan_No").gameObject.SetActive(true);

		}else if(CClub.mClubFlag == 2){
			sub.transform.FindChild("MyClan").gameObject.SetActive(true);
			sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().AddViewMyClanMem();
			sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeClubInfo();
		}
	
	}

	private int rankIndex = 0;

	public void playMenuReverseAni(){
		StartCoroutine("reverseAni","ClanMenu_1");
	}

	public void playMenuAni(){
		StartCoroutine("playAni","ClanMenu_1");
	}

	IEnumerator playAni(string strAni){
		Global.isNetwork = true;
		Global.isAnimation = true;
		menuAni[strAni].time = 0;
		menuAni[strAni].speed = 1;
		menuAni.Play(strAni);
		
		while(menuAni.IsPlaying(strAni)){
			yield return null;
		}
		Global.isNetwork = false;
		Global.isAnimation = false;
	}
	IEnumerator reverseAni(string strAni){
		Global.isNetwork = true;
		Global.isAnimation = true;
		menuAni[strAni].time = menuAni[strAni].length;
		menuAni[strAni].speed = -3;
		menuAni.Play(strAni);
		
		while(menuAni.IsPlaying(strAni)){
			yield return null;
		}
		Global.isNetwork = false;
		Global.isAnimation = false;
		gameObject.SetActive(false);
	}
	void OnEnable(){
	}

	void OnDisable(){
	
	}
	void OnDestroy(){
	
	}

	IEnumerator ReverseTween(GameObject obj, int idx){
		if(idx != 1) {
			sub.transform.FindChild("Ranking").gameObject.SendMessage("HiddenWindow",SendMessageOptions.DontRequireReceiver);
		}
		var tw = obj.GetComponent<TweenPosition>() as TweenPosition;
		tw.to = new Vector3(0,-800f,0);
		tw.from = Vector3.zero;
		tw.Reset();
		tw.enabled = true;
		tw.onFinished = delegate {
			obj.SetActive(false);
		};
		yield return new WaitForSeconds(0.15f);
		Transform tempObj = null;

		switch(idx){
		case 0:
			btnClubRw.SetActive(true);
			if(CClub.mClubFlag == 0){// 0 : no season, 1 : seaon ok but no clanmember, 2 : season OK & clanmember
				tempObj = sub.transform.FindChild("Locked") as Transform;
				sub.transform.FindChild("History").gameObject.SetActive(false);
				sub.transform.FindChild("Ranking").gameObject.SetActive(false);
				tempObj.gameObject.SetActive(true);
			}else{
				if(CClub.ClanMember == 0){// 0 : no member , 1 : clanMaseter, 2 : clanStaff, 3: ClanMember
					sub.transform.FindChild("Locked").gameObject.SetActive(false);
					tempObj = sub.transform.FindChild("MyClan_No") as Transform;
					tempObj.gameObject.SetActive(true);
				}else if(CClub.ClanMember == 1){
					CClub.myClubMemInfo.Clear();
					sub.transform.FindChild("Locked").gameObject.SetActive(false);
					tempObj = sub.transform.FindChild("MyClan") as Transform;
					tempObj.gameObject.SetActive(true);
					sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().RefreshClubInfo((mFlag)=>{
						if(mFlag == 1)
							sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeClubInfo();
						else {
							sub.transform.FindChild("Locked").gameObject.SetActive(false);
							sub.transform.FindChild("MyClan_No").gameObject.SetActive(true);
						//	var sto = sub.transform.FindChild("MyClan").gameObject.GetComponent<ClubLoading>() as ClubLoading;
						//	if(sto != null) sto.StopClubLoading();
							sub.transform.FindChild("MyClan").gameObject.SetActive(false);
							var twtw = 	sub.transform.FindChild("MyClan_No").gameObject.GetComponent<TweenPosition>() as TweenPosition;
							twtw.from = new Vector3(0,-800f,0);
							twtw.to = Vector3.zero;
							twtw.Reset();
							twtw.enabled=true;
							twtw.onFinished = null;
						}
					});
			
				}else if(CClub.ClanMember == 2){
					sub.transform.FindChild("Locked").gameObject.SetActive(false);
					tempObj = sub.transform.FindChild("MyClan") as Transform;
					tempObj.gameObject.SetActive(true);
					sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().AddViewMyClanMem();
					sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeClubInfo();
				}else if(CClub.ClanMember == 3){
					sub.transform.FindChild("Locked").gameObject.SetActive(false);
					tempObj = sub.transform.FindChild("MyClan") as Transform;
					tempObj.gameObject.SetActive(true);
					sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().RefreshClubInfo((mFlag)=>{

						if(mFlag == 1){
							sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeClubInfo();
							sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().AddViewMyClanMem();
						}
						else {
							sub.transform.FindChild("Locked").gameObject.SetActive(false);
							sub.transform.FindChild("MyClan_No").gameObject.SetActive(true);
							sub.transform.FindChild("MyClan").gameObject.SetActive(false);
							var twtw = 	sub.transform.FindChild("MyClan_No").gameObject.GetComponent<TweenPosition>() as TweenPosition;
							twtw.from = new Vector3(0,-800f,0);
							twtw.to = Vector3.zero;
							twtw.Reset();
							twtw.enabled=true;
							twtw.onFinished = null;
						}
					});
				}
			}

		//	tempObj.gameObject.SetActive(true);
			break;
		case 1: //clan Rank
			if(CClub.mClubFlag == 0){// 0 : no season, 1 : seaon ok but no clanmember, 2 : season OK & clanmember
				sub.transform.FindChild("Locked").gameObject.SetActive(false);
			}
			tempObj = sub.transform.FindChild("Ranking") as Transform;
			tempObj.gameObject.SetActive(true);
			tempObj.GetComponent<ViewClanRank>().AddViewClanRank();
			btnClubRw.SetActive(false);
			break;
		case 2: //clan history
			if(CClub.mClubFlag == 0){// 0 : no season, 1 : seaon ok but no clanmember, 2 : season OK & clanmember
				tempObj = sub.transform.FindChild("Locked") as Transform;
				tempObj.gameObject.SetActive(true);

			}else{
				tempObj = sub.transform.FindChild("History") as Transform;
				tempObj.gameObject.SetActive(true);
				tempObj.GetComponent<ViewClanHistory>().AddViewClanHistory();
			}
				btnClubRw.SetActive(true);
			break;
		case 3: //clan search
			if(CClub.mClubFlag == 0){// 0 : no season, 1 : seaon ok but no clanmember, 2 : season OK & clanmember
				sub.transform.FindChild("Locked").gameObject.SetActive(false);

			}
			tempObj = sub.transform.FindChild("Search") as Transform;
			tempObj.gameObject.SetActive(true);
			btnClubRw.SetActive(true);
			break;
		default:
			Utility.LogError("temoObj null");
			break;
		}

		tw = tempObj.GetComponent<TweenPosition>();
		tw.from = new Vector3(0,-800f,0);
		tw.to = Vector3.zero; tw.Reset();tw.enabled=true;
		tw.onFinished = null;
	}


	public void ChangeObject(int idx){
		int cnt = sub.transform.childCount;
		for(int  i = 0; i < cnt ; i++){
			var obj = sub.transform.GetChild(i).gameObject;
			if(obj.activeSelf){
				StartCoroutine(ReverseTween(obj, idx));
				break;
			}
		}
	}

	public void OnCreateClick(){
		Global.isPopUp = true;
		var temp = transform.FindChild("ClanPopup") as Transform;
		temp.gameObject.SetActive(true);
		temp.GetComponent<ClanPopup>().PopupScale();
		temp.transform.FindChild("CreateClan").gameObject.SetActive(true);
		temp.transform.FindChild("CreateClan").GetComponent<ClanCreateContent>().createClubInit();
	}


	public void OnRefresh(){
		sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().RefreshClubInfo((mFlag)=>{
			if(mFlag == 1){
				sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeClubInfo();
				sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().AddViewMyClanMem();
			}
			else {
				sub.transform.FindChild("Locked").gameObject.SetActive(false);
				sub.transform.FindChild("MyClan_No").gameObject.SetActive(true);

				sub.transform.FindChild("MyClan").gameObject.SetActive(false);
				var twtw = 	sub.transform.FindChild("MyClan_No").gameObject.GetComponent<TweenPosition>() as TweenPosition;
				twtw.from = new Vector3(0,-800f,0);
				twtw.to = Vector3.zero;
				twtw.Reset();
				twtw.enabled=true;
				twtw.onFinished = null;
			}
		});
	}



	public void CreateClanPopup(string[] str){
		var temp = transform.FindChild("ClanPopup") as Transform;
		temp.gameObject.SetActive(true);
		temp.GetComponent<ClanPopup>().OnCreateClanPopup(str,()=>{
			CClub.ClanMember = 1;
			CClub.ChangeClub = 1;
			CClub.mClubFlag = 2;
			sub.transform.FindChild("MyClan").gameObject.SetActive(true);
			sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().MyClanMemberInit();
			sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().AddViewFisrtMyClan();
			sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeClubInfo();
			sub.transform.FindChild("MyClan_No").gameObject.SetActive(false);
		//	}
		});
	}

	public void CreateFailPopup(int idx){
		var temp = transform.FindChild("ClanPopup") as Transform;
		temp.gameObject.SetActive(true);
		temp.GetComponent<ClanPopup>().OnCreateFailPopup(()=>{
			OnCreateClick();
		}, idx);
	}



	IEnumerator GetClubModeCheckLogic(){
		bool bConnect  = false;
		string mAPI = "club/getMyClubInfo";//ServerAPI.Get(90059);
		NetworkManager.instance.ClubBaseConnect("Get",new System.Collections.Generic.Dictionary<string,int>(), mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				CClub.mClubInfo = new ClubInfo(thing);
				if(CClub.mClubInfo.clubMember == 9){
					CClub.ClanMember = 1;
				}else if(CClub.mClubInfo.clubMember ==5){
					CClub.ClanMember =2;
				}else{
					CClub.ClanMember =3;
				}
				CClub.mClubFlag = 2;
			}else if(status == -1){
				
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
		OnSignOutDetail();
	}

	void OnSignOutDetail(){
		var temp = transform.FindChild("ClanPopup") as Transform;
		if(CClub.ClubMode == 3){
			temp.GetComponent<ClanPopup>().OnSignOutClubRaceFailePopup(()=>{
			
			});
			
			return;
		}
		
		if(CClub.ClanMember == 1 || CClub.ClanMember == 2){
			if(CClub.mClubInfo.clubMemberNum == 1){
				if(CClub.ClubMode == 1){
					temp.GetComponent<ClanPopup>().OnSignOutClubRaceFailePopup(()=>{
					});
					return;
				}else{
					temp.GetComponent<ClanPopup>().OnSignOutPopup(()=>{
						CClub.InitClub();
						CClub.ClanMember = 0;
						CClub.ChangeClub = 1;
						sub.transform.FindChild("MyClan").gameObject.SetActive(false);
						sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().MyClanMemberInit();
						sub.transform.FindChild("MyClan_No").gameObject.SetActive(true);
						sub.transform.FindChild("MyClan_No").localPosition = new Vector3(0,0,0);
					});
				}
			}else{
				temp.GetComponent<ClanPopup>().OnSignOutMemPopup(()=>{
					CClub.InitClub();
					CClub.ClanMember = 0;
					CClub.ChangeClub = 1;
					sub.transform.FindChild("MyClan").gameObject.SetActive(false);
					sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().MyClanMemberInit();
					sub.transform.FindChild("MyClan_No").gameObject.SetActive(true);
					sub.transform.FindChild("MyClan_No").localPosition = new Vector3(0,0,0);
				});
			}
		}else if(CClub.ClanMember == 3){
			/*
			if(CClub.mClubInfo.clubMemberNum == 1){
				temp.GetComponent<ClanPopup>().OnSignOutPopup(()=>{
					CClub.InitClub();
					CClub.ClanMember = 0;
					CClub.ChangeClub = 1;
					sub.transform.FindChild("MyClan").gameObject.SetActive(false);
					sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().MyClanMemberInit();
					sub.transform.FindChild("MyClan_No").gameObject.SetActive(true);
					sub.transform.FindChild("MyClan_No").localPosition = new Vector3(0,0,0);
				});
			}else{
				temp.GetComponent<ClanPopup>().OnSignOutMemPopup(()=>{
					CClub.InitClub();
					CClub.ClanMember = 0;
					CClub.ChangeClub = 1;
					sub.transform.FindChild("MyClan").gameObject.SetActive(false);
					sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().MyClanMemberInit();
					sub.transform.FindChild("MyClan_No").gameObject.SetActive(true);
					sub.transform.FindChild("MyClan_No").localPosition = new Vector3(0,0,0);
				});
			}*/
			temp.GetComponent<ClanPopup>().OnSignOutMemPopup(()=>{
				CClub.InitClub();
				CClub.ClanMember = 0;
				CClub.ChangeClub = 1;
				sub.transform.FindChild("MyClan").gameObject.SetActive(false);
				sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().MyClanMemberInit();
				sub.transform.FindChild("MyClan_No").gameObject.SetActive(true);
				sub.transform.FindChild("MyClan_No").localPosition = new Vector3(0,0,0);
			});
		
		}
	}

	public void OnClubRW(){
		var temp = transform.FindChild("ClanPopup") as Transform;
		temp.gameObject.SetActive(true);
		temp.GetComponent<ClanPopup>().OnClubRW(()=>{

		});
	}


	public void OnSignOut(){
		var temp = transform.FindChild("ClanPopup") as Transform;
		temp.gameObject.SetActive(true);
		StartCoroutine("GetClubModeCheckLogic");
	}

	public void OnVisitSignIn(int cIdx, string cName){
		Utility.Log("ClanWindow OnSignIn");
		var temp = transform.FindChild("ClanPopup") as Transform;
		temp.gameObject.SetActive(true);
		temp.transform.FindChild("VisitClan").gameObject.SetActive(false);
		if(CClub.mClubFlag == 0){// 0 : no season, 1 : seaon ok but no clanmember, 2 : season OK & clanmember
			temp.GetComponent<ClanPopup>().OnNoSignInPopup(()=>{
				temp.transform.FindChild("VisitClan").gameObject.SetActive(true);
			});
		}else{
			if(CClub.ClanMember == 0){
				temp.GetComponent<ClanPopup>().OnVisitSignInPopup(()=>{
					CClub.ClanMember = 3; // 회원
					CClub.mClubFlag = 2;
					CClub.ChangeClub = 1;
					transform.FindChild("MainMenu").gameObject.SendMessage("OnClanMySet");
					sub.transform.FindChild("MyClan").gameObject.SetActive(true);
					sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().AddViewJoinMyClan();
					sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeClubInfo();
					sub.transform.FindChild("MyClan_No").gameObject.SetActive(false);
					temp.transform.FindChild("VisitClan").gameObject.SetActive(false);
					sub.transform.FindChild("Search").gameObject.SetActive(false);
					sub.transform.FindChild("Ranking").gameObject.SendMessage("HiddenWindow",SendMessageOptions.DontRequireReceiver);
					sub.transform.FindChild("Ranking").gameObject.SetActive(false);
		

				},cIdx, cName);
			}else{
				temp.GetComponent<ClanPopup>().OnAlreadySignInPopup(()=>{
					temp.transform.FindChild("VisitClan").gameObject.SetActive(true);
				});
				
			}
		}
	}

	public void OnSignIn(string clanname = null, int idx=0, string clubName= null){
		Utility.Log("ClanWindow OnSignIn " + clanname);
		// idx = 0 serach idx =1 = search
		var temp = transform.FindChild("ClanPopup") as Transform;
		temp.gameObject.SetActive(true);

		if(CClub.mClubFlag == 0){// 0 : no season, 1 : seaon ok but no clanmember, 2 : season OK & clanmember
			temp.GetComponent<ClanPopup>().OnNoSignInPopup(()=>{
				Global.isNetwork = false;
			});
		}else{
			if(CClub.ClanMember == 0){
				temp.GetComponent<ClanPopup>().OnSignInPopup(()=>{
					CClub.ClanMember = 3; // 회원
					CClub.mClubFlag = 2;
					CClub.ChangeClub = 1;
					transform.FindChild("MainMenu").gameObject.SendMessage("OnClanMySet");
					sub.transform.FindChild("MyClan").gameObject.SetActive(true);
				//	sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().MyClanMemberInit();
					sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().AddViewJoinMyClan();
					sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeClubInfo();
					sub.transform.FindChild("MyClan_No").gameObject.SetActive(false);
					sub.transform.FindChild("Search").gameObject.SetActive(false);
					sub.transform.FindChild("Ranking").gameObject.SendMessage("HiddenWindow",SendMessageOptions.DontRequireReceiver);
					sub.transform.FindChild("Ranking").gameObject.SetActive(false);
					Global.isNetwork = false;
				},idx, clubName);
			
			}else{
				temp.GetComponent<ClanPopup>().OnAlreadySignInPopup(()=>{
				
				});
			
			}
		}
	}

	public void OnClanVisit(string str = null, int idx=0){
		// idx = 0 serach idx =1 = search
		var temp = transform.FindChild("ClanPopup") as Transform;
		temp.gameObject.SetActive(true);
		temp.GetComponent<ClanPopup>().PopupScale();
		temp.transform.FindChild("VisitClan").gameObject.SetActive(true);
		temp.transform.FindChild("VisitClan").GetComponent<VisitClan>().SetClanInfo(str, idx);

	}

	public void OnModifyClan(){
		Global.isPopUp = true;
		var temp = transform.FindChild("ClanPopup") as Transform;
		temp.gameObject.SetActive(true);
		temp.GetComponent<ClanPopup>().PopupScale();
		temp.transform.FindChild("ModifyClan").gameObject.SetActive(true);
		temp.transform.FindChild("ModifyClan").GetComponent<ClanCreateContent>().modifyClan();
	}

	public void ModifyClanPopup(string str){
		var temp = transform.FindChild("ClanPopup") as Transform;
		temp.gameObject.SetActive(true);
		temp.GetComponent<ClanPopup>().ModifyClubOkay(str,()=>{
			CClub.ClanMember = 1;
			sub.transform.FindChild("MyClan").gameObject.SetActive(true);
			sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeClubInfo();
			sub.transform.FindChild("MyClan_No").gameObject.SetActive(false);
			Global.isNetwork = false;
		});
	}
	System.Action entryCallback;
	public void OnEntryFlag(System.Action callback){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		this.entryCallback = callback;
		StartCoroutine("myEntryChange");
	}

	IEnumerator myEntryChange(){
		bool bConnect = false;
		string mAPI = "club/putMyEntryFlag";//destroyClub
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		if(CClub.mClubInfo.myEntryFlag == 1) {
			mDic.Add("mFlag","0");
		}else{
			mDic.Add("mFlag","1");
		}
		
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				if(CClub.mClubInfo.myEntryFlag == 1) CClub.mClubInfo.myEntryFlag =0;
				else CClub.mClubInfo.myEntryFlag = 1;
				if(entryCallback != null) entryCallback();
				entryCallback = null;
				GameObject.Find("Audio").SendMessage("CompleteSound");
			}else{
				
			}
			bConnect = true;
		});
		while(!bConnect){
			yield return null;
		}
		Global.isNetwork= false;
	}


	public void EntryFlagResults(int cnt){
		sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeUserSlot(cnt);
	}

	public void OnKickMember(System.Action callback, int userId){
		var temp = transform.FindChild("ClanPopup") as Transform;
		temp.gameObject.SetActive(true);
		System.Action mCall = ()=>{
			callback();
			sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeDeleteClubInfo();
		};
		temp.GetComponent<ClanPopup>().OnKickMember(mCall,userId);
	}




	public void searchError(int index=0){
		var temp = transform.FindChild("ClanPopup") as Transform;
		temp.gameObject.SetActive(true);
		temp.GetComponent<ClanPopup>().PopupScale();
		temp.GetComponent<ClanPopup>().searchErrorPop(index);
	}


	public void OnStaffMember(System.Action callback, int uId){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
		this.entryCallback = callback;
		/*if(Application.isEditor){
			if(entryCallback != null) entryCallback();
			entryCallback = null;
			Global.isNetwork = false;
			gameObject.GetComponent<ClubLoading>().StopClubLoading();
			sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeStaffSlot(uId);
		}else*/ StartCoroutine("setStaff", uId);
	}
	
	IEnumerator setStaff(int userId){
		bool bConnect = false;
		string mAPI = "club/setStaff";//destroyClub
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		mDic.Add("userId",CClub.myClubMemInfo[userId].userId.ToString());
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				if(entryCallback != null) entryCallback();
				entryCallback = null;
				GameObject.Find("Audio").SendMessage("CompleteSound");
				sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeStaffSlot(userId);
				if(!CClub.bClanWarChat){
				string str1 = string.Format(KoStorage.GetKorString("77605"), CClub.myClubMemInfo[userId].nickName);
				UserDataManager.instance.JiverSend(str1);
				}
			}else{
				
			}
			bConnect = true;
			gameObject.GetComponent<ClubLoading>().StopClubLoading();
		});
		while(!bConnect){
			yield return null;
		}
		Global.isNetwork= false;
	}

	public void OnStaffOutMember(System.Action callback,  int uId){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		gameObject.AddComponent<ClubLoading>().ReadyClubLoading();
		this.entryCallback = callback;
	/*	if(Application.isEditor){
			if(entryCallback != null) entryCallback();
			entryCallback = null;
			Global.isNetwork = false;
			gameObject.GetComponent<ClubLoading>().StopClubLoading();
			sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeStaffSlot(uId);
		}else*/ StartCoroutine("unSetStaff", uId);
		//temp.GetComponent<ClanPopup>().OnStaffOutMember(mCall,userId);
	}
	IEnumerator unSetStaff(int userId){
		bool bConnect = false;
		string mAPI = "club/unSetStaff";//destroyClub
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		mDic.Add("userId",CClub.myClubMemInfo[userId].userId.ToString());
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				if(entryCallback != null) entryCallback();
				entryCallback = null;
				GameObject.Find("Audio").SendMessage("CompleteSound");
				sub.transform.FindChild("MyClan").GetComponent<ViewMyClanMem>().ChangeStaffSlot(userId);
			}else{
				
			}
			bConnect = true;
			gameObject.GetComponent<ClubLoading>().StopClubLoading();
		});
		while(!bConnect){
			yield return null;
		}
		Global.isNetwork= false;
	}


}
