using UnityEngine;
using System.Collections;

public class ViewRankItem : SlotItems {

	void Start(){
		transform.FindChild("Btn_SignIn").FindChild("lbtitle").GetComponent<UILabel>().text
			= KoStorage.GetKorString("77206");
		transform.FindChild("Btn_Visit").FindChild("lbtitle").GetComponent<UILabel>().text 
			= KoStorage.GetKorString("77311");
	}

	public void ViewRankContent(int idx){
			
	}

	public void ChangeContentIndex(int mode){
		string[] str = transform.name.Split('_');
		int cnt = int.Parse(str[1]);

		if(mode == 0){
		//	rankmode = 0;
			ChangeContentsLocal(cnt);
		}else{
		//	rankmode = 1;
			ChangeContentsGlobal(cnt);
		}
	}

	public override void ChangeContents(int idx){
	}
	private int rankmode;
	public  void ChangeContentsLocal (int idx)
	{
		rankmode = 0;
		int listCount = CClub.mRankingLocal.Count;
		if(listCount <= idx){
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}

		ChangeInfo(CClub.mRankingLocal[idx], idx);

	}

	public  void ChangeContentsGlobal (int idx)
	{
		rankmode = 1;
		int listCount = CClub.mRankingGlobal.Count;
		if(listCount <= idx){
			gameObject.SetActive(false);
			return;
		}else{
			gameObject.SetActive(true);
		}
		ChangeInfo(CClub.mRankingGlobal[idx],idx);
	}

	void ChangeInfo(ClubInfoRanking mRank, int idx){
		var tr =transform.FindChild("Info") as Transform;

		tr.FindChild("lb_LV").GetComponent<UILabel>().text = string.Format("LV {0}", mRank.clubLevel);
		tr.FindChild("lb_ClanName").GetComponent<UILabel>().text = mRank.mClubName;
		tr.FindChild("lb_Local").GetComponent<UILabel>().text = mRank.mLocale;
		tr.FindChild("lb_Text").GetComponent<UILabel>().text = mRank.clubDescription;
		tr.FindChild("lb_Num").GetComponent<UILabel>().text = string.Format("{0}/{1}",mRank.clubMemberNum, CClub.MaxMember);
		tr.FindChild("lb_Victory").GetComponent<UILabel>().text = string.Format("{0:#,0}",mRank.clubVistoryNum);
		tr.FindChild("img_ClanSymbol").GetComponent<UISprite>().spriteName = mRank.clubSymbol;
		tr.FindChild("lb_Rank").GetComponent<UILabel>().text = (idx+1).ToString();

		tr = transform.FindChild("Btn_SignIn");


		if(CClub.ClanMember !=0){
			tr.gameObject.SetActive(false);
			if(mRank.clubIndex == CClub.mClubInfo.clubIndex){
		//		if(!tr.gameObject.activeSelf) return;
		//		tr.gameObject.SetActive(false);
				transform.FindChild("Btn_Visit").gameObject.SetActive(false);
			}else{
		//		if(tr.gameObject.activeSelf) return;
		//		tr.gameObject.SetActive(true);
				transform.FindChild("Btn_Visit").gameObject.SetActive(true);
			}
		}else{
			if(tr.gameObject.activeSelf)
			{}else{
			tr.gameObject.SetActive(true);
			transform.FindChild("Btn_Visit").gameObject.SetActive(true);
			}
		}

		if(tr.gameObject.activeSelf) {
			if(mRank.clubMemberNum >= CClub.MaxMember){
				tr.gameObject.SetActive(false);
			}
		}

	}

	void OnSign(){
		string[] str = transform.name.Split('_');
		int clubidx = int.Parse(str[1]);
		string clubname = string.Empty;
		if(rankmode == 0) {
			clubname = CClub.mRankingLocal[clubidx].mClubName;
			clubidx = CClub.mRankingLocal[clubidx].clubIndex;
		}
		else{
			clubname = CClub.mRankingGlobal[clubidx].mClubName;
			clubidx = CClub.mRankingGlobal[clubidx].clubIndex;

		}

		NGUITools.FindInParents<ClanWindow>(gameObject).OnSignIn(transform.name,clubidx, clubname);
	}

	void OnVisit(){
		string[] str = transform.name.Split('_');
		int clubidx = int.Parse(str[1]);
		if(rankmode == 0) clubidx = CClub.mRankingLocal[clubidx].clubIndex;
		else clubidx = CClub.mRankingGlobal[clubidx].clubIndex;
		NGUITools.FindInParents<ClanWindow>(gameObject).OnClanVisit(transform.name,clubidx);
	}
}
