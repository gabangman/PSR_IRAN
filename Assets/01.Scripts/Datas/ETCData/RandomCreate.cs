using UnityEngine;
using System.Collections;

public class RandomCreate : MonoBehaviour {

	public int[] CreateRandomValue(int count){
	
		int[] arr = new int[count];
		//int[] _arr = new int[count];
		for(int i = 0; i < arr.Length; i++){
			arr[i] = i;
		}

		for(int i = 0; i < arr.Length; i++){
			int n = Random.Range(0, count);
			int temp = arr[i];
			arr[i] = arr[n];
			arr[n] = temp;
		}
	//	foreach(int j in arr){
	//		Utility.Log("test random " + j);
	//	} 5개 하면 0, 1, 2, 3, 4 가 나온다.
		Destroy(this, 0.2f);
		return arr;
	}

	

}
