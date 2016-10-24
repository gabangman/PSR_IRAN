using UnityEngine;
using System.Collections;

public class InvenCard : MonoBehaviour {

	public Transform Grid;
	public UILabel[] _lbText;
	void Awake(){

	}



	public void InitWin(){
		/*
		int count = Grid.childCount;
	//	int cnt = myAccount.instance.account.listCardInfo.Count;
		int isHaveCard = 0; 
		_lbText[0].text = string.Format("x{0}", Global.gCouponCount);
		for(int i = 0; i < count ; i++){
			Account.CardInfo _card =  myAccount.instance.account.listCardInfo[i];
			var obj = Grid.GetChild(i).gameObject as GameObject;
				if(_card.quantity == 0){
					obj.SetActive(false);
					obj.name = _card.cardId.ToString();
				}else{
					obj.SetActive(true);
					obj.name = _card.cardId.ToString();
					obj.transform.GetChild(0).FindChild("lbLevel").GetComponent<UILabel>().text 
					= string.Format("x{0}", _card.quantity);
				Common_Card.Item _item = Common_Card.Get(_card.cardId);
				obj.transform.GetChild(0).FindChild("lbName").GetComponent<UILabel>().text=
					_item.Name;
				obj.transform.GetChild(0).FindChild("image").GetComponent<UISprite>().spriteName =
					_card.cardId.ToString();
					isHaveCard++;
				obj.transform.GetChild(0).FindChild("lbPercent").GetComponent<UILabel>().text = 
					string.Format("{0}%", (int)(_item.Up*100));
				}
		}
		
		if(isHaveCard == 0){
			
		}

		Grid.GetComponent<UIGrid>().Reposition();*/
	}



}
