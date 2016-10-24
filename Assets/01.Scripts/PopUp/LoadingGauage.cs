using UnityEngine;
using System.Collections;

public class LoadingGauage : MonoBehaviour {
	bool isWait = false;
	public delegate bool OnLoadingFinish();
	public OnLoadingFinish _onFinish;
	public GameObject Parent;

	void Awake(){
		Parent = GameObject.FindGameObjectWithTag("BottomAnchor");
	}
	
	public IEnumerator LoadingProcess(){
		isWait = false;
		Global.isNetwork = true;
		var temp = ObjectManager.CreatePrefabs("Window", "LoadingBar") as GameObject;
		ObjectManager.ChangeObjectParent(temp, Parent.transform);
		ObjectManager.ChangeObjectPosition(temp, new Vector3(0,0,-1400), Vector3.one, Vector3.zero);
		var _mvalue = temp.transform.FindChild("icon").GetComponent<UISprite>() as UISprite;
		_mvalue.fillAmount= 0.0f;
		temp.transform.GetChild(1).gameObject.SetActive(true);
		float delta = 0.05f;
		float mdelta = 0.0f;
		while(true){
			_mvalue.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			isWait = _onFinish();
			if(isWait){
				Global.isNetwork = false;
				DestroyImmediate(temp);
				Destroy(this);
				break;
			}
			yield return null;
		}
	}
	
	public IEnumerator raceStartLoading(){
		isBuyWait = true;
		var temp = ObjectManager.CreatePrefabs("Window", "LoadingBar") as GameObject;
		ObjectManager.ChangeObjectParent(temp, Parent.transform);
		ObjectManager.ChangeObjectPosition(temp, new Vector3(0,0,-1400), Vector3.one, Vector3.zero);
		var _mvalue = temp.transform.FindChild("icon").GetComponent<UISprite>() as UISprite;
		temp.transform.GetChild(1).gameObject.SetActive(false);
		_mvalue.fillAmount= 0.0f;
		float delta = 0.05f;
		float mdelta = 0.0f;
		
		while(true){
			if(!isBuyWait){
				DestroyImmediate(temp);
				Destroy(this);
				break;
			}
			_mvalue.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			yield return null;
		}
	}
	public IEnumerator StartRaceLoading(bool b){
		isBuyWait = true;
		var temp = ObjectManager.CreatePrefabs("Window", "LoadingBar") as GameObject;
		ObjectManager.ChangeObjectParent(temp, Parent.transform);
		ObjectManager.ChangeObjectPosition(temp, new Vector3(0,0,-1400), Vector3.one, Vector3.zero);
		var _mvalue = temp.transform.FindChild("icon").GetComponent<UISprite>() as UISprite;
		temp.transform.GetChild(1).gameObject.SetActive(b);
		_mvalue.fillAmount= 0.0f;
		float delta = 0.05f;
		float mdelta = 0.0f;
		while(true){
			if(!isBuyWait){
				DestroyImmediate(temp);
				Destroy(this);
				break;
			}
			_mvalue.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			yield return null;
		}
	}
	public void stopLoading(){
		isBuyWait = false;
	}

	private GameObject LoadingObj;
	public bool isBuyWait =false;
	public IEnumerator buyLoading(){
		isBuyWait = true;
		if(LoadingObj == null){
			var temp = ObjectManager.CreatePrefabs("Window", "LoadingBar") as GameObject;
			ObjectManager.ChangeObjectParent(temp, Parent.transform);
			ObjectManager.ChangeObjectPosition(temp, new Vector3(0,0,-1505), Vector3.one, Vector3.zero);
			LoadingObj = temp;
		}else LoadingObj.SetActive(true);
		var _mvalue = LoadingObj.transform.FindChild("icon").GetComponent<UISprite>() as UISprite;
		LoadingObj.transform.GetChild(1).gameObject.SetActive(false);
		_mvalue.fillAmount= 0.0f;
		float delta = 0.05f;
		float mdelta = 0.0f;
		while(true){
			if(!isBuyWait){
				DestroyImmediate(LoadingObj);
				Destroy(this);
				yield break;
			}
			_mvalue.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			//Utility.Log("mdelta " +mdelta);
			
			yield return null;
		}
		
	}

	public void BuyLoadingStopCoroutine(string str){
		isBuyWait = false;
		LoadingObj.SetActive(false);
		StopCoroutine(str);
		DestroyImmediate(LoadingObj);
		LoadingObj = null;
		Destroy(this);

	}
	public IEnumerator CrossLoading(){
	//	bool isLoading = false;
		var temp = ObjectManager.CreatePrefabs("Window", "LoadingBar") as GameObject;
		ObjectManager.ChangeObjectParent(temp, Parent.transform);
		ObjectManager.ChangeObjectPosition(temp, new Vector3(0,0,-1505), Vector3.one, Vector3.zero);
		var _mvalue = temp.transform.FindChild("icon").GetComponent<UISprite>() as UISprite;
		temp.transform.GetChild(1).gameObject.SetActive(true);
		_mvalue.fillAmount= 0.0f;
		float delta = 0.05f;
		float mdelta = 0.0f;
		while(true){
			if(!Global.isAnimation){
				DestroyImmediate(temp);
				Destroy(this);
				yield break;
			}
			_mvalue.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			//Utility.Log("mdelta " +mdelta);
			yield return null;
		}

	}

		

	void OnDestroy(){
		Global.isAnimation = false;
		StopCoroutine("raceStartLoading");
		Destroy(this);
	}
}
