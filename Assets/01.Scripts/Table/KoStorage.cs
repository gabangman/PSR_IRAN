using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class KoStorage : MonoBehaviour
{
	[System.Serializable]
	public class Item
	{
		public string ID;
		public string String;
	}

	void Awake()
	{
		instance = this;
	}


	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	public Dictionary<string, Item> Tempdictionary = new Dictionary<string, Item>();
	[SerializeField]
	Item[] array;
	
	static KoStorage instance;
	
	public void SetDataFile(string data)
	{
		//Utility.LogWarning("ko " +  instance.dictionary.Count);
		if(instance.dictionary.Count != 0 )return;
		dictionary = JsonReader.Deserialize<Dictionary<string, Item>>(data);
	//	#if UNITY_EDITOR
	//	ParsingToArray();
	//	#endif
	}
	
	
	public static string getStringDic(string ID){

		return instance.dictionary[ID].String;
	}
	/*
	public static string GetKorString(string ID){
		string ko = instance.dictionary[ID].String;
		if(string.IsNullOrEmpty(ko)) Utility.LogWarning(ko);
		return ko;//instance.dictionary[ID].String;
	}
	*/
	public static string GetKorString(string typeID)
	{ 
		Item item;
		if (!instance.dictionary.TryGetValue(typeID, out item))
			throw new KeyNotFoundException(typeID + " of " + instance.GetType() + " was not found.");
		return item.String;
	}



	public static KoStorage kostroage{
		get{
			return instance;
		}
	}
	
	void ParsingToArray()
	{
		array = new Item[dictionary.Count];
		int count = 0;
		
		foreach (var pair in dictionary)
		{
			Item entity = new Item();
			entity = pair.Value;
			entity.ID = pair.Key;
			
			array[count++] = entity;
		}
	}

	void OnDestroy(){
		//instance = null;
	}
	void OnApplicationQuit(){
		instance = null;
	}


}