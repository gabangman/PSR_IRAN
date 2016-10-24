using UnityEngine;
using System.Collections;

public class RepairPopup : MonoBehaviour {

	// Use this for initialization
	public void InitWindow (int pDurability, string name) {
		transform.FindChild("Content_BUY").gameObject.SetActive(false);
		var rObj = transform.FindChild("Content_Repair") as Transform;
		rObj.gameObject.SetActive(true);
		rObj.FindChild("lbTitle").GetComponent<UILabel>().text = "수리하기";		
		if(pDurability == 100){
			rObj.FindChild("btn_ok").gameObject.SetActive(true);	
			rObj.FindChild("btn_all").gameObject.SetActive(false);
			rObj.FindChild("btn_part").gameObject.SetActive(false);
			rObj.FindChild("lbPriceAll").gameObject.SetActive(false);
			rObj.FindChild("lbPricePart").gameObject.SetActive(false);
			rObj.FindChild("lbRepairAll").gameObject.SetActive(false);
			rObj.FindChild("lbRepairPart").gameObject.SetActive(false);
			rObj.FindChild("lbText").GetComponent<UILabel>().text = "내구도100이라 수리 못해";	
			rObj.FindChild("Progress Bar").GetChild(1).GetComponent<UISprite>().fillAmount = 1.0f;
			rObj.FindChild("Progress Bar").GetChild(1).GetComponent<UISprite>().color = Color.cyan;
		}else{
			rObj.FindChild("btn_ok").gameObject.SetActive(false);	
			rObj.FindChild("btn_all").gameObject.SetActive(true);
			rObj.FindChild("btn_part").gameObject.SetActive(true);
			rObj.FindChild("lbPriceAll").gameObject.SetActive(true);
			rObj.FindChild("lbPricePart").gameObject.SetActive(true);
			rObj.FindChild("lbRepairAll").gameObject.SetActive(true);
			rObj.FindChild("lbRepairPart").gameObject.SetActive(true);
			rObj.FindChild("lbText").GetComponent<UILabel>().text = "수리할래?";
			rObj.FindChild("Progress Bar").GetChild(1).GetComponent<UISprite>().fillAmount = pDurability*0.01f;
			if(pDurability >50) rObj.FindChild("Progress Bar").GetChild(1).GetComponent<UISprite>().color = Color.cyan;
			else if(pDurability > 25) rObj.FindChild("Progress Bar").GetChild(1).GetComponent<UISprite>().color = Color.yellow;
			else rObj.FindChild("Progress Bar").GetChild(1).GetComponent<UISprite>().color = Color.red;
		}
		this.pName = name;
	}
	
	string pName;
//	void OnOkClick(){
//		OnCloseClick();
//	}
	
	void OnCloseClick(){
		gameObject.SetActive(false);
		transform.FindChild("Content_BUY").gameObject.SetActive(true);
		transform.FindChild("Content_Repair").gameObject.SetActive(false);
		Destroy(this);
	}
	
	void OnRepairAll(){
		updateAllDurability();
		GameObject.Find("LobbyUI").SendMessage("ChangeLevelCarButton",true, SendMessageOptions.DontRequireReceiver);
		
		OnCloseClick();
	}
	void OnRepairPart(){
		updateDurability();
		GameObject.Find("LobbyUI").SendMessage("ChangeLevelCarButton",true, SendMessageOptions.DontRequireReceiver);
		OnCloseClick();
	}

	void OnSuccessClick(){
		OnCloseClick();
	}
	
	void updateDurability(){
		string[] str = pName.Split('_');
	int a = 0;
	int.TryParse(str[0], out a);
	int b = 0;
	int.TryParse(str[1], out b);
	
	/*	Account.CarInfo _carInfo = myAccount.instance.GetCarInfo(a);
		switch(b){
		case 5000: _carInfo.bodydurability=100;break;
		case 5001: _carInfo.enginedurability=100;break;
		case 5002: _carInfo.tiredurability=100;break;
		case 5003: _carInfo.gearBoxdurability=100;break;
		case 5004: _carInfo.intakedurability=100;break;
		case 5005: _carInfo.bsPowerdurability=100;break;
		case 5006: _carInfo.bsTimedurability=100;break;
		}*/
	}
	void updateAllDurability(){
		/*string[] str = pName.Split('_');
		int a = 0;
		int.TryParse(str[0], out a);
		int b = 0;
		int.TryParse(str[1], out b);
		
		Account.CarInfo _carInfo = myAccount.instance.GetCarInfo(a);
		
		_carInfo.bodydurability=100;
		 _carInfo.enginedurability=100;
		_carInfo.tiredurability=100;
		_carInfo.gearBoxdurability=100;
		_carInfo.intakedurability=100;
		 _carInfo.bsPowerdurability=100;
		_carInfo.bsTimedurability=100;*/
	}
	
}
