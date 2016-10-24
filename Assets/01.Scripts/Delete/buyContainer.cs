using UnityEngine;
using System.Collections;

public class buyContainer : MonoBehaviour {
	/*
	void OnBuyContainer(){
		CreateContainerPopup();
	}


	void CreateContainerPopup(){
		var pop = ObjectManager.CreateTagPrefabs("ContainerPopup") as GameObject;
		if(pop != null) {

		}else{
			var Parent = GameObject.FindGameObjectWithTag("BottomAnchor") as GameObject;
			pop = ObjectManager.CreateLobbyPrefabs(Parent.transform, "Window", "ContainerPopup", "ContainerPopup") as GameObject;
			pop.transform.parent = Parent.transform;
			ObjectManager.ChangeObjectPosition(pop, new Vector3(0,0,-1400), Vector3.one, Vector3.zero);
		}
		var temp = pop.GetComponent<buyContainerAction>() as buyContainerAction;
		if(temp == null) pop.AddComponent<buyContainerAction>();
		temp.Init(transform.name);
		//var popchild =  pop.transform.FindChild("Buy_container").gameObject as GameObject;
	
	}
	
	//void OnDisable(){
		//gameObject.GetComponent<TweenPosition>().enabled = false;
	//}
	*/

}
