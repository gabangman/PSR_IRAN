using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class Common_Ratio : MonoBehaviour {

	[System.Serializable]
	public class Item
	{
		public string ID;
		public int R_Ratio;
		public int R_Exp;

	}
	
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();

	static Common_Ratio instance;
	
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

	public class ItemCollection{
		public List<Item> Data = new List<Item>();
	}
	
	public void SetDataFile(string data)
	{
		//dictionary.Clear();
		if(dictionary.Count !=0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("15_reward_ratio", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
		}
	}
	void OnDestroy(){
		//instance = null;
	}

	void OnApplicationQuit(){
		instance = null;
	}
}
