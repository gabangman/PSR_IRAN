using UnityEngine;
using System.Collections;

public class JiverMain : MonoBehaviour {
	void Awake(){
		
		if(!Application.isEditor){
			string channelUrl =  "gabangman.PitRacingpubilcCh_01";
			if(!EncryptedPlayerPrefs.HasKey("getURL")){
				channelUrl = "gabangman.PitRacingpubilcCh_01";
				EncryptedPlayerPrefs.SetString("getURL",channelUrl);
			}else{
				channelUrl =	EncryptedPlayerPrefs.GetString("getURL");
			}
		}
	}
	
	void Start () {
		
		
	//	StartCoroutine("JiverChannelJoin");
		
	}
	
	IEnumerator JiverChannelJoin(){
		string appId = "348247C5-C147-401D-84EB-33E5F4CAD320";
		string userId = SystemInfo.deviceUniqueIdentifier;
		string userName = "Unity-" + userId.Substring (0, 5);
		string channelUrl = "jia_test.Unity3d";
		if(!Application.isEditor){
		
			channelUrl =	EncryptedPlayerPrefs.GetString("getURL");
		}
		SendBird.Init (appId);
		yield return new WaitForSeconds(0.5f);
		SendBird.Login (userId, GV.UserNick);
		yield return new WaitForSeconds(0.5f);
		SendBird.Join (channelUrl);
		yield return new WaitForSeconds(0.5f);
		SendBird.Connect(50);
	}
}
