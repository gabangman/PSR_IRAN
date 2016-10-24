using UnityEngine;
using System.Collections;

public class VisitClanMemItem : MonoBehaviour {

	public void visitClanContent(int idx){
		//transform.FindChild("Arrow").gameObject.SetActive(false);
		//transform.FindChild("Btn_MyClan").gameObject.SetActive(false);
		int listCount = CClub.VisitClubMemList.Count;
		if(listCount <= idx){
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}
		
		
		ClubMemberInfo mClub = CClub.VisitClubMemList[idx];

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
		
		if(mClub.clubMember == 9){
			tr.FindChild("lbRight").GetComponent<UILabel>().text ="Master";
			tr.FindChild("lbRight").gameObject.SetActive(true);
		}else 	{
			tr.FindChild("lbRight").gameObject.SetActive(false);
			tr.FindChild("lbRight").GetComponent<UILabel>().text =string.Empty;
		}

		tr = transform.FindChild("Star");
		tr.FindChild("lbQunatity").GetComponent<UILabel>().text = string.Format("{0}", mClub.clubStarCount);


		CheckUserProfileLoad(mClub);
	}

	public void OnClick(){
		//transform.parent.GetComponent<ClanMemberClick>().disableArrow();
		//transform.FindChild("Arrow").gameObject.SetActive(true);
		//transform.parent.GetComponent<ClanMemberClick>().ViewClanMemberInfo(transform.name);
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
}
