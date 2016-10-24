using UnityEngine;
using System.Collections;

public class InvenCoupon : MonoBehaviour {
	public GameObject slotItem;
	public GameObject View, Grid;
	void Start(){
		transform.FindChild("lbText").GetComponent<UILabel>().text =string.Format(KoStorage.GetKorString("75017"), GV.UserNick);
	}
	public void InitInvenItems(int idx, System.Action callback){
		if(idx == 3){
		
			int cnt = Grid.transform.childCount;
			if(cnt == 0){
				//GV.InitAccountValue();
				for(int i =0 ; i < 2; i++){
					var temp = NGUITools.AddChild(Grid, slotItem);
					temp.name = "SlotCoupon_"+i;
					temp.AddComponent<InvenCouponItems>().InitContent(i);
				}
				callback();
				Grid.transform.GetChild(0).FindChild("Silver").FindChild("Selected").gameObject.SetActive(true);
			} // cnt
			else{
				for(int i =0 ; i < 2; i++){
					Grid.transform.GetChild(i).GetComponent<InvenCouponItems>().ChangeContent(i);
				}
			}
			Grid.GetComponent<UIGrid>().Reposition();	
		} //idx
	}

	public void DisableSelect(GameObject obj){
		int cnt = Grid.transform.childCount;
		for(int i = 0; i < cnt; i++){
			var temp = Grid.transform.GetChild(i) as Transform;
			var tr = temp.FindChild("Gold").gameObject;
			if(tr.activeSelf){
				tr.transform.FindChild("Selected").gameObject.SetActive(false);
			}else{
				tr = temp.FindChild("Silver").gameObject;
				tr.transform.FindChild("Selected").gameObject.SetActive(false);
			}
		}
		string str = obj.name +"_"+ obj.transform.parent.name;
		NGUITools.FindInParents<InvenMain>(gameObject).ShowSubDetail(str,3);
	}

}
