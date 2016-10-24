using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;
public class Common_Crew_Status : MonoBehaviour {
		[System.Serializable]
		public class Item
		{
		public string ID;
		public string Name;
		public string Text;
		public string Class;
		public int ReqLV;
		public int UpLimit;
		public int Driver;
		public int Tire_1t;
		public float Tire;
		public int Chief_1t;
		public float Chief;
		public int Jack_1t;
		public float Jack;
		public int Gas_1t;
		public float Gas;
		public string ImgQ;
		public string Img_logo;
		public int StarLV;
		public int Plus_Perform;
		public int P_MIN;
		public int P_MAX;
	}
		
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	public static List<int> crewListItem = new List<int>();	
	[SerializeField] 
	Item[] array;
		
		static Common_Crew_Status instance;
		
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
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("01_12_crew", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		//dictionary.Clear();
		crewListItem.Clear();
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
			crewListItem.Add(int.Parse(_item.ID));
		}
		foreach (KeyValuePair<string, Item> pair in dictionary)
		{
			Item item = pair.Value;
			item.Name = KoStorage.GetKorString(item.Name);
			item.Text =  KoStorage.GetKorString(item.Text);

			item.Tire = (float)item.Tire_1t * 0.001f;
			item.Gas = (float)item.Gas_1t*0.001f;
			item.Chief = (float)item.Chief_1t*0.001f;
			item.Jack = (float)item.Jack_1t*0.001f;
			item.StarLV = 0;
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
