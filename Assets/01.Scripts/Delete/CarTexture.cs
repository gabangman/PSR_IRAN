using UnityEngine;
using System.Collections;

public class CarTexture : MonoBehaviour {
	/*
	public void CarSponChangeTexture(string carid, int sponID){
		string carname = carid+"_"+sponID.ToString();
		var car = transform.GetChild(0).FindChild("TireGroup_Axis_R").GetChild(0).FindChild("CarBody") as Transform;
		var text = car.renderer.material.GetTexture("_diffuse") as Texture;
		if(string.Equals(text.name, carname)){

		}else{
			Texture main = (Texture)AccountManager.instance.carbundle.Load(carname, typeof(Texture));
			car.renderer.material.SetTexture("_diffuse",main);	
			main =null;	
		}
		Destroy(this, 0.1f);
	}


	public void CarInitialize(string carid, string sponID, int num){
		string carname = string.Empty;
		carname = carid+"_"+sponID;
		var car = transform.GetChild(0).FindChild("TireGroup_Axis_R").GetChild(0).FindChild("CarBody") as Transform;
		var text = car.renderer.material.GetTexture("_diffuse") as Texture;
		if(!text){
			string abName = carname;
			Texture main = (Texture)AccountManager.instance.carbundle.Load(abName, typeof(Texture));
			car.renderer.material.SetTexture("_diffuse",main);	
		//	Utility.LogWarning(main.name);
			main =null;	
		}else{
			if(string.Equals(text.name, carname)){
		
			}else{
			//	Utility.LogWarning(carname);
				Texture main = (Texture)AccountManager.instance.carbundle.Load(carname, typeof(Texture));
				car.renderer.material.SetTexture("_diffuse",main);	
				main =null;	
			}
		}
		var tireparent = transform.GetChild(0).FindChild("TireGroup_Axis_R").GetChild(0) as Transform;
		for(int i = 0; i < tireparent.childCount; i++){
			var tires = tireparent.GetChild(i) as Transform;
			if(tires.name == "CarBody"){
			
			}else{
				tires.renderer.material.color = Color.white;
			}
		}
		Destroy(this, 0.1f);
	}

	public void defaultCarTexture(){
		var tireparent = transform.GetChild(0).FindChild("TireGroup_Axis_R").GetChild(0) as Transform;
		for(int i = 0; i < tireparent.childCount; i++){
			var tires = tireparent.GetChild(i) as Transform;
			if(tires.name == "CarBody"){
				tires.renderer.material.SetColor("_DiffuseColor",Color.black);
			}else{
				tires.renderer.material.color = Color.black;
			}
		}
		Destroy(this, 0.1f);
	}

	public void Crewinitalize(string crewid, string sponID, int num){
		string[] teamID = gameObject.name.Split("_"[0]);
		var tex = GetComponent<textureaction>() as textureaction;
		if(tex == null) tex = gameObject.AddComponent<textureaction>();
		tex.CrewHeadTextureInit(sponID,teamID[1], true);
		tex.CrewBodyTextureInit(crewid,sponID, true);
		Destroy(this, 0.1f);
	}
*/
}
