using UnityEngine;
using System.Collections;

public class ClubSubMode : RaceMode {

	// Use this for initialization
	public override void SetRaceSubMode(){
		gameObject.GetComponent<ClubMatch>().ClubModeInitialize();
	}

}
