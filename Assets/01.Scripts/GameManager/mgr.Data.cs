using UnityEngine;
using System.Collections;
//using System;
using System.IO;


public partial class GameManager :  MonoSingleton< GameManager > {


	/*

	#region data
	FileStream file, file1;
	StreamWriter sw;
	StreamReader sr;



	public void GameRaceStartData(string strData, string raceData){
		string path = pathForDocumentsFile( strData );
		file = new FileStream ( path, FileMode.Create, FileAccess.Write );
		sw = new StreamWriter( file );
		sw.WriteLine(raceData);
		sw.Close();
		file.Close();

	}

	public void GameRaceEndData(string strData, string raceData){
		string path = pathForDocumentsFile( strData );
		file = new FileStream ( path, FileMode.Create, FileAccess.Write );
		sw = new StreamWriter( file );
		sw.WriteLine(raceData);
		sw.Close();
		file.Close();
	}





	public void writeStringToFile( string fileName, string str )
	{
		#if !WEB_BUILD
		StreamWriter sw;
		FileStream	file;
		string path = pathForDocumentsFile( fileName );
		if( File.Exists( path ))
		{
			file = new FileStream ( path, FileMode.Append, FileAccess.Write );
			sw  = new StreamWriter( file );
			
		}else{
			 file = new FileStream ( path, FileMode.Create, FileAccess.Write );
			 sw = new StreamWriter( file );
		}

		sw.WriteLine( str );
		sw.Close();
		file.Close();
		#endif
	} 
	
	//파일 읽기
	public string readStringFromFile( string fileName )//, int lineIndex )
	{
		#if !WEB_BUILD
		string path = pathForDocumentsFile( fileName );
		
		if( File.Exists( path ))
		   {
			FileStream file = new FileStream ( path, FileMode.Open, FileAccess.Read );
			StreamReader sr = new StreamReader( file );
			
			string str = null;
			str = sr.ReadLine();
			
			sr.Close();
			file.Close();
			
			return str;
		}
		else
		{
			return null;
		}
		#else
		return null;
		#endif
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
	

	void OnApplicationQuitFileSave(){
		Utility.LogWarning("OnApplicationQuit - SCENE");
		#if !WEB_BUILD
		if(file != null){
			sw.Close();
			file.Close();
		}if(file1 != null){
			sr.Close();
			file1.Close();
		}
		#endif
	}

	#endregion

*/






}
