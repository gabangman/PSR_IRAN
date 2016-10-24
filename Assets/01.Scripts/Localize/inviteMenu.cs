using UnityEngine;
using System.Collections;

public class inviteMenu : MonoBehaviour {

	public UILabel[] lbText;
	void Start () {
		lbText[0].text = KoStorage.GetKorString("72602");//
		lbText[1].text = KoStorage.GetKorString("72603");//
		lbText[2].text = KoStorage.GetKorString("72604");//
		lbText[3].text = KoStorage.GetKorString("72605");//
		lbText[5].text = string.Format(KoStorage.GetKorString("72600"));
		lbText[6].text = string.Format(KoStorage.GetKorString("72601"));
		Common_Attend.Item _item;
		/*for(int i = 0; i < 4; i++){
			_item = Common_Attend.Get((8750+i));
			//lbText[i].text = _item.Name;
			lbText[i].transform.parent.FindChild("lbPrice").GetComponent<UILabel>().text 
				= string.Format(" X {0}", _item.Quantity);
		}*/
		Utility.LogWarning("modify - invitemenu");
	
	}

	void OnEnable(){
		lbText[4].text = string.Format(KoStorage.GetKorString("72606"), Global.gInviteCount);
		var  tr = transform.FindChild("Content_Title") as Transform;
		string str = string.Empty;

		if(Global.gInviteCount >= 10){
			str = "InviteEvent_1";
			tr.FindChild(str).FindChild("icon_Compete").gameObject.SetActive(true);
			tr.FindChild(str).FindChild("icon_Stamp_effect").gameObject.SetActive(false);
		}else return;
		if(Global.gInviteCount >= 20){
			str = "InviteEvent_2";
			tr.FindChild(str).FindChild("icon_Compete").gameObject.SetActive(true);
			tr.FindChild(str).FindChild("icon_Stamp_effect").gameObject.SetActive(false);
		}else return;
		if(Global.gInviteCount >= 30){
			str = "InviteEvent_3";
			tr.FindChild(str).FindChild("icon_Compete").gameObject.SetActive(true);
			tr.FindChild(str).FindChild("icon_Stamp_effect").gameObject.SetActive(false);
		}else return;
		if(Global.gInviteCount >= 40){
			str = "InviteEvent_4";
			tr.FindChild(str).FindChild("icon_Compete").gameObject.SetActive(true);
			tr.FindChild(str).FindChild("icon_Stamp_effect").gameObject.SetActive(false);
		}else return;
	}

	public void InviteEvent(){
		lbText[4].text = string.Format(KoStorage.GetKorString("72606"), Global.gInviteCount);
		var attend = transform.FindChild("Content_Title").gameObject as GameObject;
		GameObject.Find ("Audio").SendMessage("AttendSound");
		Transform target = null;
		string str = null;
		if(Global.gInviteCount == 10){
			str = "InviteEvent_1";
		}
		if(Global.gInviteCount == 20){
			str = "InviteEvent_2";
		}
		if(Global.gInviteCount == 30){
			str = "InviteEvent_3";
		}
		if(Global.gInviteCount == 40){
			str = "InviteEvent_4";
		}
		if(str == null) return;
		target = attend.transform.FindChild(str).FindChild("icon_Compete");
		target.gameObject.SetActive(true);
		Vector3 tscale = target.localScale;
		target.localScale = new Vector3(0.1f,0.1f,0.1f);
		var tw = target.gameObject.AddComponent<TweenScale>() as TweenScale;
		tw.duration = 0.25f;
		tw.to = tscale;
		tw.from = tscale * 3.0f;
		tw.delay = 0.5f;
		tw.enabled = true;
		tw.onFinished = delegate(UITweener tween) {
		//	attend.transform.parent.FindChild ("Contents_menuBG").GetComponent<UIButtonMessage>().
		//	functionName = "OnCloseClick";
			tween.transform.parent.FindChild("icon_Stamp_effect").gameObject.SetActive(true);
			//Invoke("StampSound",0.01f);
			GameObject.Find ("Audio").SendMessage("AttendStampSound");
			Destroy(tween);
		};
		}
}
