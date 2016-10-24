using UnityEngine;
using System.Collections;

public class LogOutPopup : MonoBehaviour {


	void OnCloseClick(){
		UserDataManager.instance.OnSubBacksub = null;
		Destroy(this);
		gameObject.SetActive(false);
		
	}

	bool isPress = false;
	void Start(){
	
	}

	public void InitPopUp(){
		var pop = transform.FindChild("Content_BUY") as Transform;
		if(CClub.ClanMember == 0){
		isPress = false;
		pop.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("71106");
		pop.FindChild("lbPrice").GetComponent<UILabel>().text =string.Empty;
		pop.FindChild("lbOk").gameObject.SetActive(true);
		pop.FindChild("lbOk").GetComponent<UILabel>().text = KoStorage.GetKorString("72510");
		pop.FindChild("lbName").GetComponent<UILabel>().text =KoStorage.GetKorString("71119");
		pop.FindChild("btnok").gameObject.SetActive(true);
		pop.FindChild("icon_product").gameObject.SetActive(false);
		mOut = 0;
		// 그냥 탈퇴 하면 된다.
		UserDataManager.instance.OnSubBacksub = ()=>{
				OnCloseClick();
			};
	}else{
			isPress = true;
			StartCoroutine("GetClubMode");
	}
}

	private int mOut;


	void OnOkClick(){
		if(isPress) return;
		isPress  = true;

		switch(mOut){
		case 0: Invoke("unRegistor", 0.25f); break;
		case 1:	OnCloseClick(); break;
		case 2:	StartCoroutine("LogoutProcess"); break;
		case 3:break;
		default:break;
		}
	}

	IEnumerator GetClubMode(){
		bool bConnect  = false;
		string mAPI = "club/getMyClubInfo";//ServerAPI.Get(90059);
		NetworkManager.instance.ClubBaseConnect("Get",new System.Collections.Generic.Dictionary<string,int>(), mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				CClub.mClubInfo = new ClubInfo(thing);
				if(CClub.mClubInfo.clubMember == 9){
					CClub.ClanMember = 1;
				}else if(CClub.mClubInfo.clubMember ==5){
					CClub.ClanMember =2;
				}else{
					CClub.ClanMember =3;
				}
				CClub.mClubFlag = 2;
			}else if(status == -1){
			
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
		var pop = transform.FindChild("Content_BUY") as Transform;
		if(CClub.ClubMode == 3){

			pop.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("72510");
			pop.FindChild("lbPrice").GetComponent<UILabel>().text =string.Empty;
			pop.FindChild("lbOk").gameObject.SetActive(true);
			pop.FindChild("lbOk").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");
			
			pop.FindChild("lbName").GetComponent<UILabel>().text =KoStorage.GetKorString("77402");
			pop.FindChild("btnok").gameObject.SetActive(true);
			pop.FindChild("icon_product").gameObject.SetActive(false);
			mOut = 1;
			isPress = false;
			// 클럽매칭이라 팝업으로 알려주고 탈퇴 안된다. 확인 후 팝업 종료한다. 
		}else if (CClub.ClubMode == 1){
			// 클럽 진행 중인데 혼자일 경우 탈퇴 하면 안된다. 클럽매칭이라 팝업으로 알려주고 탈퇴 안된다. 확인 후 팝업 종료한다. 

			if(CClub.mClubInfo.clubMemberNum == 1){
				pop.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("72510");
				pop.FindChild("lbPrice").GetComponent<UILabel>().text =string.Empty;
				pop.FindChild("lbOk").gameObject.SetActive(true);
				pop.FindChild("lbOk").GetComponent<UILabel>().text = KoStorage.GetKorString("71000");
				
				pop.FindChild("lbName").GetComponent<UILabel>().text =KoStorage.GetKorString("77402");
				pop.FindChild("btnok").gameObject.SetActive(true);
				pop.FindChild("icon_product").gameObject.SetActive(false);
				mOut = 1;
				isPress = false;
			}else{
				// 클럽전이 종료라서 폐쇄 또는 탈퇴 후 계정삭제 호출한다.
				pop.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("71106");
				pop.FindChild("lbPrice").GetComponent<UILabel>().text =string.Empty;
				pop.FindChild("lbOk").gameObject.SetActive(true);
				pop.FindChild("lbOk").GetComponent<UILabel>().text = KoStorage.GetKorString("72510");
				pop.FindChild("lbName").GetComponent<UILabel>().text =KoStorage.GetKorString("71119");
				pop.FindChild("btnok").gameObject.SetActive(true);
				pop.FindChild("icon_product").gameObject.SetActive(false);
				isPress = false;
				mOut = 2;
			}
			isPress = false;
			mOut = 2;
		
		}else{
			// 클럽전이 종료라서 폐쇄 또는 탈퇴 후 계정삭제 호출한다.
			pop.FindChild("lbText").GetComponent<UILabel>().text =KoStorage.GetKorString("71106");
			pop.FindChild("lbPrice").GetComponent<UILabel>().text =string.Empty;
			pop.FindChild("lbOk").gameObject.SetActive(true);
			pop.FindChild("lbOk").GetComponent<UILabel>().text = KoStorage.GetKorString("72510");
			pop.FindChild("lbName").GetComponent<UILabel>().text =KoStorage.GetKorString("71119");
			pop.FindChild("btnok").gameObject.SetActive(true);
			pop.FindChild("icon_product").gameObject.SetActive(false);
			isPress = false;
			mOut = 2;
		
		}
	
	}




	IEnumerator LogoutProcess(){
		Global.isNetwork = true;
		bool bConnect = false;
		string mAPI = "club/destroyClub";//destroyClub
		System.Collections.Generic.Dictionary<string,string> mDic = new System.Collections.Generic.Dictionary<string,string>();
		if(CClub.mClubInfo.clubMemberNum == 1){
			NetworkManager.instance.ClubBaseStringConnect("Delete",mDic,mAPI,(request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					
				}else{
					
				}
				Global.isNetwork = false;
				bConnect = true;
				
			});
			while(!bConnect){
				yield return null;
			}
			Invoke("unRegistor", 0.2f);
			
		} // 혼자이다. 이때는 클럽 폐쇄?
		else{ //마스터 이건 아닌건 탈퇴한다. 
			bConnect = false;
			mAPI = "club/unJoinClub";
			NetworkManager.instance.ClubBaseStringConnect("Delete",mDic,mAPI,(request)=>{
				Utility.ResponseLog(request.response.Text, GV.mAPI);
				var thing = SimpleJSON.JSON.Parse(request.response.Text);
				int status = thing["state"].AsInt;
				if (status == 0)
				{
					
				}else{
					
					
				}
				Global.isNetwork = false;
				bConnect = true;
			});
			
			while(!bConnect){
				yield return null;
			}
			Invoke("unRegistor", 0.2f);
		} // 마스터 이건 아니면 탈퇴
		
	}


	void unRegistor(){
		isPress=false;
		NetworkManager.instance.UnRegister(GV.UserRevId);
		Vibration.OnUnRegister();
		OnCloseClick();
		//Application.Quit();
	}




}
