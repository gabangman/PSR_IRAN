using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ModeRewardWin : MonoBehaviour {
	System.Action resultRaceCallback;
	private int resultCardNumber_0, resultCardNumber_1;
	private int myRank = 0;
	void OnClosed(){
		if(resultRaceCallback == null) return;
		resultRaceCallback();
		resultRaceCallback = null;
		UserDataManager.instance.myGameDataSave();
	}
	
	public void CompensationDisable(GameObject obj, string RankName){
		obj.transform.parent.FindChild(RankName).gameObject.SetActive(false);
		obj.SetActive(false);
		
	}

	void OnClosed1(){
		GameManager.instance.btnLobbyToRace();
	}
	void missionResult(int missionindex){
		resultRaceCallback = () => {
			GameManager.instance.btnLobbyToRace();
		};

		for(int i = 2; i < transform.childCount;i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}

		var tr = transform.FindChild("Nut_Reward") as Transform;
		tr.gameObject.SetActive(true);

		if(missionindex < 10){
			tr.FindChild("G_BG").FindChild("lbText_1").GetComponent<UILabel>().text = KoStorage.GetKorString("76117");
			tr.FindChild("G_Result").FindChild("Nut").FindChild("Nut_gold").gameObject.SetActive(true);
		}else if(missionindex < 20){
			tr.FindChild("G_BG").FindChild("lbText_1").GetComponent<UILabel>().text = KoStorage.GetKorString("76116");
			tr.FindChild("G_Result").FindChild("Nut").FindChild("Nut_silver").gameObject.SetActive(true);
		}else{
			tr.FindChild("G_BG").FindChild("lbText_1").GetComponent<UILabel>().text = KoStorage.GetKorString("76115");
			tr.FindChild("G_Result").FindChild("Nut").FindChild("Nut_bronze").gameObject.SetActive(true);
		}

		tr.FindChild("BtnOK").gameObject.SetActive(true);
		tr.FindChild("BtnOK").GetComponent<UIButtonMessage>().functionName = "OnClosed1";
		GameManager.instance.playStarSound();
	}

	IEnumerator raceUpdateWindow(float rTime, int missionIndex){
		yield return new WaitForSeconds(2.0f);
		transform.FindChild("Weekly_Reward").gameObject.SetActive(false);
		var temp = transform.FindChild("RecUpdate") as Transform;
		temp.gameObject.SetActive(true);
		transform.FindChild("BtnOK").gameObject.SetActive(true);
		temp.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.getStringDic("73602");
		temp.FindChild("lbText_1").GetComponent<UILabel>().text = KoStorage.getStringDic("73603");
		//float rTime = 	Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
		temp.FindChild("G_Rec_New").GetComponentInChildren<UILabel>().text = 
			System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((rTime/60f)) ,rTime%60.0f);
		//float fTime = gameRank.instance.listfriend[Global.gRankMyrank].fTime;
		float fTime = myAccount.instance.account.weeklyRace.rTime;
		temp.FindChild("G_Rec_Old").GetComponentInChildren<UILabel>().text = 
			System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((fTime/60f)) ,fTime%60.0f);
		//gameRank.instance.listfriend[Global.gRankMyrank].fTime = rTime;
		myAccount.instance.account.weeklyRace.rTime = rTime;
		if(missionIndex <= 0){
			resultRaceCallback = () => {
				GameManager.instance.btnLobbyToRace();
			};
		}else{
			resultRaceCallback = () => {
				missionResult(missionIndex);
			};
		}
	}
	
	void OnChangeRecord(){
		var temp = transform.FindChild("RecUpdate") as Transform;
		temp.FindChild("LevelUp").gameObject.SetActive(true);
		//obj.transform.parent.FindChild("LevelUp").gameObject.SetActive(true);
	}
	void RaceStartSave(string name, int i, int j){
	//	if(string.Equals(GV.UserRevId,"97") == false) return;
	//	string str = "race_start_"+name+"  level_"+i.ToString()+"  exp_"+j.ToString()+"  time " + NetworkManager.instance.GetCurrentDeviceTime(); 
	//	GameManager.instance.writeStringToFile("gameStart", str);
	}

	void RaceEndSave(string name, int i, int j){
	//	if(string.Equals(GV.UserRevId,"97") == false) return;
	//string str = "race_end"+name+"  level_"+i.ToString()+"  exp_"+j.ToString()+"  time " + NetworkManager.instance.GetCurrentDeviceTime(); 
	//	GameManager.instance.writeStringToFile("gameEnd", str);
	}


	int RaceExp(int addExp){
		//int mExp = Global.Exp + Global.addExp;
		return Global.Exp;

	}

	int RaceLevel(){
		return Common_Exp_Range.levelCheck(Global.Exp);
	}
	public void NewInitialize(){
		GameObject temp = null;
		MainRaceType rType = Global.gRaceInfo.mType;
		Global.addBoost = 0;
		string raceAPI = string.Empty; int tempDollar = 0;
		string raceData = string.Empty;
		Dictionary<string, int> mDic = new Dictionary<string, int>();
		myRank = Base64Manager.instance.GlobalEncoding(Global.myRank,1);
		if(myRank == 0) Global.addExp = 10;
		else Global.addExp = 5;
		int missionIndex = 0;
		switch(rType){
		case MainRaceType.Champion:
		{
			temp = transform.FindChild("Champion_Reward").gameObject;
			var temp1 = transform.FindChild("Champion_Cutscene").gameObject as GameObject;
			StartCoroutine(ChampionResult(temp, temp1));
			
			mDic.Clear();
			if(Global.gRaceInfo.extraDollar == 0 ){
				mDic.Add("raceId",GV.ChSeasonID);
			//	Utility.LogWarning("raceId " + GV.ChSeasonID);
			}else{
				mDic.Add("raceId",Global.gRaceInfo.extraDollar);
			}
			int mBous = 0;
			if(myRank == 0){
				mDic.Add("rank",1);
				mBous = 0;
			}else{
				mDic.Add("rank",0);
				mBous = (int)Mathf.Round((float)Global.gRaceInfo.mode1Rw * 0.1f);
			}
			Global.isNetwork=  true;
			mDic.Add("perfect",0);
			mDic.Add("good",0);
			tempDollar = Base64Manager.instance.GlobalEncoding(Global.gBonus);
			tempDollar += (GameManager.instance.gDrillCount + GameManager.instance.gGearCount)*Base64Manager.instance.GlobalEncoding(Global.gScoreGood)
			               +(GameManager.instance.pDrillCount + GameManager.instance.pGearCount)*Base64Manager.instance.GlobalEncoding(Global.gScorePerfect);
			tempDollar += mBous;
			mDic.Add("driver",tempDollar );
			tempDollar = Base64Manager.instance.GlobalEncoding(Global.gRaceInfo.SponBouns);
			mDic.Add("sponsor",tempDollar);
			mDic.Add("level",RaceLevel());
			mDic.Add("exp",Global.Exp);

			float rTime = Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
			string str = "record;"+rTime.ToString();
			RaceStartSave("champ", Global.glevel, Global.Exp);
			raceAPI = ServerAPI.Get(90005); // "game/race/champ"
			NetworkManager.instance.HttpRaceFinishConnect("Put", mDic,  raceAPI, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					//response:{"state":0,"msg":"sucess","time":1442547997,"result":{"rewardCarId":0,"rewardCarClass":"","openRace":0,"dollar":425,"coin":0}}
					//클라이언트에서 테이블 기준으로 처리 함.
					Global.addLevel = thing["result"]["level"].AsInt;
					Global.glevel =thing["result"]["level"].AsInt;
				}else{
					Utility.LogError("Error " + status + "msg : " +thing["msg"] );
				}
				Global.isNetwork=  false;
				resultRaceCallback = () => {
					GameManager.instance.btnLobbyToRace();
				};

				RaceEndSave("champ", Global.glevel, Global.Exp);
			}, str);
			//G_DollarCoin G_Car, G_BG, G_Title1, G_Title2
			// G_Cha, G_Text, G_Result, G_BG
		}break;
		case MainRaceType.Club:
		{
			temp = transform.FindChild("Club_Reward").gameObject;
			temp.SetActive(true);
		
			ClubMatchResult(temp, myRank);
			mDic.Clear();
			raceAPI ="club/endClubRaceMain"; //ServerAPI.Get(90046);
			Global.isNetwork=  true;
			mDic.Add("clubMatchingIndex", CClub.mClubInfo.clubMatchIndex);
			string strData = "MainRaceClubIndex;"+Global.rankRaceIdx.ToString();
			if(myRank == 0){
				mDic.Add("mRank",1);
			}else mDic.Add("mRank",0);
			Global.addExp=0;
			tempDollar = Base64Manager.instance.GlobalEncoding(Global.gBonus);
			tempDollar += (GameManager.instance.gDrillCount + GameManager.instance.gGearCount)*Base64Manager.instance.GlobalEncoding(Global.gScoreGood)
				+(GameManager.instance.pDrillCount + GameManager.instance.pGearCount)*Base64Manager.instance.GlobalEncoding(Global.gScorePerfect);
			tempDollar += Base64Manager.instance.GlobalEncoding(Global.gRaceInfo.SponBouns);
			mDic.Add("bonus",tempDollar);
			mDic.Add("ThisRaceEarnedStarCount", CClub.mClubRaceStarCount);
			RaceStartSave("club", Global.glevel, Global.Exp);
			NetworkManager.instance.ClubBaseConnect("Post", mDic, raceAPI, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					if(myRank == 0){
						string strTeamID = GV.PlayTeamID.ToString();
						if(EncryptedPlayerPrefs.HasKey(strTeamID)){
							int a = EncryptedPlayerPrefs.GetInt(strTeamID);
							if(a >= CClub.mClubRaceStarCount){
							
							}else{
								EncryptedPlayerPrefs.SetInt(strTeamID, CClub.mClubRaceStarCount);
							}
						}else{
							EncryptedPlayerPrefs.SetInt(strTeamID, CClub.mClubRaceStarCount);
						}
					}
					CClub.mClubRaceCount = thing["clubraceCount"].AsInt;
					
				}else{
				
				}
				Global.isNetwork=  false;
				resultRaceCallback = () => {
					GameManager.instance.btnLobbyToRace();
				};
				RaceEndSave("club", Global.glevel, Global.Exp);
				temp.transform.FindChild("BtnOK").gameObject.SetActive(true);
			},strData);
		}
			break;
			
		case MainRaceType.mEvent:
		{
			temp = transform.FindChild("Event_Reward").gameObject;
			temp.SetActive(true);
			
			
			resultRaceCallback = ()=>{
				GameManager.instance.btnLobbyToRace();
			};
			//G_Flag / G_Title1, G_Result, G_Title2, G_BG, G_Reward_Dollar, G_Reward_Material, G_Reward_Cube
			string strRace = Global.gRaceInfo.eventModeName;
			GameObject tmp = null; int idx = 0;
			mDic.Clear();
			switch(strRace){
			case "New": //dollar // test Driver
				tmp = temp.transform.FindChild("G_Reward_Dollar").gameObject;
				idx =0;
				temp.transform.FindChild("BtnOK").gameObject.SetActive(true);
				raceAPI = ServerAPI.Get(90046);//"game/race/eventRace/testDrive";
				mDic.Add("level",RaceLevel());
				mDic.Add("exp",Global.Exp);
				break;
			case "Select": //material featured
				tmp = temp.transform.FindChild("G_Reward_Material").gameObject;
				idx =1;
				raceAPI = ServerAPI.Get(90044);// "game/race/eventRace/featured";
				Global.addExp = 0;
				break;
			case "Qube": //Qube
				idx =2;
				tmp = temp.transform.FindChild("G_Reward_Cube").gameObject;
				temp.transform.FindChild("BtnOK").gameObject.SetActive(true);
				raceAPI = ServerAPI.Get(90048);// "game/race/eventRace/evoCube";
				mDic.Add("level",RaceLevel());
				mDic.Add("exp",Global.Exp);
				break;
			}
			Global.isNetwork=  true;
			
			mDic.Add("raceId",GV.ChSeasonID);
			mDic.Add("raceIdx", Global.rankRaceIdx);
			if(myRank == 0){
				mDic.Add("rank",1);
			}else mDic.Add("rank",0);
			mDic.Add("perfect", 0);
			mDic.Add("good",0);
			tempDollar = Base64Manager.instance.GlobalEncoding(Global.gBonus);
			tempDollar += (GameManager.instance.gDrillCount + GameManager.instance.gGearCount)*Base64Manager.instance.GlobalEncoding(Global.gScoreGood)
				+(GameManager.instance.pDrillCount + GameManager.instance.pGearCount)*Base64Manager.instance.GlobalEncoding(Global.gScorePerfect);
			mDic.Add("driver",tempDollar);
			
			tempDollar = Base64Manager.instance.GlobalEncoding(Global.gRaceInfo.SponBouns);
			
			mDic.Add("sponsor",tempDollar);
			//	raceData ="raceData;"+ makeRaceData();
			//	float rTime = Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
			//	string str = "record;"+rTime.ToString();
			RaceStartSave("event", Global.glevel, Global.Exp);
			NetworkManager.instance.HttpRaceFinishConnect("Put", mDic, raceAPI, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					//response:{"state":0,"msg":"sucess","time":1445956698,"result":{"rankRaceIdx":"16"}}	
					mEventReward(tmp, idx);
					if(idx != 1){
						Global.addLevel = thing["result"]["level"].AsInt;
						Global.glevel =thing["result"]["level"].AsInt;
					}else {
						Global.addLevel = Global.level;
					}
					RaceEndSave("event", Global.glevel, Global.Exp);
				}else{
					Utility.LogError("Error " + status + "msg : " +thing["msg"] );
				}
				Global.isNetwork=  false;
				resultRaceCallback = () => {

					GameManager.instance.btnLobbyToRace();

				};
				GAchieve.instance.achieveInfo.PlusAchievement(16015);
			}, null);
			
			
			
			
			
		}
			break;
		case MainRaceType.Weekly:
		{
			missionIndex = QuestMission(3,myRank);
		

			temp = transform.FindChild("Weekly_Reward").gameObject;
			temp.SetActive(true);
			temp.transform.FindChild("BtnOK").gameObject.SetActive(true);
			RankingRaceResult(temp, missionIndex);
			
			mDic.Clear();
			mDic.Add("raceId",GV.ChSeasonID);
			mDic.Add("rankRaceIdx", Global.rankRaceIdx);
			mDic.Add("rank",(myRank+1));
			Global.isNetwork=  true;
			mDic.Add("perfect", 0);
			mDic.Add("good",0);
			tempDollar = Base64Manager.instance.GlobalEncoding(Global.gBonus);
			tempDollar += (GameManager.instance.gDrillCount + GameManager.instance.gGearCount)*Base64Manager.instance.GlobalEncoding(Global.gScoreGood)
				+(GameManager.instance.pDrillCount + GameManager.instance.pGearCount)*Base64Manager.instance.GlobalEncoding(Global.gScorePerfect);
			mDic.Add("driver",tempDollar);
			
			tempDollar = Base64Manager.instance.GlobalEncoding(Global.gRaceInfo.SponBouns);
			
			mDic.Add("sponsor",tempDollar);
			mDic.Add("level",RaceLevel());
			mDic.Add("exp",Global.Exp);
			raceData ="raceData;"+ makeRaceData();
			float rTime = Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
			string str = "record;"+rTime.ToString();
			raceAPI =  ServerAPI.Get(90024); // game/race/rank
			RaceStartSave("rank", Global.glevel, Global.Exp);
			NetworkManager.instance.HttpRaceFinishConnect("Put", mDic,  "game/race/rank", (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					//response:{"state":0,"msg":"sucess","time":1445956698,"result":{"rankRaceIdx":"16"}}	
					Global.addLevel = thing["result"]["level"].AsInt;
					Global.glevel =thing["result"]["level"].AsInt;
					RaceEndSave("rank", Global.glevel, Global.Exp);
				}else{
					Utility.LogError("Error " + status + "msg : " +thing["msg"] );
				}
				Global.isNetwork=  false;
				gameRank.instance.listRWT.Clear();
				gameRank.instance.listRWR.Clear();
				if(missionIndex <= 0){
					resultRaceCallback = () => {
						GameManager.instance.btnLobbyToRace();
					};
				}else{
					resultRaceCallback = () => {
						missionResult(missionIndex);
					};
				}
			
				GAchieve.instance.achieveInfo.PlusAchievement(16027);
			}, str,raceData);
			//G_MyScore, G_Title1, G_Result, G_BG, G_Title2, G_Time
		}
			break;
		case MainRaceType.Regular:
		{
			
			mDic.Clear();
			mDic.Add("raceId",GV.ChSeasonID);
			mDic.Add("raceIdx", Global.rankRaceIdx);
			
			if(Global.gRaceInfo.sType == SubRaceType.DragRace){
				temp = transform.FindChild("PVP_Reward").gameObject;
				temp.SetActive(true);
				raceAPI = ServerAPI.Get(90032);//  "game/race/regularDrag";
				if(myRank == 0){
					mDic.Add("rank",1);
				}else mDic.Add("rank",0);
				missionIndex = QuestMission(2,myRank);
			}else{
				temp = transform.FindChild("Regular_Reward").gameObject;
				temp.SetActive(true);
				raceAPI = ServerAPI.Get(90028);// "game/race/regularTrack";
				if(myRank == 0){
					mDic.Add("rank",1);
				}else mDic.Add("rank",0);
				missionIndex = QuestMission(1,myRank);
			}

			// 1 regular track , 2 regular drag , 3 rank race  , 4 pvp drag, 5pvp city
			Global.isNetwork=  true;
			mDic.Add("perfect", 0);
			mDic.Add("good",0);
			tempDollar = Base64Manager.instance.GlobalEncoding(Global.gBonus);
			tempDollar += (GameManager.instance.gDrillCount + GameManager.instance.gGearCount)*Base64Manager.instance.GlobalEncoding(Global.gScoreGood)
				+(GameManager.instance.pDrillCount + GameManager.instance.pGearCount)*Base64Manager.instance.GlobalEncoding(Global.gScorePerfect);
			mDic.Add("driver",tempDollar);
			tempDollar = Base64Manager.instance.GlobalEncoding(Global.gRaceInfo.SponBouns);
			mDic.Add("sponsor",tempDollar);
			mDic.Add("level",RaceLevel());
			mDic.Add("exp",Global.Exp);
			//	raceData ="raceData;"+ makeRaceData();
			//	float rTime = Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
			//	string str = "record;"+rTime.ToString();
			NetworkManager.instance.HttpRaceFinishConnect("Put", mDic,  raceAPI, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					
					Global.addLevel = thing["result"]["level"].AsInt;
					Global.glevel =thing["result"]["level"].AsInt;
					if(Global.gRaceInfo.sType == SubRaceType.DragRace){
						//	response:{"state":0,"msg":"sucess","time":1446140092,"result":{"raceIdx":"14","materialIds":[{"id":8614}]}}
						if(myRank == 0){
							resultCardNumber_0 = thing["result"]["materialIds"][0]["id"].AsInt;//:1453818096,"result":{"raceIdx":"34","materialIds":[{"id":8613}],"level":"1"}}
						}else 	resultCardNumber_0 = 0;
						
						
						SelectCardSlot(temp, 1);
					}else{
						temp.transform.FindChild("BtnOK").gameObject.SetActive(true);
						RegularTrackResult(temp,1);
					}
					RaceEndSave("regular", Global.glevel, Global.Exp);
				}else{
					resultCardNumber_0 = 0;
					Utility.LogError("Error " + status + "msg : " +thing["msg"] );
					
				}
				if(myRank == 0){
					GAchieve.instance.achieveInfo.PlusAchievement(16018);
				}
				
				Global.isNetwork=  false;
				if(missionIndex <= 0){
					resultRaceCallback = () => {
						GameManager.instance.btnLobbyToRace();
					};
				}else{
					resultRaceCallback = () => {
						missionResult(missionIndex);
					};
				}
			}, null,null);
			
			//G_Flag, G_Title1, G_Result, G_G_BG,G_Title2, G_Reward_Dollar
		}
			break;
		case MainRaceType.PVP:
		{
			
			mDic.Clear();
			mDic.Add("raceId",GV.ChSeasonID);
			mDic.Add("raceIdx", Global.rankRaceIdx);
			
			if(Global.gRaceInfo.sType == SubRaceType.DragRace){
				temp = transform.FindChild("Regular_Reward").gameObject;
				temp.SetActive(true);
				raceAPI = ServerAPI.Get(90036);//  "game/race/pvpDrag";
				if(myRank == 0){
					mDic.Add("rank",1);
					GAchieve.instance.achieveInfo.PlusAchievement(16009);
				}else mDic.Add("rank",0);
				missionIndex = QuestMission(4,myRank);
				
			}else{
				temp = transform.FindChild("PVP_Reward").gameObject;
				temp.SetActive(true);
				raceAPI = ServerAPI.Get(90040);// "game/race/pvpTimesquare";
				if(myRank == 0){
					mDic.Add("rank",1);
					GAchieve.instance.achieveInfo.PlusAchievement(16012);
				}else mDic.Add("rank",0);
				missionIndex = QuestMission(5,myRank);
			}
			
			Global.isNetwork=  true;
			mDic.Add("perfect", 0);
			mDic.Add("good",0);
			tempDollar = Base64Manager.instance.GlobalEncoding(Global.gBonus);
			tempDollar += (GameManager.instance.gDrillCount + GameManager.instance.gGearCount)*Base64Manager.instance.GlobalEncoding(Global.gScoreGood)
				+(GameManager.instance.pDrillCount + GameManager.instance.pGearCount)*Base64Manager.instance.GlobalEncoding(Global.gScorePerfect);
			mDic.Add("driver",tempDollar);
			tempDollar = Base64Manager.instance.GlobalEncoding(Global.gRaceInfo.SponBouns);
			
			mDic.Add("sponsor",tempDollar);
			mDic.Add("level",RaceLevel());
			mDic.Add("exp",Global.Exp);
			raceData ="raceData;"+ makeRaceData();
			RaceStartSave("pvp", Global.glevel, Global.Exp);
			//	float rTime = Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
			//	string str = "record;"+rTime.ToString();
			NetworkManager.instance.HttpRaceFinishConnect("Put", mDic,  raceAPI, (request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					Global.addLevel = thing["result"]["level"].AsInt;
					Global.glevel =thing["result"]["level"].AsInt;
					if(Global.gRaceInfo.sType == SubRaceType.DragRace){
						temp.transform.FindChild("BtnOK").gameObject.SetActive(true);
						RegularResult(temp,2);
					}else{
						//response:{"state":0,"msg":"sucess","time":1446140021,"result":{"raceIdx":"15","materialIds":[{"id":8603},{"id":8611}]}}
						if(myRank == 0){
							resultCardNumber_0 = thing["result"]["materialIds"][0]["id"].AsInt;
							resultCardNumber_1 = thing["result"]["materialIds"][1]["id"].AsInt;
						}else {
							resultCardNumber_0 = 0;
							resultCardNumber_1 = 0;
						}
						
						
						SelectCardSlot(temp, 2);
					}
					RaceEndSave("pvp", Global.glevel, Global.Exp);
				}else{
					resultCardNumber_0 = 0;
					resultCardNumber_1 = 0;
					Utility.LogError("Error " + status + "msg : " +thing["msg"] );
				}
				Global.isNetwork=  false;
				if(missionIndex <= 0){
					resultRaceCallback = () => {
						GameManager.instance.btnLobbyToRace();
					};
				}else{
					resultRaceCallback = () => {
						missionResult(missionIndex);
					};
				}
			}, null,raceData);
			
			
		}
			break;
		default:
		{
			//ischeck  = true;
		}break;
		}
		
	}
	private int QuestMission(int index, int myRank){
		int a = QuestData.instance.mQuest.mGQuest.raceMode;
		int ret = 0;
		if(a == index){
			if(myRank == 0){
				QuestData.instance.mQuest.mGQuest.TargetCount++;
				ret = 1;
			}else{
				if(a == 1 || a == 3){
					if(myRank <3){
						QuestData.instance.mQuest.mGQuest.TargetCount++;
						ret = 2;
					}else {
						ret = -1;
					}
				}else{
					ret = -1;
				}
			}
			return ret;
		}

		a =  QuestData.instance.mQuest.mSQuest.raceMode;
		if(a == index){
			if(myRank == 0){
				QuestData.instance.mQuest.mSQuest.TargetCount++;
				ret = 11;
			}else{
				if(a == 1 || a == 3){
					if(myRank <3){
						QuestData.instance.mQuest.mSQuest.TargetCount++;
						ret = 12;
					}else {
						ret = -11;
					}
				}else{
					ret = -11;
				}
			}
			return ret;
		}

		a =  QuestData.instance.mQuest.mBQuest.raceMode;
		if(a == index){
			if(myRank == 0){
				QuestData.instance.mQuest.mBQuest.TargetCount++;
				ret = 21;
			}else{
				if(a == 1 || a == 3){
					if(myRank <3){
						QuestData.instance.mQuest.mBQuest.TargetCount++;
						ret = 22;
					}else {
						ret = -21;
					}
				}else{
					ret = -21;
				}
			}
			return ret;
		}
		Debug.LogError("raceMode not matching");
		return -2;
	}
	private string makeRaceData(){
		//"raceData":"{carId:1000,teamId:10,carClass:3101,crewId:1200,sponId:1300,carAbility:260,crewAbility:328,userNick:e,userURL:,Torque:33.2,PitTime:16.87625,BSPower:16,BSTime:1.2,TireTime:1.909,GBLv:1.909,BSPressTime:1.909,fGear1:0.5,fGear2:0,fGear3:0,fGear4:0,fGear5:0,fGe","carId":1000,"carClass":3101}],"time":1446608955}
		string str = string.Empty;
		JSONObject obj = new JSONObject();
		obj.AddField("carId", GV.PlayCarID );
		obj.AddField("teamId",GV.SelectedTeamID);
		obj.AddField("carClass",GV.getTeamCarClassId(GV.SelectedTeamID));
		obj.AddField("level",Global.glevel);
		obj.AddField("crewId", GV.PlayCrewID);
		obj.AddField("sponId",GV.PlaySponID);

		obj.AddField("carAbility",GV.CarAbility);
		
		obj.AddField("crewAbility", GV.CrewAbility);
		obj.AddField("userNick",GV.UserNick);
		obj.AddField("userURL",GV.mUser.profileURL);
		
		float fDelta = Base64Manager.instance.getFloatEncoding(Global.gTorque, 0.001f);
		obj.AddField("Torque", fDelta);
		if(Global.gRaceInfo.sType == SubRaceType.DragRace){
			fDelta = 0.0f;
		}else{
			fDelta = Base64Manager.instance.getFloatEncoding(Global.PitInResutTime,0.000001f) ;
		}
		obj.AddField("PitTime",fDelta);
		
		fDelta = Base64Manager.instance.getFloatEncoding(Global.gBsPower, 0.001f);
		obj.AddField("BSPower",fDelta);
		fDelta = Base64Manager.instance.getFloatEncoding(Global.gBsTime, 0.001f);
		obj.AddField("BSTime",fDelta);
		fDelta = Base64Manager.instance.getFloatEncoding(Global.gTireDelay, 0.001f);
		obj.AddField("TireTime",fDelta);
		int id = 0;
		id = Global.gMyGearLv;
		obj.AddField("GBLv",id);
		id = Base64Manager.instance.GlobalEncoding(Global.PressBoostTime);
		obj.AddField("BSPressTime",id);
		string strName = null;
		for(int i = 1; i <= 20; i++){
			//	str += "fGear"+i.ToString()+":"+GameManager.instance.GearPress[(i-1)].ToString()+",";
			strName = "fG"+i.ToString();
			obj.AddField(strName,GameManager.instance.GearPress[(i-1)]); 
		}
		
		for(int i = 1; i <= 20; i++){
			//str += "pDelay"+i.ToString()+":"+GameManager.instance.gPressTime[(i-1)].ToString()+",";
			strName = "pD"+i.ToString();
			obj.AddField(strName,GameManager.instance.gPressTime[(i-1)]); 
		}
		fDelta = 	Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
		obj.AddField("raceTime",fDelta);
		if(CClub.ClanMember != 0){
			obj.AddField("ClubSymbol",CClub.mClubInfo.clubSymbol);
			obj.AddField("ClubName",CClub.mClubInfo.mClubName);
			obj.AddField("ClubLevel",CClub.mClubInfo.clubLevel);
		}
		str = obj.Print();
		Utility.LogWarning("raceData " + str);
		//	raceDatat {"carId":1002,"teamId":10,"carClass":3102,"crewId":1200,"sponId":1300,"carAbility":279,"crewAbility":328,"userNick":"w","userURL":"https://s3-ap-northeast-1.amazonaws.com/gabangman01/MultiPicture/MultiCom_1.png","Torque":38.2,"PitTime":16.28261,"BSPower":20,"BSTime":1.24,"TireTime":1.87,"GBLv":479507,"BSPressTime":38,"fG1":0,"fG2":1,"fG3":0,"fG4":0,"fG5":0,"fG6":0.6,"fG7":1,"fG8":0,"fG9":1,"fG10":1,"fG11":0,"fG12":0,"fG13":0,"fG14":0,"fG15":0,"fG16":0,"fG17":0,"fG18":0,"fG19":0,"fG20":0,"pD1":0,"pD2":0,"pD3":0,"pD4":0,"pD5":0,"pD6":0,"pD7":0,"pD8":0,"pD9":0,"pD10":0,"pD11":0,"pD12":0,"pD13":0,"pD14":0,"pD15":0,"pD16":0,"pD17":0,"pD18":0,"pD19":0,"pD20":0,"raceTime":78.64458}
		
		return str;
	}
	void RegularResult(GameObject obj, int mCount){
		if(mCount == 2){ // pvp_drag
			obj.transform.FindChild("G_Title1").FindChild("icon_title1").GetComponent<UISprite>().spriteName = "Mode_PVP";
			obj.transform.FindChild("G_BG").FindChild("lbText_1").GetComponent<UILabel>().text  = KoStorage.GetKorString("77407");
		}else{
			obj.transform.FindChild("G_BG").FindChild("lbText_1").GetComponent<UILabel>().text  = KoStorage.GetKorString("77407");
		}
		int rank =myRank;
		Transform objTemp = null;
		
		if(rank  == 0){
			objTemp= obj.transform.FindChild("G_Reward_Dollar").FindChild("Success") as Transform;
			objTemp.gameObject.SetActive(true);
			isCoinCount = false;
			StartCoroutine("DollarCounting", objTemp.FindChild("lbText_price").GetComponent<UILabel>());
			//	if(mCount ==1)	GAchieve.instance.achieveInfo.PlusAchievement(16018);
			//	else if(mCount == 2) GAchieve.instance.achieveInfo.PlusAchievement(16009);
		}else{
			objTemp = obj.transform.FindChild("G_Reward_Dollar").FindChild("Fail") as Transform;
			objTemp.gameObject.SetActive(true);
			objTemp.FindChild("lbText1").GetComponent<UILabel>().text = KoStorage.GetKorString("77308");
			objTemp.FindChild("lbText2").GetComponent<UILabel>().text =KoStorage.getStringDic("77406");
		}
		objTemp.gameObject.SetActive(true);
		
		if(mCount == 2){
			myAccount.instance.account.mRace.pvpdragPlay++;
			if(rank == 0) 	myAccount.instance.account.mRace.pvpdragWin++;
			float rTime = 	Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
			if(rTime < myAccount.instance.account.mRace.pvpdragTime){
				myAccount.instance.account.mRace.pvpdragTime = rTime;
			}
		}else{
			myAccount.instance.account.mRace.regularStockPlay++;
			if(rank == 0) 	myAccount.instance.account.mRace.regularStockWin++;
			float rTime = 	Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
			if(rTime < myAccount.instance.account.mRace.regularStockTime){
				myAccount.instance.account.mRace.regularStockTime = rTime;
			}
			
		}
		
		
		
		
	}
	
	
	void RegularTrackResult(GameObject obj, int mCount){
		obj.transform.FindChild("G_BG").FindChild("lbText_1").GetComponent<UILabel>().text  = KoStorage.GetKorString("77407");
		int rank = Base64Manager.instance.GlobalEncoding(Global.myRank,1);
		Transform objTemp = null;
		if(rank  <= 2){
			objTemp= obj.transform.FindChild("G_Reward_Dollar").FindChild("Success") as Transform;
			objTemp.gameObject.SetActive(true);
			isCoinCount = false;
			if(rank == 0) objTemp.FindChild("icon_Tropy1").GetComponent<UISprite>().spriteName = "Trophy_G";
			if(rank == 1) objTemp.FindChild("icon_Tropy1").GetComponent<UISprite>().spriteName = "Trophy_S";
			if(rank == 2) objTemp.FindChild("icon_Tropy1").GetComponent<UISprite>().spriteName = "Trophy_B";
			
			StartCoroutine("DollarCounting", objTemp.FindChild("lbText_price").GetComponent<UILabel>());
			//	if(mCount ==1)	GAchieve.instance.achieveInfo.PlusAchievement(16018);
			//	else if(mCount == 2) {}
		}else{
			objTemp = obj.transform.FindChild("G_Reward_Dollar").FindChild("Fail") as Transform;
			objTemp.gameObject.SetActive(true);
			objTemp.FindChild("lbText1").GetComponent<UILabel>().text = KoStorage.GetKorString("77308");
			objTemp.FindChild("lbText2").GetComponent<UILabel>().text =KoStorage.getStringDic("77406");
		}
		objTemp.gameObject.SetActive(true);
		myAccount.instance.account.mRace.regularStockPlay++;
		if(rank == 0) 	myAccount.instance.account.mRace.regularStockWin++;
		float rTime = 	Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
		if(rTime < myAccount.instance.account.mRace.regularStockTime){
			myAccount.instance.account.mRace.regularStockTime = rTime;
		}
	}
	
	
	void ClubMatchResult(GameObject obj, int mRank){
		var tr = obj.transform.FindChild("G_Result") as Transform;
		tr.GetChild(0).gameObject.SetActive(false);
		tr.GetChild(1).gameObject.SetActive(false);
		GameManager.instance.playStarSound();
		if(mRank == 0){
			tr.GetChild(0).gameObject.SetActive(true);
			tr.GetChild(0).FindChild("lbStarNum").GetComponent<UILabel>().text =string.Format("X{0}",CClub.mClubRaceStarCount);
		}else{
			tr.GetChild(1).gameObject.SetActive(true);
		}

		obj.transform.FindChild("G_BG").FindChild("lbText_1").GetComponent<UILabel>().text = KoStorage.GetKorString("77401");
	}
	
	void SelectCardSlot(GameObject obj, int mCount){
		GameManager.instance.playCardSound();
		if(mCount == 1){ // regular_drag
			//obj.transform.FindChild("G_Title1").FindChild("icon_title1").GetComponent<UISprite>().spriteName = "Mode_PVP";
		}
		matCnt = Global.gRaceInfo.rewardMatCount;
		int rank = myRank;
		GameObject objTemp = null;
		if(rank  == 0){
			objTemp = obj.transform.FindChild("G_Material_Success").gameObject as GameObject;
			objTemp.AddComponent<possessCardAction>();
			obj.transform.FindChild("G_Text_tip").GetComponentInChildren<UILabel>().text = 
				KoStorage.getStringDic("73052");//"TIP :  Random Text - Featured Mode";
			
			obj.transform.FindChild("G_Text").GetComponentInChildren<UILabel>().text = 
				string.Format(KoStorage.getStringDic("73051"), Global.gRaceInfo.rewardMatCount);
			myAcc.instance.account.bLobbyBTN[2] = true;
			myAcc.instance.account.bInvenBTN[1] = true;
			//	if(mCount == 2) GAchieve.instance.achieveInfo.PlusAchievement(16012);
			//	else if(mCount == 1) GAchieve.instance.achieveInfo.PlusAchievement(16018);
		}else{
			objTemp = obj.transform.FindChild("G_Material_Fail").gameObject as GameObject;
			obj.transform.FindChild("BtnOK").gameObject.SetActive(true);
			obj.transform.FindChild("G_Text_tip").GetComponentInChildren<UILabel>().text = 
				KoStorage.getStringDic("73054");
			obj.transform.FindChild("G_Text").GetComponentInChildren<UILabel>().text = 
				KoStorage.getStringDic("73053");
		}
		objTemp.SetActive(true);
		
		
		if(mCount == 2){
			myAccount.instance.account.mRace.pvpcityPlay++;
			if(rank == 0) 	myAccount.instance.account.mRace.pvpcityWin++;
			float rTime = 	Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
			if(rTime < myAccount.instance.account.mRace.pvpcityTime){
				myAccount.instance.account.mRace.pvpcityTime = rTime;
			}
			
		}else{
			myAccount.instance.account.mRace.regularDragPlay++;
			if(rank == 0) 	myAccount.instance.account.mRace.regularDragWin++;
			float rTime = 	Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
			if(rTime < myAccount.instance.account.mRace.regularDragTime){
				myAccount.instance.account.mRace.regularDragTime = rTime;
			}
		}
		
		
		//	var mat = obj.transform.FindChild("G_Card").gameObject;
		//	mat.AddComponent<possessCardAction>();
		
	}
	GameObject FeaturedCard;
	bool isClick = false;
	private int matCnt = 0;
	void OnSlotCard(GameObject slot){
		if(matCnt == 0) return;
		matCnt--;
		slot.GetComponent<UIButtonMessage>().functionName = null;
		var temp = transform.FindChild("PVP_Reward").gameObject as GameObject;
		
		if(resultCardNumber_0 == 0){
			temp.transform.FindChild("G_Material_Success").GetComponent<possessCardAction>().PlaySelectCardAnimation(slot, 8600+matCnt, matCnt);
			//if(matCnt == 0)	{}
			//temp.transform.FindChild("BtnOK").gameObject.SetActive(true);
			return;
		}
		int resultcardID =0;
		if(matCnt == 0){
			resultcardID = resultCardNumber_0;
		}else{
			resultcardID = resultCardNumber_1;
		}
		temp.transform.FindChild("G_Material_Success").GetComponent<possessCardAction>().PlaySelectCardAnimation(slot, resultcardID, matCnt);
		//	if(matCnt == 0)	 temp.transform.FindChild("BtnOK").gameObject.SetActive(true);
	}
	
	void ChampionEventWinNew(GameObject obj){
		UILabel[] tx = obj.transform.FindChild("G_Text").GetComponentsInChildren<UILabel>();
		obj.transform.FindChild("G_Cha").GetComponentInChildren<UISprite>().spriteName
			=Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[1].AICrewID).ToString()+"A";;
		int textNum = Random.Range(0,3);
		Global.gSeasonUp = 0;
		int rank =myRank;
		myAccount.instance.account.mRace.ChampionPlay++;
		if(rank == 0){
			myAccount.instance.account.mRace.ChampionWin++;
			textNum += 73137;
			tx[0].text = KoStorage.GetKorString(textNum.ToString());
			tx[1].text =null;
			//int mSeason = Base64Manager.instance.GlobalEncoding(Global.ChampionSeason);
			int mSeason = GV.ChSeasonID;
			if(mSeason != 6035){
				GV.ChSeasonID++;
				int mySeason = (GV.ChSeasonID-6000)/5 + 1;
				if(GV.ChSeason != mySeason){
					Global.gSeasonUp = 1;
					if(GV.ChSeason == 1) GAchieve.instance.achieveInfo.FinishAchievement(16006);
					else if(GV.ChSeason == 3) GAchieve.instance.achieveInfo.FinishAchievement(16007);
					else if(GV.ChSeason == 5) GAchieve.instance.achieveInfo.FinishAchievement(16008);
				}
				Global.gSeasonUp += 10;
				GV.gChamWin = 1;
				if(GV.ChSeasonID == 6010) myAcc.instance.account.bLobbyBTN[0] = true;
				if(GV.ChSeasonID == 6007) CClub.gReview = 2;
			
			}
			GameManager.instance.playWinSound();
			float rTime = 	Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
			if(rTime < myAccount.instance.account.mRace.ChampionTime){
				myAccount.instance.account.mRace.ChampionTime = rTime;
			}
		}else{
			GV.gChamWin = 0;
			textNum += 73140;
			obj.transform.FindChild("G_Result").GetComponentInChildren<UISprite>().spriteName = "YouLose";
			tx[0].text =KoStorage.GetKorString(textNum.ToString());
			tx[1].text= null;
			GameManager.instance.playLossSound();
			//Global.gSeasonUp = 0;
		}
		obj.transform.FindChild("BtnOK").gameObject.SetActive(true);
		
	}
	void ChampionClearWin(GameObject obj){
		UILabel[] tx = obj.transform.FindChild("G_Text").GetComponentsInChildren<UILabel>();
		obj.transform.FindChild("G_Cha").GetComponentInChildren<UISprite>().spriteName
			=Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[1].AICrewID).ToString()+"A";;
		Global.gSeasonUp = 0;
		GV.gChamWin = 0;
		int rank = myRank;
		int rand = Random.Range(0,2);
		int textNum = 0;
		//	myAccount.instance.account.mRace.ChampionPlay++;
		if(rank == 0){
			textNum = 73153+rand;
			tx[0].text = KoStorage.GetKorString(textNum.ToString());
			tx[1].text =null;
			GameManager.instance.playWinSound();
			//		float rTime = 	Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
			//		if(rTime < myAccount.instance.account.mRace.ChampionTime){
			//			myAccount.instance.account.mRace.ChampionTime = rTime;
			//		}
			//		myAccount.instance.account.mRace.ChampionWin++;
		}else{
			textNum = 73155+rand;
			obj.transform.FindChild("G_Result").GetComponentInChildren<UISprite>().spriteName = "YouLose";
			tx[0].text = KoStorage.GetKorString(textNum.ToString());
			tx[1].text= null;
			GameManager.instance.playLossSound();
		}
		obj.transform.FindChild("BtnOK").gameObject.SetActive(true);
		
	}
	void mEventReward(GameObject obj, int idx){
		obj.SetActive(true);
		int rank = myRank;
		GameObject temp = null;
		if(rank == 0){
			temp = obj.transform.FindChild("Success").gameObject;
			if(idx == 0){
				temp.transform.FindChild("lbText_price").GetComponent<UILabel>().text
					= string.Format("X{0}", Global.gRaceInfo.mode1Rw);
				GV.myDollar += Global.gRaceInfo.mode1Rw;
				GV.updateDollar -= Global.gRaceInfo.mode1Rw;
				myAccount.instance.account.mRace.testdriveWin++;
			}else if(idx ==1){
				
				temp.transform.FindChild("SelectMat").gameObject.SetActive(true);
				temp.GetComponent<SelectMat>().SelectMaterialWindow();
				myAccount.instance.account.mRace.featureRaceWin++;
			}else {
				temp.transform.FindChild("lbText_price").GetComponent<UILabel>().text
					= string.Format("X{0}", Global.gRaceInfo.rewardMatCount);
				temp.transform.FindChild("CubeName").GetComponent<UILabel>().text
					= string.Format("{0}", Common_Material.Get(8620).Name);
				GV.UpdateMatCount(8620,1);
				myAccount.instance.account.mRace.evoWin++;
				myAccount.instance.account.eRace.EvoAcquistCount =1 ;
				myAcc.instance.account.bInvenBTN[2] = true;
				myAcc.instance.account.bLobbyBTN[2] = true;
			}
		}else{
			temp = obj.transform.FindChild("Fail").gameObject;
			if(idx == 1){
				temp.transform.FindChild("lbText").GetComponent<UILabel>().text =
					KoStorage.getStringDic("73053");
				obj.transform.parent.FindChild("BtnOK").gameObject.SetActive(true);
			}else {
				temp.transform.FindChild("lbText").GetComponent<UILabel>().text =
					KoStorage.getStringDic("77406");
			}
		}
		
		float rTime = 	Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
		
		if(idx == 0){
			if(rTime < myAccount.instance.account.mRace.testdriveTime){
				myAccount.instance.account.mRace.testdriveTime = rTime;
			}
			myAccount.instance.account.mRace.testdrivePlay++;
		}else if(idx == 1){
			if(rTime < myAccount.instance.account.mRace.featureRaceTime){
				myAccount.instance.account.mRace.featureRaceTime = rTime;
			}
			myAccount.instance.account.mRace.featureRacePlay++;
		}else {
			if(rTime < myAccount.instance.account.mRace.evoTime){
				myAccount.instance.account.mRace.evoTime = rTime;
			}
			myAccount.instance.account.mRace.evoPlay++;
		}
		
		temp.SetActive(true);
		
	}
	/*
	void WeeklyResult(GameObject weekly){
		var temp = weekly.transform.FindChild("G_Result")	as Transform;
		int[] rank = new int[3];
		int mrank =myRank;
		if(mrank < 3){
			string _sprite = string.Empty;
			if(mrank == 0) {_sprite = "Trophy_G"; rank[0] = 1; rank[1]= 0; rank[2]=0; GAchieve.instance.achieveInfo.PlusAchievement(16030);}
			else if(mrank == 1){_sprite = "Trophy_S";rank[0] = 0; rank[1]= 1; rank[2]=0;GAchieve.instance.achieveInfo.PlusAchievement(16033);}
			else if(mrank == 2) {_sprite = "Trophy_B";rank[0] = 0; rank[1]= 0; rank[2]=1;GAchieve.instance.achieveInfo.PlusAchievement(16036);}
			var re = temp.transform.FindChild("icon_tropy").gameObject;//.SetActive(true);
			re.GetComponent<UISprite>().spriteName = _sprite;
			re.SetActive(true);
			GameManager.instance.playWinSound();
			temp.FindChild("lbText_Fail").gameObject.SetActive(false);
		}else{
			rank[0] = 0; rank[1]= 0; rank[2]=0;
			var re = temp.transform.FindChild("lbText_Fail").gameObject;//.SetActive(true);
			re.GetComponent<UILabel>().text = KoStorage.GetKorString("73605");// TableManager.ko.dictionary["60193"].String;
			re.SetActive(true);
			GameManager.instance.playLossSound();
			temp.FindChild("icon_tropy").gameObject.SetActive(false);
		}
		weekly.transform.FindChild("G_BG").GetComponentInChildren<UILabel>().text = 
			KoStorage.GetKorString("73607");
		float rTime = 	Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
		weekly.transform.FindChild("G_Time").GetComponentInChildren<UILabel>().text = 
			System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((rTime/60f)) ,rTime%60.0f);
		//!!--Utility.Log("rTime " + rTime);
		
		if(gameRank.instance.listfriend.Count == 0) return;
		temp = weekly.transform.FindChild("G_MyScore");
		temp.FindChild("lbText_N_1").GetComponent<UILabel>().text =
			string.Format("{0}", (rank[0]));
		temp.FindChild("lbText_N_2").GetComponent<UILabel>().text =
			string.Format("{0}", (rank[1]));
		temp.FindChild("lbText_N_3").GetComponent<UILabel>().text =
			string.Format("{0}", (rank[2]));
	}*/
	
	void RankingRaceResult(GameObject weekly, int missionindex){
		SubRaceType mType = Global.gRaceInfo.sType;
		switch(mType){
		case SubRaceType.DragRace:
			break;
		case SubRaceType.RegularRace:
			break;
		case SubRaceType.CityRace:
			break;
		}
		
		var temp = weekly.transform.FindChild("G_Result")	as Transform;
		myAccount.instance.account.weeklyRace.WeeklyPlayCount++;
		int[] rank = new int[3];
		int mrank = myRank;
		if(mrank < 3){
			string _sprite = string.Empty;
			if(mrank == 0) {_sprite = "Trophy_G"; rank[0] = 1; rank[1]= 0; rank[2]=0;
				myAccount.instance.account.weeklyRace.GTrophy++;
				if(myAccount.instance.account.weeklyRace.GTrophy == 1) CClub.gReview = 1;
				SNSManager.OnRBRankingThropy(myAccount.instance.account.weeklyRace.GTrophy);
				GAchieve.instance.achieveInfo.PlusAchievement(16030);
			}
			else if(mrank == 1){_sprite = "Trophy_S";rank[0] = 0; rank[1]= 1; rank[2]=0;myAccount.instance.account.weeklyRace.STrophy++;
				GAchieve.instance.achieveInfo.PlusAchievement(16033);
			}
			else if(mrank == 2) {_sprite = "Trophy_B";rank[0] = 0; rank[1]= 0; rank[2]=1;myAccount.instance.account.weeklyRace.BTrophy++;
				GAchieve.instance.achieveInfo.PlusAchievement(16036);
			}
			var re = temp.transform.FindChild("icon_tropy").gameObject;//.SetActive(true);
			re.GetComponent<UISprite>().spriteName = _sprite;
			re.SetActive(true);
			GameManager.instance.playWinSound();
			temp.FindChild("lbText_Fail").gameObject.SetActive(false);
		}else{
			rank[0] = 0; rank[1]= 0; rank[2]=0;
			var re = temp.transform.FindChild("lbText_Fail").gameObject;//.SetActive(true);
			re.GetComponent<UILabel>().text = KoStorage.GetKorString("73605");// TableManager.ko.dictionary["60193"].String;
			re.SetActive(true);
			GameManager.instance.playLossSound();
			temp.FindChild("icon_tropy").gameObject.SetActive(false);
		}
		weekly.transform.FindChild("G_BG").GetComponentInChildren<UILabel>().text =KoStorage.GetKorString("73607");// "트로픽 획득 ??";
		float rTime = 	Base64Manager.instance.getFloatEncoding(Global.RaceResutTime,0.000001f) ;
		weekly.transform.FindChild("G_Time").GetComponentInChildren<UILabel>().text = 
			System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((rTime/60f)) ,rTime%60.0f);
		
		if(rTime <= myAccount.instance.account.weeklyRace.rTime){
			if(myAccount.instance.account.weeklyRace.rTime < 0.0f){
				myAccount.instance.account.weeklyRace.rTime = rTime;
			}else{
				StartCoroutine(raceUpdateWindow(rTime,missionindex));
			}
		}
		
		SNSManager.OnRBRankingTime(rTime);
		temp = weekly.transform.FindChild("G_MyScore");
		temp.FindChild("lbText_N_1").GetComponent<UILabel>().text =
			string.Format("{0}", ( myAccount.instance.account.weeklyRace.GTrophy));
		temp.FindChild("lbText_N_2").GetComponent<UILabel>().text =
			string.Format("{0}", (myAccount.instance.account.weeklyRace.STrophy));
		temp.FindChild("lbText_N_3").GetComponent<UILabel>().text =
			string.Format("{0}", (myAccount.instance.account.weeklyRace.BTrophy));

		
		if(mrank == 0){
			int cnt = myAccount.instance.account.weeklyRace.GTrophy;
			/*switch(cnt){
			case 50: //25코인
				mPost.nReCount = 50;
				myAccount.instance.account.weeklyRace.Trophy_Reset_Times = false;
				break;
			case 100: //50코인
				mPost.nReCount = 110;
				break; 
			case 200: // 100
				mPost.nReCount = 240;
				break;
			case 300: //150
			//	GV.myCoin += 150;
			//	GV.updateCoin = -150;
				mPost.nReCount = 390;
				break;
			case 400: // 300
				mPost.nReCount = 560;
				break;
			case 500:{ // 500
				mPost.nReCount = 750;
				myAccount.instance.account.weeklyRace.GTrophy = 0;
				myAccount.instance.account.weeklyRace.STrophy = 0;
				myAccount.instance.account.weeklyRace.BTrophy = 0;
				myAccount.instance.account.weeklyRace.Trophy_Reset_Times = true;
			}break;
			default:
				break;
			}
			bool b = false;
			int reCount = 0;
			switch(cnt){
			case 2: //25코인
				reCount = 50;
				myAccount.instance.account.weeklyRace.Trophy_Reset_Times = false;
				break;
			case 4: //50코인
				reCount= 110;
				break; 
			case 6: // 100
				reCount = 240;
				break;
			case 8: //150
				//	GV.myCoin += 150;
				//	GV.updateCoin = -150;
				reCount = 390;
				break;
			case 10: // 300
				reCount= 560;
				break;
			case 12:{ // 500
				reCount= 750;
				myAccount.instance.account.weeklyRace.GTrophy = 0;
				myAccount.instance.account.weeklyRace.STrophy = 0;
				myAccount.instance.account.weeklyRace.BTrophy = 0;
				myAccount.instance.account.weeklyRace.Trophy_Reset_Times = true;
			}break;
			default:
				b = true;
				break;
			}
			if(!b){
				Account.PostList mPost = new Account.PostList();
				mPost.nType = 0;
				int mIndex = 	myAccount.instance.account.listPost.Count;
				mPost.index = mIndex++;
				mPost.nReCount = reCount;
				mPost.receiveTime = NetworkManager.instance.GetCurrentDeviceTime().Ticks;//System.DateTime.Now.Ticks;
				myAccount.instance.account.listPost.Add(mPost);
				Global.gNewMsg = mPost.index;
				mPost = null;
			}*/
			
			
		}
		
	}
	
	
	
	
	IEnumerator ChampionResult(GameObject result, GameObject CutScene){
		if(Global.gRaceInfo.mode1Rw != 0){
			result.SetActive(true);
			isFinish = true;
			var tr = result.transform.FindChild("G_DollarCoin") as Transform;
			var lbCoin = tr.FindChild("lbText_Coin").GetComponent<UILabel>() as UILabel;
			yield return new WaitForSeconds(0.55f);
			StartCoroutine("CoinCounting", lbCoin);
			var lbCoin1 = tr.FindChild("lbText_Dollar").GetComponent<UILabel>() as UILabel;
			StartCoroutine("DollarCounting", lbCoin1);
			while(isFinish){
				yield return null;
			}
			result.SetActive(false);
			CutScene.SetActive(true);
			ChampionEventWinNew(CutScene);
		}else{
			CutScene.SetActive(true);
			ChampionClearWin(CutScene);
			yield return null;
		}
	}
	bool isCoinCount, isFinish;
	IEnumerator CoinCounting(UILabel score){
		score.text = "0";
		isCoinCount = true;
		int cnt = 0;
		int rank = myRank;
		if(rank != 0){
			isCoinCount = false;
			yield break;
		}
		while(true){
			cnt++;
			if(cnt <  Global.gRaceInfo.extraCoin){
				score.text = cnt.ToString();
				GameManager.instance.CountCoinStart();
			}else{
				score.text  = Global.gRaceInfo.extraCoin.ToString();
				isCoinCount = false;
				GV.myCoin += Global.gRaceInfo.extraCoin;
				GV.updateCoin -= Global.gRaceInfo.extraCoin;
				yield break;
			}
			yield return new WaitForSeconds(0.08f);
		}
	}
	
	IEnumerator DollarCounting(UILabel score){
		score.text = "0";
		int rank =myRank;
		int total = 0;
		if(rank == 0) {
			total = (int)Mathf.Round((float)Global.gRaceInfo.mode1Rw * 1.0f);
			GV.myDollar += Global.gRaceInfo.mode1Rw;
			GV.updateDollar -= Global.gRaceInfo.mode1Rw;
		}else if(rank == 1){
			if(Global.gRaceInfo.mType == MainRaceType.Champion){
				total = (int)Mathf.Round((float)Global.gRaceInfo.mode1Rw * 0.1f);
				GV.myDollar += total;
				GV.updateDollar -= total;
			}else{
				//total = (int)Mathf.Round((float)Global.gRaceInfo.mode1Rw * 0.5f);
				total = GV.gEntryFee*2;
				GV.myDollar += total;
				GV.updateDollar -= total;
			}
		}else if(rank == 2){
			//total = (int)Mathf.Round((float)Global.gRaceInfo.mode1Rw * 0.1f);
			total = GV.gEntryFee*1;
			GV.myDollar += total;
			GV.updateDollar -= total;
		}else{
			
		}
		while(isCoinCount){
			yield return null;
		}
		
		int mscore = 0;
		float x = (float)total * 0.02f;
		int delta = (int)x;
		
		GameManager.instance.CountMoneyStart();
		while(true){
			mscore += delta;
			score.text = System.String.Format("{0:#,0}", mscore);
			if(mscore >= total)
			{
				GameManager.instance.StartCoroutine("GameFinishGUI");
				score.text = System.String.Format("{0:#,0}", total);
				GameManager.instance.CountMoneyFinish();
				yield return new WaitForSeconds(1.0f);
				isFinish = false;
				
				yield break;
			}
			yield return new WaitForSeconds(0.01f);
		}
	}
	
	void Start(){
		GameManager.instance.RewardSound();
	}
}
