using UnityEngine;
using System.Collections;

public class ViewMyClanItem : SlotItems {

	public void visitMyClanContent(int idx){
	
	}

	public void ChangeItemContents(){
		int cnt = 0;
		string[] str = transform.name.Split('_');
		int.TryParse(str[1], out cnt);
		ChangeContents(cnt);
	}

	public override void ChangeContents (int idx)
	{
		int listCount = CClub.myClubMemInfo.Count;
		////==!!Utility.LogWarning("1 " + listCount);		//==!!Utility.LogWarning(" 2 ++ " + idx);
		if(listCount <= idx){
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}


		ClubMemberInfo mClub = CClub.myClubMemInfo[idx];
		if(mClub.myMemInfo == 1){
			transform.FindChild("BG_me").gameObject.SetActive(true);
		}else{
			transform.FindChild("BG_me").gameObject.SetActive(false);
		}
		int rank = idx+1;
		var tr = transform.FindChild("Account") as Transform;
		tr.FindChild("lbNick").GetComponent<UILabel>().text = mClub.nickName;
		tr.FindChild("lbRankNum").GetComponent<UILabel>().text = (rank).ToString();
		tr.FindChild("lbLevel").GetComponent<UILabel>().text = "LV "+mClub.mLv.ToString();
		if(string.IsNullOrEmpty(mClub.LastConnectTime)== true){
			tr.FindChild("lbtime").GetComponent<UILabel>().text = mClub.JoinTime;
		}else{
			tr.FindChild("lbtime").GetComponent<UILabel>().text = mClub.LastConnectTime;
		}
	
		float delta = 0;
		if(mClub.playTotalNumber == 0){
			delta = 0;
		}else{
			if(mClub.playNumber == 0){
				delta = 0;
			}else{
				delta =(float)mClub.playNumber/(float)mClub.playTotalNumber;
				delta = delta*100;

			}
		}
		tr.FindChild("lbIntegrityPercent").GetComponent<UILabel>().text =string.Format("{0}%",(int)delta);
		tr.FindChild("lbIntegrity").GetComponent<UILabel>().text = KoStorage.GetKorString("77110");

		if(mClub.clubMember == 9){ // 마스터 
			tr.FindChild("lbRight").GetComponent<UILabel>().text =KoStorage.GetKorString("77222");
			tr.FindChild("lbRight").gameObject.SetActive(true);
		}else if(mClub.clubMember == 5){ // 스텝
			tr.FindChild("lbRight").GetComponent<UILabel>().text =KoStorage.GetKorString("77223");
			tr.FindChild("lbRight").gameObject.SetActive(true);
		}else tr.FindChild("lbRight").gameObject.SetActive(false);
	
		CheckUserProfileLoad(mClub);

		if(mClub.mUserEntryFlag == 1){
			tr.FindChild("icon_entree").gameObject.SetActive(true);
		}else{
			tr.FindChild("icon_entree").gameObject.SetActive(false);
		}

		tr = transform.FindChild("Star");
		tr.FindChild("lbQunatity").GetComponent<UILabel>().text = string.Format("{0}", mClub.clubStarCount);

	}

	public void ChangeStaffContents(int idx){
		ClubMemberInfo mClub = CClub.myClubMemInfo[idx];
		var tr = transform.FindChild("Account") as Transform;
		if(mClub.clubMember == 9){ // 마스터 
			tr.FindChild("lbRight").GetComponent<UILabel>().text =KoStorage.GetKorString("77222");
			tr.FindChild("lbRight").gameObject.SetActive(true);
		}else if(mClub.clubMember == 5){ // 스텝
			tr.FindChild("lbRight").GetComponent<UILabel>().text =KoStorage.GetKorString("77223");
			tr.FindChild("lbRight").gameObject.SetActive(true);
		}else tr.FindChild("lbRight").gameObject.SetActive(false);
	}

	void CheckUserProfileLoad(ClubMemberInfo mClub){
		Texture2D mTex = mClub.userProfileTexture;
		StopCoroutine("rankUserPictureLoad");
		if(mTex == null){
			StartCoroutine("rankUserPictureLoad",mClub);
		}else{
			var tr = transform.FindChild("Account") as Transform;
			tr.FindChild("Pic_icon").GetComponent<UITexture>().mainTexture = mTex;
		}
	}
	
	
	IEnumerator rankUserPictureLoad(ClubMemberInfo mUser){
		
		string url = mUser.UserProfile;
		bool bHas = false;
		if(!url.Contains("http")){
			bHas = true;
		}
		if(bHas){

			mUser.userProfileTexture = (Texture2D)(Texture)Resources.Load("User_Default", typeof(Texture));
			transform.FindChild("Account").FindChild("Pic_icon").GetComponent<UITexture>().mainTexture = 	mUser.userProfileTexture;
		
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
			var tr = transform.FindChild("Account") as Transform;
			tr.FindChild("Pic_icon").GetComponent<UITexture>().mainTexture = pic;
			mUser.userProfileTexture = pic;
			www.Dispose();
		}
	}


	public override void OnClick(){
		////==!!Utility.LogWarning("OnClick");
		var tr = transform.FindChild("Select") as Transform;
		if(tr.gameObject.activeSelf) return;
		transform.parent.GetComponent<ClanMemberClick>().disableSelect();
		transform.parent.GetComponent<ClanMemberClick>().ViewClanMemberInfo(transform.name);
		tr.gameObject.SetActive(true);
	}
}
