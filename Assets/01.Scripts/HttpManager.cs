using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
public class HttpManager : MonoBehaviour {

	public enum EType {
		GET,
		POST,
		PUT,
		DELETE
	};
	
	[SerializeField] public EType _type = EType.GET;




	public void MyHTTPGet(){
		string url = "http://127.0.0.1:3000/method_get_test/users";
		WWW www = new WWW (url);
		StartCoroutine(WaitForRequest(www));
	}

	public void MyHTTPPost(){
		string url = "http://127.0.0.1:3000/method_post_test/user";
		WWWForm form = new WWWForm();
		form.AddField("id", "8");
		form.AddField("name", "brian8");
		WWW www = new WWW (url, form);
		StartCoroutine(WaitForRequest(www));
	}

	public void MyHTTPPut(){
		string url = "http://127.0.0.1:3000/method_put_test/user/id/8/ddddd";
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
		httpWebRequest.ContentType = "text/json";
		httpWebRequest.Method = "PUT";
		
		HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
		{
			string responseText = streamReader.ReadToEnd();
			//Now you have your response.
			//or false depending on information in the response
			Utility.Log(responseText);
		}   
	}

	public void MyHTTPDelete(){
		string url = "http://127.0.0.1:3000/method_del_test/user/id/8";
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
		httpWebRequest.ContentType = "text/json";
		httpWebRequest.Method = "DELETE";
		
		HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
		{
			string responseText = streamReader.ReadToEnd();
			//Now you have your response.
			//or false depending on information in the response
			Utility.Log(responseText);
		}   
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
		if (www.error == null)
		{
			// request completed!
			Utility.Log (www.text);
		}
		else
		{
			// something wrong!
			Utility.Log ("WWW error: " + www.error);
		}
	}
}
