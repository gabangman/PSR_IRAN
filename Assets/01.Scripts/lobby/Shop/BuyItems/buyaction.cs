using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
public class buyaction : BuyInterAction {
	
	//GameObject popup = null;
	string price; 
	void Awake(){
		if(gameObject.GetComponent<TweenAction>() == null)
			gameObject.AddComponent<TweenAction>();
	}
	void OnFreeSponsor(){
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		var mAction = pop.GetComponent<BuySponsor>() as BuySponsor;
		if(mAction == null) mAction = pop.AddComponent<BuySponsor>();
		mAction.SetBuySponPopUp(transform.name, 1,ResultBuyAction, 1);
	
	}
	void OnBuyCoinSponsor(){
		price =transform.FindChild("lbPrice").GetComponent<UILabel>().text;
		//CreaetPopUpWindow(price,gameObject.name, "Coin", "Sponsor");
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		var mAction = pop.GetComponent<BuySponsor>() as BuySponsor;
		if(mAction == null) mAction = pop.AddComponent<BuySponsor>();
		mAction.SetBuySponPopUp(transform.name, 1,ResultBuyAction);


	}

	void OnBuyDollarSponsor(){
		 price =transform.FindChild("lbPrice").GetComponent<UILabel>().text;
		//CreaetPopUpWindow(price,gameObject.name, "Dollar", "Sponsor");
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
		var mAction = pop.GetComponent<BuySponsor>() as BuySponsor;
		if(mAction == null) mAction = pop.AddComponent<BuySponsor>();
		mAction.SetBuySponPopUp(transform.name, 2, ResultBuyAction);
	}

	void OnBuyDollarCar(){
		price =transform.FindChild("lbPrice").GetComponent<UILabel>().text;
		//ActivePopUpWindow(price, "Dollar");
		CreaetPopUpWindow(price,gameObject.name, "Dollar", "Car");
	}

	void OnBuyCoinCar(){
		price =
			transform.FindChild("lbPrice").GetComponent<UILabel>().text;
	//	ActivePopUpWindow(price, "Coin");
		CreaetPopUpWindow(price,gameObject.name, "Coin", "Car");
	
	}	/*
	void OnBuyCoinCrew(){
		
		price =
			transform.FindChild("lbPrice").GetComponent<UILabel>().text;
		//ActivePopUpWindow(price, "Coin");
		CreaetPopUpWindow(price,gameObject.name, "Coin", "Crew");
		//Utility.Log("COIN CREW");
	}
	
	void OnBuyDollarCrew(){
		price =transform.FindChild("lbPrice").GetComponent<UILabel>().text;
		//ActivePopUpWindow(price, "Dollar");
		CreaetPopUpWindow(price,gameObject.name, "Dollar", "Crew");
	}

	void OnBuyCashClick(){

	}

	void OnBuyCouponCar(){
		CreatePopUpWindow();
	}
*/
	void Start(){

	}

	void OnEnable(){

	}

	public void ChangeContents(){
		string[] _id  = gameObject.name.Split("_"[0]);
		switch(_id[1]){
		case "sCar":SettingCarWindowInfomation(_id[0]);
			break;
		case "ssCar":SettingCarWindowInfomation(_id[0]);
			break;
		case "tCar":SettingCarWindowInfomation(_id[0]);
			break;
		case "SponosrS":SettingSponsorWindowInfomation(_id[0]);
			break;
		case "SponsorT":SettingSponsorWindowInfomation(_id[0]);
			break;
		case "Sponsor":SettingSponsorWindowInfomation(_id[0]);
			break;
		default:
			Utility.LogWarning("ChagneInfoContent");
			break;
		}
	}



	StringBuilder sb = new StringBuilder();
	//int couponCount = 0;
	void SettingCarWindowInfomation(string str){
		int id = int.Parse(str);
		Utility.LogWarning("SettingCarWindowInfomation");
		return;
	/*	bool ischeck = false;
		//List<AccountInfo.CarInfo> _car = AccountInfo.instance.listCarInfo;
		List<Account.CarInfo> _car = myAccount.instance.account.listCarInfo;
		for(int i = 0; i < _car.Count;i++){
			if(_car[i].carId == id) {
				ischeck=true;
				break;
			}
		}
		Common_Car_Status.Item _item = Common_Car_Status.Get(id);
		int mSeason = Base64Manager.instance.GlobalEncoding(Global.ChampionSeason);
		int mSLV = 0;
		if(mSeason > 6024){
			Common_Mode_Champion.Item item= Common_Mode_Champion.Get(mSeason);
			mSLV = item.Season;
		}else{
			mSLV = Global.MySeason;
		}

		if(ischeck) {
			ChangeActivate("btnSelect",true);
			ChangeActivate("btnCoin",false);
			ChangeActivate("btnDollar",false);
			ChangeLabel("lbPrice",string.Empty);
			ChangeActivate("btnCoupon",false);
		}else{
			if(mSLV < _item.ReqLV){
				ChangeActivate("btnSelect",false);
				ChangeActivate("btnCoin",false);
				ChangeActivate("btnDollar",false);
				ChangeLabel("lbPrice",string.Empty);
				ChangeActivate("btnCoupon",false);
			}else{
			//	if(mSLV == 6 && _item.ReqLV == 6){
			//		ChangeActivate("btnSelect",false);
			//		ChangeActivate("btnCoin",false);
			//		ChangeActivate("btnDollar",false);
			//		sb.Length = 0;
			//		sb.Append(KoStorage.getStringDic("60218"));
			//		ChangeLabel("lbPrice",sb.ToString());
			//		ChangeActivate("btnCoupon",false);
			//	}else{
					ChangeActivate("btnSelect",false);
					sb.Length = 0;
					sb.Append(string.Format("{0:#,0}",_item.BuyPrice));
					ChangeLabel("lbPrice",sb.ToString());
					if(_item.Res == 2){ // dollar
						ChangeActivate("btnCoin",false);
						ChangeActivate("btnDollar",true);
					}else{ //coin
						ChangeActivate("btnCoin",true);
						ChangeActivate("btnDollar",false);
					}
					bool _b = _item.Coupon_N == 0 ? false:true;
					ChangeActivate("btnCoupon",_b);
					if(_b) {
						transform.FindChild("btnCoupon").GetComponentInChildren<UILabel>().text = 
							KoStorage.getStringDic("60227");//"쿠폰";//KoStorage.getStringDic("");
						couponCount = _item.Coupon_N;
					}
			//	}
			}
		}

		sb.Length =0;
		string cls = KoStorage.getStringDic("60080")+" ";
		sb.Append(cls); sb.Append(_item.Class);
		
		//sb.Append("Class "); sb.Append(_item.Class);
		
		ChangeLabel("lbClass", sb.ToString());
		transform.FindChild("Sprite (Class)").GetComponent<UISprite>().spriteName = "Class_"+_item.Class;
		string text = string.Empty;
		sb.Length = 0;
		sb.Append(_item.Name);
		ChangeLabel("lbName", sb.ToString());
		//성능
		sb.Length = 0;
	//	CTeamAbility ability = new CTeamAbility();
	//	string[] strRet = ability.CarAbilityShop(id);
	//	ability = null;
	//	text = string.Format(KoStorage.getStringDic("60270"),strRet[0]);
	//	sb.Append(text);
	//	ChangeLabel("lbPower", sb.ToString());

		sb.Length = 0;
		//요구 시즌
		text = string.Format(KoStorage.getStringDic("60073"),_item.ReqLV);
		sb.Append(text);
		ChangeLabel("lbReqSeason",sb.ToString());
		//강화 가능단계
		sb.Length = 0;
		text = string.Format(KoStorage.getStringDic("60082"),_item.UpLimit);
		sb.Append(text);
		ChangeLabel("lbUpLimit",sb.ToString());
		//최대기어
		sb.Length = 0;
		text = string.Format(KoStorage.getStringDic("60190"),_item.GearLmt);
		sb.Append(text);
		ChangeLabel("lbUpGear",sb.ToString());
		var _star = transform.FindChild("StarLV").gameObject as GameObject;
		var lbStar = transform.FindChild("lbStarLV_no").GetComponent<UILabel>() as UILabel;
		if(_item.ReqLV < 0){
			_star.SetActive(false);
			lbStar.text = KoStorage.getStringDic("60278");
		}else{
			lbStar.text = string.Empty;
			_star.SetActive(true);
			var _starLv = _star.GetComponent<StarLevelInit>() as StarLevelInit;
			if(_starLv == null) _starLv = _star.AddComponent<StarLevelInit>();
			_starLv.Initialize(_item.StarLV);
		}
		//transform.FindChild("StarLV").gameObject.AddComponent<StarLevelInit>().Initialize(_item.StarLV);
		float f1 = _item.P_Control;
		sb.Length = 0;
		if(f1 < 0.13f){
			sb.Append(KoStorage.getStringDic("60377"));
		}else if(f1 < 0.16f){sb.Append(KoStorage.getStringDic("60376"));
		}else if(f1 < 0.21f){sb.Append(KoStorage.getStringDic("60375"));
		}else if(f1 < 0.31f){sb.Append(KoStorage.getStringDic("60374"));
		}
		ChangeLabel("lbCotrol",sb.ToString());
		*/
		
	}
	/*
	void SettingCrewWindowInfomation(string str){
		int id = int.Parse(str);
		bool ischeck = false;
		//List<AccountInfo.CrewInfo> _car = AccountInfo.instance.listCrewInfo;
		List<Account.CrewInfo> _car = myAccount.instance.account.listCrewInfo;
		for(int i = 0; i < _car.Count;i++){
			if(_car[i].crewId == id) {
				ischeck=true;
				break;
			}
		}

		Common_Crew_Status.Item _item = Common_Crew_Status.Get(id);
		int mSeason = Base64Manager.instance.GlobalEncoding(Global.ChampionSeason);
		int mSLV = 0;
		if(mSeason > 6024){
			Common_Mode_Champion.Item item= Common_Mode_Champion.Get(mSeason);
			mSLV = item.Season;
		}else{
			mSLV = Global.MySeason;
		}
		if(ischeck) {
			ChangeActivate("btnSelect",true);
			ChangeActivate("btnCoin",false);
			ChangeActivate("btnDollar",false);
			ChangeLabel("lbPrice",string.Empty);			
		}else{
			if(mSLV < _item.ReqLV){
				ChangeActivate("btnSelect",false);
				ChangeActivate("btnCoin",false);
				ChangeActivate("btnDollar",false);
				ChangeLabel("lbPrice",string.Empty);
			}else{
				if(mSLV == 6 && _item.ReqLV == 6){
					ChangeActivate("btnSelect",false);
					ChangeActivate("btnCoin",false);
					ChangeActivate("btnDollar",false);
					sb.Length = 0;
					sb.Append(KoStorage.getStringDic("60218"));
					ChangeLabel("lbPrice",sb.ToString());
				}else{ 
					ChangeActivate("btnSelect",false);
					sb.Length = 0;
					sb.Append(string.Format("{0:#,0}",_item.BuyPrice));
					ChangeLabel("lbPrice",sb.ToString());
					if(_item.Res == 2){
						ChangeActivate("btnCoin",false);
						ChangeActivate("btnDollar",true);
					}else{
						ChangeActivate("btnCoin",true);
						ChangeActivate("btnDollar",false);
					}
				//}
			}
		}
		transform.FindChild("Sprite_Tmark").GetComponent<UISprite>().spriteName = id.ToString()+"L";
		//ChildObjectSpriteChange("Spirte_Tmark",);
			transform.FindChild("Sprite (Class)").GetComponent<UISprite>().spriteName = "Class_"+_item.Class;
		sb.Length = 0;
		sb.Append(_item.Name);
		ChangeLabel("lbName", sb.ToString());

		sb.Length =0;
		string cls = KoStorage.getStringDic("60080")+" ";
		sb.Append(cls); sb.Append(_item.Class);
		ChangeLabel("lbClass", sb.ToString());
		transform.FindChild("Sprite (Class)").GetComponent<UISprite>().spriteName = "Class_"+_item.Class;
		sb.Length = 0;
	//	transform.FindChild("Sprite (Class)").GetComponent<UISprite>().spriteName = "Class_"+_item.Class;
	//	sb.Length=0;
	//	sb.Append(_item.Text);
	//	ChangeLabel("lbText",sb.ToString());
		//요구시즌
		sb.Length = 0;
		string tex = string.Format(KoStorage.getStringDic("60073"),_item.ReqLV);
		sb.Append(tex);
		ChangeLabel("lbReqSeason",sb.ToString());
		//강화가능단계
		sb.Length = 0;
		tex = string.Format(KoStorage.getStringDic("60082"),_item.UpLimit);
		sb.Append(tex);
		ChangeLabel("lbUpLimit",sb.ToString());
		//능력치
		sb.Length = 0;
		CTeamAbility ability = new CTeamAbility();
		string[] strRet = ability.CrewAbilityShop(id);
		tex = string.Format(KoStorage.getStringDic("60271"),strRet[0]);
		sb.Append(tex);
		ChangeLabel("lbTeamwork", sb.ToString());
		
		
		var _star = transform.FindChild("StarLV").gameObject as GameObject;
		_star.gameObject.SetActive(false);
		//
		var lbStar = transform.FindChild("lbStarLV_no").GetComponent<UILabel>() as UILabel;
		if(_item.ReqLV < 2){
			_star.SetActive(false);
			lbStar.text = KoStorage.getStringDic("60278");
			lbStar.transform.gameObject.SetActive(true);
		}else{
			lbStar.text = string.Empty;
			_star.SetActive(true);
			var _starLv = _star.GetComponent<StarLevelInit>() as StarLevelInit;
			if(_starLv == null) _starLv = _star.AddComponent<StarLevelInit>();
			_starLv.Initialize(_item.StarLV);
		}
		//
		
	}
*/
	void SettingSponsorWindowInfomation(string str){
		int id = int.Parse(str);
	//	Utility.LogWarning("SettingSponsorWindowInfomation");
		Common_Sponsor_Status.Item _item = Common_Sponsor_Status.Get(id);
		gameObject.transform.FindChild("lbPrice").GetComponent<UILabel>().text = 
		string.Format("{0:#,0}",_item.BuyPrice);
		gameObject.transform.FindChild("lbName").GetComponent<UILabel>().text = _item.Name;
		gameObject.transform.FindChild("Image").GetComponent<UISprite>().spriteName = _item.ID;

		int _bonus = BonusSeason(_item);
		sb.Length = 0;
		//string tex = string.Format(KoStorage.GetKorString("76104"),_bonus);
		string tex = string.Format("{0:#,0}",_bonus);
		sb.Append(tex);
		ChangeLabel("lbBonusPrice",sb.ToString());
		ChangeLabel("lbBonus", KoStorage.GetKorString("76104"));
		sb.Length = 0;
		tex = string.Format( KoStorage.GetKorString("76105"),_item.Period);
		sb.Append(tex);
		ChangeLabel("lbPeriod",sb.ToString());
		sb.Length = 0;
		tex = string.Format(KoStorage.GetKorString("76112"),  _item.Power_inc,_item.Dura_dec, _item.Led_dec); 
		sb.Append(tex);
		ChangeLabel("lbAddAblity",sb.ToString());
		int tempid = GV.getTeamSponID(GV.SelectedTeamID);
		if(id == tempid) 	{
			transform.FindChild("btnSelect").gameObject.SetActive(true);
			transform.FindChild("btnSelect").GetComponentInChildren<UILabel>().text =
				KoStorage.GetKorString("76108");
			transform.FindChild("lbPrice").gameObject.SetActive(false);
			gameObject.transform.FindChild("btnCoin").gameObject.SetActive(false);
			gameObject.transform.FindChild("btnDollar").gameObject.SetActive(false);
			gameObject.transform.FindChild("btnFree").gameObject.SetActive(false);
			return;
		}else{
			transform.FindChild("btnSelect").gameObject.SetActive(false);
			transform.FindChild("lbPrice").gameObject.SetActive(true);
			gameObject.transform.FindChild("btnFree").gameObject.SetActive(false);
			if(_item.Res == 1){
				gameObject.transform.FindChild("btnCoin").gameObject.SetActive(true);
				gameObject.transform.FindChild("btnDollar").gameObject.SetActive(false);
			}else{
				gameObject.transform.FindChild("btnDollar").gameObject.SetActive(true);
				gameObject.transform.FindChild("btnCoin").gameObject.SetActive(false);
			}

		}


		if(CClub.mClubFlag == 2){// 0 : no season, 1 : seaon ok but no clanmember, 2 : season OK & clanmember
			if(CClub.mClubInfo.clubLevel < 2){
			
			
			}else{
				int clubLevel = CClub.mClubInfo.clubLevel;
				bool bSucc = false;
				if(clubLevel >= 2 && clubLevel < 5){
					if(id <= 1301){
						bSucc = true;
					}else bSucc = false;
				}else if(clubLevel >=5 && clubLevel <10){
					if(id <= 1302){
						bSucc = true;
					}else bSucc = false;
				}else if(clubLevel >=10 && clubLevel <15){
					if(id <= 1303){
						bSucc = true;
					}else bSucc = false;
				}else if(clubLevel >=15 && clubLevel <20){
					if(id <= 1304){
						bSucc = true;
					}else bSucc = false;
				}else if(clubLevel >= 20){
					bSucc = true;
				}

				if(bSucc){
					transform.FindChild("lbPrice").gameObject.SetActive(false);
					gameObject.transform.FindChild("btnDollar").gameObject.SetActive(false);
					gameObject.transform.FindChild("btnCoin").gameObject.SetActive(false);
					var tr = gameObject.transform.FindChild("btnFree").gameObject as GameObject;
					tr.SetActive(true);
					tr.GetComponent<UIButtonMessage>().functionName = "OnFreeSponsor";
				}else{
				
				}
			
			}
		}else{ 
			
		}
	


	}
	
	int BonusSeason(Common_Sponsor_Status.Item _item){
		int season = 0;
		switch(GV.ChSeason){
		case 1: season = _item.BonusT1;
			break;
		case 2:season = _item.BonusT2;
			break;
		case 3:season = _item.BonusT3;
			break;
		case 4:season = _item.BonusT4;
			break;
		case 5: season = _item.BonusT5;
				break;
		case 6: season = _item.BonusT6; break;
		case 7: season = _item.BonusT7; break;
		case 8: season = _item.BonusT8; break;
		}
		return season;
	}

	void CreaetPopUpWindow(string price, string _id, string _type, string _buyType){
		if(Global.isPopUp) return;
		var pop = ObjectManager.SearchWindowPopup() as GameObject;
	
		pop.name = _id  + "_"+ _type;
		var popchild =  pop.transform.FindChild("Content_BUY").gameObject as GameObject;
		popchild.transform.FindChild("lbPrice").GetComponent<UILabel>().text = price.ToString();
		var _popupaction = pop.GetComponent<popupinteraction>() as popupinteraction;
		if(_popupaction == null) _popupaction = pop.AddComponent<popupinteraction>();
		_popupaction.SaveParentObject(popchild);
		_popupaction.CalledBuyCompete(ResultBuyAction);
		_popupaction.PopWindowStart();
		Global.isPopUp = true;
		popchild = pop = null;	
		_popupaction = null;
	}



	void ResultBuyAction(bool isSuccess){
		if(!isSuccess) return;
		ChangeActivate("btnSelect",true);
		transform.FindChild("btnSelect").GetComponentInChildren<UILabel>().text =
			KoStorage.GetKorString("76108");
		ChangeLabel("lbPrice",string.Empty);	
		ChangeActivate("btnCoin",false);
		ChangeActivate("btnDollar",false);
		ChangeActivate("btnCoupon",false);
		ChangeActivate("btnFree",false);
		//Utility.Log("result " + gameObject.name);
		CheckHaveSponsor();
	}

	public void CheckHaveSponsor(){
		string[] name = gameObject.name.Split("_"[0]);
		int _id = int.Parse(name[0]);
		if(_id < 1300) return; 
		//Utility.Log (" " + _id + "    " + Global.MySponsorID);
		int tempid = GV.getTeamSponID(GV.SelectedTeamID);//Base64Manager.instance.GlobalEncoding(Global.MySponsorID);
		if(tempid == _id  ){
			transform.FindChild("btnSelect").gameObject.SetActive(true);
			transform.FindChild("btnCoin").gameObject.SetActive(false);
			transform.FindChild("btnDollar").gameObject.SetActive(false);
			transform.FindChild("lbPrice").gameObject.SetActive(false);
		}
	}

	public void OnBuyCoinCar(string price1){
		price = price1;
		CreaetPopUpWindow(price,gameObject.name, "Coin", "Car");
		Destroy(this);
	}	
}
