using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class UserDataManager : MonoSingleton< UserDataManager > {
	public int ChatEnable = 0;
	public void JiverInit(){
		jiver.JiverChannelJoins();// jiverChannelJoins();
	}
	public void JiverUnSetInit(){
	//	jiver.jiverUnSetInit();
	}
	public void JiverSend(string str){
		jiver.send(str);
	}

	public List<string> jiverGetMessage(){
		return jiver.getMessageList();
	}

	public int JiverMessageCount(){
		return jiver.getMessageCount();
	}

	public void jiverAddMessage(string str){
		//jiver.addtMessage(str);
	}

	private GameObject mLobby;
	public void jiverReceivedMessage(string str){
		//Debug.LogWarning(str);
		if(CClub.mClubFlag != 2)  return;
		if(ChatEnable !=0){
			if(mLobby == null)
				mLobby = GameObject.Find("LobbyUI");

			//Debug.LogWarning("jiverReMesage : " + str);
			mLobby.SendMessage("onChatString", str , SendMessageOptions.DontRequireReceiver);
		}else{

		}
	}

	public void OnRoutinCheck(bool b){
		if(b) {
			ChatEnable = 1;
		}else{
			ChatEnable = 0;
			mLobby = null;
		}
	}

	public void OnChangeChannel(bool b){
		jiver.JiverChannelChange(b);
	}

	public void JoinMessage(){
		if(!CClub.bClanWarChat){
			Invoke("JoinSendMessage", 2.0f);
		}
	}

	void JoinSendMessage(){
		string str1 = string.Format(KoStorage.GetKorString("77203"), GV.UserNick);
		JiverSend(str1);
	}

}
