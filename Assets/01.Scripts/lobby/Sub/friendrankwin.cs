using UnityEngine;
using System.Collections;

public class friendrankwin : MonoBehaviour {

	public GameObject readyBG,SNS;
	public UILabel[] lbLables;
	public GameObject View, Grid;
	private GameObject back;

	void Awake(){
		back = View.transform.parent.parent.FindChild("Window_F").GetChild(0).gameObject;
	
	}
	void Start () {
		lbLables[0].text =  KoStorage.GetKorString("72800"); //friends
		lbLables[1].text =  KoStorage.GetKorString("72515");
	}
	bool isWaiting = false;

	IEnumerator loadingStart(){
		readyBG.SetActive(true);
		isWaiting = true;
		var val = readyBG.transform.GetChild(0).GetComponent<UISprite>() as UISprite;
		float delta = 0.05f;
		float mdelta = 0.0f;
		while(isWaiting){
			val.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			yield return null;
		}
	}

	bool isLogin = false;
	void OnFBLogin(){
		if(Global.isNetwork) return;
		Global.isNetwork =false;
		if(isLogin) return;
		isLogin = true;
		SNS.SetActive(false);
		int count = gameRank.instance.listFFR.Count;
		StartCoroutine("loadingStart");
		SNSManager.FaceBookLogin(
		()=>{
			isLogin = false;
		},
		()=>{
			isLogin = false;
			StopRankList();
			Global.isNetwork =false;
			SNS.SetActive(true);
			View.SetActive(false);
			gameRank.instance.listFFR.Clear();
		}
		);
	}

	public void addSNSFriend(){
	
		int count = gameRank.instance.listFFR.Count;
		if(count == 0){
			StartCoroutine("loadingStart");
			AccountManager.instance.StartCoroutine("FaceBookLoginResult");

		}else{
			StartCoroutine("loadingStart");
			AddFBItem();
		}
	}


	public void FBLoginComplete(){
		AddFBItem();
	}


	void AddFBItem(){
		View.SetActive(true);
		UIDraggablePanel2 dragWorld = View.GetComponent<UIDraggablePanel2>();
		dragWorld.maxScreenLine = 2;
		dragWorld.maxColLine = -1;
		int count = 0;
		if(Grid.transform.childCount == 0 ){
			count = gameRank.instance.listFFR.Count;
			if(count <= 2) count = 5;
			dragWorld.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().FBFriendRank(index);
			});
		}else{
			for(int i=0; i <Grid.transform.childCount;i++){
				var obj = Grid.transform.GetChild(i).gameObject as GameObject;
				if(obj.activeSelf){
					obj.GetComponent<FBRankItem>().ChangeFBRank();
				}
			}
		}

		isWaiting =false;
		readyBG.SetActive(false);
		Global.isNetwork = false;
		back.SetActive(true);
	}



	public void InitSet(){
		readyBG.SetActive(false);
		isWaiting = false;
		if(FB.IsLoggedIn){
			SNS.SetActive(false);
			addSNSFriend();
		}else{
			SNS.SetActive(true);
			View.SetActive(false);
			back.SetActive(false);
		}
	//	StartCoroutine("loadingStart");
	}

	public void StopRankList(){
		isWaiting = false;
		readyBG.SetActive(false);
	}
}
