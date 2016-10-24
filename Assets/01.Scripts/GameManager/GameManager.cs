using UnityEngine;
using System.Collections;

public partial class GameManager : MonoSingleton< GameManager > {

public void RaceResourceLoad(){
		fade = fadeState.fadein; //140626
		Global.isLoadFinish = false;
}


#region OnGUI
//public Texture btnTexture;
public Texture2D blackTexture;
private float alpha = 1f;
private float fadeTime;
private enum fadeState {fadein, fadeout, nothing};
private fadeState fade;
void OnGUI(){
	//	return; // test
		if(Global.isLoadFinish) return; // true 이면  로비에서 true로 넘어옴... 
		if(fade == fadeState.nothing) {fadeTime = 0.0f; return;}
		if(fade == fadeState.fadein){
		alpha  =0.5f; //more 
		fadeTime += 0.02f;
		alpha -= Mathf.Clamp01(Time.deltaTime +fadeTime);
		GUI.color = new Color(0,0,0,alpha);
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), blackTexture);
			if(alpha <= 0 ) fade = fadeState.nothing;
		return;
	}
		if(fade == fadeState.fadeout){
		alpha = 0.5f; //more dark
		fadeTime += 0.02f;
		alpha += Mathf.Clamp01(Time.deltaTime + fadeTime);
		GUI.color = new Color(0,0,0,alpha);
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), blackTexture);
		if(alpha >= 1.0f) {
				switch(gameState){
				case 1:
					SceneManager.instance.StartCoroutine("RaceGameOver");
					gameState = 0;
					break;
				case 2:
					SceneManager.instance.StartCoroutine("LoadFirstScene","Title");
					gameState = 0;
					break;
				case 3:
					SceneManager.instance.StartCoroutine("LoadFirstScene","Title");
					gameState = 0;
					break;
				case 4:
					SceneManager.instance.StartCoroutine("LoadReplayScene");
					gameState = 0;
					break;
				default:
					break;
				}
		}
		return;
	}
}
#endregion

	#region default

	void OnDestroy(){
		OnDestoryGameObject();
		System.GC.Collect();
		//Utility.Log ("OnDestroy");
	}
	#endregion
}
/*
private static GameManager s_instance = null;
//    private int _count = 0;
public static GameManager instance
{
	get
	{
		if (null == s_instance)
		{
			s_instance = FindObjectOfType(typeof(GameManager)) as GameManager;
			if (null == s_instance)
			{
				Utility.Log("Fail to get Manager Instance");
			}
		}
		return s_instance;
	}
}
void OnApplicationQuit()
{
	OnApplicationQuitFileSave();
	
}
public void init(){
	
}*/
