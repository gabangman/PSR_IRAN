using UnityEngine;
using System.Collections;

public class InvenMatInfo : MonoBehaviour {

	public UILabel[] lbtext;

	void Start(){
		lbtext[0].text = KoStorage.GetKorString("75004");
	}

	void ChangeMatInfo(){
		//!!--Utility.Log("ChangeMatInfo " + gameObject.name);
		string[] name = gameObject.name.Split('_');
		for(int i=0; i < transform.childCount;i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}
		if(name[0].Equals("Car")){
			var tr = transform.FindChild("Mat_Car") as Transform;
			tr.gameObject.SetActive(true);
			ChangeCarMatInfo(tr, name[2], name[4]);
		}else if(name[0].Equals("Crew")){
			var tr = transform.FindChild("Mat_Car") as Transform;
			tr.gameObject.SetActive(true);
			ChangeCarMatInfo(tr, name[2], name[4]);
		}else{
		//	var tr1 =transform.FindChild("Mat_Cube") as Transform;//.gameObject.SetActive(true);
		//	tr1.gameObject.SetActive(true);
		//	ChangeQubeMatInfo(tr1);
		}
	}

	void ChangeCarMatInfo(Transform tr, string matID, string deckID){
		int idx = int.Parse(matID)+int.Parse(deckID);
		if(GV.listMyMat.Count == 0) {
			tr.FindChild("Info_icon").FindChild("Icon").GetComponent<UISprite>().spriteName =GV.QubeID.ToString();
			//tr.FindChild("Info_icon").FindChild("lbName").GetComponent<UILabel>().text = "재료 카드당.";
			//tr.FindChild("Info_icon").FindChild("lbText").GetComponent<UILabel>().text = "재료카드 없어요";//mat.Text;
			//!!--Utility.Log("Mat == 0");
			return;
		}
		//int ID = GV.arrayMatID[idx];
		int ID = GV.listMyMat[idx].MatID;
		if(GV.listMyMat[idx].MatQuantity == 0){
			tr.FindChild("Btn_Resolve").gameObject.SetActive(false);
		}else{
			tr.FindChild("Btn_Resolve").gameObject.SetActive(true);
		}
		Common_Material.Item mat = Common_Material.Get(ID);
		//!!--Utility.Log("mat ID " + ID);
		tr.FindChild("Info_icon").FindChild("Icon").GetComponent<UISprite>().spriteName = mat.ID;
		tr.FindChild("Info_icon").FindChild("lbName").GetComponent<UILabel>().text = mat.Name;
		tr.FindChild("Info_icon").FindChild("lbText").GetComponent<UILabel>().text = mat.Text;

	}



}
