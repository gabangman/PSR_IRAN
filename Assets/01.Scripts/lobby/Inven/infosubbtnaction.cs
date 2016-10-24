using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class infosubbtnaction : InterAction {

	void Awake(){


	
	}



	void ChangeContentSelectItem(int oldid, int newid){
		int[] idx = {oldid,newid};
		gameObject.transform.FindChild("button").gameObject.SetActive(false);
		gameObject.transform.FindChild("check").gameObject.SetActive(true);
		gameObject.transform.FindChild("lbSelect").GetComponent<UILabel>().text = KoStorage.GetKorString("76308");
		GameObject.Find("LobbyUI").SendMessage("ChangeSeletedTeamCar",idx,SendMessageOptions.DontRequireReceiver);
	}

	int carID, crewID;
	public override void OnSelectCarClick(){
		gameObject.transform.FindChild("btnSelect").gameObject.SetActive(false);
		gameObject.transform.FindChild("btnSelected").gameObject.SetActive(true);
		string[] names = gameObject.name.Split('_');
		int mindex = int.Parse(names[2]);
		CarInfo carinfo = GV.mineCarList[mindex];
		int myTeamID = 0;
		if(GV.SelectedTeamID == GV.SelTeamID){
			myTeamID = GV.SelectedTeamID;
		}else{
			myTeamID = GV.SelTeamID;
		}

		StartCoroutine(SetCarInTeam(myTeamID, carinfo.CarIndex, ()=>{
		//	string[] names = gameObject.name.Split('_');
			int[] carids = new int[2];
			carids[1] = int.Parse(names[0]);
		//	int index = int.Parse(names[2]);
			//CarInfo carinfo = GV.mineCarList[mindex];
			carids[0] = GV.getTeamCarID(myTeamID);
			//myTeamID = 0;
			//if(GV.SelectedTeamID == GV.SelTeamID){
				//myTeamID = GV.SelectedTeamID;
				
			//}else{
				//myTeamID = GV.SelTeamID;
				//carids[0] = GV.getTeamCarID(myTeamID);
		//	}
			carids[0] = GV.getTeamCarID(myTeamID);
			CarInfo _carinfo = GV.mineCarList.Find((obj)=>obj.TeamID == myTeamID);
			if(_carinfo != null) _carinfo.TeamID = 0;
			
			carinfo.TeamID = myTeamID;
			myTeamInfo myteam = GV.getTeamTeamInfo(myTeamID);
			myteam.TeamCarID = carids[1];
			
			myteam.setTeamCarInfo(carinfo);
			
			if(GV.SelectedTeamID == GV.SelTeamID){
				GameObject.Find("LobbyUI").SendMessage("ChangeSeletedTeamCar",gameObject.name,SendMessageOptions.DontRequireReceiver);
				GameObject.Find("LobbyUI").SendMessage("SelectedTeamLVChange");
				GV.TeamChangeFlag = 1;
			}else{
				GameObject.Find("LobbyUI").SendMessage("SetSelectedTeamCar",gameObject.name,SendMessageOptions.DontRequireReceiver);
				GV.TeamChangeFlag = 2;
			}
			
			GameObject.Find("Audio").SendMessage("CompleteSound");
		}));

	

		return;

	}

	public void OnSelectedRepair(){
		GameObject.Find("LobbyUI").SendMessage("OnRepair", gameObject.name,SendMessageOptions.DontRequireReceiver);
	}


	IEnumerator SetCarInTeam(int teamId, int carIdx, System.Action Callback){
		Global.isNetwork = true;
		Dictionary<string, int> mDic = new Dictionary<string,int>();
		mDic.Add("teamId",teamId); // 팀 차량 지정
		mDic.Add("carIdx",carIdx);
		bool bConnect = false;
		string mAPI = ServerAPI.Get(90008);// "game/team/car"
		NetworkManager.instance.HttpFormConnect("Put", mDic, mAPI , (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{

			}else {Utility.LogError("state " + status + " Msg : " + thing["msg"]);}
			Global.isNetwork = false;
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
		if(Callback != null)
			Callback();
	}
}

