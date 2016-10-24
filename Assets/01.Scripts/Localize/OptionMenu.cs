using UnityEngine;
using System.Collections;

public class OptionMenu : MonoBehaviour {

	// Use this for initialization
	public UILabel[] lbText; 
	void Awake () {
	
		SetButtonInit();
		CheckboxInit();
		ButtonInit();
		Destroy(this);
	}

	string GetWinString(string id){
		return KoStorage.GetKorString(id);//.String;
	}

	void SetButtonInit(){

		lbText[0].text = GetWinString("72507"); // home
		lbText[1].text = GetWinString("72506"); //agree
		lbText[2].text = GetWinString("72508"); //tutorial
		lbText[3].text = GetWinString("72509"); //email
		lbText[4].text = GetWinString("72510"); //logout
		//lbText[1].text = title; 확인
	}

	void 	CheckboxInit(){

		lbText[5].text = GetWinString("72517"); // agree
		lbText[6].text = GetWinString("72503"); //vibration
		lbText[7].text = GetWinString("72501"); //lb bgm
		lbText[8].text = GetWinString("72505"); //lb aram
		lbText[9].text = GetWinString("72504"); //lb quailty
		//lbText[10].text = GetWinString("60034");; //lb vibrate
	}
	void 	ButtonInit(){

		lbText[10].text = GetWinString("72500"); // title
		lbText[11].text = GetWinString("72512"); //id number

		lbText[12].text = GetWinString("72513"); //version
		lbText[13].text =GetWinString("70130"); // credit

		lbText[15].text =  KoStorage.GetKorString("73202"); // drag
		lbText[14].text =  KoStorage.GetKorString("73201");
	}



}
