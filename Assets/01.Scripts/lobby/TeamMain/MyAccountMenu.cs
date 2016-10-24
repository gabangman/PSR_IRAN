using UnityEngine;
using System.Collections;

public class MyAccountMenu : MonoBehaviour {
	public GameObject snsBtn, fbBtn,VIPBtn;
	public UILabel[] lbTop;
	public GameObject slot;
	void OnClose(){
		Global.isPopUp = false;
		GetComponent<TweenAction>().ReverseTween(gameObject);
		gameObject.SetActive(false);
		Global.bLobbyBack = false;
	}
	
	
	void Start(){
		lbTop[2].text = GV.UserNick;
		lbTop[3].text = string.Format(KoStorage.GetKorString("76001"), Global.level);
		SetLoginStatus();
	
		
		VIPBtn.transform.FindChild("lbName").GetComponent<UILabel>().text = "VIP";
		VIPBtn.transform.FindChild("lbLV").GetComponent<UILabel>().text = string.Format("LV.{0}", GV.gVIP);

		StartCoroutine("getRaceCount");
		
	}

	protected void ReadyLoadingCircle(){
		Global.isNetwork = true;
		var loading = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
		loading.StartCoroutine("StartRaceLoading", false);//raceStartLoading
	}

	protected void EndLoadingCircle(){
		Global.isNetwork = false;
		var loading = gameObject.GetComponent<LoadingGauage>() as LoadingGauage;
		//loading.StopLoading("StartRaceLoading", false);//raceStartLoading
		loading.stopLoading();
	}

	IEnumerator getRaceCount(){
		bool b = true;
		string mAPI = "game/race/count/";
		ReadyLoadingCircle();
		NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			{
				//response:{"state":0,"msg":"sucess","raceCount":{"ranking":0,"regular":0,"regularDrag":0,"pvpTimesquare":0,"pvpDrag":0,
				//"test":0,"featured":0,"evocube":0,"club":0},"totalTropyCount":{"tropy1":0,"tropy2":0,"tropy3":0},"weeklyTropyCount":{"tropy1":0,"tropy2":0,"tropy3":0},"time":1458284626}
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					pRanking= thing["raceCount"]["ranking"].AsInt;
					pEvo= thing["raceCount"]["evocube"].AsInt;
					pTest= thing["raceCount"]["test"].AsInt;
					pFeatured= thing["raceCount"]["featured"].AsInt;
					pRegular= thing["raceCount"]["regular"].AsInt;
					pRegularDrag= thing["raceCount"]["regularDrag"].AsInt;
					pPVPCity= thing["raceCount"]["pvpTimesquare"].AsInt;
					pPVPDrag= thing["raceCount"]["pvpDrag"].AsInt;
					pClub= thing["raceCount"]["club"].AsInt;

					tGTrophy= thing["totalTropyCount"]["tropy1"].AsInt;
					tSTrophy= thing["totalTropyCount"]["tropy2"].AsInt;
					tBTrophy= thing["totalTropyCount"]["tropy3"].AsInt;
					wGTrophy= thing["weeklyTropyCount"]["tropy1"].AsInt;
					wSTrophy= thing["weeklyTropyCount"]["tropy2"].AsInt;
					wBTrophy= thing["weeklyTropyCount"]["tropy3"].AsInt;

					SetChampion(slot.transform.GetChild(0));
					SetRankingTrophy(slot.transform.GetChild(1)); //trophy
					SetRacePlayRecord(slot.transform.GetChild(2)); //record
					SetAmount(slot.transform.GetChild(3));
				//	EndLoadingCircle();
				}
			}

			b = false;
		});

		while(b){
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		EndLoadingCircle();

	}
	
	void SetFaceBookMyPicture(){
		var tr = transform.FindChild("MyAccount/Profile_Pic") as Transform;
		tr.GetComponent<UITexture>().mainTexture = AccountManager.instance.myPicture;
	}
	void OnEnable(){
		Invoke("SetFaceBookMyPicture",1.0f);	
		if(SNSManager.bSNS) Invoke("SetLoginStatus", 1.0f);

		if(!Global.bLobbyBack) return;
		UserDataManager.instance.OnSubBack = ()=>{
			OnClose();
		};
	}
	void OnDisable(){
		//VIPBtn.transform.FindChild("lbName").GetComponent<UILabel>().text = "VIP";
		VIPBtn.transform.FindChild("lbLV").GetComponent<UILabel>().text = string.Format("LV.{0}", GV.gVIP);
	}
	void OnGgLogout(){
		SNSManager.GoogleLogOut(()=>{
			SetLoginStatus();
		});
	}
	void OnGgLogin(){
		SNSManager.GoogleLogin(()=>{
			SetLoginStatus();
		},()=>{
			
		});
	}
	
	void OnFBLogin(){
		SNSManager.FaceBookLogin(()=>{
			SetLoginStatus();
		}, ()=>{
		});
	}
	void OnFBLogout(){
		SNSManager.FaceBookLogOut(()=>{
			SetLoginStatus();
		});
	}
	
	void SetLoginStatus(){
		
		if(Application.isEditor){
			snsBtn.transform.FindChild("icon").GetComponent<UISprite>().spriteName ="SNS_GooglePlay";
			if (Social.localUser.authenticated) {
				lbTop[1].text= KoStorage.GetKorString("72516");
			}else{
				lbTop[1].text=KoStorage.GetKorString("72515");
			}
		}else{
			if (Application.platform == RuntimePlatform.Android){
				snsBtn.transform.FindChild("icon").GetComponent<UISprite>().spriteName ="SNS_GooglePlay";
				if (Social.localUser.authenticated) {
					snsBtn.transform.GetComponent<UIButtonMessage>().functionName  = "OnGgLogout";
					lbTop[1].text= KoStorage.GetKorString("72516");
				}else{
					snsBtn.transform.GetComponent<UIButtonMessage>().functionName  = "OnGgLogin";
					lbTop[1].text=KoStorage.GetKorString("72515");
				}
				
			}else if(Application.platform == RuntimePlatform.IPhonePlayer){
				if (Social.localUser.authenticated) {
					snsBtn.transform.GetComponent<UIButtonMessage>().functionName  = "OnAppleLogOut";
					lbTop[1].text= KoStorage.GetKorString("72516");
				}else{
					snsBtn.transform.GetComponent<UIButtonMessage>().functionName  = "OnAppleLogin";
					lbTop[1].text=KoStorage.GetKorString("72515");
				}
				snsBtn.transform.FindChild("icon").GetComponent<UISprite>().spriteName = "SNS_GameCenter";
				//snsBtn.transform.GetComponent<UIButtonMessage>().functionName = "OnAppleLogin";
			}
		}
		
		if(FB.IsLoggedIn){
			lbTop[0].text = KoStorage.GetKorString("72516");
			fbBtn.transform.GetComponent<UIButtonMessage>().functionName = "OnFBLogout";
		}else{
			lbTop[0].text =KoStorage.GetKorString("72515");
			fbBtn.transform.GetComponent<UIButtonMessage>().functionName = "OnFBLogin";
		}
		if(CClub.ClanMember == 0){
			lbTop[4].transform.parent.gameObject.SetActive(false);
		}else{
			lbTop[4].transform.parent.gameObject.SetActive(true);
			lbTop[4].text = CClub.mClubInfo.mClubName;
			lbTop[5].text = string.Format(KoStorage.GetKorString("76001"), CClub.mClubInfo.clubLevel);
			transform.FindChild("MyClub").FindChild("Club_Symbol").GetComponent<UISprite>().spriteName = CClub.mClubInfo.clubSymbol;
		}
	}
	
	
	
	void ChangeUILabel(Transform tr, string name, string mText){
		tr.FindChild(name).GetComponent<UILabel>().text = mText;
	}
	void SetChampion(Transform tr){
		ChangeUILabel(tr,"lbTitle", KoStorage.GetKorString("72909"));
		ChangeUILabel(tr,"lbSeason", KoStorage.GetKorString("72906"));
		ChangeUILabel(tr,"lbSeason_N", GV.ChSeason.ToString());
		ChangeUILabel(tr,"lbSeasonLV", KoStorage.GetKorString("72907"));
		ChangeUILabel(tr,"lbSeasonLV_N", GV.ChSeasonLV.ToString());
	}

	private int wGTrophy, wSTrophy, wBTrophy;
	private int tGTrophy, tSTrophy, tBTrophy;

	void SetRankingTrophy(Transform tr){
		ChangeUILabel(tr,"lbTitle", KoStorage.GetKorString("72600"));
		
		/*ChangeUILabel(tr.GetChild(0),"lbRanking_S", KoStorage.GetKorString("72937")); //전체
		ChangeUILabel(tr.GetChild(0),"lbRanking_S_Gold_N", string.Format("{0:#,0}", myAccount.instance.account.weeklyRace.GTrophy) );
		ChangeUILabel(tr.GetChild(0),"lbRanking_S_Silver_N", string.Format("{0:#,0}", myAccount.instance.account.weeklyRace.STrophy) );
		ChangeUILabel(tr.GetChild(0),"lbRanking_S_Bronze_N", string.Format("{0:#,0}", myAccount.instance.account.weeklyRace.BTrophy) );
		
		ChangeUILabel(tr.GetChild(1),"lbRanking_S", KoStorage.GetKorString("72936")); //이번주
		ChangeUILabel(tr.GetChild(1),"lbRanking_S_Gold_N", string.Format("{0:#,0}", myAccount.instance.account.weeklyRace.GTrophy) );
		ChangeUILabel(tr.GetChild(1),"lbRanking_S_Silver_N", string.Format("{0:#,0}", myAccount.instance.account.weeklyRace.STrophy) );
		ChangeUILabel(tr.GetChild(1),"lbRanking_S_Bronze_N", string.Format("{0:#,0}", myAccount.instance.account.weeklyRace.BTrophy) );*/
		ChangeUILabel(tr.GetChild(0),"lbRanking_S", KoStorage.GetKorString("72937")); //이번주
		ChangeUILabel(tr.GetChild(0),"lbRanking_S_Gold_N", string.Format("{0:#,0}",wGTrophy) );
		ChangeUILabel(tr.GetChild(0),"lbRanking_S_Silver_N", string.Format("{0:#,0}", wSTrophy) );
		ChangeUILabel(tr.GetChild(0),"lbRanking_S_Bronze_N", string.Format("{0:#,0}",wBTrophy ) );
		
		ChangeUILabel(tr.GetChild(1),"lbRanking_S", KoStorage.GetKorString("72936")); //전체
		ChangeUILabel(tr.GetChild(1),"lbRanking_S_Gold_N", string.Format("{0:#,0}", tGTrophy ));
		ChangeUILabel(tr.GetChild(1),"lbRanking_S_Silver_N", string.Format("{0:#,0}", tSTrophy) );
		ChangeUILabel(tr.GetChild(1),"lbRanking_S_Bronze_N", string.Format("{0:#,0}",tBTrophy ) );
	}


	private int pRanking, pRegular, pRegularDrag, pPVPDrag, pPVPCity, pTest, pEvo, pFeatured, pClub;
	void SetRacePlayRecord(Transform tr){
		ChangeUILabel(tr,"lbTitle", KoStorage.GetKorString("72918"));
		
		
		int current =pRegularDrag;
		ChangeUILabel(tr,"lbRegular_D", KoStorage.GetKorString("72920"));
		if(current == 0) ChangeUILabel(tr,"lbRegular_D_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbRegular_D_N", string.Format("{0:#,0}", current));
		
		current =pRegular;
		ChangeUILabel(tr,"lbRegular_S", KoStorage.GetKorString("72919"));
		if(current == 0) ChangeUILabel(tr,"lbRegular_S_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbRegular_S_N", string.Format("{0:#,0}", current));
		
		current = pFeatured;
		ChangeUILabel(tr,"lbEvent1", KoStorage.GetKorString("72921")); //지정 차량
		if(current == 0) ChangeUILabel(tr,"lbEvent1_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbEvent1_N",string.Format("{0:#,0}", current));
		
		current = pTest;
		ChangeUILabel(tr,"lbEvent2", KoStorage.GetKorString("72922")); //신규차량
		if(current == 0) ChangeUILabel(tr,"lbEvent2_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbEvent2_N", string.Format("{0:#,0}", current));
		
		current = pEvo;
		ChangeUILabel(tr,"lbEvent3", KoStorage.GetKorString("72923"));
		if(current == 0) ChangeUILabel(tr,"lbEvent3_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbEvent3_N", string.Format("{0:#,0}", current));
		//	ChangeUILabel(tr,"lbRanking", KoStorage.GetKorString("72924"));
		//	ChangeUILabel(tr,"lbRanking_N", string.Format("{0:00} / {1:000}",  total, current));
		current =pPVPCity;
		ChangeUILabel(tr,"lbPVP_City", KoStorage.GetKorString("72925"));
		if(current == 0) ChangeUILabel(tr,"lbPVP_City_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbPVP_City_N", string.Format("{0:#,0}", current));
		
		current = pPVPDrag;
		ChangeUILabel(tr,"lbPVP_Drag", KoStorage.GetKorString("72926"));
		if(current == 0) ChangeUILabel(tr,"lbPVP_Drag_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbPVP_Drag_N",string.Format("{0:#,0}", current));
		
		
		current =pClub;
		ChangeUILabel(tr,"lbClub", KoStorage.GetKorString("72927"));
		if(current == 0) ChangeUILabel(tr,"lbClub_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbClub_N", string.Format("{0:#,0}", current));
		
		current =pRanking;
		ChangeUILabel(tr,"lbRanking_S", KoStorage.GetKorString("72924"));
		if(current == 0) ChangeUILabel(tr,"lbRanking_S_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbRanking_S_N", string.Format("{0:#,0}", current));
	}
	
	void SetHighScore(Transform tr){
		float fTime = 100f;
		ChangeUILabel(tr,"lbTitle", KoStorage.GetKorString("72908"));
		
		
		ChangeUILabel(tr,"lbChampion_S", KoStorage.GetKorString("72909"));
		fTime = myAccount.instance.account.mRace.ChampionTime;
		if(myAccount.instance.account.mRace.ChampionPlay == 0){
			ChangeUILabel(tr,"lbChampion_S_N", string.Format(KoStorage.GetKorString("72901")));
		}else{
			ChangeUILabel(tr,"lbChampion_S_N", string.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f));
		}
		
		
		fTime = myAccount.instance.account.mRace.regularDragTime;
		ChangeUILabel(tr,"lbRegular_D", KoStorage.GetKorString("72911"));
		if(myAccount.instance.account.mRace.regularDragPlay == 0){
			ChangeUILabel(tr,"lbRegular_D_N", string.Format(KoStorage.GetKorString("72901")));
		}else{
			ChangeUILabel(tr,"lbRegular_D_N", string.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f));
			
		}
		
		fTime = myAccount.instance.account.mRace.regularStockTime;
		ChangeUILabel(tr,"lbRegular_S", KoStorage.GetKorString("72910"));
		if(myAccount.instance.account.mRace.regularStockPlay == 0){
			ChangeUILabel(tr,"lbRegular_S_N", string.Format(KoStorage.GetKorString("72901")));
		}else{
			ChangeUILabel(tr,"lbRegular_S_N", string.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f));
			
		}
		
		
		fTime = myAccount.instance.account.mRace.featureRaceTime;
		ChangeUILabel(tr,"lbEvent1", KoStorage.GetKorString("72912")); //지정차량
		if(myAccount.instance.account.mRace.featureRacePlay == 0){
			ChangeUILabel(tr,"lbEvent1_N", string.Format(KoStorage.GetKorString("72901")));
		}else{
			ChangeUILabel(tr,"lbEvent1_N", string.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f));
			
		}
		
		fTime = myAccount.instance.account.mRace.testdriveTime;
		ChangeUILabel(tr,"lbEvent2", KoStorage.GetKorString("72913")); //신규차량
		if(myAccount.instance.account.mRace.testdrivePlay == 0){
			ChangeUILabel(tr,"lbEvent2_N", string.Format(KoStorage.GetKorString("72901")));
		}else{
			ChangeUILabel(tr,"lbEvent2_N", string.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f));
			
		}
		
		fTime = myAccount.instance.account.mRace.evoTime;
		ChangeUILabel(tr,"lbEvent3", KoStorage.GetKorString("72914"));
		
		if(myAccount.instance.account.mRace.evoPlay == 0){
			ChangeUILabel(tr,"lbEvent3_N", string.Format(KoStorage.GetKorString("72901")));
		}else{
			ChangeUILabel(tr,"lbEvent3_N", string.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f));
			
		}
		fTime = myAccount.instance.account.weeklyRace.rTime;
		ChangeUILabel(tr,"lbRanking", KoStorage.GetKorString("72915"));
		if(myAccount.instance.account.weeklyRace.WeeklyPlayCount == 0){
			ChangeUILabel(tr,"lbRanking_N", string.Format(KoStorage.GetKorString("72901")));
		}else{
			ChangeUILabel(tr,"lbRanking_N", string.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f));
		}
		
		fTime = myAccount.instance.account.mRace.pvpcityTime;
		ChangeUILabel(tr,"lbPVP_City", KoStorage.GetKorString("72916"));
		if(myAccount.instance.account.mRace.pvpcityPlay == 0){
			ChangeUILabel(tr,"lbPVP_City_N", string.Format(KoStorage.GetKorString("72901")));
		}else{
			ChangeUILabel(tr,"lbPVP_City_N", string.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f));
			
		}
		
		fTime = myAccount.instance.account.mRace.pvpdragTime;
		ChangeUILabel(tr,"lbPVP_Drag", KoStorage.GetKorString("72917"));
		if(myAccount.instance.account.mRace.pvpcityPlay == 0){
			ChangeUILabel(tr,"lbPVP_Drag_N", string.Format(KoStorage.GetKorString("72901")));
		}else{
			ChangeUILabel(tr,"lbPVP_Drag_N", string.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f));
			
		}
		
		fTime = myAccount.instance.account.mRace.pvpdragTime;
		ChangeUILabel(tr,"lbClub_S", KoStorage.GetKorString("72931"));
		if(myAccount.instance.account.mRace.ClubPlay == 0){
			ChangeUILabel(tr,"lbClub_S_N", string.Format(KoStorage.GetKorString("72901")));
		}else{
			ChangeUILabel(tr,"lbClub_S_N", string.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f));
			
		}
	}
	void SetRecord(Transform tr){
		int total = 20, current = 100;
		ChangeUILabel(tr,"lbTitle", KoStorage.GetKorString("72918"));
		
		current = myAccount.instance.account.mRace.regularDragPlay;
		total = myAccount.instance.account.mRace.regularDragWin;
		ChangeUILabel(tr,"lbRegular_D", KoStorage.GetKorString("72920"));
		if(current == 0) ChangeUILabel(tr,"lbRegular_D_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbRegular_D_N", string.Format("{0:#,0}/{1:#,0}",  total, current));
		//:#,0}
		current = myAccount.instance.account.mRace.regularStockPlay;
		total = myAccount.instance.account.mRace.regularStockWin;
		ChangeUILabel(tr,"lbRegular_S", KoStorage.GetKorString("72919"));
		if(current == 0) ChangeUILabel(tr,"lbRegular_S_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbRegular_S_N", string.Format("{0:#,0}/{1:#,0}",  total, current));
		
		current = myAccount.instance.account.mRace.featureRacePlay;
		total = myAccount.instance.account.mRace.featureRaceWin;
		ChangeUILabel(tr,"lbEvent1", KoStorage.GetKorString("72921")); //지정 차량
		if(current == 0) ChangeUILabel(tr,"lbEvent1_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbEvent1_N", string.Format("{0:#,0}/{1:#,0}",  total, current));
		
		current = myAccount.instance.account.mRace.testdrivePlay;
		total = myAccount.instance.account.mRace.testdriveWin;
		ChangeUILabel(tr,"lbEvent2", KoStorage.GetKorString("72922")); //신규차량
		if(current == 0) ChangeUILabel(tr,"lbEvent2_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbEvent2_N", string.Format("{0:#,0}/{1:#,0}",  total, current));
		
		current = myAccount.instance.account.mRace.evoPlay;
		total = myAccount.instance.account.mRace.evoWin;
		ChangeUILabel(tr,"lbEvent3", KoStorage.GetKorString("72923"));
		if(current == 0) ChangeUILabel(tr,"lbEvent3_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbEvent3_N", string.Format("{0:#,0}/{1:#,0}",  total, current));
		//	ChangeUILabel(tr,"lbRanking", KoStorage.GetKorString("72924"));
		//	ChangeUILabel(tr,"lbRanking_N", string.Format("{0:00} / {1:000}",  total, current));
		current = myAccount.instance.account.mRace.pvpcityPlay;
		total = myAccount.instance.account.mRace.pvpcityWin;
		ChangeUILabel(tr,"lbPVP_City", KoStorage.GetKorString("72925"));
		if(current == 0) ChangeUILabel(tr,"lbPVP_City_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbPVP_City_N", string.Format("{0:#,0}/{1:#,0}",  total, current));
		
		current = myAccount.instance.account.mRace.pvpdragPlay;
		total = myAccount.instance.account.mRace.pvpdragWin;
		ChangeUILabel(tr,"lbPVP_Drag", KoStorage.GetKorString("72926"));
		if(current == 0) ChangeUILabel(tr,"lbPVP_Drag_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbPVP_Drag_N", string.Format("{0:#,0}/{1:#,0}",  total, current));
		
		//	ChangeUILabel(tr,"lbClub", KoStorage.GetKorString("72927"));
		//	ChangeUILabel(tr,"lbClub_N", string.Format("{0:00} / {1:000}",  total, current));
		
		
		current = myAccount.instance.account.mRace.ClubPlay;
		total = myAccount.instance.account.mRace.ClubWin;
		ChangeUILabel(tr,"lbClub", KoStorage.GetKorString("72927"));
		if(current == 0) ChangeUILabel(tr,"lbClub_N", string.Format(KoStorage.GetKorString("72903")));
		else ChangeUILabel(tr,"lbClub_N", string.Format("{0:#,0}/{1:#,0}",  total, current));
		
		
		ChangeUILabel(tr,"lbRanking_S", KoStorage.GetKorString("72932"));
		ChangeUILabel(tr,"lbRanking_S_Gold_N", string.Format("{0:#,0}", myAccount.instance.account.weeklyRace.GTrophy) );
		ChangeUILabel(tr,"lbRanking_S_Silver_N", string.Format("{0:#,0}", myAccount.instance.account.weeklyRace.STrophy) );
		ChangeUILabel(tr,"lbRanking_S_Bronze_N", string.Format("{0:#,0}", myAccount.instance.account.weeklyRace.BTrophy) );
		
	}
	void SetAmount(Transform tr){
		int car = GV.mineCarList.Count; 
		int crew = GV.listmyteaminfo.Count;
		ChangeUILabel(tr,"lbTitle", KoStorage.GetKorString("72928"));
		ChangeUILabel(tr,"lbCar", KoStorage.GetKorString("72930"));
		ChangeUILabel(tr,"lbCar_N", string.Format("{0:#,0}", car));
		
		ChangeUILabel(tr,"lbCrew", KoStorage.GetKorString("72929"));
		ChangeUILabel(tr,"lbCrew_N", string.Format("{0:#,0}",  crew));
		
	}
	
	
}
