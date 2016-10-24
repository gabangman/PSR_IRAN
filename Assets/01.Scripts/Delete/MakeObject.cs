using UnityEngine;
using System.Collections;

public class MakeObject : MonoBehaviour {


	/*

	public GameObject CreatePrefabs(Transform _parent, GameObject obj){
		
		var car = Instantiate(obj) as GameObject;
		car.transform.parent = _parent.transform.parent;
		car.transform.localScale = Vector3.one;
		car.transform.localPosition = Vector3.zero;
		car.transform.localEulerAngles = Vector3.zero;
		return car;
	}
	
	public GameObject CreatePrefabs(Transform _parent, string name){
		var obj = Resources.Load("Prefabs/Lobby/"+name, typeof(GameObject)) as GameObject;
		var car = Instantiate(obj) as GameObject;
		car.transform.parent = _parent.transform.parent;
		car.transform.localScale = Vector3.one;
		car.transform.localPosition = Vector3.zero;
		car.transform.localEulerAngles = Vector3.zero;
		Resources.UnloadUnusedAssets();
		return car;
	}
	
	public GameObject CreatePrefabs(string prefabsPath, string prefabsName, Transform parent){
		var prefabs = Resources.Load(prefabsPath+ prefabsName, typeof(GameObject)) as GameObject;
		var _object = Instantiate(prefabs) as GameObject;
		_object.transform.parent = parent;
		PrefabsInitilze(_object, Vector3.zero, Vector3.zero, Vector3.one);
		Resources.UnloadUnusedAssets();
		return _object;
	}
	
	public void PrefabsInitilze(GameObject Car, Vector3 pos, Vector3 rot, Vector3 scale){
		Car.transform.localScale = scale;
		Car.transform.localPosition = pos;
		Car.transform.localEulerAngles = rot;
	}
	
	
	

	
	public void DestoryPrefabs(GameObject obj){
		//var _obj = obj.GetComponent<CCrewCreate>();
		Destroy(obj);
	}
	
	void objectDestory(){
		Destroy(this);
	}

*/
}
