using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class Common_Cost : MonoBehaviour {
	
	[System.Serializable]
	public class Item
	{
		public string ID;
		public string Name;
		public string Text;
		public int Recharge_type;
		public int Recharge_no;
		public int type;
		public string Cash_d;
		public string Cash_won;
		public string Cash_r;
		public string Cash_w;
		public string Cash_y;
		public string Cash_e;
		public int Change;
		public int Cash_Exp;
	}
	
	[System.Serializable]
	public class PackageItem
	{
		public string ID;
		public string Name;
		public string Text;
		public int Coupon_type;
		public int Car_Class;
		public int Recharge_Coupon;
		public int Recharge_Cube;
		public int type;
		public string Cash_d;
		public string Cash_won;
		public string Cash_r;
		public string Cash_w;
		public string Cash_y;
		public string Cash_e;
		public int Cash_Exp;
		public int Evo_LV;
		public int Up_LV;
	}
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	public Dictionary<string, PackageItem> Pkgdictionary = new Dictionary<string, PackageItem>();
	[SerializeField] 
	Item[] array;
	
	static Common_Cost instance;
	
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
	public static PackageItem PkgGet(int typeID)
	{ 
		PackageItem item;
		//typeID = 5000;
		if (!instance.Pkgdictionary.TryGetValue(typeID.ToString(), out item))
			throw new KeyNotFoundException(typeID + " of " + instance.GetType() + " was not found.");
		return item;
	}

	//public int count;
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
			item.Name = KoStorage.getStringDic(item.Name);

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
	{//dictionary.Clear();
		if(dictionary.Count !=0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("09_item_cost", typeof(TextAsset));
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
			item.Text = KoStorage.GetKorString(item.Text);
		}
		#if UNITY_EDITOR
		ParsingToArray();
		#endif  
	}

	public class ItemPkgCollection{
		public List<PackageItem> Data = new List<PackageItem>();
	}
	
	public void SetDataFilePackage(string data)
	{

		//Pkgdictionary.Clear();
		//dictionary.Clear();
		if(Pkgdictionary.Count !=0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("09_item_package", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemPkgCollection _data = JsonReader.Deserialize<ItemPkgCollection>(data);
		foreach(PackageItem _item  in _data.Data){
			Pkgdictionary.Add(_item.ID, _item);
		}
		foreach (KeyValuePair<string, PackageItem> pair in Pkgdictionary)
		{
			PackageItem item = pair.Value;
			item.Name = KoStorage.GetKorString(item.Name);
			item.Text = KoStorage.GetKorString(item.Text);
		}
		
	}



	void OnDestroy(){
		//instance = null;
	}
	void OnApplicationQuit(){
		instance = null;
	}
}

