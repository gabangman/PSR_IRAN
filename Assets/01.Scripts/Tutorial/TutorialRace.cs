using UnityEngine;
using System.Collections;

public class TutorialRace : MonoBehaviour {

	public UILabel[] _lbText;
	public UILabel[] _lbDes;
	public bool isTimePause = false;
	public bool isAccelPause = false, isRace02 = false;
	public GameObject board, speech,bg,TLobby;//,startBtn;
	public Transform[] SpeechBG;
	public bool isAccelPress = true, isClutchPress = true;
	void Start () {
		_lbText[0].text  = KoStorage.GetKorString("79515");
		board.GetComponent<TweenPosition>().enabled = true;
		_lbText[1].transform.gameObject.SetActive(false);
		SpeechBG = new Transform[_lbDes.Length];
		for(int i = 0; i < _lbDes.Length; i++){
			SpeechBG[i] = _lbDes[i].transform.parent;
		}

		for(int i = 0; i < _lbDes.Length; i++){
			Vector3 vec = _lbDes[i].transform.localPosition;
			_lbDes[i].transform.localPosition = new Vector3(vec.x, vec.y, -100);
		}


	}
	private bool bBoard = false;
	public void showText(){
		speech.SetActive(true);
		if(bBoard) board.SetActive(true);
	}

	public void hiddenText(){
		speech.SetActive(false);

		if(board.activeSelf) {
			bBoard = true;
			board.SetActive(false);
		}
	}

	public void TuReadyIn(){

	}

	public void GearText(){
	
	}

	public void AccelText(){
	
	}

	public void disableCrew(){
		board.SetActive(false);
		speech.SetActive(true);
	}

	public void TuReadyOut(){
		var tw = board.GetComponent<TweenPosition>() as TweenPosition;
		Vector3 to = tw.from;
		tw.from = tw.to;
		tw.to = to;
		tw.Reset();
		tw.enabled = true;
	}

	public void RaceStartRed(){
		speech.SetActive(true);
		bg.SetActive(true);
		_lbDes[0].text =	KoStorage.GetKorString("79516");
		SpeechBG[0].gameObject.SetActive(true);
		AudioManager.Instance.OnPauseMessage();
		Time.timeScale = 0.0f;
		isTimePause = true;
		isAccelPause = true;
	}

	public void RaceStartBlue(){
		speech.SetActive(true);
		_lbDes[0].text = null;
		_lbDes[1].text =null;
		SpeechBG[0].gameObject.SetActive(false);
		SpeechBG[1].gameObject.SetActive(false);
		isTimePause = false;
		AudioManager.Instance.OnPauseMessage();
		Time.timeScale = 0.0f;
	}
	public bool isFirst = true;
	void OnNextClick(){
		speech.SetActive(false);
		Time.timeScale = 1.0f;
		AudioManager.Instance.OnResumeMessage();
		bg.SetActive(false);
		//bg.GetComponent<UIButtonMessage>().functionName = "OnReturnLobby";
		isFirst = false;
	}

	public void RaceFirstAccelPress(){
		speech.SetActive(true);
		_lbDes[0].text = null;
		_lbDes[2].text =null;
		SpeechBG[0].gameObject.SetActive(false);
		SpeechBG[2].gameObject.SetActive(false);
		_lbDes[1].text = KoStorage.GetKorString("79518");
		SpeechBG[1].gameObject.SetActive(true);
		AudioManager.Instance.OnPauseMessage();
		Time.timeScale = 0.0f;
	}

	public void RaceSecondAccelPress(){
		//StartCoroutine(showbtnSpeech(1, "60168"));
		speech.SetActive(true);
		_lbDes[1].text =
			KoStorage.GetKorString("79519");
		SpeechBG[1].gameObject.SetActive(true);
		AudioManager.Instance.OnPauseMessage();
		Time.timeScale = 0.0f;
	}

	public void hiddenBtn(string name){
		//speech.SetActive(false);
		int a = 0;
		if(name == "Btn_Gear_Ring"){
			a =2 ;
		}else{
			a =1 ;
		}
		SpeechBG[a].gameObject.SetActive(false);
		_lbDes[a].text =null;
	}

	IEnumerator showbtnSpeech(int num, string number){
		_lbDes[num].text =
			KoStorage.GetKorString(number);
		SpeechBG[num].gameObject.SetActive(true);
		yield return null;

	}
	public void showBtn(string name){
		//speech.SetActive(true);
		if(name == "Btn_Gear_Ring"){
			StartCoroutine(showbtnSpeech(2, "79517"));
		}else{
			StartCoroutine(showbtnSpeech(1, "79519"));

		}
	}

	public void show2Btn(string name){
	//	speech.SetActive(true);
		if(name == "Btn_Gear_Ring"){
			_lbDes[2].text =
				KoStorage.GetKorString("79517");
			SpeechBG[2].gameObject.SetActive(true);
		}else{
			StartCoroutine("showbtn2Speech", 0.3f);//(1,"60168"));
		}
	}
	IEnumerator showbtn2Speech(float delay){
		yield return new WaitForSeconds(delay);
		_lbDes[1].text =
			KoStorage.GetKorString("79519");
		SpeechBG[1].gameObject.SetActive(true);
		//yield return null;
		
	}

	public void hidden2Btn(string name){
		if(name == "Btn_Gear_Ring"){
			_lbDes[2].text = null;
			SpeechBG[2].gameObject.SetActive(false);
		
		}else{
			StopCoroutine("showbtn2Speech");
			_lbDes[1].text = null;
			SpeechBG[1].gameObject.SetActive(false);
		}
	}

	public void ResetSpeech(){
		for(int i = 0; i < _lbDes.Length; i++){
			_lbDes[i].text = null;
			SpeechBG[i].gameObject.SetActive(false);
		}
	}

	public void PinInSpeechStart(){
		PitInSpeech("79520");
	}

	void PitInSpeech(string str){
		var tw = board.GetComponent<TweenPosition>() as TweenPosition;
		Vector3 to = tw.from;
		tw.from = tw.to;
		tw.to = to;
		tw.onFinished = null;
		tw.Reset();
		tw.enabled = true;
		_lbText[0].text  = KoStorage.GetKorString(str);
	}

	public void PinInSpeechEnd(){
		if(!board.activeSelf){
			ScrewSpeech(-1);
			return;
		}
		var tw = board.GetComponent<TweenPosition>() as TweenPosition;
		Vector3 to = tw.from;
		tw.from = tw.to;
		tw.to = to;
		tw.onFinished = delegate(UITweener tween) {
			ScrewSpeech(-1);
		};
		tw.Reset();
		tw.enabled = true;
		//ScrewSpeech(-1);
		//_lbText[0].text  = string.Empty;
	}

	public void PitInDrillSpeach(){
		ScrewSpeech(-1);
	}


	public void ScrewSpeech(int cnt){
		if(!speech.activeSelf) speech.SetActive(true);
		var scale = SpeechBG[3].transform.GetComponent<TweenScale>();
		if(scale == null)  scale = SpeechBG[3].transform.gameObject.AddComponent<TweenScale>();
		scale.onFinished = delegate(UITweener tween) {
			SpeechBG[3].transform.localScale = Vector3.one;
		};
		scale.Reset();
		scale.enabled = true;
		SpeechBG[3].gameObject.SetActive(true);
		if(cnt == -1){
			speech.SetActive(true);
			cnt = 0;
			//return;
		}else{
			cnt = cnt+1;
			if(cnt == 5) cnt =0;
		}
		string str = (79521+cnt).ToString();
		_lbDes[3].text = KoStorage.GetKorString(str);
	}

	public IEnumerator CarmeraPitIn(){
		PitInSpeech("79526");
		yield return new WaitForSeconds(1.0f);
		var tw = board.GetComponent<TweenPosition>() as TweenPosition;
		Vector3 to = tw.from;
		tw.from = tw.to;
		tw.to = to;
		tw.onFinished = null;
		tw.Reset();
		tw.enabled = true;

	}

	public IEnumerator TutorialEnd(){
		bg.GetComponent<UIButtonMessage>().functionName = null;
		bg.SetActive(true);
		bg.GetComponent<TweenAlpha>().enabled = true;
		yield return new WaitForSeconds(1.0f);
		Global.isTutorial = false;
		TLobby.SetActive(true);
		TLobby.GetComponent<TutorialLobby>().secondTutorial1();
	
		yield return new WaitForSeconds(1.0f);
		//OnReturnLobby();
		bg.SetActive(false);
	}

	public void OnReturnLobby(){
		TLobby.GetComponent<TutorialLobby>().secondTutorial();
		transform.gameObject.SetActive(false);
		Utility.Log("OnReturnLobby");
		return;
	}


}
