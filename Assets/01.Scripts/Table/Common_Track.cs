using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class Common_Track : MonoBehaviour {
	
	[System.Serializable]
	public class Item
	{
		public string ID;
		public string Name;
		public string Text;
		public int Season;
		public string Scene;
		public int Dura_dec;
		public int Distance;
		public int Distance_big;
		public float R1_Time;
		public float R2_Time;
		public int ID1;
		public string Code;
		public string NameKr;
		public int TrackLength;
		public int PitCount;
		public int ConsumeDurability;
		public System.DateTime TrackUpdate;
	}


	public static void SetXMLTest(System.Data.DataSet mSet){
		System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
		xdoc.LoadXml(mSet.GetXml());
		System.Xml.XmlNode node = xdoc.DocumentElement;
		if(node.HasChildNodes){
			System.Xml.XmlNodeList mChild = node.ChildNodes;
			Item _item = new Item();
			for(int j = 0; j < mChild.Count;j++){
				System.Xml.XmlNodeList mChildList = mChild[j].ChildNodes;
					_item = new Item();
					_item.ID1 = int.Parse(mChild[j].SelectSingleNode("ID").InnerText);
					_item.Code = mChild[j].SelectSingleNode("Code").InnerText;
					_item.NameKr = mChild[j].SelectSingleNode("NameKr").InnerText;
					_item.TrackLength = int.Parse(mChild[j].SelectSingleNode("TrackLength").InnerText);
					_item.PitCount =int.Parse( mChild[j].SelectSingleNode("PitCount").InnerText);
					_item.ConsumeDurability =int.Parse( mChild[j].SelectSingleNode("ConsumeDurability").InnerText);
					_item.TrackUpdate =  System.Convert.ToDateTime(mChild[j].SelectSingleNode("TrackUpdate").InnerText);
			}
		}else Utility.LogWarning("childnodes no");

	}
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	[SerializeField] 
	Item[] array;
	
	static Common_Track instance;
	
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
	
	public void SetDataFileEx(string data)
	{
		dictionary = JsonReader.Deserialize<Dictionary<string, Item>>(data);

		#if UNITY_EDITOR
		ParsingToArray();
		#endif  
	}
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
	{  	//dictionary.Clear();
		if(dictionary.Count !=0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("07_track", typeof(TextAsset));
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
			item.Text =  KoStorage.GetKorString(item.Text);
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
