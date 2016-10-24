using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System;
using System.IO;


public class GameOption :  MonoBehaviour {

	[System.Serializable]
	public class OptionSetting
	{
		public bool isLog;
		public bool isHighQuality;
		public bool isBGM;
		public bool isMusic;
		public bool isEffect;
		public string kakaoID;
		public string currentVersion;
		public bool isVibration;
		public bool isAlarm;
		public bool isThird;
		public void Init(){
			isLog = true;
			isHighQuality = false;
			isBGM = true;
			isMusic = true;
			isEffect = true;
			isVibration = true;
			isAlarm =true;
			isThird = false;
			kakaoID = string.Empty;
			currentVersion = string.Empty;



		}
	}


	static GameOption Instance;
	private OptionSetting _option = new OptionSetting();
	void Awake()
	{
		if (null == Instance)
			Instance = this;
		else{
		}
		Instance._option = getData();
	}


	void OnDestroy()
	{
		//Instance = null;
	}


	public void SaveOptionValue(){
		saveData();
	}


	public void DestroyDelay(){
		Instance = null;
	}

	public static OptionSetting OptionData
	{get { return Instance._option; } set{ Instance._option = value;}}


	OptionSetting getData(){
		var data = EncryptedPlayerPrefs.GetString("gameOption");
		if(!string.IsNullOrEmpty(data)) {
			var b = new BinaryFormatter();
			var m = new MemoryStream(Convert.FromBase64String(data));
			_option = (OptionSetting) b.Deserialize(m);
		}else {
			Instance._option.Init();
			saveData();
		}
		return _option;
	}

	void saveData(){
		var b = new BinaryFormatter();
		var m = new MemoryStream();
	//	Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER","yes");
		b.Serialize(m , Instance._option);
		EncryptedPlayerPrefs.SetString("gameOption",Convert.ToBase64String(m.GetBuffer())); 
	}

}
