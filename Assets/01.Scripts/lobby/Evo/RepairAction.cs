using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RepairAction : MonoBehaviour {

	System.Action Callback;
	public GameObject repair, effect, parent;
	public UISlider per;
	public UILabel lbtext, lbdura,lbduraAll;


	public void OnRepairDes(System.Action callback){
		repair.SetActive(true);
		this.Callback = callback;
	//	string[] name = gameObject.name.Split('_');
	}
	
	public void InitWindow (int pDurability, string name) {
		return;/*
		var rObj = transform.FindChild("Content_Repair") as Transform;
		rObj.FindChild("lbTitle").GetComponent<UILabel>().text = "수리하기";		
		if(pDurability == 100){
			rObj.FindChild("btn_ok").gameObject.SetActive(true);	
			rObj.FindChild("btn_all").gameObject.SetActive(false);
			rObj.FindChild("btn_part").gameObject.SetActive(false);
			rObj.FindChild("lbPriceAll").gameObject.SetActive(false);
			rObj.FindChild("lbPricePart").gameObject.SetActive(false);
			rObj.FindChild("lbRepairAll").gameObject.SetActive(false);
			rObj.FindChild("lbRepairPart").gameObject.SetActive(false);
		//	rObj.FindChild("lbText").GetComponent<UILabel>().text = "내구도100이라 수리 못해";	
			rObj.FindChild("Progress Bar").GetChild(1).GetComponent<UISprite>().fillAmount = 1.0f;
			rObj.FindChild("Progress Bar").GetChild(1).GetComponent<UISprite>().color = Color.cyan;
		}else{
			rObj.FindChild("btn_ok").gameObject.SetActive(false);	
			rObj.FindChild("btn_all").gameObject.SetActive(true);
			rObj.FindChild("btn_part").gameObject.SetActive(true);
			rObj.FindChild("lbPriceAll").gameObject.SetActive(true);
			rObj.FindChild("lbPricePart").gameObject.SetActive(true);
			rObj.FindChild("lbRepairAll").gameObject.SetActive(true);
			rObj.FindChild("lbRepairPart").gameObject.SetActive(true);
		//	rObj.FindChild("lbText").GetComponent<UILabel>().text = "수리할래?";
			rObj.FindChild("Progress Bar").GetChild(1).GetComponent<UISprite>().fillAmount = pDurability*0.01f;
			if(pDurability >50) rObj.FindChild("Progress Bar").GetChild(1).GetComponent<UISprite>().color = Color.cyan;
			else if(pDurability > 25) rObj.FindChild("Progress Bar").GetChild(1).GetComponent<UISprite>().color = Color.yellow;
			else rObj.FindChild("Progress Bar").GetChild(1).GetComponent<UISprite>().color = Color.red;
		}
		this.pName = name;*/
	}
	


	void OnClose(){
		if(Callback !=null)
			Callback();
		Callback = null;
		parent.SendMessage("OnClose");
		Global.bLobbyBack = false;
	}

	void updateInfo(bool b){
	
	}

	
	void OnRepairTeam(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnClose();
		};
		if(UserDataManager.instance.buyPriceCheck(repairPrice)) {
			Global.isNetwork = false;
			OnClose();
			return;
		}

		//CarInfo _carInfos = GV.getTeamCarInfo(GV.SelectedTeamID);
		StartCoroutine(RepairDurabilityOfCar(()=>{
			//	CarInfo _carInfo = GV.getTeamCarInfo(GV.SelectedTeamID);
			//	_carInfo.carClass.Durability = _carInfo.carClass.DurabilityRef;
			GV.updateDollar = repairPrice;
			GV.myDollar -= repairPrice;
			RepairCar.carClass.Durability = RepairCar.carClass.DurabilityRef;
			per.transform.GetChild(1).GetComponent<UISprite>().color = new Color32(0,139,255,255);
			
			GV.TeamChangeFlag =3;
			GameObject.Find("Audio").SendMessage("CompleteSound");
			GameObject.Find("LobbyUI").SendMessage("RepairLobbyAction", SendMessageOptions.DontRequireReceiver);
			//RepairCar = null;
			OnConfirm(RepairCar);





		},RepairCar.CarIndex, RepairCar.CarID, repairPrice ));
	}
	
	void OnRepairTeamMain(){
		if(Global.isNetwork) return;
		Global.isNetwork = true;

		if(UserDataManager.instance.buyPriceCheck(repairPrice)) {
			Global.isNetwork = false;
			OnClose();
			return;
		}
		//CarInfo _carInfos = GV.getTeamCarInfo(GV.SelectedTeamID);
		StartCoroutine(RepairDurabilityOfCar(()=>{
			CarInfo _carInfo = GV.getTeamCarInfo(this.repairTeamID);
			_carInfo.carClass.Durability = _carInfo.carClass.DurabilityRef;
			GV.updateDollar = repairPrice;
			GV.myDollar -= repairPrice;
			per.transform.GetChild(1).GetComponent<UISprite>().color = new Color32(0,139,255,255);
			transform.parent.gameObject.SendMessage("SetDurabilityMain");
			GameObject.Find("Audio").SendMessage("CompleteSound");
			GameObject.Find("LobbyUI").SendMessage("RepairLobbyAction", SendMessageOptions.DontRequireReceiver);
			OnConfirm(_carInfo);
			this.repairTeamID = 0;
		},RepairCar.CarIndex, RepairCar.CarID, repairPrice));
	}

	void OnConfirm(CarInfo mCarInfo){
		SetDurabilityStatus(mCarInfo,mCarInfo.CarID);
		this.RepairCar = null;
	}

	private IEnumerator RepairDurabilityOfCar(System.Action callback, int caridx, int carid, int dollar){
		Dictionary<string, int> mDic = new Dictionary<string, int>();
		bool bConnect = false;
		mDic.Clear();
		mDic.Add("carIdx",caridx);
		mDic.Add("carId",carid);
		mDic.Add("carClass", GV.mineCarList.Find(obj=>obj.CarIndex == caridx).nClassID);
		mDic.Add("dollar",dollar);
		string mAPI = ServerAPI.Get(90063);//"game/car/durability"
		NetworkManager.instance.HttpFormConnect("Put",mDic,mAPI, (request)=>{

			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				callback();

			}else{
				OnClose();
			}
			bConnect = true;
			Global.isNetwork = false;
			
		});
		while(!bConnect){
			yield return null;
		}
	}


	void OnOk(){
		this.repairTeamID = 0;
		OnClose();
	
	}

	public void repairWindowInfo(string carName){
		//!!--Utility.Log("repairWindowInfo " + carName);
		repair.SetActive(true);
		this.repairPrice = 0;
		string[] strCar = carName.Split('_');
		CarInfo _carinfo = GV.mineCarList[int.Parse(strCar[2])];
		if(_carinfo.TeamID != 0){
			this.repairTeamID = _carinfo.TeamID;
			_carinfo = GV.getTeamCarInfo(_carinfo.TeamID);
		}
		if(_carinfo == null) Utility.LogError("CarInfo is Null");
		SetDurabilityStatus(_carinfo, int.Parse(strCar[0]));
	}
	
	
	private int repairPrice = 0;
	private int repairTeamID = 0;
	public void repairTeamCar(int teamID = 0){
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnClose();
		};
		this.repairPrice = 0;
		int carID =0;
		if(teamID == 0){
			repairTeamID = GV.SelectedTeamID;
			carID = GV.getTeamCarID(GV.SelectedTeamID);
			SetDurabilityStatus(GV.getTeamCarInfo(GV.SelectedTeamID),carID);
		}else{
			repairTeamID = teamID;
			carID = GV.getTeamCarID(teamID);
			SetDurabilityStatus(GV.getTeamCarInfo(teamID),carID);
		}
	
	}
	
	protected CarInfo RepairCar;
	void SetDurabilityStatus(CarInfo _carInfo, int carID){
		this.RepairCar = _carInfo;
		int pDura = _carInfo.carClass.Durability; 
		//!!--Utility.Log("Dura " + pDura);
		//pDura = 20;
		int CarDura = _carInfo.carClass.DurabilityRef;
		var tr = transform.FindChild("Repair_Content") as Transform;
		var tr1 = tr.FindChild("btnRepair") as Transform;
		var tr2 = tr.FindChild("btnNoRepair") as Transform;
		//tr2.transform.gameObject.SetActive(false);
		if(CarDura == pDura){
			tr1.gameObject.SetActive(false);
			tr2.gameObject.SetActive(false);
			lbtext.text = KoStorage.GetKorString("76503");
			//per.transform.gameObject.SetActive(false);
			tr2.FindChild("lbOk").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");
			per.sliderValue = 1.0f;
			lbdura.text = string.Format("{0}%",100);
			//lbdura.text = string.Format("{0}",CarDura);
			lbduraAll.text = string.Format("{0}/{1}", CarDura, CarDura);
			per.transform.GetChild(1).GetComponent<UISprite>().color =new Color32(0,139,255,255);
		}else{
			tr1.gameObject.SetActive(true);
			tr2.gameObject.SetActive(false);
			lbtext.text = KoStorage.GetKorString("76501"); 
			tr1.FindChild("lbOk").GetComponent<UILabel>().text =   KoStorage.GetKorString("76502");
			//	per.transform.gameObject.SetActive(true);
			tr1.FindChild("icon").gameObject.SetActive(true);
			tr1.FindChild("lbRes").gameObject.SetActive(true);
			
			float diff1 = (float)pDura / (float)CarDura;
			int rPrice = (CarDura - pDura)*_carInfo.carClass.priceRepair; 
			this.repairPrice = rPrice;
			per.sliderValue = diff1;
			tr1.FindChild("lbRes").GetComponent<UILabel>().text = string.Format("{0:#,0}", rPrice);
			int mCnt = (int)(diff1*100);
			lbdura.text = string.Format("{0}%", mCnt);
			//lbdura.text = string.Format("{0}", (int)(pDura));
			lbduraAll.text = string.Format("{0}/{1}", (int)pDura, CarDura);
			//	lbduraAll.text = string.Format("{0}", CarDura);
			if(mCnt <= 20){
				//	per.transform.GetChild(4).gameObject.SetActive(true);
				per.transform.GetChild(1).GetComponent<UISprite>().color =new Color32(255,0,0,255);
			}else if( mCnt <50){
				per.transform.GetChild(1).GetComponent<UISprite>().color = new Color32(0,139,255,255);
				//	per.transform.GetChild(4).gameObject.SetActive(false);
			}else{
				per.transform.GetChild(1).GetComponent<UISprite>().color =new Color32(0,139,255,255);
				//	per.transform.GetChild(4).gameObject.SetActive(false);
			}
		}
	}

	string getSpritePartName(int part){
		return string.Empty;
	}
}
