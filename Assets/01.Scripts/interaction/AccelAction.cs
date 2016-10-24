using UnityEngine;
using System.Collections;

public class AccelAction : MonoBehaviour {
	public GameObject AccelBG;
	public GameObject Ring;
	void Awake(){
		Ring.GetComponent<UISprite>().alpha = 0.0f;
	}

	void OnPress(bool isPress){
		if(isPress){
			AccelBG.SetActive(true);
			Ring.GetComponent<UISprite>().alpha = 1.0f;
		}else{
			AccelBG.SetActive(false);
			Ring.GetComponent<UISprite>().alpha = 0.0f;
		}
	}
	
}
