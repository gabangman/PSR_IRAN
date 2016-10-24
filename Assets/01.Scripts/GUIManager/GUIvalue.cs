using UnityEngine;
using System.Collections;

public partial class ManagerGUI : MonoBehaviour {
	public GameObject[] _panel;
	public Transform  arrowSpeed, arrowRPM;
	private GameObject Gear;
	public Transform btnaccel,btnclutch,btnn20;
	public GameObject[] gearled;
	public GameObject[] Effect;
	public GameObject Tutorial;
	public Transform btnGearClutch;	
	public GameObject[] gearledBlink;
	public bool isPress = false;
	public bool isRelease = true;
	public UISprite dash_Speed, dash_Rpm;
	public GameObject userPanelInfo;
	public GameObject raceLED;
	public GameObject rewardText;
	public GameObject closeBtn;
	public GameObject Durability;
	public CarSpeed _carspeed = new CarSpeed();

	int[] CameraNumber;
	private TutorialRace _tutorial;
	System.Action EngineLEDCallback;
	System.Action AccelPressCallback;
	System.Action ClutchPressCallback;
	System.Action BlinkLEDCallback;
	System.Action N2OPressCallback;
	System.Action AccelReleaseCallback;
	System.Action<Transform> Hidden_TextCallback;
	System.Action<Transform> Show_TextCallback;
	System.Action<float> PressTimeCheckCallback;
	string strRpmLedCheck;
	bool isPause = false;
	float torqueTemp = 0;
	private float _speed = 0.0f, _rpm;
	private float smooth = 0.01f;
	private float yVelocity = 0.0f;
	private CarSound s_car;
	private MusicSound s_music;
	private UISound s_ui;
	private EffectSound s_effect;

	private bool isRace02StartLED = false;
	private bool isRace01 = true;
	private bool isclutchpress = false;
	private bool isrpmcheck = false; // press clutch state judgement
	private bool _isRPMCheck; //press time on/off variable
	private float checkTime;
	private float delayCheckTime = 10.0f;
	private bool isGearPress = false;
	private bool isGear = false;
	private bool isFirstPress = false;
	//private bool isGearPressCheck = false;
	private int pGear, gGear;
	int cameracount;
	string rpmjudgefinal = null;
	bool isMaxGear = false;


	Color32 accelreleasecolor = new Color32(255,255,255,255);
	Color32 accelpresscolor = new Color32(255,116,116,255);


}
