using UnityEngine;
using System.Collections;

public class InvenCar : MonoBehaviour {

	public GameObject slotItem;
	public GameObject View, Grid;
	private const int MAXSLOT = 10;
	int sCar=5;//, tCar=5, ssCar=10;
	int CarCount;
	void Awake(){
	}

	void InitCarItems(){
		CarCount = GV.mineCarList.Count;// + GV.mineSCarList.Count;
		sCar = GV.mineCarList.Count;
		if(CarCount < 6) CarCount = 6;

		int childcnt = Grid.transform.childCount;
		if(childcnt == 0 ){
			for(int i =0 ; i < CarCount; i++){
				var temp = NGUITools.AddChild(Grid, slotItem);
				temp.transform.FindChild("Board").gameObject.AddComponent<InvenCarItem>();//.ViewMaterialItem(i);
			}
		}else{
			if(childcnt < CarCount){
				for(int i =childcnt ; i < CarCount; i++){
					var temp = NGUITools.AddChild(Grid, slotItem);
					temp.transform.FindChild("Board").gameObject.AddComponent<InvenCarItem>();//.ViewMaterialItem(i);
				}
				Grid.GetComponent<UIGrid>().Reposition();
			}
		}
		for(int i = 0; i < CarCount; i++){
			var temp = Grid.transform.GetChild(i) as Transform;
			temp.transform.FindChild("Board").FindChild("Select").gameObject.SetActive(false);
			if(i >= sCar){
				temp.name = "sCar_"+"0000_D_10_"+i.ToString();
				temp.transform.FindChild("Board").GetComponent<InvenCarItem>().EmptyAction();
			}else{
				temp.name = "sCar_"+GV.mineCarList[i].CarID.ToString()+"_"+GV.mineCarList[i].ClassID+"_"+GV.mineCarList[i].TeamID+"_"+i.ToString();
				temp.transform.FindChild("Board").GetComponent<InvenCarItem>().ContentChange();
			}
		}
		Grid.transform.GetChild(0).FindChild("Board").FindChild("Select").gameObject.SetActive(true);
		GameObject.Find("LobbyUI").SendMessage("ChangeElevatorCar", Grid.transform.GetChild(0).name);
		Grid.GetComponent<UIGrid>().Reposition();
		View.GetComponent<UIDraggablePanel>().ResetPosition();
	}

	public void InitInvenItems(int idx){
		//Utility.Log(idx);
		if(idx == 0){
			if(Grid.transform.childCount  == 0 ) return;
			for(int i = 0; i < CarCount; i++){
				//var temp = Grid.transform.GetChild(i) as Transform;
			}
		}
	}

	void ChangeCarSlotItem(int idx){
		int length = 0;
		string strname = string.Empty;
		string[] ids = new string[1];
		switch(idx){
		case 0:
			strname = "sCar";
			length = GV.mineCarList.Count;
			ids = new string[length];
			//ids = GV.myStockCarList.ToArray();
			for(int i =0; i < length; i++){
				ids[i]  = GV.mineCarList[i].CarID.ToString()+"_"+GV.mineCarList[i].ClassID+"_"+GV.mineCarList[i].TeamID;
			}


//			int len = GV.mineSCarList.Count;
//			for(int i = length ; i < (length+len); i++){
//				ids[i] = GV.mineSCarList[i].CarID.ToString()+"_"+GV.mineSCarList[i].ClassID+"_"+GV.mineCarList[i].TeamID;
//			}
			break;
		case 1:
		//	strname = "tCar";
		//	length = GV.myTourCarList.Count;
		//	ids = new string[length];
		//	ids = GV.myTourCarList.ToArray();
			break;
		case 2:
			strname = "ssCar";
			length = GV.mineCarList.Count;
			ids = new string[length];
			//ids = GV.mySpecialCarList.ToArray();
			GV.mineCarList.ForEach(obj=>{
				
			});
			//!!--Utility.Log("SSCar is Error");
			break;
		default:
			break;
		}

		for(int i = 0; i < CarCount; i++){
			var temp = Grid.transform.GetChild(i) as Transform;
			if(i >= length){
				temp.name = strname	+"_0000_D_10_"+i.ToString();
				temp.transform.FindChild("Board").GetComponent<InvenCarItem>().EmptyAction();
			}else{
				temp.name = strname+"_"+ids[i]+"_"+i.ToString();
				temp.transform.FindChild("Board").GetComponent<InvenCarItem>().ContentChange();
			}
			if(i == 0) temp.transform.FindChild("Board").FindChild("Select").gameObject.SetActive(true);
		}
		View.GetComponent<UIDraggablePanel>().ResetPosition();

	}
	void ChangeTeamCarSlotItem(){
		int length = 0;
		string strname = string.Empty;
		string[] ids = new string[1];
		strname = "sCar";
		length = GV.mineCarList.Count;
		ids = new string[length];
		for(int i =0; i < length; i++){
			ids[i]  = GV.mineCarList[i].CarID.ToString()+"_"+GV.mineCarList[i].ClassID+"_"+GV.mineCarList[i].TeamID;
		}
		
		for(int i = 0; i < CarCount; i++){
			var temp = Grid.transform.GetChild(i) as Transform;
			if(i >= length){
				temp.name = strname	+"_0000_D_10_"+i.ToString();
				temp.transform.FindChild("Board").GetComponent<InvenCarItem>().EmptyAction();
			}else{
				temp.name = strname+"_"+ids[i]+"_"+i.ToString();
				temp.transform.FindChild("Board").GetComponent<InvenCarItem>().ContentChange();
				if(GV.mineCarList[i].TeamID == GV.SelectedTeamID) temp.transform.FindChild("Board").FindChild("Select").gameObject.SetActive(true);
			}
			
		}
		View.GetComponent<UIDraggablePanel>().ResetPosition();
		
	}
	public void DisableSelect(GameObject obj){
		for(int i = 0; i < CarCount; i++){
			var temp = Grid.transform.GetChild(i) as Transform;
			temp.transform.FindChild("Board").GetComponent<InvenCarItem>().NonSelectBG();
		}
		NGUITools.FindInParents<InvenMain>(gameObject).ShowSubDetail(obj.name,0);
	}
}
