using UnityEngine;
using System.Collections;

public class BuySponsor : BuyItem {
	private int resType;
	public void SetBuySponPopUp (string BuyName, int resType, System.Action<bool> responseCallback =null, int free = 0)
	{
		string[] _name = BuyName.Split("_"[0]);
		this.BuyID = int.Parse(_name[0]);
		var Pop = transform.FindChild("Content_BUY").gameObject as GameObject;
		Common_Sponsor_Status.Item _item = Common_Sponsor_Status.Get(BuyID);
		this.BuyPrice = _item.BuyPrice;
		this.resType = resType;
		int tempid = GV.getTeamSponID(GV.SelectedTeamID);
		Pop.transform.FindChild("icon_product").gameObject.SetActive(false);
		Pop.transform.FindChild("lbName1").gameObject.SetActive(false);
		Pop.transform.FindChild("lbName").gameObject.SetActive(true);

		if(tempid != 1300){
			Pop.transform.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("76114");// 
			Pop.transform.FindChild("lbName").GetComponent<UILabel>().text =KoStorage.GetKorString("76109");// "이미 보유하고 있어서 더이상 구매 할 수 없습니다.";
			Pop.transform.FindChild("Sprite (Check_V)").gameObject.SetActive(false);	
			Pop.transform.FindChild("lbPrice").GetComponent<UILabel>().text =string.Empty;
			Pop.transform.FindChild("lbOk").gameObject.SetActive(true);
			Pop.transform.FindChild("lbOk").GetComponent<UILabel>().text =KoStorage.GetKorString("71000");
			Pop.transform.FindChild("btnok").gameObject.SetActive(true);
			OnBuyClick =()=>{
				Global.isNetwork = false;
				OnCloseClick();
			};
				
		}else{
			Pop.transform.FindChild("lbName").GetComponent<UILabel>().text =KoStorage.GetKorString("76107");//].String;
			Pop.transform.FindChild("lbText").GetComponent<UILabel>().text = string.Format( KoStorage.GetKorString("76106"), _item.Name);
			if(free == 1){
				Pop.transform.FindChild("lbPrice").GetComponent<UILabel>().text = string.Empty;
				Pop.transform.FindChild("lbOk").gameObject.SetActive(true);
				Pop.transform.FindChild("lbOk").GetComponent<UILabel>().text =KoStorage.GetKorString("71000");
				Pop.transform.FindChild("btnok").gameObject.SetActive(true);
				OnBuyClick =()=>{
					Global.isNetwork = true;
					StartCoroutine("BuySponsorFreeCount", responseCallback);	
				};
			}else{
				if(resType == 1)
					Pop.transform.FindChild("btnCoin").gameObject.SetActive(true);
				else if(resType == 2) Pop.transform.FindChild("btnDollar").gameObject.SetActive(true);
				Pop.transform.FindChild("lbPrice").GetComponent<UILabel>().text = string.Format("{0:#,0}", BuyPrice);
				OnBuyClick =()=>{
					Global.isNetwork = true;
					StartCoroutine("BuySponsorCount", responseCallback);	
				};
			}
		}
		bSubCheck = true;
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBacksub= ()=>{
			OnCloseClick();
		};
	}



	IEnumerator BuySponsorCount(System.Action<bool> callback){
		Global.isNetwork  = true;
		bool b = CheckBuyMoney(resType, BuyPrice);
		if(!b) {
			yield return new WaitForSeconds(1.0f);
		}else{
			System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string,int>();
			bool bConnect = false;
			mDic.Add("teamId",GV.SelectedTeamID);
			mDic.Add("sponsorId",BuyID);
			string mAPI = ServerAPI.Get(90009);//"game/team/sponsor"
			NetworkManager.instance.HttpFormConnect("Put", mDic,  mAPI, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
				
				}else{
					b = false;
				}
				bConnect = true;
			});
			while(!bConnect){
				yield return null;
			}
		}

		ResponseFinish(b, (bSucc)=>{
			if(!bSucc) return;
			callback(bSucc);
			Vibration.OnSponsorTime();
			GV.TeamChangeFlag = 4;

			myTeamInfo myTeam = GV.getTeamTeamInfo(GV.SelectedTeamID);
			if(myTeam == null) {
				Utility.LogWarning("myTeam null"); return; 
			}
			myTeam.SponID = BuyID;
			System.DateTime sponTime = NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
			sponTime = sponTime.AddHours(12);
			myTeam.SponRemainTime = sponTime.Ticks;
			var lobby = GameObject.Find("LobbyUI") as GameObject;
			lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
			lobby.SendMessage("PlusBuyedItem",2, SendMessageOptions.DontRequireReceiver);
			GameObject.Find("Audio").SendMessage("CompleteSound");
			AccountManager.instance.SetSponTime();
		});
		Global.isNetwork = false;

	}


	IEnumerator BuySponsorFreeCount(System.Action<bool> callback){
		Global.isNetwork  = true;
		bool b = true;
		if(!b) {
			yield return new WaitForSeconds(1.0f);
		}else{
		ResponseFinish(b, (bSucc)=>{
			if(!bSucc) return;
			callback(bSucc);
			Vibration.OnSponsorTime();
			GV.TeamChangeFlag = 4;
			myTeamInfo myTeam = GV.getTeamTeamInfo(GV.SelectedTeamID);
			if(myTeam == null) {
				Utility.LogWarning("myTeam null"); return; 
			}
			myTeam.SponID = BuyID;
			System.DateTime sponTime = NetworkManager.instance.GetCurrentDeviceTime();//System.DateTime.Now;
			sponTime = sponTime.AddHours(12);
			myTeam.SponRemainTime = sponTime.Ticks;
			var lobby = GameObject.Find("LobbyUI") as GameObject;
			lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
			lobby.SendMessage("PlusBuyedItem",2, SendMessageOptions.DontRequireReceiver);
			GameObject.Find("Audio").SendMessage("CompleteSound");
			AccountManager.instance.SetSponTime();
			ClubSponInfo.instance.mClubSpon.SetTeamSpon(BuyID,GV.SelectedTeamID, sponTime.Ticks, sponTime);
		
			});
		Global.isNetwork = false;
		
		}
	}

}
