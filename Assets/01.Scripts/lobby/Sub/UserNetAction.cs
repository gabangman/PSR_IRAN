using UnityEngine;
using System.Collections;

public class UserNetAction : MonoBehaviour {

	GameObject mGrid;
	bool isWait = false;
	void Awake(){
		var temp = transform.FindChild("ListView").FindChild("View") as Transform;
		mGrid = temp.FindChild("grid").gameObject;
		if(string.Equals(gameObject.name,"Post")){
			pos = temp.transform.localPosition;
			pos1 = temp.GetComponent<UIPanel>().clipRange;
		}
	}

	void OnDisable(){
		if(string.Equals(gameObject.name,"Post")){
			//Utility.Log(mGrid.transform.childCount);
			for(int i = mGrid.transform.childCount; i > 0; i--){
				//Utility.Log(mGrid.transform.GetChild(i-1).name);
				DestroyImmediate(mGrid.transform.GetChild(i-1).gameObject);
			}
			var child = transform.FindChild("ListView").FindChild("View") as Transform;
			child.localPosition = pos;
			child.GetComponent<UIPanel>().clipRange = pos1;
			child.GetComponent<UIDraggablePanel2>().reSetGridPannel();
			//transform.FindChild("ListView").FindChild("View").GetComponent<UIDraggablePanel2>().reSetGridPannel();
		}
	}

	Vector3 pos;
	Vector4 pos1;
	public void Initialize(string name){
		if(string.Equals(name,"Invite")){
			GetInviteFriendFromServer();
		}else{
		
			GetMessageFromServer();

		}
	}

	public void reFreshMsginit(){
		
		GetMessageFromServer();
	}

	void GetMessageFromServer(){
		isWait = false;
		Global.isNetwork = true;
	//	string strUrl = ServerStringKeys.API.getMsgInfo;
	//	ProtocolManager.instance.ConnectServer(strUrl,GetMessage);
		var loading = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
		loading._onFinish = ()=>{
			return isWait;
		};
		loading.StartCoroutine("LoadingProcess");
	}

	void AcceptPostArrange(){
		isWait = false;
		//Global.isNetwork = true;
		var loading = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
		loading._onFinish = ()=>{
			return isWait;
		};
		loading.StartCoroutine("LoadingProcess");
		Vector3 pos = mGrid.transform.FindChild("last rect").localPosition;
		if(pos.y > 440)
			pos.y += 110;
		mGrid.transform.FindChild("last rect").localPosition = pos;
		StartCoroutine("ReArrangePost");
		//Invoke("ReArrangePost",0.2f);
	}

	IEnumerator ReArrangePost(){
		int cnt = mGrid.transform.childCount;
		for(int i = 0; i < cnt;i++){
			var temp =  mGrid.transform.GetChild(i).gameObject;
			if(temp.activeSelf){
				temp.GetComponent<PostMessageItem>().reUpdate(()=>{AcceptPostArrange();});
				yield return new WaitForSeconds(0.1f);
			}
		}

		isWait = true;
		Global.isNetwork = false;
		yield return null;
	}

	void GetMessage(System.Uri uri){
		Global.isNetwork = false;
		isWait = true;
		/*int nret = ProtocolManager.instance.GetIntUriQuery(uri,"nRet");
		if(nret == 1){
			int cnt = ProtocolManager.instance.GetIntUriQuery(uri,"nCnt");
			if(cnt != 0){
				string str = ProtocolManager.instance.GetUriStringQuery(uri,"List");
				gameRank.instance.InitialMsg(str);
				onMessage = delegate(UIListItem item, int index) {
					item.Target.GetComponent<UIScrollListBase>().InitPostBox(index,()=>{
						AcceptPostArrange();
					});
				};
					AddPostItem(cnt);
			}else{
				onMessage = delegate(UIListItem item, int index) {
					item.Target.GetComponent<UIScrollListBase>().InitPostBox(index,()=>{
						AcceptPostArrange();
					});
				};
					AddPostItem(0);
			}
		
		}else{
				Utility.Log("GetMessage " + nret);
			}

		isWait = true;
		Global.isNetwork = false;*/
	}

	//bool isKakaoFriend =false;
	void GetInviteFriendFromServer(){
		Global.isNetwork = true;
		isWait = false;
		StartCoroutine("InviteFriends");
		var loading = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
		loading._onFinish = ()=>{
			return isWait;
		};
		loading.StartCoroutine("LoadingProcess");
	}
	IEnumerator InviteFriends(){
		yield return new WaitForSeconds(1.0f);
		Global.isNetwork = false;
		isWait = true;
		onMessage = delegate(UIListItem item, int index) {
			item.Target.GetComponent<UIScrollListBase>().InitInviteFriend(index);
		};
	//	if(KakaoFriends.Instance.wFriends.Count != 0){
	//		AddListItem(KakaoFriends.Instance.wFriends.Count );
	//		mGrid.GetComponent<UIGrid>().Reposition();
	//	}else{
			AddListItem(0);
	//	}
	}
	void GetInviteFriendListServer(System.Uri uri){

		Global.isNetwork = false;
		isWait = true;
		/*
		int nret = ProtocolManager.instance.GetIntUriQuery(uri,"nRet");
		if(nret == 1){
			int cnt = ProtocolManager.instance.GetIntUriQuery(uri,"nCnt");
			if(cnt != 0){
				//string str = ProtocolManager.instance.GetUriStringQuery(uri,"List");
				//gameRank.instance.InitailInvite(str);
				//Utility.LogWarning("getInviteInfo"+ gameRank.instance.listInvite.Count);
				onMessage = delegate(UIListItem item, int index) {
					item.Target.GetComponent<UIScrollListBase>().InitInviteFriend(index);
				};
				//Utility.LogWarning("nret " + cnt);
				AddListItem(cnt);
				mGrid.GetComponent<UIGrid>().Reposition();
			}
		}
		Global.isNetwork = false;
		isWait = true;*/
		//gameRank.instance.InitailInvite("d");
		//AddListItem(2);
	}
	
	public delegate void OnMessage(UIListItem item, int index);
	OnMessage onMessage;
	void AddPostItem(int count){
		if(count < 4) count = 4;
		UIDraggablePanel2 dragWorld = mGrid.transform.parent.GetComponent<UIDraggablePanel2>();
		dragWorld.maxScreenLine = 1;
		dragWorld.maxColLine = 0;
		if(mGrid.transform.childCount == 0 ){
			dragWorld.Init(count, delegate(UIListItem item, int index) {
				onMessage(item, index);
			} ); 
		}
	}

	void AddListItem(int count){
		if(count == 0){
			count = 2;
		}
		count = count/2;// + count%2;
		UIDraggablePanel2 dragWorld = mGrid.transform.parent.GetComponent<UIDraggablePanel2>();
		dragWorld.maxScreenLine = 1;
		dragWorld.maxColLine = 0;
		if(mGrid.transform.childCount == 0 ){
			dragWorld.Init(count, delegate(UIListItem item, int index) {
				onMessage(item, index);
			} ); //OnInviteFriend OnAccept
		}
		//isWait = true;
	}
}
