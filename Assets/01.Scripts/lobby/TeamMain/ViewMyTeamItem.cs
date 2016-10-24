using UnityEngine;
using System.Collections;

public class ViewMyTeamItem : MonoBehaviour {

	private GameObject selectobj, selectInfo;

	void Awake(){
		selectobj = transform.GetChild(0).FindChild("SetEntry").gameObject;
		selectInfo = transform.GetChild(0).FindChild("SelectInfo").gameObject;
		selectInfo.SetActive(false);
		selectobj.SetActive(false);
		//selected : 현재 선택딘 팀으로 활성화시키고
		//selectedinfo : 클릭마다 나오게 처리함. 
	}
	void Start(){
		//if(icon_select == null)	icon_select = transform.GetChild(0).FindChild("BG_select").gameObject;
		//if(icon_bg == null)	icon_bg = transform.GetChild(0).FindChild("icon_Selected").gameObject;

	}
	public void ViewTeamContentInClub(int idx){
		transform.GetChild(0).GetComponent<UIButtonMessage>().functionName = "OnTeamClick";
		ViewTeamContent(idx);
	}

	public void unSetSelectTeam(){
		selectInfo.SetActive(false);
		selectobj.SetActive(false);
	}
	
	void OnTeamClick(GameObject obj){
		Utility.LogWarning("obj " + obj.transform.parent.name);
		if(selectInfo.activeSelf) return;
		NGUITools.FindInParents<ClanMatchGrid>(gameObject).UnSelectedInfo();
		NGUITools.FindInParents<ModeClan>(gameObject).OnSetClubTeam(obj.transform.parent.name);
		selectInfo.SetActive(true);
		selectobj.SetActive(true);
		//TeamSponCheck(obj.transform.parent.name);
	}


	public void SetTeamSpon(){
		var tr = transform.GetChild(0) as Transform;
		tr = tr.FindChild("info_Car_Yes");
		if(!tr.gameObject.activeSelf) Utility.LogWarning("");
		var mSpon = tr.FindChild("Info_Spon") as Transform;
		myTeamInfo mTeam  = GV.getTeamTeamInfo(GV.SelectedTeamID);
		if(mTeam.SponID <= 1300){
			mSpon.FindChild("icon_spon").gameObject.SetActive(false);//GetComponent<UISprite>().spriteName =myteam.SponID.ToString();
			mSpon.FindChild("lbName_spon").GetComponent<UILabel>().text = KoStorage.GetKorString("72020");
		}else{
			System.DateTime nowTime = NetworkManager.instance.GetCurrentDeviceTime();
			System.TimeSpan pTime = new System.DateTime(mTeam.SponRemainTime) -nowTime;
			if(pTime.TotalHours >0){
				mSpon.FindChild("icon_spon").gameObject.SetActive(true);
				mSpon.FindChild("icon_spon").GetComponent<UISprite>().spriteName =mTeam.SponID.ToString();
				mSpon.FindChild("lbName_spon").GetComponent<UILabel>().text =  string.Format(KoStorage.GetKorString("72019"), Common_Sponsor_Status.Get(mTeam.SponID).Name);
			}else{
				mSpon.FindChild("icon_spon").gameObject.SetActive(false);//GetComponent<UISprite>().spriteName =myteam.SponID.ToString();
				mSpon.FindChild("lbName_spon").GetComponent<UILabel>().text = KoStorage.GetKorString("72020");
				mTeam.SponID = 1300;
			}
		}
	}


	public void ViewTeamContent(int idx){
		string[] str = transform.name.Split('_');
		int id = int.Parse(str[1]);
		int cnt = Common_Team.StockTeamList.Count+Common_Team.TourTeamList.Count;
		if(id >= (10+cnt)){
			var mTr = transform.GetChild(0) as Transform;
			mTr.GetChild(0).GetComponent<UISprite>().alpha = 0.0f;
			return;
		}
		Common_Team.Item item = Common_Team.Get(id);
		var tr = transform.GetChild(0) as Transform;
		bool b = true;
		int count  = GV.ChSeason;
		//count = 3; info_Car_Yes // info_Car_No, // info_Locked //RaceComplete
		if(item.ReqLV > count){
			var child = transform.GetChild(0).FindChild("info_Locked") as Transform;
			child.gameObject.SetActive(true);
			child.FindChild("icon_crew").GetComponent<UISprite>().spriteName = item.Crew.ToString();
			child.FindChild("lbName_Crew").GetComponent<UILabel>().text = item.Name;
			child.FindChild("lbText").GetComponent<UILabel>().text  = string.Format(KoStorage.GetKorString("76032"),item.ReqLV);
			child.FindChild("icon_crewSymbol").GetComponent<UISprite>().spriteName = item.Crew+"L";
			b = false;
		}
		if(b) ChangeStockTeamItem(tr,id);
	
	}

	public void ViewTeamStockContent(int idx){
		int cnt = Common_Team.Get(idx).ReqLV;
		if(GV.ChSeason < cnt){
			gameObject.SetActive(true);
		}else gameObject.SetActive(true);

		string[] str = transform.name.Split('_');
		int id = int.Parse(str[1]);

		Common_Team.Item item = Common_Team.Get(id);
		var tr = transform.GetChild(0) as Transform;
		ChangeStockTeamItem(tr, id);

	}

	public void SelectTeamCarComplete(){
		string[] str = transform.name.Split('_');
		int id = int.Parse(str[1]);
		var tr = transform.GetChild(0) as Transform;
		myTeamInfo myteam1  = GV.getTeamTeamInfo(id);
		SetTeamYes( tr.FindChild("info_Car_Yes"), id, myteam1);
		selectInfo.SetActive(true);

	}

	public void UnSelectTeamCarComplete(){
	
	}

	public void SetTradeTeamComplete(){
		var tr = transform.GetChild(0) as Transform;
	//	var mCar =tr.FindChild("info_Car_No").FindChild("Info_Car") as Transform;
	//	mCar.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("76033");
		string[] str = transform.name.Split('_');
		int idx = int.Parse(str[1]);
		SetTeamNo( tr.FindChild("info_Car_No"), idx, Common_Team.Get(idx), true);
		tr.FindChild("info_UnLocked").gameObject.SetActive(false);

	}

	public void SetTeamCarComplete(){
		string[] str = transform.name.Split('_');
		int id = int.Parse(str[1]);
		var tr = transform.GetChild(0) as Transform;
		myTeamInfo myteam1  = GV.getTeamTeamInfo(id);
		tr.FindChild("info_Car_No").gameObject.SetActive(false);
		SetTeamYes( tr.FindChild("info_Car_Yes"), id, myteam1);
	
	}

	public void ChangeTeamAbility(){
		var tr = transform.GetChild(0).FindChild("info_Car_Yes") as Transform;
		var mCar = tr.FindChild("Info_Car") as Transform;
		var mCrew = tr.FindChild("Info_Crew") as Transform;
		myTeamInfo myteam = GV.getTeamTeamInfo(GV.SelectedTeamID);
		int id = myteam.TeamCarID;
		mCar.FindChild("lbAble_Car").GetComponent<UILabel>().text = CarAbility(id ,GV.SelectedTeamID ).ToString();
		id = myteam.TeamCrewID;
		mCrew.FindChild("lbAble_Crew").GetComponent<UILabel>().text = CrewAbility(id , GV.SelectedTeamID).ToString();

	
	}

	void SetTeamNo(Transform tr, int idx, Common_Team.Item mTeam, bool b){
		tr.gameObject.SetActive(true);
		var mCar = tr.FindChild("Info_Car") as Transform;
		var mCrew = tr.FindChild("Info_Crew") as Transform;
		if(b){
			mCar.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("76033");
		}else{
			mCar.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("76034");
		}
		int crewid = mTeam.Crew;
		Common_Crew_Status.Item crewItem = Common_Crew_Status.Get(crewid);
		
		mCrew.FindChild("lbName_Crew").GetComponent<UILabel>().text = crewItem.Name;
		mCrew.FindChild("lbAble_Crew").GetComponent<UILabel>().text = CrewAbility(crewid ,int.Parse(mTeam.ID)).ToString();
		mCrew.FindChild("icon_crew").GetComponent<UISprite>().spriteName = crewid.ToString()+"A";
	
	}

	void SetTeamYes(Transform tr, int idx, myTeamInfo mTeam){
		tr.gameObject.SetActive(true);
		var mCar = tr.FindChild("Info_Car") as Transform;
		var mCrew = tr.FindChild("Info_Crew") as Transform;
		var mSpon = tr.FindChild("Info_Spon") as Transform;

		int carid = mTeam.TeamCarID;
		Common_Car_Status.Item carItem;
		Common_Crew_Status.Item crewItem;
		carItem = Common_Car_Status.Get(carid);
		mCar.FindChild("icon_car").GetComponent<UISprite>().spriteName = carid.ToString();
		mCar.FindChild("lbName_Car").GetComponent<UILabel>().text = carItem.Name;
		mCar.FindChild("lbAble_Car").GetComponent<UILabel>().text = CarAbility(carid , mTeam.TeamCode).ToString();
		mCar.FindChild("lbClass_Car").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74024"), mTeam.myCar.ClassID);
		int crewid = mTeam.TeamCrewID;
		crewItem = Common_Crew_Status.Get(crewid);

		mCrew.FindChild("lbName_Crew").GetComponent<UILabel>().text = crewItem.Name;
		mCrew.FindChild("lbAble_Crew").GetComponent<UILabel>().text = CrewAbility(crewid , mTeam.TeamCode).ToString();
		mCrew.FindChild("icon_crew").GetComponent<UISprite>().spriteName = crewid.ToString()+"A";

		if(mTeam.SponID <= 1300){
			mSpon.FindChild("icon_spon").gameObject.SetActive(false);//GetComponent<UISprite>().spriteName =myteam.SponID.ToString();
			mSpon.FindChild("lbName_spon").GetComponent<UILabel>().text = KoStorage.GetKorString("72020");
		}else{
			mSpon.FindChild("icon_spon").gameObject.SetActive(true);
			mSpon.FindChild("icon_spon").GetComponent<UISprite>().spriteName =mTeam.SponID.ToString();
			mSpon.FindChild("lbName_spon").GetComponent<UILabel>().text =  string.Format(KoStorage.GetKorString("72019"), Common_Sponsor_Status.Get(mTeam.SponID).Name);

		}
	}

	void ChangeStockTeamItem(Transform tr, int idx){
		myTeamInfo myteam1  = GV.getTeamTeamInfo(idx);
		if(myteam1 != null){
			if(myteam1.TeamCarID == 0) SetTeamNo( tr.FindChild("info_Car_No"), idx, Common_Team.Get(idx), true);
			else SetTeamYes( tr.FindChild("info_Car_Yes"), idx, myteam1);
			if(idx == GV.SelectedTeamID) {
				selectobj.SetActive(true);
				selectInfo.SetActive(true);
			}
		}else{
			Common_Team.Item item = Common_Team.Get(idx);
			var child = transform.GetChild(0).FindChild("info_UnLocked") as Transform;
			child.gameObject.SetActive(true);
			child.FindChild("icon_crew").GetComponent<UISprite>().spriteName = item.Crew.ToString();
			child.FindChild("lbName_Crew").GetComponent<UILabel>().text = item.Name;
			child.FindChild("icon_crewSymbol").GetComponent<UISprite>().spriteName = item.Crew+"L";
		}

}

	void ChangeTourTeamItem(Transform tr, int idx){
		}

	public void ViewTeamTourContent(int idx){
		int cnt = Common_Team.Get(idx).ReqLV;
		if(GV.ChSeason < cnt){
			gameObject.SetActive(true);
		}else gameObject.SetActive(true);
		string[] str = transform.name.Split('_');
		int id = int.Parse(str[1]);
		Common_Team.Item item = Common_Team.Get(id);
		var tr = transform.GetChild(0) as Transform;
		ChangeTourTeamItem(tr, id);
	}


	void OnTourClick(){
		Utility.LogError("OnTourClick");
	}

	void OnStockClick(){
		if(selectInfo.activeSelf) return;
		NGUITools.FindInParents<ViewMyTeamSelectGrid>(gameObject).unSetSelectedObj(transform.name);
		NGUITools.FindInParents<MyTeamMainMenu2>(gameObject).OnItemChange(transform.name);
		//selectobj.SetActive(true);
		selectInfo.SetActive(true);
		RunningSponTime();
	}

	public void RunningSponTime(){
		var tr = transform.GetChild(0).FindChild("info_Car_Yes") as Transform;
		if(!tr.gameObject.activeSelf) return;
		var mSpon = tr.FindChild("Info_Spon") as Transform;
		var obj = mSpon.FindChild("icon_spon").gameObject as GameObject;
		if(obj.activeSelf){
			string str =  string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
			string str1 = AccountManager.instance.RunningSponTime();
			if(str == str1){
				obj.SetActive(false);//GetComponent<UISprite>().spriteName =myteam.SponID.ToString();
				mSpon.FindChild("lbName_spon").GetComponent<UILabel>().text = KoStorage.GetKorString("72020");
			}
		}
	
	}


	public void CheckTeamSpon(){
		SetTeamSpon();
		//ChangeTeamAbility();
	}

	void OnTeamSelect(){
	//	NGUITools.FindInParents<ViewMyTeamSelectGrid>(gameObject).setSelectTeam(transform.name);
	}

	int CarAbility(int crewid, int idx){
		CTeamAbility _ability = new CTeamAbility();
		int a  = _ability.CarAbility(crewid, idx, 1);
		_ability = null;
	//	Utility.LogWarning("CarAbility 0 " + a + " " + crewid);
		return  a;//_ability.CarAbility(crewid, idx);
	}
	
	int CrewAbility(int carid, int idx){
		CTeamAbility _ability = new CTeamAbility();
		int a = _ability.CrewAbility(carid, idx, 1);
	//	Utility.LogWarning("CrewAbility 0 " + a + " " + carid);
		_ability = null;
		return  a;//_ability.CrewAbility(crewid, idx);
	}
}
