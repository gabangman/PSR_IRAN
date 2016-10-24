using UnityEngine;
using System.Collections;

public static class VibrationActivity
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityActivityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject activityObj = unityActivityClass.GetStatic<AndroidJavaObject>("currentActivity");
	public static AndroidJavaObject pushObj =  unityActivityClass.GetStatic<AndroidJavaObject>("currentActivity");
#elif UNITY_IOS


#elif UNITY_EDITOR
	public static AndroidJavaClass unityActivityClass;
	public static AndroidJavaObject activityObj;
	public static AndroidJavaObject pushObj;
#endif
}