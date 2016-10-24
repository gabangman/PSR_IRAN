using UnityEngine;
using System.Collections;

public class PitCameraAni : MonoBehaviour {
	public void CameraPitAni(int _index){
		Animation _camAni = gameObject.GetComponent<Animation>() as Animation;
		float fDelay = Base64Manager.instance.getFloatEncoding(Global.gPitCameraTime, 0.001f);
		switch(_index){
		case 0 :
			
			_camAni["PitPlay_1"].speed = fDelay;
			_camAni.Play ("PitPlay_1");
			GameManager.instance.StartCoroutine("PitCameraUserDelay",_camAni["PitPlay_1"].length);
			break;
		case 1:
			_camAni["PitPlay_2"].speed = fDelay;
			_camAni.Play ("PitPlay_2");
			GameManager.instance.StartCoroutine("PitCameraUserDelay",_camAni["PitPlay_2"].length);
			break;
		default:
			break;
		}

		Destroy(this);
	}


	


}
