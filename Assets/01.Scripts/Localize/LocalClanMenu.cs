using UnityEngine;
using System.Collections;

public class LocalClanMenu : MonoBehaviour {
	public UILabel[] labels;

	// Use this for initialization
	void Start () {
	
		labels[0].text = KoStorage.GetKorString("77000"); // myclan
		labels[1].text = KoStorage.GetKorString("77001"); // rank
		labels[2].text = KoStorage.GetKorString("77002"); // hisotry
		labels[3].text = KoStorage.GetKorString("77003"); // search

		labels[4].text = KoStorage.GetKorString("77301"); // global
		labels[5].text = KoStorage.GetKorString("77300"); // local

		labels[6].text = string.Format("{0:#,0}", CClub.ClubDollar);
		labels[7].text = string.Format("{0:#,0}", CClub.ClubDollar);
		Destroy(this);
	}
	

}
