using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;
public class Common_Class : MonoBehaviour {

	[System.Serializable]
	public class Item
	{
		public string ID;
		public int Model;
		public string Class;
		public int Class_x;
		public int Class_power;
		public int Class_weight;
		public int Class_grip_1t;
		public float Class_grip;
		public int Class_gear;
		public int Class_bspower;
		public int Class_bstime_1t;
		public float Class_bstime;
		public int UpLimit;
		public int StarLV;
		public int Durability;
		public int Brake_1t;
		public float Brake;
		public int Repair;
		public int Sellprice;
		public int Dealer_CoinPlus;

		public int Gear1_1t;
		public float Gear1;

		public int Gear2_1t;
		public float Gear2;

		public int Gear3_1t;
		public float Gear3;

		public int Gear4_1t;
		public float Gear4;

		public int Gear5_1t;
		public float Gear5;

		public int Gear6_1t;
		public float Gear6;

		public int Gear7_1t;
		public float Gear7;

		public int Gear8_1t;
		public float Gear8;

		public int Gear9_1t;
		public float Gear9;

		public int RPM;
		public int RPMMax;
		public int RPM_Boost;
		public int GearLmt;

		public float R_Perfect;
		public float R_Good;
		public float R_Late;
		public float R_Early;

		public string P_Control;
		public int  P_MIN_1t;
		public int P_MAX_1t;
		public float p_min;
		public float p_max;
	}
	
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	
	static Common_Class instance;
	
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
	
	
	public class ItemCollection{
		public List<Item> Data = new List<Item>();
	}
	
	public void SetDataFile(string data)
	{
		//dictionary.Clear();
		if(dictionary.Count !=0) return;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("03_car_class", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
		}
		foreach (KeyValuePair<string, Item> pair in dictionary)
		{
			Item _item = pair.Value;
			_item.Brake = _item.Brake_1t * 0.001f;//KoStorage.getStringDic(_item.Name);
			_item.Class_grip = _item.Class_grip_1t * 0.001f;
			_item.Gear1 = _item.Gear1_1t * 0.001f;
			_item.Gear2 = _item.Gear2_1t * 0.001f;
			_item.Gear3 = _item.Gear3_1t * 0.001f;
			_item.Gear4 = _item.Gear4_1t * 0.001f;
			_item.Gear5 = _item.Gear5_1t * 0.001f;
			_item.Gear6 = _item.Gear6_1t * 0.001f;
			_item.Gear7 = _item.Gear7_1t * 0.001f;
			_item.Gear8 = _item.Gear8_1t * 0.001f;
			_item.Gear9 = _item.Gear9_1t * 0.001f;
			_item.Class_bstime = _item.Class_bstime_1t * 0.001f;
			_item.p_max = _item.P_MAX_1t * 0.001f;
			_item.p_min = _item.P_MIN_1t * 0.001f;
			_item.P_Control = KoStorage.GetKorString(_item.P_Control);
		}
	}


	
	void OnDestroy(){
		//instance = null;
	}
	void OnApplicationQuit(){
		instance = null;
	}
}
