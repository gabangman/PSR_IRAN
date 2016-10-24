using UnityEngine;
using System.Collections;

public class NewTitleScene : MonoBehaviour {

	public Animation titleAni;
	public GameObject whitePan;
	public Transform carParent, crewParent;
	private Transform[] wheels = new Transform[4];
	void Awake(){
		titleAni = GetComponent<Animation>();

	}

	void Start(){
		int carId = Random.Range(1000, 1024);
		int sponId = Random.Range(1300,1306);
		int crewID = Random.Range(1200,1208);
		if(crewID == 1207) crewID = 1210;
		GameObject obj = CreateAICar(carId.ToString(), sponId.ToString(),10, "SS");
		CreateAICrew(crewID.ToString(), sponId.ToString(), crewParent, obj.transform.GetChild(0).GetChild(0));
		StartCoroutine("PlayAnimation","Loading_Start");
	}


	public void CreatePrefabs(Transform _parent, string path, string name){
		var car = Resources.Load("Prefabs/"+path + "/"+name, typeof(GameObject)) as GameObject;
		var obj = Instantiate(car) as GameObject;
		obj.transform.parent = _parent.transform.parent;
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localEulerAngles = Vector3.zero;
	//	return obj;
	}

	public GameObject boostNozzleObj;
	GameObject CreateAICar(string carId, string sponID,int num, string strClass){
		//Utility.LogWarning("carid " + carId);
		var car = Resources.Load("Prefabs/MyCar/"+ carId, typeof(GameObject)) as GameObject;
		var race = Instantiate(car) as GameObject;
		race.SetActive(false);
	
		var tr = race.GetComponent<CarType>().CarClass as Transform;
		if(tr.childCount != 0) {
			for(int  i=0; i < tr.childCount; i++){
				if(strClass.Equals(tr.GetChild(i).name)) DestroyImmediate(tr.GetChild(i).gameObject);
			}
		}
		CreatePrefabs(tr.GetChild(0) , "Car_Class", carId+"_"+strClass);
		//temp1.name = strClass;
		
		race.name = carId+"_"+num.ToString();
		race.GetComponent<CarType>().CarTextureInitialize(carId, sponID);
		race.transform.parent = carParent;
		race.transform.localScale = Vector3.one;
		race.transform.localPosition = Vector3.zero;
		race.transform.localEulerAngles = Vector3.zero;
		race.SetActive(true);

		var cartype  = race.GetComponent<CarType>() as CarType;
		wheels[0] = cartype.Tires[0];//_carinit.FindWheel(car, 1);
		wheels[1] = cartype.Tires[1];//_carinit.FindWheel(car, 2);
		wheels[2]= cartype.Tires[2];//_carinit.FindWheel(car, 3);
		 wheels[3] = cartype.Tires[3];//_carinit.FindWheel(car, 4);

		Transform[] boosts = cartype.boosts;
		int count = boosts.Length;
		GameObject[] boostItem = new GameObject[count];


		string[] boostNozzle = new string[4];
			boostNozzle[0] = "NozzleFL";
			boostNozzle[1] = "NozzleFR";
			boostNozzle[2] = "NozzleRL";
			boostNozzle[3] = "NozzleRR";


		for(int i = 0; i < count; i++){
			boostItem[i] = Instantiate(boostNozzleObj) as GameObject;
			boostItem[i].transform.parent = boosts[i];
			ObjectManager.ChangeObjectPosition(boostItem[i], Vector3.zero, Vector3.one, Vector3.zero);
			boostItem[i].SetActive(true);
		}

		/*

		switch(count){
		case 2:
		{
			for(int i = 0; i < 2; i++){
				boostItem[i] = Instantiate(boostNozzle) as GameObject;
				boostItem[i].transform.parent = boosts[i];
				ObjectManager.ChangeObjectPosition(boostItem[i], Vector3.zero, Vector3.one, Vector3.zero);
			}
		}
			break;
		case 4:{
			
			for(int i = 0; i < 4; i++){
				boostItem[i] = ObjectManager.GetRaceObject("Effect",boostNozzle[i]) as GameObject;
				boostItem[i].transform.parent = boosts[i];
				ObjectManager.ChangeObjectPosition(boostItem[i], Vector3.zero, Vector3.one, Vector3.zero);
			}
		}
			break;
			
		case 8:{
			
			for(int i = 0; i < 4; i++){
				boostItem[i] = ObjectManager.GetRaceObject("Effect",boostNozzle[i]) as GameObject;
				boostItem[i].transform.parent = boosts[i];
				ObjectManager.ChangeObjectPosition(boostItem[i], Vector3.zero, Vector3.one, Vector3.zero);
			}
			for(int i = 4; i < 8; i++){
				boostItem[i] = Instantiate(boostItem[0]) as GameObject;
				boostItem[i].transform.parent = boosts[i];
				ObjectManager.ChangeObjectPosition(boostItem[i], Vector3.zero, Vector3.one, Vector3.zero);
			}
		}
			break;
		}*/


		return race;
	}
	//static StringBuilder sb = new StringBuilder();
	void CreateAICrew(string crewId, string sponID, Transform mP, Transform mP1){
		string crews = string.Empty;
			int ids = 5102;
			crews = crewId+"_"+ ids.ToString();
			var car = Resources.Load("Prefabs/PitCrew/"+ crews, typeof(GameObject)) as GameObject;
			var race = Instantiate(car) as GameObject;
			race.SetActive(false);
			race.name = crewId+"_"+ids.ToString();
			var tex = race.GetComponent<textureaction>() as textureaction;
			if(tex == null) tex = race.AddComponent<textureaction>();
			tex.CrewHeadTextureInit(sponID,ids.ToString(), true);
			tex.CrewBodyTextureInit(crewId,sponID, true);

		race.transform.parent = mP;

		race.transform.localScale = Vector3.one;
		race.transform.localPosition = Vector3.zero;
		race.transform.localEulerAngles = Vector3.zero;


		race.SetActive(true);

		ids = 5100;
		crews = crewId+"_"+ ids.ToString();
		car = Resources.Load("Prefabs/PitCrew/"+ crews, typeof(GameObject)) as GameObject;
		race = Instantiate(car) as GameObject;
		race.SetActive(false);
		race.name = crewId+"_"+ids.ToString();
		 tex = race.GetComponent<textureaction>() as textureaction;
		if(tex == null) tex = race.AddComponent<textureaction>();
		tex.CrewHeadTextureInit(sponID,ids.ToString(), true);
		tex.CrewBodyTextureInit(crewId,sponID, true);
		mP1.transform.localEulerAngles = new Vector3(0,270,0);
		race.transform.parent = mP1;
		race.transform.localScale = Vector3.one;
		race.transform.localPosition = Vector3.zero;
		race.transform.localEulerAngles = Vector3.zero;

		race.SetActive(true);
	}


	public void InitSetting(){
		GameObject.Find("LoadScene").SendMessage("showRegisterNick");
	}

	public void secondAni(){
		StartCoroutine("PlayAnimation1","Loading_Finish");
	}

	IEnumerator StartColor(){
		Color cor = whitePan.transform.renderer.material.GetColor("_TintColor");
		bool bbb = true;
		float delta = 0.1f;
		while(bbb){
			delta += 0.01f;
			whitePan.transform.renderer.material.SetColor("_TintColor", new Color(cor.r, cor.g, cor.b, delta));
			if(delta >= 1.0f){
				bbb = false;
			//	gameObject.SetActive(false);
				yield break;
			}
			yield return new WaitForSeconds(0.02f);
		}
	//	whitePan.transform.renderer.material.SetColor("_TintColor", new Color(cor.r, cor.g, cor.b, 1.0f));

	}

	IEnumerator PlayAnimation(string str){
		titleAni[str].time = 0;
		titleAni[str].speed = 1;
		titleAni.Play(str);
		while(titleAni.isPlaying){
			yield return null;
		}
		Global.bGearPress = true;
	}	
	IEnumerator PlayAnimation1(string str){
		bGUI = false;
		titleAni[str].time = 0;
		titleAni[str].speed = 1;
		titleAni.Play(str);
		while(titleAni.isPlaying){
			yield return null;
		}
		System.GC.Collect();
	//	gameObject.SetActive(false);
	//	whitePan.SetActive(true);
	//	GameObject.Find("LoadScene").SendMessage("loadScene");
	//	StartCoroutine("StartColor");
	//	bGUI = false;
	//	alpha = 0;

	//	DestroyImmediate(gameObject);
	}	

	public bool aniCheck {
	get{
			return titleAni.isPlaying;
		}
	}
	float rotateTime = 0;
	public void rotateWheel(){
		rotateTime = Time.deltaTime*100*30*1;
		wheels[0].Rotate(0,0,rotateTime);
		wheels[1].Rotate(0,0,rotateTime);
		wheels[2].Rotate(0,0,rotateTime);
		wheels[3].Rotate(0,0,rotateTime);
	}

	void FixedUpdate(){
		if(bGUI) return;
		rotateWheel();
	}

	public Texture2D blackTexture;
	private float alpha = 1f;
	private float fadeDir = 1.0f; 
	private bool m_bStop;
	private float tempalpha;
	private bool m_balphainit;
	
	// 0 일때 밝아지고
	// 1일때 가장 어두워 진다.
	// 시작하기 위해서는 검은색 텍스쳐가 한장 필요하다.
	
	void StopState()
	{
		if(m_bStop)
		{
			if(!m_balphainit)
			{
				tempalpha = alpha;
				m_balphainit = true;
			}
			
			alpha = Mathf.Clamp01(0.5f);  
		}
		else
		{
			if(m_balphainit)
			{
				m_balphainit = false;
				alpha = tempalpha;
			}
			alpha = Mathf.Clamp01(alpha);  
		}
		if(alpha == 1 || alpha == 0){
		}
	}
	
	// Update is called once per frame
	
	void  fadeIn(){ 
		fadeDir = 1;  
		//	m_bFadeIn = false;
		m_balphainit = false;
		m_bStop = false;
		return;
	} 
	
	void  fadeOut(){ 
		fadeDir = -1;    
		//	m_bFadOut = false;
		m_balphainit = false;
		m_bStop = false;
		return;
		
	} 

	bool bGUI = true;
	/*
	void OnGUI(){
		if(bGUI) return;

		alpha += 1 * Mathf.Clamp01(Time.deltaTime);
		alpha = Mathf.Clamp01(alpha);
		GUI.color = new Color(0,0,0,alpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture); 
		
		return;
	}*/

}
