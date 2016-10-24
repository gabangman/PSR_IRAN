using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RaceCameraList : MonoBehaviour {

	public List<Camera> CameraList;
	Vector2[] swayspeed;
	Vector2[] swayrange;

	void Awake(){
		CameraList = new List<Camera>();
		GameObject[] CamIn = GameObject.FindGameObjectsWithTag("CarCamera");
		string strCamName =string.Empty;
		/*switch(Global.gRaceInfo.modeType){
		case RaceModeType.DragMode:
			strCamName = "Cam_Ready_Standing";
			DragCameraInit(strCamName,CamIn);
			break;
		case RaceModeType.StockMode:
			strCamName = "Cam_Ready1";
			StockCameraInit(strCamName,CamIn);
			break;
		case RaceModeType.TouringMode:
			strCamName = "Cam_Ready_Standing";
			TouringCameraInit(strCamName,CamIn);
			break;
		case RaceModeType.EventMode:
			strCamName = "Cam_Ready_Standing";
			EventCameraInit(strCamName,CamIn);
			break;
		default:
			break;
		}*/
		switch(Global.gRaceInfo.sType){
		case SubRaceType.DragRace:
			strCamName = "Cam_Ready_Standing";
			DragCameraInit(strCamName,CamIn);
			break;
		case SubRaceType.RegularRace:
			strCamName = "Cam_Ready1";
			StockCameraInit(strCamName,CamIn);
			break;
		case SubRaceType.CityRace:
			strCamName = "Cam_Ready_Standing";
			EventCameraInit(strCamName,CamIn);
			break;
		default:
			break;
		}
		swayspeed = new Vector2[]
		{
			new Vector2(0.1f,0.2f), new Vector2(0.1f,0.8f), new Vector2(0.1f,1.2f), new Vector2(0.1f,1.5f), new Vector2(0.1f,1.8f)
			,new Vector2(0.1f,2), new Vector2(0.1f,2), new Vector2(0.1f,2), new Vector2(0.1f,2), new Vector2(0.1f,2),
			new Vector2(0.1f,1.5f), new Vector2(0.1f,0.3f), new Vector2(0.1f,2.0f), new Vector2(0.1f,0.2f), new Vector2(0.1f,1.8f)
			,new Vector2(0.1f,1.2f), new Vector2(0.1f,2), new Vector2(0.1f,0.8f), new Vector2(0.1f,1.8f), new Vector2(0.1f,1.5f)
		};
		
		swayrange = new Vector2[]
		{
			new Vector2(0.2f,0.1f), new Vector2(0.8f,0.4f), new Vector2(1.2f,1.0f), new Vector2(1.5f,1.0f), new Vector2(1.8f,1.0f)
			,new Vector2(2.0f,1.0f),new Vector2(2.0f,1.0f), new Vector2(2.0f,1.0f), new Vector2(2.0f,1.0f), new Vector2(2.0f,1.0f),
			new Vector2(1.5f,1.0f), new Vector2(1.0f,0.9f), new Vector2(2.0f,1.0f), new Vector2(0.2f,0.1f), new Vector2(1.8f,0.1f)
			,new Vector2(1.2f,1.0f), new Vector2(2.0f,1.0f), new Vector2(0.8f,0.4f), new Vector2(1.8f,1.0f), new Vector2(1.5f,1.0f)
		};
	}

	void DragCameraInit(string name, GameObject[] CamIn){
		GameObject temp = null;
		foreach(GameObject obj in CamIn){
			var cam = obj.GetComponent<Camera>() as Camera;
			cam.enabled = false;
			obj.SetActive(false);
			CameraList.Add (cam);
			if(obj.name == name){
				obj.SetActive(true);
				cam.enabled = true;
				temp = obj;
			}
		}
		
		var ani = temp.GetComponent<Animation>() as Animation;
		StartCoroutine("StandingCameraAni", ani);
	}
	void EventCameraInit(string name, GameObject[] CamIn){
		GameObject temp = null;
		foreach(GameObject obj in CamIn){
			var cam = obj.GetComponent<Camera>() as Camera;
			cam.enabled = false;
			obj.SetActive(false);
			CameraList.Add (cam);
			if(obj.name == name){
				obj.SetActive(true);
				cam.enabled = true;
				temp = obj;
			}
		}
		
		var ani = temp.GetComponent<Animation>() as Animation;
		StartCoroutine("StandingCameraAni", ani);
	}

	void StockCameraInit(string name, GameObject[] CamIn){
		foreach(GameObject obj in CamIn){
			var cam = obj.GetComponent<Camera>() as Camera;
			cam.enabled = false;
			obj.SetActive(false);
			CameraList.Add (cam);
			if(obj.name == name){
				obj.SetActive(true);
				cam.enabled = true;
				obj.GetComponent<RaceCameraAction>().enabled =true;
			}
		}
	}

	void TouringCameraInit(string name, GameObject[] CamIn){
		GameObject temp = null;
		foreach(GameObject obj in CamIn){
			var cam = obj.GetComponent<Camera>() as Camera;
			cam.enabled = false;
			obj.SetActive(false);
			CameraList.Add (cam);
			if(obj.name == name){
				obj.SetActive(true);
				cam.enabled = true;
				temp = obj;
			}
		}

		var ani = temp.GetComponent<Animation>() as Animation;
		StartCoroutine("StandingCameraAni", ani);
	}

	IEnumerator StandingCameraAni(Animation ani){
		ani.Play("Cam_Ready_1");
		while(ani.isPlaying){
			yield return null;
		}
		ani.Play("Cam_Ready_2");
		while(ani.isPlaying){
			yield return null;
		}
		GameManager.instance.ShowSignalCount();
		ani.Play("Cam_Ready_3");
		while(ani.isPlaying){
			yield return null;
		}
		GameManager.instance.SwitchingCamera("Cam_Ready1");
		GameManager.instance.ShowSignalCountStart();
	}

	Camera CurrentCamera;
	public string SwitchingCamera(string str){
	
		int i = 0 ;
		for(int j=0 ; j<CameraList.Count;j++){
			if(CameraList[j].name == str){
				i = j;
				CameraList[j].transform.gameObject.SetActive(true);
				CameraList[i].camera.enabled = true;
				CameraList[i].GetComponent<RaceCameraAction>().enabled = true;
				CameraList[i].GetComponent<RaceCameraAction>().initFOV();
				GameManager.instance.currentCamera  = CameraList[i];
			}else{
				var cTemp = CameraList[j].transform.gameObject as GameObject;
				if(cTemp.activeSelf){
					CameraList[j].camera.enabled = false;
					cTemp.SetActive(false);
					var racecam = CameraList[j].GetComponent<RaceCameraAction>() as RaceCameraAction;
					if(racecam != null) racecam.enabled = false;
				}
			}
		}

	//	if(string.Equals(str, "Cam_Main")){
	//		CurrentCamera = Camera.main;
	//		return "Cam_Main";
	//	}

		CurrentCamera = CameraList[i];
		return  CameraList[i].name;
	}

	public void InsertCameraValue(int n){
		//return;
		if(n != 0)
			CurrentCamera.transform.GetComponent<RaceCameraAction>().GearCameraValueChange(
			swayspeed[n],swayrange[n]);
		else
			CurrentCamera.transform.GetComponent<RaceCameraAction>().GearCameraValueChange(
				swayspeed[n],swayrange[n], n);
		//Utility.Log("Insert Camer " +n);
	}

	public void ZoomInCamera(string str){
		//return;
		foreach(Camera c in CameraList){
			if(c.name == str){
				TweenFOV fov = c.transform.gameObject.AddComponent<TweenFOV>() as TweenFOV;
				float cfov = c.fieldOfView;
				float delta = cfov*0.4f; //*0.25f
				//if(cfov < 10) delta = 0.5f;
				fov.method = UITweener.Method.EaseInOut;
				fov.style = UITweener.Style.Once;
				fov.duration = 0.3f;
				fov.from = cfov+delta; //125%
				fov.to = cfov-delta; // 75%
				//Utility.Log("ZoomInCamera " + str + " fov " + cfov);
				break;
			} //press Aceel
		}
	//	Utility.Log("zoomin");
	}

	public void ZoomInOutCamera(string str){
		//return;
		foreach(Camera c in CameraList){
			if(c.name == str){
				TweenFOV fov = c.transform.gameObject.AddComponent<TweenFOV>() as TweenFOV;
				float cfov = c.fieldOfView;
				float delta = +10.0f; // 4.0f
				if(cfov < 10) delta = 0.5f;
				fov.method = UITweener.Method.EaseInOut;
				fov.style = UITweener.Style.Once;
				fov.duration = 0.3f;
				fov.from = cfov-delta;
				fov.to = cfov+delta;
				//Utility.Log("ZoomInOutCamera " + str + " fov " + cfov);
				break;
			}
		// press Clutch	
		}
	//	Utility.Log("zoomout");
	}
	
	IEnumerator ZoomCameraFOV(Camera c){
		//yield break;
		float cfov = c.fieldOfView;
		c.fieldOfView = cfov*0.85f;
		yield return new WaitForSeconds(0.1f);
		TweenFOV fov = c.transform.gameObject.AddComponent<TweenFOV>() as TweenFOV;
		fov.method = UITweener.Method.EaseInOut;
		fov.style = UITweener.Style.Once;
		fov.duration = 0.3f;
		fov.from = cfov; //125%
		fov.to = cfov*1.25f;//+delta; // 75%
		fov.onFinished = delegate(UITweener tween) {
			
		};
		//Utility.Log("ZoomFOV " + caname + " fov " + cfov);
	}
	public void ZoomFOV(string caname){
		//return;
		//CurrentCamera.fieldOfView = df*0.6f;
		foreach(Camera c in CameraList){
			if(c.name == caname){
				//StartCoroutine("ZoomCameraFOV", c);
				float cfov = c.fieldOfView;
				TweenFOV fov = c.transform.gameObject.AddComponent<TweenFOV>() as TweenFOV;
				fov.method = UITweener.Method.EaseInOut;
				fov.style = UITweener.Style.Once;
				fov.duration = 0.3f;
				fov.from = cfov; //125%
				fov.to = cfov*1.25f;//+delta; // 75%
				fov.onFinished = delegate(UITweener tween) {
					
				};

				break;
			} //press Aceel
		}
	}
	                                     
	public void CamShake(int _index){
		//return;
		Cam_Shake temp1 = CameraList[_index].GetComponent<Cam_Shake>() as Cam_Shake;
		temp1.enabled = true;
	}

	public void movingFOV(){
		//return;
		var tw  = CurrentCamera.gameObject.AddComponent<TweenFOV>() as TweenFOV;
		float temp = CurrentCamera.fieldOfView;
		tw.to = temp-5;
		tw.from = temp;
		tw.duration = 0.2f;
		tw.onFinished = delegate(UITweener tween) {
			var tw1 = tween.transform.GetComponent<TweenFOV>() as TweenFOV;
			movingFOVSecond(tw1);
		};
		tw.enabled = true;
	}

	void movingFOVSecond(TweenFOV tw){
		//return;
		float old = tw.to;
		tw.to = old+10;
		tw.from = old;
		tw.duration = 0.3f;
		tw.Reset();
		tw.enabled = true;
		tw.onFinished = delegate(UITweener tween) {
			Destroy(tween);		
		};
		
	}
	public void movingFOVStop(){
	}


}
