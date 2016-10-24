using UnityEngine;
using System.Collections;
using System.Text;
public class ViewTeamCarInfo : MonoBehaviour {

	void Start(){
		transform.FindChild("btnRepair").FindChild("lbTitle").GetComponent<UILabel>().text = KoStorage.GetKorString("76007");
		transform.FindChild("btnSelect").FindChild("lbTitle").GetComponent<UILabel>().text  = KoStorage.GetKorString("76307");
		transform.FindChild("btnSelected").FindChild("lbTitle").GetComponent<UILabel>().text = KoStorage.GetKorString("76308");
	}

	public void ShowCarInfomation(GameObject nameObj, int _id){
		gameObject.name = nameObj.transform.parent.name;
		Utility.LogWarning("ShowCarInfomation " + gameObject.name+ " _ " + _id);

		string[] names = gameObject.name.Split('_');
		CarInfo carInfo = GV.GetMyCarInfo(_id, names[1], 0);
		Common_Car_Status.Item item = Common_Car_Status.Get(_id);
		transform.FindChild("lbPower").GetComponent<UILabel>().text = GV.MyCarAbilityStats(_id, names[1], 0).ToString();
		transform.FindChild("lbName").GetComponent<UILabel>().text = item.Name;
		transform.FindChild("lbClass").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74024"), names[1]);// "";
	//	transform.FindChild("lbStatus").GetComponent<UILabel>().text = 
	//		string.Format(KoStorage.GetKorString("76301"), 
	//		              carInfo.carClass.UpLimit,carInfo.carClass.StarLV,item.ReqLV, item.GearLmt);
	
		transform.FindChild("lbStarLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74028"), carInfo.carClass.StarLV);
		transform.FindChild("lbReqLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74027"),item.ReqLV);
		transform.FindChild("lbGearLV").GetComponent<UILabel>().text = string.Format(KoStorage.GetKorString("74029"), item.GearLmt+carInfo.carClass.GearLmt);
		//	클래스 [009cff]{0}[-]
	//요구시즌 : [009cff]{1}[-]시즌
	//	강화 최고단계 : [ff3600]{2}[-]단계
	//	진화 최고단계 : [ff3600]{3}[-]단계
	//	최대기어 : [009cff]{4}[-]단

	}
	void ChangeLabel( string label, string text){		
		transform.FindChild(label).GetComponent<UILabel>().text = text;
	}

	public void InitInfoSet(GameObject info, string _id){
		string[] names = gameObject.name.Split('_');
	//	if(GV.SelectedTeamID != GV.SelTeamID){
	//		info.transform.FindChild("btnSelect").gameObject.SetActive(true);	
	//		info.transform.FindChild("btnSelected").gameObject.SetActive(false);
	//	}
		int idx = int.Parse(names[2]);
		int index = GV.mineCarList[idx].TeamID;
		if(index == 0){
			info.transform.FindChild("btnSelect").gameObject.SetActive(true);
			info.transform.FindChild("btnSelected").gameObject.SetActive(false);
		}else{
			info.transform.FindChild("btnSelect").gameObject.SetActive(false);	
			info.transform.FindChild("btnSelected").gameObject.SetActive(true);
		}

//		if(_id.Equals(names[0])){
//			info.transform.FindChild("btnSelect").gameObject.SetActive(false);
//			info.transform.FindChild("btnSelected").gameObject.SetActive(true);
//		}else{
//			info.transform.FindChild("btnSelect").gameObject.SetActive(true);	
//			info.transform.FindChild("btnSelected").gameObject.SetActive(false);
//		}

	}
	
	
}
