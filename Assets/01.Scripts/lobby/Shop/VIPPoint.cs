using UnityEngine;
using System.Collections;

public class VIPPoint : MonoBehaviour {
	public GameObject VIPInfo;
	public GameObject grid, slotPrefabs;
	public GameObject VIPGage, VIPLv;
	public UILabel[] lbtext;
	void Start(){
		lbtext[0].text = string.Format(KoStorage.GetKorString("72013"), GV.gVIP);
		lbtext[1].text = KoStorage.GetKorString("72014");
		lbtext[2].text = KoStorage.GetKorString("72006");
		lbtext[3].text = KoStorage.GetKorString("72015");
		lbtext[4].text = KoStorage.GetKorString("72016");
		//ChangeVIPLevel();
		//	Utility.LogWarning("chanageVIPLevel : " + GV.gVIP);
		Common_VIP.Item item = null;// = Common_VIP.Get(1900+GV.gVIP);
		if(GV.gVIP == 10){
			item = Common_VIP.Get(1909);
			int a = item.V_point;
			int b = Common_VIP.Get(1908).V_point;
			VIPGage.transform.FindChild("Exp").GetComponent<UILabel>().text = string.Format("{0} / {1}", a-b, a-b);
			VIPGage.transform.FindChild("gage_bar").GetComponent<UISprite>().fillAmount = 1.0f;
		}else if(GV.gVIP == 0){
			item = Common_VIP.Get(1900);
			VIPGage.transform.FindChild("Exp").GetComponent<UILabel>().text = string.Format("{0} / {1}",GV.vipExp , item.V_point );
			VIPGage.transform.FindChild("gage_bar").GetComponent<UISprite>().fillAmount = 0.0f;
		}else{
			item = Common_VIP.Get(1899+GV.gVIP);
			int c = item.V_point;
			int d = Common_VIP.Get(1900+GV.gVIP).V_point;
			int vipPoint = GV.vipExp-c;
			if(vipPoint < 0) vipPoint *= -1;
			VIPGage.transform.FindChild("Exp").GetComponent<UILabel>().text = string.Format("{0} / {1}",vipPoint, d-c);
			if(vipPoint == 0){
				VIPGage.transform.FindChild("gage_bar").GetComponent<UISprite>().fillAmount = 0;
			}else{
				VIPGage.transform.FindChild("gage_bar").GetComponent<UISprite>().fillAmount = (float)vipPoint / (float)(d-c);
			}
		}
	}
	
	public void ChangeVIPLevel(){
		//GV.vipExp = 80;
		Common_VIP.SetVIPLevelConvert();
		//Utility.LogWarning("chanageVIPLevel : " + GV.gVIP + " exp : " + GV.vipExp);
		Common_VIP.Item item = null;// = Common_VIP.Get(1900+GV.gVIP);
		if(GV.gVIP == 10){
		//	item = Common_VIP.Get(1909);
		//	int a = item.V_point;
		//	int b = Common_VIP.Get(1908).V_point;
			VIPGage.transform.FindChild("Exp").GetComponent<UILabel>().text = string.Empty;//string.Format("{0} / {1}", a-b, a-b);
			VIPGage.transform.FindChild("gage_bar").GetComponent<UISprite>().fillAmount = 1.0f;
		}else if(GV.gVIP == 0){
			item = Common_VIP.Get(1900);
			VIPGage.transform.FindChild("Exp").GetComponent<UILabel>().text = string.Format("{0} / {1}",GV.vipExp , item.V_point );
			VIPGage.transform.FindChild("gage_bar").GetComponent<UISprite>().fillAmount = 0.0f;
		
		}else{
			item = Common_VIP.Get(1899+GV.gVIP);
			int c = item.V_point;
			int d = Common_VIP.Get(1900+GV.gVIP).V_point;
			
			int vipPoint = GV.vipExp-c;
			if(vipPoint < 0) vipPoint *= -1;
			VIPGage.transform.FindChild("Exp").GetComponent<UILabel>().text = string.Format("{0} / {1}",vipPoint, d-c);
			if(vipPoint == 0){
				VIPGage.transform.FindChild("gage_bar").GetComponent<UISprite>().fillAmount = 0;
			}else{
				VIPGage.transform.FindChild("gage_bar").GetComponent<UISprite>().fillAmount = (float)vipPoint / (float)(d-c);
			}
		}
		lbtext[0].text = string.Format(KoStorage.GetKorString("72013"), GV.gVIP);
	}
	
	void OnVIPClick(){
		VIPInfo.SetActive(true);
		transform.parent.GetComponent<TweenAction>().doubleTweenScale(VIPInfo);
		AddItem();
		UserDataManager.instance.OnSubBacksubsub = ()=>{
			OnClose();
		};
	}
	
	void AddItem(){
		int cnt = grid.transform.childCount;
		int max =Common_VIP.getCount();
		if(cnt == 0){
			for(int i = 0; i < max ;i++){
				var temp = NGUITools.AddChild(grid, slotPrefabs);
				changeVIPInfo(temp, i);
			}
		}else{
			
			for(int i = 0; i < max ;i++){
				var temp = grid.transform.GetChild(i).gameObject as GameObject;
				ChangeVIP(temp, i);
			}
			
		}
		grid.GetComponent<UIGrid>().Reposition();
		
	}
	void OnClose(){
		VIPInfo.SetActive(false);
		UserDataManager.instance.OnSubBacksubsub = null;
	}
	
	void changeVIPInfo(GameObject obj, int idx){
		int index = idx+1900;
		Common_VIP.Item item = Common_VIP.Get(index);
		obj.transform.FindChild("Text_2").GetComponent<UILabel>().text = item.V_Text;
		obj.transform.FindChild("VIPLevel").GetComponent<UILabel>().text = 
			string.Format("VIP \n LEVEL {0} ", item.V_level);
		if((idx+1) == GV.gVIP){
			obj.transform.FindChild("bg_MyLevel").gameObject.SetActive(true);
		}
	}
	
	void ChangeVIP(GameObject obj, int idx){
		//int index = idx+1900;
		if((idx+1) == GV.gVIP){
			obj.transform.FindChild("bg_MyLevel").gameObject.SetActive(true);
		}else{
			obj.transform.FindChild("bg_MyLevel").gameObject.SetActive(false);
		}
	}
	
}
