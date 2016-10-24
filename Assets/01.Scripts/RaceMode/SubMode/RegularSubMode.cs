using UnityEngine;
using System.Collections;

public class RegularSubMode : RaceMode {

	public override void OnNext (GameObject obj)
	{
		if(Global.isPopUp) return;
		base.OnNext (obj);
		string str = obj.name;
		if(str.Equals("Stock")) 
		{
			obj.transform.FindChild("btn_select").gameObject.SetActive(true);
			obj.transform.parent.FindChild("Drag").FindChild("btn_select").gameObject.SetActive(false);
			bool b = myAcc.instance.account.bRaceSubBTN[0];
			if(b){
				myAcc.instance.account.bRaceSubBTN[0] = false;
			}
		}
		else 
		{ //Drag
			obj.transform.FindChild("btn_select").gameObject.SetActive(true);
			obj.transform.parent.FindChild("Stock").FindChild("btn_select").gameObject.SetActive(false);
			bool b = myAcc.instance.account.bRaceSubBTN[1];
			if(b){
				myAcc.instance.account.bRaceSubBTN[1] = false;
			}
		}
		isSet = false;
		GameObject.Find("LobbyUI").SendMessage("OnRegularRaceClick",obj.name,SendMessageOptions.DontRequireReceiver);	
	
	}


	public override void SetRaceSubMode(){	
	//	if(isSet) return;
	//	isSet = true;
		var tr = transform.GetChild(0).FindChild("Stock") as Transform;
		var tr1 = transform.GetChild(0).FindChild("Drag") as Transform;
	//	if(GV.RegularTrack == 1400){
	//	GV.RegularTrack = Random.Range(1401,1407);
	//	}

		SetRegularInfo(tr,0);
		SetRegularInfo(tr1,1);
	}

	void SetRegularInfo(Transform tr, int idx){
		if(idx == 0){
			tr.FindChild("mode_Name").GetComponent<UILabel>().text = KoStorage.GetKorString("73201");
			var tr1 = tr.FindChild("Info") as Transform;
			tr1.FindChild("icon_Mode2").GetComponent<UISprite>().spriteName  = GV.RegularTrack.ToString()+"P";
			tr1.FindChild("icon_Mode1").GetComponent<UISprite>().spriteName  = GV.RegularTrack.ToString()+"T";
			Common_Track.Item tItem = Common_Track.Get(GV.RegularTrack);
			tr1.FindChild("Text_Name").GetComponent<UILabel>().text = tItem.Name;
			tr1.FindChild("Text_Distance").GetComponent<UILabel>().text =string.Format(KoStorage.GetKorString("73143"), tItem.Distance);

			tr1 = tr.FindChild("Reward");
			Common_Reward.Item item = Common_Reward.Get(Global.gRewardId);
			tr1.FindChild("Dollor").GetComponentInChildren<UILabel>().text =KoStorage.GetKorString("73205");
			tr1.FindChild("Title_Reward").GetComponent<UILabel>().text =  KoStorage.GetKorString("73144");
			tr1.FindChild("Text1").GetComponent<UILabel>().text =  KoStorage.GetKorString("73206");
		}else{
			tr.FindChild("mode_Name").GetComponent<UILabel>().text = KoStorage.GetKorString("73202");
			var tr1 = tr.FindChild("Info") as Transform;
			tr1.FindChild("icon_Mode2").GetComponent<UISprite>().spriteName  = "1412P";
			tr1.FindChild("icon_Mode1").GetComponent<UISprite>().spriteName  = "1412T";
			Common_Track.Item tItem = Common_Track.Get(1412);
			tr1.FindChild("Text_Name").GetComponent<UILabel>().text = tItem.Name;
			tr1.FindChild("Text_Distance").GetComponent<UILabel>().text =string.Format(KoStorage.GetKorString("73143"), tItem.Distance);
			tr1 = tr.FindChild("Reward");
			if(tr1.gameObject.activeSelf){
				Common_Reward.Item item = Common_Reward.Get(Global.gRewardId);
				tr1.FindChild("Material").GetComponentInChildren<UILabel>().text = string.Format(KoStorage.GetKorString("73406"), item.Reward_mat_regular_drag);
				tr1.FindChild("Title_Reward").GetComponent<UILabel>().text =  KoStorage.GetKorString("73144");
				tr1.FindChild("Text1").GetComponent<UILabel>().text =  KoStorage.GetKorString("73207");
			}
		}
	}

	void Start(){
		var tr1 = transform.GetChild(0).FindChild("Drag") as Transform;
		if(AccountManager.instance.ChampItem.S1_5_Regular_Drag == 0){
			tr1.GetComponent<UIButtonMessage>().functionName = null;
			tr1.FindChild("Locked").gameObject.SetActive(true);
			tr1.FindChild("Locked").FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73040");
			tr1.FindChild("Reward").gameObject.SetActive(false);
		}else{
			tr1.GetComponent<UIButtonMessage>().functionName = "OnNext";
			tr1.FindChild("Locked").gameObject.SetActive(false);
			tr1.FindChild("Reward").gameObject.SetActive(true);
		}
	}
}

public partial class LobbyManager : MonoBehaviour {
	void OnRegularRaceClick(string str){
		if(btnstate != buttonState.WAIT) return;
		btnstate = buttonState.MAP_RACE;
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


