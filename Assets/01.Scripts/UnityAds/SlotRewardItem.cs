using UnityEngine;
using System.Collections;

public class SlotRewardItem : MonoBehaviour {

	private bool bAccept = false;
	private UILabel lbTime;
	public void InitRewardItems(int num, GameObject obj){
		Common_Attend.Item rItem = Common_Attend.Get(num);
		if(num > GV.CurrADId){
			transform.FindChild("img_Wait").gameObject.SetActive(true);
		} else if(num == GV.CurrADId){
			transform.FindChild("img_Wait").gameObject.SetActive(true);
		}else{
			if(myAcc.instance.account.bRewards[num-8750]){
				transform.FindChild("img_Complete").gameObject.SetActive(true);
				SetCompleteButton(rItem);
			}else{
			//	transform.FindChild("img_Claim").gameObject.SetActive(true);
			//	transform.FindChild("btn_Claim").gameObject.SetActive(true);
			//	transform.FindChild("btn_Claim").GetComponent<UIButtonMessage>().target = gameObject;
			}
		//	SetCompleteButton(rItem);
		}
	}


	public void ReSetMaterialRewardItems(int reMat){
		int num = int.Parse(transform.name);
		if(num == GV.CurrADId){
			Common_Attend.Item rItem = Common_Attend.Get(num);
			this.reMatId = reMat;
			SetCompleteButton(rItem);
			return;
		}
	}

	public void ReSetRewardItems(int curId, int nexId){
		int num = int.Parse(transform.name);
		if(num == curId){
			Common_Attend.Item rItem = Common_Attend.Get(num);
			transform.FindChild("img_Complete").gameObject.SetActive(true);
			SetCompleteButton(rItem);
			return;
		}

		if(num == nexId){
		
			return;
		}
	
	
	
	
	}

	void FixedUpdate(){

	}

	private void SetCompleteButton(	Common_Attend.Item rItem ){
		transform.FindChild("img_Wait").gameObject.SetActive(false);
		//transform.FindChild("lbWaitTime").gameObject.SetActive(false);
		string str = null;
		switch(rItem.R_type){
		case 3 : str = "Coin";break;
		case 4 : str = "Dollar";break;
		case 8 : str = "Material";break;
		case 6 : str = "SilverBox";break;
		case 7 : str = "GoldBox";break;
		case 5 : str = "Cube";break;
		}
		var temp = transform.FindChild(str).gameObject as GameObject;
		temp.SetActive(true);
		temp.transform.FindChild("lbQuantity").GetComponent<UILabel>().text
			= string.Format("X {0}", rItem.R_no);
		if(rItem.R_type == 8){
			if(reMatId != 0) 	reMatId = Random.Range(8600,8620);
			temp.transform.FindChild("img_Mat").GetComponent<UISprite>().spriteName = reMatId.ToString();
			NGUITools.FindInParents<ADRewardContent>(gameObject).reMatId = reMatId;
		}
	}
	private int reMatId = 0;

	public void ChangeRewardItems(){
		int num = int.Parse(transform.name);
		if(num == GV.CurrADId){
			transform.FindChild("btn_Watch").gameObject.SetActive(false);
			SetCompleteButton(Common_Attend.Get(num));
			transform.FindChild("btn_Claim").gameObject.SetActive(true);
			transform.FindChild("btn_Claim").GetComponent<UIButtonMessage>().target = gameObject;
			transform.FindChild("img_Qu").gameObject.SetActive(false);
			bAccept = true;
		}
	}





}
