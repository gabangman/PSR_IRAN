using UnityEngine;
using System.Collections;

public class ChampionItem : MonoBehaviour {


	public void InitContent(int idx){
		var temp = transform.GetChild(0).gameObject as GameObject;

	//	if(idx == 0){
	//		transform.name = "-1";
	//		temp.SetActive(false);return;
	//	}else if(idx == 7){
	//		transform.name = "7";
	//		temp.SetActive(false);return;
	//	}else{
	//		idx -= 1;
	//		transform.name =  idx.ToString();
	//	}
		temp.SetActive(true);
		temp.name = "Stock";
		temp.GetComponent<UIButtonMessage>().target = transform.parent.parent.parent.gameObject;

		//btn_select // locked // Info // FlagGroup // Reward // SeasonClear
		var tr = temp.transform.FindChild("Info") as Transform;
		int sLV = idx +1;
		SetChampionInfo( tr, idx);
		int mSeason = GV.ChSeason;
		if(mSeason== sLV){
			temp.transform.FindChild("btn_select").gameObject.SetActive(true);
			setCurrentSeasonInfo( tr.FindChild("FlagGroup").gameObject, GV.ChSeasonLV);
			setRewardInfo(temp, GV.ChSeasonLV);
			temp.name = "ING_"+sLV;
		}else if(mSeason > sLV){
			temp.transform.FindChild("SeasonClear").gameObject.SetActive(true);
			setCurrentSeasonInfo( tr.FindChild("FlagGroup").gameObject, 10);
			temp.transform.FindChild("Reward").gameObject.SetActive(false);
			temp.name = "Clear_"+sLV;
			temp.transform.FindChild("bg3").gameObject.SetActive(false);
		}else{
			temp.transform.FindChild("Reward").gameObject.SetActive(false);
			temp.transform.FindChild("Locked").gameObject.SetActive(true);
		//	temp.transform.FindChild("Locked").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("73146");
			temp.name = "Lock_"+sLV;
		}


	}
	void setRewardInfo(GameObject obj, int id){

		Common_Reward.Item item = Common_Reward.Get(Global.gRewardId);
		var rTr = obj.transform.FindChild("Reward") as Transform;
		rTr.FindChild("Dollor").GetComponentInChildren<UILabel>().text = string.Format("{0:#,0}", item.Champ_dollar);
		rTr.FindChild("Coin").GetComponentInChildren<UILabel>().text = string.Format("{0:#,0}", item.Champ_coin);
		rTr.FindChild("Title_Reward").GetComponent<UILabel>().text = KoStorage.GetKorString("73144");
		if(id == 5){
			rTr.FindChild("Car_Lock").gameObject.SetActive(false);
			var carTr = rTr.FindChild("Car") as Transform;
			carTr.gameObject.SetActive(true);
			int r_Car =  AccountManager.instance.ChampItem.R_car;
			carTr.FindChild("Car_img").GetComponent<UISprite>().spriteName = r_Car.ToString();
			carTr.FindChild("Car_Name").GetComponent<UILabel>().text = Common_Car_Status.Get(r_Car).Name;
			carTr.FindChild("Car_Class").GetComponent<UILabel>().text =string.Format(KoStorage.GetKorString("74024"),AccountManager.instance.ChampItem.R_class  );
		}else
			rTr.FindChild("Car_Lock").GetComponentInChildren<UILabel>().text = KoStorage.GetKorString("73145");
	}
	void setCurrentSeasonInfo(GameObject obj, int id){
		if(id == 10){
			for(int i =0; i < obj.transform.childCount;i++){
				var temp = obj.transform.GetChild(i).FindChild("Background") as Transform;
				temp.gameObject.SetActive(false);
			}
			id = 6;
		}
	//	int check = id;
		//check = 5;
		for(int i =0; i < obj.transform.childCount;i++){
			var temp = obj.transform.GetChild(i).FindChild("Check") as Transform;
			if( i < (id-1))
				temp.gameObject.SetActive(true);
		
		}
	}

	void SetChampionInfo(Transform tr, int idx){
		int seasonID = 6000+5*idx;
		Common_Mode_Champion.Item item = Common_Mode_Champion.Get(seasonID);
		Common_Track.Item trackItem = Common_Track.Get(item.Track);
		var tr1 = tr.FindChild("Mapinfo") as Transform;
		tr1.FindChild("icon_MapType").GetComponent<UISprite>().spriteName = item.Track.ToString()+"T";
		tr1.FindChild("MapName").GetComponent<UILabel>().text =  trackItem.Name;
		tr1.FindChild("MapDistance").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("73143"), trackItem.Distance);
		tr1.FindChild("icon_Map_img").GetComponent<UISprite>().spriteName = item.Track.ToString()+"P";
		tr1.FindChild("icon_Cup").GetComponent<UISprite>().spriteName = item.Track.ToString()+"L";
		tr1.FindChild("icon_Cup_s").GetComponent<UISprite>().spriteName = item.Track.ToString()+"L";
		tr1.FindChild("CupName").GetComponent<UILabel>().text = item.Name;
		tr.FindChild("Season").FindChild("Season2").GetComponent<UISprite>().spriteName = "s_"+item.Season.ToString();
	
	}
}
