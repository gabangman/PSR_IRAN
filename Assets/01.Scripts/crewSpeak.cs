using UnityEngine;
using System.Collections;

public class crewSpeak : MonoBehaviour {

	public Transform[] speakPos;
	public GameObject speakPrefabs;
	private GameObject speakbaloon;
	public Vector3 _pos;
	public float posZ = 1.0f;
	public Camera guiCamera, currentCamera;
	private bool bLobby;
	private Transform _target;

	void FixedUpdate(){
		if(!speakbaloon.activeSelf) return;
		_pos = currentCamera.WorldToViewportPoint(_target.position);
		_pos = guiCamera.ViewportToWorldPoint(_pos);
		_pos.z =posZ;
		speakbaloon.transform.position = _pos;
	}

	public void LobbyCamCheck(bool b){
		bLobby = b;

		if(b){
			//if(!speakbaloon.activeSelf) speakbaloon.SetActive(true);
		}else{
			if(speakbaloon.activeSelf) speakbaloon.SetActive(false);
		}
	}
	// Use this for initialization
	void Start () {
		speakbaloon = Instantiate(speakPrefabs) as GameObject;
		speakbaloon.SetActive(false);
		if(guiCamera == null)
			guiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
		bLobby = true;
		InvokeRepeating("speakTest", 5.0f, 5.0f);
		speakbaloon.transform.parent = transform.parent;
	}

	void OnDisable(){
		if(IsInvoking("speakTest") == true)
			CancelInvoke("speakTest");
	}
	// 74050~74099
	void speakTest(){
		int n = Random.Range(0,5);
		speakbaloon.transform.localEulerAngles = Vector3.zero;
		speakbaloon.transform.localScale = Vector3.one;
		speakbaloon.transform.localPosition = Vector3.zero;
		if(bLobby)
			StartCoroutine("CrewSpeakRound",speakPos[n]);
	}


	IEnumerator CrewSpeakRound(Transform target){
		this._target = target;
		int n = Random.Range(0,50);
		n = n+74050;
		speakbaloon.transform.GetChild(0).GetComponent<UILabel>().text = KoStorage.GetKorString(n.ToString());
		speakbaloon.SetActive(bLobby);
		speakbaloon.GetComponent<TweenScale>().Reset();
		speakbaloon.GetComponent<TweenScale>().enabled = true;
	
		yield return new WaitForSeconds(4.0f);
		speakbaloon.SetActive(false);
		yield return null;
	}
}
