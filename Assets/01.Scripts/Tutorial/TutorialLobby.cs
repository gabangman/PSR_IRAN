using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class TutorialLobby : MonoBehaviour {

	public UILabel[] _text;
	public GameObject raceicon, selectcar, selectcrew, start, Agree;
	public GameObject closeButton, closePop;
	bool isClick;
	//string KoTableString;
	int clickCount = 0;
	const int textCount = 79507;
	[SerializeField] private AudioSource _TutorialAudio;
	[SerializeField] protected AudioClip s_Complete, s_Whip;


	void Awake(){
		closeButton.SetActive(false);
		if(KoStorage.kostroage != null) {
			return;
		}
		if(Agree == null) return;
	}

	void OnTutorialEnd(){
		closePop.SetActive(true);
		var popchild = closePop.transform.FindChild("Content").gameObject as GameObject;
		closePop.GetComponent<TweenAction>().doubleTweenScale(popchild);
		popchild.transform.FindChild("lbName").GetComponent<UILabel>().text = 
			KoStorage.GetKorString("79501");
		popchild.transform.FindChild("lbPrice").GetComponent<UILabel>().text = 
			KoStorage.GetKorString("71000");
		popchild.transform.FindChild("lbText").GetComponent<UILabel>().text = 
			KoStorage.GetKorString("79502");
	}

	void OnClosePop(){
	//	Global.gTutorial = 0;
	//	Global.isTutorial = false;
	//	Global.isReTutorial = false;
	//	Global.isRaceTest = false;
		closePop.SetActive(false);
	}

	void OnLobbyReturn(){
		//EncryptedPlayerPrefs.SetInt("Tutorial",100);
		Global.gTutorial = 0;
		Global.isTutorial = false;
		Global.isReTutorial = false;
		Global.isRaceTest = false;
		Application.LoadLevel("Title");
	}

	void Start () {
		if(Global.isTutorial) {
			firstTutorial();
		}else{
	
		}
		if(Global.gChampTutorial == 0){
			Global.gRaceInfo = new RaceInfo();
			Global.tutorialracesetting();
			Global.gRaceInfo.init();
			closeButton.SetActive(true);
			Global.bLobbyBack = true;
			UserDataManager.instance.OnSubBack = ()=>{
				OnTutorialEnd();
			};

		}else if(Global.gChampTutorial == 1){
			closeButton.SetActive(false);
				firstTutorial();
		}else if(Global.gChampTutorial == -1){
			closeButton.SetActive(false);
			secondTutorial1();
			_text[3].text = null;
		}
		_text[2].text = KoStorage.GetKorString("79534");
		
		if(_text[3] == null) {
			_text[4].text = KoStorage.GetKorString("76307");
			_text[5].text = KoStorage.GetKorString("76307");
			_text[6].text = KoStorage.GetKorString("76307");
			return;
		}
		_text[3].text = KoStorage.GetKorString("79510");
		_text[4].text = KoStorage.GetKorString("73048");
		_text[5].text = KoStorage.GetKorString("79512");



	}

	void firstTutorial(){
		int textcnt = textCount+clickCount;
		_text[0].text =KoStorage.GetKorString(textcnt.ToString());
		clickCount++;
		_TutorialAudio.PlayOneShot(s_Whip);
	}


	public void secondTutorial1(){
		transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
			"secondTutorial";
		transform.FindChild("BG").gameObject.SetActive(true);
		transform.FindChild("BG").GetComponent<UISprite>().alpha = 1.0f;
		_text[2].transform.gameObject.SetActive(true);
		_text[0].text = KoStorage.GetKorString("79535");
		isClick = false;
	}

	void OnReturnLobby(){
	
	}

	public void secondTutorial(){
		if(isClick) return;
		isClick = true;
		closeButton.SetActive(false);//closeButton.SetActive(Global.isReTutorial);
		transform.FindChild("BG").gameObject.SetActive(true);
		int textcnt =79529;
		clickCount = 0;
		//_text[0].text =KoStorage.getStringDic(textcnt.ToString());// dictionary[textcnt.ToString()].String;
		textAction(KoStorage.GetKorString(textcnt.ToString()));
		transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
			"OnNext2Click";
		clickCount++;
	}

	void OnNext2Click(){
		if(isClick) return;
		isClick = true;
		int textcnt = 79506;
		textAction(KoStorage.GetKorString(textcnt.ToString()));
		if(Global.isReTutorial){
			transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
				"OnCloseClick";
			return;
		}
		transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
				"OnNext3Click";
		//Utility.Log("OnNext2" + isClick);
	}

	void OnNext3Click(){
		//Utility.Log("OnNext3" + isClick);
		if(isClick) return;
		isClick = true;

		if(clickCount == 3){
		//	transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
		//		"OnNext4Click";
			selectcar.SetActive(true);
			selectcar.transform.GetChild(1).FindChild("BTN_1000").gameObject.SetActive(true);
			_text[0].text = string.Empty;
			transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
				null;
				return;
		}
		int textcnt = 79529+clickCount;
		textAction(KoStorage.GetKorString(textcnt.ToString()));
		clickCount++;
	}

	void OnNext4Click(){
		selectcar.SetActive(true);
		_text[0].text = string.Empty;
		transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
			null;
	}

	void OnNext5Click(){
		if(isClick) return;
		isClick = true;
		selectcrew.SetActive(true);
		_text[0].text = string.Empty;
		textAction(string.Empty);
		transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
			null;
	}

	void OnSelectCrew(GameObject obj){
		if(isClick) return;
		isClick = true;
		//Utility.Log("SElecet  Crew" + obj.name);
		int textcnt = 79529+clickCount;
		//_text[0].text = dictionary[textcnt.ToString()].String;
		textAction(KoStorage.GetKorString(textcnt.ToString()));
		transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
			"OnCloseClick";
		selectcrew.SetActive(false);
		_TutorialAudio.PlayOneShot(s_Complete);
	}
	void SetFinishTutorial(){
		transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
			"OnNext5Click";

		selectcar.SetActive(false);
		isClick = true;
		int textcnt = 79529+clickCount;
		textAction(KoStorage.GetKorString(textcnt.ToString()));
		clickCount++;
		Global.isNetwork = false;


	}
	void OnSelectCar(GameObject obj){
		//Utility.Log("SElecet " + obj.name);
		if(Global.isNetwork) return;
		int temp = int.Parse(obj.name);
		//Global.MyCarID =Base64Manager.instance.GlobalEncoding(temp);
		_TutorialAudio.PlayOneShot(s_Complete);
		Global.isNetwork = true;
		Invoke ("SetFinishTutorial", 1.0f);
		return; /*
			ProtocolManager.instance.addServerDataField("nCarId", temp.ToString());
			ProtocolManager.instance.ConnectServer("getTutorialCar", (uri)=>{
				int ret = 0;
				int.TryParse(System.Web.HttpUtility.ParseQueryString(uri.Query).Get("nRet"), out ret);
				if(ret == 1){
					// update car Value;
					transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
						"OnNext5Click";
						
					myAccount.instance.account.listCarInfo[0].carId =temp;
					if(temp != 1000)
					{	myAccount.instance.account.buttonStatus.TeamCarNews[0] = false;
						myAccount.instance.account.buttonStatus.TeamCarNews[1] = true;
						myAccount.instance.account.buttonStatus.isCarNew[1] = false;
						myAccount.instance.account.buttonStatus.isCarNew[0] = true;
					}
					//b = true;
				}else if(ret == 0){
					transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
						"OnNext5Click";
				}else{
					//Utility.LogError("already have processed Tutorial ");
				//Global.MyCarID = Base64Manager.instance.GlobalEncoding(Global.gTutorialCarID);
				//Global.MySponsorID = Base64Manager.instance.GlobalEncoding(Global.gTutorialSponsor);
					transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
						"OnNext5Click";
				}
				selectcar.SetActive(false);
				isClick = true;
				int textcnt = 79529+clickCount;
				textAction(KoStorage.GetKorString(textcnt.ToString()));
				clickCount++;
				Global.isNetwork = false;
				ProtocolManager.instance.setTutorial();
			}); */
		
	}

	void OnNextClick(){
		if(isClick) return;
		isClick = true;
		int textcnt = 0;
		if(clickCount == 6){
			raceicon.SetActive(false);
		}
		if(clickCount == 7){
			start.transform.parent.gameObject.SetActive(true);
			_text[2].transform.gameObject.SetActive(false);
			transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
				null;
			if(Global.gChampTutorial == 0){
				start.GetComponent<UIButtonMessage>().functionName = "OnRaceClick";
			}else if(Global.gChampTutorial == 1){
				start.GetComponent<UIButtonMessage>().functionName = "OnStart";
			}
	
		}
			textcnt = textCount+clickCount;
			clickCount++;
			textAction(KoStorage.GetKorString(textcnt.ToString()));
		if(clickCount == 3){
			transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
				"OnNext1Click";
		}			
	}
	int raceCount = 0;
	void OnNext1Click(){
		if(isClick) return;
		isClick = true;
		textActionDescription();
		clickCount++;

	}
	void textActionDescription(){
		var tween = _text[0].transform.GetComponent<TweenPosition>() as TweenPosition;
		tween.from = new Vector3(0,0,-5);
		tween.to = new Vector3(1000,0,-5);
		tween.duration = 0.1f;
		tween.onFinished = delegate(UITweener tw1) {
			var tw = tw1.transform.GetComponent<TweenPosition>() as TweenPosition;
			tw.to =new Vector3(0,0,-5);
			tw.from = new Vector3(-1000,0,-5);
			tw.duration = 0.1f;
			tw.onFinished = delegate(UITweener tween1) {
				raceicon.SetActive(true);
				raceicon.transform.GetChild(raceCount).gameObject.SetActive(true);
				raceCount++;
				if(raceCount == 3){
					transform.FindChild("BG").GetComponent<UIButtonMessage>().functionName = 
						"OnNextClick";
				}
				isClick = false;
			};
			tw.Reset();
			tw.enabled = true;
			_text[0].text = string.Empty;
			
			//_TutorialAudio.PlayOneShot(s_Whip);
		};
		tween.Reset();
		tween.enabled = true;
	
	}
	
	
	void OnRaceClick(){
		GameObject.Find("Audio").SendMessage("StartButtonPress");
		if(Global.gChampTutorial == 0){
			GV.PlayCarID = 1006;
			GV.PlaySponID = 1303;
			GV.PlayCrewID = 1200;
			GV.PlayClassID = "SS";
		}
		UserDataManager.instance.OnSubBack = null;
		Global.bLobbyBack = false;
		transform.parent.FindChild("Loading_Tutorial").gameObject.SetActive(true);
		gameObject.SetActive(false);
		//Utility.LogWarning("GV " + GV.ChSeason);
	}

	void OnTutorialCheckClick(){
		OnCloseClick();
	}

	void OnCloseClick(){

		if(Global.gChampTutorial == -1){
			Global.isTutorial = false;
			Global.isReTutorial = false;
			Global.isRaceTest = false;
			Global.gChampTutorial = 0;
			GameObject.Find("LobbyUI").SendMessage("EndTutorial",SendMessageOptions.DontRequireReceiver);
			return;
		}

		if(Global.isReTutorial){
			//Global.MyCarID = Base64Manager.instance.GlobalEncoding(Global.gTutorialCarID);// myAccount.instance.account.listCarInfo[0].carId;
		}
		Global.isTutorial = false;
		Global.isReTutorial = false;
		Global.isRaceTest = false;
		//EncryptedPlayerPrefs.SetInt("Tutorial",100);
		Global.gTutorial = 0;
		SceneManager.instance.StartCoroutine("LoadFirstScene","Title");
		UserDataManager.instance.OnSubBack = null;
		Global.bLobbyBack = false;
	}

	void textAction(string text){
		var tween = _text[0].transform.GetComponent<TweenPosition>() as TweenPosition;
		tween.from = new Vector3(50,0,-5);
		tween.to = new Vector3(1000,0,-5);
		tween.onFinished = delegate(UITweener tw1) {
			var tw = tw1.transform.GetComponent<TweenPosition>() as TweenPosition;
			tw.to =new Vector3(50,0,-5);
			tw.from = new Vector3(-1000,0,-5);
			
			tw.onFinished = delegate(UITweener tween1) {
				isClick = false;
			};
			tw.Reset();
			tw.enabled = true;
			_text[0].text = text;
			_TutorialAudio.PlayOneShot(s_Whip);
		};
		tween.Reset();
		tween.enabled = true;
	}


	void OnStart(GameObject obj){
		GameObject.Find("Audio").SendMessage("StartButtonPress");
		Global.isTutorial = false;
		Global.isReTutorial = false;
		Global.isRaceTest = false;
		//Utility.LogWarning("OnStart");
		gameObject.AddComponent<SettingRaceChamp>().setChampRace();
		obj.SetActive(false);

	}
	//선택
}
