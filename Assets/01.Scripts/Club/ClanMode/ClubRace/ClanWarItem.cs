using UnityEngine;
using System.Collections;

public class ClanWarItem : SlotItems {

	public override void OnSelect(){
		if(Global.isNetwork) return;
		var temp = transform.FindChild("Select").gameObject as GameObject;
		if(temp.activeSelf) return;
		NGUITools.FindInParents<ClubRace_Start>(gameObject).OnSelectItems(transform.name);
		//temp.SetActive(true);
	}





	public void UnSelectLine(string str){
		if(str == transform.name)
			transform.FindChild("Select").gameObject.SetActive(true);	
		else transform.FindChild("Select").gameObject.SetActive(false);	
	}

	 public void ViewClanWarInfo(ClubRaceMemberInfo mInfo, int idx){


		int cnt = mInfo.clubraceCount;
		var tr = transform.FindChild("Flags") as Transform;

		for(int i =0; i< tr.childCount; i++){
			if(i < cnt){
			tr.GetChild(i).GetChild(0).gameObject.SetActive(true); // FlagOn
			}else{
				tr.GetChild(i).GetChild(0).gameObject.SetActive(false); // FlagOn
			}
		} 
	
		tr = transform.FindChild("Account") as Transform;
		tr.FindChild("lbNick").GetComponent<UILabel>().text = mInfo.NickName;
		////==!!Utility.LogWarning(idx);
		idx = idx+1;
		tr.FindChild("lbNum").GetComponent<UILabel>().text = (idx++).ToString();
		tr.FindChild("lbLevel").GetComponent<UILabel>().text = "LV "+mInfo.mLv.ToString();


		transform.FindChild("Star").GetChild(1).GetComponent<UILabel>().text =  mInfo.EarnedStarCount.ToString();
		if(mInfo.clubUserId.ToString() == GV.UserRevId){
			transform.FindChild("img_Me").gameObject.SetActive(true);
		}else{
			//tr.FindChild("img_Me").gameObject.seat(true);
			transform.FindChild("img_Me").gameObject.SetActive(false);
		}
		var temp = transform.FindChild("Select").gameObject as GameObject;
		if(temp.activeSelf) temp.SetActive(false);
		CheckUserProfileLoad(mInfo);
	}

	private void SetFlag(){
	
	}


	private void SetStar(){
		var tr = transform.FindChild("Star") as Transform;
		for(int i =0; i< tr.childCount; i++){
		//	tr.GetChild(i).GetChild(0).gameObject.SetActive(true); // StarOn
		} 
	}

	private void SetResult(){
		//transform.FindChild("Result").gameObject.SetActive(false);
	}


	void CheckUserProfileLoad(ClubRaceMemberInfo mClub){
		Texture2D mTex = mClub.userProfileTexture;
		StopCoroutine("rankUserPictureLoad");
		if(mTex == null){
			StartCoroutine("rankUserPictureLoad",mClub);
		}else{
			var tr = transform.FindChild("Account") as Transform;
			tr.FindChild("SNS_icon").GetComponent<UITexture>().mainTexture = mTex;
		}
	}
	
	IEnumerator rankUserPictureLoad(ClubRaceMemberInfo mUser){
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
			var tr = transform.FindChild("Account") as Transform;
			tr.FindChild("SNS_icon").GetComponent<UITexture>().mainTexture = pic;
			mUser.userProfileTexture = pic;
			www.Dispose();
		}
	}


}
