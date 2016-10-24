using UnityEngine;
using System.Collections;

public class modeFlagAction : MonoBehaviour {
	Vector3 originScale;
	void OnEnable(){
		originScale = transform.localScale;
		transform.GetComponent<TweenScale>().enabled = true;

	}

	void OnDisable(){
		transform.GetComponent<TweenScale>().enabled = false;
		transform.localScale = originScale;
	}


}
