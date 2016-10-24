using UnityEngine;
using System.Collections;

public class InvenQube : MonoBehaviour {

	private bool bFirst = false;
	void Start(){
		if(transform.name.Equals("MatQube") == true)
		transform.FindChild("lbText").GetComponent<UILabel>().text =
			string.Format(KoStorage.GetKorString("75016"), GV.UserNick);
	}

	public void InitInvenItems(int idx, System.Action callback){
		callback();
		if(!bFirst)
			InitQube();
		else ChangeQubeCount();
	}
	
	void ChangeCarInfo(){
		Utility.Log("ChangeCarInfo " +gameObject.name);
		//InitQube();
		ChangeQubeMatInfo(transform);
	}

	void ChangeQubeMatInfo(Transform tr){
	//	int cnt = GV.listMyMat.Find(obj => obj.MatID == GV.QubeID).MatQuantity;
		Common_Material.Item mat = Common_Material.Get(GV.QubeID);
	//	if(cnt == 0){
	//		tr.FindChild("Info_icon").FindChild("Icon").GetComponent<UISprite>().spriteName = GV.QubeID.ToString();
	//		tr.FindChild("Info_icon").FindChild("lbName").GetComponent<UILabel>().text =  mat.Name;
	//		tr.FindChild("Info_icon").FindChild("lbText").GetComponent<UILabel>().text =  mat.Text;//mat.Text;
	//		return;
	//	}
		tr.FindChild("Info_icon").FindChild("Icon").GetComponent<UISprite>().spriteName = GV.QubeID.ToString();
		tr.FindChild("Info_icon").FindChild("lbName").GetComponent<UILabel>().text = mat.Name;
		tr.FindChild("Info_icon").FindChild("lbText").GetComponent<UILabel>().text = mat.Text;
	}

	public void InitQube(){
		transform.FindChild("Qube").FindChild("icon_item").GetComponent<UISprite>().spriteName = "8620";
		string str = GV.listMyMat.Find(obj => obj.MatID == 8620).MatQuantity.ToString();
		transform.FindChild("Qube").FindChild("lbQuantity").GetComponent<UILabel>().text = string.Format("X {0}", str);
		transform.FindChild("Qube").FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("75019");
		bFirst = true;
	}
	
	public void ChangeQubeCount(){
		//	transform.FindChild("icon_item").GetComponent<UISprite>().spriteName = "8520";
		string str = GV.listMyMat.Find(obj => obj.MatID == 8620).MatQuantity.ToString();
		transform.FindChild("Qube").FindChild("lbQuantity").GetComponent<UILabel>().text = string.Format("X {0}", str);
		//	transform.FindChild("lbText").GetComponent<UILabel>().text = "상점 고고";
	}
}
