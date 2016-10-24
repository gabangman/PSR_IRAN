using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
static public class AssetBundleManager {

static private Dictionary<string, AssetbundleRef> dictAssetBundleRefs;
static AssetBundleManager(){
	dictAssetBundleRefs = new Dictionary<string, AssetbundleRef>();
}

private class AssetbundleRef{
	public AssetBundle asstbundle = null;
	public int version;
	public string url;
	public AssetbundleRef(string strurlIn, int intversionIn){
		url  = strurlIn;
		version = intversionIn;
	}

};

public static AssetBundle getAssetBundle(string url, int version){
	string keyname = url+version.ToString();
	AssetbundleRef abRef;
	if(dictAssetBundleRefs.TryGetValue(keyname, out abRef))
		return abRef.asstbundle;
	else
		return null;
}

public static IEnumerator downloadAssetBundle(string url, int version){
	string keyname = url+version.ToString();
	if(dictAssetBundleRefs.ContainsKey(keyname)){
			yield return null;
	}else{
		while(!Caching.ready){
			
				yield return null;
		}
		using(WWW www = WWW.LoadFromCacheOrDownload(url, version)){
			while(!www.isDone){
				//AccountManager.instance.progressbarvalue = www.progress;	
					yield return null;
			}
			if(www.error != null){
				throw new Exception("www DownLoad :" + www.error);
			}
			AssetbundleRef abRef = new AssetbundleRef(url, version);
			abRef.asstbundle = www.assetBundle;
			dictAssetBundleRefs.Add(keyname, abRef);
			www.Dispose();
			//www = null;
			//delete download data cashing
			//Caching.CleanCache();
		//	Caching.maximumAvailableDiskSpace = 104857600; // 100메가 -> 바이트 
		}
	}
	
}

public static void unLoad(string url, int version, bool allObject){
	string keyname = url + version.ToString();
	AssetbundleRef abRef;
	if(dictAssetBundleRefs.TryGetValue(keyname, out abRef)){
		abRef.asstbundle.Unload(allObject);
		abRef.asstbundle = null;
		dictAssetBundleRefs.Remove(keyname);
	}
}

}

