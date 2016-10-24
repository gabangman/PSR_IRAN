using UnityEngine;
using System.Collections;

public class InvenCarJunk : MonoBehaviour {

	void Start(){
		transform.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("75020");
		transform.FindChild("BTN").GetComponentInChildren<UILabel>().text  = KoStorage.GetKorString("71000");
	}

	void OnClose(){
		NGUITools.FindInParents<InvenMain>(gameObject).DeleteCarSlot(transform.name, 1);
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		gameObject.SetActive(false);
	}

	void OnResultDisAssy(string strClass){
		int rMatCnt = 0;
		/*switch(strClass){
		case "SS":rMatCnt = 6;break;
		case "S":rMatCnt = 5;break;
		case "A":rMatCnt = 4;break;
		case "B":rMatCnt = 3;break;
		default: rMatCnt = 2;break;
		}*/
		//Utility.UtLogW("disassy Class " + strClass);
		string[] strs = strClass.Split(',');
		rMatCnt = strs.Length-1;
		var tr = transform.FindChild("Materials") as Transform;
		int[] rand = gameObject.AddComponent<RandomCreate>().CreateRandomValue(19);
		Common_Material.Item matItem;

		for(int i=0; i < tr.childCount;i++){
			var mTr = tr.GetChild(i) as Transform;
			if( i < rMatCnt){
				int mID = int.Parse(strs[i]);
				matItem = Common_Material.Get(mID);
				mTr.FindChild("ON").gameObject.SetActive(true);
				mTr.FindChild("OFF").gameObject.SetActive(false);
				mTr.FindChild("ON").FindChild("lbAmount").GetComponent<UILabel>().text = string.Format("X{0}",1);
				mTr.FindChild("ON").FindChild("lbName").GetComponent<UILabel>().text = matItem.Name;
				mTr.FindChild("ON").FindChild("Icon_Mat").GetComponent<UISprite>().spriteName = matItem.ID;
				GV.UpdateMatCount(mID,1);
			}else{
				mTr.FindChild("ON").gameObject.SetActive(false);
				mTr.FindChild("OFF").gameObject.SetActive(true);
			}
		}

		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnClose",0.1f);
			return true;
		};
	}
}
