using UnityEngine;
using System.Collections;

public class PVPSubMode : RaceMode {

	public override void OnNext (GameObject obj)
	{if(Global.isPopUp) return;
		base.OnNext (obj);
		int mode = 0;
		if(obj.name == "City"){
			obj.transform.FindChild("btn_select").gameObject.SetActive(true);
			obj.transform.parent.FindChild("Drag").FindChild("btn_select").gameObject.SetActive(false);
			bool b = myAcc.instance.account.bRaceSubBTN[3];
			if(b){
				myAcc.instance.account.bRaceSubBTN[3] = false;
			}

		}else{
			obj.transform.FindChild("btn_select").gameObject.SetActive(true);
			obj.transform.parent.FindChild("City").FindChild("btn_select").gameObject.SetActive(false);
			bool b = myAcc.instance.account.bRaceSubBTN[2];
			if(b){
				myAcc.instance.account.bRaceSubBTN[2] = false;
			}

		}
		GameObject.Find("LobbyUI").SendMessage("OnPVPRaceClick",obj.name,SendMessageOptions.DontRequireReceiver);
	}
	
	
	public override void SetRaceSubMode(){
		if(isSet) return;
		isSet = true;
		var tr = transform.GetChild(0).FindChild("City") as Transform;
		var tr1 = transform.GetChild(0).FindChild("Drag") as Transform;
		SetPVPInfo(tr,0);
		SetPVPInfo(tr1,1);
	}

	void SetPVPInfo(Transform tr, int idx){

		if(idx == 0){ //city
			tr.FindChild("mode_Name").GetComponent<UILabel>().text = KoStorage.GetKorString("73403");
			var tr1 = tr.FindChild("Info") as Transform;
			tr1.FindChild("icon_Mode2").GetComponent<UISprite>().spriteName  = "1413P";
			tr1.FindChild("icon_Mode1").GetComponent<UISprite>().spriteName  = "1413T";
			Common_Track.Item tItem = Common_Track.Get(1413);
			tr1.FindChild("Text_Name").GetComponent<UILabel>().text = tItem.Name;
			tr1.FindChild("Text_Distance").GetComponent<UILabel>().text =string.Format(KoStorage.GetKorString("73143"), tItem.Distance);
			
			tr1 = tr.FindChild("Reward");
			if(tr1.gameObject.activeSelf){
				Common_Reward.Item item = Common_Reward.Get(Global.gRewardId);
				tr1.FindChild("Material").GetComponentInChildren<UILabel>().text =string.Format(KoStorage.GetKorString("73406"), item.Reward_mat_timesquare);
				tr1.FindChild("Title_Reward").GetComponent<UILabel>().text =  KoStorage.GetKorString("73144");
				tr1.FindChild("Text1").GetComponent<UILabel>().text =  KoStorage.GetKorString("73407");
			}
			
			
		}else{ // drag
			tr.FindChild("mode_Name").GetComponent<UILabel>().text = KoStorage.GetKorString("73202");
			var tr1 = tr.FindChild("Info") as Transform;
			tr1.FindChild("icon_Mode2").GetComponent<UISprite>().spriteName  = "1412P";
			tr1.FindChild("icon_Mode1").GetComponent<UISprite>().spriteName  = "1412T_L";
			Common_Track.Item tItem = Common_Track.Get(1412);
			tr1.FindChild("Text_Name").GetComponent<UILabel>().text = tItem.Name;
			tr1.FindChild("Text_Distance").GetComponent<UILabel>().text =string.Format(KoStorage.GetKorString("73143"), tItem.Distance_big);
			
			tr1 = tr.FindChild("Reward");
			if(tr1.gameObject.activeSelf){
				Common_Reward.Item item = Common_Reward.Get(Global.gRewardId);
				tr1.FindChild("Dollor").GetComponentInChildren<UILabel>().text =  KoStorage.GetKorString("73205");//string.Format(KoStorage.GetKorString("73406"), item.Reward_mat_pvpdrag);
				tr1.FindChild("Title_Reward").GetComponent<UILabel>().text =  KoStorage.GetKorString("73144");
				tr1.FindChild("Text1").GetComponent<UILabel>().text =  KoStorage.GetKorString("73408");
			}
			
			
		}
	}

	void Start(){
		var tr = transform.GetChild(0).FindChild("Drag") as Transform;
		if(AccountManager.instance.ChampItem.S2_3_PVP_Drag == 0){
			tr.GetComponent<UIButtonMessage>().functionName = null;
			tr.FindChild("Locked").gameObject.SetActive(true);
			tr.FindChild("Locked").FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73043");
			tr.FindChild("Reward").gameObject.SetActive(false);
		}else{
			tr.GetComponent<UIButtonMessage>().functionName = "OnNext";
			tr.FindChild("Locked").gameObject.SetActive(false);
			tr.FindChild("Reward").gameObject.SetActive(true);
		}
		tr = transform.GetChild(0).FindChild("City") as Transform;
		if(AccountManager.instance.ChampItem.S3_3_PVP_City == 0){
			tr.GetComponent<UIButtonMessage>().functionName = null;
			tr.FindChild("Locked").gameObject.SetActive(true);
			tr.FindChild("Locked").FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73044");
			tr.FindChild("Reward").gameObject.SetActive(false);
		}else{
			tr.GetComponent<UIButtonMessage>().functionName = "OnNext";
			tr.FindChild("Locked").gameObject.SetActive(false);
			tr.FindChild("Reward").gameObject.SetActive(true);
		}

	}

}
	public partial class LobbyManager : MonoBehaviour {
		void OnPVPRaceClick(string str){
			if(btnstate != buttonState.WAIT) return;
			btnstate = buttonState.MAP_PVP;
			fadeIn();
			strMode = str;
			OnBackFunction = ()=>{
				isModeReturn = false;
				fadeIn();
				btnstate = buttonState.MAPTORACEMODE;
				unSetRaceSubWindow();
			};
		}
	}

