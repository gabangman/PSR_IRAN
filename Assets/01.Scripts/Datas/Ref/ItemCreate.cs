using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class ItemCreate : ScriptableObject {
	/*
	void Awake(){
	
		Utility.LogWarning("Itemcreate Awake");
	
	}

	void OnEnable(){
	
	}

	void OnDestroy(){
		Utility.LogWarning("Itemcreate Destroy");
	}


	void OnGUI(){
		//TempObj = (GameObject) EditorGUILayout.ObjectField("Temp Test", TempObj ,typeof(GameObject), false);
	
	}
	[SerializeField]
	GameObject TempObj;

	[SerializeField]
	GameObject[] car;
	[SerializeField]
	GameObject[] crew;
	[SerializeField]
	GameObject[] caritem;
	[SerializeField]
	GameObject[] crewitem;
	[SerializeField]
	TextAsset[] _Text;

	public GameObject getCarObject(string name){
		GameObject _tempObject = null;

		foreach(GameObject obj in car){
			if(obj.name.Equals(name)){
				_tempObject = obj;
			}
		}
		return _tempObject;
	}


	public GameObject getCarItemObject(string name){
		GameObject _tempObject = null;
		
		foreach(GameObject obj in caritem){
			if(obj.name.Equals(name)){
				_tempObject = obj;
			}
		}
		return _tempObject;
	}

	public TextAsset getTextAsset(string name){
		TextAsset _tempObject = null;
		
		foreach(TextAsset obj in _Text){
			if(obj.name.Equals(name)){
				_tempObject = obj;
			}
		}
		if(_tempObject == null) Utility.LogError("TextAsset Null");
		return _tempObject;
	}
	*/
}
