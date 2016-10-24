using UnityEngine;
using System.Collections;

public class BuyInterAction : MonoBehaviour,IcontrolObject {



	public void DestoryWindows(){
		DestroyImmediate(gameObject);
	}

	public void DestroyPopUp(){
	}

	public void InActivatePopUp(){
		gameObject.SetActive(false);
	}
	public void ResetIcon(GameObject _obj){
		//ChildObjectActvate(_obj.transform, "icon_Cash",false);
		//ChildObjectActvate(_obj.transform, "icon_Coin",false);
		//ChildObjectActvate(_obj.transform, "icon_Dollar",false);
	}
	
	public void ChildObjectActvate(Transform child, string targetname , bool b){
		child.FindChild(targetname).gameObject.SetActive(b);
	}
	
	public void ChildObjectLabelChange(Transform child, string targetname , string text){
		child.FindChild(targetname).GetComponent<UILabel>().text = text;
	}
	public void ChildObjectSpriteChange(Transform child, string targetname, string imagename){
		child.FindChild(targetname).GetComponent<UISprite>().spriteName = imagename;
	}

	public void ChangeLabel( string label, string text){		
		transform.FindChild(label).GetComponent<UILabel>().text = text;
	}
	
	public void ChangeActivate(string btn, bool b){
		transform.FindChild(btn).gameObject.SetActive(b);
	}
	


}
