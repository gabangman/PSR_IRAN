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

public class GAchieve  { 
	public static GAchieve instance;
	public AchievementInfo achieveInfo = new AchievementInfo();
	
	public void GetAchievementInfo(){
	/*	if(!EncryptedPlayerPrefs.HasKey("achievement")){
			instance.achieveInfo.AcheiveInit();
			var b = new BinaryFormatter();
			var m = new MemoryStream();
			b.Serialize(m, instance.achieveInfo);
			EncryptedPlayerPrefs.SetString("achievement", 
			                               Convert.ToBase64String(m.GetBuffer()) );
		}
		
		var data = EncryptedPlayerPrefs.GetString("achievement");
		if(!string.IsNullOrEmpty(data))
		{
			var b = new BinaryFormatter();
			var m = new MemoryStream(Convert.FromBase64String(data));
			instance.achieveInfo = (AchievementInfo)b.Deserialize(m);
		}*/
		instance.achieveInfo.AcheiveInit();
	}
	
	public void SaveAchievementInfo()
	{
		/*if(instance == null) return;
		var b = new BinaryFormatter();
		var m = new MemoryStream();
		b.Serialize(m, instance.achieveInfo);
		EncryptedPlayerPrefs.SetString("achievement", Convert.ToBase64String(m.GetBuffer()));*/
	}
	
}

[System.Serializable]
public class AchievementInfo{
	public List<AchievementSubInfo> ListAchieve = new List<AchievementSubInfo>();
	public bool bFlag;
	[System.Serializable]
	public class AchievementSubInfo{
		public int AcheiveID;
		public bool bFinish;
		public int mCount;
		public int TargetCount;
		public bool bReward;
		public string G_ID;
		public string Apple_ID;
		public bool bUpLoad;
		public int mPlusCount;
		public bool mUnLock;
		public bool thisComplete;
	}
	
	public void SetTargetNumberAchievement(){
		Common_Achieve.Item aItem;
		int cnt = ListAchieve.Count;
		for(int i = 0; i < cnt; i++){
			int aID = ListAchieve[i].AcheiveID;
			aItem  = Common_Achieve.Get(aID);
			ListAchieve[i].TargetCount = aItem.Target_number;
			ListAchieve[i].G_ID = aItem.Google_ID;
			ListAchieve[i].Apple_ID = aItem.Apple_ID;
			if(ListAchieve[i].bUpLoad) GV.achieveId = 10;
		}
	}

	public void AcheiveInit(){
		AchievementSubInfo AInfo;
		int id = 16000;
		for(int i = 0; i < 39; i++){
			AInfo = new AchievementSubInfo();
			AInfo.AcheiveID = id+i;
			AInfo.bFinish = false;
			AInfo.mCount = 0;
			AInfo.bReward = false;
			AInfo.bUpLoad = false;
			AInfo.mPlusCount = 0;
			AInfo.mUnLock = false;
			ListAchieve.Add(AInfo);
		}
	}
	public bool exsitAchieve(int id){
		AchievementSubInfo AInfo = ListAchieve.Find(obj=>obj.AcheiveID == id);
		return AInfo.mUnLock;
	}



	public void SetUserAchievementInfo(int AchieveID, int Count, int bComplete, int bReward){
		AchievementSubInfo AInfo = ListAchieve.Find(obj=>obj.AcheiveID == AchieveID);
		int index = ListAchieve.IndexOf(AInfo);
		AInfo.mUnLock = true;

		if(bComplete == 1){
			ListAchieve[index].bFinish = true;
			if(bReward == 1) 	ListAchieve[index].thisComplete = true;
			else 	ListAchieve[index].thisComplete = false;
		}else{
			ListAchieve[index].bFinish = false;
		}
		ListAchieve[index].mCount = Count;
		if(bReward == 1) ListAchieve[index].bReward = true;
		else ListAchieve[index].bReward = false;
	}

	public void PlusLobbyUserAchievemnet(int id){ //starUp
		int achieve = 0;
		for(int i = 0; i < 3; i++){

			achieve = id+i;
			AchievementSubInfo aInfo = ListAchieve.Find((obj) =>obj.AcheiveID == achieve);
			int index = ListAchieve.IndexOf(aInfo);
			if(ListAchieve[index].thisComplete){
			
			}else{
				if(!ListAchieve[index].mUnLock) {
					Utility.LogWarning("UNLOCK ACHIEVE");

				}else{
					int mC = ListAchieve[index].mCount;
					SNSManager.RecordAchievementItems(ListAchieve[index].G_ID,aInfo.TargetCount, (mC+1));
					GameObject.Find("LobbyUI").SendMessage("SetAchievementIncreaseStarUp",index,SendMessageOptions.DontRequireReceiver);
					GAchieve.instance.achieveInfo.bFlag = true;
					break;
				}
		
			}
		}
	}

	/*
	public void PlusLobbyAchievemnet(int id){ //starUp
		for(int i = 0; i < 3; i++){
			id += i;
			AchievementSubInfo aInfo = ListAchieve.Find((obj) =>obj.AcheiveID == id);
			int index = ListAchieve.IndexOf(aInfo);
			if(!ListAchieve[index].mUnLock) return;
			if(!ListAchieve[index].bFinish){
				int mC = ListAchieve[index].mCount;
				mC += 1;
				ListAchieve[index].mCount += 1;
				ListAchieve[index].bUpLoad = true;
				ListAchieve[index].mPlusCount = 1;
				GV.achieveId = id;
				bFlag = true;
				GameObject.Find("LobbyUI").SendMessage("SetAchievementIncreaseStarUp",index,SendMessageOptions.DontRequireReceiver);
				if(mC >= aInfo.TargetCount){
					ListAchieve[index].mCount =  aInfo.TargetCount;
					mC = aInfo.TargetCount;
					ListAchieve[index].bFinish = true;
					ListAchieve[index].bReward = false;
					GV.bachieveRewardFlag = true;

					/*if(i == 2){
						
					}else{
						int idx = id+1;
						aInfo = ListAchieve.Find((obj) =>obj.AcheiveID == idx);
						SNSManager.unLockAchievement(aInfo.G_ID);
					}
					//SNSManager.RecordAchievementItems(ListAchieve[index].G_ID,Common_Achieve.Get(id).Point);
					SNSManager.RecordAchievementItems(ListAchieve[index].G_ID,aInfo.TargetCount, mC);
				}else{
					//	SNSManager.RecordAchievementItems(ListAchieve[index].G_ID, 5);
					//	SNSManager.RecordServerAchievement(id, mC);
					SNSManager.RecordAchievementItems(ListAchieve[index].G_ID,aInfo.TargetCount, mC);
				}
				break;
			}else{
				
			}
		}
	}
*/

	public void PlusAchievement(int id = 0, int cnt = 1){
		int achieve = 0;
		for(int i = 0; i < 3; i++){
			achieve = id +i;
			AchievementSubInfo aInfo = ListAchieve.Find((obj) =>obj.AcheiveID == achieve);
			int index = ListAchieve.IndexOf(aInfo);
			if(ListAchieve[index].thisComplete) {
			//	Utility.LogWarning("id1 " + achieve);
			
			}else {
				if(!ListAchieve[index].mUnLock) {
			//		Utility.LogWarning("id2 " + achieve);
				}else{
					int mC = ListAchieve[index].mCount;
					SNSManager.RecordAchievementItems(ListAchieve[index].G_ID,aInfo.TargetCount, (mC+1));
					ListAchieve[index].mPlusCount = cnt;
					ListAchieve[index].bUpLoad = true;
					GV.achieveId = achieve;
				//	Utility.LogWarning("id3 " + achieve);
					break;
				}
			}

		}
	}

/*	public void PlusAchievement(int id = 0, int cnt = 1){
		for(int i = 0; i < 3; i++){
			id += i;
			AchievementSubInfo aInfo = ListAchieve.Find((obj) =>obj.AcheiveID == id);
			int index = ListAchieve.IndexOf(aInfo);
			if(!ListAchieve[index].mUnLock) return;
			if(!ListAchieve[index].bFinish){
				int mC = ListAchieve[index].mCount;
				mC += cnt;
				ListAchieve[index].mCount += cnt;
				ListAchieve[index].mPlusCount = cnt;
				GV.achieveId = id;
				ListAchieve[index].bUpLoad = true;
				if(mC >= aInfo.TargetCount){
					ListAchieve[index].mCount =  aInfo.TargetCount;
					mC = aInfo.TargetCount;
					ListAchieve[index].bFinish = true;
					ListAchieve[index].bReward = false;
					GV.bachieveRewardFlag = true;
					SNSManager.RecordAchievementItems(ListAchieve[index].G_ID,aInfo.TargetCount, mC);
				}else{
					SNSManager.RecordAchievementItems(ListAchieve[index].G_ID,aInfo.TargetCount, mC);
				}
				break;
			}else{
				
			}
		}
	} */
	public void FinishAchievement(int id){
		AchievementSubInfo aInfo = ListAchieve.Find((obj) =>obj.AcheiveID == id);
		int index = ListAchieve.IndexOf(aInfo);
		if(ListAchieve[index].thisComplete) return;

		ListAchieve[index].bFinish = true; 
		GV.bachieveRewardFlag = true;
		GV.achieveId = id;
		ListAchieve[index].mPlusCount = 1;
		ListAchieve[index].bUpLoad = true;
		ListAchieve[index].bReward = false;
	//	SNSManager.RecordAchievementItems(aInfo.G_ID,Common_Achieve.Get(id).Point);
		SNSManager.RecordAchievementItems(ListAchieve[index].G_ID,1, 1);
		GAchieve.instance.achieveInfo.bFlag = true;



	}

	public void FinishUserAchievement(int id){
		AchievementSubInfo aInfo = ListAchieve.Find((obj) =>obj.AcheiveID == id);
		int index = ListAchieve.IndexOf(aInfo);
		if(ListAchieve[index].thisComplete) return;
		if(!ListAchieve[index].mUnLock) {
			return;
		}
		ListAchieve[index].mPlusCount = 1;
		ListAchieve[index].bUpLoad = true;
		GV.achieveId = id;
		SNSManager.RecordAchievementItems(ListAchieve[index].G_ID,1, 1);
		GAchieve.instance.achieveInfo.bFlag = true;
	}

	public bool RewardAchievement(int id){
		AchievementSubInfo aInfo = ListAchieve.Find((obj) =>obj.AcheiveID == id);
		int index = ListAchieve.IndexOf(aInfo);
		if(ListAchieve[index].bFinish){
			ListAchieve[index].bReward = true;
			return true;
		}
		return false;
	}
	
	public AchievementSubInfo GetAchievementInfo(int idx){
		//Utility.LogWarning("get " + idx);
		return ListAchieve.Find((obj) =>obj.AcheiveID == idx);
	}

	public void SetAchieveComplete(int id){
		AchievementSubInfo aInfo = ListAchieve.Find((obj) =>obj.AcheiveID == id);
		int index = ListAchieve.IndexOf(aInfo);
		ListAchieve[index].thisComplete = true;
		ListAchieve[index].bFinish = true;
		ListAchieve[index].bReward = true;
	}
	
}