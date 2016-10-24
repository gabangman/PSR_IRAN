using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;
public class Common_ClubAI : MonoBehaviour {

	[System.Serializable]
	public class Item
	{
		/*public string ID;
		public int Entry_no;
		public int Sort_no;
		public string Name;
		public int club_crew;
		public int club_car;
		public int club_spon;
		public int club_crew_ai;
		public int club_car_ai;*/
		//{"ID":6114, "Star":15, "Car":1014, "Crew":1203, "Track":1403, "Sponsor":1305, "Car_Cp":1014, "Car_ID":6198, "Crew_ID":6529, "Car_Class
		public string ID;
		public int Star;
		public int Car;
		public int Crew;
		public int Track;
		public int Sponsor;
		public int Car_Cp;
		public int Car_ID;
		public int Crew_ID;
		public string Car_Class;
	}
	
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	[SerializeField] 
	Item[] array;
	
	static Common_ClubAI instance;
	
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
	{
		//dictionary.Clear();
		if(dictionary.Count !=0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("24_race_club", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
		}/*
		foreach (KeyValuePair<string, Item> pair in dictionary)
		{
			//Item _item = pair.Value;
			//_item.Name = KoStorage.GetKorString(_item.Name);
		}*/
		
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

