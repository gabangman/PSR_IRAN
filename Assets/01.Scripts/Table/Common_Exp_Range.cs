using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;
public class Common_Exp_Range : MonoBehaviour {
	
	[System.Serializable]
	public class Item
	{
		public string ID;
		public int LV;
		public int Max;
	}

	[System.Serializable]
	public class ClubExpItem
	{
		public string ID;
		public int Club_level;
		public int Club_point;
	}

	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	public Dictionary<string, ClubExpItem> Clubdictionary = new Dictionary<string, ClubExpItem>();
	[SerializeField] 
	Item[] array;
	
	static Common_Exp_Range instance;
	
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
	public static ClubExpItem ClubExpGet(int typeID)
	{ 
		ClubExpItem item;
		if (!instance.Clubdictionary.TryGetValue(typeID.ToString(), out item))
			throw new KeyNotFoundException(typeID + " of " + instance.GetType() + " was not found.");
		return item;
	}

	public static int levelCheck(int exp){
		if(exp <= 50) return 1;
		//if(exp == 0) return 1;
		int count = instance.dictionary.Count;
		int mCount = 0;
		int mCnt  =0;
		for(int i = 0; i < count; i++){
			int id = 9000+i;
			Item tItem = Get(id);
			if(tItem.Max >= exp){
				mCount = tItem.LV;
			//	Utility.LogWarning("equal " + tItem.Max + " >= " + exp);
				break;
			}else{
			//	Utility.LogWarning("not equal " + tItem.Max + " < " + exp);
			}
		}
		return mCount;
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


	public class ItemCollectionClub{
		public List<ClubExpItem> Data = new List<ClubExpItem>();
	}


	public void SetDataFile(string data)
	{
		//dictionary.Clear();
		if(dictionary.Count != 0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("01_exp", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
		}

		#if UNITY_EDITOR
		ParsingToArray();
		#endif  
	}

	public void SetClubDataFile(string data)
	{

		if(Clubdictionary.Count !=0) return;
		Clubdictionary.Clear();
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("01_exp_club", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollectionClub _data = JsonReader.Deserialize<ItemCollectionClub>(data);
		foreach(ClubExpItem _item  in _data.Data){
			Clubdictionary.Add(_item.ID, _item);
		}
		
	}


	void OnDestroy(){
		//instance = null;
	}
	void OnApplicationQuit(){
		instance = null;
	}


}
