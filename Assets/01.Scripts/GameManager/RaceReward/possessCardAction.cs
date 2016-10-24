using UnityEngine;
using System.Collections;

public class possessCardAction : MonoBehaviour {

	public void PlayTheCardAnimation(){
		var card1 = transform.FindChild("Slot_1") as Transform;
		var card2 = transform.FindChild("Slot_2") as Transform;
		var card3 = transform.FindChild("Slot_3") as Transform;

		var card1tween = card1.gameObject.AddComponent<TweenPosition>() as TweenPosition;
		card1tween.to = card2.localPosition;
		card1tween.from = card1.localPosition;
		Vector3 initPos =  card1.localPosition;
		card1tween.duration = 0.2f;
		card1tween.onFinished = delegate(UITweener tween) {
			var temp = tween.transform.gameObject;
			Destroy(tween);
			var cTw  = temp.AddComponent<TweenPosition>() as TweenPosition;
			cTw.to = initPos;
			cTw.from = card2.localPosition;
			cTw.duration = 0.2f;
			cTw.delay = 0.2f;
			cTw.Reset();
			cTw.enabled = true;
		};
		card1tween.Reset();
		card1tween.enabled = true;

		Vector3 initPos1 =  card3.localPosition;
		var card3tween = card3.gameObject.AddComponent<TweenPosition>() as TweenPosition;
		card3tween.to = card2.localPosition;
		card3tween.from = card3.localPosition;
		card3tween.duration = 0.2f;
		card3tween.onFinished = delegate(UITweener tween) {
			var temp = tween.transform.gameObject;
			Destroy(tween);
			var cTw  = temp.AddComponent<TweenPosition>() as TweenPosition;
			cTw.to = initPos1;
			cTw.from = card2.localPosition;
			cTw.duration = 0.2f;
			cTw.delay = 0.2f;
			cTw.onFinished = delegate {
				//int cnt = transform.childCount;
				for(int i =0; i < (4);i++){
					if(string.Equals("LevelUp", transform.GetChild(i).name) == false)
						transform.GetChild(i).GetComponent<UIButtonMessage>().functionName
						= "OnSlotCard";
				}
			};
			cTw.Reset();
			cTw.enabled = true;
		};
		card3tween.Reset();
		card3tween.enabled = true;
	}

	private Transform SelectTrans;
	private GameObject selectSlot;
	private int selectCard;
	private int[] t_cardNum = new int[3]{0,0,0};
	public void PlaySelectCardAnimation(GameObject selectSlot, int selectCard, int mCnt){
			SelectTrans = selectSlot.transform;
			////!!--Utility.Log(selectCard + " select card id ");
			this.selectCard = selectCard;
			t_cardNum[mCnt] = selectCard;
			this.selectSlot = selectSlot;
			StartCoroutine("CardScale", mCnt);
	}


	IEnumerator CardScale(int mCnt){
		TweenScale[] scales = SelectTrans.GetComponents<TweenScale>();
		scales[0].enabled = true;
		scales[1].onFinished = delegate(UITweener tween) {
			selectSlot.transform.FindChild("Mat_Q").gameObject.SetActive(false);
			var card2 = selectSlot.transform.FindChild("MyCard") as Transform;
			card2.gameObject.SetActive(true);
			////!!--Utility.Log(selectCard + " select card id CardScale");
			ChangeCardContent(selectCard, card2);
			var tr = transform.FindChild("LevelUp") as Transform;
			tr.localPosition = SelectTrans.localPosition;
			tr.gameObject.SetActive(true);
			GameManager.instance.CardSelectSound();
			transform.FindChild(selectSlot.name+"_Effect").gameObject.SetActive(true);
			if(mCnt == 0) NewRemainCardView();
		};
		scales[1].enabled = true;
		
		yield return null;
	}

	void ChangeCardContent(int cardid, Transform tr){
		Common_Material.Item _item = Common_Material.Get(cardid);
		tr.FindChild("Mat_M").GetComponent<UISprite>().spriteName = cardid.ToString();
		tr.FindChild("MatName").GetComponent<UILabel>().text = _item.Name;
		GV.UpdateMatCount(cardid,1);
	}
	void NewRemainCardView(){
		var card1 = transform.FindChild("Slot_1") as Transform;
		var card2 = transform.FindChild("Slot_2") as Transform;
		var card3 = transform.FindChild("Slot_3") as Transform;

	
		addCardNumber();
		int count = Global.gRaceInfo.rewardMatCount;
	
		var tw = card1.GetComponent<UIButtonMessage>() as UIButtonMessage;
		if(string.IsNullOrEmpty(tw.functionName) == false){
			StartCoroutine(CardSpins(card1, t_cardNum[count]));
			//!!--Utility.Log("reamin card1 "  + t_cardNum[count] +"+== " + count);
			count++;
		}
		tw = card2.GetComponent<UIButtonMessage>() as UIButtonMessage;
		if(string.IsNullOrEmpty(tw.functionName) == false){
			StartCoroutine(CardSpins(card2, t_cardNum[count]));
			//!!--Utility.Log("reamincard2 " + t_cardNum[count] + " +== " + count);
			count++;
		}
		tw = card3.GetComponent<UIButtonMessage>() as UIButtonMessage;
		if(string.IsNullOrEmpty(tw.functionName) == false){
			StartCoroutine(CardSpins(card3, t_cardNum[count]));
			//!!--Utility.Log("reamin card3 " + t_cardNum[count] + " +== " + count);
			count++;
		}
	}

	void remainCardView(){
		var card1 = transform.FindChild("Slot_1") as Transform;
		var card2 = transform.FindChild("Slot_2") as Transform;
		var card3 = transform.FindChild("Slot_3") as Transform;
		int count = 1;
		addCardNumber();
		if(card1.name != selectSlot.name){
			StartCoroutine(CardSpins(card1, t_cardNum[count]));
			count++;
		}
		if(card2.name != selectSlot.name){
			StartCoroutine(CardSpins(card2, t_cardNum[count]));
			count++;
		}
		if(card3.name != selectSlot.name){
			StartCoroutine(CardSpins(card3, t_cardNum[count]));
			count++;
		}
		//Utility.Log("featrued card " + count);
	}


	IEnumerator CardSpins(Transform cardslot, int cNum){
		TweenScale[] scales = cardslot.GetComponents<TweenScale>();
		scales[0].enabled = true;
		scales[1].onFinished = delegate(UITweener tween) {
			cardslot.transform.FindChild("Mat_Q").gameObject.SetActive(false);
			cardslot.transform.FindChild("Select").gameObject.SetActive(false);
			var card2 = cardslot.transform.FindChild("MyCard") as Transform;
			card2.gameObject.SetActive(true);
			ChangeCardContent(cNum, card2);

			transform.parent.FindChild("BtnOK").gameObject.SetActive(true);
		};
		scales[1].enabled = true;
		yield return null;
	}

	void addCardNumber(){
	
		int[] arr = gameObject.AddComponent<RandomCreate>().CreateRandomValue(20);
		int[] cardArr = new int[100];
		for(int i =0; i < cardArr.Length;i++){
			cardArr[i] = arr[Random.Range(0,20)];
		}

		int numCnt = Global.gRaceInfo.rewardMatCount;
		int cardnum = 8599;
	//	cardnum += cardArr[Random.Range(0,100)];

		if(numCnt == 2){
			for(int i = 0; i < 100; i++){
				cardnum = cardArr[i]+8599;
				if(cardnum != t_cardNum[0] && cardnum != t_cardNum[1]){
					t_cardNum[2] = cardnum;
					break;
				}
			}

			if(t_cardNum[2] >= 8620 || t_cardNum[2] < 8600 || t_cardNum[2] == 0)  t_cardNum[2] = 8617;

		}else{
			for(int i = 0; i < 100; i++){
				cardnum = cardArr[i]+8599;
				if(cardnum != t_cardNum[0]){
					t_cardNum[1] = cardnum;
					break;
				}
			}
		//	if(t_cardNum[1] == 0) t_cardNum[1] = 8618;
			if(t_cardNum[1] >= 8620 || t_cardNum[1] < 8600 || t_cardNum[1] == 0)  t_cardNum[1] = 8618;
			for(int i = 0; i < 100; i++){
				cardnum = cardArr[i]+8599;
				if(cardnum != t_cardNum[0] && cardnum != t_cardNum[1]){
					t_cardNum[2] = cardnum;
					break;
				}
			}
		//	if(t_cardNum[2] == 0) t_cardNum[2] = 8619;
			if(t_cardNum[2] >= 8620 || t_cardNum[2] < 8600 || t_cardNum[2] == 0)  t_cardNum[2] = 8619;
		}


	}

/*
	void Update(){
		if(!isClick) return;
		ApplyDelta(Time.deltaTime);
	}

	void ApplyDelta (float delta)
	{
		delta *= Mathf.Rad2Deg * Mathf.PI * 2f;
		Quaternion offset = Quaternion.Euler(rotationsPerSecond * delta);
		SelectTrans.rotation = SelectTrans.rotation * offset;
	}
	*/
}
