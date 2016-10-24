using UnityEngine;
using System.Collections;

public class TeamBuyPopup : basePopup {

	public override void OnOkClick(){
		if(Callback != null)
			Callback();
		OnCloseClick();
	}
	
	public void InitPopUp(System.Action callback, int id, int res){
		string str =KoStorage.GetKorString("76027");
		int mPrice =  Common_Team.Get(id).Buyprice;
		string str3 = string.Format("{0:#,0}",  mPrice);
		string name = Common_Team.Get(id).Name;
		string str2 =string.Format(KoStorage.GetKorString("76028"), name);// "팀구매 할래?";
		var pop = transform.FindChild("Content_BUY") as Transform;
		if(res == 1){
			int a = GV.myCoin - mPrice;
			if(a < 0){
				ChangeContentOKayString(KoStorage.GetKorString("76022"), KoStorage.GetKorString("71000"),KoStorage.GetKorString("76023"));
				this.Callback = ()=>{
					GameObject.Find("LobbyUI").SendMessage("OnDollarClick");
				};
				return;
			}else{
				pop.FindChild("btnCoin").gameObject.SetActive(true);
			}
		}
		else {
			int b = GV.myDollar - mPrice;
			if(b < 0){
				ChangeContentOKayString(KoStorage.GetKorString("76022"), KoStorage.GetKorString("71000"),KoStorage.GetKorString("76023"));
				this.Callback = 	()=>{
					GameObject.Find("LobbyUI").SendMessage("OnDollarClick");
				};
				return;
			}else{
				pop.FindChild("btnDollar").gameObject.SetActive(true);
			}

		}

	

	
		pop.FindChild("lbText").GetComponent<UILabel>().text = str;
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =str3;
		pop.FindChild("lbName").GetComponent<UILabel>().text = str2;
		pop.FindChild("icon_product").gameObject.SetActive(false);
		this.Callback = callback;
		isCallbacksub = true;
		bSub = true;
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBacksub = ()=>{
			OnCloseClick();
		};
	
	}
}
