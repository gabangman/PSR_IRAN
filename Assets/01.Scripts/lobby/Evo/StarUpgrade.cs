using UnityEngine;
using System.Collections;

public class StarUpgrade : MonoBehaviour {
	
	public delegate void OnFinish();
	public OnFinish onFinish;
	public GameObject evo, evoconfirm, effect,parent;
	int[] matCount ;
	int[] matID;
	int lackofMat;
	int lackofCoin;
	public GameObject Gage, btnYes, btnNo, myPart, VipObj;
	public GameObject[] matDecks;
	private int MAXMAT = 10;
	
	bool bUseCoin = false;
	int mMax;
	int myMat;
	int mCarID;
	int mPartID;
	string StarIconName;
	string PartNames;
	
	private void SetVIPLevel(){
		if(GV.gVIP == 0){
			VipObj.SetActive(false);
			plusSuccess= 0;
		}else{
			int vipID = 1900+(GV.gVIP-1);
			Common_VIP.Item vItem = Common_VIP.Get(vipID);
			VipObj.SetActive(true);
			VipObj.transform.FindChild("lbName").GetComponent<UILabel>().text = "VIP";
			VipObj.transform.FindChild("lbAdd").GetComponent<UILabel>().text =
				string.Format("+{0}%", vItem.V_add_upgrade);
			plusSuccess = vItem.V_add_upgrade;
		}
	}
	public void starCarWindowInfo(int carid, int partID){
		Global.isPopUp = true;
		evo.SetActive(true);evoconfirm.SetActive(false);
		SetVIPLevel();
		lackofMat = 0;
		lackofCoin = 0;
		bUseCoin = false;QubeCount = 0;
		GageCount = 0;
		CarInfo car = GV.getTeamCarInfo(GV.SelectedTeamID);
		Common_Car_Status.Item item = Common_Car_Status.Get(carid);
		int upID = 0, starlv = 0;//Common_Car_Status.Get(carid);
		string icon_name = string.Empty;
		switch(partID){
		case 5000: upID = item.Body_star_ID; starlv = car.bodyStar;icon_name = "Upgrade_icon_Body";PartNames = "76309";break;
		case 5001: upID = item.Engine_star_ID;starlv = car.engineStar;icon_name = "Upgrade_icon_Engine";PartNames = "76310";break;
		case 5002: upID = item.Tire_star_ID;starlv = car.tireStar;icon_name = "Upgrade_icon_Tire";PartNames = "76311";break;
		case 5003: upID = item.Gbox_star_ID;starlv = car.gearBoxStar;icon_name = "Upgrade_icon_Gearbox"; PartNames = "76018";break;
		case 5004: upID = item.Intake_star_ID;starlv = car.intakeStar;icon_name = "Upgrade_icon_Intake";PartNames = "76313";break;
		case 5005: upID = item.BsPower_star_ID;starlv = car.bsPowerStar;icon_name = "Upgrade_icon_Nitro";PartNames = "76019";break; 
		case 5006: upID = item.BsTime_star_ID;starlv = car.bsTimeStar;icon_name = "Upgrade_icon_NitroTime";PartNames = "76315";break;
		default: Utility.LogWarning("partID Null "); 
			break;
		}
		
		ChangeStarCount(car.carClass.StarLV, starlv, icon_name, myPart);
		Common_Mix.Item mItem = Common_Mix.Get(upID);
		MAXMAT = mItem.Item_no;
		StarIconName = icon_name;
		this.mCarID = carid;
		this.mPartID = partID;
		matCount = new int[]{GV.getMatCount(mItem.Item1),GV.getMatCount(mItem.Item2),GV.getMatCount(mItem.Item3),GV.getMatCount(8620)}; // 내 소유량
		//	matCount = new int[]{GV.getMatCount(mItem.Item1),GV.getMatCount(mItem.Item2),GV.getMatCount(mItem.Item3),10}; // 내 소유량
		
		matID = new int[]{mItem.Item1, mItem.Item2, mItem.Item3};
		DeckMatCheckCount();
	}
	void ChangeStarCount(int max ,int starLv, string sName, GameObject parent){
		int Max = max; int myLv = starLv;
		var tr = parent.transform.FindChild("StarCount") as Transform;
		for(int i = 0; i < tr.childCount; i++){
			tr.GetChild(i).gameObject.SetActive(false);
		}
		
		for(int i = 0; i < Max; i++){
			var temp = tr.GetChild(i) as Transform;
			temp.gameObject.SetActive(true);
			if(i <=(myLv) ){
				temp.FindChild("Star_on").gameObject.SetActive(true);
			}else{
				temp.FindChild("Star_on").gameObject.SetActive(false);
			}
		}
		
		myPart.transform.FindChild("icon").GetComponent<UISprite>().spriteName = sName;
		myPart.transform.FindChild("icon").GetComponent<UISprite>().MakePixelPerfect();
	}
	
	
	public void starCrewWindowInfo(int partID){
		return;
		
	}
	
	void DeckMatCheckCount(){
		bool b = false;
		for(int i =0; i <3; i++){
			matDecks[i].name = "MatDeck_"+i;
			if(MAXMAT <= matCount[i]){
				matDecks[i].transform.FindChild("lbAmount").GetComponent<UILabel>().text = 
					string.Format("{0}/{1}", MAXMAT, MAXMAT);
				GageCount++;
			}else{
				matDecks[i].transform.FindChild("lbAmount").GetComponent<UILabel>().text = 
					string.Format("[ff0000]{0}[-]/{1}", matCount[i], MAXMAT);
				b = true;
				lackofMat++;
			}
			matDecks[i].GetComponent<UpgradeMatDetail>().InitMatItemContent(matCount[i], MAXMAT, matID[i],  Common_Material.Get(matID[i]).Coin_change);
		}
		int QubeID = 8620;int QubeCoin = Common_Material.Get(8620).Coin_change;
		matDecks[3].GetComponent<UpgradeMatDetail>().InitQubeItemContent(matCount[3], QubeID, QubeCoin, plusSuccess);
		if(b){
			btnYes.SetActive(false);
			btnNo.SetActive(true);
		}else{
			btnYes.SetActive(true);
			btnNo.SetActive(false);
		}
		GageBarControl(GageCount);
	}
	
	private int GageCount;
	private int plusSuccess;
	private int QubeCount;
	void GageBarControl(int count){
		var tr = Gage.transform.FindChild("GageUp") as Transform;
		Vector3 scale = Gage.transform.FindChild("GageDown").localScale;
		float posXUp = 0;
		count = count *10;
		Vector3 upScale = Vector3.zero;
		if(count == 0){
			count += plusSuccess;
			if(count != 0)	 {
				upScale = new Vector3(scale.x * 0.01f * count, scale.y, scale.z);
				tr.GetComponent<UISprite>().alpha = 1.0f;
			}
			else  {
				upScale = new Vector3(scale.x * 0.00000001f, scale.y, scale.z);
				tr.GetComponent<UISprite>().alpha = 0.0f;
			}
		}else{
			tr.GetComponent<UISprite>().alpha = 1.0f;
			count += plusSuccess;
			if(count > 100) count = 100;
			upScale = new Vector3(scale.x * 0.01f * count, scale.y, scale.z);
		}
		tr.localScale = upScale;
		Gage.GetComponentInChildren<UILabel>().text = string.Format(KoStorage.GetKorString("76406"), count * 1);// string.Format("SUCCESS PROBABILITY : {0}%", count * 10);
	}
	
	
	public void DeckMatFullCount(GameObject obj){
		for(int i =0; i < 3; i++){
			if(obj == matDecks[i]){
				matDecks[i].transform.FindChild("lbAmount").GetComponent<UILabel>().text = 
					string.Format("{0}/{1}", MAXMAT, MAXMAT);
				GageCount++;
			}
		}
		GageBarControl(GageCount);
	}
	
	public void DeckMatFullCountMinus(GameObject obj){
		for(int i =0; i < 3; i++){
			if(obj == matDecks[i]){
				matDecks[i].transform.FindChild("lbAmount").GetComponent<UILabel>().text = 
					string.Format("[ff0000]{0}[-]/{1}", matCount[i], MAXMAT);
				GageCount--;
			}
		}
		GageBarControl(GageCount);
	}
	
	public void ClickMatCoin(int cnt){
		lackofMat--;
		lackofCoin += cnt;
		if(lackofMat == 0){
			bUseCoin = true;
			btnYes.SetActive(true);
			btnNo.SetActive(false);
		}
	}
	public void ClickMatCoinMinus(int cnt){
		//	Utility.Log("lackofMat " + lackofMat);
		lackofMat++;
		lackofCoin -= cnt;
		//	Utility.Log("cont " + lackofCoin);
		if(lackofMat != 0){
			bUseCoin = false;
			btnYes.SetActive(false);
			btnNo.SetActive(true);
		}
	}
	
	public void ClickQubeCoin(int Qcoin, int QCnt){
		Utility.LogWarning(lackofCoin + " == " + Qcoin);
		lackofCoin += Qcoin;
		QubeCount += QCnt;
		GageCount += QCnt;
		Utility.LogWarning("ClickQubeCoin " + QubeCount);
		GageBarControl(GageCount);
	}
	public void ClickQubeCoinMinus(int QCnt){
		Utility.LogWarning(lackofCoin + " == " + QCnt);
		//lackofCoin -= Qcoin;
		QubeCount -= QCnt;
		GageCount -= QCnt;
		Utility.LogWarning("ClickQubeCoinMinus " + QubeCount);
		GageBarControl(GageCount);
		int QubeID = 8620;int QubeCoin = Common_Material.Get(8620).Coin_change;
		matDecks[3].GetComponent<UpgradeMatDetail>().InitQubeItemContent(matCount[3], QubeID, QubeCoin, plusSuccess);
	}
	
	public void ClickPlusGage(int Qcoin, int QCnt){
		QubeCount ++;//= QCnt; 
		//	Utility.LogWarning("ClickPlusGage " + QubeCount);
		GageCount++;
		GageBarControl(GageCount);
	}
	
	public void ClickMinusGage(int Qcoin, int QCnt){
		QubeCount--;//-= QCnt;
		//	Utility.LogWarning("ClickMinusGage " + QubeCount);
		GageCount--;
		GageBarControl(GageCount);
	}
	
	private string makeMaterialIds(int QCnt){
		string[] strMat = new string[3];
		int cnt = 0;
		if(matCount[0] != 0){
			strMat[cnt] = matID[0].ToString()+":"+"1";
			cnt++;
		}
		
		if(matCount[1] != 0){
			strMat[cnt]= matID[1].ToString()+":"+"1";
			cnt++;
		}
		
		if(matCount[2] != 0){
			strMat[cnt]= matID[2].ToString()+":"+"1";
			cnt++;
		}
		string strMatIds = string.Empty;
		if(cnt == 0){
			if(QCnt != 0){
				strMatIds = "8620:"+QCnt.ToString();
				
			}else strMatIds = string.Empty;
			return strMatIds;
		}else{
			if(cnt == 1) strMatIds = strMat[0];
			else if(cnt == 2) strMatIds = strMat[0]+","+strMat[1];
			else if(cnt == 3) strMatIds = strMat[0]+","+strMat[1]+","+strMat[2];
		}
		
		if(QCnt != 0){
			strMatIds += ",8620:"+QCnt.ToString();
			
		}
		return strMatIds;
	}
	
	
	IEnumerator EvoStarUp(){
		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		bool bConnect = false;
		bool bSuccess = false, bError = false;
		string strMat = "materialIdx;"+ makeMaterialIds(QubeCount);
		mDic.Add("carIdx",GV.getTeamCarIndex(GV.SelectedTeamID));
		mDic.Add("part",(mPartID-4999));
		mDic.Add("materialCoin",lackofCoin);
		//mDic.Add("evoCube",QubeCount);
		mDic.Add("evoCoin",0);
		mDic.Add("vipExp",GV.vipExp);
		
		//	Utility.LogWarning("0 " + lackofCoin);
		//	Utility.LogWarning("1 " + strMat);
		string mAPI = ServerAPI.Get(90011);// "game/car/evolution"
		NetworkManager.instance.HttpFormConnect("Put", mDic,mAPI , (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				int state = thing["result"]["evolutionSucess"].AsInt;
				if(state == 1) bSuccess = true;
				else {
					
					bSuccess = false;
					
				}
				bError = false;
			}else{
				bError = true;
			}
			bConnect = true;
		},strMat);
		
		while(!bConnect){
			yield return null;
		}
		if(!bSuccess){
			if(bError){
				evo.SetActive(true); Global.isPopUp = false;
			}else{
				OnResponse(false);
			}
			yield break;
		}else{
			evoconfirm.SetActive(true);
			GameObject.Find("Audio").SendMessage("CompleteSound");
			
			var tr = evoconfirm.transform.FindChild("StarCount") as Transform;
			var ts = tr.GetComponent<TweenScale>() as TweenScale;
			ts.Reset();
			ts.enabled = true;
			
			tr = evoconfirm.transform.FindChild("BG");
			ts = tr.GetComponent<TweenScale>();
			ts.Reset();
			ts.enabled = true;
			
			tr = evoconfirm.transform.FindChild("bg_light");
			ts = tr.GetComponent<TweenScale>();
			ts.Reset();
			ts.enabled = true;
			
			tr = evoconfirm.transform.FindChild("btnok");
			var tp = tr.GetComponent<TweenPosition>() as TweenPosition;
			tp .Reset();
			tp .enabled = true;
			
			tr = evoconfirm.transform.FindChild("lbText");
			tp  = tr.GetComponent<TweenPosition>() as TweenPosition;
			tp .Reset();
			tp .enabled = true;
			
			tp = null; ts= null; tr =null;
			
			OnResponse(true);
		}
		
	}
	void OnUp(){
		if(UserDataManager.instance.buyPriceCheckCoin(lackofCoin)) return;
		evo.SetActive(false);
		//	Utility.LogWarning("Cube " + QubeCount);
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		pop.AddComponent<StarUpPopup>().InitPopUp(()=>{
			StartCoroutine("EvoStarUp");
		},
		()=>{
			evo.SetActive(true); Global.isPopUp = false;
		}, lackofCoin, PartNames);
	}
	
	
	
	void OnResponse(bool isSuccess){
		if(!isSuccess){
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			pop.AddComponent<StarUpFailPopup>().InitPopUp(()=>{
				evo.SetActive(true); Global.isPopUp = false;
				if(lackofCoin != 0){
					GV.myCoin = GV.myCoin-lackofCoin;
					GV.updateCoin = lackofCoin;
					var lobby1 = GameObject.Find("LobbyUI") as GameObject;
					lobby1.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
				}
				GV.UpdateMatCount(matID[0], -1);
				GV.UpdateMatCount(matID[1], -1);
				GV.UpdateMatCount(matID[2], -1);
				if(QubeCount != 0)
					GV.UpdateMatCount(8620, -QubeCount);
				
				starCarWindowInfo(mCarID, mPartID);
			});
			return;
		}
		CarInfo car = GV.getTeamCarInfo(GV.SelectedTeamID);
		Common_Car_Status.Item item = Common_Car_Status.Get(mCarID);
		int upID = 0, starlv = 0;//Common_Car_Status.Get(carid);
		string icon_name = string.Empty;
		switch(mPartID){
		case 5000: upID = item.Body_star_ID; car.bodyStar++;starlv = car.bodyStar;icon_name = "Upgrade_icon_Body";break;
		case 5001: upID = item.Engine_star_ID;car.engineStar++;starlv = car.engineStar;icon_name = "Upgrade_icon_Engine";break;
		case 5002: upID = item.Tire_star_ID;car.tireStar++;starlv = car.tireStar;icon_name = "Upgrade_icon_Tire";break;
		case 5003: upID = item.Gbox_star_ID;car.gearBoxStar++;starlv = car.gearBoxStar;icon_name = "Upgrade_icon_Gearbox"; break;
		case 5004: upID = item.Intake_star_ID;car.intakeStar++;starlv = car.intakeStar;icon_name = "Upgrade_icon_Intake";break;
		case 5005: upID = item.BsPower_star_ID;car.bsPowerStar++;starlv = car.bsPowerStar;icon_name = "Upgrade_icon_Nitro";break;
		case 5006: upID = item.BsTime_star_ID;car.bsTimeStar++;starlv = car.bsTimeStar;icon_name = "Upgrade_icon_NitroTime";break;
		default: Utility.LogWarning("partID Null "); break;
		}
		
		ChangeStarCount(car.carClass.StarLV, starlv, icon_name, evoconfirm);
		GV.myCoin = GV.myCoin-lackofCoin;
		GV.updateCoin = lackofCoin;
		GV.isStarLV = true;
		var lobby = GameObject.Find("LobbyUI") as GameObject;
		lobby.SendMessage("InitTopMenu", SendMessageOptions.DontRequireReceiver);
		lobby.SendMessage("ChangeLevelCarButton",false, SendMessageOptions.DontRequireReceiver);
		GV.TeamChangeFlag = 5;
		Global.isPopUp = false;
		GV.UpdateMatCount(matID[0], -1);
		GV.UpdateMatCount(matID[1], -1);
		GV.UpdateMatCount(matID[2], -1);
		if(QubeCount != 0)
			GV.UpdateMatCount(8620, -QubeCount);
		
		GAchieve.instance.achieveInfo.PlusLobbyUserAchievemnet(16003);

		SuccessEvoEffect();


		//	starCarWindowInfo(mCarID, mPartID);
	}
	
	void SuccessEvoEffect(){
		var root = transform.FindChild("Effect") as Transform;
		var car = Resources.Load("Effect_N/Evolution_Success", typeof(GameObject)) as GameObject;
		var race = Instantiate(car) as GameObject;
		race.transform.parent = root;
		race.transform.localPosition = Vector3.zero;
		Utility.LogWarning("SuccessEvoEffect");
	}

	void OnOk(){
		OnClose();
	}
	
	void OnClose(){
		Global.isPopUp = false;
		onFinish();
		parent.SendMessage("OnClose");
	}
	
	void OnCancle(){
		evo.SetActive(true);evoconfirm.SetActive(false);
		plusSuccess = 0;
		lackofMat = 0;
		lackofCoin = 0;
		GageCount = 0;
		bUseCoin = false;
		QubeCount = 0;
		matCount = new int[]{10,9,9,1};
		DeckMatCheckCount();
	}
	
}
