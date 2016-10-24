using UnityEngine;
using System.Collections;

using CallbackVoid = System.Action;
using CallbackBool = System.Action<bool>;
using CallbackError = System.Action<string, string>;
using System.Text;	
public class KakaoTalkManager : MonoBehaviour
{
    public static KakaoTalkManager instance { get; private set; }

	// 로그 및 토스트 출력 여부.
	public bool printDebug = false;
	// 초기화를 해야 사용 가능함.
	public bool isInitialized { get; private set; }
	// 이전 로그인 정보 유무.
	public bool isAuthorized { get; private set; }

	public string accessToken { get; private set; }
	public string refreshToken { get; private set; }

	private CallbackVoid callbackOnLoginComplete;
	private CallbackVoid callbackOnLoginError;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnDestory()
    {
        instance = null;
    }


}
