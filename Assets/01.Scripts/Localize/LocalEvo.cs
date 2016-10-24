using UnityEngine;
using System.Collections;

public class LocalEvo : MonoBehaviour {

	public UILabel[] labels;
	void Start () {
		
		labels[0].text =KoStorage.GetKorString("76429"); //status
		labels[1].text = KoStorage.GetKorString("76408"); // description
		labels[2].text = KoStorage.GetKorString("76407"); // use Gold
		labels[3].text = KoStorage.GetKorString("76407"); // use Gold
		labels[4].text = KoStorage.GetKorString("76407"); // use Gold
		labels[5].text = KoStorage.GetKorString("76407"); // use Gold

		labels[6].text = KoStorage.GetKorString("76405"); // btn upgrade
		labels[7].text = KoStorage.GetKorString("76405"); // btn upgrade

		labels[8].text = KoStorage.GetKorString("76402");//"진화 창?";// KoStorage.GetKorString("77004"); // title
		labels[9].text = KoStorage.GetKorString("76404"); // tip

		
		labels[10].text = KoStorage.GetKorString("71000"); // 확인
		labels[11].text = KoStorage.GetKorString("76410"); // 진화시도 
		labels[12].text = KoStorage.GetKorString("76411"); // 성
	//	labels[13].text = KoStorage.GetKorString("76406");//"성광확률?";///KoStorage.GetKorString("76411"); // 성


		Destroy(this);
	}
}
