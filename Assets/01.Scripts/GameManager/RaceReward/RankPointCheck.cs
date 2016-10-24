using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RankPointCheck : MonoBehaviour {

	System.Action<bool, string> CallBack;
	System.Action<bool> CallMiddleBack;

	public void InPitInSprite(GameObject obj, System.Action<bool> InPitInCallback){
		this.CallMiddleBack = InPitInCallback;
		var rank1 = ObjectManager.GetRaceObject("Race", "MiddleRank") as GameObject;
		ObjectManager.ChangeObjectParent(rank1,  obj.transform);
		ObjectManager.ChangeObjectPosition(rank1, Vector3.zero, Vector3.one, Vector3.zero);
		rank1.SetActive(true);
		StartCoroutine("RankOut",rank1);
	}


	public void MiddleSprite(GameObject obj, int length, System.Action<bool> CallMiddleBack){
		this.CallMiddleBack = CallMiddleBack;
		//var rank1 = ObjectManager.CreatePrefabs(obj.transform, "Race", "MiddleRank") as GameObject;
		var rank1 = ObjectManager.GetRaceObject("Race", "MiddleRank") as GameObject;
		ObjectManager.ChangeObjectParent(rank1,  obj.transform);
		ObjectManager.ChangeObjectPosition(rank1, Vector3.zero, Vector3.one, Vector3.zero);
		rank1.SetActive(true);
		StartCoroutine("RankOut",rank1);
	}

	IEnumerator RankOut(GameObject tempRank){
		yield return new WaitForSeconds(1.5f);
		yield return new WaitForSeconds(1.5f);
		tempRank.AddComponent<TweenAction>().ReverseTween(tempRank);
		yield return new WaitForSeconds(2.0f);
		CallMiddleBack(true);
		Destroy(this);
		tempRank.SetActive(false);
	}

	/*
	public void StartMiddleRank(GameObject obj, int length, System.Action<bool> CallMiddleBack){
		GameObject[]  tempRank= new GameObject[length];
		this.CallMiddleBack = CallMiddleBack;
		for(int i= 0; i < length; i++){
			//var rank1 = ObjectManager.CreatePrefabs(obj.transform, "Race", "Rank") as GameObject;
			var rank1 = ObjectManager.GetRaceObject( "Race", "Rank") as GameObject;

			ObjectManager.ChangeObjectParent(rank1,  obj.transform);
			ObjectManager.ChangeObjectPosition(rank1, Vector3.zero, Vector3.one, Vector3.zero);
			rank1.SetActive(true);
			tempRank[i] = rank1;
			var g = rank1.transform.FindChild("lbRank").gameObject ;// as UILabel;
			g.GetComponent<UILabel>().text =  (i+1).ToString();
			if( Global.Race01Ranking[i] == "M_R1"){
				rank1.transform.FindChild("myRank").gameObject.SetActive(true);
				rank1.transform.FindChild("CarThumbnail").GetComponent<UISprite>().spriteName
					= GameManager.instance.spriteName[i];
			}else{
				rank1.transform.FindChild("CarThumbnail").GetComponent<UISprite>().spriteName
					= GameManager.instance.spriteName[i];
			}
			rank1.transform.FindChild("lbName").gameObject.GetComponent<UILabel>().text = Global.Race01Ranking[i];
			rank1.transform.FindChild("lbTime").gameObject.GetComponent<UILabel>().text =
				System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((Global.Race01Time[i]/60f)) ,Global.Race01Time[i]%60.0f);
			
			Vector3 to = new Vector3(0, 150-(i*100),0);
			Vector3 from = new Vector3(-1000, 150-(i*100),0);
			rank1.AddComponent<TweenAction>().RankTween(0.2f*i, 0.4f, from, to);
		}
		StartCoroutine("RankOut",tempRank);
	}

	IEnumerator RankOut(GameObject[] tempRank){
		yield return new WaitForSeconds(1.5f);
		for(int i =0 ; i < tempRank.Length ; i++){
			Vector3 from =new Vector3(0, 150-(i*100),0);
			Vector3 to = new Vector3(-1000, 150-(i*100),0);
			tempRank[i].AddComponent<TweenAction>().RankTween(0.1f*i, 0.4f, from, to);
		}
		yield return new WaitForSeconds(2.0f);
		CallMiddleBack(true);
		Destroy(this);
	}

*/
	public void StartFinalRank(GameObject obj, int length, System.Action<bool, string> CallBack){
		GameObject[]  tempRank= new GameObject[length];
		this.CallBack = CallBack;
		string rankText = string.Empty;
		string trophy_icon = string.Empty;
		int intervalY = 100; // 5player
		int intervalYinit = 150;
		/*if(Global.gRaceInfo.gameType == RaceGameType.RankingRace) {
			if(Global.gRaceInfo.modeType == RaceModeType.DragMode ||
			   Global.gRaceInfo.modeType == RaceModeType.EventMode){
				intervalY = 120;intervalYinit = 100; // 2 player 
			}else{
				intervalY = 70;intervalYinit = 180; // 7player
			}
		
		}else if(Global.gRaceInfo.gameType == RaceGameType.ChampionRace){
			intervalY = 120;intervalYinit = 100; //2 player
		}*/
		if(Global.gRaceInfo.mType == MainRaceType.Weekly) {
				intervalY = 70;intervalYinit = 180; // 7player
		}else if(Global.gRaceInfo.mType == MainRaceType.Champion){
				intervalY = 120;intervalYinit = 100; //2 player
		}else{
			if(Global.gRaceInfo.sType != SubRaceType.RegularRace){
				intervalY = 120;intervalYinit = 100;
			}else if(Global.gRaceInfo.mType == MainRaceType.mEvent && Global.gRaceInfo.eventModeName == "Qube"){
				intervalY = 70;intervalYinit = 180; // 7player
			}
		}

		for(int i= 0; i < length; i++){
			rankText = "Rank"+(i+1);
			var rank1 = ObjectManager.GetRaceObject("Race", rankText) as GameObject;
			rank1.transform.parent = obj.transform;
			rank1.transform.localScale = Vector3.one;
			rank1.SetActive(true);
			tempRank[i] = rank1;
			TweenPosition tw = rank1.GetComponent<TweenPosition>();
			float range = intervalYinit- i*intervalY;
			tw.from = new Vector3(-800,range,0);
			tw.to = new Vector3(0,range,0);
			tw.delay = 0.1f*i;
			tw.duration = 0.4f;
			tw.enabled = true;
			var labeltime = rank1.transform.FindChild("lbTime").gameObject as GameObject;
			if(Global.gRaceInfo.mType != MainRaceType.Weekly){
				labeltime.GetComponent<UILabel>().text =
					System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((Global.Race02Time[i]/60f)) ,Global.Race02Time[i]%60.0f);
			}else{
				if(Global.gRaceInfo.sType != SubRaceType.RegularRace){
					labeltime.GetComponent<UILabel>().text =
						System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((Global.Race02Time[i]/60f)) ,Global.Race02Time[i]%60.0f);
				}else{
					if(i <3){
						labeltime.GetComponent<UILabel>().text =
							System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((Global.Race02Time[i]/60f)) ,Global.Race02Time[i]%60.0f);
					}else{
						if(Global.Race02Time[i] <= 10 || Global.Race02Time[i] > 200){
							labeltime.GetComponent<UILabel>().color = new Color32(227,52,0,255);
							labeltime.GetComponent<UILabel>().text = "RETIRE";
						}else{
							labeltime.GetComponent<UILabel>().text =
								System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((Global.Race02Time[i]/60f)) ,Global.Race02Time[i]%60.0f);
						}
					}
				}

			
			}
			string spriteName = GameManager.instance.rankSpritNames[i];
			if(string.IsNullOrEmpty(spriteName) == true){
				spriteName = "1010";
			}
			rank1.transform.FindChild("CarThumbnail").GetComponent<UISprite>().spriteName = spriteName;
			if(Global.Race02Ranking[i] == "M_R1"){
				Global.myRank = Base64Manager.instance.GlobalEncoding(i,1);
				rank1.transform.FindChild("myRank").gameObject.SetActive(true);
				rank1.transform.FindChild("winPanel").gameObject.SetActive(true);
				rank1.transform.FindChild("myRank").gameObject.SetActive(true);
				RaceTypeRankChange(rank1, 10);
			}else{
				string name = string.Empty;
				int aiID = 0;
				try{
					name =Global.Race02Ranking[i];
					aiID  = int.Parse(name[1].ToString());
					aiID--;
				}catch(System.NullReferenceException e){
					aiID = 0;
				}
				RaceTypeRankChange(rank1, aiID);
			}
			rankText = string.Empty;
			trophy_icon = string.Empty;
			if(i == 0){
				trophy_icon = "Trophy_G";
				rank1.transform.FindChild("Trophy_icon").GetComponent<UISprite>().spriteName = trophy_icon;
			}else if(i == 1){
				trophy_icon = "Trophy_S";
				rank1.transform.FindChild("Trophy_icon").GetComponent<UISprite>().spriteName = trophy_icon;
			}else if(i == 2){
				trophy_icon = "Trophy_B";
				rank1.transform.FindChild("Trophy_icon").GetComponent<UISprite>().spriteName = trophy_icon;
			}else{
				rankText =( i+1).ToString();
				rank1.transform.FindChild("Trophy_icon").gameObject.SetActive(false);
			}
			rank1.transform.FindChild("lbRank").GetComponent<UILabel>().text = rankText;
			rank1 = null;
		}

		rankText = null;

		//Utility.Log(Global.myRank + "  myRank");
		StartCoroutine("FinalRankOut", tempRank);
	}

	IEnumerator FinalRankOut(GameObject[] tempRank){
		yield return new WaitForSeconds(3.0f);
		int carlength = tempRank.Length;
		string name = string.Empty;
		int rank = Base64Manager.instance.GlobalEncoding(Global.myRank,1);
		Vector3 tempos = new Vector3(0,180,0);
		TweenPosition tween = null;
		for(int i = 0; i < carlength; i++){
			if(rank == i){
				if(rank == 0){
					if(Global.gRaceInfo.mType == MainRaceType.Champion){
						tween = tempRank[i].GetComponent<TweenPosition>();
						 tempos = tempRank[i].transform.localPosition;
						tween.from = tempos;
						tween.to = new Vector3(0,180,0);
						tween.eventReceiver = null;
						tween.callWhenFinished = null;
						tween.Reset();
						tween.enabled = true;
						tween = null;
					}else{
						if(Global.gRaceInfo.sType != SubRaceType.RegularRace){
							tween = tempRank[i].GetComponent<TweenPosition>();
							tempos = tempRank[i].transform.localPosition;
							tween.from = tempos;
							tween.to = new Vector3(0,180,0);
							tween.eventReceiver = null;
							tween.callWhenFinished = null;
							tween.Reset();
							tween.enabled = true;
							tween = null;
						}
					}
				}else{
					tween = tempRank[i].GetComponent<TweenPosition>();
					 tempos = tempRank[i].transform.localPosition;
					tween.from = tempos;
					tween.to = new Vector3(0,160,0);
					tween.eventReceiver = null;
					tween.callWhenFinished = null;
					tween.Reset();
					tween.enabled = true;
					tween = null;
				}
				name = tempRank[i].name;
				var youObj = tempRank[i].transform.FindChild("myRank").gameObject;
				youObj.SetActive(false);
				youObj = null;
			}else{
				tween = tempRank[i].GetComponent<TweenPosition>();
				tempos = tempRank[i].transform.localPosition;
				tween.from = tempos;
				tween.to = new Vector3(-1000,tempos.y,0);
				tween.eventReceiver = tempRank[i];
				tween.callWhenFinished = "selfDestroy";
				tween.Reset();
				tween.enabled = true;
				tween = null;
			}
		}
		yield return new WaitForSeconds(0.5f);
		CallBack(true,name);
		Destroy(this);
	}

	void RaceTypeRankChange(GameObject rank, int id){
		bool b = false;
		string crewName = string.Empty;
		crewName = GV.PlayCrewID.ToString()+"A";
		/*
		RaceType rType = Global.gRaceInfo.raceType;
		switch(rType){
		case RaceType.ChampionMode:
			//crewName = Global.MyCrewID.ToString()+"A"; 
			break;
		case RaceType.DailyMode:
			int tempid = Base64Manager.instance.GlobalEncoding(Global.gDailyCrewID);
			crewName = tempid.ToString()+"A";
			break;
		case RaceType.FeatureEvent:
			//crewName = Global.MyCrewID.ToString()+"A"; 
			break;
		case RaceType.RegularMode:
			b = true;
			break;
		case RaceType.WeeklyMode:
			b = true;
			break;
		default:
			break;
		}*/
		MainRaceType rType = Global.gRaceInfo.mType;
		switch(rType){
		case MainRaceType.Champion:
			//crewName = Global.MyCrewID.ToString()+"A"; 
			break;
		case MainRaceType.mEvent:
			//int tempid = Base64Manager.instance.GlobalEncoding(Global.gDailyCrewID);
			//crewName = tempid.ToString()+"A";
			b = true;
			break;
		case MainRaceType.Club:
			//crewName = Global.MyCrewID.ToString()+"A"; 
			break;
		case MainRaceType.Regular:
			b = true;
			break;
		case MainRaceType.Weekly:
			b = true;
			break;
		case MainRaceType.PVP:
			b = true;
			break;
		default:
			break;
		}
		var temp = rank.transform.FindChild("lbName") as Transform;
		var temp1 = rank.transform.FindChild("Kakao_icon") as Transform;
		var temp2 = rank.transform.FindChild("sprite_icon") as Transform;
		string nick = string.Empty;
		Texture2D myPic  = null;

		if(id == 10){
			nick = GV.UserNick;
			myPic = AccountManager.instance.myPicture;// Global.myProfile;
				temp.gameObject.SetActive(true);
				temp.GetComponent<UILabel>().text = nick;
				temp1.gameObject.SetActive(true);
				temp1.GetComponent<UITexture>().mainTexture 
					= myPic;
				temp2.gameObject.SetActive(true);
				temp2.GetComponent<UISprite>().spriteName = crewName;
		}else{
			nick = Global.gAICarInfo[id].userNick;
			myPic =Global.gAICarInfo[id].userTexture;
			crewName = Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[id].AICrewID).ToString()+"A"; 
			if(b){
				temp.gameObject.SetActive(true);
				temp.GetComponent<UILabel>().text = nick;
				temp1.gameObject.SetActive(true);
				temp1.GetComponent<UITexture>().mainTexture 
					= myPic;
				temp2.gameObject.SetActive(true);
				temp2.GetComponent<UISprite>().spriteName = crewName;
			}else{
				temp.gameObject.SetActive(true);
				temp.GetComponent<UILabel>().text = nick;
				temp1.gameObject.SetActive(false);
				temp2.gameObject.SetActive(true);
				temp2.GetComponent<UISprite>().spriteName = crewName;
			}
		} // end of else
	} //RaceTypeRankChange

}
