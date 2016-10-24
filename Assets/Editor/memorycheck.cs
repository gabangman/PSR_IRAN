using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
class memorycheck : MonoBehaviour {
	// Add menu named "Do Something" to the main menu

	static Dictionary<string, int> tempdicts = new Dictionary<string, int>();
	[MenuItem ("MyMenu/Print Item Names")]
	public static void PrintObjectName()
	{
		Object[] objects = FindObjectsOfType(typeof (UnityEngine.Object));
		
		//Dictionary<string, int> tempdicts1 = new Dictionary<string, int>();

		//이해가 안되네......ㅜㅜ



		foreach(Object obj in objects)
		{
			string key = obj.GetType().ToString();
			if(tempdicts.ContainsKey(key))
			{
				tempdicts[key]++;
			} 
			else
			{
				tempdicts[key] = 1;
			//	dictionary.Add(key,1);
				//tempdicts[key] = 1;
				//tempdicts.Add(key, 0);
				//tempdicts.Add(key,0);
			}
		}


////		foreach(string curobjects in dictionary.Keys)
		{
	//		if(!tempdicts1.ContainsKey(curobjects))
			{
	//			Debug.Log(curobjects);
			}
		}

		List<KeyValuePair<string, int>> myList = new List<KeyValuePair<string, int>>(tempdicts);
		myList.Sort(
			delegate(KeyValuePair<string, int> firstPair,
		         KeyValuePair<string, int> nextPair)
			{
			return nextPair.Value.CompareTo((firstPair.Value));
		}
		);
		
		foreach (KeyValuePair<string, int> entry in myList)
		{
			//GUILayout.Label(entry.Key + ": " + entry.Value);
			Debug.Log (entry.Key + ": " + entry.Value);
		}
		
	

	} 

}

/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class momory : MonoBehaviour
{
	void OnGUI()
	{
		Object[] objects = FindObjectsOfType(typeof (UnityEngine.Object));
		
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		
		foreach(Object obj in objects)
		{
			string key = obj.GetType().ToString();
			if(dictionary.ContainsKey(key))
			{
				dictionary[key]++;
			} 
			else
			{
				dictionary[key] = 1;
			}
		}
		
		List<KeyValuePair<string, int>> myList = new List<KeyValuePair<string, int>>(dictionary);
		myList.Sort(
			delegate(KeyValuePair<string, int> firstPair,
		         KeyValuePair<string, int> nextPair)
			{
			return nextPair.Value.CompareTo((firstPair.Value));
		}
		);
		
		foreach (KeyValuePair<string, int> entry in myList)
		{
			GUILayout.Label(entry.Key + ": " + entry.Value);
		}
		
	}
} 
 */
