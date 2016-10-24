using UnityEngine;
using System.Collections;

public class weeklyfinish : MonoBehaviour {

	public UILabel[] lbText;
	public Transform[] trRank;
	void Start () {

		if(transform.name == "DailyFinish") {
			lbText[1].text =KoStorage.GetKorString("72806");// TableManager.ko.dictionary["60184"].String;
			lbText[0].text =KoStorage.GetKorString("72805");// TableManager.ko.dictionary["60205"].String;
			lbText[2].text =KoStorage.GetKorString("71000");
			return;
		}

		lbText[0].text =KoStorage.GetKorString("72806");// TableManager.ko.dictionary["60184"].String;
		lbText[1].text =KoStorage.GetKorString("72807");// TableManager.ko.dictionary["60205"].String;
		int mRank  = UserDataManager.instance.weeklyResultRank;
		if(mRank == 0){
		//	lbText[2].text =KoStorage.getStringDic("60250");
			lbText[2].text =string.Format(KoStorage.GetKorString("72802"),KoStorage.GetKorString("72811"));
			
		}else{
			lbText[2].text =string.Format(KoStorage.GetKorString("72802"),mRank);
		}
	
	
		Common_Attend.Item item = Common_Attend.Get(8718);
		trRank[0].FindChild("lbRank").GetComponent<UILabel>().text = string.Format("{0} {1}", item.Target ,KoStorage.GetKorString("72809"));
		trRank[0].FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();
		
		item = Common_Attend.Get(8719);
		trRank[1].FindChild("lbRank").GetComponent<UILabel>().text =string.Format("{0} {1}", item.Target, KoStorage.GetKorString("72809"));
		trRank[1].FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();
		
		item = Common_Attend.Get(8720);
		trRank[2].FindChild("lbRank").GetComponent<UILabel>().text = string.Format("{0} {1}", item.Target, KoStorage.GetKorString("72809"));
		trRank[2].FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();
		
		item = Common_Attend.Get(8721);
		trRank[3].FindChild("lbRank").GetComponent<UILabel>().text =string.Format("~{0} {1}", item.Target, KoStorage.GetKorString("72809"));
		trRank[3].FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();
		
		item = Common_Attend.Get(8722);
		trRank[4].FindChild("lbRank").GetComponent<UILabel>().text =string.Format("~{0} {1}", item.Target, KoStorage.GetKorString("72809"));
		trRank[4].FindChild("lbPrice").GetComponent<UILabel>().text = item.R_no.ToString();
		
		item = Common_Attend.Get(8723);
		trRank[5].FindChild("lbRank").GetComponent<UILabel>().text =string.Format("~{0} {1}", item.Target, KoStorage.GetKorString("72809"));
		trRank[5].FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();
		Destroy(this);
	}

	void OnClose(){
		NGUITools.FindInParents<SubMenuWindow>(gameObject).OnCloseClick();
	}

}
