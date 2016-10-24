using UnityEngine;
using System.Collections;

public class LuckyBoxAni : MonoBehaviour {
	
	public Animation SideArmAni;
	public Animation CamAni;
	public Animation BoxUpAni;
	public Animation BoxOpenAni;
	public Transform LuckyCarPos;
	public Transform LuckyBox;
	public Animation JunkAni;
	public Texture Gold;
	public Texture Silver;
	// Use this for initialization
	public AudioClip upAudio, downAudio, luckyAudio,JunkAudio;
	public GameObject EffectJunk, EffectLucky;
	void Start () {
		
	}

	IEnumerator LuckyEffect(){
		yield return new WaitForSeconds(0.1f);
		var root = GameObject.Find("LuckyEffect") as GameObject;
		var car = Resources.Load("Effect_N/LuckyBox_1", typeof(GameObject)) as GameObject;
		var race = Instantiate(car) as GameObject;
		race.transform.parent = root.transform;
		race.transform.localPosition = Vector3.zero;
	}

	public void defaultBoxAnimation(bool b){
		if(b) NGUITools.PlaySound(downAudio, 1.0f, 1.0f);
		BoxUpAni.Play();
	}
	public void PlayBoxUpDownReady(int idx){
		StartCoroutine("playBoxUpDown", idx);
	}
	public IEnumerator playBoxUpDown(int idx){
		string name = BoxUpAni.clip.name;
		Global.isAnimation = true;
		
		BoxUpAni[name].time = BoxUpAni[name].length;
		BoxUpAni[name].speed = -1;
		BoxUpAni.Play(name);
		NGUITools.PlaySound(upAudio, 1.0f, 1.0f);
		while(BoxUpAni.isPlaying){
			yield return null;
		}
		setBoxTexture(idx);
		BoxUpAni[name].time = 0;
		BoxUpAni[name].speed = 1;
		BoxUpAni.Play(name);
		//yield return new WaitForSeconds(downAudio.length);
		NGUITools.PlaySound(downAudio, 1.0f, 1.0f);
		while(BoxUpAni.isPlaying){
			yield return null;
		}
		Global.isAnimation = false;
	}
	public void setBoxTexture(int idx){
		Texture main = null;string str = null;
		if(idx == 0) {main = Silver;str = Silver.name;}
		else {main = Gold;str = Gold.name;}
		for(int i = 0; i < LuckyBox.childCount; i++){
			var tex = LuckyBox.GetChild(i).renderer.material.GetTexture("_MainTex") as Texture;
			//Utility.Log( LuckyBox.GetChild(i).renderer.material.shader.name);
			if(tex.name.Equals(str)) return;
			LuckyBox.GetChild(i).renderer.material.SetTexture("_MainTex",main);	
		}
		
	}
	
	IEnumerator StartAni(){
		yield return new WaitForSeconds(0.1f);
		//		PlayAnimation(SideArmAni);
		//		PlayAnimation(CamAni);
		//		PlayAnimation(BoxOpenAni);
		//		yield return new WaitForSeconds(5.5f);
		//		ReversePlay(SideArmAni);
		//		ReversePlay(CamAni);
		//		ReversePlay(BoxOpenAni);
	}
	
	public GameObject CreateLuckyCar(int carid, string carClass){
		GameObject temp = null;
		string _carName = carid.ToString();
		var LobbyCar = ObjectManager.CreatePrefabs(LuckyCarPos.GetChild(0), "MyCar" , _carName) as GameObject;
		LobbyCar.name = _carName;
		var tr = LobbyCar.GetComponent<CarType>().CarClass as Transform;
		//var tr = LobbyCar.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).FindChild("Class") as Transform;
		if(tr.childCount != 0) {
			for(int  i=0; i < tr.childCount; i++){
				if(carClass.Equals(tr.GetChild(i).name)) DestroyImmediate(tr.GetChild(i).gameObject);
			}
		}
		var temp1 = ObjectManager.CreatePrefabs(tr.GetChild(0) , "Car_Class", _carName+"_"+carClass) as GameObject;
		temp1.name = carClass;
		AccountManager.instance.SetCarTexture(LobbyCar, 1300);
		return LobbyCar;
	}
	
	void PlayAnimation(Animation Ani){
		string name = Ani.clip.name;
		Ani[name].time = 0;
		Ani[name].speed = 1;
		Ani.Play(name);
	}
	
	void ReversePlay(Animation Ani){
		string name = Ani.clip.name;
		Ani[name].time = Ani[name].length;
		Ani[name].speed = -10;
		Ani.Play(name);
	}
	System.Action<bool> Callback;
	System.Action<int, string> LuckyCallback;
	public void ChoiceBox(int idx, System.Action<int, string> callback, int money = 0){
		this.LuckyCallback= callback;
		if(idx == 0){
			if(money == 0){
				StartCoroutine("PlayLuckySilverBox");
			}else{
				StartCoroutine("PlayLuckySilverBoxMoney");
			}
		}
		else{
			if(money == 0){
				StartCoroutine("PlayLuckyGoldBox");
			}else{
				StartCoroutine("PlayLuckyGoldBoxMoney");
			}
		}
	}
	
	void RandomCarClass(){
		
	}
	
	IEnumerator PlayLuckySilverBox(){
		Global.isAnimation = true;
		//EffectLucky.SetActive(true);	NGUITools.PlaySound(luckyAudio, 1.0f, 1.0f);
		bool bConnect = false;
		int carId =0, carIdx = 0, nCarClass = 0;
		string mClass = string.Empty;
		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		mDic.Clear(); 
		string mAPI = ServerAPI.Get(90019);//"game/container/silver"
		mDic.Add("vipExp", GV.vipExp);
		NetworkManager.instance.HttpFormConnect("Delete", mDic, mAPI , (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				carId = thing["result"]["carId"].AsInt;
				carIdx =  thing["result"]["carIdx"].AsInt;
				nCarClass =  thing["result"]["carClass"].AsInt;
				mClass = GV.ChangeCarClassIDString(nCarClass);
			}else{
				
			}
			bConnect = true;
		});
		
		
		
		while(!bConnect){
			yield return null;
		}
		EffectLucky.SetActive(true);NGUITools.PlaySound(luckyAudio, 1.0f, 1.0f);
		PlayAnimation(SideArmAni);
		PlayAnimation(CamAni);
		PlayAnimation(BoxOpenAni);
		yield return new WaitForSeconds(0.1f);
		var temp = CreateLuckyCar(carId,mClass) as GameObject;
		yield return new WaitForSeconds(1.5f);
		var ani = temp.transform.GetChild(0).GetComponent<Animation>() as Animation;
		ani.Play("LuckyboxAni");
		StartCoroutine("LuckyEffect");
		yield return new WaitForSeconds(3.0f);
		LuckyCallback(carId, mClass);
		GV.AddMyCarListLucky(carId,mClass,1, 0, nCarClass, carIdx);
		yield return new WaitForSeconds(0.5f);
		ReversePlay(SideArmAni);
		ReversePlay(CamAni);
		ReversePlay(BoxOpenAni);
		temp.SetActive(false);
		StartCoroutine("CheckAniPlay");
	}
	
	IEnumerator PlayLuckySilverBoxMoney(){
		Global.isAnimation = true;
		bool bConnect = false;
		int carId =0, carIdx = 0, nCarClass = 0;
		string mClass = string.Empty;
		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		mDic.Clear();
		mDic.Add("vipExp", GV.vipExp);
		string mAPI = ServerAPI.Get(90020);//"game/container/silver/dollar"
		NetworkManager.instance.HttpFormConnect("Delete", mDic, mAPI , (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				carId = thing["result"]["carId"].AsInt;
				carIdx =  thing["result"]["carIdx"].AsInt;
				nCarClass =  thing["result"]["carClass"].AsInt;
				mClass = GV.ChangeCarClassIDString(nCarClass);
			}else{
				
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
		EffectLucky.SetActive(true);NGUITools.PlaySound(luckyAudio, 1.0f, 1.0f);
		PlayAnimation(SideArmAni);
		PlayAnimation(CamAni);
		PlayAnimation(BoxOpenAni);
		yield return new WaitForSeconds(0.1f);
		var temp = CreateLuckyCar(carId,mClass) as GameObject;
		yield return new WaitForSeconds(1.5f);
		var ani = temp.transform.GetChild(0).GetComponent<Animation>() as Animation;
		ani.Play("LuckyboxAni");
		StartCoroutine("LuckyEffect");
		yield return new WaitForSeconds(3.0f);
		LuckyCallback(carId, mClass);
		GV.AddMyCarListLucky(carId,mClass,1, 0, nCarClass, carIdx);
		yield return new WaitForSeconds(0.5f);
		ReversePlay(SideArmAni);
		ReversePlay(CamAni);
		ReversePlay(BoxOpenAni);
		temp.SetActive(false);
		StartCoroutine("CheckAniPlay");
	}
	IEnumerator PlayLuckyGoldBox(){
		Global.isAnimation = true;
		bool bConnect = false;
		int carId =0, carIdx = 0, nCarClass = 0;
		string mClass = string.Empty;
		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		mDic.Clear(); 
		mDic.Add("vipExp", GV.vipExp);
		string mAPI = ServerAPI.Get(90021);//"game/container/gold"
		NetworkManager.instance.HttpFormConnect("Delete", mDic, mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				carId = thing["result"]["carId"].AsInt;
				carIdx =  thing["result"]["carIdx"].AsInt;
				nCarClass =  thing["result"]["carClass"].AsInt;
				mClass = GV.ChangeCarClassIDString(nCarClass);
			}else{
				
			}
			bConnect = true;
		});
		
		
		
		/*
		int carid = Random.Range(1000, 1024);
		int cnt = Random.Range(0,6);
		string mClass = "S";
		switch(cnt){
		case 0: mClass = "D";break;
		case 1: mClass = "C";break;
		case 2: mClass = "B";break;
		case 3: mClass = "A";break;
		case 4: mClass = "S";break;
		case 5: mClass = "SS";break;
		}
		int carLV = 1, crewLV = 0;
		if(GV.LuckyCarClick == 1){
			carid = GV.LuckyCarID;
			mClass = GV.LuckyCarClass;
			carLV = GV.LuckyCarLV;
			crewLV = GV.LuckyCrewLV;
			GV.LuckyCarClick = 0;
		}*/
		
		while(!bConnect){
			yield return null;
		}
		EffectLucky.SetActive(true);NGUITools.PlaySound(luckyAudio, 1.0f, 1.0f);
		PlayAnimation(SideArmAni);
		PlayAnimation(CamAni);
		PlayAnimation(BoxOpenAni);
		yield return new WaitForSeconds(0.1f);
		var temp = CreateLuckyCar(carId,mClass) as GameObject;
		yield return new WaitForSeconds(1.5f);
		var ani = temp.transform.GetChild(0).GetComponent<Animation>() as Animation;
		ani.Play("LuckyboxAni");
		StartCoroutine("LuckyEffect");
		yield return new WaitForSeconds(3.0f);
		LuckyCallback(carId, mClass);
		GV.AddMyCarListLucky(carId,mClass,1, 0, nCarClass, carIdx);
		yield return new WaitForSeconds(0.5f);
		ReversePlay(SideArmAni);
		ReversePlay(CamAni);
		ReversePlay(BoxOpenAni);
		temp.SetActive(false);
		StartCoroutine("CheckAniPlay");
	}
	
	IEnumerator PlayLuckyGoldBoxMoney(){
		Global.isAnimation = true;
		
		bool bConnect = false;
		int carId =0, carIdx = 0, nCarClass = 0;
		string mClass = string.Empty;
		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		mDic.Clear(); 
		mDic.Add("vipExp", GV.vipExp);
		string mAPI = ServerAPI.Get(90022);//"game/container/gold/coin"
		Utility.LogWarning("90022 " + mAPI);
		NetworkManager.instance.HttpFormConnect("Delete", mDic,  mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				carId = thing["result"]["carId"].AsInt;
				carIdx =  thing["result"]["carIdx"].AsInt;
				nCarClass =  thing["result"]["carClass"].AsInt;
				mClass = GV.ChangeCarClassIDString(nCarClass);
			}else{
				
			}
			bConnect = true;
		});
		
		
		while(!bConnect){
			yield return null;
		}
		EffectLucky.SetActive(true);NGUITools.PlaySound(luckyAudio, 1.0f, 1.0f);
		PlayAnimation(SideArmAni);
		PlayAnimation(CamAni);
		PlayAnimation(BoxOpenAni);
		yield return new WaitForSeconds(0.1f);
		var temp = CreateLuckyCar(carId,mClass) as GameObject;
		yield return new WaitForSeconds(1.5f);
		var ani = temp.transform.GetChild(0).GetComponent<Animation>() as Animation;
		ani.Play("LuckyboxAni");
		StartCoroutine("LuckyEffect");
		yield return new WaitForSeconds(3.0f);
		LuckyCallback(carId, mClass);
		GV.AddMyCarListLucky(carId,mClass,1, 0, nCarClass, carIdx);
		yield return new WaitForSeconds(0.5f);
		ReversePlay(SideArmAni);
		ReversePlay(CamAni);
		ReversePlay(BoxOpenAni);
		temp.SetActive(false);
		StartCoroutine("CheckAniPlay");
	}
	
	
	IEnumerator CheckAniPlay(){
		while(BoxOpenAni.isPlaying){
			yield return null;
		}
		EffectLucky.SetActive(false);
		Global.isAnimation = false;
	}
	
	public void PlayJunkAni(System.Action<bool> callback, int nIdx){
		this.Callback = callback;
		StartCoroutine("PlayCarJunkAnimation", nIdx);
	}
	
	
	private System.Collections.Generic.List<int> mMatList;
	public string GetMaterialLists(){
		if(mMatList.Count == 0) return string.Empty;
		string str = null;
		for(int i = 0; i < mMatList.Count; i++){
			str += mMatList[i].ToString();
			str +=",";
		}
		return str;
	}
	IEnumerator PlayCarJunkAnimation(int nIdx){
		Global.isAnimation = true;
		bool bConnect = false;
		bool bSucc = false;
		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		mDic.Clear(); 
		int nCnt = GV.mineCarList[nIdx].CarIndex;
		mDic.Add("carIdx",nCnt);
		nCnt = GV.mineCarList[nIdx].CarID;
		mDic.Add("carId",nCnt);
		nCnt = GV.mineCarList[nIdx].nClassID;
		mDic.Add("carClass", nCnt);
		string mAPI = ServerAPI.Get(90017);//"game/car/disassemble"
		NetworkManager.instance.HttpFormConnect("Delete", mDic, mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			mMatList = new System.Collections.Generic.List<int>();
			if (status == 0)
			{
				bSucc = true;
				//response:{"state":0,"msg":"sucess","result":[{"id":8614},{"id":8618}],"time":1444890346}
				int mCnt = thing["result"].Count;
				//Utility.LogWarning(mCnt);
				for(int i = 0; i < mCnt ; i++){
					mMatList.Add(thing["result"][i]["id"].AsInt);
				}
			}else{
				bSucc = false;				
			}
			bConnect = true;
		});
		
		while(!bConnect){
			yield return null;
		}
		EffectJunk.SetActive(true);NGUITools.PlaySound(JunkAudio, 1.0f, 1.0f);
		if(bSucc){
			JunkAni.Play();
			var obj = GameObject.Find("LobbyUI") as GameObject;
			obj.SendMessage("InvenDismantleAni");
			var delay = JunkAni.clip.length;
			yield return new WaitForSeconds(delay * 0.8f);
			obj.SendMessage("ChangeElevatorCarAssy");
			yield return new WaitForSeconds(delay * 0.2f);
			
		}else{
			yield return new WaitForSeconds(0.3f);
		}
		Callback(bSucc);
		Global.isAnimation = false;
		EffectJunk.SetActive(false);
		yield return null;
	}
	
	public void PlaySellAni(System.Action<bool> callback, int nIndex){
		this.Callback = callback;
		StartCoroutine("PlayCarSellAnimation", nIndex);
	}
	
	IEnumerator PlayCarSellAnimation(int nIdx){
		Global.isAnimation = true;
		bool bConnect = false;
		bool bSucc = false;
		System.Collections.Generic.Dictionary<string, int> mDic = new System.Collections.Generic.Dictionary<string, int>();
		mDic.Clear(); 
		int nCnt = GV.mineCarList[nIdx].CarIndex;
		mDic.Add("carIdx",nCnt);
		nCnt = GV.mineCarList[nIdx].CarID;
		mDic.Add("carId",nCnt);
		nCnt = GV.mineCarList[nIdx].nClassID;
		mDic.Add("carClass", nCnt);
		string mAPI = ServerAPI.Get(90016);//"game/car/sale"
		NetworkManager.instance.HttpFormConnect("Delete", mDic, mAPI, (request)=>{
			Utility.ResponseLog(request.response.Text, GV.mAPI);
			var thing = SimpleJSON.JSON.Parse(request.response.Text);
			int status = thing["state"].AsInt;
			if (status == 0)
			{
				bSucc = true;
			}else{
				bSucc = false;				
			}
			bConnect = true;
		});
		while(!bConnect){
			yield return null;
		}
		
		Callback(bSucc);
		Global.isAnimation = false;
		yield return null;
	}
}



