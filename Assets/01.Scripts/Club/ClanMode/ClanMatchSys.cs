using UnityEngine;
using System.Collections;

public class ClanMatchSys : MonoBehaviour {
	System.Action<bool> Callback;
//	public UISprite mapSprite;
	public UILabel lbText, lbBtn;
	void Start(){
		lbText.text = KoStorage.GetKorString("77400");
		lbBtn.text = KoStorage.GetKorString("71000");
	}
	void OnEnable(){
	//	lapCount = 1; mapCount = 1400;
	//	lap.text = lapCount.ToString() + "\n laps";
	//	mapSprite.spriteName = "1400P";
	}
	void OnClose(){
		if(Global.isNetwork) return;
		gameObject.SetActive(false);
		Callback=null;
	}

	public void OnMatching(System.Action<bool> callback){
		this.Callback = callback;
		
	}

	void OnMatchStart(){
		if(Global.isNetwork) return;
		StartCoroutine("requestMatching");
	//	Callback(true);
	}

	IEnumerator requestMatching(){
		bool bConnect = false;//club/requestClubMatching
		string mAPI = "club/requestClubMatching";///club/getClubRankingLocal
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		NetworkManager.instance.ClubBaseStringConnect("Post",mDic,mAPI,(request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				if(Callback == null) {
				//	Debug.LogError("Callback Null");
				}
				Callback(false);
			}else if(status == -421){
				if(Callback == null) {
				//	Debug.LogError("Callback Null");
				}

				Callback(true);
				//==!!Utility.LogWarning("match ~ing");	
			}else{
				if(Callback == null) {
				//	Debug.LogError("Callback Null");
				}

				Callback(true);
			}
			CClub.readyWaitTime = NetworkManager.instance.GetCurrentDeviceTime();
			Global.isNetwork = false;
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}

		OnClose();
	}


}
