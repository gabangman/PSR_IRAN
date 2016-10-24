using UnityEngine;
using System.Collections;

public class ClubMatchWait : MonoBehaviour {

	public UILabel lbTime;
	public GameObject WaitIng;
	public GameObject SearchObj;
	private bool isMatching;
	public Transform timeArrow;
	private System.DateTime cTimes;
	private float cTime = 0.0f;
	private readonly float secondsToDegrees = 360f / 60f;

	void Start(){
		if(CClub.ClanMember == 1 || CClub.ClanMember == 2){
			transform.FindChild("BtnCancel").GetComponent<UIButtonMessage>().target = gameObject;
			transform.FindChild("BtnCancel").GetComponent<UIButtonMessage>().functionName = "OnMatchCancel";
			transform.FindChild("BtnCancel").FindChild("lbText1").GetComponent<UILabel>().text =KoStorage.GetKorString("70126");
		//	transform.FindChild("BtnCancel").gameObject.SetActive(true);
		}else{
			transform.FindChild("BtnCancel").gameObject.SetActive(false);
		}
		Debug.LogWarning("CClub " + CClub.ClanMember);


	}

	void OnSetMatchStart(){

		WaitIng.SetActive(true);
		SearchObj.SetActive(true);
		ChangeWaitInfo();

		remainTime =CClub.readyWaitTime;
		bTimeClock = true;

	}

	public void ChangeWaitInfo(){
	//7118 타이틀 looking for club
//7117 looking for club.....
		transform.FindChild("Title").GetComponent<UILabel>().text = KoStorage.GetKorString("77408");
		var tr  = transform.FindChild("Score") as Transform;
		var tr1 = tr.FindChild("MyTeam")  as Transform;
		

		
		tr1.FindChild("ClanName").GetComponent<UILabel>().text = CClub.mClubInfo.mClubName;
		tr1.FindChild("ClanSymbol").GetComponent<UISprite>().spriteName = CClub.mClubInfo.clubSymbol;
		
	
		transform.FindChild("Match_ing").FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("77117");
		

		tr = transform.FindChild("Info");
		Common_Track.Item tk = Common_Track.Get(1413);
		tr.FindChild("Trackname").GetComponent<UILabel>().text = tk.Name;
		tr.FindChild("TrackDistance").GetComponent<UILabel>().text =string.Format(KoStorage.GetKorString("73143"), tk.Distance);


	}

	void OnMatchCancel(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		StartCoroutine("requestMatchCancel");
	}

	IEnumerator requestMatchCancel(){
		bool bConnect = false;///club/cancelClubMatching
		string mAPI = "club/cancelClubMatching";
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				CClub.ClubMatchFlag = 0;
				CClub.ClubMode = 0;
				isMatching = false;
			}else if(status == -422){
				//==!!Utility.LogWarning("match complete and no cancel");	
			}else{
			}
			
			Global.isNetwork = false;
			bConnect = true;
			
		});
		
		while(!bConnect){
			yield return null;
		}

		if(CClub.ClubMatchFlag == 0) NGUITools.FindInParents<ClubMatch>(gameObject).OnMatchCancel();

	}

	void OnSetMatchTime(){
		WaitIng.SetActive(false);
	//	FinishIng.SetActive(true);
		SearchObj.SetActive(false);
	//	OtherObj.SetActive(true);
		isMatching =false;
	}

	void OnSetWaitTime(){
		WaitIng.SetActive(true);
	//	FinishIng.SetActive(false);
		SearchObj.SetActive(true);
	//	OtherObj.SetActive(false);
		isMatching =true;
	}

//	System.DateTime matchedTime;
//	System.TimeSpan PassedTime;
	private bool bTimeClock;
	private System.DateTime remainTime;
	void FixedUpdate(){

		cTime += Time.deltaTime*10;
		timeArrow.localRotation = Quaternion.Euler(0f, 0f, cTime * -secondsToDegrees);
		if(!bTimeClock) return;
		System.DateTime nNow = NetworkManager.instance.GetCurrentDeviceTime();
		System.TimeSpan mCompareTime = nNow- remainTime;//System.DateTime.Now;
	
		lbTime.text = string.Format("{0:00}:{1:00}:{2:00}", mCompareTime.Hours, mCompareTime.Minutes, mCompareTime.Seconds);
	}
	

	void OnEnable(){
		if(CClub.ClubMatchFlag == 1){
			isMatching = true;
			bTimeClock = true;
		}else{
			isMatching = false;
			bTimeClock = false;
		}
	}
}
