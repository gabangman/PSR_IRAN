using UnityEngine;
using System.Collections;
using System.Text;
public class NetworkEmail : MonoBehaviour {


	public void SetTailInit(){
		transform.FindChild("btnMail").gameObject.SetActive(false);
		try{
			transform.FindChild("lb_warning").GetComponent<UILabel>().text  = string.Format("mTailCount {0} / mRequestRange {1}", NetworkManager.instance.mTailCount, NetworkManager.instance.mRequestRange);
		}catch(System.Exception e){
			
		}


	
	}
	public void SetInit(){
	
	
		if(GV.bLoding){
			transform.FindChild("btnMail").gameObject.SetActive(false);
			transform.FindChild("lb_warning").GetComponent<UILabel>().text = string.Empty;
		}else{
			transform.FindChild("btnMail").gameObject.SetActive(false);
			Invoke("CheckPopUP", 5f);
			int n = UnityEngine.Random.Range(0,7) + 71122;
			try{
				transform.FindChild("lb_warning").GetComponent<UILabel>().text  =KoStorage.GetKorString(n.ToString());
			}catch(System.Exception e){
			
			}
		}
	}

	void SendEmail ()
	{		
		////	#if UNITY_ANDROID && !UNITY_EDITOR
		//string email ="gabangmancs@gmail.com";// GV.gInfo.strEmail;
		string email = "gabangman01@gmail.com";
		StringBuilder sb = new StringBuilder();
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
		sb.Append("2.3.6========================================\r\n ");
		System.DateTime dateTime =System.DateTime.Now;
		sb.Append("Current Time : ");sb.Append(dateTime.ToString());sb.Append("\r\n");
		sb.Append("Device Model  : ");sb.Append(Global.gDeviceModel);sb.Append("\r\n");
		sb.Append("OS Version : ");sb.Append(Global.gOsVersion);sb.Append("\r\n");
		sb.Append("API Name : ");sb.Append(GV.mAPI);sb.Append("\r\n");
		string str0 = GV.mException+"_"+GV.responsStatus.ToString();
		sb.Append("API Exception : ");sb.Append(str0);sb.Append("\r\n");
		//sb.Append("API Time : ");sb.Append(GV.mReTime.ToString());sb.Append("\r\n");
		sb.Append("API Country : ");sb.Append(Global.gCountryCode);sb.Append("\r\n");
		sb.Append("API Version : ");sb.Append(Global.gVersion);sb.Append("\r\n");
		sb.Append("API ID : ");sb.Append(GV.UserRevId);sb.Append("\r\n");
		string body = EmailURL(sb.ToString());
		Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}
	
	
	string EmailURL (string url)
	{
		return WWW.EscapeURL(url).Replace("+","%20");
	}

	void CheckPopUP(){
		
		transform.FindChild("btnMail").gameObject.SetActive(true);
		transform.FindChild("btnMail").GetComponent<UIButtonMessage>().target = gameObject;
		transform.FindChild("btnMail").GetComponent<UIButtonMessage>().functionName = "SendEmail";
		try{
			transform.FindChild("btnMail").FindChild("lbOk").GetComponent<UILabel>().text =KoStorage.GetKorString("72509");

		}catch (System.Exception e){
			//transform.FindChild("btnMail").FindChild("lbOk").GetComponent<UILabel>().text ="E-Mail";// KoStorage.GetKorString("72509");
		}

	}

	void OnDisable(){
		//transform.FindChild("btnMail").gameObject.SetActive(false);
	}


}
