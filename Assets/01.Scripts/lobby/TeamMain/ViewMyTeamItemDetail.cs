using UnityEngine;
using System.Collections;

public class ViewMyTeamItemDetail : MonoBehaviour {

	void Start(){
		transform.GetChild(0).FindChild("lbTeamTitle").GetComponent<UILabel>().text
			= KoStorage.GetKorString("76035");
	}

	public void myInfoContent(string name){

		string[] str = name.Split('_');
		int id = int.Parse(str[1]);
		Common_Team.Item item = Common_Team.Get(id);
		var tr = transform.GetChild(0) as Transform;
//		Utility.LogWarning(id);
//		Utility.LogWarning(GV.SelectedTeamID);
		ChangeMainStockTeam(item, tr, id);


	}


	void FixedUpdate(){
		if(bSpon)
			lbSponTime.text = TimeSpon();
	}

	private string TimeSpon(){
		nowTime = NetworkManager.instance.GetCurrentDeviceTime();
		pTime = sponTime-nowTime;
		if(pTime.TotalHours >0)
			return string.Format("{0:00}:{1:00}:{2:00}", pTime.Hours, pTime.Minutes, pTime.Seconds);
		else 
			return string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
	}

	private bool bSpon;
	private UILabel lbSponTime;
	private System.DateTime sponTime;
	private System.DateTime nowTime;
	private System.TimeSpan pTime;

	public void ChangeMainSpon(){
		var tr = transform.GetChild(0) as Transform;
		tr.FindChild("Selected").gameObject.SetActive(true);
		var child = tr.FindChild("info") as Transform;
		int mSpon =GV.getTeamSponID(GV.SelectedTeamID); 
		if(mSpon <= 1300){
			child.FindChild("icon_spon").gameObject.SetActive(false);//GetComponent<UISprite>().spriteName =myteam.SponID.ToString();
			child.FindChild("lbSponName").GetComponent<UILabel>().text = KoStorage.GetKorString("72020");
			child.FindChild("lbSponTime").gameObject.SetActive(false);
			bSpon = false;
		}else{
			child.FindChild("icon_spon").gameObject.SetActive(true);
			child.FindChild("icon_spon").GetComponent<UISprite>().spriteName =mSpon.ToString();
			child.FindChild("lbSponName").GetComponent<UILabel>().text =  string.Format(KoStorage.GetKorString("72019"), Common_Sponsor_Status.Get(mSpon).Name);
			child.FindChild("lbSponTime").gameObject.SetActive(true);
			lbSponTime = child.FindChild("lbSponTime").GetComponent<UILabel>();
			bSpon = true;
			sponTime = new System.DateTime(GV.getTeamTeamInfo(GV.SelectedTeamID).SponRemainTime);
		}

	}


	public void myStockContent(string name){}

	void ChangeMainStockTeam(Common_Team.Item item, Transform tr, int index){

		myTeamInfo myteam = GV.getTeamTeamInfo(index);
	//	tr.FindChild("lbTeamTitle").GetComponent<UILabel>().text = item.Name;

		tr.FindChild("Selected").gameObject.SetActive(true);
		var child = tr.FindChild("info") as Transform;
		if(GV.getTeamSponID(GV.SelectedTeamID) <= 1300){
			child.FindChild("icon_spon").gameObject.SetActive(false);//GetComponent<UISprite>().spriteName =myteam.SponID.ToString();
			child.FindChild("lbSponName").GetComponent<UILabel>().text = KoStorage.GetKorString("72020");
			child.FindChild("lbSponTime").gameObject.SetActive(false);
			bSpon = false;
		}else{
			child.FindChild("icon_spon").gameObject.SetActive(true);
			child.FindChild("icon_spon").GetComponent<UISprite>().spriteName =myteam.SponID.ToString();
			child.FindChild("lbSponName").GetComponent<UILabel>().text =  string.Format(KoStorage.GetKorString("72019"), Common_Sponsor_Status.Get(myteam.SponID).Name);
			child.FindChild("lbSponTime").gameObject.SetActive(true);
			lbSponTime = child.FindChild("lbSponTime").GetComponent<UILabel>();
			bSpon = true;
			sponTime = new System.DateTime(GV.getTeamTeamInfo(GV.SelectedTeamID).SponRemainTime);
		}

	

		child.FindChild("lbLv").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("76001"), 10);

		var child1 = child.FindChild("Info_Car") as Transform;

		int id = myteam.TeamCarID;
		child1.FindChild("icon_car").GetComponent<UISprite>().spriteName = id.ToString();
		Common_Car_Status.Item _item = Common_Car_Status.Get(id);
		child1.FindChild("lbName_Car").GetComponent<UILabel>().text = _item.Name;
		child1.FindChild("lbAble_Car").GetComponent<UILabel>().text = CarAbility(id).ToString();//.Name;
		child1.FindChild("lbClass_Car").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74024"), myteam.myCar.ClassID);// _item.Class);//_item.Name;

		child1 = child.FindChild("Info_Crew");
		id = myteam.TeamCrewID;
		child1.FindChild("icon_crew").GetComponent<UISprite>().spriteName = id.ToString()+"A";
		Common_Crew_Status.Item _item1 = Common_Crew_Status.Get(id);
		child1.FindChild("lbName_Crew").GetComponent<UILabel>().text = _item1.Name;
		child1.FindChild("lbAble_Crew").GetComponent<UILabel>().text = CrewAbility(id).ToString();//.Name;
		child.FindChild("Logo_team").GetComponent<UISprite>().spriteName = id.ToString()+"L";
	}

	public void ChangeTeamAbility(){
		myTeamInfo myteam = GV.getTeamTeamInfo(GV.SelectedTeamID);
		int id = myteam.TeamCarID;
		var tr = transform.GetChild(0) as Transform;
		var child = tr.FindChild("info") as Transform;
		var child1 = child.FindChild("Info_Car") as Transform;
		child1.FindChild("lbAble_Car").GetComponent<UILabel>().text = CarAbility(id).ToString();//.Name;

		child1 = child.FindChild("Info_Crew");
		id = myteam.TeamCrewID;
		child1.FindChild("lbAble_Crew").GetComponent<UILabel>().text = CrewAbility(id).ToString();//.Name;
	}


	void ChangeMainTourTeam(Common_Team.Item item, Transform tr, int index){
		return;/*
		myTeamInfo myteam = GV.getTeamTeamInfo(index);
		tr.FindChild("lbTeamTitle").GetComponent<UILabel>().text = item.Name;
		tr.FindChild("Selected").gameObject.SetActive(true);
		var child = tr.FindChild("info") as Transform;
		if(GV.getTeamSponID() == 1300){
			child.FindChild("icon_spon").gameObject.SetActive(false);//GetComponent<UISprite>().spriteName =myteam.SponID.ToString();
		}else{
			child.FindChild("icon_spon").gameObject.SetActive(true);
			child.FindChild("icon_spon").GetComponent<UISprite>().spriteName =myteam.SponID.ToString();
			
		}
		var child1 = child.FindChild("Info_Car") as Transform;
		int id = myteam.TeamCarID;
		child1.FindChild("icon_car").GetComponent<UISprite>().spriteName = id.ToString();
		Common_Car_Status.Item _item = Common_Car_Status.Get(id);
		child1.FindChild("lbName_Car").GetComponent<UILabel>().text = _item.Name;
		child1.FindChild("lbAble_Car").GetComponent<UILabel>().text = CarAbility(id).ToString();//.Name;
		child1.FindChild("lbClass_Car").GetComponent<UILabel>().text = "Class_"+_item.Class;//_item.Name;
		
		child1 = child.FindChild("Info_Crew");
		id = myteam.TeamCrewID;
		child1.FindChild("icon_crew").GetComponent<UISprite>().spriteName = id.ToString()+"A";
		Common_Crew_Status.Item _item1 = Common_Crew_Status.Get(id);
		child1.FindChild("lbName_Crew").GetComponent<UILabel>().text = _item1.Name;
		child1.FindChild("lbAble_Crew").GetComponent<UILabel>().text = CrewAbility(id).ToString();//.Name;
	*/
	}

	int CarAbility(int idx){
		CTeamAbility _ability = new CTeamAbility();
		int a  = _ability.TeamCarAbility(idx);
		_ability = null;
		//Utility.LogWarning("CarAbility 1 " + a + " " + idx);
		return  a;//_ability.CarAbility(crewid, idx);
	}
	
	int CrewAbility(int idx){
		CTeamAbility _ability = new CTeamAbility();
		int a = _ability.TeamCrewAbility(idx);
		_ability = null;
		//Utility.LogWarning("CrewAbility 1 " +a  + " " + idx);
		return  a;//_ability.CrewAbility(crewid, idx);
	}

	void setLockIcon(){
		//transform.parent.parent.FindChild("Tour").gameObject.SetActive(false);
		transform.FindChild("Board").FindChild("info").gameObject.SetActive(false);
		transform.FindChild("Board").FindChild("icon_Lock").gameObject.SetActive(true);
		transform.GetChild(1).gameObject.SetActive(false);
	}

	void unSetLockIcon(){
		//transform.parent.parent.FindChild("Tour").gameObject.SetActive(true);
		transform.FindChild("Board").FindChild("info").gameObject.SetActive(true);
		transform.FindChild("Board").FindChild("icon_Lock").gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(true);
	}

	public void myTourContent(string name){
		Utility.LogWarning("myTourContent"+name);
		return;
		string[] n = name.Split('_');
		int idx = int.Parse(n[1]);
		int reqlv = Common_Team.Get(idx).ReqLV;
		if(GV.ChSeason < reqlv){
			unSetLockIcon();//gameObject.SetActive(true);
//			setLockIcon();
		}else 
			unSetLockIcon();//gameObject.SetActive(true);

		string[] str = name.Split('_');
		int id = int.Parse(str[1]);
		Common_Team.Item item = Common_Team.Get(id);
		var tr = transform.GetChild(0) as Transform;
		ChangeMainTourTeam(item, tr,id);
	}

	public void myStockInfoContent(string name){
		transform.name = name;
	}

	public void myTourInfoContent(string name){
		transform.name = name;
	}
}
