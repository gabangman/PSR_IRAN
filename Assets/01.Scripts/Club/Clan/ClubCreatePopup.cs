using UnityEngine;
using System.Collections;

public class ClubCreatePopup : MonoBehaviour {


	public void ChangeClanCreateInfo(string[] str){
		transform.FindChild("Text1").GetComponent<UILabel>().text = KoStorage.GetKorString("77006");
		transform.FindChild("icon_mark").GetComponent<UISprite>().spriteName ="Clubsymbol_"+str[3];
		transform.FindChild("BtnMatch").FindChild("lbText1").GetComponent<UILabel>().text =KoStorage.GetKorString("77108");
		transform.FindChild("Res").FindChild("ib_res").GetComponent<UILabel>().text =string.Format("{0:#0}",CClub.ClubDollar);
	}

}
