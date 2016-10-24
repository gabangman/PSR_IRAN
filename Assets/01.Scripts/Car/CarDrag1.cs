using UnityEngine;
using System.Collections;

public class CarDrag1 : MonoBehaviour {
	public GameObject[] DragTrigger;
	void Awake(){



	}
	void Start () {
		for(int i=0; i< DragTrigger.Length; i++){
			if(Global.DragSub == i){
				DragTrigger[i].SetActive(true);
			}else{
				DragTrigger[i].SetActive(false);
			}
		}
	}

}
