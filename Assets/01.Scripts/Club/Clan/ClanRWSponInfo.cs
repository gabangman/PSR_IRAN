using UnityEngine;
using System.Collections;

public class ClanRWSponInfo : MonoBehaviour {

	// Use this for initialization
	public UILabel[] labels;
	public UILabel[] lbNames;

	void Start () {
	
		lbNames[0].text  = Common_Sponsor_Status.Get(1301).Name;
		lbNames[1].text  = Common_Sponsor_Status.Get(1302).Name;
		lbNames[2].text  = Common_Sponsor_Status.Get(1303).Name;
		lbNames[3].text  = Common_Sponsor_Status.Get(1304).Name;
		lbNames[4].text  = Common_Sponsor_Status.Get(1305).Name;
		transform.FindChild("lbTitle").GetComponent<UILabel>().text = KoStorage.GetKorString("71113");
	
	}


	void SetInit(){
		UserDataManager.instance.OnSubBacksub = ()=>{
			OnClose();
		};
	}

	void OnClose(){
		transform.gameObject.SetActive(false);
		UserDataManager.instance.OnSubBacksub = null;
	}
}
