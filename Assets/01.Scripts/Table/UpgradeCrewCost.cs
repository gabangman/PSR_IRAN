using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class UpgradeCrewCost : MonoBehaviour {

	[System.Serializable]
	public class Item
	{
		public string ID;
		public int TeamNum;
		public int CrewNum;
		public int Upgrade_LV;
		public int UpCoin;
		
	}
	
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	[SerializeField] 
	Item[] array;
	
	static UpgradeCrewCost instance;

	void Awake()
	{
		instance = this;
	}
	
	public static Item Get(int typeID)
	{ 
		Item item;
		if (!instance.dictionary.TryGetValue(typeID.ToString(), out item))
			throw new KeyNotFoundException(typeID + " of " + instance.GetType() + " was not found.");
		return item;
	}
	
	
	
	public class ItemCollection{
		public List<Item> Data = new List<Item>();
	}
	
	public void SetDataFile(string data)
	{
		//dictionary.Clear();
		if(dictionary.Count !=0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("02_crew_upgrade", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+ data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		dictionary.Clear();
	
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
		}
		#if UNITY_EDITOR
		ParsingToArray();
		#endif  
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
