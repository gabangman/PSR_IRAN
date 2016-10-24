using UnityEngine;
using System.Collections;

public class ModeChampion : ModeParent {
	int clearSeasonID;
	public override void OnSubWindow ()
	{
		base.OnSubWindow ();
		transform.parent.GetComponent<RaceModeAnimaintion>().SelectAniPlay("ModeWin_Championship");
		string name = transform.parent.name;
	//	//!!--Utility.Log("On Champion " + name);
		string[] str = name.Split('_');
		GameObject obj = null;
		GV.gEntryFee = 0;
		if(str[1].Equals("ING")){
			obj = transform.GetChild(0).gameObject;// as GameObject;
			obj.SetActive(true);
			SetCurrentWindow(obj);
			onNext = ()=>{
				//!!--Utility.Log("onNext ");
				if(GV.ChSeasonID == 6000){
					Global.gChampTutorial = 1;
					GameObject.Find("LobbyUI").SendMessage("StartTutorial");
				}else if( GV.ChSeasonID <= 6001){
					Global.gChampTutorial = 2;
					ReadyLoadingCircle();
					gameObject.AddComponent<SettingRaceChamp>().setChampRace();
				}else{
					Global.gChampTutorial = 0;
					ReadyLoadingCircle();
					gameObject.AddComponent<SettingRaceChamp>().setChampRace();
				}

			};
		}else{
			/*
			obj = transform.GetChild(0).gameObject;// as GameObject;
			obj.SetActive(true);
			clearSeasonID = 0;
			SetClearWindow(obj, str[2]);
			//Utility.LogWarning("clear seasonID " + clearSeasonID);
			onNext = ()=>{
				//!!--Utility.Log("onNext ");
			//	ReadyLoadingCircle();
			//	gameObject.AddComponent<SettingRaceChamp>().setChampClearRace(clearSeasonID);
			};	*/
		}

	
	
	}

	void SetCurrentWindow(GameObject obj){
		Common_Mode_Champion.Item item = Common_Mode_Champion.Get(GV.ChSeasonID);
		SetBGTrackImg(item.Track);
		obj.transform.FindChild("G_Center_CHA").FindChild("icon_crew").GetComponent<UISprite>().spriteName = item.Crew.ToString()+"A";
		obj.transform.FindChild("G_Center_Car").FindChild("icon_car").GetComponent<UISprite>().spriteName =item.Car.ToString();
		obj.transform.FindChild("G_Center_Car").FindChild("lbCarName").GetComponent<UILabel>().text = Common_Car_Status.Get(item.Car).Name;
		int SLV = GV.ChSeasonLV-1;
		var temp = obj.transform.FindChild("G_Center_Flag") as Transform;
		temp.gameObject.SetActive(true);
		for(int i = 0; i < temp.childCount; i++){
			var tr = temp.GetChild(i) as Transform;
			var tr1 = tr.FindChild("Check_Arrow") as Transform;
			tr1.gameObject.SetActive(false);
			var tr2 = tr.FindChild("Check") as Transform;
			tr2.gameObject.SetActive(false);
			if(SLV == i)
				tr1.gameObject.SetActive(true);
			if(SLV  >  i)
				tr2.gameObject.SetActive(true);
		}
		int num = (GV.ChSeasonID-6000)+73102;
		string strnum = num.ToString();
		obj.transform.FindChild("G_Center_BG").GetComponentInChildren<UILabel>().text =KoStorage.GetKorString(strnum);
		entryDollar = 0;
		refreshDollar = 0;
	}

	void SetClearWindow(GameObject obj, string strSeason){
		int s = int.Parse(strSeason);
		int sID = 5999+s*5;
		clearSeasonID = sID;
		Common_Mode_Champion.Item item = Common_Mode_Champion.Get(sID);
		SetBGTrackImg(item.Track);
		obj.transform.FindChild("G_Center_CHA").FindChild("icon_crew").GetComponent<UISprite>().spriteName = item.Crew.ToString()+"A";
		obj.transform.FindChild("G_Center_Car").FindChild("icon_car").GetComponent<UISprite>().spriteName =item.Car.ToString();
		obj.transform.FindChild("G_Center_Car").FindChild("lbCarName").GetComponent<UILabel>().text = Common_Car_Status.Get(item.Car).Name;
		var temp = obj.transform.FindChild("G_Center_Flag") as Transform;
		temp.gameObject.SetActive(false);
		int num  = 73149 + Random.Range(0,4);
		obj.transform.FindChild("G_Center_BG").GetComponentInChildren<UILabel>().text =KoStorage.GetKorString(num.ToString());
		entryDollar = 0;
		refreshDollar = 0;
	}
}
