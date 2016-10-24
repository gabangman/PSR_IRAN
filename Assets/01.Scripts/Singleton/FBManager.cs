using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
public class FBManager : MonoBehaviour {

	//public static FBManager instance { get; private set; }
	void Awake(){
		//FB.Init();
	}

	public  void FBLogin(){
		if(FB.IsInitialized){
			Utility.Log("IsInitialized true");
			CallFBLogin();
		}else{
			Utility.Log("IsInitialized false");
		}
	
	}

	public void FBLogout(){
		FB.Logout();
	}

	public void FBInit(){

		if(Global.gReLoad == 0){
			Utility.LogWarning("FBINIT");
			FB.Init(OnInitComplete, OnHideUnity);
			PlayGamesPlatform.DebugLogEnabled = true;
			GooglePlayGames.PlayGamesPlatform.Activate();
		}

	}

	 void OnHideUnity(bool isGameShown)
	{
		Utility.Log("Is game showing? " + isGameShown);
	}

	void OnInitComplete()
	{
		Utility.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
		Global.gisFBLogin = FB.IsLoggedIn?1:0;
	}

	private  void CallFBLogin()
	{
		FB.Login("public_profile,email,user_friends,publish_actions", LoginCallback);
	}
	
	void LoginCallback(FBResult result)
	{
		if (result.Error != null)
			Utility.Log(string.Format(" Error Response:  {0} " + result.Error));

		else if (!FB.IsLoggedIn)
		{
			Utility.Log(string.Format(" Login cancelled by Playe "));
		}
		else
		{
			Utility.Log(string.Format("Login was successful! "  ));
		}
	}
	
	
	

}
