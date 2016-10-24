using UnityEngine;
using System.Collections;

public class ContainerAction : MonoBehaviour {

	void OnEnable(){

	}
	public void StartContainer(){
		if(tempObj != null){
	//		InitContainerInfo();
		}else{
	//		var t = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
	//		tempObj = t.makeContainerInfo(transform.parent);
	//		Destroy(t);
		}
		OnBronzeClick();
	}
	public GameObject getContainerInfo(){
		/*if(tempObj != null){
			//	InitContainerInfo();
			tempObj.SetActive(true);
			tempObj.GetComponent<TweenAction>().ForwardPlayTween(tempObj);
		}else{
			var t = gameObject.AddComponent<CPopUpCreate>() as CPopUpCreate;
			tempObj = t.makeContainerInfo(transform.parent);
			Destroy(t);
		}*
		return tempObj;*/
		return null;
	}


	GameObject tempObj;
	public Transform Gold, Silver, Bronze;
	const int originScaleY = 44;
	const int clickScaleY = 55;
	public bool OnGoldClick(){
		bool b = false;
		var carsprite = Gold.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == Color.white) b = true;
		
		carsprite.color = Color.white;
		Gold.localScale = new Vector3(180, clickScaleY,1);
		
		Silver.localScale = new Vector3(180,originScaleY,1);
		Silver.GetComponent<UISprite>().color = Color.gray;
		Bronze.localScale = new Vector3(180, originScaleY,1);
		Bronze.GetComponent<UISprite>().color = Color.gray;
		if(!b){
			tempObj.GetComponent<TweenAction>().ReverseTween(tempObj);
			tempObj.GetComponent<TweenAction>().ForwardPlayTween(tempObj);
		}

		//Utility.Log("ongoldclick" + b);
		return b;
	}
	
	public bool OnSilverClick(){
		bool b = false;
		var carsprite = Silver.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == Color.white) b = true;
		
		carsprite.color = Color.white;
		Silver.localScale = new Vector3(180, clickScaleY,1);
		
		Bronze.localScale = new Vector3(180,originScaleY,1);
		Bronze.GetComponent<UISprite>().color = Color.gray;
		Gold.localScale = new Vector3(180, originScaleY,1);
		Gold.GetComponent<UISprite>().color = Color.gray;
		if(!b){
			tempObj.GetComponent<TweenAction>().ReverseTween(tempObj);
			tempObj.GetComponent<TweenAction>().ForwardPlayTween(tempObj);
		}
		return b;
	}
	
	public bool OnBronzeClick(){
		bool b = false;
		var carsprite = Bronze.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == Color.white) b = true;
		
		carsprite.color = Color.white;
		Bronze.localScale = new Vector3(180, clickScaleY,1);
		
		Gold.localScale = new Vector3(180,originScaleY,1);
		Gold.GetComponent<UISprite>().color = Color.gray;
		Silver.localScale = new Vector3(180, originScaleY,1);
		Silver.GetComponent<UISprite>().color = Color.gray;
		if(!b){
			tempObj.GetComponent<TweenAction>().ReverseTween(tempObj);
			tempObj.GetComponent<TweenAction>().ForwardPlayTween(tempObj);
		}
		return b;
	}
	
	
	public void ResetButtonColor(){
		Gold.GetComponent<UISprite>().color = Color.gray;
		Silver.GetComponent<UISprite>().color = Color.gray;
		Bronze.GetComponent<UISprite>().color = Color.gray;
	}
}
