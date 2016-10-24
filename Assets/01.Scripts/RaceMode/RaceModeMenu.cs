using UnityEngine;
using System.Collections;

public class RaceModeMenu : MonoBehaviour {
	
	public GameObject[] subMenu;
	public GameObject[] modeSubMenu;
	public GameObject modeSub, Sub;
	public GameObject strModeObj;
	public GameObject[] objNews;

	private GameObject ClubOn;
	void Awake(){
		ClubOn = subMenu[0].transform.FindChild("On").gameObject as GameObject;
	}
	void StartRaceMenu(){
		strModeObj = subMenu[GV.gRaceMode];
		modeSubMenu[GV.gRaceMode].SetActive(true);
		if(GV.gRaceMode == 5){
			modeSubMenu[5].SendMessage("InitChampion");
			GameObject.Find("LobbyUI").SendMessage("ShowInfoTipWindow", "Champion");
		}else if(GV.gRaceMode == 0){
			GV.gRaceMode = 5;
			modeSub.SendMessage("ChangeSubModeScene", "Club", SendMessageOptions.DontRequireReceiver);
			GameObject.Find("LobbyUI").SendMessage("ShowInfoTipWindow", "Club");
		}else {
			GV.gRaceMode = 5;
			modeSub.SendMessage("ChangeSubModeScene", "Event", SendMessageOptions.DontRequireReceiver);
			GameObject.Find("LobbyUI").SendMessage("ShowInfoTipWindow", "Event");
		}
		NewButtonEventRace();
		NewButtonRaceMenu();
		SetClubOnButton();
	

	}

	public void SetClubOnButton(){
		if(CClub.ClanMember !=0){
			if(CClub.ClubMode == 1){
				if(ClubOn.activeSelf) return;
				ClubOn.SetActive(true);
			}else{
				if(!ClubOn.activeSelf) return;
				ClubOn.SetActive(false);
			}
		}
	}
	
	void Start(){
	
	}
	private void NewButtonEventRace(){
		if(myAccount.instance.account.eRace.testDrivePlayCount == 5){
			subMenu[3].transform.FindChild("icon_New").gameObject.SetActive(false);
			
			if(AccountManager.instance.ChampItem.S2_5_Event_Featured == 1){
				if(myAccount.instance.account.eRace.featuredPlayCount == 5){
					subMenu[3].transform.FindChild("icon_New").gameObject.SetActive(false);
					
					if(AccountManager.instance.ChampItem.S4_1_Event_Evocube == 1){
						if(myAccount.instance.account.eRace.EvoAcquistCount == 1){
							subMenu[3].transform.FindChild("icon_New").gameObject.SetActive(false);
						}else{
							subMenu[3].transform.FindChild("icon_New").gameObject.SetActive(true);
						}
					}
					
					
				}else{
					subMenu[3].transform.FindChild("icon_New").gameObject.SetActive(true);
				}
			}
			
			
		}else{
			subMenu[3].transform.FindChild("icon_New").gameObject.SetActive(true);
		}
	}
	
	private void NewButtonRaceMenu(){
		bool bNew = myAcc.instance.account.bRaceMenuBTN[0]; //챔피언스
		subMenu[5].transform.FindChild("icon_New").gameObject.SetActive(bNew);
		
		bNew = myAcc.instance.account.bRaceMenuBTN[1]; //weekly
		subMenu[1].transform.FindChild("icon_New").gameObject.SetActive(bNew);
		
		bNew = myAcc.instance.account.bRaceMenuBTN[2]; //PVP
		subMenu[2].transform.FindChild("icon_New").gameObject.SetActive(bNew);
		
		bNew = myAcc.instance.account.bRaceMenuBTN[3]; //Regular
		subMenu[4].transform.FindChild("icon_New").gameObject.SetActive(bNew);
		
		bNew = myAcc.instance.account.bRaceMenuBTN[4]; //club
		subMenu[0].transform.FindChild("icon_New").gameObject.SetActive(bNew);
		
	}
	
	private void NewButtonRaceSubMenu(){
		objNews[0].SetActive(myAcc.instance.account.bRaceSubBTN[0]); // regular track
		objNews[1].SetActive(myAcc.instance.account.bRaceSubBTN[1]); //regular drag
		objNews[2].SetActive(myAcc.instance.account.bRaceSubBTN[2]); //pvp drag
		objNews[3].SetActive(myAcc.instance.account.bRaceSubBTN[3]); //pvp city
		//objNews[4].SetActive(myAcc.instance.account.bRaceSubBTN[4]); //클럽
		SetClubOnButton();
	}
	
	private void NewBTNActive(){
		bool bNew = myAcc.instance.account.bRaceMenuBTN[0]; //챔피언스
		if(bNew) { myAcc.instance.account.bRaceMenuBTN[0]=false;bNew = false; subMenu[5].transform.FindChild("icon_New").gameObject.SetActive(bNew);}
		
		bNew = myAcc.instance.account.bRaceMenuBTN[1]; //weekly
		if(bNew) {myAcc.instance.account.bRaceMenuBTN[1]=false;bNew = false; subMenu[1].transform.FindChild("icon_New").gameObject.SetActive(bNew);}
		
		bNew = myAcc.instance.account.bRaceMenuBTN[2]; //PVP
		if(bNew) {myAcc.instance.account.bRaceMenuBTN[2]=false;bNew = false; subMenu[2].transform.FindChild("icon_New").gameObject.SetActive(bNew);}
		
		bNew = myAcc.instance.account.bRaceMenuBTN[3]; //Regular
		if(bNew) {myAcc.instance.account.bRaceMenuBTN[3]=false;bNew = false; subMenu[4].transform.FindChild("icon_New").gameObject.SetActive(bNew);}
		
		bNew = myAcc.instance.account.bRaceMenuBTN[4]; //Club
		if(bNew) {
			if(CClub.ClubMode == 0 || CClub.ClubMode == 3){
				myAcc.instance.account.bRaceMenuBTN[4]=false;bNew = false; subMenu[0].transform.FindChild("icon_New").gameObject.SetActive(bNew);
			}
		}
		SetClubOnButton();
	}
	
	void OnEnable(){
		NewButtonRaceSubMenu();
	}
	
	public void InitButtonStatus(){
		for(int i = 0; i < subMenu.Length; i++){
			subMenu[i].transform.FindChild("btnImage").gameObject.SetActive(false);
		}
		
		subMenu[GV.gRaceMode].transform.FindChild("btnImage").gameObject.SetActive(true);
		strModeObj = subMenu[GV.gRaceMode];
		
		for(int i = 0; i < modeSubMenu.Length-1; i++){
			modeSubMenu[i].SetActive(false);
		}
		modeSubMenu[GV.gRaceMode].SetActive(true);
		StartRaceMenu();

	}
	
	void OnRaceClick(GameObject obj){
		if(Global.isPopUp) return;
		if(strModeObj == obj.transform.parent.gameObject) return;
		GameObject.Find("LobbyUI").SendMessage("HiddenInfoTipWindow");
		string selectmode = obj.transform.parent.name;
		strModeObj.transform.FindChild("btnImage").gameObject.SetActive(false);
		strModeObj= obj.transform.parent.gameObject;
		strModeObj.transform.FindChild("btnImage").gameObject.SetActive(true);
		modeSub.SendMessage("ChangeSubModeScene", selectmode, SendMessageOptions.DontRequireReceiver);
		NewBTNActive();
	}
	public void SetObjectDisable(){
		gameObject.SetActive(false);
	}
}
