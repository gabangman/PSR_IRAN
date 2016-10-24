using UnityEngine;
using System.Collections;

public class ClubMatchStart : MonoBehaviour {
	public Transform timeArrow;
	public UILabel lbTime;
	private System.DateTime cTimes;
	private float cTime = 0.0f;
	private readonly float secondsToDegrees = 360f / 60f;
	// Use this for initialization
	void Start () {
	//	transform.GetComponent<UIButtonMessage>().target = gameObject;
	//	transform.GetComponent<UIButtonMessage>().functionName = "OnGetOppClubInfo";
	//	cTimes =NetworkManager.instance.GetCurrentDeviceTime();// System.DateTime.Now;
	//	bTimeClock = false;
	}
	

	void FixedUpdate () {
		cTime += Time.deltaTime*10;
		timeArrow.localRotation = Quaternion.Euler(0f, 0f, cTime * -secondsToDegrees);
		if(!bTimeClock) return;

		System.DateTime nNow = NetworkManager.instance.GetCurrentDeviceTime();
		System.TimeSpan mCompareTime = remainTime-nNow;//System.DateTime.Now;
		lbTime.text = string.Format("{0:00}:{1:00}:{2:00}", mCompareTime.Hours, mCompareTime.Minutes, mCompareTime.Seconds);
		mCompareTime = nNow - cTimes;
		if(mCompareTime.TotalSeconds >= CClub.mClubInfo.matchDurationSeconds){
			lbTime.text = string.Empty;
			bTimeClock = false;
			CClub.ClubMode = 4;
			NGUITools.FindInParents<ClubMatch>(gameObject).ClubModeInitialize();
			//GameObject.Find("LobbyUI").SendMessage("OnBackClick");
			return;
		}
	
	}

	void ClubMatchReadyInit(ClubReadyInfo mReady){
		//Global.isNetwork = true;
		//StartCoroutine("GetMatchInfo");
		CClub.mClubMatchingIndex = mReady.clubMatchingIndex;
		transform.FindChild("Title").GetComponent<UILabel>().text =KoStorage.GetKorString("73523");
		var tr = transform.FindChild("Score") as Transform;
		
		var tr1 = tr.FindChild("MyTeam")  as Transform;
		
		var tr2 = tr.FindChild("OtherTeam") as Transform;
		
		
		tr1.FindChild("ClanName").GetComponent<UILabel>().text = CClub.mClubInfo.mClubName;
		tr1.FindChild("ClanSymbol").GetComponent<UISprite>().spriteName = CClub.mClubInfo.clubSymbol;
		
		tr2.FindChild("ClanName").GetComponent<UILabel>().text = mReady.oppclubName;
		tr2.FindChild("ClanSymbol").GetComponent<UISprite>().spriteName =mReady.oppclubsymbol;
		
		tr = tr.FindChild("StarScore");
		
		tr.FindChild("StarCount_1").GetComponent<UILabel>().text = mReady.myClubStarCount.ToString();
		tr.FindChild("UserCount_1").GetComponent<UILabel>().text = string.Format("{0}/{1}", mReady.MemberNum, mReady.MemberTotalNum);
		
		tr.FindChild("StarCount_2").GetComponent<UILabel>().text = mReady.oppClubStarCount.ToString();
		tr.FindChild("UserCount_2").GetComponent<UILabel>().text = string.Format("{0}/{1}",mReady.oppMemberNum, mReady.oppMemberTotalNum);

		tr = transform.FindChild("ClubReward");
		
		remainTime =System.Convert.ToDateTime(mReady.matchingTime);
		cTimes = remainTime;
		System.DateTime nNow = NetworkManager.instance.GetCurrentDeviceTime();
		remainTime = remainTime.AddDays(1);
		bTimeClock = true;

	}
	private bool bTimeClock = false;
	private System.DateTime remainTime;
	void OnGetOppClubInfo(){
		//if(Global.isNetwork) return;
		//NGUITools.FindInParents<ClubMatch>(gameObject).OnNext(gameObject);
	}

	IEnumerator GetMatchInfo(){
		bool bConnect = false;///club/getMainRaceMyUserInfo
		string mAPI = "club/getMainRaceMyUserInfo";///club/getClubRankingLocal
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{

				
			}else if(status == -421){
				
			}else{
			}
			
			
			bConnect = true;
			
		});
		
		while(!bConnect){
			yield return null;
		}
		bConnect = false;//getMainRaceOppUserInfo
		mAPI = "club/getMainRaceOppUserInfo";///club/getClubRankingLocal
		mDic = new System.Collections.Generic.Dictionary<string,string>();
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				
				
			}else if(status == -421){
				
			}else{
			}
			
			Global.isNetwork = false;
			bConnect = true;
		});
		
	}
}
