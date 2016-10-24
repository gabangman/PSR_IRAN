using UnityEngine;
using System.Collections;

public class LocalRaceMenu : MonoBehaviour {

	public UILabel[] lblabel;
	public UISprite[] lbsprite;
	void Start(){
		
		lblabel[0].text = KoStorage.GetKorString("73000"); // club
		//lblabel[1].text =KoStorage.GetKorString("73600");/// "No - Name";//;KoStorage.GetKorString("74004"); // weekly
		lblabel[1].text =KoStorage.GetKorString("72933");
		lblabel[2].text = KoStorage.GetKorString("73002"); // event
		lblabel[3].text = KoStorage.GetKorString("73001"); // PVP
		lblabel[4].text = KoStorage.GetKorString("73003"); // regular
		lblabel[5].text = KoStorage.GetKorString("73004"); // champion
		

		if(AccountManager.instance.ChampItem.S3_1_Clubrace == 1){
			lbsprite[0].color = Color.white;	
		}else{
			lbsprite[0].color = Color.black;
		}

		if(AccountManager.instance.ChampItem.S2_1_Ranking == 1){
			lbsprite[1].color = Color.white;	
		}else{
			lbsprite[1].color = Color.black;
		}

		if(AccountManager.instance.ChampItem.S2_3_PVP_Drag == 1){
			lbsprite[3].color = Color.white;	
		}else{
			lbsprite[3].color = Color.black;
		}

		Destroy(this);
	}
}
