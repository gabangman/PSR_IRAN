using UnityEngine;
using System.Collections;
using System.Text;
using System;
[System.Serializable]
public class CTestList{
	public int carid;
	public int classid;
	public int idx;
}


public class DontDestroy : MonoBehaviour {
	public TextAsset serverAPI;
	public int ClanTest;
	public string DeviceIds = string.Empty;
	public int champID;
	void Awake(){
		DontDestroyOnLoad(gameObject);
		CClub.ClubTest = ClanTest;
		Global.isRaceTest = false;
		EncryptedPlayerPrefs.keys=new string[5];
		EncryptedPlayerPrefs.keys[0]="34Wrudre";
		EncryptedPlayerPrefs.keys[1]="SP9DupHa";
		EncryptedPlayerPrefs.keys[2]="frA5rAS4";
		EncryptedPlayerPrefs.keys[3]="tHat3epr";
		EncryptedPlayerPrefs.keys[4]="jaw4eDAs" ;
		//3fdcd3825a9567dc//2720ed88bcb6af14
		//EncryptedPlayerPrefs.DeleteAll();
		if(Application.isEditor){
			if(champID == 0){
				CClub.champID = 0;
			}else{
				CClub.champID = champID;
			}
		}



	}

	void DisableGame(string param){
		//GV.gRootPhone = true;
	}

	void OnPermissionResult(){
		UserDataManager.instance.onPermissionExit();
	}

	void OnRacePause(){

	}

	void OnPermissonCancle(){
	
	}

	void OnPermissonOK(){
	
	}

	void Start () {
		if(string.IsNullOrEmpty(DeviceIds) == true){
			if(Application.isEditor) Global.gDeivceID = "admin216";
		}else{
			if(Application.isEditor) Global.gDeivceID = DeviceIds;
		}

		GoogleAnalyticsV4.instance.StartSession();
		GoogleAnalyticsV4.instance.DispatchHits();
		GoogleAnalyticsV4.instance.LogScreen(new AppViewHitBuilder().SetScreenName("Splash"));
		transform.FindChild("FBManager").gameObject.SendMessage("FBInit");


		return;
	}

	void GroupDestroy(){
		Global.Init();
		Global.tutorialracesetting();

		var obj = GameObject.Find("SceneManager") as GameObject;
		DestroyImmediate(obj);
		obj = GameObject.Find("UserDataManager");
		DestroyImmediate(obj);
		obj = GameObject.Find("ProtocolManager");
		DestroyImmediate(obj);
		obj = GameObject.Find("NetworkManager");
		DestroyImmediate(obj);
		SetCountryCode("USA");
		Destroy(gameObject);
		System.GC.Collect();
	}

	void setVersCode(string param){
		Utility.LogWarning("setVersCode " + param);
	}

	void setVersionName(string param){
		Global.gVersion = param;
		Utility.LogWarning("Global.gVersion  " + param);
	}

	public void SetDeviceID2(string param){
		//userID
		if(EncryptedPlayerPrefs.HasKey("DeviceID")){
			Global.gDeivceID = EncryptedPlayerPrefs.GetString("DeviceID");
			Global.gDeivceID = param.Trim();
			//return;
		}else{
			Global.gDeivceID = param.Trim();
			EncryptedPlayerPrefs.SetString("DeviceID",Global.gDeivceID);
		}
		//Utility.LogWarning("ID  1" + Global.gDeivceID);
		//Global.gDeivceID = "admin216";
	}

	public void SetDeviceID(string param){
		//DeviceID
		if(EncryptedPlayerPrefs.HasKey("DeviceID")){
			Global.gDeivceID = EncryptedPlayerPrefs.GetString("DeviceID");
			Global.gDeivceID = param.Trim();
			//return;
		}else{
			Global.gDeivceID = param.Trim();
			EncryptedPlayerPrefs.SetString("DeviceID",Global.gDeivceID);
		}
		Utility.LogWarning("ID 2 " + param);
	}

	public void SetDeviceModel(string param){
		Global.gDeviceModel = param.Trim();
	}
	
	public void SetOSVersion(string param){
		Global.gOsVersion = param.Trim();
	}



	public void SetPushID(string param){
		if(string.IsNullOrEmpty(param) == true){
			/*if(EncryptedPlayerPrefs.HasKey("GCM_PUSHID")){
				Global.gPushID = EncryptedPlayerPrefs.GetString("GCM_PUSHID");
			}else{
			}
		}else{
			if(EncryptedPlayerPrefs.HasKey("GCM_PUSHID")){
				Global.gPushID = EncryptedPlayerPrefs.GetString("GCM_PUSHID");
			}else{
				Global.gPushID = param.Trim();
				EncryptedPlayerPrefs.SetString("GCM_PUSHID",Global.gPushID);
			}*/
		}
		Global.gPushID = param.Trim();
		Utility.LogWarning("PUSH " + Global.gPushID);
	}

	public void SetCountryCode(string param){
		if(string.IsNullOrEmpty(GV.gNationName)== true){
			GV.gNationName = param.Trim().ToString();
		}else{
		
		}

		if(EncryptedPlayerPrefs.HasKey("CountryCode")){
			Global.gCountryCode = EncryptedPlayerPrefs.GetString("CountryCode");
		//	return;
		}else{
			Global.gCountryCode = param.Trim().ToString();
			EncryptedPlayerPrefs.SetString("CountryCode",Global.gCountryCode);
		}
		Utility.LogWarning("국가 코드 " + Global.gCountryCode);
		switch(Global.gCountryCode){
		case "KOR":
			GV.gNational = "Nflag_korea";break;
		case "JPN":
			GV.gNational = "Nflag_japan";break;
		case "CHN":
			GV.gNational = "Nflag_china";break;
		case "FRA":
			GV.gNational = "Nflag_france";break;
		case "RUS":
			GV.gNational = "Nflag_rusia";break;
		case "USA":
			GV.gNational = "Nflag_eng";break;
		case "GBR":
			GV.gNational = "Nflag_eng";break;
		case "DEU":
			GV.gNational = "Nflag_germany";break;
		case "ITA":
			GV.gNational = "Nflag_italy";break;
		case "ESP":
			GV.gNational = "Nflag_spain";break;
		case "PRT":
			GV.gNational = "Nflag_portugal";break;
		case "IDN":
			GV.gNational = "Nflag_indonesia";break;
		case "MYS":
			GV.gNational = "Nflag_eng";break;
		default:
			GV.gNational = "Nflag_eng"; break;
			break;
		}
	}

	void OnApplicationPause(bool pauseStatus){
		
	
	}

	public void SetPermission(string param){
		Global.bPermission = true;
	}






}
