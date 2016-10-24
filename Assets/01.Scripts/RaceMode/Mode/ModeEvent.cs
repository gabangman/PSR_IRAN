using UnityEngine;
using System.Collections;

public class ModeEvent : ModeParent {
	public GameObject popStart, popStop;
	public override void OnSubWindow ()
	{
		//base.OnSubWindow ();
	}

	void EventRaceStart(){
		Utility.Log("eventRaceStart " + transform.name);
	}

}
