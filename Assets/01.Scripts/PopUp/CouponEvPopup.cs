using UnityEngine;
using System.Collections;

public class CouponEvPopup : basePopup {
	
	public override void OnOkClick(){
		//Callback();
	OnCloseClick();
	}
      //-1: 쿠폰 이름이 존재하지 않는 경우
		//			-2: 쿠폰 유효 기간이 지난 경우
		//			-3: 이미 아이템을 지급받은 경우
	public void InitPopUp(int idx){
		string str = string.Empty;
		if(idx == -3){ //	-3: 이미 아이템을 지급받은 경우
			str = KoStorage.GetKorString("72311"); //이미 사용 / /유효 기간 만료
		}else if(idx == -1){ // -1: 쿠폰 이름이 존재하지 않는 경우

			str = KoStorage.GetKorString("72312"); //쿠폰 번호 잘못 입력 하거나 없는 경우
		}else if(idx == -2){
			//-2: 쿠폰 유효 기간이 지난 경우
			str = KoStorage.GetKorString("72334"); 
		}
		ChangeContentNoCheckOKayString(KoStorage.GetKorString("72310"),	KoStorage.GetKorString("71000"),str);
		
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnOkClick",0.1f);
			return true;
		};
	}
	
	public void SuccessPopUp(int resType, int resValue){
		/*	string strRes = string.Empty;
	switch(resType){
		case 2 :{
			strRes = KoStorage.GetKorString("71001");
		}	break;
		case 0 :{
			strRes = KoStorage.GetKorString("71002");
		}break;
		case 3 :{
			//str = "Material";
			strRes = KoStorage.GetKorString("75001");
		}break;
		case 4 : {
			//	str = "SilverBox";
			strRes = KoStorage.GetKorString("18513");
		}break;
		case 5 : {
			//	str = "GoldBox";
			strRes = KoStorage.GetKorString("18514");
		}break;
		case 6 : {
			//str = "Cube";
			strRes = KoStorage.GetKorString("75002");
		}break;
		default:
			strRes = KoStorage.GetKorString("71001");
			break;
		} */
		//string str = string.Format(KoStorage.GetKorString("72333"),strRes, resValue);
		string str = string.Format(KoStorage.GetKorString("72333"));
		ChangeContentNoCheckOKayString(KoStorage.GetKorString("72310"),	KoStorage.GetKorString("71000"),str);
		if(UserDataManager.instance._subStatus != null) UserDataManager.instance._subStatus = null;
		UserDataManager.instance._subStatus = ()=>{
			Invoke("OnOkClick",0.1f);
			return true;
		};
	}
	
}
