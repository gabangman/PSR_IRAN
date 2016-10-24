using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using SimpleJSON;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
[System.Serializable]
public class CrossADInfo {
	public List<CrossADDetail> listCrossAd;
	public int crossVersion;
	[System.Serializable]
	public class CrossADDetail{
		public int crossID;
		public string marketURL;
		public string appIconURL;
		public string appURL;
		public int Version;
		public int appPlatform;
		public int orderid;
	}
	
	public CrossADInfo(){
		
	}
	
	public void setInit(){
		listCrossAd = new List<CrossADDetail>();
		crossVersion = 0;
	}
	
	public void addCrossInfo(int id, int orderId, int platform, string market, string appURL, string appicon){
		CrossADDetail cD = new CrossADDetail();
		cD.crossID = id;
		cD.marketURL = market;
		cD.appURL = appURL;
		cD.appPlatform = platform;
		cD.appIconURL= appicon;
		cD.orderid = orderId;
		listCrossAd.Add(cD);
	}
	
	public string getIconUrl(int id){
		
		for(int i = 0; i < listCrossAd.Count;i++){
			if(listCrossAd[i].orderid == id){
				if(Application.isEditor){
					if(listCrossAd[i].appPlatform == 1){
						return listCrossAd[i].appIconURL;
					}
				}else if(Application.platform == RuntimePlatform.Android){
					if(listCrossAd[i].appPlatform == 1){
						return listCrossAd[i].appIconURL;
					}
				}else if(Application.platform == RuntimePlatform.IPhonePlayer){
					if(listCrossAd[i].appPlatform == 2){
						return listCrossAd[i].appIconURL;
					}
				}
			}
		}
		return string.Empty;
	}


	public int getIconIndex(int id){
		for(int i = 0; i < listCrossAd.Count;i++){
			if(listCrossAd[i].orderid == id){
				if(Application.isEditor){
					if(listCrossAd[i].appPlatform == 1){
						return listCrossAd[i].crossID;
					}
				}else if(Application.platform == RuntimePlatform.Android){
					if(listCrossAd[i].appPlatform == 1){
						return listCrossAd[i].crossID;
					}
				}else if(Application.platform == RuntimePlatform.IPhonePlayer){
					if(listCrossAd[i].appPlatform == 2){
						return listCrossAd[i].crossID;
					}
				}
			}
		}
		return 0;
	}
}


public class CrossADData{
	public static CrossADData instance;// = new myAccount();
	public CrossADInfo mCrossInfo = new CrossADInfo();
	
	public CrossADData(){
		
	}
	public void GetCrossADInfo(){
		if(!EncryptedPlayerPrefs.HasKey("crossAD")){
			instance.mCrossInfo.setInit();
			var b = new BinaryFormatter();
			var m = new MemoryStream();
			b.Serialize(m, instance.mCrossInfo);
			EncryptedPlayerPrefs.SetString("crossAD", 
			                               Convert.ToBase64String(m.GetBuffer()) );
		}
		
		var data = EncryptedPlayerPrefs.GetString("crossAD");
		if(!string.IsNullOrEmpty(data))
		{
			var b = new BinaryFormatter();
			var m = new MemoryStream(Convert.FromBase64String(data));
			instance.mCrossInfo = (CrossADInfo)b.Deserialize(m);
		}
	}
	
	public void SaveCrossInfo()
	{
		if(instance == null) return;
		var b = new BinaryFormatter();
		var m = new MemoryStream();
		b.Serialize(m, instance.mCrossInfo);
		EncryptedPlayerPrefs.SetString("crossAD", Convert.ToBase64String(m.GetBuffer()));
	}
}


public class CrossAD : MonoBehaviour {
	
	void Awake(){

		if(GV.gInfo.crossADVersion == 0) return;
		CrossADData.instance = new CrossADData();
		CrossADData.instance.GetCrossADInfo();
		
		
		if(CrossADData.instance.mCrossInfo.crossVersion != GV.gInfo.crossADVersion){
			CrossADData.instance.mCrossInfo.listCrossAd.Clear();
			string path ="https://s3-ap-northeast-1.amazonaws.com/gabangman01/CrossAds/CrossAD_"+GV.gInfo.crossADVersion.ToString()+".txt";
			StartCoroutine("GetCrossAD", path);
		}else{
		//	string url = CrossADData.instance.mCrossInfo.getIconUrl(GV.gInfo.crossADId).Trim();
		//	url = System.Uri.EscapeUriString(url);
		//	StartCoroutine("getIconTexture");
			
		}
		
		transform.GetChild(0).GetComponent<UIButtonMessage>().functionName = "OnCross";
		
	}
	
	IEnumerator GetCrossAD(string path){
		
		//	string path1 = "https://s3-ap-northeast-1.amazonaws.com/gabangman01/CrossAds/CrossAD_"+"1"+".txt";
		
		
		WWW www = new WWW(path);
		yield return www;
		if (www.error == null) 
		{ 
			//Utility.Log("WWW Ok!: " + www.text); 
			CrossADData.instance.mCrossInfo.crossVersion =GV.gInfo.crossADVersion;
		}else{ 
			Utility.Log("WWW Error: " + www.error); 
			yield break;
		}
		string data = www.text;
		www.Dispose();
		//Utility.Log(data);
		string[] stringList1, stringTable;
		stringTable = data.Split('\n');
		string[] mString = new string[stringTable.Length-1];
		for(int i = 0; i < stringTable.Length-1; i++){
			mString[i] = stringTable[i].Split(';')[1];
			stringList1 = new string[6];
			stringList1 = stringTable[i].Split(';');
			string str = stringList1[0].Trim();
			string str1 = stringList1[1].Trim();
			string str2 = stringList1[2].Trim();
			CrossADData.instance.mCrossInfo.addCrossInfo(int.Parse(str),int.Parse(str1),int.Parse(str2),stringList1[3],
			                                             stringList1[4], stringList1[5]);
		}
		CrossADData.instance.SaveCrossInfo();
	//	StartCoroutine("getIconTexture");


	}
	
	IEnumerator getIconTexture(){
		yield return new WaitForSeconds(0.5f);
		string path1 = CrossADData.instance.mCrossInfo.getIconIndex(GV.gInfo.crossADId).ToString()+"_icon";
		path1 = pathForDocumentsFile(path1);
		if (File.Exists(path1)){
			transform.GetChild(0).FindChild("Texture").GetComponent<UITexture>().mainTexture = LoadTextureFile(path1);
		}else{
			string url = CrossADData.instance.mCrossInfo.getIconUrl(GV.gInfo.crossADId).Trim();
			url = System.Uri.EscapeUriString(url);
			WWW www = null;
			www = 	new WWW( url );
			yield return www;
			if( this == null )
				yield break;
			if( www.error != null )
			{
				Utility.Log( "load failed" );
				transform.GetChild(0).FindChild("Texture").GetComponent<UITexture>().mainTexture = null;
			}
			else
			{
				Texture2D pic = www.texture;
				SaveTextureFile(path1, pic);
				transform.GetChild(0).FindChild("Texture").GetComponent<UITexture>().mainTexture = pic;
				www.Dispose();
			}
		}
		yield return null;
	}
	
	
	void OnCross(){
		GameObject.Find("LobbyUI").SendMessage("OnCross",SendMessageOptions.DontRequireReceiver);
	}
	
	
	private string pathForDocumentsFile( string fileName )
	{
		return AccountManager.instance.pathForDocumentsFile(fileName);
	}
	
	
	private void SaveTextureFile(string fileName, Texture2D SaveImage)
	{
		AccountManager.instance.SaveTextureFile(fileName,SaveImage);
	}
	
	private Texture2D LoadTextureFile(string fileName)
	{
		return AccountManager.instance.LoadTextureFile(fileName);
	}

}
