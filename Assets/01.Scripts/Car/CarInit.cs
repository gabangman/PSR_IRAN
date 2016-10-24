using UnityEngine;
using System.Collections;

public class CarInit : MonoBehaviour {


	public Transform FindWheel(GameObject car, int order){
		return car.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(order);
	}
	public Transform FindPaceWheel(GameObject car, int order){
		return car.transform.GetChild(0).GetChild(4).GetChild(0).GetChild(order);
	}
	public Animation FindAnimation(GameObject Car, string name){
		return Car.transform.GetChild(0).GetChild(0).FindChild(name).GetChild(0).transform.GetComponent<Animation>();
	}

	public GameObject MakeCar(string carId,int id, GameObject parent){
		string name =carId+"_"+id.ToString();
		Utility.Log(name);
		var car = ObjectManager.GetSelectObjectWithAIRace(name);
		car.SetActive(true);
		car.transform.parent = parent.transform.GetChild(0).GetChild(0);
		car.name = carId;
		car.transform.localEulerAngles = Vector3.zero;
		car.transform.localScale = Vector3.one;
		car.transform.localPosition = Vector3.zero;
		return car;
		}
}
