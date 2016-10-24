using UnityEngine;
using System.Collections;

public class CarType : MonoBehaviour {

	public Animation CarAni;
	public GameObject Skid_L, Skid_R;
	public Transform CarClass;
	public Transform[] boosts;
	public Transform[] Tires;

	void Start(){

	}


	public void CarTextureInitialize(string carid, string sponID){
		string carname = carid+"_"+sponID;
		//var car = transform.GetChild(0).FindChild("TireGroup_Axis_R").GetChild(0).FindChild("CarBody") as Transform;
		var car = CarClass.parent.transform as Transform;
		Texture TexCar = null;
		TexCar = car.renderer.material.GetTexture("_MainTex") as Texture;
		if(!TexCar){
			//TexCar = (Texture)AccountManager.instance.carbundle.Load(carname, typeof(Texture));
			TexCar =(Texture)Resources.Load("Textures/Car/"+carname, typeof(Texture));
			car.renderer.material.SetTexture("_MainTex",TexCar);	
		}else{
			if(string.Equals(TexCar.name, carname)){
				
			}else{
				//TexCar = (Texture)AccountManager.instance.carbundle.Load(carname, typeof(Texture));
				TexCar =(Texture)Resources.Load("Textures/Car/"+carname, typeof(Texture));
				car.renderer.material.SetTexture("_MainTex",TexCar);	
			}
		}
		for(int i = 0; i < Tires.Length; i++){
			Tires[i].renderer.material.color = Color.white;
		}
		TexCar = null;
	}

	
	public void CarSponTextureInitialize(string carid, string sponID){
		string carname = carid+"_"+sponID;
		//var car = transform.GetChild(0).FindChild("TireGroup_Axis_R").GetChild(0).FindChild("CarBody") as Transform;
		var car = CarClass.parent.transform as Transform;
		Texture TexCar = null;
		TexCar = car.renderer.material.GetTexture("_MainTex") as Texture;
		if(!TexCar){
			//TexCar = (Texture)AccountManager.instance.carbundle.Load(carname, typeof(Texture));
			TexCar =(Texture)Resources.Load("Textures/Car/"+carname, typeof(Texture));
			car.renderer.material.SetTexture("_MainTex",TexCar);	
		}else{
			if(string.Equals(TexCar.name, carname)){
				
			}else{
				//TexCar = (Texture)AccountManager.instance.carbundle.Load(carname, typeof(Texture));
				TexCar =(Texture)Resources.Load("Textures/Car/"+carname, typeof(Texture));
				car.renderer.material.SetTexture("_MainTex",TexCar);	
			}
		}
		TexCar = null;
	}


}
