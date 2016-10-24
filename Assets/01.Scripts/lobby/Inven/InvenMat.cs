using UnityEngine;
using System.Collections;

public class InvenMat : MonoBehaviour {

	public GameObject slotItem;
	public GameObject MatList, cubeList;
	private GameObject View, Grid;


	void OnDisable(){
	//	MatList.SetActive(false);
	//	cubeList.SetActive(false);
	}
	void Start(){
		transform.FindChild("lbText").GetComponent<UILabel>().text =
			string.Format(KoStorage.GetKorString("75015"), GV.UserNick);
	}
	void ChangeMatSlotItem(int idx){
		if(idx == 2){
			MatList.SetActive(false);
			cubeList.SetActive(true);
		}else{
			MatList.SetActive(true);
			cubeList.SetActive(false);
			if(idx == 1){
				ChangeMat("Car");
			}else{ //car
				//ChangeMat("Car");
			}
		}
	}

	void ChangeMat(string str){
		Grid = MatList.transform.GetChild(0).gameObject;
		int cnt = Grid.transform.childCount;
			for(int i =0 ; i < cnt; i++){
				var temp = Grid.transform.GetChild(i) as Transform;
				temp.name = str+"_Mat_"+(i*4).ToString();
				temp.GetComponent<InvenMatItem>().ChangeConent(i, str);
			}
		Grid.transform.GetChild(0).GetChild(0).FindChild("Selected").gameObject.SetActive(true);
		MatList.GetComponent<UIDraggablePanel>().ResetPosition();
	}

	public void InitInvenItems(int idx, System.Action Callback){
		Grid = MatList.transform.GetChild(0).gameObject;
		if(idx == 1){
			int cnt = Grid.transform.childCount;
			if(cnt == 0){
				for(int i =0 ; i < 5; i++){
					var temp = NGUITools.AddChild(Grid, slotItem);
					temp.name = "Car_Mat_"+(i*4).ToString();
					temp.AddComponent<InvenMatItem>().InitContent((i*4));
				}
				Callback();
				if(GV.listMyMat.Count != 0)
					Grid.transform.GetChild(0).GetChild(0).FindChild("Selected").gameObject.SetActive(true);
			}else{
				NGUITools.FindInParents<InvenMain>(gameObject).ResetMatSlot();		
			}// cnt
			Grid.GetComponent<UIGrid>().Reposition();	
		} //idx
	}

	public void ResetInvenItems(System.Action Callback){
			Grid = MatList.transform.GetChild(0).gameObject;
			int cnt = Grid.transform.childCount; 
			if(cnt != 0){
				for(int i =0 ; i < 5; i++){
					var temp = Grid.transform.GetChild(i) as Transform;
					temp.name = "Car_Mat_"+(i*4).ToString();
					temp.GetComponent<InvenMatItem>().ChangeMatCount((i*4));
				}
				Callback();
			if(GV.listMyMat.Count != 0)
				Grid.transform.GetChild(0).GetChild(0).FindChild("Selected").gameObject.SetActive(true);
			} // cnt
			Grid.GetComponent<UIGrid>().Reposition();	
	}


	public void DisableSelectAll(GameObject obj){
		int cnt = Grid.transform.childCount;
		for(int i = 0; i < cnt; i++){
			var temp = Grid.transform.GetChild(i) as Transform;
			for(int j = 0; j < temp.childCount;j++){
				var temp1 = temp.GetChild(j) as Transform;
				temp1.FindChild("Selected").gameObject.SetActive(false);
			}
		}
		string str = obj.transform.parent.name +"_"+obj.name;
		NGUITools.FindInParents<InvenMain>(gameObject).ShowSubMatDetail(str,1);
	}



}
