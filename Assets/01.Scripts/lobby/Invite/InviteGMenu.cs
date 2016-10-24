using UnityEngine;
using System.Collections;

public class InviteGMenu : MonoBehaviour {
	
	public UILabel[] lbText;
	public Transform Grid, SNS, ListView;
	public GameObject slotPrefabs;
	public GameObject BtnObj, BoxObj;
	void Start () {
		
		lbText[0].text =KoStorage.GetKorString("72613");//
		lbText[1].text = KoStorage.GetKorString("72614");//
		lbText[2].text = KoStorage.GetKorString("72608");//
		lbText[3].text = KoStorage.GetKorString("72615");//
		
		
	}
	
	IEnumerator GetFBInvitableFr(){
		if(Grid.childCount != 0) yield break;
		if(Global.isNetwork){Utility.LogWarning ("get invite FB then Network active "); yield break;}
		Global.isNetwork = true;
		SNSManager.GetFaceBookInvitableFriends();
		while(Global.isNetwork){
			yield return null;
		}
		BtnObj.SetActive(true); BoxObj.SetActive(true);
		ListView.gameObject.SetActive(true);
		AddListItem();
	}
	
	public void Initialize(){
		if(SNSManager.isFB){
			SNS.gameObject.SetActive(false);
			if(Grid.childCount != 0 ) {
				BtnObj.SetActive(true); 
				BoxObj.SetActive(true);
				ListView.gameObject.SetActive(true);
				return;
			}
			//Utility.LogWarning("Init");
			
			StartCoroutine("GetFBInvitableFr");
		}else{
			if(SNS.gameObject.activeSelf) return;
			ListView.gameObject.SetActive(false);
			SNS.gameObject.SetActive(true);
			lbText[4].text =  KoStorage.GetKorString("72515");
			BtnObj.SetActive(false); BoxObj.SetActive(false);
		}
		
	}
	void AddListItem(){
		int cnt = SNSManager.facebookinvite.Count;	
		int count = 0;
		if(cnt < 10) count = 5;
		else {
			count  = cnt/2;
			if( cnt % 2 != 0) count++; 
		}
		UIDraggablePanel2 dragWorld = Grid.parent.GetComponent<UIDraggablePanel2>();
		dragWorld.maxScreenLine = 1;
		dragWorld.maxColLine = 0;
		dragWorld.Init(count, delegate(UIListItem item, int index) {
			item.Target.GetComponent<UIScrollListBase>().InitInviteFBFriend(index);
		} );
	}
	
	
	public void SetCheckBox(bool b){
		transform.FindChild("SelectAll").GetComponentInChildren<UICheckbox>().SetCheck(false);
	}
	
	void OnFBLogin(){
		SNS.gameObject.SetActive(false);
		GameObject.Find("LobbyUI").SendMessage("FacebookButtonActivity", 1, SendMessageOptions.DontRequireReceiver); 
		SNSManager.FaceBookLogin(()=>{
			if(Grid.childCount == 0 ){
				StartCoroutine("GetFBInvitableFr");
			}else{
				BtnObj.SetActive(true); BoxObj.SetActive(true);
				ListView.gameObject.SetActive(true);
			}
		}, 
		()=>{
			SNS.gameObject.SetActive(true);
			BtnObj.SetActive(false); BoxObj.SetActive(false);
			GameObject.Find("LobbyUI").SendMessage("FacebookButtonActivity", 0, SendMessageOptions.DontRequireReceiver); 
		});
		
		/*
		FB.Login("public_profile,email,user_friends,publish_actions", (result)=>{
			if (result.Error != null)
				Utility.Log(string.Format(" Error Response:  {0} " + result.Error));
			else if (!FB.IsLoggedIn)
			{
				SNS.gameObject.SetActive(true);
				BtnObj.SetActive(false); BoxObj.SetActive(false);
			}
			else
			{
				if(Grid.childCount == 0 ){
					StartCoroutine("GetFBInvitableFr");
				}else{
					BtnObj.SetActive(true); BoxObj.SetActive(true);
					ListView.gameObject.SetActive(true);
				}
			//	OnInviteCheck(true);
			//	Invoke("SetSelectAll", 0.25f);
			}
		}); */
	}
	
	void SetSelectAll(){
		//transform.FindChild("SelectAll").GetComponentInChildren<UICheckbox>().SetCheck(true);
		//GameObject.Find("LobbyUI").SendMessage("LobbyLoginButtonStatus");
	}
	
	void OnEnable(){
		//transform.FindChild("SelectAll").FindChild("Checkbox").GetComponent<UICheckbox>().SetCheck(true);// = true;
	}
	
	
	void OnInviteCheck(bool isCheck){
		int cnt = Grid.childCount;
		for(int i = 2; i < cnt; i++){
			Grid.GetChild(i).GetComponent<InviteFrItem>().CheckStageChange(isCheck);
		}
		
		cnt =  SNSManager.facebookinvite.Count;
		if(cnt == 0) return;
		for(int i=0; i < cnt ; i++){
			SNSManager.facebookinvite[i].isCheck = isCheck;
		}
		
	}
	
	public void SetSelectBox(bool b){
		var temp = transform.FindChild("SelectAll").FindChild("Checkbox").gameObject as GameObject;
		bool b1 = temp.GetComponent<UICheckbox>().isChecked;
		if(b1)
			temp.GetComponent<UICheckbox>().SetCheck(false);// = false;
		
		//	Utility.LogWarning("SetSelectBox " + transform.name);
	}
	
	void OnInviteClick(){
		string[] sb = null;
		string str = null;
		
		System.Collections.Generic.List<string> sbList = new System.Collections.Generic.List<string>();
		int cnt = SNSManager.facebookinvite.Count;
		int fCnt = 0;
		for(int i = 0; i < cnt ; i++){
			if(SNSManager.facebookinvite[i].isCheck){
				sbList.Add(SNSManager.facebookinvite[i].inviteToken);
				str += SNSManager.facebookinvite[i].inviteToken + ",";
				fCnt++;
			}
			
		}
		sb = sbList.ToArray();
		if(fCnt == 0){
			
			
			
			return;
		}
		
		BtnObj.SetActive(false);
		SNSManager.SendInviteMessage((result)=>{
			if (result.Error != null){
				Utility.Log(string.Format(" FB Login Error Response:  {0} " + result.Error));
			}
			if (result != null)
			{
				System.Object tempObj = Facebook.MiniJSON.Json.Deserialize(result.Text);
				System.Collections.Generic.Dictionary<string, System.Object> jsonDic = tempObj as System.Collections.Generic.Dictionary<string, System.Object>;
				bool issilhouett = (bool)jsonDic["cancelled"];
				Utility.LogWarning(issilhouett);
				BtnObj.SetActive(true);
			}
		}, sb);
		
		
		//	var pop = ObjectManager.SearchWindowPopup() as GameObject;
		//	pop.AddComponent<InviteGPopup>().InitPopUp();
		//	pop.GetComponent<InviteGPopup>().onFinishCallback(()=>{
		//		BtnObj.SetActive(false);
		//		SNSManager.SendInviteMessage((result)=>{
		//			if (result.Error != null){
		//				Utility.Log(string.Format(" FB Login Error Response:  {0} " + result.Error));
		//			}
		//			if (result != null)
		//			{
		//				System.Object tempObj = Facebook.MiniJSON.Json.Deserialize(result.Text);
		//				System.Collections.Generic.Dictionary<string, System.Object> jsonDic = tempObj as System.Collections.Generic.Dictionary<string, System.Object>;
		//				bool issilhouett = (bool)jsonDic["cancelled"];
		//				Utility.LogWarning(issilhouett);
		//			}
		//		}, sb);
		//	});
		
	}
	
}
