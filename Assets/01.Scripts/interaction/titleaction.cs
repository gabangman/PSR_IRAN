using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class titleaction : MonoBehaviour {


	public GameObject lobby, Menu;
	//public MovieTexture bandmovie;
	bool isClick = false;

	void Start(){
		Menu = gameObject.transform.GetChild(0).gameObject;

	}


	void GameLoading(){
	
	}


	void checkMovie(){

	}



/*
    void CallbackAppInfo(bool isSuccess, string version, int status)
    {
        if (!isSuccess)
        {
            Utility.LogError("네트워크가 원활하지 않습니다!");
            return;
        }

        if(version != StoredUserData.AppVersion)
        {
            Utility.LogError("버전이 일치하지 않습니다. 새로운 버전을 받아주세요!");
            return;
        }

        if(status != 0)
        {
            Utility.LogError("서버가 점검 중입니다!");
            return;
        }
        
		//Utility.Log (Menu.name);

		if(!PlayerPrefs.HasKey("HasID")){
			PlayerPrefs.SetInt("HasID",1000);
			Menu.transform.FindChild("Menu").FindChild("Band").gameObject.SetActive(true);
		}else{
			//Utility.LogError("로그인");

		}
	
    }
	*/
	void OnCheck(){
		if(!EncryptedPlayerPrefs.HasKey("HasID")){
			EncryptedPlayerPrefs.SetInt("HasID",1000);
			Menu.transform.FindChild("Menu").FindChild("Band").gameObject.SetActive(true);
		}else{
			Utility.LogError("로그인");

		}
	}


	/*
    void OnGUI()
    {
    //    if(GUI.Button(new Rect(0, 0, 100, 100), "Anonymous"))
        {   
           
        }


        // todo : 실제 sns 연동 이후에 해야할 작업임-> 구현은 끝나있고 "고유 아이디"란 부분에 아이디만 넣으면 됨
        //if(GUI.Button(new Rect(0, 100, 100, 100), "SNS"))
        //{
        //    // todo : 1. 로컬에 게스트 계정이 이미 있다면 SNS와 연동 할 건지 물어보고 연동하기

        //    // 2. SNS로 부터 계정 고유 아이디를 받아서 서버에 로그인 인증을 한다.            
        //    if (StoredUserData.GetSavedSnsType() == SNSType.SNS)
        //        NetworkManager.RequestLogin(CallbackFinishCertification, StoredUserData.GetUserID(), SNSType.SNS, "고유아이디");
        //    else
        //    // 3. 아이디가 없는 경우에는 고유 아이디를 받아서 서버에 계정 생성을 한다.
        //        NetworkManager.RequestCreateAccount(CallbackFinishCertification, SNSType.SNS, "고유아이디");
        //}
    }
*/
    void CallbackFinishCertification(bool isSuccess)
    {

		if(isSuccess){
			TweenFinish();
			//SaveUserData();
			//
		//	UserDataManager.instance.GettingUserInfoData();
		
		}else{
		
			Utility.LogError("CallbackFinishCertification");
		}

		isClick = false;


    }
    
	void OnGuestLoginProcess(){
	
		if(isClick) return;
		isClick = true;
	}

	void OnDisableObject(){
	
		for(int i = 0; i < transform.childCount; i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}
	void OnEnableObject(){
		
		for(int i = 0; i < transform.childCount; i++){
			transform.GetChild(i).gameObject.SetActive(true);
		}
	}



	public void TweenStart(){
		var bg = Menu.transform.GetChild(0).gameObject; //title_bg
		var title = Menu.transform.GetChild(1).gameObject; //title
		var menu = Menu.transform.GetChild(2).gameObject; //Menu
		
		TweenPosition[] menuTween = menu.GetComponents<TweenPosition>();
		menuTween[0].Reset();
		menuTween[0].enabled = true;
		
		
		for(int i = 0; i < bg.transform.childCount;i++){
			StartTweenPosition(bg.transform.GetChild(i).gameObject, 0);
			StartTweenColor(bg.transform.GetChild(i).gameObject, 0);
		}
		
		
		for(int i = 0; i < title.transform.childCount;i++){
			StartTweenPosition(title.transform.GetChild(i).gameObject, 0);
		}


	var tween = 	title.AddComponent<TweenPosition>() as TweenPosition;
		tween.duration = 2.0f;
		tween.eventReceiver = gameObject;
		tween.callWhenFinished = "OnCheck";
		
	}
	
	public void TweenFinish(){

		OnNextCheck();
	
	}

	void OnNextCheck(){

		lobby.SendMessage("OnNext", SendMessageOptions.DontRequireReceiver);
	
	}
	
	void StartTweenPosition(GameObject obj, int order){
	
		TweenPosition[] tween = obj.GetComponents<TweenPosition>();
		if(order == 3){
			tween[1].eventReceiver = gameObject;
			tween[1].callWhenFinished = "OnNextCheck";
			tween[1].Reset();
			tween[1].enabled = true;
			return;
		}
		if(tween.Length != 0){
			tween[order].Reset();
			tween[order].enabled = true;
		
		}else return;
		
	
	
	}
	
	void StartTweenColor(GameObject obj, int order){
		TweenColor[] tween = obj.GetComponents<TweenColor>();
		//Utility.Log (tween.Length);
		if(tween.Length == 0) return;
		if(order == 3){
			
		
			tween[order].duration = 1.2f;
			tween[order].eventReceiver = lobby;
			tween[order].callWhenFinished = "OnNext";
			
		}

		tween[order].Reset();
		tween[order].enabled = true;
		
	}
	
	
	
	void StartTweenAlpha(GameObject obj, int order){
		TweenAlpha[] tween = obj.GetComponents<TweenAlpha>();
		if(tween.Length == 0) return;
		
		if(order == 3){
		
			tween[order].to = 1f;
			tween[order].from = 0f;
			tween[order].duration = 1.2f;
			tween[order].eventReceiver = lobby;
			tween[order].callWhenFinished = "OnNext";
		
		}
		
		tween[order].Reset();
		tween[order].enabled = true;
	}
	

    // todo : 로그인
	void OnNext(GameObject obj){

		return;
	}



}
