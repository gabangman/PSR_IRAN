using UnityEngine;
using System.Collections;

public class ClubLoading : MonoBehaviour {

	bool isWait = false;
	public delegate bool OnLoadingFinish();
	public OnLoadingFinish _onFinish;
	public GameObject Parent;
	
	void Awake(){
		Parent = GameObject.FindGameObjectWithTag("BottomAnchor");
	}

	public void ReadyClubLoading(int cIdx = 0){
		if(cIdx == 1){
			StartCoroutine("StartClubLoadingVisit");
		}else{
			StartCoroutine("StartClubLoading");
		}

	}
	public IEnumerator StartClubLoadingVisit(){
		bWait = true;
		var temp = ObjectManager.CreatePrefabs("Window", "LoadingBar") as GameObject;
		ObjectManager.ChangeObjectParent(temp, Parent.transform);
		ObjectManager.ChangeObjectPosition(temp, new Vector3(0,0,-1500), Vector3.one, Vector3.zero);
		temp.transform.GetChild(1).transform.localPosition = new Vector3(0,353.15f,-1000);
		var _mvalue = temp.transform.FindChild("icon").GetComponent<UISprite>() as UISprite;
		temp.transform.GetChild(1).gameObject.SetActive(true);
		_mvalue.fillAmount= 0.0f;
		float delta = 0.05f;
		float mdelta = 0.0f;
	
		
		while(bWait){
			_mvalue.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			yield return null;
			
		}
		
		DestroyImmediate(temp);
		Destroy(this);
		
	}

	public IEnumerator StartClubLoading(){
		bWait = true;
		var temp = ObjectManager.CreatePrefabs("Window", "LoadingBar") as GameObject;
		ObjectManager.ChangeObjectParent(temp, Parent.transform);
		ObjectManager.ChangeObjectPosition(temp, new Vector3(0,0,-1400), Vector3.one, Vector3.zero);
		var _mvalue = temp.transform.FindChild("icon").GetComponent<UISprite>() as UISprite;
		temp.transform.GetChild(1).gameObject.SetActive(true);
		_mvalue.fillAmount= 0.0f;
		float delta = 0.05f;
		float mdelta = 0.0f;

		while(bWait){
			_mvalue.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			yield return null;
	
		}
	
		DestroyImmediate(temp);
		Destroy(this);
	
	}
	public void StopClubLoading(){
		bWait = false;
	//	StopCoroutine("StartClubLoading");
	}
	
	private GameObject LoadingObj;
	public bool bWait =false;

	
	
	void OnDestroy(){
		bWait = false;
		Global.isAnimation = false;
		StopCoroutine("StartClubLoading");
		Destroy(this);
	}
}
