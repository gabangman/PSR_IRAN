using UnityEngine;
using System.Collections;

public class InvitePopUp : MonoBehaviour {
	System.Action callback;
	public delegate void OnInviteFinish();
	public OnInviteFinish onInviteFinish;
	int cID;
	GameObject target;
	void OnCloseClick(){
		Destroy(this);
		gameObject.SetActive(false);
	}


	public void InitPopUp(int idx){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("72600");//초대이벤트
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);
		pop.FindChild("lbName1").GetComponent<UILabel>().text = "여기에 뭐 들어가?";//string.Empty;
		//	KoStorage.GetKorString("72607");
		/*
		if(Global.gInviteCount >= 40){
			pop.FindChild("lbPrice").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");
			pop.FindChild("lbName").GetComponent<UILabel>().text = 
			KoStorage.GetKorString("72607");
			callback = ()=>{
				OnCloseClick();
			};
			return;
		}

		if(!KakaoFriends.Instance.wFriends[idx].supportedDevice){
		//	if(gameRank.instance.listInvite[idx].nGubun2 == 0){
			//pop.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("72600");;//.KoStorage.getStringDic("60041");
			pop.FindChild("lbPrice").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");
			pop.FindChild("lbName").GetComponent<UILabel>().text =
				KoStorage.getStringDic("60304");
			pop.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
			callback = ()=>{
				var pop1 = transform.FindChild("Content_BUY") as Transform;
				pop1.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
				OnCloseClick();
			};
			return;
		}
		if(KakaoFriends.Instance.wFriends[idx].messageBlocked){
			pop.FindChild("lbPrice").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");
			pop.FindChild("lbName").GetComponent<UILabel>().text =
				KoStorage.getStringDic("60259");
			pop.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
			callback = ()=>{
				var pop1 = transform.FindChild("Content_BUY") as Transform;
				pop1.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
				OnCloseClick();
			};
			return;
		}
		KakaoFriends.Instance.wFriends[idx].isWaiting = 0;
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = KoStorage.GetKorString("72608");
		pop.FindChild("lbName").GetComponent<UILabel>().text =
		string.Format(KoStorage.getStringDic("60258"),KakaoFriends.Instance.wFriends[idx].nickname );
		//string.Format(KoStorage.getStringDic("60374"),KakaoFriends.Instance.wFriends[idx].nickname );
		cID = idx;
		int count = (Global.gInviteCount+1) % 10;
		if(count == 0){
			//isEvent = true;
		}else{
			//isEvent = false;
		}

		callback = ()=>{
			if(Application.isEditor){
				GameObject.Find("LobbyUI").SendMessage("InviteEventLobbyUI", SendMessageOptions.DontRequireReceiver);
				OnClosed();
				return;
			}
			Global.isNetwork = true;
			NetworkManager.RequestSendLinkMessage("Invite",cID, (status, message)=>{
				if(status == "0"){
					KakaoFriends.Instance.wFriends[idx].isWaiting = 1;
					string str = KakaoFriends.Instance.wFriends[cID].userid;
					ProtocolManager.instance.addServerDataField("nId",str);
					string Apikeys = ServerStringKeys.API.sendInviteFriend;
					ProtocolManager.instance.ConnectServer(Apikeys, (uri)=>{
						int nret = ProtocolManager.instance.GetIntUriQuery(uri,"nRet");
						if(nret == 1){
							Global.gInviteCount= ProtocolManager.instance.GetIntUriQuery(uri,"nInviteCount");
						}else{
							
						}
						InviteEventCheck(status,message);
					//	OnClosed();
					//	GameObject.Find("LobbyUI").SendMessage("InviteEventLobbyUI", SendMessageOptions.DontRequireReceiver);
						Global.isNetwork = false;
					});
				}else{
					Utility.LogWarning("Invite Kakao Error " + message);		
					InviteErrorPopUp(status, message);
					Global.isNetwork = false;
				}
			});
			}; */
		/*
		callback = ()=>{
			Global.isNetwork = true;
		//	string str = gameRank.instance.listInvite[cID].id.Replace("-","");
			string str = KakaoFriends.Instance.wFriends[cID].userid;
			ProtocolManager.instance.addServerDataField("nId",str);
			ProtocolManager.instance.ConnectServer("sendInviteFriend", (uri)=>{
				int nret = ProtocolManager.instance.GetIntUriQuery(uri,"nRet");
				if(nret == 1){
					Global.gInviteCount= ProtocolManager.instance.GetIntUriQuery(uri,"nInviteCount");
				}else{
					
				}
				Global.isNetwork = false;
				OnInviteFuel(nret);
			});
		};
		*/
	}
	void 	InviteErrorPopUp(string status,string message){
		var pop = transform.FindChild("Content_BUY") as Transform;
		transform.GetComponent<TweenAction>().doubleTweenScale(pop.gameObject);
		pop.FindChild("lbText").GetComponent<UILabel>().text =  KoStorage.GetKorString("72600");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		//Utility.LogWarning("status : " + status + " msg : " + message);
		string strMsg = null;
		switch(status){
		case "-30": //게임별 초대 메시지 쿼터 초과
			strMsg ="게임별 초대 메시지 쿼터 초과??";// KoStorage.GetKorString("72607");
			break;
		case "-17": // 수신자 초대메시지 수신 차단.
			strMsg= "수신자 초대메시지 수신 차단??";//KoStorage.getStringDic("60259");
			//KakaoFriends.Instance.wFriends[cID].isBlock = 1;
			break;
		case "-31": //동일 발/수신자 초대메시지 쿼터 초과
			strMsg="동일 발/수신자 초대메시지 쿼터 초과??";// KoStorage.getStringDic("60284");
			break;
		case "-32": //발신자 일일 초대메시지 쿼터 초과
			strMsg= " 발신자 일일 초대메시지 쿼터 초과??";
			break;
		default:
			strMsg = string.Format("알수없는 오류로 초대에 실패했습니다.({0})",status);
			break;
		}

		pop.FindChild("lbName").GetComponent<UILabel>().text =strMsg;
		callback = ()=>{
			var pop1 = transform.FindChild("Content_BUY") as Transform;
			pop1.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
			OnClosed();
		};
	}

	void InviteEventCheck(string staus, string msg){
	//	int eventID = Global.gInviteCount / 10;
		int count = Global.gInviteCount % 10;
		//KakaoFriends.Instance.wFriends[cID].isWaiting = 1;
		if(count != 0){
				OnClosed();
				GameObject.Find("LobbyUI").SendMessage("InviteEventLobbyUI", SendMessageOptions.DontRequireReceiver);
			return;
		}else{
		
		}

		var pop = transform.FindChild("Content_BUY") as Transform;
		transform.GetComponent<TweenAction>().doubleTweenScale(pop.gameObject);
		pop.FindChild("lbText").GetComponent<UILabel>().text =  KoStorage.GetKorString("72600");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);

		/*Common_Attend.Item _item = Common_Attend.Get((8749+eventID));
		string strRe = string.Empty;
		if(_item.Type == 2){
			strRe = KoStorage.GetKorString("71001");
		}else if(_item.Type == 3){
			strRe = KoStorage.getStringDic("60227");
		}

		pop.FindChild("lbName").GetComponent<UILabel>().text =
			string.Format(KoStorage.getStringDic("60262"), Global.gInviteCount, strRe, _item.Quantity); //이벤트 달성?
		*/
		Utility.LogWarning("modify - InvitePopUp");
		callback = ()=>{
			var pop1 = transform.FindChild("Content_BUY") as Transform;
			pop1.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
			GameObject.Find("LobbyUI").SendMessage("InviteEventLobbyUI", SendMessageOptions.DontRequireReceiver);
			OnClosed();
		};
	}


	void OnOkClick(){
		if(Global.isNetwork) return;
		if(callback != null){
			callback();
		}callback = null;
	}



	void OnClosed(){
		if(onInviteFinish != null)
			onInviteFinish();
		OnCloseClick();
	}

}
