using UnityEngine;
using System.Collections;

public class LocalClanSub : MonoBehaviour {

	public UILabel[] labels;
	void Start () {

	//	labels[0].text = KoStorage.GetKorString("77312"); // 
		labels[1].text = KoStorage.GetKorString("77225"); // 
		labels[2].text = KoStorage.GetKorString("77226"); // 
		labels[3].text = KoStorage.GetKorString("77227"); // 
		
		labels[4].text = KoStorage.GetKorString("73500"); //
		labels[5].text = KoStorage.GetKorString("77005"); // 
		
		
		Destroy(this);
	}
	

}
