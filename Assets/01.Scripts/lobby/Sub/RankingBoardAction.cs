using UnityEngine;
using System.Collections;

public class RankingBoardAction : MonoBehaviour {

	public GameObject SubObj;
	private Animation winAni;
	public UILabel[] lbLabels;
	bool aniDir;
	void Awake(){
		winAni = gameObject.GetComponent<Animation>() as Animation;

		//SubObj.GetComponent<SubMenuAction>().RankingBoardUpDown(checkRank, false);
		if(GV.ChSeasonID >= 6005){
			transform.FindChild("icon_fbGroup").GetChild(0).gameObject.SetActive(true);
		}

		if(GV.gInfo.crossADVersion == 0){
			transform.FindChild("Btn_crossAds").gameObject.SetActive(false);
		}else{
			transform.FindChild("Btn_crossAds").gameObject.AddComponent<CrossAD>();
			transform.FindChild("Btn_crossAds").gameObject.SetActive(true);
			transform.FindChild("Btn_crossAds").GetComponent<TweenPosition>().enabled = true;
		}
	}


	void MenuCrossTween(bool b){
		var tr = transform.FindChild("Btn_crossAds").gameObject as GameObject;
		if(!tr.activeSelf) return;
		
		return;
		var tw = tr.GetComponent<TweenPosition>() as TweenPosition;
		tw.Reset();
		if(b){
			tw.from =  Vector3.zero;
			tw.to = new Vector3(-333,0,0);
			
		}else {
			tw.to =  Vector3.zero;
			tw.from = new Vector3(-333,0,0);
		}
		
		tw.enabled = true;
	}


	void Start(){
		aniDir = true;

		lbLabels[0].text = ""; //time

	}

	void checkRank(){
		
	}

	void InitRankLable(){
		lbLabels[1].text =KoStorage.GetKorString("72605"); //times Rank
		lbLabels[2].text =  KoStorage.GetKorString("72801");; //daily
		lbLabels[3].text =KoStorage.GetKorString("72800"); //friends
		lbLabels[4].text =KoStorage.GetKorString("72200"); //world
	}


	void OnAni(){
	
		if(Global.isAnimation) return;
		Global.isAnimation = true;
		Global.isNetwork = true;
		Global.isPopUp = true;
		if(aniDir){
			aniDir = false;
			winAni["RankBoard_open_1"].time = 0;
			winAni["RankBoard_open_1"].speed = 1.0f;
			winAni.Play("RankBoard_open_1");
			
		}else{
			aniDir = true;
			winAni["RankBoard_open_1"].time =winAni["RankBoard_open_1"].length;
			winAni["RankBoard_open_1"].speed = -1.0f;
			winAni.Play("RankBoard_open_1");
		}
		Invoke("setAniButton",1.0f);
		
	}

	private float cTime = 0.0f;
	public Transform timeArrow;
	private readonly float secondsToDegrees = 360f / 60f;
	void FixedUpdate () {
		cTime += Time.deltaTime*10;
		timeArrow.localRotation = Quaternion.Euler(0f, 0f, cTime * -secondsToDegrees);
		lbLabels[0].text= UserDataManager.instance.dailyTimes();
	}

	private System.DateTime nTime;
	void setAniButton(){
		Global.isAnimation = false;
		Global.isNetwork = false;
		Global.isPopUp = false;
	}
	protected void OnIconHelpClick(){
		if(Global.isPopUp) return;
		SubObj.SendMessage("OnWeeklyWindowStart",SendMessageOptions.DontRequireReceiver);
	}

	protected void OnDailyClick(){
		if(Global.isPopUp) return;
		SubObj.SendMessage("OnRankWindows","weekly" ,SendMessageOptions.DontRequireReceiver);
	}


	protected void OnFriends(){
		if(Global.isPopUp) return;
		SubObj.SendMessage("OnFriendsWindowStart",SendMessageOptions.DontRequireReceiver);
	}

	protected void OnWorldClick(){
		if(Global.isPopUp) return;
		SubObj.SendMessage("OnRankWindows","world",SendMessageOptions.DontRequireReceiver);
	}

	protected void OnFBGroup(){
		if(Application.platform == RuntimePlatform.Android)
			Application.OpenURL(GV.gInfo.androidMarketURL);
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
			Application.OpenURL(GV.gInfo.IosURL);
	}

	protected void OnCrossWindow(){
		if(Global.isPopUp) return;
		Global.isPopUp = true;
		SubObj.SendMessage("OnCrossSubWindow",SendMessageOptions.DontRequireReceiver);
	}

}
