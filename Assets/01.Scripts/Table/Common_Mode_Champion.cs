using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class Common_Mode_Champion : MonoBehaviour {

	[System.Serializable]
	public class Item
	{
		public string ID;
		public int Season; 
		public int SeasonLV;
		public string Name; 
		public int Car; 
		public int Crew;
		public int Track; 
		public int Sponsor;
		public int Car_Cp;
		public int Car_ID;
		public int Crew_ID;
		public int R_ID; 
		public int R_car;
		public string R_class;
		public string R_open;

		public int S1_5_Regular_Drag;
		public int S2_1_Ranking;
		public int S2_3_PVP_Drag;
		public int S2_5_Event_Featured;
		public int S3_1_Clubrace;
		public int S3_3_PVP_City;
		public int S4_1_Event_Evocube;

		public string Car_Class;
	}
	
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	[SerializeField] 
	Item[] array;
	
	static Common_Mode_Champion instance;
	
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
	
	public void SetDataFileEx(string data)
	{
		dictionary = JsonReader.Deserialize<Dictionary<string, Item>>(data);
		
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

	public class ItemCollection{
		public List<Item> Data = new List<Item>();
	}
	
	public void SetDataFile(string data)
	{	//dictionary.Clear();
		if(dictionary.Count !=0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("24_race_champ", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
		}

		foreach (KeyValuePair<string, Item> pair in dictionary)
		{
			Item item = pair.Value;
			item.Name = KoStorage.GetKorString(item.Name);
		}

		#if UNITY_EDITOR
		ParsingToArray();
		#endif  
	}


	void OnDestroy(){
		//instance = null;
	}
	void OnApplicationQuit(){
		instance = null;
	}
}
