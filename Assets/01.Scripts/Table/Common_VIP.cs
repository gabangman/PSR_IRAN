using UnityEngine;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;
public class Common_VIP : MonoBehaviour {
	
	[System.Serializable]
	public class Item
	{
		public string ID;
		public int V_level;
		public int V_point;
		public string V_Text;
		public int V_add_regular;
		public int V_add_pvp;
		public int V_add_day;
		public int V_add_silver;
		public int V_add_gold;
		public int V_add_upgrade;
		public int V_add_battery;
	}
	
	public Dictionary<string, Item> dictionary = new Dictionary<string, Item>();
	[SerializeField] 
	Item[] array;
	
	static Common_VIP instance;
	
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
	public static int getCount(){
		return instance.dictionary.Count;
	}

	public class ItemCollection{
		public List<Item> Data = new List<Item>();
	}
	
	public void SetDataFile(string data)
	{	//dictionary.Clear();
		if(dictionary.Count !=0) return;
		TextAsset 	tx =(TextAsset)AccountManager.instance.txtbundle.Load("01_exp_vip", typeof(TextAsset));
		data = tx.text;
		data = "{ \"Data\" : "+data +"}";
		ItemCollection _data = JsonReader.Deserialize<ItemCollection>(data);
		foreach(Item _item  in _data.Data){
			dictionary.Add(_item.ID, _item);
			
		}
		foreach (KeyValuePair<string, Item> pair in dictionary)
		{
			Item _item = pair.Value;
			_item.V_Text = KoStorage.GetKorString(_item.V_Text);
		}

		SetVIPLevelConvert();

		#if UNITY_EDITOR
		ParsingToArray();
		#endif  
	}
	
	public  static void SetVIPLevelConvert(){
		int gExp = GV.vipExp;
		if(gExp == 0 ) {
			GV.gVIP = 0;
			GV.gVIPExp = 0;
			return;
		}
		
		
		for(int  i =0; i < 9; i++){
			int id = 1900+i;
			int clv = Get(id).V_point;
			id +=1;
			int nlv = Get(id).V_point;
			
			if(clv <= gExp && nlv > gExp){
				GV.gVIP = Get(id-1).V_level;
				int a = nlv - clv ;
				int b = gExp - clv;
				GV.gVIPExp = (float)b / (float)a;
			//	Utility.LogWarning("SetVIPLevelConvert : " + GV.gVIP + " exp0 : " + GV.vipExp + " id : " + (id-1));
				return;
			}else if(gExp == nlv){
				GV.gVIP = Get(id).V_level;
			//	Utility.LogWarning("SetVIPLevelConvert : " + GV.gVIP + " exp1 : " + GV.vipExp + " id : " + id);
				return;
			}
			
		}
		
		if(gExp >= Common_VIP.Get(1909).V_point){
			GV.gVIP = 10;
			GV.gVIPExp = 1.0f;
			return;
		}
		return;

		for(int i = 0; i < 10; i++){
			int id = 1900+i;
			Item cItem = Get(id);
			if(i == 0){
				if(gExp < cItem.V_point){
					GV.gVIP = 0;
					GV.gVIPExp = 0;
					break;
				}else if(gExp == cItem.V_point){
					GV.gVIP = 1;
					GV.gVIPExp = 0;
					break;
				}
			}else if(i == 9){
				if(gExp >= cItem.V_point){
					GV.gVIP = 10;
					GV.gVIPExp=1.0f;
					break;
				}else{
					
					break;
				}
			}else{
				Item nItem = Get(id+1);
				
				if(gExp > cItem.V_point && gExp < nItem.V_point){
					GV.gVIP = cItem.V_level;
					int a = nItem.V_point - cItem.V_point ;
					int b = gExp - cItem.V_point;
					GV.gVIPExp = (float)b / (float)a;
				}else if(gExp == nItem.V_point){
					GV.gVIP = nItem.V_level;
					GV.gVIPExp = 0.0f;
				}else if(gExp == cItem.V_point){
					GV.gVIP = cItem.V_level;
					GV.gVIPExp = 0.0f;
					
				}
				break;			
			}
			
		}
	}

	void OnDestroy(){
		//instance = null;
	}
	void OnApplicationQuit(){
		instance = null;
	}
	
	
}
