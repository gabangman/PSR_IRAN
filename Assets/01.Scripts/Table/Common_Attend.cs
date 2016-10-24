using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class Common_Attend : MonoBehaviour {

	[System.Serializable]
	public class Item
	{
		public string ID;
		public string Name;
		public string Text;
		public int Catagory;
		public string Target;
		public int R_type;
		public int R_no;
		public int AD;
	}
	
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	[SerializeField] 
	Item[] array;
	
	static Common_Attend instance;
	
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
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("15_reward", typeof(TextAsset));
		data  = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
		}
	//	foreach (KeyValuePair<string, Item> pair in dictionary)
	//	{
	//		Item item = pair.Value;
	//		item.Name = KoStorage.GetKorString(item.Name);
	//		item.Text = KoStorage.GetKorString(item.Text);
	//	}
		#if UNITY_EDITOR
		ParsingToArray();
		#endif  
	}
	void OnApplicationQuit(){
		instance = null;
	}
}


