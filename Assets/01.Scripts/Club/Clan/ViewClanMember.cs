using UnityEngine;
using System.Collections;

public class ViewClanMember : MonoBehaviour {

	void OnInfoClick(){
		Utility.Log("ViewClanMember + Info " + gameObject.name);
	}


	void OnStaffClick(){
		Utility.Log("ViewClanMember + Staff " + gameObject.name);
	}

	void OnKickClick(){
		Utility.Log("ViewClanMember + Kick " + gameObject.name);
	}


	void OnInfo(){
		Utility.Log("ClanMember + Info " + gameObject.name);
	}
	
	
	void OnStaff(){
		Utility.Log("ClanMember + Staff " + gameObject.name);
	}
	
	void OnKick(){
		Utility.Log("ClanMember + Kick " + gameObject.name);
	}


}
