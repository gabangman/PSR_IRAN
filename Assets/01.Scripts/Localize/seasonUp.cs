using UnityEngine;
using System.Collections;

public class seasonUp : MonoBehaviour {

	
	public UILabel[] lbText;
	void Start () {
	//	Utility.LogError("KO " + gameObject.name);
	//	return;
		lbText[0].text = KoStorage.GetKorString("71000");
		lbText[1].text = KoStorage.GetKorString("73147");
	//	int id = GV.getTeamCarID(GV.SelectedTeamID);
	//	string carClass =GV.getTeamCarClass(GV.SelectedTeamID);
	//	lbText[2].text = Common_Car_Status.Get(id).Name;
	//	lbText[3].text = string.Format(KoStorage.GetKorString("74024"), carClass);
	//	lbText[3].transform.parent.FindChild("icon_car").GetComponent<UISprite>().spriteName 
	//		= id.ToString();
		Destroy(this);
	}
}
