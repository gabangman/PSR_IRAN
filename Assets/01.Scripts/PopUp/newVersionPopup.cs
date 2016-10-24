using UnityEngine;
using System.Collections;

public class newVersionPopup : MonoBehaviour {
	private GameObject obj;
	private string fName;
	void Start(){
		string[] eText = new string[3];
		if(KoStorage.kostroage == null) {
			//	eText[0] = "새로운 버전";
			//	eText[1] ="확인";
			//	eText[2] = string.Format("새로운 버전이 업데이트되었습니다. \n 최신버젼으로 업데이트하시길 바랍니다.");
			
		}else{
			eText[0] =KoStorage.GetKorString("71202");
			eText[1] = KoStorage.GetKorString("71000");
			eText[2] = KoStorage.GetKorString("71117");
		}
		
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text =  eText[0];
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =string.Empty;
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("btnok").FindChild("lbOk").gameObject.SetActive(true);
		pop.FindChild("btnok").FindChild("lbOk").GetComponent<UILabel>().text =eText[1];
		pop.FindChild("lbName").GetComponent<UILabel>().text = eText[2];
		//		pop.FindChild("icon_product").gameObject.SetActive(false);
		//pop.FindChild("Sprite (Check_V)").gameObject.SetActive(true);
		var tr = pop.FindChild("Sprite (Check_V)") as Transform;
		tr.gameObject.SetActive(true);
		obj = tr.GetComponent<UIButtonMessage>().target;
		fName = tr.GetComponent<UIButtonMessage>().functionName;


	tr.GetComponent<UIButtonMessage>().target = gameObject;
		tr.GetComponent<UIButtonMessage>().functionName = "OnClose";
		//pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("btnok").GetComponent<UIButtonMessage>().target = gameObject;
		if(Application.platform == RuntimePlatform.IPhonePlayer)
			pop.FindChild("btnok").GetComponent<UIButtonMessage>().functionName = "OnGameCenter";
		else pop.FindChild("btnok").GetComponent<UIButtonMessage>().functionName = "OnGooglePlay";
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			OnClose();

		};
	}
	
	void OnGooglePlay(){
		Application.OpenURL(GV.gInfo.androidMarketURL);
		Application.Quit();
	}
	
	void OnGameCenter(){
		Application.OpenURL(GV.gInfo.IosURL);
		Application.Quit();
	}

	void OnClose(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		var tr = pop.FindChild("Sprite (Check_V)") as Transform;
		tr.gameObject.SetActive(false);
		tr.GetComponent<UIButtonMessage>().target = obj;
		tr.GetComponent<UIButtonMessage>().functionName = fName;
		Global.bLobbyBack = false;
		Global.isNetwork = false;
		transform.gameObject.SetActive(false);
		Destroy(this);


	}
	
}
