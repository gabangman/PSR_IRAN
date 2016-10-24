using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class EnStorage : MonoBehaviour
{
	[System.Serializable]
	public class Item
	{
		public string ID;
		public string String;
	}
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	[SerializeField]
	Item[] array;
	
	public void SetDataFile(string data)
	{
		dictionary = JsonReader.Deserialize<Dictionary<string, Item>>(data);
		
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
}