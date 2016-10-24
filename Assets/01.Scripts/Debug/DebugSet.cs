using UnityEngine;
using System.Collections;

public class DebugSet : MonoBehaviour {

	public UILabel[] lbTexts;

	void Start(){
		lbTexts[0].text = GV.ChSeasonID.ToString();
		lbTexts[1].text = GV.SponID.ToString();
		lbTexts[3].text = GV.myCoin.ToString();
		lbTexts[2].text = GV.myDollar.ToString();
	//	lbTexts[4].text = GV.getTeamCarID(GV.SelectedTeamID).ToString();
	//	lbTexts[5].text = "D";//GV.get).ToString();//"D";//차량 클래스 입력 D ~ SS";
	//	Utility.LogWarning("car id " + GV.getTeamCarID(GV.SelectedTeamID));
	}
	void OnDebugOk(){
		GV.ChSeasonID = int.Parse(lbTexts[0].text);
		GV.SponID = int.Parse(lbTexts[1].text);
		GV.myCoin = int.Parse(lbTexts[3].text);
		GV.myDollar = int.Parse(lbTexts[2].text);
		int a = GV.getTeamCarID(GV.SelectedTeamID);
		/*
		GV.gChamWin = 1;
		Global.gSeasonUp = 11;
		GV.gDealer = 1;
		GV.gDealerCarID = 1011;
		GV.gDealerClass = "S";
		GV.gBuyDealerCar = 1;
		GV.gADWindow = 1;


		GV.gChamWin = 1;
		Global.gSeasonUp = 10;



		GV.gDealer = 1;
		GV.gDealerCarID = 1011;
		GV.gDealerClass = "S";
		GV.gBuyDealerCar = 1;
*/	
		//GV.gDealer = 1;GV.gDealerCarID = 1002;GV.gBuyDealerCar = 1;GV.gADWindow = 1;GV.gDealerClass = "S";
	//	if(GV.TChangeCar == a) 
	//	{
	//		GV.TChangeCar = 0;
	//		return;
	//	}else{
	//		GV.TChangeClass = lbTexts[5].text;
	//	}
	}

	}
