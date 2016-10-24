using UnityEngine;
using System.Collections;

public class RaceSubItems : MonoBehaviour {

	protected GameObject mapInfo, modeInfo;
	protected bool bLock;
//	public delegate void OnNextClick();
//	public OnNextClick onNext;
//	public OnNextClick onNext1;
//	public OnNextClick onNext2;

	public virtual void OnNext(GameObject obj){}
	public virtual void OnNext1(GameObject obj){}
	public virtual void OnNext2(GameObject obj){}
	void Awake(){
		mapInfo = transform.GetChild(0).GetChild(0).gameObject;
		modeInfo = transform.GetChild(1).gameObject;
		bLock = false;
	}


	void SetDisable(){
		int cnt = modeInfo.transform.childCount;
		for(int i = 0; i < cnt; i++){
			modeInfo.transform.GetChild(i).gameObject.SetActive(false);
		}

	}
	public bool OnLockBoolean(string mapIdx){


		return bLock;
	}

	public bool OnLockBool{
		get{
			return bLock;
		}
	}

	protected void SetChampionInfo(Transform tr){
		tr = tr.GetChild(0);
		var mTr = tr.FindChild("Mode_On") as Transform;
		Common_Reward.Item item = Common_Reward.Get(Global.gRewardId);
		mTr.FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("73100");
		mTr.FindChild("Reward").FindChild("Coin").FindChild("Quantity").GetComponent<UILabel>().text =string.Format("{0:#,0}", item.Champ_coin);
		mTr.FindChild("Reward").FindChild("Dollor").FindChild("Quantity").GetComponent<UILabel>().text = string.Format("{0:#,0}", item.Champ_dollar);
	}

	protected void SetRegularInfo(Transform tr){
		tr= tr.GetChild(0);
		var mTr = tr.FindChild("Mode_On") as Transform;
		mTr.FindChild("Text1").GetComponent<UILabel>().text =KoStorage.GetKorString("73206");
		mTr.FindChild("Reward").FindChild("Quantity").GetComponent<UILabel>().text =KoStorage.GetKorString("73205");
	
	}


}
