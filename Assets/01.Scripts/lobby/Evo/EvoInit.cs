using UnityEngine;
using System.Collections;

public class EvoInit : MonoBehaviour {

	GameObject starTarget;
	// Use this for initialization
	public delegate void OnFinish();
	public OnFinish onFinish;
	void Start () {
		
	}
	
	public void ChangeCardName(){
		string[] name = gameObject.name.Split("_"[0]);
		int partID = int.Parse(name[1]);
		if(partID < 5099){
			ConverterPartIDStar(partID);
		}else{
			ConverterCrewPartIDStar(partID);
		}
		
	}
	
	void ConverterCrewPartIDStar(int partID){
		return;
	
	}
	
	void ConverterPartIDStar(int partID){
		return;
	
	}
	
	public void Show(GameObject obj){
		if(obj == null){}
		starTarget =  ObjectManager.CreateTagPrefabs("CardButton") as GameObject;
		if(starTarget == null) {
			var tempObj = GameObject.FindGameObjectWithTag("CenterAnchor") as GameObject;
			starTarget = ObjectManager.CreateLobbyPrefabs(tempObj.transform.GetChild(0), "Card", "EvolutionRepair", "CardButton");
			starTarget.GetComponent<TweenPosition>().enabled =true;
		}else{
		}
	}
	
	
	public void Hidden(){
		starTarget.GetComponent<TweenAction>().ReverseTween(starTarget);
		return;
	}
	
	public void tempHidden(){
		starTarget.GetComponent<TweenAction>().tempHidden();
	}
	
	void OnDestroy(){
		starTarget = null;
		onFinish = null;
	}

	void OnRepairRelay(string carname){
	//	string[] carIDs = carname.Split('_');
	//	int carID = int.Parse(carIDs[0]);
		starTarget.transform.FindChild("Evolution").gameObject.SetActive(false);
		var child = starTarget.transform.FindChild("Repair").gameObject as GameObject;
		child.SetActive(true);
		starTarget.GetComponent<TweenAction>().doubleTweenScale(child);
		onFinish();
		var _card = child.GetComponent<RepairAction>();
		if(_card == null) _card = child.AddComponent<RepairAction>();
		_card.repairWindowInfo(carname);
		_card.OnRepairDes(()=>{
			onFinish();
			//ChangeCardName();
		});
		
	}
	
	
	void OnStarCarPart(){
		var child  = starTarget.transform.FindChild("Evolution").gameObject as GameObject;
		child.SetActive(true);
		onFinish();
		var _card = child.GetComponent<StarUpgrade>();
		if(_card == null) _card = child.AddComponent<StarUpgrade>();
		string[] name = gameObject.name.Split("_"[0]);
		int partID = int.Parse(name[1]);
		_card.starCarWindowInfo(int.Parse(name[0]), partID);
		_card.onFinish = delegate(){
			onFinish();
		};
	}
	
	void OnStarCrew(){
		return;/*
		var child  = starTarget.transform.FindChild("Evolution").gameObject as GameObject;
		child.SetActive(true);
		starTarget.GetComponent<TweenAction>().doubleTweenScale(child);
		onFinish();
		var _card = child.GetComponent<StarUpgrade>();
		if(_card == null) _card = child.AddComponent<StarUpgrade>();
		string[] name = gameObject.name.Split("_"[0]);
		int partID = int.Parse(name[1]);
		_card.starCrewWindowInfo(partID);
		_card.onFinish = delegate(){
			onFinish();
			ChangeCardName();
		};*/
	}
}
