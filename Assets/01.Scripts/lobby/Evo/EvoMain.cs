using UnityEngine;
using System.Collections;

public class EvoMain : MonoBehaviour {

	public GameObject repair, evo;

	public void OnClose(){
		repair.SetActive(false);
		evo.SetActive(false);
		//gameObject.SetActive(false);
	}

	}
