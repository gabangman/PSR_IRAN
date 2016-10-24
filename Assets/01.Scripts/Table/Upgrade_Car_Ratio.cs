using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class Upgrade_Car_Ratio : MonoBehaviour {

	[System.Serializable]
	public class Item
	{
		public string ID;
		public string PartName;
		public string Text;
		public string Text2;
		public int Ratio_1t;
		public float Ratio;
		public int UpRatio;

		public int Ratio_2_1t;
		public float Ratio2;
		public int UpRatio_2;

		public int Ratio_3_1t;
		public float Ratio3;
		public int UpRatio_3;

		public int Ratio_4_1t;
		public float Ratio4;
		public int UpRatio_4;

		public int Ratio_5_1t;
		public float Ratio5;
		public int UpRatio_5;
	}
	
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	[SerializeField] 
	Item[] array;
	
	static Upgrade_Car_Ratio instance;
	
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
	{	//dictionary.Clear();
		if(dictionary.Count !=0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("05_car_up", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		dictionary.Clear();
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
		}
		foreach (KeyValuePair<string, Item> pair in dictionary)
		{
			Item item = pair.Value;
			item.PartName = KoStorage.GetKorString(item.PartName);
			item.Text =  KoStorage.GetKorString(item.Text);
			item.Text2 = KoStorage.GetKorString(item.Text2);
		
			item.Ratio = item.Ratio_1t * 0.001f;

			
			item.Ratio2 = item.Ratio_2_1t*0.001f;
			item.Ratio3 = item.Ratio_3_1t*0.001f;
			item.Ratio4 = item.Ratio_4_1t*0.001f;
			item.Ratio5 = item.Ratio_5_1t*0.001f;
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
