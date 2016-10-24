using UnityEngine;
using System.Collections;

public class Resultdisplayaction : MonoBehaviour {
	
	//	bool isRank;
	int earnDollar;
	public void SendToServerRaceResult(){
		RaceResultRewardPlus();
	}
	void Start(){
		
	}
	
	string rankText;
	void RaceResultRewardPlus(){
		float _r = 0.0f;
		int rank = Base64Manager.instance.GlobalEncoding(Global.myRank,1);
		if(rank > 10 || rank < 0){
			Application.Quit();
		}
		if(rank == 0){
			rankText = "WINNER";
			_r = (float)Common_Ratio.Get(6804).R_Ratio * 0.1f;
			if(Global.gRaceInfo.mType == MainRaceType.Club){
				GAchieve.instance.achieveInfo.PlusAchievement(16024);
			}else{
				GAchieve.instance.achieveInfo.PlusAchievement(16000);
			}
			//GV.bRaceLose = false;
			//Utility.LogWarning("16000");
		}else if(rank == 1){
			//GV.bRaceLose = true;
			_r = (float)Common_Ratio.Get(6805).R_Ratio * 0.1f;
			rankText = "YOU LOSE";	
		}else if(rank== 2){
			//GV.bRaceLose = true;
			_r = (float)Common_Ratio.Get(6806).R_Ratio * 0.1f;
			rankText = "YOU LOSE";	
		}else if(rank == 3){
			//GV.bRaceLose = true;
			_r = (float)Common_Ratio.Get(6807).R_Ratio * 0.1f;
			rankText = "YOU LOSE";	
		}else if(rank== 4){
			//GV.bRaceLose = true;
			_r = (float)Common_Ratio.Get(6808).R_Ratio * 0.1f;
			rankText = "YOU LOSE";	
		}else if(rank == 5){
			//GV.bRaceLose = true;
			_r = (float)Common_Ratio.Get(6809).R_Ratio * 0.1f;
			rankText = "YOU LOSE";	
		}else if(rank == 6){
			//GV.bRaceLose = true;
			_r = (float)Common_Ratio.Get(6810).R_Ratio * 0.1f;
			rankText = "YOU LOSE";	
		
		}//else  GV.bRaceLose = true;
	
		GAchieve.instance.achieveInfo.PlusAchievement(16021,GameManager.instance.PCount);
		MainRaceType rType = Global.gRaceInfo.mType;
		//Utility.Log(_r);
		switch(rType){
		case MainRaceType.Champion:
		{
			if(rank != 0) {
				Global.gRaceInfo.extraCoin = 0;
				_r = (float)Common_Ratio.Get(6801).R_Ratio * 0.1f;
				if(GV.ChSeasonID <= 6004){
					GV.bHelp = true;
				}else{
					GV.bHelp = false;
				}
			}else{
				_r = (float)Common_Ratio.Get(6800).R_Ratio * 0.1f;


			}
			rankText = string.Empty;
			Callback = (win)=>{
				win.GetComponent<ModeRewardWin>().CompensationDisable(gameObject,RankName);
			};
		}break;
		case MainRaceType.Club:
		{
			_r = 0.0f;
			Callback = (win)=>{
				win.GetComponent<ModeRewardWin>().CompensationDisable(gameObject,RankName);
			};
		}break;
		case MainRaceType.mEvent:
		{
			if(Global.gRaceInfo.eventModeName == "New"){
				if(GV.ChSeasonID <= 6004){
					if(rank != 0) GV.bHelp = true;
					else GV.bHelp = false;
				}
			}
			Callback = (win)=>{
				win.GetComponent<ModeRewardWin>().CompensationDisable(gameObject, RankName);
			};
		}break;
		case MainRaceType.Regular:
		{
			if(GV.ChSeasonID <= 6004){
				if(rank != 0) 	GV.bHelp = true;
				else GV.bHelp = false;
			}
			Callback = (win)=>{
				win.GetComponent<ModeRewardWin>().CompensationDisable(gameObject, RankName);
				
			};
		}break;
		case MainRaceType.Weekly:{
			rankText = string.Empty;
			if(GV.ChSeasonID <= 6004){
				if(rank == 6) GV.bHelp = true;
				else GV.bHelp = false;
			}
			Callback = (win)=>{
				win.GetComponent<ModeRewardWin>().CompensationDisable(gameObject, RankName);
			};
		}break;
		case MainRaceType.PVP:
		{
			Callback = (win)=>{
				win.GetComponent<ModeRewardWin>().CompensationDisable(gameObject, RankName);
			};
			_r = 0;
		}break;
		default:
			break;
		}
		_r =0.0f;
		earnDollar =(int)Mathf.Round((float)Global.gRaceInfo.mode1Rw * _r);
		earnDollar = Base64Manager.instance.GlobalEncoding(earnDollar);
		GV.gRaceCount++;
		if(!EncryptedPlayerPrefs.HasKey("DealerCount")){
			int mA = Random.Range(1,4);
			EncryptedPlayerPrefs.SetInt("DealerCount",mA);
		}
		int mRaceCount = EncryptedPlayerPrefs.GetInt("DealerCount");
		if(mRaceCount == GV.gRaceCount){
			GV.bRaceLose = true;
		}else{
			GV.bRaceLose = false;
		}

	}
	
	System.Action<GameObject> Callback;
	void CreateMissionWindow(){
		var win = ObjectManager.GetRaceObject("Race","ModeRewardWindow_1") as GameObject;
		ObjectManager.ChangeObjectParent(win,transform.parent);
		ObjectManager.ChangeObjectPosition(win,Vector3.zero, Vector3.one, Vector3.zero);
		win.SetActive(true);
		//win.AddComponent<ModeRewardWin>().initialize();

		win.AddComponent<ModeRewardWin>().NewInitialize();
		if(Callback != null)
			Callback(win);
	}
	
	void RewardTest(){
		//	if(!Global.isRaceTest) return;
		//Global.myRank = 1;
		//	Global.gRaceInfo.raceType = 
		//	RaceType.ChampionMode;
		//	RaceType.DailyMode;
		//	RaceType.FeatureEvent;
		//	RaceType.RegularMode;
		//	RaceType.WeeklyMode;
	}
	
	void RaceModeReward(UITweener tween){
		//var temp = tween.transform.gameObject as GameObject;
		isFinish = true;
	}
	
	string RankName;
	
	public void CompensationResult(GameObject comp, string name, bool b){
		RankName = name;
		earnDollar = Base64Manager.instance.GlobalEncoding(earnDollar);

		comp.transform.FindChild("lbTitle").gameObject.GetComponent<UILabel>().text = rankText;
		int vipDollar = Base64Manager.instance.GlobalEncoding(Global.gBonus);

		var temp = comp.transform.FindChild("SPONSORBONUS").gameObject;
		var temp1 = temp.transform.FindChild("score").gameObject;
		temp1.GetComponent<UILabel>().text = Base64Manager.instance.GlobalEncoding(Global.gRaceInfo.SponBouns).ToString();
		
		temp = comp.transform.FindChild("DRIVERBONUS").gameObject;
		temp1 = temp.transform.FindChild("score").gameObject;
		temp1.GetComponent<UILabel>().text = vipDollar.ToString();

			 temp = comp.transform.FindChild("PERFECTSHIFTS").gameObject;
			 temp1 = temp.transform.FindChild("score").gameObject;
			 temp1.GetComponent<UILabel>().text = Base64Manager.instance.GlobalEncoding(Global.pGearScore).ToString();
			
			temp = comp.transform.FindChild("GOODSHIFTS").gameObject;
			temp1 = temp.transform.FindChild("score").gameObject;
			temp1.GetComponent<UILabel>().text = Base64Manager.instance.GlobalEncoding(Global.gGearScore).ToString();
		if(!b){
			temp = comp.transform.FindChild("PERFECTSCREW").gameObject;
			temp1 = temp.transform.FindChild("score").gameObject;
			temp1.GetComponent<UILabel>().text = Base64Manager.instance.GlobalEncoding(Global.pDrillScore).ToString();
			
			temp = comp.transform.FindChild("GOODSCREW").gameObject;
			temp1 = temp.transform.FindChild("score").gameObject;
			temp1.GetComponent<UILabel>().text =Base64Manager.instance.GlobalEncoding(Global.gDrillScore).ToString();
		}

		if(Global.gRaceInfo.mType == MainRaceType.mEvent){
			comp.transform.FindChild("Durability").gameObject.SetActive(false);
		}else{
			int tId = 0;
			if(Global.gRaceInfo.mType == MainRaceType.Club){
				tId = CClub.ClubTeamID;
			}else{
				tId = GV.SelectedTeamID;
			}
			CarInfo carinfo = GV.getTeamCarInfo(tId);
			temp = comp.transform.FindChild("Durability").gameObject;
			int cardu = 0;
			if(carinfo.carClass.Durability <= 0) cardu = 0;
			else cardu = carinfo.carClass.Durability;
			temp.transform.GetComponent<stateUp>().ChangeBarDurability((float)cardu,  (float)carinfo.carClass.DurabilityRef);
			temp.transform.FindChild("lbStatName1").GetComponent<UILabel>().text = KoStorage.GetKorString("76020"); 
		}

		int tDollar = Base64Manager.instance.GlobalEncoding(Global.gDrillScore) 
			+ Base64Manager.instance.GlobalEncoding(Global.pDrillScore) 
			+ Base64Manager.instance.GlobalEncoding(Global.pGearScore)
		    + Base64Manager.instance.GlobalEncoding(Global.gGearScore)
			+ Base64Manager.instance.GlobalEncoding(Global.gRaceInfo.SponBouns) 
			+ Base64Manager.instance.GlobalEncoding(Global.gBonus);
	
	
		temp = comp.transform.FindChild("TOTALDALLAR").gameObject;
		var vip = temp.transform.FindChild("VIP_Add") as Transform;
		if(!b){ //pitstop_yes
			if(GV.gVIP == 0){
				vip.gameObject.SetActive(false);
			}else{
				int vipID = 1900+(GV.gVIP-1);
				Common_VIP.Item vItem = Common_VIP.Get(vipID);
				vip.gameObject.SetActive(true);
				vip.FindChild("lbAdd").GetComponent<UILabel>().text 
					= string.Format("+${0}", vItem.V_add_regular);
				vip.FindChild("lbName").GetComponent<UILabel>().text = "VIP";
				tDollar += vItem.V_add_regular;
				Global.gBonus = Base64Manager.instance.GlobalEncoding(vipDollar+vItem.V_add_regular); 
				
			}
		}else{ //pitstop_no
			if(GV.gVIP == 0){
				vip.gameObject.SetActive(false);
			}else{
				vip.gameObject.SetActive(true);
				int vipID = 1900+(GV.gVIP-1);
				Common_VIP.Item vItem = Common_VIP.Get(vipID);
				vip.FindChild("lbAdd").GetComponent<UILabel>().text 
					= string.Format("+${0}", vItem.V_add_regular);
				vip.FindChild("lbName").GetComponent<UILabel>().text = "VIP";
				tDollar += vItem.V_add_regular;
				Global.gBonus = Base64Manager.instance.GlobalEncoding(vipDollar+vItem.V_add_regular); 
			}
		}

		earnDollar =  tDollar + earnDollar;
		GV.myDollar = GV.myDollar+earnDollar;
		GV.updateDollar = -earnDollar;
		/*
		if(tDollar > 10000 || tDollar < 0 ) {
			//block 처리 요청 할 것!!!
			//ProtocolManager.instance.RequestBlock("1");
			//Application.Quit();
		}
		if(Global.gRaceInfo.mType == MainRaceType.Champion){
			var temp2 = comp.transform.FindChild("TOTALCOIN").gameObject as GameObject;
			temp2.SetActive(true);
			isCoinCount = true;
			var score = temp2.transform.FindChild("score").GetComponent<UILabel>() as UILabel;
			StartCoroutine("CoinCounting", score);
			GV.myCoin = GV.myCoin+Global.gRaceInfo.extraCoin;
			GV.updateCoin = Global.gRaceInfo.extraCoin;
		}*/
		StartCoroutine(DisplayTotalDollar(temp, earnDollar));
		temp = null;
		temp1 = null;
		comp = null;

	}
	bool isFinish = false;

	IEnumerator DisplayTotalDollar(GameObject comp, int total){
		var temp = comp.transform.FindChild("score").gameObject;
		var score = temp.GetComponent<UILabel>() as UILabel;// = 250.ToString();
		score.text = "0";

		int mscore = 0;
		float x = (float)total * 0.02f;
		int delta = (int)x;
		while(true){
			if(isFinish) break;
			yield return null;
		}
		GameManager.instance.CountMoneyStart();
		while(true){
			mscore += delta;
			score.text = System.String.Format("{0:#,0}", mscore);
			if(mscore >= total)
			{
				GameManager.instance.StartCoroutine("GameFinishGUI");
				score.text = System.String.Format("{0:#,0}", total);
				GameManager.instance.CountMoneyFinish();
				temp = null;
				score = null;
				yield return new WaitForSeconds(1.0f);
				//Global.dollar += earnDollar;
				CreateMissionWindow();
				break;
			}
			yield return new WaitForSeconds(0.01f);
		}
		yield return null;
	}
	
}
