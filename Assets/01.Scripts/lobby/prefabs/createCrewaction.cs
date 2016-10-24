using UnityEngine;
using System.Collections;

public class createCrewaction : MonoBehaviour {


	Animation[] AICREW;// = new Animation[4];

	public void CrewCreate(int crewid){
		AICREW = new Animation[4];
		makecrew(crewid, 10);
	}

	public void AICrewCreate(int num){
		int crewid = Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[num].AICrewID);
		AICREW = new Animation[4];
		makecrew(crewid, num);
	}

	void makecrew(int crewid ,int id){
		string _name = string.Empty;
		for(int i = 0; i < transform.childCount ; i++){
			_name =crewid.ToString()+"_"+ transform.GetChild(i).name+"_"+id.ToString();
			var crew = ObjectManager.GetSelectObjectWithAIRace(_name);
			if(crew == null) Utility.LogError("Crew is null" + _name);
			_name =crewid.ToString()+"_"+ transform.GetChild(i).name;
			crew.name = _name;
			crew.transform.parent = transform.parent;
			crew.transform.rotation = transform.GetChild(i).rotation;
			crew.transform.localPosition = transform.GetChild(i).position;
			crew.SetActive(true);
			AICREW[i] = crew.transform.GetChild(0).GetComponent<Animation>();
		}
	}

	public Animation[] CrewAniamtion(){
		for(int i = 0; i < AICREW.Length; i++){
			AICREW[i].wrapMode = WrapMode.Default;
			AICREW[i].Stop();
		}
		Invoke("objectDestory", 0.5f);;
		return AICREW;
	}
	/*
	public void CrewTextureInit(GameObject crew){
		crew.AddComponent<textureaction>();
		crew.GetComponent<textureaction>().ChangeHeadTexture(Global.MySponsorID);
		crew.GetComponent<textureaction>().ChangeBodyTexture(Global.MySponsorID);
	}

	public void CrewTextureInit(GameObject crew, int num){
		crew.AddComponent<textureaction>();
		crew.GetComponent<textureaction>().ChangeRaceHeadTexture(num);
		crew.GetComponent<textureaction>().ChangeRaceBodyTexture(num);
	}
	*/
}
