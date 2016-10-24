using UnityEngine;
using System.Collections;

public class weeklystart : MonoBehaviour {

	public UILabel[] lbText;
	public Transform[] trRank;
	void Start () {
		lbText[0].text = KoStorage.GetKorString("72803");//TableManager.ko.dictionary["60184"].String;
		lbText[1].text = KoStorage.GetKorString("72804");//TableManager.ko.dictionary["60207"].String;
		lbText[2].text = KoStorage.GetKorString("72805");//TableManager.ko.dictionary["60207"].String;

		Common_Attend.Item item = Common_Attend.Get(8718);
		trRank[0].FindChild("lbRank").GetComponent<UILabel>().text = string.Format("{0}", item.Target );
		trRank[0].FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();
		
		item = Common_Attend.Get(8719);
		trRank[1].FindChild("lbRank").GetComponent<UILabel>().text =string.Format("{0}", item.Target);
		trRank[1].FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();
		
		item = Common_Attend.Get(8720);
		trRank[2].FindChild("lbRank").GetComponent<UILabel>().text = string.Format("{0}", item.Target);
		trRank[2].FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();
		
		item = Common_Attend.Get(8721);
		trRank[3].FindChild("lbRank").GetComponent<UILabel>().text =string.Format("~{0}", item.Target);
		trRank[3].FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();
		
		item = Common_Attend.Get(8722);
		trRank[4].FindChild("lbRank").GetComponent<UILabel>().text =string.Format("~{0}", item.Target);
		trRank[4].FindChild("lbPrice").GetComponent<UILabel>().text = item.R_no.ToString();
		
		item = Common_Attend.Get(8723);
		trRank[5].FindChild("lbRank").GetComponent<UILabel>().text =string.Format("~{0}", item.Target);
		trRank[5].FindChild("lbPrice").GetComponent<UILabel>().text =item.R_no.ToString();




		Destroy(this);
	}
}
