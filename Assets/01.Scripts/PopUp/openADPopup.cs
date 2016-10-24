using UnityEngine;
using System.Collections;

public class openADPopup : MonoBehaviour {

	string url;
	public void openPopUp(string url){
		this.url = url;
		var pop = transform.FindChild("Content_BUY") as Transform;
		
		pop.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("71202");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");//"리뷰쓰러가기";// KoStorage.GetKorString("71000");//"시작";
		pop.FindChild("lbName").GetComponent<UILabel>().text = "마켓으로 이동????";//UserDataManager.instance.serverContents;//"server noices ";//KoStorage.getStringDic("60306");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);
		//pop.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
	}

	void OnCloseClick(){
		gameObject.SetActive(false);
		Destroy(this);
	}
	public void OnOkClick(){
		Application.OpenURL(this.url);
		//Utility.Log(this.url);
		//market://details?id=co.kr.nexteam.mahjongkakao
		gameObject.SetActive(false);
		Destroy(this);
	}
}
