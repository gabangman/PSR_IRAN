using UnityEngine;
using System.Collections;

public class LuckyboxPopup : MonoBehaviour {
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
	public void InitPopUp(System.Action callback,System.Action callback1,string str){
		this.ReadyCallback = callback;
		this.CloseCallback = callback1;
		string pstr = string.Empty;
		//pstr = KoStorage.GetKorString("74007");
				if (str.Equals ("Gold")) {
					pstr = KoStorage.GetKorString("74004");
						var pop = transform.FindChild ("Content_BUY") as Transform;
						pop.FindChild ("lbText").GetComponent<UILabel> ().text = pstr;//
						pop.FindChild ("lbPrice").GetComponent<UILabel> ().text = null;
						pop.FindChild ("lbName").GetComponent<UILabel> ().text = KoStorage.GetKorString ("74017");//string.Format(KoStorage.getStringDic("60086")+"\n\n\n\r"+KoStorage.getStringDic("60087"));
						pop.FindChild ("btnok").gameObject.SetActive (true);
						pop.FindChild ("icon_product").gameObject.SetActive (false);
						pop.FindChild("lbOk").gameObject.SetActive(true);
						pop.FindChild ("lbOk").GetComponent<UILabel> ().text = KoStorage.GetKorString ("71000");//"확인";
				} else { 
						pstr = KoStorage.GetKorString("74003");
						var pop = transform.FindChild ("Content_BUY") as Transform;
						pop.FindChild ("lbText").GetComponent<UILabel> ().text = pstr;//
						pop.FindChild ("lbPrice").GetComponent<UILabel> ().text =null;
						pop.FindChild ("lbName").GetComponent<UILabel> ().text = KoStorage.GetKorString ("74016");//string.Format(KoStorage.getStringDic("60086")+"\n\n\n\r"+KoStorage.getStringDic("60087"));
						pop.FindChild ("btnok").gameObject.SetActive (true);
						pop.FindChild ("icon_product").gameObject.SetActive (false);
						pop.FindChild("lbOk").gameObject.SetActive(true);
						pop.FindChild ("lbOk").GetComponent<UILabel> ().text = KoStorage.GetKorString ("71000");//"확인";
				}

		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnCloseClick",0.1f);
			return true;
		};
	}

	public void InitCouponPopUp(System.Action callback,System.Action callback1,string str){
		this.ReadyCallback = callback;
		this.CloseCallback = callback1;
		string pstr = string.Empty;
		//pstr = KoStorage.GetKorString("74007");
		if (str.Equals ("Gold")) {
			pstr = KoStorage.GetKorString("74004");
			var pop = transform.FindChild ("Content_BUY") as Transform;
			pop.FindChild ("lbText").GetComponent<UILabel> ().text = pstr;//
			pop.FindChild ("lbPrice").GetComponent<UILabel> ().text = null;
			pop.FindChild ("lbName").GetComponent<UILabel> ().text = KoStorage.GetKorString ("74021");//string.Format(KoStorage.getStringDic("60086")+"\n\n\n\r"+KoStorage.getStringDic("60087"));
			pop.FindChild ("btnok").gameObject.SetActive (true);
			pop.FindChild ("icon_product").gameObject.SetActive (false);
			pop.FindChild("lbOk").gameObject.SetActive(true);
			pop.FindChild ("lbOk").GetComponent<UILabel> ().text = KoStorage.GetKorString ("71000");//"확인";
		} else { 
			pstr = KoStorage.GetKorString("74003");
			var pop = transform.FindChild ("Content_BUY") as Transform;
			pop.FindChild ("lbText").GetComponent<UILabel> ().text = pstr;//
			pop.FindChild ("lbPrice").GetComponent<UILabel> ().text =null;
			pop.FindChild ("lbName").GetComponent<UILabel> ().text = KoStorage.GetKorString ("74020");//string.Format(KoStorage.getStringDic("60086")+"\n\n\n\r"+KoStorage.getStringDic("60087"));
			pop.FindChild ("btnok").gameObject.SetActive (true);
			pop.FindChild ("icon_product").gameObject.SetActive (false);
			pop.FindChild("lbOk").gameObject.SetActive(true);
			pop.FindChild ("lbOk").GetComponent<UILabel> ().text = KoStorage.GetKorString ("71000");//"확인";
		}
		
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnCloseClick",0.1f);
			return true;
		};
	}

	public void InitFailPopUp(System.Action callback,System.Action callback1,string str){
		this.ReadyCallback = callback;
		this.CloseCallback = callback1;
		string pstr = string.Empty;
		if (str.Equals ("Gold")) {
			pstr = KoStorage.GetKorString("74004");
			var pop = transform.FindChild ("Content_BUY") as Transform;
			pop.FindChild ("lbText").GetComponent<UILabel> ().text = pstr;//
			pop.FindChild ("lbPrice").GetComponent<UILabel> ().text = null;
			pop.FindChild ("lbName").GetComponent<UILabel> ().text = KoStorage.GetKorString ("74023");//string.Format(KoStorage.getStringDic("60086")+"\n\n\n\r"+KoStorage.getStringDic("60087"));
			pop.FindChild ("btnok").gameObject.SetActive (true);
			pop.FindChild ("icon_product").gameObject.SetActive (false);
			pop.FindChild("lbOk").gameObject.SetActive(true);
			pop.FindChild ("lbOk").GetComponent<UILabel> ().text = KoStorage.GetKorString ("71000");//"확인";
		} else { 
			pstr = KoStorage.GetKorString("74003");
			var pop = transform.FindChild ("Content_BUY") as Transform;
			pop.FindChild ("lbText").GetComponent<UILabel> ().text = pstr;//
			pop.FindChild ("lbPrice").GetComponent<UILabel> ().text =null;
			pop.FindChild ("lbName").GetComponent<UILabel> ().text = KoStorage.GetKorString ("74022");//string.Format(KoStorage.getStringDic("60086")+"\n\n\n\r"+KoStorage.getStringDic("60087"));
			pop.FindChild ("btnok").gameObject.SetActive (true);
			pop.FindChild ("icon_product").gameObject.SetActive (false);
			pop.FindChild("lbOk").gameObject.SetActive(true);
			pop.FindChild ("lbOk").GetComponent<UILabel> ().text = KoStorage.GetKorString ("71000");//"확인";
		}
		
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnCloseClick",0.1f);
			return true;
		};
	}

}
