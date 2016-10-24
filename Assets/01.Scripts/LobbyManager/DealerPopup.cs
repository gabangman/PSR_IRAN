using UnityEngine;
using System.Collections;

public class DealerPopup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetInit(){
		transform.FindChild("BG").FindChild("lb_text").GetComponent<UILabel>().text = KoStorage.GetKorString("74033");
		gameObject.SetActive(true);
		GetComponent<Animation>().Play("Dealer_recomend_1");

	}

	IEnumerator StartAnim(){
		yield return new WaitForSeconds(1.5f);
		GetComponent<Animation>().Play("Dealer_recomend_1");
		yield return null;
	}

	protected void OnMove(){
		GameObject.Find("LobbyUI").SendMessage("OnOpenDealer", SendMessageOptions.DontRequireReceiver);
		gameObject.SetActive(false);
		EncryptedPlayerPrefs.SetInt("DealerBuy", 1);
		//myAcc.instance.account.bLobbyBTN[4] = true;
	}

	protected void OnCancle(){
		GV.bRaceLose = false;
		GameObject.Find("LobbyUI").SendMessage("PopUpEnd");
		EncryptedPlayerPrefs.SetInt("DealerBuy", 0);
		gameObject.SetActive(false);
	}
}
