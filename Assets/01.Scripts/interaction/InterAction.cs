using UnityEngine;
using System.Collections;

public interface IcontrolObject {
	void ChildObjectActvate(Transform child, string targetname , bool b);
	void ChildObjectLabelChange(Transform child, string targetname , string text);
	void ChildObjectSpriteChange(Transform child, string targetname, string imagename);

}


public abstract class InterAction : MonoBehaviour, IcontrolObject {

	public virtual void OnNextClick(GameObject selectbtn){	}
	public virtual void OnSelectCarClick(){}
	public virtual void OnSelectCrewClick(){}
	public virtual void OnCarClick(){}
	public virtual void OnCrewClick(){}
	public virtual void Initialize(){}
	public virtual void OnCarItemClick(){}
	public  void ObjectSetActive(GameObject info, string name , bool b){
		info.transform.FindChild(name).gameObject.SetActive(b);
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


}
