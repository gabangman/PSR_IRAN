using UnityEngine;
using System.Collections;

public class InviteFriendItem : MonoBehaviour {

	//int myId = -1;
	void OnInviteFriend(GameObject obj){
		int inviteID = 0;
		int.TryParse(obj.name, out inviteID);
		//var target = gameObject.AddComponent<makePopup>().SearchWindow();//("InvitePopUp", inviteID, gameObject);
		var target = ObjectManager.SearchWindowPopup() as GameObject;
		target.AddComponent<InvitePopUp>().InitPopUp(inviteID);

		target.GetComponent<InvitePopUp>().onInviteFinish =delegate {
			obj.transform.parent.gameObject.SetActive(false);
			var t = obj.transform.parent.parent.FindChild("BtnWait") as Transform;
			t.gameObject.SetActive(true);
			/*if(KakaoFriends.Instance.wFriends[inviteID].isBlock == 1){
				t.FindChild("icon_Wait").gameObject.SetActive(false);
				t.FindChild("icon_Off").gameObject.SetActive(true);
				return;
			}
			if(KakaoFriends.Instance.wFriends[inviteID].isWaiting == 1){
				t.FindChild("icon_Wait").gameObject.SetActive(true);
				t.FindChild("icon_Off").gameObject.SetActive(false);
			}*/
		};
			
	}


	public void InitInviteBox(int idx){

		return;	
		ChangeFriend(idx);
	}




	public void CheckStageChange(bool b){
		var tr = transform.GetChild(0) as Transform;
		var tr1 = transform.GetChild(1) as Transform;

		tr.FindChild("Select").GetComponentInChildren<UICheckbox>().SetCheck(b);
		tr1.FindChild("Select").GetComponentInChildren<UICheckbox>().SetCheck(b);
	}

	void OnCheckOdd(bool b){
	//	Utility.LogWarning("GV " + GV.mInviteFlag);
	//	if(GV.mInviteFlag){
			NGUITools.FindInParents<InviteGMenu>(gameObject).SetCheckBox(b);
	//	}


	}

	void OnCheckEven(bool b){
	//	if(GV.mInviteFlag){
			NGUITools.FindInParents<InviteGMenu>(gameObject).SetCheckBox(b);
	//	}
	}
	
	public bool CheckStateOdd(){
		var tr = transform.GetChild(0) as Transform;
		return tr.FindChild("Select").GetComponentInChildren<UICheckbox>().GetCheckState();
	}

	public bool CheckStateEven(){
		var tr1 = transform.GetChild(1) as Transform;
		return tr1.FindChild("Select").GetComponentInChildren<UICheckbox>().GetCheckState();
	}



	void ChangeFriend(int idx){
		return;
		/*var odd =	transform.FindChild("odd") as Transform;
		int count = idx*2;
		KakaoFriends.wholeFriend _fr = KakaoFriends.Instance.wFriends[count];
		//gameRank.InviteFriend _fr = gameRank.instance.listInvite[count];

		string oddString = _fr.nickname;
		var oddTitle = odd.FindChild("title") as Transform;
		oddTitle.GetComponentInChildren<UILabel>().text = oddString;
		var b_accept = odd.FindChild("BtnAccept") as Transform;
		var b_wait =  odd.FindChild("BtnWait") as Transform;
		if(_fr.messageBlocked){
			b_accept.gameObject.SetActive(false);
			b_wait.gameObject.SetActive(true);
			b_wait.FindChild("icon_Wait").gameObject.SetActive(false);
			b_wait.FindChild("icon_Off").gameObject.SetActive(true);
			KakaoFriends.Instance.wFriends[count].isBlock = 1;
		}else{

			if(_fr.isWaiting == 1){
				b_accept.gameObject.SetActive(false);
				b_wait.gameObject.SetActive(true);
				b_wait.FindChild("icon_Wait").gameObject.SetActive(true);
				b_wait.FindChild("icon_Off").gameObject.SetActive(false);
			}else if(_fr.isBlock == 1){
				b_accept.gameObject.SetActive(false);
				b_wait.gameObject.SetActive(true);
				b_wait.FindChild("icon_Wait").gameObject.SetActive(false);
				b_wait.FindChild("icon_Off").gameObject.SetActive(true);
			}else{
				b_accept.gameObject.SetActive(true);
				b_wait.gameObject.SetActive(false);
				var t = b_accept.GetComponentInChildren<UIButtonMessage>() as UIButtonMessage;
				t.transform.gameObject.name = count.ToString();
			
			}
		}

		var even =  transform.FindChild("even") as Transform;
		count = idx*2+1;
		even.gameObject.SetActive(true);
		_fr = KakaoFriends.Instance.wFriends[count];
		//_fr = gameRank.instance.listInvite[count];
		oddString = _fr.nickname;
		oddTitle = even.FindChild("title");
		oddTitle.GetComponentInChildren<UILabel>().text = oddString;
		b_accept = even.FindChild("BtnAccept") as Transform;
		b_wait =  even.FindChild("BtnWait") as Transform;
		if(_fr.messageBlocked){
			b_accept.gameObject.SetActive(false);
			b_wait.gameObject.SetActive(true);
			b_wait.FindChild("icon_Wait").gameObject.SetActive(false);
			b_wait.FindChild("icon_Off").gameObject.SetActive(true);
			KakaoFriends.Instance.wFriends[count].isBlock = 1;
		}else{
			if(_fr.isWaiting == 1){
				b_accept.gameObject.SetActive(false);
				b_wait.gameObject.SetActive(true);
				b_wait.FindChild("icon_Wait").gameObject.SetActive(true);
				b_wait.FindChild("icon_Off").gameObject.SetActive(false);
			}else if(_fr.isBlock == 1){
				b_accept.gameObject.SetActive(false);
				b_wait.gameObject.SetActive(true);
				b_wait.FindChild("icon_Wait").gameObject.SetActive(false);
				b_wait.FindChild("icon_Off").gameObject.SetActive(true);
			}else{
				b_accept.gameObject.SetActive(true);
				b_wait.gameObject.SetActive(false);
				var t = b_accept.GetComponentInChildren<UIButtonMessage>() as UIButtonMessage;
				t.transform.gameObject.name = count.ToString();
			}
		}
		StopCoroutine("PicLoad");
		Texture2D tx = UserDataManager.instance.GetTexture(2*idx);
		bool isTexture = false;
		if(tx == null){
			setTextureLoad();
			StartCoroutine("PicLoad", count);
			isTexture = true;
		}else{
			setTextureLoad(idx, "odd");
		}
		tx = UserDataManager.instance.GetTexture(2*idx+1);
		if(tx == null){
			setTextureLoad();
			if(isTexture) return;
			StartCoroutine("PicLoad", count);
		}else{
			setTextureLoad(idx, "even");
		}*/
	}

	
	IEnumerator PicLoad(int idx){
		yield return StartCoroutine("PicLoad1",idx);
		idx--;
		yield return StartCoroutine("PicLoad2",idx);
	}
	
	IEnumerator PicLoad1(int idx){
		string url = null;
		WWW www = new WWW( url );
		
		yield return www;
		
		if( this == null )
			yield break;
		
		if( www.error != null )
		{
			//Utility.Log( "load failed" );
		}
		else
		{
			oddTex = www.texture;
			//UserDataManager.instance.SaveSetTexture(idx, oddTex);
			SetTextureOdd(idx);
			www.Dispose();
		}
	}

	Texture2D oddTex, evenTex;
	IEnumerator PicLoad2(int idx){
		string url = null;

		//if(!url.Contains("http")){
			
	//		yield break;
	//	}
		WWW www = new WWW( url );
		
		yield return www;
		
		if( this == null )
			yield break;
		
		if( www.error != null )
		{
			//Utility.Log( "load failed" );
		}
		else
		{
			evenTex = www.texture;
			www.Dispose();
			//UserDataManager.instance.SaveSetTexture(idx, evenTex);
			SetTextureEven(idx);
			//isTexLoad = true;
		}
	}//초대거부
	
	void SetTextureEven(int idx){
		var even =  transform.FindChild("even") as Transform;
	//	evenTex = UserDataManager.instance.GetTexture(idx);
		if(evenTex == null) evenTex = (Texture2D)Global.gDefaultIcon;
		even.FindChild("kakaopic").GetComponent<UITexture>().mainTexture = evenTex;
	}
	
	void SetTextureOdd(int idx){
		var even =  transform.FindChild("odd") as Transform;
		//oddTex = UserDataManager.instance.GetTexture(idx);
		if(oddTex == null) oddTex = (Texture2D)Global.gDefaultIcon;
		even.FindChild("kakaopic").GetComponent<UITexture>().mainTexture = oddTex;
	}
	void setTextureLoad(int idx, string str){
		Texture2D tempTex = null;
		if(str.Equals("even")){
			var even =  transform.FindChild("even") as Transform;
			// tempTex = UserDataManager.instance.GetTexture( idx*2);
			if(tempTex == null) tempTex = (Texture2D)Global.gDefaultIcon;
			even.FindChild("kakaopic").GetComponent<UITexture>().mainTexture = tempTex;
		}else{
			var odd =  transform.FindChild("odd") as Transform;
			//tempTex = UserDataManager.instance.GetTexture( idx*2+1);
			if(tempTex == null) tempTex = (Texture2D)Global.gDefaultIcon;
			odd.FindChild("kakaopic").GetComponent<UITexture>().mainTexture = tempTex;
		}
	}
	void setTextureLoad(){
		var even =  transform.FindChild("even") as Transform;
		even.FindChild("kakaopic").GetComponent<UITexture>().mainTexture = Global.gDefaultIcon;
		even =  transform.FindChild("odd") as Transform;
		even.FindChild("kakaopic").GetComponent<UITexture>().mainTexture = Global.gDefaultIcon;
	}
}
