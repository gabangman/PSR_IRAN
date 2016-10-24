using UnityEngine;
using System.Collections;

public class SlotItems : MonoBehaviour {
	
	public virtual void OnSelect(){}
	public virtual void OnClick(){}
	public virtual void OnClick(GameObject obj){}
	public virtual void ChangeContents(int idx){}
}
