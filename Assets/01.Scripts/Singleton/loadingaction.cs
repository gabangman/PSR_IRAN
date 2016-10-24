using UnityEngine;
using System.Collections;

public class loadingaction : MonoBehaviour {

	public GameObject load, textobj;
	public UISlider _slider;
	public UILabel _per, _text, _name;
	string loadText;
	void Awake(){
		LoadScreenInitiailze();
		StartCoroutine("StartRaceScene",SceneManager.instance.RaceName);
		int n = Random.Range(0,29);
		if(n >= 5){
		//	n+= 71198; //71203 ~ 71207
			int n1 = Random.Range(0,29);
			n = 71203+n1;
			//1203~1231
		}else{
			n+= 73007; // 73007~73011
		}			
		//60355 60359
		loadText = KoStorage.GetKorString(n.ToString());
		_text.text =loadText; 
	}

	void LoadScreenInitiailze(){
			load.SetActive(true);
			_slider.sliderValue = 0.0f;
			_per.text = string.Empty;
			_name.text = NewRaceName();
			changeLoadScene();
	}


	string NewRaceName(){
		string _raceName = string.Empty;
		switch(Global.gRaceInfo.mType){
		case MainRaceType.Champion:
			_raceName =  "Champion Mode";
			_raceName = KoStorage.getStringDic("73004");
			modeImage = "Mode_Champion";
			break;
		case MainRaceType.mEvent:
			string str = Global.gRaceInfo.eventModeName;

			switch(str){
			case "New":
				//_raceName =  "새로운 차량 레이스";
					_raceName = KoStorage.getStringDic("73002");
				modeImage = "Mode_Event";
				break;
			case "Select":
				//_raceName =  "지정 차량";
					_raceName = KoStorage.getStringDic("73002");
				modeImage = "Mode_Event";
				break;
			case "Qube":
				//_raceName =  "Cube 획득 레이스 ";
					_raceName = KoStorage.getStringDic("73002");
				modeImage = "Mode_Event";
				break;
			}
			//_raceName = KoStorage.GetKorString("73002");
			break;
		case MainRaceType.Regular:
			//_raceName = " RegularRace ";
				_raceName = KoStorage.getStringDic("73003");
			modeImage = "Mode_Regular";
			break;
		case MainRaceType.Weekly:
			//_raceName = "RankingRace ";
			_raceName = KoStorage.getStringDic("73600");
			modeImage = "Mode_Weekly";
			break;
		case MainRaceType.Club:
			//_raceName = "ClubRace ";
			_raceName = KoStorage.getStringDic("73000");
			modeImage = "Mode_Club";
			break;
		case MainRaceType.PVP:
			//_raceName = "PVP Race ";
			_raceName = KoStorage.getStringDic("73001");
			modeImage = "Mode_PVP";
			break;
		default:
			//_raceName = "Tutorial Race";
			_raceName = KoStorage.getStringDic("73003");
			modeImage = "Mode_Regular";
			break;
		}
		
		return _raceName;
	}
	string modeImage = string.Empty;

	IEnumerator StartRaceScene(string RaceName){
		SceneManager.instance.LoadLabel = _per;
		SceneManager.instance.LoadSlider = _slider;
		if(RaceName.Equals("Title")){
			var img = load.transform.FindChild("RaceInfo").gameObject;
			img.SetActive(false);
		}else{
			//changeLoadScene();

		}
		ObjectManager.CreateRaceObject(transform);
		ObjectManager.CreateAIObject();
		SceneManager.instance.settingRaceRenderSetting(RaceName);
		if (Application.platform == RuntimePlatform.Android){
			//yield return SceneManager.instance.StartCoroutine("RaceLoadScene", RaceName);
			yield return SceneManager.instance.StartCoroutine("LoadAdditveScene", RaceName);
		}else if(Application.platform == RuntimePlatform.IPhonePlayer){
			yield return SceneManager.instance.StartCoroutine("LoadAdditveScene", RaceName);
		}else{
			yield return SceneManager.instance.StartCoroutine("LoadAdditveScene", RaceName);
		}
		//yield return SceneManager.instance.StartCoroutine("LoadAdditveScene", RaceName);
		//RaceLoadScene

		gameObject.SetActive(false);
		//DestroyImmediate(transform.FindChild("Camera").gameObject);
	}

	void changeLoadScene(){
		load.transform.FindChild("icon_modeName").GetComponent<UISprite>().spriteName = modeImage;
		//Utility.LogError(modeImage);
		load.transform.FindChild("icon_modeName").GetComponent<UISprite>().MakePixelPerfect();
		var icon = load.transform.FindChild("RaceInfo") as Transform;
		if(Global.gRaceInfo.mType == MainRaceType.Champion) {
			icon.FindChild("icon_mode").GetComponent<UISprite>().spriteName = 
				Global.gRaceInfo.trackID +"L";
		}else{
			icon.FindChild("icon_mode").GetComponent<UISprite>().spriteName = 
				Global.gRaceInfo.trackID +"A";
		}
	
		icon.FindChild("icon_mode").GetComponent<UISprite>().MakePixelPerfect();
		icon.FindChild("icon_track").GetComponent<UISprite>().spriteName 
			= Global.gRaceInfo.trackID +"T";
	
		icon.FindChild("icon_track").GetComponent<UISprite>().MakePixelPerfect();
	}
	void OnDestroy(){
		//ObjectManager.CreateRaceObejct();
	}
}
