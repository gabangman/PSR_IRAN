using UnityEngine;
using System.Collections;

public class SelectMatItem : MonoBehaviour {

	// Use this for initialization
	private GameObject mSel;
	void Start () {
		mSel = transform.FindChild("Select").gameObject as GameObject;
	}
	
	public void InitContents(Transform tr, int id){
		transform.GetComponent<UIDragPanelContents>().draggablePanel = tr.GetComponent<UIDraggablePanel>();
		transform.FindChild("lbName").GetComponent<UILabel>().text = Common_Material.Get(id).Name;
		transform.FindChild("Icon").GetComponent<UISprite>().spriteName = id.ToString();
		int mCnt = GV.getMatCount(id);
		if(mCnt == 0){
			transform.FindChild("NoMat").gameObject.SetActive(true);
		}else{
			transform.FindChild("lbnum").GetComponent<UILabel>().text =string.Format("x{0}",mCnt);
		}
	}

	void OnSelect(){
		if(mSel.activeSelf) return;
		NGUITools.FindInParents<SelectMat>(gameObject).ResetSelectMat(transform.name);
		mSel.SetActive(true);
	}

	public void ResetSelectBG(){
		mSel.SetActive(false);
	}

}
