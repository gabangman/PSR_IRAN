using UnityEngine;
using System.Collections;

public class UpgradeMatDetail : MonoBehaviour {
	readonly int MAXCOIN = 10;
	private int MaxQubeCnt = 7;


	public void InitSubContent(int cnt){
		string str = "btn";
		this.coinCnt = MAXCOIN-cnt;
		if(cnt == MAXCOIN){
			transform.FindChild(str).gameObject.SetActive(false);
			transform.FindChild("SetReady").gameObject.SetActive(true);
		}else{
			var temp = transform.FindChild(str).gameObject as GameObject;
			temp.SetActive(true);
			temp.transform.FindChild("lbAmount").GetComponent<UILabel>().text 
				= string.Format("{0}", (coinCnt));
			transform.FindChild("SetReady").gameObject.SetActive(false);
		}
	}
	
	public void InitMatItemContent(int lackMat, int Max, int MatID, int changecoin){
		transform.FindChild("Icon_Mat").GetComponent<UISprite>().spriteName = MatID.ToString();
		
		string str = "btn";
		this.coinCnt = (Max-lackMat) * changecoin;
		if(lackMat >= Max){
			transform.FindChild(str).gameObject.SetActive(false);
			transform.FindChild("SetReady").gameObject.SetActive(true);
			transform.FindChild("SetReady").GetComponentInChildren<UIButtonMessage>().functionName = null;
		}else{
			var temp = transform.FindChild(str).gameObject as GameObject;
			temp.SetActive(true);
			temp.transform.FindChild("lbAmount").GetComponent<UILabel>().text 
				= string.Format("{0}", coinCnt);
			transform.FindChild("SetReady").FindChild("Check").GetComponent<UIButtonMessage>().functionName = "OnDwDeck";
			transform.FindChild("SetReady").gameObject.SetActive(false);
		}
		
		
	}
	
	public void InitQubeItemContent(int myQube, int MatID, int Qubecoin, int plusSuccess){
		
		QubeCnt = 0; this.QubeCoin =Qubecoin;
		myQubeCnt = myQube;
		var tr = transform.FindChild("QubeBtn") as Transform;
		tr.gameObject.SetActive(true);
		var obj = tr.FindChild("btn_type_R").gameObject as GameObject;
		if(!obj.activeSelf) obj.SetActive(true);
		tr.FindChild("lbQuantity").GetComponent<UILabel>().text = string.Format("{0}/{1}",QubeCnt, myQubeCnt);
		transform.FindChild("SetReady").gameObject.SetActive(false);	
		transform.FindChild("Icon_Mat").GetComponent<UISprite>().spriteName = MatID.ToString();
		if(plusSuccess != 0){
			MaxQubeCnt = 7 - (plusSuccess /10);
		}else MaxQubeCnt = 7;
	}
	
	private int coinCnt;
	private int QubeCnt, myQubeCnt;
	
	void OnMatDeck(){
		Utility.LogWarning("OnMatDect");
		//	string str = gameObject.name;
		//	NGUITools.FindInParents<MechanicMake>(gameObject).ClickMatCoin(coinCnt);
		//	transform.FindChild( "btn").gameObject.SetActive(false);
		//	transform.FindChild("SetReady").gameObject.SetActive(true);	
		//	NGUITools.FindInParents<MechanicMake>(gameObject).DeckMatFullCount(gameObject);
	}
	
	
	void OnUpDeck(){
		string str = gameObject.name;
		NGUITools.FindInParents<StarUpgrade>(gameObject).ClickMatCoin(coinCnt);
		transform.FindChild( "btn").gameObject.SetActive(false);
		transform.FindChild("SetReady").gameObject.SetActive(true);	
		NGUITools.FindInParents<StarUpgrade>(gameObject).DeckMatFullCount(gameObject);
	}
	
	void OnDwDeck(){
		//	string str = gameObject.name;
		NGUITools.FindInParents<StarUpgrade>(gameObject).ClickMatCoinMinus(coinCnt);
		transform.FindChild( "btn").gameObject.SetActive(true);
		transform.FindChild("SetReady").gameObject.SetActive(false);	
		NGUITools.FindInParents<StarUpgrade>(gameObject).DeckMatFullCountMinus(gameObject);
	}
	int QubeCoin;
	void OnUpQube(){
		int qcoin = MaxQubeCnt - QubeCnt;
		transform.FindChild("SetReady").gameObject.SetActive(true);	
		NGUITools.FindInParents<StarUpgrade>(gameObject).ClickQubeCoin(qcoin*QubeCoin,qcoin);
		transform.FindChild("QubeBtn").gameObject.SetActive(false);
	}
	void OnDwQube(){
		transform.FindChild("SetReady").gameObject.SetActive(false);	
		NGUITools.FindInParents<StarUpgrade>(gameObject).ClickQubeCoinMinus(QubeCnt);
		transform.FindChild("QubeBtn").gameObject.SetActive(true);
	}
	
	void OnQubeL(){
		if(myQubeCnt == 0) return;
		if(QubeCnt <= 0) return;
		QubeCnt--;
		NGUITools.FindInParents<StarUpgrade>(gameObject).ClickMinusGage(QubeCoin,(QubeCnt));
		var tr = transform.FindChild("QubeBtn") as Transform;
		tr.FindChild("lbQuantity").GetComponent<UILabel>().text = string.Format("{0}/{1}", QubeCnt, myQubeCnt);// QubeCnt.ToString();
		var obj = tr.FindChild("btn_type_R").gameObject as GameObject;
		if(obj.activeSelf) return;
		obj.SetActive(true);
	}
	
	void OnQubeR(){
		if(myQubeCnt == 0 || myQubeCnt == QubeCnt || QubeCnt > MaxQubeCnt) {
			if(myQubeCnt == 0 || QubeCnt == 0) return;
			var tr1 = transform.FindChild("QubeBtn") as Transform;
			tr1.FindChild("btn_type_R").gameObject.SetActive(false);
			return;
		}
		QubeCnt++;
		int cnt = MaxQubeCnt - QubeCnt;
		NGUITools.FindInParents<StarUpgrade>(gameObject).ClickPlusGage(QubeCoin, QubeCnt);
		CheckQubeCount();
		var tr = transform.FindChild("QubeBtn") as Transform;
		tr.FindChild("lbQuantity").GetComponent<UILabel>().text = string.Format("{0}/{1}", QubeCnt, myQubeCnt);
		if(cnt == 0){
			tr.FindChild("btn_type_R").gameObject.SetActive(false);
		}
		
	}
	
	void CheckQubeCount(){
		
	}
}
