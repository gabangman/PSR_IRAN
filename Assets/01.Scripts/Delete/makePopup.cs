using UnityEngine;
using System.Collections;

public class makePopup : MonoBehaviour {
	/*
	public void makePopupDismantle(string createpop){
		var pop =SearchWindow();
		pop.transform.FindChild("Content_BUY").gameObject.SetActive(false);
		var popchild = pop.transform.FindChild("Dismantle_Result").gameObject as GameObject;
		popchild.SetActive(true);
		pop.GetComponent<TweenAction>().doubleTweenScale(popchild);
		pop.AddComponent(createpop);
		Destroy(this);
	}

	public void makePopupAction(string popupcreate){
		var pop =SearchWindow();
		var popchild = pop.transform.FindChild("Content_BUY").gameObject as GameObject;
		pop.GetComponent<TweenAction>().doubleTweenScale(popchild);
		pop.AddComponent(popupcreate);
		Destroy(this);
	}

	public void makePopupAction(string popupcreate, int idx){
		var pop = SearchWindow();
		var popchild = pop.transform.FindChild("Content_BUY").gameObject as GameObject;
		pop.GetComponent<TweenAction>().doubleTweenScale(popchild);
		pop.AddComponent(popupcreate);
		pop.SendMessage("InitPopUp", idx,SendMessageOptions.DontRequireReceiver);
		Destroy(this);
	}

	public void makePopupAction(string popupcreate, GameObject obj){
		var pop =makePopUp();
		var popchild = pop.transform.FindChild("Content_BUY").gameObject as GameObject;
		pop.GetComponent<TweenAction>().doubleTweenScale(popchild);
		pop.AddComponent(popupcreate);
		pop.SendMessage("InitPopUp", obj,SendMessageOptions.DontRequireReceiver);
		Destroy(this);
	}
	public void makePopupAction(string popupcreate, string str){
		var pop = SearchWindow();
		var popchild = pop.transform.FindChild("Content_BUY").gameObject as GameObject;
		pop.GetComponent<TweenAction>().doubleTweenScale(popchild);
		pop.AddComponent(popupcreate);
		pop.SendMessage("InitPopUp", str,SendMessageOptions.DontRequireReceiver);
		Destroy(this);
	}
	public GameObject makePopUp(){
		var pop =SearchWindow();
		var popchild = pop.transform.FindChild("Content_BUY").gameObject as GameObject;
		pop.GetComponent<TweenAction>().doubleTweenScale(popchild);
		Destroy(this, 0.1f);
		return pop;
	}

	public GameObject makeEventPopUp(){
		var pop =SearchWindow();
		Destroy(this, 0.1f);
		return pop;
	}

	public GameObject SearchWindow(){
		var pop = ObjectManager.CreateTagPrefabs("PopWindow") as GameObject;
		if(pop != null) {
			
		}else{
			var Parent = GameObject.FindGameObjectWithTag("BottomAnchor") as GameObject;
			pop = ObjectManager.CreateLobbyPrefabs(Parent.transform, "Window", "popUp", "PopWindow") as GameObject;
			pop.transform.parent = Parent.transform;
			ObjectManager.ChangeObjectPosition(pop, new Vector3(0,0,-1400), Vector3.one, Vector3.zero);
		}
		return pop;
	}
		*/

}
