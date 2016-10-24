using UnityEngine;
using System.Collections;

public class StarLevelInit : MonoBehaviour {

	public void Initialize(int lv){
		for(int i = 0;i <  transform.childCount; i++){
			if(i < lv){
				transform.GetChild(i).FindChild("Sprit_on").gameObject.SetActive(true);
			}else{
				transform.GetChild(i).FindChild("Sprit_on").gameObject.SetActive(false);
			}
		}
		Destroy(this);
	}
	
}
