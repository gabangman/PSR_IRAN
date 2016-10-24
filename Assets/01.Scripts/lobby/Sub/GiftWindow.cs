using UnityEngine;
using System.Collections;

public class GiftWindow : MonoBehaviour {
	
	GameObject mGrid;
	bool isWait = false;
	void Awake(){
		var temp = transform.FindChild("ListView").FindChild("View") as Transform;
		mGrid = temp.FindChild("grid").gameObject;
		pos = temp.transform.localPosition;
		pos1 = temp.GetComponent<UIPanel>().clipRange;
	}
	
	void OnDisable(){
			for(int i = mGrid.transform.childCount; i > 0; i--){
				DestroyImmediate(mGrid.transform.GetChild(i-1).gameObject);
			}
			var child = transform.FindChild("ListView").FindChild("View") as Transform;
			child.localPosition = pos;
			child.GetComponent<UIPanel>().clipRange = pos1;
			child.GetComponent<UIDraggablePanel2>().reSetGridPannel();
			//transform.FindChild("ListView").FindChild("View").GetComponent<UIDraggablePanel2>().reSetGridPannel();
	}
	
	Vector3 pos;
	Vector4 pos1;
	public void InitializeGiftBox(){
			GetMessageFromServer();
	}
	
	public void reFreshMsginit(){
		GetMessageFromServer();
	}
	
	void GetMessageFromServer(){
		isWait = false;
		var loading = gameObject.AddComponent<LoadingGauage>() as LoadingGauage;
		loading._onFinish = ()=>{
			return isWait;
		};
		loading.StartCoroutine("LoadingProcess");

		int cnt = gameRank.instance.listGift.Count;
		onMessage = delegate(UIListItem item, int index) {
			item.Target.GetComponent<UIScrollListBase>().InitGiftBox(index,()=>{
				AcceptPostArrange();
			});
		};
		if(cnt != 0){
			AddPostItem(cnt);
		}else{
			if(mGrid.transform.childCount == 0 ){
				StartCoroutine("GetMessageList");
			}
		}

	}


	IEnumerator GetMessageList(){
		Global.isNetwork = true;
		//bConnect = false;
		string mAPI = ServerAPI.Get(90055); // "game/giftbox/"
		NetworkManager.instance.HttpConnect("Get", mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				Global.gNewMsg = thing["result"].Count;
				if(Global.gNewMsg != 0){
					gameRank.instance.InitializeGiftList(thing["result"], Global.gNewMsg);
					
				}else{

				}
			}else{
				if(status == -105){

					return;
				}
			}
			Global.isNetwork  = false;
		});
		
		while(Global.isNetwork){
			yield return null;
		}
		AddPostItem(Global.gNewMsg);
	}
	
	void AcceptPostArrange(){
		isWait = false;
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
		yield return null;
	}

	private delegate void OnMessage(UIListItem item, int index);
	private OnMessage onMessage;
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
		isWait = true;
	}

}
