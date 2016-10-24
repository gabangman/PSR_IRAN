using UnityEngine;
using System.Collections;

public class StarUpPopup : MonoBehaviour {

	System.Action ReadyCallback, CloseCallback;
	public  void OnOkClick(){
		ReadyCallback();
		OnClose();
	}
	
	public void OnCloseClick(){
		CloseCallback();
		OnClose();
	}
	
	public void OnClose(){
		CloseCallback = null;
		gameObject.SetActive(false);
		Destroy(this);
	}
	public void InitPopUp(System.Action callback,System.Action callback1,int money, string strNum){
		this.ReadyCallback = callback;
		this.CloseCallback = callback1;
		string pstr = KoStorage.GetKorString(strNum);
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("76403"), pstr);//
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = null;// KoStorage.GetKorString("71000");//"확인";
		pop.FindChild("lbOk").gameObject.SetActive(true);
		pop.FindChild("lbOk").GetComponent<UILabel>().text =  KoStorage.GetKorString("71000");
		if(money == 0){
			pop.FindChild("lbName").GetComponent<UILabel>().text = KoStorage.GetKorString("76430");
		}else{
			pop.FindChild("lbName").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("76409"));//"박스 구매 할꺼야?";//string.Format(KoStorage.getStringDic("60086")+"\n\n\n\r"+KoStorage.getStringDic("60087"));
		}
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);
	}

}
