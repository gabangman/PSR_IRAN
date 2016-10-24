using UnityEngine;
using System.Collections;

public class ClanMatchHistory : InfoWindow {

	public override void ShowWindow ()
	{
		showSub(gameObject);
	}

	public override void HiddenWindow ()
	{
		hiddenSub(gameObject);
	}

	void ChangeInfo(){
		Utility.Log("ClanMatchHistory " + gameObject.name);
	}

}
