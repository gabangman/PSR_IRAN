using UnityEngine;
using System.Collections;

public class UIScrollListBase : MonoBehaviour {
	
	public UIListItem ListItem;
	int myId = -1;
	public void Init(int index){
		//Utility.Log(index);
	}
	/*void OnAccept(GameObject obj){
	//	string[] str = gameObject.name.Split('_');
	//	int _id = int.Parse(str[1]);
	//	Utility.Log(gameObject.name);
	//	gameRank.instance.postTestItem.RemoveAt(_id);
		//DestroyImmediate(gameObject);
	//	thiscallback();
	}*/

	public void AchievementList(int index){
		if(index == myId) return;
		myId = index;
		var temp = gameObject.GetComponent<AchievementItem>() as AchievementItem;
		if(temp == null) temp = gameObject.AddComponent<AchievementItem	>();
	}
	public void InitPostBox(int index, System.Action callback){
		if(index == myId) return;
		myId = index;
		var temp = gameObject.GetComponent<PostMessageItem>() as PostMessageItem;
		if(temp == null) temp = gameObject.AddComponent<PostMessageItem>();
		temp.ChangeGiftItems(myId);
		temp.thiscallback = callback;
	}

	public void InitGiftBox(int index , System.Action callback){
		if(index == myId) return;
		myId = index;
		var temp = gameObject.GetComponent<PostMessageItem>() as PostMessageItem;
		if(temp == null) temp = gameObject.AddComponent<PostMessageItem>();
		temp.ChangeGiftItems(myId);
		temp.thiscallback = callback;
	}

	//public System.Action thiscallback;
	public void InitPostBoxTest(int index, System.Action callback){
		//Utility.Log("tt " +index);
		//Utility.Log("t1" + myId);
		if(index == myId) return;
		myId = index;
	//	if(gameRank.instance.postTestItem.Count <= myId) {
	//		gameObject.SetActive(false);
	//		return;
	//	}
	
	//	gameObject.SetActive(true);
	//	transform.FindChild("Accept").GetComponentInChildren<UILabel>().text = gameRank.instance.postTestItem[myId].index.ToString();
	//	thiscallback = callback;
	}

	public void reUpdate(){
		string[] str = gameObject.name.Split('_');
		int _id = int.Parse(str[1]);
		//Utility.Log("tt " +_id + "== " + myId);
		//Utility.Log("t1" + myId);
		if(gameRank.instance.listGift.Count <= _id) {
			gameObject.SetActive(false);
			return;
		}
		gameObject.SetActive(true);
		//transform.FindChild("Accept").GetComponentInChildren<UILabel>().text = gameRank.instance.listGift[_id].index.ToString();
		//thiscallback = callback;

	}


	public void InitInviteFriend(int index){
		if(index == myId) return;
		myId = index;
		var t  =gameObject.GetComponent<InviteFriendItem>() as InviteFriendItem;
		if(t == null) t = gameObject.AddComponent<InviteFriendItem>();
		t.InitInviteBox(myId);
	}

	public void InitInviteFBFriend(int index){
		if(index == myId) return;
		myId = index;
		if(GV.DefaultProfileTexture == null) {
			GV.DefaultProfileTexture = (Texture)Resources.Load("User_Default", typeof(Texture));
		}

		var t  =gameObject.GetComponent<InviteFrItem>() as InviteFrItem;
		if(t == null) t = gameObject.AddComponent<InviteFrItem>();
		t.InitInviteBox(myId);
	}

	public void WorldRank(int idx){
		if(idx == myId) return;
			myId = idx;
		var temp = gameObject.GetComponent<WorldRankItem>() as WorldRankItem;
		if(temp == null) temp = gameObject.AddComponent<WorldRankItem>();
		temp.ChangeWorldRankContent(idx);
	}


	public void FBFriendRank(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp = gameObject.GetComponent<FBRankItem>() as FBRankItem;
		if(temp == null) temp = gameObject.AddComponent<FBRankItem>();
		temp.ChangeFBRankContent(idx);
	}
	public void FBFriendRankTest(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp = gameObject.GetComponent<FBRankItem>() as FBRankItem;
		if(temp == null) temp = gameObject.AddComponent<FBRankItem>();
		temp.DefaultChangeFBRankContent(idx);
	}

	
	public void WeeklyRank(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp = gameObject.GetComponent<WeeklyRankItem>() as WeeklyRankItem;
		if(temp == null) temp = gameObject.AddComponent<WeeklyRankItem>();
		temp.ChangeWeeklyRankContent(idx);
	}


	public void RankingWeeklyByTime(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp = gameObject.GetComponent<RankingItems>() as RankingItems;
		if(temp == null) temp = gameObject.AddComponent<RankingItems>();
		temp.ChangeRankingWeeklyByTime(idx);
	}
	public void RankingWeeklyByThrophy(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp = gameObject.GetComponent<RankingItems>() as RankingItems;
		if(temp == null) temp = gameObject.AddComponent<RankingItems>();

		temp.ChangeRankingWeeklyByThrophy(idx);
	}

	public void RankingWorldByTime(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp = gameObject.GetComponent<RankingItems>() as RankingItems;
		if(temp == null) temp = gameObject.AddComponent<RankingItems>();
		temp.ChangeRankingWorldByTime(idx);
	}
	public void RankingWorldByThrophy(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp = gameObject.GetComponent<RankingItems>() as RankingItems;
		if(temp == null) temp = gameObject.AddComponent<RankingItems>();
		temp.ChangeRankingWorldByThrophy(idx);
	}


	public void FriendRank(int idx){
		if(idx == myId) return;
			myId = idx;
		var temp1 = gameObject.GetComponent<FriendRankItem>() as FriendRankItem;
		if(temp1 == null) temp1 = gameObject.AddComponent<FriendRankItem>();
		temp1.FRankContent(idx);
	}

	public void ViewVisitClanMem(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp1 = gameObject.GetComponent<VisitClanMemItem>() as VisitClanMemItem;
		if(temp1 == null) temp1 = gameObject.AddComponent<VisitClanMemItem>();
		temp1.visitClanContent(idx);
	}

	public void ViewMyClan(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp1 = gameObject.GetComponent<ViewMyClanItem>() as ViewMyClanItem;
		if(temp1 == null) temp1 = gameObject.AddComponent<ViewMyClanItem>();
		temp1.visitMyClanContent(idx);
		temp1.ChangeContents(idx);
	}

	public void ViewHistory(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp1 = gameObject.GetComponent<ViewHistoryItem>() as ViewHistoryItem;
		if(temp1 == null) temp1 = gameObject.AddComponent<ViewHistoryItem>();
		temp1.ViewHistoryContent(idx);
		temp1.ChangeContents(idx);
	}
	public void ViewRankGlobal(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp1 = gameObject.GetComponent<ViewRankItem>() as ViewRankItem;
		if(temp1 == null) temp1 = gameObject.AddComponent<ViewRankItem>();
//		temp1.ViewRankContent(idx);
		temp1.ChangeContentsGlobal(idx);
	}
	public void ViewRankLocal(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp1 = gameObject.GetComponent<ViewRankItem>() as ViewRankItem;
		if(temp1 == null) temp1 = gameObject.AddComponent<ViewRankItem>();
	//	temp1.ViewRankContent(idx);
		temp1.ChangeContentsLocal(idx);
	}
	public void ViewSearch(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp1 = gameObject.GetComponent<ViewSearchItem>() as ViewSearchItem;
		if(temp1 == null) temp1 = gameObject.AddComponent<ViewSearchItem>();
		temp1.ViewSearchContent(idx);
		temp1.ChangeContents(idx);
	}


	public void ViewMyTeam(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp1 = gameObject.GetComponent<ViewMyTeamItem>() as ViewMyTeamItem;
		if(temp1 == null) temp1 = gameObject.AddComponent<ViewMyTeamItem>();
		temp1.ViewTeamContent(idx);
	}

	public void ViewClanHistory(int idx){
		if(idx == myId) return;
		myId = idx;
		var temp1 = gameObject.GetComponent<ClanWarItem>() as ClanWarItem;
		if(temp1 == null) temp1 = gameObject.AddComponent<ClanWarItem>();
		//temp1.ViewClanWarInfo(idx);
	}

	public void ViewRaceResultDetail(int idx, ClubRaceResultInfo mResult){
		if(idx == myId) return;
		myId = idx;
		var temp1 = gameObject.GetComponent<ClubMatchUserResultDetail>() as ClubMatchUserResultDetail;
		if(temp1 == null) temp1 = gameObject.AddComponent<ClubMatchUserResultDetail>();
		temp1.ViewTeamContent(idx, mResult);
	}
	/*
	Texture2D oddTex, evenTex;
	public void InitTestBox(int idx){
		if(idx == myId) return;
			myId = idx;

		var odd =	transform.FindChild("odd") as Transform;
		int count = idx*2;
		//KakaoFriends.Friend _fr = KakaoFriends.Instance.friends[count];
		string oddString = "Testodd " + count.ToString();
		var oddTitle = odd.FindChild("title") as Transform;
		oddTitle.GetComponentInChildren<UILabel>().text = oddString;
	
		odd.FindChild("BtnAccept").gameObject.SetActive(true);
		var t = odd.FindChild("BtnAccept").GetComponentInChildren<UIButtonMessage>() as UIButtonMessage;
		t.transform.gameObject.name = count.ToString();
		odd.FindChild("BtnWait").gameObject.SetActive(false);

		var even =  transform.FindChild("even") as Transform;
		count = idx*2+1;
		even.gameObject.SetActive(true);

		oddString = "Test even " + count.ToString();
		oddTitle = even.FindChild("title");
		oddTitle.GetComponentInChildren<UILabel>().text = oddString;

		even.FindChild("BtnAccept").gameObject.SetActive(false);
		even.FindChild("BtnWait").gameObject.SetActive(true);

		StopCoroutine("PicTestLoad");
		Texture2D tx = UserDataManager.instance.GetTexture(2*idx);
		if(tx == null){
			setTextureLoad();
			StartCoroutine("PicTestLoad", idx);
		}else{
			setTextureLoad(idx,"even");
			setTextureLoad(idx,"odd");
		}
	}
	void setTextureLoad(){
		var even =  transform.FindChild("even") as Transform;
		even.FindChild("kakaopic").GetComponent<UITexture>().mainTexture = Global.gDefaultIcon;
		even =  transform.FindChild("odd") as Transform;
		even.FindChild("kakaopic").GetComponent<UITexture>().mainTexture = Global.gDefaultIcon;
	}

	void setTextureLoad(int idx, string str){
		if(str.Equals("even")){
			var even =  transform.FindChild("even") as Transform;
			evenTex = UserDataManager.instance.GetTexture( idx*2);
			even.FindChild("kakaopic").GetComponent<UITexture>().mainTexture = evenTex;
		}else{
			var odd =  transform.FindChild("odd") as Transform;
			oddTex = UserDataManager.instance.GetTexture( idx*2+1);
			odd.FindChild("kakaopic").GetComponent<UITexture>().mainTexture = oddTex;
		}
	}

	IEnumerator PicTestLoad(int count){

		yield return StartCoroutine("PicLoadTest1", count*2);
		yield return StartCoroutine("PicLoadTest2", (count*2+1));

	}

	IEnumerator PicLoadTest1(int idx){
		string url = "https://cdn1.iconfinder.com/data/icons/free-dark-blue-cloud-icons/96/SMS.png";
		WWW www = new WWW( url );
		//var t = gameObject.AddComponent<UITexture>() as UITexture;
		yield return www;
		
		if( this == null )
			yield break;
		
		if( www.error != null )
		{
			Utility.Log( "load failed" );
			//SetDefaultTexture(idx, false);
		}
		else
		{
			evenTex = www.texture;
			www.Dispose();
			UserDataManager.instance.SaveSetTexture(idx, evenTex);
			SetTextureEven(idx);
		}
	}

	IEnumerator PicLoadTest2(int idx){
		string url ="https://cdn1.iconfinder.com/data/icons/dot/256/cafe_coffee.png";
		WWW www = new WWW( url );
		
		yield return www;
		
		if( this == null )
			yield break;
		
		if( www.error != null )
		{
			Utility.Log( "load failed" );
			//	SetDefaultTexture(idx, false);
		}
		else
		{
	
			//SetTextureOdd(1);
			oddTex =www.texture;
			UserDataManager.instance.SaveSetTexture(idx,oddTex);
			SetTextureOdd(idx);
			www.Dispose();
		}
	}
	void SetTextureEven(int idx){
		var even =  transform.FindChild("even") as Transform;
		evenTex = UserDataManager.instance.GetTexture(idx);
		if(evenTex == null) evenTex = (Texture2D)Global.gDefaultIcon;
		even.FindChild("kakaopic").GetComponent<UITexture>().mainTexture = evenTex;
	}
	
	void SetTextureOdd(int idx){
		var even =  transform.FindChild("odd") as Transform;
		oddTex = UserDataManager.instance.GetTexture(idx);
		if(oddTex == null) oddTex = (Texture2D)Global.gDefaultIcon;
		even.FindChild("kakaopic").GetComponent<UITexture>().mainTexture = oddTex;
	}
}

*/
/*그런 현상이 있습니다~~~ 있고요~~
SetData에서 즉시 데이터 교체가 이루어지지 않고
Coroutine 같은 곳에서 일정 시간 후에 데이터 교체가 이루어져서 생기는 문제입니다...
저는 아래와 같이 설정하니 문제 없이 사진 로드가 되었습니다. 참고하세요~
중요하게 보실부분은 StopCoroutine 입니다

public void Load( string uid )
{
mUID = uid;


StopCoroutine( "load" );
if( uid != null )
StartCoroutine( "load", uid );
}

private IEnumerator load( string uid )
{
string url = Kakao.cKakao.Instance.GetPictureUrl( uid );

WWW www = new WWW( url );

yield return www;

if( this == null )
yield break;

if( www.error != null )
{
Utility.Log( "load failed" );
SetDefaultTexture();
}
else
{
Texture2D tex = www.texture;
www.Dispose();
SetTexture( tex );

cPicturePool.Instance.Save( uid, tex );
}*/
}
