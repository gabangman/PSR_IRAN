using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Reflection;

public class CrossADSub : MonoBehaviour {

	public GameObject readyBG, View, Grid;
	public GameObject slotPrefab;

	public void SetInit(){
	
		if(Application.isEditor){
			addItemAndroid();
		}else if(Application.platform == RuntimePlatform.Android){
			addItemAndroid();
		}else if(Application.platform == RuntimePlatform.IPhonePlayer){
			addItemIphone();
		}
	}



	void addItemAndroid(){
		int cnt = Grid.transform.childCount;
		if(cnt == 0){
			int mCnt = CrossADData.instance.mCrossInfo.listCrossAd.Count;
			int textCnt = 0;
			for(int i =0;  i < mCnt; i++){
				if(CrossADData.instance.mCrossInfo.listCrossAd[i].appPlatform == 1){
					var obj = NGUITools.AddChild(Grid, slotPrefab) as GameObject;
					obj.name = i.ToString();
					obj.transform.GetChild(2).GetComponent<UIButtonMessage>().target= gameObject;
					obj.transform.GetChild(2).GetComponent<UIButtonMessage>().functionName= "OnGoURL";
					obj.transform.GetChild(0).gameObject.SetActive(false);
					obj.transform.GetChild(2).gameObject.SetActive(false);
					textCnt++;
				}
			}
			Grid.GetComponent<UIGrid>().Reposition();
			StartCoroutine("getTexture", textCnt );
		}else{
			Grid.GetComponent<UIGrid>().Reposition();
		}
	}

	void addItemIphone(){
		int cnt = Grid.transform.childCount;
		if(cnt == 0){
			int mCnt = CrossADData.instance.mCrossInfo.listCrossAd.Count;
			int textCnt = 0;
			for(int i =0;  i < mCnt; i++){
				if(CrossADData.instance.mCrossInfo.listCrossAd[i].appPlatform == 2){
					var obj = NGUITools.AddChild(Grid, slotPrefab) as GameObject;
					obj.name = i.ToString();
					obj.transform.GetChild(2).GetComponent<UIButtonMessage>().target= gameObject;
					obj.transform.GetChild(2).GetComponent<UIButtonMessage>().functionName= "OnGoURL";
					obj.transform.GetChild(0).gameObject.SetActive(false);
					obj.transform.GetChild(2).gameObject.SetActive(false);
					textCnt++;
				}
			}
			Grid.GetComponent<UIGrid>().Reposition();
			StartCoroutine("getTexture", textCnt );
		}else{
			Grid.GetComponent<UIGrid>().Reposition();
		}
	}


	public void OnGoURL(GameObject obj){
		string str = obj.transform.parent.name;
		int idx = int.Parse(str);
		str = CrossADData.instance.mCrossInfo.listCrossAd[idx].marketURL;
		Application.OpenURL(str);
	}

	bool isWaiting = false;
	IEnumerator Loading(){
		readyBG.SetActive(true);
		isWaiting = true;
		var val = readyBG.GetComponent<UISprite>() as UISprite;
		//Utility.Log(val.name);
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

	void StopLoading(){
		isWaiting = false;
		readyBG.SetActive(false);
	}
	IEnumerator getTexture(int mCnt){


		Texture2D[] picTexture = new Texture2D[mCnt];
		string url =null; string pathName = null;
		WWW www = null;
		StartCoroutine("Loading");
		for(int i = 0; i < mCnt; i++){
			int index =int.Parse(Grid.transform.GetChild(i).name);
			pathName = CrossADData.instance.mCrossInfo.listCrossAd[index].crossID.ToString()+"_app";
			pathName = pathForDocumentsFile(pathName);
			if (File.Exists(pathName)){
				picTexture[i] = LoadTextureFile(pathName);
			}else{
				url = CrossADData.instance.mCrossInfo.listCrossAd[index].appURL;
				www = 	new WWW( url );
				
				yield return www;
				if( this == null )
					yield break;
				if( www.error != null )
				{
					//Utility.Log( "load failed" );
				}
				else
				{
					Texture2D pic = www.texture;
					picTexture[i] = pic;
					SaveTextureFile(pathName,pic);
					www.Dispose();
				}
			}
		}
		StopLoading();
		for(int i = 0; i < mCnt; i++){
			Grid.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
			Grid.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
			Grid.transform.GetChild(i).GetChild(0).GetComponent<UITexture>().mainTexture = picTexture[i];
			if(i == 0){
				Grid.transform.GetChild(i).FindChild("Limit_L").gameObject.SetActive(true);
			}else if(i == (mCnt-1)){
				Grid.transform.GetChild(i).FindChild("Limit_R").gameObject.SetActive(true);
			}
		}

		yield return null;
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
