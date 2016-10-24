using UnityEngine;
using System.Collections;

public class MatSellPopup :  basePopup {
	
	
	public override void OnOkClick(){
		Callback();
		Global.isPopUp = false;
		Callback = null;
		Destroy(this);
		gameObject.SetActive(false);
	}
	
	public void InitPopUp(System.Action callback, string[] strs){
		this.Callback = callback;
		ChangeContentOKayString(KoStorage.GetKorString("75013"), KoStorage.GetKorString("71000"),
		                        string.Format(KoStorage.GetKorString("75014"),strs[0], string.Format("{0:#,0}", int.Parse(strs[1]))) );
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnCloseClick",0.1f);
			return true;
		};
	}
}
