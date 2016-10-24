using UnityEngine;
using System.Collections;

public class registerMenu : MonoBehaviour {
	public GameObject reg0, reg1;
	public UILabel nickLabel;
	string strCN;
	bool isClick = false;
	void OnRegisterOk(){
		if(isClick) return;
		isClick = true;
		// 닉네임 저장 후 서버에 아이디 요청
		if(nickLabel.text == string.Empty){
			isClick =false;
			return;
		}
		EncryptedPlayerPrefs.SetString("mNick", nickLabel.text);
		GV.UserNick = nickLabel.text;
		NetworkManager.instance.Register((status)=>{
			transform.GetComponent<TitleManager>().StartCoroutine("RequestServer");
			StartCoroutine("hiddenRgWin", reg0);
		});
	}

	IEnumerator hiddenRgWin(GameObject a){
		yield return new WaitForSeconds(2.0f);
		a.GetComponent<TweenAction>().ReverseTween(a);
	}

	void OnRegisterCancle(){
		//게임 종료
		EncryptedPlayerPrefs.DeleteAll();
		Application.Quit();
	}
	
	void OnSubmit(){
		
		
	}

	public void goRegistor(){
		reg0.SetActive(true);
	}

	public void showRegisterNick(){
		reg1.SetActive(true);
		GV.UserNick= EncryptedPlayerPrefs.GetString("mNick");
		reg1.transform.GetChild(0).FindChild("lbDes").GetComponent<UILabel>().text =
			string.Format(KoStorage.GetKorString("70127"), GV.UserNick);
		StartCoroutine("hiddenRgWin", reg1);
	}
	public void hiddenRegisterNick(){
		reg1.GetComponent<TweenAction>().ReverseTween(gameObject);
	}

}
