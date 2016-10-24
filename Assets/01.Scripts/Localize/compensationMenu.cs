using UnityEngine;
using System.Collections;

public class compensationMenu : MonoBehaviour {

	public UILabel[] lbText;
	void Start () {
		/*lbText[0].text = "드라이버 수당";
		lbText[1].text ="나사교체(GOOD)";
		lbText[2].text = "기어변속(GOOD)";
		lbText[3].text = "나사교체(PERFECT)";
		lbText[4].text = "기어변속(PERFECT)";
		lbText[5].text ="레이스 상금";
		lbText[6].text = "스폰서 보너스";
		*/
		lbText[0].text = KoStorage.GetKorString("73018"); // driver
		lbText[1].text = KoStorage.GetKorString("73016"); // good screw
		lbText[2].text = KoStorage.GetKorString("73014"); // good shift 
		lbText[3].text = KoStorage.GetKorString("73015"); //perfect screw
		lbText[4].text =  KoStorage.GetKorString("73013"); //perfect shifte
		//lbText[5].text = KoStorage.getStringDic("60138");
		lbText[6].text =  KoStorage.GetKorString("73017"); //sponsor bonus



		lbText[8].text = KoStorage.GetKorString("73018"); // driver
		lbText[9].text = KoStorage.GetKorString("73017"); // /sponsor bonus
		lbText[10].text = KoStorage.GetKorString("73013"); // perfect shift
		lbText[11].text = KoStorage.GetKorString("73014"); //good shift 

		bool bReward = false;
		switch(Global.gRaceInfo.mType){
		case MainRaceType.Club: bReward = false;break;
		case MainRaceType.Champion: bReward = true;break;
		case MainRaceType.PVP: bReward = false;break;
		case MainRaceType.Regular: bReward =true;break;
		case MainRaceType.Weekly: bReward = false;break;
		case MainRaceType.mEvent: {
			if(GV.gSetEventMode == 0) bReward = true;
			else bReward = false;
		}break;
		}
		
		if(!bReward){
		//	lbText[5].text = KoStorage.GetKorString("73012"); // 상금없음
		}else{
		//	lbText[5].transform.parent.gameObject.SetActive(false);
		}

		/*
		if(Global.gRaceInfo.raceType == RaceType.WeeklyMode || Global.gRaceInfo.raceType == RaceType.FeatureEvent){
		//	lbText[5].text = KoStorage.getStringDic("60138");
			lbText[5].transform.parent.gameObject.SetActive(false);
		}else{
			lbText[5].text = KoStorage.getStringDic("60138");
		}*/
		
		
		Destroy(this);
	}


}
