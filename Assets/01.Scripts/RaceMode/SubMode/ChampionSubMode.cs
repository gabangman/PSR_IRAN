using UnityEngine;
using System.Collections;

public class ChampionSubMode : RaceMode {
	public GameObject View, Grid;
	public GameObject slotItem;
	public UIScrollBar mScroll;
	public override void OnNext (GameObject obj)
	{
		if(Global.isPopUp) return;
		string name = obj.name;
		if(name.Contains("Lock")== true) return;
		base.OnNext (obj);
		GameObject.Find("LobbyUI").SendMessage("OnChampionRaceClick",obj.name,SendMessageOptions.DontRequireReceiver);
	
	}

	public override void SetRaceSubMode(){
		string str = Grid.transform.GetChild(0).name;
		if(str == "0") return;
		InitChampion();
	}


	public void InitChampion(){
		int cnt = Grid.transform.childCount;
			for(int i =0 ; i < 6; i++){
				var temp = Grid.transform.GetChild(i).gameObject as GameObject;
				if(temp.name.Equals(i.ToString()) == false){
					temp.name = i.ToString();
					temp.AddComponent<ChampionItem>().InitContent(i);
				}
			}
			Grid.GetComponent<UIGrid>().Reposition();
			mScroll = View.transform.parent.FindChild("Window").GetComponentInChildren<UIScrollBar>();
			Invoke("SetScroll" , 0.25f);
	}

	void SetScroll(){
		int m = GV.ChSeason;
		float len = 0;
		float t = 1.0f/6;
		if(m == 6){
			t = 0;
		}else{
			
		}
		len = (float)m / 6.0f;
		len = len - t;
		if(len <0 ) len = 0;
	
		//	//!!--Utility.Log("length " + len  + "  " + t);
		mScroll.scrollValue = len;
	}

	void OnCenter(){
		Grid.GetComponent<ChampionOnCenter>().enabled = true;
	}

	void OnEnable(){
		if(mScroll == null) return;
		SetScroll();
	

	
	}
}

public partial class LobbyManager : MonoBehaviour {
	
	void OnChampionRaceClick(string str){
		if(btnstate != buttonState.WAIT) return;
		btnstate = buttonState.MAP_CHAMPION;
		fadeIn();
		strMode = str;
		OnBackFunction = ()=>{
			isModeReturn = false;
			fadeIn();
			btnstate = buttonState.MAPTORACEMODE;
		//	if(_mode != null){
		//		_mode.InfoWindowDisable();
		//		_mode.InfoWinDisable();
		//	}
			unSetRaceSubWindow();
			//	_mode.WindowDisable();
			
		};
	}
}

