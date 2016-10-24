using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class ModeAICrew : MonoBehaviour {
	
	[System.Serializable]
	public class Item
	{
		public string ID;
		public int CrewMin;// PartName;
		public int CrewMax;//string Text;
		public int Time_Pit_1t;// Text2;
		public float Time_Pit;// Text2;
		public int EntFee;// Ratio;
	}
	
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	[SerializeField] 
	Item[] array;
	
	static ModeAICrew instance;
	void Awake()
	{
		instance = this;
	}
	public static  int GetDictionaryCount(){
		int  count =  instance.dictionary.Count;
		//dictionary.Count;
		return count;
		
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
			int id = 6500+i;
			Item tItem = Get(id);
			if(tItem.CrewMax >= tarAbility && tItem.CrewMin <= tarAbility){
				//Utility.Log(string.Format("{0} , {1} " , tItem.CrewMax, tItem.CrewMin));
				//Utility.Log(Global.gCrewAbility + "  --  " + id);

				item = tItem;
				break;
			}
		}
		if(item == null) Utility.LogError(" crew item is null");	
		return item;
	}
	
	public static float GetPitTime(int tarAbility){
		return GetRangeItem(tarAbility).Time_Pit;
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
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("01_crew_ai", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
		}

		foreach (KeyValuePair<string, Item> pair in dictionary)
		{
			Item item = pair.Value;
			item.Time_Pit = item.Time_Pit_1t*0.001f;//KoStorage.getStringDic(item.Name);
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
