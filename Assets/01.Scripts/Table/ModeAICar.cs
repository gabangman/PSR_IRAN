using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class ModeAICar : MonoBehaviour {
	[System.Serializable]
	public class Item
	{
		public string ID;
		public int CarMin;// PartName;
		public int CarMax;//string Text;
		public int StarLV;
		public int Torque_1t;
		public float Torque;// Text2;
		public int Torque_pr;
		public int B_Power_1t;
		public float B_Power;// Time_R2;// Text2;
		public int B_Time_1t;
		public float B_Time;
		public int Skid_Time_1t;
		public float Skid_Time;
		public int Gear_LV;
		public int B_Event;
		public int B_Event_R;
		public int EntFee;// Ratio;
	}
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	[SerializeField] 
	Item[] array;
	
	static ModeAICar instance;
	void Awake()
	{
		instance = this;
	}
	public static Item Get(int typeID)
	{ 
		Item item;
		//typeID = 5000;
		if (!instance.dictionary.TryGetValue(typeID.ToString(), out item))
			throw new KeyNotFoundException(typeID + " of " + instance.GetType() + " was not found.");
		return item;
	}
	public static Item GetRangeItem(int tarAbility){
		Item item = null;
		int count = instance.dictionary.Count;
		
		for(int i = 0; i < count; i++){
			int id = 6100+i;
			Item tItem = Get(id);
			//Utility.Log(string.Format("{0} , {1} " , tItem.CarMax, tItem.CarMax));
			//Utility.Log(Global.gCarAbility + "  --  " + id);
			if(tItem.CarMax >= tarAbility && tItem.CarMin <= tarAbility){
				item = tItem;
				break;
			}
		}
		//if(item == null) Utility.LogError("car item is null");	
		return item;
	}
	public static  int GetDictionaryCount(){
		int  count =  instance.dictionary.Count;
		//dictionary.Count;
		return count;
		
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
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("03_car_ai", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
		}
		foreach (KeyValuePair<string, Item> pair in dictionary)
		{
			Item item = pair.Value;
			item.Torque = item.Torque_1t * 0.001f;
			item.B_Power = item.B_Power_1t * 0.001f;
			item.B_Time = item.B_Time_1t * 0.001f;
			item.Skid_Time = item.Skid_Time_1t * 0.001f;
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
