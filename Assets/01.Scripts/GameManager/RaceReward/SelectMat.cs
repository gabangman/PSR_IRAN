using UnityEngine;
using System.Collections;

public class SelectMat : MonoBehaviour {

	public GameObject mSucc, mSelect, mItem;
	public UILabel[] lblabes;
	public Transform mGrid;
	private GameObject selectBtn;
	void Start(){
		lblabes[0].text = string.Format(KoStorage.GetKorString("73051"), 1);
		lblabes[1].text =KoStorage.GetKorString("76307");
		selectBtn = transform.FindChild("SelectMat").FindChild("Btn_Select").gameObject;
		selectBtn.SetActive(false);
	}


	public void SelectMaterialWindow(){
		InitializeFeatured();
	}

	private void InitializeFeatured(){

		int idx = 0;
		for(int i = 0; i < 20; i++){
			idx = 8600+i;
			var temp = NGUITools.AddChild(mGrid.gameObject, mItem);
			temp.name = idx.ToString();
			temp.GetComponent<SelectMatItem>().InitContents(mGrid.parent, idx);
		}
		mGrid.GetComponent<UIGrid>().Reposition();
	}

	void OnSelect(){
		if(mSelect_Name.Equals(string.Empty)) return;
		//lblabes[2].text = "";
		int matID = int.Parse(mSelect_Name);
		mSelect.gameObject.SetActive(false);
		Global.isNetwork = true;
		StartCoroutine(addMaterialFeaturedRace(matID, ()=>{
			mSucc.transform.FindChild("icon_Mat1").GetComponent<UISprite>().spriteName = matID.ToString();
			mSucc.transform.FindChild("MatName1").GetComponent<UILabel>().text = Common_Material.Get(matID).Name;
			mSucc.gameObject.SetActive(true);
			GV.UpdateMatCount(matID, 1);
			transform.parent.parent.FindChild("BtnOK").gameObject.SetActive(true);
			GameManager.instance.featuredSelectSound();
		}));
	}

	IEnumerator addMaterialFeaturedRace(int matId, System.Action callback){
		System.Collections.Generic.Dictionary<string,int> mDic = new System.Collections.Generic.Dictionary<string,int>();
		mDic.Add("materialId", matId);
		string mAPI = ServerAPI.Get(90015); //game/material
		NetworkManager.instance.HttpFormConnect("Post", mDic,  mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				callback();
				callback = null;
			}else{
				Utility.LogError("Error " + status + "msg : " +thing["msg"] );
			}
			Global.isNetwork=  false;
		});
		yield return null;
	}

	private string mSelect_Name;
	public void ResetSelectMat(string name){
		mSelect_Name = string.Empty;
		this.mSelect_Name = name;
		if(!selectBtn.activeSelf) selectBtn.SetActive(true);
		for(int i = 0; i < 20; i++){
			mGrid.GetChild(i).transform.GetComponent<SelectMatItem>().ResetSelectBG();
		}
	}

}
