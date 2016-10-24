using UnityEngine;
using System.Collections;

public class createPBaction : MonoBehaviour {

	public void CreatePB(int id){
		string name = Base64Manager.instance.GlobalEncoding(Global.gAICarInfo[id].AISponsorID).ToString()+"_PB";
		//Texture main = (Texture)UserDataManager.instance.pbbundle.Load(name, typeof(Texture));
		Texture main = (Texture)Resources.Load("Textures/PitBox/"+name, typeof(Texture));
		if(main == null) Utility.LogError("texture_null" + name);
		gameObject.renderer.material.mainTexture = main;
		//Resources.UnloadUnusedAssets();
		main = null;
		Destroy(this, 0.2f);
	}

	public void CreateMyPB(int sponID){
		string name = sponID.ToString()+"_PB";
		//Texture main = (Texture)UserDataManager.instance.pbbundle.Load(name, typeof(Texture));
		Texture main = (Texture)Resources.Load("Textures/PitBox/"+name, typeof(Texture));
		if(main == null) Utility.LogError("texture_null");
		gameObject.renderer.material.mainTexture = main;
		//Resources.UnloadUnusedAssets();
		main = null;
		Destroy(this, 0.2f);
	}
}
