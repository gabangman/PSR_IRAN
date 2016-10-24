using UnityEngine;
using System.Collections;

public class InvenButton : MonoBehaviour {
	public GameObject[] Btns;
	// Use this for initialization


	void OnEnable(){
		bool bNew = false;
		bNew = myAcc.instance.account.bInvenBTN[0];
		Btns[0].SetActive(bNew);
		if(bNew) myAcc.instance.account.bInvenBTN[0] = false;

		bool bNew1 = myAcc.instance.account.bInvenBTN[1];
		Btns[1].SetActive(bNew1);

		bool bNew2 = myAcc.instance.account.bInvenBTN[2];
		Btns[2].SetActive(bNew2);

		bool bNew3 = myAcc.instance.account.bInvenBTN[3];
		Btns[3].SetActive(bNew3);
	
	}

	public  void ReSetSubMenuBTN(){
		this.OnEnable();
	}

}
