using UnityEngine;
using System.Collections;

public class ClubMatchUserResult : MonoBehaviour {

	public GameObject View, Grid;
	public GameObject slotPrefabs;
	public GameObject readyBG;


	void Start(){

		var obj = transform.FindChild("Detail").gameObject as GameObject;
		obj.transform.FindChild("lb_ClanName_My").GetComponent<UILabel>().text = CClub.mClubInfo.mClubName;
		obj.transform.FindChild("img_ClanSymbol_My").GetComponent<UISprite>().spriteName =CClub.mClubInfo.clubSymbol;
	}



	public void AddResultList(string clanName, string clanSymbol, ClubRaceResultInfo mResultInfo){
		StartCoroutine("Loading");
		int cnt = Grid.transform.childCount;
		if(cnt == 0){
			var obj = transform.FindChild("Detail").gameObject as GameObject;
			obj.transform.FindChild("img_ClanSymbol_Op").GetComponent<UISprite>().spriteName = clanSymbol;
			obj.transform.FindChild("lb_ClanName_Op").GetComponent<UILabel>().text = clanName;

	//	{"state":0,"oppclubName":"11111","oppclubsymbol":"Clubsymbol_1","oppClubStarCount":0,"myClubStarCount":0,"oppMemberTotalNum":1,"oppMemberNum":1,"myMemberTotalNum":1,"myMemberNum":1,"raceResult":0,"racePrize":312,"winBonus":0}
			for(int i = 0; i < 30; i++){
				var temp = NGUITools.AddChild(Grid.gameObject,slotPrefabs);
				//ChangeInfo(temp, i);
			}
			Grid.GetComponent<UIGrid>().Reposition();
		}else{
			Grid.GetComponent<UIGrid>().Reposition();
		}
		StopLoading();
	}


	public void AddItem(string clanName, string clanSymbol, ClubRaceResultInfo mResultInfo){
		StartCoroutine("Loading");
		UIDraggablePanel2 drag = View.GetComponent<UIDraggablePanel2>();
		drag.maxScreenLine = 2;
		drag.maxColLine = -1;
		int count = 0;
		if(Grid.transform.childCount == 0 ){
			var obj = transform.FindChild("Detail").gameObject as GameObject;
			obj.transform.FindChild("img_ClanSymbol_Op").GetComponent<UISprite>().spriteName = clanSymbol;
			obj.transform.FindChild("lb_ClanName_Op").GetComponent<UILabel>().text = clanName;
			int mCnt0 = mResultInfo.mMyMember.Count;
			int mCnt1 = mResultInfo.mOpMember.Count;
			int mCnt2 = mCnt0 - mCnt1;
			if(mCnt2 == 0) count = mCnt0;
			else if(mCnt2 > 0) count = mCnt0;
			else if(mCnt2 < 0) count = mCnt1;
			if(count < 6) count = 6;
			drag.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().ViewRaceResultDetail(index, mResultInfo);
			});
			Grid.transform.GetComponent<UIGrid>().Reposition();
		}else{
			Grid.transform.GetComponent<UIGrid>().Reposition();
		}
		StopLoading();
	}


	bool isWaiting = false;
	IEnumerator Loading(){
		readyBG.SetActive(true);
		isWaiting = true;
		var val = readyBG.transform.GetChild(0).GetComponent<UISprite>() as UISprite;
		float delta = 0.05f;
		float mdelta = 0.0f;
		while(isWaiting){
			val.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			yield return null;
		}
	}
	
	void StopLoading(){
		isWaiting = false;
		readyBG.SetActive(false);
	}

	void ChangeInfo(GameObject obj, int i){
		var tr = obj.transform.FindChild("UserList_My") as Transform;
		var tr1 = tr.FindChild("Account") as Transform;
		tr1.FindChild("SNS_icon");
		tr1.FindChild("lbNick").GetComponent<UILabel>().text = "";
		tr1.FindChild("lbNum").GetComponent<UILabel>().text ="";
		tr1.FindChild("lbLevel").GetComponent<UILabel>().text = "";

		tr1 = tr.transform.FindChild("Flags");

		tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
		tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(true);
		tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = "";

		tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(true);
		tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(true);
		tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text = "";

		tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(true);
		tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(true);
		tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text = "";


		tr = obj.transform.FindChild("UserList_Op") as Transform;
		tr1 = tr.FindChild("Account");
		tr1.FindChild("SNS_icon");
		tr1.FindChild("lbNick").GetComponent<UILabel>().text = "";
		tr1.FindChild("lbNum").GetComponent<UILabel>().text ="";
		tr1.FindChild("lbLevel").GetComponent<UILabel>().text = "";
		
		tr1 = tr.transform.FindChild("Flags");
		tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
		tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(true);
		tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = "";
		
		tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(true);
		tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(true);
		tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text = "";
		
		tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(true);
		tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(true);
		tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text = "";
	
	}


	void CheckUserProfileLoad_my(ClubRaceMemberInfo mClub){
		Texture2D mTex = mClub.userProfileTexture;
		StopCoroutine("UserPictureLoad_my");
		if(mTex == null){
			StartCoroutine("UserPictureLoad_my",mClub);
		}else{
			var tr = transform.FindChild("UserList_My") as Transform;
			var tr1 = tr.FindChild("Account") as Transform;
			tr1.FindChild("SNS_icon").GetComponent<UITexture>().mainTexture = mTex;
		}
	}

	void CheckUserProfileLoad_opp(ClubRaceMemberInfo mClub){
		Texture2D mTex = mClub.userProfileTexture;
		StopCoroutine("UserPictureLoad_opp");
		if(mTex == null){
			StartCoroutine("UserPictureLoad_opp",mClub);
		}else{
			var tr = transform.FindChild("UserList_Op") as Transform;
			var tr1 = tr.FindChild("Account") as Transform;
			tr1.FindChild("SNS_icon").GetComponent<UITexture>().mainTexture = mTex;
		}
	}
	
	IEnumerator UserPictureLoad_my(ClubRaceMemberInfo mUser){
		string url = mUser.profileUrl;
		bool bHas = false;
		if(!url.Contains("http")){
			bHas = true;
		}
		if(bHas){
			mUser.userProfileTexture = (Texture2D)(Texture)Resources.Load("User_Default", typeof(Texture));
			transform.FindChild("Account").FindChild("SNS_icon").GetComponent<UITexture>().mainTexture = 	mUser.userProfileTexture;
			
			yield break;
		}
		WWW www = new WWW( url );
		yield return www;
		if( this == null )
			yield break;
		if( www.error != null )
		{
			//Utility.Log( "load failed" );
		}
		else
		{
			Texture2D pic = www.texture;
		//	var tr = transform.FindChild("Account") as Transform;
		//	tr.FindChild("SNS_icon").GetComponent<UITexture>().mainTexture = pic;
		//	mUser.userProfileTexture = pic;
			www.Dispose();
		}
	}

	IEnumerator UserPictureLoad_opp(ClubRaceMemberInfo mUser){
		string url = mUser.profileUrl;
		bool bHas = false;
		if(!url.Contains("http")){
			bHas = true;
		}
		if(bHas){
			mUser.userProfileTexture = (Texture2D)(Texture)Resources.Load("User_Default", typeof(Texture));
			//transform.FindChild("Account").FindChild("SNS_icon").GetComponent<UITexture>().mainTexture = 	mUser.userProfileTexture;
		//	var tr = transform.FindChild("UserList_Op") as Transform;
		//	var tr1 = tr.FindChild("Account") as Transform;
		//	tr1.FindChild("SNS_icon").GetComponent<UITexture>().mainTexture = mTex;
			yield break;
		}
		WWW www = new WWW( url );
		yield return www;
		if( this == null )
			yield break;
		if( www.error != null )
		{
			//Utility.Log( "load failed" );
		}
		else
		{
			Texture2D pic = www.texture;
		//	var tr = transform.FindChild("Account") as Transform;
		//	tr.FindChild("SNS_icon").GetComponent<UITexture>().mainTexture = pic;
		//	mUser.userProfileTexture = pic;
			www.Dispose();
		}
	}


}
