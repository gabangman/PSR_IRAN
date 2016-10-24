using UnityEngine;
using System.Collections;

public class matResultPopup : MonoBehaviour {

	System.Action<bool> onClick;

	public void OnClick(int coin, System.Action<bool> callback){
		this.onClick = callback;
		string price = coin.ToString();
		var popChild = gameObject.transform.FindChild("Content_BUY") as Transform;
		popChild.FindChild("btnCoin").gameObject.SetActive(true);
		popChild.FindChild("icon_product").gameObject.SetActive(false);//GetComponent<UISprite>().spriteName  =name[1]+"D";
		popChild.FindChild("lbPrice").GetComponent<UILabel>().text = price;
		price = "정말 제작할래?";
		popChild.transform.FindChild("lbName").gameObject.SetActive(true);
		popChild.FindChild("lbName").GetComponent<UILabel>().text = price;
		price = "부품 제작";
		popChild.FindChild("lbText").GetComponent<UILabel>().text = price;
	}

	void OnOkClick(){
		onClick(true);
		gameObject.SetActive(false);
		Destroy(this);
	
	
	}

	void OnCloseClick(){
		onClick(false);
		gameObject.SetActive(false);
		Destroy(this);
	}
}
