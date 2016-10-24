using UnityEngine;
using System.Collections;

public class ClanCreateContent : MonoBehaviour {

	public UILabel[] uiLabel;
	private int carindex,crewindex,typeindex,clanindex;
	private int carmax=20, crewmax=5, typemax=1,clanmax=30;
	public UILabel[] testLabel;
	private UISprite clanSprite;
	public UILabel[] labels;
	public GameObject[] arrBTN; // R -> L, sym, type, ability
	private UIInput mInput;
	private readonly int nameMin = 2;
	private readonly int nameMax = 12;
	private readonly int descMin = 2;
	private readonly int descMax = 200;




	void Start(){
	//	testLabel[1].text = "AnyOne";
	//	testLabel[2].text = "0";
	
		labels[0].text = KoStorage.GetKorString("77100");
		labels[1].text = KoStorage.GetKorString("77102");
		labels[2].text = KoStorage.GetKorString("77103");
		labels[3].text = KoStorage.GetKorString("77104");
		labels[4].text = KoStorage.GetKorString("77107");

		//labels = null;
		if(arrBTN.Length == 0) {
			arrBTN = new GameObject[2];
			arrBTN[0] = transform.FindChild("ClanSymbol").FindChild("btn_Sym_R").gameObject;
			arrBTN[1] = transform.FindChild("ClanSymbol").FindChild("btn_Sym_L").gameObject;
			return;
		}
		arrBTN[1].SetActive(false);arrBTN[3].SetActive(false);arrBTN[5].SetActive(false);

	}

	public void modifyClan(){
		if(mInput == null){
			mInput = transform.FindChild("ClanDescription").GetComponentInChildren<UIInput>();
			mInput.isMultiLine = true;
		}
	
		labels[6].text = KoStorage.GetKorString("77107");
		labels[5].text =KoStorage.GetKorString("71000");
		labels[4].text =string.Format(KoStorage.GetKorString("77113"),CClub.mClubInfo.mClubName);
	//	clanSprite = testLabel[0].transform.parent.FindChild("icon_Symbol").GetComponent<UISprite>();
	//	uiLabel[0].text = CClub.mClubInfo.mClubName;
		uiLabel[1].text = CClub.mClubInfo.clubDescription;
	//	string[] str = CClub.mClubInfo.clubSymbol.Split('_');

	//	int.TryParse(str[1], out clanindex);
	//	clanSprite.spriteName = "Clubsymbol_"+clanindex.ToString();

		if(CClub.ClanMember == 1){
			transform.FindChild("Btn_Create").gameObject.SetActive(true);
			transform.FindChild("Btn_Create").GetComponent<UIButtonMessage>().functionName = "OnClanModify";

			testLabel[2].text = string.Format(KoStorage.GetKorString("72611"), ConverterByte(CClub.mClubInfo.clubDescription), descMax);
			mInput.transform.GetComponent<BoxCollider>().enabled = true;
		}else{
			transform.FindChild("Btn_Create").gameObject.SetActive(false);
			testLabel[2].text = string.Empty;
			mInput.transform.GetComponent<BoxCollider>().enabled = false;
		}
		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			NGUITools.FindInParents<ClanPopup>(gameObject).OnClose();
		};
	}

	

	public void createClubInit(){
		if(mInput == null){
			mInput = transform.FindChild("ClanDescription").GetComponentInChildren<UIInput>();
			mInput.isMultiLine = true;
		}

		labels[5].text = KoStorage.GetKorString("77108");
		labels[6].text = KoStorage.GetKorString("77108");
		clanSprite = testLabel[0].transform.parent.FindChild("icon_Symbol").GetComponent<UISprite>();
		clanSprite.spriteName = "Clubsymbol_1";
		carindex = 0;
		clanindex = 1;
		uiLabel[0].text = KoStorage.GetKorString("77101");
		uiLabel[1].text = KoStorage.GetKorString("77004");
		testLabel[1].text = string.Format(KoStorage.GetKorString("72612"), 0, nameMax);

	
		testLabel[2].text = string.Format(KoStorage.GetKorString("72612"), 0, descMax);
		nameLengh = 0;
		descLengh = 0;
		transform.FindChild("Btn_Create").FindChild("On").gameObject.SetActive(false);
		transform.FindChild("Btn_Create").FindChild("Off").gameObject.SetActive(true);
		transform.FindChild("Btn_Create").GetComponent<UIButtonMessage>().functionName = string.Empty;

		Global.bLobbyBack = true;
		UserDataManager.instance.OnSubBack = ()=>{
			NGUITools.FindInParents<ClanPopup>(gameObject).OnClose();
		};

	}


	private int nameLengh, descLengh;
	void OnSubmitClanName(string str)
	{
		int strByte = ConverterByte(str);
		if(strByte >= 2 && strByte <= nameMax ){
			testLabel[1].text = string.Format(KoStorage.GetKorString("72611"),strByte, nameMax);
			nameLengh = strByte;
			BtnCreateActive(1);
		}else{
			testLabel[1].text = string.Format(KoStorage.GetKorString("72612"), strByte, nameMax);
			nameLengh =strByte;
			BtnCreateActive(0);
		}
	}
	
	void OnSubmitText(string str)
	{
		//Utility.Log(str);
		int strByte = ConverterByte(str);
		if(strByte >= 2 && strByte <= descMax){
			testLabel[2].text = string.Format(KoStorage.GetKorString("72611"), strByte, descMax);
			descLengh = strByte;
			BtnCreateActive(2);
		}else{
			testLabel[2].text = string.Format(KoStorage.GetKorString("72612"),strByte, descMax);
			descLengh = strByte;
			BtnCreateActive(0);
		}
	}


	void OnModifyDesc(string str){
		int strByte = ConverterByte(str);
	//	testLabel[2].text  = string.Format("{0} Byte", str.Length);
		if(strByte >= 2 && strByte <= descMax){
			testLabel[2].text = string.Format(KoStorage.GetKorString("72611"), strByte, descMax);
			BtnModifyActive(1);
		}else{
			testLabel[2].text = string.Format(KoStorage.GetKorString("72612"),strByte, descMax);
			BtnModifyActive(0);
		}

	}
	void BtnModifyActive(int idx){
		if(idx == 0){
			transform.FindChild("Btn_Create").FindChild("BG_Off").gameObject.SetActive(true);
			transform.FindChild("Btn_Create").GetComponent<UIButtonMessage>().functionName = string.Empty;
		}else if(idx == 1){
			transform.FindChild("Btn_Create").FindChild("BG_Off").gameObject.SetActive(false);
			transform.FindChild("Btn_Create").GetComponent<UIButtonMessage>().functionName ="OnClanModify";
		
		}
	}
	public void BtnCreateActive(int idx){
		if(idx == 0) return;

		if(idx == 1){
			if(descLengh >=2 && descLengh <=descMax) {
				idx =100;
			}
		
		}else if(idx == 2){
			if(nameLengh >=2 && nameLengh <=nameMax) {
				idx =100;
			}
		}

		if(idx == 100){
			transform.FindChild("Btn_Create").FindChild("On").gameObject.SetActive(true);
			transform.FindChild("Btn_Create").FindChild("Off").gameObject.SetActive(false);
			transform.FindChild("Btn_Create").GetComponent<UIButtonMessage>().functionName = "OnClanCreate";
		}
	
	}


	int ConverterByte(string str){
		return str.Length;
	//	byte []bArray_ = System.Text.Encoding.Default.GetBytes(str);
	//	byte []u8Array_ = System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, bArray_);
	//	return u8Array_.Length;
	}
	void OnClanModify(){
		
		string[] strCheck = new string[6];
		strCheck[1] = uiLabel[0].text; // name;
		strCheck[2] = uiLabel[1].text; //description
		strCheck[3] = "1";
		NGUITools.FindInParents<ClanWindow>(gameObject).ModifyClanPopup(strCheck[2]);
	}

	void OnBtnClick(GameObject clickedbtn){
		string[] btn = clickedbtn.name.Split('_');
		switch(btn[1]){
		case "Sym":{
			if(btn[2].Equals("R")){
				if(clanindex < clanmax){
					clanindex++;
					if(clanindex == clanmax) arrBTN[0].SetActive(false);
					if(!arrBTN[1].activeSelf) arrBTN[1].SetActive(true);

				}else {
					arrBTN[0].SetActive(false);
					return;
				}
			}else{
				if(clanindex > 1){
					clanindex--;
					if(clanindex == 1) arrBTN[1].SetActive(false);
					if(!arrBTN[0].activeSelf) arrBTN[0].SetActive(true);
				}else {
					arrBTN[1].SetActive(false);
					return;
				}
			}
			clanSprite.spriteName = "Clubsymbol_"+clanindex.ToString();
		}break;
		
	}
	
	}

	void OnClanCreate(){
	//	uiLabel[0].text = KoStorage.GetKorString("77101");
	//	uiLabel[1].text = KoStorage.GetKorString("77004");
		string[] strCheck = new string[6];
		if(string.IsNullOrEmpty(uiLabel[0].text))
			strCheck[0] = "No";
		else strCheck[0] = "Yes";
		if(string.Equals(uiLabel[0].text, KoStorage.GetKorString("77101")) == true){
			strCheck[0] = "No";
		}else strCheck[0] = "Yes";


		if(string.Equals(strCheck[0],"Yes") == true){
			if(string.IsNullOrEmpty(uiLabel[1].text))
				strCheck[0] = "No1";
			else strCheck[0] = "Yes";

			if(string.Equals(uiLabel[1].text, KoStorage.GetKorString("77004")) == true){
				strCheck[0] = "No1";
			}else strCheck[0] = "Yes";
		}
		if(strCheck[0].Equals("Yes")){
			strCheck[1] = uiLabel[0].text; // name;
			strCheck[2] = uiLabel[1].text; //description
			strCheck[3] = clanindex.ToString();
		//	strCheck[4] = (carindex*50).ToString();
		//	strCheck[5] = typeindex.ToString();
		}
		transform.parent.gameObject.SendMessage("OnClanCreatePopup" , strCheck);

	}

	void OnEnable(){
	//	uiLabel[0].text = "you can type here";
	//	uiLabel[1].text = "you can type here";
		//carindex = 0;crewindex=0;typeindex=0;clanindex=0;
	//	testLabel[0].text = carindex.ToString();
	//	testLabel[1].text = crewindex.ToString();
	//	testLabel[2].text = typeindex.ToString();
	//	testLabel[3].text = clanindex.ToString();
	}
}
