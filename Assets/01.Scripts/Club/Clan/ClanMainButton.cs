using UnityEngine;
using System.Collections;

public class ClanMainButton : MonoBehaviour {
	public Transform[] trBtn;
	public UILabel[] text;
	public UISprite[] sprite;
//	float clickScaleY = 52f*1.2f;
//	float originalScaleY = 52f;
	private bool isButtonPress = false;
	public Color pressColor, ReleaseColor;
	void Start(){
		ChangeColor(0);

	}

	void ChangeColor(int idx){
		for(int i = 0; i < 4; i++){
			if(i == idx){
				sprite[i].color =pressColor;
				trBtn[i].parent.FindChild("Select").gameObject.SetActive(true);
				//	trBtn[i].localScale = new Vector3(180, clickScaleY,1);
			}else{
				sprite[i].color = ReleaseColor;
				trBtn[i].parent.FindChild("Select").gameObject.SetActive(false);
				//	trBtn[i].localScale = new Vector3(180, originalScaleY,1);
			}
		}
	}

	void OnEnable(){
		////==!!Utility.LogWarning("ClanMainButton");
		ResetButtonPress();
	}
	void ResetButtonPress(){
		isButtonPress = false;
		////==!!Utility.LogWarning("resetbtn");
	}

	void OnClanMySet(){
		ChangeColor(0);
	}
	void OnClanMy(){
		if(Global.isNetwork) return;
		if(isButtonPress) return;
		isButtonPress = true; Invoke("ResetButtonPress",0.35f);
		if(sprite[0].color == pressColor) return;
		ChangeColor(0);
		transform.parent.GetComponent<ClanWindow>().ChangeObject(0);
	}
	void OnClanRank(){
		if(Global.isNetwork) return;
		if(isButtonPress) return;
		isButtonPress = true; Invoke("ResetButtonPress",0.35f);
		if(sprite[1].color == pressColor) return;
		ChangeColor(1);
		transform.parent.GetComponent<ClanWindow>().ChangeObject(1);
	
	}
	void OnClanHistory(){
		if(Global.isNetwork) return;
		if(isButtonPress) return;
		isButtonPress = true; 
		Invoke("ResetButtonPress",0.35f);
		if(sprite[2].color == pressColor) return;
		ChangeColor(2);
		transform.parent.GetComponent<ClanWindow>().ChangeObject(2);
	}
	void OnClanSearch(){
		if(Global.isNetwork) return;
		if(isButtonPress) return;
		isButtonPress = true; Invoke("ResetButtonPress",0.35f);
		if(sprite[3].color == pressColor) return;
		ChangeColor(3);

		transform.parent.GetComponent<ClanWindow>().ChangeObject(3);
	}


}
