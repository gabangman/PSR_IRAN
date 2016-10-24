using UnityEngine;
using System.Collections;

public class OpenLanguage : MonoBehaviour {
	public UILabel[] uination;
	private bool isPop;
	void OnClose(GameObject obj){
		obj.transform.parent.gameObject.SetActive(false);
		NGUITools.FindInParents<SubMenuWindow>(gameObject).OptionReturn();
		Global.isPopUp = false;
		UserDataManager.instance.OnSubBacksub = null;
		UserDataManager.instance.OnSubBacksubsub = null;
	}


	void Awake(){
		uination[0].text = KoStorage.GetKorString("72518");
		uination[1].text = KoStorage.GetKorString("72521");//"USA???";
		uination[2].text =KoStorage.GetKorString("72522");// "FRA???";
		uination[3].text =KoStorage.GetKorString("72523");// "RUS???";
		uination[4].text = KoStorage.GetKorString("72524");//"TUR??";
		uination[5].text =KoStorage.GetKorString("72525");// "KOR???";
		uination[6].text = KoStorage.GetKorString("72526");//"CHN??";
		uination[7].text =KoStorage.GetKorString("72527");// "JPN???";
		uination[8].text =KoStorage.GetKorString("72528");// "DEU???";
		uination[9].text =KoStorage.GetKorString("72529");// "ITA???";
		uination[10].text ="Indonesia";//KoStorage.GetKorString("72529");// "ITA???";
		isPop = false;
	}

	void OnEnable(){
		if(!isPop) return;
	}

	public void SetLanguage(){
		UserDataManager.instance.OnSubBacksub  = ()=>{
			OnClose(transform.FindChild("Close").gameObject);
		};
	}

	public void OnSetCoutry(GameObject obj){
		if(Global.gCountryCode.Equals(obj.name)) return;
		if(Global.isPopUp) return;
		Global.isPopUp = true;
		string str = obj.name;
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<ChangeLanguagePopUp>().InitPopUp();
		pop.GetComponent<ChangeLanguagePopUp>().onFinishCallback(()=>{
			Global.isPopUp = false;
			EncryptedPlayerPrefs.SetString("CountryCode",str);
			Global.gCountryCode = str;
		});
	}


}
