using UnityEngine;
using System.Collections;

public class AchievementItem : MonoBehaviour {

	void Start(){
	
	}
	public void ChangeAchieveContent(){
		int idx = int.Parse(transform.name);
		if(idx >= 16003 && idx < 16006)
			setAchieveContent(idx);
	}

	public void setUserAchieveContent(int idx){

	}
	public void setAchieveContent(int idx){

		AchievementInfo.AchievementSubInfo sInfo;
		int mCnt = 0;
		for(int i = 0; i < 3; i++){
			int initID = idx+i;
			sInfo = GAchieve.instance.achieveInfo.GetAchievementInfo(initID);
			Common_Achieve.Item item = Common_Achieve.Get(initID);
			string rType = "icon_coin";
			if(sInfo.bFinish){
				if(sInfo.bReward){
					if(i == 2){
						transform.FindChild("Reward").gameObject.SetActive(false);
						transform.FindChild("Accept").gameObject.SetActive(false);
						StarCountControl(4);
						GageBarControl( transform.FindChild("Gage"),item.Target_number, item.Target_number);
					}
				}else{
					transform.FindChild("Reward").gameObject.SetActive(false);
					transform.FindChild("Accept").gameObject.SetActive(true);
					transform.FindChild("Accept").GetComponentInChildren<UIButtonMessage>().functionName = "OnAccept";
					var trchild =transform.FindChild("Accept").FindChild("Reward");
					trchild.GetComponentInChildren<UILabel>().text = string.Format("X {0}", item.R_no);
					trchild.GetComponentInChildren<UISprite>().spriteName = rType;
					transform.name = initID.ToString();
					trchild = transform.FindChild("Gage") as Transform;
					GageBarControl(trchild, item.Target_number, item.Target_number);
					StarCountControl(i+1);
					trchild =transform.FindChild("Info");
					trchild.FindChild("lbExp").GetComponent<UILabel>().text = string.Format("{0} / {1}",item.Target_number , item.Target_number);
					trchild.FindChild("lbTitle_1").GetComponent<UILabel>().text = item.Name;
					trchild.FindChild("lbTitle_2").GetComponent<UILabel>().text = item.Text;
					break;
				}
			}else{
				int myCount = sInfo.mCount;
				var trchild = transform.FindChild("Gage") as Transform;
				GageBarControl(trchild,myCount, item.Target_number);
				StarCountControl(i+1);
				trchild =transform.FindChild("Info");
				//if(myCount== -1) trchild.FindChild("lbExp").GetComponent<UILabel>().text = string.Format("{0} / {1}",myCount , item.Target_number);
				//else  trchild.FindChild("lbExp").GetComponent<UILabel>().text = string.Format("{0} / {1}",myCount , item.Target_number);
				trchild.FindChild("lbExp").GetComponent<UILabel>().text = string.Format("{0} / {1}",myCount , item.Target_number);
				trchild.FindChild("lbTitle_1").GetComponent<UILabel>().text = item.Name;
				trchild.FindChild("lbTitle_2").GetComponent<UILabel>().text = item.Text;
				
				trchild =transform.FindChild("Reward");
				trchild.GetComponentInChildren<UILabel>().text = string.Format("X {0}", item.R_no);
				trchild.GetComponentInChildren<UISprite>().spriteName = rType;
				transform.name = initID.ToString();
				break;
			}
		}


		/*
		int initID = 0;
		int mCnt = 0;
		int myCount = 1;
		AchievementInfo.AchievementSubInfo sInfo;
		for(int i = 0; i < 3; i++){
			initID = idx+i;
			sInfo = GAchieve.instance.achieveInfo.GetAchievementInfo(initID);
			Common_Achieve.Item item = Common_Achieve.Get(initID);
			string rType = "icon_coin";
			if(sInfo.bFinish){
				if(sInfo.bReward){
					mCnt++;
					if(mCnt == 3){
						transform.FindChild("Reward").gameObject.SetActive(false);
						transform.FindChild("Accept").gameObject.SetActive(false);
						StarCountControl(4);
						GageBarControl( transform.FindChild("Gage"),item.Target_number, item.Target_number);
					}
				}else{
					transform.FindChild("Reward").gameObject.SetActive(false);
					transform.FindChild("Accept").gameObject.SetActive(true);
					//transform.FindChild("Accept").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("72104");
					transform.FindChild("Accept").GetComponentInChildren<UIButtonMessage>().functionName = "OnAccept";
					var trchild =transform.FindChild("Accept").FindChild("Reward");
					trchild.GetComponentInChildren<UILabel>().text = string.Format("X {0}", item.R_no);
					trchild.GetComponentInChildren<UISprite>().spriteName = rType;
					transform.name = initID.ToString();
					trchild = transform.FindChild("Gage") as Transform;
					GageBarControl(trchild, item.Target_number, item.Target_number);
					StarCountControl(mCnt+1);
					trchild =transform.FindChild("Info");
					trchild.FindChild("lbExp").GetComponent<UILabel>().text = string.Format("{0} / {1}",item.Target_number , item.Target_number);
					trchild.FindChild("lbTitle_1").GetComponent<UILabel>().text = item.Name;
					trchild.FindChild("lbTitle_2").GetComponent<UILabel>().text = item.Text;

					break;
				}
			}else{
				myCount = sInfo.mCount;
				var trchild = transform.FindChild("Gage") as Transform;
				GageBarControl(trchild,myCount, item.Target_number);
				StarCountControl(mCnt+1);
				trchild =transform.FindChild("Info");
				if(myCount== -1) trchild.FindChild("lbExp").GetComponent<UILabel>().text = string.Format("{0} / {1}",myCount , item.Target_number);
				else  trchild.FindChild("lbExp").GetComponent<UILabel>().text = string.Format("{0} / {1}",myCount , item.Target_number);

				trchild.FindChild("lbTitle_1").GetComponent<UILabel>().text = item.Name;
				trchild.FindChild("lbTitle_2").GetComponent<UILabel>().text = item.Text;

				trchild =transform.FindChild("Reward");
				trchild.GetComponentInChildren<UILabel>().text = string.Format("X {0}", item.R_no);
				trchild.GetComponentInChildren<UISprite>().spriteName = rType;
				transform.name = initID.ToString();
				break;
			}
		
		}*/
	}


	void OnAccept(GameObject obj){
	//	Utility.LogWarning("Achieve OnAccept ");
		string str = obj.transform.parent.parent.name;
		transform.FindChild("Accept").gameObject.SetActive(false);
		int idx = int.Parse(str);
		int mid = idx;
		GV.bachieveRewardFlag = false;
		Common_Achieve.Item aItem = Common_Achieve.Get (idx);
		string rType = null;
		switch(aItem.R_type){
		case 3:{
			rType = "icon_coin";
			GV.myCoin += aItem.R_no;
			GV.updateCoin = -aItem.R_no;
			GameObject.Find("LobbyUI").SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
		}break;
		default:
			break;
		}
		//GAchieve.instance.achieveInfo.RewardAchievement(idx);
		AchievementInfo.AchievementSubInfo sInfo;
		int nextID = idx - 15999;
		int mID =  nextID  % 3;
	
		if(mID == 0){
			aItem = Common_Achieve.Get(idx);
			transform.FindChild("Reward").gameObject.SetActive(false);
			transform.FindChild("Accept").gameObject.SetActive(false);
			StarCountControl(4);
			GageBarControl( transform.FindChild("Gage"),aItem.Target_number, aItem.Target_number);
		}else{
			idx++;
			aItem = Common_Achieve.Get(idx);
			sInfo = GAchieve.instance.achieveInfo.GetAchievementInfo(idx);
			int temp = sInfo.mCount;
			if(sInfo.bFinish){
				if(sInfo.bReward){
						transform.FindChild("Reward").gameObject.SetActive(false);
						transform.FindChild("Accept").gameObject.SetActive(false);
						StarCountControl(4);
						GageBarControl( transform.FindChild("Gage"),aItem.Target_number, aItem.Target_number);
				}else{
					//sInfo.mCount = 0;
					transform.FindChild("Reward").gameObject.SetActive(false);
					transform.FindChild("Accept").gameObject.SetActive(true);
					transform.FindChild("Accept").GetComponentInChildren<UIButtonMessage>().functionName = "OnAccept";
					var trchild =transform.FindChild("Accept").FindChild("Reward");
					trchild.GetComponentInChildren<UILabel>().text = string.Format("X {0}", aItem.R_no);
					trchild.GetComponentInChildren<UISprite>().spriteName = rType;
					transform.name = idx.ToString();
					trchild = transform.FindChild("Gage") as Transform;
					GageBarControl(trchild, sInfo.mCount, aItem.Target_number);
					StarCountControl(mID+1);
					trchild =transform.FindChild("Info");
					if(sInfo.mCount == -1) trchild.FindChild("lbExp").GetComponent<UILabel>().text = string.Format("{0} / {1}",0 , aItem.Target_number);
					else  trchild.FindChild("lbExp").GetComponent<UILabel>().text = string.Format("{0} / {1}",sInfo.mCount , aItem.Target_number);
					trchild.FindChild("lbExp").GetComponent<UILabel>().text = string.Format("{0} / {1}",sInfo.mCount , aItem.Target_number);
					trchild.FindChild("lbTitle_1").GetComponent<UILabel>().text = aItem.Name;
					trchild.FindChild("lbTitle_2").GetComponent<UILabel>().text = aItem.Text;

				}
			}else{
				aItem = Common_Achieve.Get(idx);
				sInfo = GAchieve.instance.achieveInfo.GetAchievementInfo(idx);
				//sInfo.mCount = 0;
				var trchild = transform.FindChild("Gage") as Transform;
				GageBarControl(trchild,sInfo.mCount, aItem.Target_number);
				StarCountControl(mID+1);
				trchild =transform.FindChild("Info");
				if(sInfo.mCount == -1) trchild.FindChild("lbExp").GetComponent<UILabel>().text = string.Format("{0} / {1}",0 , aItem.Target_number);
				else  trchild.FindChild("lbExp").GetComponent<UILabel>().text = string.Format("{0} / {1}",sInfo.mCount , aItem.Target_number);
				trchild.FindChild("lbExp").GetComponent<UILabel>().text = string.Format("{0} / {1}",sInfo.mCount , aItem.Target_number);
				trchild.FindChild("lbTitle_1").GetComponent<UILabel>().text = aItem.Name;
				trchild.FindChild("lbTitle_2").GetComponent<UILabel>().text = aItem.Text;
				trchild =transform.FindChild("Reward");
				trchild.gameObject.SetActive(true);
				trchild.GetComponentInChildren<UILabel>().text = string.Format("X {0}", aItem.R_no);
				trchild.GetComponentInChildren<UISprite>().spriteName = rType;
				transform.name = idx.ToString();
			}
		}
		AccountManager.instance.SetNextAchievement(mid, mID);
	}


	void GageBarControl(Transform trchild, int count, int Max){
			var tr = trchild.FindChild("P_FG") as Transform;
		Vector3 scale = trchild.FindChild("P_BG").localScale;
		Vector3 scale2 = tr.localScale;
		float posXUp = 0;
		float poxCount = ((float)count / (float)Max ) * scale.x;
		Vector3 upScale = Vector3.zero;
		if(count <= 0){
			upScale = new Vector3(0.0001f, scale2.y, scale2.z);
			tr.gameObject.SetActive(false);
		}else{
			if(count >  Max){
				poxCount = 1.0f*scale.x;
			}
			upScale = new Vector3(poxCount, scale2.y, scale2.z);
			tr.localScale = upScale;
		}
	}

	void StarCountControl(int myCount){
		var tr = transform.FindChild("Star") as Transform;
		myCount--;
		for(int i =0; i < tr.childCount;i ++){
			if(i < myCount)
				tr.GetChild(i).FindChild("Star_on").gameObject.SetActive(true);
		}
	}


}
