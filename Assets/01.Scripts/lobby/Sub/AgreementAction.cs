using UnityEngine;
using System.Collections;

public class AgreementAction : MonoBehaviour {

	bool isUser = true, isCompany = true;
	public UICheckbox chk1, chk2;
	public UILabel[] _longText;
	bool isAgree1, isAgree2;


	public GameObject Tutorial;

	void Awake(){
		isAgree1 = false;
		isAgree2 = false;
		if(KoStorage.kostroage != null) {
			setTextString();
			//koTableAsset = null;
			return;
		}
	}

	void setTextString(){
		_longText[0].text =KoStorage.GetKorString("70101");
		_longText[1].text = KoStorage.GetKorString("70101");
		_longText[2].text =  KoStorage.GetKorString("70106");
		_longText[3].text = KoStorage.GetKorString("70103");
		_longText[4].text = KoStorage.GetKorString("70102");
		_longText[5].text =KoStorage.GetKorString("70105");
		_longText[6].text =KoStorage.GetKorString("70104");
	}

	void OnCloseClick(){
		if(isUser && isCompany) {
			gameObject.SetActive(false);
			//transform.parent.gameObject.SendMessage("StartTutorial");
			//Destroy(gameObject);
			if(Tutorial != null){
				Tutorial.SendMessage("StartTutorial");
			}
		}
		else UserDataManager.instance.StartShowTip("Check Please");
	
	}

	void SetGameObject(GameObject obj){
		Tutorial = obj;
	}


	void OnUserClick(bool ischeck){
		isUser = ischeck;
	}

	void OnCompanyClick(bool ischeck){
		isCompany = ischeck;
	}

	void OnConfirmone(){
		isAgree1 = !isAgree1;
		chk1.checkSprite.alpha =  isAgree1 ? 1f : 0f;
		if(isAgree1 && isAgree2){
			if(Tutorial != null){
				Tutorial.SetActive(true);
				Tutorial.SendMessage("StartFirstTutorial");
				StartCoroutine("agreeOk");
				return;
			}
			StartCoroutine("agreeOk");
			//gameObject.SetActive(false);
		}
	}

	void OnConfirmtwo(){
		isAgree2 = !isAgree2;
		chk2.checkSprite.alpha =  isAgree2 ? 1f : 0f;
		if(isAgree1 && isAgree2){
			if(Tutorial != null){
				Tutorial.SetActive(true);
				Tutorial.SendMessage("StartFirstTutorial");
				StartCoroutine("agreeOk");
				return;
			}
			StartCoroutine("agreeOk");//gameObject.SetActive(false);
		}
	}

	IEnumerator agreeOk(){
		EncryptedPlayerPrefs.SetInt("Agree",100);
		yield return new WaitForSeconds(0.5f);
		Global.isAnimation = false;
		gameObject.SetActive(false);
	}
	
	void OnCheckClick(bool isCheck){
	
	}

	void OnAgreeClick(){
		gameObject.SetActive(false);
	}

}
