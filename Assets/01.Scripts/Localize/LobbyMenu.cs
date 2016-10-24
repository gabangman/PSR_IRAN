using UnityEngine;
using System.Collections;

public class LobbyMenu : MonoBehaviour {

	void Start(){
		string title = string.Empty;
		title = KoStorage.GetKorString("72000");
		if(transform.GetChild(1).gameObject.activeSelf)
			transform.GetChild(1).GetComponentInChildren<UILabel>().text = title; //clanr

		//title = TableManager.ko.dictionary["60009"].String;
		title = KoStorage.GetKorString("72001");
		if(transform.GetChild(2).gameObject.activeSelf)
		transform.GetChild(2).GetComponentInChildren<UILabel>().text = title; // myteam

		//title = TableManager.ko.dictionary["60013"].String;
		title = KoStorage.GetKorString("72002");
		if(transform.GetChild(3).gameObject.activeSelf)
		transform.GetChild(3).GetComponentInChildren<UILabel>().text = title; // upg

		//title = TableManager.ko.dictionary["60012"].String;
		title = KoStorage.GetKorString("72003");
		if(transform.GetChild(4).gameObject.activeSelf)
		transform.GetChild(4).GetComponentInChildren<UILabel>().text = title; // invne

		//title = TableManager.ko.dictionary["60010"].String;
		title = KoStorage.GetKorString("72004");
		if(transform.GetChild(5).gameObject.activeSelf)
		transform.GetChild(5).GetComponentInChildren<UILabel>().text = title; //shop

	//	title = KoStorage.GetKorString("72005");
	//	if(transform.GetChild(6).gameObject.activeSelf)
	//		transform.GetChild(6).GetComponentInChildren<UILabel>().text = title; //next
		title = KoStorage.GetKorString("76007");
	//	if(transform.GetChild(7).gameObject.activeSelf)
	//		transform.GetChild(7).GetComponentInChildren<UILabel>().text = title; //next
		transform.GetChild(7).FindChild("Label").GetComponent<UILabel>().text = title;
		Destroy(this);
	}

}
