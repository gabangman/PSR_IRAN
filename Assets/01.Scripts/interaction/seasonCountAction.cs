using UnityEngine;
using System.Collections;

public class seasonCountAction : MonoBehaviour {

	void Start(){
		GameObject obj = transform.parent.gameObject;
		if(obj.name == "1400") return;
		if(obj.name == "1406") return;
		int season = int.Parse(obj.name);
		season = season - 1400;
		if(season == 0) return;
		//Utility.Log(gameObject.name);
		var tr = transform.FindChild("Count") as Transform;
		int cnt = tr.childCount;
	//	Utility.Log(Global.MySeason);
	//	Utility.Log(season);
		if(GV.ChSeason < season) return;
		if(GV.ChSeason > season) {
			for(int i = 0 ; i <cnt; i++){
				tr.GetChild(i).FindChild("flag_on").gameObject.SetActive(true);
			}
			//Utility.Log("TEST");
		}else{
			
			for(int i = 0 ; i <cnt; i++){
				if((i+1) < GV.ChSeasonLV){
					tr.GetChild(i).FindChild("flag_on").gameObject.SetActive(true);
				}
			}
			//int mSeason1 = Base64Manager.instance.GlobalEncoding(Global.ChampionSeason);
			int mSeason1 =GV.ChSeasonID;
			if(mSeason1 >= 6030) {
				tr.GetChild(cnt-1).FindChild("flag_on").gameObject.SetActive(true);
			}
		}
	}
}
