using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;
public class Common_Sponsor_Status : MonoBehaviour {

	[System.Serializable]
	public class Item
	{
		public string ID;
		public string Name;
		public string Text;
		public int BonusT1;
		public int BonusT2;
		public int BonusT3;
		public int BonusT4;
		public int BonusT5;
		public int BonusT6;
		public int BonusT7;
		public int BonusT8;
		public int Period;
		public int Res;
		public string img;
		public int BuyPrice;
		public int ReqLV;
		public int Power_inc;
		public int Dura_dec;
		public int Led_dec;

	}
	
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	[SerializeField] 
	Item[] array;
	
	static Common_Sponsor_Status instance;
	
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
	public static  int GetDictionaryCount(){
		int  count =  instance.dictionary.Count;
		//dictionary.Count;
		return count;
		
	}
	

	public void SetDataFileEx(string data)
	{
		dictionary = JsonReader.Deserialize<Dictionary<string, Item>>(data);


		foreach (KeyValuePair<string, Item> pair in dictionary)
		{
			Item item = pair.Value;
			item.Name =  KoStorage.GetKorString(item.Name);//TableManager.ko.dictionary[item.Name].String;
			item.Text =  KoStorage.GetKorString(item.Text);//TableManager.ko.dictionary[item.Text].String;
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

	public class ItemCollection{
		public List<Item> Data = new List<Item>();
	}
	
	public void SetDataFile(string data)
	{	//dictionary.Clear();
		if(dictionary.Count !=0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("09_13_sponsor", typeof(TextAsset));
		data= tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
		}
		foreach (KeyValuePair<string, Item> pair in dictionary)
		{
			Item item = pair.Value;
		//	item.Name = TableManager.ko.dictionary[item.Name].String;
	//		item.Text = TableManager.ko.dictionary[item.Text].String;
			item.Name = KoStorage.getStringDic(item.Name);
			item.Text =  KoStorage.getStringDic(item.Text);
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
