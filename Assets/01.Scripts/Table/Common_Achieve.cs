using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class Common_Achieve : MonoBehaviour {

	[System.Serializable]
	public class Item
	{
		public string ID;
		public string Name;
		public string Text;
		public int Target_output;
		public int Target_number;
		public int R_type;
		public int R_no;
		public int R_exp;
		public string Google_ID;
		public string Apple_ID;
		public int Point;
	}

	//	{"ID":16500, "Season_num":1, "Nut_num":3, "Target_num":9, "R_type":3, "R_S_1":3, "R_exp":15},
	[System.Serializable]
	public class ItemDaily
	{
		public string ID;
		public int Season_num;
		public int Nut_num;
		public int Target_num;
		public int R_type;
		public int R_S_1;
		public int R_exp;
	
	}


	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	public Dictionary<string, ItemDaily> dictionaryDaily = new Dictionary<string, ItemDaily>();
	static Common_Achieve instance;
	
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

	public static ItemDaily GetDaily(int typeID)
	{ 
		ItemDaily item;
		//typeID = 5000;
		if (!instance.dictionaryDaily.TryGetValue(typeID.ToString(), out item))
			throw new KeyNotFoundException(typeID + " of " + instance.GetType() + " was not found.");
		return item;
	}
	public static int getCount(){
		return instance.dictionary.Count;
	}
	
	public class ItemCollection{
		public List<Item> Data = new List<Item>();
	}

	public class ItemCollectionDaily{
		public List<ItemDaily> Data = new List<ItemDaily>();
	}

	public void SetDataFile(string data)
	{
		//dictionary.Clear();
		if(dictionary.Count !=0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("16_achieve", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
		}
		foreach (KeyValuePair<string, Item> pair in dictionary)
		{
				Item _item = pair.Value;
			_item.Name = KoStorage.GetKorString(_item.Name);
			_item.Text =  KoStorage.GetKorString(_item.Text);
		}
		GAchieve.instance.achieveInfo.SetTargetNumberAchievement();
	}

	public void SetDataFileDaily()
	{
		if(dictionaryDaily.Count !=0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("16_achieve_daily", typeof(TextAsset));
		string data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollectionDaily _data = JsonReader.Deserialize<ItemCollectionDaily>(data);
		foreach(ItemDaily _item  in _data.Data){
			dictionaryDaily.Add(_item.ID, _item);
		}

		if(QuestData.instance.mQuest.mGQuest.index == -1) GV.QuestReset = true;
		if(GV.QuestReset){
			QuestData.instance.mQuest.setDailyInit();
			GV.QuestReset = false;
		}


	}
	
	void OnDestroy(){
		//instance = null;
	}
	void OnApplicationQuit(){
		instance = null;
	}
}
