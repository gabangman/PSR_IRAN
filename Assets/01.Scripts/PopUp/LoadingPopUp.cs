using UnityEngine;
using System.Collections;

public class LoadingPopUp : MonoBehaviour {

	
	public GameObject loadingBar;
	
	/*public void CreateLoadingBar(){
		if(loadingBar != null) return;
		var temp = ObjectManager.CreatePrefabs("Window", "SubLoadingBar") as GameObject;
		temp.SetActive(false);
		loadingBar = temp;
	}*/
	
	public void ChangeObject(GameObject obj){
	//	CreateLoadingBar();
		ObjectManager.ChangeObjectParent(loadingBar, obj.transform);
		ObjectManager.ChangeObjectPosition(loadingBar, new Vector3(500,0,-1500), Vector3.one, Vector3.zero);
	}
	
	public IEnumerator LoadingProcess(){
		tweenPositionAction(0);
		var _mvalue = loadingBar.transform.FindChild("icon").GetComponent<UISprite>() as UISprite;
		_mvalue.fillAmount= 0.0f;
		float delta = 0.05f;
		float mdelta = 0.0f;
		while(true){
			_mvalue.fillAmount = mdelta;
			mdelta += delta;
			if(mdelta >1.0f){
				mdelta = 0.0f;
			}
			
			if(!Global.isNetwork){
				//loadingBar.SetActive(false);
				tweenPositionAction(1);
				yield break;
			}
			yield return null;
		}
	}
	
	public void tweenPositionAction(int id){
		if(loadingBar == null) return;
		loadingBar.SetActive(true);
		var tw = loadingBar.GetComponent<TweenPosition>() as TweenPosition;
		Vector3 to = tw.to;
		tw.to = tw.from;
		tw.from = to;
		
		if(id == 1){
			tw.onFinished = delegate(UITweener tween) {
				loadingBar.SetActive(false);
			};
		}else{
			tw.onFinished = null;
		}
		tw.Reset();
		tw.enabled = true;
		
	}
}
