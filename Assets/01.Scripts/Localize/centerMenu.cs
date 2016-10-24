using UnityEngine;
using System.Collections;

public class centerMenu : MonoBehaviour {

	public UILabel[] lbText;
	void Start () {

		string str = transform.name;
		if(str.Equals("MyShop")){
			lbText[0].text = KoStorage.GetKorString("74000");
			lbText[1].text = KoStorage.GetKorString("74001");
			lbText[2].text = KoStorage.GetKorString("74002");
		
		}else if(str.Equals("MyTeamCenter")){
			
		
		
		
		}else if(str.Equals("MyTeamUpMenu")){
		
			lbText[0].text = KoStorage.GetKorString("76218"); // car up
			lbText[1].text = KoStorage.GetKorString("76200"); // crew up
		
		}else if(str.Equals("MyInventory")){
			lbText[0].text = KoStorage.GetKorString("75000"); // car
			lbText[1].text = KoStorage.GetKorString("75001"); // material
			lbText[2].text = KoStorage.GetKorString("75002"); // cube
			lbText[3].text = KoStorage.GetKorString("75003"); // coupon
		}


		Destroy(this);
	}
}
