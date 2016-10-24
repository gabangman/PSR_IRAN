using UnityEngine;
using System.Collections;

public class modeInfoAction : MonoBehaviour {

	public GameObject CreateRaceModeInfo(int info1, int info2){
		info1 += 70000;
		info2 += 70000;
		var temp = ObjectManager.CreateTagPrefabs("ModeInfo") as GameObject;
		string str = KoStorage.GetKorString(info1.ToString());//TableManager.ko.dictionary[info1.ToString()].String;
		string str1 = KoStorage.GetKorString(info2.ToString());//TableManager.ko.dictionary[info2.ToString()].String;
		if(temp == null){
			GameObject Parent = GameObject.FindGameObjectWithTag("BottomAnchor");
			temp =  ObjectManager.CreateLobbyPrefabs(Parent.transform.GetChild(0), "Window", "ModeInfo", "ModeInfo");
		}disableInfo(temp.transform);
		var child = temp.transform.FindChild("raceInfo") as Transform;
		child.transform.FindChild("info1").GetComponent<UILabel>().text = str;
		child.transform.FindChild("info2").GetComponent<UILabel>().text = str1;

		child.gameObject.SetActive(true);
		temp.transform.GetComponent<TweenAction>().ForwardPlayTween(child.gameObject);

		Destroy(this,0.2f);
		return child.gameObject;
	}


	public GameObject CreateSponsorInfo(int info1){
		info1 += 70000;
		var temp = ObjectManager.CreateTagPrefabs("ModeInfo") as GameObject;
		string str = KoStorage.GetKorString(info1.ToString());//TableManager.ko.dictionary[info1.ToString()].String;
		//string str1 = TableManager.ko.dictionary[info2.ToString()].String;
		if(temp == null){
			GameObject Parent = GameObject.FindGameObjectWithTag("BottomAnchor");
			temp =  ObjectManager.CreateLobbyPrefabs(Parent.transform.GetChild(0), "Window", "ModeInfo", "ModeInfo");	}disableInfo(temp.transform);
		var child = temp.transform.FindChild("sponsorInfo") as Transform;
		child.gameObject.SetActive(true);
		temp.transform.GetComponent<TweenAction>().ForwardPlayTween(child.gameObject);
		child.transform.FindChild("info1").GetComponent<UILabel>().text = str;
		//child.transform.FindChild("info2").GetComponent<UILabel>().text = str1;
		Destroy(this,0.2f);
		return child.gameObject;
	}

	public GameObject CreateTeamInfo(int info1){
		info1 += 70000;
		var temp = ObjectManager.CreateTagPrefabs("ModeInfo") as GameObject;
		string str = KoStorage.GetKorString(info1.ToString());//TableManager.ko.dictionary[info1.ToString()].String;
		//string str1 = TableManager.ko.dictionary[info2.ToString()].String;
		if(temp == null){
			GameObject Parent = GameObject.FindGameObjectWithTag("BottomAnchor");
			temp =  ObjectManager.CreateLobbyPrefabs(Parent.transform.GetChild(0), "Window", "ModeInfo", "ModeInfo");
		}disableInfo(temp.transform);
		var child = temp.transform.FindChild("teamInfo") as Transform;
		child.gameObject.SetActive(true);
		child.FindChild("img_NPC").gameObject.SetActive(true);
		temp.transform.GetComponent<TweenAction>().ForwardPlayTween(child.gameObject);
		child.transform.FindChild("info1").GetComponent<UILabel>().text = str;
		//child.transform.FindChild("info2").GetComponent<UILabel>().text = str1;
		Destroy(this,0.2f);
		return child.gameObject;
	}

	public GameObject CreateUpgradeInfo(int info1){
		info1 += 70000;
		var temp = ObjectManager.CreateTagPrefabs("ModeInfo") as GameObject;
		string str = KoStorage.GetKorString(info1.ToString());//TableManager.ko.dictionary[info1.ToString()].String;
		//string str1 = TableManager.ko.dictionary[info2.ToString()].String;
		if(temp == null){
			GameObject Parent = GameObject.FindGameObjectWithTag("BottomAnchor");
			temp =  ObjectManager.CreateLobbyPrefabs(Parent.transform.GetChild(0), "Window", "ModeInfo", "ModeInfo");
		}disableInfo(temp.transform);
		var child = temp.transform.FindChild("upgradeInfo") as Transform;
		child.gameObject.SetActive(true);
		temp.transform.GetComponent<TweenAction>().ForwardPlayTween(child.gameObject);
		child.transform.FindChild("info1").GetComponent<UILabel>().text = str;
		//child.transform.FindChild("info2").GetComponent<UILabel>().text = str1;
		Destroy(this,0.2f);
		return child.gameObject;
	}

	public GameObject CreateShopInfo(int info1){
		info1 += 70000;
		var temp = ObjectManager.CreateTagPrefabs("ModeInfo") as GameObject;
		string str =KoStorage.GetKorString(info1.ToString());// TableManager.ko.dictionary[info1.ToString()].String;
		//string str1 = TableManager.ko.dictionary[info2.ToString()].String;
		if(temp == null){
			GameObject Parent = GameObject.FindGameObjectWithTag("BottomAnchor");
			temp =  ObjectManager.CreateLobbyPrefabs(Parent.transform.GetChild(0), "Window", "ModeInfo", "ModeInfo");

		}
		disableInfo(temp.transform);
		var child = temp.transform.FindChild("shopInfo") as Transform;
		child.gameObject.SetActive(true);
		temp.transform.GetComponent<TweenAction>().ForwardPlayTween(child.gameObject);
		child.transform.FindChild("info1").GetComponent<UILabel>().text = str;
		//child.transform.FindChild("info2").GetComponent<UILabel>().text = str1;
		Destroy(this,0.2f);
		return child.gameObject;
	}


	void disableInfo(Transform child){
		int cnt = child.childCount;
		for(int i = 0; i < cnt; i++){
			child.GetChild(i).gameObject.SetActive(false);
		}
	}

}
