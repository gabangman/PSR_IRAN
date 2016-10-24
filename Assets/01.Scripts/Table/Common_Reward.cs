using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class Common_Reward : MonoBehaviour {
	
	[System.Serializable]
	public class Item
	{
		public string ID;
		public int Season;
		public int SeasonLV;
		public int Refresh_weekly;
		public int Entry_Weekly;

		public int Refresh_drag;
		public int Entry_drag;
		public int Reward_PVP_drag;

		public int Refresh_timesquare;
		public int Entry_timesquare;
		public int Reward_mat2;
		public int Reward_mat_timesquare;

		public int Reward_newcar;
		public int Reward_mat3;
		public int Reward_selectcar;
		public int Reward_resource;

		public int Refresh_regular;
		public int Entry_regular_stock;
		public int Reward_regular_stock;

		public int Entry_regular_drag;
		public int Reward_mat1_regular_drag;
		public int Reward_mat_regular_drag;

		public int Champ_coin;
		public int Champ_dollar;
		public int Common_perfect;
		public int Common_good;

		public int MatchPro_D;
		public int MatchPro_C;
		public int MatchPro_B;
		public int MatchPro_A;
		public int MatchPro_S;
		public int MatchPro_SS;
			//"MatchPro_D":100, "MatchPro_C":0, "MatchPro_B":0, "MatchPro_A":0, "MatchPro_S":0, "MatchPro_SS":0},
			//	NewBtnReset (shop[1], myAcc.instance.account.bLobbyBTN[4]);
	}

	[System.Serializable]
	public class ClubItem
	{
		public string ID;
		public int Num;
		public int Reward_1;
		public int Reward_2;
		public int Reward_3;
		public int WinBonus;
	
		//6700
	}
	
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	public Dictionary<string, ClubItem> Clubdictionary = new Dictionary<string, ClubItem>();
	[SerializeField] 
	Item[] array;
	
	static Common_Reward instance;
	
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

	public static ClubItem ClubGet(int typeID)
	{ 
		ClubItem item;
		//typeID = 5000;
		if (!instance.Clubdictionary.TryGetValue(typeID.ToString(), out item))
			throw new KeyNotFoundException(typeID + " of " + instance.GetType() + " was not found.");
		return item;
	}
	
	//public int count;
	public static  int GetDictionaryCount(){
		int  count =  instance.dictionary.Count;
		//dictionary.Count;
		return count;
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
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("15_reward_race", typeof(TextAsset));
		data =tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
		}
		#if UNITY_EDITOR
		ParsingToArray();
		#endif  
	}
	public class ClubItemCollection{
		public List<ClubItem> Data = new List<ClubItem>();
	}
	public void SetClubDataFile(string data)
	{
		if(Clubdictionary.Count !=0) return;
		Clubdictionary.Clear();
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("15_reward_club", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ClubItemCollection _data = JsonReader.Deserialize<ClubItemCollection>(data);
		foreach(ClubItem _item  in _data.Data){
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
