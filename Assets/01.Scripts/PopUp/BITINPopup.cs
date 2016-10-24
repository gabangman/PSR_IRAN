using UnityEngine;
using System.Collections;

public class BITINPopup : basePopup {

	void Start(){

	}
	private string emailTo = string.Empty;
	public void InitPopUp(){
		string[] eText = new string[3];
		if(KoStorage.kostroage == null) {
			eText[0] = "Undergoing server maintenance";
			eText[1] ="CONFIRM";
			eText[2] = string.Format("The game is undergoing maintenance\nPlease connect again later.");
		}else{
			eText[0] =KoStorage.GetKorString("71104");// "서버 점검 중";
			eText[1] = KoStorage.GetKorString("71000");
			eText[2] = KoStorage.GetKorString("71103");
		}

		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = eText[0];//"게임 탈퇴";//KoStorage.getStringDic("60234");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =eText[1];// "탈퇴하기";
		pop.FindChild("lbName").GetComponent<UILabel>().text = eText[2] ;
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);

		pop.FindChild("btnMail").GetComponent<UIButtonMessage>().target = gameObject;
		pop.FindChild("btnMail").GetComponent<UIButtonMessage>().functionName = "SendEmail";
		try{
			pop.FindChild("btnMail").FindChild("lbOk").GetComponent<UILabel>().text =KoStorage.GetKorString("72509");
			
		}catch (System.Exception e){
			pop.FindChild("btnMail").FindChild("lbOk").GetComponent<UILabel>().text ="E-Mail";
		}
		
		pop.FindChild("btnok").GetComponent<UIButtonMessage>().functionName = "OnQuit";
		emailTo ="gabangman01@gmail.com";

	}

	public void InitNetworkPopUp(){
		string[] eText = new string[3];
		if(KoStorage.kostroage == null) {
			eText[0] = "Connect failed";
			eText[1] ="CONFIRM";
			eText[2] = string.Format("[009cff]CONNECT FAILED[-] \n RESTART THE GAME");
		}else{
			eText[0] = KoStorage.GetKorString("71105");// "연결 실패";
			eText[1] = KoStorage.GetKorString("71000");
			eText[2] = KoStorage.GetKorString("72314");
		}
		ChangeContentNoCheckOKayString(eText[0], eText[1], eText[2]);
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("btnMail").GetComponent<UIButtonMessage>().target = gameObject;
		pop.FindChild("btnMail").GetComponent<UIButtonMessage>().functionName = "SendEmail";
		try{
			pop.FindChild("btnMail").FindChild("lbOk").GetComponent<UILabel>().text =KoStorage.GetKorString("72509");
			
		}catch (System.Exception e){
			pop.FindChild("btnMail").FindChild("lbOk").GetComponent<UILabel>().text ="E-Mail";
		}

		pop.FindChild("btnok").GetComponent<UIButtonMessage>().functionName = "OnQuit";
		emailTo = "gabangman01@gmail.com";
	}



	public void InitPopUpEmergency(){
		string[] eText = new string[3];
		if(KoStorage.kostroage == null) {
		//	eText[0] = "Connect failed";
		//	eText[1] ="CONFIRM";
		//	eText[2] = string.Format("[009cff]CONNECT FAILED[-] \n RESTART THE GAME");
		}else{
			eText[0] =KoStorage.GetKorString("71104");
			eText[1] = KoStorage.GetKorString("71000");
			eText[2] = KoStorage.GetKorString("71120");
		}
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = eText[0];
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =eText[1];
		pop.FindChild("lbName").GetComponent<UILabel>().text = eText[2] ;
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);

	}

	void OnQuit(){
		Vibration.OnNoticQuit();
	}

	void SendEmail ()
	{		
		////	#if UNITY_ANDROID && !UNITY_EDITOR
		//string email ="gabangmancs@gmail.com";// GV.gInfo.strEmail;
		string email = emailTo;
		if(string.IsNullOrEmpty(email) == true){
			email = "gabangmancs@gmail.com";
		}
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		string str = null;
		try{
			str = KoStorage.GetKorString("72530");
		}catch(System.Exception e){
			str ="";
		}
		
		
		sb.Append(str);
		sb.Append("\r\n ");
		string subject = EmailURL(sb.ToString());
		sb.Length = 0;
		try{
			str = KoStorage.GetKorString("72531");
		}catch(System.Exception e){
			str = "";
			
		}
		sb.Append(str);
		sb.Append("\r\n\n\n\n ");
		try{
			str = KoStorage.GetKorString("72532");
		}catch(System.Exception e){
			str = "";
		}
		sb.Append(str);
		sb.Append("\r\n ");
		sb.Append("1.3.6 =========================================\r\n ");
		System.DateTime dateTime =System.DateTime.Now;
		sb.Append("Current Time : ");sb.Append(dateTime.ToString());sb.Append("\r\n");
		sb.Append("Device Model  : ");sb.Append(Global.gDeviceModel);sb.Append("\r\n");
		sb.Append("API Name : ");sb.Append(GV.mAPI);sb.Append("\r\n");
		string str0 = GV.mException+"_"+GV.responsStatus.ToString();
		sb.Append("API Exception : ");sb.Append(str0);sb.Append("\r\n");
		sb.Append("API Country : ");sb.Append(Global.gCountryCode);sb.Append("\r\n");
		sb.Append("API Version : ");sb.Append(Global.gVersion);sb.Append("\r\n");
		sb.Append("API ID : ");sb.Append(GV.UserRevId);sb.Append("\r\n");
		string body = EmailURL(sb.ToString());
		Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}

	public void BlockInitPopUp(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		
		pop.FindChild("lbName").GetComponent<UILabel>().text =string.Format(KoStorage.GetKorString("71102"),GV.UserRevId,GV.gInfo.strEmail);
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = string.Empty;
		pop.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("71014");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("btnok").FindChild("lbOk").GetComponent<UILabel>().text  =  KoStorage.GetKorString("71000");
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		pop.FindChild("lbName").GetComponent<UILabel>().maxLineCount = 5;

		pop.FindChild("btnMail").GetComponent<UIButtonMessage>().target = gameObject;
		pop.FindChild("btnMail").GetComponent<UIButtonMessage>().functionName = "SendEmail";
		try{
			pop.FindChild("btnMail").FindChild("lbOk").GetComponent<UILabel>().text =KoStorage.GetKorString("72509");
			
		}catch (System.Exception e){
			pop.FindChild("btnMail").FindChild("lbOk").GetComponent<UILabel>().text ="E-Mail";
		}
		
		pop.FindChild("btnok").GetComponent<UIButtonMessage>().functionName = "OnQuit";
		emailTo = "gabangmancs@gmail.com";
	}

	
	string EmailURL (string url)
	{
		return WWW.EscapeURL(url).Replace("+","%20");
	}


	public void OnRootPopup(){
		string[] eText = new string[3];
		eText[0] = KoStorage.GetKorString("71202");// "연결 실패";
		eText[1] = KoStorage.GetKorString("71000");
		eText[2] = KoStorage.GetKorString("72818");
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbText").GetComponent<UILabel>().text = eText[0];//
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =eText[1];//
		pop.FindChild("lbName").GetComponent<UILabel>().text = eText[2] ;
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
	}


	public void DeleteInitPopUp(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		pop.FindChild("lbName").GetComponent<UILabel>().text ="게임데이터를 삭제하오니 재 실행 해주세요";//string.Format(KoStorage.GetKorString("71102")+"\n UserId : {1}",GV.gInfo.strEmail,GV.UserRevId);
		pop.FindChild("lbPrice").GetComponent<UILabel>().text = string.Empty;
		pop.FindChild("lbText").GetComponent<UILabel>().text = KoStorage.GetKorString("71014");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("btnok").FindChild("lbOk").GetComponent<UILabel>().text  =  KoStorage.GetKorString("71000");
		pop.FindChild("Sprite (Check_V)").gameObject.SetActive(false);
		pop.FindChild("lbName").GetComponent<UILabel>().maxLineCount = 3;
		pop.FindChild("btnMail").GetComponent<UIButtonMessage>().target = gameObject;
		pop.FindChild("btnMail").GetComponent<UIButtonMessage>().functionName = "SendEmail";
		try{
			pop.FindChild("btnMail").FindChild("lbOk").GetComponent<UILabel>().text =KoStorage.GetKorString("72509");
			
		}catch (System.Exception e){
			pop.FindChild("btnMail").FindChild("lbOk").GetComponent<UILabel>().text ="E-Mail";
		}
		
		pop.FindChild("btnok").GetComponent<UIButtonMessage>().functionName = "OnQuit";
		emailTo = "gabangmancs@gmail.com";
	}


}
