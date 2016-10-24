using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class Common_Car_Status : MonoBehaviour {

		[System.Serializable]
		public class Item
		{
		public string ID;
		public string Name;
		public string Text;
		public int Model;
		public int Class_ID;
		public string Class;
		public int ReqLV;


		public int Power;
		public int Weight;
		public int Grip_1t;
		public float Grip;
		public int Gbox;
		public int Brake_1t;
		public float Brake;
		public int Durability;
		public int BsPower;
		public int BsTime_1t;
		public float BsTime;

		public int Gear1_1t;
		public int Gear2_1t;
		public int Gear3_1t;
		public int Gear4_1t;
		public int Gear5_1t;
		public int Gear6_1t;
		public int Gear7_1t;
		public int Gear8_1t;
		public int Gear9_1t;
		public float Gear1;
		public float Gear2;
		public float Gear3;
		public float Gear4;
		public float Gear5;
		public float Gear6;
		public float Gear7;
		public float Gear8;
		public float Gear9;
		public int LED2_1t;
		public int LED3_1t;
		public int LED4_1t;
		public int LED5_1t;
		public int LED6_1t;
		public int LED7_1t;
		public int LED8_1t;
		public int LED9_1t;
		public int LED10_1t;

		public float LED2;
		public float LED3;
		public float LED4;
		public float LED5;
		public float LED6;
		public float LED7;
		public float LED8;
		public float LED9;
		public float LED10;

		public int RPM;
		public int RPMMax;
		public int RPM_Boost;
		public int GearLmt;
		public int Res;
		public int BuyPrice;
		public int Sellprice;
		public int Coupon_N;
		public int Spon;

		//public int UpLimit;
		//public int StarLV;
		//public int ReqLV;
		public int BodyAble;
		public int EngineAble;
		public int TireAble;
		public int GBoxAble;
		public int IntakeAble;
		public int BsPowerAble;
		public int BsTimeAble;

		public int Body_star_ID;
		public int Engine_star_ID;
		public int Tire_star_ID;
		public int Gbox_star_ID;
		public int Intake_star_ID;
		public int BsPower_star_ID;
		public int BsTime_star_ID;
	
	}
		
		public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
		[SerializeField] 
		Item[] array;
		
		static Common_Car_Status instance;
		public static List<int> wholeCarList= new List<int>();
		public static List<int> SpecialCarList= new List<int>();
		public static List<int> stockCarList = new List<int>();
		public static List<int> tourCarList = new List<int>();
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
		
	

	public class ItemCollection{
		public List<Item> Data = new List<Item>();
	}

	public void SetDataFile(string data)
	{

		TextAsset  tx =(TextAsset)AccountManager.instance.txtbundle.Load("03_10_car", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+ data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		dictionary.Clear();
		wholeCarList.Clear();
		SpecialCarList.Clear();
		tourCarList.Clear();
		stockCarList.Clear();
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
			wholeCarList.Add(int.Parse(_item.ID));
			if(_item.Model == 3){
				SpecialCarList.Add(int.Parse(_item.ID));
			}else if(_item.Model == 1){
				stockCarList.Add(int.Parse(_item.ID));
			}else if(_item.Model == 2){
				tourCarList.Add(int.Parse(_item.ID));
			}
		}


		foreach (KeyValuePair<string, Item> pair in dictionary)
		{
			Item _item = pair.Value;
			_item.Name = KoStorage.GetKorString(_item.Name);
			_item.Text =  KoStorage.GetKorString(_item.Text);

			_item.Gear1 = _item.Gear1_1t * 0.001f;
			_item.Gear2 = _item.Gear2_1t * 0.001f;
			_item.Gear3 = _item.Gear3_1t * 0.001f;
			_item.Gear4 = _item.Gear4_1t * 0.001f;
			_item.Gear5 = _item.Gear5_1t * 0.001f;
			_item.Gear6 = _item.Gear6_1t * 0.001f;
			_item.Gear7 = _item.Gear7_1t * 0.001f;
			_item.Gear8 = _item.Gear8_1t * 0.001f;
			_item.Gear9 = _item.Gear9_1t * 0.001f;
			
			_item.LED2 = _item.LED2_1t * 0.001f;
			_item.LED3 = _item.LED3_1t * 0.001f;
			_item.LED4 = _item.LED4_1t * 0.001f;
			_item.LED5 = _item.LED5_1t * 0.001f;
			_item.LED6 = _item.LED6_1t * 0.001f;
			_item.LED7 = _item.LED7_1t * 0.001f;
			_item.LED8 = _item.LED8_1t * 0.001f;
			_item.LED9 = _item.LED9_1t * 0.001f;
			_item.LED10 = _item.LED10_1t * 0.001f;
			_item.Grip = _item.Grip_1t * 0.001f;
			_item.Brake = _item.Brake_1t * 0.001f;
			_item.BsTime = _item.BsTime_1t * 0.001f;
		}
		//Utility.Log (dictionary.Count);
			FeaturedCarSelect();
			#if UNITY_EDITOR
			ParsingToArray();
			#endif  
		}
		

	void FeaturedCarSelect(){
		if(GV.bDayReset){
			GV.bDayReset = false;
			int carId = 1000;
			for(int i = 0; i < 100; i++){
				carId = (int)Well512.Next(0,24);
				carId+=1000;
				if( Get (carId).ReqLV <= GV.ChSeason){
					myAccount.instance.account.eRace.featuredCarID = carId;
					break;
				}
			}
		}
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
