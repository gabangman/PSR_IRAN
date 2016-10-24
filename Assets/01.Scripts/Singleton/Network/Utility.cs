using UnityEngine;
using System.Collections;

public class  Utility
{
	// long형 숫자를 local DateTime으로 변환.
	public static System.DateTime ConvertTime(long time)
	{
		long beginTicks = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc).Ticks;
		System.DateTime dt = new System.DateTime(beginTicks + time * 10000, System.DateTimeKind.Utc);
		return dt.ToLocalTime();
	}	

	


	public static void Log(object message){
		//Utility.LogWarning(str);
	}
	
	public static void LogWarning(object str){
		//Utility.LogError(str);
	}
	
	public static void LogError(object str){
		//Utility.Log(str);
	}

	public static void ResponseLog(object str, string mAPI, int index = 0){

		if(index == 0) {
			NetworkManager.instance.mTailCount += 1;
		}

		else{
		
		}
	
		int a = NetworkManager.instance.mTailCount;
		int b = NetworkManager.instance.mRequestRange;
		if(Application.isEditor){
			Debug.LogWarning(string.Format("response {0} = API : {1},  mTailCount : {2} / mReqeustRange {3}", str, mAPI,a,b));
		}
	//	if(a != b){
			//Debug.LogWarning(string.Format("{0} diff {1}", a,b));
			//170 diff 169

		return;
		if(a != b){
			Debug.LogWarning("Not Same ");
			var obj = NetworkManager.instance.OnNetworkFailed() as GameObject;
			if(obj.GetComponent<NetworkEmail>() == null){
				obj.AddComponent<NetworkEmail>();
			}
			obj.GetComponent<NetworkEmail>().SetTailInit();
		}
			//Debug.LogWarning("response " + str + "_" + mAPI);
		//	Utility.ResponseLog(request.response.Text, GV.mAPI);
	}

}
