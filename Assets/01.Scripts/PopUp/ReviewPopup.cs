using UnityEngine;
using System.Collections;

public class ReviewPopup : MonoBehaviour {
	/*
	void Start(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		
		pop.FindChild("lbText").GetComponent<UILabel>().text =  KoStorage.getStringDic("60309");//"리뷰쓰기";//KoStorage.getStringDic("60234");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = KoStorage.getStringDic("60311");//"리뷰쓰러가기";// KoStorage.GetKorString("71000");//"시작";
		pop.FindChild("lbName").GetComponent<UILabel>().text = KoStorage.getStringDic("60306");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);
	}*/
	
	public void OnOkClick(){
		if(Global.isNetwork) return;
	//	Global.isNetwork = true;
		string str = GV.gInfo.HomeURL;
		if(Application.platform == RuntimePlatform.Android){
			str = "https://play.google.com/store/apps/details?id=com.gabangmanstudio.pitstopracing";
		}else if(Application.platform == RuntimePlatform.IPhonePlayer){
			str = "https://itunes.apple.com/app/pit-stop-racing-club-vs-club/id1127728680";
			str = System.Uri.EscapeUriString(str);
		}else{
			
		}
		Application.OpenURL(str);
		//UserDataManager.instance.isReview = true;//=  
		UserDataManager.instance.isPause = true;
		isReturn = true;
		Global.gReview = 0;
		CClub.gReview = 0;
		myAcc.instance.SaveAccountInfo();
	}
	public void InitPopUp(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text =  KoStorage.GetKorString("71202");//"리뷰쓰기";//KoStorage.getStringDic("60234");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = string.Empty;//KoStorage.GetKorString("71109");//"리뷰쓰러가기";// KoStorage.GetKorString("71000");//"시작";
		pop.FindChild("lbName").GetComponent<UILabel>().text = KoStorage.GetKorString("71110");
		pop.FindChild("lbOk").gameObject.SetActive(true);
		pop.FindChild("lbOk").GetComponent<UILabel>().text = KoStorage.GetKorString("71109");//"리뷰쓰러가기";// KoStorage.GetKorString("71000");//"시작";
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);
	
	}
	void OnCloseClick(){
		//Global.isNetwork = false;
		GameObject.Find("LobbyUI").SendMessage("ReviewFail");
		Global.gReview = 0;
		CClub.gReview = 0;
		Destroy(this);
		gameObject.SetActive(false);
	}

	bool isReturn = false;
	void OnApplicationFocus(bool _state){
		Global.isNetwork = false;
		if(isReturn) {
			StartCoroutine("OnWriteReview1");
			isReturn = false;
			//UserDataManager.instance.isReview = false;
			UserDataManager.instance.isPause = false;
		}
	
	}
	
	void OnApplicationPause(bool _state){
	
	}

	IEnumerator OnWriteReview1(){
		//if(UserDataManager.instance.isGoogle) yield break;
		if(UserDataManager.instance.isPause) yield break;
		Global.isNetwork = false;
		GameObject.Find("LobbyUI").SendMessage("ReviewEnd");
		Destroy(this);
		gameObject.SetActive(false);
		yield return null;
	}
}
