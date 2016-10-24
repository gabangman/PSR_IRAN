using UnityEngine;
using System.Collections;
using System.Text;
public class Infoaction : MonoBehaviour {


	public void ShowCrewInfomation(GameObject nameObj, int _id){
	//	var info = gameObject as GameObject;
	/*	Common_Crew_Status.Item _item = Common_Crew_Status.Get(_id);
		//info.name = nameObj.transform.parent.gameObject.name;
		gameObject.name = nameObj.transform.parent.name;
		StringBuilder sb = new StringBuilder();
		transform.FindChild("Sprite_Tmark").GetComponent<UISprite>().spriteName =_id+"L";
		//ChildObjectSpriteChange("Spirte_Tmark",);
		transform.FindChild("Sprite (Class)").GetComponent<UISprite>().spriteName = "Class_"+_item.Class;
		sb.Length = 0;
		sb.Append(_item.Name);
		ChangeLabel("lbName", sb.ToString());
		
		sb.Length =0;
		string cls = KoStorage.getStringDic("60080")+" ";
		sb.Append(cls); sb.Append(_item.Class);
		//sb.Append("Class "); sb.Append(_item.Class);
		ChangeLabel("lbClass", sb.ToString());
		
		//	sb.Length=0;
		//	sb.Append(_item.Text);
		//	ChangeLabel("lbText",sb.ToString());
		//요구시즌
		sb.Length = 0;
		string tex = string.Format(KoStorage.getStringDic("60073"),_item.ReqLV);
		sb.Append(tex);
		ChangeLabel("lbReqSeason",sb.ToString());
		//강화가능단계
		sb.Length = 0;
		tex = string.Format(KoStorage.getStringDic("60082"),_item.UpLimit);
		sb.Append(tex);
		ChangeLabel("lbUpLimit",sb.ToString());
		//능력치
		sb.Length = 0;
		CTeamAbility ability = new CTeamAbility();
		string[] strRet = ability.CrewAbility(_id);
		tex = string.Format(KoStorage.getStringDic("60271"),strRet[0]);
		sb.Append(tex);
		ChangeLabel("lbTeamwork", sb.ToString());
*/
		/*

		transform.FindChild("Sprite_Tmark").GetComponent<UISprite>().spriteName = _id.ToString()+"L";
		StringBuilder sb = new StringBuilder();
		sb.Length = 0;
		sb.Append(_item.Name);
		UILabelChangeText(info, "lbName", sb.ToString());

		sb.Length =0;
		sb.Append("Class "); sb.Append(_item.Class);
		UILabelChangeText(info, "lbClass", sb.ToString());
	
		sb.Length = 0;
		sb.Append("요구시즌 : "); 
		string tex = string.Format("[00ff00] {0} [-]",_item.ReqLV);
		sb.Append(tex);
		sb.Append(" 시즌"); 
		UILabelChangeText(info, "lbReqSeason", sb.ToString());

		sb.Length = 0;
		sb.Append("강화가능단계 : "); 
		tex = string.Format("[00ff00] {0} [-]",_item.UpLimit);
		sb.Append(tex);
		sb.Append(" 단계"); 
		UILabelChangeText(info, "lbUpLimit", sb.ToString());

		sb.Length = 0;

		Account.CrewInfo mCrew = myAccount.instance.GetCrewInfo(_id);

		float team = 0;
		team += _item.Chief *100 + (( Mathf.Abs(Upgrade_Crew_Ratio.Get((int)CrewPartID.Chief).Ratio) )*500)
			+ _item.Jack *100 + (( Mathf.Abs(Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio)  )*mCrew.jackLv);
		team += _item.Gas*100 + (Mathf.Abs(Upgrade_Crew_Ratio.Get( (int)CrewPartID.GasMan).Ratio) * (mCrew.gasLv) )*1000
			+_item.Jack *100 + (( Mathf.Abs(Upgrade_Crew_Ratio.Get((int)CrewPartID.JackMan).Ratio) * mCrew.jackLv )*10);
		team += _item.Tire*100 + (Mathf.Abs(Upgrade_Crew_Ratio.Get((int)CrewPartID.Tire).Ratio) * (mCrew.tireLv) )*500;
		string str = string.Format("팀워크 : [ff5400] {0} [-]",(int)team);
		sb.Append(str);

		UILabelChangeText(info, "lbTeamwork", sb.ToString());
	*/



	}

	public void ShowCarInfomation(GameObject nameObj, int _id){
		Utility.LogWarning("ShowCarInfomation");
		return;
		//var info = gameObject as GameObject;
	//	Common_Car_Status.Item _item =  Common_Car_Status.Get (_id);
	//	info.name = nameObj.transform.parent.gameObject.name;
	//	gameObject.name = nameObj.transform.parent.name;
	//	StringBuilder sb = new StringBuilder();
	//	sb.Length =0;
	//	string cls = KoStorage.getStringDic("60080")+" ";
	//	sb.Append(cls); sb.Append(_item.Class);
		//sb.Append("Class "); sb.Append(_item.Class);
	//	ChangeLabel("lbClass", sb.ToString());
	//	transform.FindChild("Sprite (Class)").GetComponent<UISprite>().spriteName = "Class_"+_item.Class;
	//	string text = string.Empty;
	//	sb.Length = 0;
	//	sb.Append(_item.Name);
	//	ChangeLabel("lbName", sb.ToString());
		//성능
	//	sb.Length = 0;
	//	CTeamAbility ability = new CTeamAbility();
	//	string[] strRet = ability.CarAbility(_id);
	//	ability = null;
	//	text = string.Format(KoStorage.getStringDic("60270"),strRet[0]);
	//	sb.Append(text);
	//	ChangeLabel("lbPower", sb.ToString());
		
	//	sb.Length = 0;
		//요구 시즌
	//	text = string.Format(KoStorage.getStringDic("60073"),_item.ReqLV);
	//	sb.Append(text);
	//	ChangeLabel("lbReqSeason",sb.ToString());
		//강화 가능단계
	//	sb.Length = 0;
	//	text = string.Format(KoStorage.getStringDic("60082"),_item.UpLimit);
	//	sb.Append(text);
	//	ChangeLabel("lbUpLimit",sb.ToString());
		//최대기어
	//	sb.Length = 0;
	//	text = string.Format(KoStorage.getStringDic("60190"),_item.GearLmt);
	//	sb.Append(text);
	//	ChangeLabel("lbUpGear",sb.ToString());
	//	float f1 = _item.P_Control;
	//	sb.Length = 0;
	//	if(f1 < 0.13f){
	//		sb.Append(KoStorage.getStringDic("60377"));
	//	}else if(f1 < 0.16f){sb.Append(KoStorage.getStringDic("60376"));
	//	}else if(f1 < 0.21f){sb.Append(KoStorage.getStringDic("60375"));
	//	}else if(f1 < 0.31f){sb.Append(KoStorage.getStringDic("60374"));
	//	}
	//	ChangeLabel("lbControl",sb.ToString());
		/*


		sb.Length =0;
		sb.Append("Class "); sb.Append(_carstatus.Class);
		UILabelChangeText(info, "lbClass",  sb.ToString());
		
		sb.Length = 0;
		sb.Append(_carstatus.Name);
		UILabelChangeText(info ,"lbName", sb.ToString());

		sb.Length = 0;
		sb.Append(" 출력 : ");
		string text = string.Format("[ff5400] {0} [-]",(_carstatus.Power*2));
		sb.Append(text);
		UILabelChangeText(info,"lbPower", sb.ToString());
		
		sb.Length = 0;
		sb.Append("요구시즌 : "); 
		text = string.Format("[00ff00] {0} [-]",_carstatus.ReqLV);
		sb.Append(text);
		sb.Append(" 시즌"); 
		UILabelChangeText(info,"lbReqSeason", sb.ToString());

		sb.Length = 0;
		sb.Append("강화가능단계 : "); 
		text = string.Format("[00ff00] {0} [-]",_carstatus.UpLimit);
		sb.Append(text);
		sb.Append(" 단계"); 
		UILabelChangeText(info,"lbUpLimit", sb.ToString());

		sb.Length = 0;
		sb.Append("최대 기어 : "); 
		text = string.Format("[00ff00] {0} [-] 단",_carstatus.GearLmt);
		sb.Append(text);
		UILabelChangeText(info,"lbUpGear", sb.ToString());
		*/
	}
	void ChangeLabel( string label, string text){		
		transform.FindChild(label).GetComponent<UILabel>().text = text;
	}
	public void InitInfoSet(GameObject info, string _id){
		//string carID = Global.MyCarID.ToString();
		if(_id.Equals(gameObject.name)){
		info.transform.FindChild("button").gameObject.SetActive(false);
		info.transform.FindChild("check").gameObject.SetActive(true);
			info.transform.FindChild("lbSelect").GetComponent<UILabel>().text = KoStorage.GetKorString("76308");
		}else{
		info.transform.FindChild("button").gameObject.SetActive(true);	
		info.transform.FindChild("check").gameObject.SetActive(false);
			info.transform.FindChild("lbSelect").GetComponent<UILabel>().text =  KoStorage.getStringDic("60273");
		}
	}
	

}
