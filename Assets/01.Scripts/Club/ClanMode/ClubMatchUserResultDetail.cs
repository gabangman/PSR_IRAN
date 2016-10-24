using UnityEngine;
using System.Collections;

public class ClubMatchUserResultDetail : MonoBehaviour {


	public void ViewTeamContent(int idx, ClubRaceResultInfo mResult){
		ChangeInfo(gameObject, idx,mResult);
	}
	void ChangeInfo(GameObject obj, int i,ClubRaceResultInfo mResult){
		int mCnt = mResult.mMyMember.Count;
		resultClubUserInfo mClub = null;
		Transform tr = null;
		Transform tr1 = null;
		tr = obj.transform.FindChild("UserList_My") as Transform;
	//	Debug.LogWarning("count : " + i);
	//	Debug.LogWarning("mResult " + mResult.mMyMember.Count);
		if(i >= mCnt){
			tr.gameObject.SetActive(false);
			tr.FindChild("Account").gameObject.SetActive(false);
			tr.transform.FindChild("Flags").gameObject.SetActive(false);
		}else{
			tr.gameObject.SetActive(true);
			mClub = mResult.mMyMember[i];
			tr1 = tr.FindChild("Account") as Transform;
			tr1.gameObject.SetActive(true);
			tr1.FindChild("SNS_icon");
			tr1.FindChild("lbNick").GetComponent<UILabel>().text =mClub.NickName;
			tr1.FindChild("lbNum").GetComponent<UILabel>().text = (i+1).ToString();
			tr1.FindChild("lbLevel").GetComponent<UILabel>().text = string.Format("LV.{0}", mClub.mLv);
			
			tr1 = tr.transform.FindChild("Flags");
			tr1.gameObject.SetActive(true);
			CheckUserProfileLoad_my(mClub);

			int count = mResult.mMyMember[i].mRaceInfo.Count;
			if(count == 1){
				int mNum = mResult.mMyMember[i].mRaceInfo[0].raceNumber;
				if(mNum == 1){
					tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mMyMember[i].mRaceInfo[0].raceStar.ToString();

					tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(false);
					tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(true);
					tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text = string.Empty;
					
					
					tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(false);
					tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(true);
					tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text =string.Empty;


				}else if(mNum == 2){
					tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mMyMember[i].mRaceInfo[0].raceStar.ToString();

					tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = "0";
					
					
					tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(false);
					tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(true);
					tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text =string.Empty;

				
				
				}else if(mNum == 3){
					tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mMyMember[i].mRaceInfo[0].raceStar.ToString();

					tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = "0";
					
					
					tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text ="0";

				}


			}else if(count == 2){

				int mNum1 = mResult.mMyMember[i].mRaceInfo[0].raceNumber;
				int mNum2 = mResult.mMyMember[i].mRaceInfo[1].raceNumber;

				if(mNum1 == 1 && mNum2 == 2){
					tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mMyMember[i].mRaceInfo[0].raceStar.ToString();
					
					tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mMyMember[i].mRaceInfo[1].raceStar.ToString();
					
					
					tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(false);
					tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(true);
					tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text =string.Empty;
				}else if(mNum1 == 1 && mNum2 == 3){
					tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mMyMember[i].mRaceInfo[0].raceStar.ToString();
					
					tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text = "0";
					
					
					tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text =mResult.mMyMember[i].mRaceInfo[1].raceStar.ToString();
				}else if(mNum1 == 2 && mNum2 == 3){
					tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = "0";
					
					tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mMyMember[i].mRaceInfo[0].raceStar.ToString();
					
					
					tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text =mResult.mMyMember[i].mRaceInfo[1].raceStar.ToString();

				}else{
					for(int l = 0;l < 2; l++){
						string strFlag = "Flag"+(l+1).ToString();
						tr1.FindChild(strFlag).FindChild("FlagOn").gameObject.SetActive(true);
						tr1.FindChild(strFlag).FindChild("FlagOff").gameObject.SetActive(false);
						tr1.FindChild(strFlag).FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mMyMember[i].mRaceInfo[l].raceStar.ToString();
					}
					tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(false);
					tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(true);
					tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text = string.Empty;
				}


			
					
			}else if(count == 3){

				for(int l = 0;l < 3; l++){
					string strFlag = "Flag"+(l+1).ToString();
					tr1.FindChild(strFlag).FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild(strFlag).FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild(strFlag).FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mMyMember[i].mRaceInfo[l].raceStar.ToString();
					
				}
			}
	
		}


		mCnt = mResult.mOpMember.Count;
		tr = obj.transform.FindChild("UserList_Op") as Transform;
		if(i >= mCnt){
			tr.gameObject.SetActive(false);
			tr.FindChild("Account").gameObject.SetActive(false);
			tr.transform.FindChild("Flags").gameObject.SetActive(false);
		}else{
			tr.gameObject.SetActive(true);
			mClub = mResult.mOpMember[i];
		tr1 = tr.FindChild("Account");
		tr1.gameObject.SetActive(true);
		tr1.FindChild("SNS_icon");
		tr1.FindChild("lbNick").GetComponent<UILabel>().text =mClub.NickName;
		tr1.FindChild("lbNum").GetComponent<UILabel>().text = (i+1).ToString();
			tr1.FindChild("lbLevel").GetComponent<UILabel>().text =string.Format("LV.{0}", mClub.mLv);
		CheckUserProfileLoad_opp(mClub);
		tr1 = tr.transform.FindChild("Flags");
		tr1.gameObject.SetActive(true);
		int count = mResult.mOpMember[i].mRaceInfo.Count;
	
		if(count == 1){
				int mNum = mResult.mOpMember[i].mRaceInfo[0].raceNumber;
				if(mNum == 1){
					tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mOpMember[i].mRaceInfo[0].raceStar.ToString();
					
					tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(false);
					tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(true);
					tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text = string.Empty;
					
					
					tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(false);
					tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(true);
					tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text =string.Empty;
					
					
				}else if(mNum == 2){
					tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mOpMember[i].mRaceInfo[0].raceStar.ToString();
					
					tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = "0";
					
					
					tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(false);
					tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(true);
					tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text =string.Empty;
					
					
					
				}else if(mNum == 3){
					tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mOpMember[i].mRaceInfo[0].raceStar.ToString();
					
					tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = "0";
					
					
					tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text ="0";
					
				}
			
			
			}else if(count == 2){

				int mNum1 = mResult.mOpMember[i].mRaceInfo[0].raceNumber;
				int mNum2 = mResult.mOpMember[i].mRaceInfo[1].raceNumber;
				
				if(mNum1 == 1 && mNum2 == 2){
					tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mOpMember[i].mRaceInfo[0].raceStar.ToString();
					
					tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mOpMember[i].mRaceInfo[1].raceStar.ToString();
					
					
					tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(false);
					tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(true);
					tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text =string.Empty;
				}else if(mNum1 == 1 && mNum2 == 3){
					tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mOpMember[i].mRaceInfo[0].raceStar.ToString();
					
					tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text = "0";
					
					
					tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text =mResult.mOpMember[i].mRaceInfo[1].raceStar.ToString();
				}else if(mNum1 == 2 && mNum2 == 3){
					tr1.FindChild("Flag1").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag1").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag1").FindChild("lbStarNum1").GetComponent<UILabel>().text = "0";
					
					tr1.FindChild("Flag2").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag2").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag2").FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mOpMember[i].mRaceInfo[0].raceStar.ToString();
					
					
					tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text =mResult.mOpMember[i].mRaceInfo[1].raceStar.ToString();
					
				}else{
					for(int l = 0;l < 2; l++){
						string strFlag = "Flag"+(l+1).ToString();
						tr1.FindChild(strFlag).FindChild("FlagOn").gameObject.SetActive(true);
						tr1.FindChild(strFlag).FindChild("FlagOff").gameObject.SetActive(false);
						tr1.FindChild(strFlag).FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mOpMember[i].mRaceInfo[l].raceStar.ToString();
					}
						tr1.FindChild("Flag3").FindChild("FlagOn").gameObject.SetActive(false);
						tr1.FindChild("Flag3").FindChild("FlagOff").gameObject.SetActive(true);
						tr1.FindChild("Flag3").FindChild("lbStarNum1").GetComponent<UILabel>().text = string.Empty;
				}
				
				

			
			}else if(count == 3){

				for(int l = 0;l < 3; l++){
					string strFlag = "Flag"+(l+1).ToString();
					tr1.FindChild(strFlag).FindChild("FlagOn").gameObject.SetActive(true);
					tr1.FindChild(strFlag).FindChild("FlagOff").gameObject.SetActive(false);
					tr1.FindChild(strFlag).FindChild("lbStarNum1").GetComponent<UILabel>().text = mResult.mOpMember[i].mRaceInfo[l].raceStar.ToString();
					
				}
			}
		}
		
	}
	
	
	void CheckUserProfileLoad_my(resultClubUserInfo mClub){
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
	
	void CheckUserProfileLoad_opp(resultClubUserInfo mClub){
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
	
	IEnumerator UserPictureLoad_my(resultClubUserInfo mUser){
		string url = mUser.profileUrl;
		bool bHas = false;
		if(!url.Contains("http")){
			bHas = true;
		}
		if(bHas){
			mUser.userProfileTexture = (Texture2D)(Texture)Resources.Load("User_Default", typeof(Texture));
			var tr = transform.FindChild("UserList_My") as Transform;
			var tr1 = tr.FindChild("Account") as Transform;
			tr1.FindChild("SNS_icon").GetComponent<UITexture>().mainTexture = 	mUser.userProfileTexture ;
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
			var tr = transform.FindChild("UserList_My") as Transform;
			var tr1 = tr.FindChild("Account") as Transform;
			tr1.FindChild("SNS_icon").GetComponent<UITexture>().mainTexture = pic;
			www.Dispose();
		}
	}
	
	IEnumerator UserPictureLoad_opp(resultClubUserInfo mUser){
		string url = mUser.profileUrl;
		bool bHas = false;
		if(!url.Contains("http")){
			bHas = true;
		}
		if(bHas){
			mUser.userProfileTexture = (Texture2D)(Texture)Resources.Load("User_Default", typeof(Texture));
			var tr = transform.FindChild("UserList_Op") as Transform;
			var tr1 = tr.FindChild("Account") as Transform;
			tr1.FindChild("SNS_icon").GetComponent<UITexture>().mainTexture = 	mUser.userProfileTexture ;
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
			var tr = transform.FindChild("UserList_Op") as Transform;
			var tr1 = tr.FindChild("Account") as Transform;
			tr1.FindChild("SNS_icon").GetComponent<UITexture>().mainTexture = pic;
			www.Dispose();
		}
	}
	

}
