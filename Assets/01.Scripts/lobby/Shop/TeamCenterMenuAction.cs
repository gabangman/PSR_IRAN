using UnityEngine;
using System.Collections;

public class TeamCenterMenuAction : InfoWindow {
	
	public Transform Car, Spon;
	const int originScaleY = 44;
	const int clickScaleY = 55;
	public Transform upCar, upCrew, upMenu;
	//public Transform mechanicInven, mechanicMake, mechanicMenu;
	
	public Transform shopMenu;
	public Transform[] shop;//Container, shopCar, shopShow;
	public Color PressColor,ReleaseColor;
	
	public Transform[] upgradeSub; //0 btn repair 1 star
	public Transform[] invenSub; // 0~2 car 3~5 mat 
	public Transform invenCar, invenMat, invenCoupon,invenQube, invenMenu;

	private Animation teamUpAni, InvenAni, ShopAni;

	void Awake(){
		//	Car.GetComponent<UISprite>().color = PressColor;
		//	Spon.GetComponent<UISprite>().color = PressColor;
		upCar.GetComponent<UISprite>().color = PressColor;
		upCrew.GetComponent<UISprite>().color = PressColor;
		invenCar.GetComponent<UISprite>().color = PressColor;
		invenMat.GetComponent<UISprite>().color = PressColor;
		invenCoupon.GetComponent<UISprite>().color = PressColor;
		invenQube.GetComponent<UISprite>().color = PressColor;
		shop[0].GetComponent<UISprite>().color = PressColor;
		shop[1].GetComponent<UISprite>().color = PressColor;
		shop[2].GetComponent<UISprite>().color = PressColor;
		
		teamUpAni = upMenu.GetComponent<Animation>();
		InvenAni = invenMenu.GetComponent<Animation>();
		ShopAni = invenMenu.GetComponent<Animation>();


		upMenu.gameObject.AddComponent<SubButtonAniControl>();
		invenMenu.gameObject.AddComponent<SubButtonAniControl>();
		shopMenu.gameObject.AddComponent<SubButtonAniControl>();
		//	shopContainer.GetComponent<UISprite>().color = PressColor;
		//	shopCar.GetComponent<UISprite>().color = PressColor;
		//	shopShow.GetComponent<UISprite>().color = PressColor;
	}
	
	void OnEnable(){
		//NewBtnReset (shop[1], myAcc.instance.account.bLobbyBTN[4]);
	}
	
	
	
	
	void NewBtnReset(Transform tr, bool b){
		tr.parent.FindChild("icon_New").gameObject.SetActive(b);
	}
	
	public bool OnCarClick(){
		bool b = false;
		var carsprite = Car.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == PressColor) b = true;
		else{
			carsprite.color = PressColor;
			Car.parent.FindChild("Select").gameObject.SetActive(true);
			Spon.GetComponent<UISprite>().color = ReleaseColor;
			Spon.parent.FindChild("Select").gameObject.SetActive(false);
		}
		//bool isNew = myAccount.instance.account.buttonStatus.isTeamCarNew;
		//if(isNew)   {
		//	isNew = myAccount.instance.checkNewCar();
		//	myAccount.instance.account.buttonStatus.isTeamCarNew = isNew;
		//	NewBtnReset(Car, isNew);
		//}
		return b;
	}
	
	public bool OnSponClick(){
		bool b = false;
		var carsprite = Spon.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == Color.white) b = true;
		else{
			carsprite.color = PressColor;
			Spon.parent.FindChild("Select").gameObject.SetActive(true);
			Car.GetComponent<UISprite>().color = ReleaseColor;
			Car.parent.FindChild("Select").gameObject.SetActive(false);
		}
		//	bool isNew = myAccount.instance.account.buttonStatus.isTeamCrewNew;
		//	if(isNew)   {
		//		isNew = myAccount.instance.checkNewCrew();
		//		myAccount.instance.account.buttonStatus.isTeamCrewNew = isNew;
		//		NewBtnReset(Crew, isNew);
		//	}
		return b;
	}
	
	
	
	public void InitCenterMenu(){
		//	Car.GetComponent<UISprite>().color = PressColor;
		//	Spon.GetComponent<UISprite>().color = ReleaseColor;
		//	Car.transform.parent.gameObject.SetActive(false);
		//	Spon.transform.parent.gameObject.SetActive(false);
	}
	
	public void InitCenterUpMenu(string str){
		//showSub(upgradeSub[0].parent.parent.gameObject);
		
		if(str.Equals("Car")){
			upCrew.GetComponent<UISprite>().color =ReleaseColor;
			upCar.GetComponent<UISprite>().color = PressColor;
			upCrew.parent.FindChild("Select").gameObject.SetActive(false);
			upCar.parent.FindChild("Select").gameObject.SetActive(true);
			//		upgradeSub[0].transform.parent.gameObject.SetActive(false);
			//		upgradeSub[1].transform.parent.gameObject.SetActive(true);
		}else{
			upCrew.GetComponent<UISprite>().color =PressColor;
			upCar.GetComponent<UISprite>().color = ReleaseColor;
			upCrew.parent.FindChild("Select").gameObject.SetActive(true);
			upCar.parent.FindChild("Select").gameObject.SetActive(false);
			//		upgradeSub[0].transform.parent.gameObject.SetActive(false);
			//		upgradeSub[1].transform.parent.gameObject.SetActive(true);
			
		}
		showSub(upgradeSub[0].parent.parent.gameObject);
		//	upgradeSub[0].transform.parent.gameObject.SetActive(false);
		//	upgradeSub[1].transform.parent.gameObject.SetActive(true);
		//	upgradeSub[1].transform.parent.GetComponent<UIButtonMessage>().functionName =
		//		"OnUpCrewStar";
	}
	
	public void InitCenterInvenMenu(){
		invenCar.GetComponent<UISprite>().color = PressColor;
		invenCar.parent.FindChild("Select").gameObject.SetActive(true);
		invenMat.GetComponent<UISprite>().color = ReleaseColor;
		invenMat.parent.FindChild("Select").gameObject.SetActive(false);
		invenCoupon.GetComponent<UISprite>().color = ReleaseColor;
		invenCoupon.parent.FindChild("Select").gameObject.SetActive(false);
		invenQube.GetComponent<UISprite>().color = ReleaseColor;
		invenQube.parent.FindChild("Select").gameObject.SetActive(false);
		InvenMenuAcivation(0);
	
		//ShowInvenCar();
	}
	
	void InvenMenuAcivation(int idx){
		if(idx == 0){
			ShowInvenCar();
			invenSub[2].parent.parent.gameObject.SetActive(true);
			//	invenSub[4].parent.parent.gameObject.SetActive(false);
		}else if(idx == 1){
			invenSub[2].parent.parent.gameObject.SetActive(false);
			//	invenSub[4].parent.parent.gameObject.SetActive(false);
		}else {
			invenSub[2].parent.parent.gameObject.SetActive(false);
			//	invenSub[4].parent.parent.gameObject.SetActive(true);
		}
		
		//invenSub[0].GetComponent<UISprite>().color = PressColor;
		//	invenSub[1].GetComponent<UISprite>().color = PressColor;
		invenSub[2].GetComponent<UISprite>().color = PressColor;
		//	invenSub[2].parent.FindChild("Select").gameObject.SetActive(true);
		//	invenSub[3].GetComponent<UISprite>().color = PressColor;
		//	invenSub[4].GetComponent<UISprite>().color = PressColor;
		//	invenSub[5].GetComponent<UISprite>().color = ReleaseColor;
	}
	
	
	
	public void InitCenterShopMenu(){
		//	shopContainer.GetComponent<UISprite>().color = PressColor;
		//	shopCar.GetComponent<UISprite>().color = ReleaseColor;
		//	shopShow.GetComponent<UISprite>().color = ReleaseColor;
		shop[0].GetComponent<UISprite>().color = PressColor;
		shop[1].GetComponent<UISprite>().color = ReleaseColor;
		shop[2].GetComponent<UISprite>().color = ReleaseColor;
		
		shop[0].parent.FindChild("Select").gameObject.SetActive(true);
		shop[1].parent.FindChild("Select").gameObject.SetActive(false);
		shop[2].parent.FindChild("Select").gameObject.SetActive(false);
		int a = EncryptedPlayerPrefs.GetInt("DealerBuy");
		if(a != 10){
			//int a = EncryptedPlayerPrefs.GetInt("DealerBuy");
			if(a == 0){
				NewBtnReset (shop[1],false);
			}else if(a == 1){
				NewBtnReset (shop[1],true);
			}else if(a == 2){	// 구매 했다.
				NewBtnReset (shop[1],false);
			}
		}else{
			NewBtnReset (shop[1],false);
		}
	}
	
	public void InitDealerMenu(){
		int a = EncryptedPlayerPrefs.GetInt("DealerBuy");
		if(a != 10){
			if(a == 0){
				NewBtnReset (shop[1],false);
			}else if(a == 1){
				NewBtnReset (shop[1],true);
			}else if(a == 2){	// 구매 했다.
				NewBtnReset (shop[1],false);
			}
		}else{
			NewBtnReset (shop[1],false);
		}
	}
	public bool OnUpCarClick(){
		bool b = false;
		var carsprite = upCar.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == PressColor) b = true;
		else{
			carsprite.color =PressColor;
			upCrew.GetComponent<UISprite>().color =ReleaseColor;
			upCar.parent.FindChild("Select").gameObject.SetActive(true);
			upCrew.parent.FindChild("Select").gameObject.SetActive(false);
		}
		return b;
	}
	public bool OnUpCrewClick(){
		bool b = false;
		var carsprite = upCrew.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == PressColor) b = true;
		else{
			upCar.GetComponent<UISprite>().color =ReleaseColor;
			carsprite.color = PressColor;
			upCrew.parent.FindChild("Select").gameObject.SetActive(true);
			upCar.parent.FindChild("Select").gameObject.SetActive(false);
		}
		return b;
	}
	
	public bool OnInvenCar(){
		bool b = false;
		var carsprite = invenCar.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == PressColor) b = true;
		{
			carsprite.color = PressColor;
			invenCoupon.GetComponent<UISprite>().color = ReleaseColor;
			invenMat.GetComponent<UISprite>().color = ReleaseColor;
			invenQube.GetComponent<UISprite>().color = ReleaseColor;
			
			invenCar.parent.FindChild("Select").gameObject.SetActive(true);
			invenCoupon.parent.FindChild("Select").gameObject.SetActive(false);
			invenMat.parent.FindChild("Select").gameObject.SetActive(false);
			invenQube.parent.FindChild("Select").gameObject.SetActive(false);
			
			
		}
		if(!b){
			myAcc.instance.account.bInvenBTN[0] = false;
			invenMenu.GetComponent<InvenButton>().ReSetSubMenuBTN();
		}
		return b;
	}
	
	public bool OnInvenMat(){
		bool b = false;
		myAcc.instance.account.bInvenBTN[1] = false;
		var carsprite = invenMat.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == PressColor) b = true;
		{
			carsprite.color = PressColor;
			invenCoupon.GetComponent<UISprite>().color = ReleaseColor;
			invenCar.GetComponent<UISprite>().color = ReleaseColor;
			invenQube.GetComponent<UISprite>().color = ReleaseColor;
			
			invenMat.parent.FindChild("Select").gameObject.SetActive(true);
			invenCoupon.parent.FindChild("Select").gameObject.SetActive(false);
			invenCar.parent.FindChild("Select").gameObject.SetActive(false);
			invenQube.parent.FindChild("Select").gameObject.SetActive(false);
		}
		if(!b){
			//myAcc.instance.account.bInvenBTN[1] = false;
			//invenMenu.GetComponent<InvenButton>().ReSetSubMenuBTN();
		}	
		invenMenu.GetComponent<InvenButton>().ReSetSubMenuBTN();

		return b;
	}
	
	public bool OnInvenCoupon(){
		bool b = false;
		var carsprite = invenCoupon.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == PressColor) b = true;
		{
			carsprite.color = PressColor;
			invenMat.GetComponent<UISprite>().color = ReleaseColor;
			invenCar.GetComponent<UISprite>().color = ReleaseColor;
			invenQube.GetComponent<UISprite>().color = ReleaseColor;
			
			
			invenCoupon.parent.FindChild("Select").gameObject.SetActive(true);
			invenMat.parent.FindChild("Select").gameObject.SetActive(false);
			invenCar.parent.FindChild("Select").gameObject.SetActive(false);
			invenQube.parent.FindChild("Select").gameObject.SetActive(false);
		}
		if(!b){
			myAcc.instance.account.bInvenBTN[3] = false;
			invenMenu.GetComponent<InvenButton>().ReSetSubMenuBTN();
		}
		return b;
		
	}
	
	public bool OnInvenQube(){
		bool b = false;
		var carsprite = invenQube.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == PressColor) b = true;
		{
			carsprite.color = PressColor;
			invenMat.GetComponent<UISprite>().color = ReleaseColor;
			invenCar.GetComponent<UISprite>().color = ReleaseColor;
			invenCoupon.GetComponent<UISprite>().color = ReleaseColor;
			
			invenQube.parent.FindChild("Select").gameObject.SetActive(true);
			invenMat.parent.FindChild("Select").gameObject.SetActive(false);
			invenCar.parent.FindChild("Select").gameObject.SetActive(false);
			invenCoupon.parent.FindChild("Select").gameObject.SetActive(false);
		}
		
		if(!b){
			myAcc.instance.account.bInvenBTN[2] = false;
			invenMenu.GetComponent<InvenButton>().ReSetSubMenuBTN();
		}
		return b;
		
	}
	
	public bool OnChoiceCar(int idx){
		bool b = false;
		var sprite = invenSub[idx].GetComponent<UISprite>() as UISprite;
		if(sprite.color == PressColor) b = true;
		else {
			for(int i = 2; i < 3; i++){
				if(idx == i){
					invenSub[i].GetComponent<UISprite>().color = PressColor;
					//	invenSub[i].parent.FindChild("Select").gameObject.SetActive(true);
				}
				
				else{ 
					invenSub[i].GetComponent<UISprite>().color = ReleaseColor;
					//	invenSub[i].parent.FindChild("Select").gameObject.SetActive(false);
				}
			}
		}
		return b;
	}
	
	public bool OnChoiceMat(int idx){
		bool b = false;
		var sprite = invenSub[idx].GetComponent<UISprite>() as UISprite;
		if(sprite.color == PressColor) b = true;
		else {
			for(int i = 4; i < 6; i++){
				if(idx == i){
					invenSub[i].GetComponent<UISprite>().color = PressColor;
					//invenSub[i].parent.FindChild("Select").gameObject.SetActive(true);
				}
				
				else{ 
					invenSub[i].GetComponent<UISprite>().color = ReleaseColor;
					//	invenSub[i].parent.FindChild("Select").gameObject.SetActive(false);
				}
			}
		}
		return b;
	}
	
	
	
	/*public bool OnInvenPart(){
		bool b = false;
		var carsprite = mechanicMake.GetComponent<UISprite>() as UISprite;
		if(carsprite.color == PressColor) b = true;
		else{
			carsprite.color =PressColor;
			mechanicInven.GetComponent<UISprite>().color = ReleaseColor;
		}
		return b;
	}
*/
	
	
	public bool OnShopSub(int idx){
		bool b = false;
		var sprite = shop[idx].GetComponent<UISprite>() as UISprite;
		if(sprite.color == PressColor) b = true;
		else {
			for(int i = 0; i < 3; i++){
				if(idx == i)
				{shop[i].GetComponent<UISprite>().color = PressColor;
					shop[i].parent.FindChild("Select").gameObject.SetActive(true);}
				else {
					shop[i].GetComponent<UISprite>().color = ReleaseColor;
					shop[i].parent.FindChild("Select").gameObject.SetActive(false);
				}
			}
		}
		return b;
	}
	/*
	public bool OnRepairSub(){
		bool b = false;
		var carsprite = upgradeSub[0].GetComponent<UISprite>() as UISprite;
		if(carsprite.color == PressColor) b = true;
		else{
			carsprite.color =PressColor;
			upgradeSub[1].GetComponent<UISprite>().color = ReleaseColor;
		}
		return b;
	}
	
	public bool OnStarSub(){
		bool b = false;
		var carsprite = upgradeSub[1].GetComponent<UISprite>() as UISprite;
		if(carsprite.color == PressColor) b = true;
		else{
			carsprite.color =PressColor;
			upgradeSub[0].GetComponent<UISprite>().color = ReleaseColor;
		}
		return b;
	}
*/
	public void LobbyInit(){
		if(upMenu.gameObject.activeSelf){
			var temp = upMenu.GetComponent<SubButtonAniControl>() as SubButtonAniControl;
			if(temp == null) temp = upMenu.gameObject.AddComponent<SubButtonAniControl>();
			temp.ReverseAni(0);
		}
		if(invenMenu.gameObject.activeSelf){
			var temp = invenMenu.GetComponent<SubButtonAniControl>() as SubButtonAniControl;
			if(temp == null) temp = invenMenu.gameObject.AddComponent<SubButtonAniControl>();
			temp.ReverseAni(1);
			//invenMenu.GetComponent<SubButtonAniControl>().ReverseAni(1);
			//StartCoroutine("ReverseAni", 1);
		}

		if(shopMenu.gameObject.activeSelf) {
			var temp = shopMenu.GetComponent<SubButtonAniControl>() as SubButtonAniControl;
			if(temp == null) temp = shopMenu.gameObject.AddComponent<SubButtonAniControl>();
			temp.ReverseAni(2);
			//invenMenu.GetComponent<SubButtonAniControl>().ReverseAni(2);
			//StartCoroutine("ReverseAni", 2);
		}
	

		//	invenMenu.gameObject.SetActive(false);
	//	shopMenu.gameObject.SetActive(false);
	//	StartCoroutine("ReverseAni");
		gameObject.SetActive(false);
	}

	public void ShopInit(){
		upMenu.gameObject.SetActive(false);invenMenu.gameObject.SetActive(false);
		shopMenu.gameObject.SetActive(true);
		var temp  = shopMenu.GetComponent<SubButtonAniControl>() as SubButtonAniControl;
		if(temp == null) temp  = shopMenu.gameObject.AddComponent<SubButtonAniControl>();
		temp.FowardAni(2);
		gameObject.SetActive(false);
	}
	public void TeamInit(){
		upMenu.gameObject.SetActive(false);invenMenu.gameObject.SetActive(false);
		gameObject.SetActive(true);
	}
	public void TeamUpInit(){
		upMenu.gameObject.SetActive(true);invenMenu.gameObject.SetActive(false);
		//StartCoroutine("ForwardAni",1);
		//upMenu.GetComponent<SubButtonAniControl>().FowardAni(0);
		var temp  = upMenu.GetComponent<SubButtonAniControl>() as SubButtonAniControl;
		if(temp == null) temp  = upMenu.gameObject.AddComponent<SubButtonAniControl>();
		temp.FowardAni(0);
		//gameObject.SetActive(false);

	}
	
	public void TeamCrewInit(){
		//	upMenu.gameObject.SetActive(false);mechanicMenu.gameObject.SetActive(false);
		//	gameObject.SetActive(true);
	}
	
	public void TeamUpCrewInit(){
		//	showSub(upgradeSub[0].parent.parent.gameObject);
		//upMenu.gameObject.SetActive(true);
		//	upgradeSub[0].transform.parent.gameObject.SetActive(false);
		//	upgradeSub[1].transform.parent.gameObject.SetActive(true);
		//	upgradeSub[1].transform.parent.GetComponent<UIButtonMessage>().functionName =
		//		"OnUpCrewStar";
	}
	public void TeamUpCarInit(){
		//	showSub(upgradeSub[0].parent.parent.gameObject);
		//	upgradeSub[0].transform.parent.gameObject.SetActive(true);
		//	upgradeSub[1].transform.parent.gameObject.SetActive(true);
		//	upgradeSub[1].transform.parent.GetComponent<UIButtonMessage>().functionName =
		//		"OnUpCarStar";
		//upMenu.gameObject.SetActive(true);mechanicMenu.gameObject.SetActive(false);
		//gameObject.SetActive(false);
	}
	public void InvenInit(){
		upMenu.gameObject.SetActive(false);
		invenMenu.gameObject.SetActive(true);
		var temp  = invenMenu.GetComponent<SubButtonAniControl>() as SubButtonAniControl;
		if(temp == null) temp  = invenMenu.gameObject.AddComponent<SubButtonAniControl>();
		temp.FowardAni(1);
		//invenMenu.GetComponent<SubButtonAniControl>().FowardAni(1);
		//StartCoroutine("ForwardAni",0);
		gameObject.SetActive(false);
	}
	
	public void ShowInvenCar(){
		var obj = invenSub[0].parent.parent.gameObject as GameObject;
		if(obj.activeSelf) return;
		showSub(obj);
	}
	
	public void HiddenInvenCar(){
		var obj = invenSub[0].parent.parent.gameObject as GameObject;
		if(!obj.activeSelf) return;
		hiddenSub(obj);
	}
	
	public void ShowInvenMat(){
		//var obj = invenSub[3].parent.parent.gameObject as GameObject;
		var obj = invenSub[4].parent.parent.gameObject as GameObject;
		if(obj.activeSelf) return;
		showSub(obj);
	}
	
	public void HiddenInvenMat(){
		//var obj = invenSub[3].parent.parent.gameObject as GameObject;
		var obj = invenSub[4].parent.parent.gameObject as GameObject;
		if(!obj.activeSelf) return;
		hiddenSub(obj);
	}
	
	public void initInvenSub(int idx){
		if(idx == 2){
			HiddenInvenCar();
			HiddenInvenMat();
		}else if(idx == 1) {
			ShowInvenMat();
			HiddenInvenCar();
		}else if(idx == 3){
			ShowInvenCar();
			HiddenInvenMat();
		}else{
			ShowInvenCar();
			HiddenInvenMat();
		}
		
	}

	public void disableTeamUp(){
		var temp = upMenu.GetComponent<SubButtonAniControl>() as SubButtonAniControl;
		if(temp == null) temp = upMenu.gameObject.AddComponent<SubButtonAniControl>();
		temp.ReverseAni(0);
	}

	public void disableShop(){
		var temp = shopMenu.GetComponent<SubButtonAniControl>() as SubButtonAniControl;
		if(temp == null) temp = shopMenu.gameObject.AddComponent<SubButtonAniControl>();
		temp.ReverseAni(2);
	}
	public void disableInven(){
		var temp = invenMenu.GetComponent<SubButtonAniControl>() as SubButtonAniControl;
		if(temp == null) temp = invenMenu.gameObject.AddComponent<SubButtonAniControl>();
		temp.ReverseAni(1);
	}
	/*
	GameObject Inven;
	public void InvenWindow(){
		var inven = ObjectManager.CreateTagPrefabs("Inven") as GameObject;
		if(inven != null) {
			
		}else{
			inven = ObjectManager.CreateLobbyPrefabs(gameObject.transform, "Window", "InvenCard", "inven") as GameObject;
			inven.transform.parent = gameObject.transform.parent;
			ObjectManager.ChangeObjectPosition(inven, new Vector3(0,0,0), Vector3.one, Vector3.zero);
		}
		inven.GetComponent<InvenCard>().InitWin();
		Inven = inven;
	}

	public void InvenHidden(){
		if(Inven == null) return;
		Inven.GetComponent<TweenAction>().ReverseTween(Inven);
		Inven = null;
	}

	void OnDisable(){
		//upMenu.gameObject.SetActive(false);
	//	gameObject.SetActive(false);
	}*/
}
