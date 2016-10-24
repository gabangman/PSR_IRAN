using UnityEngine;
using System.Collections;

public class stateUp : InterAction {

	public void InitBsTimeStatus(float currentValue, float NextValue){
		string sec = KoStorage.getStringDic("71005");
		if(currentValue <= 0 ){
			transform.FindChild("lbBsTime+").gameObject.SetActive(false);
			transform.FindChild("lbBsTime").gameObject.SetActive(true);
		}else{
			if(NextValue <= 0){
				transform.FindChild("lbBsTime+").gameObject.SetActive(false);

			}else{
				transform.FindChild("lbBsTime+").gameObject.SetActive(true);
				string str = string.Format("+{0:F1}", (NextValue-currentValue));
				//Utility.Log(str);
				str += sec;
				transform.FindChild("lbBsTime+").GetComponent<UILabel>().text = str;
			}
			transform.FindChild("lbBsTime").gameObject.SetActive(true);

			string str1 = string.Format("{0:F1}", currentValue);
			str1 += sec;
			transform.FindChild("lbBsTime").GetComponent<UILabel>().text = str1;
		}
		//_name.transform.FindChild("lbBsTime+").gameObject.SetActive(true);
		//_name.transform.FindChild("lbBsTime+").GetComponent<UILabel>().text = nextvalue.ToString()+" SEC";
		//_name.transform.FindChild("lbBsTime+").gameObject.SetActive(false);
	}

	public void InitStatus(float currentValue, float NextValue, float total, int max){

		 float x1 = (currentValue*max)/total;
		float y1 = (NextValue*max)/total;
		int x = (int)x1;
		int y  = (int)y1;
		int _value = (int)Mathf.Round(currentValue);
		int _next = (int)Mathf.Round(NextValue);

		gameObject.transform.FindChild("lbpresent").GetComponent<UILabel>().text = _value.ToString();
		int next =_value - _next;

		if(next <0){
			next = -next;
		}
		if(next == 0){
			gameObject.transform.FindChild("lbplus").GetComponent<UILabel>().text = " ";
			var tr1 = gameObject.transform.FindChild("boxplus") as Transform;
			tr1.localScale = new Vector3((float)y,tr1.localScale.y, tr1.localScale.z);

		}else {
			int[] arr = new int[] { x, y};
		//	t[0] = x;
		//	t[1] = y;
			if(currentValue - NextValue < 0) gameObject.transform.FindChild("lbplus").GetComponent<UILabel>().text = "+"+next.ToString();
			else gameObject.transform.FindChild("lbplus").GetComponent<UILabel>().text = "-"+next.ToString();
			StartCoroutine("StatusBarProgress",arr);
			
		}

		//StopCoroutine("StatusBarProgress");
		var tr = gameObject.transform.FindChild("boxpresent") as Transform;
		tr.localScale = new Vector3((float)x,tr.localScale.y, tr.localScale.z);	
		
	}



	IEnumerator StatusBarProgress(int[] arr){

		var tr = gameObject.transform.FindChild("boxplus") as Transform;
		float temp =(float)arr[0];
		float delta =-temp+(float)arr[1]; 
		delta = delta*0.1f;
		//Utility.Log (temp);
		for(;;){
			temp += delta;
			if( temp >= arr[1]){
				//temp = (float)y;
				tr.localScale = new Vector3((float)temp,tr.localScale.y, tr.localScale.z);
				//Utility.LogWarning("finish");
				break;
			}
			//Utility.Log ("progress " + temp);
			tr.localScale = new Vector3((float)temp,tr.localScale.y, tr.localScale.z);
			yield return null;

		}
	}



	public void ChangeStatus(float currentValue, float NextValue, float total, int max){
		
		float x1 = (currentValue*max)/total;
		float y1 = (NextValue*max)/total;
		int x = (int)x1;
		int y  = (int)y1;
		int _value = (int)Mathf.Round(currentValue);
		int _next = (int)Mathf.Round(NextValue);
		
		gameObject.transform.FindChild("lbpresent").GetComponent<UILabel>().text = _value.ToString();
		int next =_value - _next;
		
		if(next <0){
			next = -next;
		}
		if(next == 0){
			gameObject.transform.FindChild("lbplus").GetComponent<UILabel>().text = " ";
			var tr1 = gameObject.transform.FindChild("boxplus") as Transform;
			tr1.localScale = new Vector3((float)y,tr1.localScale.y, tr1.localScale.z);
			//Utility.Log ("next value " + next);
		}else {
			int[] arr = new int[] { x, y};
		
			if(currentValue - NextValue < 0) gameObject.transform.FindChild("lbplus").GetComponent<UILabel>().text = "+"+next.ToString();
			else gameObject.transform.FindChild("lbplus").GetComponent<UILabel>().text = "-"+next.ToString();
			StartCoroutine("StatusBarProgress",arr);
			
		}
		
		//StopCoroutine("StatusBarProgress");
		var tr = gameObject.transform.FindChild("boxpresent") as Transform;
		tr.localScale = new Vector3((float)x,tr.localScale.y, tr.localScale.z);	
		
	}

	public void ChangeBar(float value, float total){
	//	Utility.LogWarning("value " + value);
	//	Utility.LogWarning("t name " + transform.name);
		float curVal = value/total;
		Vector3 scale = transform.FindChild("bg").localScale;
		transform.FindChild("bar1").localScale = new Vector3(curVal*scale.x, scale.y, scale.z);
		transform.FindChild("lbStat1").GetComponent<UILabel>().text = string.Format("{0}", Mathf.Round(value));
	}

	public void ChangeBarDurability(float value, float total){
		float mVal = value;
		if(value <= 0 ) mVal = 1.0f;
		float curVal = mVal/total;
		Vector3 scale = transform.FindChild("bg").localScale;
		////!!--Utility.Log(curVal);
		if(curVal*100 <= 20){
			transform.FindChild("bar1").GetComponent<UISprite>().color = new Color32(255,0,0,255);
			transform.FindChild("img_Alert").gameObject.SetActive(true);
			var temp = GameObject.Find ("LobbyUI") as GameObject; 
			if(temp != null)
				temp.SendMessage("RankingBoardArrowChecking", true, SendMessageOptions.DontRequireReceiver);
		}else{
			transform.FindChild("bar1").GetComponent<UISprite>().color = new Color32(0,139,255,255);
			transform.FindChild("img_Alert").gameObject.SetActive(false);
			var temp = GameObject.Find ("LobbyUI") as GameObject; 
			if(temp != null)
				temp.SendMessage("RankingBoardArrowChecking", false,SendMessageOptions.DontRequireReceiver);
		}
		
		transform.FindChild("bar1").localScale = new Vector3(curVal*scale.x, scale.y, scale.z);
		if(value <= 0.001f  ) {
			curVal = 0.0f;
			mVal = 0;
		}
		transform.FindChild("lbStat1").GetComponent<UILabel>().text = string.Format("{0}%", (int)(curVal*100));
		transform.FindChild("lbStat2").GetComponent<UILabel>().text = string.Format("{0}/{1}", (int)mVal,(int)(total));
	}
	
	public void ChangeDashBoardBarDurability(float value, float total){
		float mVal = value;
		if(value <= 0 ) mVal = 1.0f;
		float curVal = mVal/total;
		Vector3 scale = transform.FindChild("bg").localScale;
		////!!--Utility.Log(curVal);
		if(curVal*100 <= 20){
			transform.FindChild("bar1").GetComponent<UISprite>().color = new Color32(255,0,0,255);
			transform.FindChild("img_Alert").gameObject.SetActive(true);
		}else{
			transform.FindChild("bar1").GetComponent<UISprite>().color = new Color32(0,139,255,255);
		}
		transform.FindChild("bar1").localScale = new Vector3(curVal*scale.x, scale.y, scale.z);
	}
}
