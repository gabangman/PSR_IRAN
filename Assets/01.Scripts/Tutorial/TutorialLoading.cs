using UnityEngine;
using System.Collections;

public class TutorialLoading : MonoBehaviour {

	public UISlider _slider;
	public UILabel _label,_per;
	private AsyncOperation async = null;
	IEnumerator Start(){
		// 튜터리얼 레이스 끝나고 타이틀 로드시 false로 변경할것!
		if(Global.gChampTutorial == 0){
			Global.isRaceTest = true; 
			Global.gAICarInfo = new AICarInfo[4];
			Global.gRaceInfo.trackID = "1401";
		}else {
			Global.isRaceTest = false; 
		}
		yield return StartCoroutine("LoadTutorial");
	
	}


	public IEnumerator LoadTutorial()
	{
		_per.text = string.Empty;
		_label.text =KoStorage.GetKorString("70118");
			async = Application.LoadLevelAsync("Track1");
			while(async.isDone == false) 
			{
		//		float p = async.progress *100f;
		//		int pRounded = Mathf.RoundToInt(p);
				//_per.text = System.String.Format("...{0:0}%",pRounded);
				_slider.sliderValue = async.progress;
				yield return null;
			}
	}

	bool isTutorial = false;
	IEnumerator WaitTutorial(){
		for(;;){
			if(isTutorial) yield break;
			yield return null;
		}
	}
}
