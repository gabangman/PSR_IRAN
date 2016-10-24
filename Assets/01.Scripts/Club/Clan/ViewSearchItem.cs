using UnityEngine;
using System.Collections;

public class ViewSearchItem : SlotItems {
	void Start(){
		transform.FindChild("Btn_SignIn").FindChild("lbtitle").GetComponent<UILabel>().text
			= KoStorage.GetKorString("77206");
		transform.FindChild("Btn_Visit").FindChild("lbtitle").GetComponent<UILabel>().text 
			= KoStorage.GetKorString("77311");
	}
	public void ViewSearchContent(int idx){
	}

	
	public override void ChangeContents (int idx)
	{
		int listCount = CClub.mSeachList.Count;
		if(listCount <= idx){
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}

		ClubInfoSearch mClub = CClub.mSeachList[idx];

		var tr = transform.FindChild("Info") as Transform;
		tr.FindChild("img_ClanSymbol").GetComponent<UISprite>().spriteName = mClub.clubSymbol;
		tr.FindChild("lb_ClanName").GetComponent<UILabel>().text = mClub.mClubName;
		tr.FindChild("lb_Num").GetComponent<UILabel>().text = string.Format("{0}/{1}", mClub.clubMember, CClub.MaxMember);
		tr.FindChild("lb_Victory").GetComponent<UILabel>().text = string.Format("{0:#,0}",mClub.clubVictoryNum);//mClub.clubVictoryNum.ToString();
		tr.FindChild("lb_Text").GetComponent<UILabel>().text = mClub.clubDescription;
		tr.FindChild("lb_LV").GetComponent<UILabel>().text ="LV "+ mClub.clubLevel.ToString();
		tr.FindChild("lb_Local").GetComponent<UILabel>().text = mClub.mLocale;
		tr.FindChild("lb_Rank").GetComponent<UILabel>().text = string.Format("{0}", (idx+1));


		tr = transform.FindChild("Btn_SignIn");
	

		if(CClub.ClanMember !=0){
			tr.gameObject.SetActive(false);
			if(mClub.clubIndex == CClub.mClubInfo.clubIndex){
				//		if(!tr.gameObject.activeSelf) return;
				//		tr.gameObject.SetActive(false);
				transform.FindChild("Btn_Visit").gameObject.SetActive(false);
			}else{
				//		if(tr.gameObject.activeSelf) return;
				//		tr.gameObject.SetActive(true);
				transform.FindChild("Btn_Visit").gameObject.SetActive(true);
			}
		}else{
			if(tr.gameObject.activeSelf) return;
			tr.gameObject.SetActive(true);
			transform.FindChild("Btn_Visit").gameObject.SetActive(true);
		}
		
		if(tr.gameObject.activeSelf) {
			if(mClub.clubMember >= CClub.MaxMember){
				tr.gameObject.SetActive(false);
			}
		}
	
	}

	void OnSign(){
		//AccountManager.instance.onSignClan(gameObject.name);
		string[] str = transform.name.Split('_');
		int cnt = int.Parse(str[1]);
		NGUITools.FindInParents<ClanWindow>(gameObject).OnSignIn(transform.name,CClub.mSeachList[cnt].clubIndex, CClub.mSeachList[cnt].mClubName);
	}
	
	void OnVisit(){
		//AccountManager.instance.onVisitClan(gameObject.name);
		string[] str = transform.name.Split('_');
		int cnt = int.Parse(str[1]);
		NGUITools.FindInParents<ClanWindow>(gameObject).OnClanVisit(transform.name,CClub.mSeachList[cnt].clubIndex);
	}

}
