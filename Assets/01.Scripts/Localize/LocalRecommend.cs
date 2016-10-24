using UnityEngine;
using System.Collections;

public class LocalRecommend : MonoBehaviour {

	public UILabel[] labels;
	void Start(){
		labels[0].text =KoStorage.GetKorString("74035");
	}

 	protected void ChangeTextLabel(int idx){
		if(idx == 0){ // 구매 했다. 
			labels[1].text =KoStorage.GetKorString("74012"); //change
		}else{ // 추천 차량 없다. 
			labels[1].text =KoStorage.GetKorString("74034");
		}
	}
}
