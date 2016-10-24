using UnityEngine;
using UnityEditor;
public class makeScirptObject : MonoBehaviour {
	[MenuItem("MyMenu/Create CustomData")]
	static void CreateCustomData()
	{
		ItemCreate newitem = ScriptableObject.CreateInstance<ItemCreate>();
		AssetDatabase.CreateAsset(newitem, "Assets/Item.asset");
	}

}



