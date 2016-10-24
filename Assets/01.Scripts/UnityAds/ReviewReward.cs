using UnityEngine;
using System.Collections;

public class ReviewReward : MonoBehaviour {

	public UILabel[] lbtext;
	public GameObject[] objs;
	private bool bTap = false;
	private bool bCircle = false;
	private int mBalance = 0;


	void Start () {
		lbtext[0].text = KoStorage.GetKorString("71110");
		lbtext[1].text = KoStorage.GetKorString("70126");
		lbtext[2].text = KoStorage.GetKorString("71109");
		lbtext[3].text = KoStorage.GetKorString("72104");
	}
	
	public void OnClaim(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		StartCoroutine("setReivewReward", 100);
	}

	public void SetInitReview(){
		objs[0].SetActive(true);
		objs[1].SetActive(false);
	}
	
	protected IEnumerator setReivewReward(int balance){

		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		mDic.Add("rewardId",8767);
		mDic.Add("type",2);
		mDic.Add("value",100);
		string mAPI = ServerAPI.Get(90068);//"user/reward"
		NetworkManager.instance.HttpFormConnect("Put",mDic, mAPI ,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				GV.myCoin += balance;
				GV.updateCoin = -balance;
				var lobby = GameObject.Find("LobbyUI") as GameObject;
				lobby.SendMessage("InitTopMenuReward", SendMessageOptions.DontRequireReceiver);
			
			}
			Global.isNetwork = false;
		});
		
		while(Global.isNetwork){
			yield return null;
		}
		GameObject.Find("Audio").SendMessage("CompleteSound");
		GameObject.Find("LobbyUI").SendMessage("ReviewEnd");
		gameObject.SetActive(false);
	}
	

	private void SetClose(){
		objs[0].SetActive(false);
		objs[1].SetActive(false);
		NGUITools.FindInParents<SubMenuWindow>(gameObject).OnCloseClick();
	}
	
	IEnumerator LoadingCircle(){
		objs[0].SetActive(true);
		transform.parent.FindChild("Contents_menuBG").gameObject.SetActive(false);
		var _mvalue = objs[0].transform.FindChild("Circle").GetComponent<UISprite>() as UISprite;
		_mvalue.fillAmount= 0.0f;
		float delta = 0.05f;
		float mdelta = 0.0f;
		while(bCircle){
			_mvalue.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			yield return null;
		}
		objs[0].SetActive(false);
	}
	

	bool isReturn = false;
	void OnApplicationFocus(bool _state){
	/*	Global.isNetwork = false;
		if(isReturn) {
			StartCoroutine("OnWriteReview1");
			isReturn = false;
			UserDataManager.instance.isPause = false;
		}
		*/
	}
	private System.DateTime clickTime;
	void OnApplicationPause(bool _state){
	//	Utility.Log("state " + _state);
		if(_state){
			clickTime = System.DateTime.Now;
		//	Utility.Log("state 1" + clickTime);
		}else{
			Global.isNetwork = false;
			if(isReturn){
			//	System.DateTime reTime =  System.DateTime.Now;
				System.TimeSpan sTime =  System.DateTime.Now - clickTime;
	//			Utility.Log("state 2" + System.DateTime.Now);
	//			Utility.Log("state 3" + sTime.Seconds);
				if(sTime.TotalSeconds >= 8){
					objs[0].SetActive(false);
					objs[1].SetActive(true);
				}
			}
		}
	}
	

	void OnReviewOk(){
		if(Global.isNetwork) return;
		isReturn = true;
		string str = GV.gInfo.HomeURL;
		if(Application.platform == RuntimePlatform.Android){
			str = "https://play.google.com/store/apps/details?id=com.gabangmanstudio.pitstopracing";
		}else if(Application.platform == RuntimePlatform.IPhonePlayer){
			str = "https://itunes.apple.com/app/pit-stop-racing-club-vs-club/id1127728680";
			str = System.Uri.EscapeUriString(str);
		}else{
			
		}
		Application.OpenURL(str);
		UserDataManager.instance.isPause = true;
		Global.gReview = 0;
		CClub.gReview = 0;
		myAcc.instance.SaveAccountInfo();

	}

	void OnReviewCancle(){
		GameObject.Find("LobbyUI").SendMessage("ReviewFail");
		Global.gReview = 0;
		CClub.gReview = 0;
		gameObject.SetActive(false);
	}
}
