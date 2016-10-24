using UnityEngine;
using System.Collections;
using System.Text;
public class textureaction :MonoBehaviour {

	public void ChangeAnimation(){
		Animation Driver = transform.GetChild(0).GetComponent<Animation>() as Animation;
		if(Driver == null) Utility.LogWarning("error");
		Driver.wrapMode = WrapMode.Loop;
		Driver.Play("driver_idle");
	}

	GameObject FindHeadCrew(){
		var child = transform.GetChild(0).FindChild ("Root").GetChild(0).GetChild(0).GetChild(0).GetChild(0).FindChild("Head") as Transform;
		GameObject tr = null;
		for (int i = 0 ; i < child.childCount; i++){
			var temp = child.GetChild(i).gameObject as GameObject;
			if(temp.GetComponent<Renderer>() != null){
				tr = temp;
				break;
			}
		}
		//Utility.Log(tr.name);
		return tr;
	}

	void OnEnable(){
		string[] _name = gameObject.name.Split("_"[0]);
		//Utility.Log("name " + gameObject.name);
		//StartCoroutine("CrewAniPlay",_name[1]);	
		CrewAniPlay(_name[1]);
	}

	GameObject FindBodyCrew(){
		var child = transform.GetChild(0) as Transform;
		string _name  = child.name;
		var _ch = transform.GetChild(0).FindChild(_name) as Transform;
		return _ch.gameObject;
	}

	string SetShader(string _shader){
		string t = string.Empty;
		if(_shader.Length > 15)
			t = "_diffuse";
		else t = "_MainTex";

		if(_shader == "Diffuse")
			t = "_MainTex";
		return t;
	}

	public void CrewAniPlay(string crewid){
		Animation crewani  = transform.GetChild(0).GetComponent<Animation>();
		crewani.wrapMode = WrapMode.Loop;
		//Utility.Log("B");
		switch(crewid){
		case "5100":
			//yield return new WaitForSeconds(1.0f);
			if(transform.parent.name == "Driver_Axis"){
				crewani.Play("driver_idle");
			}else{
				crewani.Play("driver_idle_stand");
			}
			break;
		case "5101" :
			crewani.Play("tire_idle");
			break;
		case "5102" :
			crewani.Play("chief_idle_stand");
			break;
		case "5103" :
			crewani.Play("jack_idle");
			break;
		case "5104":
			crewani.Play("gas_idle");
			break;
		}
		//yield return null;
	}

	//bool isRace = false;

	public void defaultCrewBodyTexture(){
		var _child = FindBodyCrew() as GameObject;
		//Texture _texture = null;
		string sh = string.Empty;
		if(_child.renderer.materials.Length == 1){
			sh = SetShader(_child.renderer.material.shader.name);
			_child.renderer.material.SetTexture(sh,null);
			//_child.renderer.material.SetColor("",Color.black);
			_child.renderer.material.color = Color.black;
		}else{
				sh = SetShader(_child.renderer.materials[0].shader.name);
				_child.renderer.materials[0].SetTexture(sh,null);
				//_child.renderer.material.SetColor("",Color.black);
				_child.renderer.materials[0].color = Color.black;	
			sh = SetShader(_child.renderer.materials[1].shader.name);
				_child.renderer.materials[1].SetTexture(sh,null);
				_child.renderer.materials[1].color = Color.black;
		}
	}

	public void defaultCrewHeadTexture(){
		var _child = FindHeadCrew() as GameObject;
		string sh = string.Empty;
		if(_child == null) return;
			sh = SetShader(_child.renderer.material.shader.name);
			_child.renderer.material.SetTexture(sh,null);
			_child.renderer.material.color = Color.black;
	}


	public void CrewBodyTextureInit(string crewID, string sponID, bool b){
		CrewBodyTextureInit(crewID, sponID);
		//isRace = b;
	}
	public void CrewHeadTextureInit(string sponID, string crewID, bool b){
		CrewHeadTextureInit(sponID, crewID);
		//isRace =b;
	}
	public void CrewBodyTextureInit(string crewID, string sponID){
		var _child = FindBodyCrew() as GameObject;

		StringBuilder sb = new StringBuilder();
		sb.Length = 0;
		sb.Append(crewID);
		sb.Append("_");
		sb.Append(sponID);
		//Utility.LogError("CrewBodyTextureInit"); return;
		string sh = string.Empty;
		Texture _texture = null;
		if(_child.renderer.materials.Length == 1){
			sh = SetShader(_child.renderer.material.shader.name);
			_texture = _child.renderer.material.GetTexture(sh);
			if(_texture == null) {
				Texture main =  (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
				//Texture main = (Texture)AccountManager.instance.carbundle.Load(sb.ToString(), typeof(Texture));
				_child.renderer.material.SetTexture(sh,main);
				_child.renderer.material.color = Color.white;
			}else{
				if(string.Equals(sb.ToString(), _texture.name)){
					return;
				}else{
					Texture main =  (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
					//Texture main = (Texture)AccountManager.instance.carbundle.Load(sb.ToString(), typeof(Texture));
					_child.renderer.material.SetTexture(sh,main);
					_child.renderer.material.color = Color.white;
				}
			}
		}else{
			sh = SetShader(_child.renderer.materials[0].shader.name);
			_texture = _child.renderer.material.GetTexture(sh);
			if(_texture == null) {
				Texture main =  (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
				//Texture main = (Texture)AccountManager.instance.carbundle.Load(sb.ToString(), typeof(Texture));
				_child.renderer.materials[0].SetTexture(sh,main);
				sh = SetShader(_child.renderer.materials[1].shader.name);
				_child.renderer.materials[1].SetTexture(sh,main);
				_child.renderer.materials[0].color = Color.white;
				_child.renderer.materials[1].color = Color.white;
			}else{
				if(string.Equals(sb.ToString(), _texture.name)){
					return;
				}else{
					//Texture main = (Texture)AccountManager.instance.carbundle.Load(sb.ToString(), typeof(Texture));
					Texture main =  (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
					_child.renderer.materials[0].SetTexture(sh,main);
					sh = SetShader(_child.renderer.materials[1].shader.name);
					_child.renderer.materials[1].SetTexture(sh,main);
					_child.renderer.materials[0].color = Color.white;
					_child.renderer.materials[1].color = Color.white;
				}
			}
		}
		
	}

	//	Texture main =  (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
	//	if(main == null) Utility.LogWarning("texture is null");
	//	if(_child.renderer.materials.Length == 1){
	//		string sh = SetShader(_child.renderer.material.shader.name);
		//	string gettex = _child.renderer.material.GetTexture(sh).name;
		//	if(gettex != main.name) 
	//			_child.renderer.material.SetTexture(sh,main);
	//	}else{
		//	string sh = SetShader(_child.renderer.materials[1].shader.name);
			//string gettex = _child.renderer.materials[1].gett.name;
			//if(gettex != main.name) 
		//	_child.renderer.materials[1].SetTexture(sh,main);
		//
	//		sh = SetShader(_child.renderer.materials[0].shader.name);
		//	gettex = _child.renderer.materials[0].GetTexture(sh).name;
		//	if(gettex != main.name) 
	//			_child.renderer.materials[0].SetTexture(sh,main);
	//	}
		//Resources.UnloadUnusedAssets();
		//main = null;
	//}

	public void CrewHeadTextureInit(string sponID, string crewID){
		var _child = FindHeadCrew() as GameObject;
		//Utility.Log("CHILD3 " + _child.name);
		if(_child == null) return;
		StringBuilder sb = new StringBuilder();
		sb.Length = 0;
		sb.Append(sponID);
		sb.Append("_");
		sb.Append(crewID);
		string sh = string.Empty;
		Texture _texture = null;
	//	Utility.LogError("CrewHeadTextureInit"); return;
		sh = SetShader(_child.renderer.material.shader.name);
		_texture = _child.renderer.material.GetTexture(sh);
		if(_texture == null) {
			Texture main =  (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
			//Texture main = (Texture)AccountManager.instance.carbundle.Load(sb.ToString(), typeof(Texture));
			if(main == null){
				sb.Replace("5101","5103");
				sb.Replace("5104","5103");
				 main =  (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
				//main = (Texture)AccountManager.instance.carbundle.Load(sb.ToString(), typeof(Texture));
			}
			_child.renderer.material.SetTexture(sh,main);
			_child.renderer.material.color = Color.white;
			return;
		}else{
			if(string.Equals(sb.ToString(), _texture.name)){
				return;
			}else{
				Texture main =  (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
				//Texture main = (Texture)AccountManager.instance.carbundle.Load(sb.ToString(), typeof(Texture));
				if(main == null){
					sb.Replace("5101","5103");
					sb.Replace("5104","5103");
					main =  (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
					//main = (Texture)AccountManager.instance.carbundle.Load(sb.ToString(), typeof(Texture));
				}
				_child.renderer.material.SetTexture(sh,main);
				_child.renderer.material.color = Color.white;
			}
		}

		
	}


	/*
	public void ChangeBodyTexture(int sponID){
		string[] _name = gameObject.name.Split("_"[0]);
		var _child = FindBodyCrew() as GameObject;
		StringBuilder sb = new StringBuilder();
		sb.Length = 0;
		sb.Append(_name[0]);
		sb.Append("_");
		sb.Append(sponID.ToString());
		Texture main = ObjectManager.GetSelectObjectWithAITexture(sb.ToString());
		//Texture main = (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
	//	Utility.LogWarning(sb.ToString());
		if(main == null ) {
			main = (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
		}
		if(main == null) {
			Utility.LogError("error");
		
		}//else{
			if(_child.renderer.materials.Length == 1){
				string sh = SetShader(_child.renderer.material.shader.name);
				//Texture gettex = _child.renderer.material.GetTexture(sh);
				//Utility.Log(gettex.name);
				//if(main.name == gettex.name) return;
				_child.renderer.material.SetTexture(sh,main);
			}else{
				string sh = SetShader(_child.renderer.materials[0].shader.name);
				_child.renderer.materials[1].SetTexture(sh,main);
				sh = SetShader(_child.renderer.materials[0].shader.name);
				_child.renderer.materials[0].SetTexture(sh,main);
			}
		//}
		//CrewAniPlay(_name[1]);
		Resources.UnloadUnusedAssets();
		main = null;
		Destroy(main);
	}

	public void ChangeHeadTexture(int sponID){
		string[] _name = gameObject.name.Split("_"[0]);
		var _child = FindHeadCrew() as GameObject;
		if(_child == null) return;
		StringBuilder sb = new StringBuilder();
		sb.Length = 0;
		sb.Append(sponID.ToString());
		sb.Append("_");
		sb.Append(_name[1]);
		sb.Append("_Crews");
		Texture main = (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
	//	Utility.LogWarning(sb.ToString());
		if(main == null) {
			sb.Replace("5101","5103");
			sb.Replace("5104","5103");
			main = (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));

			string sh = SetShader(_child.renderer.material.shader.name);
			_child.renderer.material.SetTexture(sh,main);
		}else{
			//_child.renderer.material.mainTexture = main;
			if(_name[1].Equals("5100")){
				string sh = SetShader(_child.renderer.material.shader.name);
				_child.renderer.material.SetTexture(sh,main);
			}else {
				string sh = SetShader(_child.renderer.material.shader.name);
				_child.renderer.material.SetTexture(sh,main);
			}
		}
		Resources.UnloadUnusedAssets();
		main = null;
		Destroy(main);
	}


	public void ChangeRaceBodyTexture(int num){
		string[] _name = gameObject.name.Split("_"[0]);
		var _child = FindBodyCrew() as GameObject;
		StringBuilder sb = new StringBuilder();
		sb.Length = 0;
		sb.Append(_name[0]);
		sb.Append("_");
		sb.Append(num.ToString());
		//sb.Append("_CrewsText");
		//Texture main = (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
		//Utility.LogWarning(sb.ToString());
		Texture main = ObjectManager.GetSelectObjectWithAITexture(sb.ToString());
		if(main == null) {
				Utility.LogError("error" + sb.ToString());
		}else{
			if(_child.renderer.materials.Length == 1){
				string sh = SetShader(_child.renderer.material.shader.name);
				_child.renderer.material.SetTexture(sh,main);
			}else{
				string sh = SetShader(_child.renderer.materials[0].shader.name);
				_child.renderer.materials[1].SetTexture(sh,main);
				sh = SetShader(_child.renderer.materials[0].shader.name);
				_child.renderer.materials[0].SetTexture(sh,main);
			}
		}
		//CrewAniPlay(_name[1]);
		//Resources.UnloadUnusedAssets();
		main = null;
		//Destroy(main);
	}
	
	public void ChangeRaceHeadTexture(int num){
		string[] _name = gameObject.name.Split("_"[0]);
		var _child = FindHeadCrew() as GameObject;
		StringBuilder sb = new StringBuilder();
		sb.Length = 0;
		sb.Append(num.ToString());
		sb.Append("_");
		sb.Append(_name[1]);

		//Texture main = (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
		//	Utility.LogWarning(sb.ToString());
		Texture main = ObjectManager.GetSelectObjectWithAITexture(sb.ToString());
		//if(main != null)
		//{
		//	string sh = SetShader(_child.renderer.material.shader.name);
		//	_child.renderer.material.SetTexture(sh,main);
		//}
		//else Utility.LogError("HEAD" + sb.ToString());
	
		if(main == null) {
			sb.Replace("5101","5103");
			sb.Replace("5104","5103");
			main = ObjectManager.GetSelectObjectWithAITexture(sb.ToString());
			//main = (Texture)Resources.Load("Textures/PitCrew/"+sb.ToString(), typeof(Texture));
			//
			string sh = SetShader(_child.renderer.material.shader.name);
			_child.renderer.material.SetTexture(sh,main);
		}else{
			//_child.renderer.material.mainTexture = main;
			if(_name[1].Equals("5100")){
				string sh = SetShader(_child.renderer.material.shader.name);
				_child.renderer.material.SetTexture(sh,main);
			}else {
				string sh = SetShader(_child.renderer.material.shader.name);
				_child.renderer.material.SetTexture(sh,main);
			}
		}
		Resources.UnloadUnusedAssets();
	
		main = null;
		//Destroy(main);
	}
*/
}
