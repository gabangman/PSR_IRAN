using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class Common_Team : MonoBehaviour {

	[System.Serializable]
	public class Item
	{
		public string ID;
		public int ReqLV;
		public int Crew;// Text;
		public string Name;
		public int Model;
		public int Car_free;
		public string Sponsor_avilable_code;
		public string Car;
		public int Res;
		public int Buyprice;
		public int Team_x;
	}
	
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	public static List<string> StockTeamList =  new List<string>();
	public static List<string> TourTeamList= new List<string>(); 
	public static List<int> stockCrewList =  new List<int>();
	public static List<int> tourCrewList= new List<int>(); 
	static Common_Team instance;
	
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
	
	[SerializeField] 
	Item[] array;
	public class ItemCollection{
		public List<Item> Data = new List<Item>();
	}
	
	public void SetDataFile(string data)
	{
		//dictionary.Clear();
		if(dictionary.Count !=0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("01_1_team", typeof(TextAsset));
		data = tx.text;
		dictionary.Clear();
		TourTeamList.Clear();
		StockTeamList.Clear();
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
			if(_item.Model == 1){
				StockTeamList.Add (_item.ID);
				stockCrewList.Add(_item.Crew);
			}
			else{ 
				TourTeamList.Add(_item.ID);
				tourCrewList.Add(_item.Crew);
			}
		}
		foreach (KeyValuePair<string, Item> pair in dictionary)
		{

			Item item = pair.Value;
			item.Name = KoStorage.GetKorString(item.Name);
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

	void OnDestroy(){
		//instance = null;
	}
	void OnApplicationQuit(){
		instance = null;
	}
}

