using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
public class AchievementWindow : MonoBehaviour {

	public UILabel[] lbText;
	public UIDraggablePanel2 dragWorld;
	public Transform view; 
	public Transform grid;
	//public GameObject readyBG;
	public GameObject achieveitems;
	public GameObject BtnAchieve;
	void Start () {
		lbText[0].text =KoStorage.GetKorString("72100");// TableManager.ko.dictionary["60184"].String;
		lbText[2].text =KoStorage.GetKorString("72100");
	
		if(Application.isEditor){
			BtnAchieve.transform.FindChild("icon").GetComponent<UISprite>().spriteName = "SNS_GooglePlay";
			BtnAchieve.transform.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnAchieveGoogle";
	//		lbText[2].text =string.Format(KoStorage.getStringDic("60367"),KoStorage.getStringDic("60250"));
			lbText[1].text =KoStorage.GetKorString("72102");
		}else{
			if (Application.platform == RuntimePlatform.Android){
				BtnAchieve.transform.FindChild("icon").GetComponent<UISprite>().spriteName = "SNS_GooglePlay";
				BtnAchieve.transform.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnAchieveGoogle";
	//			lbText[2].text =string.Format(KoStorage.getStringDic("60367"),KoStorage.getStringDic("60250"));
				lbText[1].text =KoStorage.GetKorString("72102");
			}else if(Application.platform == RuntimePlatform.IPhonePlayer){
				BtnAchieve.transform.FindChild("icon").GetComponent<UISprite>().spriteName = "SNS_GameCenter";
				BtnAchieve.transform.FindChild("btn").GetComponent<UIButtonMessage>().functionName = "OnAchieveApple";
	//			lbText[2].text =string.Format(KoStorage.getStringDic("60367"),KoStorage.getStringDic("60250"));
				lbText[1].text =KoStorage.GetKorString("72102");
			}
		}
	}

	void OnAchieveGoogle(){
		SNSManager.OnAchieveGoogleClick2();
		/*SNSManager.OnAchieveGoogleClick(()=>{
			var pop = ObjectManager.SearchWindowPopup() as GameObject;
			pop.AddComponent<SocialLogInPopUp>().InitPopUp(1);
			pop.GetComponent<SocialLogInPopUp>().onFinishCallback(()=>{
				NGUITools.FindInParents<SubMenuWindow>(gameObject).OnChangeWindow(gameObject);
			});
		
		});*/
	}


	void OnGoogleRank(){
		SNSManager.OnRankGoogleClick2();
	}
	
	void OnGoogleAchieve(){
		SNSManager.OnAchieveGoogleClick2();
		
	}


	void OnAchieveApple(){
	
	}

	public void setAchievement(){
		//StartCoroutine("getWorldRank");
		AddAchieveItem();
	}
	
	public void stopAchievement(){
	
	}
	
	public void AddAchieveItem(){
		int cnt = grid.childCount;
		if(cnt == 0){
			int max = Common_Achieve.getCount() / 3;
			for(int i =0; i < max; i++){
				var temp = NGUITools.AddChild(grid.gameObject, achieveitems);
				temp.name = (16000+i*3).ToString();
				temp.AddComponent<AchievementItem>().setAchieveContent(16000+i*3);
			}
			grid.GetComponent<UIGrid>().Reposition();
			GAchieve.instance.achieveInfo.bFlag = false;
		}else{
			if(GAchieve.instance.achieveInfo.bFlag){
				for(int i =0; i < cnt; i++){
					var temp = grid.GetChild(i) as Transform;	
					temp.GetComponent<AchievementItem>().ChangeAchieveContent();
				}
				GAchieve.instance.achieveInfo.bFlag = false;
			}
			view.GetComponent<UIDraggablePanel>().ResetPosition();
		}

	}

	public void AddItem(){
		dragWorld.maxScreenLine = 2;
		dragWorld.maxColLine = -1;
		int count = 0;
		
		if(grid.childCount == 0 ){
			count = 10;
			//Utility.Log(count);
			if(count <= 2) count = 4;
			dragWorld.Init(count, delegate(UIListItem item, int index) {
				item.Target.GetComponent<UIScrollListBase>().AchievementList(index);
			});
		}
		
		
	}
}
