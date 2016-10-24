using UnityEngine;
using System.Collections;

public class ClanMemberClick : MonoBehaviour {
	void Start(){
		if(viewInfo == null) {
			return;
		}
		viewInfo.FindChild("Entree").GetComponent<UIButtonMessage>().target = gameObject;
		viewInfo.FindChild("Entree").GetComponent<UIButtonMessage>().functionName = "OnEntryFlag";
		viewInfo.FindChild("Kick_On").GetComponent<UIButtonMessage>().target = gameObject;
		//viewInfo.FindChild("Kick_On").GetComponent<UIButtonMessage>().functionName = "OnKick";
		viewInfo.FindChild("Staff").GetComponent<UIButtonMessage>().functionName = "OnStaff";
		viewInfo.FindChild("Staff").GetComponent<UIButtonMessage>().target = gameObject;


		viewInfo.FindChild("Entree").FindChild("On").FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("77111");
		viewInfo.FindChild("Entree").FindChild("Off").FindChild("lbText").GetComponent<UILabel>().text =  KoStorage.GetKorString("77112");
		viewInfo.FindChild("Kick_On").FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("77227");
		viewInfo.FindChild("Kick_Off").FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("77227");
		//viewInfo.FindChild("Staff").FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("77226");
		viewInfo.FindChild("Staff").GetChild(0).FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("77223");
		viewInfo.FindChild("Staff").GetChild(1).FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("77223");
	}
	public void disableSelect(){
		if(viewInfo == null) return;
		viewInfo.gameObject.SetActive(false);
		int cnt = transform.childCount;
		for(int i = 0; i < cnt ; i++){
		 transform.GetChild(i).FindChild("Select").gameObject.SetActive(false);
		}
	}

	void OnDisable(){
		disableSelect();
	}

	public void SetViewInfo(Transform tr){
		if(viewInfo == null){
			viewInfo = tr;
			viewInfo.FindChild("Entree").GetComponent<UIButtonMessage>().target = gameObject;
			viewInfo.FindChild("Entree").GetComponent<UIButtonMessage>().functionName = "OnEntryFlag";
			viewInfo.FindChild("Kick_On").GetComponent<UIButtonMessage>().target = gameObject;
			viewInfo.FindChild("Staff").GetComponent<UIButtonMessage>().functionName = "OnStaff";
			viewInfo.FindChild("Staff").GetComponent<UIButtonMessage>().target = gameObject;
			viewInfo.FindChild("Entree").FindChild("On").FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("77111");
			viewInfo.FindChild("Entree").FindChild("Off").FindChild("lbText").GetComponent<UILabel>().text =  KoStorage.GetKorString("77112");
			viewInfo.FindChild("Kick_On").FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("77227");
			viewInfo.FindChild("Kick_Off").FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("77227");
			viewInfo.FindChild("Staff").GetChild(0).FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("77223");
			viewInfo.FindChild("Staff").GetChild(1).FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("77223");
		}
	}

	public void unSetViewInfo(){
		viewInfo.name = "BtnMember";
		if(viewInfo.gameObject.activeSelf) viewInfo.gameObject.SetActive(false);
		viewInfo = null;

	}

	public Transform viewInfo;
	public void ViewClanMemberInfo(string name){
		viewInfo.name = name;
		viewInfo.gameObject.SetActive(true);
	

		string[] str = name.Split('_');
		int cnt = int.Parse(str[1]);
		ClubMemberInfo mClub= CClub.myClubMemInfo[cnt];
		if(mClub.myMemInfo == 1){
			viewInfo.FindChild("Entree").gameObject.SetActive(true);
			if(CClub.mClubInfo.myEntryFlag == 1){
				viewInfo.FindChild("Entree").FindChild("On").gameObject.SetActive(true);
				viewInfo.FindChild("Entree").FindChild("Off").gameObject.SetActive(false);
			}else{
				viewInfo.FindChild("Entree").FindChild("On").gameObject.SetActive(false);
				viewInfo.FindChild("Entree").FindChild("Off").gameObject.SetActive(true);
			}
		}else{
			viewInfo.FindChild("Entree").gameObject.SetActive(false);
		}

		if(CClub.ClanMember == 1){
			if(mClub.myMemInfo  == 1){
				viewInfo.FindChild("Kick_On").gameObject.SetActive(false);
				viewInfo.FindChild("Kick_Off").gameObject.SetActive(false);
				viewInfo.FindChild("Staff").gameObject.SetActive(false);
			}else{
				viewInfo.FindChild("Staff").gameObject.SetActive(true);
				if(mClub.clubMember == 5){
					viewInfo.FindChild("Staff").GetChild(0).gameObject.SetActive(false);
					viewInfo.FindChild("Staff").GetChild(1).gameObject.SetActive(true);
				}else{
					viewInfo.FindChild("Staff").GetChild(0).gameObject.SetActive(true);
					viewInfo.FindChild("Staff").GetChild(1).gameObject.SetActive(false);
				}
				if(CClub.ClubMode == 1 || CClub.ClubMode == 3){
					viewInfo.FindChild("Kick_On").gameObject.SetActive(false);
					viewInfo.FindChild("Kick_Off").gameObject.SetActive(true);
				}else{
					viewInfo.FindChild("Kick_On").gameObject.SetActive(true);
					viewInfo.FindChild("Kick_Off").gameObject.SetActive(false);
				}

			}
		}else if(CClub.ClanMember == 2){
			viewInfo.FindChild("Staff").gameObject.SetActive(false);
			if(CClub.ClubMode == 1 || CClub.ClubMode == 3){
				viewInfo.FindChild("Kick_On").gameObject.SetActive(false);
				viewInfo.FindChild("Kick_Off").gameObject.SetActive(true);
			}else{
				viewInfo.FindChild("Kick_On").gameObject.SetActive(true);
				viewInfo.FindChild("Kick_Off").gameObject.SetActive(false);
			}

			if(mClub.myMemInfo  == 1 || mClub.clubMember == 9){
				viewInfo.FindChild("Kick_On").gameObject.SetActive(false);
				viewInfo.FindChild("Kick_Off").gameObject.SetActive(false);
				viewInfo.FindChild("Staff").gameObject.SetActive(false);
			}
		}else{
			viewInfo.FindChild("Kick_On").gameObject.SetActive(false);
			viewInfo.FindChild("Kick_Off").gameObject.SetActive(false);
			viewInfo.FindChild("Staff").gameObject.SetActive(false);
		}

		// 0 : no member , 1 : clanMaseter, 2 : clanStaff, 3: ClanMember

	}

	public void OnEntryFlag(){
		string[] str = viewInfo.transform.name.Split('_');
		int cnt = int.Parse(str[1]);
		ClubMemberInfo mClub= CClub.myClubMemInfo[cnt];
		NGUITools.FindInParents<ClanWindow>(gameObject).OnEntryFlag(()=>{
			if(CClub.mClubInfo.myEntryFlag == 1){
				viewInfo.FindChild("Entree").FindChild("On").gameObject.SetActive(true);
				viewInfo.FindChild("Entree").FindChild("Off").gameObject.SetActive(false);
				mClub.mUserEntryFlag = 1;
			}else{
				viewInfo.FindChild("Entree").FindChild("On").gameObject.SetActive(false);
				viewInfo.FindChild("Entree").FindChild("Off").gameObject.SetActive(true);
				mClub.mUserEntryFlag = 0;
			}
			NGUITools.FindInParents<ClanWindow>(gameObject).EntryFlagResults(cnt);
		});
	
	}

	public void OnKick(){
		//if(CClub.ClubMode == 1) return;
		string[] str =viewInfo.transform.name.Split('_');
		int cnt = int.Parse(str[1]);
		ClubMemberInfo mClub= CClub.myClubMemInfo[cnt];
		NGUITools.FindInParents<ClanWindow>(gameObject).OnKickMember(()=>{
			//CClub.myClubMemInfo.RemoveAt(cnt);
			if(!CClub.bClanWarChat){
				string str1 = string.Format(KoStorage.GetKorString("77205"), mClub.nickName);
				UserDataManager.instance.JiverSend(str1);
			}
			CClub.myClubMemInfo.Remove(mClub);
			disableSelect();
			CClub.mClubInfo.clubMemberNum--;

		}, mClub.userId);
	}

	public void OnStaff(){
		string[] str = viewInfo.transform.name.Split('_');
		int cnt = int.Parse(str[1]);
		ClubMemberInfo mClub= CClub.myClubMemInfo[cnt];
		if(mClub.clubMember == 5){
			NGUITools.FindInParents<ClanWindow>(gameObject).OnStaffOutMember(()=>{
				CClub.myClubMemInfo[cnt].clubMember = 1;
				viewInfo.FindChild("Staff").GetChild(0).gameObject.SetActive(true);
				viewInfo.FindChild("Staff").GetChild(1).gameObject.SetActive(false);
			},cnt);
		}else if(mClub.clubMember == 9){
		
		}else{
			NGUITools.FindInParents<ClanWindow>(gameObject).OnStaffMember(()=>{
				CClub.myClubMemInfo[cnt].clubMember = 5;
				//viewInfo.FindChild("Staff").gameObject.SetActive(true);
				viewInfo.FindChild("Staff").GetChild(0).gameObject.SetActive(false);
				viewInfo.FindChild("Staff").GetChild(1).gameObject.SetActive(true);
			}, cnt);
		}
	}

	public void OnStaffOut(){
		string[] str = viewInfo.transform.name.Split('_');
		int cnt = int.Parse(str[1]);
		ClubMemberInfo mClub= CClub.myClubMemInfo[cnt];
		NGUITools.FindInParents<ClanWindow>(gameObject).OnStaffOutMember(()=>{
			CClub.myClubMemInfo[cnt].clubMember = 1;
			viewInfo.FindChild("Staff").gameObject.SetActive(true);
		},cnt);
	}
}
