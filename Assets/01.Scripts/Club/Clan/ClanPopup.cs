using UnityEngine;
using System.Collections;

public class ClanPopup : MonoBehaviour {

	public Transform[] tr;
	System.Action Callback;
	void Start(){
	}
	void OnDisable(){
		tr[0].gameObject.SetActive(false);
		tr[1].gameObject.SetActive(false);
		tr[2].gameObject.SetActive(false);
		tr[3].gameObject.SetActive(false);
		tr[4].gameObject.SetActive(false);

		transform.GetChild(6).gameObject.SetActive(false);
		transform.GetChild(7).gameObject.SetActive(false);
		kickoffUserId = -1;
	}
	public void OnClose(){
		OnCloseClick();
	}
	void OnCloseClick(){
		Global.isPopUp = false;
		UserDataManager.instance.OnSubBack = null;
		Global.bLobbyBack = false;
		gameObject.SetActive(false);
	}

	void CreateFailedCloseOk(){
		Global.isPopUp = false;
		this.OnDisable();
		//Callback(); 
		Callback = null;
		NGUITools.FindInParents<ClanWindow>(gameObject).OnCreateClick();
	}

	void CloseOk(){
		Global.isPopUp = false;
		this.OnDisable();
		if(Callback != null) Callback();
		Callback = null;
	}

	public void OnClubRW(System.Action callback){
		this.Callback = callback;
		transform.GetChild(7).gameObject.SetActive(true);
		UserDataManager.instance.OnSubBack = ()=>{
			OnCloseClick();
		};
		Global.bLobbyBack = true;
		transform.GetChild(7).gameObject.SendMessage("SetInit",SendMessageOptions.DontRequireReceiver);
	}

	void OnEnable(){
	
	//	//==!!Utility.LogWarning("clanPopUp scale");
	}

	public void PopupScale(){

		var scale = gameObject.GetComponents<TweenScale>() as TweenScale[];
		scale[0].Reset();
		scale[0].enabled = true;
		scale[1].Reset();
		scale[1].enabled = true;
	}

	void OnSign(){
		Utility.Log("ClanPopup + OnSign");
		OnCloseClick();
	}

	void OnSignOk(){
		OnCloseClick();
	}

	void ResetClanPopup(){
		this.OnDisable();
	}

	bool buyPriceCheck(int dollar){
		bool b = false;
		int dec = GV.myDollar - dollar;
		if(dec < 0) b = true;
		if(b){
		
		}
		return b;
		//return true;
	}
	void CreateOk(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		if(buyPriceCheck(CClub.ClubDollar)){
			tr[4].gameObject.SetActive(false);
			tr[3].gameObject.SetActive(true);
			PopupScale();
			Transform mTr = tr[3];
			mTr.gameObject.GetComponent<ClanRegularPopup>().NoCreateNotMoney();
			Global.isNetwork = false;
			Callback = null;
			return;
		}else {
			StartCoroutine("ClubCreate");
		}
	
	

	
	//	Dictionary<string,string> mDic = new Dictionary<string, string>();
	//	mDic.Add("mClubName",this.strCreate[1] );
		//	mDic.Add("mClubSymbol",this.strCreate[2] );
		//	mDic.Add("mDescription","Clubsymbol_"+this.strCreate[3] );
		//	mDic.Add("dollar","CClub.clubDollar);
	//	mDic.Add("userInfoData",makeCustomUserData());
	//	BasedConnect("Post","club/createClub",mDic,"createClub");


	}

	IEnumerator ClubCreate(){
		bool bConnect = false;
		int idx = 0;
		string mAPI = "club/createClub";
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
	//	//==!!Utility.LogWarning(this.strCreate[1]);
		//	//==!!Utility.LogWarning(this.strCreate[2]);mDescription
	//	//==!!Utility.LogWarning(this.strCreate[3]);
		mDic.Add("mClubName",this.strCreate[1] );
		mDic.Add("mDescription",this.strCreate[2] );
		mDic.Add("mClubSymbol","Clubsymbol_"+this.strCreate[3] );
		mDic.Add("dollar",CClub.ClubDollar.ToString());
		mDic.Add("userInfoData",AccountManager.instance.makeCustomUserData());
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				GV.myDollar -= CClub.ClubDollar;;
				GV.updateDollar = CClub.ClubDollar;;
				
				var lobby = GameObject.Find("LobbyUI") as GameObject;
				lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
				GameObject.Find("Audio").SendMessage("CompleteSound");
			}else if(status == -403){
			
				idx = 1;
			}
			bConnect = true;
		});


		while(!bConnect){
			yield return null;
		}
		if(idx == 1) {
			Global.isNetwork = false;
			OnCloseClick();
			yield break;
		}
		bConnect = false;
		mAPI = "club/getMyClubInfo";
		NetworkManager.instance.ClubBaseConnect("Get",new System.Collections.Generic.Dictionary<string,int>(),mAPI,(request)=>{
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



		bConnect = false;
		mAPI = "club/getClubMemberInfo";
		mDic.Clear();
		mDic.Add("clubIndex",CClub.mClubInfo.clubIndex.ToString());
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				CClub.myClubMemInfo.Clear();
		//	respons : {"state":0,"member_list":[{"userId":487,"clubStarCount":"temp","playTotalNumber":"temp","playMyNumber":"temp","clubMember":9,"joinTime":"2016-04-27 15:01:51","userInfoData":{"profileUrl":"User_Default","nickName":"3199","mLv":"1"}}]}
				int cnt = thing["memberList"].Count;
				ClubMemberInfo cMemInfo;
				for(int i =0; i < cnt; i++){
					cMemInfo = new ClubMemberInfo(thing["memberList"][i]);
					CClub.myClubMemInfo.Add(cMemInfo);
				}

				if(CClub.myClubMemInfo.Count >=2){
					CClub.myClubMemInfo.Sort(delegate(ClubMemberInfo x, ClubMemberInfo y) {
						int a = y.clubStarCount.CompareTo(x.clubStarCount);
						if(a == 0)
							a =  y.playNumber.CompareTo(x.playNumber);
						if(a == 0) 	a =  y.mLv.CompareTo(x.mLv);
						return a;
					});
				}
			}else{

			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
		if(Callback == null)
		{

		}else{
			Callback();
			Callback = null;
		}
		Global.isNetwork = false;
		OnCloseClick();
	}


	void SignOutOk(){
		Utility.LogWarning(Global.isNetwork);
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		StartCoroutine("ClanDestory");
	}

	IEnumerator ClanDestory(){
		bool bConnect = false;
		string mAPI = "club/destroyClub";//destroyClub
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		NetworkManager.instance.ClubBaseStringConnect("Delete",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				if(Callback == null){
					//==!!Utility.LogWarning("ClubCreate Callback Null");
				}else{
					Callback();
				}
				Callback = null;
				UserDataManager.instance.JiverUnSetInit();
				
			}else{
				//state = -1 현재 매칭중이거나 레이스 중임.
				
			}
			
			Global.isNetwork = false;
			bConnect = true;
		});
		
		
		while(!bConnect){
			yield return null;
		}
		OnCloseClick();
		
	}

	
	
	public void CheckVisitCancle(){

		if(tr[1].gameObject.activeSelf){
			PopupScale();
			tr[1].gameObject.SendMessage("ResetVisitView",true);
		}
	}

	void OnClanCreatePopup(string[] str){
		if(str[0].Equals("Yes")){
			OnCloseClick();
			NGUITools.FindInParents<ClanWindow>(gameObject).CreateClanPopup(str);
		}else if(str[0].Equals("No")){
			OnCloseClick(); // 클럽 이름
			NGUITools.FindInParents<ClanWindow>(gameObject).CreateFailPopup(0);
		}else if(str[0].Equals("No1")){ // 클럽 설명문
			OnCloseClick();
			NGUITools.FindInParents<ClanWindow>(gameObject).CreateFailPopup(1);
		}
	}

	public void OnSignOutPopup(System.Action callback){

		tr[3].gameObject.SetActive(true);
		PopupScale();
		Transform mTr = tr[3];
		this.Callback = callback;
		mTr.gameObject.SendMessage("SignOutPopup");
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnCloseClick();
		};
	}
	public void OnSignOutFailPopup(System.Action callback){

		tr[3].gameObject.SetActive(true);
		PopupScale();
		Transform mTr = tr[3];
		this.Callback = callback;
		mTr.gameObject.SendMessage("SignOutFailPopup");
	}

	public void OnSignOutClubRaceFailePopup(System.Action callback){
		
		tr[3].gameObject.SetActive(true);
		PopupScale();
		Transform mTr = tr[3];
		this.Callback = callback;
		mTr.gameObject.SendMessage("SignOutRaceFailPopup");
	}
	public void OnSignOutMemPopup(System.Action callback){

		tr[3].gameObject.SetActive(true);
		PopupScale();
		Transform mTr = tr[3];
		this.Callback = callback;
		mTr.gameObject.SendMessage("SignOutMemPopup");
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnCloseClick();
		};
	}
	public void SignOutMemOk(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		StartCoroutine("SignOutMember");
	}

	public void OnSignInPopup(System.Action callback, int idx, string cName){

		tr[3].gameObject.SetActive(true);
		PopupScale();
		Transform mTr = tr[3];
		this.Callback = callback;
		//mTr.gameObject.SendMessage("SignInPopup", cName, SendMessageOptions.DontRequireReceiver);
		mTr.gameObject.GetComponent<ClanRegularPopup>().SignInPopup(0, cName);
	//	kickoffUserId = CClub.mSeachList[idx].clubIndex;
		kickoffUserId = idx;
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnCloseClick();
		};
	}

	public void SignInOk(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		StartCoroutine("SignInMember");
	}





	public void OnNoSignInPopup(System.Action callback){

		tr[3].gameObject.SetActive(true);
		PopupScale();
		Transform mTr = tr[3];
		this.Callback = callback;
		mTr.gameObject.SendMessage("NoSignInPopUp", 0, SendMessageOptions.DontRequireReceiver);
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnCloseClick();
		};
	}

	public void OnAlreadySignInPopup(System.Action callback){

		tr[3].gameObject.SetActive(true);
		PopupScale();
		Transform mTr = tr[3];
		this.Callback = callback;
		mTr.gameObject.SendMessage("AlreadySignInPopUp", 0, SendMessageOptions.DontRequireReceiver);
	}

	public void searchErrorPop(int index=0){

		tr[3].gameObject.SetActive(true);
	//	PopupScale();
		Transform mTr = tr[3];
		this.Callback = null;
		mTr.gameObject.SendMessage("SearchErrorPopUp", index, SendMessageOptions.DontRequireReceiver);
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnCloseClick();
		};
	}

	public void OnVisitSignInPopup(System.Action callback, int cIdx, string cName){

	//	tr[1].gameObject.SendMessage("ResetVisitView",false);
		tr[3].gameObject.SetActive(true);
		PopupScale();
		Transform mTr = tr[3];
		this.Callback = callback;
		//mTr.gameObject.SendMessage("SignInPopup", 1, SendMessageOptions.DontRequireReceiver);
		mTr.gameObject.GetComponent<ClanRegularPopup>().SignInPopup(1, cName);
		kickoffUserId = cIdx;
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnCloseClick();
		};
	}

	public void OnCreateClanPopup(string[] str,System.Action callback){
		tr[4].gameObject.SetActive(true);
		PopupScale();
		Transform mTr = tr[4];
		this.Callback = callback;
		strCreate = new string[str.Length];
		System.Array.Copy(str, strCreate, str.Length);
	//	mTr.gameObject.SendMessage("CreateClanPopup");
		var temp = mTr.gameObject.GetComponent<ClubCreatePopup>() as ClubCreatePopup;
		if(temp == null) temp = mTr.gameObject.AddComponent<ClubCreatePopup>();
		temp.ChangeClanCreateInfo(strCreate);

		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnCloseClick();
		};

	}
	private string[] strCreate;
	public void OnCreateFailPopup(System.Action callback, int idx){
		PopupScale();
		tr[3].gameObject.SetActive(true);
		Transform mTr = tr[3];
		this.Callback = callback;
		mTr.gameObject.SendMessage("CreateFailPopup", idx, SendMessageOptions.DontRequireReceiver);
	}

	void OnClanModifyPopup(string[] str){
	//	PopupScale();
		if(str[0].Equals("Yes")){
			OnCloseClick();
			//NGUITools.FindInParents<ClanWindow>(gameObject).ModifyClanPopup(str);
		}else if(str[0].Equals("No")){
			OnCloseClick(); // 클럽 이름
			NGUITools.FindInParents<ClanWindow>(gameObject).CreateFailPopup(0);
		}else if(str[0].Equals("No1")){ // 클럽 설명문
			OnCloseClick();
			NGUITools.FindInParents<ClanWindow>(gameObject).CreateFailPopup(1);
		}
	}

	public void OnModifyClanPopup(string[] str,System.Action callback){
		tr[3].gameObject.SetActive(true);
		PopupScale();
		//==!!Utility.LogWarning("popScale");
		Transform mTr = tr[3];
		this.Callback = callback;
		strCreate = new string[str.Length];
		System.Array.Copy(str, strCreate, str.Length);
		mTr.gameObject.SendMessage("ModifyClanPopup");
	}

	public void ModifyClubOkay(string str,System.Action callback){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		this.Callback = callback;
		strCreate = new string[1];
		strCreate[0] = str;
		StartCoroutine("ClubModify");
	}

	public void ModifyOk(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		StartCoroutine("ClubModify");
	}

	IEnumerator ClubModify(){
		bool bConnect = false;
		string mAPI = "club/modifyClub";//destroyClub
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		mDic.Add("mClubName",CClub.mClubInfo.mClubName);
		mDic.Add("mDescription",this.strCreate[0] );
		mDic.Add("mClubSymbol",CClub.mClubInfo.clubSymbol);

		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				if(Callback == null){
					//==!!Utility.LogWarning("ClubCreate Callback Null");
				}else{
					CClub.mClubInfo.clubDescription = strCreate[0];
					Callback();
				}
				Callback = null;
			}else{
			
			}
			
			Global.isNetwork = false;
			bConnect = true;
		});
		
		
		while(!bConnect){
			yield return null;
		}
		OnCloseClick();
		
	}

	private int kickoffUserId;
	public void OnKickMember(System.Action callback, int userId){
		PopupScale();
		tr[3].gameObject.SetActive(true);
		Transform mTr = tr[3];
		this.Callback = callback;
		mTr.gameObject.SendMessage("KickMemeberPopup");
		kickoffUserId = userId;
	}

	public void OnEntryFlag(System.Action callback){
		PopupScale();
		tr[3].gameObject.SetActive(true);
		Transform mTr = tr[3];
		this.Callback = callback;
		mTr.gameObject.SendMessage("EntryFlagPopup");
	}

	public void OnEntryOk(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		StartCoroutine("myEntryChange");
	}

	IEnumerator myEntryChange(){
		bool bConnect = false;
		string mAPI = "club/putMyEntryFlag";//destroyClub
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		if(CClub.mClubInfo.myEntryFlag == 1) 	mDic.Add("mFlag","0");
		else mDic.Add("mFlag","1");

		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{

				if(CClub.mClubInfo.myEntryFlag == 1) CClub.mClubInfo.myEntryFlag =0;
				else CClub.mClubInfo.myEntryFlag = 1;
				if(Callback == null){
					//==!!Utility.LogWarning("ClubCreate Callback Null");
				}else{
					Callback();
				}
			
			}else{
			
			}
			Global.isNetwork = false;
			bConnect = true;
			Callback = null;
		});
		while(!bConnect){
			yield return null;
		}
		OnCloseClick();
	}

	public void OnKickOk(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		StartCoroutine("KickMember");
	}

	IEnumerator KickMember(){
		bool bConnect = false;
		string mAPI = "club/kickoffClubMember";//
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		mDic.Add("kickoffUserId",kickoffUserId.ToString());
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				if(Callback == null){
					//==!!Utility.LogWarning("ClubCreate Callback Null");
				}else{
					Callback();
				}
			}else{
				
				
			}
			
			Global.isNetwork = false;
			bConnect = true;
			Callback = null;
		});
		
		
		while(!bConnect){
			yield return null;
		}


		OnCloseClick();
		
	}


	IEnumerator SignOutMember(){
		bool bConnect = false;
		string mAPI = "club/unJoinClub";
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		NetworkManager.instance.ClubBaseStringConnect("Delete",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				if(Callback == null){
					//==!!Utility.LogWarning("SignOutMember Callback Null");
				}else{
					Callback();

				}
				UserDataManager.instance.JiverUnSetInit();
			}else{
				
			
			}
			
			Global.isNetwork = false;
			bConnect = true;
			Callback = null;
		});

		while(!bConnect){
			yield return null;
		}


	
		OnCloseClick();

		
	}

	IEnumerator SignInMember(){
		bool bConnect = false;
		int idx = 0;
		string mAPI = "club/JoinClub";
		//==!!Utility.LogWarning(kickoffUserId);
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		mDic.Add("clubIndex",kickoffUserId.ToString());
		mDic.Add("userInfoData",AccountManager.instance.makeCustomUserData());
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				
			}else{
				
				idx = 1;
			}
			
			
			bConnect = true;
		
		});
		
		
		while(!bConnect){
			yield return null;
		}
		
		
		if(idx == 1) {
			OnCloseClick();
			yield break;
		}
		bConnect = false;
		mAPI = "club/getMyClubInfo";
		NetworkManager.instance.ClubBaseConnect("Get",new System.Collections.Generic.Dictionary<string,int>(),mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				
				//	respons : {"state":0,"clubMember":9,"clubDescription":"ddaabb","mClubName":"test1","clubSymbol":"Clubsymbol_5","clubChatCh":"temp","clubMemberNum":1,"clubMemberTotalNum":30,"victoryNum":"temp","playNum":"temp","clubLevel":1,"clubExp":0,"myEntryFlag":1,"clubIndex":66,"createTime":"2016-04-27 15:01:51","matchingTime":"0000-00-00 00:00:00","dev_isMatchJustFinished":false,"myEntry":0,"clubMode":0}
				
				CClub.mClubInfo = new ClubInfo(thing);
				if(CClub.mClubInfo.clubMember == 9){
					CClub.ClanMember = 1;
				}else if(CClub.mClubInfo.clubMember ==5){
					CClub.ClanMember =2;
				}else{
					CClub.ClanMember =3;
				}
				CClub.mClubFlag = 2;
				UserDataManager.instance.JiverInit();
			}else{
				
				
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
		
		bConnect = false;
		mAPI = "club/getClubMemberInfo";
		mDic.Clear();
		mDic.Add("clubIndex",CClub.mClubInfo.clubIndex.ToString());
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				//	respons : {"state":0,"member_list":[{"userId":487,"clubStarCount":"temp","playTotalNumber":"temp","playMyNumber":"temp","clubMember":9,"joinTime":"2016-04-27 15:01:51","userInfoData":{"profileUrl":"User_Default","nickName":"3199","mLv":"1"}}]}
				int cnt = thing["memberList"].Count;
				ClubMemberInfo cMemInfo;
				for(int i =0; i < cnt; i++){
					cMemInfo = new ClubMemberInfo(thing["memberList"][i]);
					CClub.myClubMemInfo.Add(cMemInfo);
				}

				if(CClub.myClubMemInfo.Count >=2){
					CClub.myClubMemInfo.Sort(delegate(ClubMemberInfo x, ClubMemberInfo y) {
						int a = y.clubStarCount.CompareTo(x.clubStarCount);
						if(a == 0)
							a =  y.playNumber.CompareTo(x.playNumber);
						if(a == 0) 	a =  y.mLv.CompareTo(x.mLv);
						return a;
					});
				}
			}else{
				
			}
			UserDataManager.instance.JoinMessage();
			Global.isNetwork = false;
			bConnect = true;
	
		});
		
		while(!bConnect){
			yield return null;
		}
		if(Callback != null) //==!!Utility.LogWarning("Callback Null");
			Callback();
		Callback = null;
		OnCloseClick();
		
	}


}
