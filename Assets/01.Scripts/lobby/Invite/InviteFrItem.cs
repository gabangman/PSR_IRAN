using UnityEngine;
using System.Collections;

public class InviteFrItem : MonoBehaviour {

	void OnEnable(){
	//	transform.FindChild("odd").FindChild("Select").GetChild(0).GetComponent<UICheckbox>().SetCheck(true);// = true;
	//	transform.FindChild("even").FindChild("Select").GetChild(0).GetComponent<UICheckbox>().SetCheck(true);//isChecked = true;
	//	StopCoroutine("FBPicLoad_1");StopCoroutine("FBPicLoad_2");
	}

	int myOdd =-1; int myEven=-1;

	public void CheckStageChange(bool b){
		transform.FindChild("odd").FindChild("Select").GetChild(0).GetComponent<UICheckbox>().SetCheck(b);
		transform.FindChild("even").FindChild("Select").GetChild(0).GetComponent<UICheckbox>().SetCheck(b);
	//	if( myOdd >= 0)
	//		SNSManager.facebookinvite[myOdd].isCheck = b;
	//	if(myEven >= 0) 
	//		SNSManager.facebookinvite[myEven].isCheck = b;
	}

	void OnCheckEven(bool isCheck){
	//	Utility.LogWarning("OnCheckEven " + 
		if( myEven >= 0)
			SNSManager.facebookinvite[myEven].isCheck = isCheck;
		if(!isCheck){
			NGUITools.FindInParents<InviteGMenu>(gameObject).SetSelectBox(false);
			//Utility.LogWarning("OnCheckEven " + transform.name);
		}
	}
	
	void OnCheckOdd(bool isCheck){
		if(myOdd >= 0) 
			SNSManager.facebookinvite[myOdd].isCheck = isCheck;
		if(!isCheck){
			NGUITools.FindInParents<InviteGMenu>(gameObject).SetSelectBox(false);
			//Utility.LogWarning("OnCheckOdd " + transform.name);
		}
	}

	public void InitInviteBox(int myID){
		int oddIndex = myID*2;
		myOdd = oddIndex;
		int cnt = SNSManager.facebookinvite.Count;
		var odd =	transform.FindChild("odd") as Transform;
		StopCoroutine("FBPicLoad");
		int[] picIndex = new int[2];
		if(cnt <= oddIndex) {
			odd.gameObject.SetActive(false);	
			myOdd =-1;
		}else{
			SetFBSlot(odd, oddIndex);
			bool b = SNSManager.facebookinvite[oddIndex].issihouett;
			if(!b){
				Texture2D Img = SNSManager.facebookinvite[oddIndex].FBImg;
				if(Img == null){
					odd.FindChild("info").FindChild("SNSpic").GetComponent<UITexture>().mainTexture 
						= (Texture2D)GV.DefaultProfileTexture;
					//StartCoroutine("FBPicLoad_1", oddIndex);
					picIndex[0] = oddIndex;
				}else{
					odd.FindChild("info").FindChild("SNSpic").GetComponent<UITexture>().mainTexture 
						= Img;
					picIndex[0] = -1;
				}
			}else{
			
				odd.FindChild("info").FindChild("SNSpic").GetComponent<UITexture>().mainTexture 
					=(Texture2D)GV.DefaultProfileTexture;
				picIndex[0] = -1;
			}
		
		}

		int evenIndex = myID*2+1;
		myEven = evenIndex;
		var even =	transform.FindChild("even") as Transform;
		if(cnt <= evenIndex) {
			even.gameObject.SetActive(false);	
			myEven =-1;
		}else{
			SetFBSlot(even, evenIndex);
			bool b = SNSManager.facebookinvite[evenIndex].issihouett;
			if(!b){
				Texture2D Img = SNSManager.facebookinvite[evenIndex].FBImg;
				if(Img == null){
					even.FindChild("info").FindChild("SNSpic").GetComponent<UITexture>().mainTexture 
						= (Texture2D)GV.DefaultProfileTexture;
					picIndex[1] = evenIndex;
				}else{
					even.FindChild("info").FindChild("SNSpic").GetComponent<UITexture>().mainTexture 
						= Img;
					picIndex[1] = -1;
				}
			}else{
				even.FindChild("info").FindChild("SNSpic").GetComponent<UITexture>().mainTexture 
					=  (Texture2D)GV.DefaultProfileTexture;
				picIndex[1] = -1;
			}
		}

		StartCoroutine("FBPicLoad", picIndex);

	}

	private void SetFBSlot(Transform tr, int idx){
		tr.gameObject.SetActive(true);
		tr.FindChild("info").FindChild("lbNick").GetComponent<UILabel>().text 
			= SNSManager.facebookinvite[idx].FBName;
		bool b1=	SNSManager.facebookinvite[idx].isCheck;
		tr.FindChild("Select").GetChild(0).GetComponent<UICheckbox>().SetCheck(b1);
	}

	IEnumerator FBPicLoad(int[] idx){
		yield return StartCoroutine("FBPicLoad_1",idx[0]);
		yield return StartCoroutine("FBPicLoad_2",idx[1]);
	}



	IEnumerator FBPicLoad_1(int idx){
		if(idx < 0) yield break;
		string url = SNSManager.facebookinvite[idx].FBUrl;
		WWW www = new WWW( url );
	
		yield return www;
		
		if( this == null )
			yield break;
		
		if( www.error != null )
		{
			//Utility.Log( "load failed" );
		}
		else
		{
			SNSManager.facebookinvite[idx].FBImg = www.texture;
			transform.FindChild("odd").FindChild("info").FindChild("SNSpic").GetComponent<UITexture>().mainTexture
				= www.texture;//as Transform;
			www.Dispose();	www = null;
		}
	}
	
	IEnumerator FBPicLoad_2(int idx){
		if(idx < 0) yield break;
		string url = SNSManager.facebookinvite[idx].FBUrl;
		WWW www = new WWW( url );
		yield return www;
		if( this == null )
			yield break;
		
		if( www.error != null )
		{
			//Utility.Log( "load failed" );
		}
		else
		{
			SNSManager.facebookinvite[idx].FBImg = www.texture;
			transform.FindChild("even").FindChild("info").FindChild("SNSpic").GetComponent<UITexture>().mainTexture
				= www.texture;//as Transform;
			www.Dispose();
			www = null;
		}
	}//초대거부

}
