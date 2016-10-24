using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
public class SaveRaceResult : MonoBehaviour {
	/*
	FileStream file;
	StreamWriter sw;

	public void StartWriteResult(){
		string str = "myRace";
		return;
		#if UNITY_EDITOR
		if(Global.isRaceTest) return;
		string path = pathForDocumentsFile( str );
		if( File.Exists( path ))
		{
			file = new FileStream ( path, FileMode.Append, FileAccess.Write );
			sw = new StreamWriter( file );
		}else{
			file = new FileStream ( path, FileMode.Append, FileAccess.Write );
			sw = new StreamWriter( file );
			string _text = SaveStringInit();
			sw.WriteLine(_text);

		}

		string _tex = SaveString();
		sw.WriteLine(_tex);

		sw.Close();
		file.Close();
		Destroy(this);
		Utility.LogWarning("SAVE FILE");
	
	#endif

	}


	string SaveStringInit(){
		StringBuilder sb = new StringBuilder();
		sb.Length = 0;

		sb.Append(" TIME :      ");
		sb.Append("Perfect Gear :  ");
		sb.Append("Good Gear :  ");
		sb.Append("Perfect Screw : ");
		sb.Append("good Screw : ");
		sb.Append("Race01Time : ");
		sb.Append("PitInTime : ");
		sb.Append("Race02Time :  ");
		sb.Append("raceTime :  ");
		sb.Append("CarID  :  ");
		sb.Append("CrewID : ");

		return sb.ToString();
	}


	string SaveString(){
		StringBuilder sb = new StringBuilder();
		sb.Length = 0;
		sb.Append(" Time : /");

		string theTime = System.DateTime.Now.ToString("hh:mm:ss"); 
		sb.Append("  / ");
		sb.Append(theTime);

		sb.Append(" /  ");
		sb.Append(Global.pGearScore.ToString());
	
		sb.Append("  / ");
		sb.Append(Global.gGearScore.ToString());

		//string _text = System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((Global.Race02Time[i]/60f)) ,Global.Race02Time[i]%60.0f);
		sb.Append("  / ");
		sb.Append(Global.pDrillScore.ToString());


		sb.Append(" /  ");
		sb.Append(Global.gDrillScore.ToString());



		sb.Append("  / ");
		//sb.Append("  Time1 :  ");
	//	string _text = System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((Global.Race1ResutTime/60f)) ,Global.Race1ResutTime%60.0f);
		string _text = string.Empty;
		sb.Append(_text);

		//sb.Append("  Time2 :  ");
		sb.Append("  / ");
		 _text = System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((Global.PitInResutTime/60f)) ,Global.PitInResutTime%60.0f);
		sb.Append(_text);

		//sb.Append("  Time3 :  ");
		sb.Append(" /  ");
		_text = string.Empty;
	//	_text = System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((Global.Race2ResutTime/60f)) ,Global.Race2ResutTime%60.0f);
		sb.Append(_text);

		//sb.Append("  Time4 :  ");sb.Append("   ");
		sb.Append(" /  ");
		_text = System.String.Format("{0:00}:{1:00.000}", Mathf.Floor((Global.RaceResutTime/60f)) ,Global.RaceResutTime%60.0f);
		sb.Append(_text);

		//sb.Append("CarID  :  ");
		string carName= string.Empty;

//		if(Global.gRaceInfo.raceType == RaceType.DailyMode)
//			carName = Base64Manager.instance.GlobalEncoding(Global.gDailyCarID).ToString();
//		else carName = Base64Manager.instance.GlobalEncoding(Global.MyCarID).ToString();
		carName = GV.PlayCarID.ToString();
		sb.Append(" /  ");
		sb.Append(carName);

		//sb.Append("CrewID : ");
		sb.Append(" /  ");
		//carName = Base64Manager.instance.GlobalEncoding(Global.MyCrewID).ToString();
		carName = GV.PlayCrewID.ToString();
		sb.Append(carName);
	
		return sb.ToString();
	}



	//파일 경로 알아오기
	public string pathForDocumentsFile( string fileName )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			string path = Application.dataPath.Substring( 0, Application.dataPath.Length - 5 );
			path = path.Substring( 0, path.LastIndexOf('/'));
			return Path.Combine( Path.Combine( path, "Documents"), fileName );
		}
		else if( Application.platform == RuntimePlatform.Android )
		{
			string path = Application.persistentDataPath;
			path = path.Substring( 0, path.LastIndexOf('/'));
			return Path.Combine( path, fileName );
		}
		else
		{
			string path = Application.dataPath;
			
			path = path.Substring( 0, path.LastIndexOf('/'));
			//Utility.Log (path);
			return Path.Combine( path, fileName );
		}
	}
*/
}
