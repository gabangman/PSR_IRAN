using UnityEngine;
using System.Collections;

public class AdTapjoyReward : MonoBehaviour {
	
	public UILabel[] lbtext;
	public GameObject[] objs;
	private bool bTap = false;
	private bool bCircle = false;
	private int mBalance = 0;
	
	
	void Start () {
		lbtext[0].text = KoStorage.GetKorString("70131");
		lbtext[1].text = KoStorage.GetKorString("70132");
	}
	
	public void SetTapJoyReward(int balance1){
		int balance = balance1;
		objs[1].SetActive(true);
		lbtext[2].text = string.Format("X{0}", balance);
		StartCoroutine("setTapJoyReward", balance);
	}
	
	protected IEnumerator setTapJoyReward(int balance){
		Global.isNetwork = true;
		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		mDic.Add("rewardId",8767);
		mDic.Add("type",2);
		mDic.Add("value",GV.gTapJoy);
		string mAPI = ServerAPI.Get(90068);//"user/reward"
		NetworkManager.instance.HttpFormConnect("Put",mDic, mAPI ,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				if(balance > 0){
					GV.myCoin += balance;
					GV.updateCoin = -balance;
					GameObject.Find ("Audio").SendMessage("SeasonUpSound");
				}
				if(balance < 0) Utility.LogError("Tapjoy " +  balance);
				objs[1].transform.FindChild("btnok").gameObject.SetActive(true);
				
				var lobby = GameObject.Find("LobbyUI") as GameObject;
				lobby.SendMessage("InitTopMenuReward", SendMessageOptions.DontRequireReceiver);
				UserDataManager.instance.StartCoroutine("TapJoyRewardSpend",balance);
			}
			Global.isNetwork = false;
		});
		
		while(Global.isNetwork){
			yield return null;
		}
	}
	
	public void SetCheckTapJoyReward(){
		
		SetTapJoyReward(GV.gTapJoy);
		GV.gTapJoy = 0;
	}
	
	private void SetClose(){
		objs[0].SetActive(false);
		objs[1].SetActive(false);
		NGUITools.FindInParents<SubMenuWindow>(gameObject).OnCloseClick();
	}
	
	IEnumerator LoadingCircle(){
		objs[0].SetActive(true);
		transform.parent.FindChild("Contents_menuBG").gameObject.SetActive(false);
		var _mvalue = objs[0].transform.FindChild("Circle").GetComponent<UISprite>() as UISprite;
		_mvalue.fillAmount= 0.0f;
		float delta = 0.05f;
		float mdelta = 0.0f;
		while(bCircle){
			_mvalue.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			yield return null;
		}
		objs[0].SetActive(false);
	}
	
	protected void OnOk(){
		GameObject.Find("Audio").SendMessage("CompleteSound");
		NGUITools.FindInParents<SubMenuWindow>(gameObject).OnCloseClick();
	}
	
	
	
}
