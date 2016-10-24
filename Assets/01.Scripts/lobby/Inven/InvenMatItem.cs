using UnityEngine;
using System.Collections;

public class InvenMatItem : MonoBehaviour {

	public void InitContent(int idx){
		int id = idx;
		int maxlength = GV.listMyMat.Count;//.Length;
		for(int i = 0; i < 4; i++){
			var tr = transform.GetChild(i) as Transform;
			id = idx+i;
			if(id > maxlength || maxlength == 0){
				tr.gameObject.SetActive(false);
			}else{
				MatInfo mat = GV.listMyMat[id];
				if(mat.MatID > 8619){
					tr.gameObject.SetActive(false);
				}else{
					tr.FindChild("Selected").gameObject.SetActive(false);
					tr.FindChild("Icon_Mat").GetComponent<UISprite>().spriteName = mat.MatID.ToString() ;
					string strQ = mat.MatQuantity.ToString();
					if(mat.MatQuantity == 0) tr.FindChild("lbAmount").GetComponent<UILabel>().color = Color.red;
					else 	tr.FindChild("lbAmount").GetComponent<UILabel>().color = Color.white;
					tr.FindChild("lbAmount").GetComponent<UILabel>().text = string.Format("X{0}", strQ);
				}
			}
		}
	}
	
	void OnClick(GameObject obj){
		var temp = obj.transform.FindChild("Selected").gameObject as GameObject;
		if(temp.activeSelf) return;
		NGUITools.FindInParents<InvenMat>(gameObject).DisableSelectAll(obj);
		temp.SetActive(true);
		
	}

	public void ChangeConent(int id, string str){
		for(int i = 0; i < 4; i++){
			transform.GetChild(i).FindChild("Selected").gameObject.SetActive(false);
		}

	}

	public void ChangeMatCount(int idx){
		int id = idx;
		int maxlength = GV.listMyMat.Count;
		for(int i = 0; i < 4; i++){
			var tr = transform.GetChild(i) as Transform;
			id = idx+i;
			if(id > maxlength || maxlength == 0){
				tr.gameObject.SetActive(false);
			}else{
				MatInfo mat = GV.listMyMat[id];
				if(mat.MatID > 8619){
					tr.gameObject.SetActive(false);
				}else{
					tr.FindChild("Selected").gameObject.SetActive(false);
					if(mat.MatQuantity == 0) tr.FindChild("lbAmount").GetComponent<UILabel>().color = Color.red;
					else 	tr.FindChild("lbAmount").GetComponent<UILabel>().color = Color.white;
					tr.FindChild("lbAmount").GetComponent<UILabel>().text = 	string.Format("X{0}", mat.MatQuantity);
				}
			}
		}
	}

}
