using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using SimpleJSON;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
[System.Serializable]
public class QuestDailyInfo {
	public QuestDetail mGQuest = new QuestDetail();
	public QuestDetail mSQuest= new QuestDetail();
	public QuestDetail mBQuest= new QuestDetail();
	
	
	[System.Serializable]
	public class QuestDetail
	{
		public string ID;
		public int Season_num;
		public int Nut_num;
		public int TargetCount;
		public int R_type;
		public int R_S_1;
		public int R_exp;
		public bool bFinish;
		public bool bReward;
		public int index;
		public int raceMode; // 1 regular track , 2 regular drag , 3 rank race  , 4 pvp drag, 5pvp city
		
		public QuestDetail(int index, string id){
			this.index = index;
			this.ID =id;
			bReward = false;
			bFinish = false;
			TargetCount = 0;
		}
		
		public QuestDetail(){
			this.index = -1;
		}
	}
	public QuestDailyInfo(){
		
	}
	// 1 season = 16500 ~ 16517
	// 2 season = 16518 ~ 16535
	// 3 season = 16536 ~ 16553
	// 4 season = 16554 ~ 16571
	// 5 season = 16572 ~ 16589
	// 6 season = 16590 ~ 16607
	// 7 season = 16608 ~ 16625
	
	public void setInit(){
		GV.QuestReset = true;
	}
	
	public void setDailyInit(){
		int mySeason = (GV.ChSeasonID-6000)/5 + 1;
		int idx = 16482 + (mySeason*18);
		int mBcnt=0, mScnt = 0, mGcnt = 0;
		int mRandom = (int)Well512.Next(0,6);
		for(int i = 0; i < 18; i++){
			int id = idx + i;
			Common_Achieve.ItemDaily item = Common_Achieve.GetDaily(id);
			if(item.Nut_num == 1){
				if(mBcnt == mRandom){
				//	Debug.LogWarning("B " + item.ID + "_" + mRandom);
					mBQuest = new QuestDetail(i, item.ID);
				}
				mBcnt++;
			}else if(item.Nut_num == 2){
				if(mScnt == mRandom){
				//	Debug.LogWarning("S " + item.ID + "_" + mRandom);
					mSQuest = new QuestDetail(i, item.ID);
				}
				mScnt++;
			}else if(item.Nut_num == 3){
				if(mGcnt == mRandom){
				//	Debug.LogWarning("G " +  item.ID + "_" + mRandom);
					mGQuest = new QuestDetail(i, item.ID);
				}
				mGcnt++;
			}
		}

		if(mySeason == 1){
			if(mGQuest.raceMode  == 0 ||  mGQuest.raceMode == 1){
					mGQuest.raceMode = 1;
				if(mBQuest.raceMode == 2 || mBQuest.raceMode == 0){
					mBQuest.raceMode =2;
					mSQuest.raceMode = 3;
				}else{
					mBQuest.raceMode =3;
					mSQuest.raceMode = 2;
				}
			
			}else if (mGQuest.raceMode == 2){
				if(mBQuest.raceMode == 3 || mBQuest.raceMode == 0){
					mBQuest.raceMode =3;
					mSQuest.raceMode = 1;
				}else{
					mBQuest.raceMode =1;
					mSQuest.raceMode = 3;
				}
			}else if (mGQuest.raceMode == 3){
				if(mBQuest.raceMode == 1 || mBQuest.raceMode == 0){
					mBQuest.raceMode =1;
					mSQuest.raceMode = 2;
				}else{
					mBQuest.raceMode =2;
					mSQuest.raceMode = 1;
				}
			}
		}else if(mySeason == 2){
			int rand = 0;
			for(int i = 0; i < 30; i++){
				rand =  (int)Well512.Next(1,5);
				if(rand >=1 && rand < 4){
					int temp = mGQuest.raceMode;
					if(temp != rand){
						mGQuest.raceMode = rand;
						break;
					}
				}else{
					
				}
				
			}
			
			for(int i = 0; i < 30; i++){
				rand =  (int)Well512.Next(1,5);
				if(rand >=1 && rand < 4){
					int temp = mSQuest.raceMode;
					if(temp != rand && mGQuest.raceMode != rand){
						mSQuest.raceMode = rand;
						break;
					}
				}else{
					
				}
				
			}
			
			for(int i = 0; i < 30; i++){
				rand =  (int)Well512.Next(1,5);
				if(rand >=1 && rand < 4){
					int temp = mBQuest.raceMode;
					if(temp != rand && mGQuest.raceMode != rand && mSQuest.raceMode != rand){
						mBQuest.raceMode = rand;
						break;
					}
				}else{
					
				}
				
			}
		}else{
			int rand = 0;
			for(int i = 0; i < 30; i++){
				rand =  (int)Well512.Next(1,6);
				if(rand >=1 && rand < 5){
					int temp = mGQuest.raceMode;
					if(temp != rand){
						mGQuest.raceMode = rand;
						break;
					}
				}else{
				
				}

			}

			for(int i = 0; i < 30; i++){
				rand =  (int)Well512.Next(1,6);
				if(rand >=1 && rand < 5){
					int temp = mSQuest.raceMode;
					if(temp != rand && mGQuest.raceMode != rand){
						mSQuest.raceMode = rand;
						break;
					}
				}else{
				
				}
			
			}

			for(int i = 0; i < 30; i++){
				rand =  (int)Well512.Next(1,6);
				if(rand >=1 && rand < 5){
					int temp = mBQuest.raceMode;
					if(temp != rand && mGQuest.raceMode != rand && mSQuest.raceMode != rand){
						mBQuest.raceMode = rand;
						break;
					}
				}else{
				
				}
		
			}
		
			
		}

	}
}


public class QuestData{
	public static QuestData instance;
	public QuestDailyInfo mQuest = new QuestDailyInfo();
	public QuestData(){
		
	}
	public void GetQuestInfo(){
		if(!EncryptedPlayerPrefs.HasKey("Quest")){
			instance.mQuest.setInit();
			var b = new BinaryFormatter();
			var m = new MemoryStream();
			b.Serialize(m, instance.mQuest);
			EncryptedPlayerPrefs.SetString("Quest", 
			                               Convert.ToBase64String(m.GetBuffer()) );
		}
		
		var data = EncryptedPlayerPrefs.GetString("Quest");
		if(!string.IsNullOrEmpty(data))
		{
			var b = new BinaryFormatter();
			var m = new MemoryStream(Convert.FromBase64String(data));
			instance.mQuest = (QuestDailyInfo)b.Deserialize(m);
		}
	}
	
	public void SaveQuestInfo()
	{
		if(instance == null) return;
		var b = new BinaryFormatter();
		var m = new MemoryStream();
		b.Serialize(m, instance.mQuest);
		EncryptedPlayerPrefs.SetString("Quest", Convert.ToBase64String(m.GetBuffer()));
	}
	
}

public class QuestBoard : MonoBehaviour {
	private Animation winAni;
	public GameObject SubObj;
	bool aniDir;
	// Use this for initialization
	public UILabel[] lbLabels;
	public UILabel[] lbMode;
	public UILabel[] lbFinish;
	public UILabel[] lbMission;
	public UILabel[] lbQuantity;
	public GameObject[] objOks;
	public GameObject[] rewardObjs;
	void Awake(){
		winAni = gameObject.GetComponent<Animation>() as Animation;
		//SubObj.GetComponent<SubMenuAction>().RankingBoardUpDown(checkRank, false);
		
	}
	
	void checkRank(){
		
	}
	
	void Start () {
		aniDir = true;
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	string modeString(int index){
		string str = null;
		//1 regular track , 2 regular drag , 3 rank race  , 4 pvp drag, 5pvp city
		switch(index){
		case 1: str = KoStorage.GetKorString("72910"); break;
		case 2: str = KoStorage.GetKorString("72911"); break;
		case 3: str = KoStorage.GetKorString("72932"); break;
		case 4: str = KoStorage.GetKorString("72917"); break;
		case 5: str = KoStorage.GetKorString("72916"); break;

		}
		return str;
	}
	void SetMission(){
		int QID = int.Parse(QuestData.instance.mQuest.mGQuest.ID);
		Common_Achieve.ItemDaily item = Common_Achieve.GetDaily(QID);
		int targetCount = QuestData.instance.mQuest.mGQuest.TargetCount;
		int index = 2;
	if(targetCount >= item.Target_num){
			targetCount = item.Target_num;
			QuestData.instance.mQuest.mGQuest.bFinish = true;
			if(QuestData.instance.mQuest.mGQuest.bReward){
				objOks[index].SetActive(false);
				lbFinish[index].transform.gameObject.SetActive(true);
				lbFinish[index].text  = KoStorage.GetKorString("76118");
			}else{
				objOks[index].transform.FindChild("Btnimg_off").gameObject.SetActive(false);
				objOks[index].transform.FindChild("Btnimg_on").gameObject.SetActive(true);
				objOks[index].GetComponent<UIButtonMessage>().functionName = "OnMission3";
				int rType = item.R_type;
				string sType = "R_"+rType.ToString();
				rewardObjs[index].transform.FindChild(sType).gameObject.SetActive(true);
				lbQuantity[index].text = string.Format("X{0}",item.R_S_1);
				
			}
		}else{
			objOks[index].GetComponent<UIButtonMessage>().functionName = "";
			int rType = item.R_type;
			string sType = "R_"+rType.ToString();
			rewardObjs[index].transform.FindChild(sType).gameObject.SetActive(true);
			lbQuantity[index].text = string.Format("X{0}",item.R_S_1);
			
			
		}
		
		
		lbMission[index].text = string.Format("{0}/{1}", targetCount, item.Target_num);
		lbMode[index].text = modeString(QuestData.instance.mQuest.mGQuest.raceMode);
		
		QID =  int.Parse(QuestData.instance.mQuest.mSQuest.ID);
		item = Common_Achieve.GetDaily(QID);
		targetCount = QuestData.instance.mQuest.mSQuest.TargetCount;
		index = 1;
			if(targetCount >= item.Target_num){
			targetCount = item.Target_num;
			QuestData.instance.mQuest.mSQuest.bFinish = true;
			if(QuestData.instance.mQuest.mSQuest.bReward){
				objOks[index].SetActive(false);
				lbFinish[index].transform.gameObject.SetActive(true);
				lbFinish[index].text  = KoStorage.GetKorString("76118");
			}else{
				objOks[index].transform.FindChild("Btnimg_off").gameObject.SetActive(false);
				objOks[index].transform.FindChild("Btnimg_on").gameObject.SetActive(true);
				objOks[index].GetComponent<UIButtonMessage>().functionName = "OnMission2";
				int rType = item.R_type;
				string sType = "R_"+rType.ToString();
				rewardObjs[index].transform.FindChild(sType).gameObject.SetActive(true);
				lbQuantity[index].text = string.Format("X{0}",item.R_S_1);
				
			}
		}else{
			objOks[index].GetComponent<UIButtonMessage>().functionName = "";
			int rType = item.R_type;
			string sType = "R_"+rType.ToString();
			rewardObjs[index].transform.FindChild(sType).gameObject.SetActive(true);
			lbQuantity[index].text = string.Format("X{0}",item.R_S_1);
		}
		
		
		lbMission[index].text = string.Format("{0}/{1}", targetCount, item.Target_num);
		lbMode[index].text = modeString(QuestData.instance.mQuest.mSQuest.raceMode);
		
		
		QID =  int.Parse(QuestData.instance.mQuest.mBQuest.ID);
		item = Common_Achieve.GetDaily(QID);
		targetCount = QuestData.instance.mQuest.mBQuest.TargetCount;
		index = 0;
			if(targetCount >= item.Target_num){
			targetCount = item.Target_num;
			QuestData.instance.mQuest.mBQuest.bFinish = true;
			if(QuestData.instance.mQuest.mBQuest.bReward){
				objOks[index].SetActive(false);
				lbFinish[index].transform.gameObject.SetActive(true);
				lbFinish[index].text  = KoStorage.GetKorString("76118");
			}else{
				objOks[index].transform.FindChild("Btnimg_off").gameObject.SetActive(false);
				objOks[index].transform.FindChild("Btnimg_on").gameObject.SetActive(true);
				objOks[index].GetComponent<UIButtonMessage>().functionName = "OnMission1";
				int rType = item.R_type;
				string sType = "R_"+rType.ToString();
				rewardObjs[index].transform.FindChild(sType).gameObject.SetActive(true);
				lbQuantity[index].text = string.Format("X{0}",item.R_S_1);
			}
		}else{
			objOks[index].GetComponent<UIButtonMessage>().functionName = "";
			int rType = item.R_type;
			string sType = "R_"+rType.ToString();
			rewardObjs[index].transform.FindChild(sType).gameObject.SetActive(true);
			lbQuantity[index].text = string.Format("X{0}",item.R_S_1);
		}
		
		lbMission[index].text = string.Format("{0}/{1}", targetCount, item.Target_num);
		lbMode[index].text = modeString(QuestData.instance.mQuest.mBQuest.raceMode);
		
		
		
	}
	void OnAni(){
		
		//Debug.LogWarning(QuestData.instance.mQuest.mBQuest.ID);
		if(Global.isAnimation) return;
		Global.isAnimation = true;
		Global.isNetwork = true;
		Global.isPopUp = true;
		if(lbLabels[1] != null){
			//lbLabels[1].text = KoStorage.GetKorString("");
			lbLabels[1] = null;
			SetMission();
			
		}
		if(aniDir){
			aniDir = false;
			winAni["QuestBoard_open_1"].time = 0;
			winAni["QuestBoard_open_1"].speed = 1.0f;
			winAni.Play("QuestBoard_open_1");
			
		}else{
			aniDir = true;
			winAni["QuestBoard_open_1"].time =winAni["QuestBoard_open_1"].length;
			winAni["QuestBoard_open_1"].speed = -1.0f;
			winAni.Play("QuestBoard_open_1");
		}
		Invoke("setAniButton",1.0f);
		
	}
	
	void setAniButton(){
		Global.isAnimation = false;
		Global.isNetwork = false;
		Global.isPopUp = false;
	}
	
	
	private float cTime = 0.0f;
	public Transform timeArrow;
	private System.DateTime nTime;
	private readonly float secondsToDegrees = 360f / 60f;
	void FixedUpdate () {
		cTime += Time.deltaTime*10;
		timeArrow.localRotation = Quaternion.Euler(0f, 0f, cTime * -secondsToDegrees);
		lbLabels[0].text= UserDataManager.instance.dailyTimes();
	}
	
	
	void OnMission1(){
		int QID = int.Parse(QuestData.instance.mQuest.mBQuest.ID);
		Common_Achieve.ItemDaily item = Common_Achieve.GetDaily(QID);
		int rType = item.R_type;
		int rReward = item.R_S_1;
		objOks[0].SetActive(false);
		GameObject.Find("Audio").SendMessage("CompleteSound",SendMessageOptions.DontRequireReceiver);
		NetworkManager.instance.OnQuestReward(rType,rReward,(b)=>{
			//objOks[0].SetActive(false);
			if(!b){
				objOks[0].SetActive(true);
			}else{
				lbFinish[0].transform.gameObject.SetActive(true);
				lbFinish[0].text  = KoStorage.GetKorString("76118");
				QuestData.instance.mQuest.mBQuest.bReward = true;
				QuestData.instance.SaveQuestInfo();
			}
			
		});
	}
	
	void OnMission2(){
		int QID = int.Parse(QuestData.instance.mQuest.mSQuest.ID);
		Common_Achieve.ItemDaily item = Common_Achieve.GetDaily(QID);
		int rType = item.R_type;
		int rReward = item.R_S_1;
		objOks[1].SetActive(false);
		GameObject.Find("Audio").SendMessage("CompleteSound",SendMessageOptions.DontRequireReceiver);
		NetworkManager.instance.OnQuestReward(rType,rReward,(b)=>{
			if(!b){
				objOks[1].SetActive(true);
			}else{
				lbFinish[1].transform.gameObject.SetActive(true);
				lbFinish[1].text  = KoStorage.GetKorString("76118");
				QuestData.instance.mQuest.mSQuest.bReward = true;
				QuestData.instance.SaveQuestInfo();
			}
		});
	}
	void OnMission3(){
		int QID = int.Parse(QuestData.instance.mQuest.mGQuest.ID);
		Common_Achieve.ItemDaily item = Common_Achieve.GetDaily(QID);
		int rType = item.R_type;
		int rReward = item.R_S_1;
		objOks[2].SetActive(false);
		GameObject.Find("Audio").SendMessage("CompleteSound",SendMessageOptions.DontRequireReceiver);
		NetworkManager.instance.OnQuestReward(rType,rReward,(b)=>{
			if(!b){
				objOks[2].SetActive(true);
			}else{
				lbFinish[2].transform.gameObject.SetActive(true);
				lbFinish[2].text  = KoStorage.GetKorString("76118");
				QuestData.instance.mQuest.mGQuest.bReward = true;
				QuestData.instance.SaveQuestInfo();
			}
		});
	}
	
}
