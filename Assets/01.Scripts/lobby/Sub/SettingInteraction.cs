using UnityEngine;
using System.Collections;

public class SettingInteraction : MonoBehaviour {


	public void InitValue(bool ischeck){
		transform.FindChild("Checkbox").GetComponent<UICheckbox>().isChecked = ischeck;
		//transform.FindChild("Slider").GetComponent<UISlider>().sliderValue = _value;
	}

	public void GraphicSetttingInit(string Name, bool isHigh){
		transform.FindChild(Name).GetComponent<UICheckbox>().isChecked = isHigh;
	//	transform.FindChild("CheckboxL").GetComponent<UICheckbox>().isChecked = isLow;
	}

}
